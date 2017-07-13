using ACE.Database;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
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
                var oType = (ItemType)aceO.GetIntProperty(PropertyInt.ItemType);
                var oDescFlag = (ObjectDescriptionFlag)aceO.AceObjectDescriptionFlags;

                if (aceO.GetBoolProperty(PropertyBool.GeneratorStatus) ?? false)  // Generator
                {
                    aceO.SetPosition(PositionType.Location, aceO.GetPosition(PositionType.Location).InFrontOf(-2.0));
                    aceO.GetPosition(PositionType.Location).PositionZ = aceO.GetPosition(PositionType.Location).PositionZ - 0.5f;
                    results.Add(new Generator(new ObjectGuid(aceO.AceObjectId), aceO));
                    aceO.SetBoolProperty(PropertyBool.GeneratorEnteredWorld, true);
                    var objectList = GeneratorFactory.CreateWorldObjectsFromGenerator(aceO) ?? new List<WorldObject>();
                    objectList.ForEach(o => results.Add(o));
                    continue;
                }

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

                switch (oType)
                {
#if DEBUG
                    default:
                        // Use the DebugObject to assist in building proper objects for weenies
                        // FIXME(ddevec): Some objects have null location.  This freaks out the landblock... ignore them?
                        if (aceO.GetPosition(PositionType.Location) != null)
                        {
                            results.Add(new DebugObject(aceO));
                        }
                        break;
#endif
                }
            }
            return results;
        }
    }
}
