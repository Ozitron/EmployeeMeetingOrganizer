using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using EmployeeMeetingOrganizer.UI.Data;
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
            Employees = new ObservableCollection<NavigationItemViewModel>();
            _eventAggregator.GetEvent<AfterEmployeeSavedEvent>().Subscribe(AfterEmployeeSaved);
        }

        private void AfterEmployeeSaved(AfterEmployeeSavedEventArgs obj)
        {
            var lookupItem = Employees.Single(i => i.Id == obj.Id);
            lookupItem.DisplayMember = obj.DisplayMember;
        }

        public async Task LoadAsync()
        {
            var lookup = await _employeeLookupService.GetEmployeeLookupAsync();
            Employees.Clear();
            foreach (var item in lookup)
            {
                Employees.Add(new NavigationItemViewModel(item.Id, item.DisplayMember, _eventAggregator));
            }
        }

        public ObservableCollection<NavigationItemViewModel> Employees { get; }

        private NavigationItemViewModel _selectedEmployee;

        public NavigationItemViewModel SelectedEmployee
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
