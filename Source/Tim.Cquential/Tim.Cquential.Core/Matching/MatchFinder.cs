using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.Cquential.Core.Matching
{
    public class MatchFinder<T> : IMatchFinder<T>
    {
        public IEnumerable<IMatch<T>> FindMatches(IEnumerable<T> sequence, IQuery<T> query)
        {
            throw new NotImplementedException();
        }
    }
}
