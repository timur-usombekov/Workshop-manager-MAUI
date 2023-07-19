using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkshopManager.Models
{
    public class TypeOfJob
    {
        [PrimaryKey, AutoIncrement, Unique]
        public long ID { get; set; }
        public string Job { get; set; }
    }
}
