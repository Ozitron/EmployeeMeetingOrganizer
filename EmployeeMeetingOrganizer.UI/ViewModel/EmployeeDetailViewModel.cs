using System.Threading.Tasks;
using System.Windows.Input;
using EmployeeMeetingOrganizer.Model;
using EmployeeMeetingOrganizer.UI.Data.Interface;
using EmployeeMeetingOrganizer.UI.Event;
using EmployeeMeetingOrganizer.UI.ViewModel.Base;
using EmployeeMeetingOrganizer.UI.ViewModel.Interface;
using Prism.Commands;
using Prism.Events;

namespace EmployeeMeetingOrganizer.UI.ViewModel
{
    internal class EmployeeDetailViewModel : ViewModelBase, IEmployeeDetailViewModel
    {
        private readonly IEmployeeDataService _dataService;
        private readonly IEventAggregator _eventAggregator;

        public EmployeeDetailViewModel(IEmployeeDataService dataService, IEventAggregator eventAggregator)
        {
            _dataService = dataService;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<OpenEmployeeDetailViewEvent>()
                .Subscribe(OnOpenEmployeeDetailView);

            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
        }

        private bool OnSaveCanExecute()
        {
            return true;
        }

        private async void OnSaveExecute()
        {
            await _dataService.SaveAsync(Employee);
            _eventAggregator.GetEvent<AfterEmployeeSavedEvent>().Publish(
                new AfterEmployeeSavedEventArgs
                    { Id = Employee.Id, DisplayMember = $"{Employee.FirstName} {Employee.LastName}" });
        }

        private async void OnOpenEmployeeDetailView(int employeeId)
        {
            await LoadAsync(employeeId);
        }

        public async Task LoadAsync(int employeeId)
        {
            Employee = await _dataService.GetByIdAsync(employeeId);
        }

        private Employee _employee;

        public Employee Employee
        {
            get => _employee;
            private set
            {
                _employee = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand { get; }
    }
}
