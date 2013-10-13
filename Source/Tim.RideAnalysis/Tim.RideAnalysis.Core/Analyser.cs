using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tim.Cquential.Core;
using Tim.Cquential.Language;

namespace Tim.RideAnalysis.Core
{
    using Models;
    using Tim.Cquential.Core.Matching;
    using Tim.Cquential.Language.Parsers;

    public class Analyser
    {
        public IEnumerable<Match> SearchRide(Ride ride, string queryString)
        {
            var parser = new ExpressionWithAggregationsQueryParser<Leg>();
            var query = parser.Parse(queryString);
            var finder = new MatchFinder<Leg>();

            var matches = finder.FindMatches(ride.Legs.ToArray(), query);

            return matches.Select(m => new Tim.RideAnalysis.Models.Match
            {
                Legs = m.Sequence
            });
        }
    }
}
