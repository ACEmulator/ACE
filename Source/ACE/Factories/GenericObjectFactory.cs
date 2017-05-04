using ACE.Database;
using ACE.Entity;
using ACE.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Factories
{
    public class GenericObjectFactory
    {
        public static List<WorldObject> CreateWorldObjects(List<AceObject> sourceObjects)
        {
            List<WorldObject> results = new List<WorldObject>();

            foreach (var aceO in sourceObjects)
            {
                ObjectType ot = (ObjectType)aceO.TypeId;
                ObjectDescriptionFlag oDescFlag = (ObjectDescriptionFlag)aceO.WdescBitField;

                if ((oDescFlag & ObjectDescriptionFlag.LifeStone) != 0)
                {
                    results.Add(new Lifestone(aceO));
                    continue;
                }
                else if ((oDescFlag & ObjectDescriptionFlag.Portal) != 0)
                {
                    results.Add(new Portal(aceO as AcePortalObject));
                    continue;
                }
                else if ((oDescFlag & ObjectDescriptionFlag.Door) != 0)
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
