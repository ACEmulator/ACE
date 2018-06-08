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
    public static class PropertyManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // caching internally to the server
        private static ConcurrentDictionary<string, ConfigurationEntry<bool>> CachedBooleanSettings = new ConcurrentDictionary<string, ConfigurationEntry<bool>>();
        private static ConcurrentDictionary<string, ConfigurationEntry<int>> CachedIntegerSettings = new ConcurrentDictionary<string, ConfigurationEntry<int>>();
        private static ConcurrentDictionary<string, ConfigurationEntry<float>> CachedFloatSettings = new ConcurrentDictionary<string, ConfigurationEntry<float>>();
        private static ConcurrentDictionary<string, ConfigurationEntry<string>> CachedStringSettings = new ConcurrentDictionary<string, ConfigurationEntry<string>>();

        private static Timer _workerThread;

        /// <summary>
        /// Initializes the PropertyManager.
        /// Run this only once per server instance.
        /// </summary>
        public static void Initialize()
        {
            LoadPropertiesFromDB();
            _workerThread = new Timer(300000);
            _workerThread.Elapsed += DoWork;
            _workerThread.AutoReset = true;
            _workerThread.Start();
        }

        /// <summary>
        /// Stops updating the cached store from the database.
        /// </summary>
        public static void StopUpdating()
        {
            _workerThread.Stop();
        }

        /// <summary>
        /// Retrieves a boolean property from the cache or database
        /// </summary>
        /// <param name="key">The string key for the property</param>
        /// <param name="fallback">The value to return if the property cannot be found. This will be cached as well.</param>
        /// <returns>A boolean value representing the property</returns>
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
            var isModified = false;

            if (dbValue == null || dbValue?.Value == null)
            {
                isModified = true;
            }

            var boolVal = dbValue?.Value ?? fallback;

            CachedBooleanSettings[key] = new ConfigurationEntry<bool>(isModified, boolVal);
            return boolVal;
        }

        /// <summary>
        /// Modifies a boolean value in the cache and marks it for being synced on the next cycle.
        /// </summary>
        /// <param name="key">The string key for the property</param>
        /// <param name="newVal">The value to replace the old value with</param>
        public static void ModifyBool(string key, bool newVal)
        {
            // modify and flag the cache value for the next update.
            CachedBooleanSettings[key] = new ConfigurationEntry<bool>(true, newVal);
        }

        /// <summary>
        /// Retreives an integer property from the cache or database
        /// </summary>
        /// <param name="key">The string key for the property</param>
        /// <param name="fallback">The value to return if the property cannot be found. This will be cached as well.</param>
        /// <returns>An integer value representing the property</returns>
        public static int GetInt(string key, int fallback = 0)
        {
            if (CachedIntegerSettings.ContainsKey(key))
            {
                return CachedIntegerSettings[key].Item;
            }

            var dbValue = DatabaseManager.ServerConfig.GetInt(key);
            var isModified = false;

            if (dbValue == null || dbValue?.Value == null)
            {
                isModified = true;
            }

            var intVal = dbValue?.Value ?? fallback;
            CachedIntegerSettings[key] = new ConfigurationEntry<int>(isModified, intVal);
            return intVal;
        }

        /// <summary>
        /// Modifies an integer value in the cache and marks it for being synced on the next cycle.
        /// </summary>
        /// <param name="key">The string key for the property</param>
        /// <param name="newVal">The value to replace the old value with</param>
        public static void ModifyInt(string key, int newVal)
        {
            CachedIntegerSettings[key] = new ConfigurationEntry<int>(true, newVal);
        }

        /// <summary>
        /// Retrieves a float property from the cache or database
        /// </summary>
        /// <param name="key">The string key for the property</param>
        /// <param name="fallback">The value to return if the property cannot be found. This will be cached as well.</param>
        /// <returns>A float value representing the property</returns>
        public static float GetFloat(string key, float fallback = 0.0f)
        {
            if (CachedFloatSettings.ContainsKey(key))
            {
                return CachedFloatSettings[key].Item;
            }

            var dbValue = DatabaseManager.ServerConfig.GetFloat(key);
            var isModified = false;

            if (dbValue == null || dbValue?.Value == null)
            {
                isModified = true;
            }

            var floatVal = dbValue?.Value ?? fallback;
            CachedFloatSettings[key] = new ConfigurationEntry<float>(isModified, floatVal);
            return floatVal;
        }

        /// <summary>
        /// Modifies a float value in the cache and marks it for being synced on the next cycle.
        /// </summary>
        /// <param name="key">The string key for the property</param>
        /// <param name="newVal">The value to replace the old value with</param>
        public static void ModifyFloat(string key, float newVal)
        {
            CachedFloatSettings[key] = new ConfigurationEntry<float>(true, newVal);
        }

        /// <summary>
        /// Retreives a string property from the cache or database
        /// </summary>
        /// <param name="key">The string key for the property</param>
        /// <param name="fallback">The value to return if the property cannot be found. This will be cached as well.</param>
        /// <returns>A string value representing the property</returns>
        public static string GetString(string key, string fallback = "")
        {
            if (CachedStringSettings.ContainsKey(key))
            {
                return CachedStringSettings[key].Item;
            }

            var dbValue = DatabaseManager.ServerConfig.GetString(key);
            var isModified = false;

            if (dbValue == null || dbValue?.Value == null)
            {
                isModified = true;
            }

            var stringVal = dbValue?.Value ?? fallback;
            CachedStringSettings[key] = new ConfigurationEntry<string>(isModified, stringVal);
            return stringVal;
        }

        /// <summary>
        /// Modifies a string value in the cache and marks it for being synced on the next cycle
        /// </summary>
        /// <param name="key">The string key for the property</param>
        /// <param name="newVal">The value to replace the old value with</param>
        public static void ModifyString(string key, string newVal)
        {
            CachedStringSettings[key] = new ConfigurationEntry<string>(true, newVal);
        }

        /// <summary>
        /// Resyncs the variables with the database manually.
        /// Disables the timer so that the elapsed event cannot run during the update operation.
        /// </summary>
        public static void ResyncVariables()
        {
            _workerThread.Stop();
            DoWork(null, null);
            _workerThread.Start();
        }

        /// <summary>
        /// Loads the variables from the database directly into the cache.
        /// </summary>
        private static void LoadPropertiesFromDB()
        {
            log.Debug("Fetching boolean properties from database");
            foreach (var i in DatabaseManager.ServerConfig.GetAllBools())
                CachedBooleanSettings[i.Key] = new ConfigurationEntry<bool>(false, i.Value ?? false);

            log.Debug("Fetching integer properties from database");
            foreach (var i in DatabaseManager.ServerConfig.GetAllInts())
                CachedIntegerSettings[i.Key] = new ConfigurationEntry<int>(false, i.Value ?? 0);

            log.Debug("Fetching float properties from database");
            foreach (var i in DatabaseManager.ServerConfig.GetAllFloats())
                CachedFloatSettings[i.Key] = new ConfigurationEntry<float>(false, i.Value ?? 0.0f);

            log.Debug("Fetching string properties from database");
            foreach (var i in DatabaseManager.ServerConfig.GetAllStrings())
                CachedStringSettings[i.Key] = new ConfigurationEntry<string>(false, i.Value ?? "");
        }

        /// <summary>
        /// Writes all of the updated boolean values from the cache into the database.
        /// </summary>
        private static void WriteBoolToDB()
        {
            log.Info("Beginning to write modified boolean properties into database");
            foreach (var i in CachedBooleanSettings.Where(r => r.Value.Modified == true))
            {
                // this probably should be upsert. This does 2 queries per modified datapoint.
                // perhaps run a transaction to queue all the queries at once.
                if (DatabaseManager.ServerConfig.BoolExists(i.Key))
                    DatabaseManager.ServerConfig.ModifyBool(new Database.Models.Config.BoolStat { Key = i.Key, Value = i.Value.Item });
                else
                    DatabaseManager.ServerConfig.AddBool(i.Key, i.Value.Item);
            }
        }

        /// <summary>
        /// Writes all of the updated integer values from the cache into the database.
        /// </summary>
        private static void WriteIntToDB()
        {
            log.Info("Beginning to write modified integer properties into database");
            foreach (var i in CachedIntegerSettings.Where(r => r.Value.Modified == true))
            {
                // todo: see boolean section for caveat in this approach
                if (DatabaseManager.ServerConfig.IntExists(i.Key))
                    DatabaseManager.ServerConfig.ModifyInt(new Database.Models.Config.IntegerStat { Key = i.Key, Value = i.Value.Item });
                else
                    DatabaseManager.ServerConfig.AddInt(i.Key, i.Value.Item);
            }
        }

        /// <summary>
        /// Writes all of the updated float values from the cache into the database.
        /// </summary>
        private static void WriteFloatToDB()
        {
            // float next
            log.Info("Beginning to write modified float properties into database");
            foreach (var i in CachedFloatSettings.Where(r => r.Value.Modified == true))
            {
                // todo: see boolean section for caveat in this approach
                if (DatabaseManager.ServerConfig.FloatExists(i.Key))
                    DatabaseManager.ServerConfig.ModifyFloat(new Database.Models.Config.FloatStat { Key = i.Key, Value = i.Value.Item });
                else
                    DatabaseManager.ServerConfig.AddFloat(i.Key, i.Value.Item);
            }
        }

        /// <summary>
        /// Writes all of the updated string values from the cache into the database.
        /// </summary>
        private static void WriteStringToDB()
        {
            log.Info("Beginning to write modified string properties into database");
            foreach (var i in CachedStringSettings.Where(r => r.Value.Modified == true))
            {
                // todo: see boolean section for caveat in this approach
                if (DatabaseManager.ServerConfig.StringExists(i.Key))
                    DatabaseManager.ServerConfig.ModifyString(new Database.Models.Config.StringStat { Key = i.Key, Value = i.Value.Item });
                else
                    DatabaseManager.ServerConfig.AddString(i.Key, i.Value.Item);
            }
        }

        private static void DoWork(Object source, ElapsedEventArgs e)
        {
            // first, check for variables updated on the server-side. Write those to the DB.
            // then, compare variables to DB and update from DB as necessary. (needs to minimize r/w)

            WriteBoolToDB();
            WriteIntToDB();
            WriteFloatToDB();
            WriteStringToDB();

            // next, we need to fetch all of the variables from the DB and compare them quickly.
            LoadPropertiesFromDB();
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

        public override string ToString()
        {
            return Item.ToString() + " " + Modified;
        }
    }
}
