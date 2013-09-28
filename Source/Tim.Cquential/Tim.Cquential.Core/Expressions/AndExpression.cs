using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Tim.Cquential.Core.Expressions
{
    using Core;

    public class AndExpression<T> : BinaryBooleanOperationExpression<T>
    {
        public AndExpression(IExpression<T> left, IExpression<T> right)
            :base(left,right){}


        public override bool GetBoolValue(IMatchCandidate<T> context)
        {
            return _left.GetBoolValue(context) && _right.GetBoolValue(context);
        }

        public override bool IsBooleanMutable(IMatchCandidate<T> context)
        {
            var leftValue = _left.GetBoolValue(context);
            var rightValue = _right.GetBoolValue(context);

            var leftMutability = _left.IsBooleanMutable(context);
            var rightMutability = _right.IsBooleanMutable(context);

            if (!leftValue && !leftMutability)
                return false;
            else if (!rightValue && !rightMutability)
                return false;
            else if (leftValue && rightValue && !leftMutability && !rightMutability)
                return false;
            else return true;
        }
    }
}
