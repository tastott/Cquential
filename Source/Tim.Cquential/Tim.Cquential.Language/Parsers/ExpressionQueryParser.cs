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
    
    public class ExpressionQueryParser<T> : IQueryParser<T>
    {
        private TokenTreeMaker _tokenTreeMaker;
        private ExpressionFactory<T> _expressions;

        public ExpressionQueryParser()
        {
            _tokenTreeMaker = new TokenTreeMaker(new TokenShunter());
            _expressions = new ExpressionFactory<T>();
        }

        public ExpressionQueryParser(TokenTreeMaker tokenTreeMaker)
        {
            _tokenTreeMaker = tokenTreeMaker;
            _expressions = new ExpressionFactory<T>();
        }

        public IQuery<T> Parse(IEnumerable<Token> rpnTokens)
        {
            var tokenTreeRoot = _tokenTreeMaker.MakeTree(rpnTokens);
            IExpression<T> expression = CreateExpressionTree(tokenTreeRoot);

            return new ExpressionQuery<T>(expression);
        }

        public IQuery<T> Parse(string queryString)
        {
            var tokenizer = new Tokenizer();
            var tokens = tokenizer.Tokenize(queryString);

            return Parse(tokens);
        }

        private IExpression<T> CreateExpressionTree(TokenTreeNode root)
        {
            switch (root.Type)
            {
                case TokenType.Constant:
                    double value;
                    if (!double.TryParse(root.Value, out value))
                        throw new Exception(String.Format("Value could not be converted to a double: '{0}'", root.Value));

                    return _expressions.Constant(value);

                case TokenType.Aggregate:
                    return ParseAggregate(root.Value);

                case TokenType.Function:
                    return MakeFunctionExpression(root);

                case TokenType.Operator:
                    var expLeft = CreateExpressionTree(root.Children[0]);
                    var expRight = CreateExpressionTree(root.Children[1]);
                    return _expressions.Operation(root.Value, expLeft, expRight);

                default:
                    throw new Exception(String.Format("Unrecognised TokenType: '{0}'", root.Type.ToString()));
            }

              
        }
        
        private IExpression<T> MakeFunctionExpression(TokenTreeNode root)
        {
            var aggregatePattern = new Regex(@"^\[\*\]\.([A-Za-z0-9]+)$");
            System.Text.RegularExpressions.Match match = null;

            string functionName = root.Value;
            var child = root.Children.Single();

            aggregatePattern.TryMatch(child.Value, out match);

            switch (functionName)
            {
                case "MAX":
                    return _expressions.Max(match.Groups[1].Value);
                case "MIN":
                    return _expressions.Min(match.Groups[1].Value);
                case "AVG":
                case "COUNT":
                    if (child.Type != TokenType.Aggregate) throw new Exception(String.Format("Cannot apply MAX function to parameter type '{0}'", child.Type.ToString()));
                    if(!match.Success) throw new Exception(String.Format("Cannot apply MAX function parameter '{0}'", child.Value));
                    return _expressions.Aggregate(functionName, match.Groups[1].Value);

                case "ALL":
                    var left = ParseRelativeItem(child.Children[0].Value);
                    var right = ParseRelativeItem(child.Children[1].Value);

                    return _expressions.AllTrue(child.Value, left.Item1, left.Item2, right.Item1, right.Item2);


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

        private IExpression<T> ParseAggregate(string input)
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
                    if (staticIndex == 0) return _expressions.FirstItemMember(memberName);
                    else return _expressions.StaticItemMember(staticIndex, memberName);
                }
                else throw new Exception("Unrecognised indexed item reference");

            }
            else if (aggregatePattern.TryMatch(input, out match))
            {
                string functionName = match.Groups[1].Value;
                string memberName = match.Groups[2].Value;

                return _expressions.Aggregate(functionName, memberName);
            }
            else throw new Exception(String.Format("Input not parseable: {0}", input));
        }
    }
}
