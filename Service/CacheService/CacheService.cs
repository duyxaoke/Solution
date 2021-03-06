﻿using System;
using System.Runtime.Caching;

namespace Service.CacheService
{
    public interface ICacheProviderService : IDisposable
    {
        object Get(string key);
        void Set(string key, object data);
        bool IsSet(string key);
        void Invalidate(string key);
    }
    public class CacheProviderService : ICacheProviderService
    {
        private static ObjectCache Cache => MemoryCache.Default;

        public object Get(string key)
        {
            return Cache[key];
        }

        public void Set(string key, object data)
        {
            CacheItemPolicy policy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(15)
            };
            Cache.Add(new CacheItem(key, data), policy);
        }

        public bool IsSet(string key)
        {
            return (Cache[key] != null);
        }

        public void Invalidate(string key)
        {
            Cache.Remove(key);
        }

        public void Dispose()
        {
           
        }
    }
}
