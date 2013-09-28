using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tim.Cquential.Language
{
    using Core;

    public interface IQueryParser
    {
        IQuery<T> Parse<T>(IEnumerable<Token> rpnTokens);
        IQuery<T> Parse<T>(string queryString);
    }
}
