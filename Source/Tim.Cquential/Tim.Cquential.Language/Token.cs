using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tim.Cquential.Language
{
    public enum TokenType
    {
        Aggregate,
        Operator,
        Constant,
        LeftParenthesis,
        RightParenthesis,
        Function
    }

    public class Token
    {
        public TokenType Type { get; set; }
        public string Value { get; set; }

        public Token(TokenType type, string value)
        {
            Type = type;
            Value = value;
        }
    }
}
