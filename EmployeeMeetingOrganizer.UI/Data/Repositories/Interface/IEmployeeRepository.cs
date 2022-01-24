using EmployeeMeetingOrganizer.Model;

namespace EmployeeMeetingOrganizer.UI.Data.Repositories
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        void RemovePhoneNumber(EmployeePhone model);
    }
}
