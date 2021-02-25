using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

using ACE.Database.Models.Shard;
using ACE.Entity;

namespace ACE.Database
{
    public class ShardDatabaseWithCaching : ShardDatabase
    {
        public TimeSpan PlayerBiotaRetentionTime { get; set; }
        public TimeSpan NonPlayerBiotaRetentionTime { get; set; }

        public ShardDatabaseWithCaching(TimeSpan playerBiotaRetentionTime, TimeSpan nonPlayerBiotaRetentionTime)
        {
            PlayerBiotaRetentionTime = playerBiotaRetentionTime;
            NonPlayerBiotaRetentionTime = nonPlayerBiotaRetentionTime;
        }


        private class CacheObject<T>
        {
            public DateTime LastSeen;
            public ShardDbContext Context;
            public T CachedObject;
        }

        private readonly object biotaCacheMutex = new object();

        private readonly Dictionary<uint, CacheObject<Biota>> biotaCache = new Dictionary<uint, CacheObject<Biota>>();

        private static readonly TimeSpan MaintenanceInterval = TimeSpan.FromMinutes(1);

        private DateTime lastMaintenanceInterval;

        /// <summary>
        /// Make sure this is called from within a lock(biotaCacheMutex)
        /// </summary>
        private void TryPerformMaintenance()
        {
            if (lastMaintenanceInterval + MaintenanceInterval > DateTime.UtcNow)
                return;

            var removals = new Collection<uint>();

            foreach (var kvp in biotaCache)
            {
                if (ObjectGuid.IsPlayer(kvp.Key))
                {
                    if (kvp.Value.LastSeen + PlayerBiotaRetentionTime < DateTime.UtcNow)
                        removals.Add(kvp.Key);
                }
                else
                {
                    if (kvp.Value.LastSeen + NonPlayerBiotaRetentionTime < DateTime.UtcNow)
                        removals.Add(kvp.Key);
                }
            }

            foreach (var removal in removals)
                biotaCache.Remove(removal);

            lastMaintenanceInterval = DateTime.UtcNow;
        }

        private void TryAddToCache(ShardDbContext context, Biota biota)
        {
            lock (biotaCacheMutex)
            {
                if (ObjectGuid.IsPlayer(biota.Id))
                {
                    if (PlayerBiotaRetentionTime > TimeSpan.Zero)
                        biotaCache[biota.Id] = new CacheObject<Biota> {LastSeen = DateTime.UtcNow, Context = context, CachedObject = biota};
                }
                else if (NonPlayerBiotaRetentionTime > TimeSpan.Zero)
                    biotaCache[biota.Id] = new CacheObject<Biota> {LastSeen = DateTime.UtcNow, Context = context, CachedObject = biota};
            }
        }

        public List<uint> GetBiotaCacheKeys()
        {
            lock (biotaCacheMutex)
                return biotaCache.Keys.ToList();
        }


        public override Biota GetBiota(ShardDbContext context, uint id, bool doNotAddToCache = false)
        {
            lock (biotaCacheMutex)
            {
                TryPerformMaintenance();

                if (biotaCache.TryGetValue(id, out var cachedBiota))
                {
                    cachedBiota.LastSeen = DateTime.UtcNow;

                    return cachedBiota.CachedObject;
                }
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
            CacheObject<Biota> cachedBiota;

            lock (biotaCacheMutex)
                biotaCache.TryGetValue(biota.Id, out cachedBiota);

            if (cachedBiota != null)
            {
                cachedBiota.LastSeen = DateTime.UtcNow;

                rwLock.EnterReadLock();
                try
                {
                    ACE.Database.Adapter.BiotaUpdater.UpdateDatabaseBiota(cachedBiota.Context, biota, cachedBiota.CachedObject);
                }
                finally
                {
                    rwLock.ExitReadLock();
                }

                return DoSaveBiota(cachedBiota.Context, cachedBiota.CachedObject);
            }

            // Biota does not exist in the cache

            var context = new ShardDbContext();

            var existingBiota = base.GetBiota(context, biota.Id);

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
            lock (biotaCacheMutex)
                biotaCache.Remove(id);

            return base.RemoveBiota(id);
        }
    }
}
