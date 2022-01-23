using System.Collections.ObjectModel;
using System.Threading.Tasks;
using EmployeeMeetingOrganizer.Model;
using EmployeeMeetingOrganizer.UI.Data.Interface;
using EmployeeMeetingOrganizer.UI.Event;
using EmployeeMeetingOrganizer.UI.ViewModel.Base;
using EmployeeMeetingOrganizer.UI.ViewModel.Interface;
using Prism.Events;

namespace EmployeeMeetingOrganizer.UI.ViewModel
{
    internal class NavigationViewModel : ViewModelBase, INavigationViewModel
    {
        private readonly IEmployeeLookupDataService _employeeLookupService;
        private readonly IEventAggregator _eventAggregator;

        public NavigationViewModel(IEmployeeLookupDataService employeeLookupService,
            IEventAggregator eventAggregator)
        {
            _employeeLookupService = employeeLookupService;
            _eventAggregator = eventAggregator;
            Employees = new ObservableCollection<LookupItem>();
        }

        public async Task LoadAsync()
        {
            var lookup = await _employeeLookupService.GetEmployeeLookupAsync();
            Employees.Clear();
            foreach (var item in lookup)
            {
                Employees.Add(item);
            }
        }

        public ObservableCollection<LookupItem> Employees { get; }

        private LookupItem _selectedEmployee;

        public LookupItem SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                _selectedEmployee = value;
                OnPropertyChanged();

                if (_selectedEmployee != null)
                {
                    _eventAggregator.GetEvent<OpenEmployeeDetailViewEvent>()
                        .Publish(_selectedEmployee.Id);
                }
            }
        }
    }
}
