using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkshopManager.Models
{
    public class Employee
    {
        [PrimaryKey, AutoIncrement, Unique]
        public long ID { get; set; }
        public string FullName { get; set; }
        [Unique]
        public string Phone { get; set; }
        public Status Status { get; set; }
    }

    public enum Status
    {
        Working,
        OnVacation,
        Fired
    }
}
