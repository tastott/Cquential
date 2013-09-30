using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tim.Cquential.Core;

namespace Tim.Cquential.Language.Fluent
{
    public class FluentOperatorBuilder<T>
    {
        public FluentOperandBuilder<T> IsGreaterThan()
        {
            return null;
        }

        public FluentOperandBuilder<T> And()
        {
            return null;
        }

        public FluentOperandBuilder<T> Times()
        {
            return null;
        }

        public IQuery<T> ToQuery()
        {
            return null;
        }
    }
}
