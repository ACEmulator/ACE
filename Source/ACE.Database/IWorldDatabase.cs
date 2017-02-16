using ACE.Entity;
using System.Collections.Generic;

namespace ACE.Database
{
    public interface IWorldDatabase
    {
        List<TeleportLocation> GetLocations();
    }
}
