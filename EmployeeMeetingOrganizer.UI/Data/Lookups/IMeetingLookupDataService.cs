using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeMeetingOrganizer.Model;

namespace EmployeeMeetingOrganizer.UI.Data.Lookups
{
    public interface IMeetingLookupDataService
    {
        Task<List<LookupItem>> GetMeetingLookupAsync();
    }
}
