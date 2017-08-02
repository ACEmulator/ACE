using ACE.Database;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Managers;
using System.Collections.Generic;

namespace ACE.Factories
{
    public class WorldObjectFactory
    {
        public static List<WorldObject> CreateWorldObjects(List<AceObject> sourceObjects)
        {
            var results = new List<WorldObject>();

            foreach (var aceO in sourceObjects)
            {
                if (aceO.GeneratorStatus ?? false)  // Generator
                {
                    aceO.Location = aceO.Location.InFrontOf(-2.0);
                    aceO.Location.PositionZ = aceO.Location.PositionZ - 0.5f;
                    results.Add(new Generator(new ObjectGuid(aceO.AceObjectId), aceO));
                    aceO.GeneratorEnteredWorld = true;
                    var objectList = GeneratorFactory.CreateWorldObjectsFromGenerator(aceO) ?? new List<WorldObject>();
                    objectList.ForEach(o => results.Add(o));
                    continue;
                }

                if (aceO.Location != null)
                {
                    ////WorldObject wo = new GenericObject(aceO);

                    results.Add(CreateWorldObject(aceO));
                }
            }
            return results;
        }

        public static WorldObject CreateWorldObject(AceObject aceO)
        {
            WeenieType objWeenieType = (WeenieType?)aceO.WeenieType ?? WeenieType.Generic;

            switch (objWeenieType)
            {
                case WeenieType.LifeStone:
                    return new Lifestone(aceO);
                case WeenieType.Door:
                    return new Door(aceO);
                case WeenieType.Portal:
                    return new Portal(aceO);
                // case WeenieType.PKModifier:
                //    return new PKModifier(AceObject);
                default:
                    return new GenericObject(aceO);
            }
        }

        public static WorldObject CreateNewWorldObject(uint weenieId)
        {
            AceObject aceObject = (AceObject)DatabaseManager.World.GetAceObjectByWeenie(weenieId).Clone(GuidManager.NewItemGuid().Full);
            WorldObject wo = CreateWorldObject(aceObject);
            return wo;
        }
    }
}
