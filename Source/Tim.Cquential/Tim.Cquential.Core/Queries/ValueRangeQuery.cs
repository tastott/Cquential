using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tim.Cquential.Core.Matching;

namespace Tim.Cquential.Core.Queries
{
    public class ValueRangeQuery<T> : IQuery<T>
    {
        protected IDictionary<string, Func<IAggregator<T>>> _aggregatorFactory;
        private Func<IMatchCandidate<T>, ValueRange<bool>> _valueRangeFunc;

        public ValueRangeQuery(Func<IMatchCandidate<T>, 
            ValueRange<bool>> valueRangeFunc,
            IDictionary<string, Func<IAggregator<T>>> aggregatorFactory)
        {
            _valueRangeFunc = valueRangeFunc;
            _aggregatorFactory = aggregatorFactory;
        }

        public MatchStatus IsMatch(IMatchCandidate<T> candidate)
        {
            var range = _valueRangeFunc(candidate);

            bool stable = range.Max == range.Min;
            bool isMatch = range.Max && range.Min;

            return new MatchStatus(isMatch, !stable);
        }

        public IMatchCandidate<T> NewMatchCandidate()
        {
            return new AggregatingMatchCandidate<T>(_aggregatorFactory.ToDictionary(kv => kv.Key, kv => kv.Value()));
        }
    }
}
