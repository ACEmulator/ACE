using System.Collections.Generic;

using ACE.Entity;

namespace ACE.Database
{
    /// <summary>
    /// methods common to the world database and the shard database(s)
    /// </summary>
    public interface ICommonDatabase
    {
        List<AceObject> GetObjectsByLandblock(ushort landblock);

        AceObject GetObject(uint aceObjectId);

        bool SaveObject(AceObject aceObject);
    }
}
