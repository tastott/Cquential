using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.Cquential.Core.Matching
{
    public class AggregatingMatchCandidate<T> : MatchCandidate<T>
    {
        private IDictionary<string, IAggregator<T>> _aggregators;

        public AggregatingMatchCandidate(IDictionary<string, IAggregator<T>> aggregators)
        {
            _aggregators = aggregators;
        }

        public override void Put(T item)
        {
            base.Put(item);
            foreach (var a in _aggregators.Values) a.Put(item);
        }

        public IAggregator<T> GetAggregator(string key)
        {
            return _aggregators[key];
        }
    }
}
