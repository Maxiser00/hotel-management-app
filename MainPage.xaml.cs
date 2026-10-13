using HegreHotel.Views;
using HegreHotel.Views.Chambre;
using HegreHotel.Views.Client;
using HegreHotel.Views.Reservation;
using HegreHotel.Views.GestionDepart;
using Microsoft.Maui.Controls;

namespace HegreHotel
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnClientsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ClientsPage());
        }

        private async void OnChambresClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ChambresPage());
        }

        private async void OnReservationsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ReservationsPage());
        }
        
        private async void OnGestionDepartClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new GestionDepartPage());
        }
    }
}