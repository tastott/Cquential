using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.Cquential.Core.Expressions.ContextExpressions
{
    public class CountExpression<T> : ContextExpression<T>
    {
        public CountExpression()
            : base(c => c.Sequence.Count(), NumericMutability.Increasable)
        { }

        public override bool Equals(object obj)
        {
            var that = obj as CountExpression<T>;

            if (that != null) return true;
            return false;
        }
    }
}
