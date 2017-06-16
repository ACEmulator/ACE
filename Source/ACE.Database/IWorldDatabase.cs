using ACE.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Database
{
    public interface IWorldDatabase : ICommonDatabase
    {
        List<TeleportLocation> GetPointsOfInterest();

        List<AceCreatureStaticLocation> GetCreaturesByLandblock(ushort landblock);

        List<AceCreatureGeneratorLocation> GetCreatureGeneratorsByLandblock(ushort landblock);

        bool InsertStaticCreatureLocation(AceCreatureStaticLocation acsl);

        AceObject GetRandomWeenieOfType(uint typeId);

        AceObject GetBaseAceObjectDataByWeenie (uint weenieClassId);
    }
}
