using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Tim.Cquential.Utilities
{
    public static class RegexExtensions
    {
        public static IEnumerable<Match> ToEnumerable(this MatchCollection matchCollection)
        {
            var matches = new List<Match>();

            foreach (var match in matchCollection) matches.Add((Match)match);

            return matches;
        }

        public static bool TryMatch(this Regex regex, string input, out Match match)
        {
            match = regex.Match(input);

            if (match.Success) return true;
            else
            {
                match = null;
                return false;
            }
        }
    }
}
