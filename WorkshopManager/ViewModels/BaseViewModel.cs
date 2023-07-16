using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;
using WorkshopManager.Models;

namespace WorkshopManager.ViewModels
{
    public abstract class BaseViewModel 
    {
        protected SQLiteConnection Connection { get; }

        public BaseViewModel()
        {
            Connection = WorkshopDB.Init();
        }

        public abstract void Add();
        public abstract void Remove();
    }
}
