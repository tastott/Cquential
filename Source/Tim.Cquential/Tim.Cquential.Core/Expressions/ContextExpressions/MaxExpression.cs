using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.Cquential.Core.Expressions.ContextExpressions
{
    public class MaxExpression<T> : ContextExpression<T>
    {
        private string _memberName;

        public MaxExpression(string memberName)
            : base(c => c.Sequence.Max(i => GetMemberFunction(memberName)(i)), NumericMutability.Increasable)
        {
            _memberName = memberName;
        }

        public override bool Equals(object obj)
        {
            var that = obj as MaxExpression<T>;

            if (that != null) return this._memberName.Equals(that._memberName);
            return false;
        }
    }
}
