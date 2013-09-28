using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;

namespace Tim.Cquential.Core.Matching
{
    [TestClass]
    public class MatchFinderTests
    {
        [TestMethod]
        public void FindFindsWholeSequenceMatch()
        {
            var sequence = new int[]{1,2,3};
            var query = new SumQuery(s => Tuple.Create(s == 6, s==6));
            var finder = new MatchFinder<int>();

            var matches = finder.FindMatches(sequence, query);

            matches.Should()
                .OnlyContain(m => m.Sequence.SequenceEqual(sequence));
        }

        [TestMethod]
        public void FindFindsPartialMatch()
        {
            var sequence = new int[] { 4, 1, 2, 3, 4 };
            var query = new SumQuery(s => Tuple.Create(s == 6, s==6));
            var finder = new MatchFinder<int>();
            var expected = new int[] { 1, 2, 3 };

            var matches = finder.FindMatches(sequence, query);

            matches.Should()
                .OnlyContain(m => m.Sequence.SequenceEqual(expected));
        }

        [TestMethod]
        public void FindFindsOverlappingMatches()
        {
            var sequence = new int[] { 1, 2, 3, 2, 1 };
            var query = new SumQuery(s => Tuple.Create(s == 6, s==6));
            var finder = new MatchFinder<int>();
            var expected = new int[][] 
            { 
                new int[]{1, 2, 3 },
                new int[]{3, 2, 1 }
            };

            var matches = finder.FindMatches(sequence, query);

            matches.Should()
                .ContainSingle(m => m.Sequence.SequenceEqual(expected[0]))
                .And.ContainSingle(m => m.Sequence.SequenceEqual(expected[1]));
        }
    }

    internal class AddAggregator: IAggregator<int>
    {
        private int _sum;

        public void Put(int item)
        {
            _sum += item;
        }

        public double Value
        {
	        get { return _sum; }
        }
}

    internal class SumQuery : IQuery<int>
    {
        private Func<double, Tuple<bool, bool>> _condition;

        public SumQuery(Func<double, Tuple<bool, bool>> condition)
        {
            _condition = condition;
        }

        public Tuple<bool, bool> IsMatch(IMatchCandidate<int> candidate)
        {
            double value = candidate.GetAggregator("sequence").Value;

            return _condition(value);
        }

        public IDictionary<string, Func<IAggregator<int>>> AggregatorFactory
        {
            get
            {
                return new Dictionary<string, Func<IAggregator<int>>>
                {
                    {"sequence", () => new AddAggregator()}
                };
            }
        }
    }
}
