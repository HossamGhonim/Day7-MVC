namespace MVC2.Models
{
    public class CourseAssignment
    {
        public int CourseId { get; set; }
        public Course Course { get; set; }

        public int InstructorId { get; set; }
        public Instructor Instructor { get; set; }

        public int Hours { get; set; }
    }
}
