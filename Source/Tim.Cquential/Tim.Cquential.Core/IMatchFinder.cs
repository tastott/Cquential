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
        /// <param name="sequence"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        IEnumerable<Match<T>> FindMatches(T[] sequence, IQuery<T> query);
    }
}
