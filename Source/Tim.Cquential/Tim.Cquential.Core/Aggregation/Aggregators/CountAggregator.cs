using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.Cquential.Core.Aggregation.Aggregators
{
    public class CountAggregator<T> : IAggregator<T>
    {
        private int _count;

        public CountAggregator()
        {
            _count = 0;
        }

        public void Put(T item)
        {
            ++_count;
        }

        public double Value
        {
            get { return _count; }
        }
    }
}
