using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

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
                        log.Debug($"[DATABASE] Successfully connected to {config.Database} database on {config.Host}:{config.Port}.");
                        return true;
                    }
                }

                log.Error($"[DATABASE] Attempting to reconnect to {config.Database} database on {config.Host}:{config.Port} in 5 seconds...");

                if (retryUntilFound)
                    Thread.Sleep(5000);
                else
                    return false;
            }
        }


        // =====================================
        // Weenie
        // =====================================

        /// <summary>
        /// This will populate all sub collections except the following: LandblockInstances, PointsOfInterest
        /// </summary>
        public virtual Weenie GetWeenie(WorldDbContext context, uint weenieClassId)
        {
            var weenie = context.Weenie.FirstOrDefault(r => r.ClassId == weenieClassId);

            if (weenie == null)
                return null;

            // Base properties for every weenie (ACBaseQualities)
            weenie.WeeniePropertiesBool = context.WeeniePropertiesBool.Where(r => r.ObjectId == weenie.ClassId).ToList();
            weenie.WeeniePropertiesDID = context.WeeniePropertiesDID.Where(r => r.ObjectId == weenie.ClassId).ToList();
            weenie.WeeniePropertiesFloat = context.WeeniePropertiesFloat.Where(r => r.ObjectId == weenie.ClassId).ToList();
            weenie.WeeniePropertiesIID = context.WeeniePropertiesIID.Where(r => r.ObjectId == weenie.ClassId).ToList();
            weenie.WeeniePropertiesInt = context.WeeniePropertiesInt.Where(r => r.ObjectId == weenie.ClassId).ToList();
            weenie.WeeniePropertiesInt64 = context.WeeniePropertiesInt64.Where(r => r.ObjectId == weenie.ClassId).ToList();
            weenie.WeeniePropertiesPosition = context.WeeniePropertiesPosition.Where(r => r.ObjectId == weenie.ClassId).ToList();
            weenie.WeeniePropertiesString = context.WeeniePropertiesString.Where(r => r.ObjectId == weenie.ClassId).ToList();

            var weenieType = (WeenieType)weenie.Type;

            bool isCreature = weenieType == WeenieType.Creature || weenieType == WeenieType.Cow ||
                              weenieType == WeenieType.Sentinel || weenieType == WeenieType.Admin ||
                              weenieType == WeenieType.Vendor ||
                              weenieType == WeenieType.CombatPet || weenieType == WeenieType.Pet;

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

            weenie.WeeniePropertiesSpellBook = context.WeeniePropertiesSpellBook.Where(r => r.ObjectId == weenie.ClassId).OrderBy(i => i.Id).ToList();

            weenie.WeeniePropertiesTextureMap = context.WeeniePropertiesTextureMap.Where(r => r.ObjectId == weenie.ClassId).ToList();

            return weenie;
        }

        /// <summary>
        /// This will populate all sub collections except the following: LandblockInstances, PointsOfInterest
        /// </summary>
        public virtual List<Weenie> GetAllWeenies()
        {
            using (var context = new WorldDbContext())
            {
                context.Weenie.Load();

                // Base properties for every weenie (ACBaseQualities)
                context.WeeniePropertiesBool.Load();
                context.WeeniePropertiesDID.Load();
                context.WeeniePropertiesFloat.Load();
                context.WeeniePropertiesIID.Load();
                context.WeeniePropertiesInt.Load();
                context.WeeniePropertiesInt64.Load();
                context.WeeniePropertiesPosition.Load();
                context.WeeniePropertiesString.Load();

                context.WeeniePropertiesAnimPart.Load();

                //if (isCreature)
                {
                    context.WeeniePropertiesAttribute.Load();
                    context.WeeniePropertiesAttribute2nd.Load();

                    context.WeeniePropertiesBodyPart.Load();
                }

                //if (weenieType == WeenieType.Book)
                {
                    context.WeeniePropertiesBook.Load();
                    context.WeeniePropertiesBookPageData.Load();
                }

                context.WeeniePropertiesCreateList.Load();
                context.WeeniePropertiesEmote.Load();
                context.WeeniePropertiesEmoteAction.Load();
                context.WeeniePropertiesEventFilter.Load();

                context.WeeniePropertiesGenerator.Load();
                context.WeeniePropertiesPalette.Load();

                //if (isCreature)
                {
                    context.WeeniePropertiesSkill.Load();
                }

                context.WeeniePropertiesSpellBook.Load();

                context.WeeniePropertiesTextureMap.Load();

                return context.Weenie.ToList();
            }
        }

        /// <summary>
        /// This will populate all sub collections except the following: LandblockInstances, PointsOfInterest
        /// </summary>
        public Weenie GetWeenie(uint weenieClassId)
        {
            using (var context = new WorldDbContext())
            {
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                return GetWeenie(context, weenieClassId);
            }
        }

        /// <summary>
        /// This will populate all sub collections except the following: LandblockInstances, PointsOfInterest
        /// </summary>
        public Weenie GetWeenie(WorldDbContext context, string weenieClassName)
        {
            var result = context.Weenie
                .FirstOrDefault(r => r.ClassName == weenieClassName);

            if (result != null)
                return GetWeenie(context, result.ClassId);

            return null;
        }

        /// <summary>
        /// This will populate all sub collections except the following: LandblockInstances, PointsOfInterest
        /// </summary>
        public Weenie GetWeenie(string weenieClassName)
        {
            using (var context = new WorldDbContext())
            {
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                return GetWeenie(context, weenieClassName);
            }
        }


        public Dictionary<uint, string> GetAllWeenieNames(WorldDbContext context)
        {
            return context.Weenie
                .Include(r => r.WeeniePropertiesString)
                .ToDictionary(r => r.ClassId, r => r.WeeniePropertiesString.FirstOrDefault(p => p.Type == (int)PropertyString.Name)?.Value ?? "");
        }

        public Dictionary<uint, string> GetAllWeenieNames()
        {
            using (var context = new WorldDbContext())
            {
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                return GetAllWeenieNames(context);
            }
        }

        public Dictionary<uint, string> GetAllWeenieClassNames(WorldDbContext context)
        {
            return context.Weenie
                .ToDictionary(r => r.ClassId, r => r.ClassName);
        }

        public Dictionary<uint, string> GetAllWeenieClassNames()
        {
            using (var context = new WorldDbContext())
            {
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                return GetAllWeenieClassNames(context);
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



        // =====================================
        // CookBook
        // =====================================

        public virtual CookBook GetCookbook(WorldDbContext context, uint sourceWeenieClassId, uint targetWeenieClassId)
        {
            var result = context.CookBook
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
                .FirstOrDefault(r => r.SourceWCID == sourceWeenieClassId && r.TargetWCID == targetWeenieClassId);

            return result;
        }

        public virtual List<CookBook> GetAllCookbooks()
        {
            using (var context = new WorldDbContext())
            {
                context.CookBook.Load();

                context.Recipe.Load();

                context.RecipeMod.Load();
                context.RecipeModsBool.Load();
                context.RecipeModsDID.Load();
                context.RecipeModsFloat.Load();
                context.RecipeModsIID.Load();
                context.RecipeModsInt.Load();
                context.RecipeModsString.Load();

                context.RecipeRequirementsBool.Load();
                context.RecipeRequirementsDID.Load();
                context.RecipeRequirementsFloat.Load();
                context.RecipeRequirementsIID.Load();
                context.RecipeRequirementsInt.Load();
                context.RecipeRequirementsString.Load();

                return context.CookBook.ToList();
            }
        }

        public CookBook GetCookbook(uint sourceWeenieClassId, uint targetWeenieClassId)
        {
            using (var context = new WorldDbContext())
            {
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                return GetCookbook(context, sourceWeenieClassId, targetWeenieClassId);
            }
        }

        public List<CookBook> GetCookbooksByRecipeId(uint recipeId)
        {
            var results = new List<CookBook>();

            using (var context = new WorldDbContext())
            {
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                var baseRecords = context.CookBook.Where(i => i.RecipeId == recipeId).ToList();

                foreach (var baseRecord in baseRecords)
                {
                    var cookbook = GetCookbook(context, baseRecord.SourceWCID, baseRecord.TargetWCID);

                    results.Add(cookbook);
                }
            }

            return results;
        }


        // =====================================
        // Encounter
        // =====================================


        // =====================================
        // Event
        // =====================================

        /// <summary>
        /// This takes under 1 second to complete.
        /// </summary>
        public virtual List<Event> GetAllEvents(WorldDbContext context)
        {
            return context.Event
                .ToList();
        }

        /// <summary>
        /// This takes under 1 second to complete.
        /// </summary>
        public List<Event> GetAllEvents()
        {
            using (var context = new WorldDbContext())
            {
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                return GetAllEvents(context);
            }
        }


        // =====================================
        // HousePortal
        // =====================================


        // =====================================
        // LandblockInstance
        // =====================================

        public LandblockInstance GetLandblockInstanceByGuid(WorldDbContext context, uint guid)
        {
            return context.LandblockInstance
                .Include(r => r.LandblockInstanceLink)
                .FirstOrDefault(r => r.Guid == guid);
        }

        public LandblockInstance GetLandblockInstanceByGuid(uint guid)
        {
            using (var context = new WorldDbContext())
            {
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                return GetLandblockInstanceByGuid(context, guid);
            }
        }

        public List<LandblockInstance> GetLandblockInstancesByWcid(WorldDbContext context, uint wcid)
        {
            return context.LandblockInstance
                .Include(r => r.LandblockInstanceLink)
                .Where(i => i.WeenieClassId == wcid)
                .ToList();
        }

        public List<LandblockInstance> GetLandblockInstancesByWcid(uint wcid)
        {
            using (var context = new WorldDbContext())
            {
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                return GetLandblockInstancesByWcid(context, wcid);
            }
        }


        // =====================================
        // PointsOfInterest
        // =====================================


        // =====================================
        // Quest
        // =====================================


        // =====================================
        // Recipe
        // =====================================

        public Recipe GetRecipe(WorldDbContext context, uint recipeId)
        {
            var result = context.Recipe
                .Include(r => r.RecipeMod)
                    .ThenInclude(r => r.RecipeModsBool)
                .Include(r => r.RecipeMod)
                    .ThenInclude(r => r.RecipeModsDID)
                .Include(r => r.RecipeMod)
                    .ThenInclude(r => r.RecipeModsFloat)
                .Include(r => r.RecipeMod)
                    .ThenInclude(r => r.RecipeModsIID)
                .Include(r => r.RecipeMod)
                    .ThenInclude(r => r.RecipeModsInt)
                .Include(r => r.RecipeMod)
                    .ThenInclude(r => r.RecipeModsString)
                .Include(r => r.RecipeRequirementsBool)
                .Include(r => r.RecipeRequirementsDID)
                .Include(r => r.RecipeRequirementsFloat)
                .Include(r => r.RecipeRequirementsIID)
                .Include(r => r.RecipeRequirementsInt)
                .Include(r => r.RecipeRequirementsString)
                .FirstOrDefault(r => r.Id == recipeId);

            return result;
        }

        public Recipe GetRecipe(uint recipeId)
        {
            using (var context = new WorldDbContext())
            {
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                return GetRecipe(context, recipeId);
            }
        }


        // =====================================
        // Spell
        // =====================================

        public Dictionary<uint, string> GetAllSpellNames(WorldDbContext context)
        {
            return context.Spell
                .ToDictionary(r => r.Id, r => r.Name);
        }

        public Dictionary<uint, string> GetAllSpellNames()
        {
            using (var context = new WorldDbContext())
            {
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                return GetAllSpellNames(context);
            }
        }


        // =====================================
        // TreasureDeath
        // =====================================

        public Dictionary<uint, TreasureDeath> GetAllTreasureDeath(WorldDbContext context)
        {
            return context.TreasureDeath
                .ToDictionary(r => r.TreasureType, r => r);
        }

        public Dictionary<uint, TreasureDeath> GetAllTreasureDeath()
        {
            using (var context = new WorldDbContext())
            {
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                return GetAllTreasureDeath(context);
            }
        }


        // =====================================
        // TreasureMaterial
        // =====================================


        // =====================================
        // TreasureWielded
        // =====================================

        public Dictionary<uint, List<TreasureWielded>> GetAllTreasureWielded(WorldDbContext context)
        {
            var results = context.TreasureWielded;

            var treasure = new Dictionary<uint, List<TreasureWielded>>();

            foreach (var record in results)
            {
                if (!treasure.ContainsKey(record.TreasureType))
                    treasure.Add(record.TreasureType, new List<TreasureWielded>());

                treasure[record.TreasureType].Add(record);
            }

            return treasure;

        }

        public Dictionary<uint, List<TreasureWielded>> GetAllTreasureWielded()
        {
            using (var context = new WorldDbContext())
            {
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                return GetAllTreasureWielded(context);
            }
        }


        // =====================================
        // Version
        // =====================================

        /// <summary>
        /// Get the version information stored in database
        /// </summary>
        public ACE.Database.Models.World.Version GetVersion(WorldDbContext context)
        {
            var version = context.Version
                .FirstOrDefault(r => r.Id == 1);

            return version;
        }

        /// <summary>
        /// Get the version information stored in database
        /// </summary>
        public ACE.Database.Models.World.Version GetVersion()
        {
            using (var context = new WorldDbContext())
            {
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                return GetVersion(context);
            }
        }

        // =====================================
        // IsWorldDatabaseGuidRangeValid
        // =====================================

        public bool IsWorldDatabaseGuidRangeValid(WorldDbContext context)
        {
            return context.LandblockInstance.FirstOrDefault(i => i.Guid >= 0x80000000) == null;
        }

        public bool IsWorldDatabaseGuidRangeValid()
        {
            using (var context = new WorldDbContext())
            {
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                return IsWorldDatabaseGuidRangeValid(context);
            }
        }
    }
}
