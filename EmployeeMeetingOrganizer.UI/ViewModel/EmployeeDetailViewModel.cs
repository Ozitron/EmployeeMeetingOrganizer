using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using EmployeeMeetingOrganizer.Model;
using EmployeeMeetingOrganizer.UI.Data;
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

            Departments = new ObservableCollection<LookupItem>();
        }

        public ObservableCollection<LookupItem> Departments { get; }

        public async Task LoadAsync(int? employeeId)
        {
            var employee = employeeId.HasValue
                ? await _employeeRepository.GetByIdAsync(employeeId.Value)
                : CreateNewEmployee();

            InitializeEmployee(employee);

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

        private bool OnSaveCanExecute()
        {
            return Employee != null && !Employee.HasErrors && HasChanges;
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
    }
}
