using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using ACE.Common;
using ACE.Entity;
using ACE.Network;

using log4net;
using ACE.Database;
using System.Collections.Concurrent;

namespace ACE.Managers
{
    /// <summary>
    /// FIXME(ddevec): Should be integrated into ShardDatabase...
    /// </summary>
    public static class DbManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static BlockingCollection<Tuple<Task<bool>, ObjectGuid, long>> saveObjects = new BlockingCollection<Tuple<Task<bool>, ObjectGuid, long>>();
        private static Thread taskCleanThread;

        private static long cacheVersion = 0;
        // NOTE: long in tuple (and cacheVersion) are there so if an item is queued for save multiple times, we don't remove it from the cache prematurely
        private static Dictionary<ObjectGuid, Tuple<AceObject, long>> saveCache = new Dictionary<ObjectGuid, Tuple<AceObject, long>>();
        private static object cacheLock = new object();

        public static void Initialize()
        {
            // starts game loop.
            taskCleanThread = new Thread(Tick);
            taskCleanThread.Start();
        }

        /// <summary>
        /// Saves AceObject to database, does not pause execution of main game thread.
        /// </summary>
        /// <param name="ao"></param>
        public static Task<bool> SaveShardObject(AceObject ao)
        {
            long saveVersion;
            lock (cacheLock)
            {
                ObjectGuid key = new ObjectGuid(ao.AceObjectId);
                var valueTuple = new Tuple<AceObject, long>(ao, cacheVersion);
                saveVersion = cacheVersion;
                cacheVersion++;

                if (saveCache.ContainsKey(key))
                {
                    saveCache[key] = valueTuple;
                }
                else
                {
                    saveCache.Add(key, valueTuple);
                }
            }
            Task<bool> ret = new Task<bool>(() => DatabaseManager.Shard.SaveObject(ao));
            saveObjects.Add(new Tuple<Task<bool>, ObjectGuid, long>(ret, new ObjectGuid(ao.AceObjectId), saveVersion));
            return ret;
        }

        public static AceCharacter LoadShardCharacter(ObjectGuid guid)
        {
            // First, see if the item is in the cache
            AceCharacter ret = null;
            lock (cacheLock)
            {
                if (saveCache.ContainsKey(guid))
                {
                    ret = saveCache[guid].Item1 as AceCharacter;
                    if (ret == null) {
                        log.Error($"Guid: {guid} attempting to be loaded as character, but saved as object");
                    }
                }
            }

            if (ret == null)
            {
                return DatabaseManager.Shard.GetCharacter(guid.Full);
            }
            else
            {
                return ret;
            }
        }

        public static AceObject LoadShardObject(ObjectGuid guid)
        {
            AceObject ret = null;
            lock (cacheLock)
            {
                if (saveCache.ContainsKey(guid))
                {
                    ret = saveCache[guid].Item1 as AceObject;
                }
            }

            if (ret == null)
            {
                return DatabaseManager.Shard.GetObject(guid.Full);
            }
            else
            {
                return ret;
            }
        }

        /// <summary>
        /// Shutdowns DB Saver Manager Safely
        /// </summary>
        public static void ShutDown()
        {
            saveObjects.CompleteAdding();

            // Wait for all the tasks to finish
            taskCleanThread.Join();
        }

        private static void Tick()
        {
            // Stops us from shutting down before all the saves are done
            while (!saveObjects.IsCompleted)
            {
                var saveTuple = saveObjects.Take();
                Task<bool> saveTask = saveTuple.Item1;
                ObjectGuid guid = saveTuple.Item2;
                long version = saveTuple.Item3;

                saveTask.Start();
                saveTask.Wait();

                lock (cacheLock)
                {
                    // NOTE(ddevec): The item MUST exist in the cache if we just ran it, no need to check key
                    var cacheTup = saveCache[guid];
                    long cachedVersion = cacheTup.Item2;
                    if (cachedVersion == version)
                    {
                        saveCache.Remove(guid);
                    }
                }
            }
        }
    }
}