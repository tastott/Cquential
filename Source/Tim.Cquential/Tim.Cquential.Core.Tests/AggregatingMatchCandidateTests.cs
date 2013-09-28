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

            candidate.Put(1);

            aggregatorMocker.Verify(a => a.Put(1), Times.Once());
        }

        [TestMethod]
        public void GetMatchGetsMatchWithPutSequence()
        {
            var aggregators = new Dictionary<string, IAggregator<int>>();
            var candidate = new AggregatingMatchCandidate<int>(aggregators);
            var expected = new int[] { 1, 2, 3 };

            candidate.Put(1);
            candidate.Put(2);
            candidate.Put(3);

            candidate.GetMatch()
                .Sequence
                .Should()
                .Equal(expected);
        }
    }
}
