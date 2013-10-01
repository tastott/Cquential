using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.Cquential.Core.Expressions.ContextExpressions
{
    public class AverageExpression<T> : ContextExpression<T>
    {
        private string _memberName;

        public AverageExpression(string memberName)
            : base(c => c.Sequence.Average(i => GetMemberFunction(memberName)(i)), NumericMutability.CanIncreaseOrDecrease)
        {
            _memberName = memberName;
        }

        public override bool Equals(object obj)
        {
            var that = obj as AverageExpression<T>;

            if (that != null) return this._memberName.Equals(that._memberName);
            return false;
        }
    }
}
