using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.Cquential.Core.Expressions.NumericToBooleanOperations
{
    public class GreaterThanExpression<T> : AbstractNumericsToBooleanOperationExpression<T>
    {
        public GreaterThanExpression(IExpression<T> left, IExpression<T> right)
            : base
            (
                left, right
                ,(l, r) => l > r
                ,">"
            ) { }

        public override bool Equals(object obj)
        {
            var that = obj as GreaterThanExpression<T>;

            if (that != null) return this._left.Equals(that._left) && this._right.Equals(that._right);
            else return false;
        }
    }
}
