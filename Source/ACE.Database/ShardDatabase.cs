using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ACE.Entity;
using ACE.Entity.Enum;

namespace ACE.Database
{
    public class ShardDatabase : Database, IShardDatabase
    {
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
            GetAcePositions = 23,
            GetAceAttributes = 24,
            // ReSharper disable once InconsistentNaming
            GetAceAttributes2nd = 25,
            GetAceObjectPropertiesSkill = 26,
            SaveAceObject = 27,
            SaveAceObjectPropertiesInt = 28,
            SaveAceObjectPropertiesBigInt = 29,
            SaveAceObjectPropertiesDouble = 30,
            SaveAceObjectPropertiesBool = 31,
            SaveAceObjectPropertiesString = 32,
            SaveAceObjectPropertiesDid = 33,
            SaveAceObjectPropertiesIid = 34,

            AddFriend = 101,
            DeleteFriend = 102,
            GetFriends = 103,
            DeleteAllFriends = 104,

            GetCharacters = 105,
            GetNextCharacterId = 106,
            IsCharacterNameAvailable = 107
            // keep on going, you get the idea
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
                ShardPreparedStatement.GetAceObjectPropertiesInt,
                typeof(AceObjectPropertiesInt),
                ConstructedStatementType.GetList);

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
                ShardPreparedStatement.GetAcePositions,
                typeof(Position),
                ConstructedStatementType.GetList);

            ConstructStatement(
                ShardPreparedStatement.GetAceAttributes,
                typeof(AceObjectPropertiesAttribute),
                ConstructedStatementType.GetList);

            ConstructStatement(
                ShardPreparedStatement.GetAceAttributes2nd,
                typeof(AceObjectPropertiesAttribute2nd),
                ConstructedStatementType.GetList);

            ConstructStatement(
                ShardPreparedStatement.GetAceObjectPropertiesSkill,
                typeof(AceObjectPropertiesSkill),
                ConstructedStatementType.GetList);

            ConstructStatement(
                ShardPreparedStatement.SaveAceObject,
                typeof(AceObject),
                ConstructedStatementType.Insert);

            ConstructStatement(
                ShardPreparedStatement.SaveAceObjectPropertiesInt,
                typeof(AceObjectPropertiesInt),
                ConstructedStatementType.Insert);

            ConstructStatement(
                ShardPreparedStatement.SaveAceObjectPropertiesBigInt,
                typeof(AceObjectPropertiesInt64),
                ConstructedStatementType.Insert);

            ConstructStatement(
                ShardPreparedStatement.SaveAceObjectPropertiesBool,
                typeof(AceObjectPropertiesBool),
                ConstructedStatementType.Insert);

            ConstructStatement(
                ShardPreparedStatement.SaveAceObjectPropertiesDouble,
                typeof(AceObjectPropertiesDouble),
                ConstructedStatementType.Insert);

            ConstructStatement(
                ShardPreparedStatement.SaveAceObjectPropertiesString,
                typeof(AceObjectPropertiesString),
                ConstructedStatementType.Insert);

            ConstructStatement(
                ShardPreparedStatement.SaveAceObjectPropertiesIid,
                typeof(AceObjectPropertiesInstanceId),
                ConstructedStatementType.Insert);

            ConstructStatement(
                ShardPreparedStatement.SaveAceObjectPropertiesDid,
                typeof(AceObjectPropertiesDataId),
                ConstructedStatementType.Insert);
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
            var objects = ExecuteConstructedGetListStatement<ShardPreparedStatement, Position>(ShardPreparedStatement.GetAcePositions, criteria);
            return objects.ToDictionary(x => x.PositionType, x => x);
        }

        private List<AceObjectPropertiesSkill> GetAceObjectPropertiesSkill(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "AceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<ShardPreparedStatement, AceObjectPropertiesSkill>(ShardPreparedStatement.GetAceObjectPropertiesSkill, criteria);
            return objects;
        }

        private List<AceObjectPropertiesAttribute> GetAceObjectPropertiesAttribute(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "AceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<ShardPreparedStatement, AceObjectPropertiesAttribute>(ShardPreparedStatement.GetAceAttributes, criteria);
            return objects;
        }

        // ReSharper disable once InconsistentNaming
        private List<AceObjectPropertiesAttribute2nd> GetAceObjectPropertiesAttribute2nd(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "AceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<ShardPreparedStatement, AceObjectPropertiesAttribute2nd>(ShardPreparedStatement.GetAceAttributes2nd, criteria);
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

        private bool SaveAceObjectPropertiesInt(List<AceObjectPropertiesInt> propList)
        {
            foreach (var pl in propList)
            {
                if (!ExecuteConstructedInsertStatement(ShardPreparedStatement.SaveAceObjectPropertiesInt, typeof(AceObjectPropertiesInt), pl))
                    return false;
            }
            return true;
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
            throw new NotImplementedException();
        }

        public bool SaveObject(AceObject aceObject)
        {
            // delete + save object
            if (!ExecuteConstructedInsertStatement(ShardPreparedStatement.SaveAceObject, typeof(AceObject), aceObject))
                return false;

            // delete properties first

            // save properties
            if (!SaveAceObjectPropertiesInt(aceObject.IntProperties))
                return false;

            // SaveAceObjectPropertiesBigInt(aceObject.Int64Properties);
            // SAveAceObjectPropertiesBool(aceObject.BoolProperties);
            // SaveAceObjectPropertiesDouble(aceObject.DoubleProperties);
            // SaveAceObjectPropertiesString(aceObject.StringProperties);
            // SaveAceObjectPropertiesIid(aceObject.InstanceIdProperties);
            // SaveAceObjectPropertiesDid(aceObject.DataIdProperties);
            // SaveAceObjectTextureMaps(aceObject.TextureOverrides);
            // SaveAceObjectAnimations(aceObject.AnimationOverrides);
            // SaveAceObjectPalettes(aceObject.PaletteOverrides);

            // save positions, skills and attributes
            // SaveAceObjectPostions(aceObject.Positions);
            // SaveAceObjectPropertiesAttribute(aceObject.AceObjectPropertiesAttributes);
            // SaveAceObjectPropertiesAttribute2nd(aceObject.AceObjectPropertiesAttributes2nd);
            // SaveAceObjectPropertiesSkill(aceObject.AceObjectPropertiesSkills);

            return true;
        }

        public uint SetCharacterAccessLevelByName(string name, AccessLevel accessLevel)
        {
            throw new NotImplementedException();
        }
    }
}
