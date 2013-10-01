using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.Cquential.Core.Aggregation.Aggregators
{
    public class IndexedItemAggregator<T> : IAggregator<T>
    {
        private int _index;
        private Func<T, double> _memberFunction;
        private double _value;
        private int _count;

        public IndexedItemAggregator(int index, Func<T, double> memberFunction)
        {
            _index = index;
            _memberFunction = memberFunction;
            _value = 0;
            _count = 0;
        }

        public void Put(T item)
        {
            if (_count == _index) _value = _memberFunction(item);
            ++_count;
        }

        public double Value
        {
            get { return _value; }
        }
    }
}
