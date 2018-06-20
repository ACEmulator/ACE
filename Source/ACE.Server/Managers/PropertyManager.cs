using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using ACE.Database;
using System.Linq;
using log4net;
using System.Collections.ObjectModel;

namespace ACE.Server.Managers
{
    public static class PropertyManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // caching internally to the server
        private static ConcurrentDictionary<string, ConfigurationEntry<bool>> CachedBooleanSettings = new ConcurrentDictionary<string, ConfigurationEntry<bool>>();
        private static ConcurrentDictionary<string, ConfigurationEntry<long>> CachedLongSettings = new ConcurrentDictionary<string, ConfigurationEntry<long>>();
        private static ConcurrentDictionary<string, ConfigurationEntry<double>> CachedDoubleSettings = new ConcurrentDictionary<string, ConfigurationEntry<double>>();
        private static ConcurrentDictionary<string, ConfigurationEntry<string>> CachedStringSettings = new ConcurrentDictionary<string, ConfigurationEntry<string>>();

        private static Timer _workerThread;

        /// <summary>
        /// Initializes the PropertyManager.
        /// Run this only once per server instance.
        /// </summary>
        /// <param name="loadDefaultValues">Should we use the DefaultPropertyManager to load the default properties for keys?</param>
        public static void Initialize(bool loadDefaultValues = true)
        {
            if (loadDefaultValues)
            {
                DefaultPropertyManager.LoadDefaultProperties();
            }
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
        /// <param name="fallback">The value to return if the property cannot be found.</param>
        /// <param name="cacheFallback">Whether or not the fallback property should be cached.</param>
        /// <returns>A boolean value representing the property</returns>
        public static Property<bool> GetBool(string key, bool fallback = false, bool cacheFallback = true)
        {
            // first, check the cache. If the key exists in the cache, grab it regardless of it's modified value
            // then, check the database. if the key exists in the database, grab it and cache it
            // finally, set it to a default of false.
            if (CachedBooleanSettings.ContainsKey(key))
            {
                return new Property<bool>(CachedBooleanSettings[key].Item, CachedBooleanSettings[key].Description);
            }

            var dbValue = DatabaseManager.Shard.Config.GetBool(key);
            var useFallback = false;

            if (dbValue == null || dbValue?.Value == null)
            {
                useFallback = true;
            }

            var boolVal = dbValue?.Value ?? fallback;

            if (!useFallback || (useFallback && cacheFallback))
                CachedBooleanSettings[key] = new ConfigurationEntry<bool>(useFallback, boolVal, dbValue.Description);
            return new Property<bool>(boolVal, dbValue.Description);
        }

        /// <summary>
        /// Modifies a boolean value in the cache and marks it for being synced on the next cycle.
        /// </summary>
        /// <param name="key">The string key for the property</param>
        /// <param name="newVal">The value to replace the old value with</param>
        public static void ModifyBool(string key, bool newVal)
        {
            if (CachedBooleanSettings.ContainsKey(key))
                CachedBooleanSettings[key].Modify(newVal);
            else
                CachedBooleanSettings[key] = new ConfigurationEntry<bool>(true, newVal);
        }

        public static void ModifyBoolDescription(string key, string description)
        {
            if (CachedBooleanSettings.ContainsKey(key))
                CachedBooleanSettings[key].ModifyDescription(description);
            else
                log.Warn($"Attempted to modify {key} which did not exist in the BOOL cache.");
        }

        /// <summary>
        /// Retreives an integer property from the cache or database
        /// </summary>
        /// <param name="key">The string key for the property</param>
        /// <param name="fallback">The value to return if the property cannot be found.</param>
        /// <param name="cacheFallback">Whether or not the fallback property should be cached</param>
        /// <returns>An integer value representing the property</returns>
        public static Property<long> GetLong(string key, long fallback = 0, bool cacheFallback = true)
        {
            if (CachedLongSettings.ContainsKey(key))
            {
                return new Property<long>(CachedLongSettings[key].Item, CachedLongSettings[key].Description);
            }

            var dbValue = DatabaseManager.Shard.Config.GetLong(key);
            var useFallback = false;

            if (dbValue == null || dbValue?.Value == null)
            {
                useFallback = true;
            }

            var intVal = dbValue?.Value ?? fallback;
            if (!useFallback || (useFallback && cacheFallback))
                CachedLongSettings[key] = new ConfigurationEntry<long>(useFallback, intVal, dbValue.Description);
            return new Property<long>(intVal, dbValue.Description);
        }

        /// <summary>
        /// Modifies an integer value in the cache and marks it for being synced on the next cycle.
        /// </summary>
        /// <param name="key">The string key for the property</param>
        /// <param name="newVal">The value to replace the old value with</param>
        public static void ModifyLong(string key, long newVal)
        {
            if (CachedLongSettings.ContainsKey(key))
                CachedLongSettings[key].Modify(newVal);
            else
                CachedLongSettings[key] = new ConfigurationEntry<long>(true, newVal);
        }

        public static void ModifyLongDescription(string key, string description)
        {
            if (CachedLongSettings.ContainsKey(key))
                CachedLongSettings[key].ModifyDescription(description);
            else
                log.Warn($"Attempted to modify {key} which did not exist in the LONG cache.");
        }

        /// <summary>
        /// Retrieves a float property from the cache or database
        /// </summary>
        /// <param name="key">The string key for the property</param>
        /// <param name="fallback">The value to return if the property cannot be found.</param>
        /// <param name="cacheFallback">Whether or not the fallpack property should be cached</param>
        /// <returns>A float value representing the property</returns>
        public static Property<double> GetDouble(string key, double fallback = 0.0f, bool cacheFallback = true)
        {
            if (CachedDoubleSettings.ContainsKey(key))
            {
                return new Property<double>(CachedDoubleSettings[key].Item, CachedDoubleSettings[key].Description);
            }

            var dbValue = DatabaseManager.Shard.Config.GetLong(key);
            var useFallback = false;

            if (dbValue == null || dbValue?.Value == null)
            {
                useFallback = true;
            }

            var floatVal = dbValue?.Value ?? fallback;
            if (!useFallback || (useFallback && cacheFallback))
                CachedDoubleSettings[key] = new ConfigurationEntry<double>(useFallback, floatVal, dbValue.Description);
            return new Property<double>(CachedDoubleSettings[key].Item, CachedDoubleSettings[key].Description);
        }

        /// <summary>
        /// Modifies a float value in the cache and marks it for being synced on the next cycle.
        /// </summary>
        /// <param name="key">The string key for the property</param>
        /// <param name="newVal">The value to replace the old value with</param>
        public static void ModifyDouble(string key, double newVal)
        {
            if (CachedDoubleSettings.ContainsKey(key))
                CachedDoubleSettings[key].Modify(newVal);
            else
                CachedDoubleSettings[key] = new ConfigurationEntry<double>(true, newVal);
        }

        public static void ModifyDoubleDescription(string key, string description)
        {
            if (CachedDoubleSettings.ContainsKey(key))
                CachedDoubleSettings[key].ModifyDescription(description);
            else
                log.Warn($"Attempted to modify the description of {key} which did not exist in the DOUBLE cache.");
        }

        /// <summary>
        /// Retreives a string property from the cache or database
        /// </summary>
        /// <param name="key">The string key for the property</param>
        /// <param name="fallback">The value to return if the property cannot be found.</param>
        /// <param name="cacheFallback">Whether or not the fallback value will be cached.</param>
        /// <returns>A string value representing the property</returns>
        public static Property<string> GetString(string key, string fallback = "", bool cacheFallback = true)
        {
            if (CachedStringSettings.ContainsKey(key))
            {
                return new Property<string>(CachedStringSettings[key].Item, CachedStringSettings[key].Description);
            }

            var dbValue = DatabaseManager.Shard.Config.GetString(key);
            var useFallback = false;

            if (dbValue == null || dbValue?.Value == null)
            {
                useFallback = true;
            }

            var stringVal = dbValue?.Value ?? fallback;
            if (!useFallback || (useFallback && cacheFallback))
                CachedStringSettings[key] = new ConfigurationEntry<string>(useFallback, stringVal, dbValue.Description);
            return new Property<string>(stringVal, dbValue.Description);
        }

        /// <summary>
        /// Modifies a string value in the cache and marks it for being synced on the next cycle
        /// </summary>
        /// <param name="key">The string key for the property</param>
        /// <param name="newVal">The value to replace the old value with</param>
        public static void ModifyString(string key, string newVal)
        {
            if (CachedStringSettings.ContainsKey(key))
                CachedStringSettings[key].Modify(newVal);
            else
                CachedStringSettings[key] = new ConfigurationEntry<string>(true, newVal);
        }

        public static void ModifyStringDescription(string key, string description)
        {
            if (CachedStringSettings.ContainsKey(key))
                CachedStringSettings[key].ModifyDescription(description);
            else
                log.Warn($"Attempted to modify {key} which did not exist in the STRING cache.");
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
            foreach (var i in DatabaseManager.Shard.Config.GetAllBools())
                CachedBooleanSettings[i.Key] = new ConfigurationEntry<bool>(false, i.Value, i.Description);

            log.Debug("Fetching long properties from database");
            foreach (var i in DatabaseManager.Shard.Config.GetAllLongs())
                CachedLongSettings[i.Key] = new ConfigurationEntry<long>(false, i.Value, i.Description);

            log.Debug("Fetching double properties from database");
            foreach (var i in DatabaseManager.Shard.Config.GetAllDoubles())
                CachedDoubleSettings[i.Key] = new ConfigurationEntry<double>(false, i.Value, i.Description);

            log.Debug("Fetching string properties from database");
            foreach (var i in DatabaseManager.Shard.Config.GetAllStrings())
                CachedStringSettings[i.Key] = new ConfigurationEntry<string>(false, i.Value, i.Description);
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
                if (DatabaseManager.Shard.Config.BoolExists(i.Key))
                    DatabaseManager.Shard.Config.ModifyBool(new Database.Models.Shard.ConfigPropertiesBoolean { Key = i.Key, Value = i.Value.Item, Description = i.Value.Description });
                else
                    DatabaseManager.Shard.Config.AddBool(i.Key, i.Value.Item, i.Value.Description);
            }
        }

        /// <summary>
        /// Writes all of the updated integer values from the cache into the database.
        /// </summary>
        private static void WriteLongToDB()
        {
            log.Info("Beginning to write modified integer properties into database");
            foreach (var i in CachedLongSettings.Where(r => r.Value.Modified == true))
            {
                // todo: see boolean section for caveat in this approach
                if (DatabaseManager.Shard.Config.LongExists(i.Key))
                    DatabaseManager.Shard.Config.ModifyLong(new Database.Models.Shard.ConfigPropertiesLong { Key = i.Key, Value = i.Value.Item, Description = i.Value.Description });
                else
                    DatabaseManager.Shard.Config.AddLong(i.Key, i.Value.Item, i.Value.Description);
            }
        }

        /// <summary>
        /// Writes all of the updated float values from the cache into the database.
        /// </summary>
        private static void WriteDoubleToDB()
        {
            // float next
            log.Info("Beginning to write modified float properties into database");
            foreach (var i in CachedDoubleSettings.Where(r => r.Value.Modified == true))
            {
                // todo: see boolean section for caveat in this approach
                if (DatabaseManager.Shard.Config.LongExists(i.Key))
                    DatabaseManager.Shard.Config.ModifyDouble(new Database.Models.Shard.ConfigPropertiesDouble { Key = i.Key, Value = i.Value.Item, Description = i.Value.Description });
                else
                    DatabaseManager.Shard.Config.AddDouble(i.Key, i.Value.Item, i.Value.Description);
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
                if (DatabaseManager.Shard.Config.StringExists(i.Key))
                    DatabaseManager.Shard.Config.ModifyString(new Database.Models.Shard.ConfigPropertiesString { Key = i.Key, Value = i.Value.Item, Description = i.Value.Description });
                else
                    DatabaseManager.Shard.Config.AddString(i.Key, i.Value.Item, i.Value.Description);
            }
        }

        private static void DoWork(Object source, ElapsedEventArgs e)
        {
            // first, check for variables updated on the server-side. Write those to the DB.
            // then, compare variables to DB and update from DB as necessary. (needs to minimize r/w)

            WriteBoolToDB();
            WriteLongToDB();
            WriteDoubleToDB();
            WriteStringToDB();

            // next, we need to fetch all of the variables from the DB and compare them quickly.
            LoadPropertiesFromDB();
        }

    }

    public struct Property<T>
    {
        public Property(T item, string description) : this()
        {
            Item = item;
            Description = description;
        }

        public T Item { get;  private set; }
        public string Description { get;  private set; }
    }

    class ConfigurationEntry<T>
    {
        public bool Modified;
        public T Item;
        public string Description;

        public ConfigurationEntry(bool modified, T item)
        {
            this.Modified = modified;
            this.Item = item;
        }

        public ConfigurationEntry(bool modified, T item, string description)
        {
            this.Modified = modified;
            this.Item = item;
            this.Description = description;
        }

        public void Modify(T item)
        {
            this.Item = item;
            this.Modified = true;
        }

        public void ModifyDescription(string description)
        {
            this.Description = description;
            this.Modified = true;
        }

        public override string ToString()
        {
            return Item.ToString() + " " + Modified;
        }
    }

    public static class DefaultPropertyManager
    {
        private static ReadOnlyDictionary<A,V> DictOf<A, V>()
        {
            return new ReadOnlyDictionary<A, V>(new Dictionary<A, V>());
        }

        private static ReadOnlyDictionary<A, V> DictOf<A, V>(params (A, V)[] pairs) {
            return new ReadOnlyDictionary<A, V>(pairs.ToDictionary(
                (tup) => tup.Item1,
                (tup) => tup.Item2
            ));
        }

        public static void LoadDefaultProperties()
        {
            // Place any default properties to load in here
            //bool
            foreach (var item in DefaultBooleanProperties)
            {
                PropertyManager.ModifyBool(item.Key, item.Value);
            }
            //float
            foreach (var item in DefaultDoubleProperties)
            {
                PropertyManager.ModifyDouble(item.Key, item.Value);
            }
            //int
            foreach (var item in DefaultLongProperties)
            {
                PropertyManager.ModifyLong(item.Key, item.Value);
            }
            //string
            foreach (var item in DefaultStringProperties)
            {
                PropertyManager.ModifyString(item.Key, item.Value);
            }
        }

        public static readonly ReadOnlyDictionary<string, bool> DefaultBooleanProperties = DictOf<string, bool>();
        public static readonly ReadOnlyDictionary<string, long> DefaultLongProperties = DictOf<string, long>();
        public static readonly ReadOnlyDictionary<string, double> DefaultDoubleProperties =
            DictOf(
                ("xp_modifier", 1.0d),
                ("luminance_modifier", 1.0d),
                ("vitae_penalty", 0.05d),
                ("vitae_min", 0.60d)
                );
        public static readonly ReadOnlyDictionary<string, string> DefaultStringProperties =
            DictOf(
                ("motd_string", "Welcome to Asheron's Call\npowered by ACEmulator\n\nFor more information on commands supported by this server, type @acehelp")
                );
    }


}
