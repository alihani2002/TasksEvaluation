using Microsoft.AspNetCore.Mvc;
using TasksEvaluation.Core.DTOs;
using TasksEvaluation.Core.Interfaces.IServices;

namespace TasksEvaluation.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        // GET: Student
        public async Task<IActionResult> Index()
        {
            var students = await _studentService.GetStudents();
            return View(students);
        }

        // GET: Student/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var student = await _studentService.GetStudent(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // GET: Student/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Student/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FullName,MobileNumber,Email,ProfilePic,GroupId")] StudentDTO studentDTO)
        {
            if (ModelState.IsValid)
            {
                await _studentService.Create(studentDTO);
                return RedirectToAction(nameof(Index));
            }
            return View(studentDTO);
        }

        // GET: Student/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var student = await _studentService.GetStudent(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Student/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,MobileNumber,Email,ProfilePic,GroupId")] StudentDTO studentDTO)
        {
            if (id != studentDTO.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _studentService.Update(studentDTO);
                }
                catch (ArgumentNullException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(studentDTO);
        }

        // GET: Student/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _studentService.GetStudent(id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _studentService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
