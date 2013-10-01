using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.Cquential.Core.Aggregation.Aggregators
{
    public class AverageAggregator<T> : IAggregator<T>
    {
        private Func<T, double> _memberFunction;
        private double _sum;
        private int _count;

        public AverageAggregator(Func<T, double> memberFunction)
        {
            _memberFunction = memberFunction;
            _sum = 0;
            _count = 0;
        }

        public void Put(T item)
        {
            _sum += _memberFunction(item);
            ++_count;
        }

        public double Value
        {
            get { return _sum / _count; }
        }
    }
}
