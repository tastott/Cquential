﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.Cquential.Core.Expressions.NumericToNumericOperations
{
    public class MinusExpression<T> : AbstractNumericsToNumericOperationExpression<T>
    {
        public MinusExpression(IExpression<T> left, IExpression<T> right)
            : base
            (
                left, right
                ,(l, r) => l - r
                ,"-"
            ) { }

        public override bool Equals(object obj)
        {
            var that = obj as MinusExpression<T>;

            if (that != null) return this._left.Equals(that._left) && this._right.Equals(that._right);
            else return false;
        }
    }
}
