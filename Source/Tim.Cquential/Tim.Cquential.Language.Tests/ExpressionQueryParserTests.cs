﻿using System;
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
    using Tim.Cquential.Core.Expressions;
  
    [TestClass]
    public class ExpressionQueryParserTests
    {
        public virtual ExpressionFactory<T> GetExpressionFactory<T>()
        {
            return new ExpressionFactory<T>();
        }

        public virtual IQueryParser<T> GetParser<T>()
        {
            return new ExpressionQueryParser<T>();
        }

        public virtual IMatchCandidate<T> MakeCandidate<T>(IEnumerable<T> sequence)
        {
            return new TestMatchCandidate<T>(sequence);
        }

        [TestMethod]
        public void ParseExpressionWithConstants()
        {
            var expressions = GetExpressionFactory<Leg>();
            var parser = GetParser<Leg>();
            var tokens = new Tokenizer().Tokenize("3 + 4 > 2 AND 4 / 2 <= 4");

            var expected =
                expressions.And
                (
                    expressions.GreaterThan
                    (
                        expressions.Plus
                        (
                            expressions.Constant(3),
                            expressions.Constant(4)
                        ),
                        expressions.Constant(2)
                    ),
                    expressions.LessThanOrEqualTo
                    (
                        expressions.Divide
                        (
                            expressions.Constant(4),
                            expressions.Constant(2)
                        ),
                        expressions.Constant(4)
                    )
                );

            var query = parser.Parse(tokens) as ExpressionQuery<Leg>;

            query.Expression
                .Should()
                .Be(expected);
        }

        [TestMethod]
        public void ParseExpressionWithBracketedStaticItemReferenceAndConstant()
        {
            var expressions = GetExpressionFactory<Leg>();
            var parser = GetParser<Leg>();
            var tokens = new Tokenizer().Tokenize("[0].Speed > 10 AND [0].StartElevation < 50");
            var expected =
                expressions.And
                (
                    expressions.GreaterThan
                    (
                        expressions.FirstItemMember("Speed"),
                        expressions.Constant(10)
                    ),
                    expressions.LessThan
                    (
                        expressions.FirstItemMember("StartElevation"),
                        expressions.Constant(50)
                    )
                );


            var query = parser.Parse(tokens) as ExpressionQuery<Leg>;
            var context = MakeCandidate<Leg>(new List<Leg> { new Leg { Speed = 9, StartElevation = 2 } });

            query.Expression
                .Should()
                .Be(expected);
        }

        [TestMethod]
        public void ParseExpressionWithStaticItemReferenceAndConstant()
        {
            var expressions = GetExpressionFactory<Leg>();
            var parser = GetParser<Leg>();
            var tokens = new Tokenizer().Tokenize("0.Speed > 10 AND 0.StartElevation < 50");
            var expected =
                expressions.And
                (
                    expressions.GreaterThan
                    (
                        expressions.FirstItemMember("Speed"),
                        expressions.Constant(10)
                    ),
                    expressions.LessThan
                    (
                        expressions.FirstItemMember("StartElevation"),
                        expressions.Constant(50)
                    )
                );


            var query = parser.Parse(tokens) as ExpressionQuery<Leg>;
            var context = MakeCandidate<Leg>(new List<Leg> { new Leg { Speed = 9, StartElevation = 2 } });

            query.Expression
                .Should()
                .Be(expected);
        }

        [TestMethod]
        public void ParseExpressionWithBracketedFinalItemReferenceAndConstant()
        {
            var expressions = GetExpressionFactory<Leg>();
            var parser = GetParser<Leg>();
            var tokens = new Tokenizer().Tokenize("[n].Speed > 10 AND [n].StartElevation < 50");
            var expected =
                expressions.And
                (
                    expressions.GreaterThan
                    (
                        expressions.LastItemMember("Speed"),
                        expressions.Constant(10)
                    ),
                    expressions.LessThan
                    (
                        expressions.LastItemMember("StartElevation"),
                        expressions.Constant(50)
                    )
                );

            var query = parser.Parse(tokens) as ExpressionQuery<Leg>;

            query.Expression
                .Should()
                .Be(expected);
        }

        [TestMethod]
        public void ParseExpressionWithFinalItemReferenceAndConstant()
        {
            var expressions = GetExpressionFactory<Leg>();
            var parser = GetParser<Leg>();
            var tokens = new Tokenizer().Tokenize("n.Speed > 10 AND n.StartElevation < 50");
            var expected =
                expressions.And
                (
                    expressions.GreaterThan
                    (
                        expressions.LastItemMember("Speed"),
                        expressions.Constant(10)
                    ),
                    expressions.LessThan
                    (
                        expressions.LastItemMember("StartElevation"),
                        expressions.Constant(50)
                    )
                );

            var query = parser.Parse(tokens) as ExpressionQuery<Leg>;

            query.Expression
                .Should()
                .Be(expected);
        }


        [TestMethod]
        public void ParseExpressionWithMax()
        {
            var expressions = GetExpressionFactory<Leg>();
            var parser = GetParser<Leg>();
            var tokens = new Tokenizer().Tokenize("MAX(Speed) > 10");
            var expected =
                expressions.GreaterThan
                (
                    expressions.Max("Speed"),
                    expressions.Constant(10)
                );

            var query = parser.Parse(tokens) as ExpressionQuery<Leg>;

            query.Expression
                .Should()
                .Be(expected);
        }

        [TestMethod]
        public void ParseExpressionWithMaxAndMin()
        {
            var expressions = GetExpressionFactory<Leg>();
            var parser = GetParser<Leg>();
            var tokens = new Tokenizer().Tokenize("MAX(Speed) > MIN(Speed) * 2");
            var expected =
                expressions.GreaterThan
                (
                    expressions.Max("Speed"),
                    expressions.Times
                    (
                        expressions.Min("Speed"),
                        expressions.Constant(2)
                    )
                );

            var query = parser.Parse(tokens) as ExpressionQuery<Leg>;

            query.Expression
                .Should()
                .Be(expected);
        }

        [TestMethod]
        public void ParseExpressionWithAll()
        {
            var expressions = GetExpressionFactory<Leg>();
            var parser = GetParser<Leg>();
            var tokens = new Tokenizer().Tokenize("ALL([x].Speed > [x-1].Speed)");
            var expected =
                expressions.AllTrue(">", 0, "Speed", -1, "Speed");

            var query = parser.Parse(tokens) as ExpressionQuery<Leg>;

            query.Expression
                .Should()
                .Be(expected);
        }

        [TestMethod]
        public void ParseExpressionWithCount()
        {
            var expressions = GetExpressionFactory<Leg>();
            var parser = GetParser<Leg>();
            var tokens = new Tokenizer().Tokenize("COUNT() > 2");
            var expected =
                expressions.GreaterThan
                (
                    expressions.Count(),
                    expressions.Constant(2)
                );

            var query = parser.Parse(tokens) as ExpressionQuery<Leg>;

            query.Expression
                .Should()
                .Be(expected);
        }

        [TestMethod]
        public void SimpleExpressionWithConstantsIsImmutable()
        {
            var parser = GetParser<Leg>();
            var tokens = new Tokenizer().Tokenize("10 < 3");

            var query = parser.Parse(tokens) as ExpressionQuery<Leg>;

            query.Expression
                .IsBooleanMutable(null)
                .Should()
                .Be(false);
        }

        [TestMethod]
        public void ComplexExpressionWithConstantsIsImmutable()
        {
            var parser = GetParser<Leg>();
            var tokens = new Tokenizer().Tokenize("4 >= 3 AND 10 < 3");

            var query = parser.Parse(tokens) as ExpressionQuery<Leg>;

            query.Expression
                .IsBooleanMutable(null)
                .Should()
                .Be(false);
        }

        [TestMethod]
        public void ArbitraryExpressionIsImmutable()
        {
            var parser = GetParser<Leg>();
            var tokens = new Tokenizer().Tokenize("0.Speed > 10 AND MAX(Speed) > MIN(Speed) * 2");

            var query = parser.Parse(tokens) as ExpressionQuery<Leg>;
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
            var parser = GetParser<Leg>();
            var tokens = new Tokenizer().Tokenize("0.Speed > 10 AND MAX(Speed) > MIN(Speed) * 2");

            var query = parser.Parse(tokens) as ExpressionQuery<Leg>;
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
            var parser = GetParser<Leg>();
            var tokens = new Tokenizer().Tokenize("ALL([x].Speed > [x-1].Speed)");

            var query = parser.Parse(tokens) as ExpressionQuery<Leg>;
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

    internal class TestMatchCandidate<T> : IMatchCandidate<T>
    {
        private IEnumerable<T> _sequence;

        public TestMatchCandidate(IEnumerable<T> sequence)
        {
            _sequence = sequence;
        }

        public IEnumerable<T> Sequence { get { return _sequence; } }

        public void Put(T item, int index)
        {
            throw new NotImplementedException();
        }

        public int FromIndex
        {
            get { throw new NotImplementedException(); }
        }

        public int ToIndex
        {
            get { throw new NotImplementedException(); }
        }
    }
}
