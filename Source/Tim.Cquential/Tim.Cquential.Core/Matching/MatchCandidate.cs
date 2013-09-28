using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.Cquential.Core.Matching
{
    public class MatchCandidate<T> : IMatchCandidate<T>
    {
        protected IList<T> _sequence;

        public MatchCandidate()
        {
            _sequence = new List<T>();
        }

        public virtual void Put(T item)
        {
            _sequence.Add(item);
        }

        public Match<T> GetMatch()
        {
            return new Match<T>
            {
                Sequence = _sequence.ToList()
            };
        }

        public virtual IEnumerable<T> Sequence
        {
            get { return _sequence; }
        }

        
    }
}
