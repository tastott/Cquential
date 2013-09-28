using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.Cquential.Core
{
    public class Match<T>
    {
        public IEnumerable<T> Sequence { get; set; }
    }
}
