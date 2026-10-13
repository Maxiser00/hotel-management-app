using HegreHotel.Models;
using Microsoft.Maui.Controls;
using System.IO;
using Microsoft.Maui.Storage;

namespace HegreHotel.Views.Chambre
{
    public partial class ModifierChambrePage : ContentPage
    {
        Models.Chambre _chambre;
        private List<Status> _statuts;
        
        public ModifierChambrePage(Models.Chambre chambre)
        {
            InitializeComponent();
            _chambre = chambre;

            EntryNom.Text = _chambre.Nom;
            EntryDescription.Text = _chambre.Description;
            EntryTarif.Text = _chambre.Tarif.ToString();
            EntryNbPersonne.Text = _chambre.NbPersonne.ToString();
            
            LoadStatuts();
        }
        
        private async void LoadStatuts()
        {
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "HegreHotel.db3");
            var db = SingletonConnection.GetInstance(dbPath);

            _statuts = await db.Table<Status>().ToListAsync();

            PickerStatut.ItemsSource = _statuts.Select(s => s.Libelle).ToList();
            PickerStatut.SelectedIndex = _statuts.FindIndex(s => s.Id == _chambre.StatusId);
        }

        private async void OnEnregistrerClicked(object sender, EventArgs e)
        {
            if (PickerStatut.SelectedIndex == -1)
            {
                await DisplayAlert("Erreur", "Veuillez sélectionner un statut", "OK");
                return;
            }
            
            _chambre.Nom = EntryNom.Text;
            _chambre.Description = EntryDescription.Text;
            _chambre.Tarif = decimal.TryParse(EntryTarif.Text, out decimal tarif) ? tarif : _chambre.Tarif;
            _chambre.NbPersonne = int.TryParse(EntryNbPersonne.Text, out int nb) ? nb : _chambre.NbPersonne;
            
            _chambre.StatusId = _statuts[PickerStatut.SelectedIndex].Id;

            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "HegreHotel.db3");
            var db = SingletonConnection.GetInstance(dbPath);
            
            Console.WriteLine(_chambre.StatusId);
            await db.UpdateAsync(_chambre);
            await Navigation.PopAsync();
        }
    }
}