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
    using Tim.Cquential.Core.Aggregation;
    
    public class ExpressionQueryParser<T> : IQueryParser<T>
    {
        protected TokenTreeMaker _tokenTreeMaker;
        protected ExpressionFactory<T> _expressions;
        protected IDictionary<string, FunctionDefinition<T>> _functionDefs;

        public ExpressionQueryParser()
            : this(new TokenTreeMaker(new TokenShunter()), new ExpressionFactory<T>()) { }

        public ExpressionQueryParser(ExpressionFactory<T> expressions)
            : this(new TokenTreeMaker(new TokenShunter()), expressions) { }


        public ExpressionQueryParser(TokenTreeMaker tokenTreeMaker, ExpressionFactory<T> expressions)
        {
            _tokenTreeMaker = tokenTreeMaker;
            _expressions = expressions;
            _functionDefs = new Dictionary<string, FunctionDefinition<T>>
            {
                {"MAX", new FunctionDefinition<T>(1, (x,c) => x.Max(c[0]))},
                {"MIN", new FunctionDefinition<T>(1, (x,c) => x.Min(c[0]))},
                {"AVG", new FunctionDefinition<T>(1, (x,c) => x.Average(c[0]))},
                {"COUNT", new FunctionDefinition<T>(0, (x,c) => x.Count())}
            };
        }

        public virtual IQuery<T> Parse(IEnumerable<Token> rpnTokens)
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

        protected IExpression<T> CreateExpressionTree(TokenTreeNode root)
        {
            switch (root.Type)
            {
                case TokenType.Constant:
                    double value;
                    if (!double.TryParse(root.Value, out value))
                        throw new Exception(String.Format("Value could not be converted to a double: '{0}'", root.Value));

                    return _expressions.Constant(value);

                case TokenType.SingleItemMember:
                    return ParseSingleItemMember(root.Value);

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

        protected IExpression<T> MakeFunctionExpression(TokenTreeNode root)
        {
            string functionName = root.Value;
            var parameters = root.Children.Select(x => x.Value).ToArray();

            FunctionDefinition<T> functionDef;
            if(_functionDefs.TryGetValue(functionName, out functionDef))
            {
                if(parameters.Count() != functionDef.Parameters)
                    throw new Exception(
                        String.Format("Expected {0} parameter(s) for function '{1}' but found {2}", functionDef.Parameters, functionName, parameters.Count()));

                return functionDef.GetExpression(_expressions, parameters);
            }
            else
            {
                switch (functionName)
                {
                    case "ALL":
                        var left = ParseRelativeItem(root.Children[0].Children[0].Value);
                        var right = ParseRelativeItem(root.Children[0].Children[1].Value);

                        return _expressions.AllTrue(root.Children[0].Value, left.Item1, left.Item2, right.Item1, right.Item2);


                    default:
                        throw new Exception(String.Format("Unrecognised function name: '{0}'", functionName));
                }
            }
        }


        protected Tuple<int, string> ParseRelativeItem(string input)
        {
             var relativeIndexPattern = new Regex(@"^\[x(-[0-9]+)?\]\.([A-Za-z]+)$");
             System.Text.RegularExpressions.Match match;

             if (!relativeIndexPattern.TryMatch(input, out match))
                        throw new Exception(String.Format("Cannot parse input as relative item reference: '{0}'", input));

            int offset = 0;
            if(match.Groups[1].Success) offset = int.Parse(match.Groups[1].Value);

            return Tuple.Create(offset, match.Groups[2].Value);
        }

        protected IExpression<T> ParseSingleItemMember(string input)
        {
            var indexedItemPattern = new Regex(@"^\[?((?<static>[0-9]+)|(?<last>n)|(?<relative>x(-|\+)[0-9]+))\]?\.(?<member>[A-Za-z0-9]+)$");

            var match = indexedItemPattern.Match(input);

            if (match.Groups["static"].Success)
            {
                int index = int.Parse(match.Groups["static"].Value);

                if (index == 0) return _expressions.FirstItemMember(match.Groups["member"].Value);
                else return _expressions.StaticItemMember(index, match.Groups["member"].Value);
            }
            else if (match.Groups["last"].Success)
            {
                return _expressions.LastItemMember(match.Groups["member"].Value);
            }
            //else if (match.Groups["relative"].Success)
            //{
            //    throw new NotImplementedException();
            //}
            else throw new Exception(String.Format("Input not parseable: {0}", input));
            
            //if (indexedItemPattern.TryMatch(input, out match))
            //{
            //    string indexString = match.Groups[1].Value;
            //    string memberName = match.Groups[2].Value;

            //    var relativeIndexPattern = new Regex("^[A-Za-z](-[0-9]+)?$"); //TODO: Extend to positive offset
            //    int staticIndex;

            //    var relativeIndexMatch = relativeIndexPattern.Match(indexString);

            //    if (relativeIndexMatch.Success)
            //    {
            //        string offsetString = relativeIndexMatch.Groups[1].Value;
            //        int offset = 0;
            //        if (offsetString != "") int.Parse(relativeIndexMatch.Groups[1].Value);

            //        throw new Exception("Not yet implemented");
            //    }
            //    else if (int.TryParse(indexString, out staticIndex))
            //    {
            //        if (staticIndex == 0) return _expressions.FirstItemMember(memberName);
            //        else return _expressions.StaticItemMember(staticIndex, memberName);
            //    }
            //    else throw new Exception("Unrecognised indexed item reference");

            //}
            //else if (aggregatePattern.TryMatch(input, out match))
            //{
            //    string functionName = match.Groups[1].Value;
            //    string memberName = match.Groups[2].Value;

            //    return _expressions.Aggregate(functionName, memberName);
            //}
            
        }
    }

    public class FunctionDefinition<T>
    {
        private Func<ExpressionFactory<T>, string[], IExpression<T>> _getExpression;

        public FunctionDefinition(int parameters, Func<ExpressionFactory<T>, string[], IExpression<T>> getExpression)
        {
            Parameters = parameters;
            _getExpression = getExpression;
        }

        public int Parameters { get; private set; }
        public IExpression<T> GetExpression(ExpressionFactory<T> factory, string[] parameters)
        {
            return _getExpression(factory, parameters);
        }
    }

}
