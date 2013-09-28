using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.Cquential.Core.Expressions
{
    public abstract class BinaryBooleanOperationExpression<T> : IExpression<T>
    {
        protected IExpression<T> _left;
        protected IExpression<T> _right;

        public BinaryBooleanOperationExpression(IExpression<T> left, IExpression<T> right)
        {
            _left = left;
            _right = right;
        }

        public Type ReturnType
        {
            get { return typeof(bool); }
        }

        public abstract bool GetBoolValue(IMatchCandidate<T> context);

        public double GetNumericValue(IMatchCandidate<T> context)
        {
            throw new Exception("A numeric value is not applicable to this operation");
        }

        public NumericMutability GetNumericMutability()
        {
            throw new Exception("Numeric mutability is not applicable to this operation");
        }

        public abstract bool IsBooleanMutable(IMatchCandidate<T> context);
    }
}
