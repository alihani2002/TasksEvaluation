using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasksEvaluation.Core.DTOs
{
    public class SolutionStudentDTO : BaseDTO
    {
        public string SolutionFile { get; set; }
        public string Notes { get; set; }
        public int? StudentId { get; set; }
        public int? AssignmentId { get; set; }
        public int? GradeId { get; set; }

        // New properties for displaying related data
        public string StudentName { get; set; }
        public string AssignmentTitle { get; set; }
    }

}
