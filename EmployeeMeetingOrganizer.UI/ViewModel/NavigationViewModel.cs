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
            _eventAggregator.GetEvent<AfterEmployeeDeletedEvent>().Subscribe(AfterEmployeeDeleted);
        }

        private void AfterEmployeeSaved(AfterEmployeeSavedEventArgs obj)
        {
            var lookupItem = Employees.SingleOrDefault(i => i.Id == obj.Id);
            if (lookupItem == null)
            {
                Employees.Add(new NavigationItemViewModel(obj.Id, obj.DisplayMember,
                    _eventAggregator));
            }
            else
            {
                lookupItem.DisplayMember = obj.DisplayMember;
            }
        }

        private void AfterEmployeeDeleted(int employeeId)
        {
            var employee = Employees.SingleOrDefault(f => f.Id == employeeId);
            if (employee != null)
            {
                Employees.Remove(employee);
            }
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

    }
}
