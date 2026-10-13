using HegreHotel.Models;
using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Maui.Storage;

namespace HegreHotel.Views.Reservation
{
    public partial class AjouterReservationPage : ContentPage
    {
        private List<Models.Client> _clients;
        private List<Models.Chambre> _chambresDispo;
        private bool _datesValidees = false;

        public AjouterReservationPage()
        {
            InitializeComponent();
        }

        private async void OnVerifierDisponibiliteClicked(object sender, EventArgs e)
        {
            if (DatePickerDebut.Date > DatePickerFin.Date)
            {
                await DisplayAlert("Erreur", "La date de début doit être avant la date de fin.", "OK");
                return;
            }

            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "HegreHotel.db3");
            var db = SingletonConnection.GetInstance(dbPath);

            var reservations = await db.Table<Models.Reservation>().ToListAsync();
            var chambresReservees = reservations
                .Where(r => !(DatePickerFin.Date <= r.DateDebut || DatePickerDebut.Date >= r.DateFin))
                .Select(r => r.ChambreId)
                .ToList();

            _chambresDispo = await db.Table<Models.Chambre>().Where(c => !chambresReservees.Contains(c.Id)).ToListAsync();

            if (_chambresDispo.Count == 0)
            {
                await DisplayAlert("Aucune chambre disponible", "Aucune chambre n'est disponible pour cette période.", "OK");
                return;
            }

            _clients = await db.Table<Models.Client>().ToListAsync();
            PickerClient.ItemsSource = _clients.Select(c => $"{c.Nom} {c.Prenom}").ToList();
            PickerChambre.ItemsSource = _chambresDispo.Select(ch => ch.Nom).ToList();

            SelectionSection.IsVisible = true;
            _datesValidees = true;
        }

        private async void OnEnregistrerClicked(object sender, EventArgs e)
        {
            if (!_datesValidees)
            {
                await DisplayAlert("Erreur", "Veuillez d'abord vérifier la disponibilité des chambres.", "OK");
                return;
            }

            if (PickerClient.SelectedIndex == -1 || PickerChambre.SelectedIndex == -1)
            {
                await DisplayAlert("Erreur", "Veuillez sélectionner un client et une chambre", "OK");
                return;
            }

            var reservation = new Models.Reservation
            {
                ClientId = _clients[PickerClient.SelectedIndex].Id,
                ChambreId = _chambresDispo[PickerChambre.SelectedIndex].Id,
                NbInvite = int.TryParse(EntryNbInvite.Text, out int nb) ? nb : 0,
                DateDebut = DatePickerDebut.Date,
                DateFin = DatePickerFin.Date,
                DemandeSpecifique = EntryDemandeSpecifique.Text
            };

            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "HegreHotel.db3");
            var db = SingletonConnection.GetInstance(dbPath);
            await db.InsertAsync(reservation);

            await DisplayAlert("Succès", "Réservation ajoutée avec succès.", "OK");
            await Navigation.PopAsync();
        }
    }
}
