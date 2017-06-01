using ACE.Entity;
using ACE.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Database
{
    public interface IShardDatabase : ICommonDatabase
    {
        Task<List<CachedCharacter>> GetCharacters(uint accountId);

        /// <summary>
        /// Loads an object by name.  Primary use case: characters.
        /// </summary>
        Task<ObjectInfo> GetObjectInfoByName(string name);
        
        Task DeleteFriend(uint characterId, uint friendCharacterId);

        Task AddFriend(uint characterId, uint friendCharacterId);

        Task RemoveAllFriends(uint characterId);

        bool IsCharacterNameAvailable(string name);

        void DeleteOrRestore(ulong unixTime, uint id);

        /// <summary>
        /// Saves character options (F11 tab)
        /// </summary>
        void SaveCharacterOptions(AceCharacter character);

        uint SetCharacterAccessLevelByName(string name, AccessLevel accessLevel);

        uint RenameCharacter(string currentName, string newName);

        uint GetNextCharacterId();
    }
}
