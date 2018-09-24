using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

using log4net;

using ACE.Database.Models.World;
using ACE.Database.Models.World.Internal;
using ACE.Entity.Enum;

namespace ACE.Database
{
    public class WorldDatabase 
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public bool Exists(bool retryUntilFound)
        {
            var config = Common.ConfigManager.Config.MySql.World;

            for (; ; )
            {
                using (var context = new WorldDbContext())
                {
                    if (((RelationalDatabaseCreator)context.Database.GetService<IDatabaseCreator>()).Exists())
                    {
                        log.Debug($"Successfully connected to {config.Database} database on {config.Host}:{config.Port}.");
                        return true;
                    }
                }

                log.Error($"Attempting to reconnect to {config.Database} database on {config.Host}:{config.Port} in 5 seconds...");

                if (retryUntilFound)
                    Thread.Sleep(5000);
                else
                    return false;
            }
        }


        /// <summary>
        /// Will return uint.MaxValue if no records were found within the range provided.
        /// </summary>
        public uint GetMaxGuidFoundInRange(uint min, uint max)
        {
            using (var context = new WorldDbContext())
            {
                var results = context.LandblockInstance
                    .AsNoTracking()
                    .Where(r => r.Guid >= min && r.Guid <= max)
                    .ToList();

                if (!results.Any())
                    return uint.MaxValue;

                var maxId = min;

                foreach (var result in results)
                {
                    if (result.Guid > maxId)
                        maxId = result.Guid;
                }

                return maxId;
            }
        }


        private readonly ConcurrentDictionary<uint, Weenie> weenieCache = new ConcurrentDictionary<uint, Weenie>();
        private readonly ConcurrentDictionary<uint, byte> weeniesNotFound = new ConcurrentDictionary<uint, byte>();

        /// <summary>
        /// This will populate all sub collections except the followign: LandblockInstances, PointsOfInterest<para />
        /// This will also update the weenie cache.
        /// </summary>
        private Weenie GetWeenie(WorldDbContext context, uint weenieClassId)
        {
            // Base properties for every weenie (ACBaseQualities)
            var weenie = context.Weenie
                .Include(r => r.WeeniePropertiesBool)
                .Include(r => r.WeeniePropertiesDID)
                .Include(r => r.WeeniePropertiesFloat)
                .Include(r => r.WeeniePropertiesIID)
                .Include(r => r.WeeniePropertiesInt)
                .Include(r => r.WeeniePropertiesInt64)
                .Include(r => r.WeeniePropertiesPosition)
                .Include(r => r.WeeniePropertiesString)
                .FirstOrDefault(r => r.ClassId == weenieClassId);

            if (weenie == null)
            {
                weenieCache.TryRemove(weenieClassId, out _);
                weeniesNotFound.TryAdd(weenieClassId, 0);
                return null;
            }

            var weenieType = (WeenieType)weenie.Type;

            bool isCreature = weenieType == WeenieType.Creature || weenieType == WeenieType.Cow ||
                              weenieType == WeenieType.Sentinel || weenieType == WeenieType.Admin ||
                              weenieType == WeenieType.Vendor;

            //.Include(r => r.LandblockInstances)   // When we grab a weenie, we don't need to also know everywhere it exists in the world
            //.Include(r => r.PointsOfInterest)     // I think these are just foreign keys for the POI table

            weenie.WeeniePropertiesAnimPart = context.WeeniePropertiesAnimPart.Where(r => r.ObjectId == weenie.ClassId).ToList();

            if (isCreature)
            {
                weenie.WeeniePropertiesAttribute = context.WeeniePropertiesAttribute.Where(r => r.ObjectId == weenie.ClassId).ToList();
                weenie.WeeniePropertiesAttribute2nd = context.WeeniePropertiesAttribute2nd.Where(r => r.ObjectId == weenie.ClassId).ToList();

                weenie.WeeniePropertiesBodyPart = context.WeeniePropertiesBodyPart.Where(r => r.ObjectId == weenie.ClassId).ToList();
            }

            if (weenieType == WeenieType.Book)
            {
                weenie.WeeniePropertiesBook = context.WeeniePropertiesBook.FirstOrDefault(r => r.ObjectId == weenie.ClassId);
                weenie.WeeniePropertiesBookPageData = context.WeeniePropertiesBookPageData.Where(r => r.ObjectId == weenie.ClassId).ToList();
            }

            weenie.WeeniePropertiesCreateList = context.WeeniePropertiesCreateList.Where(r => r.ObjectId == weenie.ClassId).ToList();
            weenie.WeeniePropertiesEmote = context.WeeniePropertiesEmote.Include(r => r.WeeniePropertiesEmoteAction).Where(r => r.ObjectId == weenie.ClassId).ToList();
            weenie.WeeniePropertiesEventFilter = context.WeeniePropertiesEventFilter.Where(r => r.ObjectId == weenie.ClassId).ToList();

            weenie.WeeniePropertiesGenerator = context.WeeniePropertiesGenerator.Where(r => r.ObjectId == weenie.ClassId).ToList();
            weenie.WeeniePropertiesPalette = context.WeeniePropertiesPalette.Where(r => r.ObjectId == weenie.ClassId).ToList();

            if (isCreature)
            {
                weenie.WeeniePropertiesSkill = context.WeeniePropertiesSkill.Where(r => r.ObjectId == weenie.ClassId).ToList();
            }

            weenie.WeeniePropertiesSpellBook = context.WeeniePropertiesSpellBook.Where(r => r.ObjectId == weenie.ClassId).ToList();

            weenie.WeeniePropertiesTextureMap = context.WeeniePropertiesTextureMap.Where(r => r.ObjectId == weenie.ClassId).ToList();

            // If the weenie doesn't exist in the cache, we'll add it.
            weenieCache.TryAdd(weenieClassId, weenie);
            weeniesNotFound.TryRemove(weenieClassId, out _);

            return weenie;
        }

        /// <summary>
        /// This will populate all sub collections except the followign: LandblockInstances, PointsOfInterest<para />
        /// This will also update the weenie cache.
        /// </summary>
        public Weenie GetWeenie(uint weenieClassId)
        {
            using (var context = new WorldDbContext())
                return GetWeenie(context, weenieClassId);
        }

        public uint GetWeenieClassId(string weenieClassName)
        {
            using (var context = new WorldDbContext())
            {
                var result = context.Weenie
                    .AsNoTracking()
                    .FirstOrDefault(r => r.ClassName == weenieClassName);

                if (result != null)
                    return result.ClassId;

                return 0;
            }
        }

        /// <summary>
        /// This will populate all sub collections except the followign: LandblockInstances, PointsOfInterest<para />
        /// This will also update the weenie cache.
        /// </summary>
        public Weenie GetWeenie(string weenieClassName)
        {
            var weenieClassId = GetWeenieClassId(weenieClassName);

            return GetWeenie(weenieClassId);
        }

        /// <summary>
        /// Returns the number of weenies currently cached.
        /// </summary>
        public int GetWeenieCacheCount()
        {
            return weenieCache.Count;
        }

        public void ClearWeenieCache()
        {
            weenieCache.Clear();
            weeniesNotFound.Clear();
        }

        /// <summary>
        /// Weenies will have all their collections populated except the followign: LandblockInstances, PointsOfInterest
        /// </summary>
        public Weenie GetCachedWeenie(uint weenieClassId)
        {
            if (weenieCache.TryGetValue(weenieClassId, out var value))
                return value;

            if (weeniesNotFound.ContainsKey(weenieClassId))
                return null;

            return GetWeenie(weenieClassId); // This will add the result into the weenieCache
        }

        /// <summary>
        /// Weenies will have all their collections populated except the followign: LandblockInstances, PointsOfInterest
        /// </summary>
        public Weenie GetCachedWeenie(string weenieClassName)
        {
            foreach (var weenie in weenieCache.Values)
            {
                if (weenie.ClassName == weenieClassName)
                    return weenie;
            }

            var weenieClassId = GetWeenieClassId(weenieClassName);

            return GetWeenie(weenieClassId); // This will add the result into the weenieCache
        }

        /// <summary>
        /// This will make sure every weenie in the database has been read and cached.<para />
        /// This function may take 10+ minutes to complete.
        /// </summary>
        public void CacheAllWeenies()
        {
            using (var context = new WorldDbContext())
            {
                var results = context.Weenie
                    .AsNoTracking()
                    .ToList();

                foreach (var result in results)
                {
                    if (weenieCache.ContainsKey(result.ClassId))
                        continue;

                    GetWeenie(context, result.ClassId);
                }
            }
        }

        public List<Weenie> GetRandomWeeniesOfType(int weenieTypeId, int count)
        {
            using (var context = new WorldDbContext())
            {
                var results = context.Weenie
                    .AsNoTracking()
                    .Where(r => r.Type == weenieTypeId)
                    .ToList();

                var rand = new Random();

                var weenies = new List<Weenie>();

                for (int i = 0; i < count; i++)
                {
                    var index = rand.Next(0, results.Count - 1);

                    var weenie = GetCachedWeenie(results[index].ClassId);

                    weenies.Add(weenie);
                }

                return weenies;
            }
        }

        /// <summary>
        /// Weenies will have all their collections populated except the followign: LandblockInstances, PointsOfInterest
        /// </summary>
        public Dictionary<Weenie, List<LandblockInstance>> GetCachedWeenieInstancesByLandblock(ushort landblock)
        {
            var builder = new Dictionary<uint, List<LandblockInstance>>();

            using (var context = new WorldDbContext())
            {
                var results = context.LandblockInstance
                    .Include(r => r.LandblockInstanceLink)
                    .AsNoTracking()
                    .Where(r => r.Landblock == landblock)
                    .ToList();

                foreach (var result in results)
                {
                    if (builder.TryGetValue(result.WeenieClassId, out var value))
                        value.Add(result);
                    else
                        builder[result.WeenieClassId] = new List<LandblockInstance>() { result };
                }
            }

            var ret = new Dictionary<Weenie, List<LandblockInstance>>();

            foreach (var kvp in builder)
                ret[GetCachedWeenie(kvp.Key)] = kvp.Value;

            return ret;
        }


        private readonly ConcurrentDictionary<ushort, List<LandblockInstance>> cachedLandblockInstances = new ConcurrentDictionary<ushort, List<LandblockInstance>>();

        /// <summary>
        /// Returns the number of LandblockInstances currently cached.
        /// </summary>
        public int GetLandblockInstancesCacheCount()
        {
            return cachedLandblockInstances.Count;
        }

        /// <summary>
        /// Weenies will have all their collections populated except the following: LandblockInstances, PointsOfInterest
        /// </summary>
        public List<LandblockInstance> GetCachedInstancesByLandblock(ushort landblock)
        {
            if (cachedLandblockInstances.TryGetValue(landblock, out var value))
                return value;

            using (var context = new WorldDbContext())
            {
                var results = context.LandblockInstance
                    .Include(r => r.LandblockInstanceLink)
                    .AsNoTracking()
                    .Where(r => r.Landblock == landblock)
                    .ToList();

                cachedLandblockInstances.TryAdd(landblock, results.ToList());
            }

            return cachedLandblockInstances[landblock];
        }

        public LandblockInstance GetLandblockInstanceByGuid(uint guid)
        {
            using (var context = new WorldDbContext())
            {
                return context.LandblockInstance
                    .AsNoTracking()
                    .FirstOrDefault(r => r.Guid == guid);
            }
        }


        private readonly ConcurrentDictionary<string, PointsOfInterest> cachedPointsOfInterest = new ConcurrentDictionary<string, PointsOfInterest>();

        /// <summary>
        /// Returns the number of PointsOfInterest currently cached.
        /// </summary>
        public int GetPointsOfInterestCacheCount()
        {
            return cachedPointsOfInterest.Count;
        }

        public PointsOfInterest GetCachedPointOfInterest(string name)
        {
            if (cachedPointsOfInterest.TryGetValue(name.ToLower(), out var value))
                return value;

            using (var context = new WorldDbContext())
            {
                var result = context.PointsOfInterest
                    .AsNoTracking()
                    .FirstOrDefault(r => r.Name.ToLower() == name.ToLower());

                if (result != null)
                {
                    cachedPointsOfInterest[name.ToLower()] = result;
                    return result;
                }
            }

            return null;
        }

        private readonly Dictionary<uint, Dictionary<uint, CookBook>> cookbookCache = new Dictionary<uint, Dictionary<uint, CookBook>>();

        /// <summary>
        /// Returns the number of Recipies currently cached.
        /// </summary>
        public int GetCookbookCacheCount()
        {
            lock (cookbookCache)
                return cookbookCache.Count;
        }

        public CookBook GetCachedCookbook(uint sourceWeenieClassid, uint targetWeenieClassId)
        {
            lock (cookbookCache)
            {
                if (cookbookCache.TryGetValue(sourceWeenieClassid, out var recipiesForSource))
                {
                    if (recipiesForSource.TryGetValue(targetWeenieClassId, out var value))
                        return value;
                }
            }

            using (var context = new WorldDbContext())
            {
                var result = context.CookBook
                    .AsNoTracking()
                    .Include(r => r.Recipe)
                    .Include(r => r.Recipe.RecipeMod)
                        .ThenInclude(r => r.RecipeModsBool)
                    .Include(r => r.Recipe.RecipeMod)
                        .ThenInclude(r => r.RecipeModsDID)
                    .Include(r => r.Recipe.RecipeMod)
                        .ThenInclude(r => r.RecipeModsFloat)
                    .Include(r => r.Recipe.RecipeMod)
                        .ThenInclude(r => r.RecipeModsIID)
                    .Include(r => r.Recipe.RecipeMod)
                        .ThenInclude(r => r.RecipeModsInt)
                    .Include(r => r.Recipe.RecipeMod)
                        .ThenInclude(r => r.RecipeModsString)
                    .Include(r => r.Recipe.RecipeRequirementsBool)
                    .Include(r => r.Recipe.RecipeRequirementsDID)
                    .Include(r => r.Recipe.RecipeRequirementsFloat)
                    .Include(r => r.Recipe.RecipeRequirementsIID)
                    .Include(r => r.Recipe.RecipeRequirementsInt)
                    .Include(r => r.Recipe.RecipeRequirementsString)
                    .FirstOrDefault(r => r.SourceWCID == sourceWeenieClassid && r.TargetWCID == targetWeenieClassId);

                lock (cookbookCache)
                {
                    // We double check before commiting the recipe.
                    // We could be in this lock, and queued up behind us is an attempt to add a result for the same source:target pair.
                    if (cookbookCache.TryGetValue(sourceWeenieClassid, out var sourceRecipies))
                    {
                        if (!sourceRecipies.ContainsKey(targetWeenieClassId))
                            sourceRecipies.Add(targetWeenieClassId, result);
                    }
                    else
                        cookbookCache.Add(sourceWeenieClassid, new Dictionary<uint, CookBook>() { { targetWeenieClassId, result } });
                }

                return result;
            }
        }

        private readonly ConcurrentDictionary<uint, Spell> spellCache = new ConcurrentDictionary<uint, Spell>();

        /// <summary>
        /// Returns the number of Spells currently cached.
        /// </summary>
        public int GetSpellCacheCount()
        {
            return spellCache.Count;
        }

        public Spell GetCachedSpell(uint spellId)
        {
            if (spellCache.TryGetValue(spellId, out var spell))
                return spell;

            using (var context = new WorldDbContext())
            {
                var result = context.Spell
                    .AsNoTracking()
                    .FirstOrDefault(r => r.Id == spellId);

                if (result != null)
                {
                    spellCache[spellId] = result;
                    return result;
                }
            }

            return null;
        }


        private readonly ConcurrentDictionary<ushort, List<Encounter>> cachedEncounters = new ConcurrentDictionary<ushort, List<Encounter>>();

        /// <summary>
        /// Returns the number of Encounters currently cached.
        /// </summary>
        public int GetEncounterCacheCount()
        {
            return cachedEncounters.Count;
        }

        public List<Encounter> GetCachedEncountersByLandblock(ushort landblock)
        {
            if (cachedEncounters.TryGetValue(landblock, out var value))
                return value;

            using (var context = new WorldDbContext())
            {
                var results = context.Encounter
                    .AsNoTracking()
                    .Where(r => r.Landblock == landblock)
                    .ToList();

                cachedEncounters.TryAdd(landblock, results.ToList());
            }

            return cachedEncounters[landblock];
        }

        private readonly ConcurrentDictionary<string, Event> cachedEvents = new ConcurrentDictionary<string, Event>();

        /// <summary>
        /// Returns the number of Events currently cached.
        /// </summary>
        public int GetEventsCacheCount()
        {
            return cachedEvents.Count;
        }

        public Event GetCachedEvent(string name)
        {
            if (cachedEvents.TryGetValue(name.ToLower(), out var value))
                return value;

            using (var context = new WorldDbContext())
            {
                var result = context.Event
                    .AsNoTracking()
                    .FirstOrDefault(r => r.Name.ToLower() == name.ToLower());

                if (result != null)
                {
                    cachedEvents[name.ToLower()] = result;
                    return result;
                }
            }

            return null;
        }

        public List<Event> GetAllEvents()
        {
            using (var context = new WorldDbContext())
            {
                var results = context.Event
                    .AsNoTracking();

                if (results != null)
                {
                    foreach(var result in results)
                    {
                        cachedEvents[result.Name.ToLower()] = result;
                    }

                    return results.ToList();
                }
            }

            return null;
        }

        private readonly ConcurrentDictionary<uint, TreasureDeath> cachedDeathTreasure = new ConcurrentDictionary<uint, TreasureDeath>();

        /// <summary>
        /// Returns the number of TreasureDeath currently cached.
        /// </summary>
        public int GetDeathTreasureCacheCount()
        {
            return cachedDeathTreasure.Count;
        }

        public TreasureDeath GetCachedDeathTreasure(uint dataId)
        {
            if (cachedDeathTreasure.TryGetValue(dataId, out var value))
                return value;

            using (var context = new WorldDbContext())
            {
                var result = context.TreasureDeath
                    .AsNoTracking()
                    .FirstOrDefault(r => r.TreasureType == dataId);

                if (result != null)
                {
                    cachedDeathTreasure[dataId] = result;
                    return result;
                }
            }

            return null;
        }

        private readonly ConcurrentDictionary<uint, List<TreasureWielded>> cachedWieldedTreasure = new ConcurrentDictionary<uint, List<TreasureWielded>>();

        /// <summary>
        /// Returns the number of TreasureWielded currently cached.
        /// </summary>
        public int GetWieldedTreasureCacheCount()
        {
            return cachedWieldedTreasure.Count;
        }

        public List<TreasureWielded> GetCachedWieldedTreasure(uint dataId)
        {
            if (cachedWieldedTreasure.TryGetValue(dataId, out var value))
                return value;

            using (var context = new WorldDbContext())
            {
                var results = context.TreasureWielded
                    .AsNoTracking()
                    .Where(r => r.TreasureType == dataId)
                    .ToList();

                cachedWieldedTreasure[dataId] = results;
                return results;
            }
        }
    }
}
