using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.Cquential.Core
{
    public interface IQuery<T>
    {
        /// <summary>
        /// Indicates whether or not the candidate is a match for this query and whether or not the 
        /// result is liable to change given an updated candidate.
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Item1 is true if this is a match. Item2 is true if the match status is permanent.</returns>
        Tuple<bool, bool> IsMatch(IMatchCandidate<T> candidate);

        IDictionary<string, Func<IAggregator<T>>> AggregatorFactory {get;}
    }
}
