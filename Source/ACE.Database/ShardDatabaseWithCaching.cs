using System;
using System.Collections.Concurrent;
using System.Threading;

using log4net;

using ACE.Database.Models.Shard;
using ACE.Entity;

namespace ACE.Database
{
    public class ShardDatabaseWithCaching : ShardDatabase
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public TimeSpan CharacterRetentionTime { get; set; }
        public TimeSpan PlayerBiotaRetentionTime { get; set; }
        public TimeSpan NonPlayerBiotaRetentionTime { get; set; }

        public ShardDatabaseWithCaching(TimeSpan characterRetentionTime, TimeSpan playerBiotaRetentionTime, TimeSpan nonPlayerBiotaRetentionTime)
        {
            CharacterRetentionTime = characterRetentionTime;
            PlayerBiotaRetentionTime = playerBiotaRetentionTime;
            NonPlayerBiotaRetentionTime = nonPlayerBiotaRetentionTime;
        }


        private class CacheObject<T>
        {
            public DateTime LastSeen;
            public ShardDbContext Context;
            public T CachedObject;
        }

        private readonly ConcurrentDictionary<uint, CacheObject<Character>> characterCache = new ConcurrentDictionary<uint, CacheObject<Character>>();

        private readonly ConcurrentDictionary<uint, CacheObject<Biota>> biotaCache = new ConcurrentDictionary<uint, CacheObject<Biota>>();

        private void TryAddToCache(ShardDbContext context, Biota biota)
        {
            if (ObjectGuid.IsPlayer(biota.Id))
            {
                if (PlayerBiotaRetentionTime > TimeSpan.Zero)
                    biotaCache[biota.Id] = new CacheObject<Biota> { LastSeen = DateTime.UtcNow, Context = context, CachedObject = biota };
            }
            else if (NonPlayerBiotaRetentionTime > TimeSpan.Zero)
                biotaCache[biota.Id] = new CacheObject<Biota> { LastSeen = DateTime.UtcNow, Context = context, CachedObject = biota };
        }


        public override Biota GetBiota(ShardDbContext context, uint id)
        {
            if (biotaCache.TryGetValue(id, out var value))
            {
                value.LastSeen = DateTime.UtcNow;

                return value.CachedObject;
            }

            var biota = base.GetBiota(context, id);

            if (biota != null)
                TryAddToCache(context, biota);

            return biota;
        }

        public override Biota GetBiota(uint id)
        {
            if (ObjectGuid.IsPlayer(id))
            {
                if (PlayerBiotaRetentionTime > TimeSpan.Zero)
                {
                    var context = new ShardDbContext();

                    var biota = GetBiota(context, id); // This will add the result into the caches

                    return biota;
                }
            }
            else if (NonPlayerBiotaRetentionTime > TimeSpan.Zero)
            {
                var context = new ShardDbContext();

                var biota = GetBiota(context, id); // This will add the result into the caches

                return biota;
            }

            return base.GetBiota(id);
        }

        public override bool SaveBiota(ACE.Entity.Models.Biota biota, ReaderWriterLockSlim rwLock)
        {
            if (biotaCache.TryGetValue(biota.Id, out var value))
            {
                value.LastSeen = DateTime.UtcNow;

                rwLock.EnterReadLock();
                try
                {
                    ACE.Database.Adapter.BiotaUpdater.UpdateBiota(value.Context, biota, value.CachedObject);
                }
                finally
                {
                    rwLock.ExitReadLock();
                }

                return DoSaveBiota(value.Context, value.CachedObject);
            }

            // Biota does not exist in the cache

            var context = new ShardDbContext();

            var existingBiota = GetBiota(context, biota.Id);

            rwLock.EnterReadLock();
            try
            {
                if (existingBiota == null)
                {
                    existingBiota = ACE.Database.Adapter.BiotaConverter.ConvertFromEntityBiota(biota);

                    context.Biota.Add(existingBiota);
                }
                else
                {
                    ACE.Database.Adapter.BiotaUpdater.UpdateBiota(context, biota, existingBiota);
                }
            }
            finally
            {
                rwLock.ExitReadLock();
            }

            if (DoSaveBiota(context, existingBiota))
            {
                TryAddToCache(context, existingBiota);

                return true;
            }

            return false;
        }

        public override bool RemoveBiota(uint id)
        {
            biotaCache.TryRemove(id, out _);

            return base.RemoveBiota(id);
        }
    }
}
