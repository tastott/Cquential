using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.Cquential.Core.Aggregation.Aggregators
{
    public class LastItemAggregator<T> : IAggregator<T>
    {
        private Func<T, double> _memberFunction;
        private double _value;

        public LastItemAggregator(Func<T, double> memberFunction)
        {
            _memberFunction = memberFunction;
            _value = 0;
        }

        public void Put(T item)
        {
            _value = _memberFunction(item);
        }

        public double Value
        {
            get { return _value; }
        }
    }
}
