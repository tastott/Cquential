using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.Cquential.Core.Matching
{
    public class MatchCandidate<T> : IMatchCandidate<T>
    {
        protected int? _firstIndex;
        protected int? _lastIndex;

        public virtual void Put(T item, int index)
        {
            if (!_firstIndex.HasValue) _firstIndex = index;
            _lastIndex = index;
        }

        public int FromIndex { get { return _firstIndex.Value; } }
        public int ToIndex { get { return _lastIndex.Value; } }

        public IEnumerable<T> Sequence { get { return null; } }
    }
}
