using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ACE.Server.WebApi
{
    public sealed class GUIDToUserIdCache
    {
        private static readonly Lazy<GUIDToUserIdCache> lazy =
            new Lazy<GUIDToUserIdCache>(() => new GUIDToUserIdCache());

        public static GUIDToUserIdCache Instance => lazy.Value;
        private ConcurrentDictionary<Guid, uint> Cache;
        private GUIDToUserIdCache() { Cache = new ConcurrentDictionary<Guid, uint>(); }
        public static uint? GetUserId(Guid guid)
        {
            ConcurrentDictionary<Guid, uint> cache = Instance.Cache;
            return cache.GetValueOrDefault(guid);
        }
        public static void SetUserId(Guid guid, uint userId)
        {
            ConcurrentDictionary<Guid, uint> cache = Instance.Cache;
            cache[guid] = userId;
        }
    }
}
