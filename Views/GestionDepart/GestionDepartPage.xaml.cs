using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using HegreHotel.Models;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.IO;
using Microsoft.Maui.Storage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HegreHotel.Views.GestionDepart;

public partial class GestionDepartPage : ContentPage
{
    public ObservableCollection<Models.Reservation> ReservationsDeb { get; set; } = new ObservableCollection<Models.Reservation>();
    public ObservableCollection<Models.Reservation> ReservationsFin { get; set; } = new ObservableCollection<Models.Reservation>();

    public GestionDepartPage()
    {
        InitializeComponent();
        LoadReservations();
    }
    
    private async void LoadReservations()
    {
        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "HegreHotel.db3");
        var db = SingletonConnection.GetInstance(dbPath);
        var reservationList = await db.Table<Models.Reservation>().ToListAsync();
        ReservationsDeb.Clear();
        ReservationsFin.Clear();
        foreach (var reservation in reservationList)
        {
            if (reservation.DateDebut == DateTime.Now.Date)
            {
                ReservationsDeb.Add(reservation);
            } else if (reservation.DateFin == DateTime.Now.Date)
            {
                ReservationsFin.Add(reservation);
            }
            
        }
        
        
        GestionArriveListView.ItemsSource = ReservationsDeb;

        GestionDepartListView.ItemsSource = ReservationsFin;
        
    }
    
    private void GestionPage_OnAppearing(object? sender, EventArgs e)
    {
        LoadReservations();
    }
}