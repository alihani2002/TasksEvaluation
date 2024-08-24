using Microsoft.AspNetCore.Mvc;

namespace TasksEvaluation.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
