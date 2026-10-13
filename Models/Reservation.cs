namespace HegreHotel.Models;

using SQLite;

public class Reservation
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public DateTime DateDebut { get; set; }

    public DateTime DateFin { get; set; }

    public string? DemandeSpecifique { get; set; }
    
    public int NbInvite { get; set; }
    
    public int ClientId { get; set; }
    public int ChambreId { get; set; }
    
}
