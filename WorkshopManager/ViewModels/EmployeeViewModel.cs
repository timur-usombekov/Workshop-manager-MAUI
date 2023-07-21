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
        public Command GoToAddNewEmployeePageCommand { get; set; }
        public Command<EmployeeWithOccupations> GoToEmployeeDetailsPageCommand { get; set; }
        public Command TESTCommand { get; set; }

        public ObservableCollection<EmployeeWithOccupations> EmployeeWithOccupationsCollection { get; set; }

        public EmployeeViewModel(AddNewEmployeeViewModel addNewEmployeeViewModel) 
        {
            GoToAddNewEmployeePageCommand = new Command(GoToAddNewEmployeePage);
            GoToEmployeeDetailsPageCommand = new Command<EmployeeWithOccupations>(GoToEmployeeDetailsPage);
            TESTCommand = new Command(()=>Debug.WriteLine("No info to debug"));

            EmployeeWithOccupationsCollection = new ObservableCollection<EmployeeWithOccupations>();
            ResetEmployeeWithOccupations();

            MessagingCenter.Subscribe<AddNewEmployeeViewModel>(this, "Reset view", (sender) =>
            {
                ResetEmployeeWithOccupations();
            });
            MessagingCenter.Subscribe<EmployeeDetailsPageViewModel>(this, "Reset view", (sender) =>
            {
                ResetEmployeeWithOccupations();
                WorkshopDB.GetAllTablesToDebug();
            });
            //addNewEmployeeViewModel.AddedNewEmployee += ResetEmployeeWithOccupations;
        }

        public async void GoToAddNewEmployeePage()
        {
            await Shell.Current.GoToAsync(nameof(AddNewEmployeePage));
        }
        public async void GoToEmployeeDetailsPage(EmployeeWithOccupations employeeWithOccupations)
        {
            if (employeeWithOccupations is null)
                return;

            await Shell.Current.GoToAsync($"{nameof(EmployeeDetailsPage)}",  true, 
                new Dictionary<string, object>
                {
                    {"EmployeeWithOccupations", employeeWithOccupations}
                });
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
