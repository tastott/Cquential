using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.Cquential.Core.Aggregation
{
    using Expressions;
    using Matching;

    public class AggregatingContextExpression<T> : IExpression<T>
    {
        public string AggregatorKey { get; private set; }
        private NumericMutability _mutability;

        public AggregatingContextExpression(string aggregatorKey, NumericMutability mutability)
        {
            AggregatorKey = aggregatorKey;
            _mutability = mutability;
        }

        public Type ReturnType
        {
            get { return typeof(double); }
        }

        public bool GetBoolValue(IMatchCandidate<T> context)
        {
            throw new NotImplementedException();
        }

        public double GetNumericValue(IMatchCandidate<T> context)
        {
            var aggregatorContext = context as AggregatingMatchCandidate<T>;

            return aggregatorContext.GetAggregator(AggregatorKey).Value;
        }

        public NumericMutability GetNumericMutability()
        {
            return _mutability;
        }

        public bool IsBooleanMutable(IMatchCandidate<T> context)
        {
            throw new NotImplementedException();
        }
    }
}
