namespace HegreHotel.Models;

using SQLite;

public class Chambre
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [MaxLength(50)]
    public string? Nom { get; set; }

    public string? Description { get; set; }

    public decimal Tarif { get; set; }

    public int NbPersonne { get; set; }
    
    public int StatusId { get; set; }

    [Ignore]  
    public Status Status { get; set; }
}