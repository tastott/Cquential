using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.Cquential.Core.Expressions.ContextExpressions
{
    public class MinExpression<T> : ContextExpression<T>
    {
        private string _memberName;

        public MinExpression(string memberName)
            : base(c => c.Sequence.Min(i => GetMemberFunction(memberName)(i)), NumericMutability.Increasable)
        {
            _memberName = memberName;
        }

        public override bool Equals(object obj)
        {
            var that = obj as MinExpression<T>;

            if (that != null) return this._memberName.Equals(that._memberName);
            return false;
        }
    }
}
