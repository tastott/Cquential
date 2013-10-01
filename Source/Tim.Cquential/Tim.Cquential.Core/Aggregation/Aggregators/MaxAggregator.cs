using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.Cquential.Core.Aggregation.Aggregators
{
    public class MaxAggregator<T> : IAggregator<T>
    {
        private Func<T, double> _memberFunction;
        private double _value;

        public MaxAggregator(Func<T, double> memberFunction)
        {
            _memberFunction = memberFunction;
            _value = double.MinValue;
        }

        public void Put(T item)
        {
            _value = Math.Max(_value, _memberFunction(item));
        }

        public double Value
        {
            get { return _value; }
        }
    }
}
