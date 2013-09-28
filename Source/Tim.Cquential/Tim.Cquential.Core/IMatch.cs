using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.Cquential.Core
{
    public interface IMatch<T>
    {
        IEnumerable<T> Sequence { get; }
    }
}
