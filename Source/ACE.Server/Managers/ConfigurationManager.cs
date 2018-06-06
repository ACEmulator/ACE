using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using ACE.Database;
using System.Linq;
using log4net;

namespace ACE.Server.Managers
{
    public static class ConfigurationManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // caching internally to the server
        private static ConcurrentDictionary<string, ConfigurationEntry<bool>> CachedBooleanSettings = new ConcurrentDictionary<string, ConfigurationEntry<bool>>();
        private static ConcurrentDictionary<string, ConfigurationEntry<int>> CachedIntegerSettings = new ConcurrentDictionary<string, ConfigurationEntry<int>>();
        private static ConcurrentDictionary<string, ConfigurationEntry<float>> CachedFloatSettings = new ConcurrentDictionary<string, ConfigurationEntry<float>>();
        private static ConcurrentDictionary<string, ConfigurationEntry<string>> CachedStringSettings = new ConcurrentDictionary<string, ConfigurationEntry<string>>();

        private static Timer _workerThread;

        public static void StartUpdating()
        {
            _workerThread = new Timer(30000);
            _workerThread.Elapsed += DoWork;
        }

        public static void StopUpdating()
        {
            _workerThread.Stop();
        }

        public static bool GetBool(string key, bool fallback = false)
        {
            // first, check the cache. If the key exists in the cache, grab it regardless of it's modified value
            // then, check the database. if the key exists in the database, grab it and cache it
            // finally, set it to a default of false.
            if (CachedBooleanSettings.ContainsKey(key))
            {
                return CachedBooleanSettings[key].Item;
            }

            var dbValue = DatabaseManager.ServerConfig.GetBool(key);

            var boolVal = dbValue?.Value ?? fallback;
            CachedBooleanSettings[key] = new ConfigurationEntry<bool>(false, boolVal);
            return boolVal;
        }

        public static void ModifyBool(string key, bool newVal)
        {
            // modify and flag the cache value for the next update.
            CachedBooleanSettings[key] = new ConfigurationEntry<bool>(true, newVal);
        }

        public static int GetInt(string key, int fallback = 0)
        {
            if (CachedIntegerSettings.ContainsKey(key))
            {
                return CachedIntegerSettings[key].Item;
            }

            var dbValue = DatabaseManager.ServerConfig.GetInt(key);

            var intVal = dbValue?.Value ?? fallback;
            CachedIntegerSettings[key] = new ConfigurationEntry<int>(false, intVal);
            return intVal;
        }

        public static void ModifyInt(string key, int newVal)
        {
            CachedIntegerSettings[key] = new ConfigurationEntry<int>(true, newVal);
        }

        public static float GetFloat(string key, float fallback = 0.0f)
        {
            if (CachedFloatSettings.ContainsKey(key))
            {
                return CachedFloatSettings[key].Item;
            }

            var dbValue = DatabaseManager.ServerConfig.GetFloat(key);

            var floatVal = dbValue?.Value ?? fallback;
            CachedFloatSettings[key] = new ConfigurationEntry<float>(false, floatVal);
            return floatVal;
        }

        public static void ModifyFloat(string key, float newVal)
        {
            CachedFloatSettings[key] = new ConfigurationEntry<float>(true, newVal);
        }

        public static string GetString(string key, string fallback = "")
        {
            if (CachedStringSettings.ContainsKey(key))
            {
                return CachedStringSettings[key].Item;
            }

            var dbValue = DatabaseManager.ServerConfig.GetString(key);

            var stringVal = dbValue?.Value ?? fallback;
            CachedStringSettings[key] = new ConfigurationEntry<string>(false, stringVal);
            return stringVal;
        }

        public static void ModifyString(string key, string newVal)
        {
            CachedStringSettings[key] = new ConfigurationEntry<string>(true, newVal);
        }

        private static void DoWork(Object source, System.Timers.ElapsedEventArgs e)
        {
            // first, check for variables updated on the server-side. Write those to the DB.
            // then, compare variables to DB and update from DB as necessary. (needs to minimize r/w to DB)

            // we start with boolean
            log.Debug("Beginning to write modified boolean properties into database");
            foreach (var i in CachedBooleanSettings.Where(r => r.Value.Modified == true))
            {
                // todo: figure out how to handle situations where the value is new in the cache and not in the DB
                // right now, this will update the boolean but if the value doesn't exist the query won't work.
                // solutions: call exists before modify/add, have modify fallback to add if possible at the EF layer
                DatabaseManager.ServerConfig.ModifyBool(new Database.Models.Config.BoolStat { Key = i.Key, Value = i.Value.Item });
            }

            // int next
            log.Debug("Beginning to write modified integer properties into database");
            foreach (var i in CachedIntegerSettings.Where(r => r.Value.Modified == true))
            {
                // todo: see boolean section for caveat in this approach
                DatabaseManager.ServerConfig.ModifyInt(new Database.Models.Config.IntegerStat { Key = i.Key, Value = i.Value.Item });
            }

            // float next
            log.Debug("Beginning to write modified float properties into database");
            foreach (var i in CachedFloatSettings.Where(r => r.Value.Modified == true))
            {
                // todo: see boolean section for caveat in this approach
                DatabaseManager.ServerConfig.ModifyFloat(new Database.Models.Config.FloatStat { Key = i.Key, Value = i.Value.Item });
            }

            // finally, string
            log.Debug("Beginning to write modified string properties into database");
            foreach (var i in CachedStringSettings.Where(r => r.Value.Modified == true))
            {
                // todo: see boolean section for caveat in this approach
                DatabaseManager.ServerConfig.ModifyString(new Database.Models.Config.StringStat { Key = i.Key, Value = i.Value.Item });
            }

            // next, we need to fetch all of the variables from the DB and compare them quickly.
            log.Debug("Fetching boolean properties from database");
            foreach (var i in DatabaseManager.ServerConfig.GetAllBools())
            {
                // note: we don't have any tracking of what we placed in the database from the cache.
                // this draws everything from the database, which is unnecessary for the items that are pulled from cache to db
                // todo: ignore items we updated in the previous
                CachedBooleanSettings[i.Key] = new ConfigurationEntry<bool>(false, i.Value ?? false);
            }

            log.Debug("Fetching integer properties from database");
            foreach (var i in DatabaseManager.ServerConfig.GetAllInts())
                // todo: see boolean caveat
                CachedIntegerSettings[i.Key] = new ConfigurationEntry<int>(false, i.Value ?? 0);

            log.Debug("Fetching float properties from database");
            foreach (var i in DatabaseManager.ServerConfig.GetAllFloats())
                // todo: see boolean caveat
                CachedFloatSettings[i.Key] = new ConfigurationEntry<float>(false, i.Value ?? 0.0f);

            log.Debug("Fetching string properties from database");
            foreach (var i in DatabaseManager.ServerConfig.GetAllStrings())
                // todo: see boolean caveat
                CachedStringSettings[i.Key] = new ConfigurationEntry<string>(false, i.Value ?? "");
        }

    }

    class ConfigurationEntry<T>
    {
        public bool Modified;
        public T Item;

        public ConfigurationEntry(bool modified, T item)
        {
            this.Modified = modified;
            this.Item = item;
        }
    }
}
