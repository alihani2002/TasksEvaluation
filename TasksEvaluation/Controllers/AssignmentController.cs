﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TasksEvaluation.Core.DTOs;
using TasksEvaluation.Core.Interfaces.IServices;

namespace TasksEvaluation.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AssignmentController : Controller
    {
        private readonly IAssignmentService _assignmentService;
        private readonly IMapper _mapper;

        public AssignmentController(IAssignmentService assignmentService, IMapper mapper)
        {
            _assignmentService = assignmentService;
            _mapper = mapper;
        }

        // GET: Assignment
        public async Task<IActionResult> Index()
        {
            var assignmentDTOs = await _assignmentService.GetAssignments();
            return View(assignmentDTOs);
        }

        // GET: Assignment/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var assignmentDTO = await _assignmentService.GetAssignment(id);
            if (assignmentDTO == null)
            {
                return NotFound();
            }
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
        public async Task<IActionResult> Create(AssignmentDTO assignmentDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _assignmentService.Create(assignmentDTO);
                    TempData["SuccessMessage"] = "Assignment created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Log the exception
                    ModelState.AddModelError("", $"An error occurred while creating the assignment: {ex.Message}");
                }
            }
            return View(assignmentDTO);
        }




        // GET: Assignment/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var assignmentDTO = await _assignmentService.GetAssignment(id);
            if (assignmentDTO == null)
            {
                return NotFound();
            }
            return View(assignmentDTO);
        }

        // POST: Assignment/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,DeadLine,GroupId")] AssignmentDTO assignmentDTO)
        {
            if (id != assignmentDTO.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _assignmentService.Update(assignmentDTO);
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
            var assignmentDTO = await _assignmentService.GetAssignment(id);
            if (assignmentDTO == null)
            {
                return NotFound();
            }
            return View(assignmentDTO);
        }

        // POST: Assignment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _assignmentService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
