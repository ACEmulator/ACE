using System;
using System.Collections.Generic;
using System.Linq;

using MySql.Data.MySqlClient;

using log4net;

using ACE.Entity;
using ACE.Entity.Enum;

namespace ACE.Database
{
    public class ShardDatabase : CommonDatabase, IShardDatabase
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private enum ShardPreparedStatement
        {
            // these are for the world database, but there's a lot of overlap
            GetObjectsByLandblock,

            GetContractTracker,
            GetSpellBarPositions,

            GetCharacters,
            IsCharacterNameAvailable,
            
            DeleteContractTrackers,
            DeleteSpellBarPositions,

            InsertContractTracker,
            InsertSpellBarPositions,

            UpdateContractTracker,
            
            DeleteContractTracker,

            GetCurrentId,
        }
        
        private void ConstructMaxQueryStatement(ShardPreparedStatement id, string tableName, string columnName)
        {
            // NOTE: when moved to WordDatabase, ace_shard needs to be changed to ace_world
            AddPreparedStatement<ShardPreparedStatement>(id, $"SELECT MAX(`{columnName}`) FROM `{tableName}` WHERE `{columnName}` >= ? && `{columnName}` < ?",
                MySqlDbType.UInt32, MySqlDbType.UInt32);
        }

        protected override void InitializePreparedStatements()
        {
            base.InitializePreparedStatements();

            ConstructStatement(ShardPreparedStatement.IsCharacterNameAvailable, typeof(CachedCharacter), ConstructedStatementType.Get);
            
            ConstructStatement(ShardPreparedStatement.GetCharacters, typeof(CachedCharacter), ConstructedStatementType.GetList);
            
            ConstructStatement(ShardPreparedStatement.GetContractTracker, typeof(AceContractTracker), ConstructedStatementType.GetList);
            ConstructStatement(ShardPreparedStatement.GetSpellBarPositions, typeof(AceObjectPropertiesSpellBarPositions), ConstructedStatementType.GetList);

            // Delete statements
            ConstructStatement(ShardPreparedStatement.DeleteContractTrackers, typeof(AceContractTracker), ConstructedStatementType.DeleteList);
            ConstructStatement(ShardPreparedStatement.DeleteSpellBarPositions, typeof(AceObjectPropertiesSpellBarPositions), ConstructedStatementType.DeleteList);

            // Insert statements
            ConstructStatement(ShardPreparedStatement.InsertContractTracker, typeof(AceContractTracker), ConstructedStatementType.InsertList);
            ConstructStatement(ShardPreparedStatement.InsertSpellBarPositions, typeof(AceObjectPropertiesSpellBarPositions), ConstructedStatementType.InsertList);

            // Updates
            ConstructStatement(ShardPreparedStatement.UpdateContractTracker, typeof(AceContractTracker), ConstructedStatementType.Update);

            // deletes for properties
            ConstructStatement(ShardPreparedStatement.DeleteContractTracker, typeof(AceContractTracker), ConstructedStatementType.Delete);

            // FIXME(ddevec): Use max/min values defined in factory -- this is just for demonstration purposes
            ConstructMaxQueryStatement(ShardPreparedStatement.GetCurrentId, "ace_object", "aceObjectId");
        }

        private uint GetMaxGuid(ShardPreparedStatement id, uint min, uint max)
        {
            object[] critera = new object[] { min, max };
            var res = SelectPreparedStatement<ShardPreparedStatement>(id, critera);
            var ret = res.Rows[0][0];
            if (ret is DBNull)
            {
                return uint.MaxValue;
            }

            return (uint)res.Rows[0][0];
        }

        public uint GetCurrentId(uint min, uint max)
        {
            return GetMaxGuid(ShardPreparedStatement.GetCurrentId, min, max);
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

        public List<CachedCharacter> GetCharacters(uint accountId)
        {
            var criteria = new Dictionary<string, object> { { "accountId", accountId }, { "deleted", 0 } };
            var objects = ExecuteConstructedGetListStatement<ShardPreparedStatement, CachedCharacter>(ShardPreparedStatement.GetCharacters, criteria);

            return objects;
        }
        
        public AceCharacter GetCharacter(uint id)
        {
            AceCharacter character = new AceCharacter(id);

            // load common stuff here
            LoadIntoObject(character);

            // fetch common stuff here (is there any?)

            return character;
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
        
        public override List<AceObject> GetObjectsByLandblock(ushort landblock)
        {
            var criteria = new Dictionary<string, object> { { "landblock", landblock } };
            var objects = ExecuteConstructedGetListStatement<ShardPreparedStatement, CachedWorldObject>(ShardPreparedStatement.GetObjectsByLandblock, criteria);
            List<AceObject> ret = new List<AceObject>();
            objects.ForEach(cwo =>
            {
                var o = base.GetObject(cwo.AceObjectId);
                ret.Add(o);
            });
            return ret;
        }

        public bool IsCharacterNameAvailable(string name)
        {
            var cc = new CachedCharacter();
            var criteria = new Dictionary<string, object> { { "name", name } };
            return !(ExecuteConstructedGetStatement<CachedCharacter, ShardPreparedStatement>(ShardPreparedStatement.IsCharacterNameAvailable, criteria, cc));
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
