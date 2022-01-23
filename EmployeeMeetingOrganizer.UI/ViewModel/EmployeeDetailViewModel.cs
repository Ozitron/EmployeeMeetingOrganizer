using System.Threading.Tasks;
using System.Windows.Input;
using EmployeeMeetingOrganizer.UI.Data;
using EmployeeMeetingOrganizer.UI.Data.Repositories;
using EmployeeMeetingOrganizer.UI.Event;
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
        private EmployeeWrapper _employee;
        private bool _hasChanges;

        public EmployeeDetailViewModel(IEmployeeRepository dataService, IEventAggregator eventAggregator)
        {
            _employeeRepository = dataService;
            _eventAggregator = eventAggregator;

            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
        }

        public async Task LoadAsync(int employeeId)
        {
            var employee = await _employeeRepository.GetByIdAsync(employeeId);

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


    }
}
