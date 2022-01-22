using System.Collections.ObjectModel;
using System.Threading.Tasks;
using EmployeeMeetingOrganizer.Model;
using EmployeeMeetingOrganizer.UI.Data;

namespace EmployeeMeetingOrganizer.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IEmployeeDataService _employeeDataService;
        private Employee _selectedEmployee;
        public ObservableCollection<Employee> Employees { get; set; }

        public MainViewModel(IEmployeeDataService employeeDataService)
        {
            Employees = new ObservableCollection<Employee>();
            _employeeDataService = employeeDataService;
        }

        public async Task LoadAsync()
        {
            var employees = await _employeeDataService.GetAllAsync();
            Employees.Clear();
            foreach (var employee in employees)
            {
                Employees.Add(employee);
            }
        }

        public Employee SelectedEmployee
        {
            get { return _selectedEmployee; }
            set
            {
                _selectedEmployee = value;
                OnPropertyChanged();
            }
        }
    }
}
