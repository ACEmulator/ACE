using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Timers;

using log4net;

using ACE.Database;

namespace ACE.Server.Managers
{
    public static class PropertyManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // caching internally to the server
        private static readonly ConcurrentDictionary<string, ConfigurationEntry<bool>> CachedBooleanSettings = new ConcurrentDictionary<string, ConfigurationEntry<bool>>();
        private static readonly ConcurrentDictionary<string, ConfigurationEntry<long>> CachedLongSettings = new ConcurrentDictionary<string, ConfigurationEntry<long>>();
        private static readonly ConcurrentDictionary<string, ConfigurationEntry<double>> CachedDoubleSettings = new ConcurrentDictionary<string, ConfigurationEntry<double>>();
        private static readonly ConcurrentDictionary<string, ConfigurationEntry<string>> CachedStringSettings = new ConcurrentDictionary<string, ConfigurationEntry<string>>();

        private static Timer _workerThread;

        /// <summary>
        /// Initializes the PropertyManager.
        /// Run this only once per server instance.
        /// </summary>
        /// <param name="loadDefaultValues">Should we use the DefaultPropertyManager to load the default properties for keys?</param>
        public static void Initialize(bool loadDefaultValues = true)
        {
            if (loadDefaultValues)
                DefaultPropertyManager.LoadDefaultProperties();

            LoadPropertiesFromDB();

            _workerThread = new Timer(300000);
            _workerThread.Elapsed += DoWork;
            _workerThread.AutoReset = true;
            _workerThread.Start();
        }


        /// <summary>
        /// Loads the variables from the database directly into the cache.
        /// </summary>
        private static void LoadPropertiesFromDB()
        {
            foreach (var i in DatabaseManager.ShardConfig.GetAllBools())
                CachedBooleanSettings[i.Key] = new ConfigurationEntry<bool>(false, i.Value, i.Description);

            foreach (var i in DatabaseManager.ShardConfig.GetAllLongs())
                CachedLongSettings[i.Key] = new ConfigurationEntry<long>(false, i.Value, i.Description);

            foreach (var i in DatabaseManager.ShardConfig.GetAllDoubles())
                CachedDoubleSettings[i.Key] = new ConfigurationEntry<double>(false, i.Value, i.Description);

            foreach (var i in DatabaseManager.ShardConfig.GetAllStrings())
                CachedStringSettings[i.Key] = new ConfigurationEntry<string>(false, i.Value, i.Description);
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
            // first, check the cache. If the key exists in the cache, grab it regardless of its modified value
            // then, check the database. if the key exists in the database, grab it and cache it
            // finally, set it to a default of false.
            if (CachedBooleanSettings.ContainsKey(key))
                return new Property<bool>(CachedBooleanSettings[key].Item, CachedBooleanSettings[key].Description);

            var dbValue = DatabaseManager.ShardConfig.GetBool(key);

            bool useFallback = dbValue?.Value == null;

            var value = dbValue?.Value ?? fallback;

            if (!useFallback || cacheFallback)
                CachedBooleanSettings[key] = new ConfigurationEntry<bool>(useFallback, value, dbValue?.Description);

            return new Property<bool>(value, dbValue?.Description);
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
                return new Property<long>(CachedLongSettings[key].Item, CachedLongSettings[key].Description);

            var dbValue = DatabaseManager.ShardConfig.GetLong(key);

            bool useFallback = dbValue?.Value == null;

            var value = dbValue?.Value ?? fallback;

            if (!useFallback || cacheFallback)
                CachedLongSettings[key] = new ConfigurationEntry<long>(useFallback, value, dbValue?.Description);

            return new Property<long>(value, dbValue?.Description);
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
                return new Property<double>(CachedDoubleSettings[key].Item, CachedDoubleSettings[key].Description);

            var dbValue = DatabaseManager.ShardConfig.GetDouble(key);

            bool useFallback = dbValue?.Value == null;

            var value = dbValue?.Value ?? fallback;

            if (!useFallback || cacheFallback)
                CachedDoubleSettings[key] = new ConfigurationEntry<double>(useFallback, value, dbValue?.Description);

            return new Property<double>(value, dbValue?.Description);
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
                return new Property<string>(CachedStringSettings[key].Item, CachedStringSettings[key].Description);

            var dbValue = DatabaseManager.ShardConfig.GetString(key);

            bool useFallback = dbValue?.Value == null;

            var value = dbValue?.Value ?? fallback;

            if (!useFallback || cacheFallback)
                CachedStringSettings[key] = new ConfigurationEntry<string>(useFallback, value, dbValue?.Description);

            return new Property<string>(value, dbValue?.Description);
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
        /// Writes all of the updated boolean values from the cache into the database.
        /// </summary>
        private static void WriteBoolToDB()
        {
            foreach (var i in CachedBooleanSettings.Where(r => r.Value.Modified))
            {
                // this probably should be upsert. This does 2 queries per modified datapoint.
                // perhaps run a transaction to queue all the queries at once.
                if (DatabaseManager.ShardConfig.BoolExists(i.Key))
                    DatabaseManager.ShardConfig.SaveBool(new Database.Models.Shard.ConfigPropertiesBoolean { Key = i.Key, Value = i.Value.Item, Description = i.Value.Description });
                else
                    DatabaseManager.ShardConfig.AddBool(i.Key, i.Value.Item, i.Value.Description);
            }
        }

        /// <summary>
        /// Writes all of the updated integer values from the cache into the database.
        /// </summary>
        private static void WriteLongToDB()
        {
            foreach (var i in CachedLongSettings.Where(r => r.Value.Modified))
            {
                // todo: see boolean section for caveat in this approach
                if (DatabaseManager.ShardConfig.LongExists(i.Key))
                    DatabaseManager.ShardConfig.SaveLong(new Database.Models.Shard.ConfigPropertiesLong { Key = i.Key, Value = i.Value.Item, Description = i.Value.Description });
                else
                    DatabaseManager.ShardConfig.AddLong(i.Key, i.Value.Item, i.Value.Description);
            }
        }

        /// <summary>
        /// Writes all of the updated float values from the cache into the database.
        /// </summary>
        private static void WriteDoubleToDB()
        {
            foreach (var i in CachedDoubleSettings.Where(r => r.Value.Modified))
            {
                // todo: see boolean section for caveat in this approach
                if (DatabaseManager.ShardConfig.DoubleExists(i.Key))
                    DatabaseManager.ShardConfig.SaveDouble(new Database.Models.Shard.ConfigPropertiesDouble { Key = i.Key, Value = i.Value.Item, Description = i.Value.Description });
                else
                    DatabaseManager.ShardConfig.AddDouble(i.Key, i.Value.Item, i.Value.Description);
            }
        }

        /// <summary>
        /// Writes all of the updated string values from the cache into the database.
        /// </summary>
        private static void WriteStringToDB()
        {
            foreach (var i in CachedStringSettings.Where(r => r.Value.Modified))
            {
                // todo: see boolean section for caveat in this approach
                if (DatabaseManager.ShardConfig.StringExists(i.Key))
                    DatabaseManager.ShardConfig.SaveString(new Database.Models.Shard.ConfigPropertiesString { Key = i.Key, Value = i.Value.Item, Description = i.Value.Description });
                else
                    DatabaseManager.ShardConfig.AddString(i.Key, i.Value.Item, i.Value.Description);
            }
        }

        private static void DoWork(Object source, ElapsedEventArgs e)
        {
            var startTime = DateTime.UtcNow;

            // first, check for variables updated on the server-side. Write those to the DB.
            // then, compare variables to DB and update from DB as necessary. (needs to minimize r/w)
            
            WriteBoolToDB();
            WriteLongToDB();
            WriteDoubleToDB();
            WriteStringToDB();

            // next, we need to fetch all of the variables from the DB and compare them quickly.
            LoadPropertiesFromDB();

            log.Debug($"PropertyManager DoWork took {(DateTime.UtcNow - startTime).TotalMilliseconds:N0} ms");
        }
    }

    public struct Property<T>
    {
        public Property(T item, string description) : this()
        {
            Item = item;
            Description = description;
        }

        public T Item { get; }
        public string Description { get; }
    }

    class ConfigurationEntry<T>
    {
        public bool Modified;
        public T Item;
        public string Description;

        public ConfigurationEntry(bool modified, T item)
        {
            Modified = modified;
            Item = item;
        }

        public ConfigurationEntry(bool modified, T item, string description)
        {
            Modified = modified;
            Item = item;
            Description = description;
        }

        public void Modify(T item)
        {
            Item = item;
            Modified = true;
        }

        public void ModifyDescription(string description)
        {
            Description = description;
            Modified = true;
        }

        public override string ToString()
        {
            return Item + " " + Modified;
        }
    }

    public static class DefaultPropertyManager
    {
        private static ReadOnlyDictionary<A,V> DictOf<A, V>()
        {
            return new ReadOnlyDictionary<A, V>(new Dictionary<A, V>());
        }

        private static ReadOnlyDictionary<A, V> DictOf<A, V>(params (A, V)[] pairs)
        {
            return new ReadOnlyDictionary<A, V>(pairs.ToDictionary
            (
                tup => tup.Item1,
                tup => tup.Item2
            ));
        }

        public static void LoadDefaultProperties()
        {
            // Place any default properties to load in here

            //bool
            foreach (var item in DefaultBooleanProperties)
                PropertyManager.ModifyBool(item.Key, item.Value);

            //float
            foreach (var item in DefaultDoubleProperties)
                PropertyManager.ModifyDouble(item.Key, item.Value);

            //int
            foreach (var item in DefaultLongProperties)
                PropertyManager.ModifyLong(item.Key, item.Value);

            //string
            foreach (var item in DefaultStringProperties)
                PropertyManager.ModifyString(item.Key, item.Value);
        }

        // ==================================================================================
        // To change these values for the server,
        // please use the /modifybool, /modifylong, /modifydouble, and /modifystring commands
        // ==================================================================================

        public static readonly ReadOnlyDictionary<string, bool> DefaultBooleanProperties =
            DictOf(
                ("house_purchase_requirements", true),
                ("house_rent_enabled", true),
                ("pk_server", false)
                );

        public static readonly ReadOnlyDictionary<string, long> DefaultLongProperties =
            DictOf<string, long>(
                ("char_delete_time", 3600),     // the amount of time in seconds a deleted character can be restored
                ("mansion_min_rank", 6),        // overrides the default allegiance rank required to own a mansion
                ("max_chars_per_account", 11)   // retail defaults to 11, client supports up to 20
                );

        public static readonly ReadOnlyDictionary<string, double> DefaultDoubleProperties =
            DictOf(
                ("luminance_modifier", 1.0),
                ("vitae_penalty", 0.05),
                ("vitae_penalty_max", 0.40),
                ("xp_modifier", 1.0)
                );

        public static readonly ReadOnlyDictionary<string, string> DefaultStringProperties =
            DictOf(
                ("popup_header", "Welcome to Asheron's Call!"),
                ("popup_welcome", "To begin your training, speak to the Society Greeter. Walk up to the Society Greeter using the 'W' key, then double-click on her to initiate a conversation."),
                ("popup_motd", ""),
                ("server_motd", "")
                );
    }
}
