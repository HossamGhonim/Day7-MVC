using System.ComponentModel.DataAnnotations;

namespace MVC2.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Manager { get; set; }
        [Required]
        [RegularExpression("^(EG|USA)$", ErrorMessage = "Location must be either EG or USA")]
        public string Location { get; set; }
        public ICollection<Instructor> Instructors { get; set; } = new List<Instructor>();
        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
