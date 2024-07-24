﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksEvaluation.Core.Entities.Business;

namespace TasksEvaluation.Core.DTOs
{
    public class SolutionDTO : BaseDTO
    {
        public string SolutionFile { get; set; }
        public string Notes { get; set; }
        public int? StudentId { get; set; }
        public int? AssignmentId { get; set; }
    }
}
