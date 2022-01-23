using EmployeeMeetingOrganizer.UI.ViewModel.Base;

namespace EmployeeMeetingOrganizer.UI.ViewModel
{
    internal class NavigationItemViewModel : ViewModelBase
    {
        private string _displayMember;

        public NavigationItemViewModel(int id, string displayMember)
        {
            Id = id;
            DisplayMember = displayMember;
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
    }
}
