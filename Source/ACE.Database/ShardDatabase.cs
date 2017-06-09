using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ACE.Entity;
using ACE.Entity.Enum;

using log4net;

namespace ACE.Database
{
    public class ShardDatabase : Database, IShardDatabase
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private enum ShardPreparedStatement
        {
            // these are for the world database, but there's a lot of overlap
            // so we're just going to reserve 0-100 for them here
            TeleportLocationSelect = 0,
            GetWeenieClass = 1,
            GetObjectsByLandblock = 2,
            GetCreaturesByLandblock = 3,
            GetWeeniePalettes = 4,
            GetWeenieTextureMaps = 5,
            GetWeenieAnimations = 6,
            GetPaletteOverridesByObject = 7,
            GetAnimationOverridesByObject = 8,
            GetTextureOverridesByObject = 9,
            GetCreatureDataByWeenie = 10,
            InsertCreatureStaticLocation = 11,
            GetCreatureGeneratorByLandblock = 12,
            GetCreatureGeneratorData = 13,
            GetPortalObjectsByAceObjectId = 14,
            GetItemsByTypeId = 15,
            GetAceObjectPropertiesInt = 16,
            GetAceObjectPropertiesBigInt = 17,
            GetAceObjectPropertiesDouble = 18,
            GetAceObjectPropertiesBool = 19,
            GetAceObjectPropertiesString = 20,
            GetAceObjectPropertiesDid = 21,
            GetAceObjectPropertiesIid = 22,
            GetAceObjectPropertiesPositions = 23,
            GetAceObjectPropertiesAttributes = 24,
            GetAceObjectPropertiesAttributes2nd = 25,
            GetAceObjectPropertiesSkills = 26,

            SaveAceObject = 27,

            AddFriend = 101,
            DeleteFriend = 102,
            GetFriends = 103,
            DeleteAllFriends = 104,

            GetCharacters = 105,
            GetNextCharacterId = 106,
            IsCharacterNameAvailable = 107,
            // keep on going, you get the idea

            // DDEVEC -- Maybe fixme hacky --
            // The Delete fields are mirrors of Get at +200 -- elides the need for extra attributes (piggyback on DbGetList attribute)
            DeletePaletteOverridesByObject = 207,
            DeleteAnimationOverridesByObject = 208,
            DeleteTextureOverridesByObject = 209,
            DeleteAceObjectPropertiesInt = 216,
            DeleteAceObjectPropertiesBigInt = 217,
            DeleteAceObjectPropertiesDouble = 218,
            DeleteAceObjectPropertiesBool = 219,
            DeleteAceObjectPropertiesString = 220,
            DeleteAceObjectPropertiesDid = 221,
            DeleteAceObjectPropertiesIid = 222,
            DeleteAceObjectPropertiesPositions = 223,
            DeleteAceObjectPropertiesAttributes = 224,
            DeleteAceObjectPropertiesAttributes2nd = 225,
            DeleteAceObjectPropertiesSkills = 226,

            // DDEVEC -- Maybe fixme hacky --
            // The Insert fields are mirrors of Get at +300 -- elides the need for extra attributes (piggyback on DbGetList attribute)
            InsertPaletteOverridesByObject = 307,
            InsertAnimationOverridesByObject = 308,
            InsertTextureOverridesByObject = 309,
            InsertAceObjectPropertiesInt = 316,
            InsertAceObjectPropertiesBigInt = 317,
            InsertAceObjectPropertiesDouble = 318,
            InsertAceObjectPropertiesBool = 319,
            InsertAceObjectPropertiesString = 320,
            InsertAceObjectPropertiesDid = 321,
            InsertAceObjectPropertiesIid = 322,
            InsertAceObjectPropertiesPositions = 323,
            InsertAceObjectPropertiesAttributes = 324,
            InsertAceObjectPropertiesAttributes2nd = 325,
            InsertAceObjectPropertiesSkills = 326,
        }

        protected override Type PreparedStatementType
        {
            get
            {
                return typeof(ShardPreparedStatement);
            }
        }

        protected override void InitializePreparedStatements()
        {
            ConstructStatement(
               ShardPreparedStatement.GetCharacters,
                typeof(CachedCharacter),
                ConstructedStatementType.GetList);

			ConstructStatement(
                ShardPreparedStatement.GetNextCharacterId,
                typeof(CachedCharacter),
                ConstructedStatementType.GetAggregate);

            ConstructStatement(
                ShardPreparedStatement.IsCharacterNameAvailable,
                typeof(CachedCharacter),
                ConstructedStatementType.Get);

            ConstructStatement(
                ShardPreparedStatement.SaveAceObject,
                typeof(AceObject),
                ConstructedStatementType.Insert);

            // Get lists
            ConstructStatement(
                ShardPreparedStatement.GetAceObjectPropertiesBigInt,
                typeof(AceObjectPropertiesInt64),
                ConstructedStatementType.GetList);

            ConstructStatement(
                ShardPreparedStatement.GetAceObjectPropertiesBool,
                typeof(AceObjectPropertiesBool),
                ConstructedStatementType.GetList);

            ConstructStatement(
                ShardPreparedStatement.GetAceObjectPropertiesDouble,
                typeof(AceObjectPropertiesDouble),
                ConstructedStatementType.GetList);

            ConstructStatement(
                ShardPreparedStatement.GetAceObjectPropertiesString,
                typeof(AceObjectPropertiesString),
                ConstructedStatementType.GetList);

            ConstructStatement(
                ShardPreparedStatement.GetAceObjectPropertiesIid,
                typeof(AceObjectPropertiesInstanceId),
                ConstructedStatementType.GetList);

            ConstructStatement(
                ShardPreparedStatement.GetAceObjectPropertiesDid,
                typeof(AceObjectPropertiesDataId),
                ConstructedStatementType.GetList);

            ConstructStatement(
                ShardPreparedStatement.GetTextureOverridesByObject,
                typeof(TextureMapOverride),
                ConstructedStatementType.GetList);

            ConstructStatement(
                ShardPreparedStatement.GetPaletteOverridesByObject,
                typeof(PaletteOverride),
                ConstructedStatementType.GetList);

            ConstructStatement(
                ShardPreparedStatement.GetAnimationOverridesByObject,
                typeof(AnimationOverride),
                ConstructedStatementType.GetList);

            ConstructStatement(
                ShardPreparedStatement.GetAceObjectPropertiesPositions,
                typeof(Position),
                ConstructedStatementType.GetList);

            ConstructStatement(
                ShardPreparedStatement.GetAceObjectPropertiesAttributes,
                typeof(AceObjectPropertiesAttribute),
                ConstructedStatementType.GetList);

            ConstructStatement(
                ShardPreparedStatement.GetAceObjectPropertiesAttributes2nd,
                typeof(AceObjectPropertiesAttribute2nd),
                ConstructedStatementType.GetList);

            ConstructStatement(
                ShardPreparedStatement.GetAceObjectPropertiesSkills,
                typeof(AceObjectPropertiesSkill),
                ConstructedStatementType.GetList);

            ConstructStatement(
                ShardPreparedStatement.GetAceObjectPropertiesInt,
                typeof(AceObjectPropertiesInt),
                ConstructedStatementType.GetList);


            // Delete statements
            ConstructStatement(
                ShardPreparedStatement.DeleteAceObjectPropertiesInt,
                typeof(AceObjectPropertiesInt),
                ConstructedStatementType.DeleteList);

            ConstructStatement(
                ShardPreparedStatement.DeleteAceObjectPropertiesBigInt,
                typeof(AceObjectPropertiesInt64),
                ConstructedStatementType.DeleteList);

            ConstructStatement(
                ShardPreparedStatement.DeleteAceObjectPropertiesBool,
                typeof(AceObjectPropertiesBool),
                ConstructedStatementType.DeleteList);

            ConstructStatement(
                ShardPreparedStatement.DeleteAceObjectPropertiesDouble,
                typeof(AceObjectPropertiesDouble),
                ConstructedStatementType.DeleteList);

            ConstructStatement(
                ShardPreparedStatement.DeleteAceObjectPropertiesString,
                typeof(AceObjectPropertiesString),
                ConstructedStatementType.DeleteList);

            ConstructStatement(
                ShardPreparedStatement.DeleteAceObjectPropertiesIid,
                typeof(AceObjectPropertiesInstanceId),
                ConstructedStatementType.DeleteList);

            ConstructStatement(
                ShardPreparedStatement.DeleteAceObjectPropertiesDid,
                typeof(AceObjectPropertiesDataId),
                ConstructedStatementType.DeleteList);

            ConstructStatement(
                ShardPreparedStatement.DeleteAceObjectPropertiesPositions,
                typeof(Position),
                ConstructedStatementType.DeleteList);

            ConstructStatement(
                ShardPreparedStatement.DeleteAceObjectPropertiesSkills,
                typeof(AceObjectPropertiesSkill),
                ConstructedStatementType.DeleteList);

            ConstructStatement(
                ShardPreparedStatement.DeleteAceObjectPropertiesAttributes,
                typeof(AceObjectPropertiesAttribute),
                ConstructedStatementType.DeleteList);

            ConstructStatement(
                ShardPreparedStatement.DeleteAceObjectPropertiesAttributes2nd,
                typeof(AceObjectPropertiesAttribute2nd),
                ConstructedStatementType.DeleteList);

            ConstructStatement(
                ShardPreparedStatement.DeleteTextureOverridesByObject,
                typeof(TextureMapOverride),
                ConstructedStatementType.DeleteList);

            ConstructStatement(
                ShardPreparedStatement.DeletePaletteOverridesByObject,
                typeof(PaletteOverride),
                ConstructedStatementType.DeleteList);

            ConstructStatement(
                ShardPreparedStatement.DeleteAnimationOverridesByObject,
                typeof(AnimationOverride),
                ConstructedStatementType.DeleteList);

            // Insert statements
            ConstructStatement(
                ShardPreparedStatement.InsertAceObjectPropertiesInt,
                typeof(AceObjectPropertiesInt),
                ConstructedStatementType.InsertList);

            ConstructStatement(
                ShardPreparedStatement.InsertAceObjectPropertiesBigInt,
                typeof(AceObjectPropertiesInt64),
                ConstructedStatementType.InsertList);

            ConstructStatement(
                ShardPreparedStatement.InsertAceObjectPropertiesBool,
                typeof(AceObjectPropertiesBool),
                ConstructedStatementType.InsertList);

            ConstructStatement(
                ShardPreparedStatement.InsertAceObjectPropertiesDouble,
                typeof(AceObjectPropertiesDouble),
                ConstructedStatementType.InsertList);

            ConstructStatement(
                ShardPreparedStatement.InsertAceObjectPropertiesString,
                typeof(AceObjectPropertiesString),
                ConstructedStatementType.InsertList);

            ConstructStatement(
                ShardPreparedStatement.InsertAceObjectPropertiesIid,
                typeof(AceObjectPropertiesInstanceId),
                ConstructedStatementType.InsertList);

            ConstructStatement(
                ShardPreparedStatement.InsertAceObjectPropertiesDid,
                typeof(AceObjectPropertiesDataId),
                ConstructedStatementType.InsertList);

            ConstructStatement(
                ShardPreparedStatement.InsertAceObjectPropertiesPositions,
                typeof(Position),
                ConstructedStatementType.InsertList);

            ConstructStatement(
                ShardPreparedStatement.InsertAceObjectPropertiesSkills,
                typeof(AceObjectPropertiesSkill),
                ConstructedStatementType.InsertList);

            ConstructStatement(
                ShardPreparedStatement.InsertAceObjectPropertiesAttributes,
                typeof(AceObjectPropertiesAttribute),
                ConstructedStatementType.InsertList);

            ConstructStatement(
                ShardPreparedStatement.InsertAceObjectPropertiesAttributes2nd,
                typeof(AceObjectPropertiesAttribute2nd),
                ConstructedStatementType.InsertList);

            ConstructStatement(
                ShardPreparedStatement.InsertTextureOverridesByObject,
                typeof(TextureMapOverride),
                ConstructedStatementType.InsertList);

            ConstructStatement(
                ShardPreparedStatement.InsertPaletteOverridesByObject,
                typeof(PaletteOverride),
                ConstructedStatementType.InsertList);

            ConstructStatement(
                ShardPreparedStatement.InsertAnimationOverridesByObject,
                typeof(AnimationOverride),
                ConstructedStatementType.InsertList);
        }

        public Task AddFriend(uint characterId, uint friendCharacterId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteFriend(uint characterId, uint friendCharacterId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAllFriends(uint characterId)
        {
            throw new NotImplementedException();
        }

        public void DeleteOrRestore(ulong unixTime, uint id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CachedCharacter>> GetCharacters(uint accountId)
        {
            var criteria = new Dictionary<string, object> { { "accountId", accountId }, { "deleted", 0 } };
            var objects = ExecuteConstructedGetListStatement<ShardPreparedStatement, CachedCharacter>(ShardPreparedStatement.GetCharacters, criteria);

            return objects;
        }

        public uint GetNextCharacterId()
        {
            uint maxId = ExecuteConstructedGetAggregateStatement<ShardPreparedStatement, CachedCharacter, uint>(ShardPreparedStatement.GetNextCharacterId);

            ObjectGuid nextGuid = new ObjectGuid(maxId + 1, GuidType.Player);
            return nextGuid.Full;
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
            AceObject aceObject = new AceObject(aceObjectId);
            LoadIntoObject(aceObject);
            return aceObject;
        }

        private void LoadIntoObject(AceObject aceObject)
        {
            // TODO: still to implement - load spells, friends, allegiance info, spell comps, spell bars
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
            aceObject.Positions = GetAceObjectPostions(aceObject.AceObjectId);
            aceObject.AceObjectPropertiesAttributes = GetAceObjectPropertiesAttribute(aceObject.AceObjectId);
            aceObject.AceObjectPropertiesAttributes2nd = GetAceObjectPropertiesAttribute2nd(aceObject.AceObjectId);
            aceObject.AceObjectPropertiesSkills = GetAceObjectPropertiesSkill(aceObject.AceObjectId);
        }

        private Dictionary<PositionType, Position> GetAceObjectPostions(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "AceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<ShardPreparedStatement, Position>(ShardPreparedStatement.GetAceObjectPropertiesPositions, criteria);
            return objects.ToDictionary(x => x.PositionType, x => x);
        }

        private List<AceObjectPropertiesSkill> GetAceObjectPropertiesSkill(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "AceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<ShardPreparedStatement, AceObjectPropertiesSkill>(ShardPreparedStatement.GetAceObjectPropertiesSkills, criteria);
            return objects;
        }

        private List<AceObjectPropertiesAttribute> GetAceObjectPropertiesAttribute(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "AceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<ShardPreparedStatement, AceObjectPropertiesAttribute>(ShardPreparedStatement.GetAceObjectPropertiesAttributes, criteria);
            return objects;
        }

        // ReSharper disable once InconsistentNaming
        private List<AceObjectPropertiesAttribute2nd> GetAceObjectPropertiesAttribute2nd(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "AceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<ShardPreparedStatement, AceObjectPropertiesAttribute2nd>(ShardPreparedStatement.GetAceObjectPropertiesAttributes2nd, criteria);
            return objects;
        }
        private List<AceObjectPropertiesInt> GetAceObjectPropertiesInt(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "AceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<ShardPreparedStatement, AceObjectPropertiesInt>(ShardPreparedStatement.GetAceObjectPropertiesInt, criteria);
            return objects;
        }

        private List<AceObjectPropertiesInt64> GetAceObjectPropertiesBigInt(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "AceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<ShardPreparedStatement, AceObjectPropertiesInt64>(ShardPreparedStatement.GetAceObjectPropertiesBigInt, criteria);
            return objects;
        }

        private List<AceObjectPropertiesBool> GetAceObjectPropertiesBool(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "AceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<ShardPreparedStatement, AceObjectPropertiesBool>(ShardPreparedStatement.GetAceObjectPropertiesBool, criteria);
            return objects;
        }

        private List<AceObjectPropertiesDouble> GetAceObjectPropertiesDouble(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "AceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<ShardPreparedStatement, AceObjectPropertiesDouble>(ShardPreparedStatement.GetAceObjectPropertiesDouble, criteria);
            return objects;
        }

        private List<AceObjectPropertiesString> GetAceObjectPropertiesString(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "AceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<ShardPreparedStatement, AceObjectPropertiesString>(ShardPreparedStatement.GetAceObjectPropertiesString, criteria);
            return objects;
        }

        private List<AceObjectPropertiesDataId> GetAceObjectPropertiesDid(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "AceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<ShardPreparedStatement, AceObjectPropertiesDataId>(ShardPreparedStatement.GetAceObjectPropertiesDid, criteria);
            return objects;
        }

        private List<AceObjectPropertiesInstanceId> GetAceObjectPropertiesIid(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "AceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<ShardPreparedStatement, AceObjectPropertiesInstanceId>(ShardPreparedStatement.GetAceObjectPropertiesIid, criteria);
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


        public Task<ObjectInfo> GetObjectInfoByName(string name)
        {
            throw new NotImplementedException();
        }

        public List<AceObject> GetObjectsByLandblock(ushort landblock)
        {
            throw new NotImplementedException();
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

        public void SaveCharacterOptions(AceCharacter character)
        {
            // throw new NotImplementedException();
        }

        public bool SaveObject(AceObject aceObject)
        {
            DatabaseTransaction transaction = BeginTransaction();

            // Update the character table -- save the AceObject to ace_object.
            SaveAceObjectBase(transaction, aceObject);

            //throw new NotImplementedException();
            SaveAceObjectPropertiesInt(transaction, aceObject.AceObjectId, aceObject.IntProperties);
            SaveAceObjectPropertiesBigInt(transaction, aceObject.AceObjectId, aceObject.Int64Properties);
            SaveAceObjectPropertiesBool(transaction, aceObject.AceObjectId, aceObject.BoolProperties);
            SaveAceObjectPropertiesDouble(transaction, aceObject.AceObjectId, aceObject.DoubleProperties);
            SaveAceObjectPropertiesString(transaction, aceObject.AceObjectId, aceObject.StringProperties);
            SaveAceObjectPropertiesIid(transaction, aceObject.AceObjectId, aceObject.InstanceIdProperties);
            SaveAceObjectPropertiesDid(transaction, aceObject.AceObjectId, aceObject.DataIdProperties);
            SaveAceObjectTextureMaps(transaction, aceObject.AceObjectId, aceObject.TextureOverrides);
            SaveAceObjectAnimations(transaction, aceObject.AceObjectId, aceObject.AnimationOverrides);
            SaveAceObjectPalettes(transaction, aceObject.AceObjectId, aceObject.PaletteOverrides);


            SaveAceObjectPropertiesPositions(transaction, aceObject.AceObjectId, aceObject.Positions.Values.ToList());
            SaveAceObjectPropertiesAttributes(transaction, aceObject.AceObjectId, aceObject.AceObjectPropertiesAttributes);
            SaveAceObjectPropertiesAttribute2nd(transaction, aceObject.AceObjectId, aceObject.AceObjectPropertiesAttributes2nd);
            SaveAceObjectPropertiesSkill(transaction, aceObject.AceObjectId, aceObject.AceObjectPropertiesSkills);

            // FIXME(ddevec): Should we wait on this TXN?  I have no idea.
            Task<bool> txn = transaction.Commit();
            txn.Wait();
            bool result = txn.Result;
            return result;
        }

        public uint SetCharacterAccessLevelByName(string name, AccessLevel accessLevel)
        {
            throw new NotImplementedException();
        }

        private bool SaveAceObjectBase(DatabaseTransaction transaction, AceObject obj)
        {
            transaction.AddPreparedInsertStatement<ShardPreparedStatement, AceObject>(ShardPreparedStatement.SaveAceObject, obj);
            return true;
        }

        // FIXME(ddevec): These are a lot of functions that essentially do the same thing... but the SharedPreparedStatement.--- makes them a pain to tempalte/reduce
        private bool SaveAceObjectPropertiesInt(DatabaseTransaction transaction, uint aceObjectId, List<AceObjectPropertiesInt> properties)
        {
            // First  delete
            var critera = new Dictionary<string, object> { { "AceObjectId", aceObjectId} };
            transaction.AddPreparedDeleteListStatement<ShardPreparedStatement, AceObjectPropertiesInt>(ShardPreparedStatement.DeleteAceObjectPropertiesInt, critera);

            // Then  re-insert
            transaction.AddPreparedInsertListStatement<ShardPreparedStatement, AceObjectPropertiesInt>(ShardPreparedStatement.InsertAceObjectPropertiesInt, properties);
            return true;
        }

        private bool SaveAceObjectPropertiesBigInt(DatabaseTransaction transaction, uint aceObjectId, List<AceObjectPropertiesInt64> properties)
        {
            var critera = new Dictionary<string, object> { { "AceObjectId", aceObjectId} };
            transaction.AddPreparedDeleteListStatement<ShardPreparedStatement, AceObjectPropertiesInt64>(ShardPreparedStatement.DeleteAceObjectPropertiesBigInt, critera);
            transaction.AddPreparedInsertListStatement<ShardPreparedStatement, AceObjectPropertiesInt64>(ShardPreparedStatement.InsertAceObjectPropertiesBigInt, properties);
            return true;
        }

        private bool SaveAceObjectPropertiesBool(DatabaseTransaction transaction, uint aceObjectId, List<AceObjectPropertiesBool> properties)
        {
            var critera = new Dictionary<string, object> { { "AceObjectId", aceObjectId} };
            transaction.AddPreparedDeleteListStatement<ShardPreparedStatement, AceObjectPropertiesBool>(ShardPreparedStatement.DeleteAceObjectPropertiesBool, critera);
            transaction.AddPreparedInsertListStatement<ShardPreparedStatement, AceObjectPropertiesBool>(ShardPreparedStatement.InsertAceObjectPropertiesBool, properties);
            return true;
        }

        private bool SaveAceObjectPropertiesDouble(DatabaseTransaction transaction, uint aceObjectId, List<AceObjectPropertiesDouble> properties)
        {
            var critera = new Dictionary<string, object> { { "AceObjectId", aceObjectId} };
            transaction.AddPreparedDeleteListStatement<ShardPreparedStatement, AceObjectPropertiesDouble>(ShardPreparedStatement.DeleteAceObjectPropertiesDouble, critera);
            transaction.AddPreparedInsertListStatement<ShardPreparedStatement, AceObjectPropertiesDouble>(ShardPreparedStatement.InsertAceObjectPropertiesDouble, properties);
            return true;
        }

        private bool SaveAceObjectPropertiesString(DatabaseTransaction transaction, uint aceObjectId, List<AceObjectPropertiesString> properties)
        {
            var critera = new Dictionary<string, object> { { "AceObjectId", aceObjectId} };
            transaction.AddPreparedDeleteListStatement<ShardPreparedStatement, AceObjectPropertiesString>(ShardPreparedStatement.DeleteAceObjectPropertiesString, critera);
            transaction.AddPreparedInsertListStatement<ShardPreparedStatement, AceObjectPropertiesString>(ShardPreparedStatement.InsertAceObjectPropertiesString, properties);
            return true;
        }

        private bool SaveAceObjectPropertiesDid(DatabaseTransaction transaction, uint aceObjectId, List<AceObjectPropertiesDataId> properties)
        {
            var critera = new Dictionary<string, object> { { "AceObjectId", aceObjectId} };
            transaction.AddPreparedDeleteListStatement<ShardPreparedStatement, AceObjectPropertiesDataId>(ShardPreparedStatement.DeleteAceObjectPropertiesDid, critera);
            transaction.AddPreparedInsertListStatement<ShardPreparedStatement, AceObjectPropertiesDataId>(ShardPreparedStatement.InsertAceObjectPropertiesDid, properties);
            return true;
        }

        private bool SaveAceObjectPropertiesIid(DatabaseTransaction transaction, uint aceObjectId, List<AceObjectPropertiesInstanceId> properties)
        {
            var critera = new Dictionary<string, object> { { "AceObjectId", aceObjectId} };
            transaction.AddPreparedDeleteListStatement<ShardPreparedStatement, AceObjectPropertiesInstanceId>(ShardPreparedStatement.DeleteAceObjectPropertiesIid, critera);
            transaction.AddPreparedInsertListStatement<ShardPreparedStatement, AceObjectPropertiesInstanceId>(ShardPreparedStatement.InsertAceObjectPropertiesIid, properties);
            return true;
        }

        private bool SaveAceObjectTextureMaps(DatabaseTransaction transaction, uint aceObjectId, List<TextureMapOverride> properties)
        {
            var critera = new Dictionary<string, object> { { "aceObjectId", aceObjectId} };
            transaction.AddPreparedDeleteListStatement<ShardPreparedStatement, TextureMapOverride>(ShardPreparedStatement.DeleteTextureOverridesByObject, critera);
            transaction.AddPreparedInsertListStatement<ShardPreparedStatement, TextureMapOverride>(ShardPreparedStatement.InsertTextureOverridesByObject, properties);
            return true;
        }

        private bool SaveAceObjectPalettes(DatabaseTransaction transaction, uint aceObjectId, List<PaletteOverride> properties)
        {
            var critera = new Dictionary<string, object> { { "aceObjectId", aceObjectId} };
            transaction.AddPreparedDeleteListStatement<ShardPreparedStatement, PaletteOverride>(ShardPreparedStatement.DeletePaletteOverridesByObject, critera);
            transaction.AddPreparedInsertListStatement<ShardPreparedStatement, PaletteOverride>(ShardPreparedStatement.InsertPaletteOverridesByObject, properties);
            return true;
        }

        private bool SaveAceObjectAnimations(DatabaseTransaction transaction, uint aceObjectId, List<AnimationOverride> properties)
        {
            var critera = new Dictionary<string, object> { { "aceObjectId", aceObjectId} };
            transaction.AddPreparedDeleteListStatement<ShardPreparedStatement, AnimationOverride>(ShardPreparedStatement.DeleteAnimationOverridesByObject, critera);
            transaction.AddPreparedInsertListStatement<ShardPreparedStatement, AnimationOverride>(ShardPreparedStatement.InsertAnimationOverridesByObject, properties);
            return true;
        }

        private bool SaveAceObjectPropertiesPositions(DatabaseTransaction transaction, uint aceObjectId, List<Position> properties)
        {
            var critera = new Dictionary<string, object> { { "aceObjectId", aceObjectId} };
            transaction.AddPreparedDeleteListStatement<ShardPreparedStatement, Position>(ShardPreparedStatement.DeleteAceObjectPropertiesPositions, critera);
            transaction.AddPreparedInsertListStatement<ShardPreparedStatement, Position>(ShardPreparedStatement.InsertAceObjectPropertiesPositions, properties);
            return true;
        }

        private bool SaveAceObjectPropertiesAttributes(DatabaseTransaction transaction, uint aceObjectId, List<AceObjectPropertiesAttribute> properties)
        {
            var critera = new Dictionary<string, object> { { "aceObjectId", aceObjectId} };
            transaction.AddPreparedDeleteListStatement<ShardPreparedStatement, AceObjectPropertiesAttribute>(ShardPreparedStatement.DeleteAceObjectPropertiesAttributes, critera);
            transaction.AddPreparedInsertListStatement<ShardPreparedStatement, AceObjectPropertiesAttribute>(ShardPreparedStatement.InsertAceObjectPropertiesAttributes, properties);
            return true;
        }
        
        private bool SaveAceObjectPropertiesAttribute2nd(DatabaseTransaction transaction, uint aceObjectId, List<AceObjectPropertiesAttribute2nd> properties)
        {
            var critera = new Dictionary<string, object> { { "aceObjectId", aceObjectId} };
            transaction.AddPreparedDeleteListStatement<ShardPreparedStatement, AceObjectPropertiesAttribute2nd>(ShardPreparedStatement.DeleteAceObjectPropertiesAttributes2nd, critera);
            transaction.AddPreparedInsertListStatement<ShardPreparedStatement, AceObjectPropertiesAttribute2nd>(ShardPreparedStatement.InsertAceObjectPropertiesAttributes2nd, properties);
            return true;
        }

        // SaveAceObjectPropertiesSkill(transaction, aceObject.AceObjectPropertiesSkills);
        private bool SaveAceObjectPropertiesSkill(DatabaseTransaction transaction, uint aceObjectId, List<AceObjectPropertiesSkill> properties)
        {
            var critera = new Dictionary<string, object> { { "aceObjectId", aceObjectId} };
            transaction.AddPreparedDeleteListStatement<ShardPreparedStatement, AceObjectPropertiesSkill>(ShardPreparedStatement.DeleteAceObjectPropertiesSkills, critera);
            transaction.AddPreparedInsertListStatement<ShardPreparedStatement, AceObjectPropertiesSkill>(ShardPreparedStatement.InsertAceObjectPropertiesSkills, properties);
            return true;
        }
    }
}
