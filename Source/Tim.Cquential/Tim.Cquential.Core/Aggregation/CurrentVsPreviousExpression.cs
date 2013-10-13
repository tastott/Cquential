using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tim.Cquential.Core.Expressions;
using Tim.Cquential.Core.Matching;

namespace Tim.Cquential.Core.Aggregation
{
    public class CurrentVsPreviousExpression<T> : IExpression<T>
    {
        string _currentAggregatorKey;
        string _previousAggregatorKey;
        Func<double, double, bool> _operation;

        public CurrentVsPreviousExpression(string currentAggregatorKey, string previousAggregatorKey, Func<double, double, bool> operation)
        {
            _currentAggregatorKey = currentAggregatorKey;
            _previousAggregatorKey = previousAggregatorKey;
            _operation = operation;
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public Type ReturnType
        {
            get { return typeof(bool); }
        }

        public bool GetBoolValue(IMatchCandidate<T> context)
        {
            var aggregatorContext = context as AggregatingMatchCandidate<T>;

            var currentValue = aggregatorContext.GetAggregator(_currentAggregatorKey).Value;
            var previousValue = aggregatorContext.GetAggregator(_previousAggregatorKey).Value;

            return _operation(currentValue, previousValue);
        }

        public double GetNumericValue(IMatchCandidate<T> context)
        {
            throw new NotImplementedException();
        }

        public NumericMutability GetNumericMutability()
        {
            throw new NotImplementedException();
        }

        public bool IsBooleanMutable(IMatchCandidate<T> context)
        {
            throw new NotImplementedException();
        }

        public Tuple<bool, bool> GetBoolStatus(IMatchCandidate<T> context)
        {
            var value = GetBoolValue(context);

            if (!value) return Tuple.Create(value, false);
            else return Tuple.Create(value, true);
        }
    }
}
