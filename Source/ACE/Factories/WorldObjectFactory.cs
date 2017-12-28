using ACE.Database;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Managers;
using System.Collections.Generic;
using System;

namespace ACE.Factories
{
    public class WorldObjectFactory
    {
        public static List<WorldObject> CreateWorldObjects(List<AceObject> sourceObjects)
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
                    var objectList = GeneratorFactory.CreateWorldObjectsFromGenerator(aceO) ?? new List<WorldObject>();
                    objectList.ForEach(o => results.Add(o));
                    continue;
                }

                if (aceO.Location != null)
                {
                    WorldObject wo = CreateWorldObject(aceO);
                    if (wo != null)
                        results.Add(wo);
                    // TODO: this is a hack job. Remove this and do it right. 
                    foreach (var item in wo.WieldList)
                    {
                        WorldObject wo2 = CreateNewWorldObject(item.WeenieClassId);
                        wo2.Location = wo.Location;
                        wo2.CurrentWieldedLocation = wo.ValidLocations;
                        wo2.WielderId = wo.Guid.Full;
                        results.Add(wo2);
                    }
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
                case WeenieType.Book:
                    return new Book(aceO);
                // case WeenieType.PKModifier:
                //    return new PKModifier(aceO);
                case WeenieType.Cow:
                    return new Cow(aceO);
                case WeenieType.Creature:
                    return new Creature(aceO);
                case WeenieType.Container:
                    return new Container(aceO);
                case WeenieType.Scroll:
                    return new Scroll(aceO);
                case WeenieType.Vendor:
                    return new Vendor(aceO);
                case WeenieType.Coin:
                    return new Coin(aceO);
                case WeenieType.Key:
                    return new Key(aceO);
                case WeenieType.Food:
                    return new Food(aceO);
                case WeenieType.Gem:
                    return new Gem(aceO);
                case WeenieType.Game:
                    return new Game(aceO);
                case WeenieType.GamePiece:
                    return new GamePiece(aceO);
                case WeenieType.Clothing:
                    return new Clothing(aceO);
                case WeenieType.Missile:
                    return new Missile(aceO);
                case WeenieType.Ammunition:
                    return new Ammunition(aceO);
                case WeenieType.MeleeWeapon:
                    return new MeleeWeapon(aceO);
                case WeenieType.Caster:
                    return new Caster(aceO);
                default:
                    return new GenericObject(aceO);
            }
        }

        public static WorldObject CreateCorpse(Creature cs)
        {
            ////Console.WriteLine("Inside CreateCorpse");
            WorldObject wo = CreateNewWorldObject(1217);
            ////Console.WriteLine("Added location to corpse");
            wo.Location = cs.Location;
            return wo;
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
            Console.WriteLine("Creating new world object");
            WorldObject wo = CreateWorldObject(weenieId, GuidManager.NewItemGuid());
            return wo;
        }
    }
}
