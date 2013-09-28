using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.Cquential.Core
{
    public class MatchStatus
    {
        public MatchStatus(bool isMatch, bool isMutable)
        {
            IsMatch = isMatch;
            IsMutable = isMutable;
        }

        public bool IsMatch { get; private set; }
        public bool IsMutable { get; private set; }
    }
}
