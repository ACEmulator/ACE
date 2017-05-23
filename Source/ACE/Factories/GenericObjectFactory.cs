using ACE.Database;
using ACE.Entity;
using ACE.Entity.Enum;
using System.Collections.Generic;

namespace ACE.Factories
{
    public class GenericObjectFactory
    {
        public static List<WorldObject> CreateWorldObjects(List<AceObject> sourceObjects)
        {
            var results = new List<WorldObject>();

            foreach (var aceO in sourceObjects)
            {
                var ot = (ObjectType)aceO.ItemType;
                var oDescFlag = (ObjectDescriptionFlag)aceO.AceObjectDescriptionFlags;

                if ((oDescFlag & ObjectDescriptionFlag.LifeStone) != 0)
                {
                    results.Add(new Lifestone(aceO));
                    continue;
                }
                if ((oDescFlag & ObjectDescriptionFlag.Portal) != 0)
                {
                    var acePO = DatabaseManager.World.GetPortalObjectsByAceObjectId(aceO.AceObjectId);
                    results.Add(new Portal(acePO));
                    continue;
                }
                if ((oDescFlag & ObjectDescriptionFlag.Door) != 0)
                {
                    results.Add(new Door(aceO));
                    continue;
                }

                switch (ot)
                {
#if DEBUG
                    default:
                        // Use the DebugObject to assist in building proper objects for weenies
                        results.Add(new DebugObject(aceO));
                        break;
#endif
                }
            }

            return results;
        }
    }
}
