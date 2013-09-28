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
    public class TokenTreeMakerTests
    {
        private TokenTreeMaker GetTreeMaker()
        {
            return new TokenTreeMaker(new TokenShunter());
        }

        private string WriteTree(TokenTreeNode node)
        {
            var result = node.Value;

            if (node.Children.Any())
            {
                result += "(" + String.Join(",", node.Children.Select(c => WriteTree(c))) + ")";
            }

            return result;
        }

        [TestMethod]
        public void MakeTreeParsesSimpleExpression()
        {
            var tokens = new Token[]
            {
                new Token(TokenType.Constant, "4"),
                new Token(TokenType.Operator, "+"),
                new Token(TokenType.Constant, "3")
            };

            var tree = GetTreeMaker().MakeTree(tokens);
            var treeString = WriteTree(tree);

            treeString.Should().Be("+(4,3)");
        }

        [TestMethod]
        public void MakeTreeParsesExpressionWithCorrectOperatorPrecedence()
        {
            var tokens = new Token[]
            {
                new Token(TokenType.Constant, "4"),
                new Token(TokenType.Operator, "+"),
                new Token(TokenType.Constant, "3"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Constant, "8")
            };

            var tree = GetTreeMaker().MakeTree(tokens);
            var treeString = WriteTree(tree);

            treeString.Should().Be("+(4,*(3,8))");
        }

        [TestMethod]
        public void MakeTreeParsesExpressionWithFunction()
        {
            var tokens = new Token[]
            {
                new Token(TokenType.Function, "GIBLETS"),
                new Token(TokenType.LeftParenthesis, "("),
                new Token(TokenType.Constant, "3"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Constant, "8"),
                new Token(TokenType.RightParenthesis, ")"),
                new Token(TokenType.Operator, "+"),
                new Token(TokenType.Constant, "8")
            };

            var tree = GetTreeMaker().MakeTree(tokens);
            var treeString = WriteTree(tree);

            treeString.Should().Be("+(GIBLETS(*(3,8)),8)");
        }
    }
}
