using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.Cquential.Core
{
    public interface IMatchFinder<T>
    {
        /// <summary>
        /// Finds sub-sequences of a sequence which match a query.
        /// </summary>
        /// <param name="seqeunce"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        IEnumerable<IMatch<T>> FindMatches(IEnumerable<T> seqeunce, IQuery<T> query);
    }
}
