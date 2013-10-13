using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Tim.Cquential.Core.Expressions.BooleanToBooleanOperations
{
    using Core;

    public class AndExpression<T> : AbstractBooleanToBooleansOperationExpression<T>
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

        public override bool Equals(object obj)
        {
            var that = obj as AndExpression<T>;

            if (that != null) return this._left.Equals(that._left) && this._right.Equals(that._right);
            else return false;
        }

        public override Tuple<bool, bool> GetBoolStatus(IMatchCandidate<T> context)
        {
            var leftStatus = _left.GetBoolStatus(context);
            var rightStatus = _right.GetBoolStatus(context);

            var value = leftStatus.Item1 && rightStatus.Item1;
            bool isMutable;

            if (!leftStatus.Item1 && !leftStatus.Item2)
                isMutable =  false;
            else if (!rightStatus.Item1 && !rightStatus.Item2)
                isMutable =  false;
            else if (leftStatus.Item1 && rightStatus.Item1 && !leftStatus.Item2 && !rightStatus.Item2)
                isMutable =  false;
            else isMutable =  true;

            return Tuple.Create(value, isMutable);
        }
    }
}
