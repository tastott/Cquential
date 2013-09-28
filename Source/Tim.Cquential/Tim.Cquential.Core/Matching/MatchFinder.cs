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
            var matchCandidates = new List<IMatchCandidate<T>>();
            var completedMatches = new List<IMatch<T>>();

            int counter = 0;
            int itemCount = sequence.Count();

            foreach (var item in sequence)
            {
                //Add new match candidate starting on this item
                var aggregators = query.AggregatorFactory.ToDictionary(kv => kv.Key, kv => kv.Value());
                var newCandidate = new MatchCandidate<T>(aggregators);
                matchCandidates.Add(newCandidate);

                //Add current leg to candidates and evaluate
                var closedCandidates = new List<IMatchCandidate<T>>();
                foreach (var candidate in matchCandidates)
                {
                    candidate.Put(item);
                    var result = query.IsMatch(candidate);

                    //Process closed candidates
                    if (result.Item2)
                    {
                        closedCandidates.Add(candidate);

                        if (result.Item1)
                        {
                            completedMatches.Add(candidate.GetMatch());
                        }
                    }
                    else if (counter == itemCount - 1 && result.Item1) //If this is the last leg, treat the first (longest) match as complete
                    {
                        completedMatches.Add(candidate.GetMatch());
                        break;
                    }
                }

                //Remove closed
                matchCandidates.RemoveAll(mb => closedCandidates.Contains(mb));

                ++counter;
            }

            return completedMatches;
        }
    }
}
