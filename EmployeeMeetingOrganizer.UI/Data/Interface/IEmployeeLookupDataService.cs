using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeMeetingOrganizer.Model;

namespace EmployeeMeetingOrganizer.UI.Data.Interface;

public interface IEmployeeLookupDataService
{
    Task<IEnumerable<LookupItem>> GetEmployeeLookupAsync();
}