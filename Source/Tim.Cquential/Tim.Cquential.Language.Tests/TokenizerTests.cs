using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;

namespace Tim.Cquential.Language
{
    using Tokens;
    using Utilities;

    [TestClass]
    public class TokenizerTests
    {
        [TestMethod]
        public void TokenizeRemovesWhitespace()
        {
            var tokenizer = new Tokenizer();
            string input = " 1     + 2   ";
            var expected = new string[] { "1", "+", "2" };

            var tokens = tokenizer.Tokenize(input).ToList();

            tokens.Select(t => t.Value)
                .Should()
                .Equal(expected);
        }

        [TestMethod]
        public void TokenizeIdentifiesNumericOperators()
        {
            var tokenizer = new Tokenizer();
            string input = "*/+-%";
            var expected = new string[] { "*", "/", "+","-","%"};

            var tokens = tokenizer.Tokenize(input).ToList();

            tokens.Should()
                .OnlyContain(t => t.Type == TokenType.Operator);

            tokens.Select(t => t.Value)
                .Should()
                .Equal(expected);
        }

        [TestMethod]
        public void TokenizeIdentifiesComparisonOperators()
        {
            var tokenizer = new Tokenizer();
            string input = "><!==<=>=";
            var expected = new string[] { ">", "<", "!=", "=", "<=", ">=" };

            var tokens = tokenizer.Tokenize(input).ToList();

            tokens.Should()
                .OnlyContain(t => t.Type == TokenType.Operator);

            tokens.Select(t => t.Value)
                .Should()
                .Equal(expected);
        }

        [TestMethod]
        public void TokenizeIdentifiesParentheses()
        {
            var tokenizer = new Tokenizer();
            string input = "()";

            var tokens = tokenizer.Tokenize(input).ToList();

            tokens[0].Should()
                .Match<Token>(t => t.Type == TokenType.LeftParenthesis && t.Value == "(");

            tokens[1].Should()
                .Match<Token>(t => t.Type == TokenType.RightParenthesis && t.Value == ")");

            tokens.Should()
                .HaveCount(2);
        }

        [TestMethod]
        public void TokenizeIdentifiesBooleanOperators()
        {
            var tokenizer = new Tokenizer();
            string input = "AND OR";

            var tokens = tokenizer.Tokenize(input).ToList();

            tokens[0].Should()
                .Match<Token>(t => t.Type == TokenType.Operator && t.Value == "AND");

            tokens[1].Should()
                .Match<Token>(t => t.Type == TokenType.Operator && t.Value == "OR");

            tokens.Should()
                .HaveCount(2);

        }

        [TestMethod]
        public void TokenizeIdentifiesStaticLegReference()
        {
            var tokenizer = new Tokenizer();
            string input = "[x].Speed";

            var tokens = tokenizer.Tokenize(input).ToList();

            tokens.Should()
                .OnlyContain(t => t.Type == TokenType.Aggregate && t.Value == "[x].Speed");
        }

        [TestMethod]
        public void TokenizeIdentifiesRelativeLegReference()
        {
            var tokenizer = new Tokenizer();
            string input = "[x-1].Speed";

            var tokens = tokenizer.Tokenize(input).ToList();

            tokens.Should()
                .OnlyContain(t => t.Type == TokenType.Aggregate && t.Value == "[x-1].Speed");
        }

        [TestMethod]
        public void TokenizeIdentifiesAggregateFunction()
        {
            var tokenizer = new Tokenizer();
            string input = "BLAH([*].Speed)";

            var tokens = tokenizer.Tokenize(input);

            var expected = new TupleList<string, TokenType>
            { 
                {"BLAH", TokenType.Function},
                {"(", TokenType.LeftParenthesis},
                {"[*].Speed", TokenType.Aggregate},
                {")", TokenType.RightParenthesis},
            };

            AssertTokensMatch(expected, tokens);
        }

        [TestMethod]
        public void TokenizeTokenizesSomeArbitraryInput()
        {
            var tokenizer = new Tokenizer();
            string input = "BLAH([*].Speed)> 5 AND ALL([x].Speed>[x-1].Speed) AND [0].Speed < 5   ";

            var tokens = tokenizer.Tokenize(input);

            var expected = new TupleList<string, TokenType>
            { 
                {"BLAH", TokenType.Function},
                {"(", TokenType.LeftParenthesis},
                {"[*].Speed", TokenType.Aggregate},
                {")", TokenType.RightParenthesis},
                {">", TokenType.Operator},
                {"5", TokenType.Constant},
                {"AND",TokenType.Operator}, 
                {"ALL", TokenType.Function},
                {"(", TokenType.LeftParenthesis},
                {"[x].Speed",TokenType.Aggregate},
                {">",TokenType.Operator},
                {"[x-1].Speed",TokenType.Aggregate},
                {")", TokenType.RightParenthesis},
                {"AND",TokenType.Operator},
                {"[0].Speed",TokenType.Aggregate},
                {"<",TokenType.Operator},
                {"5",TokenType.Constant},    
            };

            AssertTokensMatch(expected, tokens);
        }

        private void AssertTokensMatch(TupleList<string, TokenType> expected, IEnumerable<Token> actual)
        {
            var tokens = actual.ToList();
            var expectedTokens = expected.Select(tuple => new Token(tuple.Item2, tuple.Item1));

            tokens.Should()
                .Equal(expectedTokens);
        }
    }
}
