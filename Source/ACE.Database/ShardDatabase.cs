using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

using log4net;

using ACE.Common.Extensions;
using ACE.Database.Entity;
using ACE.Database.Models.Shard;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

namespace ACE.Database
{
    public class ShardDatabase
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public bool Exists(bool retryUntilFound)
        {
            var config = Common.ConfigManager.Config.MySql.Shard;

            for (; ; )
            {
                using (var context = new ShardDbContext())
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
            using (var context = new ShardDbContext())
            {
                var results = context.Biota
                    .AsNoTracking()
                    .Where(r => r.Id >= min && r.Id <= max)
                    .ToList();

                if (!results.Any())
                    return uint.MaxValue;

                var maxId = min;

                foreach (var result in results)
                {
                    if (result.Id > maxId)
                        maxId = result.Id;
                }

                return maxId;
            }
        }

        /// <summary>
        /// This will return available id's, in the form of sequence gaps starting from min.<para />
        /// If a gap is just 1 value wide, then both start and end will be the same number.
        /// </summary>
        public List<(uint start, uint end)> GetSequenceGaps(uint min, uint limitAvailableIDsReturned)
        {
            // References:
            // https://stackoverflow.com/questions/4340793/how-to-find-gaps-in-sequential-numbering-in-mysql/29736658#29736658
            // https://stackoverflow.com/questions/50402015/how-to-execute-sqlquery-with-entity-framework-core-2-1

            // This query is ugly, but very fast.
            var sql = "SELECT"                                                                          + Environment.NewLine +
                      " z.gap_starts_at, z.gap_ends_at_not_inclusive, @available_ids:=@available_ids+(z.gap_ends_at_not_inclusive - z.gap_starts_at) as running_total_available_ids" + Environment.NewLine +
                      "FROM ("                                                                          + Environment.NewLine +
                      " SELECT"                                                                         + Environment.NewLine +
                      "  @rownum:=@rownum+1 AS gap_starts_at,"                                          + Environment.NewLine +
                      "  @available_ids:=0,"                                                            + Environment.NewLine +
                      "  IF(@rownum=id, 0, @rownum:=id) AS gap_ends_at_not_inclusive"                   + Environment.NewLine +
                      " FROM"                                                                           + Environment.NewLine +
                      "  (SELECT @rownum:=(SELECT MIN(id)-1 FROM biota WHERE id > " + min + ")) AS a"   + Environment.NewLine +
                      "  JOIN biota"                                                                    + Environment.NewLine +
                      "  WHERE id > " + min                                                             + Environment.NewLine +
                      "  ORDER BY id"                                                                   + Environment.NewLine +
                      " ) AS z"                                                                         + Environment.NewLine +
                      "WHERE z.gap_ends_at_not_inclusive!=0 AND @available_ids<" + limitAvailableIDsReturned + "; ";

            using (var context = new ShardDbContext())
            {
                var connection = context.Database.GetDbConnection();
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = sql;
                var reader = command.ExecuteReader();

                var gaps = new List<(uint start, uint end)>();

                while (reader.Read())
                {
                    var gap_starts_at               = reader.GetFieldValue<double>(0);
                    var gap_ends_at_not_inclusive   = reader.GetFieldValue<decimal>(1);
                    //var running_total_available_ids = reader.GetFieldValue<double>(2);

                    gaps.Add(((uint)gap_starts_at, (uint)gap_ends_at_not_inclusive - 1));
                }

                return gaps;
            }
        }


        public int GetBiotaCount()
        {
            using (var context = new ShardDbContext())
                return context.Biota.Count();
        }

        [Flags]
        enum PopulatedCollectionFlags
        {
            BiotaPropertiesAnimPart             = 0x1,
            BiotaPropertiesAttribute            = 0x2,
            BiotaPropertiesAttribute2nd         = 0x4,
            BiotaPropertiesBodyPart             = 0x8,
            BiotaPropertiesBook                 = 0x10,
            BiotaPropertiesBookPageData         = 0x20,
            BiotaPropertiesBool                 = 0x40,
            BiotaPropertiesCreateList           = 0x80,
            BiotaPropertiesDID                  = 0x100,
            BiotaPropertiesEmote                = 0x200,
            BiotaPropertiesEnchantmentRegistry  = 0x400,
            BiotaPropertiesEventFilter          = 0x800,
            BiotaPropertiesFloat                = 0x1000,
            BiotaPropertiesGenerator            = 0x2000,
            BiotaPropertiesIID                  = 0x4000,
            BiotaPropertiesInt                  = 0x8000,
            BiotaPropertiesInt64                = 0x10000,
            BiotaPropertiesPalette              = 0x20000,
            BiotaPropertiesPosition             = 0x40000,
            BiotaPropertiesSkill                = 0x80000,
            BiotaPropertiesSpellBook            = 0x100000,
            BiotaPropertiesString               = 0x200000,
            BiotaPropertiesTextureMap           = 0x400000,
            HousePermission                     = 0x800000,
        }

        private static void SetBiotaPopulatedCollections(Biota biota)
        {
            PopulatedCollectionFlags populatedCollectionFlags = 0;

            if (biota.BiotaPropertiesAnimPart != null && biota.BiotaPropertiesAnimPart.Count > 0) populatedCollectionFlags |= PopulatedCollectionFlags.BiotaPropertiesAnimPart;
            if (biota.BiotaPropertiesAttribute != null && biota.BiotaPropertiesAttribute.Count > 0) populatedCollectionFlags |= PopulatedCollectionFlags.BiotaPropertiesAttribute;
            if (biota.BiotaPropertiesAttribute2nd != null && biota.BiotaPropertiesAttribute2nd.Count > 0) populatedCollectionFlags |= PopulatedCollectionFlags.BiotaPropertiesAttribute2nd;
            if (biota.BiotaPropertiesBodyPart != null && biota.BiotaPropertiesBodyPart.Count > 0) populatedCollectionFlags |= PopulatedCollectionFlags.BiotaPropertiesBodyPart;
            if (biota.BiotaPropertiesBook != null) populatedCollectionFlags |= PopulatedCollectionFlags.BiotaPropertiesBook;
            if (biota.BiotaPropertiesBookPageData != null && biota.BiotaPropertiesBookPageData.Count > 0) populatedCollectionFlags |= PopulatedCollectionFlags.BiotaPropertiesBookPageData;
            if (biota.BiotaPropertiesBool != null && biota.BiotaPropertiesBool.Count > 0) populatedCollectionFlags |= PopulatedCollectionFlags.BiotaPropertiesBool;
            if (biota.BiotaPropertiesCreateList != null && biota.BiotaPropertiesCreateList.Count > 0) populatedCollectionFlags |= PopulatedCollectionFlags.BiotaPropertiesCreateList;
            if (biota.BiotaPropertiesDID != null && biota.BiotaPropertiesDID.Count > 0) populatedCollectionFlags |= PopulatedCollectionFlags.BiotaPropertiesDID;
            if (biota.BiotaPropertiesEmote != null && biota.BiotaPropertiesEmote.Count > 0) populatedCollectionFlags |= PopulatedCollectionFlags.BiotaPropertiesEmote;
            if (biota.BiotaPropertiesEnchantmentRegistry != null && biota.BiotaPropertiesEnchantmentRegistry.Count > 0) populatedCollectionFlags |= PopulatedCollectionFlags.BiotaPropertiesEnchantmentRegistry;
            if (biota.BiotaPropertiesEventFilter != null && biota.BiotaPropertiesEventFilter.Count > 0) populatedCollectionFlags |= PopulatedCollectionFlags.BiotaPropertiesEventFilter;
            if (biota.BiotaPropertiesFloat != null && biota.BiotaPropertiesFloat.Count > 0) populatedCollectionFlags |= PopulatedCollectionFlags.BiotaPropertiesFloat;
            if (biota.BiotaPropertiesGenerator != null && biota.BiotaPropertiesGenerator.Count > 0) populatedCollectionFlags |= PopulatedCollectionFlags.BiotaPropertiesGenerator;
            if (biota.BiotaPropertiesIID != null && biota.BiotaPropertiesIID.Count > 0) populatedCollectionFlags |= PopulatedCollectionFlags.BiotaPropertiesIID;
            if (biota.BiotaPropertiesInt != null && biota.BiotaPropertiesInt.Count > 0) populatedCollectionFlags |= PopulatedCollectionFlags.BiotaPropertiesInt;
            if (biota.BiotaPropertiesInt64 != null && biota.BiotaPropertiesInt64.Count > 0) populatedCollectionFlags |= PopulatedCollectionFlags.BiotaPropertiesInt64;
            if (biota.BiotaPropertiesPalette != null && biota.BiotaPropertiesPalette.Count > 0) populatedCollectionFlags |= PopulatedCollectionFlags.BiotaPropertiesPalette;
            if (biota.BiotaPropertiesPosition != null && biota.BiotaPropertiesPosition.Count > 0) populatedCollectionFlags |= PopulatedCollectionFlags.BiotaPropertiesPosition;
            if (biota.BiotaPropertiesSkill != null && biota.BiotaPropertiesSkill.Count > 0) populatedCollectionFlags |= PopulatedCollectionFlags.BiotaPropertiesSkill;
            if (biota.BiotaPropertiesSpellBook != null && biota.BiotaPropertiesSpellBook.Count > 0) populatedCollectionFlags |= PopulatedCollectionFlags.BiotaPropertiesSpellBook;
            if (biota.BiotaPropertiesString != null && biota.BiotaPropertiesString.Count > 0) populatedCollectionFlags |= PopulatedCollectionFlags.BiotaPropertiesString;
            if (biota.BiotaPropertiesTextureMap != null && biota.BiotaPropertiesTextureMap.Count > 0) populatedCollectionFlags |= PopulatedCollectionFlags.BiotaPropertiesTextureMap;
            if (biota.HousePermission != null && biota.HousePermission.Count > 0) populatedCollectionFlags |= PopulatedCollectionFlags.HousePermission;

            biota.PopulatedCollectionFlags = (uint)populatedCollectionFlags;
        }

        private static readonly ConditionalWeakTable<Biota, ShardDbContext> BiotaContexts = new ConditionalWeakTable<Biota, ShardDbContext>();

        private static Biota GetBiota(ShardDbContext context, uint id)
        {
            var biota = context.Biota
                .FirstOrDefault(r => r.Id == id);

            if (biota == null)
                return null;

            PopulatedCollectionFlags populatedCollectionFlags = (PopulatedCollectionFlags)biota.PopulatedCollectionFlags;

            // todo: There are gains to be had here if we can conditionally perform mulitple .Include (.Where) statements in a single query.
            // todo: Until I figure out how to do that, this is still pretty good. Mag-nus 2018-08-10
            if (populatedCollectionFlags.HasFlag(PopulatedCollectionFlags.BiotaPropertiesAnimPart)) biota.BiotaPropertiesAnimPart = context.BiotaPropertiesAnimPart.Where(r => r.ObjectId == biota.Id).ToList();
            if (populatedCollectionFlags.HasFlag(PopulatedCollectionFlags.BiotaPropertiesAttribute)) biota.BiotaPropertiesAttribute = context.BiotaPropertiesAttribute.Where(r => r.ObjectId == biota.Id).ToList();
            if (populatedCollectionFlags.HasFlag(PopulatedCollectionFlags.BiotaPropertiesAttribute2nd)) biota.BiotaPropertiesAttribute2nd = context.BiotaPropertiesAttribute2nd.Where(r => r.ObjectId == biota.Id).ToList();
            if (populatedCollectionFlags.HasFlag(PopulatedCollectionFlags.BiotaPropertiesBodyPart)) biota.BiotaPropertiesBodyPart = context.BiotaPropertiesBodyPart.Where(r => r.ObjectId == biota.Id).ToList();
            if (populatedCollectionFlags.HasFlag(PopulatedCollectionFlags.BiotaPropertiesBook)) biota.BiotaPropertiesBook = context.BiotaPropertiesBook.FirstOrDefault(r => r.ObjectId == biota.Id);
            if (populatedCollectionFlags.HasFlag(PopulatedCollectionFlags.BiotaPropertiesBookPageData)) biota.BiotaPropertiesBookPageData = context.BiotaPropertiesBookPageData.Where(r => r.ObjectId == biota.Id).ToList();
            if (populatedCollectionFlags.HasFlag(PopulatedCollectionFlags.BiotaPropertiesBool)) biota.BiotaPropertiesBool = context.BiotaPropertiesBool.Where(r => r.ObjectId == biota.Id).ToList();
            if (populatedCollectionFlags.HasFlag(PopulatedCollectionFlags.BiotaPropertiesCreateList)) biota.BiotaPropertiesCreateList = context.BiotaPropertiesCreateList.Where(r => r.ObjectId == biota.Id).ToList();
            if (populatedCollectionFlags.HasFlag(PopulatedCollectionFlags.BiotaPropertiesDID)) biota.BiotaPropertiesDID = context.BiotaPropertiesDID.Where(r => r.ObjectId == biota.Id).ToList();
            if (populatedCollectionFlags.HasFlag(PopulatedCollectionFlags.BiotaPropertiesEmote)) biota.BiotaPropertiesEmote = context.BiotaPropertiesEmote.Include(r => r.BiotaPropertiesEmoteAction).Where(r => r.ObjectId == biota.Id).ToList();
            if (populatedCollectionFlags.HasFlag(PopulatedCollectionFlags.BiotaPropertiesEnchantmentRegistry)) biota.BiotaPropertiesEnchantmentRegistry = context.BiotaPropertiesEnchantmentRegistry.Where(r => r.ObjectId == biota.Id).ToList();
            if (populatedCollectionFlags.HasFlag(PopulatedCollectionFlags.BiotaPropertiesEventFilter)) biota.BiotaPropertiesEventFilter = context.BiotaPropertiesEventFilter.Where(r => r.ObjectId == biota.Id).ToList();
            if (populatedCollectionFlags.HasFlag(PopulatedCollectionFlags.BiotaPropertiesFloat)) biota.BiotaPropertiesFloat = context.BiotaPropertiesFloat.Where(r => r.ObjectId == biota.Id).ToList();
            if (populatedCollectionFlags.HasFlag(PopulatedCollectionFlags.BiotaPropertiesGenerator)) biota.BiotaPropertiesGenerator = context.BiotaPropertiesGenerator.Where(r => r.ObjectId == biota.Id).ToList();
            if (populatedCollectionFlags.HasFlag(PopulatedCollectionFlags.BiotaPropertiesIID)) biota.BiotaPropertiesIID = context.BiotaPropertiesIID.Where(r => r.ObjectId == biota.Id).ToList();
            if (populatedCollectionFlags.HasFlag(PopulatedCollectionFlags.BiotaPropertiesInt)) biota.BiotaPropertiesInt = context.BiotaPropertiesInt.Where(r => r.ObjectId == biota.Id).ToList();
            if (populatedCollectionFlags.HasFlag(PopulatedCollectionFlags.BiotaPropertiesInt64)) biota.BiotaPropertiesInt64 = context.BiotaPropertiesInt64.Where(r => r.ObjectId == biota.Id).ToList();
            if (populatedCollectionFlags.HasFlag(PopulatedCollectionFlags.BiotaPropertiesPalette)) biota.BiotaPropertiesPalette = context.BiotaPropertiesPalette.Where(r => r.ObjectId == biota.Id).ToList();
            if (populatedCollectionFlags.HasFlag(PopulatedCollectionFlags.BiotaPropertiesPosition)) biota.BiotaPropertiesPosition = context.BiotaPropertiesPosition.Where(r => r.ObjectId == biota.Id).ToList();
            if (populatedCollectionFlags.HasFlag(PopulatedCollectionFlags.BiotaPropertiesSkill)) biota.BiotaPropertiesSkill = context.BiotaPropertiesSkill.Where(r => r.ObjectId == biota.Id).ToList();
            if (populatedCollectionFlags.HasFlag(PopulatedCollectionFlags.BiotaPropertiesSpellBook)) biota.BiotaPropertiesSpellBook = context.BiotaPropertiesSpellBook.Where(r => r.ObjectId == biota.Id).ToList();
            if (populatedCollectionFlags.HasFlag(PopulatedCollectionFlags.BiotaPropertiesString)) biota.BiotaPropertiesString = context.BiotaPropertiesString.Where(r => r.ObjectId == biota.Id).ToList();
            if (populatedCollectionFlags.HasFlag(PopulatedCollectionFlags.BiotaPropertiesTextureMap)) biota.BiotaPropertiesTextureMap = context.BiotaPropertiesTextureMap.Where(r => r.ObjectId == biota.Id).ToList();
            if (populatedCollectionFlags.HasFlag(PopulatedCollectionFlags.HousePermission)) biota.HousePermission = context.HousePermission.Where(r => r.HouseId == biota.Id).ToList();

            return biota;
        }

        public Biota GetBiota(uint id)
        {
            if (ObjectGuid.IsPlayer(id))
            {
                var context = new ShardDbContext();

                var biota = GetBiota(context, id);

                if (biota != null)
                    BiotaContexts.Add(biota, context);

                return biota;
            }

            using (var context = new ShardDbContext())
            {
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                return GetBiota(context, id);
            }
        }

        public List<Biota> GetBiotasByWcid(uint wcid)
        {
            using (var context = new ShardDbContext())
            {
                var results = context.Biota.Where(r => r.WeenieClassId == wcid);

                var biotas = new List<Biota>();
                foreach (var result in results)
                    biotas.Add(GetBiota(context, result.Id));

                return biotas;
            }
        }

        public bool SaveBiota(Biota biota, ReaderWriterLockSlim rwLock)
        {
            if (BiotaContexts.TryGetValue(biota, out var cachedContext))
            {
                rwLock.EnterReadLock();
                try
                {
                    SetBiotaPopulatedCollections(biota);

                    Exception firstException = null;
                    retry:

                    try
                    {
                        cachedContext.SaveChanges();

                        if (firstException != null)
                            log.Debug($"SaveBiota 0x{biota.Id:X8}:{biota.GetProperty(PropertyString.Name)} retry succeeded after initial exception of: {firstException.GetFullMessage()}");

                        return true;
                    }
                    catch (Exception ex)
                    {
                        if (firstException == null)
                        {
                            firstException = ex;
                            goto retry;
                        }

                        // Character name might be in use or some other fault
                        log.Error($"SaveBiota 0x{biota.Id:X8}:{biota.GetProperty(PropertyString.Name)} failed first attempt with exception: {firstException}");
                        log.Error($"SaveBiota 0x{biota.Id:X8}:{biota.GetProperty(PropertyString.Name)} failed second attempt with exception: {ex}");
                        return false;
                    }
                }
                finally
                {
                    rwLock.ExitReadLock();
                }
            }

            if (ObjectGuid.IsPlayer(biota.Id))
            {
                var context = new ShardDbContext();

                BiotaContexts.Add(biota, context);

                rwLock.EnterReadLock();
                try
                {
                    SetBiotaPopulatedCollections(biota);

                    context.Biota.Add(biota);

                    Exception firstException = null;
                    retry:

                    try
                    {
                        context.SaveChanges();

                        if (firstException != null)
                            log.Debug($"SaveBiota 0x{biota.Id:X8}:{biota.GetProperty(PropertyString.Name)} retry succeeded after initial exception of: {firstException.GetFullMessage()}");

                        return true;
                    }
                    catch (Exception ex)
                    {
                        if (firstException == null)
                        {
                            firstException = ex;
                            goto retry;
                        }

                        // Character name might be in use or some other fault
                        log.Error($"SaveBiota 0x{biota.Id:X8}:{biota.GetProperty(PropertyString.Name)} failed first attempt with exception: {firstException}");
                        log.Error($"SaveBiota 0x{biota.Id:X8}:{biota.GetProperty(PropertyString.Name)} failed second attempt with exception: {ex}");
                        return false;
                    }
                }
                finally
                {
                    rwLock.ExitReadLock();
                }
            }

            using (var context = new ShardDbContext())
            {
                var existingBiota = GetBiota(context, biota.Id);

                rwLock.EnterReadLock();
                try
                {
                    SetBiotaPopulatedCollections(biota);

                    if (existingBiota == null)
                        context.Biota.Add(biota);
                    else
                        UpdateBiota(context, existingBiota, biota);

                    Exception firstException = null;
                    retry:

                    try
                    {
                        context.SaveChanges();

                        if (firstException != null)
                            log.Debug($"SaveBiota 0x{biota.Id:X8}:{biota.GetProperty(PropertyString.Name)} retry succeeded after initial exception of: {firstException.GetFullMessage()}");

                        return true;
                    }
                    catch (Exception ex)
                    {
                        if (firstException == null)
                        {
                            firstException = ex;
                            goto retry;
                        }

                        // Character name might be in use or some other fault
                        log.Error($"SaveBiota 0x{biota.Id:X8}:{biota.GetProperty(PropertyString.Name)} failed first attempt with exception: {firstException}");
                        log.Error($"SaveBiota 0x{biota.Id:X8}:{biota.GetProperty(PropertyString.Name)} failed second attempt with exception: {ex}");
                        return false;
                    }
                }
                finally
                {
                    rwLock.ExitReadLock();
                }
            }
        }

        private void UpdateBiota(ShardDbContext context, Biota existingBiota, Biota biota)
        {
            // This pattern is described here: https://docs.microsoft.com/en-us/ef/core/saving/disconnected-entities
            // You'll notice though that we're not using the recommended: context.Entry(existingEntry).CurrentValues.SetValues(newEntry);
            // It is EXTREMLY slow. 4x or more slower. I suspect because it uses reflection to find the properties that the object contains
            // Manually setting the properties like we do below is the best case scenario for performance. However, it also has risks.
            // If we add columns to the schema and forget to add those changes here, changes to the biota may not propegate to the database.
            // Mag-nus 2018-08-18

            context.Entry(existingBiota).CurrentValues.SetValues(biota);

            foreach (var value in biota.BiotaPropertiesAnimPart)
            {
                BiotaPropertiesAnimPart existingValue = (value.Id == 0 ? null : existingBiota.BiotaPropertiesAnimPart.FirstOrDefault(r => r.Id == value.Id));

                if (existingValue == null)
                    existingBiota.BiotaPropertiesAnimPart.Add(value);
                else
                {
                    existingValue.Index = value.Index;
                    existingValue.AnimationId = value.AnimationId;
                    existingValue.Order = value.Order;
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesAnimPart)
            {
                if (!biota.BiotaPropertiesAnimPart.Any(p => p.Id == value.Id))
                    context.BiotaPropertiesAnimPart.Remove(value);
            }

            foreach (var value in biota.BiotaPropertiesAttribute)
            {
                BiotaPropertiesAttribute existingValue = (value.Id == 0 ? null : existingBiota.BiotaPropertiesAttribute.FirstOrDefault(r => r.Id == value.Id));

                if (existingValue == null)
                    existingBiota.BiotaPropertiesAttribute.Add(value);
                else
                {
                    existingValue.Type = value.Type;
                    existingValue.InitLevel = value.InitLevel;
                    existingValue.LevelFromCP = value.LevelFromCP;
                    existingValue.CPSpent = value.CPSpent;
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesAttribute)
            {
                if (!biota.BiotaPropertiesAttribute.Any(p => p.Id == value.Id))
                    context.BiotaPropertiesAttribute.Remove(value);
            }

            foreach (var value in biota.BiotaPropertiesAttribute2nd)
            {
                BiotaPropertiesAttribute2nd existingValue = (value.Id == 0 ? null : existingBiota.BiotaPropertiesAttribute2nd.FirstOrDefault(r => r.Id == value.Id));

                if (existingValue == null)
                    existingBiota.BiotaPropertiesAttribute2nd.Add(value);
                else
                {
                    existingValue.Type = value.Type;
                    existingValue.InitLevel = value.InitLevel;
                    existingValue.LevelFromCP = value.LevelFromCP;
                    existingValue.CPSpent = value.CPSpent;
                    existingValue.CurrentLevel = value.CurrentLevel;
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesAttribute2nd)
            {
                if (!biota.BiotaPropertiesAttribute2nd.Any(p => p.Id == value.Id))
                    context.BiotaPropertiesAttribute2nd.Remove(value);
            }

            foreach (var value in biota.BiotaPropertiesBodyPart)
            {
                BiotaPropertiesBodyPart existingValue = (value.Id == 0 ? null : existingBiota.BiotaPropertiesBodyPart.FirstOrDefault(r => r.Id == value.Id));

                if (existingValue == null)
                    existingBiota.BiotaPropertiesBodyPart.Add(value);
                else
                {
                    existingValue.Key = value.Key;
                    existingValue.DType = value.DType;
                    existingValue.DVal = value.DVal;
                    existingValue.DVar = value.DVar;
                    existingValue.BaseArmor = value.BaseArmor;
                    existingValue.ArmorVsSlash = value.ArmorVsSlash;
                    existingValue.ArmorVsPierce = value.ArmorVsPierce;
                    existingValue.ArmorVsBludgeon = value.ArmorVsBludgeon;
                    existingValue.ArmorVsCold = value.ArmorVsCold;
                    existingValue.ArmorVsFire = value.ArmorVsFire;
                    existingValue.ArmorVsAcid = value.ArmorVsAcid;
                    existingValue.ArmorVsElectric = value.ArmorVsElectric;
                    existingValue.ArmorVsNether = value.ArmorVsNether;
                    existingValue.BH = value.BH;
                    existingValue.HLF = value.HLF;
                    existingValue.MLF = value.MLF;
                    existingValue.LLF = value.LLF;
                    existingValue.HRF = value.HRF;
                    existingValue.MRF = value.MRF;
                    existingValue.LRF = value.LRF;
                    existingValue.HLB = value.HLB;
                    existingValue.MLB = value.MLB;
                    existingValue.LLB = value.LLB;
                    existingValue.HRB = value.HRB;
                    existingValue.MRB = value.MRB;
                    existingValue.LRB = value.LRB;
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesBodyPart)
            {
                if (!biota.BiotaPropertiesBodyPart.Any(p => p.Id == value.Id))
                    context.BiotaPropertiesBodyPart.Remove(value);
            }

            if (biota.BiotaPropertiesBook != null)
            {
                if (existingBiota.BiotaPropertiesBook == null)
                    existingBiota.BiotaPropertiesBook = biota.BiotaPropertiesBook;
                else
                {
                    existingBiota.BiotaPropertiesBook.MaxNumPages = biota.BiotaPropertiesBook.MaxNumPages;
                    existingBiota.BiotaPropertiesBook.MaxNumCharsPerPage = biota.BiotaPropertiesBook.MaxNumCharsPerPage;
                }
            }
            else
            {
                if (existingBiota.BiotaPropertiesBook != null)
                    context.BiotaPropertiesBook.Remove(existingBiota.BiotaPropertiesBook);
            }

            foreach (var value in biota.BiotaPropertiesBookPageData)
            {
                BiotaPropertiesBookPageData existingValue = (value.Id == 0 ? null : existingBiota.BiotaPropertiesBookPageData.FirstOrDefault(r => r.Id == value.Id));

                if (existingValue == null)
                    existingBiota.BiotaPropertiesBookPageData.Add(value);
                else
                {
                    existingValue.PageId = value.PageId;
                    existingValue.AuthorId = value.AuthorId;
                    existingValue.AuthorName = value.AuthorName;
                    existingValue.AuthorAccount = value.AuthorAccount;
                    existingValue.IgnoreAuthor = value.IgnoreAuthor;
                    existingValue.PageText = value.PageText;
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesBookPageData)
            {
                if (!biota.BiotaPropertiesBookPageData.Any(p => p.Id == value.Id))
                    context.BiotaPropertiesBookPageData.Remove(value);
            }

            foreach (var value in biota.BiotaPropertiesBool)
            {
                BiotaPropertiesBool existingValue = (value.Id == 0 ? null : existingBiota.BiotaPropertiesBool.FirstOrDefault(r => r.Id == value.Id));

                if (existingValue == null)
                    existingBiota.BiotaPropertiesBool.Add(value);
                else
                {
                    existingValue.Type = value.Type;
                    existingValue.Value = value.Value;
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesBool)
            {
                if (!biota.BiotaPropertiesBool.Any(p => p.Id == value.Id))
                    context.BiotaPropertiesBool.Remove(value);
            }

            foreach (var value in biota.BiotaPropertiesCreateList)
            {
                BiotaPropertiesCreateList existingValue = (value.Id == 0 ? null : existingBiota.BiotaPropertiesCreateList.FirstOrDefault(r => r.Id == value.Id));

                if (existingValue == null)
                    existingBiota.BiotaPropertiesCreateList.Add(value);
                else
                {
                    existingValue.DestinationType = value.DestinationType;
                    existingValue.WeenieClassId = value.WeenieClassId;
                    existingValue.StackSize = value.StackSize;
                    existingValue.Palette = value.Palette;
                    existingValue.Shade = value.Shade;
                    existingValue.TryToBond = value.TryToBond;
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesCreateList)
            {
                if (!biota.BiotaPropertiesCreateList.Any(p => p.Id == value.Id))
                    context.BiotaPropertiesCreateList.Remove(value);
            }

            foreach (var value in biota.BiotaPropertiesDID)
            {
                BiotaPropertiesDID existingValue = (value.Id == 0 ? null : existingBiota.BiotaPropertiesDID.FirstOrDefault(r => r.Id == value.Id));

                if (existingValue == null)
                    existingBiota.BiotaPropertiesDID.Add(value);
                else
                {
                    existingValue.Type = value.Type;
                    existingValue.Value = value.Value;
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesDID)
            {
                if (!biota.BiotaPropertiesDID.Any(p => p.Id == value.Id))
                    context.BiotaPropertiesDID.Remove(value);
            }

            foreach (var value in biota.BiotaPropertiesEmote)
            {
                BiotaPropertiesEmote existingValue = (value.Id == 0 ? null : existingBiota.BiotaPropertiesEmote.FirstOrDefault(r => r.Id == value.Id));

                if (existingValue == null)
                    existingBiota.BiotaPropertiesEmote.Add(value);
                else
                {
                    existingValue.Category = value.Category;
                    existingValue.Probability = value.Probability;
                    existingValue.WeenieClassId = value.WeenieClassId;
                    existingValue.Style = value.Style;
                    existingValue.Substyle = value.Substyle;
                    existingValue.Quest = value.Quest;
                    existingValue.VendorType = value.VendorType;
                    existingValue.MinHealth = value.MinHealth;
                    existingValue.MaxHealth = value.MaxHealth;

                    foreach (var value2 in value.BiotaPropertiesEmoteAction)
                    {
                        BiotaPropertiesEmoteAction existingValue2 = (value2.Id == 0 ? null : existingValue.BiotaPropertiesEmoteAction.FirstOrDefault(r => r.Id == value2.Id));

                        if (existingValue2 == null)
                            existingValue.BiotaPropertiesEmoteAction.Add(value2);
                        else
                        {
                            existingValue2.EmoteId = value2.EmoteId;
                            existingValue2.Order = value2.Order;
                            existingValue2.Type = value2.Type;
                            existingValue2.Delay = value2.Delay;
                            existingValue2.Extent = value2.Extent;
                            existingValue2.Motion = value2.Motion;
                            existingValue2.Message = value2.Message;
                            existingValue2.TestString = value2.TestString;
                            existingValue2.Min = value2.Min;
                            existingValue2.Max = value2.Max;
                            existingValue2.Min64 = value2.Min64;
                            existingValue2.Max64 = value2.Max64;
                            existingValue2.MinDbl = value2.MinDbl;
                            existingValue2.MaxDbl = value2.MaxDbl;
                            existingValue2.Stat = value2.Stat;
                            existingValue2.Display = value2.Display;
                            existingValue2.Amount = value2.Amount;
                            existingValue2.Amount64 = value2.Amount64;
                            existingValue2.HeroXP64 = value2.HeroXP64;
                            existingValue2.Percent = value2.Percent;
                            existingValue2.SpellId = value2.SpellId;
                            existingValue2.WealthRating = value2.WealthRating;
                            existingValue2.TreasureClass = value2.TreasureClass;
                            existingValue2.TreasureType = value2.TreasureType;
                            existingValue2.PScript = value2.PScript;
                            existingValue2.Sound = value2.Sound;
                            existingValue2.DestinationType = value2.DestinationType;
                            existingValue2.WeenieClassId = value2.WeenieClassId;
                            existingValue2.StackSize = value2.StackSize;
                            existingValue2.Palette = value2.Palette;
                            existingValue2.Shade = value2.Shade;
                            existingValue2.TryToBond = value2.TryToBond;
                            existingValue2.ObjCellId = value2.ObjCellId;
                            existingValue2.OriginX = value2.OriginX;
                            existingValue2.OriginY = value2.OriginY;
                            existingValue2.OriginZ = value2.OriginZ;
                            existingValue2.AnglesW = value2.AnglesW;
                            existingValue2.AnglesX = value2.AnglesX;
                            existingValue2.AnglesY = value2.AnglesY;
                            existingValue2.AnglesZ = value2.AnglesZ;
                        }
                    }
                    foreach (var value2 in value.BiotaPropertiesEmoteAction)
                    {
                        if (!existingValue.BiotaPropertiesEmoteAction.Any(p => p.Id == value2.Id))
                            context.BiotaPropertiesEmoteAction.Remove(value2);
                    }
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesEmote)
            {
                if (!biota.BiotaPropertiesEmote.Any(p => p.Id == value.Id))
                    context.BiotaPropertiesEmote.Remove(value);
            }

            foreach (var value in biota.BiotaPropertiesEnchantmentRegistry)
            {
                BiotaPropertiesEnchantmentRegistry existingValue = (value.ObjectId == 0 ? null : existingBiota.BiotaPropertiesEnchantmentRegistry.FirstOrDefault(r => r.ObjectId == value.ObjectId && r.SpellId == value.SpellId && r.LayerId == value.LayerId && r.CasterObjectId == value.CasterObjectId));

                if (existingValue == null)
                    existingBiota.BiotaPropertiesEnchantmentRegistry.Add(value);
                else
                {
                    existingValue.EnchantmentCategory = value.EnchantmentCategory;
                    existingValue.SpellId = value.SpellId;
                    existingValue.LayerId = value.LayerId;
                    existingValue.HasSpellSetId = value.HasSpellSetId;
                    existingValue.SpellCategory = value.SpellCategory;
                    existingValue.PowerLevel = value.PowerLevel;
                    existingValue.StartTime = value.StartTime;
                    existingValue.Duration = value.Duration;
                    existingValue.CasterObjectId = value.CasterObjectId;
                    existingValue.DegradeModifier = value.DegradeModifier;
                    existingValue.DegradeLimit = value.DegradeLimit;
                    existingValue.LastTimeDegraded = value.LastTimeDegraded;
                    existingValue.StatModType = value.StatModType;
                    existingValue.StatModKey = value.StatModKey;
                    existingValue.StatModValue = value.StatModValue;
                    existingValue.SpellSetId = value.SpellSetId;
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesEnchantmentRegistry)
            {
                if (!biota.BiotaPropertiesEnchantmentRegistry.Any(p => p.ObjectId == value.ObjectId && p.SpellId == value.SpellId && p.LayerId == value.LayerId && p.CasterObjectId == value.CasterObjectId))
                    context.BiotaPropertiesEnchantmentRegistry.Remove(value);
            }

            foreach (var value in biota.BiotaPropertiesEventFilter)
            {
                BiotaPropertiesEventFilter existingValue = (value.Id == 0 ? null : existingBiota.BiotaPropertiesEventFilter.FirstOrDefault(r => r.Id == value.Id));

                if (existingValue == null)
                    existingBiota.BiotaPropertiesEventFilter.Add(value);
                else
                {
                    existingValue.Event = value.Event;
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesEventFilter)
            {
                if (!biota.BiotaPropertiesEventFilter.Any(p => p.Id == value.Id))
                    context.BiotaPropertiesEventFilter.Remove(value);
            }

            foreach (var value in biota.BiotaPropertiesFloat)
            {
                BiotaPropertiesFloat existingValue = (value.Id == 0 ? null : existingBiota.BiotaPropertiesFloat.FirstOrDefault(r => r.Id == value.Id));

                if (existingValue == null)
                    existingBiota.BiotaPropertiesFloat.Add(value);
                else
                {
                    existingValue.Type = value.Type;
                    existingValue.Value = value.Value;
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesFloat)
            {
                if (!biota.BiotaPropertiesFloat.Any(p => p.Id == value.Id))
                    context.BiotaPropertiesFloat.Remove(value);
            }

            foreach (var value in biota.BiotaPropertiesGenerator)
            {
                BiotaPropertiesGenerator existingValue = (value.Id == 0 ? null : existingBiota.BiotaPropertiesGenerator.FirstOrDefault(r => r.Id == value.Id));

                if (existingValue == null)
                    existingBiota.BiotaPropertiesGenerator.Add(value);
                else
                {
                    existingValue.Probability = value.Probability;
                    existingValue.WeenieClassId = value.WeenieClassId;
                    existingValue.Delay = value.Delay;
                    existingValue.InitCreate = value.InitCreate;
                    existingValue.MaxCreate = value.MaxCreate;
                    existingValue.WhenCreate = value.WhenCreate;
                    existingValue.WhereCreate = value.WhereCreate;
                    existingValue.StackSize = value.StackSize;
                    existingValue.PaletteId = value.PaletteId;
                    existingValue.Shade = value.Shade;
                    existingValue.ObjCellId = value.ObjCellId;
                    existingValue.OriginX = value.OriginX;
                    existingValue.OriginY = value.OriginY;
                    existingValue.OriginZ = value.OriginZ;
                    existingValue.AnglesW = value.AnglesW;
                    existingValue.AnglesX = value.AnglesX;
                    existingValue.AnglesY = value.AnglesY;
                    existingValue.AnglesZ = value.AnglesZ;
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesGenerator)
            {
                if (!biota.BiotaPropertiesGenerator.Any(p => p.Id == value.Id))
                    context.BiotaPropertiesGenerator.Remove(value);
            }

            foreach (var value in biota.BiotaPropertiesIID)
            {
                BiotaPropertiesIID existingValue = (value.Id == 0 ? null : existingBiota.BiotaPropertiesIID.FirstOrDefault(r => r.Id == value.Id));

                if (existingValue == null)
                    existingBiota.BiotaPropertiesIID.Add(value);
                else
                {
                    existingValue.Type = value.Type;
                    existingValue.Value = value.Value;
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesIID)
            {
                if (!biota.BiotaPropertiesIID.Any(p => p.Id == value.Id))
                    context.BiotaPropertiesIID.Remove(value);
            }

            foreach (var value in biota.BiotaPropertiesInt)
            {
                BiotaPropertiesInt existingValue = (value.Id == 0 ? null : existingBiota.BiotaPropertiesInt.FirstOrDefault(r => r.Id == value.Id));

                if (existingValue == null)
                    existingBiota.BiotaPropertiesInt.Add(value);
                else
                {
                    existingValue.Type = value.Type;
                    existingValue.Value = value.Value;
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesInt)
            {
                if (!biota.BiotaPropertiesInt.Any(p => p.Id == value.Id))
                    context.BiotaPropertiesInt.Remove(value);
            }

            foreach (var value in biota.BiotaPropertiesInt64)
            {
                BiotaPropertiesInt64 existingValue = (value.Id == 0 ? null : existingBiota.BiotaPropertiesInt64.FirstOrDefault(r => r.Id == value.Id));

                if (existingValue == null)
                    existingBiota.BiotaPropertiesInt64.Add(value);
                else
                {
                    existingValue.Type = value.Type;
                    existingValue.Value = value.Value;
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesInt64)
            {
                if (!biota.BiotaPropertiesInt64.Any(p => p.Id == value.Id))
                    context.BiotaPropertiesInt64.Remove(value);
            }

            foreach (var value in biota.BiotaPropertiesPalette)
            {
                BiotaPropertiesPalette existingValue = (value.Id == 0 ? null : existingBiota.BiotaPropertiesPalette.FirstOrDefault(r => r.Id == value.Id));

                if (existingValue == null)
                    existingBiota.BiotaPropertiesPalette.Add(value);
                else
                {
                    existingValue.SubPaletteId = value.SubPaletteId;
                    existingValue.Offset = value.Offset;
                    existingValue.Length = value.Length;
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesPalette)
            {
                if (!biota.BiotaPropertiesPalette.Any(p => p.Id == value.Id))
                    context.BiotaPropertiesPalette.Remove(value);
            }

            foreach (var value in biota.BiotaPropertiesPosition)
            {
                BiotaPropertiesPosition existingValue = (value.Id == 0 ? null : existingBiota.BiotaPropertiesPosition.FirstOrDefault(r => r.Id == value.Id));

                if (existingValue == null)
                    existingBiota.BiotaPropertiesPosition.Add(value);
                else
                {
                    existingValue.PositionType = value.PositionType;
                    existingValue.ObjCellId = value.ObjCellId;
                    existingValue.OriginX = value.OriginX;
                    existingValue.OriginY = value.OriginY;
                    existingValue.OriginZ = value.OriginZ;
                    existingValue.AnglesW = value.AnglesW;
                    existingValue.AnglesX = value.AnglesX;
                    existingValue.AnglesY = value.AnglesY;
                    existingValue.AnglesZ = value.AnglesZ;
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesPosition)
            {
                if (!biota.BiotaPropertiesPosition.Any(p => p.Id == value.Id))
                    context.BiotaPropertiesPosition.Remove(value);
            }

            foreach (var value in biota.BiotaPropertiesSkill)
            {
                BiotaPropertiesSkill existingValue = (value.Id == 0 ? null : existingBiota.BiotaPropertiesSkill.FirstOrDefault(r => r.Id == value.Id));

                if (existingValue == null)
                    existingBiota.BiotaPropertiesSkill.Add(value);
                else
                {
                    existingValue.Type = value.Type;
                    existingValue.LevelFromPP = value.LevelFromPP;
                    existingValue.SAC = value.SAC;
                    existingValue.PP = value.PP;
                    existingValue.InitLevel = value.InitLevel;
                    existingValue.ResistanceAtLastCheck = value.ResistanceAtLastCheck;
                    existingValue.LastUsedTime = value.LastUsedTime;
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesSkill)
            {
                if (!biota.BiotaPropertiesSkill.Any(p => p.Id == value.Id))
                    context.BiotaPropertiesSkill.Remove(value);
            }

            foreach (var value in biota.BiotaPropertiesSpellBook)
            {
                BiotaPropertiesSpellBook existingValue = (value.Id == 0 ? null : existingBiota.BiotaPropertiesSpellBook.FirstOrDefault(r => r.Id == value.Id));

                if (existingValue == null)
                    existingBiota.BiotaPropertiesSpellBook.Add(value);
                else
                {
                    existingValue.Spell = value.Spell;
                    existingValue.Probability = value.Probability;
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesSpellBook)
            {
                if (!biota.BiotaPropertiesSpellBook.Any(p => p.Id == value.Id))
                    context.BiotaPropertiesSpellBook.Remove(value);
            }

            foreach (var value in biota.BiotaPropertiesString)
            {
                BiotaPropertiesString existingValue = (value.Id == 0 ? null : existingBiota.BiotaPropertiesString.FirstOrDefault(r => r.Id == value.Id));

                if (existingValue == null)
                    existingBiota.BiotaPropertiesString.Add(value);
                else
                {
                    existingValue.Type = value.Type;
                    existingValue.Value = value.Value;
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesString)
            {
                if (!biota.BiotaPropertiesString.Any(p => p.Id == value.Id))
                    context.BiotaPropertiesString.Remove(value);
            }

            foreach (var value in biota.BiotaPropertiesTextureMap)
            {
                BiotaPropertiesTextureMap existingValue = (value.Id == 0 ? null : existingBiota.BiotaPropertiesTextureMap.FirstOrDefault(r => r.Id == value.Id));

                if (existingValue == null)
                    existingBiota.BiotaPropertiesTextureMap.Add(value);
                else
                {
                    existingValue.Index = value.Index;
                    existingValue.OldId = value.OldId;
                    existingValue.NewId = value.NewId;
                    existingValue.Order = value.Order;
                }
            }
            foreach (var value in existingBiota.BiotaPropertiesTextureMap)
            {
                if (!biota.BiotaPropertiesTextureMap.Any(p => p.Id == value.Id))
                    context.BiotaPropertiesTextureMap.Remove(value);
            }

            foreach (var value in biota.HousePermission)
            {
                HousePermission existingValue = (value.Id == 0 ? null : existingBiota.HousePermission.FirstOrDefault(r => r.Id == value.Id));

                if (existingValue == null)
                    existingBiota.HousePermission.Add(value);
                else
                {
                    existingValue.PlayerGuid = value.PlayerGuid;
                    existingValue.Storage = value.Storage;
                }
            }
            foreach (var value in existingBiota.HousePermission)
            {
                if (!biota.HousePermission.Any(p => p.Id == value.Id))
                    context.HousePermission.Remove(value);
            }
        }

        public bool SaveBiotasInParallel(IEnumerable<(Biota biota, ReaderWriterLockSlim rwLock)> biotas)
        {
            var result = true;

            Parallel.ForEach(biotas, biota =>
            {
                if (!SaveBiota(biota.biota, biota.rwLock))
                    result = false;
            });

            return result;
        }

        public bool RemoveBiota(Biota biota, ReaderWriterLockSlim rwLock)
        {
            if (BiotaContexts.TryGetValue(biota, out var cachedContext))
            {
                BiotaContexts.Remove(biota);

                rwLock.EnterReadLock();
                try
                {
                    cachedContext.Biota.Remove(biota);

                    Exception firstException = null;
                    retry:

                    try
                    {
                        cachedContext.SaveChanges();

                        if (firstException != null)
                            log.Debug($"RemoveBiota 0x{biota.Id:X8}:{biota.GetProperty(PropertyString.Name)} retry succeeded after initial exception of: {firstException.GetFullMessage()}");

                        return true;
                    }
                    catch (Exception ex)
                    {
                        if (firstException == null)
                        {
                            firstException = ex;
                            goto retry;
                        }

                        // Character name might be in use or some other fault
                        log.Error($"RemoveBiota 0x{biota.Id:X8}:{biota.GetProperty(PropertyString.Name)} failed first attempt with exception: {firstException}");
                        log.Error($"RemoveBiota 0x{biota.Id:X8}:{biota.GetProperty(PropertyString.Name)} failed second attempt with exception: {ex}");
                        return false;
                    }
                }
                finally
                {
                    rwLock.ExitReadLock();

                    cachedContext.Dispose();
                }
            }

            if (!ObjectGuid.IsPlayer(biota.Id))
            {
                using (var context = new ShardDbContext())
                {
                    var existingBiota = context.Biota
                        .AsNoTracking()
                        .FirstOrDefault(r => r.Id == biota.Id);

                    if (existingBiota == null)
                        return true;

                    rwLock.EnterWriteLock();
                    try
                    {
                        context.Biota.Remove(biota);

                        Exception firstException = null;
                        retry:

                        try
                        {
                            context.SaveChanges();

                            if (firstException != null)
                                log.Debug($"RemoveBiota 0x{biota.Id:X8}:{biota.GetProperty(PropertyString.Name)} retry succeeded after initial exception of: {firstException.GetFullMessage()}");

                            return true;
                        }
                        catch (Exception ex)
                        {
                            if (firstException == null)
                            {
                                firstException = ex;
                                goto retry;
                            }

                            // Character name might be in use or some other fault
                            log.Error($"RemoveBiota 0x{biota.Id:X8}:{biota.GetProperty(PropertyString.Name)} failed first attempt with exception: {firstException}");
                            log.Error($"RemoveBiota 0x{biota.Id:X8}:{biota.GetProperty(PropertyString.Name)} failed second attempt with exception: {ex}");
                            return false;
                        }
                    }
                    finally
                    {
                        rwLock.ExitWriteLock();
                    }
                }
            }

            // If we got here, the biota didn't come from the database through this class.
            // Most likely, it doesn't exist in the database, so, no need to remove.
            return true;
        }

        public bool RemoveBiotasInParallel(IEnumerable<(Biota biota, ReaderWriterLockSlim rwLock)> biotas)
        {
            var result = true;

            Parallel.ForEach(biotas, biota =>
            {
                if (!RemoveBiota(biota.biota, biota.rwLock))
                    result = false;
            });

            return result;
        }

        public void FreeBiotaAndDisposeContext(Biota biota)
        {
            if (BiotaContexts.TryGetValue(biota, out var context))
            {
                BiotaContexts.Remove(biota);
                context.Dispose();
            }
        }

        public void FreeBiotaAndDisposeContexts(IEnumerable<Biota> biotas)
        {
            foreach (var biota in biotas)
                FreeBiotaAndDisposeContext(biota);
        }


        public PossessedBiotas GetPossessedBiotasInParallel(uint id)
        {
            var inventory = GetInventoryInParallel(id, true);

            var wieldedItems = GetWieldedItemsInParallel(id);

            return new PossessedBiotas(inventory, wieldedItems);
        }

        public List<Biota> GetInventoryInParallel(uint parentId, bool includedNestedItems)
        {
            var inventory = new ConcurrentBag<Biota>();

            using (var context = new ShardDbContext())
            {
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                var results = context.BiotaPropertiesIID
                    .Where(r => r.Type == (ushort)PropertyInstanceId.Container && r.Value == parentId)
                    .ToList();

                Parallel.ForEach(results, result =>
                {
                    var biota = GetBiota(result.ObjectId);

                    if (biota != null)
                    {
                        inventory.Add(biota);

                        if (includedNestedItems && biota.WeenieType == (int)WeenieType.Container)
                        {
                            var subItems = GetInventoryInParallel(biota.Id, false);

                            foreach (var subItem in subItems)
                                inventory.Add(subItem);
                        }
                    }
                });
            }

            return inventory.ToList();
        }

        public List<Biota> GetWieldedItemsInParallel(uint parentId)
        {
            var wieldedItems = new ConcurrentBag<Biota>();

            using (var context = new ShardDbContext())
            {
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                var results = context.BiotaPropertiesIID
                    .Where(r => r.Type == (ushort)PropertyInstanceId.Wielder && r.Value == parentId)
                    .ToList();

                Parallel.ForEach(results, result =>
                {
                    var biota = GetBiota(result.ObjectId);

                    if (biota != null)
                        wieldedItems.Add(biota);
                });
            }

            return wieldedItems.ToList();
        }

        public List<Biota> GetStaticObjectsByLandblock(ushort landblockId)
        {
            var staticObjects = new List<Biota>();

            var staticLandblockId = (uint)(0x70000 | landblockId);

            var min = staticLandblockId << 12;
            var max = min | 0xFFF;

            using (var context = new ShardDbContext())
            {
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                var results = context.Biota.Where(b => b.Id >= min && b.Id <= max).ToList();

                foreach (var result in results)
                {
                    var biota = GetBiota(result.Id);
                    staticObjects.Add(biota);
                }
            }

            return staticObjects;
        }

        public List<Biota> GetStaticObjectsByLandblockInParallel(ushort landblockId)
        {
            var staticObjects = new ConcurrentBag<Biota>();

            var staticLandblockId = (uint)(0x70000 | landblockId);

            var min = staticLandblockId << 12;
            var max = min | 0xFFF;

            using (var context = new ShardDbContext())
            {
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                var results = context.Biota.Where(b => b.Id >= min && b.Id <= max).ToList();

                Parallel.ForEach(results, result =>
                {
                    var biota = GetBiota(result.Id);
                    staticObjects.Add(biota);
                });
            }

            return staticObjects.ToList();
        }

        public List<Biota> GetDynamicObjectsByLandblock(ushort landblockId)
        {
            var dynamics = new List<Biota>();

            var min = (uint)(landblockId << 16);
            var max = min | 0xFFFF;

            using (var context = new ShardDbContext())
            {
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                var results = context.BiotaPropertiesPosition
                    .Where(p => p.PositionType == 1 && p.ObjCellId >= min && p.ObjCellId <= max && p.ObjectId >= 0x80000000)
                    .ToList();

                foreach (var result in results)
                {
                    var biota = GetBiota(result.ObjectId);

                    // Filter out objects that are in a container
                    if (biota.BiotaPropertiesIID.FirstOrDefault(r => r.Type == 2 && r.Value != 0) != null)
                        continue;

                    // Filter out wielded objects
                    if (biota.BiotaPropertiesIID.FirstOrDefault(r => r.Type == 3 && r.Value != 0) != null)
                        continue;

                    dynamics.Add(biota);
                }
            }

            return dynamics;
        }

        public List<Biota> GetDynamicObjectsByLandblockInParallel(ushort landblockId)
        {
            var dynamics = new ConcurrentBag<Biota>();

            var min = (uint)(landblockId << 16);
            var max = min | 0xFFFF;

            using (var context = new ShardDbContext())
            {
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                var results = context.BiotaPropertiesPosition
                    .Where(p => p.PositionType == 1 && p.ObjCellId >= min && p.ObjCellId <= max && p.ObjectId >= 0x80000000)
                    .ToList();

                Parallel.ForEach(results, result =>
                {
                    var biota = GetBiota(result.ObjectId);

                    // Filter out objects that are in a container
                    if (biota.BiotaPropertiesIID.FirstOrDefault(r => r.Type == 2 && r.Value != 0) != null)
                        return;

                    // Filter out wielded objects
                    if (biota.BiotaPropertiesIID.FirstOrDefault(r => r.Type == 3 && r.Value != 0) != null)
                        return;

                    dynamics.Add(biota);
                });
            }

            return dynamics.ToList();
        }

        public List<Biota> GetHousesOwned()
        {
            using (var context = new ShardDbContext())
            {
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                var results = context.Biota.Where(i => i.WeenieType == (int)WeenieType.SlumLord).ToList();

                return results;
            }
        }


        public bool IsCharacterNameAvailable(string name)
        {
            using (var context = new ShardDbContext())
            {
                var result = context.Character
                    .AsNoTracking()
                    .Where(r => !r.IsDeleted)
                    .Where(r => !(r.DeleteTime > 0))
                    .FirstOrDefault(r => r.Name == name);

                return result == null;
            }
        }

        private static readonly ConditionalWeakTable<Character, ShardDbContext> CharacterContexts = new ConditionalWeakTable<Character, ShardDbContext>();

        public Character GetFullCharacter(string name)
        {
            var context = new ShardDbContext();

            var result = context.Character
                .Include(r => r.CharacterPropertiesContract)
                .Include(r => r.CharacterPropertiesFillCompBook)
                .Include(r => r.CharacterPropertiesFriendList)
                .Include(r => r.CharacterPropertiesQuestRegistry)
                .Include(r => r.CharacterPropertiesShortcutBar)
                .Include(r => r.CharacterPropertiesSpellBar)
                .Include(r => r.CharacterPropertiesTitleBook)
                .FirstOrDefault(r => r.Name == name && !r.IsDeleted);

            if (result == null)
                Console.WriteLine($"ShardDatabase.GetFullCharacter({name}): couldn't find character");
            else
                CharacterContexts.Add(result, context);

            return result;
        }

        public List<Character> GetCharacters(uint accountId, bool includeDeleted)
        {
            var context = new ShardDbContext();

            var results = context.Character
                .Include(r => r.CharacterPropertiesContract)
                .Include(r => r.CharacterPropertiesFillCompBook)
                .Include(r => r.CharacterPropertiesFriendList)
                .Include(r => r.CharacterPropertiesQuestRegistry)
                .Include(r => r.CharacterPropertiesShortcutBar)
                .Include(r => r.CharacterPropertiesSpellBar)
                .Include(r => r.CharacterPropertiesTitleBook)
                .Where(r => r.AccountId == accountId && (includeDeleted || !r.IsDeleted))
                .ToList();

            foreach (var result in results)
                CharacterContexts.Add(result, context);

            return results;
        }

        public Character GetCharacterByName(string name) // When searching by name, only non-deleted characters matter
        {
            var context = new ShardDbContext();

            var result = context.Character
                //.Include(r => r.CharacterPropertiesContract)
                //.Include(r => r.CharacterPropertiesFillCompBook)
                //.Include(r => r.CharacterPropertiesFriendList)
                //.Include(r => r.CharacterPropertiesQuestRegistry)
                //.Include(r => r.CharacterPropertiesShortcutBar)
                //.Include(r => r.CharacterPropertiesSpellBar)
                //.Include(r => r.CharacterPropertiesTitleBook)
                .FirstOrDefault(r => r.Name == name.ToLower() && !r.IsDeleted);

            if (result != null)
                CharacterContexts.Add(result, context);

            return result;
        }

        public Character GetCharacterByGuid(uint guid)
        {
            var context = new ShardDbContext();

            var result = context.Character
                //.Include(r => r.CharacterPropertiesContract)
                //.Include(r => r.CharacterPropertiesFillCompBook)
                //.Include(r => r.CharacterPropertiesFriendList)
                //.Include(r => r.CharacterPropertiesQuestRegistry)
                //.Include(r => r.CharacterPropertiesShortcutBar)
                //.Include(r => r.CharacterPropertiesSpellBar)
                //.Include(r => r.CharacterPropertiesTitleBook)
                .FirstOrDefault(r => r.Id == guid);

            if (result != null)
                CharacterContexts.Add(result, context);

            return result;
        }

        public bool SaveCharacter(Character character, ReaderWriterLockSlim rwLock)
        {
            if (CharacterContexts.TryGetValue(character, out var cachedContext))
            {
                rwLock.EnterReadLock();
                try
                {
                    Exception firstException = null;
                    retry:

                    try
                    {
                        cachedContext.SaveChanges();

                        if (firstException != null)
                            log.Debug($"SaveCharacter 0x{character.Id:X8}:{character.Name} retry succeeded after initial exception of: {firstException.GetFullMessage()}");

                        return true;
                    }
                    catch (Exception ex)
                    {
                        if (firstException == null)
                        {
                            firstException = ex;
                            goto retry;
                        }

                        // Character name might be in use or some other fault
                        log.Error($"SaveCharacter 0x{character.Id:X8}:{character.Name} failed first attempt with exception: {firstException}");
                        log.Error($"SaveCharacter 0x{character.Id:X8}:{character.Name} failed second attempt with exception: {ex}");
                        return false;
                    }
                }
                finally
                {
                    rwLock.ExitReadLock();
                }
            }

            var context = new ShardDbContext();

            CharacterContexts.Add(character, context);

            rwLock.EnterReadLock();
            try
            {
                context.Character.Add(character);

                Exception firstException = null;
                retry:

                try
                {
                    context.SaveChanges();

                    if (firstException != null)
                        log.Debug($"SaveCharacter 0x{character.Id:X8}:{character.Name} retry succeeded after initial exception of: {firstException.GetFullMessage()}");

                    return true;
                }
                catch (Exception ex)
                {
                    if (firstException == null)
                    {
                        firstException = ex;
                        goto retry;
                    }

                    // Character name might be in use or some other fault
                    log.Error($"SaveCharacter 0x{character.Id:X8}:{character.Name} failed first attempt with exception: {firstException}");
                    log.Error($"SaveCharacter 0x{character.Id:X8}:{character.Name} failed second attempt with exception: {ex}");
                    return false;
                }
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }


        public bool AddCharacterInParallel(Biota biota, ReaderWriterLockSlim biotaLock, IEnumerable<(Biota biota, ReaderWriterLockSlim rwLock)> possessions, Character character, ReaderWriterLockSlim characterLock)
        {
            if (!SaveBiota(biota, biotaLock))
                return false; // Biota save failed which mean Character fails.

            if (!SaveBiotasInParallel(possessions))
                return false;

            if (!SaveCharacter(character, characterLock))
                return false;

            return true;
        }


        /// <summary>
        /// This will get all player biotas that are backed by characters that are not deleted.
        /// </summary>
        public List<Biota> GetAllPlayerBiotasInParallel()
        {
            var biotas = new ConcurrentBag<Biota>();

            using (var context = new ShardDbContext())
            {
                var results = context.Character
                    .Where(r => !r.IsDeleted)
                    .AsNoTracking()
                    .ToList();

                Parallel.ForEach(results, result =>
                {
                    var biota = GetBiota(result.Id);

                    biotas.Add(biota);
                });
            }

            return biotas.ToList();
        }

        public uint? GetAllegianceID(uint monarchID)
        {
            using (var context = new ShardDbContext())
            {
                var query = from biota in context.Biota
                            join iid in context.BiotaPropertiesIID on biota.Id equals iid.ObjectId
                            where biota.WeenieType == (int)WeenieType.Allegiance && iid.Type == (int)PropertyInstanceId.Monarch && iid.Value == monarchID
                            select biota.Id;

                return query.FirstOrDefault();
            }
        }
    }
}
