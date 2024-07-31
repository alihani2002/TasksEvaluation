using Microsoft.AspNetCore.Mvc;

namespace TasksEvaluation.Controllers
{
    public class EvaluatedAssignmentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
