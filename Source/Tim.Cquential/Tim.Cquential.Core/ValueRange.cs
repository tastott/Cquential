using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.Cquential.Core
{
    public class ValueRange<T>
    {
        public T Min { get; private set; }
        public T Max { get; private set; }

        public ValueRange(T min, T max)
        {
            Min = min;
            Max = max;
        }
    }
}
