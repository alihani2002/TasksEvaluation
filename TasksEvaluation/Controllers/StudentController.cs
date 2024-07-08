using Microsoft.AspNetCore.Mvc;
using TasksEvaluation.Core.DTOs;
using TasksEvaluation.Infrastructure.Repositories;
using AutoMapper;
using TasksEvaluation.Core.Entities.Business;
using TasksEvaluation.Core.IRepositories;

namespace TasksEvaluation.Controllers
{
    public class StudentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StudentController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: Student
        public async Task<IActionResult> Index()
        {
            var students = await _unitOfWork.Students.GetAll();
            var studentDTOs = _mapper.Map<List<StudentDTO>>(students);
            return View(studentDTOs);
        }

        // GET: Student/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var student = await _unitOfWork.Students.GetById(id);
            if (student == null)
            {
                return NotFound();
            }
            var studentDTO = _mapper.Map<StudentDTO>(student);
            return View(studentDTO);
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
                var student = _mapper.Map<Student>(studentDTO);
                await _unitOfWork.Students.Add(student);
                _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            return View(studentDTO);
        }

        // GET: Student/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var student = await _unitOfWork.Students.GetById(id);
            if (student == null)
            {
                return NotFound();
            }
            var studentDTO = _mapper.Map<StudentDTO>(student);
            return View(studentDTO);
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
                    var student = await _unitOfWork.Students.GetById(id);
                    if (student == null)
                    {
                        return NotFound();
                    }

                    _mapper.Map(studentDTO, student);
                    _unitOfWork.Students.Update(student);
                    _unitOfWork.Complete();
                }
                catch (Exception)
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
            var student = await _unitOfWork.Students.GetById(id);
            if (student == null)
            {
                return NotFound();
            }
            var studentDTO = _mapper.Map<StudentDTO>(student);
            return View(studentDTO);
        }

        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _unitOfWork.Students.GetById(id);
            if (student == null)
            {
                return NotFound();
            }

            _unitOfWork.Students.Remove(student);
            _unitOfWork.Complete();
            return RedirectToAction(nameof(Index));
        }
    }
}
