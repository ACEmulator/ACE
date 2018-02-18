using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

using log4net;

using ACE.Database.Models.Shard;
using ACE.Entity;
using ACE.Entity.Enum;

namespace ACE.Database
{
    public class ShardDatabase : CommonDatabase
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

        public bool AddCharacter(Character character, Biota biota)
        {
            using (var context = new ShardDbContext())
            {
                if (!AddBiota(biota))
                    return false; // Biota save failed which mean Character fails.

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

        /// <summary>
        /// Will return a biota from the db with tracking enabled.
        /// This will populate all sub collections except the followign: BiotaPropertiesEmoteAction
        /// </summary>
        public Biota GetBiota(uint id)
        {
            using (var context = new ShardDbContext())
            {
                var result = context.Biota
                    .Include(r => r.BiotaPropertiesAnimPart)
                    .Include(r => r.BiotaPropertiesAttribute)
                    .Include(r => r.BiotaPropertiesAttribute2nd)
                    .Include(r => r.BiotaPropertiesBodyPart)
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
                    .Include(r => r.BiotaPropertiesSkill)
                    .Include(r => r.BiotaPropertiesSpellBook)
                    .Include(r => r.BiotaPropertiesString)
                    .Include(r => r.BiotaPropertiesTextureMap)
                    .FirstOrDefault(r => r.Id == id);

                return result;
            }
        }

















        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************

        private enum ShardPreparedStatement
        {
            // these are for the world database, but there's a lot of overlap
            GetContractTracker,
            GetSpellBarPositions,
            IsCharacterNameAvailable,
            DeleteContractTrackers,
            DeleteSpellBarPositions,
            InsertContractTracker,
            InsertSpellBarPositions,
            DeleteContractTracker,
        }
        
        public void AddFriend(uint characterId, uint friendCharacterId)
        {
            throw new NotImplementedException();
        }

        public void DeleteFriend(uint characterId, uint friendCharacterId)
        {
            throw new NotImplementedException();
        }

        public bool DeleteContract(AceContractTracker contract)
        {
            DatabaseTransaction transaction = BeginTransaction();
            DeleteAceContractTracker(transaction, contract);
            return transaction.Commit().Result;
        }

        public void RemoveAllFriends(uint characterId)
        {
            throw new NotImplementedException();
        }

        public bool DeleteOrRestore(ulong unixTime, uint aceObjectId)
        {
            AceCharacter aceCharacter = new AceCharacter(aceObjectId);
            LoadIntoObject(aceCharacter);
            aceCharacter.DeleteTime = unixTime;

            aceCharacter.Deleted = false;  // This is a reminder - the DB will set this 1 hour after deletion.

            base.SaveObject(aceCharacter);

            return true;
        }

        public bool DeleteCharacter(uint aceObjectId)
        {
            AceCharacter aceCharacter = new AceCharacter(aceObjectId);
            LoadIntoObject(aceCharacter);
            aceCharacter.Deleted = true;

            base.SaveObject(aceCharacter);

            return true;
        }

        protected override void LoadIntoObject(AceObject aceObject)
        {
            base.LoadIntoObject(aceObject);
            aceObject.TrackedContracts = GetAceContractList(aceObject.AceObjectId).ToDictionary(x => x.ContractId, x => x);
            aceObject.SpellsInSpellBars = GetAceObjectPropertiesSpellBarPositions(aceObject.AceObjectId);
        }
        
        private List<AceContractTracker> GetAceContractList(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<ShardPreparedStatement, AceContractTracker>(ShardPreparedStatement.GetContractTracker, criteria);
            return objects;
        }

        private List<AceObjectPropertiesSpellBarPositions> GetAceObjectPropertiesSpellBarPositions(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<ShardPreparedStatement, AceObjectPropertiesSpellBarPositions>(ShardPreparedStatement.GetSpellBarPositions, criteria);
            return objects;
        }

        public ObjectInfo GetObjectInfoByName(string name)
        {
            throw new NotImplementedException();
        }
        
        public uint RenameCharacter(string currentName, string newName)
        {
            throw new NotImplementedException();
        }
        
        public uint SetCharacterAccessLevelByName(string name, AccessLevel accessLevel)
        {
            throw new NotImplementedException();
        }

        protected override bool DeleteObjectDependencies(DatabaseTransaction transaction, AceObject aceObject)
        {
            DeleteAceContractTrackers(transaction, aceObject.AceObjectId);
            DeleteAceObjectPropertiesSpellBarPositions(transaction, aceObject.AceObjectId);
            return true;
        }
        
        protected override bool SaveObjectDependencies(DatabaseTransaction transaction, AceObject aceObject)
        {
            DeleteAceContractTrackers(transaction, aceObject.AceObjectId);
            SaveAceContractTracker(transaction, aceObject.AceObjectId, aceObject.TrackedContracts.Select(x => x.Value).ToList());

            DeleteAceObjectPropertiesSpellBarPositions(transaction, aceObject.AceObjectId);
            SaveAceObjectPropertiesSpellBarPositions(transaction, aceObject.SpellsInSpellBars);

            return true;
        }
        
        private bool SaveAceContractTracker(DatabaseTransaction transaction, uint aceObjectId, List<AceContractTracker> properties)
        {
            properties.ForEach(a => a.AceObjectId = aceObjectId);
            transaction.AddPreparedInsertListStatement<ShardPreparedStatement, AceContractTracker>(ShardPreparedStatement.InsertContractTracker, properties);
            return true;
        }

        private bool SaveAceObjectPropertiesSpellBarPositions(DatabaseTransaction transaction, List<AceObjectPropertiesSpellBarPositions> properties)
        {
            transaction.AddPreparedInsertListStatement(ShardPreparedStatement.InsertSpellBarPositions, properties);
            return true;
        }

        private bool DeleteAceContractTrackers(DatabaseTransaction transaction, uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            transaction.AddPreparedDeleteListStatement<ShardPreparedStatement, AceContractTracker>(ShardPreparedStatement.DeleteContractTrackers, criteria);
            return true;
        }

        private bool DeleteAceObjectPropertiesSpellBarPositions(DatabaseTransaction transaction, uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            transaction.AddPreparedDeleteListStatement<ShardPreparedStatement, AceObjectPropertiesSpellBarPositions>(ShardPreparedStatement.DeleteSpellBarPositions, criteria);
            return true;
        }

        private void DeleteAceContractTracker(DatabaseTransaction transaction, AceContractTracker contract)
        {
            transaction.AddPreparedDeleteStatement<ShardPreparedStatement, AceContractTracker>(ShardPreparedStatement.DeleteContractTracker, contract);
        }
    }
}
