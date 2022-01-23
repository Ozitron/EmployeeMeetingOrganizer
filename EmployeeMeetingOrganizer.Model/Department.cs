using System.ComponentModel.DataAnnotations;

namespace EmployeeMeetingOrganizer.Model
{
    public class Department
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }
    }
}
