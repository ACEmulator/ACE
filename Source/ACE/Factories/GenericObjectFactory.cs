////using ACE.Database;
////using ACE.Entity;
////using ACE.Entity.Enum;
////using ACE.Entity.Enum.Properties;
////using ACE.Managers;
////using System.Collections.Generic;

////namespace ACE.Factories
////{
////    public class GenericObjectFactory
////    {
////        public static List<WorldObject> CreateWorldObjects(List<AceObject> sourceObjects)
////        {
////            var results = new List<WorldObject>();

////            foreach (var aceO in sourceObjects)
////            {
////                if (aceO.GeneratorStatus ?? false)  // Generator
////                {
////                    aceO.Location = aceO.Location.InFrontOf(-2.0);
////                    aceO.Location.PositionZ = aceO.Location.PositionZ - 0.5f;
////                    results.Add(new Generator(new ObjectGuid(aceO.AceObjectId), aceO));
////                    aceO.GeneratorEnteredWorld = true;
////                    var objectList = GeneratorFactory.CreateWorldObjectsFromGenerator(aceO) ?? new List<WorldObject>();
////                    objectList.ForEach(o => results.Add(o));
////                    continue;
////                }

////                if (aceO.Location != null)
////                {
////                    WorldObject wo = new GenericObject(aceO);

////                    results.Add(wo.GetObjectFromWeenieType());
////                }
////            }
////                return results;
////        }
////    }
////}
