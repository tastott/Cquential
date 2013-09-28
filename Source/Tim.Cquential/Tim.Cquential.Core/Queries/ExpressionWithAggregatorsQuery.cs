using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.Cquential.Core.Queries
{
    using Matching;
    using Expressions;

    /// <summary>
    /// A query which evaluates matches using an expression tree and aggregators.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExpressionWithAggregatorsQuery<T> : ExpressionQuery<T>
    {
        protected IDictionary<string, Func<IAggregator<T>>> _aggregatorFactory;

        public ExpressionWithAggregatorsQuery(IExpression<T> expression, IDictionary<string, Func<IAggregator<T>>> aggregatorFactory)
            :base(expression)
        {
            _aggregatorFactory = aggregatorFactory;
        }

        public override IMatchCandidate<T> NewMatchCandidate()
        {
            return new AggregatingMatchCandidate<T>(_aggregatorFactory.ToDictionary(kv => kv.Key, kv => kv.Value()));
        }
    }
}
