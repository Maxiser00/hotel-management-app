using HegreHotel.Models;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Microsoft.Maui.Storage;
using System.Collections.Generic;

namespace HegreHotel.Views.Chambre
{
    public partial class ChambresPage : ContentPage
    {
        private ObservableCollection<Models.Chambre> _toutesLesChambres = new();
        private List<Status> _statuts;

        public ChambresPage()
        {
            InitializeComponent();
            LoadChambresEtStatuts();
        }

        private async void LoadChambresEtStatuts()
        {
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "HegreHotel.db3");
            var db = SingletonConnection.GetInstance(dbPath);

            _statuts = await db.Table<Status>().ToListAsync();

            var statutsAvecTous = new List<string> { "Tous" };
            statutsAvecTous.AddRange(_statuts.Select(s => s.Libelle));
            PickerFiltreStatus.ItemsSource = statutsAvecTous;

            var chambresList = await db.Table<Models.Chambre>().ToListAsync();
            var reservationsList = await db.Table<Models.Reservation>().ToListAsync();

            DateTime today = DateTime.Today;

            foreach (var chambre in chambresList)
            {
                var chambreReservee = reservationsList
                    .Where(r => r.ChambreId == chambre.Id && r.DateDebut <= today && r.DateFin >= today)
                    .OrderByDescending(r => r.DateFin) 
                    .FirstOrDefault();

                if (chambreReservee != null)
                {
                    chambre.StatusId = 2;
                }
                else
                {
                    var chambreFinieRecemment = reservationsList
                        .Where(r => r.ChambreId == chambre.Id && r.DateFin >= today.AddDays(-2) && r.DateFin < today)
                        .OrderByDescending(r => r.DateFin)
                        .FirstOrDefault();

                    if (chambreFinieRecemment != null)
                    {
                        chambre.StatusId = 3;
                    }
                    else
                    {
                        chambre.StatusId = 1; 
                    }
                }
                chambre.Status = _statuts.FirstOrDefault(s => s.Id == chambre.StatusId);
                await db.UpdateAsync(chambre);
            }

            _toutesLesChambres.Clear();
            foreach (var chambre in chambresList)
            {
                _toutesLesChambres.Add(chambre);
            }

            FiltrerChambres();
        }
        
        private void OnFiltreChanged(object sender, EventArgs e)
        {
            FiltrerChambres();
        }

        private void FiltrerChambres()
        {
            if (PickerFiltreStatus.SelectedIndex <= 0) 
            {
                ChambresListView.ItemsSource = _toutesLesChambres;
            }
            else
            {
                string statutSelectionne = PickerFiltreStatus.SelectedItem.ToString();
                var statut = _statuts.FirstOrDefault(s => s.Libelle == statutSelectionne);

                if (statut != null)
                {
                    ChambresListView.ItemsSource = _toutesLesChambres.Where(c => c.StatusId == statut.Id).ToList();
                }
                else
                {
                    Console.WriteLine($"⚠ Filtrage impossible : statut '{statutSelectionne}' introuvable.");
                }
            }
        }

        private async void OnAjouterChambreClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AjouterChambrePage());
        }

        private async void OnModifierChambreClicked(object sender, EventArgs e)
        {
            if (sender is ImageButton imageButton && imageButton.CommandParameter is Models.Chambre chambre)
            {
                await Navigation.PushAsync(new ModifierChambrePage(chambre));
            }
        }

        private async void OnSupprimerChambreClicked(object sender, EventArgs e)
        {
            if (sender is ImageButton imageButton && imageButton.CommandParameter is Models.Chambre chambre)
            {
                bool confirmation = await DisplayAlert("Suppression", "Voulez-vous vraiment supprimer cette chambre ?", "Oui", "Non");
                if (confirmation)
                {
                    string dbPath = Path.Combine(FileSystem.AppDataDirectory, "HegreHotel.db3");
                    var db = SingletonConnection.GetInstance(dbPath);
                    await db.DeleteAsync(chambre);
                    LoadChambresEtStatuts();
                }
            }
        }

        private void ChambresPage_OnAppearing(object? sender, EventArgs e)
        {
            LoadChambresEtStatuts();
        }
    }
}
