using HegreHotel.Models;
using Microsoft.Maui.Controls;
using System.IO;
using Microsoft.Maui.Storage;

namespace HegreHotel.Views.Client
{
    public partial class AjouterClientPage : ContentPage
    {
        public AjouterClientPage()
        {
            InitializeComponent();
        }

        private async void OnEnregistrerClicked(object sender, EventArgs e)
        {
            var client = new Models.Client()
            {
                Nom = EntryNom.Text,
                Prenom = EntryPrenom.Text,
                Email = EntryEmail.Text,
                Telephone = EntryTelephone.Text,
            };

            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "HegreHotel.db3");
            var db = SingletonConnection.GetInstance(dbPath);
            await db.InsertAsync(client);
            await Navigation.PopAsync();
        }
    }
}