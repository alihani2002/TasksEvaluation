using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TasksEvaluation.Core.DTOs;
using TasksEvaluation.Core.Entities.Business;
using TasksEvaluation.Core.IRepositories;

namespace TasksEvaluation.Controllers
{
    public class AssignmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AssignmentController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: Assignment
        public async Task<IActionResult> Index()
        {
            var assignments = await _unitOfWork.Assignments.GetAll();
            var assignmentDTOs = _mapper.Map<List<AssignmentDTO>>(assignments);
            return View(assignmentDTOs);
        }

        // GET: Assignment/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var assignment = await _unitOfWork.Assignments.GetById(id);
            if (assignment == null)
            {
                return NotFound();
            }
            var assignmentDTO = _mapper.Map<AssignmentDTO>(assignment);
            return View(assignmentDTO);
        }

        // GET: Assignment/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Assignment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,DeadLine")] AssignmentDTO assignmentDTO)
        {
            if (ModelState.IsValid)
            {
                var assignment = _mapper.Map<Assignment>(assignmentDTO);
                await _unitOfWork.Assignments.Add(assignment);
                _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            return View(assignmentDTO);
        }

        // GET: Assignment/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var assignment = await _unitOfWork.Assignments.GetById(id);
            if (assignment == null)
            {
                return NotFound();
            }
            var assignmentDTO = _mapper.Map<AssignmentDTO>(assignment);
            return View(assignmentDTO);
        }

        // POST: Assignment/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,DeadLine")] AssignmentDTO assignmentDTO)
        {
            if (id != assignmentDTO.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var assignment = await _unitOfWork.Assignments.GetById(id);
                    if (assignment == null)
                    {
                        return NotFound();
                    }

                    _mapper.Map(assignmentDTO, assignment);
                    _unitOfWork.Assignments.Update(assignment);
                    _unitOfWork.Complete();
                }
                catch (Exception)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(assignmentDTO);
        }

        // GET: Assignment/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var assignment = await _unitOfWork.Assignments.GetById(id);
            if (assignment == null)
            {
                return NotFound();
            }
            var assignmentDTO = _mapper.Map<AssignmentDTO>(assignment);
            return View(assignmentDTO);
        }

        // POST: Assignment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var assignment = await _unitOfWork.Assignments.GetById(id);
            if (assignment == null)
            {
                return NotFound();
            }

            _unitOfWork.Assignments.Remove(assignment);
            _unitOfWork.Complete();
            return RedirectToAction(nameof(Index));
        }
    }
}
