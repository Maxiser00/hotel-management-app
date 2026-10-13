using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HegreHotel.Views.Client;

public partial class DetailsClientPage : ContentPage
{
    public Models.Client Client { get; set; }

    public DetailsClientPage(Models.Client client)
    {
        InitializeComponent();
        BindingContext = client;
        Client = client;
        LoadReservations();
    }
    
    private async void LoadReservations()
    {
        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "HegreHotel.db3");
        var db = SingletonConnection.GetInstance(dbPath);
        List<Models.Reservation> reservationsList = await db.Table<Models.Reservation>().Where(x => x.ClientId == Client.Id).ToListAsync();
        List<Models.Chambre> cahmbresList = await db.Table<Models.Chambre>().ToListAsync();
        List<ReservationInfo> reservationInfoList = new List<ReservationInfo>();
        
        if (reservationsList is null)
        {
            await DisplayAlert("Aucune réservation", "Le client n'a pas de réservations", "OK");
        }
        else
        {
            foreach (Models.Reservation reservation in reservationsList)
            {
                ReservationInfo reservationInfo = new ReservationInfo()
                {
                    DateDebut = reservation.DateDebut,
                    DateFin = reservation.DateFin,
                    DemandeSpecifique = reservation.DemandeSpecifique,
                    NbInvite = reservation.NbInvite
                };
                foreach (Models.Chambre chambre in cahmbresList)
                {
                    if (reservation.ChambreId == chambre.Id)
                    {
                        reservationInfo.ChambreNom = chambre.Nom;
                    }
                }
                reservationInfoList.Add(reservationInfo);
            }
        }
        
        ListeReservations.ItemsSource = reservationInfoList;
    }
    
    public class ReservationInfo
    {
        public DateTime DateDebut { get; set; }

        public DateTime DateFin { get; set; }

        public string? DemandeSpecifique { get; set; }
    
        public int NbInvite { get; set; }
        
        public string? ChambreNom { get; set; }
    }
}