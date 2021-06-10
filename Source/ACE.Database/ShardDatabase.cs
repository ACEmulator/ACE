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

using ACE.Common;
using ACE.Common.Extensions;
using ACE.Database.Entity;
using ACE.Database.Models.Shard;
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


        /// <summary>
        /// Will return uint.MaxValue if no records were found within the range provided.
        /// </summary>
        public uint GetMaxGuidFoundInRange(uint min, uint max)
        {
            using (var context = new ShardDbContext())
            {
                var result = context.Biota
                    .AsNoTracking()
                    .Where(r => r.Id >= min && r.Id <= max)
                    .OrderByDescending(r => r.Id)
                    .FirstOrDefault();

                if (result == null)
                    return uint.MaxValue;

                return result.Id;
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
            var sql = "SET @available_ids=0, @rownum=0;"                                                + Environment.NewLine +
                      "SELECT"                                                                          + Environment.NewLine +
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
                context.Database.SetCommandTimeout(TimeSpan.FromMinutes(5));

                var connection = context.Database.GetDbConnection();
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = sql;
                var reader = command.ExecuteReader();

                var gaps = new List<(uint start, uint end)>();

                while (reader.Read())
                {
                    var gap_starts_at               = reader.GetFieldValue<long>(0);
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
            BiotaPropertiesAllegiance           = 0x1000000,
        }

        protected static void SetBiotaPopulatedCollections(Biota biota)
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
            if (biota.BiotaPropertiesAllegiance != null && biota.BiotaPropertiesAllegiance.Count > 0) populatedCollectionFlags |= PopulatedCollectionFlags.BiotaPropertiesAllegiance;

            biota.PopulatedCollectionFlags = (uint)populatedCollectionFlags;
        }

        public virtual Biota GetBiota(ShardDbContext context, uint id, bool doNotAddToCache = false)
        {
            var biota = context.Biota
                .FirstOrDefault(r => r.Id == id);

            if (biota == null)
                return null;

            PopulatedCollectionFlags populatedCollectionFlags = (PopulatedCollectionFlags)biota.PopulatedCollectionFlags;

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
            if (populatedCollectionFlags.HasFlag(PopulatedCollectionFlags.BiotaPropertiesAllegiance)) biota.BiotaPropertiesAllegiance = context.BiotaPropertiesAllegiance.Where(r => r.AllegianceId == biota.Id).ToList();

            return biota;
        }

        public virtual Biota GetBiota(uint id, bool doNotAddToCache = false)
        {
            using (var context = new ShardDbContext())
                return GetBiota(context, id, doNotAddToCache);
        }

        public List<Biota> GetBiotasByWcid(uint wcid)
        {
            using (var context = new ShardDbContext())
            {
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                var results = context.Biota.Where(r => r.WeenieClassId == wcid);

                var biotas = new List<Biota>();
                foreach (var result in results)
                {
                    var biota = GetBiota(result.Id);
                    biotas.Add(biota);
                }

                return biotas;
            }
        }

        public List<Biota> GetBiotasByType(WeenieType type)
        {
            // warning: this query is currently unindexed!
            using (var context = new ShardDbContext())
            {
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                var iType = (int)type;

                var results = context.Biota.Where(r => r.WeenieType == iType);

                var biotas = new List<Biota>();
                foreach (var result in results)
                {
                    var biota = GetBiota(result.Id);
                    biotas.Add(biota);
                }

                return biotas;
            }
        }

        protected bool DoSaveBiota(ShardDbContext context, Biota biota)
        {
            SetBiotaPopulatedCollections(biota);

            Exception firstException = null;
            retry:

            try
            {
                context.SaveChanges();

                if (firstException != null)
                    log.Debug($"[DATABASE] DoSaveBiota 0x{biota.Id:X8}:{biota.GetProperty(PropertyString.Name)} retry succeeded after initial exception of: {firstException.GetFullMessage()}");

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
                log.Error($"[DATABASE] DoSaveBiota 0x{biota.Id:X8}:{biota.GetProperty(PropertyString.Name)} failed first attempt with exception: {firstException.GetFullMessage()}");
                log.Error($"[DATABASE] DoSaveBiota 0x{biota.Id:X8}:{biota.GetProperty(PropertyString.Name)} failed second attempt with exception: {ex.GetFullMessage()}");
                return false;
            }
        }

        public virtual bool SaveBiota(ACE.Entity.Models.Biota biota, ReaderWriterLockSlim rwLock)
        {
            using (var context = new ShardDbContext())
            {
                var existingBiota = GetBiota(context, biota.Id);

                rwLock.EnterReadLock();
                try
                {
                    if (existingBiota == null)
                    {
                        existingBiota = ACE.Database.Adapter.BiotaConverter.ConvertFromEntityBiota(biota);

                        context.Biota.Add(existingBiota);
                    }
                    else
                    {
                        ACE.Database.Adapter.BiotaUpdater.UpdateDatabaseBiota(context, biota, existingBiota);
                    }
                }
                finally
                {
                    rwLock.ExitReadLock();
                }

                return DoSaveBiota(context, existingBiota);
            }
        }

        public bool SaveBiotasInParallel(IEnumerable<(ACE.Entity.Models.Biota biota, ReaderWriterLockSlim rwLock)> biotas)
        {
            var result = true;

            Parallel.ForEach(biotas, ConfigManager.Config.Server.Threading.DatabaseParallelOptions, biota =>
            {
                if (!SaveBiota(biota.biota, biota.rwLock))
                    result = false;
            });

            return result;
        }

        public virtual bool RemoveBiota(uint id)
        {
            using (var context = new ShardDbContext())
            {
                var existingBiota = context.Biota
                    .AsNoTracking()
                    .FirstOrDefault(r => r.Id == id);

                if (existingBiota == null)
                    return true;

                context.Biota.Remove(existingBiota);

                Exception firstException = null;
                retry:

                try
                {
                    context.SaveChanges();

                    if (firstException != null)
                        log.Debug($"[DATABASE] RemoveBiota 0x{id:X8} retry succeeded after initial exception of: {firstException.GetFullMessage()}");

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
                    log.Error($"[DATABASE] RemoveBiota 0x{id:X8} failed first attempt with exception: {firstException.GetFullMessage()}");
                    log.Error($"[DATABASE] RemoveBiota 0x{id:X8} failed second attempt with exception: {ex.GetFullMessage()}");
                    return false;
                }
            }
        }

        public bool RemoveBiotasInParallel(IEnumerable<uint> ids)
        {
            var result = true;

            Parallel.ForEach(ids, ConfigManager.Config.Server.Threading.DatabaseParallelOptions, id =>
            {
                if (!RemoveBiota(id))
                    result = false;
            });

            return result;
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

                Parallel.ForEach(results, ConfigManager.Config.Server.Threading.DatabaseParallelOptions, result =>
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

                Parallel.ForEach(results, ConfigManager.Config.Server.Threading.DatabaseParallelOptions, result =>
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

        public List<Biota> GetHousesOwned()
        {
            using (var context = new ShardDbContext())
            {
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                var query = from biota in context.Biota
                            join iid in context.BiotaPropertiesIID on biota.Id equals iid.ObjectId
                            where biota.WeenieType == (int)WeenieType.SlumLord && iid.Type == (ushort)PropertyInstanceId.HouseOwner
                            select biota;

                var results = query.ToList();

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

        public List<Character> GetCharacters(uint accountId, bool includeDeleted)
        {
            var context = new ShardDbContext();

            var query = context.Character.Where(r => r.AccountId == accountId && (includeDeleted || !r.IsDeleted));

            var results = query.ToList();

            query.Include(r => r.CharacterPropertiesContractRegistry).Load();
            query.Include(r => r.CharacterPropertiesFillCompBook).Load();
            query.Include(r => r.CharacterPropertiesFriendList).Load();
            query.Include(r => r.CharacterPropertiesQuestRegistry).Load();
            query.Include(r => r.CharacterPropertiesShortcutBar).Load();
            query.Include(r => r.CharacterPropertiesSpellBar).Load();
            query.Include(r => r.CharacterPropertiesSquelch).Load();
            query.Include(r => r.CharacterPropertiesTitleBook).Load();

            foreach (var result in results)
                CharacterContexts.Add(result, context);

            return results;
        }

        public Character GetCharacterStubByName(string name) // When searching by name, only non-deleted characters matter
        {
            var context = new ShardDbContext();

            var result = context.Character
                .FirstOrDefault(r => r.Name == name.ToLower() && !r.IsDeleted);

            return result;
        }

        public Character GetCharacterStubByGuid(uint guid)
        {
            var context = new ShardDbContext();

            var result = context.Character
                .FirstOrDefault(r => r.Id == guid);

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
                            log.Debug($"[DATABASE] SaveCharacter-1 0x{character.Id:X8}:{character.Name} retry succeeded after initial exception of: {firstException.GetFullMessage()}");

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
                        log.Error($"[DATABASE] SaveCharacter-1 0x{character.Id:X8}:{character.Name} failed first attempt with exception: {firstException.GetFullMessage()}");
                        log.Error($"[DATABASE] SaveCharacter-1 0x{character.Id:X8}:{character.Name} failed second attempt with exception: {ex.GetFullMessage()}");
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
                        log.Debug($"[DATABASE] SaveCharacter-2 0x{character.Id:X8}:{character.Name} retry succeeded after initial exception of: {firstException.GetFullMessage()}");

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
                    log.Error($"[DATABASE] SaveCharacter-2 0x{character.Id:X8}:{character.Name} failed first attempt with exception: {firstException.GetFullMessage()}");
                    log.Error($"[DATABASE] SaveCharacter-2 0x{character.Id:X8}:{character.Name} failed second attempt with exception: {ex.GetFullMessage()}");
                    return false;
                }
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }


        public bool AddCharacterInParallel(ACE.Entity.Models.Biota biota, ReaderWriterLockSlim biotaLock, IEnumerable<(ACE.Entity.Models.Biota biota, ReaderWriterLockSlim rwLock)> possessions, Character character, ReaderWriterLockSlim characterLock)
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
        public List<ACE.Entity.Models.Biota> GetAllPlayerBiotasInParallel()
        {
            var biotas = new ConcurrentBag<ACE.Entity.Models.Biota>();

            using (var context = new ShardDbContext())
            {
                var results = context.Character
                    .Where(r => !r.IsDeleted)
                    .AsNoTracking()
                    .ToList();

                Parallel.ForEach(results, result =>
                {
                    var biota = GetBiota(result.Id, true);

                    if (biota != null)
                    {
                        var convertedBiota = ACE.Database.Adapter.BiotaConverter.ConvertToEntityBiota(biota);

                        biotas.Add(convertedBiota);
                    }
                    else
                        log.Error($"ShardDatabase.GetAllPlayerBiotasInParallel() - couldn't find biota for character 0x{result.Id:X8}");
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

        public bool RenameCharacter(Character character, string newName, ReaderWriterLockSlim rwLock)
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
                        character.Name = newName;
                        cachedContext.SaveChanges();

                        if (firstException != null)
                            log.Debug($"[DATABASE] RenameCharacter 0x{character.Id:X8}:{character.Name} retry succeeded after initial exception of: {firstException.GetFullMessage()}");

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
                        log.Error($"[DATABASE] RenameCharacter 0x{character.Id:X8}:{character.Name} failed first attempt with exception: {firstException.GetFullMessage()}");
                        log.Error($"[DATABASE] RenameCharacter 0x{character.Id:X8}:{character.Name} failed second attempt with exception: {ex.GetFullMessage()}");
                        return false;
                    }
                }
                finally
                {
                    rwLock.ExitReadLock();
                }
            }

            character.Name = newName;

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
                        log.Debug($"[DATABASE] RenameCharacter 0x{character.Id:X8}:{character.Name} retry succeeded after initial exception of: {firstException.GetFullMessage()}");

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
                    log.Error($"[DATABASE] RenameCharacter 0x{character.Id:X8}:{character.Name} failed first attempt with exception: {firstException.GetFullMessage()}");
                    log.Error($"[DATABASE] RenameCharacter 0x{character.Id:X8}:{character.Name} failed second attempt with exception: {ex.GetFullMessage()}");
                    return false;
                }
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }
    }
}
