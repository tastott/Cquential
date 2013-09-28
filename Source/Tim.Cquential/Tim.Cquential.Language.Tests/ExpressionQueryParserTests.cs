using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using Moq;

namespace Tim.Cquential.Language.Parsers
{
    using Core;
    using Core.Queries;
    using Core.Matching;
    using Tokens;
  
    [TestClass]
    public class ExpressionQueryParserTests
    {
        public IQueryParser GetParser()
        {
            return new ExpressionQueryParser(new TokenTreeMaker(new TokenShunter()));
        }

        public IMatchCandidate<T> MakeCandidate<T>(IEnumerable<T> sequence)
        {
            return new TestMatchCandidate<T>(sequence);
        }

        [TestMethod]
        public void ParseExpressionWithConstants()
        {
            var parser = GetParser();
            var tokens = new Tokenizer().Tokenize("3 + 4 > 2 AND 4 / 2 <= 4");

            var query = parser.Parse<Leg>(tokens) as ExpressionQuery<Leg>;

            query.Expression
                .GetBoolValue(null)
                .Should()
                .Be(true);
        }

        [TestMethod]
        public void ParseExpressionWithStaticLegReferenceAndConstant()
        {
            var parser = GetParser();
            var tokens = new Tokenizer().Tokenize("[0].Speed > 10 AND [0].StartElevation < 50");

            var query = parser.Parse<Leg>(tokens) as ExpressionQuery<Leg>;
            var context = MakeCandidate<Leg>(new List<Leg> { new Leg { Speed = 9, StartElevation = 2 } });

            query.Expression
                .GetBoolValue(context)
                .Should()
                .Be(false);
        }

        [TestMethod]
        public void ParseExpressionWithFinalLegReferenceAndConstant()
        {
            var parser = GetParser();
            var tokens = new Tokenizer().Tokenize("[n].Speed > 10 AND [n].StartElevation < 50");

            var query = parser.Parse<Leg>(tokens) as ExpressionQuery<Leg>;
            var legs = new List<Leg> 
                {
                    new Leg { Speed = 12, StartElevation = 2 } ,
                    new Leg { Speed = 5, StartElevation = 2 } 
                };

            query.Expression
                .GetBoolValue(MakeCandidate<Leg>(legs))
                .Should()
                .Be(false);
        }


        [TestMethod]
        public void ParseExpressionWithMax()
        {
            var parser = GetParser();
            var tokens = new Tokenizer().Tokenize("MAX([*].Speed) > 10");

            var query = parser.Parse<Leg>(tokens) as ExpressionQuery<Leg>;
            var legs = new List<Leg> 
                { 
                    new Leg { Speed = 9}, 
                    new Leg { Speed = 10},
                    new Leg { Speed = 11},
                };

            query.Expression
                .GetBoolValue(MakeCandidate<Leg>(legs))
                .Should()
                .Be(true);
        }

        [TestMethod]
        public void ParseExpressionWithMaxAndMin()
        {
            var parser = GetParser();
            var tokens = new Tokenizer().Tokenize("MAX([*].Speed) > MIN([*].Speed) * 2");

            var query = parser.Parse<Leg>(tokens) as ExpressionQuery<Leg>;
            var legs = new List<Leg> 
                { 
                    new Leg { Speed = 2}, 
                    new Leg { Speed = 5},
                    new Leg { Speed = 3},
                };

            query.Expression
                .GetBoolValue(MakeCandidate<Leg>(legs))
                .Should()
                .Be(true);
        }

        [TestMethod]
        public void ParseExpressionWithAll()
        {
            var parser = GetParser();
            var tokens = new Tokenizer().Tokenize("ALL([x].Speed > [x-1].Speed)");

            var query = parser.Parse<Leg>(tokens) as ExpressionQuery<Leg>;
            var legs = new List<Leg> 
                { 
                    new Leg { Speed = 2}, 
                    new Leg { Speed = 5},
                    new Leg { Speed = 6},
                    new Leg { Speed = 7},
                    new Leg { Speed = 11},
                };

            query.Expression
                .GetBoolValue(MakeCandidate<Leg>(legs))
                .Should()
                .Be(true);
        }

        [TestMethod]
        public void SimpleExpressionWithConstantsIsImmutable()
        {
            var parser = GetParser();
            var tokens = new Tokenizer().Tokenize("10 < 3");

            var query = parser.Parse<Leg>(tokens) as ExpressionQuery<Leg>;

            query.Expression
                .IsBooleanMutable(null)
                .Should()
                .Be(false);
        }

        [TestMethod]
        public void ComplexExpressionWithConstantsIsImmutable()
        {
            var parser = GetParser();
            var tokens = new Tokenizer().Tokenize("4 >= 3 AND 10 < 3");

            var query = parser.Parse<Leg>(tokens) as ExpressionQuery<Leg>;

            query.Expression
                .IsBooleanMutable(null)
                .Should()
                .Be(false);
        }

        [TestMethod]
        public void ArbitraryExpressionIsImmutable()
        {
            var parser = GetParser();
            var tokens = new Tokenizer().Tokenize("[0].Speed > 10 AND MAX([*].Speed) > MIN([*].Speed) * 2");

            var query = parser.Parse<Leg>(tokens) as ExpressionQuery<Leg>;
            var legs = new List<Leg>
                {
                    new Leg{Speed = 9},
                    new Leg{Speed = 20}
                };

            query.Expression
                .GetBoolValue(MakeCandidate<Leg>(legs))
                .Should()
                .Be(false);

            query.Expression
                 .IsBooleanMutable(MakeCandidate<Leg>(legs))
                 .Should()
                 .Be(false);
        }

        [TestMethod]
        public void AnotherArbitraryExpressionIsImmutable()
        {
            var parser = GetParser();
            var tokens = new Tokenizer().Tokenize("[0].Speed > 10 AND MAX([*].Speed) > MIN([*].Speed) * 2");

            var query = parser.Parse<Leg>(tokens) as ExpressionQuery<Leg>;
            var legs = new List<Leg>
                {
                    new Leg{Speed = 11},
                    new Leg{Speed = 23}
                };

            query.Expression
                .GetBoolValue(MakeCandidate<Leg>(legs))
                .Should()
                .Be(true);

            query.Expression
                 .IsBooleanMutable(MakeCandidate<Leg>(legs))
                 .Should()
                 .Be(false);
        }

        [TestMethod]
        public void ExpressionWithAllIsImmutableWhenFalse()
        {
            var parser = GetParser();
            var tokens = new Tokenizer().Tokenize("ALL([x].Speed > [x-1].Speed)");

            var query = parser.Parse<Leg>(tokens) as ExpressionQuery<Leg>;
            var legs = new List<Leg> 
                { 
                    new Leg { Speed = 2}, 
                    new Leg { Speed = 5},
                    new Leg { Speed = 4},
                };

            query.Expression
                .GetBoolValue(MakeCandidate<Leg>(legs))
                .Should()
                .Be(false);

            query.Expression
                .IsBooleanMutable(MakeCandidate<Leg>(legs))
                .Should()
                .Be(false);
        }

    }

    internal class Leg
    {
        public double Speed { get; set; }
        public double StartElevation { get; set; }
    }

    internal class TestMatchCandidate<T> : MatchCandidate<T>
    {
        private IEnumerable<T> _sequence;

        public TestMatchCandidate(IEnumerable<T> sequence)
        {
            _sequence = sequence;
        }

        public override IEnumerable<T> Sequence { get { return _sequence; } }
    }
}
