using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.Cquential.Core.Aggregation.Aggregators
{
    public class PenultimateItemAggregator<T> : IAggregator<T>
    {
        private Func<T, double> _memberFunction;
        private double _value;
        private double _previousValue;

        public PenultimateItemAggregator(Func<T, double> memberFunction)
        {
            _memberFunction = memberFunction;
            _value = 0;
            _previousValue = 0;
        }

        public void Put(T item)
        {
            _previousValue = _value;
            _value = _memberFunction(item);
        }

        public double Value
        {
            get { return _previousValue; }
        }
    }
}
