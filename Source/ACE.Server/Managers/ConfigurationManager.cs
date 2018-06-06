using System;
using System.Collections.Generic;
using System.Text;

using ACE.Database;

namespace ACE.Server.Managers
{
    public static class ConfigurationManager
    {
        // caching internally to the server
        private static Dictionary<string, ConfigurationEntry<bool>> CachedBooleanSettings = new Dictionary<string, ConfigurationEntry<bool>>();
        private static Dictionary<string, ConfigurationEntry<int>> CachedIntegerSettings = new Dictionary<string, ConfigurationEntry<int>>();
        private static Dictionary<string, ConfigurationEntry<float>> CachedFloatSettings = new Dictionary<string, ConfigurationEntry<float>>();
        private static Dictionary<string, ConfigurationEntry<string>> CachedStringSettings = new Dictionary<string, ConfigurationEntry<string>>();

        public static bool GetBool(string key)
        {
            // first, check the cache. If the key exists in the cache, grab it regardless of it's modified value
            // then, check the database. if the key exists in the database, grab it and cache it
            // finally, set it to a default of false.
            if (CachedBooleanSettings.ContainsKey(key))
            {
                return CachedBooleanSettings[key].Item;
            }

            var dbValue = DatabaseManager.ServerConfig.GetBool(key);

            var boolVal = dbValue?.Value ?? false;
            CachedBooleanSettings.Add(key, new ConfigurationEntry<bool>(false, boolVal));
            return boolVal;
        }

        public static void ModifyBool(string key, bool newVal)
        {
            // modify and flag the cache value for the next update.
            CachedBooleanSettings[key] = new ConfigurationEntry<bool>(true, newVal);
        }

        public static int GetInt(string key)
        {
            if (CachedIntegerSettings.ContainsKey(key))
            {
                return CachedIntegerSettings[key].Item;
            }

            var dbValue = DatabaseManager.ServerConfig.GetInt(key);

            var intVal = dbValue?.Value ?? 0;
            CachedIntegerSettings.Add(key, new ConfigurationEntry<int>(false, intVal));
            return intVal;
        }

        public static void ModifyInt(string key, int newVal)
        {
            CachedIntegerSettings[key] = new ConfigurationEntry<int>(true, newVal);
        }

        public static float GetFloat(string key)
        {
            if (CachedFloatSettings.ContainsKey(key))
            {
                return CachedFloatSettings[key].Item;
            }

            var dbValue = DatabaseManager.ServerConfig.GetFloat(key);

            var floatVal = dbValue?.Value ?? 0.0f;
            CachedFloatSettings.Add(key, new ConfigurationEntry<float>(false, floatVal));
            return floatVal;
        }

        public static void ModifyFloat(string key, float newVal)
        {
            CachedFloatSettings[key] = new ConfigurationEntry<float>(true, newVal);
        }

        public static string GetString(string key)
        {
            if (CachedStringSettings.ContainsKey(key))
            {
                return CachedStringSettings[key].Item;
            }

            var dbValue = DatabaseManager.ServerConfig.GetString(key);

            var stringVal = dbValue?.Value ?? "";
            CachedStringSettings.Add(key, new ConfigurationEntry<string>(false, stringVal));
            return stringVal;
        }

        public static void ModifyString(string key, string newVal)
        {
            CachedStringSettings[key] = new ConfigurationEntry<string>(true, newVal);
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
