using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

using log4net;

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
                var results = context.Biota.AsNoTracking().Where(r => r.Id >= min && r.Id <= max);

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

        public bool AddCharacter(Character character, Biota biota, IEnumerable<Biota> inventory)
        {
            if (!AddBiota(biota))
                return false; // Biota save failed which mean Character fails.

            if (!AddBiotas(inventory))
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
            {
                context.Biota.Add(biota);

                try
                {
                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    // Character name might be in use or some other fault
                    log.Error($"AddBiota failed with exception: {ex}");
                    return false;
                }
            }
        }

        public bool AddBiotas(IEnumerable<Biota> biotas)
        {
            using (var context = new ShardDbContext())
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
                    // Character name might be in use or some other fault
                    log.Error($"AddBiota failed with exception: {ex}");
                    return false;
                }
            }
        }


        /// <summary>
        /// Will return a biota from the db with tracking enabled.
        /// This will populate all sub collections except the followign: BiotaPropertiesEmoteAction
        /// </summary>
        public Biota GetBiota(uint id)
        {
            using (var context = new ShardDbContext())
                return GetBiota(context, id);
        }

        private static Biota GetBiota(ShardDbContext context, uint id)
        {
            return context.Biota
                // Should we add .AsNoTracking() here since we're disposing of the context anyway?
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

        /// <summary>
        /// Until we can automatically detected removed rows from a biota in SaveBiota, we must manually request their removal.
        /// </summary>
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


        public List<Biota> GetInventory(uint parentId, bool includedNestedItems)
        {
            List<Biota> inventory;

            using (var context = new ShardDbContext())
            {
                inventory = GetInventory(context, parentId);

                if (includedNestedItems)
                {
                    foreach (var item in inventory)
                    {
                        if (item.WeenieType == (int)WeenieType.Container)
                        {
                            var subItems = GetInventory(context, item.Id);

                            inventory.AddRange(subItems);
                        }
                    }
                }
            }

            return inventory;
        }

        private static List<Biota> GetInventory(ShardDbContext context, uint parentId)
        {
            var inventory = new List<Biota>();

            var results = context.BiotaPropertiesIID
                .AsNoTracking()
                .Where(r => r.Type == (ushort)PropertyInstanceId.Container && r.Value == parentId);

            foreach (var result in results)
            {
                var biota = GetBiota(context, result.ObjectId);

                if (biota != null)
                    inventory.Add(biota);
            }

            return inventory;
        }
    }
}
