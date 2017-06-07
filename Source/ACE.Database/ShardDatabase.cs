﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            // fetch all common data
            // fetch the base object from ace_object - this has little/nothing more than a weenie id and some flags
            // fetch all properties and load them in (use Properties object)
            // fetch all positions, load them in (use Position object)
            // fetch primary attributes (need object)
            // fetch secondary attributes (need object)
            // fetch palette / animation / texture overrides
            aceObject.IntProperties = GetAceObjectPropertiesInt(aceObject.AceObjectId);
            aceObject.Int64Properties = GetAceObjectPropertiesBigInt(aceObject.AceObjectId);
            aceObject.BoolProperties = GetAceObjectPropertiesBool(aceObject.AceObjectId);
            aceObject.DoubleProperties = GetAceObjectPropertiesDouble(aceObject.AceObjectId);
            aceObject.InstanceIdProperties = GetAceObjectPropertiesIid(aceObject.AceObjectId);
            aceObject.DataIdProperties = GetAceObjectPropertiesDid(aceObject.AceObjectId);
            aceObject.TextureOverrides = GetAceObjectTextureMaps(aceObject.AceObjectId);
            aceObject.AnimationOverrides = GetAceObjectAnimations(aceObject.AceObjectId);
            aceObject.PaletteOverrides = GetAceObjectPalettes(aceObject.AceObjectId);

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
            throw new NotImplementedException();
        }

        public bool SaveObject(AceObject aceObject)
        {
            throw new NotImplementedException();
        }

        public uint SetCharacterAccessLevelByName(string name, AccessLevel accessLevel)
        {
            throw new NotImplementedException();
        }
    }
}
