using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using EmployeeMeetingOrganizer.Model;
using EmployeeMeetingOrganizer.UI.Data.Lookups;
using EmployeeMeetingOrganizer.UI.Data.Repositories;
using EmployeeMeetingOrganizer.UI.Event;
using EmployeeMeetingOrganizer.UI.View.Services;
using EmployeeMeetingOrganizer.UI.ViewModel.Base;
using EmployeeMeetingOrganizer.UI.ViewModel.Interface;
using EmployeeMeetingOrganizer.UI.Wrapper;
using Prism.Commands;
using Prism.Events;

namespace EmployeeMeetingOrganizer.UI.ViewModel
{
    internal class EmployeeDetailViewModel : ViewModelBase, IEmployeeDetailViewModel
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEventAggregator _eventAggregator;
        private readonly IMessageDialogService _messageDialogService;
        private readonly IDepartmentsLookupDataService _departmentsLookupDataService;
        private PhoneNumberWrapper _selectedPhoneNumber;
        private EmployeeWrapper _employee;
        private bool _hasChanges;

        public EmployeeDetailViewModel(IEmployeeRepository dataService,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService,
            IDepartmentsLookupDataService departmentsLookupDataService)
        {
            _employeeRepository = dataService;
            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;
            _departmentsLookupDataService = departmentsLookupDataService;

            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute);
            AddPhoneNumberCommand = new DelegateCommand(OnAddPhoneNumberExecute);
            RemovePhoneNumberCommand = new DelegateCommand(OnRemovePhoneNumberExecute, OnRemovePhoneNumberCanExecute);


            Departments = new ObservableCollection<LookupItem>();
            PhoneNumbers = new ObservableCollection<PhoneNumberWrapper>();
        }

        public ObservableCollection<LookupItem> Departments { get; }
        
        public ObservableCollection<PhoneNumberWrapper> PhoneNumbers { get; }

        public async Task LoadAsync(int? employeeId)
        {
            var employee = employeeId.HasValue
                ? await _employeeRepository.GetByIdAsync(employeeId.Value)
                : CreateNewEmployee();

            InitializeEmployee(employee);

            InitializeEmployeePhoneNumbers(employee.PhoneNumbers);

            await LoadDepartmentsLookupAsync();
        }

        private void InitializeEmployee(Employee employee)
        {
            Employee = new EmployeeWrapper(employee);
            Employee.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = _employeeRepository.HasChanges();
                }

                if (e.PropertyName == nameof(Employee.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            if (Employee.Id == 0)
            {
                Employee.FirstName = "";
            }
        }

        private void InitializeEmployeePhoneNumbers(ICollection<EmployeePhone> phoneNumbers)
        {
            foreach (var wrapper in PhoneNumbers)
            {
                wrapper.PropertyChanged -= EmployeePhoneNumberWrapper_PropertyChanged;
            }
            PhoneNumbers.Clear();
            foreach (var employeePhoneNumber in phoneNumbers)
            {
                var wrapper = new PhoneNumberWrapper(employeePhoneNumber);
                PhoneNumbers.Add(wrapper);
                wrapper.PropertyChanged += EmployeePhoneNumberWrapper_PropertyChanged;
            }
        }

        private void EmployeePhoneNumberWrapper_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = _employeeRepository.HasChanges();
            }
            if (e.PropertyName == nameof(PhoneNumberWrapper.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        private async Task LoadDepartmentsLookupAsync()
        {
            Departments.Clear();
            //Departments.Add(new LookupItem { DisplayMember = "-" });

            var lookup = await _departmentsLookupDataService.GetDepartmentsLookupAsync();
            foreach (var lookupItem in lookup)
            {
                Departments.Add(lookupItem);
            }
        }

        public EmployeeWrapper Employee
        {
            get => _employee;
            private set
            {
                _employee = value;
                OnPropertyChanged();
            }
        }

        public PhoneNumberWrapper SelectedPhoneNumber
        {
            get => _selectedPhoneNumber;
            set
            {
                _selectedPhoneNumber = value;
                OnPropertyChanged();
                ((DelegateCommand)RemovePhoneNumberCommand).RaiseCanExecuteChanged();
            }
        }

        public bool HasChanges
        {
            get => _hasChanges;
            set
            {
                if (_hasChanges != value)
                {
                    _hasChanges = value;
                    OnPropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public ICommand SaveCommand { get; }

        public ICommand DeleteCommand { get; }

        public ICommand AddPhoneNumberCommand { get; }

        public ICommand RemovePhoneNumberCommand { get; }

        private bool OnSaveCanExecute()
        {
            return Employee != null 
                   && !Employee.HasErrors
                   && PhoneNumbers.All(pn => !pn.HasErrors)
                   && HasChanges;
        }

        private async void OnSaveExecute()
        {
            await _employeeRepository.SaveAsync();
            HasChanges = _employeeRepository.HasChanges();

            _eventAggregator.GetEvent<AfterEmployeeSavedEvent>().Publish(
                new AfterEmployeeSavedEventArgs
                { Id = Employee.Id, DisplayMember = $"{Employee.FirstName} {Employee.LastName}" });
        }

        private async void OnDeleteExecute()
        {
            var result = _messageDialogService.ShowOkCancelDialog($"Do you really want to delete the employee {Employee.FirstName} {Employee.LastName}?",
                "Question");
            if (result == MessageDialogResult.OK)
            {
                _employeeRepository.Remove(Employee.Model);
                await _employeeRepository.SaveAsync();
                _eventAggregator.GetEvent<AfterEmployeeDeletedEvent>().Publish(Employee.Id);
            }
        }

        private Employee CreateNewEmployee()
        {
            var employee = new Employee();
            _employeeRepository.Add(employee);
            return employee;
        }

        private void OnAddPhoneNumberExecute()
        {
            var newNumber = new PhoneNumberWrapper(new EmployeePhone());
            newNumber.PropertyChanged += EmployeePhoneNumberWrapper_PropertyChanged;
            PhoneNumbers.Add(newNumber);
            Employee.Model.PhoneNumbers.Add(newNumber.Model);
            newNumber.Number = "";
        }

        private void OnRemovePhoneNumberExecute()
        {
            SelectedPhoneNumber.PropertyChanged -= EmployeePhoneNumberWrapper_PropertyChanged;
            _employeeRepository.RemovePhoneNumber(SelectedPhoneNumber.Model);
            PhoneNumbers.Remove(SelectedPhoneNumber);
            SelectedPhoneNumber = null;
            HasChanges = _employeeRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private bool OnRemovePhoneNumberCanExecute()
        {
            return SelectedPhoneNumber != null;
        }
    }
}
