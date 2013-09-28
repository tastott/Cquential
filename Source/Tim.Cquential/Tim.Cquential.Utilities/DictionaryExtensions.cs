using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tim.Cquential.Utilities
{
    public static class DictionaryExtensions
    {
        public static void AddMany<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IEnumerable<TKey> keys, TValue value)
        {
            foreach (var key in keys)
            {
                dictionary.Add(key, value);
            }
        }
    }
}
