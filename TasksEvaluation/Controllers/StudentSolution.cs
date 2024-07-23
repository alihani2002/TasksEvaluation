using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TasksEvaluation.Core.DTOs;
using TasksEvaluation.Core.Entities.Business;
using TasksEvaluation.Core.Interfaces.IRepositories;
using TasksEvaluation.Core.IRepositories;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TasksEvaluation.Controllers
{
    public class StudentSolution : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBaseRepository<Student> _baseRepository;

        public StudentSolution(IMapper mapper, IBaseRepository<Student> baseRepository, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _baseRepository = baseRepository;
            _unitOfWork = unitOfWork;
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
                    var student = await _baseRepository.GetByEmail(model.Email);
                    TempData["Student"] = Newtonsoft.Json.JsonConvert.SerializeObject(student);
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }

            // If we got this far, something failed; redisplay the form
            return View(model);
        }

        public IActionResult Index()
        {
            var studentJson = TempData["Student"] as string;
            var student = studentJson == null ? null : Newtonsoft.Json.JsonConvert.DeserializeObject<Student>(studentJson);

            ViewData["student"] = student;
            return View();
        }
    }
}
