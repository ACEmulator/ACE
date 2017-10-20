using System;
using System.Collections.Generic;
using System.Linq;
using ACE.Entity;
using ACE.Entity.Enum;
using MySql.Data.MySqlClient;
using log4net;
// ReSharper disable InconsistentNaming

namespace ACE.Database
{
    public class ShardDatabase : Database, IShardDatabase
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private enum ShardPreparedStatement
        {
            // these are for the world database, but there's a lot of overlap
            GetObjectsByLandblock,
            GetPaletteOverridesByObject,
            GetAnimationOverridesByObject,
            GetTextureOverridesByObject,
            GetAceObjectPropertiesInt,
            GetAceObjectPropertiesBigInt,
            GetAceObjectPropertiesDouble,
            GetAceObjectPropertiesBool,
            GetAceObjectPropertiesString,
            GetAceObjectPropertiesDid,
            GetAceObjectPropertiesIid,
            GetAceObjectPropertiesPositions,
            GetAceObjectPropertiesAttributes,
            GetAceObjectPropertiesAttributes2nd,
            GetAceObjectPropertiesSkills,
            GetAceObjectPropertiesSpell,
            GetAceObjectPropertiesSpellBarPositions,
            GetAceObjectPropertiesBook,
            GetAceContractTracker,

            SaveAceObject,
            DeleteAceObject,
            GetAceObject,
            UpdateAceObject,
            GetAceObjectsByContainerId,
            GetAceObjectsByWielderId,

            GetCharacters,
            IsCharacterNameAvailable,

            DeletePaletteOverridesByObject,
            DeleteAnimationOverridesByObject,
            DeleteTextureOverridesByObject,
            DeleteAceObjectPropertiesInt,
            DeleteAceObjectPropertiesBigInt,
            DeleteAceObjectPropertiesDouble,
            DeleteAceObjectPropertiesBool,
            DeleteAceObjectPropertiesString,
            DeleteAceObjectPropertiesDid,
            DeleteAceObjectPropertiesIid,
            DeleteAceObjectPropertiesPositions,
            DeleteAceObjectPropertiesAttributes,
            DeleteAceObjectPropertiesAttributes2nd,
            DeleteAceObjectPropertiesSkills,
            DeleteAceObjectPropertiesSpell,
            DeleteAceObjectPropertiesSpellBarPositions,
            DeleteAceObjectPropertiesBook,
            DeleteAceContractTracker,

            InsertPaletteOverridesByObject,
            InsertAnimationOverridesByObject,
            InsertTextureOverridesByObject,
            InsertAceObjectPropertiesInt,
            InsertAceObjectPropertiesBigInt,
            InsertAceObjectPropertiesDouble,
            InsertAceObjectPropertiesBool,
            InsertAceObjectPropertiesString,
            InsertAceObjectPropertiesDid,
            InsertAceObjectPropertiesIid,
            InsertAceObjectPropertiesPositions,
            InsertAceObjectPropertiesAttributes,
            InsertAceObjectPropertiesAttributes2nd,
            InsertAceObjectPropertiesSkills,
            InsertAceObjectPropertiesSpells,
            InsertAceObjectPropertiesSpellBarPositions,
            InsertAceObjectPropertiesBook,
            InsertAceContractTracker,

            // note, this section is all "Property" singular
            UpdateAceObjectPropertyInt,
            UpdateAceObjectPropertyBigInt,
            UpdateAceObjectPropertyDouble,
            UpdateAceObjectPropertyBool,
            UpdateAceObjectPropertyString,
            UpdateAceObjectPropertyDid,
            UpdateAceObjectPropertyIid,
            UpdateAceObjectPropertyPosition,
            UpdateAceObjectPropertyAttribute,
            UpdateAceObjectPropertyAttribute2nd,
            UpdateAceObjectPropertySkill,
            UpdateAceContractTracker,

            DeleteAceObjectPropertyInt,
            DeleteAceObjectPropertyBigInt,
            DeleteAceObjectPropertyDouble,
            DeleteAceObjectPropertyBool,
            DeleteAceObjectPropertyString,
            DeleteAceObjectPropertyDid,
            DeleteAceObjectPropertyIid,
            DeleteAceObjectPropertyPosition,
            DeleteAceObjectPropertyAttribute,
            DeleteAceObjectPropertyAttribute2nd,
            DeleteAceObjectPropertySkill,
            // Used to get max values from DB
            GetCurrentId,
        }

        protected override Type PreparedStatementType
        {
            get
            {
                return typeof(ShardPreparedStatement);
            }
        }

        private void ConstructMaxQueryStatement(ShardPreparedStatement id, string tableName, string columnName)
        {
            // NOTE: when moved to WordDatabase, ace_shard needs to be changed to ace_world
            AddPreparedStatement<ShardPreparedStatement>(id, $"SELECT MAX(`{columnName}`) FROM `{tableName}` WHERE `{columnName}` >= ? && `{columnName}` < ?",
                MySqlDbType.UInt32, MySqlDbType.UInt32);
        }

        protected override void InitializePreparedStatements()
        {
            ConstructStatement(ShardPreparedStatement.IsCharacterNameAvailable, typeof(CachedCharacter), ConstructedStatementType.Get);

            ConstructStatement(ShardPreparedStatement.DeleteAceObject, typeof(AceObject), ConstructedStatementType.Delete);
            ConstructStatement(ShardPreparedStatement.SaveAceObject, typeof(AceObject), ConstructedStatementType.Insert);
            ConstructStatement(ShardPreparedStatement.GetAceObject, typeof(AceObject), ConstructedStatementType.Get);
            ConstructStatement(ShardPreparedStatement.UpdateAceObject, typeof(AceObject), ConstructedStatementType.Update);

            ConstructStatement(ShardPreparedStatement.GetCharacters, typeof(CachedCharacter), ConstructedStatementType.GetList);

            ConstructStatement(ShardPreparedStatement.GetAceObjectPropertiesBigInt, typeof(AceObjectPropertiesInt64), ConstructedStatementType.GetList);
            ConstructStatement(ShardPreparedStatement.GetAceObjectPropertiesInt, typeof(AceObjectPropertiesInt), ConstructedStatementType.GetList);
            ConstructStatement(ShardPreparedStatement.GetAceObjectPropertiesBool, typeof(AceObjectPropertiesBool), ConstructedStatementType.GetList);
            ConstructStatement(ShardPreparedStatement.GetAceObjectPropertiesDouble, typeof(AceObjectPropertiesDouble), ConstructedStatementType.GetList);
            ConstructStatement(ShardPreparedStatement.GetAceObjectPropertiesString, typeof(AceObjectPropertiesString), ConstructedStatementType.GetList);
            ConstructStatement(ShardPreparedStatement.GetAceObjectPropertiesIid, typeof(AceObjectPropertiesInstanceId), ConstructedStatementType.GetList);
            ConstructStatement(ShardPreparedStatement.GetAceObjectPropertiesDid, typeof(AceObjectPropertiesDataId), ConstructedStatementType.GetList);

            ConstructStatement(ShardPreparedStatement.GetAceObjectsByContainerId, typeof(CachedInventoryObject), ConstructedStatementType.GetList);
            ConstructStatement(ShardPreparedStatement.GetAceObjectsByWielderId, typeof(CachedWieldedObject), ConstructedStatementType.GetList);

            ConstructStatement(ShardPreparedStatement.GetTextureOverridesByObject, typeof(TextureMapOverride), ConstructedStatementType.GetList);
            ConstructStatement(ShardPreparedStatement.GetPaletteOverridesByObject, typeof(PaletteOverride), ConstructedStatementType.GetList);
            ConstructStatement(ShardPreparedStatement.GetAnimationOverridesByObject, typeof(AnimationOverride), ConstructedStatementType.GetList);

            ConstructStatement(ShardPreparedStatement.GetAceObjectPropertiesPositions, typeof(AceObjectPropertiesPosition), ConstructedStatementType.GetList);

            ConstructStatement(ShardPreparedStatement.GetAceObjectPropertiesAttributes, typeof(AceObjectPropertiesAttribute), ConstructedStatementType.GetList);
            ConstructStatement(ShardPreparedStatement.GetAceObjectPropertiesAttributes2nd, typeof(AceObjectPropertiesAttribute2nd), ConstructedStatementType.GetList);
            ConstructStatement(ShardPreparedStatement.GetAceObjectPropertiesSkills, typeof(AceObjectPropertiesSkill), ConstructedStatementType.GetList);
            ConstructStatement(ShardPreparedStatement.GetAceObjectPropertiesSpell, typeof(AceObjectPropertiesSpell), ConstructedStatementType.GetList);
            ConstructStatement(ShardPreparedStatement.GetAceObjectPropertiesSpellBarPositions, typeof(AceObjectPropertiesSpellBarPositions), ConstructedStatementType.GetList);
            ConstructStatement(ShardPreparedStatement.GetAceObjectPropertiesBook, typeof(AceObjectPropertiesBook), ConstructedStatementType.GetList);
            ConstructStatement(ShardPreparedStatement.GetAceContractTracker, typeof(AceContractTracker), ConstructedStatementType.GetList);

            // Delete statements
            ConstructStatement(ShardPreparedStatement.DeleteAceObjectPropertiesInt, typeof(AceObjectPropertiesInt), ConstructedStatementType.DeleteList);
            ConstructStatement(ShardPreparedStatement.DeleteAceObjectPropertiesBigInt, typeof(AceObjectPropertiesInt64), ConstructedStatementType.DeleteList);
            ConstructStatement(ShardPreparedStatement.DeleteAceObjectPropertiesBool, typeof(AceObjectPropertiesBool), ConstructedStatementType.DeleteList);
            ConstructStatement(ShardPreparedStatement.DeleteAceObjectPropertiesDouble, typeof(AceObjectPropertiesDouble), ConstructedStatementType.DeleteList);
            ConstructStatement(ShardPreparedStatement.DeleteAceObjectPropertiesString, typeof(AceObjectPropertiesString), ConstructedStatementType.DeleteList);
            ConstructStatement(ShardPreparedStatement.DeleteAceObjectPropertiesIid, typeof(AceObjectPropertiesInstanceId), ConstructedStatementType.DeleteList);
            ConstructStatement(ShardPreparedStatement.DeleteAceObjectPropertiesDid, typeof(AceObjectPropertiesDataId), ConstructedStatementType.DeleteList);
            ConstructStatement(ShardPreparedStatement.DeleteAceObjectPropertiesPositions, typeof(AceObjectPropertiesPosition), ConstructedStatementType.DeleteList);
            ConstructStatement(ShardPreparedStatement.DeleteAceObjectPropertiesSkills, typeof(AceObjectPropertiesSkill), ConstructedStatementType.DeleteList);
            ConstructStatement(ShardPreparedStatement.DeleteAceObjectPropertiesAttributes, typeof(AceObjectPropertiesAttribute), ConstructedStatementType.DeleteList);
            ConstructStatement(ShardPreparedStatement.DeleteAceObjectPropertiesAttributes2nd, typeof(AceObjectPropertiesAttribute2nd), ConstructedStatementType.DeleteList);
            ConstructStatement(ShardPreparedStatement.DeleteTextureOverridesByObject, typeof(TextureMapOverride), ConstructedStatementType.DeleteList);
            ConstructStatement(ShardPreparedStatement.DeletePaletteOverridesByObject, typeof(PaletteOverride), ConstructedStatementType.DeleteList);
            ConstructStatement(ShardPreparedStatement.DeleteAnimationOverridesByObject, typeof(AnimationOverride), ConstructedStatementType.DeleteList);
            ConstructStatement(ShardPreparedStatement.DeleteAceObjectPropertiesSpell, typeof(AceObjectPropertiesSpell), ConstructedStatementType.DeleteList);
            ConstructStatement(ShardPreparedStatement.DeleteAceObjectPropertiesSpellBarPositions, typeof(AceObjectPropertiesSpellBarPositions), ConstructedStatementType.DeleteList);
            ConstructStatement(ShardPreparedStatement.DeleteAceObjectPropertiesBook, typeof(AceObjectPropertiesBook), ConstructedStatementType.DeleteList);
            ConstructStatement(ShardPreparedStatement.DeleteAceContractTracker, typeof(AceContractTracker), ConstructedStatementType.DeleteList);

            // Insert statements
            ConstructStatement(ShardPreparedStatement.InsertAceObjectPropertiesInt, typeof(AceObjectPropertiesInt), ConstructedStatementType.InsertList);
            ConstructStatement(ShardPreparedStatement.InsertAceObjectPropertiesBigInt, typeof(AceObjectPropertiesInt64), ConstructedStatementType.InsertList);
            ConstructStatement(ShardPreparedStatement.InsertAceObjectPropertiesBool, typeof(AceObjectPropertiesBool), ConstructedStatementType.InsertList);
            ConstructStatement(ShardPreparedStatement.InsertAceObjectPropertiesDouble, typeof(AceObjectPropertiesDouble), ConstructedStatementType.InsertList);
            ConstructStatement(ShardPreparedStatement.InsertAceObjectPropertiesString, typeof(AceObjectPropertiesString), ConstructedStatementType.InsertList);
            ConstructStatement(ShardPreparedStatement.InsertAceObjectPropertiesIid, typeof(AceObjectPropertiesInstanceId), ConstructedStatementType.InsertList);
            ConstructStatement(ShardPreparedStatement.InsertAceObjectPropertiesDid, typeof(AceObjectPropertiesDataId), ConstructedStatementType.InsertList);
            ConstructStatement(ShardPreparedStatement.InsertAceObjectPropertiesPositions, typeof(AceObjectPropertiesPosition), ConstructedStatementType.InsertList);
            ConstructStatement(ShardPreparedStatement.InsertAceObjectPropertiesSkills, typeof(AceObjectPropertiesSkill), ConstructedStatementType.InsertList);
            ConstructStatement(ShardPreparedStatement.InsertAceObjectPropertiesAttributes, typeof(AceObjectPropertiesAttribute), ConstructedStatementType.InsertList);
            ConstructStatement(ShardPreparedStatement.InsertAceObjectPropertiesAttributes2nd, typeof(AceObjectPropertiesAttribute2nd), ConstructedStatementType.InsertList);
            ConstructStatement(ShardPreparedStatement.InsertTextureOverridesByObject, typeof(TextureMapOverride), ConstructedStatementType.InsertList);
            ConstructStatement(ShardPreparedStatement.InsertPaletteOverridesByObject, typeof(PaletteOverride), ConstructedStatementType.InsertList);
            ConstructStatement(ShardPreparedStatement.InsertAnimationOverridesByObject, typeof(AnimationOverride), ConstructedStatementType.InsertList);
            ConstructStatement(ShardPreparedStatement.InsertAceObjectPropertiesSpells, typeof(AceObjectPropertiesSpell), ConstructedStatementType.InsertList);
            ConstructStatement(ShardPreparedStatement.InsertAceObjectPropertiesSpellBarPositions, typeof(AceObjectPropertiesSpellBarPositions), ConstructedStatementType.InsertList);
            ConstructStatement(ShardPreparedStatement.InsertAceObjectPropertiesBook, typeof(AceObjectPropertiesBook), ConstructedStatementType.InsertList);
            ConstructStatement(ShardPreparedStatement.InsertAceContractTracker, typeof(AceContractTracker), ConstructedStatementType.InsertList);

            // Updates
            ConstructStatement(ShardPreparedStatement.UpdateAceObjectPropertyInt, typeof(AceObjectPropertiesInt), ConstructedStatementType.Update);
            ConstructStatement(ShardPreparedStatement.UpdateAceObjectPropertyBigInt, typeof(AceObjectPropertiesInt64), ConstructedStatementType.Update);
            ConstructStatement(ShardPreparedStatement.UpdateAceObjectPropertyBool, typeof(AceObjectPropertiesBool), ConstructedStatementType.Update);
            ConstructStatement(ShardPreparedStatement.UpdateAceObjectPropertyDouble, typeof(AceObjectPropertiesDouble), ConstructedStatementType.Update);
            ConstructStatement(ShardPreparedStatement.UpdateAceObjectPropertyString, typeof(AceObjectPropertiesString), ConstructedStatementType.Update);
            ConstructStatement(ShardPreparedStatement.UpdateAceObjectPropertyIid, typeof(AceObjectPropertiesInstanceId), ConstructedStatementType.Update);
            ConstructStatement(ShardPreparedStatement.UpdateAceObjectPropertyDid, typeof(AceObjectPropertiesDataId), ConstructedStatementType.Update);
            ConstructStatement(ShardPreparedStatement.UpdateAceObjectPropertyPosition, typeof(AceObjectPropertiesPosition), ConstructedStatementType.Update);
            ConstructStatement(ShardPreparedStatement.UpdateAceObjectPropertySkill, typeof(AceObjectPropertiesSkill), ConstructedStatementType.Update);
            ConstructStatement(ShardPreparedStatement.UpdateAceObjectPropertyAttribute, typeof(AceObjectPropertiesAttribute), ConstructedStatementType.Update);
            ConstructStatement(ShardPreparedStatement.UpdateAceObjectPropertyAttribute2nd, typeof(AceObjectPropertiesAttribute2nd), ConstructedStatementType.Update);
            ConstructStatement(ShardPreparedStatement.UpdateAceContractTracker, typeof(AceContractTracker), ConstructedStatementType.Update);

            // deletes for properties
            ConstructStatement(ShardPreparedStatement.DeleteAceObjectPropertyInt, typeof(AceObjectPropertiesInt), ConstructedStatementType.Delete);
            ConstructStatement(ShardPreparedStatement.DeleteAceObjectPropertyBigInt, typeof(AceObjectPropertiesInt64), ConstructedStatementType.Delete);
            ConstructStatement(ShardPreparedStatement.DeleteAceObjectPropertyBool, typeof(AceObjectPropertiesBool), ConstructedStatementType.Delete);
            ConstructStatement(ShardPreparedStatement.DeleteAceObjectPropertyDouble, typeof(AceObjectPropertiesDouble), ConstructedStatementType.Delete);
            ConstructStatement(ShardPreparedStatement.DeleteAceObjectPropertyString, typeof(AceObjectPropertiesString), ConstructedStatementType.Delete);
            ConstructStatement(ShardPreparedStatement.DeleteAceObjectPropertyIid, typeof(AceObjectPropertiesInstanceId), ConstructedStatementType.Delete);
            ConstructStatement(ShardPreparedStatement.DeleteAceObjectPropertyDid, typeof(AceObjectPropertiesDataId), ConstructedStatementType.Delete);
            ConstructStatement(ShardPreparedStatement.DeleteAceObjectPropertyPosition, typeof(AceObjectPropertiesPosition), ConstructedStatementType.Delete);
            ConstructStatement(ShardPreparedStatement.DeleteAceObjectPropertySkill, typeof(AceObjectPropertiesSkill), ConstructedStatementType.Delete);
            ConstructStatement(ShardPreparedStatement.DeleteAceObjectPropertyAttribute, typeof(AceObjectPropertiesAttribute), ConstructedStatementType.Delete);
            ConstructStatement(ShardPreparedStatement.DeleteAceObjectPropertyAttribute2nd, typeof(AceObjectPropertiesAttribute2nd), ConstructedStatementType.Delete);            

            // FIXME(ddevec): Use max/min values defined in factory -- this is just for demonstration purposes
            ConstructMaxQueryStatement(ShardPreparedStatement.GetCurrentId, "ace_object", "aceObjectId");
        }

        private uint GetMaxGuid(ShardPreparedStatement id, uint min, uint max)
        {
            object[] critera = new object[] { min, max };
            MySqlResult res = SelectPreparedStatement<ShardPreparedStatement>(id, critera);
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

            SaveObject(aceCharacter);

            return true;
        }

        public bool DeleteCharacter(uint aceObjectId)
        {
            AceCharacter aceCharacter = new AceCharacter(aceObjectId);
            LoadIntoObject(aceCharacter);
            // aceCharacter.IsDeleted = true;
            aceCharacter.Deleted = true;

            SaveObject(aceCharacter);

            return true;
        }

        public List<CachedCharacter> GetCharacters(uint accountId)
        {
            var criteria = new Dictionary<string, object> { { "accountId", accountId }, { "deleted", 0 } };
            var objects = ExecuteConstructedGetListStatement<ShardPreparedStatement, CachedCharacter>(ShardPreparedStatement.GetCharacters, criteria);

            return objects;
        }

        public Dictionary<ObjectGuid, AceObject> GetInventoryByContainerId(uint containerId)
        {
            var criteria = new Dictionary<string, object> { { "containerId", containerId } };
            var objects = ExecuteConstructedGetListStatement<ShardPreparedStatement, CachedInventoryObject>(ShardPreparedStatement.GetAceObjectsByContainerId, criteria);
            return objects.ToDictionary(x => new ObjectGuid(x.AceObjectId), x => GetObject(x.AceObjectId));
        }

        public Dictionary<ObjectGuid, AceObject> GetItemsByWielderId(uint wielderId)
        {
            var criteria = new Dictionary<string, object> { { "wielderId", wielderId } };
            var objects = ExecuteConstructedGetListStatement<ShardPreparedStatement, CachedWieldedObject>(ShardPreparedStatement.GetAceObjectsByWielderId, criteria);
            return objects.ToDictionary(x => new ObjectGuid(x.AceObjectId), x => GetObject(x.AceObjectId));
        }

        public AceCharacter GetCharacter(uint id)
        {
            AceCharacter character = new AceCharacter(id);

            // load common stuff here
            LoadIntoObject(character);

            // fetch common stuff here (is there any?)

            return character;
        }

        public AceObject GetObject(uint aceObjectId)
        {
            AceObject aceObject = GetAceObject(aceObjectId);
            LoadIntoObject(aceObject);
            return aceObject;
        }

        private void LoadIntoObject(AceObject aceObject)
        {
            // this flag determines when the subsequentnt calls to "save" trigger an insert or an update
            aceObject.HasEverBeenSavedToDatabase = true;

            // TODO: still to implement - load spells, friends, allegiance info, spell comps
            aceObject.IntProperties = GetAceObjectPropertiesInt(aceObject.AceObjectId);
            aceObject.Int64Properties = GetAceObjectPropertiesBigInt(aceObject.AceObjectId);
            aceObject.BoolProperties = GetAceObjectPropertiesBool(aceObject.AceObjectId);
            aceObject.DoubleProperties = GetAceObjectPropertiesDouble(aceObject.AceObjectId);
            aceObject.StringProperties = GetAceObjectPropertiesString(aceObject.AceObjectId);
            aceObject.InstanceIdProperties = GetAceObjectPropertiesIid(aceObject.AceObjectId);
            aceObject.DataIdProperties = GetAceObjectPropertiesDid(aceObject.AceObjectId);
            aceObject.TextureOverrides = GetAceObjectTextureMaps(aceObject.AceObjectId);
            aceObject.AnimationOverrides = GetAceObjectAnimations(aceObject.AceObjectId);
            aceObject.PaletteOverrides = GetAceObjectPalettes(aceObject.AceObjectId);
            aceObject.AceObjectPropertiesPositions = GetAceObjectPostions(aceObject.AceObjectId).ToDictionary(x => (PositionType)x.DbPositionType,
                x => new Position(x));
            aceObject.AceObjectPropertiesAttributes = GetAceObjectPropertiesAttribute(aceObject.AceObjectId).ToDictionary(x => (Ability)x.AttributeId,
                x => new CreatureAbility(x));
            aceObject.AceObjectPropertiesAttributes2nd = GetAceObjectPropertiesAttribute2nd(aceObject.AceObjectId).ToDictionary(x => (Ability)x.Attribute2ndId,
                x => new CreatureVital(aceObject, x));
            aceObject.AceObjectPropertiesSkills = GetAceObjectPropertiesSkill(aceObject.AceObjectId).ToDictionary(x => (Skill)x.SkillId,
                x => new CreatureSkill(aceObject, x));
            aceObject.SpellIdProperties = GetAceObjectPropertiesSpell(aceObject.AceObjectId);
            aceObject.SpellsInSpellBars = GetAceObjectPropertiesSpellBarPositions(aceObject.AceObjectId);
            aceObject.Inventory = GetInventoryByContainerId(aceObject.AceObjectId);
            // Ok now, check to see if we loaded any containers that themselves may have items ... Og II
            foreach (var invItem in aceObject.Inventory)
            {
                if (invItem.Value.WeenieType == (uint)WeenieType.Container)
                    invItem.Value.Inventory = GetInventoryByContainerId(invItem.Key.Full);
            }
            aceObject.BookProperties = GetAceObjectPropertiesBook(aceObject.AceObjectId).ToDictionary(x => x.Page);
            aceObject.WieldedItems = GetItemsByWielderId(aceObject.AceObjectId);
            aceObject.TrackedContracts = GetAceContractList(aceObject.AceObjectId).ToDictionary(x => x.ContractId, x => x);
        }

        private List<AceObjectPropertiesPosition> GetAceObjectPostions(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<ShardPreparedStatement, AceObjectPropertiesPosition>(ShardPreparedStatement.GetAceObjectPropertiesPositions, criteria);
            objects.ForEach(o =>
            {
                o.HasEverBeenSavedToDatabase = true;
                o.IsDirty = false;
            });
            return objects;
        }

        private List<AceObjectPropertiesSkill> GetAceObjectPropertiesSkill(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<ShardPreparedStatement, AceObjectPropertiesSkill>(ShardPreparedStatement.GetAceObjectPropertiesSkills, criteria);
            objects.ForEach(o =>
            {
                o.HasEverBeenSavedToDatabase = true;
                o.IsDirty = false;
            });
            return objects;
        }

        private List<AceObjectPropertiesAttribute> GetAceObjectPropertiesAttribute(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<ShardPreparedStatement, AceObjectPropertiesAttribute>(ShardPreparedStatement.GetAceObjectPropertiesAttributes, criteria);
            objects.ForEach(o =>
            {
                o.HasEverBeenSavedToDatabase = true;
                o.IsDirty = false;
            });
            return objects;
        }

        // ReSharper disable once InconsistentNaming
        private List<AceObjectPropertiesAttribute2nd> GetAceObjectPropertiesAttribute2nd(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<ShardPreparedStatement, AceObjectPropertiesAttribute2nd>(ShardPreparedStatement.GetAceObjectPropertiesAttributes2nd, criteria);
            objects.ForEach(o =>
            {
                o.HasEverBeenSavedToDatabase = true;
                o.IsDirty = false;
            });
            return objects;
        }

        private List<AceObjectPropertiesInt> GetAceObjectPropertiesInt(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<ShardPreparedStatement, AceObjectPropertiesInt>(ShardPreparedStatement.GetAceObjectPropertiesInt, criteria);
            objects.ForEach(o =>
            {
                o.HasEverBeenSavedToDatabase = true;
                o.IsDirty = false;
            });
            return objects;
        }

        private List<AceObjectPropertiesInt64> GetAceObjectPropertiesBigInt(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<ShardPreparedStatement, AceObjectPropertiesInt64>(ShardPreparedStatement.GetAceObjectPropertiesBigInt, criteria);
            objects.ForEach(o =>
            {
                o.HasEverBeenSavedToDatabase = true;
                o.IsDirty = false;
            });
            return objects;
        }

        private List<AceObjectPropertiesBool> GetAceObjectPropertiesBool(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<ShardPreparedStatement, AceObjectPropertiesBool>(ShardPreparedStatement.GetAceObjectPropertiesBool, criteria);
            objects.ForEach(o =>
            {
                o.HasEverBeenSavedToDatabase = true;
                o.IsDirty = false;
            });
            return objects;
        }

        private List<AceObjectPropertiesDouble> GetAceObjectPropertiesDouble(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<ShardPreparedStatement, AceObjectPropertiesDouble>(ShardPreparedStatement.GetAceObjectPropertiesDouble, criteria);
            objects.ForEach(o =>
            {
                o.HasEverBeenSavedToDatabase = true;
                o.IsDirty = false;
            });
            return objects;
        }

        private List<AceObjectPropertiesString> GetAceObjectPropertiesString(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<ShardPreparedStatement, AceObjectPropertiesString>(ShardPreparedStatement.GetAceObjectPropertiesString, criteria);
            objects.ForEach(o =>
            {
                o.HasEverBeenSavedToDatabase = true;
                o.IsDirty = false;
            });
            return objects;
        }

        private List<AceObjectPropertiesDataId> GetAceObjectPropertiesDid(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<ShardPreparedStatement, AceObjectPropertiesDataId>(ShardPreparedStatement.GetAceObjectPropertiesDid, criteria);
            objects.ForEach(o =>
            {
                o.HasEverBeenSavedToDatabase = true;
                o.IsDirty = false;
            });
            return objects;
        }

        private List<AceObjectPropertiesInstanceId> GetAceObjectPropertiesIid(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<ShardPreparedStatement, AceObjectPropertiesInstanceId>(ShardPreparedStatement.GetAceObjectPropertiesIid, criteria);
            objects.ForEach(o =>
            {
                o.HasEverBeenSavedToDatabase = true;
                o.IsDirty = false;
            });
            return objects;
        }

        private List<AceObjectPropertiesSpell> GetAceObjectPropertiesSpell(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<ShardPreparedStatement, AceObjectPropertiesSpell>(ShardPreparedStatement.GetAceObjectPropertiesSpell, criteria);
            return objects;
        }

        private List<AceContractTracker> GetAceContractList(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<ShardPreparedStatement, AceContractTracker>(ShardPreparedStatement.GetAceContractTracker, criteria);
            return objects;
        }

        private List<AceObjectPropertiesSpellBarPositions> GetAceObjectPropertiesSpellBarPositions(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<ShardPreparedStatement, AceObjectPropertiesSpellBarPositions>(ShardPreparedStatement.GetAceObjectPropertiesSpellBarPositions, criteria);
            return objects;
        }

        private List<AceObjectPropertiesBook> GetAceObjectPropertiesBook(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<ShardPreparedStatement, AceObjectPropertiesBook>(ShardPreparedStatement.GetAceObjectPropertiesBook, criteria);
            return objects;
        }

        private List<TextureMapOverride> GetAceObjectTextureMaps(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<ShardPreparedStatement, TextureMapOverride>(ShardPreparedStatement.GetTextureOverridesByObject, criteria);
            return objects;
        }

        private List<PaletteOverride> GetAceObjectPalettes(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<ShardPreparedStatement, PaletteOverride>(ShardPreparedStatement.GetPaletteOverridesByObject, criteria);
            return objects;
        }

        private List<AnimationOverride> GetAceObjectAnimations(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<ShardPreparedStatement, AnimationOverride>(ShardPreparedStatement.GetAnimationOverridesByObject, criteria);
            return objects;
        }

        public ObjectInfo GetObjectInfoByName(string name)
        {
            throw new NotImplementedException();
        }

        public AceObject GetAceObject(uint objId)
        {
            AceObject ret = new AceObject();
            var criteria = new Dictionary<string, object> { { "aceObjectId", objId } };
            bool success = ExecuteConstructedGetStatement<ShardPreparedStatement>(ShardPreparedStatement.GetAceObject, typeof(AceObject), criteria, ret);
            if (!success)
            {
                return null;
            }
            ret.HasEverBeenSavedToDatabase = true;
            return ret;
        }

        private Dictionary<PositionType, Position> GetAceObjectPropertiesPositions(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<ShardPreparedStatement, AceObjectPropertiesPosition>(ShardPreparedStatement.GetAceObjectPropertiesPositions, criteria);
            return objects.ToDictionary(x => (PositionType)x.DbPositionType, x => new Position(x));
        }

        public List<AceObject> GetObjectsByLandblock(ushort landblock)
        {
            var criteria = new Dictionary<string, object> { { "landblock", landblock } };
            var objects = ExecuteConstructedGetListStatement<ShardPreparedStatement, CachedWorldObject>(ShardPreparedStatement.GetObjectsByLandblock, criteria);
            List<AceObject> ret = new List<AceObject>();
            objects.ForEach(cwo =>
            {
                var o = GetAceObject(cwo.AceObjectId);
                o.DataIdProperties = GetAceObjectPropertiesDid(o.AceObjectId);
                o.InstanceIdProperties = GetAceObjectPropertiesIid(o.AceObjectId);
                o.IntProperties = GetAceObjectPropertiesInt(o.AceObjectId);
                o.Int64Properties = GetAceObjectPropertiesBigInt(o.AceObjectId);
                o.BoolProperties = GetAceObjectPropertiesBool(o.AceObjectId);
                o.DoubleProperties = GetAceObjectPropertiesDouble(o.AceObjectId);
                o.StringProperties = GetAceObjectPropertiesString(o.AceObjectId);
                o.TextureOverrides = GetAceObjectTextureMaps(o.AceObjectId);
                o.AnimationOverrides = GetAceObjectAnimations(o.AceObjectId);
                o.PaletteOverrides = GetAceObjectPalettes(o.AceObjectId);
                o.AceObjectPropertiesPositions = GetAceObjectPropertiesPositions(o.AceObjectId);
                ret.Add(o);
            });
            return ret;
        }

        public bool IsCharacterNameAvailable(string name)
        {
            var cc = new CachedCharacter();
            var criteria = new Dictionary<string, object> { { "name", name } };
            return !(ExecuteConstructedGetStatement(ShardPreparedStatement.IsCharacterNameAvailable, typeof(CachedCharacter), criteria, cc));
        }

        public uint RenameCharacter(string currentName, string newName)
        {
            throw new NotImplementedException();
        }

        public bool SaveObject(AceObject aceObject)
        {
            DatabaseTransaction transaction = BeginTransaction();

            SaveObjectInternal(transaction, aceObject);

            // Do we have any inventory to save - if not, we are done here?
            if (aceObject.Inventory.Count > 0)
            {
                foreach (AceObject invItem in aceObject.Inventory.Values)
                {
                    if (invItem.IsDirty)
                    {
                        DeleteObjectInternal(transaction, invItem);
                        invItem.SetDirtyFlags();
                    }
                    SaveObjectInternal(transaction, invItem);

                    // Was the item I just saved a container?   If so, we need to save the items in the container as well. Og II
                    if (invItem.WeenieType == (uint)WeenieType.Container && invItem.Inventory.Count > 0)
                    {
                        foreach (AceObject contInvItem in invItem.Inventory.Values)
                        {
                            if (contInvItem.IsDirty)
                            {
                                DeleteObjectInternal(transaction, contInvItem);
                                contInvItem.SetDirtyFlags();
                            }
                            SaveObjectInternal(transaction, contInvItem);
                        }
                    }
                }
            }

            // Do we have any wielded items to save - if so, let's save them.
            if (aceObject.WieldedItems.Count > 0)
            {
                foreach (AceObject wieldedItem in aceObject.WieldedItems.Values)
                {
                    if (wieldedItem.IsDirty)
                    {
                        DeleteObjectInternal(transaction, wieldedItem);
                        wieldedItem.SetDirtyFlags();
                    }
                    SaveObjectInternal(transaction, wieldedItem);
                }
            }

            return transaction.Commit().Result;
        }

        public bool DeleteObject(AceObject aceObject)
        {
            DatabaseTransaction transaction = BeginTransaction();

            DeleteObjectInternal(transaction, aceObject);

            // Do we have any  - if not, we are done here?
            if (aceObject.Inventory.Count <= 0)
                return transaction.Commit().Result;
            foreach (AceObject invItem in aceObject.Inventory.Values)
            {
                DeleteObjectInternal(transaction, invItem);
                // Was the item I just deleted a container?   If so, we need to delete the items in the container as well. Og II
                if (invItem.WeenieType != (uint)WeenieType.Container)
                    continue;
                foreach (AceObject contInvItem in invItem.Inventory.Values)
                {
                    DeleteObjectInternal(transaction, contInvItem);
                }
            }
            return transaction.Commit().Result;
        }

        public uint SetCharacterAccessLevelByName(string name, AccessLevel accessLevel)
        {
            throw new NotImplementedException();
        }

        private bool DeleteObjectInternal(DatabaseTransaction transaction, AceObject aceObject)
        {
            // TODO: Database is designed to cascade delete so in reality we only need to call DeleteAceObjectBase
            // We need to decide to either let the db do the work or if we are going to keep doing it on the application side
            // should we remove the cascade deletes? Og II
            DeleteAceObjectPropertiesInt(transaction, aceObject.AceObjectId, aceObject.IntProperties);
            DeleteAceObjectPropertiesBigInt(transaction, aceObject.AceObjectId, aceObject.Int64Properties);
            DeleteAceObjectPropertiesBool(transaction, aceObject.AceObjectId, aceObject.BoolProperties);
            DeleteAceObjectPropertiesDouble(transaction, aceObject.AceObjectId, aceObject.DoubleProperties);
            DeleteAceObjectPropertiesString(transaction, aceObject.AceObjectId, aceObject.StringProperties);
            DeleteAceObjectPropertiesIid(transaction, aceObject.AceObjectId, aceObject.InstanceIdProperties);
            DeleteAceObjectPropertiesDid(transaction, aceObject.AceObjectId, aceObject.DataIdProperties);
            DeleteAceObjectTextureMaps(transaction, aceObject.AceObjectId, aceObject.TextureOverrides);
            DeleteAceObjectAnimations(transaction, aceObject.AceObjectId, aceObject.AnimationOverrides);
            DeleteAceObjectPalettes(transaction, aceObject.AceObjectId, aceObject.PaletteOverrides);

            DeleteAceObjectPropertiesPositions(transaction, aceObject.AceObjectId);
            DeleteAceObjectPropertiesAttributes(transaction, aceObject.AceObjectId);
            DeleteAceObjectPropertiesAttribute2nd(transaction, aceObject.AceObjectId);
            DeleteAceObjectPropertiesSkill(transaction, aceObject.AceObjectId);
            DeleteAceObjectPropertiesSpells(transaction, aceObject.AceObjectId);
            DeleteAceObjectPropertiesSpellBarPositions(transaction, aceObject.AceObjectId);
            DeleteAceContractTracker(transaction, aceObject.AceObjectId);

            DeleteAceObjectBase(transaction, aceObject);

            return true;
        }

        private bool SaveObjectInternal(DatabaseTransaction transaction, AceObject aceObject)
        {
            // Update the character table -- save the AceObject to ace_object.
            SaveAceObjectBase(transaction, aceObject);

            SaveAceObjectPropertiesInt(transaction, aceObject.AceObjectId, aceObject.IntProperties);
            SaveAceObjectPropertiesBigInt(transaction, aceObject.AceObjectId, aceObject.Int64Properties);
            SaveAceObjectPropertiesBool(transaction, aceObject.AceObjectId, aceObject.BoolProperties);
            SaveAceObjectPropertiesDouble(transaction, aceObject.AceObjectId, aceObject.DoubleProperties);
            SaveAceObjectPropertiesString(transaction, aceObject.AceObjectId, aceObject.StringProperties);
            SaveAceObjectPropertiesIid(transaction, aceObject.AceObjectId, aceObject.InstanceIdProperties);
            SaveAceObjectPropertiesDid(transaction, aceObject.AceObjectId, aceObject.DataIdProperties);

            DeleteAceObjectTextureMaps(transaction, aceObject.AceObjectId, aceObject.TextureOverrides);
            SaveAceObjectTextureMaps(transaction, aceObject.AceObjectId, aceObject.TextureOverrides);

            DeleteAceObjectPropertiesSpells(transaction, aceObject.AceObjectId);
            SaveAceObjectPropertiesSpells(transaction, aceObject.AceObjectId, aceObject.SpellIdProperties);

            DeleteAceObjectPropertiesSpellBarPositions(transaction, aceObject.AceObjectId);
            SaveAceObjectPropertiesSpellBarPositions(transaction, aceObject.SpellsInSpellBars);

            DeleteAceObjectAnimations(transaction, aceObject.AceObjectId, aceObject.AnimationOverrides);
            SaveAceObjectAnimations(transaction, aceObject.AceObjectId, aceObject.AnimationOverrides);

            DeleteAceObjectPalettes(transaction, aceObject.AceObjectId, aceObject.PaletteOverrides);
            SaveAceObjectPalettes(transaction, aceObject.AceObjectId, aceObject.PaletteOverrides);

            SaveAceObjectPropertiesAttributes(transaction, aceObject.AceObjectId, aceObject.AceObjectPropertiesAttributes);

            SaveAceObjectPropertiesAttribute2nd(transaction, aceObject.AceObjectId, aceObject.AceObjectPropertiesAttributes2nd);

            SaveAceObjectPropertiesSkill(transaction, aceObject.AceObjectId, aceObject.AceObjectPropertiesSkills);

            DeleteAceObjectPropertiesBook(transaction, aceObject.AceObjectId);
            SaveAceObjectPropertiesBook(transaction, aceObject.AceObjectId, aceObject.BookProperties);

            DeleteAceContractTracker(transaction, aceObject.AceObjectId);
            SaveAceContractTracker(transaction, aceObject.AceObjectId, aceObject.TrackedContracts.Select(x => x.Value).ToList());

            // positions are special.  the object is actually fully replaced, so we can't do dirty tracking on it (for now)
            // as a result, we delete them all then reinsert them all
            DeleteAceObjectPropertiesPositions(transaction, aceObject.AceObjectId);
            SaveAceObjectPropertiesPositions(transaction, aceObject.AceObjectId,
                aceObject.AceObjectPropertiesPositions.Select(x => x.Value.GetAceObjectPosition(aceObject.AceObjectId, x.Key)).ToList());

            return true;
        }

        private bool SaveAceObjectBase(DatabaseTransaction transaction, AceObject obj)
        {
            if (obj.IsDirty)
            {
                if (!obj.HasEverBeenSavedToDatabase)
                    transaction.AddPreparedInsertStatement<ShardPreparedStatement, AceObject>(ShardPreparedStatement.SaveAceObject, obj);
                else
                    transaction.AddPreparedUpdateStatement<ShardPreparedStatement, AceObject>(ShardPreparedStatement.UpdateAceObject, obj);
            }

            return true;
        }

        // FIXME(ddevec): These are a lot of functions that essentially do the same thing... but the SharedPreparedStatement.--- makes them a pain to template/reduce
        private bool SaveAceObjectPropertiesInt(DatabaseTransaction transaction, uint aceObjectId, List<AceObjectPropertiesInt> properties)
        {
            // calling ToList forces the enumerable evaluation so that we can call .Remove from within the loop
            var theDirtyOnes = properties.Where(p => p.IsDirty).ToList();

            foreach (var prop in theDirtyOnes)
            {
                if (prop.HasEverBeenSavedToDatabase)
                {
                    if (prop.PropertyValue == null)
                    {
                        // delete it
                        transaction.AddPreparedDeleteStatement<ShardPreparedStatement, AceObjectPropertiesInt>(ShardPreparedStatement.DeleteAceObjectPropertyInt, prop);
                        properties.Remove(prop);
                    }
                    else
                    {
                        // update it
                        transaction.AddPreparedUpdateStatement<ShardPreparedStatement, AceObjectPropertiesInt>(ShardPreparedStatement.UpdateAceObjectPropertyInt, prop);
                    }
                }
                else if (prop.PropertyValue != null)
                {
                    transaction.AddPreparedInsertStatement<ShardPreparedStatement, AceObjectPropertiesInt>(ShardPreparedStatement.InsertAceObjectPropertiesInt, prop);
                }
            }

            return true;
        }

      private bool SaveAceObjectPropertiesBigInt(DatabaseTransaction transaction, uint aceObjectId, List<AceObjectPropertiesInt64> properties)
        {
            // calling ToList forces the enumerable evaluation so that we can call .Remove from within the loop
            var theDirtyOnes = properties.Where(p => p.IsDirty).ToList();

            foreach (var prop in theDirtyOnes)
            {
                if (prop.HasEverBeenSavedToDatabase)
                {
                    if (prop.PropertyValue == null)
                    {
                        // delete it
                        transaction.AddPreparedDeleteStatement<ShardPreparedStatement, AceObjectPropertiesInt64>(ShardPreparedStatement.DeleteAceObjectPropertyBigInt, prop);
                        properties.Remove(prop);
                    }
                    else
                    {
                        // update it
                        transaction.AddPreparedUpdateStatement<ShardPreparedStatement, AceObjectPropertiesInt64>(ShardPreparedStatement.UpdateAceObjectPropertyBigInt, prop);
                    }
                }
                else if (prop.PropertyValue != null)
                {
                    transaction.AddPreparedInsertStatement<ShardPreparedStatement, AceObjectPropertiesInt64>(ShardPreparedStatement.InsertAceObjectPropertiesBigInt, prop);
                }
            }

            return true;
        }

        private bool SaveAceObjectPropertiesBool(DatabaseTransaction transaction, uint aceObjectId, List<AceObjectPropertiesBool> properties)
        {
            // calling ToList forces the enumerable evaluation so that we can call .Remove from within the loop
            var theDirtyOnes = properties.Where(p => p.IsDirty).ToList();

            foreach (var prop in theDirtyOnes)
            {
                if (prop.HasEverBeenSavedToDatabase)
                {
                    if (prop.PropertyValue == null)
                    {
                        // delete it
                        transaction.AddPreparedDeleteStatement<ShardPreparedStatement, AceObjectPropertiesBool>(ShardPreparedStatement.DeleteAceObjectPropertyBool, prop);
                        properties.Remove(prop);
                    }
                    else
                    {
                        // update it
                        transaction.AddPreparedUpdateStatement<ShardPreparedStatement, AceObjectPropertiesBool>(ShardPreparedStatement.UpdateAceObjectPropertyBool, prop);
                    }
                }
                else if (prop.PropertyValue != null)
                {
                    transaction.AddPreparedInsertStatement<ShardPreparedStatement, AceObjectPropertiesBool>(ShardPreparedStatement.InsertAceObjectPropertiesBool, prop);
                }
            }

            return true;
        }

        private bool SaveAceObjectPropertiesDouble(DatabaseTransaction transaction, uint aceObjectId, List<AceObjectPropertiesDouble> properties)
        {
            // calling ToList forces the enumerable evaluation so that we can call .Remove from within the loop
            var theDirtyOnes = properties.Where(p => p.IsDirty).ToList();

            foreach (var prop in theDirtyOnes)
            {
                if (prop.HasEverBeenSavedToDatabase)
                {
                    if (prop.PropertyValue == null)
                    {
                        // delete it
                        transaction.AddPreparedDeleteStatement<ShardPreparedStatement, AceObjectPropertiesDouble>(ShardPreparedStatement.DeleteAceObjectPropertyDouble, prop);
                        properties.Remove(prop);
                    }
                    else
                    {
                        // update it
                        transaction.AddPreparedUpdateStatement<ShardPreparedStatement, AceObjectPropertiesDouble>(ShardPreparedStatement.UpdateAceObjectPropertyDouble, prop);
                    }
                }
                else if (prop.PropertyValue != null)
                {
                    transaction.AddPreparedInsertStatement<ShardPreparedStatement, AceObjectPropertiesDouble>(ShardPreparedStatement.InsertAceObjectPropertiesDouble, prop);
                }
            }

            return true;
        }

        private bool SaveAceObjectPropertiesString(DatabaseTransaction transaction, uint aceObjectId, List<AceObjectPropertiesString> properties)
        {
            // calling ToList forces the enumerable evaluation so that we can call .Remove from within the loop
            var theDirtyOnes = properties.Where(p => p.IsDirty).ToList();

            foreach (var prop in theDirtyOnes)
            {
                if (prop.HasEverBeenSavedToDatabase)
                {
                    if (string.IsNullOrWhiteSpace(prop.PropertyValue))
                    {
                        // delete it
                        transaction.AddPreparedDeleteStatement<ShardPreparedStatement, AceObjectPropertiesString>(ShardPreparedStatement.DeleteAceObjectPropertyString, prop);
                        properties.Remove(prop);
                    }
                    else
                    {
                        // update it
                        transaction.AddPreparedUpdateStatement<ShardPreparedStatement, AceObjectPropertiesString>(ShardPreparedStatement.UpdateAceObjectPropertyString, prop);
                    }
                }
                else if (!string.IsNullOrWhiteSpace(prop.PropertyValue))
                {
                    transaction.AddPreparedInsertStatement<ShardPreparedStatement, AceObjectPropertiesString>(ShardPreparedStatement.InsertAceObjectPropertiesString, prop);
                }
            }

            return true;
        }

        private bool SaveAceObjectPropertiesDid(DatabaseTransaction transaction, uint aceObjectId, List<AceObjectPropertiesDataId> properties)
        {
            // calling ToList forces the enumerable evaluation so that we can call .Remove from within the loop
            var theDirtyOnes = properties.Where(p => p.IsDirty).ToList();

            foreach (var prop in theDirtyOnes)
            {
                if (prop.HasEverBeenSavedToDatabase)
                {
                    if (prop.PropertyValue == null)
                    {
                        // delete it
                        transaction.AddPreparedDeleteStatement<ShardPreparedStatement, AceObjectPropertiesDataId>(ShardPreparedStatement.DeleteAceObjectPropertyDid, prop);
                        properties.Remove(prop);
                    }
                    else
                    {
                        // update it
                        transaction.AddPreparedUpdateStatement<ShardPreparedStatement, AceObjectPropertiesDataId>(ShardPreparedStatement.UpdateAceObjectPropertyDid, prop);
                    }
                }
                else if (prop.PropertyValue != null)
                {
                    transaction.AddPreparedInsertStatement<ShardPreparedStatement, AceObjectPropertiesDataId>(ShardPreparedStatement.InsertAceObjectPropertiesDid, prop);
                }
            }

            return true;
        }

        private bool SaveAceObjectPropertiesIid(DatabaseTransaction transaction, uint aceObjectId, List<AceObjectPropertiesInstanceId> properties)
        {
            // calling ToList forces the enumerable evaluation so that we can call .Remove from within the loop
            var theDirtyOnes = properties.Where(p => p.IsDirty).ToList();

            foreach (var prop in theDirtyOnes)
            {
                if (prop.HasEverBeenSavedToDatabase)
                {
                    if (prop.PropertyValue == null)
                    {
                        // delete it
                        transaction.AddPreparedDeleteStatement<ShardPreparedStatement, AceObjectPropertiesInstanceId>(ShardPreparedStatement.DeleteAceObjectPropertyIid, prop);
                        properties.Remove(prop);
                    }
                    else
                    {
                        // update it
                        transaction.AddPreparedUpdateStatement<ShardPreparedStatement, AceObjectPropertiesInstanceId>(ShardPreparedStatement.UpdateAceObjectPropertyIid, prop);
                    }
                }
                else if (prop.PropertyValue != null)
                {
                    transaction.AddPreparedInsertStatement<ShardPreparedStatement, AceObjectPropertiesInstanceId>(ShardPreparedStatement.InsertAceObjectPropertiesIid, prop);
                }
            }

            return true;
        }

        private bool SaveAceObjectTextureMaps(DatabaseTransaction transaction, uint aceObjectId, List<TextureMapOverride> properties)
        {
            properties.ForEach(a => a.AceObjectId = aceObjectId);
            transaction.AddPreparedInsertListStatement<ShardPreparedStatement, TextureMapOverride>(ShardPreparedStatement.InsertTextureOverridesByObject, properties);
            return true;
        }

        private bool SaveAceObjectPropertiesSpells(DatabaseTransaction transaction, uint aceObjectId, List<AceObjectPropertiesSpell> properties)
        {
            properties.ForEach(a => a.AceObjectId = aceObjectId);
            transaction.AddPreparedInsertListStatement<ShardPreparedStatement, AceObjectPropertiesSpell>(ShardPreparedStatement.InsertAceObjectPropertiesSpells, properties);
            return true;
        }

        private bool SaveAceObjectPropertiesSpellBarPositions(DatabaseTransaction transaction, List<AceObjectPropertiesSpellBarPositions> properties)
        {
            transaction.AddPreparedInsertListStatement<ShardPreparedStatement, AceObjectPropertiesSpellBarPositions>(ShardPreparedStatement.InsertAceObjectPropertiesSpellBarPositions, properties);
            return true;
        }

        private bool SaveAceObjectPalettes(DatabaseTransaction transaction, uint aceObjectId, List<PaletteOverride> properties)
        {
            properties.ForEach(a => a.AceObjectId = aceObjectId);
            transaction.AddPreparedInsertListStatement<ShardPreparedStatement, PaletteOverride>(ShardPreparedStatement.InsertPaletteOverridesByObject, properties);
            return true;
        }

        private bool SaveAceObjectAnimations(DatabaseTransaction transaction, uint aceObjectId, List<AnimationOverride> properties)
        {
            properties.ForEach(a => a.AceObjectId = aceObjectId);
            transaction.AddPreparedInsertListStatement<ShardPreparedStatement, AnimationOverride>(ShardPreparedStatement.InsertAnimationOverridesByObject, properties);
            return true;
        }

        private bool SaveAceObjectPropertiesPositions(DatabaseTransaction transaction, uint aceObjectId, List<AceObjectPropertiesPosition> properties)
        {
            transaction.AddPreparedInsertListStatement<ShardPreparedStatement, AceObjectPropertiesPosition>(ShardPreparedStatement.InsertAceObjectPropertiesPositions, properties);
            return true;
        }

        private bool SaveAceObjectPropertiesAttributes(DatabaseTransaction transaction, uint aceObjectId, Dictionary<Ability, CreatureAbility> attributes)
        {
            var attribs = attributes.Values.Select(x => x.GetAttribute()).ToList();

            // setting AceObjectId doesn't trigger dirty
            attribs.ForEach(a => a.AceObjectId = aceObjectId);

            var theDirtyOnes = attribs.Where(x => x.IsDirty).ToList();

            foreach (var prop in theDirtyOnes)
            {
                if (prop.HasEverBeenSavedToDatabase)
                {
                    // update it
                    transaction.AddPreparedUpdateStatement<ShardPreparedStatement, AceObjectPropertiesAttribute>(ShardPreparedStatement.UpdateAceObjectPropertyAttribute, prop);
                }
                else
                {
                    transaction.AddPreparedInsertStatement<ShardPreparedStatement, AceObjectPropertiesAttribute>(ShardPreparedStatement.InsertAceObjectPropertiesAttributes, prop);
                }
            }

            return true;
        }

        private bool SaveAceObjectPropertiesAttribute2nd(DatabaseTransaction transaction, uint aceObjectId, Dictionary<Ability, CreatureVital> properties)
        {
            var attribs = properties.Values.Select(x => x.GetVital()).ToList();

            // setting AceObjectId doesn't trigger dirty
            attribs.ForEach(a => a.AceObjectId = aceObjectId);

            var theDirtyOnes = attribs.Where(x => x.IsDirty).ToList();

            foreach (var prop in theDirtyOnes)
            {
                if (prop.HasEverBeenSavedToDatabase)
                {
                    // update it
                    transaction.AddPreparedUpdateStatement<ShardPreparedStatement, AceObjectPropertiesAttribute2nd>(ShardPreparedStatement.UpdateAceObjectPropertyAttribute2nd, prop);
                }
                else
                {
                    transaction.AddPreparedInsertStatement<ShardPreparedStatement, AceObjectPropertiesAttribute2nd>(ShardPreparedStatement.InsertAceObjectPropertiesAttributes2nd, prop);
                }
            }

            return true;
        }

        // SaveAceObjectPropertiesSkill(transaction, aceObject.AceObjectPropertiesSkills);
        private bool SaveAceObjectPropertiesSkill(DatabaseTransaction transaction, uint aceObjectId, Dictionary<Skill, CreatureSkill> skills)
        {
            var tempSkills = skills.Values.Select(x => x.GetAceObjectSkill()).ToList();

            // setting AceObjectId doesn't trigger dirty
            tempSkills.ForEach(a => a.AceObjectId = aceObjectId);

            // calling ToList forces the enumerable evaluation so that we can call .Remove from within the loop
            var theDirtyOnes = tempSkills.Where(x => x.IsDirty).ToList();

            foreach (var prop in theDirtyOnes)
            {
                if (prop.HasEverBeenSavedToDatabase)
                {
                    if (prop.SkillStatus == (ushort)SkillStatus.Untrained)
                    {
                        // delete it.  this is possibly (likely?) a no-op
                        transaction.AddPreparedDeleteStatement<ShardPreparedStatement, AceObjectPropertiesSkill>(ShardPreparedStatement.DeleteAceObjectPropertiesSkills, prop);
                    }
                    else
                    {
                        // update it
                        transaction.AddPreparedUpdateStatement<ShardPreparedStatement, AceObjectPropertiesSkill>(ShardPreparedStatement.UpdateAceObjectPropertySkill, prop);
                    }
                }
                else
                {
                    transaction.AddPreparedInsertStatement<ShardPreparedStatement, AceObjectPropertiesSkill>(ShardPreparedStatement.InsertAceObjectPropertiesSkills, prop);
                }
            }

            return true;
        }

        private bool SaveAceObjectPropertiesBook(DatabaseTransaction transaction, uint aceObjectId, Dictionary<uint, AceObjectPropertiesBook> properties)
        {
            var pages = properties.Values.ToList();
            pages.ForEach(a => a.AceObjectId = aceObjectId);

            foreach (var page in pages)
                transaction.AddPreparedInsertStatement<ShardPreparedStatement, AceObjectPropertiesBook>(ShardPreparedStatement.InsertAceObjectPropertiesBook, page);
            return true;
        }

        private bool SaveAceContractTracker(DatabaseTransaction transaction, uint aceObjectId, List<AceContractTracker> properties)
        {
            properties.ForEach(a => a.AceObjectId = aceObjectId);
            transaction.AddPreparedInsertListStatement<ShardPreparedStatement, AceContractTracker>(ShardPreparedStatement.InsertAceContractTracker, properties);
            return true;
        }

        private bool DeleteAceObjectBase(DatabaseTransaction transaction, AceObject obj)
        {
            transaction.AddPreparedDeleteStatement<ShardPreparedStatement, AceObject>(ShardPreparedStatement.DeleteAceObject, obj);
            return true;
        }

        // FIXME(ddevec): These are a lot of functions that essentially do the same thing... but the SharedPreparedStatement.--- makes them a pain to tempalte/reduce
        private bool DeleteAceObjectPropertiesInt(DatabaseTransaction transaction, uint aceObjectId, List<AceObjectPropertiesInt> properties)
        {
            var critera = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            transaction.AddPreparedDeleteListStatement<ShardPreparedStatement, AceObjectPropertiesInt>(ShardPreparedStatement.DeleteAceObjectPropertiesInt, critera);
            return true;
        }

        private bool DeleteAceObjectPropertiesSpells(DatabaseTransaction transaction, uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            transaction.AddPreparedDeleteListStatement<ShardPreparedStatement, AceObjectPropertiesSpell>(ShardPreparedStatement.DeleteAceObjectPropertiesSpell, criteria);
            return true;
        }

        private bool DeleteAceObjectPropertiesSpellBarPositions(DatabaseTransaction transaction, uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            transaction.AddPreparedDeleteListStatement<ShardPreparedStatement, AceObjectPropertiesSpellBarPositions>(ShardPreparedStatement.DeleteAceObjectPropertiesSpellBarPositions, criteria);
            return true;
        }

        private bool DeleteAceObjectPropertiesBigInt(DatabaseTransaction transaction, uint aceObjectId, List<AceObjectPropertiesInt64> properties)
        {
            var critera = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            transaction.AddPreparedDeleteListStatement<ShardPreparedStatement, AceObjectPropertiesInt64>(ShardPreparedStatement.DeleteAceObjectPropertiesBigInt, critera);
            return true;
        }

        private bool DeleteAceObjectPropertiesBool(DatabaseTransaction transaction, uint aceObjectId, List<AceObjectPropertiesBool> properties)
        {
            var critera = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            transaction.AddPreparedDeleteListStatement<ShardPreparedStatement, AceObjectPropertiesBool>(ShardPreparedStatement.DeleteAceObjectPropertiesBool, critera);
            return true;
        }

        private bool DeleteAceObjectPropertiesDouble(DatabaseTransaction transaction, uint aceObjectId, List<AceObjectPropertiesDouble> properties)
        {
            var critera = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            transaction.AddPreparedDeleteListStatement<ShardPreparedStatement, AceObjectPropertiesDouble>(ShardPreparedStatement.DeleteAceObjectPropertiesDouble, critera);
            return true;
        }

        private bool DeleteAceObjectPropertiesString(DatabaseTransaction transaction, uint aceObjectId, List<AceObjectPropertiesString> properties)
        {
            var critera = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            transaction.AddPreparedDeleteListStatement<ShardPreparedStatement, AceObjectPropertiesString>(ShardPreparedStatement.DeleteAceObjectPropertiesString, critera);
            return true;
        }

        private bool DeleteAceObjectPropertiesDid(DatabaseTransaction transaction, uint aceObjectId, List<AceObjectPropertiesDataId> properties)
        {
            var critera = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            transaction.AddPreparedDeleteListStatement<ShardPreparedStatement, AceObjectPropertiesDataId>(ShardPreparedStatement.DeleteAceObjectPropertiesDid, critera);
            return true;
        }

        private bool DeleteAceObjectPropertiesIid(DatabaseTransaction transaction, uint aceObjectId, List<AceObjectPropertiesInstanceId> properties)
        {
            var critera = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            transaction.AddPreparedDeleteListStatement<ShardPreparedStatement, AceObjectPropertiesInstanceId>(ShardPreparedStatement.DeleteAceObjectPropertiesIid, critera);
            return true;
        }

        private bool DeleteAceObjectTextureMaps(DatabaseTransaction transaction, uint aceObjectId, List<TextureMapOverride> properties)
        {
            var critera = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            transaction.AddPreparedDeleteListStatement<ShardPreparedStatement, TextureMapOverride>(ShardPreparedStatement.DeleteTextureOverridesByObject, critera);
            return true;
        }

        private bool DeleteAceObjectPalettes(DatabaseTransaction transaction, uint aceObjectId, List<PaletteOverride> properties)
        {
            var critera = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            transaction.AddPreparedDeleteListStatement<ShardPreparedStatement, PaletteOverride>(ShardPreparedStatement.DeletePaletteOverridesByObject, critera);
            return true;
        }

        private bool DeleteAceObjectAnimations(DatabaseTransaction transaction, uint aceObjectId, List<AnimationOverride> properties)
        {
            var critera = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            transaction.AddPreparedDeleteListStatement<ShardPreparedStatement, AnimationOverride>(ShardPreparedStatement.DeleteAnimationOverridesByObject, critera);
            return true;
        }

        private bool DeleteAceObjectPropertiesPositions(DatabaseTransaction transaction, uint aceObjectId)
        {
            var critera = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            transaction.AddPreparedDeleteListStatement<ShardPreparedStatement, AceObjectPropertiesPosition>(ShardPreparedStatement.DeleteAceObjectPropertiesPositions, critera);
            return true;
        }

        private bool DeleteAceObjectPropertiesAttributes(DatabaseTransaction transaction, uint aceObjectId)
        {
            var critera = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            transaction.AddPreparedDeleteListStatement<ShardPreparedStatement, AceObjectPropertiesAttribute>(ShardPreparedStatement.DeleteAceObjectPropertiesAttributes, critera);
            return true;
        }

        private bool DeleteAceObjectPropertiesAttribute2nd(DatabaseTransaction transaction, uint aceObjectId)
        {
            var critera = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            transaction.AddPreparedDeleteListStatement<ShardPreparedStatement, AceObjectPropertiesAttribute2nd>(ShardPreparedStatement.DeleteAceObjectPropertiesAttributes2nd, critera);
            return true;
        }

        // SaveAceObjectPropertiesSkill(transaction, aceObject.AceObjectPropertiesSkills);
        private bool DeleteAceObjectPropertiesSkill(DatabaseTransaction transaction, uint aceObjectId)
        {
            var critera = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            transaction.AddPreparedDeleteListStatement<ShardPreparedStatement, AceObjectPropertiesSkill>(ShardPreparedStatement.DeleteAceObjectPropertiesSkills, critera);
            return true;
        }

        private bool DeleteAceObjectPropertiesBook(DatabaseTransaction transaction, uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            transaction.AddPreparedDeleteListStatement<ShardPreparedStatement, AceObjectPropertiesBook>(ShardPreparedStatement.DeleteAceObjectPropertiesBook, criteria);
            return true;
        }

        private bool DeleteAceContractTracker(DatabaseTransaction transaction, uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            transaction.AddPreparedDeleteListStatement<ShardPreparedStatement, AceContractTracker>(ShardPreparedStatement.DeleteAceContractTracker, criteria);
            return true;
        }
    }
}
