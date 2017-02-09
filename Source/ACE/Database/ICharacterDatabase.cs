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
        
        Task CreateCharacter(Character character);

        Task UpdateCharacter(Character character);

        Task<Character> LoadCharacter(uint id);
    }
}
