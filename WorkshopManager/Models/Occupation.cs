using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkshopManager.Models
{
    public class Occupation
    {
        [PrimaryKey, AutoIncrement, Unique]
        public long ID { get; set; }
        public long TypeOfJobID { get; set; }
        public long EmployeeID { get; set; }
    }
}
