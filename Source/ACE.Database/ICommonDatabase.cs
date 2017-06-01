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

        /// <summary>
        /// todo: deprecate this.  we dont' want the data layer creating creatures
        /// from weenies.  that's up the call stack from here
        /// </summary>
        AceCreatureObject GetCreatureDataByWeenie(uint weenieClassId);

        AceObject GetObject(uint aceObjectId);

        bool SaveObject(AceObject aceObject);

        BaseAceObject GetWeenie(uint weenieClassId);

        BaseAceObject GetRandomWeenieOfType(uint typeId);

        bool InsertStaticCreatureLocation(AceCreatureStaticLocation acsl);
    }
}
