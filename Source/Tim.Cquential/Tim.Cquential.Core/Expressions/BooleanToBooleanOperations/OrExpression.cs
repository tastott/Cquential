using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tim.Cquential.Core.Expressions.BooleanToBooleanOperations
{
    using Core;

    public class OrExpression<T> : AbstractBooleanToBooleansOperationExpression<T>
    {
        public OrExpression(IExpression<T> left, IExpression<T> right)
            :base(left,right){}

        public override bool GetBoolValue(IMatchCandidate<T> context)
        {
            var leftValue = _left.GetBoolValue(context);
            var rightValue = _right.GetBoolValue(context);

            return leftValue || rightValue;
        }

        public override bool IsBooleanMutable(IMatchCandidate<T> context)
        {
            var leftValue = _left.GetBoolValue(context);
            var rightValue = _right.GetBoolValue(context);

            var leftMutability = _left.IsBooleanMutable(context);
            var rightMutability = _right.IsBooleanMutable(context);

            if (leftValue && !leftMutability)
                return false;
            else if (rightValue && !rightMutability)
                return false;
            else if (!leftValue && !rightValue && !leftMutability && !rightMutability)
                return false;
            else return true;
        }

        public override bool Equals(object obj)
        {
            var that = obj as OrExpression<T>;

            if (that != null) return this._left.Equals(that._left) && this._right.Equals(that._right);
            else return false;
        }
    }
}
