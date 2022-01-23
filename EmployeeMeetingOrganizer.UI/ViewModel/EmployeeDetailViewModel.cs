using System.Threading.Tasks;
using System.Windows.Input;
using EmployeeMeetingOrganizer.UI.Data.Interface;
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
        private readonly IEmployeeDataService _dataService;
        private readonly IEventAggregator _eventAggregator;
        private EmployeeWrapper _employee;

        public EmployeeDetailViewModel(IEmployeeDataService dataService, IEventAggregator eventAggregator)
        {
            _dataService = dataService;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<OpenEmployeeDetailViewEvent>()
                .Subscribe(OnOpenEmployeeDetailView);

            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
        }

        public async Task LoadAsync(int employeeId)
        {
            var employee = await _dataService.GetByIdAsync(employeeId);

            Employee = new EmployeeWrapper(employee);

            Employee.PropertyChanged += (s, e) =>
            {
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

        public ICommand SaveCommand { get; }

        private bool OnSaveCanExecute()
        {
            return Employee != null && !Employee.HasErrors;
        }

        private async void OnSaveExecute()
        {
            await _dataService.SaveAsync(Employee.Model);
            _eventAggregator.GetEvent<AfterEmployeeSavedEvent>().Publish(
                new AfterEmployeeSavedEventArgs
                { Id = Employee.Id, DisplayMember = $"{Employee.FirstName} {Employee.LastName}" });
        }

        private async void OnOpenEmployeeDetailView(int employeeId)
        {
            await LoadAsync(employeeId);
        }

    }
}
