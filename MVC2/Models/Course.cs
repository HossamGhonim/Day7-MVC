using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC2.Models
{
    
        public class Course
        {

        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        [Range(1, 100, ErrorMessage = "Degree must be between 1 and 100")]
        public int Degree { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "MinDegree must be between 0 and 100")]
        public int MinDegree { get; set; }

        public int Num { get; set; }
            public string Topic { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<CourseAssignment> CourseAssignments { get; set; } = new List<CourseAssignment>();
    }
}
