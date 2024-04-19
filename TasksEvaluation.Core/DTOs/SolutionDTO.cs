using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasksEvaluation.Core.DTOs
{
    public class SolutionDTO : BaseDTO
    {
        public string SolutionFile { get; set; }
        public string Notes { get; set; }
    }
}
