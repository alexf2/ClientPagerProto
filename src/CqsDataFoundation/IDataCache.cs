using System;

namespace CqsDataFoundation
{
    public interface IDataCache
    {
        T Get<T>(string key);
        void Set<T>(string key, T val);
        void Set<T>(string key, T val, TimeSpan expiration, bool absolute = false);
        void Remove(string key);
        bool Contains(string key);
    }
}
