using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksEvaluation.Core.DTOs;

namespace TasksEvaluation.Core.Interfaces.IServices
{
    public interface IAssignmentService
    {
        Task<IEnumerable<AssignmentDTO>> GetAssignments();
        Task<AssignmentDTO> GetAssignment(int id);
        Task<AssignmentDTO> Create(AssignmentDTO model);
        Task Update(AssignmentDTO model);
        Task Delete(int id);
    }
}
