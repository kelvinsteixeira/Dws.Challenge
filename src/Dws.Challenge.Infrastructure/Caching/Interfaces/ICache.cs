namespace Dws.Challenge.Infrastructure.Caching.Interfaces
{
    public interface ICache
    {
        void Save<T>(string key, T value);

        T Get<T>(string key);

        bool ContainsKey(string key);
    }
}