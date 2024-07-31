using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;
using TasksEvaluation.Core.Entities.Business;
using TasksEvaluation.Core.Interfaces.IServices;

namespace TasksEvaluation.Controllers
{
    public class EvaluatedAssignmentController : Controller
    {
        private readonly IEvaluatedAssignmentService _evaluatedAssignmentService;

        public EvaluatedAssignmentController(IEvaluatedAssignmentService evaluatedAssignmentService)
        {
            _evaluatedAssignmentService = evaluatedAssignmentService;
        }

        public async Task<IActionResult> Index()
        {
            var studentJson = HttpContext.Session.GetString("Student");
            var student = studentJson == null ? null : JsonConvert.DeserializeObject<Student>(studentJson);
            ViewBag.Student = student;
            var assignments = await _evaluatedAssignmentService.GetAssignmentsWithsolutions(student.Id);
            return View(assignments);
        }
    }
}
