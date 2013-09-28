using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tim.Cquential.Language
{
    /// <summary>
    /// Converts a sequence of tokens in infix notation to a sequence in postfix notation.
    /// </summary>
    public interface ITokenShunter
    {
        IEnumerable<Token> Shunt(IEnumerable<Token> infixTokens);
    }
}
