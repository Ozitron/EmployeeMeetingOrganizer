using System;
using System.Threading.Tasks;
using EmployeeMeetingOrganizer.DataAccess;
using EmployeeMeetingOrganizer.Model;
using EmployeeMeetingOrganizer.UI.Data.Interface;
using Microsoft.EntityFrameworkCore;

namespace EmployeeMeetingOrganizer.UI.Data
{
    internal class EmployeeDataService : IEmployeeDataService
    {
        private readonly Func<OrganizerContext> _contextCreator;

        public EmployeeDataService(Func<OrganizerContext> contextCreator)
        {
            _contextCreator = contextCreator;
        }

        public async Task<Employee> GetByIdAsync(int employeeId)
        {
            await using var ctx = _contextCreator();
            return await ctx.Employees.AsNoTracking().SingleAsync(e => e.Id == employeeId);
        }
    }
}
