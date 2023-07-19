using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WorkshopManager.Models;
using WorkshopManager.Views;

namespace WorkshopManager.ViewModels
{
    public class EmployeeViewModel: INotifyPropertyChanged
    {
        public Command GoToCommand { get; set; }
        public Command TESTCommand { get; set; }

        public ObservableCollection<EmployeeWithOccupations> EmployeeWithOccupationsCollection { get; set; }

        public EmployeeViewModel(AddNewEmployeeViewModel addNewEmployeeViewModel) 
        {
            GoToCommand = new Command(GoTo);
            TESTCommand = new Command(()=>Debug.WriteLine("No info to debug"));

            EmployeeWithOccupationsCollection = new ObservableCollection<EmployeeWithOccupations>();

            addNewEmployeeViewModel.AddedNewEmployee += ResetEmployeeWithOccupations;
        }

        public void GoTo()
        {
            Shell.Current.GoToAsync(nameof(AddNewEmployeePage));
        }

        private void ResetEmployeeWithOccupations()
        {
            EmployeeWithOccupationsCollection.Clear();

            foreach(Employee emp in WorkshopDB.Connection.Table<Employee>()) 
            {
                EmployeeWithOccupations employeeWithOccupations = new EmployeeWithOccupations();
                employeeWithOccupations.Employee = emp;
                var queryToOccupation = WorkshopDB.Connection.Table<Occupation>().Where(o => o.EmployeeID == emp.ID);
                foreach (var occupation in queryToOccupation)
                {
                    var queryToJob = WorkshopDB.Connection.Table<TypeOfJob>().Where(t => t.ID == occupation.TypeOfJobID).ToList();

                    foreach (var job in queryToJob)
                    {
                        employeeWithOccupations.Job.Add(job);
                    }
                }
                EmployeeWithOccupationsCollection.Add(employeeWithOccupations);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
