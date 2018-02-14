using System.Collections.Generic;

using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Entity.WorldObjects;
using ACE.Server.Managers;

namespace ACE.Server.Factories
{
    public static class WorldObjectFactory
    {
        /// <summary>
        /// This will create a WorldObject without generating a new GUID.
        /// If biota is null, one will be created with default values for this WorldObject type.
        /// </summary>
        public static WorldObject CreateWorldObject(Weenie weenie, Biota biota = null)
        {
            var objWeenieType = (WeenieType)weenie.Type;

            switch (objWeenieType)
            {
                case WeenieType.LifeStone:
                    return new Lifestone(weenie, biota);
                case WeenieType.Door:
                    return new Door(weenie, biota);
                case WeenieType.Portal:
                    return new Portal(weenie, biota);
                case WeenieType.Book:
                    return new Book(weenie, biota);
                // case WeenieType.PKModifier:
                //    return new PKModifier(weenie, biota);
                case WeenieType.Cow:
                    return new Cow(weenie, biota);
                case WeenieType.Creature:
                    return new Creature(weenie, biota);
                case WeenieType.Container:
                    return new Container(weenie, biota);
                case WeenieType.Scroll:
                    return new Scroll(weenie, biota);
                case WeenieType.Vendor:
                    return new Vendor(weenie, biota);
                case WeenieType.Coin:
                    return new Coin(weenie, biota);
                case WeenieType.Key:
                    return new Key(weenie, biota);
                case WeenieType.Food:
                    return new Food(weenie, biota);
                case WeenieType.Gem:
                    return new Gem(weenie, biota);
                case WeenieType.Game:
                    return new Game(weenie, biota);
                case WeenieType.GamePiece:
                    return new GamePiece(weenie, biota);
                case WeenieType.AllegianceBindstone:
                    return new Bindstone(weenie, biota);
                case WeenieType.Clothing:
                    return new Clothing(weenie, biota);
                default:
                    return new GenericObject(weenie, biota);
            }
        }


        /// <summary>
        /// This will create a list of WorldObjects, all with new GUIDs and for every position provided.
        /// </summary>
        public static List<WorldObject> CreateNewWorldObjects(Dictionary<Weenie, List<LandblockInstances>> sourceObjects)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// This will create a new WorldObject with a new GUID.
        /// </summary>
        public static WorldObject CreateNewWorldObject(Weenie weenie)
        {
            var worldObject = CreateWorldObject(weenie);

            worldObject.Guid = GuidManager.NewDynamicGuid();

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
        /// It will return null if weenieClassDescription was not found.
        /// </summary>
        public static WorldObject CreateNewWorldObject(string weenieClassDescription)
        {
            var classId = DatabaseManager.World.GetWeenieClassId(weenieClassDescription);

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
