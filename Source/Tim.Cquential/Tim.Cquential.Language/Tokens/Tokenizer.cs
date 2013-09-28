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
        private static Regex LegReferencePattern = new Regex(@"^\[[^\]]+\]\.[A-Za-z]+");
        private static Regex FunctionPattern = new Regex(@"^[A-Za-z]+(?=\()");
        private static Regex AggregatePattern = new Regex(@"^[A-Za-z]+\(\[\*\]\.[A-Za-z]+\)");
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
                Token token = null;
                string remainingWord = word;

                while (!String.IsNullOrEmpty(remainingWord))
                {
                    if (remainingWord.StartsWith("("))
                    {
                        token = new Token(TokenType.LeftParenthesis, "(");
                        remainingWord = remainingWord.Substring(1);
                    }
                    else if (remainingWord.StartsWith(")"))
                    {
                        token = new Token(TokenType.RightParenthesis, ")");
                        remainingWord = remainingWord.Substring(1);
                    }
                    else if (double.TryParse(remainingWord, out value))
                    {
                        token = new Token(TokenType.Constant, word);
                        remainingWord = "";
                    }
                    else if (FunctionPattern.TryMatch(remainingWord, out match))
                    {
                        token = new Token(TokenType.Function, match.Value);
                        remainingWord = FunctionPattern.Replace(remainingWord, "", 1);
                    }
                    //else if (AggregatePattern.TryMatch(remainingWord, out match))
                    //{
                    //    token = new Token(TokenType.Aggregate, match.Value);
                    //    remainingWord = AggregatePattern.Replace(remainingWord, "", 1);
                    //}
                    else if (LegReferencePattern.TryMatch(remainingWord, out match))
                    {
                        token = new Token(TokenType.Aggregate, match.Value);
                        remainingWord = LegReferencePattern.Replace(remainingWord, "", 1);
                    }
                    else if (BooleanOperator.TryMatch(remainingWord, out match))
                    {
                        token = new Token(TokenType.Operator, match.Value);
                        remainingWord = BooleanOperator.Replace(remainingWord, "", 1);
                    }
                    else if (NumericOperators.Contains(remainingWord[0]))
                    {
                        token = new Token(TokenType.Operator, remainingWord.Substring(0, 1));
                        remainingWord = remainingWord.Substring(1);
                    }
                    else if (ComparisonOperators.Any(co => remainingWord.StartsWith(co)))
                    {
                        string op = ComparisonOperators.First(co => remainingWord.StartsWith(co));

                        token = new Token(TokenType.Operator, op);
                        remainingWord = remainingWord.Substring(op.Length);
                    }
                    else throw new Exception(String.Format("Unrecognised token in input: '{0}'", word));

                    tokens.Add(token);
                }                
            }

            return tokens;
        }
    }
}
