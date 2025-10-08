using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC2.Data;
using MVC2.Models;

namespace MVC2.Controllers
{
    public class DepartmentController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        
        public IActionResult GetAll()
        {
            var departments = db.Departments
                .Include(d => d.Students)   
                .Include(d => d.Instructors) 
                .Select(d => new DepartmentVm
                {
                    Id = d.Id,
                    Name = d.Name,
                    Manager = d.Manager,
                    StdCount = d.Students.Count(),
                    InsCount = d.Instructors.Count(),
                    StdNames = d.Students.Select(s => s.Name).ToList(),
                    InsNames = d.Instructors.Select(i => i.Name).ToList()
                })
                .ToList();

            return View(departments); 
        }
        public IActionResult Index()
        {
            var departments = db.Departments.ToList();
            return View(departments);
        }

        // GET: Departments/Create
        public IActionResult Add()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Add2()
        {
            return View();
        }
        [AddFooterFilter]
        [HttpPost]
        public IActionResult Add2(Department department)
        {
            if (!ModelState.IsValid)
                return View(department);

            // التحقق من الـ Location
            if (department.Location != "EG" && department.Location != "USA")
            {
                ModelState.AddModelError("Location", "Location must be either 'EG' or 'USA'.");
                return View(department);
            }

            db.Departments.Add(department);
            db.SaveChanges();

            // اعرض نفس الصفحة مع الرسالة بدل ما تعمل Redirect
            return View(department);
        }


        // POST: Departments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Department department)
        {
            if (ModelState.IsValid)
            {
                db.Departments.Add(department);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        // GET: Departments/Edit/5
        public IActionResult Edit(int id)
        {
            var department = db.Departments.Find(id);
            if (department == null) return NotFound();

            return View(department);
        }

        // POST: Departments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Department department)
        {
            if (id != department.Id) return NotFound();

            if (ModelState.IsValid)
            {
                db.Departments.Update(department);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }
        // GET: Departments/Delete/5


        // تنفيذ عملية الحذف فعلاً
        [HttpPost]
        [ValidateAntiForgeryToken] // حماية من هجمات CSRF
        public IActionResult Delete(int id)
        {
            var department = db.Departments.Find(id);
            if (department == null)
                return NotFound();

            db.Departments.Remove(department);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        public IActionResult TestException()
        {
            throw new Exception("Test exception — something went wrong!");
        }
    }
}
