using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using FluentAssertions;

namespace Tim.Cquential.Core.Matching
{
    [TestClass]
    public class MatchCandidateTests
    {
        [TestMethod]
        public void GetAggregatorGetsAggregator()
        {
            var aggregators = new Dictionary<string, IAggregator<int>>
            {
                {"abc", new Mock<IAggregator<int>>().Object}
            };
            var candidate = new AggregatingMatchCandidate<int>(aggregators);

            candidate.GetAggregator("abc")
                .Should()
                .BeSameAs(aggregators["abc"]);
        }

        [TestMethod]
        public void PutPutsItemOntoAggregator()
        {
            var aggregatorMocker =  new Mock<IAggregator<int>>();
            var aggregators = new Dictionary<string, IAggregator<int>>
            {
                {"abc", aggregatorMocker.Object}
            };
            var candidate = new AggregatingMatchCandidate<int>(aggregators);

            candidate.Put(1, 0);

            aggregatorMocker.Verify(a => a.Put(1), Times.Once());
        }

        [TestMethod]
        public void GetMatchGetsMatchWithPutSequence()
        {
            var aggregators = new Dictionary<string, IAggregator<int>>();
            var candidate = new AggregatingMatchCandidate<int>(aggregators);
            var expected = new int[] { 2, 3, 4 };

            candidate.Put(2, 0);
            candidate.Put(3, 1);
            candidate.Put(4, 2);

            candidate.FromIndex.Should().Be(0);
            candidate.ToIndex.Should().Be(2);
        }
    }
}
