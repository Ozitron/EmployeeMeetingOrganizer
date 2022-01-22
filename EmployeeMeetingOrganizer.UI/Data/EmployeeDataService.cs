using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeMeetingOrganizer.DataAccess;
using EmployeeMeetingOrganizer.Model;
using Microsoft.EntityFrameworkCore;

namespace EmployeeMeetingOrganizer.UI.Data
{
    internal class EmployeeDataService : IEmployeeDataService
    {
        private Func<OrganizerContext> _contextCreator;

        public EmployeeDataService(Func<OrganizerContext> contextCreator)
        {
            _contextCreator = contextCreator;
        }

        public async Task<List<Employee>> GetAllAsync()
        {
            using (var ctx = _contextCreator())
            {
                return await ctx.Employees.AsNoTracking().ToListAsync();
            }
        }
    }
}
