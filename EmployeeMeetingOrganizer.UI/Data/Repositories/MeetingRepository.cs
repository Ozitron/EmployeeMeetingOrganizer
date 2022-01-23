using System.Threading.Tasks;
using EmployeeMeetingOrganizer.DataAccess;
using EmployeeMeetingOrganizer.Model;
using Microsoft.EntityFrameworkCore;

namespace EmployeeMeetingOrganizer.UI.Data.Repositories
{
    public class MeetingRepository : GenericRepository<Meeting, OrganizerContext>,
        IMeetingRepository
    {
        public MeetingRepository(OrganizerContext context) : base(context)
        {
        }

        public override async Task<Meeting> GetByIdAsync(int id)
        {
            return await Context.Meetings
                .Include(m => m.Employees)
                .SingleAsync(m => m.Id == id);
        }
    }
}
