using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
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
                if (_fullNameEntry == value)
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
                if (_phoneEntry == value)
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
            TypeOfJobs = new ObservableCollection<TypeOfJob>(WorkshopDB.Connection.Table<TypeOfJob>());
        }

        public void AddNewEmployee()
        {
            if (string.IsNullOrWhiteSpace(FullNameEntry))
            {
                Error("You did not enter a name");
                return;
            }
            if (string.IsNullOrWhiteSpace(PhoneEntry))
            {
                Error("You did not enter a phone");
                return;
            }
            if (SelectedTypeOfJobs.Count == 0)
            {
                Error("You did not pick any job");
                return;
            }

            Employee employee = new Employee()
            {
                FullName = FullNameEntry, 
                Phone = PhoneEntry,
                Status = Status.Working
            };

            try
            {
                WorkshopDB.Connection.Insert(employee);
            }
            catch (SQLite.SQLiteException)
            {
                Error("Database already has this phone");
                return;
            }

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

            MessagingCenter.Send(this, "Reset view");

            Shell.Current.GoToAsync("..");
        }

        public async void GetNewJobForDB()
        {
            var job = await Shell.Current.DisplayPromptAsync("add new job", "new job");
            TypeOfJob typeOfJob = new() { Job = job };

            WorkshopDB.Connection.Insert(typeOfJob);
            TypeOfJobs.Add(typeOfJob);
        }
        public async void Error(string ex)
        {
            await Shell.Current.DisplayAlert("Error", ex, "OK");
        }

        private void ClearEntry()
        {
            FullNameEntry = null;
            PhoneEntry = null;
            SelectedTypeOfJobs.Clear();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        //public event Action AddedNewEmployee;    
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        //public void OnEmployeeAdded()
        //{
        //    AddedNewEmployee?.Invoke();
        //}
    }
}
