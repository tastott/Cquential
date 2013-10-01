using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.Cquential.Core.Expressions.ContextExpressions
{
    public class LastItemMemberExpression<T> : ContextExpression<T>
    {
        private string _memberName;

        public LastItemMemberExpression(string memberName)
            : base(c => GetMemberFunction(memberName)(c.Sequence.Last()), NumericMutability.CanIncreaseOrDecrease)
        {
            _memberName = memberName;
        }

        public override bool Equals(object obj)
        {
            var that = obj as LastItemMemberExpression<T>;

            if (that != null) return this._memberName.Equals(that._memberName);
            else return false;
        }
    }
}
