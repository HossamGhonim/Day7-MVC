using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC2.Models;
using MVC2.Data;
using MVC2.Filters;

namespace MVC2.Controllers
{
    public class StudentController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public IActionResult Index()
        {
            var students = db.Students.Include(s => s.Department).ToList();
            return View(students);
        }
        [CheckUserFilter]
        public IActionResult Details(int id)
        {
            // احصل على UserType من HttpContext.Items اللي تم حفظه في الفلتر
            string? userType = HttpContext.Items["UserType"] as string;

            var student = db.Students
                .Include(s => s.Department)
                .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Course)
                .FirstOrDefault(s => s.Id == id);

            if (student == null) return NotFound();

            ViewBag.UserType = userType; // يتم تمرير القيمة للـ View
            return View(student);
        }

        [HttpGet]
        public ActionResult Add()
        {
            ViewBag.Departments = db.Departments.ToList();
            return View();
        }
        [HttpPost]
        [HttpPost]
        public IActionResult Add(Student student)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Departments = db.Departments.ToList();
                return View(student);
            }

            try
            {
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.InnerException?.Message ?? ex.Message);
                return View(student);
            }
        }
        public IActionResult Edit(int id)
        {
            var student = db.Students.FirstOrDefault(s => s.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            ViewBag.Departments = db.Departments.ToList();
            return View(student);
        }

        // POST: Student/Edit
        [HttpPost]
        public IActionResult Edit(Student student)
        {
            if (ModelState.IsValid)
            {
                db.Students.Update(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Departments = db.Departments.ToList();
            return View(student);
        }
        public IActionResult Delete(int id)
        {
            var student = db.Students.FirstOrDefault(s => s.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student); 
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var student = db.Students.FirstOrDefault(s => s.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            db.Students.Remove(student);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult TestException()
        {
            throw new Exception("Test exception — something went wrong!");
        }
    }
}
