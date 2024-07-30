﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksEvaluation.Core.Entities.Business;

namespace TasksEvaluation.Core.Interfaces.IServices
{
    public interface IGradeService
    {
        Task<IEnumerable<EvaluationGrade>> GetGrades();
    }
}
