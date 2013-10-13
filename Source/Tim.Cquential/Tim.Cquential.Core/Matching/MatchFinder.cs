using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.Cquential.Core.Matching
{
    public class MatchFinder<T> : IMatchFinder<T>
    {
        public IEnumerable<Match<T>> FindMatches(T[] sequence, IQuery<T> query)
        {
            var matchCandidates = new List<MatchCandidateWithPreviousState<T>>();
            var completedMatches = new List<Match<T>>();

            int counter = 0;
            int itemCount = sequence.Length;

            for(int i = 0; i < itemCount; i++)
            {
                var item = sequence[i];

                //Add new match candidate starting on this item
                var newCandidate = query.NewMatchCandidate();
                matchCandidates.Add(new MatchCandidateWithPreviousState<T>(newCandidate));
                
                //Store up to one completed match ending at or one item before this one
                Match<T> completedMatch = null;

                //Add current leg to candidates and evaluate
                var closedCandidates = new List<MatchCandidateWithPreviousState<T>>();
                foreach (var candidateState in matchCandidates)
                {
                    var candidate = candidateState.Candidate;

                    candidate.Put(item, i);
                    var result = query.IsMatch(candidate);

                    //Process definite non-matches
                    if (!result.IsMutable && !result.IsMatch)
                    {
                        closedCandidates.Add(candidateState);

                        //Take first (longest) match
                        if (candidateState.LastMatchIndex.HasValue && completedMatch == null)
                        {
                            completedMatch = new Match<T>(candidate.FromIndex, candidateState.LastMatchIndex.Value, sequence);
                        }
                    }
                    else if (counter == itemCount - 1)
                    {
                        //Take first (longest) match
                        if (completedMatch == null)
                        {
                            if (result.IsMatch)
                            {
                                completedMatch = new Match<T>(candidate.FromIndex, candidate.ToIndex, sequence);
                            }
                            else if (candidateState.LastMatchIndex.HasValue)
                            {
                                completedMatch = new Match<T>(candidate.FromIndex, candidateState.LastMatchIndex.Value, sequence);
                            }
                        }
                        
                    }

                    if (result.IsMatch) candidateState.LastMatchIndex = i;
                }

                //Remove closed
                matchCandidates.RemoveAll(mb => closedCandidates.Contains(mb));

                if (completedMatch != null) completedMatches.Add(completedMatch);

                ++counter;
            }

            return completedMatches;
        }
    }

    internal class MatchCandidateWithPreviousState<T>
    {
        public MatchCandidateWithPreviousState(IMatchCandidate<T> candidate)
        {
            Candidate = candidate;
        }

        public IMatchCandidate<T> Candidate { get; private set; }
        public int? LastMatchIndex { get; set; }
    }
}
