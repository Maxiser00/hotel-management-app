using HegreHotel.Models;
using Microsoft.Maui.Controls;
using System.IO;
using Microsoft.Maui.Storage;

namespace HegreHotel.Views.Client
{
    public partial class ModifierClientPage : ContentPage
    {
        Models.Client _client;
        public ModifierClientPage(Models.Client client)
        {
            InitializeComponent();
            _client = client;

            EntryNom.Text = _client.Nom;
            EntryPrenom.Text = _client.Prenom;
            EntryEmail.Text = _client.Email;
            EntryTelephone.Text = _client.Telephone;
        }

        private async void OnEnregistrerClicked(object sender, EventArgs e)
        {
            _client.Nom = EntryNom.Text;
            _client.Prenom = EntryPrenom.Text;
            _client.Email = EntryEmail.Text;
            _client.Telephone = EntryTelephone.Text;

            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "HegreHotel.db3");
            var db = SingletonConnection.GetInstance(dbPath);
            await db.UpdateAsync(_client);
            await Navigation.PopAsync();
        }
    }
}