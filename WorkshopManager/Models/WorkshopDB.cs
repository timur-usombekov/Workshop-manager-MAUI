using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkshopManager.Models
{
    public static class WorkshopDB
    {
        private static SQLiteConnection _db;

        public static SQLiteConnection Connection
        {
            get 
            {
                if (_db == null)
                    Init();
                
                return _db; 
            }
            private set
            {
                _db = value;
            }
        }
        
        public static SQLiteConnection Init()
        {
            if (_db != null)
                return _db;

            var path = Path.Combine(FileSystem.AppDataDirectory, "WorkshopDB.sqlite3");
            _db = new SQLiteConnection(path);


            _db.DropTable<Employee>();
            _db.DropTable<TypeOfJob>();
            _db.DropTable<Occupation>();

            _db.CreateTable<Employee>();
            _db.CreateTable<TypeOfJob>();
            _db.CreateTable<Occupation>();

            return _db;
        }

        public static void GetAllTablesToDebug()
        {
            Debug.WriteLine($"Occupation - {Connection.Table<Occupation>().Count()}");
            foreach (var item in Connection.Table<Occupation>())
            {
                Debug.WriteLine($"id - {item.ID}, employee - {item.EmployeeID}, job - {item.TypeOfJobID}");
            }
            Debug.WriteLine($"Employee - {Connection.Table<Employee>().Count()}");
            foreach (var item in Connection.Table<Employee>())
            {
                Debug.WriteLine($"id - {item.ID}, name - {item.FullName}, phone - {item.Phone}, status - {item.Status}");
            }
            Debug.WriteLine($"Job - {Connection.Table<TypeOfJob>().Count()}");
            foreach (var item in Connection.Table<TypeOfJob>())
            {
                Debug.WriteLine($"id - {item.ID}, job - {item.Job}");
            }
        }
    }

}
