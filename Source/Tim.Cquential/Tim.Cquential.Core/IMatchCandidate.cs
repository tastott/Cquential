﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.Cquential.Core
{
    public interface IMatchCandidate<T>
    {
        void Put(T item, int index);
        int FromIndex { get; }
        int ToIndex { get; }
        IEnumerable<T> Sequence { get; }
    }
}
