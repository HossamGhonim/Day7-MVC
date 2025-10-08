using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using MVC2.Models;

namespace MVC2.Data
{
    public class ApplicationDbContext : DbContext
    {
       
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<CourseAssignment> CourseAssignments { get; set; }
        public DbSet<Department> Departments { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            {
                optionsBuilder.UseSqlServer("Server=.;Database=UNI4;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }
        protected override void OnModelCreating(ModelBuilder mb)
        {
            base.OnModelCreating(mb);

            // Enrollment (Student-Course)
            mb.Entity<Enrollment>()
              .HasKey(e => new { e.StudentId, e.CourseId });

            mb.Entity<Enrollment>()
              .HasOne(e => e.Student)
              .WithMany(s => s.Enrollments)
              .HasForeignKey(e => e.StudentId);

            mb.Entity<Enrollment>()
              .HasOne(e => e.Course)
              .WithMany(c => c.Enrollments)
              .HasForeignKey(e => e.CourseId);

            // CourseAssignment (Course-Instructor)
            mb.Entity<CourseAssignment>()
              .HasKey(ca => new { ca.CourseId, ca.InstructorId });

            mb.Entity<CourseAssignment>()
              .HasOne(ca => ca.Course)
              .WithMany(c => c.CourseAssignments)
              .HasForeignKey(ca => ca.CourseId);

            mb.Entity<CourseAssignment>()
              .HasOne(ca => ca.Instructor)
              .WithMany(i => i.CourseAssignments)
              .HasForeignKey(ca => ca.InstructorId);
        }
    }
}
