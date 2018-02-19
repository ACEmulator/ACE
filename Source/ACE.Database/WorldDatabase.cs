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
                var results = context.LandblockInstances
                    .AsNoTracking()
                    .Where(r => r.Guid >= min && r.Guid <= max);

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


        /// <summary>
        /// This will populate all sub collections except the followign: LandblockInstances, PointsOfInterest, WeeniePropertiesEmoteAction
        /// </summary>
        public Weenie GetWeenie(uint weenieClassId)
        {
            using (var context = new WorldDbContext())
            {
                var result = context.Weenie.AsNoTracking()
                    .Include(r => r.WeeniePropertiesBook)
                    //.Include(r => r.LandblockInstances)   / When we grab a weenie, we don't need to also know everywhere it exists in the world
                    //.Include(r => r.PointsOfInterest)     // I think these are just foreign keys for the POI table
                    .Include(r => r.WeeniePropertiesAnimPart)
                    .Include(r => r.WeeniePropertiesAttribute)
                    .Include(r => r.WeeniePropertiesAttribute2nd)
                    .Include(r => r.WeeniePropertiesBodyPart)
                    .Include(r => r.WeeniePropertiesBookPageData)
                    .Include(r => r.WeeniePropertiesBool)
                    .Include(r => r.WeeniePropertiesCreateList)
                    .Include(r => r.WeeniePropertiesDID)
                    .Include(r => r.WeeniePropertiesEmote).ThenInclude(emote => emote.WeeniePropertiesEmoteAction)
                    .Include(r => r.WeeniePropertiesEmoteAction)
                    .Include(r => r.WeeniePropertiesEventFilter)
                    .Include(r => r.WeeniePropertiesFloat)
                    .Include(r => r.WeeniePropertiesGenerator)
                    .Include(r => r.WeeniePropertiesIID)
                    .Include(r => r.WeeniePropertiesInt)
                    .Include(r => r.WeeniePropertiesInt64)
                    .Include(r => r.WeeniePropertiesPalette)
                    .Include(r => r.WeeniePropertiesPosition)
                    .Include(r => r.WeeniePropertiesSkill)
                    .Include(r => r.WeeniePropertiesSpellBook)
                    .Include(r => r.WeeniePropertiesString)
                    .Include(r => r.WeeniePropertiesTextureMap)
                    .FirstOrDefault(r => r.ClassId == weenieClassId);

                weenieCache.TryAdd(weenieClassId, result);

                return result;
            }
        }

        public uint GetWeenieClassId(string weenieClassName)
        {
            using (var context = new WorldDbContext())
            {
                var result = context.Weenie.AsNoTracking()
                    .FirstOrDefault(r => r.ClassName == weenieClassName);

                if (result != null)
                    return result.ClassId;

                return 0;
            }
        }

        public Weenie GetWeenie(string weenieClassName)
        {
            return GetWeenie(GetWeenieClassId(weenieClassName));
        }

        private readonly ConcurrentDictionary<uint, Weenie> weenieCache = new ConcurrentDictionary<uint, Weenie>();

        /// <summary>
        /// Weenies will have all their collections populated except the followign: LandblockInstances, PointsOfInterest, WeeniePropertiesEmoteAction
        /// </summary>
        public Weenie GetCachedWeenie(uint weenieClassId)
        {
            if (weenieCache.TryGetValue(weenieClassId, out var value))
                return value;

            var result = GetWeenie(weenieClassId);

            weenieCache.TryAdd(weenieClassId, result);

            return result;
        }

        /// <summary>
        /// Weenies will have all their collections populated except the followign: LandblockInstances, PointsOfInterest, WeeniePropertiesEmoteAction
        /// </summary>
        public Dictionary<Weenie, List<LandblockInstances>> GetCachedWeenieInstancesByLandblock(ushort landblock)
        {
            var builder = new Dictionary<uint, List<LandblockInstances>>();

            using (var context = new WorldDbContext())
            {
                var results = context.LandblockInstances
                    .AsNoTracking()
                    .Where(r => r.Landblock == landblock);

                foreach (var result in results)
                {
                    if (builder.TryGetValue(result.WeenieClassId, out var value))
                        value.Add(result);
                    else
                        builder[result.WeenieClassId] = new List<LandblockInstances>() { result };
                }
            }

            var ret = new Dictionary<Weenie, List<LandblockInstances>>();

            foreach (var kvp in builder)
                ret[GetCachedWeenie(kvp.Key)] = kvp.Value;

            return ret;
        }


        private readonly ConcurrentDictionary<string, PointsOfInterest> cachedAcePositions = new ConcurrentDictionary<string, PointsOfInterest>();

        public PointsOfInterest GetCachedPointOfInterest(string name)
        {
            if (cachedAcePositions.TryGetValue(name.ToLower(), out var value))
                return value;

            using (var context = new WorldDbContext())
            {
                var result = context.PointsOfInterest
                    .AsNoTracking()
                    .FirstOrDefault(r => r.Name.ToLower() == name.ToLower());

                if (result != null)
                {
                    cachedAcePositions[name.ToLower()] = result;
                    return result;
                }
            }

            return null;
        }


        private readonly Dictionary<uint, Dictionary<uint, AceRecipe>> recipeCache = new Dictionary<uint, Dictionary<uint, AceRecipe>>();

        public AceRecipe GetCachedRecipe(uint sourceWeenieClassid, uint targetWeenieClassId)
        {
            lock (recipeCache)
            {
                if (recipeCache.TryGetValue(sourceWeenieClassid, out var recipiesForSource))
                {
                    if (recipiesForSource.TryGetValue(targetWeenieClassId, out var value))
                        return value;
                }
            }

            using (var context = new WorldDbContext())
            {
                var result = context.AceRecipe
                    .AsNoTracking()
                    .FirstOrDefault(r => r.SourceWcid == sourceWeenieClassid && r.TargetWcid == targetWeenieClassId);

                lock (recipeCache)
                {
                    // We double check before commiting the recipe.
                    // We could be in this lock, and queued up behind us is an attempt to add a result for the same source:target pair.
                    if (recipeCache.TryGetValue(sourceWeenieClassid, out var sourceRecipies))
                    {
                        if (!sourceRecipies.ContainsKey(targetWeenieClassId))
                            sourceRecipies.Add(targetWeenieClassId, result);
                    }
                    else
                        recipeCache.Add(sourceWeenieClassid, new Dictionary<uint, AceRecipe>() { { targetWeenieClassId, result } });
                }

                return result;
            }
        }
    }
}
