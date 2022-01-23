using EmployeeMeetingOrganizer.Model;
using Microsoft.EntityFrameworkCore;

namespace EmployeeMeetingOrganizer.DataAccess
{
    public class OrganizerContext : DbContext
    {
        //const string connectionString = "Data Source=.;Initial Database=EmployeeMeetingOrganizerDb;Integrated Security=True";

        const string connectionString = "Data Source=.\\SQLExpress;Initial Catalog=EmployeeMeetingOrganizerDb;Integrated Security=True";

        public OrganizerContext() { }

        public OrganizerContext(DbContextOptions options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            new DbInitializer(modelBuilder).Seed();
        }

        public Microsoft.EntityFrameworkCore.DbSet<Employee> Employees { get; set; }

        public Microsoft.EntityFrameworkCore.DbSet<Department> Departments { get; set; }

        public Microsoft.EntityFrameworkCore.DbSet<EmployeePhone> PhoneNumbers { get; set; }

        public Microsoft.EntityFrameworkCore.DbSet<Meeting> Meetings { get; set; }


        public class DbInitializer
        {
            private readonly ModelBuilder modelBuilder;

            public DbInitializer(ModelBuilder modelBuilder)
            {
                this.modelBuilder = modelBuilder;
            }

            public void Seed()
            {
                modelBuilder.Entity<Employee>().HasData(
                    new Employee { Id = 1, FirstName = "Ozan", LastName = "Komurcu", Email = "ozankomurcu@gmail.com", DepartmentId = 3},
                    new Employee { Id = 2, FirstName = "John", LastName = "Doe", Email = "john@doe.com" , DepartmentId = 1},
                    new Employee { Id = 3, FirstName = "Jolene", LastName = "Doe", Email = "jolene@doe.com", DepartmentId = 2}
                );

                modelBuilder.Entity<Department>().HasData(
                    new Department { Id = 1, Name = "HR" },
                    new Department { Id = 2, Name = "Finance" },
                    new Department { Id = 3, Name = "IT" }
                );

                //modelBuilder.Entity<Meeting>().HasData(
                //new Meeting
                //{
                //    Title = "Sprint Planning",
                //    DateFrom = new DateTime(2022, 5, 26),
                //    DateTo = new DateTime(2022, 5, 26)
                //});

            }
        }
    }
}