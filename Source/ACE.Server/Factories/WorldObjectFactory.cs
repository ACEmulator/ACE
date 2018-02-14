using System.Collections.Generic;

using ACE.Database;
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
        /// </summary>
        public static WorldObject CreateWorldObject(Weenie weenie)
        {
            var objWeenieType = (WeenieType)weenie.Type;

            switch (objWeenieType)
            {
                case WeenieType.LifeStone:
                    return new Lifestone(weenie);
                case WeenieType.Door:
                    return new Door(weenie);
                case WeenieType.Portal:
                    return new Portal(weenie);
                case WeenieType.Book:
                    return new Book(weenie);
                // case WeenieType.PKModifier:
                //    return new PKModifier(weenie);
                case WeenieType.Cow:
                    return new Cow(weenie);
                case WeenieType.Creature:
                    return new Creature(weenie);
                case WeenieType.Container:
                    return new Container(weenie);
                case WeenieType.Scroll:
                    return new Scroll(weenie);
                case WeenieType.Vendor:
                    return new Vendor(weenie);
                case WeenieType.Coin:
                    return new Coin(weenie);
                case WeenieType.Key:
                    return new Key(weenie);
                case WeenieType.Food:
                    return new Food(weenie);
                case WeenieType.Gem:
                    return new Gem(weenie);
                case WeenieType.Game:
                    return new Game(weenie);
                case WeenieType.GamePiece:
                    return new GamePiece(weenie);
                case WeenieType.AllegianceBindstone:
                    return new Bindstone(weenie);
                case WeenieType.Clothing:
                    return new Clothing(weenie);
                default:
                    return new GenericObject(weenie);
            }
        }


        /// <summary>
        /// This will create a list of WorldObjects, all with new GUIDs.
        /// </summary>
        public static List<WorldObject> CreateNewWorldObjects(Dictionary<Weenie, List<LandblockInstances>> sourceObjects)
        {
            return new List<WorldObject>();
        }

        /// <summary>
        /// This will create a new WorldObject with a new GUID.
        /// </summary>
        public static WorldObject CreateNewWorldObject(Weenie weenie)
        {
            var guid = GuidManager.NewDynamicGuid();

            var worldObject = CreateWorldObject(weenie);

            worldObject.Guid = guid;

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



        public static List<WorldObject> CreateWorldObjects(List<AceObject> sourceObjects)
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
