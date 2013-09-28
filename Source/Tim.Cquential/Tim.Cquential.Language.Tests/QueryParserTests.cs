using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tim.Cquential.Language
{
    using Tokens;

    [TestClass]
    public class QueryParserTests
    {
        public IQueryParser GetParser()
        {
            return new QueryParser(new TokenTreeMaker(new TokenShunter()));
        }

        [TestMethod]
        public void ParseExpressionWithConstants()
        {
            Assert.Inconclusive("Not tested.");

            //var parser = GetParser();
            //var tokens = new Tokenizer().Tokenize("3 + 4 > 2 AND 4 / 2 <= 4");

            //var query = parser.Parse<object>(tokens);

            //Assert.IsTrue(query.Condition(null));
        }

        [TestMethod]
        public void ParseExpressionWithStaticLegReferenceAndConstant()
        {
            Assert.Inconclusive("Not tested.");
            //var parser = GetParser();
            //var tokens = new Tokenizer().Tokenize("[0].Speed > 10 AND [0].StartElevation < 50");

            //var query = parser.Parse<object>(tokens);
            //var context = new ConditionContext
            //{
            //    Legs = new List<Leg> { new Leg { Speed = 9, StartElevation = 2 } }
            //};

            //Assert.IsFalse(query.Condition(context));
        }

        [TestMethod]
        public void ParseExpressionWithFinalLegReferenceAndConstant()
        {
            Assert.Inconclusive("Not tested.");
            //var parser = GetParser();
            //var tokens = new Tokenizer().Tokenize("[n].Speed > 10 AND [n].StartElevation < 50");

            //var query = parser.Parse<object>(tokens);
            //var context = new ConditionContext
            //{
            //    Legs = new List<Leg> 
            //    {
            //        new Leg { Speed = 12, StartElevation = 2 } ,
            //        new Leg { Speed = 5, StartElevation = 2 } 
            //    }
            //};

            //Assert.False(query.Condition(context));
        }


        [TestMethod]
        public void ParseExpressionWithMax()
        {
            Assert.Inconclusive("Not tested.");
            //var parser = GetParser();
            //var tokens = new Tokenizer().Tokenize("MAX([*].Speed) > 10");

            //var query = parser.Parse<object>(tokens);
            //var context = new ConditionContext
            //{
            //    Legs = new List<Leg> 
            //    { 
            //        new Leg { Speed = 9}, 
            //        new Leg { Speed = 10},
            //        new Leg { Speed = 11},
            //    }
            //};

            //Assert.True(query.Condition(context));
        }

        [TestMethod]
        public void ParseExpressionWithMaxAndMin()
        {
            Assert.Inconclusive("Not tested.");
            //var parser = GetParser();
            //var tokens = new Tokenizer().Tokenize("MAX([*].Speed) > MIN([*].Speed) * 2");

            //var query = parser.Parse<object>(tokens);
            //var context = new ConditionContext
            //{
            //    Legs = new List<Leg> 
            //    { 
            //        new Leg { Speed = 2}, 
            //        new Leg { Speed = 5},
            //        new Leg { Speed = 3},
            //    }
            //};

            //Assert.True(query.Condition(context));
        }

        [TestMethod]
        public void ParseExpressionWithAll()
        {
            Assert.Inconclusive("Not tested.");
            //var parser = GetParser();
            //var tokens = new Tokenizer().Tokenize("ALL([x].Speed > [x-1].Speed)");

            //var query = parser.Parse<object>(tokens);
            //var context = new ConditionContext
            //{
            //    Legs = new List<Leg> 
            //    { 
            //        new Leg { Speed = 2}, 
            //        new Leg { Speed = 5},
            //        new Leg { Speed = 6},
            //        new Leg { Speed = 7},
            //        new Leg { Speed = 11},
            //    }
            //};

            //Assert.True(query.Condition(context));
        }

        [TestMethod]
        public void SimpleExpressionWithConstantsIsImmutable()
        {
            Assert.Inconclusive("Not tested.");
            //var parser = GetParser();
            //var tokens = new Tokenizer().Tokenize("10 < 3");

            //var query = parser.Parse<object>(tokens);
    
            //Assert.True(query.ConditionExpression.GetBooleanMutability(null) == BooleanMutability.Fixed);
        }

        [TestMethod]
        public void ComplexExpressionWithConstantsIsImmutable()
        {
            Assert.Inconclusive("Not tested.");
            //var parser = GetParser();
            //var tokens = new Tokenizer().Tokenize("4 >= 3 AND 10 < 3");

            //var query = parser.Parse<object>(tokens);

            //Assert.True(query.ConditionExpression.GetBooleanMutability(null) == BooleanMutability.Fixed);
        }

        [TestMethod]
        public void ArbitraryExpressionIsImmutable()
        {
            Assert.Inconclusive("Not tested.");
            //var parser = GetParser();
            //var tokens = new Tokenizer().Tokenize("[0].Speed > 10 AND MAX([*].Speed) > MIN([*].Speed) * 2");

            //var query = parser.Parse<object>(tokens);
            //var context = new ConditionContext
            //{
            //    Legs = new List<Leg>
            //    {
            //        new Leg{Speed = 9},
            //        new Leg{Speed = 20}
            //    }
            //};

            //Assert.True(query.ConditionExpression.GetBooleanMutability(context) == BooleanMutability.Fixed);
            //Assert.False(query.ConditionExpression.GetBoolValue(context));
        }

        [TestMethod]
        public void AnotherArbitraryExpressionIsImmutable()
        {
            Assert.Inconclusive("Not tested.");
            //var parser = GetParser();
            //var tokens = new Tokenizer().Tokenize("[0].Speed > 10 AND MAX([*].Speed) > MIN([*].Speed) * 2");

            //var query = parser.Parse<object>(tokens);
            //var context = new ConditionContext
            //{
            //    Legs = new List<Leg>
            //    {
            //        new Leg{Speed = 11},
            //        new Leg{Speed = 23}
            //    }
            //};

            //Assert.True(query.ConditionExpression.GetBooleanMutability(context) == BooleanMutability.Fixed);
            //Assert.True(query.ConditionExpression.GetBoolValue(context));
        }

        [TestMethod]
        public void ExpressionWithAllIsImmutableWhenFalse()
        {
            Assert.Inconclusive("Not tested.");
            //var parser = GetParser();
            //var tokens = new Tokenizer().Tokenize("ALL([x].Speed > [x-1].Speed)");

            //var query = parser.Parse<object>(tokens);
            //var context = new ConditionContext
            //{
            //    Legs = new List<Leg> 
            //    { 
            //        new Leg { Speed = 2}, 
            //        new Leg { Speed = 5},
            //        new Leg { Speed = 4},
            //    }
            //};

            //Assert.False(query.Condition(context));
            //Assert.That(query.ConditionExpression.GetBooleanMutability(context), Is.EqualTo(BooleanMutability.Fixed));
        }

    }
}
