using HegreHotel.Models;
using Microsoft.Maui.Controls;
using System.IO;
using Microsoft.Maui.Storage;

namespace HegreHotel.Views.Chambre
{
    public partial class AjouterChambrePage : ContentPage
    {
        public AjouterChambrePage()
        {
            InitializeComponent();
        }

        private async void OnEnregistrerClicked(object sender, EventArgs e)
        {
            var chambre = new Models.Chambre
            {
                Nom = EntryNom.Text,
                Description = EntryDescription.Text,
                Tarif = decimal.TryParse(EntryTarif.Text, out decimal tarif) ? tarif : 0,
                NbPersonne = int.TryParse(EntryNbPersonne.Text, out int nb) ? nb : 0,
                StatusId = 1
            };

            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "HegreHotel.db3");
            var db = SingletonConnection.GetInstance(dbPath);
            await db.InsertAsync(chambre);
            await Navigation.PopAsync();
        }
    }
}