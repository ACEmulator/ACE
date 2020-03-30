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

            if (Program.IsRunningInContainer && !GetString("content_folder").Equals("/ace/Content"))
                ModifyString("content_folder", "/ace/Content");

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
        /// <returns>true if the property was modified, false if no property exists with the given key</returns>
        public static bool ModifyBool(string key, bool newVal)
        {
            if (!DefaultPropertyManager.DefaultBooleanProperties.ContainsKey(key))
                return false;

            if (CachedBooleanSettings.ContainsKey(key))
                CachedBooleanSettings[key].Modify(newVal);
            else
                CachedBooleanSettings[key] = new ConfigurationEntry<bool>(true, newVal, DefaultPropertyManager.DefaultBooleanProperties[key].Description);

            return true;
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
        /// <returns>true if the property was modified, false if no property exists with the given key</returns>
        public static bool ModifyLong(string key, long newVal)
        {
            if (!DefaultPropertyManager.DefaultLongProperties.ContainsKey(key))
                return false;

            if (CachedLongSettings.ContainsKey(key))
                CachedLongSettings[key].Modify(newVal);
            else
                CachedLongSettings[key] = new ConfigurationEntry<long>(true, newVal, DefaultPropertyManager.DefaultLongProperties[key].Description);
            return true;
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
        public static bool ModifyDouble(string key, double newVal)
        {
            if (!DefaultPropertyManager.DefaultDoubleProperties.ContainsKey(key))
                return false;
            if (CachedDoubleSettings.ContainsKey(key))
                CachedDoubleSettings[key].Modify(newVal);
            else
                CachedDoubleSettings[key] = new ConfigurationEntry<double>(true, newVal, DefaultPropertyManager.DefaultDoubleProperties[key].Description);
            return true;
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
        /// <returns>true if the property was modified, false if no property exists with the given key</returns>
        public static bool ModifyString(string key, string newVal)
        {
            if (!DefaultPropertyManager.DefaultStringProperties.ContainsKey(key))
                return false;

            if (CachedStringSettings.ContainsKey(key))
                CachedStringSettings[key].Modify(newVal);
            else
                CachedStringSettings[key] = new ConfigurationEntry<string>(true, newVal, DefaultPropertyManager.DefaultStringProperties[key].Description);
            return true;
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
        public static string ListProperties()
        {
            string props = "Boolean properties:\n";
            foreach (var item in DefaultPropertyManager.DefaultBooleanProperties)
                props += string.Format("\t{0}: {1} (current is {2}, default is {3})\n", item.Key, item.Value.Description, GetBool(item.Key).Item, item.Value.Item);

            props += "\nLong properties:\n";
            foreach (var item in DefaultPropertyManager.DefaultLongProperties)
                props += string.Format("\t{0}: {1} (current is {2}, default is {3})\n", item.Key, item.Value.Description, GetLong(item.Key).Item, item.Value.Item);

            props += "\nDouble properties:\n";
            foreach (var item in DefaultPropertyManager.DefaultDoubleProperties)
                props += string.Format("\t{0}: {1} (current is {2}, default is {3})\n", item.Key, item.Value.Description, GetDouble(item.Key).Item, item.Value.Item);

            props += "\nString properties:\n";
            foreach (var item in DefaultPropertyManager.DefaultStringProperties)
                props += string.Format("\t{0}: {1} (default is hidden)\n", item.Key, item.Value.Description);

            return props;
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
                PropertyManager.ModifyBool(item.Key, item.Value.Item);

            //float
            foreach (var item in DefaultDoubleProperties)
                PropertyManager.ModifyDouble(item.Key, item.Value.Item);

            //int
            foreach (var item in DefaultLongProperties)
                PropertyManager.ModifyLong(item.Key, item.Value.Item);

            //string
            foreach (var item in DefaultStringProperties)
                PropertyManager.ModifyString(item.Key, item.Value.Item);
        }

        // ==================================================================================
        // To change these values for the server,
        // please use the /modifybool, /modifylong, /modifydouble, and /modifystring commands
        // ==================================================================================

        public static readonly ReadOnlyDictionary<string, Property<bool>> DefaultBooleanProperties =
            DictOf(
                ("account_login_boots_in_use", new Property<bool>(true, "if FALSE, oldest connection to account is not booted when new connection occurs")),
                ("advanced_combat_pets", new Property<bool>(false, "(non-retail function) If enabled, Combat Pets can cast spells")),
                ("advocate_fane_auto_bestow", new Property<bool>(false, "If enabled, Advocate Fane will automatically bestow new advocates to advocate_fane_auto_bestow_level")),
                ("aetheria_heal_color", new Property<bool>(false, "If enabled, changes the aetheria healing over time messages from the default retail red color to green")),
                ("assess_creature_mod", new Property<bool>(false, "(non-retail function) If enabled, re-enables former skill formula, when assess creature skill is not trained or spec'ed")),
                ("chess_enabled", new Property<bool>(true, "if FALSE then chess will be disabled")),
                ("client_movement_formula", new Property<bool>(false, "If enabled, server uses DoMotion/StopMotion self-client movement methods instead of apply_raw_movement")),
                ("container_opener_name", new Property<bool>(false, "If enabled, when a player tries to open a container that is already in use by someone else, replaces 'someone else' in the message with the actual name of the player")),
                ("corpse_decay_tick_logging", new Property<bool>(false, "If ENABLED then player corpse ticks will be logged")),
                ("corpse_destroy_pyreals", new Property<bool>(true, "If FALSE then pyreals will not be completely destroyed on player death")),
                ("craft_exact_msg", new Property<bool>(false, "If TRUE, and player has crafting chance of success dialog enabled, shows them an additional message in their chat window with exact %")),
                ("creatures_drop_createlist_wield", new Property<bool>(false, "If FALSE then Wielded items in CreateList will not drop. Retail defaulted to TRUE but there are currently data errors")),
                ("equipmentsetid_enabled", new Property<bool>(false, "enable this to allow adding EquipmentSetIDs to loot armor")),
                ("equipmentsetid_name_decoration", new Property<bool>(false, "enable this to add the EquipmentSet name to loot armor name")),
                ("fastbuff", new Property<bool>(true, "If TRUE, enables the fast buffing trick from retail.")),
                ("fellow_kt_killer", new Property<bool>(true, "if FALSE, fellowship kill tasks will share with the fellowship, even if the killer doesn't have the quest")),
                ("fellow_kt_landblock", new Property<bool>(false, "if TRUE, fellowship kill tasks will share with landblock range (192 distance radius, or entire dungeon)")),
                ("fellow_quest_bonus", new Property<bool>(false, "if TRUE, applies EvenShare formula to fellowship quest reward XP (300% max bonus, defaults to false in retail)")),
                ("gateway_ties_summonable", new Property<bool>(true, "if disabled, players cannot summon ties from gateways. defaults to enabled, as in retail")),
                ("house_per_char", new Property<bool>(false, "if TRUE, allows 1 house per char instead of 1 house per account")),
                ("house_purchase_requirements", new Property<bool>(true, "if disabled, requirements to purchase/rent house are not checked")),
                ("house_rent_enabled", new Property<bool>(true, "If FALSE then rent is not required")),
                ("iou_trades", new Property<bool>(false, "(non-retail function) If enabled, IOUs can be traded for objects that are missing in DB but added/restored later on")),
                ("item_dispel", new Property<bool>(false, "if enabled, allows players to dispel items. defaults to end of retail, where item dispels could only target creatures")),
                ("lifestone_broadcast_death", new Property<bool>(true, "if true, player deaths are additionally broadcast to other players standing near the destination lifestone")),
                ("log_audit", new Property<bool>(true, "if FALSE then audit channel is not logged")),
                ("loot_quality_mod", new Property<bool>(true, "if FALSE then the loot quality modifier of a Death Treasure profile does not affect loot generation")),
                ("npc_hairstyle_fullrange", new Property<bool>(false, "if TRUE, allows generated creatures to use full range of hairstyles. Retail only allowed first nine (0-8) out of 51")),
                ("override_encounter_spawn_rates", new Property<bool>(false, "if enabled, landblock encounter spawns are overidden by double properties below.")),
                ("player_config_command", new Property<bool>(false, "If enabled, players can use /config to change their settings via text commands")),
                ("player_receive_immediate_save", new Property<bool>(false, "if enabled, when the player receives items from an NPC, they will be saved immediately")),
                ("pk_server", new Property<bool>(false, "set this to TRUE for darktide servers")),
                ("pk_server_safe_training_academy", new Property<bool>(false, "set this to TRUE to disable pk fighting in training academy and time to exit starter town safely")),
                ("pkl_server", new Property<bool>(false, "set this to TRUE for pink servers")),
                ("quest_info_enabled", new Property<bool>(false, "toggles the /myquests player command")),
                ("runrate_add_hooks", new Property<bool>(false, "if TRUE, adds some runrate hooks that were missing from retail (exhaustion done, raise skill/attribute")),
                ("require_spell_comps", new Property<bool>(true, "if FALSE spell components are no longer required to be in inventory to cast spells. defaults to enabled, as in retail")),
                ("salvage_handle_overages", new Property<bool>(false, "in retail, if 2 salvage bags were combined beyond 100 structure, the overages would be lost")),
                ("show_dat_warning", new Property<bool>(false, "if TRUE, will alert player (dat_warning_msg) when client attempts to download from server and boot them from game, disabled by default")),
                ("show_dot_messages", new Property<bool>(false, "enabled, shows combat messages for DoT damage ticks. defaults to disabled, as in retail")),
                ("show_mana_conv_bonus_0", new Property<bool>(true, "if disabled, only shows mana conversion bonus if not zero, during appraisal of casting items")),
                ("smite_uses_takedamage", new Property<bool>(false, "if enabled, smite applies damage via TakeDamage")),
                ("spellcast_recoil_queue", new Property<bool>(false, "if true, players can queue the next spell to cast during recoil animation")),
                ("spell_projectile_ethereal", new Property<bool>(false, "broadcasts all spell projectiles as ethereal to clients only, and manually send stop velocity on collision. can fix various issues with client missing target id.")),
                ("suicide_instant_death", new Property<bool>(false, "if enabled, @die command kills player instantly. defaults to disabled, as in retail")),
                ("taboo_table", new Property<bool>(true, "if enabled, taboo table restricts player names during character creation")),
                ("tailoring_intermediate_uieffects", new Property<bool>(false, "If true, tailoring intermediate icons retain the magical/elemental highlight of the original item")),
                ("universal_masteries", new Property<bool>(true, "if TRUE, matches end of retail masteries - players wielding almost any weapon get +5 DR, except if the weapon \"seems tough to master\". " +
                                                                 "if FALSE, players start with mastery of 1 melee and 1 ranged weapon type based on heritage, and can later re-select these 2 masteries")),
                ("use_wield_requirements", new Property<bool>(true, "disable this to bypass wield requirements. mostly for dev debugging")),
                ("world_closed", new Property<bool>(false, "enable this to startup world as a closed to players world"))
                );

        public static readonly ReadOnlyDictionary<string, Property<long>> DefaultLongProperties =
            DictOf(
                ("char_delete_time", new Property<long>(3600, "the amount of time in seconds a deleted character can be restored")),
                ("mansion_min_rank", new Property<long>(6, "overrides the default allegiance rank required to own a mansion")),
                ("max_chars_per_account", new Property<long>(11, "retail defaults to 11, client supports up to 20")),
                ("pk_timer", new Property<long>(20, "the number of seconds where a player cannot perform certain actions (ie. teleporting) after becoming involved in a PK battle")),
                ("player_save_interval", new Property<long>(300, "the number of seconds between automatic player saves")),
                ("teleport_visibility_fix", new Property<long>(0, "Fixes some possible issues with invisible players and mobs. 0 = default / disabled, 1 = players only, 2 = creatures, 3 = all world objects"))
                );

        public static readonly ReadOnlyDictionary<string, Property<double>> DefaultDoubleProperties =
            DictOf(

                ("minor_cantrip_drop_rate", new Property<double>(1.0, "Modifier for minor cantrip drop rate, 1 being normal")),
                ("major_cantrip_drop_rate", new Property<double>(1.0, "Modifier for major cantrip drop rate, 1 being normal")),
                ("epic_cantrip_drop_rate", new Property<double>(1.0, "Modifier for epic cantrip drop rate, 1 being normal")),
                ("legendary_cantrip_drop_rate", new Property<double>(1.0, "Modifier for legendary cantrip drop rate, 1 being normal")),

                ("advocate_fane_auto_bestow_level", new Property<double>(1, "the level that advocates are automatically bestowed by Advocate Fane if advocate_fane_auto_bestow is true")),
                ("aetheria_drop_rate", new Property<double>(1.0, "Modifier for Aetheria drop rate, 1 being normal")),
                ("chess_ai_start_time", new Property<double>(-1.0, "the number of seconds for the chess ai to start. defaults to -1 (disabled)")),
                ("encounter_delay", new Property<double>(1800, "the number of seconds a generator profile for regions is delayed from returning to free slots")),
                ("encounter_regen_interval", new Property<double>(600, "the number of seconds a generator for regions at which spawns its next set of objects")),
                ("equipmentsetid_drop_rate", new Property<double>(1.0, "Modifier for EquipmentSetID drop rate, 1 being normal")),
                ("ignore_magic_armor_pvp_scalar", new Property<double>(1.0, "Scales the effectiveness of IgnoreMagicArmor (ie. hollow weapons) in pvp battles. 1.0 = full effectiveness / ignore all enchantments on armor (default), 0.5 = half effectiveness / use half enchantments from armor, 0.0 = no effectiveness / use full enchantments from armor")),
                ("ignore_magic_resist_pvp_scalar", new Property<double>(1.0, "Scales the effectiveness of IgnoreMagicResist (ie. hollow weapons) in pvp battles. 1.0 = full effectiveness / ignore all resistances from life enchantments (default), 0.5 = half effectiveness / use half resistances from life enchantments, 0.0 = no effectiveness / use full resistances from life enchantments")),
                ("luminance_modifier", new Property<double>(1.0, "scales the amount of luminance received by players")),
                ("pk_new_character_grace_period", new Property<double>(300, "the number of seconds, in addition to pk_respite_timer, that a player killer is set to non-player killer status after first exiting training academy")),
                ("pk_respite_timer", new Property<double>(300, "the number of seconds that a player killer is set to non-player killer status after dying to another player killer")),
                ("quest_mindelta_rate", new Property<double>(1.0, "scales all quest min delta time between solves, 1 being normal")),
                ("spellcast_max_angle", new Property<double>(5.0, "for advanced player spell casting, the maximum angle to target release a spell projectile. retail seemed to default to a lower 5-20 value here, although some players seem to prefer a higher 45 degree angle")),
                ("trophy_drop_rate", new Property<double>(1.0, "Modifier for trophies dropped on creature death")),
                ("vendor_unique_rot_time", new Property<double>(300, "the number of seconds before unique items sold to vendors disappear")),
                ("vitae_penalty", new Property<double>(0.05, "the amount of vitae penalty a player gets per death")),
                ("vitae_penalty_max", new Property<double>(0.40, "the maximum vitae penalty a player can have")),
                ("xp_modifier", new Property<double>(1.0, "scales the amount of xp received by players"))
                );

        public static readonly ReadOnlyDictionary<string, Property<string>> DefaultStringProperties =
            DictOf(
                ("content_folder", new Property<string>(".\\Content", "for content creators to live edit weenies. defaults to Content folder found in same directory as ACE.Server.dll")),
                ("dat_warning_msg", new Property<string>("Your DAT files are incomplete.\nACEmulator does not support dynamic DAT updating at this time.\nPlease visit https://emulator.ac/how-to-play to download the complete DAT files.", "Warning message displayed (if show_dat_warning is true) to player if client attempts DAT download from server")),
                ("popup_header", new Property<string>("Welcome to Asheron's Call!", "Welcome message displayed when you log in")),
                ("popup_welcome", new Property<string>("To begin your training, speak to the Society Greeter. Walk up to the Society Greeter using the 'W' key, then double-click on her to initiate a conversation.", "Welcome message popup in training halls")),
                ("popup_motd", new Property<string>("", "Popup message of the day")),
                ("server_motd", new Property<string>("", "Server message of the day"))
                );
    }
}
