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

        bool SaveObject(AceObject aceObject);

        bool DeleteObject(AceObject aceObject);

        AceObject GetObject(uint objId);
    }
}
