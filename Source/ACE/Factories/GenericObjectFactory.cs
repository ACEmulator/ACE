using ACE.Database;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Managers;
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
                   results.Add(new Portal(aceO));
                   continue;
                }
                if ((oDescFlag & ObjectDescriptionFlag.Door) != 0)
                {
                    results.Add(new Door(aceO));
                    continue;
                }

                switch (ot)
                {
                    case 0: // Generator
                        // TODO: Is this the best way to figure out what's a generator and whats not? more research needed
                        if ((oDescFlag & (ObjectDescriptionFlag.Attackable | ObjectDescriptionFlag.Stuck | ObjectDescriptionFlag.Hidden)) != 0)
                        {
                            aceO.Location = aceO.Location.InFrontOf(-2.0);
                            aceO.Location.PositionZ = aceO.Location.PositionZ - 0.5f;
                            results.Add(new Generator(new ObjectGuid(GuidManager.NewItemGuid()), aceO));
                            var objectList = GeneratorFactory.CreateWorldObjectsFromGenerator(aceO) ?? new List<WorldObject>();
                            objectList.ForEach(o => results.Add(o));
                        }
                        break;
#if DEBUG
                    default:
                        // Use the DebugObject to assist in building proper objects for weenies
                        // FIXME(ddevec): Some objects have null location.  This freaks out the landblock... ignore them?
                        if (aceO.Location != null)
                            results.Add(new DebugObject(aceO));
                        break;
#endif
                }
            }

            return results;
        }
    }
}
