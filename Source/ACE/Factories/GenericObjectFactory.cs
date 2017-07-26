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
                ////var oType = (ItemType)aceO.ItemType;
                ////var oDescFlag = (ObjectDescriptionFlag)aceO.AceObjectDescriptionFlags;

                ////WeenieType objWeenieType = (WeenieType)(aceO.WeenieType ?? (uint)WeenieType.Generic);

                if (aceO.GeneratorStatus ?? false)  // Generator
                {                    
                    ////aceO.Location = aceO.Location.InFrontOf(-2.0);
                    ////aceO.Location.PositionZ = aceO.Location.PositionZ - 0.5f;
                    ////results.Add(new Generator(new ObjectGuid(aceO.AceObjectId), aceO));
                    ////aceO.GeneratorEnteredWorld = true;
                    ////var objectList = GeneratorFactory.CreateWorldObjectsFromGenerator(aceO) ?? new List<WorldObject>();
                    ////objectList.ForEach(o => results.Add(o));
                    continue;
                }

                ////switch (objWeenieType)
                ////{
                ////    case WeenieType.LifeStone:
                ////        results.Add(new Lifestone(aceO));
                ////        continue;
                ////    case WeenieType.Door:
                ////        results.Add(new Door(aceO));
                ////        continue;
                ////    case WeenieType.Portal:
                ////        results.Add(new Portal(aceO));
                ////        continue;
                ////    default:
                if (aceO.Location != null)
                {
                    ////            results.Add(new Generic(aceO));
                    WorldObject wo = new Generic(aceO);

                    results.Add(wo.GetObjectFromWeenieType());
                }
                ////        break;
                ////}

                ////                if ((oDescFlag & ObjectDescriptionFlag.LifeStone) != 0)
                ////                {
                ////                    results.Add(new Lifestone(aceO));
                ////                    continue;
                ////                }
                ////                if ((oDescFlag & ObjectDescriptionFlag.Portal) != 0)
                ////                {
                ////                   results.Add(new Portal(aceO));
                ////                   continue;
                ////                }
                ////                if ((oDescFlag & ObjectDescriptionFlag.Door) != 0)
                ////                {
                ////                    results.Add(new Door(aceO));
                ////                    continue;
                ////                }

                ////                switch (oType)
                ////                {
                ////#if DEBUG
                ////                    default:
                ////                        // Use the DebugObject to assist in building proper objects for weenies
                ////                        // FIXME(ddevec): Some objects have null location.  This freaks out the landblock... ignore them?
                ////                        if (aceO.Location != null)
                ////                        {
                ////                            results.Add(new DebugObject(aceO));
                ////                        }
                ////                        break;
                ////#endif
                ////                }
            }
                return results;
        }

        ////public object GetObjectFromWeenieType(AceObject ao)
        ////{
        ////    WeenieType objWeenieType = (WeenieType)(ao.WeenieType ?? (uint)WeenieType.Generic);

        ////    ////if (aceO.GeneratorStatus ?? false)  // Generator
        ////    ////{
        ////    ////    ////aceO.Location = aceO.Location.InFrontOf(-2.0);
        ////    ////    ////aceO.Location.PositionZ = aceO.Location.PositionZ - 0.5f;
        ////    ////    ////results.Add(new Generator(new ObjectGuid(aceO.AceObjectId), aceO));
        ////    ////    ////aceO.GeneratorEnteredWorld = true;
        ////    ////    ////var objectList = GeneratorFactory.CreateWorldObjectsFromGenerator(aceO) ?? new List<WorldObject>();
        ////    ////    ////objectList.ForEach(o => results.Add(o));
        ////    ////    continue;
        ////    ////}

        ////    switch (objWeenieType)
        ////    {
        ////        case WeenieType.LifeStone:
        ////            return new Lifestone(ao);
        ////        ////break;
        ////        case WeenieType.Door:
        ////            return new Door(ao);
        ////        ////break;
        ////        case WeenieType.Portal:
        ////            return new Portal(ao);
        ////        ////break;
        ////        default:
        ////            ////if (aceO.Location != null)
        ////            ////{
        ////            return new Generic(ao);
        ////            ////}
        ////            ////break;
        ////    }
        ////}
    }
}
