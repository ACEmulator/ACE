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
            var config = Common.ConfigManager.Config.MySql.World;

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


        public List<Character> GetCharacters(uint accountId, bool includeDeleted)
        {
            using (var context = new ShardDbContext())
            {
                var results = context.Character
                    .Include(r => r.CharacterPropertiesContract)
                    .Include(r => r.CharacterPropertiesFillCompBook)
                    .Include(r => r.CharacterPropertiesFriendList)
                    .Include(r => r.CharacterPropertiesQuestRegistry)
                    .Include(r => r.CharacterPropertiesShortcutBar)
                    .Include(r => r.CharacterPropertiesSpellBar)
                    .Include(r => r.CharacterPropertiesTitleBook)
                    .AsNoTracking()
                    .Where(r => r.AccountId == accountId && (includeDeleted || !r.IsDeleted))
                    .ToList();

                return results;
            }
        }

        public List<Character> GetAllCharacters()
        {
            using (var context = new ShardDbContext())
            {
                var results = context.Character
                    .AsNoTracking()
                    .ToList();

                return results;
            }
        }

        public List<Character> GetAllegianceCharacters(uint monarchID)
        {
            using (var context = new ShardDbContext())
            {
                /*var results = context.Character
                    .AsNoTracking()
                    .Where(r => r.Biota.BiotaProperties)
                    .ToList();

                return results;*/
            }
            return null;
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

        public bool IsCharacterPlussed(uint biotaId)
        {
            using (var context = new ShardDbContext())
            {
                var result = context.Biota
                    .AsNoTracking()
                    .Include(r => r.BiotaPropertiesBool)
                    .FirstOrDefault(r => r.Id == biotaId);

                if (result == null)
                    return false;

                if (result.GetProperty(PropertyBool.IsAdmin, new ReaderWriterLockSlim()) ?? false)
                    return true;
                if (result.GetProperty(PropertyBool.IsArch, new ReaderWriterLockSlim()) ?? false)
                    return true;
                if (result.GetProperty(PropertyBool.IsPsr, new ReaderWriterLockSlim()) ?? false)
                    return true;
                if (result.GetProperty(PropertyBool.IsSentinel, new ReaderWriterLockSlim()) ?? false)
                    return true;

                if (result.WeenieType == (int)WeenieType.Admin || result.WeenieType == (int)WeenieType.Sentinel)
                    return true;

                return false;
            }
        }

        public bool AddCharacterInParallel(Biota biota, IEnumerable<Biota> possessions, Character character)
        {
            if (!AddBiota(biota))
                return false; // Biota save failed which mean Character fails.

            if (!AddBiotasInParallel(possessions))
                return false;

            using (var context = new ShardDbContext())
            {
                context.Character.Add(character);

                try
                {
                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    // Character name might be in use or some other fault
                    log.Error($"AddCharacter failed with exception: {ex}");
                    return false;
                }
            }
        }

        public bool DeleteOrRestoreCharacter(ulong unixTime, uint guid)
        {
            using (var context = new ShardDbContext())
            {
                var result = context.Character
                    .FirstOrDefault(r => r.Id == guid);

                if (result != null)
                {
                    result.DeleteTime = unixTime;
                    result.IsDeleted = false;
                }
                else
                    return false;

                try
                {
                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    log.Error($"DeleteOrRestoreCharacter failed with exception: {ex}");
                    return false;
                }
            }
        }

        public bool MarkCharacterDeleted(uint guid)
        {
            using (var context = new ShardDbContext())
            {
                var result = context.Character
                    .FirstOrDefault(r => r.Id == guid);

                if (result != null)
                    result.IsDeleted = true;
                else
                    return false;

                try
                {
                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    log.Error($"MarkCharacterDeleted failed with exception: {ex}");
                    return false;
                }
            }
        }

        public bool SaveCharacter(Character character)
        {
            using (var context = new ShardDbContext())
            {
                context.Character.Update(character);

                try
                {
                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    // Character name might be in use or some other fault
                    log.Error($"SaveCharacter failed with exception: {ex}");
                    return false;
                }
            }
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
            BiotaPropertiesTextureMap           = 0x400000
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

            biota.PopulatedCollectionFlags = (uint)populatedCollectionFlags;
        }

        public bool AddBiota(Biota biota)
        {
            using (var context = new ShardDbContext())
            {
                SetBiotaPopulatedCollections(biota);

                context.Biota.Add(biota);

                try
                {
                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    log.Error($"AddBiota failed with exception: {ex}");
                    return false;
                }
            }
        }

        public bool AddBiotasInParallel(IEnumerable<Biota> biotas)
        {
            var result = true;

            Parallel.ForEach(biotas, biota =>
            {
                if (!AddBiota(biota))
                    result = false;
            });

            return result;
        }

        public Biota GetBiota(uint id)
        {
            using (var context = new ShardDbContext())
            {
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

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

                return biota;
            }
        }

        public bool SaveBiota(Biota biota)
        {
            using (var context = new ShardDbContext())
            {
                SetBiotaPopulatedCollections(biota);

                context.Biota.Update(biota);

                try
                {
                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    // Character name might be in use or some other fault
                    log.Error($"SaveBiota failed with exception: {ex}");
                    return false;
                }
            }
        }

        public bool SaveBiotasInParallel(IEnumerable<Biota> biotas)
        {
            var result = true;

            Parallel.ForEach(biotas, biota =>
            {
                if (!SaveBiota(biota))
                    result = false;
            });

            return result;
        }

        public bool RemoveBiota(Biota biota)
        {
            using (var context = new ShardDbContext())
            {
                context.Biota.Remove(biota);

                try
                {
                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    // Character name might be in use or some other fault
                    log.Error($"RemoveBiota failed with exception: {ex}");
                    return false;
                }
            }
        }

        public bool RemoveBiotasInParallel(IEnumerable<Biota> biotas)
        {
            var result = true;

            Parallel.ForEach(biotas, biota =>
            {
                if (!RemoveBiota(biota))
                    result = false;
            });

            return result;
        }


        public bool RemoveEntity(object entity)
        {
            using (var context = new ShardDbContext())
            {
                context.Remove(entity);

                try
                {
                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    // Character name might be in use or some other fault
                    log.Error($"RemoveEntity failed with exception: {ex}");
                    return false;
                }
            }
        }

        public bool RemoveEntity(BiotaPropertiesBool entity)
        {
            using (var context = new ShardDbContext())
            {
                context.BiotaPropertiesBool.Remove(entity);

                try
                {
                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    // Character name might be in use or some other fault
                    log.Error($"RemoveEntity failed with exception: {ex}");
                    return false;
                }
            }
        }

        public bool RemoveEntity(BiotaPropertiesDID entity)
        {
            using (var context = new ShardDbContext())
            {
                context.BiotaPropertiesDID.Remove(entity);

                try
                {
                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    // Character name might be in use or some other fault
                    log.Error($"RemoveEntity failed with exception: {ex}");
                    return false;
                }
            }
        }

        public bool RemoveEntity(BiotaPropertiesEnchantmentRegistry entity)
        {
            using (var context = new ShardDbContext())
            {
                context.BiotaPropertiesEnchantmentRegistry.Remove(entity);

                try
                {
                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    // Character name might be in use or some other fault
                    log.Error($"RemoveEntity failed with exception: {ex}");
                    return false;
                }
            }
        }

        public bool RemoveEntity(BiotaPropertiesFloat entity)
        {
            using (var context = new ShardDbContext())
            {
                context.BiotaPropertiesFloat.Remove(entity);

                try
                {
                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    // Character name might be in use or some other fault
                    log.Error($"RemoveEntity failed with exception: {ex}");
                    return false;
                }
            }
        }

        public bool RemoveEntity(BiotaPropertiesIID entity)
        {
            using (var context = new ShardDbContext())
            {
                context.BiotaPropertiesIID.Remove(entity);

                try
                {
                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    // Character name might be in use or some other fault
                    log.Error($"RemoveEntity failed with exception: {ex}");
                    return false;
                }
            }
        }

        public bool RemoveEntity(BiotaPropertiesInt entity)
        {
            using (var context = new ShardDbContext())
            {
                context.BiotaPropertiesInt.Remove(entity);

                try
                {
                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    // Character name might be in use or some other fault
                    log.Error($"RemoveEntity failed with exception: {ex}");
                    return false;
                }
            }
        }

        public bool RemoveEntity(BiotaPropertiesInt64 entity)
        {
            using (var context = new ShardDbContext())
            {
                context.BiotaPropertiesInt64.Remove(entity);

                try
                {
                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    // Character name might be in use or some other fault
                    log.Error($"RemoveEntity failed with exception: {ex}");
                    return false;
                }
            }
        }

        public bool RemoveEntity(BiotaPropertiesPosition entity)
        {
            using (var context = new ShardDbContext())
            {
                context.BiotaPropertiesPosition.Remove(entity);

                try
                {
                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    // Character name might be in use or some other fault
                    log.Error($"RemoveEntity failed with exception: {ex}");
                    return false;
                }
            }
        }

        public bool RemoveEntity(CharacterPropertiesShortcutBar entity)
        {
            using (var context = new ShardDbContext())
            {
                context.CharacterPropertiesShortcutBar.Remove(entity);

                try
                {
                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    // Character name might be in use or some other fault
                    log.Error($"RemoveEntity failed with exception: {ex}");
                    return false;
                }
            }
        }

        public bool RemoveEntity(CharacterPropertiesSpellBar entity)
        {
            using (var context = new ShardDbContext())
            {
                context.CharacterPropertiesSpellBar.Remove(entity);

                try
                {
                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    // Character name might be in use or some other fault
                    log.Error($"RemoveEntity failed with exception: {ex}");
                    return false;
                }
            }
        }

        public bool RemoveEntity(BiotaPropertiesSpellBook entity)
        {
            using (var context = new ShardDbContext())
            {
                context.BiotaPropertiesSpellBook.Remove(entity);

                try
                {
                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    // Character name might be in use or some other fault
                    log.Error($"RemoveEntity failed with exception: {ex}");
                    return false;
                }
            }
        }

        public bool RemoveEntity(BiotaPropertiesString entity)
        {
            using (var context = new ShardDbContext())
            {
                context.BiotaPropertiesString.Remove(entity);

                try
                {
                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    // Character name might be in use or some other fault
                    log.Error($"RemoveEntity failed with exception: {ex}");
                    return false;
                }
            }
        }


        public PlayerBiotas GetPlayerBiotasInParallel(uint id)
        {
            var biota = GetBiota(id);

            var inventory = GetInventoryInParallel(id, true);

            var wieldedItems = GetWieldedItemsInParallel(id);

            return new PlayerBiotas(biota, inventory, wieldedItems);
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

                        if (includedNestedItems && biota.WeenieType == (int) WeenieType.Container)
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


        public List<Biota> GetObjectsByLandblockInParallel(ushort landblockId)
        {
            var decayables = new ConcurrentBag<Biota>();

            using (var context = new ShardDbContext())
            {
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                var results = context.BiotaPropertiesPosition
                    .Where(p => p.ObjCellId >> 16 == landblockId)
                    .ToList();

                Parallel.ForEach(results, result =>
                {
                    var biota = GetBiota(result.ObjectId);

                    if (biota != null && biota.WeenieType == (int)WeenieType.Corpse)
                        decayables.Add(biota);
                });
            }

            return decayables.ToList();
        }
    }
}
