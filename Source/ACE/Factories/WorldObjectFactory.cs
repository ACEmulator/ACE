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
                    WorldObject wo = CreateWorldObject(aceO);
                    if (wo != null)
                        results.Add(wo);
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
                case WeenieType.Container:
                    return new Container(aceO);
                // case WeenieType.PKModifier:
                //    return new PKModifier(aceO);
                case WeenieType.Cow:
                    return new Cow(aceO);
                case WeenieType.Creature:
                    return new Creature(aceO);
                case WeenieType.Container:
                    return new Container(aceO);
                default:
                    return new GenericObject(aceO);
            }
        }

        public static WorldObject CreateWorldObject(uint weenieId)
        {
            AceObject aceObject = DatabaseManager.World.GetAceObjectByWeenie(weenieId);

            return CreateWorldObject(aceObject);
        }

        public static WorldObject CreateWorldObject(uint weenieId, ObjectGuid guid)
        {
            AceObject aceObject = (AceObject)DatabaseManager.World.GetAceObjectByWeenie(weenieId).Clone(guid.Full);

            return CreateWorldObject(aceObject);
        }

        public static WorldObject CreateNewWorldObject(uint weenieId)
        {
            WorldObject wo = CreateWorldObject(weenieId, GuidManager.NewItemGuid());

            return wo;
        }

        public static WorldObject CreateNewWorldObject(uint weenieId, ObjectGuid guid)
        {
			AceObject aceObject = (AceObject)DatabaseManager.World.GetAceObjectByWeenie(weenieId).Clone(GuidManager.NewItemGuid().Full);
            WorldObject wo = CreateWorldObject(weenieId, guid);
            return wo;
        }
    }
}
