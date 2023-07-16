using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkshopManager.Models
{
    public static class WorkshopDB
    {
        private static SQLiteConnection _db;

        public static SQLiteConnection Init()
        {
            if (_db != null)
                return _db;

            var path = Path.Combine(FileSystem.AppDataDirectory, "WorkshopDB.sqlite3");
            _db = new SQLiteConnection(path);

            _db.CreateTable<Employee>();

            return _db;
        }
    }

}
