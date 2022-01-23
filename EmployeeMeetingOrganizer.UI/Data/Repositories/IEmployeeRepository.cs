using System.Threading.Tasks;
using EmployeeMeetingOrganizer.Model;

namespace EmployeeMeetingOrganizer.UI.Data.Repositories
{
    public interface IEmployeeRepository
    {
        Task<Employee> GetByIdAsync(int employeeId);

        Task SaveAsync();

        bool HasChanges();

        void Add(Employee employee);

        void Remove(Employee model);
    }
}
