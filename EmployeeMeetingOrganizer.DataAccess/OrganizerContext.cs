using EmployeeMeetingOrganizer.Model;
using Microsoft.EntityFrameworkCore;

namespace EmployeeMeetingOrganizer.DataAccess
{
    public class OrganizerContext : DbContext
    {
        //const string connectionString = "Data Source=.;Initial Database=EmployeeMeetingOrganizerDb;Integrated Security=True";

        const string connectionString = "Data Source=.\\SQLExpress;Initial Catalog=EmployeeMeetingOrganizerDb;Integrated Security=True";
        
        public OrganizerContext() : base() { }

        public OrganizerContext(DbContextOptions<OrganizerContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

}