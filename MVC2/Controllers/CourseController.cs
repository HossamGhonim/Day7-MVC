using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC2.Data;
using MVC2.Models;

namespace MVC2.Controllers
{
    public class CourseController : Controller
    {
        ApplicationDbContext db=new ApplicationDbContext();

        public IActionResult Index()
        {
            var courses = db.Courses.ToList();
            ViewBag.LastCourse = HttpContext.Session.GetString("LastCourse");
            ViewBag.TotalAdded = HttpContext.Session.GetInt32("TotalAdded") ?? 0;

            ViewBag.Message = TempData["Message"];
            

            // Cookies
            ViewBag.LastCourseCookie = Request.Cookies["LastCourseCookie"];
            return View(courses);
        }
        public IActionResult Error(string message)
        {
            ViewBag.ErrorMessage = message ?? "Error!";
            return View();
        }
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Course course)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(kv => kv.Value.Errors.Count > 0)
                    .SelectMany(kv => kv.Value.Errors.Select(e => $"{kv.Key}: {e.ErrorMessage}"))
                    .ToList();

                ViewBag.ModelErrors = errors;
                return View(course);
            }

            if (course.MinDegree >= course.Degree)
            {
                ModelState.AddModelError(nameof(course.MinDegree), "MinDegree must be less than Degree");
                ViewBag.ModelErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View(course);
            }

            bool exists = db.Courses.Any(c => c.Name.Trim().ToLower() == (course.Name ?? "").Trim().ToLower());
            if (exists)
            {
                ModelState.AddModelError(nameof(course.Name), "Course name must be unique");
                ViewBag.ModelErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View(course);
            }

            db.Courses.Add(course);

            try
            {
                var rows = db.SaveChanges();
                if (rows <= 0)
                {
                    ModelState.AddModelError("", "SaveChanges returned 0 rows — check DB/migration.");
                    ViewBag.ModelErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    return View(course);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Save failed: " + ex.Message);
                ViewBag.ModelErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View(course);
            }

            // ✅ Session
            HttpContext.Session.SetString("LastCourse", course.Name);
            int count = (HttpContext.Session.GetInt32("TotalAdded") ?? 0) + 1;
            HttpContext.Session.SetInt32("TotalAdded", count);

            // ✅ TempData
            TempData["Message"] = $"Course '{course.Name}' added successfully!";

            // ✅ Cookies
            Response.Cookies.Append("LastCourseCookie", course.Name, new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddMinutes(60), // يعيش ساعة
                HttpOnly = true
            });

            return RedirectToAction("Index");
        }



        public IActionResult Edit(int id)
        {
            var course = db.Courses.FirstOrDefault(c => c.Id == id);
            if (course == null) return NotFound();
            return View(course);
        }

        [HttpPost]
        public IActionResult Edit(Course course)
        {
            if (!ModelState.IsValid)
                return View(course);

            if (course.MinDegree >= course.Degree)
            {
                ModelState.AddModelError("", "MinDegree must be less than Degree");
                return View(course);
            }

            if (db.Courses.Any(c => c.Name == course.Name && c.Id != course.Id))
            {
                ModelState.AddModelError("Name", "Course name must be unique");
                return View(course);
            }

            var dbCourse = db.Courses.FirstOrDefault(c => c.Id == course.Id);
            if (dbCourse == null) return NotFound();

            dbCourse.Name = course.Name;
            dbCourse.Degree = course.Degree;
            dbCourse.MinDegree = course.MinDegree;
            dbCourse.Topic = course.Topic;

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var course = db.Courses
                           .Include(c => c.Enrollments) // عشان نعرف لو في تسجيلات مرتبطة
                           .FirstOrDefault(c => c.Id == id);

            if (course == null) return NotFound();

            return View(course);
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var course = db.Courses
                           .Include(c => c.Enrollments)
                           .FirstOrDefault(c => c.Id == id);

            if (course == null) return NotFound();

            // خيار 1: منع الحذف لو فيه Enrollments مرتبطة
            if (course.Enrollments != null && course.Enrollments.Any())
            {
                ModelState.AddModelError("", "لا يمكنك حذف هذا الكورس لأنه مرتبط بتسجيلات (Enrollments). احذف التسجيلات أولاً أو قم بإلغاء الربط.");
                return View("Delete", course);
            }

            try
            {
                db.Courses.Remove(course);
                db.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", "حدث خطأ أثناء الحذف: " + ex.Message);
                return View("Delete", course);
            }

            return RedirectToAction("Index");
        }
    }

}
