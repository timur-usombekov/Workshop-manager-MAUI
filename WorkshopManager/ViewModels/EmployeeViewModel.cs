using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkshopManager.Models;

namespace WorkshopManager.ViewModels
{
    public class EmployeeViewModel
    {
        private WorkshopDB _db;

        public List<Employee> Employees { get; set; }

        public EmployeeViewModel()
        {
            _db = new WorkshopDB();
        }
    }
}
