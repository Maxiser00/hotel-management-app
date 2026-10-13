using SQLite;
using System.IO;
using Microsoft.Maui.Storage;

namespace HegreHotel
{
    public class SingletonConnection : SQLiteAsyncConnection
    {
        private static SingletonConnection? _instance;

        private SingletonConnection(string dbPath) : base(dbPath)
        {
        }

        public static SingletonConnection GetInstance(string dbPath)
        {
            if (_instance == null)
            {
                _instance = new SingletonConnection(dbPath);
            }
            return _instance;
        }
    }
}