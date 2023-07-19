using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkshopManager.Models
{
    public class EmployeeWithOccupations
    {
        public Employee Employee { get; set; }
        public ObservableCollection<TypeOfJob> Job { get; set; }

        public EmployeeWithOccupations() 
        {
            Job = new ObservableCollection<TypeOfJob>();
        }
    }
}
