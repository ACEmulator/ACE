using System.Collections.Generic;

using ACE.Entity;

namespace ACE.Database
{
    public interface IWorldDatabase
    {
        List<TeleportLocation> GetLocations();

        AcePortalObject GetPortalObjectsByAceObjectId(uint aceObjectId);

        List<AceObject> GetObjectsByLandblock(ushort landblock);

        List<AceCreatureStaticLocation> GetCreaturesByLandblock(ushort landblock);

        List<AceCreatureGeneratorLocation> GetCreatureGeneratorsByLandblock(ushort landblock);

        AceCreatureObject GetCreatureDataByWeenie(uint weenieClassId);

        BaseAceObject GetBaseAceObjectDataByWeenie(uint weenieClassId);

        bool InsertStaticCreatureLocation(AceCreatureStaticLocation acsl);
    }
}
