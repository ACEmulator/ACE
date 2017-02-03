using ACE.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        void CreateCharacter(uint id, uint accountId, string name, uint templateOption, uint startArea, bool isAdmin, bool isEnvoy);
    }
}
