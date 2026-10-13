using System.IO;
using Microsoft.Maui.Storage;
using System.Threading.Tasks;
using HegreHotel.Models;

namespace HegreHotel
{
    public class Database
    {
        public static async Task CreateDatabaseAsync()
        {
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "HegreHotel.db3");
            var db = SingletonConnection.GetInstance(dbPath);

            await db.CreateTableAsync<Client>();
            await db.CreateTableAsync<Reservation>();
            await db.CreateTableAsync<Chambre>();
            await db.CreateTableAsync<Status>();
            
            var statuts = await db.Table<Status>().ToListAsync();
            if (statuts.Count == 0)
            {
                await db.InsertAsync(new Status { Libelle = "Disponible" });
                await db.InsertAsync(new Status { Libelle = "Occupée" });
                await db.InsertAsync(new Status { Libelle = "Indisponible" });
            }
        }
    }
}