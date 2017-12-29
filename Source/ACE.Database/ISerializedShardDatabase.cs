using ACE.Entity;
using ACE.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ACE.Database
{
    public interface ISerializedShardDatabase
    {
        Task<List<AceObject>> GetObjectsByLandblock(ushort landblock);

        Task<AceObject> GetObject(uint aceObjectId);

        Task<bool> SaveObject(AceObject aceObject);

        Task<bool> DeleteObject(AceObject aceObject);

        Task<List<CachedCharacter>> GetCharacters(uint subscriptionId);

        /// <summary>
        /// Loads an object by name.  Primary use case: characters.
        /// </summary>
        Task<ObjectInfo> GetObjectInfoByName(string name);

        Task DeleteFriend(uint characterId, uint friendCharacterId);

        Task<bool> DeleteContract(AceContractTracker contract);

        Task AddFriend(uint characterId, uint friendCharacterId);

        Task RemoveAllFriends(uint characterId);

        Task<bool> IsCharacterNameAvailable(string name);

        Task<bool> DeleteOrRestore(ulong unixTime, uint id);

        Task<bool> DeleteCharacter(uint id);

        Task<uint> GetCurrentId(uint min, uint max);

        Task<uint> SetCharacterAccessLevelByName(string name, AccessLevel accessLevel);

        Task<uint> RenameCharacter(string currentName, string newName);

        Task<AceCharacter> GetCharacter(uint id);
    }
}
