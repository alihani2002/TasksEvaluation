using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TasksEvaluation.Core.DTOs;
using TasksEvaluation.Core.Entities.Business;
using TasksEvaluation.Core.Interfaces.IServices;
using TasksEvaluation.Core.Mapper;
using TasksEvaluation.Infrastructure.Services;

namespace TasksEvaluation.Controllers
{
    public class SolutionController : Controller
    {
        private readonly ISolutionService _solutionService;
        private readonly IGradeService _evaluationGradeService;
        private readonly IBaseMapper<UploadSolutionDTO, SolutionDTO> _UploadsolutionDTOMapper;
        private readonly IBaseMapper<SolutionDTO, UploadSolutionDTO> _UploadsolutionMapper;

        public SolutionController(ISolutionService solutionService, IGradeService evaluationGradeService, IBaseMapper<UploadSolutionDTO, SolutionDTO> uploadsolutionDTOMapper, IBaseMapper<SolutionDTO, UploadSolutionDTO> uploadsolutionMapper)
        {
            _solutionService = solutionService;
            _evaluationGradeService = evaluationGradeService;
            _UploadsolutionDTOMapper = uploadsolutionDTOMapper;
            _UploadsolutionMapper = uploadsolutionMapper;
        }

        public async Task<IActionResult> Index()
        {
            var solutions = await _solutionService.GetStudenSolutions();
            return View(solutions);
        }

        public async Task<IActionResult> Details(int id)
        {
            var solution = await _solutionService.GetSolutionWithStudent(id);
            if (solution == null)
            {
                return NotFound();
            }
            ViewBag.Grades = await _evaluationGradeService.GetGrades();
            return View(solution);
        }

        [HttpPost]
        public async Task<IActionResult> GradeSolution(int solutionId, int gradeId)
        {
            var solution = await _solutionService.GetSolution(solutionId);
            if (solution == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                
                    solution.GradeId = gradeId;
                    await _solutionService.Update(solution);
                return RedirectToAction("Index");
            }
            return NotFound();
        }
    }
}
