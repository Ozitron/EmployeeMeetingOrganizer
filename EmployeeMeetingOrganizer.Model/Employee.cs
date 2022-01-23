using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace EmployeeMeetingOrganizer.Model
{
    public class Employee
    {
        public Employee()
        {
            PhoneNumbers = new Collection<EmployeePhone>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        public int? DepartmentId { get; set; }

        public Department EmployeeDepartment { get; set; }

        public ICollection<EmployeePhone> PhoneNumbers { get; set; }
    }
}