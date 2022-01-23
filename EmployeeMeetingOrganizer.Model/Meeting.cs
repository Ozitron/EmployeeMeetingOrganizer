using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace EmployeeMeetingOrganizer.Model
{
    public class Meeting
    {
        public Meeting()
        {
            Employees = new Collection<Employee>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}
