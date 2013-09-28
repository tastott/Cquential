using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.Cquential.Core
{
    public interface IAggregator<T>
    {
        void Put(T item);
        double Value { get; }
    }
}
