using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tim.Cquential.Core.Expressions
{
    public class NumericToBooleanOperationExpression<T>: BinaryOperationExpression<T>
    {
        private int[,,] _mutabilities;

        public NumericToBooleanOperationExpression(Func<object, object, object> operationFunc,
            IExpression<T> left, IExpression<T> right, int[,,] mutabilities)
            : base(typeof(bool), typeof(double), operationFunc, left, right)
        {
            _mutabilities = mutabilities;
        }

        public override NumericMutability GetNumericMutability()
        {
            throw new Exception("Numeric mutability is not applicable to this operator");
        }

        public override bool IsBooleanMutable(IMatchCandidate<T> context)
        {
            int leftMutability = (int)_left.GetNumericMutability();
            int rightMutability = (int)_right.GetNumericMutability();
            int currentValue = GetBoolValue(context) ? 1 : 0;

            return _mutabilities[leftMutability, rightMutability, currentValue] == 1;
        }
    }
}
