using System.Collections.Generic;
using System.Threading.Tasks;

using ACE.Entity;
using ACE.Entity.Enum;

namespace ACE.Database
{
    public interface ICharacterDatabase
    {
        Task<Position> GetPosition(uint id);

        CharacterPosition GetCharacterPosition(uint id, CharacterPositionType type);

        void DeleteOrRestore(ulong unixTime, uint id);

        Task<List<CachedCharacter>> GetByAccount(uint accountId);

        uint GetMaxId();

        bool IsNameAvailable(string name);
        
        Task<bool> CreateCharacter(Character character);

        Task UpdateCharacter(Character character);

        Task<Character> LoadCharacter(uint id);

        /// <summary>
        /// Loads a character by name.  Only the fields from the character table are loaded.
        /// </summary>
        Task<Character> GetCharacterByName(string name);

        Task DeleteFriend(uint characterId, uint friendCharacterId);
        Task AddFriend(uint characterId, uint friendCharacterId);
        Task RemoveAllFriends(uint characterId);
        
        /// <summary>
        /// loads object properties into the provided db object
        /// </summary>
        Task LoadCharacterProperties(DbObject dbObject);

        /// <summary>
        /// loads positons into the provided db object
        /// </summary>
        void LoadCharacterPositions(Character character);

        /// <summary>
        /// Saves character options (F11 tab)
        /// </summary>
        void SaveCharacterOptions(Character character);

        /// <summary>
        /// Saves the character positions
        /// </summary>
        void SaveCharacterPositions(Character character);

        /// <summary>
        /// saves all object properties in the provided db object
        /// </summary>
        void SaveCharacterProperties(DbObject dbObject, Database.DatabaseTransaction transaction);

        uint SetCharacterAccessLevelByName(string name, AccessLevel accessLevel);

        uint RenameCharacter(string currentName, string newName);
    }
}
