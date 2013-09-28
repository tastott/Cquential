using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.Cquential.Core.Matching
{
    public class MatchCandidate<T> : IMatchCandidate<T>, IMatch<T>
    {
        public MatchCandidate(IDictionary<string, IAggregator<T>> aggregators)
        {
        }

        public void Put(T item)
        {
            throw new NotImplementedException();
        }

        public IAggregator<T> GetAggregator(string key)
        {
            throw new NotImplementedException();
        }

        public IMatch<T> GetMatch()
        {
            return this;
        }

        public IEnumerable<T> Sequence
        {
            get { throw new NotImplementedException(); }
        }

        
    }
}
