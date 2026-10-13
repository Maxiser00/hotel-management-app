namespace HegreHotel.Models;

using SQLite;

public class Status
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    
    public string? Libelle { get; set; }
}
