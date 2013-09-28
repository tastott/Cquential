using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tim.Cquential.Core.Expressions
{
    using Matching;

    public class ContextExpression<T> : IExpression<T>
    {
        private Func<MatchCandidate<T>, double> _valueFunc;
        private NumericMutability _mutability;

        public ContextExpression(Func<MatchCandidate<T>, double> valueFunc, NumericMutability mutability)
        {
            _valueFunc = valueFunc;
            _mutability = mutability;
        }

        public Type ReturnType { get { return typeof(double); } }

        

        public NumericMutability GetNumericMutability()
        {
            return _mutability;
        }

        public bool GetBoolValue(IMatchCandidate<T> context)
        {
            throw new NotImplementedException();
        }

        public double GetNumericValue(IMatchCandidate<T> context)
        {
            return _valueFunc(context as MatchCandidate<T>);
        }

        public bool IsBooleanMutable(IMatchCandidate<T> context)
        {
            throw new NotImplementedException();
        }
    }
}
