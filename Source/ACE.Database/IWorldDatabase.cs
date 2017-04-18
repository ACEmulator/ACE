using System.Collections.Generic;

using ACE.Entity;

namespace ACE.Database
{
    public interface IWorldDatabase
    {
        List<TeleportLocation> GetLocations();
		
        PortalDestination GetPortalDestination(uint weenieClassId);

        List<AceObject> GetObjectsByLandblock(ushort landblock);

        List<AceCreatureStaticLocation> GetCreaturesByLandblock(ushort landblock);

        AceCreatureObject GetCreatureDataByWeenie(uint weenieClassId);

        bool InsertStaticCreatureLocation(AceCreatureStaticLocation acsl);
    }
}
