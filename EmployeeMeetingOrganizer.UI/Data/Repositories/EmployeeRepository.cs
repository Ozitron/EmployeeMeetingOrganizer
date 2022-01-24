using System.Threading.Tasks;
using EmployeeMeetingOrganizer.DataAccess;
using EmployeeMeetingOrganizer.Model;
using Microsoft.EntityFrameworkCore;

namespace EmployeeMeetingOrganizer.UI.Data.Repositories
{
    internal class EmployeeRepository : GenericRepository<Employee, OrganizerDbContext>, IEmployeeRepository
    {
        public EmployeeRepository(OrganizerDbContext context)
            : base(context)
        {
        }

        public override async Task<Employee> GetByIdAsync(int employeeId)
        {
            return await Context.Employees
                .Include(e => e.PhoneNumbers)
                .SingleAsync(e => e.Id == employeeId);
        }

        public void RemovePhoneNumber(EmployeePhone model)
        {
            Context.PhoneNumbers.Remove(model);
        }
    }
}
