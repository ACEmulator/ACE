using System.Collections.Generic;
using System.Threading.Tasks;

using ACE.Database;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Managers;

namespace ACE.Factories
{
    public class WorldObjectFactory
    {
        public static async Task<List<WorldObject>> CreateWorldObjects(List<AceObject> sourceObjects)
        {
            var results = new List<WorldObject>();

            foreach (var aceO in sourceObjects)
            {
                // FIXME: This generator part is all wrong. Needs overhaul.
                if (aceO.GeneratorStatus ?? false)  // Generator
                {
                    aceO.Location = aceO.Location.InFrontOf(-2.0);
                    aceO.Location.PositionZ = aceO.Location.PositionZ - 0.5f;
                    results.Add(new Generator(new ObjectGuid(aceO.AceObjectId), aceO));
                    aceO.GeneratorEnteredWorld = true;
                    var objectList = await GeneratorFactory.CreateWorldObjectsFromGenerator(aceO) ?? new List<WorldObject>();
                    objectList.ForEach(o => results.Add(o));
                    continue;
                }

                if (aceO.Location != null)
                {
                    WorldObject wo = await CreateWorldObject(aceO);
                    if (wo != null)
                        results.Add(wo);
                    // TODO: this is a hack job. Remove this and do it right. 
                    foreach (var item in wo.WieldList)
                    {
                        WorldObject wo2 = await CreateNewWorldObject(item.WeenieClassId);
                        wo2.Location = wo.Location;
                        wo2.CurrentWieldedLocation = wo.ValidLocations;
                        wo2.WielderId = wo.Guid.Full;
                        results.Add(wo2);
                    }
                }
            }
            return results;
        }

        public static async Task<WorldObject> CreateWorldObject(AceObject aceO)
        {
            WeenieType objWeenieType = (WeenieType?)aceO.WeenieType ?? WeenieType.Generic;

            switch (objWeenieType)
            {
                case WeenieType.LifeStone:
                    return await WorldObject.CreateWorldObject<Lifestone>(aceO);
                case WeenieType.Door:
                    return await WorldObject.CreateWorldObject<Door>(aceO);
                case WeenieType.Portal:
                    return await WorldObject.CreateWorldObject<Portal>(aceO);
                case WeenieType.Book:
                    return await WorldObject.CreateWorldObject<Book>(aceO);
                // case WeenieType.PKModifier:
                //    return new PKModifier(aceO);
                case WeenieType.Cow:
                    return await WorldObject.CreateWorldObject<Cow>(aceO);
                case WeenieType.Creature:
                    return await WorldObject.CreateWorldObject<Creature>(aceO);
                case WeenieType.Container:
                    return await WorldObject.CreateWorldObject<Container>(aceO);
                case WeenieType.Scroll:
                    return await WorldObject.CreateWorldObject<Scroll>(aceO);
                case WeenieType.Vendor:
                    return await WorldObject.CreateWorldObject<Vendor>(aceO);
                case WeenieType.Coin:
                    return await WorldObject.CreateWorldObject<Coin>(aceO);
                case WeenieType.Key:
                    return await WorldObject.CreateWorldObject<Key>(aceO);
                case WeenieType.Food:
                    return await WorldObject.CreateWorldObject<Food>(aceO);
                case WeenieType.Gem:
                    return await WorldObject.CreateWorldObject<Gem>(aceO);
                case WeenieType.Game:
                    return await WorldObject.CreateWorldObject<Game>(aceO);
                case WeenieType.GamePiece:
                    return await WorldObject.CreateWorldObject<GamePiece>(aceO);
                default:
                    return await WorldObject.CreateWorldObject<GenericObject>(aceO);
            }
        }

        public static async Task<WorldObject> CreateWorldObject(uint weenieId)
        {
            AceObject aceObject = await DatabaseManager.World.GetAceObjectByWeenie(weenieId);

            return await CreateWorldObject(aceObject);
        }

        public static async Task<WorldObject> CreateWorldObject(uint weenieId, ObjectGuid guid)
        {
            AceObject aceObject = (AceObject)(await DatabaseManager.World.GetAceObjectByWeenie(weenieId)).Clone(guid.Full);
            return await CreateWorldObject(aceObject);
        }

        public static async Task<WorldObject> CreateNewWorldObject(uint weenieId)
        {
            WorldObject wo = await CreateWorldObject(weenieId, GuidManager.NewItemGuid());
            return wo;
        }
    }
}
