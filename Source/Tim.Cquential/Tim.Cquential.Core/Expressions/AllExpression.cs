using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tim.Cquential.Core.Expressions
{
    using Core;
    using Matching;

    public class AllExpression<T> : IExpression<T>
    {
        private Func<object, object, object> _booleanOperation;
        private Func<IMatchCandidate<T>, double[]> _leftValuesFunc;
        private Func<IMatchCandidate<T>, double[]> _rightValuesFunc;

        public AllExpression(Func<object, object, object> booleanOperation,
            Func<IMatchCandidate<T>, double[]> leftValuesFunc, Func<IMatchCandidate<T>, double[]> rightValuesFunc)
        {
            _booleanOperation = booleanOperation;
            _leftValuesFunc = leftValuesFunc;
            _rightValuesFunc = rightValuesFunc;
        }


        public Type ReturnType { get { return typeof(bool); } }

        public bool GetBoolValue(IMatchCandidate<T> context)
        {
            var leftLegs = _leftValuesFunc(context);
            var rightLegs = _rightValuesFunc(context);

            if (leftLegs.Length != rightLegs.Length) throw new Exception("Comparison on unmatched number of values");

            for (int i = 0; i < leftLegs.Length; i++)
            {
                if (!(bool)_booleanOperation(leftLegs[i], rightLegs[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public double GetNumericValue(IMatchCandidate<T> context)
        {
            throw new Exception("ALL function cannot return a numeric value");
        }

        public NumericMutability GetNumericMutability()
        {
            throw new Exception("Numeric mutability is not applicable to the ALL function");
        }

        public bool IsBooleanMutable(IMatchCandidate<T> context)
        {
            var value = GetBoolValue(context);

            if (!value) return false;
            else return true;
        }


        public Tuple<bool, bool> GetBoolStatus(IMatchCandidate<T> context)
        {
            var value = GetBoolValue(context);

            if (!value) return Tuple.Create(value, false);
            else return Tuple.Create(value, true);
        }
    }
}
