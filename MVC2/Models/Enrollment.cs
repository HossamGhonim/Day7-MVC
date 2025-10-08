namespace MVC2.Models
{
    public enum Grade { A, B, C, D, F, None }

    public class Enrollment
    {
        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public Grade Grade { get; set; } = Grade.None;
    }
}
