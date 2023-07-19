using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using WorkshopManager.Models;

namespace WorkshopManager.ViewModels
{
    public class AddNewEmployeeViewModel: INotifyPropertyChanged
    {
        private string _fullNameEntry;
        private string _phoneEntry;

        public string FullNameEntry
        {
            get { return _fullNameEntry; }
            set
            {
                if (FullNameEntry == value)
                    return;

                _fullNameEntry = value;
                OnPropertyChanged(nameof(FullNameEntry));
            }
        }
        public string PhoneEntry
        {
            get { return _phoneEntry; }
            set
            {
                if (PhoneEntry == value)
                    return;

                _phoneEntry = value;
                OnPropertyChanged(nameof(PhoneEntry));
            }
        }

        public Command AddNewEmployeeCommand { get; set; }
        public Command GetNewJobCommand { get; set; }

        public ObservableCollection<object> SelectedTypeOfJobs { get; set; }
        public ObservableCollection<TypeOfJob> TypeOfJobs { get; set; }

        public AddNewEmployeeViewModel()
        {
            AddNewEmployeeCommand = new Command(AddNewEmployee);
            GetNewJobCommand = new Command(GetNewJobForDB);

            SelectedTypeOfJobs = new ObservableCollection<object>();
            TypeOfJobs = new ObservableCollection<TypeOfJob>();
        }

        public void AddNewEmployee()
        {
            if (string.IsNullOrWhiteSpace(FullNameEntry))
            {
                Shell.Current.DisplayAlert("Error", "You did not enter a name", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(PhoneEntry))
            {
                Shell.Current.DisplayAlert("Error", "You did not enter a phone", "OK");
                return;
            }
            if (SelectedTypeOfJobs.Count == 0)
            {
                Shell.Current.DisplayAlert("Error", "You did not pick any job", "OK");
                return;
            }

            Employee employee = new Employee()
            {
                FullName = FullNameEntry, 
                Phone = PhoneEntry,
                Status = Status.Working
            };

            WorkshopDB.Connection.Insert(employee);
            // TODO: Add insert into the Employees collection here (or not?)

            foreach (object job in SelectedTypeOfJobs)
            {
                TypeOfJob typeOfJob = job as TypeOfJob;

                Occupation occupation = new Occupation()
                {
                    EmployeeID = employee.ID,
                    TypeOfJobID = typeOfJob.ID
                };

                WorkshopDB.Connection.Insert(occupation);
                Debug.WriteLine($"add new worker with id={employee.ID} and typeOfJob={typeOfJob.ID}");
            }

            ClearEntry();
            OnEmployeeAdded();

            Shell.Current.GoToAsync("..");
        }

        public async void GetNewJobForDB()
        {
            var job = await Shell.Current.DisplayPromptAsync("add new job", "new job");
            TypeOfJob typeOfJob = new() { Job = job };

            WorkshopDB.Connection.Insert(typeOfJob);
            TypeOfJobs.Add(typeOfJob);
        }

        private void ClearEntry()
        {
            FullNameEntry = null;
            PhoneEntry = null;
            SelectedTypeOfJobs.Clear();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public event Action AddedNewEmployee;    
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void OnEmployeeAdded()
        {
            AddedNewEmployee?.Invoke();
        }
    }
}
