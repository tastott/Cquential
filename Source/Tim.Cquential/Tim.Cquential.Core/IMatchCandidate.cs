using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.Cquential.Core
{
    public interface IMatchCandidate<T>
    {
        void Put(T item);
        IAggregator<T> GetAggregator(string key);
        IMatch<T> GetMatch();
    }
}
