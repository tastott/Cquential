using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.Cquential.Core.Matching
{
    public class MatchCandidate<T> : IMatchCandidate<T>, IMatch<T>
    {
        private IDictionary<string, IAggregator<T>> _aggregators;
        private IList<T> _sequence;

        public MatchCandidate(IDictionary<string, IAggregator<T>> aggregators)
        {
            _aggregators = aggregators;
            _sequence = new List<T>();
        }

        public void Put(T item)
        {
            _sequence.Add(item);
            foreach (var a in _aggregators.Values) a.Put(item);
        }

        public IAggregator<T> GetAggregator(string key)
        {
            return _aggregators[key];
        }

        public IMatch<T> GetMatch()
        {
            return this;
        }

        public IEnumerable<T> Sequence
        {
            get { return _sequence; }
        }

        
    }
}
