using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;

namespace Tim.Cquential.Language
{
    using Tokens;

    [TestClass]
    public class TokenShunterTests
    {
        public ITokenShunter GetShunter()
        {
            return new TokenShunter();
        }

        [TestMethod]
        public void ShuntConvertsAnArbitraryInfixTokenSequenceToRpn()
        {
            var tokens = new Token[]
            {
                //3 + 4 * 2 / 1 - 5 ^ 2 ^ 3
                new Token(TokenType.Constant, "3"),
                new Token(TokenType.Operator, "+"),
                new Token(TokenType.Constant, "4"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Constant, "2"),
                new Token(TokenType.Operator, "/"),
                new Token(TokenType.Constant, "1"),
                new Token(TokenType.Operator, "-"),
                new Token(TokenType.Constant, "5"),
                new Token(TokenType.Operator, "^"),
                new Token(TokenType.Constant, "2"),
                new Token(TokenType.Operator, "^"),
                new Token(TokenType.Constant, "3"),
            };

            var shunter = GetShunter();

            //3 + 4 * 2 / 1 - 5 ^ 2 ^ 3
            var results = shunter.Shunt(tokens);
            var expected = "3 4 2 * 1 / + 5 2 3 ^ ^ -";
            var result = String.Join(" ", results.Select(t => t.Value));

            result.ShouldBeEquivalentTo(expected);
        }

        [TestMethod]
        public void ShuntConvertsAnArbitraryInfixTokenSequenceWithParenthesesToRpn()
        {
            var tokens = new Token[]
            {
                //3 + 4 * 2 / ( 1 - 5 ) ^ 2 ^ 3
                new Token(TokenType.Constant, "3"),
                new Token(TokenType.Operator, "+"),
                new Token(TokenType.Constant, "4"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Constant, "2"),
                new Token(TokenType.Operator, "/"),
                new Token(TokenType.LeftParenthesis, "("),
                new Token(TokenType.Constant, "1"),
                new Token(TokenType.Operator, "-"),
                new Token(TokenType.Constant, "5"),
                new Token(TokenType.RightParenthesis, ")"),
                new Token(TokenType.Operator, "^"),
                new Token(TokenType.Constant, "2"),
                new Token(TokenType.Operator, "^"),
                new Token(TokenType.Constant, "3"),
            };

            var shunter = GetShunter();

            //3 + 4 * 2 / ( 1 - 5 ) ^ 2 ^ 3
            var results = shunter.Shunt(tokens);
            var expected = "3 4 2 * 1 5 - 2 3 ^ ^ / +";
            var result = String.Join(" ", results.Select(t => t.Value));

            result.ShouldBeEquivalentTo(expected);
        }

        [TestMethod]
        public void ShuntConvertsAnArbitraryInfixTokenSequenceWithFunctionToRpn()
        {
            var tokens = new Token[]
            {
                //GIBLETS(4 * 2) - 5
                new Token(TokenType.Function, "GIBLETS"),
                new Token(TokenType.LeftParenthesis, "("),
                new Token(TokenType.Constant, "4"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Constant, "2"),
                new Token(TokenType.RightParenthesis, ")"),
                new Token(TokenType.Operator, "-"),
                new Token(TokenType.Constant, "5"),
            };

            var shunter = GetShunter();

            var results = shunter.Shunt(tokens);
            var expected = "4 2 * GIBLETS 5 -";
            var result = String.Join(" ", results.Select(t => t.Value));

            result.ShouldBeEquivalentTo(expected);
        }

        [TestMethod]
        public void ShuntConvertsAnArbitraryInfixTokenSequenceWithFinalParenthesisToRpn()
        {
            var tokens = new Token[]
            {
                //5 * (4 - 2)
                new Token(TokenType.Constant, "5"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.LeftParenthesis, "("),
                new Token(TokenType.Constant, "4"),
                new Token(TokenType.Operator, "-"),
                new Token(TokenType.Constant, "2"),
                new Token(TokenType.RightParenthesis, ")")
            };

            var shunter = GetShunter();

            var results = shunter.Shunt(tokens);
            var expected = "5 4 2 - *";
            var result = String.Join(" ", results.Select(t => t.Value));

            result.ShouldBeEquivalentTo(expected);
        }

        [TestMethod]
        public void ShuntGetsParameterCountOfFunctions()
        {
            var tokens = new Token[]
            {
                //MONKEY(BRAINS() * 3)
                new Token(TokenType.Function, "MONKEY"),
                new Token(TokenType.LeftParenthesis, "("),
                new Token(TokenType.Function, "BRAINS"),
                new Token(TokenType.LeftParenthesis, "("),
                new Token(TokenType.RightParenthesis, ")"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Constant, "3"),
                new Token(TokenType.RightParenthesis, ")"),
            };

            var shunter = GetShunter();

            var results = shunter.Shunt(tokens).ToArray();
            var expected = "BRAINS 3 * MONKEY";
            var result = String.Join(" ", results.Select(t => t.Value));

            result.ShouldBeEquivalentTo(expected);

            results[0].ParameterCount.Should().Be(0);
            results[3].ParameterCount.Should().Be(1);
        }
    }
}
