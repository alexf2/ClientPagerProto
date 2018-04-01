using System;
using System.Runtime.Caching;

namespace CqsDataFoundation
{
    public sealed class MemCache : IDataCache
    {
        readonly MemoryCache _cache;
        readonly CacheItemPolicy _defPolicy;

        public MemCache(string name)
        {
            _cache = new MemoryCache(name);
            _defPolicy = new CacheItemPolicy() { SlidingExpiration = new TimeSpan(0, 10, 0) };
        }

        #region IDataCache
        public T Get<T>(string key)
        {
            return (T)_cache.Get(key);
        }

        public void Set<T>(string key, T val)
        {
            _cache.Set(key, val, _defPolicy);
        }

        public void Set<T>(string key, T val, TimeSpan expiration, bool absolute = false)
        {
            var policy = new CacheItemPolicy();

            if (absolute)
                policy.AbsoluteExpiration = DateTimeOffset.Now.Add(expiration);
            else
                policy.SlidingExpiration = expiration;

            _cache.Set(key, val, policy);
        }

        public void Remove(string key)
        {
            if (_cache.Contains(key))
                _cache.Remove(key);
        }

        public bool Contains(string key)
        {
            return _cache.Contains(key);
        }
        #endregion IDataCache
    }

}
