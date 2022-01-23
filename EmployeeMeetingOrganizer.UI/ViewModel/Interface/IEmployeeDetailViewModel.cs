using System.Threading.Tasks;

namespace EmployeeMeetingOrganizer.UI.ViewModel.Interface;

public interface IEmployeeDetailViewModel
{
    Task LoadAsync(int employeeId);
}