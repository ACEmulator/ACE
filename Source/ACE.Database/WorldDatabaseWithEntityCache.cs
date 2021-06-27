using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using ACE.Common;
using ACE.Database.Adapter;
using ACE.Database.Models.World;
using ACE.Database.Extensions;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

namespace ACE.Database
{
    public class WorldDatabaseWithEntityCache : WorldDatabase
    {
        // =====================================
        // Weenie
        // =====================================

        private readonly ConcurrentDictionary<uint /* WCID */, ACE.Entity.Models.Weenie> weenieCache = new ConcurrentDictionary<uint /* WCID */, ACE.Entity.Models.Weenie>();

        private readonly ConcurrentDictionary<string /* Class Name */, uint /* WCID */> weenieClassNameToClassIdCache = new ConcurrentDictionary<string, uint>();

        /// <summary>
        /// This will populate all sub collections except the following: LandblockInstances, PointsOfInterest<para />
        /// This will also update the weenie cache.
        /// </summary>
        public override Weenie GetWeenie(WorldDbContext context, uint weenieClassId)
        {
            var weenie = base.GetWeenie(context, weenieClassId);

            // If the weenie doesn't exist in the cache, we'll add it.
            if (weenie != null)
            {
                weenieCache[weenieClassId] = WeenieConverter.ConvertToEntityWeenie(weenie);
                weenieClassNameToClassIdCache[weenie.ClassName.ToLower()] = weenie.ClassId;
            }
            else
                weenieCache[weenieClassId] = null;

            return weenie;
        }

        /// <summary>
        /// This will populate all sub collections except the following: LandblockInstances, PointsOfInterest<para />
        /// This will also update the weenie cache.
        /// </summary>
        public override List<Weenie> GetAllWeenies()
        {
            var weenies = base.GetAllWeenies();

            // Add the weenies to the cache
            foreach (var weenie in weenies)
            {
                weenieCache[weenie.ClassId] = WeenieConverter.ConvertToEntityWeenie(weenie);
                weenieClassNameToClassIdCache[weenie.ClassName.ToLower()] = weenie.ClassId;
            }

            return weenies;
        }


        /// <summary>
        /// This will make sure every weenie in the database has been read and cached.<para />
        /// This function may take 10+ seconds to complete.
        /// </summary>
        public void CacheAllWeenies()
        {
            GetAllWeenies();

            PopulateWeenieSpecificCaches();
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
            weenieClassNameToClassIdCache.Clear();

            weenieSpecificCachesPopulated = false;
        }

        /// <summary>
        /// Weenies will have all their collections populated except the following: LandblockInstances, PointsOfInterest
        /// </summary>
        public ACE.Entity.Models.Weenie GetCachedWeenie(uint weenieClassId)
        {
            if (weenieCache.TryGetValue(weenieClassId, out var value))
                return value;

            GetWeenie(weenieClassId); // This will add the result into the caches

            weenieCache.TryGetValue(weenieClassId, out value);

            return value;
        }

        /// <summary>
        /// Weenies will have all their collections populated except the following: LandblockInstances, PointsOfInterest
        /// </summary>
        public ACE.Entity.Models.Weenie GetCachedWeenie(string weenieClassName)
        {
            if (weenieClassNameToClassIdCache.TryGetValue(weenieClassName.ToLower(), out var value))
                return GetCachedWeenie(value); // This will add the result into the caches

            GetWeenie(weenieClassName); // This will add the result into the caches

            weenieClassNameToClassIdCache.TryGetValue(weenieClassName.ToLower(), out value);

            return GetCachedWeenie(value); // This will add the result into the caches
        }

        public bool ClearCachedWeenie(uint weenieClassId)
        {
            return weenieCache.TryRemove(weenieClassId, out _);
        }



        private bool weenieSpecificCachesPopulated;

        private readonly ConcurrentDictionary<WeenieType, List<ACE.Entity.Models.Weenie>> weenieCacheByType = new ConcurrentDictionary<WeenieType, List<ACE.Entity.Models.Weenie>>();

        private readonly Dictionary<uint /* Spell ID */, ACE.Entity.Models.Weenie> scrollsBySpellID = new Dictionary<uint, ACE.Entity.Models.Weenie>();

        private void PopulateWeenieSpecificCaches()
        {
            // populate weenieCacheByType
            foreach (var weenie in weenieCache.Values)
            {
                if (weenie == null)
                    continue;

                if (!weenieCacheByType.TryGetValue(weenie.WeenieType, out var weenies))
                {
                    weenies = new List<ACE.Entity.Models.Weenie>();
                    weenieCacheByType[weenie.WeenieType] = weenies;
                }

                if (!weenies.Contains(weenie))
                    weenies.Add(weenie);
            }

            // populate scrollsBySpellID
            foreach (var weenie in weenieCache.Values)
            {
                if (weenie == null)
                    continue;

                if (weenie.WeenieType == WeenieType.Scroll)
                {
                    if (weenie.PropertiesDID.TryGetValue(PropertyDataId.Spell, out var value))
                        scrollsBySpellID[value] = weenie;
                }
            }

            weenieSpecificCachesPopulated = true;
        }

        public List<ACE.Entity.Models.Weenie> GetRandomWeeniesOfType(int weenieTypeId, int count)
        {
            if (!weenieCacheByType.TryGetValue((WeenieType)weenieTypeId, out var weenies))
            {
                if (!weenieSpecificCachesPopulated)
                {
                    using (var context = new WorldDbContext())
                    {
                        var results = context.Weenie
                            .AsNoTracking()
                            .Where(r => r.Type == weenieTypeId)
                            .ToList();

                        weenies = new List<ACE.Entity.Models.Weenie>();

                        if (results.Count == 0)
                            return weenies;

                        for (int i = 0; i < count; i++)
                        {
                            var index = ThreadSafeRandom.Next(0, results.Count - 1);

                            var weenie = GetCachedWeenie(results[index].ClassId);

                            weenies.Add(weenie);
                        }

                        return weenies;
                    }
                }

                weenies = new List<ACE.Entity.Models.Weenie>();
                weenieCacheByType[(WeenieType)weenieTypeId] = weenies;
            }

            if (weenies.Count == 0)
                return new List<ACE.Entity.Models.Weenie>();

            {
                var results = new List<ACE.Entity.Models.Weenie>();

                for (int i = 0; i < count; i++)
                {
                    var index = ThreadSafeRandom.Next(0, weenies.Count - 1);

                    var weenie = GetCachedWeenie(weenies[index].WeenieClassId);

                    results.Add(weenie);
                }

                return results;
            }
        }

        public ACE.Entity.Models.Weenie GetScrollWeenie(uint spellID)
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

                        var result = query.FirstOrDefault();

                        if (result == null) return null;

                        weenie = WeenieConverter.ConvertToEntityWeenie(result);

                        scrollsBySpellID[spellID] = weenie;
                    }
                }
            }

            return weenie;
        }

        private readonly ConcurrentDictionary<string, uint> creatureWeenieNamesLowerInvariantCache = new ConcurrentDictionary<string, uint>();

        public bool IsCreatureNameInWorldDatabase(string name)
        {
            if (creatureWeenieNamesLowerInvariantCache.TryGetValue(name.ToLowerInvariant(), out _))
                return true;

            using (var context = new WorldDbContext())
            {
                return IsCreatureNameInWorldDatabase(context, name);
            }
        }

        public bool IsCreatureNameInWorldDatabase(WorldDbContext context, string name)
        {
            var query = from weenieRecord in context.Weenie
                        join stringProperty in context.WeeniePropertiesString on weenieRecord.ClassId equals stringProperty.ObjectId
                        where weenieRecord.Type == (int)WeenieType.Creature && stringProperty.Type == (ushort)PropertyString.Name && stringProperty.Value.Equals(name, StringComparison.InvariantCultureIgnoreCase)
                        select weenieRecord;

            var weenie = query
                .Include(r => r.WeeniePropertiesString)
                .AsNoTracking()
                .FirstOrDefault();

            if (weenie == null)
                return false;

            var weenieName = weenie.GetProperty(PropertyString.Name).ToLowerInvariant();

            creatureWeenieNamesLowerInvariantCache.TryAdd(weenieName, weenie.ClassId);

            return true;
        }


        // =====================================
        // CookBook
        // =====================================

        private readonly Dictionary<uint /* source WCID */, Dictionary<uint /* target WCID */, CookBook>> cookbookCache = new Dictionary<uint, Dictionary<uint, CookBook>>();

        public override CookBook GetCookbook(WorldDbContext context, uint sourceWeenieClassId, uint targetWeenieClassId)
        {
            var cookbook = base.GetCookbook(context, sourceWeenieClassId, targetWeenieClassId);

            lock (cookbookCache)
            {
                // We double check before commiting the recipe.
                // We could be in this lock, and queued up behind us is an attempt to add a result for the same source:target pair.
                if (cookbookCache.TryGetValue(sourceWeenieClassId, out var sourceRecipes))
                {
                    if (!sourceRecipes.ContainsKey(targetWeenieClassId))
                        sourceRecipes.Add(targetWeenieClassId, cookbook);
                }
                else
                    cookbookCache.Add(sourceWeenieClassId, new Dictionary<uint, CookBook>() { { targetWeenieClassId, cookbook } });
            }

            return cookbook;
        }

        public override List<CookBook> GetAllCookbooks()
        {
            var cookbooks = base.GetAllCookbooks();

            // Add the cookbooks to the cache
            lock (cookbookCache)
            {
                foreach (var cookbook in cookbooks)
                {
                    // We double check before commiting the recipe.
                    // We could be in this lock, and queued up behind us is an attempt to add a result for the same source:target pair.
                    if (cookbookCache.TryGetValue(cookbook.SourceWCID, out var sourceRecipes))
                    {
                        if (!sourceRecipes.ContainsKey(cookbook.TargetWCID))
                            sourceRecipes.Add(cookbook.TargetWCID, cookbook);
                    }
                    else
                        cookbookCache.Add(cookbook.SourceWCID, new Dictionary<uint, CookBook>() { { cookbook.TargetWCID, cookbook } });
                }
            }

            return cookbooks;
        }


        /// <summary>
        /// This can take 1-2 minutes to complete.
        /// </summary>
        public void CacheAllCookbooks()
        {
            GetAllCookbooks();
        }

        /// <summary>
        /// Returns the number of Cookbooks currently cached.
        /// </summary>
        public int GetCookbookCacheCount()
        {
            lock (cookbookCache)
                return cookbookCache.Count(r => r.Value != null);
        }

        public void ClearCookbookCache()
        {
            lock (cookbookCache)
                cookbookCache.Clear();
        }

        public CookBook GetCachedCookbook(uint sourceWeenieClassId, uint targetWeenieClassId)
        {
            lock (cookbookCache)
            {
                if (cookbookCache.TryGetValue(sourceWeenieClassId, out var recipesForSource))
                {
                    if (recipesForSource.TryGetValue(targetWeenieClassId, out var value))
                        return value;
                }
            }

            return GetCookbook(sourceWeenieClassId, targetWeenieClassId);  // This will add the result into the cache
        }


        // =====================================
        // Encounter
        // =====================================

        private readonly ConcurrentDictionary<ushort /* Landblock */, List<Encounter>> cachedEncounters = new ConcurrentDictionary<ushort, List<Encounter>>();

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

        public bool ClearCachedEncountersByLandblock(ushort landblock)
        {
            return cachedEncounters.TryRemove(landblock, out _);
        }


        // =====================================
        // Event
        // =====================================

        private readonly ConcurrentDictionary<string /* Event Name */, Event> cachedEvents = new ConcurrentDictionary<string, Event>();

        public override List<Event> GetAllEvents(WorldDbContext context)
        {
            var events = base.GetAllEvents(context);

            foreach (var result in events)
                cachedEvents[result.Name.ToLower()] = result;

            return events;
        }

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


        // =====================================
        // HousePortal
        // =====================================

        private readonly ConcurrentDictionary<uint /* House ID */, List<HousePortal>> cachedHousePortals = new ConcurrentDictionary<uint, List<HousePortal>>();

        /// <summary>
        /// This takes under ? second to complete.
        /// </summary>
        public void CacheAllHousePortals()
        {
            using (var context = new WorldDbContext())
            {
                var results = context.HousePortal
                    .AsNoTracking()
                    .AsEnumerable()
                    .GroupBy(r => r.HouseId);

                foreach (var result in results)
                    cachedHousePortals[result.Key] = result.ToList();
            }
        }

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


        private readonly ConcurrentDictionary<uint /* Landblock */, List<HousePortal>> cachedHousePortalsByLandblock = new ConcurrentDictionary<uint, List<HousePortal>>();

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


        // =====================================
        // LandblockInstance
        // =====================================

        private readonly ConcurrentDictionary<ushort /* Landblock */, List<LandblockInstance>> cachedLandblockInstances = new ConcurrentDictionary<ushort, List<LandblockInstance>>();

        /// <summary>
        /// Returns the number of LandblockInstances currently cached.
        /// </summary>
        public int GetLandblockInstancesCacheCount()
        {
            return cachedLandblockInstances.Count(r => r.Value != null);
        }

        /// <summary>
        /// Clears the cached landblock instances for all landblocks
        /// </summary>
        public void ClearCachedLandblockInstances()
        {
            cachedLandblockInstances.Clear();
        }

        /// <summary>
        /// Clears the cached landblock instances for a specific landblock
        /// </summary>
        public bool ClearCachedInstancesByLandblock(ushort landblock)
        {
            return cachedLandblockInstances.TryRemove(landblock, out _);
        }

        public List<LandblockInstance> GetCachedInstancesByLandblock(WorldDbContext context, ushort landblock)
        {
            if (cachedLandblockInstances.TryGetValue(landblock, out var value))
                return value;

            var results = context.LandblockInstance
                .Include(r => r.LandblockInstanceLink)
                .AsNoTracking()
                .Where(r => r.Landblock == landblock)
                .ToList();

            cachedLandblockInstances.TryAdd(landblock, results.ToList());

            return cachedLandblockInstances[landblock];
        }

        /// <summary>
        /// Returns statics spawn map and their links for the landblock
        /// </summary>
        public List<LandblockInstance> GetCachedInstancesByLandblock(ushort landblock)
        {
            using (var context = new WorldDbContext())
                return GetCachedInstancesByLandblock(context, landblock);
        }


        private readonly ConcurrentDictionary<ushort /* Landblock */, uint /* House GUID */> cachedBasementHouseGuids = new ConcurrentDictionary<ushort, uint>();

        public uint GetCachedBasementHouseGuid(ushort landblock)
        {
            if (cachedBasementHouseGuids.TryGetValue(landblock, out var value))
                return value;

            using (var context = new WorldDbContext())
            {
                var result = context.LandblockInstance
                    .AsNoTracking()
                    .Where(r => r.Landblock == landblock
                                && r.WeenieClassId != 11730 /* Exclude House Portal */
                                && r.WeenieClassId != 278   /* Exclude Door */
                                && r.WeenieClassId != 568   /* Exclude Door (entry) */
                                && !r.IsLinkChild)
                    .FirstOrDefault();

                if (result == null)
                    return 0;

                cachedBasementHouseGuids[landblock] = result.Guid;

                return result.Guid;
            }
        }


        // =====================================
        // PointsOfInterest
        // =====================================

        private readonly ConcurrentDictionary<string, PointsOfInterest> cachedPointsOfInterest = new ConcurrentDictionary<string, PointsOfInterest>();

        /// <summary>
        /// Retrieves all points of interest from the database and adds/updates the points of interest cache entries with every point of interest retrieved.
        /// 57 entries cached in 00:00:00.0057937
        /// </summary>
        public void CacheAllPointsOfInterest()
        {
            using (var context = new WorldDbContext())
            {
                var results = context.PointsOfInterest
                    .AsNoTracking();

                foreach (var result in results)
                    cachedPointsOfInterest[result.Name.ToLower()] = result;
            }
        }

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


        // =====================================
        // Quest
        // =====================================

        private readonly ConcurrentDictionary<string, Quest> cachedQuest = new ConcurrentDictionary<string, Quest>();

        public bool ClearCachedQuest(string questName)
        {
            return cachedQuest.TryRemove(questName, out _);
        }

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


        // =====================================
        // Recipe
        // =====================================


        // =====================================
        // Spell
        // =====================================

        private readonly ConcurrentDictionary<uint /* Spell ID */, Spell> spellCache = new ConcurrentDictionary<uint, Spell>();

        /// <summary>
        /// This takes under 1 second to complete.
        /// </summary>
        public void CacheAllSpells()
        {
            using (var context = new WorldDbContext())
            {
                var results = context.Spell
                    .AsNoTracking();

                foreach (var result in results)
                    spellCache[result.Id] = result;
            }
        }

        /// <summary>
        /// Returns the number of Spells currently cached.
        /// </summary>
        public int GetSpellCacheCount()
        {
            return spellCache.Count(r => r.Value != null);
        }

        public void ClearSpellCache()
        {
            spellCache.Clear();
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



        // =====================================
        // TreasureDeath
        // =====================================

        private readonly ConcurrentDictionary<uint /* Data ID */, TreasureDeath> cachedDeathTreasure = new ConcurrentDictionary<uint, TreasureDeath>();

        /// <summary>
        /// This takes under 1 second to complete.
        /// </summary>
        public void CacheAllTreasuresDeath()
        {
            using (var context = new WorldDbContext())
            {
                var results = context.TreasureDeath
                    .AsNoTracking();

                foreach (var result in results)
                    cachedDeathTreasure[result.TreasureType] = result;
            }
        }

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


        // =====================================
        // TreasureMaterial
        // =====================================

        // The Key is the Material Code (derived from PropertyInt.TsysMaterialData)
        // The Value is a list of all
        private Dictionary<int /* Material Code */, Dictionary<int /* Tier */, List<TreasureMaterialBase>>> cachedTreasureMaterialBase;
        
        public void CacheAllTreasureMaterialBase()
        {
            using (var context = new WorldDbContext())
            {
                var table = new Dictionary<int, Dictionary<int, List<TreasureMaterialBase>>>();

                var results = context.TreasureMaterialBase.Where(i => i.Probability > 0).ToList();

                foreach (var result in results)
                {
                    if (!table.TryGetValue((int)result.MaterialCode, out var materialCode))
                    {
                        materialCode = new Dictionary<int, List<TreasureMaterialBase>>();
                        table.Add((int)result.MaterialCode, materialCode);
                    }
                    if (!materialCode.TryGetValue((int)result.Tier, out var chances))
                    {
                        chances = new List<TreasureMaterialBase>();
                        materialCode.Add((int)result.Tier, chances);
                    }
                    chances.Add(result.Clone());
                }
                TreasureMaterialBase_Normalize(table);

                cachedTreasureMaterialBase = table;
            }
        }

        private static readonly float NormalizeEpsilon = 0.00001f;

        private void TreasureMaterialBase_Normalize(Dictionary<int, Dictionary<int, List<TreasureMaterialBase>>> materialBase)
        {
            foreach (var kvp in materialBase)
            {
                var materialCode = kvp.Key;
                var tiers = kvp.Value;

                foreach (var kvp2 in tiers)
                {
                    var tier = kvp2.Key;
                    var list = kvp2.Value;

                    var totalProbability = list.Sum(i => i.Probability);

                    if (Math.Abs(1.0f - totalProbability) < NormalizeEpsilon)
                        continue;

                    //Console.WriteLine($"TotalProbability {totalProbability} found for TreasureMaterialBase {materialCode} tier {tier}");

                    var factor = 1.0f / totalProbability;

                    foreach (var item in list)
                        item.Probability *= factor;

                    /*totalProbability = list.Sum(i => i.Probability);

                    Console.WriteLine($"After: {totalProbability}");*/
                }
            }
        }

        public List<TreasureMaterialBase> GetCachedTreasureMaterialBase(int materialCode, int tier)
        {
            if (cachedTreasureMaterialBase == null)
                CacheAllTreasureMaterialBase();

            if (cachedTreasureMaterialBase.TryGetValue(materialCode, out var tiers) && tiers.TryGetValue(tier, out var treasureMaterialBase))
                return treasureMaterialBase;
            else
                return null;
        }


        private Dictionary<int /* Material ID */, Dictionary<int /* Color Code */, List<TreasureMaterialColor>>> cachedTreasureMaterialColor;
        
        public void CacheAllTreasureMaterialColor()
        {
            using (var context = new WorldDbContext())
            {
                var table = new Dictionary<int, Dictionary<int, List<TreasureMaterialColor>>>();

                var results = context.TreasureMaterialColor.ToList();

                foreach (var result in results)
                {
                    if (!table.TryGetValue((int)result.MaterialId, out var colorCodes))
                    {
                        colorCodes = new Dictionary<int, List<TreasureMaterialColor>>();
                        table.Add((int)result.MaterialId, colorCodes);
                    }
                    if (!colorCodes.TryGetValue((int)result.ColorCode, out var list))
                    {
                        list = new List<TreasureMaterialColor>();
                        colorCodes.Add((int)result.ColorCode, list);
                    }
                    list.Add(result.Clone());
                }

                TreasureMaterialColor_Normalize(table);

                cachedTreasureMaterialColor = table;
            }
        }

        private void TreasureMaterialColor_Normalize(Dictionary<int, Dictionary<int, List<TreasureMaterialColor>>> materialColor)
        {
            foreach (var kvp in materialColor)
            {
                var material = kvp.Key;
                var colorCodes = kvp.Value;

                foreach (var kvp2 in colorCodes)
                {
                    var colorCode = kvp2.Key;
                    var list = kvp2.Value;

                    var totalProbability = list.Sum(i => i.Probability);

                    if (Math.Abs(1.0f - totalProbability) < NormalizeEpsilon)
                        continue;

                    //Console.WriteLine($"TotalProbability {totalProbability} found for TreasureMaterialColor {(MaterialType)material} ColorCode {colorCode}");

                    var factor = 1.0f / totalProbability;

                    foreach (var item in list)
                        item.Probability *= factor;

                    /*totalProbability = list.Sum(i => i.Probability);

                    Console.WriteLine($"After: {totalProbability}");*/
                }
            }
        }

        public List<TreasureMaterialColor> GetCachedTreasureMaterialColors(int materialId, int tsysColorCode)
        {
            if (cachedTreasureMaterialColor == null)
                CacheAllTreasureMaterialColor();

            if (cachedTreasureMaterialColor.TryGetValue(materialId, out var colorCodes) && colorCodes.TryGetValue(tsysColorCode, out var result))
                return result;
            else
                return null;
        }


        // The Key is the Material Group (technically a MaterialId, but more generic...e.g. "Material.Metal", "Material.Cloth", etc.)
        // The Value is a list of all
        private Dictionary<int /* Material Group */, Dictionary<int /* Tier */, List<TreasureMaterialGroups>>> cachedTreasureMaterialGroups;

        public void CacheAllTreasureMaterialGroups()
        {
            using (var context = new WorldDbContext())
            {
                var table = new Dictionary<int, Dictionary<int, List<TreasureMaterialGroups>>>();

                var results = context.TreasureMaterialGroups.ToList();

                foreach (var result in results)
                {
                    if (!table.TryGetValue((int)result.MaterialGroup, out var tiers))
                    {
                        tiers = new Dictionary<int, List<TreasureMaterialGroups>>();
                        table.Add((int)result.MaterialGroup, tiers);
                    }
                    if (!tiers.TryGetValue((int)result.Tier, out var list))
                    {
                        list = new List<TreasureMaterialGroups>();
                        tiers.Add((int)result.Tier, list);
                    }
                    list.Add(result.Clone());
                }
                TreasureMaterialGroups_Normalize(table);

                cachedTreasureMaterialGroups = table;
            }
        }

        private void TreasureMaterialGroups_Normalize(Dictionary<int, Dictionary<int, List<TreasureMaterialGroups>>> materialGroups)
        {
            foreach (var kvp in materialGroups)
            {
                var materialGroup = kvp.Key;
                var tiers = kvp.Value;

                foreach (var kvp2 in tiers)
                {
                    var tier = kvp2.Key;
                    var list = kvp2.Value;

                    var totalProbability = list.Sum(i => i.Probability);

                    if (Math.Abs(1.0f - totalProbability) < NormalizeEpsilon)
                        continue;

                    //Console.WriteLine($"TotalProbability {totalProbability} found for TreasureMaterialGroup {(MaterialType)materialGroup} tier {tier}");

                    var factor = 1.0f / totalProbability;

                    foreach (var item in list)
                        item.Probability *= factor;

                    /*totalProbability = list.Sum(i => i.Probability);

                    Console.WriteLine($"After: {totalProbability}");*/
                }
            }
        }

        public List<TreasureMaterialGroups> GetCachedTreasureMaterialGroup(int materialGroup, int tier)
        {
            if (cachedTreasureMaterialGroups == null)
                CacheAllTreasureMaterialGroups();

            if (cachedTreasureMaterialGroups.TryGetValue(materialGroup, out var tiers) && tiers.TryGetValue(tier, out var treasureMaterialGroup))
                return treasureMaterialGroup;
            else
                return null;
        }


        // =====================================
        // TreasureWielded
        // =====================================

        private readonly ConcurrentDictionary<uint /* Data ID */, List<TreasureWielded>> cachedWieldedTreasure = new ConcurrentDictionary<uint, List<TreasureWielded>>();

        /// <summary>
        /// This takes under 1 second to complete.
        /// </summary>
        public void CacheAllTreasureWielded()
        {
            using (var context = new WorldDbContext())
            {
                var results = context.TreasureWielded
                    .AsNoTracking()
                    .AsEnumerable()
                    .GroupBy(r => r.TreasureType);

                foreach (var result in results)
                    cachedWieldedTreasure[result.Key] = result.ToList();
            }
        }

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

        public void ClearWieldedTreasureCache()
        {
            cachedWieldedTreasure.Clear();
        }
    }
}
