using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.Cquential.Core.Aggregation.Aggregators
{
    public class MinAggregator<T> : IAggregator<T>
    {
        private Func<T, double> _memberFunction;
        private double _value;

        public MinAggregator(Func<T, double> memberFunction)
        {
            _memberFunction = memberFunction;
            _value = double.MaxValue;
        }

        public void Put(T item)
        {
            _value = Math.Min(_value, _memberFunction(item));
        }

        public double Value
        {
            get { return _value; }
        }
    }
}
