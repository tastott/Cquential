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
            var query = new SumQuery(s => Tuple.Create(s == 6, s < 6));
            var finder = new MatchFinder<int>();

            var matches = finder.FindMatches(sequence, query);

            matches.Should()
                .OnlyContain(m => m.Sequence.SequenceEqual(sequence));
        }

        [TestMethod]
        public void FindFindsPartialMatch()
        {
            var sequence = new int[] { 4, 1, 2, 3, 4 };
            var query = new SumQuery(s => Tuple.Create(s == 6, s < 6));
            var finder = new MatchFinder<int>();
            var expected = new int[] { 1, 2, 3 };

            var matches = finder.FindMatches(sequence, query);

            matches.Should()
                .OnlyContain(m => m.Sequence.SequenceEqual(expected));
        }

        [TestMethod]
        public void FindIgnoresShorterOverlappingMatchesWithSameEnd()
        {
            var sequence = new int[] { 1, 2, 3, 4, 5, 20 };
            var query = new SumQuery(s => Tuple.Create(s >= 6 && s < 16, s < 16 ));
            var finder = new MatchFinder<int>();
            var expected = new int[][] 
            { 
                new int[]{1, 2, 3, 4, 5 },
            };

            var matches = finder.FindMatches(sequence, query);

            matches.Should()
                .OnlyContain(m => m.Sequence.SequenceEqual(expected[0]));
        }

        [TestMethod]
        public void FindFindsImpermanentMatch()
        {
            var sequence = new int[] { 1, 2, 3, 4};
            var query = new SumQuery(s => Tuple.Create(s == 6, true));
            var finder = new MatchFinder<int>();
            var expected = new int[] { 1, 2, 3 };

            var matches = finder.FindMatches(sequence, query);

            matches.Should()
                .ContainSingle(m => m.Sequence.SequenceEqual(expected));
        }

        [TestMethod]
        public void FindFindsLongestMatchFromSameStartPoint()
        {
            var sequence = new int[] { 1, 2, 3, 0, 0, 0 };
            var query = new SumQuery(s => Tuple.Create(s == 6, s < 6));
            var finder = new MatchFinder<int>();
            var expected = new int[] { 1, 2, 3, 0, 0, 0 };

            var matches = finder.FindMatches(sequence, query);

            matches.Should()
                .ContainSingle(m => m.Sequence.SequenceEqual(expected));
        }
    }

    internal class SummingMatchCandidate : IMatchCandidate<int>
    {
        private int? _firstIndex;
        private int? _lastIndex;

        private int _sum;
        private IList<int> _sequence;

        public SummingMatchCandidate()
        {
            _sequence = new List<int>();
        }

        public void Put(int item, int index)
        {
            if (!_firstIndex.HasValue) _firstIndex = index;
            _lastIndex = index;

            _sequence.Add(item);
            _sum += item;
        }

        public double Value
        {
	        get { return _sum; }
        }


        public int FromIndex
        {
            get { return _firstIndex.Value; }
        }

        public int ToIndex
        {
            get {return _lastIndex.Value; }
        }
    }

    internal class SumQuery : IQuery<int>
    {
        private Func<double, Tuple<bool, bool>> _condition;

        public SumQuery(Func<double, Tuple<bool, bool>> condition)
        {
            _condition = condition;
        }

        public MatchStatus IsMatch(IMatchCandidate<int> candidate)
        {
            double value = (candidate as SummingMatchCandidate).Value;
            var tuple = _condition(value);

            return new MatchStatus(tuple.Item1, tuple.Item2);
        }

        public IMatchCandidate<int> NewMatchCandidate()
        {
            return new SummingMatchCandidate();
        }
    }
}
