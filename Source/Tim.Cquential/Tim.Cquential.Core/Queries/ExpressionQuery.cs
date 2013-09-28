using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.Cquential.Core.Queries
{
    using Expressions;
    using Matching;

    /// <summary>
    /// A query which evaluates matches using an expression tree.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExpressionQuery<T> : IQuery<T>
    {
        public ExpressionQuery(IExpression<T> expression)
        {
            Expression = expression;
        }

        public IExpression<T> Expression { get; private set; }

        public virtual MatchStatus IsMatch(IMatchCandidate<T> candidate)
        {
            var result = Expression.GetBoolValue(candidate);
            var mutable = Expression.IsBooleanMutable(candidate);

            return new MatchStatus(result, mutable);
        }

        public virtual IMatchCandidate<T> NewMatchCandidate()
        {
            return new MatchCandidate<T>();
        }
    }
}
