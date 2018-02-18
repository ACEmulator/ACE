using System.Collections.Generic;

using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Managers;
using ACE.Server.WorldObjects;

namespace ACE.Server.Factories
{
    public static class WorldObjectFactory
    {
        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public static WorldObject CreateWorldObject(Weenie weenie, ObjectGuid guid)
        {
            var objWeenieType = (WeenieType)weenie.Type;

            switch (objWeenieType)
            {
                case WeenieType.LifeStone:
                    return new Lifestone(weenie, guid);
                case WeenieType.Door:
                    return new Door(weenie, guid);
                case WeenieType.Portal:
                    return new Portal(weenie, guid);
                case WeenieType.Book:
                    return new Book(weenie, guid);
                // case WeenieType.PKModifier:
                //    return new PKModifier(weenie, guid);
                case WeenieType.Cow:
                    return new Cow(weenie, guid);
                case WeenieType.Creature:
                    return new Creature(weenie, guid);
                case WeenieType.Container:
                    return new Container(weenie, guid);
                case WeenieType.Scroll:
                    return new Scroll(weenie, guid);
                case WeenieType.Vendor:
                    return new Vendor(weenie, guid);
                case WeenieType.Coin:
                    return new Coin(weenie, guid);
                case WeenieType.Key:
                    return new Key(weenie, guid);
                case WeenieType.Food:
                    return new Food(weenie, guid);
                case WeenieType.Gem:
                    return new Gem(weenie, guid);
                case WeenieType.Game:
                    return new Game(weenie, guid);
                case WeenieType.GamePiece:
                    return new GamePiece(weenie, guid);
                case WeenieType.AllegianceBindstone:
                    return new Bindstone(weenie, guid);
                case WeenieType.Clothing:
                    return new Clothing(weenie, guid);
                default:
                    return new GenericObject(weenie, guid);
            }
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public static WorldObject CreateWorldObject(Biota biota)
        {
            var objWeenieType = (WeenieType)biota.WeenieType;

            switch (objWeenieType)
            {
                case WeenieType.LifeStone:
                    return new Lifestone(biota);
                case WeenieType.Door:
                    return new Door(biota);
                case WeenieType.Portal:
                    return new Portal(biota);
                case WeenieType.Book:
                    return new Book(biota);
                // case WeenieType.PKModifier:
                //    return new PKModifier(biota);
                case WeenieType.Cow:
                    return new Cow(biota);
                case WeenieType.Creature:
                    return new Creature(biota);
                case WeenieType.Container:
                    return new Container(biota);
                case WeenieType.Scroll:
                    return new Scroll(biota);
                case WeenieType.Vendor:
                    return new Vendor(biota);
                case WeenieType.Coin:
                    return new Coin(biota);
                case WeenieType.Key:
                    return new Key(biota);
                case WeenieType.Food:
                    return new Food(biota);
                case WeenieType.Gem:
                    return new Gem(biota);
                case WeenieType.Game:
                    return new Game(biota);
                case WeenieType.GamePiece:
                    return new GamePiece(biota);
                case WeenieType.AllegianceBindstone:
                    return new Bindstone(biota);
                case WeenieType.Clothing:
                    return new Clothing(biota);
                default:
                    return new GenericObject(biota);
            }
        }

        /// <summary>
        /// This will create a list of WorldObjects, all with new GUIDs and for every position provided.
        /// </summary>
        public static List<WorldObject> CreateWorldObjects(Dictionary<Weenie, List<LandblockInstances>> sourceObjects)
        {
            var results = new List<WorldObject>();

            foreach (var kvp in sourceObjects)
            {
                foreach (var landblockInstance in kvp.Value)
                {
                    ObjectGuid guid;

                    if (landblockInstance.Guid != 0)
                        guid = new ObjectGuid(landblockInstance.Guid);
                    else
                        guid = GuidManager.NewDynamicGuid();

                    var worldObject = CreateWorldObject(kvp.Key, guid);

                    worldObject.SetPosition(PositionType.Location, new Position(landblockInstance.ObjCellId, landblockInstance.OriginX, landblockInstance.OriginY, landblockInstance.OriginZ, landblockInstance.AnglesX, landblockInstance.AnglesY, landblockInstance.AnglesZ, landblockInstance.AnglesW));

                    // todo what about LinkSlot and LinkController? See below commented out code CreateWorldObjects

                    results.Add(worldObject);
                }
            }

            return results;
        }


        /// <summary>
        /// This will create a new WorldObject with a new GUID.
        /// </summary>
        public static WorldObject CreateNewWorldObject(Weenie weenie)
        {
            var worldObject = CreateWorldObject(weenie, GuidManager.NewDynamicGuid());

            return worldObject;
        }

        /// <summary>
        /// This will create a new WorldObject with a new GUID.
        /// It will return null if weenieClassId was not found.
        /// </summary>
        public static WorldObject CreateNewWorldObject(uint weenieClassId)
        {
            var weenie = DatabaseManager.World.GetCachedWeenie(weenieClassId);

            if (weenie == null)
                return null;

            return CreateNewWorldObject(weenie);
        }

        /// <summary>
        /// This will create a new WorldObject with a new GUID.
        /// It will return null if weenieClassName was not found.
        /// </summary>
        public static WorldObject CreateNewWorldObject(string weenieClassName)
        {
            var classId = DatabaseManager.World.GetWeenieClassId(weenieClassName);

            if (classId == 0)
                return null;

            return CreateNewWorldObject(classId);
        }





        // todo DO WE NEED TO INCORP THIS CODE INTO THE NEW ENTITY FRAMEWORK MODEL?
        // I'm not sure aobut the Link stuff
        private static List<WorldObject> CreateWorldObjects(List<AceObject> sourceObjects)
        {
            throw new System.NotImplementedException();
            /*var results = new List<WorldObject>();

            var linkSourceResults = new List<AceObject>();
            var linkResults = new Dictionary<int, List<AceObject>>();

            foreach (var aceO in sourceObjects)
            {
                if (aceO.LinkSlot > 0)
                {
                    if (aceO.LinkSource ?? false)
                    {
                        linkSourceResults.Add(aceO);
                        continue;
                    }

                    if (!linkResults.ContainsKey((int)aceO.LinkSlot))
                        linkResults.Add((int)aceO.LinkSlot, new List<AceObject>());
                    linkResults[(int)aceO.LinkSlot].Add(aceO);
                    continue;
                }

                if (aceO.Location != null)
                {
                    WorldObject wo;

                    if (aceO.AceObjectId > 0)
                        wo = CreateWorldObject(aceO);
                    else
                    {
                        // Here we're letting the server assign the guid (We're not doing a clone because the object will never be saved back to db)
                        aceO.AceObjectId = GuidManager.NewGeneratorGuid().Full;
                        wo = CreateWorldObject(aceO);
                    }

                    if (wo != null)
                        results.Add(wo);                  
                }
            }

            foreach (var aceO in linkSourceResults)
            {
                int linkSlot = (int)aceO.LinkSlot;

                WorldObject wo;

                if (aceO.AceObjectId > 0)
                    wo = CreateWorldObject(aceO);
                else
                {
                    // Here we're letting the server assign the guid (We're not doing a clone because the object will never be saved back to db)
                    aceO.AceObjectId = GuidManager.NewGeneratorGuid().Full;
                    wo = CreateWorldObject(aceO);
                }

                if (linkResults.ContainsKey(linkSlot) && wo.GeneratorProfiles.Count > 0)
                {
                    var profileTemplate = wo.GeneratorProfiles[0];
                    foreach (var link in linkResults[linkSlot])
                    {
                        var profile = (AceObjectGeneratorProfile)profileTemplate.Clone();
                        profile.WeenieClassId = link.WeenieClassId;
                        profile.LandblockRaw = link.Location.Cell;
                        profile.PositionX = link.Location.PositionX;
                        profile.PositionY = link.Location.PositionY;
                        profile.PositionZ = link.Location.PositionZ;
                        profile.RotationW = link.Location.RotationW;
                        profile.RotationX = link.Location.RotationX;
                        profile.RotationY = link.Location.RotationY;
                        profile.RotationZ = link.Location.RotationZ;
                        profile.Probability = Math.Abs(profile.Probability);
                        wo.GeneratorProfiles.Add(profile);
                    }

                    wo.SelectGeneratorProfiles();
                    wo.UpdateGeneratorInts();
                    wo.QueueGenerator();
                }

                if (wo != null)
                    results.Add(wo);
            }
            return results;*/
        }
    }
}
