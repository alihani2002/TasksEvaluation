using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TasksEvaluation.Core.DTOs;
using TasksEvaluation.Core.Entities.Business;
using TasksEvaluation.Core.Interfaces.IRepositories;
using TasksEvaluation.Core.IRepositories;
using System.Threading.Tasks;
using System.Linq;
using TasksEvaluation.Core.Interfaces.IServices;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace TasksEvaluation.Controllers
{
    public class StudentSolutionController : Controller
    {
        private readonly ISolutionService _solutionService;
        private readonly IMapper _mapper;
        private readonly IAssignmentService _assignmentService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBaseRepository<Student> _studentBaseRepository;

        public StudentSolutionController(IMapper mapper, IBaseRepository<Student> studentBaseRepository, IUnitOfWork unitOfWork, IAssignmentService assignmentService, ISolutionService solutionService)
        {
            _mapper = mapper;
            _studentBaseRepository = studentBaseRepository;
            _unitOfWork = unitOfWork;
            _assignmentService = assignmentService;
            _solutionService = solutionService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(StudentLogInDTO model)
        {
            if (ModelState.IsValid)
            {
                var students = await _unitOfWork.Students.GetAll();

                if (students.Any(s => s.Email == model.Email))
                {
                    var student = await _studentBaseRepository.GetByEmail(model.Email);
                    HttpContext.Session.SetString("Student", JsonConvert.SerializeObject(student));
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }

            return View(model);
        }

        public async Task<IActionResult> Index()
        {
            var studentJson = HttpContext.Session.GetString("Student");
            var student = studentJson == null ? null : JsonConvert.DeserializeObject<Student>(studentJson);
            ViewBag.Student = student;

            if (student != null)
            {
                var assignments = await _assignmentService.GetAssignmentsWhere(assignment => assignment.GroupId == student.GroupId.Value);
                return View(assignments);
            }

            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult UploadSolution(int assignmentId)
        {
            var studentJson = HttpContext.Session.GetString("Student");
            var student = studentJson == null ? null : JsonConvert.DeserializeObject<Student>(studentJson);
            ViewBag.Student = student;

            if (student != null)
            {
                var model = new SolutionDTO
                {
                    AssignmentId = assignmentId,
                    StudentId = student.Id
                };
                return View(model);
            }

            return RedirectToAction(nameof(Login));
        }

        [HttpPost]
        public async Task<IActionResult> UploadSolution(UploadSolutionDTO model)
        {
            if (ModelState.IsValid)
            {
                var result = await _solutionService.UploadSolution(model);

                if (!string.IsNullOrEmpty(result.Notes))
                {
                    ModelState.AddModelError(string.Empty, result.Notes);
                    return View(model);
                }

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
    }
}
