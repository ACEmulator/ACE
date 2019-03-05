using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

using log4net;

using ACE.Database.Entity;
using ACE.Database.Models.World;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

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


        private readonly ConcurrentDictionary<uint, Weenie> weenieCache = new ConcurrentDictionary<uint, Weenie>();

        /// <summary>
        /// This will populate all sub collections except the following: LandblockInstances, PointsOfInterest<para />
        /// This will also update the weenie cache.
        /// </summary>
        public Weenie GetWeenie(WorldDbContext context, uint weenieClassId)
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
                weenieCache[weenieClassId] = null;
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
            weenieCache[weenieClassId] = weenie;

            return weenie;
        }

        /// <summary>
        /// This will populate all sub collections except the following: LandblockInstances, PointsOfInterest<para />
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
        /// This will populate all sub collections except the following: LandblockInstances, PointsOfInterest<para />
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
            return weenieCache.Count(r => r.Value != null);
        }

        public void ClearWeenieCache()
        {
            weenieCache.Clear();
        }

        /// <summary>
        /// Weenies will have all their collections populated except the following: LandblockInstances, PointsOfInterest
        /// </summary>
        public Weenie GetCachedWeenie(uint weenieClassId)
        {
            if (weenieCache.TryGetValue(weenieClassId, out var value))
                return value;

            return GetWeenie(weenieClassId); // This will add the result into the weenieCache
        }

        /// <summary>
        /// Weenies will have all their collections populated except the following: LandblockInstances, PointsOfInterest
        /// </summary>
        public Weenie GetCachedWeenie(string weenieClassName)
        {
            foreach (var weenie in weenieCache.Values)
            {
                if (weenie != null && weenie.ClassName == weenieClassName)
                    return weenie;
            }

            var weenieClassId = GetWeenieClassId(weenieClassName);

            return GetWeenie(weenieClassId); // This will add the result into the weenieCache
        }

        private readonly ConcurrentDictionary<int, List<Weenie>> weenieCacheByType = new ConcurrentDictionary<int, List<Weenie>>();

        public List<Weenie> GetRandomWeeniesOfType(int weenieTypeId, int count)
        {
            if (!weenieCacheByType.TryGetValue(weenieTypeId, out var weenies))
            {
                if (!weenieSpecificCachesPopulated)
                {
                    using (var context = new WorldDbContext())
                    {
                        var results = context.Weenie
                            .AsNoTracking()
                            .Where(r => r.Type == weenieTypeId)
                            .ToList();

                        var rand = new Random();

                        weenies = new List<Weenie>();

                        for (int i = 0; i < count; i++)
                        {
                            var index = rand.Next(0, results.Count - 1);

                            var weenie = GetCachedWeenie(results[index].ClassId);

                            weenies.Add(weenie);
                        }

                        return weenies;
                    }
                }

                weenies = new List<Weenie>();
                weenieCacheByType[weenieTypeId] = weenies;
            }

            if (weenies.Count == 0)
                return new List<Weenie>();

            {
                var rand = new Random();

                var results = new List<Weenie>();

                for (int i = 0; i < count; i++)
                {
                    var index = rand.Next(0, weenies.Count - 1);

                    var weenie = GetCachedWeenie(weenies[index].ClassId);

                    results.Add(weenie);
                }

                return results;
            }
        }

        private readonly Dictionary<uint, Weenie> scrollsBySpellID = new Dictionary<uint, Weenie>();

        public Weenie GetScrollWeenie(uint spellID)
        {
            if (!scrollsBySpellID.TryGetValue(spellID, out var weenie))
            {
                if (!weenieSpecificCachesPopulated)
                {
                    using (var context = new WorldDbContext())
                    {
                        var query = from weenieRecord in context.Weenie
                            join did in context.WeeniePropertiesDID on weenieRecord.ClassId equals did.ObjectId
                            where weenieRecord.Type == (int)WeenieType.Scroll && did.Type == (ushort)PropertyDataId.Spell && did.Value == spellID
                            select weenieRecord;

                        weenie = query.FirstOrDefault();

                        scrollsBySpellID[spellID] = weenie;
                    }
                }
            }

            return weenie;
        }

        /// <summary>
        /// This will make sure every weenie in the database has been read and cached.<para />
        /// This function may take 15+ minutes to complete.
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

            PopulateWeenieSpecificCaches();
        }

        /// <summary>
        /// This will make sure every weenie in the database has been read and cached.<para />
        /// This function may take 2+ minutes to complete.
        /// </summary>
        public void CacheAllWeeniesInParallel()
        {
            using (var context = new WorldDbContext())
            {
                var results = context.Weenie
                    .AsNoTracking()
                    .ToList();

                Parallel.ForEach(results, result =>
                {
                    if (!weenieCache.ContainsKey(result.ClassId))
                        GetWeenie(result.ClassId);
                });
            }

            PopulateWeenieSpecificCaches();
        }

        private bool weenieSpecificCachesPopulated;

        private void PopulateWeenieSpecificCaches()
        {
            // populate weenieCacheByType
            foreach (var weenie in weenieCache.Values)
            {
                if (weenie == null)
                    continue;

                if (!weenieCacheByType.TryGetValue(weenie.Type, out var weenies))
                {
                    weenies = new List<Weenie>();
                    weenieCacheByType[weenie.Type] = weenies;
                }

                if (!weenies.Contains(weenie))
                    weenies.Add(weenie);
            }

            // populate scrollsBySpellID
            foreach (var weenie in weenieCache.Values)
            {
                if (weenie == null)
                    continue;

                if (weenie.Type == (int)WeenieType.Scroll)
                {
                    foreach (var record in weenie.WeeniePropertiesDID)
                    {
                        if (record.Type == (ushort)PropertyDataId.Spell)
                        {
                            scrollsBySpellID[record.Value] = weenie;
                            break;
                        }
                    }
                }
            }

            weenieSpecificCachesPopulated = true;
        }

        /// <summary>
        /// Weenies will have all their collections populated except the following: LandblockInstances, PointsOfInterest
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
            return cachedLandblockInstances.Count(r => r.Value != null);
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
                    .Include(r => r.LandblockInstanceLink)
                    .AsNoTracking()
                    .FirstOrDefault(r => r.Guid == guid);
            }
        }

        public List<LandblockInstance> GetLandblockInstancesByWcid(uint wcid)
        {
            using (var context = new WorldDbContext())
            {
                return context.LandblockInstance
                    .Include(r => r.LandblockInstanceLink)
                    .AsNoTracking()
                    .Where(i => i.WeenieClassId == wcid)
                    .ToList();
            }
        }


        public List<HouseListResults> GetHousesAll()
        {
            using (var context = new WorldDbContext())
            {
                var query = from weenie in context.Weenie
                            join winst in context.LandblockInstance on weenie.ClassId equals winst.WeenieClassId
                            where weenie.Type == (int)WeenieType.SlumLord
                            select new HouseListResults(weenie, winst);

                return query.ToList();
            }
        }

        private readonly ConcurrentDictionary<uint, List<HousePortal>> cachedHousePortals = new ConcurrentDictionary<uint, List<HousePortal>>();

        public List<HousePortal> GetCachedHousePortals(uint houseId)
        {
            if (cachedHousePortals.TryGetValue(houseId, out var value))
                return value;

            using (var context = new WorldDbContext())
            {
                var results = context.HousePortal
                    .AsNoTracking()
                    .Where(p => p.HouseId == houseId)
                    .ToList();

                cachedHousePortals[houseId] = results;

                return results;
            }
        }

        /// <summary>
        /// This takes under ? second to complete.
        /// </summary>
        public void CacheAllHousePortals()
        {
            using (var context = new WorldDbContext())
            {
                var results = context.HousePortal
                    .AsNoTracking()
                    .GroupBy(r => r.HouseId)
                    .ToList();

                foreach (var result in results)
                    cachedHousePortals[result.Key] = result.ToList();
            }
        }

        private readonly ConcurrentDictionary<uint, List<HousePortal>> cachedHousePortalsByLandblock = new ConcurrentDictionary<uint, List<HousePortal>>();

        public List<HousePortal> GetCachedHousePortalsByLandblock(uint landblockId)
        {
            if (cachedHousePortalsByLandblock.TryGetValue(landblockId, out var value))
                return value;

            using (var context = new WorldDbContext())
            {
                var results = context.HousePortal
                    .AsNoTracking()
                    .Where(p => landblockId == p.ObjCellId >> 16)
                    .ToList();

                cachedHousePortalsByLandblock[landblockId] = results;

                return results;
            }
        }


        private readonly ConcurrentDictionary<string, PointsOfInterest> cachedPointsOfInterest = new ConcurrentDictionary<string, PointsOfInterest>();

        /// <summary>
        /// Returns the number of PointsOfInterest currently cached.
        /// </summary>
        public int GetPointsOfInterestCacheCount()
        {
            return cachedPointsOfInterest.Count(r => r.Value != null);
        }

        /// <summary>
        /// Returns the PointsOfInterest cache.
        /// </summary>
        public ConcurrentDictionary<string, PointsOfInterest> GetPointsOfInterestCache()
        {
            return new ConcurrentDictionary<string, PointsOfInterest>(cachedPointsOfInterest);
        }

        public PointsOfInterest GetCachedPointOfInterest(string name)
        {
            var nameToLower = name.ToLower();

            if (cachedPointsOfInterest.TryGetValue(nameToLower, out var value))
                return value;

            using (var context = new WorldDbContext())
            {
                var result = context.PointsOfInterest
                    .AsNoTracking()
                    .FirstOrDefault(r => r.Name.ToLower() == nameToLower);

                cachedPointsOfInterest[nameToLower] = result;
                return result;
            }
        }

        /// <summary>
        /// Retrieves all points of interest from the database and adds/updates the points of interest cache entries with every point of interest retrieved.
        /// 57 entries cached in 00:00:00.0057937
        /// </summary>
        public void CacheAllPointsOfInterest()
        {
            using (var context = new WorldDbContext())
            {
                var results = context.PointsOfInterest
                    .AsNoTracking()
                    .ToList();

                foreach (var result in results)
                    cachedPointsOfInterest[result.Name.ToLower()] = result;
            }
        }

        private readonly Dictionary<uint, Dictionary<uint, CookBook>> cookbookCache = new Dictionary<uint, Dictionary<uint, CookBook>>();

        /// <summary>
        /// Returns the number of Cookbooks currently cached.
        /// </summary>
        public int GetCookbookCacheCount()
        {
            lock (cookbookCache)
                return cookbookCache.Count(r => r.Value != null);
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

        /// <summary>
        /// This can take 1-2 minutes to complete.
        /// </summary>
        public void CacheAllCookbooksInParallel()
        {
            using (var context = new WorldDbContext())
            {
                var results = context.CookBook
                    .AsNoTracking()
                    .ToList();

                Parallel.ForEach(results, result =>
                {
                    GetCachedCookbook(result.SourceWCID, result.TargetWCID);
                });
            }
        }

        public Weenie GetScrollBySpellID(uint spellID)
        {
            using (var context = new WorldDbContext())
            {
                var result = from weenie in context.Weenie
                             join did in context.WeeniePropertiesDID on weenie.ClassId equals did.ObjectId
                             where weenie.Type == 34 && did.Type == 28 && did.Value == spellID
                             select weenie;
                return result.FirstOrDefault();
                //return result.FirstOrDefault().ClassName;
            }
        }

        private readonly ConcurrentDictionary<uint, Spell> spellCache = new ConcurrentDictionary<uint, Spell>();

        /// <summary>
        /// Returns the number of Spells currently cached.
        /// </summary>
        public int GetSpellCacheCount()
        {
            return spellCache.Count(r => r.Value != null);
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

                spellCache[spellId] = result;
                return result;
            }
        }

        /// <summary>
        /// This takes under 1 second to complete.
        /// </summary>
        public void CacheAllSpells()
        {
            using (var context = new WorldDbContext())
            {
                var results = context.Spell
                    .AsNoTracking()
                    .ToList();

                foreach (var result in results)
                    spellCache[result.Id] = result;
            }
        }


        private readonly ConcurrentDictionary<ushort, List<Encounter>> cachedEncounters = new ConcurrentDictionary<ushort, List<Encounter>>();

        /// <summary>
        /// Returns the number of Encounters currently cached.
        /// </summary>
        public int GetEncounterCacheCount()
        {
            return cachedEncounters.Count(r => r.Value != null);
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

                cachedEncounters.TryAdd(landblock, results);
                return results;
            }
        }

        private readonly ConcurrentDictionary<string, Event> cachedEvents = new ConcurrentDictionary<string, Event>();

        /// <summary>
        /// Returns the number of Events currently cached.
        /// </summary>
        public int GetEventsCacheCount()
        {
            return cachedEvents.Count(r => r.Value != null);
        }

        public Event GetCachedEvent(string name)
        {
            var nameToLower = name.ToLower();

            if (cachedEvents.TryGetValue(nameToLower, out var value))
                return value;

            using (var context = new WorldDbContext())
            {
                var result = context.Event
                    .AsNoTracking()
                    .FirstOrDefault(r => r.Name.ToLower() == nameToLower);

                cachedEvents[nameToLower] = result;
                return result;
            }
        }

        /// <summary>
        /// This takes under 1 second to complete.
        /// </summary>
        public List<Event> GetAllEvents()
        {
            using (var context = new WorldDbContext())
            {
                var results = context.Event
                    .AsNoTracking()
                    .ToList();

                foreach (var result in results)
                    cachedEvents[result.Name.ToLower()] = result;

                return results;
            }
        }

        private readonly ConcurrentDictionary<uint, TreasureDeath> cachedDeathTreasure = new ConcurrentDictionary<uint, TreasureDeath>();

        /// <summary>
        /// Returns the number of TreasureDeath currently cached.
        /// </summary>
        public int GetDeathTreasureCacheCount()
        {
            return cachedDeathTreasure.Count(r => r.Value != null);
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

                cachedDeathTreasure[dataId] = result;
                return result;
            }
        }

        /// <summary>
        /// This takes under 1 second to complete.
        /// </summary>
        public void CacheAllDeathTreasures()
        {
            using (var context = new WorldDbContext())
            {
                var results = context.TreasureDeath
                    .AsNoTracking()
                    .ToList();

                foreach (var result in results)
                    cachedDeathTreasure[result.TreasureType] = result;
            }
        }

        private readonly ConcurrentDictionary<uint, List<TreasureWielded>> cachedWieldedTreasure = new ConcurrentDictionary<uint, List<TreasureWielded>>();

        /// <summary>
        /// Returns the number of TreasureWielded currently cached.
        /// </summary>
        public int GetWieldedTreasureCacheCount()
        {
            return cachedWieldedTreasure.Count(r => r.Value != null);
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

        /// <summary>
        /// This takes under 1 second to complete.
        /// </summary>
        public void CacheAllWieldedTreasuresInParallel()
        {
            using (var context = new WorldDbContext())
            {
                var results = context.TreasureWielded
                    .AsNoTracking()
                    .GroupBy(r => r.TreasureType)
                    .ToList();

                foreach (var result in results)
                    cachedWieldedTreasure[result.Key] = result.ToList();
            }
        }

        private readonly ConcurrentDictionary<string, Quest> cachedQuest = new ConcurrentDictionary<string, Quest>();

        public Quest GetCachedQuest(string questName)
        {
            if (cachedQuest.TryGetValue(questName, out var quest))
                return quest;

            using (var context = new WorldDbContext())
            {
                quest = context.Quest.FirstOrDefault(q => q.Name.Equals(questName));
                cachedQuest[questName] = quest;

                return quest;
            }
        }
    }
}
