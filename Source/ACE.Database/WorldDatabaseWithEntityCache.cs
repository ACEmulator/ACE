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

                Parallel.ForEach(results, ConfigManager.Config.Server.Threading.DatabaseParallelOptions, result =>
                {
                    if (!weenieCache.ContainsKey(result.ClassId))
                        GetWeenie(result.ClassId); // This will add the result into the caches
                });
            }

            PopulateWeenieSpecificCaches();
        }

        public void CacheAllWeeniesNew()
        {
            using (var ctx = new WorldDbContext())
            {
                ctx.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                var weenies = ctx.Weenie.ToDictionary(i => i.ClassId, i => i);
                var animParts = ctx.WeeniePropertiesAnimPart.AsEnumerable().GroupBy(i => i.ObjectId).ToDictionary(i => i.Key, i => i.ToList());
                var attributes = ctx.WeeniePropertiesAttribute.AsEnumerable().GroupBy(i => i.ObjectId).ToDictionary(i => i.Key, i => i.ToList());
                var vitals = ctx.WeeniePropertiesAttribute2nd.AsEnumerable().GroupBy(i => i.ObjectId).ToDictionary(i => i.Key, i => i.ToList());
                var bodyParts = ctx.WeeniePropertiesBodyPart.AsEnumerable().GroupBy(i => i.ObjectId).ToDictionary(i => i.Key, i => i.ToList());
                var books = ctx.WeeniePropertiesBook.ToDictionary(i => i.ObjectId, i => i);
                var pages = ctx.WeeniePropertiesBookPageData.AsEnumerable().GroupBy(i => i.ObjectId).ToDictionary(i => i.Key, i => i.ToList());
                var bools = ctx.WeeniePropertiesBool.AsEnumerable().GroupBy(i => i.ObjectId).ToDictionary(i => i.Key, i => i.ToList());
                var createLists = ctx.WeeniePropertiesCreateList.AsEnumerable().GroupBy(i => i.ObjectId).ToDictionary(i => i.Key, i => i.ToList());
                var dids = ctx.WeeniePropertiesDID.AsEnumerable().GroupBy(i => i.ObjectId).ToDictionary(i => i.Key, i => i.ToList());
                var emotes = ctx.WeeniePropertiesEmote.AsEnumerable().GroupBy(i => i.ObjectId).ToDictionary(i => i.Key, i => i.ToList());
                var emoteActions = ctx.WeeniePropertiesEmoteAction.AsEnumerable().GroupBy(i => i.EmoteId).ToDictionary(i => i.Key, i => i.ToList());
                var filters = ctx.WeeniePropertiesEventFilter.AsEnumerable().GroupBy(i => i.ObjectId).ToDictionary(i => i.Key, i => i.ToList());
                var floats = ctx.WeeniePropertiesFloat.AsEnumerable().GroupBy(i => i.ObjectId).ToDictionary(i => i.Key, i => i.ToList());
                var generators = ctx.WeeniePropertiesGenerator.AsEnumerable().GroupBy(i => i.ObjectId).ToDictionary(i => i.Key, i => i.ToList());
                var iids = ctx.WeeniePropertiesIID.AsEnumerable().GroupBy(i => i.ObjectId).ToDictionary(i => i.Key, i => i.ToList());
                var ints = ctx.WeeniePropertiesInt.AsEnumerable().GroupBy(i => i.ObjectId).ToDictionary(i => i.Key, i => i.ToList());
                var int64s = ctx.WeeniePropertiesInt64.AsEnumerable().GroupBy(i => i.ObjectId).ToDictionary(i => i.Key, i => i.ToList());
                var palettes = ctx.WeeniePropertiesPalette.AsEnumerable().GroupBy(i => i.ObjectId).ToDictionary(i => i.Key, i => i.ToList());
                var positions = ctx.WeeniePropertiesPosition.AsEnumerable().GroupBy(i => i.ObjectId).ToDictionary(i => i.Key, i => i.ToList());
                var skills = ctx.WeeniePropertiesSkill.AsEnumerable().GroupBy(i => i.ObjectId).ToDictionary(i => i.Key, i => i.ToList());
                var spellbooks = ctx.WeeniePropertiesSpellBook.AsEnumerable().GroupBy(i => i.ObjectId).ToDictionary(i => i.Key, i => i.ToList());
                var strings = ctx.WeeniePropertiesString.AsEnumerable().GroupBy(i => i.ObjectId).ToDictionary(i => i.Key, i => i.ToList());
                var textureMaps = ctx.WeeniePropertiesTextureMap.AsEnumerable().GroupBy(i => i.ObjectId).ToDictionary(i => i.Key, i => i.ToList());

                foreach (var kvp in weenies)
                {
                    var wcid = kvp.Key;
                    var weenie = kvp.Value;

                    if (animParts.TryGetValue(wcid, out var animPart))
                        weenie.WeeniePropertiesAnimPart = animPart;

                    if (attributes.TryGetValue(wcid, out var attribute))
                        weenie.WeeniePropertiesAttribute = attribute;

                    if (vitals.TryGetValue(wcid, out var vital))
                        weenie.WeeniePropertiesAttribute2nd = vital;

                    if (bodyParts.TryGetValue(wcid, out var bodyPart))
                        weenie.WeeniePropertiesBodyPart = bodyPart;

                    if (books.TryGetValue(wcid, out var book))
                        weenie.WeeniePropertiesBook = book;

                    if (pages.TryGetValue(wcid, out var page))
                        weenie.WeeniePropertiesBookPageData = page;

                    if (bools.TryGetValue(wcid, out var _bool))
                        weenie.WeeniePropertiesBool = _bool;

                    if (createLists.TryGetValue(wcid, out var createList))
                        weenie.WeeniePropertiesCreateList = createList;

                    if (dids.TryGetValue(wcid, out var did))
                        weenie.WeeniePropertiesDID = did;

                    if (emotes.TryGetValue(wcid, out var emote))
                    {
                        foreach (var e in emote)
                        {
                            if (emoteActions.TryGetValue(e.Id, out var action))
                                e.WeeniePropertiesEmoteAction = action;
                        }
                        weenie.WeeniePropertiesEmote = emote;
                    }

                    if (filters.TryGetValue(wcid, out var filter))
                        weenie.WeeniePropertiesEventFilter = filter;

                    if (floats.TryGetValue(wcid, out var _float))
                        weenie.WeeniePropertiesFloat = _float;

                    if (generators.TryGetValue(wcid, out var gen))
                        weenie.WeeniePropertiesGenerator = gen;

                    if (iids.TryGetValue(wcid, out var iid))
                        weenie.WeeniePropertiesIID = iid;

                    if (ints.TryGetValue(wcid, out var _int))
                        weenie.WeeniePropertiesInt = _int;

                    if (int64s.TryGetValue(wcid, out var _int64))
                        weenie.WeeniePropertiesInt64 = _int64;

                    if (palettes.TryGetValue(wcid, out var palette))
                        weenie.WeeniePropertiesPalette = palette;

                    if (positions.TryGetValue(wcid, out var pos))
                        weenie.WeeniePropertiesPosition = pos;

                    if (skills.TryGetValue(wcid, out var skill))
                        weenie.WeeniePropertiesSkill = skill;

                    if (spellbooks.TryGetValue(wcid, out var spellbook))
                        weenie.WeeniePropertiesSpellBook = spellbook;

                    if (strings.TryGetValue(wcid, out var str))
                        weenie.WeeniePropertiesString = str;

                    if (textureMaps.TryGetValue(wcid, out var textureMap))
                        weenie.WeeniePropertiesTextureMap = textureMap;

                    weenieCache[wcid] = WeenieConverter.ConvertToEntityWeenie(weenie);

                    weenieClassNameToClassIdCache[weenie.ClassName.ToLower()] = weenie.ClassId;
                }

                PopulateWeenieSpecificCaches();
            }
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

                Parallel.ForEach(results, ConfigManager.Config.Server.Threading.DatabaseParallelOptions, result =>
                {
                    GetCookbook(result.SourceWCID, result.TargetWCID);  // This will add the result into the cache
                });
            }
        }

        public void CacheAllCookbooksNew()
        {
            using (var ctx = new WorldDbContext())
            {
                ctx.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                // 19402 -> 19515 has multiple recipes?
                // original handles with FirstOrDefault()

                // this can be used if db is normalized
                //var cookbooks = ctx.CookBook.AsEnumerable().GroupBy(i => i.SourceWCID).ToDictionary(i => i.Key, i => i.ToDictionary(j => j.TargetWCID, j => j));

                var cookbooks = ctx.CookBook.AsEnumerable().GroupBy(i => i.SourceWCID).ToDictionary(i => i.Key, i => i.GroupBy(j => j.TargetWCID).Select(j => j.FirstOrDefault()).ToDictionary(j => j.TargetWCID, j => j));

                var recipes = ctx.Recipe.AsEnumerable().ToDictionary(i => i.Id, i => i);

                var recipeMods = ctx.RecipeMod.AsEnumerable().GroupBy(i => i.RecipeId).ToDictionary(i => i.Key, i => i.ToList());
                var recipeModBools = ctx.RecipeModsBool.AsEnumerable().GroupBy(i => i.RecipeModId).ToDictionary(i => i.Key, i => i.ToList());
                var recipeModDIDs = ctx.RecipeModsDID.AsEnumerable().GroupBy(i => i.RecipeModId).ToDictionary(i => i.Key, i => i.ToList());
                var recipeModFloats = ctx.RecipeModsFloat.AsEnumerable().GroupBy(i => i.RecipeModId).ToDictionary(i => i.Key, i => i.ToList());
                var recipeModInts = ctx.RecipeModsInt.AsEnumerable().GroupBy(i => i.RecipeModId).ToDictionary(i => i.Key, i => i.ToList());
                var recipeModIIDs = ctx.RecipeModsIID.AsEnumerable().GroupBy(i => i.RecipeModId).ToDictionary(i => i.Key, i => i.ToList());
                var recipeModStrings = ctx.RecipeModsString.AsEnumerable().GroupBy(i => i.RecipeModId).ToDictionary(i => i.Key, i => i.ToList());

                var recipeReqBools = ctx.RecipeRequirementsBool.AsEnumerable().GroupBy(i => i.RecipeId).ToDictionary(i => i.Key, i => i.ToList());
                var recipeReqDIDs = ctx.RecipeRequirementsDID.AsEnumerable().GroupBy(i => i.RecipeId).ToDictionary(i => i.Key, i => i.ToList());
                var recipeReqFloats = ctx.RecipeRequirementsFloat.AsEnumerable().GroupBy(i => i.RecipeId).ToDictionary(i => i.Key, i => i.ToList());
                var recipeReqInts = ctx.RecipeRequirementsInt.AsEnumerable().GroupBy(i => i.RecipeId).ToDictionary(i => i.Key, i => i.ToList());
                var recipeReqIIDs = ctx.RecipeRequirementsIID.AsEnumerable().GroupBy(i => i.RecipeId).ToDictionary(i => i.Key, i => i.ToList());
                var recipeReqStrings = ctx.RecipeRequirementsString.AsEnumerable().GroupBy(i => i.RecipeId).ToDictionary(i => i.Key, i => i.ToList());

                foreach (var kvp in cookbooks)
                {
                    var sourceWcid = kvp.Key;
                    var targetCookbook = kvp.Value;

                    foreach (var kvp2 in targetCookbook)
                    {
                        var targetWcid = kvp2.Key;
                        var cookbook = kvp2.Value;

                        if (!recipes.TryGetValue(cookbook.RecipeId, out var recipe))
                            continue;

                        cookbook.Recipe = recipe;

                        if (recipeReqBools.TryGetValue(cookbook.RecipeId, out var reqBool))
                            recipe.RecipeRequirementsBool = reqBool;

                        if (recipeReqDIDs.TryGetValue(cookbook.RecipeId, out var reqDID))
                            recipe.RecipeRequirementsDID = reqDID;

                        if (recipeReqFloats.TryGetValue(cookbook.RecipeId, out var reqFloat))
                            recipe.RecipeRequirementsFloat = reqFloat;

                        if (recipeReqInts.TryGetValue(cookbook.RecipeId, out var reqInt))
                            recipe.RecipeRequirementsInt = reqInt;

                        if (recipeReqIIDs.TryGetValue(cookbook.RecipeId, out var reqIID))
                            recipe.RecipeRequirementsIID = reqIID;

                        if (recipeReqStrings.TryGetValue(cookbook.RecipeId, out var reqString))
                            recipe.RecipeRequirementsString = reqString;

                        if (recipeMods.TryGetValue(cookbook.RecipeId, out var _recipeMod))
                        {
                            recipe.RecipeMod = _recipeMod;

                            foreach (var recipeMod in _recipeMod)
                            {
                                if (recipeModBools.TryGetValue(recipeMod.Id, out var modBool))
                                    recipeMod.RecipeModsBool = modBool;

                                if (recipeModDIDs.TryGetValue(recipeMod.Id, out var modDID))
                                    recipeMod.RecipeModsDID = modDID;

                                if (recipeModFloats.TryGetValue(recipeMod.Id, out var modFloat))
                                    recipeMod.RecipeModsFloat = modFloat;

                                if (recipeModInts.TryGetValue(recipeMod.Id, out var modInt))
                                    recipeMod.RecipeModsInt = modInt;

                                if (recipeModIIDs.TryGetValue(recipeMod.Id, out var modIID))
                                    recipeMod.RecipeModsIID = modIID;

                                if (recipeModStrings.TryGetValue(recipeMod.Id, out var modString))
                                    recipeMod.RecipeModsString = modString;
                            }
                        }

                        if (!cookbookCache.TryGetValue(sourceWcid, out var targetCookbooks))
                        {
                            targetCookbooks = new Dictionary<uint, CookBook>();
                            cookbookCache.Add(sourceWcid, targetCookbooks);
                        }
                        targetCookbooks[targetWcid] = cookbook;
                    }
                }
            }
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
