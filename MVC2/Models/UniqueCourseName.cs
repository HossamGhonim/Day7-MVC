using MVC2.Data;
using System.ComponentModel.DataAnnotations;

namespace MVC2.Models
{
    public class UniqueCourseNameAttribute : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var db = (ApplicationDbContext)validationContext.GetService(typeof(ApplicationDbContext));
            if (db == null)
            {
                return new ValidationResult("Database context is not available");
            }

            var course = (Course)validationContext.ObjectInstance;
            string name = value as string;

            if (string.IsNullOrWhiteSpace(name))
            {
                return new ValidationResult("Course name is required");
            }

            bool exists = db.Courses.Any(c => c.Name == name && c.Id != course.Id);

            if (exists)
            {
                return new ValidationResult("Course name must be unique");
            }

            return ValidationResult.Success;
        }
    }
}
