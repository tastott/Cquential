using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.RideAnalysis.Utilities
{
    public static class ArrayExtensions
    {
        public static T[] Coarsen<T>(this T[] source, int coarseness)
        {
            T[] destination = new T[source.Length / coarseness];

            for (int i = 0; i < destination.Length; i++)
            {
                destination[i] = source[i * coarseness];
            }

            return destination;
        }

        public static IEnumerable<T> Coarsen<T>(this IEnumerable<T> source, int coarseness)
        {
            return source.Where((x,i) => i % coarseness == 0);
        }
    }
}
