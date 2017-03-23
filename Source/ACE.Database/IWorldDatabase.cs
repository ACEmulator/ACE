using System.Collections.Generic;

using ACE.Entity;

namespace ACE.Database
{
    public interface IWorldDatabase
    {
        List<TeleportLocation> GetLocations();

        List<AceObject> GetObjectsByLandblock(ushort landblock);
    }
}
