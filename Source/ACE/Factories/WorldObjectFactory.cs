using ACE.Database;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Managers;
using System;
using System.Collections.Generic;

namespace ACE.Factories
{
    public class WorldObjectFactory
    {
        public static List<WorldObject> CreateWorldObjects(List<AceObject> sourceObjects)
        {
            var results = new List<WorldObject>();

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
                    else
                    {
                        if (!linkResults.ContainsKey((int)aceO.LinkSlot))
                            linkResults.Add((int)aceO.LinkSlot, new List<AceObject>());
                        linkResults[(int)aceO.LinkSlot].Add(aceO);
                        continue;
                    }
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
            try
            {
                AceObject aceObject = (AceObject)DatabaseManager.World.GetAceObjectByWeenie(weenieId).Clone(guid.Full);

                return CreateWorldObject(aceObject);
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public static WorldObject CreateNewWorldObject(uint weenieId)
        {
            WorldObject wo = CreateWorldObject(weenieId, GuidManager.NewItemGuid());
            return wo;
        }
    }
}
