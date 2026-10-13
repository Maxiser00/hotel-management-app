using HegreHotel.Models;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.IO;
using Microsoft.Maui.Storage;

namespace HegreHotel.Views.Client
{
    public partial class ClientsPage : ContentPage
    {
        public ObservableCollection<Models.Client> Clients { get; set; } = new ObservableCollection<Models.Client>();

        public ClientsPage()
        {
            InitializeComponent();
            LoadClients();
        }

        private async void LoadClients()
        {
            try
            {
                string dbPath = Path.Combine(FileSystem.AppDataDirectory, "HegreHotel.db3");
                var db = SingletonConnection.GetInstance(dbPath);
                var clientsList = await db.Table<Models.Client>().ToListAsync();

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Clients.Clear();
                    foreach (var client in clientsList)
                        Clients.Add(client);

                    ClientsCollectionView.ItemsSource = Clients;
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du chargement des clients : {ex.Message}");
            }
        }

        private async void OnAjouterClientClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AjouterClientPage());
        }

        private async void OnModifierClientClicked(object sender, EventArgs e)
        {
            if (sender is ImageButton imageButton && imageButton.CommandParameter is Models.Client client)
            {
                await Navigation.PushAsync(new ModifierClientPage(client));
            }
        }

        private async void OnSupprimerClientClicked(object sender, EventArgs e)
        {
            if (sender is ImageButton imageButton && imageButton.CommandParameter is Models.Client client)
            {
                bool confirmation = await DisplayAlert("Suppression", "Voulez-vous vraiment supprimer ce client ?", "Oui", "Non");
                if (confirmation)
                {
                    string dbPath = Path.Combine(FileSystem.AppDataDirectory, "HegreHotel.db3");
                    var db = SingletonConnection.GetInstance(dbPath);
                    await db.DeleteAsync(client);

                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        Clients.Remove(client);
                    });
                }
            }
        }

        private void ClientsPage_OnAppearing(object sender, EventArgs e)
        {
            LoadClients();
        }

        private async void OnClientTapped(object sender, EventArgs e)
        {
            if (sender is Frame frame && frame.BindingContext is Models.Client client)
            {
                await Navigation.PushAsync(new DetailsClientPage(client));
            }
        }
    }
}
