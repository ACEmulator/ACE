using ACE.Entity;
using ACE.Entity.Enum;
using System.Collections.Generic;

namespace ACE.Database
{
    internal interface IShardDatabase : ICommonDatabase
    {
        List<CachedCharacter> GetCharacters(uint accountId);

        /// <summary>
        /// Loads an object by name.  Primary use case: characters.
        /// </summary>
        ObjectInfo GetObjectInfoByName(string name);

        void DeleteFriend(uint characterId, uint friendCharacterId);

        void AddFriend(uint characterId, uint friendCharacterId);

        void RemoveAllFriends(uint characterId);

        bool IsCharacterNameAvailable(string name);

        bool DeleteOrRestore(ulong unixTime, uint id);

        bool DeleteCharacter(uint id);

        uint SetCharacterAccessLevelByName(string name, AccessLevel accessLevel);

        uint RenameCharacter(string currentName, string newName);

        uint GetNextCharacterId();

        uint GetMaxPlayerId();

        AceCharacter GetCharacter(uint id);

        bool SaveObject(AceObject aceObject);
    }
}
