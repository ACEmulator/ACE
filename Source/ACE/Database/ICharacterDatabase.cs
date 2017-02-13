using ACE.Entity;
using ACE.Network;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ACE.Database
{
    public interface ICharacterDatabase
    {
        Task<Position> GetPosition(uint id);

        void DeleteOrRestore(ulong unixTime, uint id);

        Task<List<CachedCharacter>> GetByAccount(uint accountId);

        uint GetMaxId();

        bool IsNameAvailable(string name);
        
        Task<bool> CreateCharacter(Character character);

        Task UpdateCharacter(Character character);

        Task<Character> LoadCharacter(uint id);

        /// <summary>
        /// loads object properties into the provided db object
        /// </summary>
        void LoadCharacterProperties(DbObject dbObject);

        /// <summary>
        /// saves all object properties in the provided db object
        /// </summary>
        void SaveCharacterProperties(DbObject dbObject, Database.DatabaseTransaction transaction);
    }
}
