using System.Collections.Generic;

using ACE.Entity;
using ACE.Entity.Enum;

namespace ACE.Database
{
    internal interface IShardDatabase : ICommonDatabase
    {
        List<CachedCharacter> GetCharacters(uint subscriptionId);

        /// <summary>
        /// Loads an object by name.  Primary use case: characters.
        /// </summary>
        ObjectInfo GetObjectInfoByName(string name);

        void DeleteFriend(uint characterId, uint friendCharacterId);

        bool DeleteContract(AceContractTracker contract);

        void AddFriend(uint characterId, uint friendCharacterId);

        void RemoveAllFriends(uint characterId);

        bool IsCharacterNameAvailable(string name);

        bool DeleteOrRestore(ulong unixTime, uint id);

        bool DeleteCharacter(uint id);

        uint SetCharacterAccessLevelByName(string name, AccessLevel accessLevel);

        uint RenameCharacter(string currentName, string newName);

        uint GetCurrentId(uint min, uint max);

        AceCharacter GetCharacter(uint id);
    }
}
