using System.Windows.Input;
using EmployeeMeetingOrganizer.UI.Event;
using EmployeeMeetingOrganizer.UI.ViewModel.Base;
using Prism.Commands;
using Prism.Events;

namespace EmployeeMeetingOrganizer.UI.ViewModel
{
    internal class NavigationItemViewModel : ViewModelBase
    {
        private string _displayMember;
        private IEventAggregator _eventAggregator;

        public NavigationItemViewModel(int id, string displayMember, IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            Id = id;
            DisplayMember = displayMember;
            OpenEmployeeDetailViewCommand = new DelegateCommand(OnOpenEmployeeDetailView);
        }

        public int Id { get; }

        public string DisplayMember
        {
            get { return _displayMember; }
            set
            {
                _displayMember = value;
                OnPropertyChanged();
            }
        }

        public ICommand OpenEmployeeDetailViewCommand
        { get; }

        private void OnOpenEmployeeDetailView()
        {
            _eventAggregator.GetEvent<OpenEmployeeDetailViewEvent>()
                .Publish(Id);
        }
    }
}
