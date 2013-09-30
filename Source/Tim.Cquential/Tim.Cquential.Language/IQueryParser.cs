using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tim.Cquential.Language
{
    using Core;

    public interface IQueryParser<T>
    {
        IQuery<T> Parse(IEnumerable<Token> rpnTokens);
        IQuery<T> Parse(string queryString);
    }
}
