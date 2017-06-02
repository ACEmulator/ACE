using System;
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
            // keep on going, you get the idea
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

        public Task<List<CachedCharacter>> GetCharacters(uint accountId)
        {
            throw new NotImplementedException();
        }
        
        public uint GetNextCharacterId()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
