using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.Cquential.Core.Queries
{
    using Expressions;

    /// <summary>
    /// A query which evaluates matches using an expression tree and aggregators.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExpressionWithAggregatorsQuery<T> : IQuery<T>
    {
        private IExpression<T> _expression;

        public ExpressionWithAggregatorsQuery(IExpression<T> expression, IDictionary<string, Func<IAggregator<T>>> aggregatorFactory)
        {
            _expression = expression;
            AggregatorFactory = aggregatorFactory;
        }

        public MatchStatus IsMatch(IMatchCandidate<T> candidate)
        {
            var result = _expression.GetBoolValue(candidate);
            var mutable = _expression.IsBooleanMutable(candidate);

            return new MatchStatus(result, mutable);
        }

        public IDictionary<string, Func<IAggregator<T>>> AggregatorFactory { get; private set; }


        public IMatchCandidate<T> NewMatchCandidate()
        {
            throw new NotImplementedException();
        }
    }
}
