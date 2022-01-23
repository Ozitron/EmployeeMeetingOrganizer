using System.Threading.Tasks;
using EmployeeMeetingOrganizer.UI.ViewModel.Base;
using EmployeeMeetingOrganizer.UI.ViewModel.Interface;

namespace EmployeeMeetingOrganizer.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel(INavigationViewModel navigationViewModel,
            IEmployeeDetailViewModel employeeDetailViewModel)
        {
            NavigationViewModel = navigationViewModel;
            EmployeeDetailViewModel = employeeDetailViewModel;
        }

        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }

        public INavigationViewModel NavigationViewModel { get; }

        public IEmployeeDetailViewModel EmployeeDetailViewModel { get; }
    }
}
