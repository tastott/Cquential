using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tim.Cquential.Language
{
    public interface ITokenizer
    {
        IEnumerable<Token> Tokenize(string input);
    }
}
