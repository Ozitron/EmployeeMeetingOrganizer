using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using EmployeeMeetingOrganizer.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace EmployeeMeetingOrganizer.DataAccess
{
    public class OrganizerContext : DbContext
    {
        //const string connectionString = "Data Source=.;Initial Database=EmployeeMeetingOrganizerDb;Integrated Security=True";

        const string connectionString = "Data Source=.\\SQLExpress;Initial Catalog=EmployeeMeetingOrganizerDb;Integrated Security=True";
        
        public OrganizerContext() : base() { }

        public OrganizerContext(DbContextOptions<OrganizerContext> options) : base(options) { }

        public Microsoft.EntityFrameworkCore.DbSet<Employee> Employees { get; set; }

        public Microsoft.EntityFrameworkCore.DbSet<Department> Departments { get; set; }

        public Microsoft.EntityFrameworkCore.DbSet<EmployeePhone> PhoneNumbers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}