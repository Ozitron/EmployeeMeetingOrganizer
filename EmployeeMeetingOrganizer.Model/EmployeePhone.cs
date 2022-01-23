using System.ComponentModel.DataAnnotations;

namespace EmployeeMeetingOrganizer.Model
{
    public class EmployeePhone
    {
        public int Id { get; set; }

        [Phone]
        [Required]
        public string Number { get; set; }

        public int EmployeeId { get; set; }

        public Employee Employee { get; set; }
    }
}
