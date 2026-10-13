namespace HegreHotel.Models;

using SQLite;

public class Client
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [MaxLength(100)]
    public string? Nom { get; set; }

    [MaxLength(100)]
    public string? Prenom { get; set; }

    [MaxLength(150), Unique]
    public string? Email { get; set; }

    [MaxLength(15), Unique]
    public string? Telephone { get; set; }
    
    public override string ToString() => $"{Nom} {Prenom}";
}