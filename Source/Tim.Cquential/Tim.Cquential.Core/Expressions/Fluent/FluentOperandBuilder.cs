using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.Cquential.Core.Expressions.Fluent
{
    public class FluentOperandBuilder<T>
    {
        public FluentOperatorBuilder<T> StaticItem(int itemIndex, System.Linq.Expressions.Expression<Func<T, double>> memberExpression)
        {
            return null;
        }

        public FluentOperatorBuilder<T> Constant(double value)
        {
            return null;
        }

        public FluentOperatorBuilder<T> Max(System.Linq.Expressions.Expression<Func<T, double>> memberExpression)
        {
            return null;
        }

        public FluentOperatorBuilder<T> Min(System.Linq.Expressions.Expression<Func<T, double>> memberExpression)
        {
            return null;
        }
    }
}
