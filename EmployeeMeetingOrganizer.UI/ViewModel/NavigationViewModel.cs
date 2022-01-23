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
            _eventAggregator.GetEvent<AfterDetailSavedEvent>().Subscribe(AfterDetailSaved);
            _eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);
        }

        private void AfterDetailSaved(AfterDetailSavedEventArgs obj)
        {
            switch (obj.ViewModelName)
            {
                case nameof(EmployeeDetailViewModel):
                    var lookupItem = Employees.SingleOrDefault(l => l.Id == obj.Id);
                    if (lookupItem == null)
                    {
                        Employees.Add(new NavigationItemViewModel(obj.Id, obj.DisplayMember,
                            nameof(EmployeeDetailViewModel),
                            _eventAggregator));
                    }
                    else
                    {
                        lookupItem.DisplayMember = obj.DisplayMember;
                    }
                    break;
            }
        }

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            switch (args.ViewModelName)
            {
                case nameof(EmployeeDetailViewModel):
                    var employee = Employees.SingleOrDefault(e => e.Id == args.Id);
                    if (employee != null)
                    {
                        Employees.Remove(employee);
                    }
                    break;
            }
        }

        public async Task LoadAsync()
        {
            var lookup = await _employeeLookupService.GetEmployeeLookupAsync();
            Employees.Clear();
            foreach (var item in lookup)
            {
                Employees.Add(new NavigationItemViewModel(item.Id, item.DisplayMember, nameof(EmployeeDetailViewModel), _eventAggregator));
            }
        }

        public ObservableCollection<NavigationItemViewModel> Employees { get; }

    }
}
