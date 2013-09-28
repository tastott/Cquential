using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tim.Cquential.Core.Expressions
{
    public class NumericToNumericOperationExpression<T> : BinaryOperationExpression<T>
    {
        private int[,] _mutabilities;

        public NumericToNumericOperationExpression(Func<object, object, object> operationFunc, 
            IExpression<T> left, IExpression<T> right, int[,] mutabilities)
            :base(typeof(double), typeof(double), operationFunc, left, right)
        {
            _mutabilities = mutabilities;
        }

        public override NumericMutability GetNumericMutability()
        {
            int leftMutability = (int)_left.GetNumericMutability();
            int rightMutability = (int)_right.GetNumericMutability();

            return (NumericMutability)_mutabilities[leftMutability, rightMutability];
        }

        public override bool IsBooleanMutable(IMatchCandidate<T> context)
        {
            throw new Exception("Boolean mutability is not applicable to this operator");
        }
    }
}
