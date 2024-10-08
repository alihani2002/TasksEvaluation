﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasksEvaluation.Core.Mapper
{
    public interface IBaseMapper<TSource , TDestination>
    {
        TDestination MapModel(TSource source);
        IEnumerable<TDestination> MapList(IEnumerable<TSource> source);
    }

}
