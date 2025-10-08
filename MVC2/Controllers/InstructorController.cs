using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC2.Data;
using MVC2.Models;

namespace MVC2.Controllers
{
    public class InstructorController : Controller
    {
        ApplicationDbContext _context = new ApplicationDbContext();
        public IActionResult Index()
        {
            var instructors = _context.Instructors.Include(i => i.Department);
            return View(instructors.ToList());
        }

        // GET: Instructor/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = _context.Instructors
                .Include(i => i.Department)
                .FirstOrDefault(m => m.Id == id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // GET: Instructor/Create
        public IActionResult Add()
        {
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name");
            return View();
        }

        // POST: Instructor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add([Bind("Id,Name,Address,Age,salary,Degree,DepartmentId")] Instructor instructor)
        {
            ModelState.Remove("Department");
            ModelState.Remove("CourseAssignments");

            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                TempData["Error"] = $"Validation failed: {errors}";

                var invalidKeys = ModelState.Where(x => x.Value.Errors.Count > 0 || x.Value.ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
                    .Select(x => x.Key).ToList();
                TempData["InvalidKeys"] = string.Join(", ", invalidKeys);

                var allStates = string.Join(", ", ModelState.Select(s => $"{s.Key}: {s.Value.ValidationState}"));
                TempData["AllStates"] = allStates;

                TempData["Debug"] = $"Name: '{instructor.Name}', DepartmentId: {instructor.DepartmentId ?? 0}, Salary: {instructor.salary ?? 0}";
            }

            if (ModelState.IsValid)
            {
                // أضف ده عشان تشوف القيمة قبل الحفظ
                TempData["SaveDebug"] = $"Saving Instructor: Name='{instructor.Name}', Salary={instructor.salary ?? 0}";

                _context.Add(instructor);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", instructor.DepartmentId);
            return View(instructor);
        }

        // GET: Instructor/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = _context.Instructors.Find(id);
            if (instructor == null)
            {
                return NotFound();
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", instructor.DepartmentId);
            return View(instructor);
        }

        // POST: Instructor/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name,Address,Age,salary,Degree,DepartmentId")] Instructor instructor)
        {
            if (id != instructor.Id)
            {
                return NotFound();
            }

            // نفس الـ remove هنا
            ModelState.Remove("Department");
            ModelState.Remove("CourseAssignments");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(instructor);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstructorExists(instructor.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", instructor.DepartmentId);
            return View(instructor);
        }

        // GET: Instructor/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = _context.Instructors
                .Include(i => i.Department)
                .FirstOrDefault(m => m.Id == id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // POST: Instructor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var instructor = _context.Instructors.Find(id);
            if (instructor != null)
            {
                _context.Instructors.Remove(instructor);
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool InstructorExists(int id)
        {
            return _context.Instructors.Any(e => e.Id == id);
        }
    }
}
