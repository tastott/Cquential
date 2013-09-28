using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.Cquential.Core.Queries
{
    using Expressions;

    /// <summary>
    /// A query which evaluates matches using an expression tree.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExpressionQuery<T> : IQuery<T>
    {
        private IExpression<T> _expression;

        public ExpressionQuery(IExpression<T> expression, IDictionary<string, Func<IAggregator<T>>> aggregatorFactory)
        {
            _expression = expression;
            AggregatorFactory = aggregatorFactory;
        }

        public Tuple<bool, bool> IsMatch(IMatchCandidate<T> candidate)
        {
            var result = _expression.GetBoolValue(candidate);
            var mutable = _expression.IsBooleanMutable(candidate);

            return Tuple.Create(result, mutable);
        }

        public IDictionary<string, Func<IAggregator<T>>> AggregatorFactory { get; private set; }
    }
}
