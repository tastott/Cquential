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
        private string _aggregatorKey;
        private NumericMutability _mutability;

        public AggregatingContextExpression(string aggregatorKey, NumericMutability mutability)
        {
            _aggregatorKey = aggregatorKey;
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

            return aggregatorContext.GetAggregator(_aggregatorKey).Value;
        }

        public NumericMutability GetNumericMutability()
        {
            return _mutability;
        }

        public bool IsBooleanMutable(IMatchCandidate<T> context)
        {
            throw new NotImplementedException();
        }


        public Tuple<bool, bool> GetBoolStatus(IMatchCandidate<T> context)
        {
            throw new NotImplementedException();
        }
    }
}
