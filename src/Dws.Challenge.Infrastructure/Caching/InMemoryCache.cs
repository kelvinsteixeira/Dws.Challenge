using System.Collections.Generic;
using Dws.Challenge.Infrastructure.Caching.Interfaces;
using Dws.Challenge.Infrastructure.Extensions;

namespace Dws.Challenge.Infrastructure.Caching
{
    public class InMemoryCache : ICache
    {
        private readonly Dictionary<string, object> memory = new Dictionary<string, object>();

        public bool ContainsKey(string key) => this.memory.ContainsKey(key);

        public T Get<T>(string key)
        {
            if (memory.TryGetValue(key, out object val))
            {
                return (T)val;
            }

            return default;
        }

        public void Save<T>(string key, T value)
        {
            this.memory.Upsert(key, value);
        }
    }
}