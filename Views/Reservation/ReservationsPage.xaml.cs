using HegreHotel.Models;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.IO;
using Microsoft.Maui.Storage;

namespace HegreHotel.Views.Reservation
{
    public partial class ReservationsPage : ContentPage
    {
        public ObservableCollection<Models.Reservation> Reservations { get; set; } = new ObservableCollection<Models.Reservation>();

        public ReservationsPage()
        {
            InitializeComponent();
            LoadReservations();
        }

        private async void LoadReservations()
        {
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "HegreHotel.db3");
            var db = SingletonConnection.GetInstance(dbPath);
            var reservationsList = await db.Table<Models.Reservation>().ToListAsync();
            Reservations.Clear();
            foreach (var reservation in reservationsList)
            {
                Reservations.Add(reservation);
            }
            ReservationsListView.ItemsSource = Reservations;
        }

        private async void OnAjouterReservationClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AjouterReservationPage());
        }

        private async void OnModifierReservationClicked(object sender, EventArgs e)
        {
            if (sender is ImageButton imageButton && imageButton.CommandParameter is Models.Reservation reservation)
            {
                await Navigation.PushAsync(new ModifierReservationPage(reservation));
            }
        }

        private async void OnSupprimerReservationClicked(object sender, EventArgs e)
        {
            if (sender is ImageButton imageButton && imageButton.CommandParameter is Models.Reservation reservation)
            {
                bool confirmation = await DisplayAlert("Suppression", "Voulez-vous vraiment supprimer cette réservation ?", "Oui", "Non");
                if (confirmation)
                {
                    string dbPath = Path.Combine(FileSystem.AppDataDirectory, "HegreHotel.db3");
                    var db = SingletonConnection.GetInstance(dbPath);
                    await db.DeleteAsync(reservation);
                    LoadReservations();
                }
            }
        }

        private void ReservationsPage_OnAppearing(object? sender, EventArgs e)
        {
            LoadReservations();
        }
    }
}
