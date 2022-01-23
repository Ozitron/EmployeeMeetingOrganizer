using System.Threading.Tasks;

namespace EmployeeMeetingOrganizer.UI.ViewModel.Interface;

public interface IEmployeeDetailViewModel
{
    bool HasChanges { get; }
    Task LoadAsync(int? employeeId);
}