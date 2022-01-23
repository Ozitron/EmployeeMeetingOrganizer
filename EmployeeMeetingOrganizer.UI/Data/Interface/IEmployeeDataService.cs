using System.Threading.Tasks;
using EmployeeMeetingOrganizer.Model;

namespace EmployeeMeetingOrganizer.UI.Data.Interface
{
    public interface IEmployeeDataService
    {
        Task<Employee> GetByIdAsync(int employeeId);

        Task SaveAsync(Employee employee);
    }
}
