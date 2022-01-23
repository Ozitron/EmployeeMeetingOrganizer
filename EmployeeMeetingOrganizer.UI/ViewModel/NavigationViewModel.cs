using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using EmployeeMeetingOrganizer.UI.Data;
using EmployeeMeetingOrganizer.UI.Data.Lookups;
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
        private readonly IMeetingLookupDataService _meetingLookupService;

        public NavigationViewModel(IEmployeeLookupDataService employeeLookupService,
            IMeetingLookupDataService meetingLookupService,
            IEventAggregator eventAggregator)
        {
            _employeeLookupService = employeeLookupService;
            _meetingLookupService = meetingLookupService;
            _eventAggregator = eventAggregator;
            Employees = new ObservableCollection<NavigationItemViewModel>();
            Meetings = new ObservableCollection<NavigationItemViewModel>();
            _eventAggregator.GetEvent<AfterDetailSavedEvent>().Subscribe(AfterDetailSaved);
            _eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);
        }

        private void AfterDetailSaved(AfterDetailSavedEventArgs args)
        {
            switch (args.ViewModelName)
            {
                case nameof(EmployeeDetailViewModel):
                    AfterDetailSaved(Employees, args);
                    break;
                case nameof(MeetingDetailViewModel):
                    AfterDetailSaved(Meetings, args);
                    break;
            }
        }

        private void AfterDetailSaved(ObservableCollection<NavigationItemViewModel> items, AfterDetailSavedEventArgs args)
        {
            var lookupItem = items.SingleOrDefault(l => l.Id == args.Id);
            if (lookupItem == null)
            {
                items.Add(new NavigationItemViewModel(args.Id, args.DisplayMember,
                    args.ViewModelName,
                    _eventAggregator));
            }
            else
            {
                lookupItem.DisplayMember = args.DisplayMember;
            }
        }

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            switch (args.ViewModelName)
            {
                case nameof(EmployeeDetailViewModel):
                    AfterDetailDeleted(Employees, args);
                    break;
                case nameof(MeetingDetailViewModel):
                    AfterDetailDeleted(Meetings, args);
                    break;
            }
        }

        private void AfterDetailDeleted(ObservableCollection<NavigationItemViewModel> items,
            AfterDetailDeletedEventArgs args)
        {
            var item = items.SingleOrDefault(f => f.Id == args.Id);
            if (item != null)
            {
                items.Remove(item);
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

            lookup = await _meetingLookupService.GetMeetingLookupAsync();
            Meetings.Clear();
            foreach (var item in lookup)
            {
                Meetings.Add(new NavigationItemViewModel(item.Id, item.DisplayMember,
                    nameof(MeetingDetailViewModel),
                    _eventAggregator));
            }
        }

        public ObservableCollection<NavigationItemViewModel> Employees { get; }
        public ObservableCollection<NavigationItemViewModel> Meetings { get; }

    }
}
