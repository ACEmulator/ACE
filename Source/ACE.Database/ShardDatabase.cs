using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

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
                    .Where(r => r.Id >= min && r.Id <= max);

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


        public List<Character> GetCharacters(uint accountId)
        {
            using (var context = new ShardDbContext())
            {
                var results = context.Character
                    .AsNoTracking()
                    .Where(r => r.AccountId == accountId && !r.IsDeleted).ToList();

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

        public bool IsCharacterPlussed(uint biotaId)
        {
            var ret = false;

            using (var context = new ShardDbContext())
            {
                var result = context.Biota
                    .AsNoTracking()
                    .Include(r => r.BiotaPropertiesBool)
                    .FirstOrDefault(r => r.Id == biotaId);

                if (result.GetProperty(PropertyBool.IsAdmin) ?? false)
                    ret = true;
                if (result.GetProperty(PropertyBool.IsArch) ?? false)
                    ret = true;
                if (result.GetProperty(PropertyBool.IsPsr) ?? false)
                    ret = true;
                if (result.GetProperty(PropertyBool.IsSentinel) ?? false)
                    ret = true;

                if (result.WeenieType == (int)WeenieType.Admin || result.WeenieType == (int)WeenieType.Sentinel)
                    ret = true;

                return ret;
            }
        }

        public bool AddCharacter(Character character, Biota biota, IEnumerable<Biota> possessions)
        {
            using (var context = new ShardDbContext())
            {
                if (!AddBiota(context, biota))
                    return false; // Biota save failed which mean Character fails.

                if (!AddBiotas(context, possessions))
                    return false;

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
                    .FirstOrDefault(r => r.BiotaId == guid);

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
                    .FirstOrDefault(r => r.BiotaId == guid);

                if (result != null)
                {
                    result.IsDeleted = true;
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


        public bool AddBiota(Biota biota)
        {
            using (var context = new ShardDbContext())
                return AddBiota(context, biota);
        }

        private static bool AddBiota(ShardDbContext context, Biota biota)
        {
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

        public bool AddBiotas(IEnumerable<Biota> biotas)
        {
            using (var context = new ShardDbContext())
                return AddBiotas(context, biotas);
        }

        private static bool AddBiotas(ShardDbContext context, IEnumerable<Biota> biotas)
        {
            foreach (var biota in biotas)
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

        public Biota GetBiota(uint id)
        {
            using (var context = new ShardDbContext())
            {
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                return GetBiota(context, id);
            }
        }

        /// <summary>
        /// This method will populate all sub collections of the biota.
        /// It is slower, but should be used if you are not sure if the biota is a player or not.
        /// </summary>
        private static Biota GetBiota(ShardDbContext context, uint id)
        {
            return context.Biota
                .Include(r => r.BiotaPropertiesAnimPart)
                .Include(r => r.BiotaPropertiesAttribute)
                .Include(r => r.BiotaPropertiesAttribute2nd)
                .Include(r => r.BiotaPropertiesBodyPart)
                .Include(r => r.BiotaPropertiesBook)
                .Include(r => r.BiotaPropertiesBookPageData)
                .Include(r => r.BiotaPropertiesBool)
                .Include(r => r.BiotaPropertiesContract)
                .Include(r => r.BiotaPropertiesCreateList)
                .Include(r => r.BiotaPropertiesDID)
                .Include(r => r.BiotaPropertiesEmote).ThenInclude(emote => emote.BiotaPropertiesEmoteAction)
                .Include(r => r.BiotaPropertiesEmoteAction)
                .Include(r => r.BiotaPropertiesEventFilter)
                .Include(r => r.BiotaPropertiesFloat)
                .Include(r => r.BiotaPropertiesFriendListFriend)
                .Include(r => r.BiotaPropertiesFriendListObject)
                .Include(r => r.BiotaPropertiesGenerator)
                .Include(r => r.BiotaPropertiesIID)
                .Include(r => r.BiotaPropertiesInt)
                .Include(r => r.BiotaPropertiesInt64)
                .Include(r => r.BiotaPropertiesPalette)
                .Include(r => r.BiotaPropertiesPosition)
                .Include(r => r.BiotaPropertiesShortcutBarObject)
                .Include(r => r.BiotaPropertiesSkill)
                .Include(r => r.BiotaPropertiesSpellBar)
                .Include(r => r.BiotaPropertiesSpellBook)
                .Include(r => r.BiotaPropertiesString)
                .Include(r => r.BiotaPropertiesTextureMap)
                .FirstOrDefault(r => r.Id == id);
        }

        /// <summary>
        /// Use this method when you know if the biota is a player or not.
        /// Player biotas reference additional tables. Knowing if the biota is a player can significantly improve the query perofrmance.
        /// </summary>
        private static Biota GetBiota(ShardDbContext context, uint id, bool isPlayer)
        {
            if (isPlayer)
            {
                return context.Biota
                    //.Include(r => r.BiotaPropertiesAnimPart)
                    .Include(r => r.BiotaPropertiesAttribute)
                    .Include(r => r.BiotaPropertiesAttribute2nd)
                    .Include(r => r.BiotaPropertiesBodyPart)
                    //.Include(r => r.BiotaPropertiesBook)
                    //.Include(r => r.BiotaPropertiesBookPageData)
                    .Include(r => r.BiotaPropertiesBool)
                    .Include(r => r.BiotaPropertiesContract)
                    //.Include(r => r.BiotaPropertiesCreateList)
                    .Include(r => r.BiotaPropertiesDID)
                    //.Include(r => r.BiotaPropertiesEmote).ThenInclude(emote => emote.BiotaPropertiesEmoteAction)
                    //.Include(r => r.BiotaPropertiesEmoteAction)
                    //.Include(r => r.BiotaPropertiesEventFilter)
                    .Include(r => r.BiotaPropertiesFloat)
                    //.Include(r => r.BiotaPropertiesFriendListFriend)
                    .Include(r => r.BiotaPropertiesFriendListObject)
                    //.Include(r => r.BiotaPropertiesGenerator)
                    .Include(r => r.BiotaPropertiesIID)
                    .Include(r => r.BiotaPropertiesInt)
                    .Include(r => r.BiotaPropertiesInt64)
                    //.Include(r => r.BiotaPropertiesPalette)
                    .Include(r => r.BiotaPropertiesPosition)
                    .Include(r => r.BiotaPropertiesShortcutBarObject)
                    .Include(r => r.BiotaPropertiesSkill)
                    .Include(r => r.BiotaPropertiesSpellBar)
                    .Include(r => r.BiotaPropertiesSpellBook)
                    .Include(r => r.BiotaPropertiesString)
                    //.Include(r => r.BiotaPropertiesTextureMap)
                    .FirstOrDefault(r => r.Id == id);
            }

            // Base properties for every biota
            var biota = context.Biota
                .Include(r => r.BiotaPropertiesBool)
                .Include(r => r.BiotaPropertiesDID)
                .Include(r => r.BiotaPropertiesFloat)
                .Include(r => r.BiotaPropertiesIID)
                .Include(r => r.BiotaPropertiesInt)
                .Include(r => r.BiotaPropertiesInt64)
                .Include(r => r.BiotaPropertiesString)
                .Include(r => r.BiotaPropertiesSpellBook)
                .FirstOrDefault(r => r.Id == id);

            if (biota == null)
                return null;

            var weenieType = (WeenieType)biota.WeenieType;

            bool isCreature = weenieType == WeenieType.Creature || weenieType == WeenieType.Cow ||
                              weenieType == WeenieType.Sentinel || weenieType == WeenieType.Admin ||
                              weenieType == WeenieType.Vendor;

            biota.BiotaPropertiesAnimPart = context.BiotaPropertiesAnimPart.Where(r => r.ObjectId == biota.Id).ToList();

            if (isCreature)
            {
                biota.BiotaPropertiesAttribute = context.BiotaPropertiesAttribute.Where(r => r.ObjectId == biota.Id).ToList();
                biota.BiotaPropertiesAttribute2nd = context.BiotaPropertiesAttribute2nd.Where(r => r.ObjectId == biota.Id).ToList();

                biota.BiotaPropertiesBodyPart = context.BiotaPropertiesBodyPart.Where(r => r.ObjectId == biota.Id).ToList();
            }

            if (weenieType == WeenieType.Book)
            {
                biota.BiotaPropertiesBook = context.BiotaPropertiesBook.FirstOrDefault(r => r.ObjectId == biota.Id);
                biota.BiotaPropertiesBookPageData = context.BiotaPropertiesBookPageData.Where(r => r.ObjectId == biota.Id).ToList();
            }

            // Player only
            //biota.BiotaPropertiesContract = context.BiotaPropertiesContract.Where(r => r.ObjectId == biota.Id).ToList();

            biota.BiotaPropertiesCreateList = context.BiotaPropertiesCreateList.Where(r => r.ObjectId == biota.Id).ToList();
            biota.BiotaPropertiesEmote = context.BiotaPropertiesEmote.Where(r => r.ObjectId == biota.Id).ToList();
            biota.BiotaPropertiesEmoteAction = context.BiotaPropertiesEmoteAction.Where(r => r.ObjectId == biota.Id).ToList();
            biota.BiotaPropertiesEventFilter = context.BiotaPropertiesEventFilter.Where(r => r.ObjectId == biota.Id).ToList();

            // Player only
            //biota.BiotaPropertiesFriendListFriend = context.BiotaPropertiesFriendList.Where(r => r.FriendId == biota.Id).ToList();
            //biota.BiotaPropertiesFriendListObject = context.BiotaPropertiesFriendList.Where(r => r.ObjectId == biota.Id).ToList();

            biota.BiotaPropertiesGenerator = context.BiotaPropertiesGenerator.Where(r => r.ObjectId == biota.Id).ToList();
            biota.BiotaPropertiesPalette = context.BiotaPropertiesPalette.Where(r => r.ObjectId == biota.Id).ToList();
            biota.BiotaPropertiesPosition = context.BiotaPropertiesPosition.Where(r => r.ObjectId == biota.Id).ToList();

            // Player only
            //biota.BiotaPropertiesShortcutBarObject = context.BiotaPropertiesShortcutBar.Where(r => r.ObjectId == biota.Id).ToList();

            if (isCreature)
            {
                biota.BiotaPropertiesSkill = context.BiotaPropertiesSkill.Where(r => r.ObjectId == biota.Id).ToList();
            }

            // Player only
            //biota.BiotaPropertiesSpellBar = context.BiotaPropertiesSpellBar.Where(r => r.ObjectId == biota.Id).ToList();

            biota.BiotaPropertiesTextureMap = context.BiotaPropertiesTextureMap.Where(r => r.ObjectId == biota.Id).ToList();

            return biota;
        }

        public bool SaveBiota(Biota biota)
        {
            using (var context = new ShardDbContext())
            {
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

        public bool SaveBiotas(IEnumerable<Biota> biotas)
        {
            using (var context = new ShardDbContext())
            {
                foreach (var biota in biotas)
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


        public PlayerBiotas GetPlayerBiotas(uint id)
        {
            using (var context = new ShardDbContext())
            {
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                var biota = GetBiota(context, id, true);

                var inventory = GetInventory(context, id, true);

                var wieldedItems = GetWieldedItems(context, id);

                return new PlayerBiotas(biota, inventory, wieldedItems);
            }
        }

        public List<Biota> GetInventory(uint parentId, bool includedNestedItems)
        {
            List<Biota> inventory;

            using (var context = new ShardDbContext())
            {
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                inventory = GetInventory(context, parentId, includedNestedItems);
            }

            return inventory;
        }

        private static List<Biota> GetInventory(ShardDbContext context, uint parentId, bool includedNestedItems)
        {
            var inventory = new List<Biota>();

            var parentIdAsInt = unchecked((int)parentId);

            var results = context.BiotaPropertiesIID
                .Where(r => r.Type == (ushort)PropertyInstanceId.Container && r.Value == parentIdAsInt);

            foreach (var result in results)
            {
                var biota = GetBiota(context, result.ObjectId, false);

                if (biota != null)
                {
                    inventory.Add(biota);

                    if (includedNestedItems && biota.WeenieType == (int)WeenieType.Container)
                    {
                        var subItems = GetInventory(context, biota.Id, false);

                        inventory.AddRange(subItems);
                    }
                }
            }

            return inventory;
        }

        public List<Biota> GetWieldedItems(uint parentId)
        {
            List<Biota> inventory;

            using (var context = new ShardDbContext())
            {
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                inventory = GetWieldedItems(context, parentId);
            }

            return inventory;
        }

        private static List<Biota> GetWieldedItems(ShardDbContext context, uint parentId)
        {
            var items = new List<Biota>();

            var results = context.BiotaPropertiesIID
                    .Where(r => r.Type == (ushort)PropertyInstanceId.Wielder && r.Value == parentId);

            foreach (var result in results)
            {
                var biota = GetBiota(context, result.ObjectId, false);

                if (biota != null)
                    items.Add(biota);
            }

            return items;
        }
    }
}
