using System.Collections.Generic;

namespace Dws.Challenge.Infrastructure.Extensions
{
    public static class DictionaryExtensions
    {
        public static void Upsert<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }
        }
    }
}