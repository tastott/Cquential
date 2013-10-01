using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Tim.Cquential.Language.Tokens
{
    using Utilities;

    public class Tokenizer : ITokenizer
    {
        private static Regex WordSplitter = new Regex(@"\s+");
        private static Regex AggregateFunctionPattern = new Regex(@"^(?<function>[A-Za-z]+)\((?<member>[A-Za-z]+)\)");
        private static Regex LegReferencePattern = new Regex(@"^((\[[^\]]+\])|([^\[\]\.]+))\.[A-Za-z]+");
        private static Regex FunctionPattern = new Regex(@"^[A-Za-z]+(?=\()");
        private static Regex BooleanOperator = new Regex(@"^(AND|OR)(?!\w)");
        private static char[] NumericOperators = new char[] { '+', '-', '/', '%', '*'};
        private static string[] ComparisonOperators = new string[] {"<=", ">=", ">", "<", "=", "!="  };
        
        public IEnumerable<Token> Tokenize(string input)
        {
            var tokens = new List<Token>();
            var words = WordSplitter.Split(input);

            foreach (var word in words)
            {
                double value;
                Match match;
                string remainingWord = word;

                while (!String.IsNullOrEmpty(remainingWord))
                {
                    if (remainingWord.StartsWith("("))
                    {
                        tokens.Add(new Token(TokenType.LeftParenthesis, "("));
                        remainingWord = remainingWord.Substring(1);
                    }
                    else if (remainingWord.StartsWith(")"))
                    {
                        tokens.Add(new Token(TokenType.RightParenthesis, ")"));
                        remainingWord = remainingWord.Substring(1);
                    }
                    else if (double.TryParse(remainingWord, out value))
                    {
                        tokens.Add(new Token(TokenType.Constant, word));
                        remainingWord = "";
                    }
                    else if (AggregateFunctionPattern.TryMatch(remainingWord, out match))
                    {
                        tokens.Add(new Token(TokenType.Function, match.Groups["function"].Value));
                        tokens.Add(new Token(TokenType.LeftParenthesis, "("));
                        tokens.Add(new Token(TokenType.SingleItemMember, match.Groups["member"].Value));
                        tokens.Add(new Token(TokenType.RightParenthesis, ")"));
                        remainingWord = AggregateFunctionPattern.Replace(remainingWord, "", 1);
                    }
                    else if (FunctionPattern.TryMatch(remainingWord, out match))
                    {
                        tokens.Add(new Token(TokenType.Function, match.Value));
                        remainingWord = FunctionPattern.Replace(remainingWord, "", 1);
                    }
                    //else if (AggregatePattern.TryMatch(remainingWord, out match))
                    //{
                    //    token = new Token(TokenType.Aggregate, match.Value);
                    //    remainingWord = AggregatePattern.Replace(remainingWord, "", 1);
                    //}
                    else if (LegReferencePattern.TryMatch(remainingWord, out match))
                    {
                        tokens.Add(new Token(TokenType.SingleItemMember, match.Value));
                        remainingWord = LegReferencePattern.Replace(remainingWord, "", 1);
                    }
                    else if (BooleanOperator.TryMatch(remainingWord, out match))
                    {
                        tokens.Add(new Token(TokenType.Operator, match.Value));
                        remainingWord = BooleanOperator.Replace(remainingWord, "", 1);
                    }
                    else if (NumericOperators.Contains(remainingWord[0]))
                    {
                        tokens.Add(new Token(TokenType.Operator, remainingWord.Substring(0, 1)));
                        remainingWord = remainingWord.Substring(1);
                    }
                    else if (ComparisonOperators.Any(co => remainingWord.StartsWith(co)))
                    {
                        string op = ComparisonOperators.First(co => remainingWord.StartsWith(co));

                        tokens.Add(new Token(TokenType.Operator, op));
                        remainingWord = remainingWord.Substring(op.Length);
                    }
                    else throw new Exception(String.Format("Unrecognised token in input: '{0}'", word));
                }                
            }

            return tokens;
        }
    }
}
