using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeMeetingOrganizer.Model;

namespace EmployeeMeetingOrganizer.UI.Data.Repositories
{
    public interface IMeetingRepository : IGenericRepository<Meeting>
    {
        Task<List<Employee>> GetAllEmployeesAsync();
    }
}
