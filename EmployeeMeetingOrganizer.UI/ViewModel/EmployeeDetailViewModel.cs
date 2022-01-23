using System.Threading.Tasks;
using EmployeeMeetingOrganizer.Model;
using EmployeeMeetingOrganizer.UI.Data;
using EmployeeMeetingOrganizer.UI.Data.Interface;
using EmployeeMeetingOrganizer.UI.Event;
using EmployeeMeetingOrganizer.UI.ViewModel.Base;
using EmployeeMeetingOrganizer.UI.ViewModel.Interface;
using Prism.Events;

namespace EmployeeMeetingOrganizer.UI.ViewModel
{
    internal class EmployeeDetailViewModel : ViewModelBase, IEmployeeDetailViewModel
    {
        private readonly IEmployeeDataService _dataService;

        public EmployeeDetailViewModel(IEmployeeDataService dataService, IEventAggregator eventAggregator)
        {
            _dataService = dataService;
            eventAggregator.GetEvent<OpenEmployeeDetailViewEvent>()
                .Subscribe(OnOpenEmployeeDetailView);
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
    }
}
