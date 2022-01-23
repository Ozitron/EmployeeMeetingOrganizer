using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeMeetingOrganizer.DataAccess;
using EmployeeMeetingOrganizer.Model;
using EmployeeMeetingOrganizer.UI.Data.Lookups;
using Microsoft.EntityFrameworkCore;

namespace EmployeeMeetingOrganizer.UI.Data
{
    public class LookupDataService : IEmployeeLookupDataService, IDepartmentsLookupDataService
    {
        private readonly Func<OrganizerContext> _contextCreator;

        public LookupDataService(Func<OrganizerContext> contextCreator)
        {
            _contextCreator = contextCreator;
        }

        public async Task<IEnumerable<LookupItem>> GetEmployeeLookupAsync()
        {
            await using var ctx = _contextCreator();
            return await ctx.Employees.AsNoTracking()
                .Select(e =>
                    new LookupItem
                    {
                        Id = e.Id,
                        DisplayMember = e.FirstName + " " + e.LastName
                    })
                .ToListAsync();
        }

        public async Task<IEnumerable<LookupItem>> GetDepartmentsLookupAsync()
        {
            await using var ctx = _contextCreator();
            return await ctx.Departments.AsNoTracking()
                .Select(d =>
                    new LookupItem
                    {
                        Id = d.Id,
                        DisplayMember = d.Name
                    })
                .ToListAsync();
        }
    }
}
