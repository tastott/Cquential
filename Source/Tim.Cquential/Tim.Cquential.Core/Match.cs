using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.Cquential.Core
{
    public class Match<T>
    {
        private int _firstIndex;
        private int _lastIndex;
        private T[] _wholeSequence;

        public Match(int firstIndex, int lastIndex, T[] wholeSequence)
        {
            _firstIndex = firstIndex;
            _lastIndex = lastIndex;
            _wholeSequence = wholeSequence;
        }

        public IEnumerable<T> Sequence
        {
            get
            {
                int matchLength = _lastIndex - _firstIndex + 1;
                T[] sequence =new T[matchLength];
                Array.Copy(_wholeSequence, _firstIndex, sequence, 0, matchLength);

                return sequence;
            }
        }
    }
}
