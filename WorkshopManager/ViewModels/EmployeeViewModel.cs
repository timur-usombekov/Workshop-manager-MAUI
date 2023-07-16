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
    public class EmployeeViewModel: BaseViewModel, INotifyPropertyChanged
    {
        private string _fullNameEntry;
        private string _phoneEntry;
        private string _typeOfJobEntry;

        public string FullNameEntry
        {
            get { return _fullNameEntry; }
            set 
            { 
                if(FullNameEntry == value)
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
        public string TypeOfJobEntry
        {
            get { return _typeOfJobEntry; }
            set
            {
                if (PhoneEntry == value)
                    return;

                _typeOfJobEntry = value;
                OnPropertyChanged(nameof(TypeOfJobEntry));
            }
        }

        public Command AddEmployeeCommand { get; set; }
        public Command DeleteAllEmployeesCommand { get; set; }
        public Command GoToCommand { get; set; }
        public ObservableCollection<Employee> Employees { get; set; }

        public EmployeeViewModel() 
        {
            GoToCommand = new Command(GoTo);
            AddEmployeeCommand = new Command(Add);
            DeleteAllEmployeesCommand = new Command(Remove);
            Employees = new ObservableCollection<Employee>(Connection.Table<Employee>());
        }

        public override void Add()
        {
            Employee employee = new Employee() 
                { 
                    FullName = FullNameEntry, 
                    Phone = PhoneEntry, 
                    TypeOfJob = TypeOfJobEntry
            };
            Connection.Insert(employee);
            Employees.Add(employee);

            Shell.Current.GoToAsync("..");
        }

        public override void Remove()
        {
            Connection.DeleteAll<Employee>();
            Employees.Clear();
        }

        public void GoTo()
        {
            Shell.Current.GoToAsync(nameof(AddNewEmployeePage));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
