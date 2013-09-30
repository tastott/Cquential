using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.Cquential.Core.Expressions.ContextExpressions
{
    public class FirstItemMemberExpression<T> : ContextExpression<T>
    {
        private string _memberName;

        public FirstItemMemberExpression(string memberName)
            : base(c => GetMemberFunction(memberName)(c.Sequence.First()), NumericMutability.Fixed)
        {
            _memberName = memberName;
        }

        public override bool Equals(object obj)
        {
            var that = obj as FirstItemMemberExpression<T>;

            if (that != null) return this._memberName.Equals(that._memberName);
            else return false;
        }
    }
}
