using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Tim.Cquential.Language.Parsers
{
    using Tokens;
    using Core;
    using Core.Expressions;
    using Core.Queries;
    using Utilities;

    public class ExpressionQueryParser : IQueryParser
    {
        private TokenTreeMaker _tokenTreeMaker;

        public ExpressionQueryParser(TokenTreeMaker tokenTreeMaker)
        {
            _tokenTreeMaker = tokenTreeMaker;
        }

        public IQuery<T> Parse<T>(IEnumerable<Token> rpnTokens)
        {
            var tokenTreeRoot = _tokenTreeMaker.MakeTree(rpnTokens);
            IExpression<T> expression = CreateExpressionTree<T>(tokenTreeRoot);

            return new ExpressionWithAggregatorsQuery<T>(expression, null);
        }

        public IQuery<T> Parse<T>(string queryString)
        {
            var tokenizer = new Tokenizer();
            var tokens = tokenizer.Tokenize(queryString);

            return Parse<T>(tokens);
        }

        private IExpression<T> CreateExpressionTree<T>(TokenTreeNode root)
        {
            switch (root.Type)
            {
                case TokenType.Constant:
                    double value;
                    if (!double.TryParse(root.Value, out value))
                        throw new Exception(String.Format("Value could not be converted to a double: '{0}'", root.Value));

                    return ExpressionFactory.Constant<T>(value);

                case TokenType.Aggregate:
                    return ParseAggregate<T>(root.Value);

                case TokenType.Function:
                    return MakeFunctionExpression<T>(root);

                case TokenType.Operator:
                    var expLeft = CreateExpressionTree<T>(root.Children[0]);
                    var expRight = CreateExpressionTree<T>(root.Children[1]);
                    return ExpressionFactory.Operation(root.Value, expLeft, expRight);

                default:
                    throw new Exception(String.Format("Unrecognised TokenType: '{0}'", root.Type.ToString()));
            }

              
        }
        
        private IExpression<T> MakeFunctionExpression<T>(TokenTreeNode root)
        {
            var aggregatePattern = new Regex(@"^\[\*\]\.([A-Za-z0-9]+)$");
            System.Text.RegularExpressions.Match match = null;

            string functionName = root.Value;
            var child = root.Children.Single();

            
            switch (functionName)
            {
                case "MAX":
                case "MIN":
                case "AVG":
                case "COUNT":
                    if (child.Type != TokenType.Aggregate) throw new Exception(String.Format("Cannot apply MAX function to parameter type '{0}'", child.Type.ToString()));
                    if(!aggregatePattern.TryMatch(child.Value, out match)) throw new Exception(String.Format("Cannot apply MAX function parameter '{0}'", child.Value));
                    return ExpressionFactory.Aggregate<T>(functionName, match.Groups[1].Value);

                case "ALL":
                    var left = ParseRelativeItem(child.Children[0].Value);
                    var right = ParseRelativeItem(child.Children[1].Value);

                    return ExpressionFactory.AllTrue<T>(child.Value, left.Item1, left.Item2, right.Item1, right.Item2);


                default:
                    throw new Exception(String.Format("Unrecognised function name: '{0}'", functionName));
            }
        }

        
        private Tuple<int, string> ParseRelativeItem(string input)
        {
             var relativeIndexPattern = new Regex(@"^\[x(-[0-9]+)?\]\.([A-Za-z]+)$");
             System.Text.RegularExpressions.Match match;

             if (!relativeIndexPattern.TryMatch(input, out match))
                        throw new Exception(String.Format("Cannot parse input as relative item reference: '{0}'", input));

            int offset = 0;
            if(match.Groups[1].Success) offset = int.Parse(match.Groups[1].Value);

            return Tuple.Create(offset, match.Groups[2].Value);
        }

        private IExpression<T> ParseAggregate<T>(string input)
        {
            var indexedItemPattern = new Regex(@"^\[([A-Za-z0-9-]+)\]\.([A-Za-z0-9]+)$");
            var aggregatePattern = new Regex(@"^([A-Za-z0-9]+)\(\[\*\]\.([A-Za-z0-9]+)\)$");

            System.Text.RegularExpressions.Match match = null;

            if (indexedItemPattern.TryMatch(input, out match))
            {
                string indexString = match.Groups[1].Value;
                string memberName = match.Groups[2].Value;

                var relativeIndexPattern = new Regex("^[A-Za-z](-[0-9]+)?$"); //TODO: Extend to positive offset
                int staticIndex;

                var relativeIndexMatch = relativeIndexPattern.Match(indexString);

                if (relativeIndexMatch.Success)
                {
                    string offsetString = relativeIndexMatch.Groups[1].Value;
                    int offset = 0;
                    if (offsetString != "") int.Parse(relativeIndexMatch.Groups[1].Value);

                    throw new Exception("Not yet implemented");
                }
                else if (int.TryParse(indexString, out staticIndex))
                {
                    return ExpressionFactory.StaticItemMember<T>(staticIndex, memberName);
                }
                else throw new Exception("Unrecognised indexed item reference");

            }
            else if (aggregatePattern.TryMatch(input, out match))
            {
                string functionName = match.Groups[1].Value;
                string memberName = match.Groups[2].Value;

                return ExpressionFactory.Aggregate<T>(functionName, memberName);
            }
            else throw new Exception(String.Format("Input not parseable: {0}", input));
        }
    }
}
