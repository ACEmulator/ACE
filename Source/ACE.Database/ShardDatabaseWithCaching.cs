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


        public TimeSpan PlayerBiotaRetentionTime { get; set; }
        public TimeSpan NonPlayerBiotaRetentionTime { get; set; }

        public ShardDatabaseWithCaching(TimeSpan playerBiotaRetentionTime, TimeSpan nonPlayerBiotaRetentionTime)
        {
            PlayerBiotaRetentionTime = playerBiotaRetentionTime;
            NonPlayerBiotaRetentionTime = nonPlayerBiotaRetentionTime;
        }


        public class CacheObject<T>
        {
            public DateTime LastSeen;
            public ShardDbContext Context;
            public T CachedObject;
        }

        public readonly ConcurrentDictionary<uint, CacheObject<Biota>> BiotaCache = new ConcurrentDictionary<uint, CacheObject<Biota>>();

        private static readonly TimeSpan MaintenanceInterval = TimeSpan.FromMinutes(1);

        private DateTime lastMaintenanceInterval;

        private void TryPerformMaintenance()
        {
            if (lastMaintenanceInterval + MaintenanceInterval > DateTime.UtcNow)
                return;

            foreach (var kvp in BiotaCache)
            {
                if (ObjectGuid.IsPlayer(kvp.Key))
                {
                    if (kvp.Value.LastSeen + PlayerBiotaRetentionTime < DateTime.UtcNow)
                        BiotaCache.TryRemove(kvp.Key, out _);
                }
                else
                {
                    if (kvp.Value.LastSeen + NonPlayerBiotaRetentionTime < DateTime.UtcNow)
                        BiotaCache.TryRemove(kvp.Key, out _);
                }
            }

            lastMaintenanceInterval = DateTime.UtcNow;
        }

        private void TryAddToCache(ShardDbContext context, Biota biota)
        {
            if (ObjectGuid.IsPlayer(biota.Id))
            {
                if (PlayerBiotaRetentionTime > TimeSpan.Zero)
                    BiotaCache[biota.Id] = new CacheObject<Biota> { LastSeen = DateTime.UtcNow, Context = context, CachedObject = biota };
            }
            else if (NonPlayerBiotaRetentionTime > TimeSpan.Zero)
                BiotaCache[biota.Id] = new CacheObject<Biota> { LastSeen = DateTime.UtcNow, Context = context, CachedObject = biota };
        }


        public override Biota GetBiota(ShardDbContext context, uint id, bool doNotAddToCache = false)
        {
            TryPerformMaintenance();

            if (BiotaCache.TryGetValue(id, out var value))
            {
                value.LastSeen = DateTime.UtcNow;

                return value.CachedObject;
            }

            var biota = base.GetBiota(context, id);

            if (biota != null && !doNotAddToCache)
                TryAddToCache(context, biota);

            return biota;
        }

        public override Biota GetBiota(uint id, bool doNotAddToCache = false)
        {
            if (ObjectGuid.IsPlayer(id))
            {
                if (PlayerBiotaRetentionTime > TimeSpan.Zero)
                {
                    var context = new ShardDbContext();

                    var biota = GetBiota(context, id, doNotAddToCache); // This will add the result into the caches

                    return biota;
                }
            }
            else if (NonPlayerBiotaRetentionTime > TimeSpan.Zero)
            {
                var context = new ShardDbContext();

                var biota = GetBiota(context, id, doNotAddToCache); // This will add the result into the caches

                return biota;
            }

            return base.GetBiota(id, doNotAddToCache);
        }

        public override bool SaveBiota(ACE.Entity.Models.Biota biota, ReaderWriterLockSlim rwLock)
        {
            if (BiotaCache.TryGetValue(biota.Id, out var value))
            {
                value.LastSeen = DateTime.UtcNow;

                rwLock.EnterReadLock();
                try
                {
                    ACE.Database.Adapter.BiotaUpdater.UpdateDatabaseBiota(value.Context, biota, value.CachedObject);
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
                    ACE.Database.Adapter.BiotaUpdater.UpdateDatabaseBiota(context, biota, existingBiota);
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
            BiotaCache.TryRemove(id, out _);

            return base.RemoveBiota(id);
        }
    }
}
