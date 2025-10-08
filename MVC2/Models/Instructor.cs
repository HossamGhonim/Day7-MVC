using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC2.Models
{
    public class Instructor
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]

        public string Name { get; set; }
        public string Address { get; set; }
        public int Age { get; set; }
        [Column("Salary")]
        public double? salary { get; set; }
        public string Degree { get; set; }
        [Required(ErrorMessage = "Department is required")]
        public int? DepartmentId { get; set; }
        public Department Department { get; set; }
        public ICollection<CourseAssignment> CourseAssignments { get; set; } = new List<CourseAssignment>();


    }
}
