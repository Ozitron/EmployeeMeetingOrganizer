using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeMeetingOrganizer.Model;

namespace EmployeeMeetingOrganizer.UI.Data.Lookups
{
    internal interface IDepartmentsLookupDataService
    {
        Task<IEnumerable<LookupItem>> GetDepartmentsLookupAsync();
    }
}
