using HegreHotel.Models;
using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Maui.Storage;

namespace HegreHotel.Views.Reservation
{
    public partial class ModifierReservationPage : ContentPage
    {
        private Models.Reservation _reservation;
        private List<Models.Client> _clients;
        private List<Models.Chambre> _chambres;

        public ModifierReservationPage(Models.Reservation reservation)
        {
            InitializeComponent();
            _reservation = reservation;
            LoadClientsAndChambres();
        }

        private async void LoadClientsAndChambres()
        {
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "HegreHotel.db3");
            var db = SingletonConnection.GetInstance(dbPath);

            _clients = await db.Table<Models.Client>().ToListAsync();
            _chambres = await db.Table<Models.Chambre>().ToListAsync();

            PickerClient.ItemsSource = _clients.Select(c => $"{c.Nom} {c.Prenom}").ToList();
            PickerChambre.ItemsSource = _chambres.Select(ch => ch.Nom).ToList();

            PickerClient.SelectedIndex = _clients.FindIndex(c => c.Id == _reservation.ClientId);
            PickerChambre.SelectedIndex = _chambres.FindIndex(ch => ch.Id == _reservation.ChambreId);
            
            EntryNbInvite.Text = _reservation.NbInvite.ToString();
            DatePickerDebut.Date = _reservation.DateDebut;
            DatePickerFin.Date = _reservation.DateFin;
            EntryDemandeSpecifique.Text = _reservation.DemandeSpecifique;
        }

        private async void OnEnregistrerClicked(object sender, EventArgs e)
        {
            if (PickerClient.SelectedIndex == -1 || PickerChambre.SelectedIndex == -1)
            {
                await DisplayAlert("Erreur", "Veuillez sélectionner un client et une chambre", "OK");
                return;
            }

            _reservation.ClientId = _clients[PickerClient.SelectedIndex].Id;
            _reservation.ChambreId = _chambres[PickerChambre.SelectedIndex].Id;
            _reservation.NbInvite = int.TryParse(EntryNbInvite.Text, out int nb) ? nb : _reservation.NbInvite;
            _reservation.DateDebut = DatePickerDebut.Date;
            _reservation.DateFin = DatePickerFin.Date;
            _reservation.DemandeSpecifique = EntryDemandeSpecifique.Text;

            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "HegreHotel.db3");
            var db = SingletonConnection.GetInstance(dbPath);
            await db.UpdateAsync(_reservation);
            await Navigation.PopAsync();
        }
    }
}
