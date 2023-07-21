using Microsoft.Maui.Controls;
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

namespace WorkshopManager.ViewModels
{
    [QueryProperty(nameof(EmployeeWithOccupations), "EmployeeWithOccupations")]
    public class EmployeeDetailsPageViewModel: INotifyPropertyChanged
    { 
        private EmployeeWithOccupations _employeeWithOccupations;
        private Status _status;
        public Status StatusValue
        {
            get => _status;
            set
            {
                if (_status == value)
                    return;
                _status = value;
                OnPropertyChanged(nameof(StatusValue));
            }
        }
        public EmployeeWithOccupations EmployeeWithOccupations
        {
            get { return _employeeWithOccupations; }
            set 
            {
                if (_employeeWithOccupations == value)
                    return;

                _employeeWithOccupations = value;
                OnPropertyChanged(nameof(EmployeeWithOccupations));
                UpdateSelectedTypeOfJobs();
            }
        }

        public ObservableCollection<TypeOfJob> TypeOfJobs { get; set; }
        public ObservableCollection<object> SelectedTypeOfJobs { get; set; }

        public Command EmployeeChangeInfoCommand { get; set; }

        public EmployeeDetailsPageViewModel() 
        {
            TypeOfJobs = new ObservableCollection<TypeOfJob>(WorkshopDB.Connection.Table<TypeOfJob>());
            SelectedTypeOfJobs = new ObservableCollection<object>();
            EmployeeChangeInfoCommand = new Command(EmployeeChangeInfo);
        }

        public void EmployeeChangeInfo()
        {
            if (string.IsNullOrWhiteSpace(EmployeeWithOccupations.Employee.FullName))
            {
                Error("You did not enter a name");
                return;
            }
            if (string.IsNullOrWhiteSpace(EmployeeWithOccupations.Employee.Phone))
            {
                Error("You did not enter a phone");
                return;
            }
            if (SelectedTypeOfJobs.Count == 0)
            {
                Error("You did not pick any job");
                return;
            }

            var updatedEmployee = new Employee()
            {
                ID = EmployeeWithOccupations.Employee.ID,
                FullName = EmployeeWithOccupations.Employee.FullName,
                Phone = EmployeeWithOccupations.Employee.Phone,
                Status = StatusValue
            };

            try
            {
                WorkshopDB.Connection.Update(updatedEmployee);
            }
            catch (SQLite.SQLiteException)
            {
                Error("Database already has this phone");
                return;
            }

            var previousOccupations = WorkshopDB.Connection.Table<Occupation>()
                .Where(o => o.EmployeeID == updatedEmployee.ID).ToList();

            var selectedJobIDs = new HashSet<long>(SelectedTypeOfJobs.
                Cast<TypeOfJob>().Select(j => j.ID));

            foreach (var previousOccupation in previousOccupations)
            {
                if (!selectedJobIDs.Contains(previousOccupation.TypeOfJobID))
                {
                    // Delete the row from the Occupation table
                    WorkshopDB.Connection.Delete(previousOccupation);
                }
            }

            foreach (TypeOfJob selectedJob in SelectedTypeOfJobs)
            {
                if (!previousOccupations.Any(o => o.TypeOfJobID == selectedJob.ID))
                {
                    // Insert a new row into the Occupation table
                    var newOccupation = new Occupation
                    {
                        EmployeeID = updatedEmployee.ID,
                        TypeOfJobID = selectedJob.ID
                    };
                    WorkshopDB.Connection.Insert(newOccupation);
                }
            }


            MessagingCenter.Send(this, "Reset view");


            Shell.Current.GoToAsync("..");
        }

        private void UpdateSelectedTypeOfJobs()
        {
            if (EmployeeWithOccupations is null)
                return;

            foreach (var job in TypeOfJobs)
            {
                foreach (var empJob in EmployeeWithOccupations.Job)
                {
                    if (job.ID == empJob.ID)
                    {
                        SelectedTypeOfJobs.Add(job);
                    }
                }
            }

            StatusValue = EmployeeWithOccupations.Employee.Status;
        }
        private async void Error(string message)
        {
            await Shell.Current.DisplayAlert("Error", message, "OK");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
