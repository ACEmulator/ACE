using System.Collections.Generic;
using System;
using ACE.Database;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.WorldObjects;
using ACE.Server.Managers;
using ACE.Server.Network.Sequence;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity.Enum.Properties;
using ACE.Factories;

namespace ACE.Server.Factories
{
    public class LootGenerationFactory
    {
        // This is throw away code to understand the world object creation process.

        public static void Spawn(WorldObject inventoryItem, Position position)
        {
            throw new System.NotImplementedException();
            /* this was commented out because it's setting the phsycisdescriptionflag. Not sure we should be setting that here.
             // if we need to spawn things into the landscape, we should have such a factory that uses does that. LootGen factory doesn't seem appropriate for that
            inventoryItem.Sequences.GetNextSequence(SequenceType.ObjectTeleport);
            inventoryItem.Sequences.GetNextSequence(SequenceType.ObjectVector);
            inventoryItem.Location = position.InFrontOf(1.00f);
            inventoryItem.PhysicsDescriptionFlag |= PhysicsDescriptionFlag.Position;
            LandblockManager.AddObject(inventoryItem);*/
        }

        public static List<WorldObject> CreateRandomObjectsOfType(WeenieType type, int count)
        {
            var weenies = DatabaseManager.World.GetRandomWeeniesOfType((int)type, count);

            var worldObjects = new List<WorldObject>();

            foreach (var weenie in weenies)
            {
                var wo = WorldObjectFactory.CreateNewWorldObject(weenie.ClassId);
                worldObjects.Add(wo);
            }

            return worldObjects;
        }

        public static void CreateRandomTestWorldObjects(Player player, uint typeId, uint numItems)
        {
            throw new System.NotImplementedException();/*
            var weenieList = DatabaseManager.World.GetRandomWeeniesOfType(typeId, numItems);
            List<WorldObject> items = new List<WorldObject>();
            for (int i = 0; i < numItems; i++)
            {
                WorldObject wo = WorldObjectFactory.CreateNewWorldObject(weenieList[i].WeenieClassId);
                items.Add(wo);
            }
            player.HandleAddNewWorldObjectsToInventory(items);*/
        }

        public static WorldObject CreateRandomLootObjects(int tier)
        {
            WorldObject wo;
            switch (tier)
            {
                case 1:
                    //loot tier 1
                    wo = CreateTier1Objects();
                    break;
                case 2:
                    wo = CreateTier2Objects();
                    //loot tier 2
                    break;
                case 3:
                    //loot tier 3
                    wo = CreateTier3Objects();
                    break;
                case 4:
                    //loot tier 4
                    wo = CreateTier4Objects();
                    break;
                case 5:
                    //loot tier 5
                    wo = CreateTier5Objects();
                    break;
                case 6:
                    //loot tier 6
                    wo = CreateTier6Objects();
                    break;
                case 7:
                    //loot tier 7
                    wo = CreateTier7Objects();
                    break;
                default:
                    //loot tier 8
                    wo = CreateTier8Objects();
                    break;
            }
            return wo;
        }

        public static WorldObject CreateTier1Objects()
        {
            WorldObject wo;
            int id = 0;
            int chance;
            Random r = new Random();
                int type = r.Next(1, 6);
                switch (type)
                {
                    case 1:
                        //mundane items
                        int mundaneType = r.Next(1, 7);
                        switch (mundaneType)
                        {
                            case 1:
                                //peas
                                // Lead Pea
                                id = 8329;
                                break;
                            case 2:
                                //mana stones
                                chance = r.Next(1, 100);
                                if(chance < 75)
                                {
                                    //Minor Mana Stone
                                    id = 27331;
                                }
                                else
                                {
                                    //Lesser Mana Stone
                                    id = 2434;
                                }
                                break;
                            case 3:
                                //potions
                                chance = r.Next(1, 375);
                                if (chance < 100)
                                {
                                    //Stamina Potion
                                    id = 378;
                                }
                                else if (chance < 200)
                                {
                                    //Potion of Healing
                                    id = 377;
                                }
                                else if (chance < 300)
                                {
                                    //Mana Potion
                                    id = 379;
                                }
                                else if (chance < 325)
                                {
                                    //Health Draught
                                    id = 2457;
                                }
                                else if (chance < 350)
                                {
                                    //Mana Draught
                                    id = 2460;
                                }
                                else
                                {
                                    //Stamina Tincture
                                    id = 27326;
                                }
                                break;
                            case 4:
                                //healing kits
                                chance = r.Next(1, 100);
                                if (chance < 75)
                                {
                                    //Handy Healing Kit
                                    id = 628;
                                }
                                else
                                {
                                    //Adept Healing Kit
                                    id = 629;
                                }
                                break;
                            case 5:
                                //lockpicks
                                chance = r.Next(1, 100);
                                if (chance < 75)
                                {
                                    //Plain Lockpick
                                    id = 513;
                                }
                                else
                                {
                                    //Reliable Lockpick
                                    id = 545;
                                }
                                break;
                            //case 6:
                            //    //coalesced mana
                            //    //Yellow Mana
                            //    id = 42518;
                            //    break;
                            case 6:
                                //Food Items
                                id = CreateFood();
                                break;
                            case 8:
                                //spell scrolls level 1-3
                                break;
                            default:
                                break;
                        }
                    wo = WorldObjectFactory.CreateNewWorldObject((uint)id);
                    return wo;
                case 2:
                    //jewels
                    wo = CreateJewels(1);
                    return wo;
                case 3:
                    //armor
                    wo = CreateArmor(1);
                    return wo;
                case 4:
                    //weapons
                    wo = CreateWeapon(1);
                    return wo;
                default:
                    //jewelry
                    wo = CreateJewelry(1);
                    return wo;
            }
        }

        public static WorldObject CreateTier2Objects()
        {
            int id = 0;
            int chance;
            WorldObject wo;
            Random r = new Random();
                int type = r.Next(1, 6);
                switch (type)
                {
                    case 1:
                        //mundane items
                        int mundaneType = r.Next(1, 7);
                        switch (mundaneType)
                        {
                            case 1:
                                //peas
                                chance = r.Next(1, 100);
                                if (chance < 75)
                                {
                                    //lead pea
                                    id = 8329;
                                }
                                else
                                {
                                    //Iron Pea
                                    id = 8328;
                                }
                                break;
                            case 2:
                                //mana stones
                                chance = r.Next(1, 100);
                                if (chance < 75)
                                {
                                    //Minor Mana Stone
                                    id = 27331;
                                }
                                else if (chance < 95)
                                {
                                    //Less Mana Stone
                                    id = 2434;
                                }
                                else
                                {
                                    //Mana Stone
                                    id = 2435;
                                }
                                break;
                            case 3:
                                //potions
                                chance = r.Next(1, 450);
                                if (chance < 100)
                                {
                                    //Stamina Potion
                                    id = 378;
                                }
                                else if (chance < 200)
                                {
                                    //Potion of Healing
                                    id = 377;
                                }
                                else if (chance < 300)
                                {
                                    //Mana Potion
                                    id = 379;
                                }
                                else if (chance < 325)
                                {
                                    //Health Draught
                                    id = 2457;
                                }
                                else if (chance < 350)
                                {
                                    //Mana Draught
                                    id = 2460;
                                }
                                else if (chance < 375)
                                {
                                    //Stamina Tincture
                                    id = 27326;
                                }
                                else if (chance < 400)
                                {
                                    //Stamina Elixer
                                    id = 2470;
                                }
                                else if (chance < 425)
                                {
                                    //health Tincture
                                    id = 27319;
                                }
                                else
                                {
                                    //Mana Tincture
                                    id = 27322;
                                }
                                break;
                            case 4:
                                //healing kits
                                chance = r.Next(1, 100);
                                if (chance < 75)
                                {
                                    //Adept Healing Kit
                                    id = 629;
                                }
                                else
                                {
                                    //Gifted Healing Kit
                                    id = 630;
                                }
                                break;
                            case 5:
                                //lockpicks
                                chance = r.Next(1, 105);
                                if (chance < 75)
                                {
                                    //Plain Lockpick
                                    id = 513;
                                }
                                else if(chance < 100)
                                {
                                    //Reliable Lockpick
                                    id = 545;
                                }
                                else
                                {
                                    //Good Lockpick
                                    id = 512;
                                }
                                break;
                            //case 6:
                            //    //coalesced mana
                            //    //Yellow Mana
                            //    id = 42518;
                            //    break;
                            case 6:
                                id = CreateFood();
                                break;
                            case 8:
                                //spell scrolls level 3-5
                                break;
                            default:
                                break;
                        }
                    wo = WorldObjectFactory.CreateNewWorldObject((uint)id);
                    return wo;
                case 2:
                    //jewels
                    wo = CreateJewels(2);
                    return wo;
                case 3:
                    //armor
                    wo = CreateArmor(2);
                    return wo;
                case 4:
                    //weapons
                    wo = CreateWeapon(2);
                    return wo;
                default:
                    //jewelry
                    wo = CreateJewelry(2);
                    return wo;
            }
        }

        public static WorldObject CreateTier3Objects()
        {
            int id = 0;
            int chance;
            WorldObject wo;
            Random r = new Random();
                int type = r.Next(1, 6);
                switch (type)
                {
                    case 1:
                        //mundane items
                        int mundaneType = r.Next(1, 7);
                        switch (mundaneType)
                        {
                            case 1:
                                //peas
                                chance = r.Next(1, 125);
                                if (chance < 75)
                                {
                                    //lead pea
                                    id = 8329;
                                }
                                else if(chance < 100)
                                {
                                    //Iron Pea
                                    id = 8328;
                                }
                                else
                                {
                                    //Copper Pea
                                    id = 8326;
                                }
                                break;
                            case 2:
                                //mana stones
                                chance = r.Next(1, 100);
                                if (chance < 50)
                                {
                                    //Lesser Mana Stone
                                    id = 2434;
                                }
                                else if (chance < 95)
                                {
                                    //Mana Stone
                                    id = 2435;
                                }
                                else
                                {
                                    //Moderate Mana Stone
                                    id = 27330;
                                }
                                break;
                            case 3:
                                //potions
                                chance = r.Next(1, 450);
                                if (chance < 100)
                                {
                                    //Stamina Tincture
                                    id = 27326;
                                }
                                else if (chance < 200)
                                {
                                    //Potion of Healing
                                    id = 377;
                                }
                                else if (chance < 300)
                                {
                                    //Mana Elixer
                                    id = 27322;
                                }
                                else if (chance < 325)
                                {
                                    //health Tincture
                                    id = 27319; ;
                                }
                                else if (chance < 350)
                                {
                                    //Mana Draught
                                    id = 2460;
                                }
                                else if (chance < 375)
                                {
                                    //Stamina Elixer
                                    id = 2470;
                                }
                                else if (chance < 400)
                                {
                                    //Stamina Brew
                                    id = 27324;
                                }
                                else if (chance < 425)
                                {
                                    //health Elixer
                                    id = 2458;
                                }
                                else
                                {
                                    //Mana Elixer
                                    id = 2461;
                                }
                                break;
                            case 4:
                                //healing kits
                                chance = r.Next(1, 100);
                                if (chance < 75)
                                {
                                    //Adept Healing Kit
                                    id = 629;
                                }
                                else if (chance < 90)
                                {
                                    //Gifted Healing Kit
                                    id = 630;
                                }
                                else
                                {
                                    //Excellent Healing Kit
                                    id = 631;
                                }
                                break;
                            case 5:
                                //lockpicks
                                chance = r.Next(1, 105);
                                if (chance < 100)
                                {
                                    //Reliable Lockpick
                                    id = 545;
                                }
                                else if (chance < 100)
                                {
                                    //Good Lockpick
                                    id = 512;
                                }
                                else
                                {
                                    //Excellent Lockpick
                                    id = 514;
                                }
                                break;
                            //case 6:
                            //    //coalesced mana
                            //    chance = r.Next(1, 100);
                            //    if (chance < 90)
                            //    {
                            //        //Coalecsed Mana (Yellow)
                            //        id = 42518;
                            //    }
                            //    else
                            //    {
                            //        //Coalecsed Mana (Red)
                            //        id = 42517;
                            //    }
                            //    break;
                            case 6:
                                id = CreateFood();
                                break;
                            default:
                                break;
                        }
                    wo = WorldObjectFactory.CreateNewWorldObject((uint)id);
                    return wo;
                case 2:
                    //jewels
                    wo = CreateJewels(3);
                    return wo;
                case 3:
                    //armor
                    wo = CreateArmor(3);
                    return wo;
                case 4:
                    //weapons
                    wo = CreateWeapon(3);
                    return wo;
                default:
                    //jewelry
                    wo = CreateJewelry(3);
                    return wo;
            }
        }

        public static WorldObject CreateTier4Objects()
        {
            int id = 0;
            int chance;
            WorldObject wo;
            Random r = new Random();
                int type = r.Next(1, 6);
                switch (type)
                {
                    case 1:
                        //mundane items
                        int mundaneType = r.Next(1, 7);
                        switch (mundaneType)
                        {
                            case 1:
                                //peas
                                chance = r.Next(1, 125);
                                if (chance < 75)
                                {
                                    //lead pea
                                    id = 8329;
                                }
                                else if (chance < 100)
                                {
                                    //Iron Pea
                                    id = 8328;
                                }
                                else
                                {
                                    //Copper Pea
                                    id = 8326;
                                }
                                break;
                            case 2:
                                //mana stones
                                chance = r.Next(1, 100);
                                if (chance < 50)
                                {
                                    //Lesser Mana Stone
                                    id = 2434;
                                }
                                else if (chance < 95)
                                {
                                    //Mana Stone
                                    id = 2435;
                                }
                                else
                                {
                                    //Moderate Mana Stone
                                    id = 27330;
                                }
                                break;
                            case 3:
                                //potions
                                chance = r.Next(1, 450);
                                if (chance < 100)
                                {
                                    //Stamina Tincture
                                    id = 27326;
                                }
                                else if (chance < 200)
                                {
                                    //Potion of Healing
                                    id = 377;
                                }
                                else if (chance < 300)
                                {
                                    //Mana Elixer
                                    id = 27322;
                                }
                                else if (chance < 325)
                                {
                                    //health Tincture
                                    id = 27319; ;
                                }
                                else if (chance < 350)
                                {
                                    //Mana Draught
                                    id = 2460;
                                }
                                else if (chance < 375)
                                {
                                    //Stamina Elixer
                                    id = 2470;
                                }
                                else if (chance < 400)
                                {
                                    //Stamina Brew
                                    id = 27324;
                                }
                                else if (chance < 425)
                                {
                                    //health Elixer
                                    id = 2458;
                                }
                                else
                                {
                                    //Mana Elixer
                                    id = 2461;
                                }
                                break;
                            case 4:
                                //healing kits
                                chance = r.Next(1, 100);
                                if (chance < 75)
                                {
                                    //Adept Healing Kit
                                    id = 629;
                                }
                                else if (chance < 90)
                                {
                                    //Gifted Healing Kit
                                    id = 630;
                                }
                                else
                                {
                                    //Excellent Healing Kit
                                    id = 631;
                                }
                                break;
                            case 5:
                                //lockpicks
                                chance = r.Next(1, 105);
                                if (chance < 100)
                                {
                                    //Reliable Lockpick
                                    id = 545;
                                }
                                else if (chance < 100)
                                {
                                    //Good Lockpick
                                    id = 512;
                                }
                                else
                                {
                                    //Excellent Lockpick
                                    id = 514;
                                }
                                break;
                            //case 6:
                            //    //coalesced mana
                            //    chance = r.Next(1, 100);
                            //    if (chance < 50)
                            //    {
                            //        //Coalesced Mana (Yellow)
                            //        id = 42518;
                            //    }
                            //    else if(chance < 85)
                            //    {
                            //        //Coalesced Mana (Red)
                            //        id = 42517;
                            //    }
                            //    else
                            //    {
                            //        //Coalesced Mana (Blue)
                            //        id = 42516;
                            //    }
                            //    break;
                            case 6:
                                id = CreateFood();
                                break;
                            default:
                                break;
                        }
                    wo = WorldObjectFactory.CreateNewWorldObject((uint)id);
                    return wo;
                case 2:
                    //jewels
                    wo = CreateJewels(4);
                    return wo;
                case 3:
                    //armor
                    wo = CreateArmor(4);
                    return wo;
                case 4:
                    //weapons
                    wo = CreateWeapon(4);
                    return wo;
                default:
                    //jewelry
                    wo = CreateJewelry(4);
                    return wo;
            }
        }

        public static WorldObject CreateTier5Objects()
        {
            int id = 0;
            int chance;
            WorldObject wo;
            Random r = new Random();
                int type = r.Next(1, 6);
                switch (type)
                {
                    case 1:
                        //mundane items
                        int mundaneType = r.Next(1, 7);
                        switch (mundaneType)
                        {
                            case 1:
                                //peas
                                chance = r.Next(1, 125);
                                if (chance < 75)
                                {
                                    //Copper Pea
                                    id = 8326;
                                }
                                else if (chance < 100)
                                {
                                    //Silver Pea
                                    id = 8331;
                                }
                                else
                                {
                                    //Gold Pea
                                    id = 8327;
                                }
                                break;
                            case 2:
                                //mana stones
                                chance = r.Next(1, 100);
                                if (chance < 50)
                                {
                                    //Moderate Mana Stone
                                    id = 27330;
                                }
                                else if (chance < 95)
                                {
                                    //Greater Mana Stone
                                    id = 2436;
                                }
                                else
                                {
                                    //Major Mana Stone
                                    id = 27328;
                                }
                                break;
                            case 3:
                                //potions
                                chance = r.Next(1, 450);
                                if (chance < 100)
                                {
                                    //Stamina Brew
                                    id = 27324;
                                }
                                else if (chance < 200)
                                {
                                    //health Elixer
                                    id = 2458;
                                }
                                else if (chance < 300)
                                {
                                    //Mana Elixer
                                    id = 2461;
                                }
                                else if (chance < 325)
                                {
                                    //health Tonic
                                    id = 27320; ;
                                }
                                else if (chance < 350)
                                {
                                    //Mana Tonic
                                    id = 27323;
                                }
                                else if (chance < 375)
                                {
                                    //Stamina Tonic
                                    id = 27327;
                                }
                                else if (chance < 400)
                                {
                                    //Stamina Philtre
                                    id = 27325;
                                }
                                else if (chance < 425)
                                {
                                    //health Philtre
                                    id = 27318;
                                }
                                else
                                {
                                    //Mana Philtre
                                    id = 27321;
                                }
                                break;
                            case 4:
                                //healing kits
                                chance = r.Next(1, 100);
                                if (chance < 75)
                                {
                                    //Excellent Healing Kit
                                    id = 631;
                                }
                                else if (chance < 90)
                                {
                                    //Peerless Healing Kit
                                    id = 632;
                                }
                                else
                                {
                                    //Treated Healing Kit
                                    id = 9229;
                                }
                                break;
                            case 5:
                                //lockpicks
                                chance = r.Next(1, 105);
                                if (chance < 100)
                                {
                                    //Excellent Lockpick
                                    id = 514;
                                }
                                else if (chance < 100)
                                {
                                    //Superb Lockpick
                                    id = 515;
                                }
                                else
                                {
                                    //Peerless Lockpick
                                    id = 516;
                                }
                                break;
                            //case 6:
                            //    //coalesced mana
                            //    chance = r.Next(1, 100);
                            //    if (chance < 50)
                            //    {
                            //        //Coalesced Mana (Yellow)
                            //        id = 42518;
                            //    }
                            //    else if (chance < 85)
                            //    {
                            //        //Coalesced Mana (Red)
                            //        id = 42517;
                            //    }
                            //    else
                            //    {
                            //        //Coalesced Mana (Blue)
                            //        id = 42516;
                            //    }
                            //    break;
                            case 6:
                                id = CreateFood();
                                break;
                            default:
                                break;
                        }
                        wo = WorldObjectFactory.CreateNewWorldObject((uint)id);
                        return wo;
                    case 2:
                        //jewels
                        wo = CreateJewels(5);
                        return wo;
                    case 3:
                        //armor
                        wo = CreateArmor(5);
                        return wo;
                    case 4:
                        //weapons
                        wo = CreateWeapon(5);
                        return wo;
                    default:
                        //jewelry
                        wo = CreateJewelry(5);
                        return wo;
                }
            }

        public static WorldObject CreateTier6Objects()
        {
            int id = 0;
            int chance;
            WorldObject wo;
            Random r = new Random();
                int type = r.Next(1, 6);
                switch (type)
                {
                    case 1:
                        //mundane items
                        int mundaneType = r.Next(1, 7);
                        switch (mundaneType)
                        {
                            case 1:
                                //peas
                                chance = r.Next(1, 125);
                                if (chance < 75)
                                {
                                    //Silver Pea
                                    id = 8331;
                                }
                                else if (chance < 100)
                                {
                                    //Gold Pea
                                    id = 8327;
                                }
                                else
                                {
                                    //Pyreal Pea
                                    id = 8330;
                                }
                                break;
                            case 2:
                                //mana stones
                                chance = r.Next(1, 100);
                                if (chance < 75)
                                {
                                    //Greater Mana Stone
                                    id = 2436;
                                }
                                else
                                {
                                    //Major Mana Stone
                                    id = 27328;
                                }
                                break;
                            case 3:
                                //potions
                                chance = r.Next(1, 450);
                                if (chance < 100)
                                {
                                    //Stamina Brew
                                    id = 27324;
                                }
                                else if (chance < 200)
                                {
                                    //health Elixer
                                    id = 2458;
                                }
                                else if (chance < 300)
                                {
                                    //Mana Elixer
                                    id = 2461;
                                }
                                else if (chance < 325)
                                {
                                    //health Tonic
                                    id = 27320; ;
                                }
                                else if (chance < 350)
                                {
                                    //Mana Tonic
                                    id = 27323;
                                }
                                else if (chance < 375)
                                {
                                    //Stamina Tonic
                                    id = 27327;
                                }
                                else if (chance < 400)
                                {
                                    //Stamina Philtre
                                    id = 27325;
                                }
                                else if (chance < 425)
                                {
                                    //health Philtre
                                    id = 27318;
                                }
                                else
                                {
                                    //Mana Philtre
                                    id = 27321;
                                }
                                break;
                            case 4:
                                //healing kits
                                chance = r.Next(1, 100);
                                if (chance < 75)
                                {
                                    //Excellent Healing Kit
                                    id = 631;
                                }
                                else if (chance < 90)
                                {
                                    //Peerless Healing Kit
                                    id = 632;
                                }
                                else
                                {
                                    //Treated Healing Kit
                                    id = 9229;
                                }
                                break;
                            case 5:
                                //lockpicks
                                chance = r.Next(1, 105);
                                if (chance < 100)
                                {
                                    //Excellent Lockpick
                                    id = 514;
                                }
                                else if (chance < 100)
                                {
                                    //Superb Lockpick
                                    id = 515;
                                }
                                else
                                {
                                    //Peerless Lockpick
                                    id = 516;
                                }
                                break;
                            //case 6:
                            //    //coalesced mana
                            //    chance = r.Next(1, 100);
                            //    if (chance < 50)
                            //    {
                            //        //Coalesced Mana (Yellow)
                            //        id = 42518;
                            //    }
                            //    else if (chance < 85)
                            //    {
                            //        //Coalesced Mana (Red)
                            //        id = 42517;
                            //    }
                            //    else
                            //    {
                            //        //Coalesced Mana (Blue)
                            //        id = 42516;
                            //    }
                            //    break;
                            case 6:
                                id = CreateFood();
                                break;
                            default:
                                break;
                        }
                        wo = WorldObjectFactory.CreateNewWorldObject((uint)id);
                        return wo;
                    case 2:
                        //jewels
                        wo = CreateJewels(6);
                        return wo;
                    case 3:
                        //armor
                        wo = CreateArmor(6);
                        return wo;
                    case 4:
                        //weapons
                        wo = CreateWeapon(6);
                        return wo;
                    default:
                        //jewelry
                        wo = CreateJewelry(6);
                        return wo;
                }
            }

        public static WorldObject CreateTier7Objects()
        {
            int id = 0;
            int chance;
            WorldObject wo;
            Random r = new Random();
                int type = r.Next(1, 6);
                switch (type)
                {
                    case 1:
                        //mundane items
                        int mundaneType = r.Next(1, 7);
                        switch (mundaneType)
                        {
                            case 1:
                                //peas
                                chance = r.Next(1, 125);
                                if (chance < 75)
                                {
                                    //Silver Pea
                                    id = 8331;
                                }
                                else if (chance < 100)
                                {
                                    //Gold Pea
                                    id = 8327;
                                }
                                else
                                {
                                    //Pyreal Pea
                                    id = 8330;
                                }
                                break;
                            case 2:
                                //mana stones
                                chance = r.Next(1, 100);
                                if (chance < 75)
                                {
                                    //Greater Mana Stone
                                    id = 2436;
                                }
                                else
                                {
                                    //Major Mana Stone
                                    id = 27328;
                                }
                                break;
                            case 3:
                                //potions
                                chance = r.Next(1, 450);
                                if (chance < 100)
                                {
                                    //Stamina Brew
                                    id = 27324;
                                }
                                else if (chance < 200)
                                {
                                    //health Elixer
                                    id = 2458;
                                }
                                else if (chance < 300)
                                {
                                    //Mana Elixer
                                    id = 2461;
                                }
                                else if (chance < 325)
                                {
                                    //health Tonic
                                    id = 27320; ;
                                }
                                else if (chance < 350)
                                {
                                    //Mana Tonic
                                    id = 27323;
                                }
                                else if (chance < 375)
                                {
                                    //Stamina Tonic
                                    id = 27327;
                                }
                                else if (chance < 400)
                                {
                                    //Stamina Philtre
                                    id = 27325;
                                }
                                else if (chance < 425)
                                {
                                    //health Philtre
                                    id = 27318;
                                }
                                else
                                {
                                    //Mana Philtre
                                    id = 27321;
                                }
                                break;
                            case 4:
                                //healing kits
                                chance = r.Next(1, 100);
                                if (chance < 75)
                                {
                                    //Excellent Healing Kit
                                    id = 631;
                                }
                                else if (chance < 90)
                                {
                                    //Peerless Healing Kit
                                    id = 632;
                                }
                                else
                                {
                                    //Treated Healing Kit
                                    id = 9229;
                                }
                                break;
                            case 5:
                                //lockpicks
                                chance = r.Next(1, 105);
                                if (chance < 100)
                                {
                                    //Excellent Lockpick
                                    id = 514;
                                }
                                else if (chance < 100)
                                {
                                    //Superb Lockpick
                                    id = 515;
                                }
                                else
                                {
                                    //Peerless Lockpick
                                    id = 516;
                                }
                                break;
                            //case 6:
                            //    //coalesced mana
                            //    chance = r.Next(1, 100);
                            //    if (chance < 50)
                            //    {
                            //        //Coalesced Mana (Yellow)
                            //        id = 42518;
                            //    }
                            //    else if (chance < 85)
                            //    {
                            //        //Coalesced Mana (Red)
                            //        id = 42517;
                            //    }
                            //    else
                            //    {
                            //        //Coalesced Mana (Blue)
                            //        id = 42516;
                            //    }
                            //    break;
                            case 6:
                                id = CreateFood();
                                break;
                            default:
                                break;
                        }
                        wo = WorldObjectFactory.CreateNewWorldObject((uint)id);
                        return wo;
                    case 2:
                        //jewels
                        wo = CreateJewels(7);
                        return wo;
                    case 3:
                        //armor
                        wo = CreateArmor(7);
                        return wo;
                    case 4:
                        //weapons
                        wo = CreateWeapon(7);
                        return wo;
                    default:
                        //jewelry
                        wo = CreateJewelry(7);
                        return wo;
                }
            }

        public static WorldObject CreateTier8Objects()
        {
            int id = 0;
            int chance;
            WorldObject wo;
            Random r = new Random();
                int type = r.Next(1, 6);
                switch (type)
                {
                    case 1:
                        //mundane items
                        int mundaneType = r.Next(1, 7);
                        switch (mundaneType)
                        {
                            case 1:
                                //peas
                                chance = r.Next(1, 125);
                                if (chance < 75)
                                {
                                    //Silver Pea
                                    id = 8331;
                                }
                                else if (chance < 100)
                                {
                                    //Gold Pea
                                    id = 8327;
                                }
                                else
                                {
                                    //Pyreal Pea
                                    id = 8330;
                                }
                                break;
                            case 2:
                                //mana stones
                                chance = r.Next(1, 100);
                                if (chance < 75)
                                {
                                    //Greater Mana Stone
                                    id = 2436;
                                }
                                else
                                {
                                    //Major Mana Stone
                                    id = 27328;
                                }
                                break;
                            case 3:
                                //potions
                                chance = r.Next(1, 450);
                                if (chance < 100)
                                {
                                    //Stamina Brew
                                    id = 27324;
                                }
                                else if (chance < 200)
                                {
                                    //health Elixer
                                    id = 2458;
                                }
                                else if (chance < 300)
                                {
                                    //Mana Elixer
                                    id = 2461;
                                }
                                else if (chance < 325)
                                {
                                    //health Tonic
                                    id = 27320; ;
                                }
                                else if (chance < 350)
                                {
                                    //Mana Tonic
                                    id = 27323;
                                }
                                else if (chance < 375)
                                {
                                    //Stamina Tonic
                                    id = 27327;
                                }
                                else if (chance < 400)
                                {
                                    //Stamina Philtre
                                    id = 27325;
                                }
                                else if (chance < 425)
                                {
                                    //health Philtre
                                    id = 27318;
                                }
                                else
                                {
                                    //Mana Philtre
                                    id = 27321;
                                }
                                break;
                            case 4:
                                //healing kits
                                chance = r.Next(1, 100);
                                if (chance < 75)
                                {
                                    //Excellent Healing Kit
                                    id = 631;
                                }
                                else if (chance < 90)
                                {
                                    //Peerless Healing Kit
                                    id = 632;
                                }
                                else
                                {
                                    //Treated Healing Kit
                                    id = 9229;
                                }
                                break;
                            case 5:
                                //lockpicks
                                chance = r.Next(1, 105);
                                if (chance < 100)
                                {
                                    //Excellent Lockpick
                                    id = 514;
                                }
                                else if (chance < 100)
                                {
                                    //Superb Lockpick
                                    id = 515;
                                }
                                else
                                {
                                    //Peerless Lockpick
                                    id = 516;
                                }
                                break;
                            //case 6:
                            //    //coalesced mana
                            //    chance = r.Next(1, 100);
                            //    if (chance < 50)
                            //    {
                            //        //Coalesced Mana (Yellow)
                            //        id = 42518;
                            //    }
                            //    else if (chance < 85)
                            //    {
                            //        //Coalesced Mana (Red)
                            //        id = 42517;
                            //    }
                            //    else
                            //    {
                            //        //Coalesced Mana (Blue)
                            //        id = 42516;
                            //    }
                            //    break;
                            //case 7:
                            //    //spell components
                            //    chance = r.Next(1, 170);
                            //    if (chance < 100)
                            //    {
                            //        //Quill of Infliction
                            //        id = 37363;
                            //    }
                            //    else if (chance == 100)
                            //    {
                            //        //Quill of Benevolence
                            //        id = 37365;
                            //    }
                            //    else if (chance == 101)
                            //    {
                            //        //Quill of Extraction
                            //        id = 37362;
                            //    }
                            //    else if (chance == 102)
                            //    {
                            //        //Quill of Introspection
                            //        id = 37364;
                            //    }
                            //    else if (chance == 103)
                            //    {
                            //        //Ink of Conveyance
                            //        id = 37360;
                            //    }
                            //    else if (chance == 104)
                            //    {
                            //        //Ink of direction
                            //        id = 37361;
                            //    }
                            //    else if (chance == 105)
                            //    {
                            //        //Ink of Formation
                            //        id = 37353;
                            //    }
                            //    else if (chance == 106)
                            //    {
                            //        //Ink of Nullification
                            //        id = 37354;
                            //    }
                            //    else if (chance == 107)
                            //    {
                            //        //Ink of Objectification
                            //        id = 37355;
                            //    }
                            //    else if (chance == 108)
                            //    {
                            //        //Ink of Partition
                            //        id = 37357;
                            //    }
                            //    else if (chance == 109)
                            //    {
                            //        //Ink of Separation
                            //        id = 37358;
                            //    }
                            //    else if (chance == 110)
                            //    {
                            //        //Parabolic Ink
                            //        id = 37356;
                            //    }
                            //    else if (chance == 111)
                            //    {
                            //        //Alacritous Ink
                            //        id = 37359;
                            //    }
                            //    else if (chance == 112)
                            //    {
                            //        //Mana Scarab
                            //        id = 37115;
                            //    }
                            //    else if (chance == 113)
                            //    {
                            //        //glyph of alchemy
                            //        id = 37343;
                            //    }
                            //    else if (chance == 114)
                            //    {
                            //        //glyph of alchemy
                            //        id = 37343;
                            //    }
                            //    else if (chance == 115)
                            //    {
                            //        //glyph of arcane lore
                            //        id = 37344;
                            //    }
                            //    else if (chance == 116)
                            //    {
                            //        //glyph of armor
                            //        id = 37345;
                            //    }
                            //    else if (chance == 117)
                            //    {
                            //        //glyph of armor tinkering
                            //        id = 37346;
                            //    }
                            //    else if (chance == 118)
                            //    {
                            //        //glyph of bludgeoning
                            //        id = 37347;
                            //    }
                            //    else if (chance == 119)
                            //    {
                            //        //glyph of cooking
                            //        id = 37349;
                            //    }
                            //    else if (chance == 120)
                            //    {
                            //        //glyph of coordination
                            //        id = 37350;
                            //    }
                            //    else if (chance == 121)
                            //    {
                            //        //glyph of corrosion
                            //        id = 37342;
                            //    }
                            //    else if (chance == 122)
                            //    {
                            //        //glyph of creature enchantment
                            //        id = 37351;
                            //    }
                            //    else if (chance == 123)
                            //    {
                            //        //glyph of damage
                            //        id = 43379;
                            //    }
                            //    else if (chance == 124)
                            //    {
                            //        //glyph of deception
                            //        id = 37352;
                            //    }
                            //    else if (chance == 125)
                            //    {
                            //        //glyph of dirty fighting
                            //        id = 45370;
                            //    }
                            //    else if (chance == 126)
                            //    {
                            //        //glyph of dual wield
                            //        id = 45371;
                            //    }
                            //    else if (chance == 127)
                            //    {
                            //        //glyph of endurance
                            //        id = 37300;
                            //    }
                            //    else if (chance == 128)
                            //    {
                            //        //glyph of finesse weapon
                            //        id = 37373;
                            //    }
                            //    else if (chance == 129)
                            //    {
                            //        //glyph of flame
                            //        id = 37301;
                            //    }
                            //    else if (chance == 130)
                            //    {
                            //        //glyph of fletching
                            //        id = 37302;
                            //    }
                            //    else if (chance == 131)
                            //    {
                            //        //glyph of focus
                            //        id = 37303;
                            //    }
                            //    else if (chance == 132)
                            //    {
                            //        //glyph of frost
                            //        id = 37348;
                            //    }
                            //    else if (chance == 133)
                            //    {
                            //        //glyph of healing
                            //        id = 37304;
                            //    }
                            //    else if (chance == 134)
                            //    {
                            //        //glyph of health
                            //        id = 37305;
                            //    }
                            //    else if (chance == 135)
                            //    {
                            //        //glyph of heavy weapons
                            //        id = 37369;
                            //    }
                            //    else if (chance == 136)
                            //    {
                            //        //glyph of item enchantment
                            //        id = 37309;
                            //    }
                            //    else if (chance == 137)
                            //    {
                            //        //glyph of item tinnkering
                            //        id = 37310;
                            //    }
                            //    else if (chance == 138)
                            //    {
                            //        //glyph of jump
                            //        id = 37311;
                            //    }
                            //    else if (chance == 139)
                            //    {
                            //        //glyph of leadership
                            //        id = 37312;
                            //    }
                            //    else if (chance == 140)
                            //    {
                            //        //glyph of life magic
                            //        id = 37313;
                            //    }
                            //    else if (chance == 141)
                            //    {
                            //        //glyph of light weapons
                            //        id = 37339;
                            //    }
                            //    else if (chance == 142)
                            //    {
                            //        //glyph of lightning
                            //        id = 37314;
                            //    }
                            //    else if (chance == 143)
                            //    {
                            //        //glyph of lockpick
                            //        id = 37315;
                            //    }
                            //    else if (chance == 144)
                            //    {
                            //        //glyph of loyalty
                            //        id = 37316;
                            //    }
                            //    else if (chance == 145)
                            //    {
                            //        //glyph of magic defense
                            //        id = 37317;
                            //    }
                            //    else if (chance == 146)
                            //    {
                            //        //glyph of magic item tinkering
                            //        id = 38760;
                            //    }
                            //    else if (chance == 147)
                            //    {
                            //        //glyph of mana
                            //        id = 37318;
                            //    }
                            //    else if (chance == 148)
                            //    {
                            //        //glyph of mana conversion
                            //        id = 37319;
                            //    }
                            //    else if (chance == 149)
                            //    {
                            //        //glyph of mana regeneration
                            //        id = 37321;
                            //    }
                            //    else if (chance == 150)
                            //    {
                            //        //glyph of melee defense
                            //        id = 37323;
                            //    }
                            //    else if (chance == 151)
                            //    {
                            //        //glyph of missile defense
                            //        id = 37324;
                            //    }
                            //    else if (chance == 152)
                            //    {
                            //        //glyph of Missile weapons
                            //        id = 37338;
                            //    }
                            //    else if (chance == 153)
                            //    {
                            //        //glyph of monster appraisal
                            //        id = 37325;
                            //    }
                            //    else if (chance == 154)
                            //    {
                            //        //glyph of nether
                            //        id = 43387;
                            //    }
                            //    else if (chance == 155)
                            //    {
                            //        //glyph of person appraisal
                            //        id = 37326;
                            //    }
                            //    else if (chance == 156)
                            //    {
                            //        //glyph of piercing
                            //        id = 37327;
                            //    }
                            //    else if (chance == 157)
                            //    {
                            //        //glyph of quickness
                            //        id = 37328;
                            //    }
                            //    else if (chance == 158)
                            //    {
                            //        //glyph of recklessness
                            //        id = 45372;
                            //    }
                            //    else if (chance == 159)
                            //    {
                            //        //glyph of regeneration
                            //        id = 37307;
                            //    }
                            //    else if (chance == 160)
                            //    {
                            //        //glyph of run
                            //        id = 37329;
                            //    }
                            //    else if (chance == 161)
                            //    {
                            //        //glyph of salvaging
                            //        id = 37330;
                            //    }
                            //    else if (chance == 162)
                            //    {
                            //        //glyph of self
                            //        id = 37331;
                            //    }
                            //    else if (chance == 163)
                            //    {
                            //        //glyph of shield
                            //        id = 45373;
                            //    }
                            //    else if (chance == 164)
                            //    {
                            //        //glyph of slashing
                            //        id = 37332;
                            //    }
                            //    else if (chance == 165)
                            //    {
                            //        //glyph of sneak attack
                            //        id = 45374;
                            //    }
                            //    else if (chance == 166)
                            //    {
                            //        //glyph of stamina
                            //        id = 37333;
                            //    }
                            //    else if (chance == 167)
                            //    {
                            //        //glyph of stamina regeneration
                            //        id = 37336;
                            //    }
                            //    else if (chance == 168)
                            //    {
                            //        //glyph of strength
                            //        id = 37337;
                            //    }
                            //    else if (chance == 169)
                            //    {
                            //        //glyph of summoning
                            //        id = 49455;
                            //    }
                            //    else if (chance == 170)
                            //    {
                            //        //glyph of two handed combat
                            //        id = 41747;
                            //    }
                            //    else if (chance == 171)
                            //    {
                            //        //glyph of void magic
                            //        id = 43380;
                            //    }
                            //    else if (chance == 172)
                            //    {
                            //        //glyph of war magic
                            //        id = 37340;
                            //    }
                            //    else if (chance == 173)
                            //    {
                            //        //glyph of weapon tinkering
                            //        id = 37341;
                            //    }
                            //    break;
                            case 6:
                                id = CreateFood();
                                break;
                            default:
                                break;
                        }
                        wo = WorldObjectFactory.CreateNewWorldObject((uint)id);
                        return wo;
                    case 2:
                        //jewels
                        wo = CreateJewels(8);
                        return wo;
                    case 3:
                        //armor
                        wo = CreateArmor(8);
                        return wo;
                    case 4:
                        //weapons
                        wo = CreateWeapon(8);
                        return wo;
                    default:
                        //jewelry
                        wo = CreateJewelry(8);
                        return wo;
                }
            
        }

        public static WorldObject CreateJewels(int tier)
        {
            Random r = new Random();
            int spellChance = 0;
            int gemType = 0;
            int workmanship = 0;
            int value = 100;
            int rank = 0;
            int difficulty = 0;
            int mana_cost = 0;
            int spellDID = 0;
            int max_mana = 0;
            int skill_level_limit = 0;
            int spellcraft = 0;
            int[] creature1 = { 2, 18, 256, 274, 298, 322, 346, 418, 467, 557, 581, 605, 629, 653, 678, 702, 726, 750, 774, 798, 824, 850, 874, 898, 922, 946, 970, 982, 1349, 1373, 1397, 1421, 1445, 1715, 1739, 1763, 5779, 5803, 5827, 5843, 5867, 6116 };
            int[] creature2 = { 1328, 245, 257, 275, 299, 323, 347, 419, 468, 558, 582, 606, 630, 654, 679, 703, 727, 751, 775, 799, 825, 851, 875, 899, 923, 947, 971, 983, 1350, 1374, 1398, 1422, 1446, 1716, 1740, 1764, 5780, 5804, 5828, 5844, 5868, 6117 };
            int[] creature3 = { 1329, 246, 258, 276, 300, 324, 348, 420, 469, 559, 583, 607, 631, 655, 680, 704, 728, 752, 776, 800, 826, 852, 876, 900, 924, 948, 972, 984, 1351, 1375, 1399, 1423, 1447, 1717, 1741, 1765, 5781, 5805, 5829, 5845, 5869, 6118 };
            int[] creature4 = { 1330, 247, 259, 277, 301, 325, 349, 421, 470, 560, 584, 608, 632, 656, 681, 705, 729, 753, 777, 801, 827, 853, 877, 901, 925, 949, 973, 985, 1352, 1376, 1400, 1424, 1448, 1718, 1742, 1766, 5782, 5806, 5830, 5846, 5870, 6119 };
            int[] creature5 = { 1331, 248, 260, 278, 302, 326, 350, 422, 471, 561, 585, 609, 633, 657, 682, 706, 730, 754, 778, 802, 828, 854, 878, 902, 926, 950, 974, 986, 1353, 1377, 1401, 1425, 1449, 1719, 1743, 1767, 5783, 5807, 5831, 5847, 5871, 6120 };
            int[] creature6 = { 1332, 249, 261, 279, 303, 327, 351, 423, 472, 562, 586, 610, 634, 658, 683, 707, 731, 755, 779, 803, 829, 855, 879, 903, 927, 951, 975, 987, 1354, 1378, 1402, 1426, 1450, 1720, 1744, 1768, 5784, 5808, 5831, 5848, 5872, 6121 };
            int[] creature7 = { 2087, 2245, 2243, 2281, 2275, 2223, 5105, 2309, 2243, 2215, 2249, 2267, 2323, 2287, 2195, 2197, 2251, 2277, 2325, 2289, 2293, 2226, 2241, 2263, 2271, 2233, 2256, 2301, 2061, 2059, 2081, 2067, 2091, 2211, 2237, 2191, 5785, 5809, 5833, 5857, 5881, 6122, 5417, 3519};
            int[] creature8 = { 4325, 4560, 4544, 4596, 4518, 4538, 5032, 4624, 4558, 4530, 4564, 4582, 4638, 4602, 4510, 4512, 4566, 4592, 4640, 4604, 4608, 4542, 4556, 4578, 4586, 4548, 4572, 4616, 4299, 4297, 4319, 4305, 4329, 4526, 4552, 4506, 5786, 5810, 5834, 5858, 5882, 6123, 5418, 4502};
            int[] life1 = { 165, 54, 212, 515, 1018, 1030, 1066, 20, 1109, 1133, 24 };
            int[] life2 = { 166, 189, 213, 516, 1019, 1031, 1067, 1090, 1110, 1134, 1308 };
            int[] life3 = { 167, 190, 214, 517, 1020, 1032, 1068, 1091, 1111, 1135, 1309 };
            int[] life4 = { 168, 190, 215, 518, 1021, 1033, 1069, 1092, 1112, 1136, 1310 };
            int[] life5 = { 169, 190, 216, 519, 1022, 1034, 1070, 1093, 1113, 1137, 1311 };
            int[] life6 = { 170, 190, 217, 520, 1023, 1035, 1071, 1094, 1114, 1138, 1312 };
            int[] life7 = { 2185, 2187, 2183, 2149, 2153, 2155, 2159, 2157, 2151, 2161, 2053};
            int[] life8 = { 4496, 4498, 4494, 4460, 4464, 4466, 4470, 4468, 4462 ,4472, 4291};
            int[] t1gems = new int[] { 2433, 2418, 2419, 2420, 2426, 2431, 2413, 2414, 2427, 2428, 2429, 2430, 2415, 2405, 2416, 2406, 2433, 2417 };
            int[] t2gems = new int[] { 2433, 2418, 2419, 2420, 2426, 2431, 2413, 2414, 2427, 2428, 2429, 2430, 2415, 2405, 2416, 2406, 2433, 2417, 2393, 2395, 2398, 2399, 2400, 2401, 2394, 2396, 2397 };
            int[] t3gems = new int[] { 2433, 2418, 2419, 2420, 2426, 2431, 2413, 2414, 2427, 2428, 2429, 2430, 2415, 2405, 2416, 2406, 2433, 2417, 2393, 2395, 2398, 2399, 2400, 2401, 2394, 2396, 2397, 2425, 2421, 2422, 2423 };
            int[] t4gems = new int[] { 2433, 2418, 2419, 2420, 2426, 2431, 2413, 2414, 2427, 2428, 2429, 2430, 2415, 2405, 2416, 2406, 2433, 2417, 2393, 2395, 2398, 2399, 2400, 2401, 2394, 2396, 2397, 2425, 2421, 2422, 2423, 2405, 2408, 2402, 2403, 2407 };
            int[] t5gems = new int[] { 2433, 2418, 2419, 2420, 2426, 2431, 2413, 2414, 2427, 2428, 2429, 2430, 2415, 2405, 2416, 2406, 2433, 2417, 2393, 2395, 2398, 2399, 2400, 2401, 2394, 2396, 2397, 2425, 2421, 2422, 2423, 2405, 2408, 2402, 2403, 2407, 2409, 2411, 2412, 2410 };
            switch (tier)
            {
                case 1:
                    //tier 1
                    gemType = t1gems[r.Next(0, t1gems.Length)];
                    if(r.Next(0,2) == 1)
                    {
                        spellChance = r.Next(0, 100);
                        if(spellChance < 30)
                        {
                            spellDID = creature1[r.Next(0, creature1.Length)];
                            mana_cost = 50;
                            max_mana = r.Next(mana_cost, mana_cost + 50);
                            spellcraft = 50;
                        }
                        else if (spellChance < 60)
                        {
                            spellDID = life1[r.Next(0, life1.Length)];
                            mana_cost = 50;
                            max_mana = r.Next(mana_cost, mana_cost + 50);
                            spellcraft = 50;
                        }
                        else if (spellChance < 75)
                        {
                            spellDID = creature2[r.Next(0, creature2.Length)];
                            mana_cost = 100;
                            max_mana = r.Next(mana_cost, mana_cost + 50);
                            spellcraft = 100;
                        }
                        else if (spellChance < 90)
                        {
                            spellDID = life2[r.Next(0, life2.Length)];
                            mana_cost = 100;
                            max_mana = r.Next(mana_cost, mana_cost + 50);
                            spellcraft = 100;
                        }
                        else if (spellChance < 95)
                        {
                            spellDID = creature3[r.Next(0, creature3.Length)];
                            mana_cost = 150;
                            max_mana = r.Next(mana_cost, mana_cost + 50);
                            spellcraft = 150;
                        }
                        else
                        {
                            spellDID = life3[r.Next(0, life3.Length)];
                            mana_cost = 150;
                            max_mana = r.Next(mana_cost, mana_cost + 50);
                            spellcraft = 150;
                        }
                    }
                    workmanship = GetWorkmanship(1);
                    break;
                case 2:
                    //tier 2
                    gemType = t2gems[r.Next(0, t2gems.Length)];
                    if (r.Next(0, 2) == 1)
                    {
                        spellChance = r.Next(0, 100);
                        if (spellChance < 30)
                        {
                            spellDID = creature3[r.Next(0, creature1.Length)];
                            mana_cost = 150;
                            max_mana = r.Next(mana_cost, mana_cost + 50);
                            spellcraft = 150;
                        }
                        else if (spellChance < 60)
                        {
                            spellDID = life3[r.Next(0, life1.Length)];
                            mana_cost = 150;
                            max_mana = r.Next(mana_cost, mana_cost + 50);
                            spellcraft = 150;
                        }
                        else if (spellChance < 75)
                        {
                            spellDID = creature4[r.Next(0, creature2.Length)];
                            mana_cost = 200;
                            max_mana = r.Next(mana_cost, mana_cost + 50);
                            spellcraft = 200;
                        }
                        else if (spellChance < 90)
                        {
                            spellDID = life4[r.Next(0, life2.Length)];
                            mana_cost = 200;
                            max_mana = r.Next(mana_cost, mana_cost + 50);
                            spellcraft = 200;
                        }
                        else if (spellChance < 95)
                        {
                            spellDID = creature5[r.Next(0, creature3.Length)];
                            mana_cost = 250;
                            max_mana = r.Next(mana_cost, mana_cost + 50);
                            spellcraft = 250;
                        }
                        else
                        {
                            spellDID = life5[r.Next(0, life3.Length)];
                            mana_cost = 250;
                            max_mana = r.Next(mana_cost, mana_cost + 50);
                            spellcraft = 250;
                        }
                    }
                    workmanship = GetWorkmanship(2);
                    break;
                case 3:
                    //tier 3
                    gemType = t3gems[r.Next(0, t3gems.Length)];
                    if (r.Next(0, 2) == 1)
                    {
                        spellChance = r.Next(0, 100);
                        if (spellChance < 30)
                        {
                            spellDID = creature4[r.Next(0, creature1.Length)];
                            mana_cost = 200;
                            max_mana = r.Next(mana_cost, mana_cost + 50);
                            spellcraft = 200;
                        }
                        else if (spellChance < 60)
                        {
                            spellDID = life4[r.Next(0, life1.Length)];
                            mana_cost = 200;
                            max_mana = r.Next(mana_cost, mana_cost + 50);
                            spellcraft = 200;
                        }
                        else if (spellChance < 75)
                        {
                            spellDID = creature5[r.Next(0, creature2.Length)];
                            mana_cost = 250;
                            max_mana = r.Next(mana_cost, mana_cost + 50);
                            spellcraft = 250;
                        }
                        else if (spellChance < 90)
                        {
                            spellDID = life5[r.Next(0, life2.Length)];
                            mana_cost = 250;
                            max_mana = r.Next(mana_cost, mana_cost + 50);
                            spellcraft = 250;
                        }
                        else if (spellChance < 95)
                        {
                            spellDID = creature6[r.Next(0, creature3.Length)];
                            mana_cost = 300;
                            max_mana = r.Next(mana_cost, mana_cost + 50);
                            spellcraft = 300;
                        }
                        else
                        {
                            spellDID = life6[r.Next(0, life3.Length)];
                            mana_cost = 300;
                            max_mana = r.Next(mana_cost, mana_cost + 50);
                            spellcraft = 300;
                        }
                    }
                    workmanship = GetWorkmanship(3);
                    break;
                case 4:
                    //tier 4
                    gemType = t4gems[r.Next(0, t4gems.Length)];
                    if (r.Next(0, 2) == 1)
                    {
                        if (spellChance < 35)
                        {
                            spellDID = creature5[r.Next(0, creature2.Length)];
                            mana_cost = 250;
                            max_mana = r.Next(mana_cost, mana_cost + 50);
                            spellcraft = 250;
                        }
                        else if (spellChance < 70)
                        {
                            spellDID = life5[r.Next(0, life2.Length)];
                            mana_cost = 250;
                            max_mana = r.Next(mana_cost, mana_cost + 50);
                            spellcraft = 250;
                        }
                        else if (spellChance < 85)
                        {
                            spellDID = creature6[r.Next(0, creature3.Length)];
                            mana_cost = 300;
                            max_mana = r.Next(mana_cost, mana_cost + 50);
                            spellcraft = 300;
                        }
                        else
                        {
                            spellDID = life6[r.Next(0, life3.Length)];
                            mana_cost = 300;
                            max_mana = r.Next(mana_cost, mana_cost + 50);
                            spellcraft = 300;
                        }
                    }
                    workmanship = GetWorkmanship(4);
                    break;
                case 5:
                    //tier 5
                    gemType = t5gems[r.Next(0, t5gems.Length)];
                    if (r.Next(0, 2) == 1)
                    {
                        spellChance = r.Next(0, 100);
                        if (spellChance < 30)
                        {
                            spellDID = creature5[r.Next(0, creature1.Length)];
                            mana_cost = 250;
                            max_mana = r.Next(mana_cost, mana_cost + 50);
                            spellcraft = 250;
                        }
                        else if (spellChance < 60)
                        {
                            spellDID = life5[r.Next(0, life1.Length)];
                            mana_cost = 250;
                            max_mana = r.Next(mana_cost, mana_cost + 50);
                            spellcraft = 250;
                        }
                        else if (spellChance < 75)
                        {
                            spellDID = creature6[r.Next(0, creature2.Length)];
                            mana_cost = 300;
                            max_mana = r.Next(mana_cost, mana_cost + 50);
                            spellcraft = 300;
                        }
                        else if (spellChance < 90)
                        {
                            spellDID = life6[r.Next(0, life2.Length)];
                            mana_cost = 300;
                            max_mana = r.Next(mana_cost, mana_cost + 50);
                            spellcraft = 300;
                        }
                        else if (spellChance < 95)
                        {
                            spellDID = creature7[r.Next(0, creature3.Length)];
                            mana_cost = 350;
                            max_mana = r.Next(mana_cost, mana_cost + 50);
                            spellcraft = 350;
                        }
                        else
                        {
                            spellDID = life7[r.Next(0, life3.Length)];
                            mana_cost = 350;
                            max_mana = r.Next(mana_cost, mana_cost + 50);
                            spellcraft = 350;
                        }
                    }
                    workmanship = GetWorkmanship(5);
                    break;
                case 6:
                    //tier 6
                    gemType = t5gems[r.Next(0, t5gems.Length)];
                    if (r.Next(0, 2) == 1)
                    {
                        if (spellChance < 35)
                        {
                            spellDID = creature6[r.Next(0, creature2.Length)];
                            mana_cost = 300;
                            max_mana = r.Next(mana_cost, mana_cost + 50);
                            spellcraft = 300;
                        }
                        else if (spellChance < 70)
                        {
                            spellDID = life6[r.Next(0, life2.Length)];
                            mana_cost = 300;
                            max_mana = r.Next(mana_cost, mana_cost + 50);
                            spellcraft = 300;
                        }
                        else if (spellChance < 85)
                        {
                            spellDID = creature7[r.Next(0, creature3.Length)];
                            mana_cost = 350;
                            max_mana = r.Next(mana_cost, mana_cost + 50);
                            spellcraft = 350;
                        }
                        else
                        {
                            spellDID = life7[r.Next(0, life3.Length)];
                            mana_cost = 350;
                            max_mana = r.Next(mana_cost, mana_cost + 50);
                            spellcraft = 350;
                        }
                    }
                    workmanship = GetWorkmanship(6);
                    break;
                case 7:
                    //tier 7
                    gemType = t5gems[r.Next(0, t5gems.Length)];
                    if (r.Next(0, 2) == 1)
                    {
                        spellChance = r.Next(0, 100);
                        if (spellChance < 30)
                        {
                            spellDID = creature6[r.Next(0, creature1.Length)];
                            mana_cost = 300;
                            max_mana = r.Next(mana_cost, mana_cost + 50);
                            spellcraft = 300;
                        }
                        else if (spellChance < 60)
                        {
                            spellDID = life6[r.Next(0, life1.Length)];
                            mana_cost = 300;
                            max_mana = r.Next(mana_cost, mana_cost + 50);
                            spellcraft = 300;
                        }
                        else if (spellChance < 75)
                        {
                            spellDID = creature7[r.Next(0, creature2.Length)];
                            mana_cost = 350;
                            max_mana = r.Next(mana_cost, mana_cost + 50);
                            spellcraft = 350;
                        }
                        else if (spellChance < 90)
                        {
                            spellDID = life7[r.Next(0, life2.Length)];
                            mana_cost = 350;
                            max_mana = r.Next(mana_cost, mana_cost + 50);
                            spellcraft = 350;
                        }
                        else if (spellChance < 95)
                        {
                            spellDID = creature8[r.Next(0, creature3.Length)];
                            mana_cost = 400;
                            max_mana = r.Next(mana_cost, mana_cost + 50);
                            spellcraft = 400;
                        }
                        else
                        {
                            spellDID = life8[r.Next(0, life3.Length)];
                            mana_cost = 400;
                            max_mana = r.Next(mana_cost, mana_cost + 50);
                            spellcraft = 400;
                        }
                    }
                    workmanship = GetWorkmanship(7);
                    break;
                case 8:
                    //tier 8
                    gemType = t5gems[r.Next(0, t5gems.Length)];
                    if (r.Next(0, 2) == 1)
                    {
                        if (spellChance < 35)
                        {
                            spellDID = creature7[r.Next(0, creature2.Length)];
                            mana_cost = 350;
                            max_mana = r.Next(mana_cost, mana_cost + 50);
                            spellcraft = 350;
                        }
                        else if (spellChance < 70)
                        {
                            spellDID = life7[r.Next(0, life2.Length)];
                            mana_cost = 350;
                            max_mana = r.Next(mana_cost, mana_cost + 50);
                            spellcraft = 350;
                        }
                        else if (spellChance < 85)
                        {
                            spellDID = creature8[r.Next(0, creature3.Length)];
                            mana_cost = 400;
                            max_mana = r.Next(mana_cost, mana_cost + 50);
                            spellcraft = 400;
                        }
                        else
                        {
                            spellDID = life8[r.Next(0, life3.Length)];
                            mana_cost = 400;
                            max_mana = r.Next(mana_cost, mana_cost + 50);
                            spellcraft = 400;
                        }
                    }
                    workmanship = GetWorkmanship(8);
                    break;
                default:
                    break;
            }
            WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)gemType) as Gem;
            wo.SetProperty(PropertyInt.ItemWorkmanship, workmanship);
            if(spellDID > 0)
            {
                wo.SetProperty(PropertyInt.ItemUseable, 8);
            }
            else
            {
                wo.SetProperty(PropertyInt.ItemUseable, 1);
            }
            if (spellDID > 0)
            {
                wo.SetProperty(PropertyInt.UiEffects, 1);
            }
            else
            {
                wo.SetProperty(PropertyInt.UiEffects, 0);
            }
            value = workmanship * r.Next(5, 100) + spellcraft;
            wo.SetProperty(PropertyInt.Value, value);
            wo.SetProperty(PropertyDataId.Spell, (uint)spellDID);
            wo.SetProperty(PropertyInt.ItemAllegianceRankLimit, rank);
            wo.SetProperty(PropertyInt.ItemDifficulty, difficulty);
            wo.SetProperty(PropertyInt.ItemManaCost, mana_cost);
            wo.SetProperty(PropertyInt.ItemMaxMana, max_mana);
            wo.SetProperty(PropertyInt.ItemSkillLevelLimit, skill_level_limit);
            wo.SetProperty(PropertyInt.ItemSpellcraft, spellcraft);
            wo.RemoveProperty(PropertyInt.ItemDifficulty);
            wo.RemoveProperty(PropertyInt.ItemSkillLevelLimit);
            if (spellDID == 0)
            {
                wo.RemoveProperty(PropertyInt.ItemManaCost);
                wo.RemoveProperty(PropertyInt.ItemMaxMana);
                wo.RemoveProperty(PropertyInt.ItemCurMana);
                wo.RemoveProperty(PropertyInt.ItemSpellcraft);
                wo.RemoveProperty(PropertyInt.ItemDifficulty);
                wo.RemoveProperty(PropertyDataId.Spell);
            }
            return wo;
        }

        public static int CreateFood()
        {
            Random r = new Random();
            int foodType = 0;
            int[] food = { 258, 4746, 259, 547, 260, 5758, 261, 262, 263, 264, 265 };
            foodType = food[r.Next(0, 11)];
            return foodType;
        }

        public static WorldObject CreateJewelry(int tier)
        {
            Random r = new Random();
            int[][] JewelrySpells = LootHelper.JewelrySpells;
            int[][] JewelryCantrips = LootHelper.JewelryCantrips;
            int[] jewelryItems = { 621, 295, 297, 294, 623, 622 };
            int jewelType = jewelryItems[r.Next(0, jewelryItems.Length)];
            int numSpells = 0;
            int numCantrips = 0;
            int lowSpellTier = 0;
            int highSpellTier = 0;
            int minorCantrips = GetNumMinorCantrips(tier);
            int majorCantrips = GetNumMajorCantrips(tier);
            int epicCantrips = GetNumEpicCantrips(tier);
            int legendaryCantrips = GetNumLegendaryCantrips(tier);
            int workmanship = 0;
            int value = 100;
            //int rank = 0;
            int difficulty = 0;
            int max_mana = 0;
            //int skill_level_limit = 0;
            int spellcraft = 0; 
            lowSpellTier = GetLowSpellTier(tier);
            highSpellTier = GetHighSpellTier(tier);
            numSpells = GetNumSpells(tier);
            numCantrips = minorCantrips + majorCantrips + epicCantrips + legendaryCantrips;
            if(numCantrips > 10)
            {
                minorCantrips = 0;
            }
            numCantrips = minorCantrips + majorCantrips + epicCantrips + legendaryCantrips;
            WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)jewelType);
            workmanship = GetWorkmanship(tier);
            value = GetValue(tier);
            spellcraft = GetSpellcraft(numSpells, tier);
            max_mana = GetMaxMana(numSpells, tier);
            difficulty = GetDifficulty(tier, spellcraft);
            int mT = GetMaterialType(1, tier);
            wo.SetProperty(PropertyInt.AppraisalLongDescDecoration, 1);
            wo.SetProperty(PropertyInt.MaterialType, mT);
            wo.SetProperty(PropertyInt.Value, value);
            wo.SetProperty(PropertyInt.ItemWorkmanship, workmanship);
            wo.SetProperty(PropertyInt.ItemDifficulty, difficulty);
            wo.SetProperty(PropertyFloat.ManaRate, GetManaRate());
            wo.SetProperty(PropertyInt.ItemMaxMana, max_mana);
            wo.SetProperty(PropertyInt.ItemCurMana, max_mana);
            wo.SetProperty(PropertyInt.ItemSpellcraft, spellcraft);
            wo.RemoveProperty(PropertyInt.ItemSkillLevelLimit);
            int[] shuffledValues = new int[JewelrySpells.Length];
            for (int i = 0; i < JewelrySpells.Length; i++)
            {
                shuffledValues[i] = i;
            }
            Shuffle(shuffledValues);
            if (numSpells - numCantrips > 0)
            {
                wo.SetProperty(PropertyInt.UiEffects, 1);
                for (int a = 0; a < numSpells - numCantrips; a++)
                {
                    int col = r.Next(lowSpellTier - 1, highSpellTier);
                    int spellID = JewelrySpells[shuffledValues[a]][col];
                    var result = new BiotaPropertiesSpellBook { ObjectId = wo.Biota.Id, Spell = spellID, Object = wo.Biota };
                    wo.Biota.BiotaPropertiesSpellBook.Add(result);
                }
            }
            if (numCantrips > 0)
            {
                shuffledValues = new int[JewelryCantrips.Length];
                for (int i = 0; i < JewelryCantrips.Length; i++)
                {
                    shuffledValues[i] = i;
                }
                Shuffle(shuffledValues);
                int shuffledPlace = 0;
                wo.SetProperty(PropertyInt.UiEffects, 1);
                //minor cantripps
                for (int a = 0; a < minorCantrips; a++)
                {
                    int spellID = JewelryCantrips[shuffledValues[shuffledPlace]][0];
                    shuffledPlace++;
                    var result = new BiotaPropertiesSpellBook { ObjectId = wo.Biota.Id, Spell = spellID, Object = wo.Biota };
                    wo.Biota.BiotaPropertiesSpellBook.Add(result);
                }
                //major cantrips
                for (int a = 0; a < majorCantrips; a++)
                {
                    int spellID = JewelryCantrips[shuffledValues[shuffledPlace]][1];
                    shuffledPlace++;
                    var result = new BiotaPropertiesSpellBook { ObjectId = wo.Biota.Id, Spell = spellID, Object = wo.Biota };
                    wo.Biota.BiotaPropertiesSpellBook.Add(result);
                }
                // epic cantrips
                for (int a = 0; a < epicCantrips; a++)
                {
                    int spellID = JewelryCantrips[shuffledValues[shuffledPlace]][2];
                    shuffledPlace++;
                    var result = new BiotaPropertiesSpellBook { ObjectId = wo.Biota.Id, Spell = spellID, Object = wo.Biota };
                    wo.Biota.BiotaPropertiesSpellBook.Add(result);
                }
                //legendary cantrips
                for (int a = 0; a < legendaryCantrips; a++)
                {
                    int spellID = JewelryCantrips[shuffledValues[shuffledPlace]][3];
                    shuffledPlace++;
                    var result = new BiotaPropertiesSpellBook { ObjectId = wo.Biota.Id, Spell = spellID, Object = wo.Biota };
                    wo.Biota.BiotaPropertiesSpellBook.Add(result);
                }
            }
            if (numSpells == 0)
            {
                wo.RemoveProperty(PropertyInt.ItemManaCost);
                wo.RemoveProperty(PropertyInt.ItemMaxMana);
                wo.RemoveProperty(PropertyInt.ItemCurMana);
                wo.RemoveProperty(PropertyInt.ItemSpellcraft);
                wo.RemoveProperty(PropertyInt.ItemDifficulty);
            }
            return wo;
        }

        static void Shuffle<T>(T[] array)
        {
            Random _r = new Random();
            int n = array.Length;
            for (int i = 0; i < n; i++)
            {
                int r = i + _r.Next(n - i);
                T t = array[r];
                array[r] = array[i];
                array[i] = t;
            }
        }

        public static WorldObject CreateWeapon(int tier)
        {
            Random r = new Random();
            int weaponWeenie = 0;
            int weaponType = 0;
            ///Properties for weapons
            int numSpells = GetNumSpells(tier);
            int damageType = 0;  //
            int damage = 0; //
            double damageVariance = 0; //
            double weaponDefense = 0; //
            double weaponOffense = 0; //
            int longDescDecoration = 5; // 
            double magicD = GetMissileDMod(tier);
            double missileD = GetMissileDMod(tier);
            int gemCount = r.Next(1, 6);
            int gemType = r.Next(10, 51);
            int materialType = GetMaterialType(2, tier);
            int workmanship = GetWorkmanship(tier);
            int value = GetValue(tier);
            int uiEffects = 0;
            int spellCraft = GetSpellcraft(numSpells, tier);
            int itemDifficulty = GetDifficulty(tier, spellCraft); ;
            int wieldDiff = GetWield(tier, 3);
            int wieldRequirments = 2;
            int wieldSkillType = 0;
            int maxMana = GetMaxMana(numSpells, tier);
            int subType = 0;

            weaponType = r.Next(0, 5);
            switch (weaponType)
            {
                case 0:
                    ////Heavy Weapons, of which there are 7 sub types
                    wieldSkillType = 44;
                    subType = r.Next(0, 7);
                    if (subType == 0)
                    {
                        weaponDefense = GetMaxDamageMod(tier, 18);
                        weaponOffense = GetMaxDamageMod(tier, 22);
                        damage = GetMaxDamage(1, wieldDiff, 1);
                        damageVariance = GetVariance(1, 1);
                        int subAxeType = r.Next(0, 3);
                        ////There are 4 subtypes of axes
                        if (subAxeType == 0)
                        {
                            ////Battle Axe
                            weaponWeenie = 301;
                        }
                        if (subAxeType == 1)
                        {
                            ////Silifi
                            weaponWeenie = 344;
                        }
                        if (subAxeType == 2)
                        {
                            ////Wr Axe, but this is weenie ID for acid war axe. TO BE FIXED
                            weaponWeenie = 1439;
                        }
                    }
                    if (subType == 1)
                    {
                        damage = GetMaxDamage(1, wieldDiff, 2);
                        weaponDefense = GetMaxDamageMod(tier, 20);
                        weaponOffense = GetMaxDamageMod(tier, 20);
                        ////There are three subtypes of daggers
                        int subDaggerType = r.Next(0, 3);
                        if (subDaggerType == 0)
                        {
                            ////Dirk
                            damageVariance = GetVariance(1, 2);
                            weaponWeenie = 22440;
                        }
                        if (subDaggerType == 1)
                        {
                            ////Stiletto
                            damage = GetMaxDamage(1, wieldDiff, 3);
                            damageVariance = GetVariance(1, 3);
                            weaponWeenie = 7565;
                        }
                        if (subDaggerType == 2)
                        {
                            ////Jambiya
                            damage = GetMaxDamage(1, wieldDiff, 3);
                            damageVariance = GetVariance(1, 3);
                            weaponWeenie = 319;
                        }
                    }
                    if (subType == 2)
                    {
                        ////There are 4 subtypes of maces
                        damage = GetMaxDamage(1, wieldDiff, 4);
                        weaponDefense = GetMaxDamageMod(tier, 22);
                        weaponOffense = GetMaxDamageMod(tier, 18);
                        damageVariance = GetVariance(1, 4);
                        int subMaceType = r.Next(0, 4);
                        if (subMaceType == 0)
                        {
                            ////Flanged Mace
                            weaponWeenie = 30586;
                        }
                        if (subMaceType == 1)
                        {
                            ////Mace
                            weaponWeenie = 331;
                        }
                        if (subMaceType == 2)
                        {
                            ////Mazule
                            weaponWeenie = 30581;
                        }
                        if (subMaceType == 3)
                        {
                            ////Morning Star
                            weaponWeenie = 332;
                        }
                    }
                    if (subType == 3)
                    {
                        ////There are three subtypes of spears
                        damage = GetMaxDamage(1, wieldDiff, 5);
                        weaponDefense = GetMaxDamageMod(tier, 15);
                        weaponOffense = GetMaxDamageMod(tier, 25);
                        damageVariance = GetVariance(1, 5);
                        int subSpearType = r.Next(1, 3);
                        //////
                        //////
                        //////Spine Glaive is not in the database
                        if (subSpearType == 0)
                        {
                            ////Spine Glaive
                            weaponWeenie = 38932;
                        }
                        if (subSpearType == 1)
                        {
                            ////Partizan
                            weaponWeenie = 29972;
                        }
                        if (subSpearType == 2)
                        {
                            ////Trident
                            weaponWeenie = 7772;
                        }
                    }
                    if (subType == 4)
                    {
                        ////There are two subtypes of staves
                        damage = GetMaxDamage(1, wieldDiff, 8);
                        weaponDefense = GetMaxDamageMod(tier, 25);
                        weaponOffense = GetMaxDamageMod(tier, 15);
                        damageVariance = GetVariance(1, 6);
                        int subStaffType = r.Next(0, 1);
                        if (subStaffType == 0)
                        {
                            ////Nabut
                            weaponWeenie = 333;
                        }
                        if (subStaffType == 1)
                        {
                            ////Stick
                            /////This is not in the database
                            weaponWeenie = 31788;
                        }
                    }
                    if (subType == 5)
                    {
                        ////There are six subtypes of swords
                        damage = GetMaxDamage(1, wieldDiff, 6);
                        weaponDefense = GetMaxDamageMod(tier, 20);
                        weaponOffense = GetMaxDamageMod(tier, 20);
                        int subSwordType = r.Next(0, 5);
                        if (subSwordType == 0)
                        {
                            ////Flamberge
                            damageVariance = GetVariance(1, 7);
                            weaponWeenie = 30576;
                        }
                        if (subSwordType == 1)
                        {
                            ////Ken
                            damageVariance = GetVariance(1, 7);
                            weaponWeenie = 327;
                        }
                        if (subSwordType == 2)
                        {
                            ////Long Sword
                            damageVariance = GetVariance(1, 7);
                            weaponWeenie = 351;
                        }
                        if (subSwordType == 3)
                        {
                            ////Tachi
                            damageVariance = GetVariance(1, 7);
                            weaponWeenie = 353;
                        }
                        if (subSwordType == 4)
                        {
                            ////Takuba
                            damageVariance = GetVariance(1, 7);
                            weaponWeenie = 354;
                        }
                        if (subSwordType == 5)
                        {
                            ////Schlager
                            /////Not in DB
                            damage = GetMaxDamage(1, wieldDiff, 7);
                            damageVariance = GetVariance(1, 8);
                            weaponWeenie = 45108;
                        }
                    }
                    if (subType == 6)
                    {
                        ////There are 2 subtypes of UA
                        damage = GetMaxDamage(1, wieldDiff, 9);
                        weaponDefense = GetMaxDamageMod(tier, 20);
                        weaponOffense = GetMaxDamageMod(tier, 20);
                        damageVariance = GetVariance(1, 9);
                        int subUAType = r.Next(0, 2);
                        if (subUAType == 0)
                        {
                            ////Cestus
                            weaponWeenie = 4190;
                            damageType = 4;
                        }
                        if (subUAType == 1)
                        {
                            ////Nekode
                            weaponWeenie = 4195;
                            damageType = 3;
                        }
                    }
                    break;
                case 1:
                    ///Light weapons
                    wieldSkillType = 46;
                    subType = r.Next(0, 6);
                    if (subType == 0)
                    {
                        damage = GetMaxDamage(2, wieldDiff, 1);
                        damageVariance = GetVariance(2, 1);
                        weaponDefense = GetMaxDamageMod(tier, 18);
                        weaponOffense = GetMaxDamageMod(tier, 22);
                        int subAxeType = r.Next(0, 4);
                        ////There are 4 subtypes of axes
                        if (subAxeType == 0)
                        {
                            ////Dolabra
                            weaponWeenie = 30561;
                        }
                        if (subAxeType == 1)
                        {
                            ////Hand Axe
                            weaponWeenie = 303;
                        }
                        if (subAxeType == 2)
                        {
                            ////Ono
                            weaponWeenie = 336;
                        }
                        if (subAxeType == 3)
                        {
                            ////War Hammer
                            weaponWeenie = 359;
                        }
                    }
                    if (subType == 1)
                    {
                        ////There are 2 subtypes of daggers
                        damage = GetMaxDamage(2, wieldDiff, 2);
                        weaponDefense = GetMaxDamageMod(tier, 20);
                        weaponOffense = GetMaxDamageMod(tier, 20);
                        int subDaggerType = r.Next(0, 2);
                        if (subDaggerType == 0)
                        {
                            ////Dagger
                            damageVariance = GetVariance(2, 2);
                            weaponWeenie = 314;
                        }
                        if (subDaggerType == 1)
                        {
                            ////Khanjar
                            damageVariance = GetVariance(2, 3);
                            damage = GetMaxDamage(2, wieldDiff, 3);
                            weaponWeenie = 328;
                        }
                    }
                    if (subType == 2)
                    {
                        damage = GetMaxDamage(2, wieldDiff, 4);
                        damageVariance = GetVariance(2, 4);
                        weaponDefense = GetMaxDamageMod(tier, 22);
                        weaponOffense = GetMaxDamageMod(tier, 18);
                        ////There are 3 subtypes of maces
                        int subMaceType = r.Next(0, 3);
                        if (subMaceType == 0)
                        {
                            ////Club
                            weaponWeenie = 309;
                        }
                        if (subMaceType == 1)
                        {
                            ////Kasrullah
                            weaponWeenie = 325;
                        }
                        if (subMaceType == 2)
                        {
                            ////Spiked Club
                            weaponWeenie = 7768;
                        }
                    }
                    if (subType == 3)
                    {
                        damage = GetMaxDamage(2, wieldDiff, 5);
                        damageVariance = GetVariance(2, 6);
                        weaponDefense = GetMaxDamageMod(tier, 15);
                        weaponOffense = GetMaxDamageMod(tier, 25);
                        ////There are 2 subtypes of spears
                        int subSpearType = r.Next(0, 2);
                        if (subSpearType == 0)
                        {
                            ////Spear
                            weaponWeenie = 348;
                        }
                        if (subSpearType == 1)
                        {
                            ////Yari
                            weaponWeenie = 362;
                        }
                    }
                    if (subType == 4)
                    {
                        damage = GetMaxDamage(2, wieldDiff, 8);
                        damageVariance = GetVariance(2, 7);
                        weaponDefense = GetMaxDamageMod(tier, 25);
                        weaponOffense = GetMaxDamageMod(tier, 15);
                        ////There is 1 subtypes of staves
                        int subStaffType = r.Next(0, 1);
                        if (subStaffType == 0)
                        {
                            ////Quarter Staff
                            weaponWeenie = 338;
                        }
                    }
                    if (subType == 5)
                    {
                        damage = GetMaxDamage(2, wieldDiff, 6);
                        damageVariance = GetVariance(2, 8);
                        ////There are 6 subtypes of swords
                        weaponDefense = GetMaxDamageMod(tier, 20);
                        weaponOffense = GetMaxDamageMod(tier, 20);
                        int subSwordType = r.Next(0, 4);
                        if (subSwordType == 0)
                        {
                            ////Broad Sword
                            weaponWeenie = 350;
                        }
                        //if (subSwordType == 1)
                        //{
                        //    ////Dericost Blade
                        //    ///Not in DB
                        //    weaponWeenie = 40910;
                        //}
                        //if (subSwordType == 1)
                        //{
                        //    ////Epee
                        //    damageVariance = GetVariance(2, 9);
                        //    damage = GetMaxDamage(2, wieldDiff, 7);
                        //    weaponWeenie = 45099;
                        //}
                        if (subSwordType == 1)
                        {
                            ////Kaskara
                            weaponWeenie = 324;
                        }
                        if (subSwordType == 2)
                        {
                            ////Spada
                            weaponWeenie = 30571;
                        }
                        if (subSwordType == 3)
                        {
                            ////Shamshir
                            weaponWeenie = 340;
                        }
                    }
                    if (subType == 6)
                    {
                        damage = GetMaxDamage(2, wieldDiff, 9);
                        weaponDefense = GetMaxDamageMod(tier, 20);
                        weaponOffense = GetMaxDamageMod(tier, 20);
                        damageVariance = GetVariance(2, 10);
                        ////There are 2 subtypes of UA
                        int subUAType = r.Next(0, 2);
                        if (subUAType == 0)
                        {
                            ////Knuckles
                            weaponWeenie = 30611;
                        }
                        if (subUAType == 1)
                        {
                            ////Katar
                            weaponWeenie = 326;
                        }
                    }
                    break;
                case 2:
                    subType = r.Next(0, 6);
                    wieldSkillType = 45;
                    if (subType == 0)
                    {
                        damage = GetMaxDamage(2, wieldDiff, 1);
                        damageVariance = GetVariance(2, 1);
                        weaponDefense = GetMaxDamageMod(tier, 18);
                        weaponOffense = GetMaxDamageMod(tier, 22);
                        int subAxeType = r.Next(1, 4);
                        ////There are 4 subtypes of axes
                        if (subAxeType == 0)
                        {
                            ////Hammer
                            ////Not in DB
                            weaponWeenie = 41420;
                            damageType = 4;
                        }
                        if (subAxeType == 1)
                        {
                            ////Shou-ono
                            weaponWeenie = 342;
                            damageType = 1;
                        }
                        if (subAxeType == 2)
                        {
                            ////Hatchet
                            weaponWeenie = 30556;
                            damageType = 1;
                        }
                        if (subAxeType == 3)
                        {
                            ////Tungi
                            weaponWeenie = 357;
                            damageType = 1;
                        }
                    }
                    if (subType == 1)
                    {
                        damage = GetMaxDamage(2, wieldDiff, 2);
                        damageVariance = GetVariance(2, 2);
                        weaponDefense = GetMaxDamageMod(tier, 20);
                        weaponOffense = GetMaxDamageMod(tier, 20);
                        ////There are 3 subtypes of daggers
                        int subDaggerType = r.Next(0, 2);
                        if (subDaggerType == 0)
                        {
                            ////Knife
                            damageVariance = GetVariance(2, 3);
                            damage = GetMaxDamage(2, wieldDiff, 3);
                            weaponWeenie = 329;
                        }
                        //if (subDaggerType == 1)
                        //{
                        //    ////Lancet
                        //    damageVariance = GetVariance(2, 3);
                        //    damage = GetMaxDamage(2, wieldDiff, 3);
                        //    weaponWeenie = 31794;
                        //}
                        if (subDaggerType == 1)
                        {
                            ////Poniard
                            weaponWeenie = 30946;
                        }
                    }
                    if (subType == 2)
                    {
                        damage = GetMaxDamage(2, wieldDiff, 4);
                        damageVariance = GetVariance(2, 4);
                        weaponDefense = GetMaxDamageMod(tier, 22);
                        weaponOffense = GetMaxDamageMod(tier, 18);
                        ////There are 4 subtypes of maces
                        int subMaceType = r.Next(0, 4);
                        if (subMaceType == 0)
                        {
                            ////Board with Nail
                            weaponWeenie = 7767;
                            damageType = 2;
                        }
                        if (subMaceType == 1)
                        {
                            ////Dabus
                            weaponWeenie = 313;
                            damageType = 4;
                        }
                        if (subMaceType == 2)
                        {
                            ////Tofun
                            weaponWeenie = 356;
                            damageType = 4;
                        }
                        if (subMaceType == 3)
                        {
                            ////Jitte
                            damageVariance = GetVariance(2, 5);
                            weaponWeenie = 321;
                            damageType = 4;
                        }
                    }
                    if (subType == 3)
                    {
                        damage = GetMaxDamage(2, wieldDiff, 5);
                        damageVariance = GetVariance(2, 6);
                        ////There are 2 subtypes of spears
                        weaponDefense = GetMaxDamageMod(tier, 15);
                        weaponOffense = GetMaxDamageMod(tier, 25);
                        int subSpearType = r.Next(0, 2);
                        if (subSpearType == 0)
                        {
                            ////Budiaq
                            weaponWeenie = 308;
                        }
                        if (subSpearType == 1)
                        {
                            ////Naginata
                            weaponWeenie = 7771;
                        }
                    }
                    if (subType == 4)
                    {
                        damage = GetMaxDamage(2, wieldDiff, 8);
                        damageVariance = GetVariance(2, 7);
                        weaponDefense = GetMaxDamageMod(tier, 25);
                        weaponOffense = GetMaxDamageMod(tier, 15);
                        ////There is 2 subtypes of staves
                        int subStaffType = r.Next(0, 2);
                        if (subStaffType == 0)
                        {
                            ////Bastone
                            weaponWeenie = 30606;
                        }
                        if (subStaffType == 1)
                        {
                            ////Jo
                            weaponWeenie = 322;
                        }
                    }
                    if (subType == 5)
                    {
                        damage = GetMaxDamage(2, wieldDiff, 6);
                        damageVariance = GetVariance(2, 8);
                        weaponDefense = GetMaxDamageMod(tier, 20);
                        weaponOffense = GetMaxDamageMod(tier, 20);
                        ////There are 6 subtypes of swords
                        int subSwordType = r.Next(0, 6);
                        if (subSwordType == 0)
                        {
                            ////Rapier
                            damageVariance = GetVariance(2, 9);
                            damage = GetMaxDamage(2, wieldDiff, 7);
                            weaponWeenie = 6853;
                        }
                        if (subSwordType == 1)
                        {
                            ////Sabra
                            weaponWeenie = 30566;
                        }
                        if (subSwordType == 2)
                        {
                            ////Scimitar
                            weaponWeenie = 339;
                        }
                        if (subSwordType == 3)
                        {
                            ////Short Sword
                            weaponWeenie = 352;
                        }
                        if (subSwordType == 4)
                        {
                            ////Simi
                            weaponWeenie = 345;
                        }
                        if (subSwordType == 5)
                        {
                            ////Yaoji
                            weaponWeenie = 361;
                        }
                    }
                    //if (subType == 6)
                    //{
                    //    damage = GetMaxDamage(2, wieldDiff, 9);
                    //    damageVariance = GetVariance(2, 10);
                    //    weaponDefense = GetMaxDamageMod(tier, 20);
                    //    weaponOffense = GetMaxDamageMod(tier, 20);
                    //    ////There are 2 subtypes of UA
                    //    int subUAType = r.Next(0, 2);
                    //    if (subUAType == 0)
                    //    {
                    //        ////Claw
                    //        weaponWeenie = 31784;
                    //        damageType = 3;
                    //    }
                    //    if (subUAType == 1)
                    //    {
                    //        ////Hand Wraps
                    //        weaponWeenie = 45118;
                    //        damageType = 4;
                    //    }
                    //}
                    break;
                //    case 3:
                /////Two handed
                //wieldSkillType = 41;
                //        damageVariance = GetVariance(3, 1);
                //        damage = GetMaxDamage(3, wieldDiff, 1);
                //        subType = r.Next(0, 4);
                //        if (subType == 0)
                //        {
                //            damageVariance = GetVariance(2, 1);
                //            ////There are 4 subtypes of axes

                //            weaponDefense = GetMaxDamageMod(tier, 20);
                //            weaponOffense = GetMaxDamageMod(tier, 20);
                //            int subSwordType = r.Next(0, 3);
                //            if (subSwordType == 0)
                //            {
                //                ////Nodachi
                //                weaponWeenie = 40760;
                //            }
                //            if (subSwordType == 1)
                //            {
                //                ////Shashqa
                //                weaponWeenie = 41067;
                //            }
                //            if (subSwordType == 2)
                //            {
                //                ////Spadone
                //                weaponWeenie = 29975;
                //            }
                //        }
                //        if (subType == 1)
                //        {
                //            ////There are 4 subtypes of Maces
                //            weaponDefense = GetMaxDamageMod(tier, 22);
                //            weaponOffense = GetMaxDamageMod(tier, 18);
                //            int subMaceType = r.Next(0, 4);
                //            if (subMaceType == 0)
                //            {
                //                ////Great Star Mace
                //                weaponWeenie = 41057;
                //            }
                //            if (subMaceType == 1)
                //            {
                //                ////Quadrelle
                //                weaponWeenie = 29965;
                //            }
                //            if (subMaceType == 2)
                //            {
                //                ////Khanda-handled Mace
                //                weaponWeenie = 41062;
                //            }
                //            if (subMaceType == 3)
                //            {
                //                ////Tetsubo
                //                weaponWeenie = 46604;
                //            }
                //        }
                //        if (subType == 2)
                //        {
                //            ////There are 1 subtypes of axes
                //            weaponDefense = GetMaxDamageMod(tier, 18);
                //            weaponOffense = GetMaxDamageMod(tier, 22);
                //            int subAxeType = r.Next(0, 1);
                //            if (subAxeType == 0)
                //            {
                //                ////Greataxe
                //                weaponWeenie = 41052;
                //            }
                //        }
                //        if (subType == 3)
                //        {
                //            ////There are 4 subtypes of spears
                //            damage = GetMaxDamage(3, wieldDiff, 2);
                //            weaponDefense = GetMaxDamageMod(tier, 15);
                //            weaponOffense = GetMaxDamageMod(tier, 25);
                //            int subSpearType = r.Next(0, 4);
                //            if (subSpearType == 0)
                //            {
                //                ////Assagai
                //                weaponWeenie = 41036;
                //            }
                //            if (subSpearType == 1)
                //            {
                //                ////Pike
                //                weaponWeenie = 41046;
                //            }
                //            if (subSpearType == 2)
                //            {
                //                ////Corsesca
                //                weaponWeenie = 40818;
                //            }
                //            if (subSpearType == 3)
                //            {
                //                ////Magari Yari
                //                weaponWeenie = 46605;
                //            }
                //        }
                //        break;
                case 3:
                    return CreateMissileWeapon(tier);
                case 4:
                    return CreateCaster(tier);

            }
            String elementName = "";
            int elementalChance = 0;
            if (numSpells > 0)
            {
                uiEffects = 1;
            }
            if (wieldDiff > 0)
            {
                elementalChance = r.Next(0, 100);
                if (elementalChance > 90)
                {
                    int chance = r.Next(0, 4);
                    switch (chance)
                    {

                        case 0:
                            //cold
                            damageType = 8;
                            uiEffects = 129;
                            elementName = "Frost";
                            break;
                        case 1:
                            //fire
                            damageType = 16;
                            uiEffects = 33;
                            elementName = "Fire";
                            break;
                        case 2:
                            //acid
                            damageType = 32;
                            uiEffects = 247;
                            elementName = "Acid";
                            break;
                        case 3:
                            //electric
                            damageType = 64;
                            uiEffects = 64;
                            elementName = "Electric";
                            break;

                    }
                }
            }
            ///To be done: setting random burdens,
            WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)weaponWeenie);
            wo.SetProperty(PropertyInt.GemCount, gemCount);
            wo.SetProperty(PropertyInt.GemType, gemType);
            wo.SetProperty(PropertyInt.UiEffects, uiEffects);
            wo.SetProperty(PropertyInt.Value, value);
            wo.SetProperty(PropertyInt.Damage, damage);
            wo.SetProperty(PropertyInt.WeaponSkill, wieldSkillType);
            int lowSpellTier = GetLowSpellTier(tier);
            int highSpellTier = GetHighSpellTier(tier);
            int minorCantrips = GetNumMinorCantrips(tier);
            int majorCantrips = GetNumMajorCantrips(tier);
            int epicCantrips = GetNumEpicCantrips(tier);
            int legendaryCantrips = GetNumLegendaryCantrips(tier);
            int numCantrips = minorCantrips + majorCantrips + epicCantrips + legendaryCantrips;
            int[][] spells = LootHelper.MeleeSpells;
            int[][] cantrips = LootHelper.MeleeCantrips;
            if (elementalChance > 90)
            {
                wo.SetProperty(PropertyInt.DamageType, damageType);
            }
            if (numSpells > 0)
            {
                wo.SetProperty(PropertyInt.ItemSpellcraft, spellCraft);
                wo.SetProperty(PropertyInt.ItemDifficulty, itemDifficulty);
                wo.SetProperty(PropertyInt.ItemMaxMana, maxMana);
                wo.SetProperty(PropertyInt.ItemCurMana, maxMana);
                wo.SetProperty(PropertyFloat.ManaRate, GetManaRate());
                int[] shuffledValues = new int[spells.Length];
                for (int i = 0; i < spells.Length; i++)
                {
                    shuffledValues[i] = i;
                }
                Shuffle(shuffledValues);
                if (numSpells - numCantrips > 0)
                {
                    wo.SetProperty(PropertyInt.UiEffects, 1);
                    for (int a = 0; a < numSpells - numCantrips; a++)
                    {
                        int col = r.Next(lowSpellTier - 1, highSpellTier);
                        int spellID = spells[shuffledValues[a]][col];
                        var result = new BiotaPropertiesSpellBook { ObjectId = wo.Biota.Id, Spell = spellID, Object = wo.Biota };
                        wo.Biota.BiotaPropertiesSpellBook.Add(result);
                    }
                }
                if (numCantrips > 0)
                {
                    shuffledValues = new int[cantrips.Length];
                    for (int i = 0; i < cantrips.Length; i++)
                    {
                        shuffledValues[i] = i;
                    }
                    Shuffle(shuffledValues);
                    int shuffledPlace = 0;
                    wo.SetProperty(PropertyInt.UiEffects, 1);
                    //minor cantripps
                    for (int a = 0; a < minorCantrips; a++)
                    {
                        int spellID = cantrips[shuffledValues[shuffledPlace]][0];
                        shuffledPlace++;
                        var result = new BiotaPropertiesSpellBook { ObjectId = wo.Biota.Id, Spell = spellID, Object = wo.Biota };
                        wo.Biota.BiotaPropertiesSpellBook.Add(result);
                    }
                    //major cantrips
                    for (int a = 0; a < majorCantrips; a++)
                    {
                        int spellID = cantrips[shuffledValues[shuffledPlace]][1];
                        shuffledPlace++;
                        var result = new BiotaPropertiesSpellBook { ObjectId = wo.Biota.Id, Spell = spellID, Object = wo.Biota };
                        wo.Biota.BiotaPropertiesSpellBook.Add(result);
                    }
                    // epic cantrips
                    for (int a = 0; a < epicCantrips; a++)
                    {
                        int spellID = cantrips[shuffledValues[shuffledPlace]][2];
                        shuffledPlace++;
                        var result = new BiotaPropertiesSpellBook { ObjectId = wo.Biota.Id, Spell = spellID, Object = wo.Biota };
                        wo.Biota.BiotaPropertiesSpellBook.Add(result);
                    }
                    //legendary cantrips
                    for (int a = 0; a < legendaryCantrips; a++)
                    {
                        int spellID = cantrips[shuffledValues[shuffledPlace]][3];
                        shuffledPlace++;
                        var result = new BiotaPropertiesSpellBook { ObjectId = wo.Biota.Id, Spell = spellID, Object = wo.Biota };
                        wo.Biota.BiotaPropertiesSpellBook.Add(result);
                    }
                }
            }
                wo.SetProperty(PropertyInt.MaterialType, GetMaterialType(2, tier));
                wo.SetProperty(PropertyInt.ItemWorkmanship, workmanship);
                wo.SetProperty(PropertyInt.WieldDifficulty, wieldDiff);
                wo.SetProperty(PropertyInt.WieldRequirements, wieldRequirments);
                wo.SetProperty(PropertyInt.WieldSkilltype, wieldSkillType);
                wo.SetProperty(PropertyInt.AppraisalLongDescDecoration, longDescDecoration);
                wo.SetProperty(PropertyFloat.DamageVariance, damageVariance);
                wo.SetProperty(PropertyFloat.WeaponDefense, weaponDefense);
                wo.SetProperty(PropertyFloat.WeaponOffense, weaponOffense);
                wo.SetProperty(PropertyFloat.WeaponMissileDefense, missileD);
                wo.SetProperty(PropertyFloat.WeaponMagicDefense, magicD);
                if (numSpells == 0)
                {
                    wo.RemoveProperty(PropertyInt.ItemManaCost);
                    wo.RemoveProperty(PropertyInt.ItemMaxMana);
                    wo.RemoveProperty(PropertyInt.ItemCurMana);
                    wo.RemoveProperty(PropertyInt.ItemSpellcraft);
                    wo.RemoveProperty(PropertyInt.ItemDifficulty);
                }
                if (wieldDiff == 0)
                {
                    wo.RemoveProperty(PropertyInt.WieldDifficulty);
                    wo.RemoveProperty(PropertyInt.WieldRequirements);
                    wo.RemoveProperty(PropertyInt.WieldSkilltype);
                }

                return wo;
            }

        public static WorldObject CreateArmor(int tier)
        {
            Random r = new Random();
            int lowSpellTier = 0;
            int highSpellTier = 0;
            ////Double values needed
            double armorModAcid = 0;
            double armorModBludge = 0;
            double armorModCold = 0;
            double armorModElectric = 0;
            double armorModFire = 0;
            double armorModNether = 0;
            double armorModPierce = 0;
            double armorModSlash = 0;
            int spellArray = 0;
            int cantripArray = 0;
            int equipSetId = 0;
            int wieldDifficulty = 0;
            int maxMana = 0;
            int curMana = 0;
            int spellcraft = 0;
            int materialType = 0;
            int armorType = 0;
            int armorPieceType = 0;
            int[][] spells;
            int[][] cantrips;
            int armorWeenie = 0;
            int palette = 0;
            switch (tier)
            {
                case 1:
                    lowSpellTier = 1;
                    highSpellTier = 3;
                    armorType = r.Next(0, 4);
                    ////Leather Armor
                    if (armorType == 0)
                    {
                        int armorPiece = r.Next(0, 16);
                        if (armorPiece == 0)
                        {
                            ////helm
                            armorWeenie = 25636;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 1)
                        {
                            ////head
                            armorWeenie = 25640;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 2)
                        {
                            ////Chest
                            armorWeenie = 25639;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 3)
                        {
                            ////Chest
                            armorWeenie = 25641;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 4)
                        {
                            ////Chest
                            armorWeenie = 25638;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 5)
                        {
                            ////arms
                            armorWeenie = 25651;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 6)
                        {
                            ////Hands
                            armorWeenie = 25642;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 7)
                        {
                            ////Lower Arms
                            armorWeenie = 25637;
                            armorPieceType = 1;
                            spellArray = 4;
                            cantripArray = 4;
                        }
                        else if (armorPiece == 8)
                        {
                            ////Upper arms
                            armorWeenie = 25648;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 9)
                        {
                            ////Abdomen
                            armorWeenie = 25643;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 10)
                        {
                            ////Abdomen
                            armorWeenie = 25650;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 11)
                        {
                            ////legs
                            armorWeenie = 25647;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 12)
                        {
                            ////legs
                            armorWeenie = 25645;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 13)
                        {
                            ////Upper legs
                            armorWeenie = 25652;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 14)
                        {
                            ////lower legs
                            armorWeenie = 25644;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else
                        {
                            ////feet
                            armorWeenie = 25661;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                        }
                        materialType = GetMaterialType(5, 1);
                    }
                    ////Studded Leather
                    if (armorType == 1)
                    {
                        int armorPiece = r.Next(0, 13);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 554;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 116;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 38;
                            armorPieceType = 1;
                            spellArray = 4;
                            cantripArray = 4;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 42;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 48;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 723;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 53;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 7)
                        {
                            armorWeenie = 59;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 8)
                        {
                            armorWeenie = 63;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 9)
                        {
                            armorWeenie = 68;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 10)
                        {
                            armorWeenie = 89;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 11)
                        {
                            armorWeenie = 99;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 12)
                        {
                            armorWeenie = 105;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else
                        {
                            armorWeenie = 112;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(5, 1);
                    }
                    ////Chainmail
                    if (armorType == 2)
                    {
                        ////Thinking a dictionary with random roll/WeenieID would be more concise.
                        int armorPiece = r.Next(0, 13);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 35;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 413;
                            armorPieceType = 1;
                            spellArray = 4;
                            cantripArray = 4;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 414;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 85;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 55;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 415;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 2605;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 7)
                        {
                            armorWeenie = 71;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 8)
                        {
                            armorWeenie = 80;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 9)
                        {
                            armorWeenie = 416;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 10)
                        {
                            armorWeenie = 96;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 11)
                        {
                            armorWeenie = 101;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else
                        {
                            armorWeenie = 108;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Random Armor Items
                    if (armorType == 3)
                    {
                        int armorPiece = r.Next(0, 16);
                        if (armorPiece == 0)
                        {
                            ////bandana
                            armorWeenie = 28612;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 1)
                        {
                            ////beret
                            armorWeenie = 28605;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 2)
                        {
                            ////Cloth Cap
                            armorWeenie = 118;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 3)
                        {
                            ////Cloth gloves
                            armorWeenie = 121;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 4)
                        {
                            ////Cowl
                            armorWeenie = 119;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 5)
                        {
                            ////crown
                            armorWeenie = 296;
                            armorPieceType = 1;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 6)
                        {
                            ////Fez
                            armorWeenie = 5894;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 7)
                        {
                            ////hood
                            armorWeenie = 5905;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 8)
                        {
                            ////Kasa
                            armorWeenie = 5901;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 9)
                        {
                            ////Metal cap
                            armorWeenie = 46;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 10)
                        {
                            ////Qafiya
                            armorWeenie = 76;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 11)
                        {
                            ////turban
                            armorWeenie = 135;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 12)
                        {
                            ////loafers
                            armorWeenie = 28610;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 13)
                        {
                            ////sandals
                            armorWeenie = 129;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 14)
                        {
                            ////shoes
                            armorWeenie = 132;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 15)
                        {
                            ////slippers
                            armorWeenie = 133;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 16)
                        {
                            ////steel toed boots
                            armorWeenie = 7897;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(6, 1);
                        }
                        else if (armorPiece == 17)
                        {
                            ////buckler
                            armorWeenie = 44;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 18)
                        {
                            ////kite shield
                            armorWeenie = 91;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 19)
                        {
                            ////large Kite Shield
                            armorWeenie = 92;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else
                        {
                            ////Round Shield
                            armorWeenie = 93;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                    }
                    break;
                case 2:
                    lowSpellTier = 3;
                    highSpellTier = 5;
                    armorType = r.Next(0, 7);
                    ////Leather Armor
                    if (armorType == 0)
                    {
                        int armorPiece = r.Next(0, 16);
                        if (armorPiece == 0)
                        {
                            ////helm
                            armorWeenie = 25636;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 1)
                        {
                            ////head
                            armorWeenie = 25640;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 2)
                        {
                            ////Chest
                            armorWeenie = 25639;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 3)
                        {
                            ////Chest
                            armorWeenie = 25641;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 4)
                        {
                            ////Chest
                            armorWeenie = 25638;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 5)
                        {
                            ////arms
                            armorWeenie = 25651;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 6)
                        {
                            ////Hands
                            armorWeenie = 25642;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 7)
                        {
                            ////Lower Arms
                            armorWeenie = 25637;
                            armorPieceType = 1;
                            spellArray = 4;
                            cantripArray = 4;
                        }
                        else if (armorPiece == 8)
                        {
                            ////Upper arms
                            armorWeenie = 25648;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 9)
                        {
                            ////Abdomen
                            armorWeenie = 25643;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 10)
                        {
                            ////Abdomen
                            armorWeenie = 25650;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 11)
                        {
                            ////legs
                            armorWeenie = 25647;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 12)
                        {
                            ////legs
                            armorWeenie = 25645;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 13)
                        {
                            ////Upper legs
                            armorWeenie = 25652;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 14)
                        {
                            ////lower legs
                            armorWeenie = 25644;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else
                        {
                            ////feet
                            armorWeenie = 25661;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                        }
                        materialType = GetMaterialType(5, 1);
                    }
                    ////Studded Leather
                    if (armorType == 1)
                    {
                        ////Thinking a dictionary with random roll/WeenieID would be more concise.
                        int armorPiece = r.Next(0, 15);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 554;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 116;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 38;
                            armorPieceType = 1;
                            spellArray = 4;
                            cantripArray = 4;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 42;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 48;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 723;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 53;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 7)
                        {
                            armorWeenie = 59;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 8)
                        {
                            armorWeenie = 63;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 9)
                        {
                            armorWeenie = 68;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 10)
                        {
                            armorWeenie = 68;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 11)
                        {
                            armorWeenie = 89;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 12)
                        {
                            armorWeenie = 99;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 13)
                        {
                            armorWeenie = 105;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else
                        {
                            armorWeenie = 112;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(5, 1);
                    }
                    ////Chainmail
                    if (armorType == 2)
                    {
                        int armorPiece = r.Next(0, 13);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 35;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 413;
                            armorPieceType = 1;
                            spellArray = 4;
                            cantripArray = 4;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 414;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 85;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 55;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 415;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 2605;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 7)
                        {
                            armorWeenie = 71;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 8)
                        {
                            armorWeenie = 80;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 9)
                        {
                            armorWeenie = 416;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 10)
                        {
                            armorWeenie = 96;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 11)
                        {
                            armorWeenie = 101;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else
                        {
                            armorWeenie = 108;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Platemail
                    if (armorType == 3)
                    {
                        int armorPiece = r.Next(0, 11);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 40;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 51;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 57;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 61;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 66;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 72;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 82;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 7)
                        {
                            armorWeenie = 87;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 8)
                        {
                            armorWeenie = 103;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 9)
                        {
                            armorWeenie = 110;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 114;
                            armorPieceType = 1;
                            spellArray = 4;
                            cantripArray = 4;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Scalemail
                    if (armorType == 4)
                    {
                        int armorPiece = r.Next(0, 14);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 552;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 37;
                            armorPieceType = 1;
                            spellArray = 4;
                            cantripArray = 4;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 41;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 793;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 52;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 58;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 62;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 7)
                        {
                            armorWeenie = 67;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 8)
                        {
                            armorWeenie = 73;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 9)
                        {
                            armorWeenie = 83;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 10)
                        {
                            armorWeenie = 88;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 11)
                        {
                            armorWeenie = 98;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 12)
                        {
                            armorWeenie = 104;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else
                        {
                            armorWeenie = 111;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Yoroi
                    if (armorType == 5)
                    {
                        int armorPiece = r.Next(0, 8);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 43;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 54;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 64;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 69;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 2437;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 90;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 102;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else
                        {
                            armorWeenie = 113;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Diforsa
                    if (armorType == 6)
                    {
                        int armorPiece = r.Next(0, 13);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 28367;
                            armorPieceType = 1;
                            spellArray = 4;
                            cantripArray = 4;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 28628;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 28630;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 28632;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 28633;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 28634;
                            armorPieceType = 31;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 30948;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 7)
                        {
                            armorWeenie = 28618;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 8)
                        {
                            armorWeenie = 28621;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 9)
                        {
                            armorWeenie = 28623;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 10)
                        {
                            armorWeenie = 30949;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 11)
                        {
                            armorWeenie = 28625;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                        }
                        else
                        {
                            armorWeenie = 28626;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Random Armor Items
                    if (armorType == 7)
                    {
                        int armorPiece = r.Next(0, 30);
                        if (armorPiece == 0)
                        {
                            ////bandana
                            armorWeenie = 28612;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 1)
                        {
                            ////beret
                            armorWeenie = 28605;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 2)
                        {
                            ////Cloth Cap
                            armorWeenie = 118;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 3)
                        {
                            ////Cloth gloves
                            armorWeenie = 121;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 4)
                        {
                            ////Cowl
                            armorWeenie = 119;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 5)
                        {
                            ////crown
                            armorWeenie = 296;
                            armorPieceType = 1;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 6)
                        {
                            ////Fez
                            armorWeenie = 5894;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 7)
                        {
                            ////hood
                            armorWeenie = 5905;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 8)
                        {
                            ////Kasa
                            armorWeenie = 5901;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 9)
                        {
                            ////Metal cap
                            armorWeenie = 46;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 10)
                        {
                            ////Qafiya
                            armorWeenie = 76;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 11)
                        {
                            ////turban
                            armorWeenie = 135;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 12)
                        {
                            ////loafers
                            armorWeenie = 28610;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 13)
                        {
                            ////sandals
                            armorWeenie = 129;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 14)
                        {
                            ////shoes
                            armorWeenie = 132;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 15)
                        {
                            ////slippers
                            armorWeenie = 133;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 16)
                        {
                            ////steel toed boots
                            armorWeenie = 7897;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(6, 1);
                        }
                        else if (armorPiece == 17)
                        {
                            ////buckler
                            armorWeenie = 44;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 18)
                        {
                            ////kite shield
                            armorWeenie = 91;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 19)
                        {
                            ////large Kite Shield
                            armorWeenie = 92;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 20)
                        {
                            ////Circlet
                            armorWeenie = 29528;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 21)
                        {
                            ////Armet
                            armorWeenie = 8488;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 22)
                        {
                            ////Baigha
                            armorWeenie = 550;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 23)
                        {
                            ////Heaume
                            armorWeenie = 74;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 24)
                        {
                            ////Helmet
                            armorWeenie = 75;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        //else if (armorPiece == 25)
                        //{
                        //    ////Horned Helm
                        //    armorWeenie = 92;
                        //    armorPieceType = 5;
                        //    spellArray = 10;
                        //    cantripArray = 10;
                        //}
                        else if (armorPiece == 25)
                        {
                            ////Kabuton
                            armorWeenie = 77;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 26)
                        {
                            ////sollerets
                            armorWeenie = 107;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 27)
                        {
                            ////viamontian laced boots
                            armorWeenie = 28611;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 28)
                        {
                            ////RTower Shield
                            armorWeenie = 95;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else
                        {
                            ////Round Shield
                            armorWeenie = 93;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                    }
                    break;
                case 3:
                    lowSpellTier = 4;
                    highSpellTier = 6;
                    armorType = r.Next(0, 11);
                    ////Leather Armor
                    if (armorType == 0)
                    {
                        int armorPiece = r.Next(0, 16);
                        if (armorPiece == 0)
                        {
                            ////helm
                            armorWeenie = 25636;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 1)
                        {
                            ////head
                            armorWeenie = 25640;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 2)
                        {
                            ////Chest
                            armorWeenie = 25639;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 3)
                        {
                            ////Chest
                            armorWeenie = 25641;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 4)
                        {
                            ////Chest
                            armorWeenie = 25638;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 5)
                        {
                            ////arms
                            armorWeenie = 25651;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 6)
                        {
                            ////Hands
                            armorWeenie = 25642;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 7)
                        {
                            ////Lower Arms
                            armorWeenie = 25637;
                            armorPieceType = 1;
                            spellArray = 4;
                            cantripArray = 4;
                        }
                        else if (armorPiece == 8)
                        {
                            ////Upper arms
                            armorWeenie = 25648;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 9)
                        {
                            ////Abdomen
                            armorWeenie = 25643;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 10)
                        {
                            ////Abdomen
                            armorWeenie = 25650;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 11)
                        {
                            ////legs
                            armorWeenie = 25647;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 12)
                        {
                            ////legs
                            armorWeenie = 25645;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 13)
                        {
                            ////Upper legs
                            armorWeenie = 25652;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 14)
                        {
                            ////lower legs
                            armorWeenie = 25644;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else
                        {
                            ////feet
                            armorWeenie = 25661;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                        }
                        materialType = GetMaterialType(5, 1);
                    }
                    ////Studded Leather
                    if (armorType == 1)
                    {
                        int armorPiece = r.Next(0, 15);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 554;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 116;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 38;
                            armorPieceType = 1;
                            spellArray = 4;
                            cantripArray = 4;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 42;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 48;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 723;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 53;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 7)
                        {
                            armorWeenie = 59;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 8)
                        {
                            armorWeenie = 63;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 9)
                        {
                            armorWeenie = 68;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 10)
                        {
                            armorWeenie = 68;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 11)
                        {
                            armorWeenie = 89;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 12)
                        {
                            armorWeenie = 99;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 13)
                        {
                            armorWeenie = 105;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else
                        {
                            armorWeenie = 112;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(5, 1);
                    }
                    ////Chainmail
                    if (armorType == 2)
                    {
                        int armorPiece = r.Next(0, 13);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 35;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 413;
                            armorPieceType = 1;
                            spellArray = 4;
                            cantripArray = 4;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 414;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 85;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 55;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 415;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 2605;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 7)
                        {
                            armorWeenie = 71;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 8)
                        {
                            armorWeenie = 80;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 9)
                        {
                            armorWeenie = 416;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 10)
                        {
                            armorWeenie = 96;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 11)
                        {
                            armorWeenie = 101;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else
                        {
                            armorWeenie = 108;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Platemail
                    if (armorType == 3)
                    {
                        int armorPiece = r.Next(0, 12);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 40;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 51;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 57;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 61;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 66;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 72;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 82;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 7)
                        {
                            armorWeenie = 87;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 8)
                        {
                            armorWeenie = 103;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 9)
                        {
                            armorWeenie = 110;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 114;
                            armorPieceType = 1;
                            spellArray = 4;
                            cantripArray = 4;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Scalemail
                    if (armorType == 4)
                    {
                        int armorPiece = r.Next(0, 14);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 552;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 37;
                            armorPieceType = 1;
                            spellArray = 4;
                            cantripArray = 4;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 41;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 793;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 52;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 58;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 62;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 7)
                        {
                            armorWeenie = 67;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 8)
                        {
                            armorWeenie = 73;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 9)
                        {
                            armorWeenie = 83;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 10)
                        {
                            armorWeenie = 88;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 11)
                        {
                            armorWeenie = 98;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 12)
                        {
                            armorWeenie = 104;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else
                        {
                            armorWeenie = 111;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Yoroi
                    if (armorType == 5)
                    {
                        int armorPiece = r.Next(0, 8);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 43;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 54;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 64;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 69;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 2437;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 90;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 102;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else
                        {
                            armorWeenie = 113;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Diforsa
                    if (armorType == 6)
                    {
                        int armorPiece = r.Next(0, 13);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 28367;
                            armorPieceType = 1;
                            spellArray = 4;
                            cantripArray = 4;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 28628;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 28630;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 28632;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 28633;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 28634;
                            armorPieceType = 31;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 30948;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 7)
                        {
                            armorWeenie = 28618;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 8)
                        {
                            armorWeenie = 28621;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 9)
                        {
                            armorWeenie = 28623;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 10)
                        {
                            armorWeenie = 30949;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 11)
                        {
                            armorWeenie = 28625;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                        }
                        else
                        {
                            armorWeenie = 28626;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////celdon
                    if (armorType == 7)
                    {
                        int armorPiece = r.Next(0, 4);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 6044;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 6043;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 6045;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 6048;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ///Amuli
                    if (armorType == 8)
                    {
                        int armorPiece = r.Next(0, 2);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 6046;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else
                        {
                            armorWeenie = 6047;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(6, 1);
                    }
                    ////Koujia
                    if (armorType == 9)
                    {
                        int armorPiece = r.Next(0, 3);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 6003;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 6004;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 6005;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Tenassa
                    if (armorType == 10)
                    {
                        int armorPiece = r.Next(0, 3);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 31026;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 28622;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 28624;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    if (armorType == 11)
                    {
                        int armorPiece = r.Next(0, 30);
                        if (armorPiece == 0)
                        {
                            ////bandana
                            armorWeenie = 28612;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 1)
                        {
                            ////beret
                            armorWeenie = 28605;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 2)
                        {
                            ////Cloth Cap
                            armorWeenie = 118;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 3)
                        {
                            ////Cloth gloves
                            armorWeenie = 121;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 4)
                        {
                            ////Cowl
                            armorWeenie = 119;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 5)
                        {
                            ////crown
                            armorWeenie = 296;
                            armorPieceType = 1;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 6)
                        {
                            ////Fez
                            armorWeenie = 5894;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 7)
                        {
                            ////hood
                            armorWeenie = 5905;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 8)
                        {
                            ////Kasa
                            armorWeenie = 5901;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 9)
                        {
                            ////Metal cap
                            armorWeenie = 46;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 10)
                        {
                            ////Qafiya
                            armorWeenie = 76;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 11)
                        {
                            ////turban
                            armorWeenie = 135;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 12)
                        {
                            ////loafers
                            armorWeenie = 28610;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 13)
                        {
                            ////sandals
                            armorWeenie = 129;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 14)
                        {
                            ////shoes
                            armorWeenie = 132;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 15)
                        {
                            ////slippers
                            armorWeenie = 133;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 16)
                        {
                            ////steel toed boots
                            armorWeenie = 7897;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(6, 1);
                        }
                        else if (armorPiece == 17)
                        {
                            ////buckler
                            armorWeenie = 44;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 18)
                        {
                            ////kite shield
                            armorWeenie = 91;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 19)
                        {
                            ////large Kite Shield
                            armorWeenie = 92;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 20)
                        {
                            ////Circlet
                            armorWeenie = 29528;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 21)
                        {
                            ////Armet
                            armorWeenie = 8488;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 22)
                        {
                            ////Baigha
                            armorWeenie = 550;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 23)
                        {
                            ////Heaume
                            armorWeenie = 74;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 24)
                        {
                            ////Helmet
                            armorWeenie = 75;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        //else if (armorPiece == 25)
                        //{
                        //    ////Horned Helm
                        //    armorWeenie = 92;
                        //    armorPieceType = 5;
                        //    spellArray = 10;
                        //    cantripArray = 10;
                        //}
                        else if (armorPiece == 25)
                        {
                            ////Kabuton
                            armorWeenie = 77;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 26)
                        {
                            ////sollerets
                            armorWeenie = 107;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 27)
                        {
                            ////viamontian laced boots
                            armorWeenie = 28611;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 28)
                        {
                            ////RTower Shield
                            armorWeenie = 95;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        //else if (armorPiece == 29)
                        //{
                        //    ////Signet Crown
                        //    armorWeenie = 31868;
                        //    armorPieceType = 2;
                        //    spellArray = 1;
                        //    cantripArray = 1;
                        //    materialType = GetMaterialType(7, 1);
                        //}
                        //else if (armorPiece == 30)
                        //{
                        //    ////Suikan Over-Robe
                        //    armorWeenie = 44801;
                        //    equipSetId = 15;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 31)
                        //{
                        //    ////Faran Over-Robe
                        //    armorWeenie = 44799;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 32)
                        //{
                        //    ////Dho Vest and  Over-Robe
                        //    armorWeenie = 44800;
                        //    equipSetId = 14;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 33)
                        //{
                        //    ////Suikan Over-Robe
                        //    armorWeenie = 44802;
                        //    ////equipSetId = 15;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        else
                        {
                            ////Round Shield
                            armorWeenie = 93;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                    }
                    break;
                case 4:
                    lowSpellTier = 5;
                    highSpellTier = 6;
                    armorType = r.Next(0, 11);
                    ////Leather Armor
                    if (armorType == 0)
                    {
                        int armorPiece = r.Next(0, 16);
                        if (armorPiece == 0)
                        {
                            ////helm
                            armorWeenie = 25636;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 1)
                        {
                            ////head
                            armorWeenie = 25640;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 2)
                        {
                            ////Chest
                            armorWeenie = 25639;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 3)
                        {
                            ////Chest
                            armorWeenie = 25641;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 4)
                        {
                            ////Chest
                            armorWeenie = 25638;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 5)
                        {
                            ////arms
                            armorWeenie = 25651;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 6)
                        {
                            ////Hands
                            armorWeenie = 25642;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 7)
                        {
                            ////Lower Arms
                            armorWeenie = 25637;
                            armorPieceType = 1;
                            spellArray = 4;
                            cantripArray = 4;
                        }
                        else if (armorPiece == 8)
                        {
                            ////Upper arms
                            armorWeenie = 25648;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 9)
                        {
                            ////Abdomen
                            armorWeenie = 25643;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 10)
                        {
                            ////Abdomen
                            armorWeenie = 25650;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 11)
                        {
                            ////legs
                            armorWeenie = 25647;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 12)
                        {
                            ////legs
                            armorWeenie = 25645;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 13)
                        {
                            ////Upper legs
                            armorWeenie = 25652;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 14)
                        {
                            ////lower legs
                            armorWeenie = 25644;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else
                        {
                            ////feet
                            armorWeenie = 25661;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                        }
                        materialType = GetMaterialType(5, 1);
                    }
                    ////Studded Leather
                    if (armorType == 1)
                    {
                        ////Thinking a dictionary with random roll/WeenieID would be more concise.
                        int armorPiece = r.Next(0, 15);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 554;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 116;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 38;
                            armorPieceType = 1;
                            spellArray = 4;
                            cantripArray = 4;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 42;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 48;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 723;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 53;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 7)
                        {
                            armorWeenie = 59;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 8)
                        {
                            armorWeenie = 63;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 9)
                        {
                            armorWeenie = 68;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 10)
                        {
                            armorWeenie = 68;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 11)
                        {
                            armorWeenie = 89;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 12)
                        {
                            armorWeenie = 99;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 13)
                        {
                            armorWeenie = 105;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else
                        {
                            armorWeenie = 112;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(5, 1);
                    }
                    ////Chainmail
                    if (armorType == 2)
                    {
                        int armorPiece = r.Next(0, 13);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 35;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 413;
                            armorPieceType = 1;
                            spellArray = 4;
                            cantripArray = 4;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 414;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 85;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 55;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 415;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 2605;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 7)
                        {
                            armorWeenie = 71;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 8)
                        {
                            armorWeenie = 80;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 9)
                        {
                            armorWeenie = 416;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 10)
                        {
                            armorWeenie = 96;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 11)
                        {
                            armorWeenie = 101;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else
                        {
                            armorWeenie = 108;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Platemail
                    if (armorType == 3)
                    {
                        int armorPiece = r.Next(0, 11);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 40;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 51;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 57;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 61;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 66;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 72;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 82;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 7)
                        {
                            armorWeenie = 87;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 8)
                        {
                            armorWeenie = 103;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 9)
                        {
                            armorWeenie = 110;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 114;
                            armorPieceType = 1;
                            spellArray = 4;
                            cantripArray = 4;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Scalemail
                    if (armorType == 4)
                    {
                        int armorPiece = r.Next(0, 14);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 552;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 37;
                            armorPieceType = 1;
                            spellArray = 4;
                            cantripArray = 4;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 41;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 793;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 52;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 58;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 62;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 7)
                        {
                            armorWeenie = 67;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 8)
                        {
                            armorWeenie = 73;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 9)
                        {
                            armorWeenie = 83;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 10)
                        {
                            armorWeenie = 88;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 11)
                        {
                            armorWeenie = 98;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 12)
                        {
                            armorWeenie = 104;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else
                        {
                            armorWeenie = 111;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Yoroi
                    if (armorType == 5)
                    {
                        int armorPiece = r.Next(0, 8);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 43;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 54;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 64;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 69;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 2437;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 90;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 102;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else
                        {
                            armorWeenie = 113;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Diforsa
                    if (armorType == 6)
                    {
                        int armorPiece = r.Next(0, 13);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 28367;
                            armorPieceType = 1;
                            spellArray = 4;
                            cantripArray = 4;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 28628;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 28630;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 28632;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 28633;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 28634;
                            armorPieceType = 31;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 30948;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 7)
                        {
                            armorWeenie = 28618;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 8)
                        {
                            armorWeenie = 28621;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 9)
                        {
                            armorWeenie = 28623;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 10)
                        {
                            armorWeenie = 30949;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 11)
                        {
                            armorWeenie = 28625;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                        }
                        else
                        {
                            armorWeenie = 28626;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////celdon
                    if (armorType == 7)
                    {
                        int armorPiece = r.Next(0, 4);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 6044;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 6043;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 6045;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 6048;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ///Amuli
                    if (armorType == 8)
                    {
                        int armorPiece = r.Next(0, 2);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 6046;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else
                        {
                            armorWeenie = 6047;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(6, 1);
                    }
                    ////Koujia
                    if (armorType == 9)
                    {
                        int armorPiece = r.Next(0, 3);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 6003;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 6004;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 6005;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Tenassa
                    if (armorType == 10)
                    {
                        int armorPiece = r.Next(0, 3);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 31026;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 28622;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 28624;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    if (armorType == 11)
                    {
                        int armorPiece = r.Next(0, 30);
                        if (armorPiece == 0)
                        {
                            ////bandana
                            armorWeenie = 28612;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 1)
                        {
                            ////beret
                            armorWeenie = 28605;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 2)
                        {
                            ////Cloth Cap
                            armorWeenie = 118;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 3)
                        {
                            ////Cloth gloves
                            armorWeenie = 121;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 4)
                        {
                            ////Cowl
                            armorWeenie = 119;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 5)
                        {
                            ////crown
                            armorWeenie = 296;
                            armorPieceType = 1;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 6)
                        {
                            ////Fez
                            armorWeenie = 5894;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 7)
                        {
                            ////hood
                            armorWeenie = 5905;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 8)
                        {
                            ////Kasa
                            armorWeenie = 5901;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 9)
                        {
                            ////Metal cap
                            armorWeenie = 46;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 10)
                        {
                            ////Qafiya
                            armorWeenie = 76;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 11)
                        {
                            ////turban
                            armorWeenie = 135;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 12)
                        {
                            ////loafers
                            armorWeenie = 28610;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 13)
                        {
                            ////sandals
                            armorWeenie = 129;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 14)
                        {
                            ////shoes
                            armorWeenie = 132;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 15)
                        {
                            ////slippers
                            armorWeenie = 133;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 16)
                        {
                            ////steel toed boots
                            armorWeenie = 7897;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(6, 1);
                        }
                        else if (armorPiece == 17)
                        {
                            ////buckler
                            armorWeenie = 44;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 18)
                        {
                            ////kite shield
                            armorWeenie = 91;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 19)
                        {
                            ////large Kite Shield
                            armorWeenie = 92;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 20)
                        {
                            ////Circlet
                            armorWeenie = 29528;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 21)
                        {
                            ////Armet
                            armorWeenie = 8488;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 22)
                        {
                            ////Baigha
                            armorWeenie = 550;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 23)
                        {
                            ////Heaume
                            armorWeenie = 74;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 24)
                        {
                            ////Helmet
                            armorWeenie = 75;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        //else if (armorPiece == 25)
                        //{
                        //    ////Horned Helm
                        //    armorWeenie = 92;
                        //    armorPieceType = 5;
                        //    spellArray = 10;
                        //    cantripArray = 10;
                        //}
                        else if (armorPiece == 25)
                        {
                            ////Kabuton
                            armorWeenie = 77;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 26)
                        {
                            ////sollerets
                            armorWeenie = 107;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 27)
                        {
                            ////viamontian laced boots
                            armorWeenie = 28611;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 28)
                        {
                            ////RTower Shield
                            armorWeenie = 95;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        //else if (armorPiece == 29)
                        //{
                        //    ////Signet Crown
                        //    armorWeenie = 31868;
                        //    armorPieceType = 2;
                        //    spellArray = 1;
                        //    cantripArray = 1;
                        //    materialType = GetMaterialType(7, 1);
                        //}
                        //else if (armorPiece == 30)
                        //{
                        //    ////Suikan Over-Robe
                        //    armorWeenie = 44801;
                        //    equipSetId = 15;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 31)
                        //{
                        //    ////Faran Over-Robe
                        //    armorWeenie = 44799;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 32)
                        //{
                        //    ////Dho Vest and  Over-Robe
                        //    armorWeenie = 44800;
                        //    equipSetId = 14;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 33)
                        //{
                        //    ////Suikan Over-Robe
                        //    armorWeenie = 44802;
                        //    ////equipSetId = 15;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        else
                        {
                            ////Round Shield
                            armorWeenie = 93;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                    }
                    break;
                case 5:
                    lowSpellTier = 5;
                    highSpellTier = 7;
                    armorType = r.Next(0, 15);
                    ////Leather Armor
                    if (armorType == 0)
                    {
                        int armorPiece = r.Next(0, 16);
                        if (armorPiece == 0)
                        {
                            ////helm
                            armorWeenie = 25636;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 1)
                        {
                            ////head
                            armorWeenie = 25640;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 2)
                        {
                            ////Chest
                            armorWeenie = 25639;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 3)
                        {
                            ////Chest
                            armorWeenie = 25641;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 4)
                        {
                            ////Chest
                            armorWeenie = 25638;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 5)
                        {
                            ////arms
                            armorWeenie = 25651;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 6)
                        {
                            ////Hands
                            armorWeenie = 25642;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 7)
                        {
                            ////Lower Arms
                            armorWeenie = 25637;
                            armorPieceType = 1;
                            spellArray = 4;
                            cantripArray = 4;
                        }
                        else if (armorPiece == 8)
                        {
                            ////Upper arms
                            armorWeenie = 25648;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 9)
                        {
                            ////Abdomen
                            armorWeenie = 25643;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 10)
                        {
                            ////Abdomen
                            armorWeenie = 25650;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 11)
                        {
                            ////legs
                            armorWeenie = 25647;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 12)
                        {
                            ////legs
                            armorWeenie = 25645;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 13)
                        {
                            ////Upper legs
                            armorWeenie = 25652;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 14)
                        {
                            ////lower legs
                            armorWeenie = 25644;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else
                        {
                            ////feet
                            armorWeenie = 25661;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                        }
                        materialType = GetMaterialType(5, 1);
                    }
                    ////Studded Leather
                    if (armorType == 1)
                    {
                        int armorPiece = r.Next(0, 15);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 554;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 116;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 38;
                            armorPieceType = 1;
                            spellArray = 4;
                            cantripArray = 4;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 42;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 48;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 723;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 53;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 7)
                        {
                            armorWeenie = 59;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 8)
                        {
                            armorWeenie = 63;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 9)
                        {
                            armorWeenie = 68;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 10)
                        {
                            armorWeenie = 68;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 11)
                        {
                            armorWeenie = 89;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 12)
                        {
                            armorWeenie = 99;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 13)
                        {
                            armorWeenie = 105;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else
                        {
                            armorWeenie = 112;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(5, 1);
                    }
                    ////Chainmail
                    if (armorType == 2)
                    {
                        int armorPiece = r.Next(0, 13);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 35;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 413;
                            armorPieceType = 1;
                            spellArray = 4;
                            cantripArray = 4;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 414;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 85;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 55;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 415;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 2605;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 7)
                        {
                            armorWeenie = 71;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 8)
                        {
                            armorWeenie = 80;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 9)
                        {
                            armorWeenie = 416;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 10)
                        {
                            armorWeenie = 96;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 11)
                        {
                            armorWeenie = 101;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else
                        {
                            armorWeenie = 108;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Platemail
                    if (armorType == 3)
                    {
                        int armorPiece = r.Next(0, 11);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 40;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 51;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 57;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 61;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 66;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 72;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 82;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 7)
                        {
                            armorWeenie = 87;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 8)
                        {
                            armorWeenie = 103;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 9)
                        {
                            armorWeenie = 110;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 114;
                            armorPieceType = 1;
                            spellArray = 4;
                            cantripArray = 4;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Scalemail
                    if (armorType == 4)
                    {
                        int armorPiece = r.Next(0, 14);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 552;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 37;
                            armorPieceType = 1;
                            spellArray = 4;
                            cantripArray = 4;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 41;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 793;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 52;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 58;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 62;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 7)
                        {
                            armorWeenie = 67;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 8)
                        {
                            armorWeenie = 73;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 9)
                        {
                            armorWeenie = 83;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 10)
                        {
                            armorWeenie = 88;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 11)
                        {
                            armorWeenie = 98;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 12)
                        {
                            armorWeenie = 104;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else
                        {
                            armorWeenie = 111;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Yoroi
                    if (armorType == 5)
                    {
                        int armorPiece = r.Next(0, 8);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 43;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 54;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 64;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 69;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 2437;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 90;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 102;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else
                        {
                            armorWeenie = 113;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Diforsa
                    if (armorType == 6)
                    {
                        int armorPiece = r.Next(0, 13);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 28367;
                            armorPieceType = 1;
                            spellArray = 4;
                            cantripArray = 4;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 28628;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 28630;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 28632;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 28633;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 28634;
                            armorPieceType = 31;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 30948;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 7)
                        {
                            armorWeenie = 28618;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 8)
                        {
                            armorWeenie = 28621;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 9)
                        {
                            armorWeenie = 28623;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 10)
                        {
                            armorWeenie = 30949;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 11)
                        {
                            armorWeenie = 28625;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                        }
                        else
                        {
                            armorWeenie = 28626;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////celdon
                    if (armorType == 7)
                    {
                        int armorPiece = r.Next(0, 4);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 6044;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 6043;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 6045;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 6048;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ///Amuli
                    if (armorType == 8)
                    {
                        int armorPiece = r.Next(0, 2);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 6046;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else
                        {
                            armorWeenie = 6047;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(6, 1);
                    }
                    ////Koujia
                    if (armorType == 9)
                    {
                        int armorPiece = r.Next(0, 3);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 6003;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 6004;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 6005;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Tenassa
                    if (armorType == 10)
                    {
                        int armorPiece = r.Next(0, 3);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 31026;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 28622;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 28624;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Lorica
                    if (armorType == 11)
                    {
                        int armorPiece = r.Next(0, 6);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 27220;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 27221;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 27222;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 27223;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 27224;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 27225;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Nariyid
                    if (armorType == 12)
                    {
                        int armorPiece = r.Next(0, 7);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 27226;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 27227;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 27228;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 27229;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 27230;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 27231;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 27232;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Chiran
                    if (armorType == 13)
                    {
                        int armorPiece = r.Next(0, 5);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 27215;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 27216;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 27217;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 27218;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 27219;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Alduressa
                    if (armorType == 14)
                    {
                        int armorPiece = r.Next(0, 5);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 30950;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 28629;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 30951;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 28617;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else
                        {
                            armorWeenie = 28620;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    if (armorType == 15)
                    {
                        int armorPiece = r.Next(0, 30);
                        if (armorPiece == 0)
                        {
                            ////bandana
                            armorWeenie = 28612;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 1)
                        {
                            ////beret
                            armorWeenie = 28605;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 2)
                        {
                            ////Cloth Cap
                            armorWeenie = 118;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 3)
                        {
                            ////Cloth gloves
                            armorWeenie = 121;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 4)
                        {
                            ////Cowl
                            armorWeenie = 119;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 5)
                        {
                            ////crown
                            armorWeenie = 296;
                            armorPieceType = 1;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 6)
                        {
                            ////Fez
                            armorWeenie = 5894;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 7)
                        {
                            ////hood
                            armorWeenie = 5905;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 8)
                        {
                            ////Kasa
                            armorWeenie = 5901;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 9)
                        {
                            ////Metal cap
                            armorWeenie = 46;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 10)
                        {
                            ////Qafiya
                            armorWeenie = 76;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 11)
                        {
                            ////turban
                            armorWeenie = 135;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 12)
                        {
                            ////loafers
                            armorWeenie = 28610;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 13)
                        {
                            ////sandals
                            armorWeenie = 129;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 14)
                        {
                            ////shoes
                            armorWeenie = 132;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 15)
                        {
                            ////slippers
                            armorWeenie = 133;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 16)
                        {
                            ////steel toed boots
                            armorWeenie = 7897;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(6, 1);
                        }
                        else if (armorPiece == 17)
                        {
                            ////buckler
                            armorWeenie = 44;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 18)
                        {
                            ////kite shield
                            armorWeenie = 91;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 19)
                        {
                            ////large Kite Shield
                            armorWeenie = 92;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 20)
                        {
                            ////Circlet
                            armorWeenie = 29528;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 21)
                        {
                            ////Armet
                            armorWeenie = 8488;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 22)
                        {
                            ////Baigha
                            armorWeenie = 550;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 23)
                        {
                            ////Heaume
                            armorWeenie = 74;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 24)
                        {
                            ////Helmet
                            armorWeenie = 75;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        //else if (armorPiece == 25)
                        //{
                        //    ////Horned Helm
                        //    armorWeenie = 92;
                        //    armorPieceType = 5;
                        //    spellArray = 10;
                        //    cantripArray = 10;
                        //}
                        else if (armorPiece == 25)
                        {
                            ////Kabuton
                            armorWeenie = 77;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 26)
                        {
                            ////sollerets
                            armorWeenie = 107;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 27)
                        {
                            ////viamontian laced boots
                            armorWeenie = 28611;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 28)
                        {
                            ////RTower Shield
                            armorWeenie = 95;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        //else if (armorPiece == 29)
                        //{
                        //    ////Signet Crown
                        //    armorWeenie = 31868;
                        //    armorPieceType = 2;
                        //    spellArray = 1;
                        //    cantripArray = 1;
                        //    materialType = GetMaterialType(7, 1);
                        //}
                        //else if (armorPiece == 30)
                        //{
                        //    ////Suikan Over-Robe
                        //    armorWeenie = 44801;
                        //    equipSetId = 15;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 31)
                        //{
                        //    ////Faran Over-Robe
                        //    armorWeenie = 44799;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 32)
                        //{
                        //    ////Dho Vest and  Over-Robe
                        //    armorWeenie = 44800;
                        //    equipSetId = 14;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 33)
                        //{
                        //    ////Suikan Over-Robe
                        //    armorWeenie = 44802;
                        //    ////equipSetId = 15;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 34)
                        //{
                        //    ////Coronet
                        //    armorWeenie = 31866;
                        //    armorPieceType = 2;
                        //    spellArray = 1;
                        //    cantripArray = 1;
                        //    materialType = GetMaterialType(7, 1);
                        //}
                        //else if (armorPiece == 35)
                        //{
                        //    ////Diadem
                        //    armorWeenie = 31867;
                        //    armorPieceType = 2;
                        //    spellArray = 1;
                        //    cantripArray = 1;
                        //    materialType = GetMaterialType(7, 1);
                        //}
                        else
                        {
                            ////Round Shield
                            armorWeenie = 93;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                    }
                    break;
                    ///////
                    ////

                case 6:
                    lowSpellTier = 6;
                    highSpellTier = 7;
                    armorType = r.Next(0, 16);
                    ////Leather Armor
                    if (armorType == 0)
                    {
                        int armorPiece = r.Next(0, 16);
                        if (armorPiece == 0)
                        {
                            ////helm
                            armorWeenie = 25636;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 1)
                        {
                            ////head
                            armorWeenie = 25640;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 2)
                        {
                            ////Chest
                            armorWeenie = 25639;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 3)
                        {
                            ////Chest
                            armorWeenie = 25641;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 4)
                        {
                            ////Chest
                            armorWeenie = 25638;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 5)
                        {
                            ////arms
                            armorWeenie = 25651;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 6)
                        {
                            ////Hands
                            armorWeenie = 25642;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 7)
                        {
                            ////Lower Arms
                            armorWeenie = 25637;
                            armorPieceType = 1;
                            spellArray = 4;
                            cantripArray = 4;
                        }
                        else if (armorPiece == 8)
                        {
                            ////Upper arms
                            armorWeenie = 25648;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 9)
                        {
                            ////Abdomen
                            armorWeenie = 25643;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 10)
                        {
                            ////Abdomen
                            armorWeenie = 25650;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 11)
                        {
                            ////legs
                            armorWeenie = 25647;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 12)
                        {
                            ////legs
                            armorWeenie = 25645;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 13)
                        {
                            ////Upper legs
                            armorWeenie = 25652;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 14)
                        {
                            ////lower legs
                            armorWeenie = 25644;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else
                        {
                            ////feet
                            armorWeenie = 25661;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                        }
                        materialType = GetMaterialType(5, 1);
                    }
                    ////Studded Leather
                    if (armorType == 1)
                    {
                        int armorPiece = r.Next(0, 15);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 554;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 116;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 38;
                            armorPieceType = 1;
                            spellArray = 4;
                            cantripArray = 4;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 42;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 48;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 723;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 53;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 7)
                        {
                            armorWeenie = 59;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 8)
                        {
                            armorWeenie = 63;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 9)
                        {
                            armorWeenie = 68;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 10)
                        {
                            armorWeenie = 68;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 11)
                        {
                            armorWeenie = 89;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 12)
                        {
                            armorWeenie = 99;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 13)
                        {
                            armorWeenie = 105;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else
                        {
                            armorWeenie = 112;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(5, 1);
                    }
                    ////Chainmail
                    if (armorType == 2)
                    {
                        int armorPiece = r.Next(0, 13);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 35;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 413;
                            armorPieceType = 1;
                            spellArray = 4;
                            cantripArray = 4;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 414;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 85;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 55;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 415;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 2605;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 7)
                        {
                            armorWeenie = 71;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 8)
                        {
                            armorWeenie = 80;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 9)
                        {
                            armorWeenie = 416;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 10)
                        {
                            armorWeenie = 96;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 11)
                        {
                            armorWeenie = 101;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else
                        {
                            armorWeenie = 108;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Platemail
                    if (armorType == 3)
                    {
                        int armorPiece = r.Next(0, 11);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 40;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 51;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 57;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 61;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 66;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 72;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 82;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 7)
                        {
                            armorWeenie = 87;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 8)
                        {
                            armorWeenie = 103;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 9)
                        {
                            armorWeenie = 110;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 114;
                            armorPieceType = 1;
                            spellArray = 4;
                            cantripArray = 4;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Scalemail
                    if (armorType == 4)
                    {
                        int armorPiece = r.Next(0, 14);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 552;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 37;
                            armorPieceType = 1;
                            spellArray = 4;
                            cantripArray = 4;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 41;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 793;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 52;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 58;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 62;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 7)
                        {
                            armorWeenie = 67;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 8)
                        {
                            armorWeenie = 73;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 9)
                        {
                            armorWeenie = 83;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 10)
                        {
                            armorWeenie = 88;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 11)
                        {
                            armorWeenie = 98;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 12)
                        {
                            armorWeenie = 104;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else
                        {
                            armorWeenie = 111;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Yoroi
                    if (armorType == 5)
                    {
                        int armorPiece = r.Next(0, 8);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 43;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 54;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 64;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 69;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 2437;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 90;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 102;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else
                        {
                            armorWeenie = 113;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Diforsa
                    if (armorType == 6)
                    {
                        int armorPiece = r.Next(0, 13);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 28367;
                            armorPieceType = 1;
                            spellArray = 4;
                            cantripArray = 4;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 28628;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 28630;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 28632;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 28633;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 28634;
                            armorPieceType = 31;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 30948;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 7)
                        {
                            armorWeenie = 28618;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 8)
                        {
                            armorWeenie = 28621;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 9)
                        {
                            armorWeenie = 28623;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 10)
                        {
                            armorWeenie = 30949;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 11)
                        {
                            armorWeenie = 28625;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                        }
                        else
                        {
                            armorWeenie = 28626;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////celdon
                    if (armorType == 7)
                    {
                        int armorPiece = r.Next(0, 4);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 6044;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 6043;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 6045;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 6048;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ///Amuli
                    if (armorType == 8)
                    {
                        int armorPiece = r.Next(0, 2);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 6046;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else
                        {
                            armorWeenie = 6047;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(6, 1);
                    }
                    ////Koujia
                    if (armorType == 9)
                    {
                        int armorPiece = r.Next(0, 3);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 6003;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 6004;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 6005;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Tenassa
                    if (armorType == 10)
                    {
                        int armorPiece = r.Next(0, 3);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 31026;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 28622;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 28624;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Lorica
                    if (armorType == 11)
                    {
                        int armorPiece = r.Next(0, 6);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 27220;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 27221;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 27222;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 27223;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 27224;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 27225;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Nariyid
                    if (armorType == 12)
                    {
                        int armorPiece = r.Next(0, 7);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 27226;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 27227;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 27228;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 27229;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 27230;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 27231;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 27232;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Chiran
                    if (armorType == 13)
                    {
                        int armorPiece = r.Next(0, 5);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 27215;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 27216;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 27217;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 27218;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 27219;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Alduressa
                    if (armorType == 14)
                    {
                        int armorPiece = r.Next(0, 5);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 30950;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 28629;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 30951;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 28617;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else
                        {
                            armorWeenie = 28620;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    //////Knorr Academy Armor
                    //if (armorType == 15)
                    //{
                    //    int armorPiece = r.Next(0, 8);
                    //    if (armorPiece == 0)
                    //    {
                    //        armorWeenie = 43053;
                    //        armorPieceType = 4;
                    //        spellArray = 9;
                    //        cantripArray = 9;
                    //    }
                    //    else if (armorPiece == 1)
                    //    {
                    //        armorWeenie = 43048;
                    //        armorPieceType = 1;
                    //        spellArray = 2;
                    //        cantripArray = 2;
                    //    }
                    //    else if (armorPiece == 2)
                    //    {
                    //        armorWeenie = 43049;
                    //        armorPieceType = 3;
                    //        spellArray = 5;
                    //        cantripArray = 5;
                    //    }
                    //    else if (armorPiece == 3)
                    //    {
                    //        armorWeenie = 43051;
                    //        armorPieceType = 1;
                    //        spellArray = 8;
                    //        cantripArray = 8;
                    //    }
                    //    else if (armorPiece == 4)
                    //    {
                    //        armorWeenie = 43068;
                    //        armorPieceType = 2;
                    //        spellArray = 1;
                    //        cantripArray = 1;
                    //    }
                    //    else if (armorPiece == 5)
                    //    {
                    //        armorWeenie = 43052;
                    //        armorPieceType = 1;
                    //        spellArray = 3;
                    //        cantripArray = 3;
                    //    }
                    //    else if (armorPiece == 6)
                    //    {
                    //        armorWeenie = 43054;
                    //        armorPieceType = 1;
                    //        spellArray = 7;
                    //        cantripArray = 7;
                    //    }
                    //    else
                    //    {
                    //        armorWeenie = 43055;
                    //        armorPieceType = 1;
                    //        spellArray = 4;
                    //        cantripArray = 4;
                    //    }
                    //    materialType = GetMaterialType(7, 1);
                    //}
                    //////Sedgemail Leather Armor
                    //if (armorType == 16)
                    //{
                    //    int armorPiece = r.Next(0, 6);
                    //    if (armorPiece == 0)
                    //    {
                    //        armorWeenie = 43829;
                    //        armorPieceType = 2;
                    //        spellArray = 1;
                    //        cantripArray = 1;
                    //    }
                    //    else if (armorPiece == 1)
                    //    {
                    //        armorWeenie = 43830;
                    //        armorPieceType = 3;
                    //        spellArray = 5;
                    //        cantripArray = 5;
                    //    }
                    //    else if (armorPiece == 3)
                    //    {
                    //        armorWeenie = 43831;
                    //        armorPieceType = 1;
                    //        spellArray = 8;
                    //        cantripArray = 8;
                    //    }
                    //    else if (armorPiece == 4)
                    //    {
                    //        armorWeenie = 43832;
                    //        armorPieceType = 4;
                    //        spellArray = 9;
                    //        cantripArray = 9;
                    //    }
                    //    else if (armorPiece == 5)
                    //    {
                    //        armorWeenie = 43833;
                    //        armorPieceType = 1;
                    //        spellArray = 3;
                    //        cantripArray = 3;
                    //    }
                    //    else
                    //    {
                    //        armorWeenie = 43828;
                    //        armorPieceType = 1;
                    //        spellArray = 2;
                    //        cantripArray = 2;
                    //    }
                    //    materialType = GetMaterialType(5, 1);
                    //}
                    //////Haebrean
                    //if (armorType == 17)
                    //{
                    //    int armorPiece = r.Next(0, 9);
                    //    if (armorPiece == 0)
                    //    {
                    //        armorWeenie = 42755;
                    //        armorPieceType = 4;
                    //        spellArray = 9;
                    //        cantripArray = 9;
                    //    }
                    //    else if (armorPiece == 1)
                    //    {
                    //        armorWeenie = 42749;
                    //        armorPieceType = 1;
                    //        spellArray = 2;
                    //        cantripArray = 2;
                    //    }
                    //    else if (armorPiece == 2)
                    //    {
                    //        armorWeenie = 42750;
                    //        armorPieceType = 3;
                    //        spellArray = 5;
                    //        cantripArray = 5;
                    //    }
                    //    else if (armorPiece == 3)
                    //    {
                    //        armorWeenie = 42751;
                    //        armorPieceType = 1;
                    //        spellArray = 6;
                    //        cantripArray = 6;
                    //    }
                    //    else if (armorPiece == 4)
                    //    {
                    //        armorWeenie = 42752;
                    //        armorPieceType = 1;
                    //        spellArray = 8;
                    //        cantripArray = 8;
                    //    }
                    //    else if (armorPiece == 5)
                    //    {
                    //        armorWeenie = 42753;
                    //        armorPieceType = 2;
                    //        spellArray = 1;
                    //        cantripArray = 1;
                    //    }
                    //    else if (armorPiece == 6)
                    //    {
                    //        armorWeenie = 42754;
                    //        armorPieceType = 1;
                    //        spellArray = 3;
                    //        cantripArray = 3;
                    //    }
                    //    else if (armorPiece == 7)
                    //    {
                    //        armorWeenie = 42756;
                    //        armorPieceType = 1;
                    //        spellArray = 7;
                    //        cantripArray = 7;
                    //    }
                    //    else
                    //    {
                    //        armorWeenie = 42757;
                    //        armorPieceType = 1;
                    //        spellArray = 4;
                    //        cantripArray = 4;
                    //    }
                    //    materialType = GetMaterialType(7, 1);
                    //}
                    if (armorType == 15)
                    {
                        int armorPiece = r.Next(0, 34);
                        if (armorPiece == 0)
                        {
                            ////bandana
                            armorWeenie = 28612;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 1)
                        {
                            ////beret
                            armorWeenie = 28605;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 2)
                        {
                            ////Cloth Cap
                            armorWeenie = 118;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 3)
                        {
                            ////Cloth gloves
                            armorWeenie = 121;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 4)
                        {
                            ////Cowl
                            armorWeenie = 119;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 5)
                        {
                            ////crown
                            armorWeenie = 296;
                            armorPieceType = 1;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 6)
                        {
                            ////Fez
                            armorWeenie = 5894;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 7)
                        {
                            ////hood
                            armorWeenie = 5905;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 8)
                        {
                            ////Kasa
                            armorWeenie = 5901;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 9)
                        {
                            ////Metal cap
                            armorWeenie = 46;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 10)
                        {
                            ////Qafiya
                            armorWeenie = 76;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 11)
                        {
                            ////turban
                            armorWeenie = 135;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 12)
                        {
                            ////loafers
                            armorWeenie = 28610;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 13)
                        {
                            ////sandals
                            armorWeenie = 129;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 14)
                        {
                            ////shoes
                            armorWeenie = 132;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 15)
                        {
                            ////slippers
                            armorWeenie = 133;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 16)
                        {
                            ////steel toed boots
                            armorWeenie = 7897;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(6, 1);
                        }
                        else if (armorPiece == 17)
                        {
                            ////buckler
                            armorWeenie = 44;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 18)
                        {
                            ////kite shield
                            armorWeenie = 91;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 19)
                        {
                            ////large Kite Shield
                            armorWeenie = 92;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 20)
                        {
                            ////Circlet
                            armorWeenie = 29528;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 21)
                        {
                            ////Armet
                            armorWeenie = 8488;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 22)
                        {
                            ////Baigha
                            armorWeenie = 550;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 23)
                        {
                            ////Heaume
                            armorWeenie = 74;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 24)
                        {
                            ////Helmet
                            armorWeenie = 75;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        //else if (armorPiece == 25)
                        //{
                        //    ////Horned Helm
                        //    armorWeenie = 92;
                        //    armorPieceType = 5;
                        //    spellArray = 10;
                        //    cantripArray = 10;
                        //}
                        else if (armorPiece == 25)
                        {
                            ////Kabuton
                            armorWeenie = 77;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 26)
                        {
                            ////sollerets
                            armorWeenie = 107;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 27)
                        {
                            ////viamontian laced boots
                            armorWeenie = 28611;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 28)
                        {
                            ////RTower Shield
                            armorWeenie = 95;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        //else if (armorPiece == 29)
                        //{
                        //    ////Signet Crown
                        //    armorWeenie = 31868;
                        //    armorPieceType = 2;
                        //    spellArray = 1;
                        //    cantripArray = 1;
                        //    materialType = GetMaterialType(7, 1);
                        //}
                        //else if (armorPiece == 30)
                        //{
                        //    ////Suikan Over-Robe
                        //    armorWeenie = 44801;
                        //    equipSetId = 15;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 31)
                        //{
                        //    ////Faran Over-Robe
                        //    armorWeenie = 44799;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 32)
                        //{
                        //    ////Dho Vest and  Over-Robe
                        //    armorWeenie = 44800;
                        //    equipSetId = 14;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 33)
                        //{
                        //    ////Suikan Over-Robe
                        //    armorWeenie = 44802;
                        //    ////equipSetId = 15;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 30)
                        //{
                        //    ////Coronet
                        //    armorWeenie = 31866;
                        //    armorPieceType = 2;
                        //    spellArray = 1;
                        //    cantripArray = 1;
                        //    materialType = GetMaterialType(7, 1);
                        //}
                        //else if (armorPiece == 31)
                        //{
                        //    ////Diadem
                        //    armorWeenie = 31867;
                        //    armorPieceType = 2;
                        //    spellArray = 1;
                        //    cantripArray = 1;
                        //    materialType = GetMaterialType(7, 1);
                        //}
                        //else if (armorPiece == 32)
                        //{
                        //    ////Teardrop
                        //    armorWeenie = 31864;
                        //    armorPieceType = 2;
                        //    spellArray = 1;
                        //    cantripArray = 1;
                        //    materialType = GetMaterialType(7, 1);
                        //}
                        //else if (armorPiece == 37)
                        //{
                        //    ////Lyceum Hood
                        //    armorWeenie = 44977;
                        //    armorPieceType = 2;
                        //    spellArray = 1;
                        //    cantripArray = 1;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 38)
                        //{
                        //    ////Empyrean Over-Robe
                        //    armorWeenie = 43274;
                        //    armorPieceType = 1;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        else
                        {
                            ////Round Shield
                            armorWeenie = 93;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                    }
                    break;
                case 7:
                    lowSpellTier = 6;
                    highSpellTier = 8;
                    armorType = r.Next(0, 16);
                    ////Leather Armor
                    if (armorType == 0)
                    {
                        int armorPiece = r.Next(0, 16);
                        if (armorPiece == 0)
                        {
                            ////helm
                            armorWeenie = 25636;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 1)
                        {
                            ////head
                            armorWeenie = 25640;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 2)
                        {
                            ////Chest
                            armorWeenie = 25639;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 3)
                        {
                            ////Chest
                            armorWeenie = 25641;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 4)
                        {
                            ////Chest
                            armorWeenie = 25638;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 5)
                        {
                            ////arms
                            armorWeenie = 25651;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 6)
                        {
                            ////Hands
                            armorWeenie = 25642;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 7)
                        {
                            ////Lower Arms
                            armorWeenie = 25637;
                            armorPieceType = 1;
                            spellArray = 4;
                            cantripArray = 4;
                        }
                        else if (armorPiece == 8)
                        {
                            ////Upper arms
                            armorWeenie = 25648;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 9)
                        {
                            ////Abdomen
                            armorWeenie = 25643;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 10)
                        {
                            ////Abdomen
                            armorWeenie = 25650;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 11)
                        {
                            ////legs
                            armorWeenie = 25647;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 12)
                        {
                            ////legs
                            armorWeenie = 25645;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 13)
                        {
                            ////Upper legs
                            armorWeenie = 25652;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 14)
                        {
                            ////lower legs
                            armorWeenie = 25644;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else
                        {
                            ////feet
                            armorWeenie = 25661;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                        }
                        materialType = GetMaterialType(5, 1);
                    }
                    ////Studded Leather
                    if (armorType == 1)
                    {
                        int armorPiece = r.Next(0, 15);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 554;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 116;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 38;
                            armorPieceType = 1;
                            spellArray = 4;
                            cantripArray = 4;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 42;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 48;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 723;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 53;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 7)
                        {
                            armorWeenie = 59;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 8)
                        {
                            armorWeenie = 63;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 9)
                        {
                            armorWeenie = 68;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 10)
                        {
                            armorWeenie = 68;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 11)
                        {
                            armorWeenie = 89;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 12)
                        {
                            armorWeenie = 99;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 13)
                        {
                            armorWeenie = 105;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else
                        {
                            armorWeenie = 112;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(5, 1);
                    }
                    ////Chainmail
                    if (armorType == 2)
                    {
                        int armorPiece = r.Next(0, 13);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 35;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 413;
                            armorPieceType = 1;
                            spellArray = 4;
                            cantripArray = 4;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 414;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 85;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 55;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 415;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 2605;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 7)
                        {
                            armorWeenie = 71;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 8)
                        {
                            armorWeenie = 80;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 9)
                        {
                            armorWeenie = 416;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 10)
                        {
                            armorWeenie = 96;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 11)
                        {
                            armorWeenie = 101;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else
                        {
                            armorWeenie = 108;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Platemail
                    if (armorType == 3)
                    {
                        int armorPiece = r.Next(0, 11);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 40;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 51;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 57;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 61;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 66;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 72;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 82;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 7)
                        {
                            armorWeenie = 87;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 8)
                        {
                            armorWeenie = 103;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 9)
                        {
                            armorWeenie = 110;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 114;
                            armorPieceType = 1;
                            spellArray = 4;
                            cantripArray = 4;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Scalemail
                    if (armorType == 4)
                    {
                        int armorPiece = r.Next(0, 14);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 552;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 37;
                            armorPieceType = 1;
                            spellArray = 4;
                            cantripArray = 4;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 41;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 793;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 52;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 58;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 62;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 7)
                        {
                            armorWeenie = 67;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 8)
                        {
                            armorWeenie = 73;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 9)
                        {
                            armorWeenie = 83;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 10)
                        {
                            armorWeenie = 88;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 11)
                        {
                            armorWeenie = 98;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 12)
                        {
                            armorWeenie = 104;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else
                        {
                            armorWeenie = 111;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Yoroi
                    if (armorType == 5)
                    {
                        int armorPiece = r.Next(0, 8);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 43;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 54;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 64;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 69;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 2437;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 90;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 102;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else
                        {
                            armorWeenie = 113;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Diforsa
                    if (armorType == 6)
                    {
                        int armorPiece = r.Next(0, 13);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 28367;
                            armorPieceType = 1;
                            spellArray = 4;
                            cantripArray = 4;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 28628;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 28630;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 28632;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 28633;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 28634;
                            armorPieceType = 31;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 30948;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 7)
                        {
                            armorWeenie = 28618;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 8)
                        {
                            armorWeenie = 28621;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 9)
                        {
                            armorWeenie = 28623;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 10)
                        {
                            armorWeenie = 30949;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 11)
                        {
                            armorWeenie = 28625;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                        }
                        else
                        {
                            armorWeenie = 28626;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////celdon
                    if (armorType == 7)
                    {
                        int armorPiece = r.Next(0, 4);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 6044;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 6043;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 6045;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 6048;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ///Amuli
                    if (armorType == 8)
                    {
                        int armorPiece = r.Next(0, 2);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 6046;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else
                        {
                            armorWeenie = 6047;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(6, 1);
                    }
                    ////Koujia
                    if (armorType == 9)
                    {
                        int armorPiece = r.Next(0, 3);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 6003;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 6004;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 6005;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Tenassa
                    if (armorType == 10)
                    {
                        int armorPiece = r.Next(0, 3);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 31026;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 28622;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 28624;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Lorica
                    if (armorType == 11)
                    {
                        int armorPiece = r.Next(0, 6);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 27220;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 27221;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 27222;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 27223;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 27224;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 27225;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Nariyid
                    if (armorType == 12)
                    {
                        int armorPiece = r.Next(0, 7);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 27226;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 27227;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 27228;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 27229;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 27230;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 27231;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 27232;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Chiran
                    if (armorType == 13)
                    {
                        int armorPiece = r.Next(0, 5);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 27215;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 27216;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 27217;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 27218;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 27219;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Alduressa
                    if (armorType == 14)
                    {
                        int armorPiece = r.Next(0, 6);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 30950;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 28629;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 30951;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 28617;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else
                        {
                            armorWeenie = 28620;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Knorr Academy Armor
                    //if (armorType == 15)
                    //{
                    //    int armorPiece = r.Next(0, 8);
                    //    if (armorPiece == 0)
                    //    {
                    //        armorWeenie = 43053;
                    //        armorPieceType = 4;
                    //        spellArray = 9;
                    //        cantripArray = 9;
                    //    }
                    //    else if (armorPiece == 1)
                    //    {
                    //        armorWeenie = 43048;
                    //        armorPieceType = 1;
                    //        spellArray = 2;
                    //        cantripArray = 2;
                    //    }
                    //    else if (armorPiece == 2)
                    //    {
                    //        armorWeenie = 43049;
                    //        armorPieceType = 3;
                    //        spellArray = 5;
                    //        cantripArray = 5;
                    //    }
                    //    else if (armorPiece == 3)
                    //    {
                    //        armorWeenie = 43051;
                    //        armorPieceType = 1;
                    //        spellArray = 8;
                    //        cantripArray = 8;
                    //    }
                    //    else if (armorPiece == 4)
                    //    {
                    //        armorWeenie = 43068;
                    //        armorPieceType = 2;
                    //        spellArray = 1;
                    //        cantripArray = 1;
                    //    }
                    //    else if (armorPiece == 5)
                    //    {
                    //        armorWeenie = 43052;
                    //        armorPieceType = 1;
                    //        spellArray = 3;
                    //        cantripArray = 3;
                    //    }
                    //    else if (armorPiece == 6)
                    //    {
                    //        armorWeenie = 43054;
                    //        armorPieceType = 1;
                    //        spellArray = 7;
                    //        cantripArray = 7;
                    //    }
                    //    else
                    //    {
                    //        armorWeenie = 43055;
                    //        armorPieceType = 1;
                    //        spellArray = 4;
                    //        cantripArray = 4;
                    //    }
                    //    materialType = GetMaterialType(7, 1);
                    //}
                    //////Sedgemail Leather Armor
                    //if (armorType == 16)
                    //{
                    //    int armorPiece = r.Next(0, 6);
                    //    if (armorPiece == 0)
                    //    {
                    //        armorWeenie = 43829;
                    //        armorPieceType = 2;
                    //        spellArray = 1;
                    //        cantripArray = 1;
                    //    }
                    //    //else if (armorPiece == 1)
                    //    //{
                    //    //    armorWeenie = 43830;
                    //    //    armorPieceType = 3;
                    //    //    spellArray = 5;
                    //    //    cantripArray = 5;
                    //    //}
                    //    else if (armorPiece == 3)
                    //    {
                    //        armorWeenie = 43831;
                    //        armorPieceType = 1;
                    //        spellArray = 8;
                    //        cantripArray = 8;
                    //    }
                    //    else if (armorPiece == 4)
                    //    {
                    //        armorWeenie = 43832;
                    //        armorPieceType = 4;
                    //        spellArray = 9;
                    //        cantripArray = 9;
                    //    }
                    //    else if (armorPiece == 5)
                    //    {
                    //        armorWeenie = 43833;
                    //        armorPieceType = 1;
                    //        spellArray = 3;
                    //        cantripArray = 3;
                    //    }
                    //    else
                    //    {
                    //        armorWeenie = 43828;
                    //        armorPieceType = 1;
                    //        spellArray = 2;
                    //        cantripArray = 2;
                    //    }
                    //    materialType = GetMaterialType(5, 1);
                    //}
                    //////Haebrean
                    //if (armorType == 17)
                    //{
                    //    int armorPiece = r.Next(0, 9);
                    //    if (armorPiece == 0)
                    //    {
                    //        armorWeenie = 42755;
                    //        armorPieceType = 4;
                    //        spellArray = 9;
                    //        cantripArray = 9;
                    //    }
                    //    else if (armorPiece == 1)
                    //    {
                    //        armorWeenie = 42749;
                    //        armorPieceType = 1;
                    //        spellArray = 2;
                    //        cantripArray = 2;
                    //    }
                    //    else if (armorPiece == 2)
                    //    {
                    //        armorWeenie = 42750;
                    //        armorPieceType = 3;
                    //        spellArray = 5;
                    //        cantripArray = 5;
                    //    }
                    //    else if (armorPiece == 3)
                    //    {
                    //        armorWeenie = 42751;
                    //        armorPieceType = 1;
                    //        spellArray = 6;
                    //        cantripArray = 6;
                    //    }
                    //    else if (armorPiece == 4)
                    //    {
                    //        armorWeenie = 42752;
                    //        armorPieceType = 1;
                    //        spellArray = 8;
                    //        cantripArray = 8;
                    //    }
                    //    else if (armorPiece == 5)
                    //    {
                    //        armorWeenie = 42753;
                    //        armorPieceType = 2;
                    //        spellArray = 1;
                    //        cantripArray = 1;
                    //    }
                    //    else if (armorPiece == 6)
                    //    {
                    //        armorWeenie = 42754;
                    //        armorPieceType = 1;
                    //        spellArray = 3;
                    //        cantripArray = 3;
                    //    }
                    //    else if (armorPiece == 7)
                    //    {
                    //        armorWeenie = 42756;
                    //        armorPieceType = 1;
                    //        spellArray = 7;
                    //        cantripArray = 7;
                    //    }
                    //    else
                    //    {
                    //        armorWeenie = 42757;
                    //        armorPieceType = 1;
                    //        spellArray = 4;
                    //        cantripArray = 4;
                    //    }
                    //    materialType = GetMaterialType(7, 1);
                    //}
                    if (armorType == 15)
                    {
                        int armorPiece = r.Next(0, 29);
                        if (armorPiece == 0)
                        {
                            ////bandana
                            armorWeenie = 28612;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 1)
                        {
                            ////beret
                            armorWeenie = 28605;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 2)
                        {
                            ////Cloth Cap
                            armorWeenie = 118;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 3)
                        {
                            ////Cloth gloves
                            armorWeenie = 121;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 4)
                        {
                            ////Cowl
                            armorWeenie = 119;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 5)
                        {
                            ////crown
                            armorWeenie = 296;
                            armorPieceType = 1;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 6)
                        {
                            ////Fez
                            armorWeenie = 5894;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 7)
                        {
                            ////hood
                            armorWeenie = 5905;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 8)
                        {
                            ////Kasa
                            armorWeenie = 5901;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 9)
                        {
                            ////Metal cap
                            armorWeenie = 46;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 10)
                        {
                            ////Qafiya
                            armorWeenie = 76;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 11)
                        {
                            ////turban
                            armorWeenie = 135;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 12)
                        {
                            ////loafers
                            armorWeenie = 28610;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 13)
                        {
                            ////sandals
                            armorWeenie = 129;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 14)
                        {
                            ////shoes
                            armorWeenie = 132;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 15)
                        {
                            ////slippers
                            armorWeenie = 133;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 16)
                        {
                            ////steel toed boots
                            armorWeenie = 7897;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(6, 1);
                        }
                        else if (armorPiece == 17)
                        {
                            ////buckler
                            armorWeenie = 44;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 18)
                        {
                            ////kite shield
                            armorWeenie = 91;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 19)
                        {
                            ////large Kite Shield
                            armorWeenie = 92;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 20)
                        {
                            ////Circlet
                            armorWeenie = 29528;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 21)
                        {
                            ////Armet
                            armorWeenie = 8488;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 22)
                        {
                            ////Baigha
                            armorWeenie = 550;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 23)
                        {
                            ////Heaume
                            armorWeenie = 74;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 24)
                        {
                            ////Helmet
                            armorWeenie = 75;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        //else if (armorPiece == 25)
                        //{
                        //    ////Horned Helm
                        //    armorWeenie = 92;
                        //    armorPieceType = 5;
                        //    spellArray = 10;
                        //    cantripArray = 10;
                        //}
                        else if (armorPiece == 25)
                        {
                            ////Kabuton
                            armorWeenie = 77;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 26)
                        {
                            ////sollerets
                            armorWeenie = 107;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 27)
                        {
                            ////viamontian laced boots
                            armorWeenie = 28611;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 28)
                        {
                            ////RTower Shield
                            armorWeenie = 95;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        //else if (armorPiece == 29)
                        //{
                        //    ////Signet Crown
                        //    armorWeenie = 31868;
                        //    armorPieceType = 2;
                        //    spellArray = 1;
                        //    cantripArray = 1;
                        //    materialType = GetMaterialType(7, 1);
                        //}
                        //else if (armorPiece == 30)
                        //{
                        //    ////Suikan Over-Robe
                        //    armorWeenie = 44801;
                        //    equipSetId = 15;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 31)
                        //{
                        //    ////Faran Over-Robe
                        //    armorWeenie = 44799;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 32)
                        //{
                        //    ////Dho Vest and  Over-Robe
                        //    armorWeenie = 44800;
                        //    equipSetId = 14;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 33)
                        //{
                        //    ////Suikan Over-Robe
                        //    armorWeenie = 44802;
                        //    ////equipSetId = 15;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 30)
                        //{
                        //    ////Coronet
                        //    armorWeenie = 31866;
                        //    armorPieceType = 2;
                        //    spellArray = 1;
                        //    cantripArray = 1;
                        //    materialType = GetMaterialType(7, 1);
                        //}
                        //else if (armorPiece == 31)
                        //{
                        //    ////Diadem
                        //    armorWeenie = 31867;
                        //    armorPieceType = 2;
                        //    spellArray = 1;
                        //    cantripArray = 1;
                        //    materialType = GetMaterialType(7, 1);
                        //}
                        //else if (armorPiece == 32)
                        //{
                        //    ////Teardrop
                        //    armorWeenie = 31864;
                        //    armorPieceType = 2;
                        //    spellArray = 1;
                        //    cantripArray = 1;
                        //    materialType = GetMaterialType(7, 1);
                        //}
                        //else if (armorPiece == 37)
                        //{
                        //    ////Lyceum Hood
                        //    armorWeenie = 44977;
                        //    armorPieceType = 2;
                        //    spellArray = 1;
                        //    cantripArray = 1;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 38)
                        //{
                        //    ////Empyrean Over-Robe
                        //    armorWeenie = 43274;
                        //    armorPieceType = 1;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        else
                        {
                            ////Round Shield
                            armorWeenie = 93;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                    }
                    wieldDifficulty = 150;
                    break;
                case 8:
                    lowSpellTier = 6;
                    highSpellTier = 8;
                    armorType = r.Next(0, 16);
                    ////Leather Armor
                    if (armorType == 0)
                    {
                        int armorPiece = r.Next(0, 16);
                        if (armorPiece == 0)
                        {
                            ////helm
                            armorWeenie = 25636;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 1)
                        {
                            ////head
                            armorWeenie = 25640;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 2)
                        {
                            ////Chest
                            armorWeenie = 25639;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 3)
                        {
                            ////Chest
                            armorWeenie = 25641;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 4)
                        {
                            ////Chest
                            armorWeenie = 25638;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 5)
                        {
                            ////arms
                            armorWeenie = 25651;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 6)
                        {
                            ////Hands
                            armorWeenie = 25642;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 7)
                        {
                            ////Lower Arms
                            armorWeenie = 25637;
                            armorPieceType = 1;
                            spellArray = 4;
                            cantripArray = 4;
                        }
                        else if (armorPiece == 8)
                        {
                            ////Upper arms
                            armorWeenie = 25648;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 9)
                        {
                            ////Abdomen
                            armorWeenie = 25643;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 10)
                        {
                            ////Abdomen
                            armorWeenie = 25650;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 11)
                        {
                            ////legs
                            armorWeenie = 25647;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 12)
                        {
                            ////legs
                            armorWeenie = 25645;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 13)
                        {
                            ////Upper legs
                            armorWeenie = 25652;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 14)
                        {
                            ////lower legs
                            armorWeenie = 25644;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else
                        {
                            ////feet
                            armorWeenie = 25661;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                        }
                        materialType = GetMaterialType(5, 1);
                    }
                    ////Studded Leather
                    if (armorType == 1)
                    {
                        int armorPiece = r.Next(0, 15);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 554;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 116;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 38;
                            armorPieceType = 1;
                            spellArray = 4;
                            cantripArray = 4;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 42;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 48;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 723;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 53;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 7)
                        {
                            armorWeenie = 59;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 8)
                        {
                            armorWeenie = 63;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 9)
                        {
                            armorWeenie = 68;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 10)
                        {
                            armorWeenie = 68;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 11)
                        {
                            armorWeenie = 89;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 12)
                        {
                            armorWeenie = 99;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 13)
                        {
                            armorWeenie = 105;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else
                        {
                            armorWeenie = 112;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(5, 1);
                    }
                    ////Chainmail
                    if (armorType == 2)
                    {
                        int armorPiece = r.Next(0, 13);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 35;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 413;
                            armorPieceType = 1;
                            spellArray = 4;
                            cantripArray = 4;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 414;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 85;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 55;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 415;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 2605;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 7)
                        {
                            armorWeenie = 71;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 8)
                        {
                            armorWeenie = 80;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 9)
                        {
                            armorWeenie = 416;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 10)
                        {
                            armorWeenie = 96;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 11)
                        {
                            armorWeenie = 101;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else
                        {
                            armorWeenie = 108;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Platemail
                    if (armorType == 3)
                    {
                        int armorPiece = r.Next(0, 11);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 40;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 51;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 57;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 61;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 66;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 72;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 82;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 7)
                        {
                            armorWeenie = 87;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 8)
                        {
                            armorWeenie = 103;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 9)
                        {
                            armorWeenie = 110;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 114;
                            armorPieceType = 1;
                            spellArray = 4;
                            cantripArray = 4;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Scalemail
                    if (armorType == 4)
                    {
                        int armorPiece = r.Next(0, 14);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 552;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 37;
                            armorPieceType = 1;
                            spellArray = 4;
                            cantripArray = 4;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 41;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 793;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 52;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 58;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 62;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 7)
                        {
                            armorWeenie = 67;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 8)
                        {
                            armorWeenie = 73;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 9)
                        {
                            armorWeenie = 83;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 10)
                        {
                            armorWeenie = 88;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 11)
                        {
                            armorWeenie = 98;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 12)
                        {
                            armorWeenie = 104;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else
                        {
                            armorWeenie = 111;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Yoroi
                    if (armorType == 5)
                    {
                        int armorPiece = r.Next(0, 8);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 43;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 54;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 64;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 69;
                            armorPieceType = 1;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 2437;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 90;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 102;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else
                        {
                            armorWeenie = 113;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Diforsa
                    if (armorType == 6)
                    {
                        int armorPiece = r.Next(0, 13);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 28367;
                            armorPieceType = 1;
                            spellArray = 4;
                            cantripArray = 4;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 28628;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 28630;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 28632;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 28633;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 28634;
                            armorPieceType = 31;
                            spellArray = 8;
                            cantripArray = 8;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 30948;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 7)
                        {
                            armorWeenie = 28618;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 8)
                        {
                            armorWeenie = 28621;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else if (armorPiece == 9)
                        {
                            armorWeenie = 28623;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 10)
                        {
                            armorWeenie = 30949;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        else if (armorPiece == 11)
                        {
                            armorWeenie = 28625;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                        }
                        else
                        {
                            armorWeenie = 28626;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////celdon
                    if (armorType == 7)
                    {
                        int armorPiece = r.Next(0, 4);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 6044;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 6043;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 6045;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 6048;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ///Amuli
                    if (armorType == 8)
                    {
                        int armorPiece = r.Next(0, 2);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 6046;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else
                        {
                            armorWeenie = 6047;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        materialType = GetMaterialType(6, 1);
                    }
                    ////Koujia
                    if (armorType == 9)
                    {
                        int armorPiece = r.Next(0, 3);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 6003;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 6004;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 6005;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Tenassa
                    if (armorType == 10)
                    {
                        int armorPiece = r.Next(0, 3);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 31026;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 28622;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 28624;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Lorica
                    if (armorType == 11)
                    {
                        int armorPiece = r.Next(0, 6);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 27220;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 27221;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 27222;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 27223;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 27224;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 27225;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Nariyid
                    if (armorType == 12)
                    {
                        int armorPiece = r.Next(0, 7);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 27226;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 27227;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 27228;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 27229;
                            armorPieceType = 1;
                            spellArray = 6;
                            cantripArray = 6;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 27230;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 27231;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 27232;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Chiran
                    if (armorType == 13)
                    {
                        int armorPiece = r.Next(0, 5);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 27215;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 27216;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 27217;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 27218;
                            armorPieceType = 1;
                            spellArray = 7;
                            cantripArray = 7;
                        }
                        else
                        {
                            armorWeenie = 27219;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    ////Alduressa
                    if (armorType == 14)
                    {
                        int armorPiece = r.Next(0, 5);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 30950;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 28629;
                            armorPieceType = 1;
                            spellArray = 2;
                            cantripArray = 2;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 30951;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 28617;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                        }
                        else
                        {
                            armorWeenie = 28620;
                            armorPieceType = 1;
                            spellArray = 3;
                            cantripArray = 3;
                        }
                        materialType = GetMaterialType(7, 1);
                    }
                    //////Knorr Academy Armor
                    //if (armorType == 15)
                    //{
                    //    int armorPiece = r.Next(0, 8);
                    //    if (armorPiece == 0)
                    //    {
                    //        armorWeenie = 43053;
                    //        armorPieceType = 4;
                    //        spellArray = 9;
                    //        cantripArray = 9;
                    //    }
                    //    else if (armorPiece == 1)
                    //    {
                    //        armorWeenie = 43048;
                    //        armorPieceType = 1;
                    //        spellArray = 2;
                    //        cantripArray = 2;
                    //    }
                    //    else if (armorPiece == 2)
                    //    {
                    //        armorWeenie = 43049;
                    //        armorPieceType = 3;
                    //        spellArray = 5;
                    //        cantripArray = 5;
                    //    }
                    //    else if (armorPiece == 3)
                    //    {
                    //        armorWeenie = 43051;
                    //        armorPieceType = 1;
                    //        spellArray = 8;
                    //        cantripArray = 8;
                    //    }
                    //    else if (armorPiece == 4)
                    //    {
                    //        armorWeenie = 43068;
                    //        armorPieceType = 2;
                    //        spellArray = 1;
                    //        cantripArray = 1;
                    //    }
                    //    else if (armorPiece == 5)
                    //    {
                    //        armorWeenie = 43052;
                    //        armorPieceType = 1;
                    //        spellArray = 3;
                    //        cantripArray = 3;
                    //    }
                    //    else if (armorPiece == 6)
                    //    {
                    //        armorWeenie = 43054;
                    //        armorPieceType = 1;
                    //        spellArray = 7;
                    //        cantripArray = 7;
                    //    }
                    //    else
                    //    {
                    //        armorWeenie = 43055;
                    //        armorPieceType = 1;
                    //        spellArray = 4;
                    //        cantripArray = 4;
                    //    }
                    //    materialType = GetMaterialType(7, 1);
                    //}
                    //////Sedgemail Leather Armor
                    //if (armorType == 16)
                    //{
                    //    int armorPiece = r.Next(0, 6);
                    //    if (armorPiece == 0)
                    //    {
                    //        armorWeenie = 43829;
                    //        armorPieceType = 2;
                    //        spellArray = 1;
                    //        cantripArray = 1;
                    //    }
                    //    else if (armorPiece == 1)
                    //    {
                    //        armorWeenie = 43830;
                    //        armorPieceType = 3;
                    //        spellArray = 5;
                    //        cantripArray = 5;
                    //    }
                    //    else if (armorPiece == 3)
                    //    {
                    //        armorWeenie = 43831;
                    //        armorPieceType = 1;
                    //        spellArray = 8;
                    //        cantripArray = 8;
                    //    }
                    //    else if (armorPiece == 4)
                    //    {
                    //        armorWeenie = 43832;
                    //        armorPieceType = 4;
                    //        spellArray = 9;
                    //        cantripArray = 9;
                    //    }
                    //    else if (armorPiece == 5)
                    //    {
                    //        armorWeenie = 43833;
                    //        armorPieceType = 1;
                    //        spellArray = 3;
                    //        cantripArray = 3;
                    //    }
                    //    else
                    //    {
                    //        armorWeenie = 43828;
                    //        armorPieceType = 1;
                    //        spellArray = 2;
                    //        cantripArray = 2;
                    //    }
                    //    materialType = GetMaterialType(5, 1);
                    //}
                    //////Haebrean
                    //if (armorType == 17)
                    //{
                    //    int armorPiece = r.Next(0, 9);
                    //    if (armorPiece == 0)
                    //    {
                    //        armorWeenie = 42755;
                    //        armorPieceType = 4;
                    //        spellArray = 9;
                    //        cantripArray = 9;
                    //    }
                    //    else if (armorPiece == 1)
                    //    {
                    //        armorWeenie = 42749;
                    //        armorPieceType = 1;
                    //        spellArray = 2;
                    //        cantripArray = 2;
                    //    }
                    //    else if (armorPiece == 2)
                    //    {
                    //        armorWeenie = 42750;
                    //        armorPieceType = 3;
                    //        spellArray = 5;
                    //        cantripArray = 5;
                    //    }
                    //    else if (armorPiece == 3)
                    //    {
                    //        armorWeenie = 42751;
                    //        armorPieceType = 1;
                    //        spellArray = 6;
                    //        cantripArray = 6;
                    //    }
                    //    else if (armorPiece == 4)
                    //    {
                    //        armorWeenie = 42752;
                    //        armorPieceType = 1;
                    //        spellArray = 8;
                    //        cantripArray = 8;
                    //    }
                    //    else if (armorPiece == 5)
                    //    {
                    //        armorWeenie = 42753;
                    //        armorPieceType = 2;
                    //        spellArray = 1;
                    //        cantripArray = 1;
                    //    }
                    //    else if (armorPiece == 6)
                    //    {
                    //        armorWeenie = 42754;
                    //        armorPieceType = 1;
                    //        spellArray = 3;
                    //        cantripArray = 3;
                    //    }
                    //    else if (armorPiece == 7)
                    //    {
                    //        armorWeenie = 42756;
                    //        armorPieceType = 1;
                    //        spellArray = 7;
                    //        cantripArray = 7;
                    //    }
                    //    else
                    //    {
                    //        armorWeenie = 42757;
                    //        armorPieceType = 1;
                    //        spellArray = 4;
                    //        cantripArray = 4;
                    //    }
                    //    materialType = GetMaterialType(7, 1);
                    //}
                    //////Olthoi Alduressa
                    //if (armorType == 18)
                    //{
                    //    int armorPiece = r.Next(0, 5);
                    //    if (armorPiece == 0)
                    //    {
                    //        armorWeenie = 37207;
                    //        armorPieceType = 4;
                    //        wieldDifficulty = 180;
                    //        spellArray = 9;
                    //        cantripArray = 9;
                    //    }
                    //    else if (armorPiece == 1)
                    //    {
                    //        armorWeenie = 37217;
                    //        armorPieceType = 1;
                    //        wieldDifficulty = 180;
                    //        spellArray = 2;
                    //        cantripArray = 2;
                    //    }
                    //    else if (armorPiece == 2)
                    //    {
                    //        armorWeenie = 37187;
                    //        armorPieceType = 3;
                    //        wieldDifficulty = 180;
                    //        spellArray = 5;
                    //        cantripArray = 5;
                    //    }
                    //    else if (armorPiece == 4)
                    //    {
                    //        armorWeenie = 37200;
                    //        armorPieceType = 1;
                    //        wieldDifficulty = 180;
                    //        spellArray = 8;
                    //        cantripArray = 8;
                    //    }
                    //    else
                    //    {
                    //        armorWeenie = 37195;
                    //        armorPieceType = 2;
                    //        wieldDifficulty = 180;
                    //        spellArray = 1;
                    //        cantripArray = 1;
                    //    }
                    //    materialType = GetMaterialType(7, 1);
                    //}
                    //////Olthoi Amuli
                    //if (armorType == 19)
                    //{
                    //    int armorPiece = r.Next(0, 5);
                    //    if (armorPiece == 0)
                    //    {
                    //        armorWeenie = 37208;
                    //        armorPieceType = 4;
                    //        wieldDifficulty = 180;
                    //        spellArray = 9;
                    //        cantripArray = 9;
                    //    }
                    //    else if (armorPiece == 1)
                    //    {
                    //        armorWeenie = 37299;
                    //        armorPieceType = 1;
                    //        wieldDifficulty = 180;
                    //        spellArray = 2;
                    //        cantripArray = 2;
                    //    }
                    //    else if (armorPiece == 2)
                    //    {
                    //        armorWeenie = 37188;
                    //        armorPieceType = 3;
                    //        wieldDifficulty = 180;
                    //        spellArray = 5;
                    //        cantripArray = 5;
                    //    }
                    //    else if (armorPiece == 3)
                    //    {
                    //        armorWeenie = 37201;
                    //        armorPieceType = 1;
                    //        wieldDifficulty = 180;
                    //        spellArray = 8;
                    //        cantripArray = 8;
                    //    }
                    //    else
                    //    {
                    //        armorWeenie = 37196;
                    //        armorPieceType = 2;
                    //        wieldDifficulty = 180;
                    //        spellArray = 1;
                    //        cantripArray = 1;
                    //    }
                    //    materialType = GetMaterialType(6, 1);
                    //}
                    //////Olthoi Celdon
                    //if (armorType == 20)
                    //{
                    //    int armorPiece = r.Next(0, 7);
                    //    if (armorPiece == 0)
                    //    {
                    //        armorWeenie = 37209;
                    //        armorPieceType = 4;
                    //        wieldDifficulty = 180;
                    //        spellArray = 9;
                    //        cantripArray = 9;
                    //    }
                    //    else if (armorPiece == 1)
                    //    {
                    //        armorWeenie = 37214;
                    //        armorPieceType = 1;
                    //        wieldDifficulty = 180;
                    //        spellArray = 2;
                    //        cantripArray = 2;
                    //    }
                    //    else if (armorPiece == 2)
                    //    {
                    //        armorWeenie = 37189;
                    //        armorPieceType = 3;
                    //        wieldDifficulty = 180;
                    //        spellArray = 5;
                    //        cantripArray = 5;
                    //    }
                    //    else if (armorPiece == 3)
                    //    {
                    //        armorWeenie = 37202;
                    //        armorPieceType = 1;
                    //        wieldDifficulty = 180;
                    //        spellArray = 8;
                    //        cantripArray = 8;
                    //    }
                    //    else if (armorPiece == 4)
                    //    {
                    //        armorWeenie = 37192;
                    //        armorPieceType = 1;
                    //        wieldDifficulty = 180;
                    //        spellArray = 6;
                    //        cantripArray = 6;
                    //    }
                    //    else if (armorPiece == 5)
                    //    {
                    //        armorWeenie = 37205;
                    //        armorPieceType = 1;
                    //        wieldDifficulty = 180;
                    //        spellArray = 3;
                    //        cantripArray = 3;
                    //    }
                    //    else
                    //    {
                    //        armorWeenie = 37197;
                    //        armorPieceType = 2;
                    //        wieldDifficulty = 180;
                    //        spellArray = 1;
                    //        cantripArray = 1;
                    //    }
                    //    materialType = GetMaterialType(7, 1);
                    //}
                    //////Olthoi Celdon
                    //if (armorType == 21)
                    //{
                    //    int armorPiece = r.Next(0, 5);
                    //    if (armorPiece == 0)
                    //    {
                    //        armorWeenie = 37215;
                    //        armorPieceType = 1;
                    //        wieldDifficulty = 180;
                    //        spellArray = 2;
                    //        cantripArray = 2;
                    //    }
                    //    else if (armorPiece == 1)
                    //    {
                    //        armorWeenie = 37190;
                    //        armorPieceType = 3;
                    //        wieldDifficulty = 180;
                    //        spellArray = 5;
                    //        cantripArray = 5;
                    //    }
                    //    else if (armorPiece == 2)
                    //    {
                    //        armorWeenie = 37203;
                    //        armorPieceType = 1;
                    //        wieldDifficulty = 180;
                    //        spellArray = 8;
                    //        cantripArray = 8;
                    //    }
                    //    else if (armorPiece == 3)
                    //    {
                    //        armorWeenie = 37206;
                    //        armorPieceType = 1;
                    //        wieldDifficulty = 180;
                    //        spellArray = 3;
                    //        cantripArray = 3;
                    //    }
                    //    else
                    //    {
                    //        armorWeenie = 37198;
                    //        armorPieceType = 2;
                    //        wieldDifficulty = 180;
                    //        spellArray = 1;
                    //        cantripArray = 1;
                    //    }
                    //    materialType = GetMaterialType(7, 1);
                    //}
                    if (armorType == 15)
                    {
                        int armorPiece = r.Next(0, 29);
                        if (armorPiece == 0)
                        {
                            ////bandana
                            armorWeenie = 28612;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 1)
                        {
                            ////beret
                            armorWeenie = 28605;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 2)
                        {
                            ////Cloth Cap
                            armorWeenie = 118;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 3)
                        {
                            ////Cloth gloves
                            armorWeenie = 121;
                            armorPieceType = 3;
                            spellArray = 5;
                            cantripArray = 5;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 4)
                        {
                            ////Cowl
                            armorWeenie = 119;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 5)
                        {
                            ////crown
                            armorWeenie = 296;
                            armorPieceType = 1;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 6)
                        {
                            ////Fez
                            armorWeenie = 5894;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 7)
                        {
                            ////hood
                            armorWeenie = 5905;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 8)
                        {
                            ////Kasa
                            armorWeenie = 5901;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 9)
                        {
                            ////Metal cap
                            armorWeenie = 46;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 10)
                        {
                            ////Qafiya
                            armorWeenie = 76;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 11)
                        {
                            ////turban
                            armorWeenie = 135;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 12)
                        {
                            ////loafers
                            armorWeenie = 28610;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 13)
                        {
                            ////sandals
                            armorWeenie = 129;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 14)
                        {
                            ////shoes
                            armorWeenie = 132;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 15)
                        {
                            ////slippers
                            armorWeenie = 133;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(8, 1);
                        }
                        else if (armorPiece == 16)
                        {
                            ////steel toed boots
                            armorWeenie = 7897;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(6, 1);
                        }
                        else if (armorPiece == 17)
                        {
                            ////buckler
                            armorWeenie = 44;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 18)
                        {
                            ////kite shield
                            armorWeenie = 91;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 19)
                        {
                            ////large Kite Shield
                            armorWeenie = 92;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 20)
                        {
                            ////Circlet
                            armorWeenie = 29528;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 21)
                        {
                            ////Armet
                            armorWeenie = 8488;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 22)
                        {
                            ////Baigha
                            armorWeenie = 550;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 23)
                        {
                            ////Heaume
                            armorWeenie = 74;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 24)
                        {
                            ////Helmet
                            armorWeenie = 75;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        //else if (armorPiece == 25)
                        //{
                        //    ////Horned Helm
                        //    armorWeenie = 92;
                        //    armorPieceType = 5;
                        //    spellArray = 10;
                        //    cantripArray = 10;
                        //}
                        else if (armorPiece == 25)
                        {
                            ////Kabuton
                            armorWeenie = 77;
                            armorPieceType = 2;
                            spellArray = 1;
                            cantripArray = 1;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 26)
                        {
                            ////sollerets
                            armorWeenie = 107;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 27)
                        {
                            ////viamontian laced boots
                            armorWeenie = 28611;
                            armorPieceType = 4;
                            spellArray = 9;
                            cantripArray = 9;
                            materialType = GetMaterialType(7, 1);
                        }
                        else if (armorPiece == 28)
                        {
                            ////RTower Shield
                            armorWeenie = 95;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                        //else if (armorPiece == 29)
                        //{
                        //    ////Signet Crown
                        //    armorWeenie = 31868;
                        //    armorPieceType = 2;
                        //    spellArray = 1;
                        //    cantripArray = 1;
                        //    materialType = GetMaterialType(7, 1);
                        //}
                        //else if (armorPiece == 30)
                        //{
                        //    ////Suikan Over-Robe
                        //    armorWeenie = 44801;
                        //    equipSetId = 15;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 31)
                        //{
                        //    ////Faran Over-Robe
                        //    armorWeenie = 44799;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 32)
                        //{
                        //    ////Dho Vest and  Over-Robe
                        //    armorWeenie = 44800;
                        //    equipSetId = 14;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 33)
                        //{
                        //    ////Suikan Over-Robe
                        //    armorWeenie = 44802;
                        //    ////equipSetId = 15;
                        //    armorPieceType = 5;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 30)
                        //{
                        //    ////Coronet
                        //    armorWeenie = 31866;
                        //    armorPieceType = 2;
                        //    spellArray = 1;
                        //    cantripArray = 1;
                        //    materialType = GetMaterialType(7, 1);
                        //}
                        //else if (armorPiece == 31)
                        //{
                        //    ////Diadem
                        //    armorWeenie = 31867;
                        //    armorPieceType = 2;
                        //    spellArray = 1;
                        //    cantripArray = 1;
                        //    materialType = GetMaterialType(7, 1);
                        //}
                        //else if (armorPiece == 32)
                        //{
                        //    ////Teardrop
                        //    armorWeenie = 31864;
                        //    armorPieceType = 2;
                        //    spellArray = 1;
                        //    cantripArray = 1;
                        //    materialType = GetMaterialType(7, 1);
                        //}
                        //else if (armorPiece == 37)
                        //{
                        //    ////Lyceum Hood
                        //    armorWeenie = 44977;
                        //    armorPieceType = 2;
                        //    spellArray = 1;
                        //    cantripArray = 1;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        //else if (armorPiece == 38)
                        //{
                        //    ////Empyrean Over-Robe
                        //    armorWeenie = 43274;
                        //    armorPieceType = 1;
                        //    spellArray = 2;
                        //    cantripArray = 2;
                        //    materialType = GetMaterialType(8, 1);
                        //}
                        else
                        {
                            ////Round Shield
                            armorWeenie = 93;
                            armorPieceType = 5;
                            spellArray = 10;
                            cantripArray = 10;
                            materialType = GetMaterialType(7, 1);
                        }
                    }
                    wieldDifficulty = 180;
                    break;

            }
            ////ArmorModVsSlash, with a value between 0.0-2.0.
            armorModSlash = .1 * r.Next(1, 21);
            ////ArmorModVsPierce, with a value between 0.0-2.0.
            armorModPierce = .1 * r.Next(1, 21);
            ////ArmorModVsBludgeon, with a value between 0.0-2.0.
            armorModBludge = .1 * r.Next(1, 21);
            ////ArmorModVsCold, with a value between 0.0-2.0.
            armorModCold = .1 * r.Next(1, 21);
            ////ArmorModVsFire, with a value between 0.0-2.0.
            armorModFire = .1 * r.Next(1, 21);
            ////ArmorModVsAcid, with a value between 0.0-2.0.
            armorModAcid = .1 * r.Next(1, 21);
            ////ArmorModVsElectric, with a value between 0.0-2.0.
            armorModElectric = .1 * r.Next(1, 21);
            ////ArmorModVsNether, with a value between 0.0-2.0.
            armorModNether = .1 * r.Next(1, 21);

            WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)armorWeenie);
            int gemCount = r.Next(1, 6);
            int gemType = r.Next(10, 51);
            wo.SetProperty(PropertyInt.MaterialType, materialType);
            wo.SetProperty(PropertyInt.GemCount, gemCount);
            wo.SetProperty(PropertyInt.GemType, gemType);
            wo.SetProperty(PropertyFloat.ArmorModVsSlash, armorModSlash);
            wo.SetProperty(PropertyFloat.ArmorModVsPierce, armorModPierce);
            wo.SetProperty(PropertyFloat.ArmorModVsCold, armorModCold);
            wo.SetProperty(PropertyFloat.ArmorModVsBludgeon, armorModBludge);
            wo.SetProperty(PropertyFloat.ArmorModVsFire, armorModFire);
            wo.SetProperty(PropertyFloat.ArmorModVsAcid, armorModAcid);
            wo.SetProperty(PropertyFloat.ArmorModVsElectric, armorModElectric);
            wo.SetProperty(PropertyFloat.ArmorModVsNether, armorModNether);
            wo.SetProperty(PropertyInt.AppraisalItemSkill, 7);
            wo.SetProperty(PropertyInt.EquipmentSetId, equipSetId);
            wo.SetProperty(PropertyInt.AppraisalLongDescDecoration, 1);
            ////Encumberance will be added based on item in the future
            int numSpells = GetNumSpells(tier);
            maxMana = GetMaxMana(numSpells, tier);
            curMana = maxMana;
            wo.SetProperty(PropertyInt.ItemMaxMana, maxMana);
            wo.SetProperty(PropertyInt.ItemCurMana, curMana);
            int numCantrips = GetNumCantrips(numSpells);
            if(spellArray == 1)
            {
                spells = LootHelper.HeadSpells;
            }
            else if (spellArray == 2)
            {
                spells = LootHelper.ChestSpells;
            }
            else if (spellArray == 3)
            {
                spells = LootHelper.UpperArmSpells;
            }
            else if (spellArray == 4)
            {
                spells = LootHelper.LowerArmSpells;
            }
            else if (spellArray == 5)
            {
                spells = LootHelper.HandSpells;
            }
            else if (spellArray == 6)
            {
                spells = LootHelper.AbdomenSpells;
            }
            else if (spellArray == 7)
            {
                spells = LootHelper.UpperLegSpells;
            }
            else if (spellArray == 8)
            {
                spells = LootHelper.LowerLegSpells;
            }
            else if (spellArray == 9)
            {
                spells = LootHelper.FeetSpells;
            }
            else
            {
                spells = LootHelper.ShieldSpells;
            }
            if (cantripArray == 1)
            {
                cantrips = LootHelper.HeadCantrips;
            }
            else if (cantripArray == 2)
            {
                cantrips = LootHelper.ChestCantrips;
            }
            else if (cantripArray == 3)
            {
                cantrips = LootHelper.UpperArmCantrips;
            }
            else if (cantripArray == 4)
            {
                cantrips = LootHelper.LowerArmCantrips;
            }
            else if (cantripArray == 5)
            {
                cantrips = LootHelper.HandCantrips;
            }
            else if (cantripArray == 6)
            {
                cantrips = LootHelper.AbdomenCantrips;
            }
            else if (cantripArray == 7)
            {
                cantrips = LootHelper.UpperLegCantrips;
            }
            else if (cantripArray == 8)
            {
                cantrips = LootHelper.LowerLegCantrips;
            }
            else if (cantripArray == 9)
            {
                cantrips = LootHelper.FeetCantrips;
            }
            else
            {
                cantrips = LootHelper.ShieldCantrips;
            }
            int[] shuffledValues = new int[spells.Length];
            for (int i = 0; i < spells.Length; i++)
            {
                shuffledValues[i] = i;
            }
            Shuffle(shuffledValues);
            if (numSpells - numCantrips > 0)
            {
                wo.SetProperty(PropertyInt.UiEffects, 1);
                for (int a = 0; a < numSpells - numCantrips; a++)
                {
                    int col = r.Next(lowSpellTier - 1, highSpellTier);
                    int spellID = spells[shuffledValues[a]][col];
                    var result = new BiotaPropertiesSpellBook { ObjectId = wo.Biota.Id, Spell = spellID, Object = wo.Biota };
                    wo.Biota.BiotaPropertiesSpellBook.Add(result);
                }
            }
            int minorCantrips = GetNumMinorCantrips(tier);
            int majorCantrips = GetNumMajorCantrips(tier);
            int epicCantrips = GetNumEpicCantrips(tier);
            int legendaryCantrips = GetNumLegendaryCantrips(tier);
            if (numCantrips > 0)
            {
                shuffledValues = new int[cantrips.Length];
                for (int i = 0; i < cantrips.Length; i++)
                {
                    shuffledValues[i] = i;
                }
                Shuffle(shuffledValues);
                int shuffledPlace = 0;
                wo.SetProperty(PropertyInt.UiEffects, 1);
                //minor cantripps
                for (int a = 0; a < minorCantrips; a++)
                {
                    int spellID = cantrips[shuffledValues[shuffledPlace]][0];
                    shuffledPlace++;
                    var result = new BiotaPropertiesSpellBook { ObjectId = wo.Biota.Id, Spell = spellID, Object = wo.Biota };
                    wo.Biota.BiotaPropertiesSpellBook.Add(result);
                }
                //major cantrips
                for (int a = 0; a < majorCantrips; a++)
                {
                    int spellID = cantrips[shuffledValues[shuffledPlace]][1];
                    shuffledPlace++;
                    var result = new BiotaPropertiesSpellBook { ObjectId = wo.Biota.Id, Spell = spellID, Object = wo.Biota };
                    wo.Biota.BiotaPropertiesSpellBook.Add(result);
                }
                // epic cantrips
                for (int a = 0; a < epicCantrips; a++)
                {
                    int spellID = cantrips[shuffledValues[shuffledPlace]][2];
                    shuffledPlace++;
                    var result = new BiotaPropertiesSpellBook { ObjectId = wo.Biota.Id, Spell = spellID, Object = wo.Biota };
                    wo.Biota.BiotaPropertiesSpellBook.Add(result);
                }
                //legendary cantrips
                for (int a = 0; a < legendaryCantrips; a++)
                {
                    int spellID = cantrips[shuffledValues[shuffledPlace]][3];
                    shuffledPlace++;
                    var result = new BiotaPropertiesSpellBook { ObjectId = wo.Biota.Id, Spell = spellID, Object = wo.Biota };
                    wo.Biota.BiotaPropertiesSpellBook.Add(result);
                }
            }
            if (wieldDifficulty > 0)
            {
                wo.SetProperty(PropertyInt.WieldDifficulty, wieldDifficulty);
            }
            else
            {
                wo.RemoveProperty(PropertyInt.WieldDifficulty);
            }
            /////Setting random color
            wo.SetProperty(PropertyInt.PaletteTemplate, r.Next(1, 19));
            double shade = .1 * (double)r.Next(0, 10);
            wo.SetProperty(PropertyFloat.Shade, shade);
            spellcraft = GetSpellcraft(numSpells, tier);
            wo.SetProperty(PropertyInt.ItemSpellcraft, spellcraft);
            wo.SetProperty(PropertyInt.ItemDifficulty, GetDifficulty(tier, spellcraft));
            wo.SetProperty(PropertyInt.ItemWorkmanship, GetWorkmanship(tier));
            wo.SetProperty(PropertyInt.UiEffects, 1);
            wo.SetProperty(PropertyInt.Value, GetValue(tier));
            wo.SetProperty(PropertyInt.GemCount, GetWorkmanship(tier));
            wo.SetProperty(PropertyInt.ArmorLevel, GetArmorLevel(tier, armorPieceType));
            if (numSpells == 0)
            {
                wo.RemoveProperty(PropertyInt.ItemManaCost);
                wo.RemoveProperty(PropertyInt.ItemMaxMana);
                wo.RemoveProperty(PropertyInt.ItemCurMana);
                wo.RemoveProperty(PropertyInt.ItemSpellcraft);
                wo.RemoveProperty(PropertyInt.ItemDifficulty);
                wo.SetProperty(PropertyInt.UiEffects, 0);
            }
            
            return wo;
        }

        public static int GetArmorLevel(int tier, int armorType)
        {
            Random r = new Random();
            switch(tier)
            {
                case 1:
                    if(armorType == 1)
                    {
                        return r.Next(10, 38);
                    }
                    else if(armorType == 5)
                    {
                        return r.Next(10, 34);
                    }
                    else
                    {
                        return r.Next(10, 51);
                    }
                case 2:
                    if (armorType == 1)
                    {
                        return r.Next(37, 73);
                    }
                    else if (armorType == 5)
                    {
                        return r.Next(34, 58);
                    }
                    else
                    {
                        return r.Next(51, 91);
                    }
                case 3:
                    if (armorType == 1)
                    {
                        return r.Next(73, 109);
                    }
                    else if (armorType == 5)
                    {
                        return r.Next(58, 82);
                    }
                    else
                    {
                        return r.Next(92, 132);
                    }
                case 4:
                    if (armorType == 1)
                    {
                        return r.Next(109, 145);
                    }
                    else if (armorType == 5)
                    {
                        return r.Next(82, 106);
                    }
                    else
                    {
                        return r.Next(133, 173);
                    }
                case 5:
                    if (armorType == 1)
                    {
                        return r.Next(145, 181);
                    }
                    else if (armorType == 5)
                    {
                        return r.Next(106, 130);
                    }
                    else
                    {
                        return r.Next(173, 213);
                    }
                case 6:
                    if (armorType == 1)
                    {
                        return r.Next(181, 217);
                    }
                    else if (armorType == 5)
                    {
                        return r.Next(130, 154);
                    }
                    else
                    {
                        return r.Next(213, 254);
                    }
                case 7:
                    if (armorType == 1)
                    {
                        return r.Next(217, 253);
                    }
                    else if (armorType == 5)
                    {
                        return r.Next(154, 178);
                    }
                    else
                    {
                        return r.Next(254, 294);
                    }
                case 8:
                    if (armorType == 1)
                    {
                        return r.Next(253, 304);
                    }
                    else if (armorType == 5)
                    {
                        return r.Next(178, 202);
                    }
                    else
                    {
                        return r.Next(294, 335);
                    }
            }
            return 0;
        }

        public static int GetWield(int tier, int type)
        {
            Random r = new Random();
            int wield = 0;
            int chance = 0;
            ////Types: 1 Missiles, 2 Casters, 3 melee weapons, 4 covenant armor
            switch(type)
            {
                case 1:
                    switch(tier)
                    {
                        case 1:
                            wield = 0;
                            break;
                        case 2:
                            chance = r.Next(0, 100);
                            if (chance < 60)
                            {
                                wield = 0;
                            }
                            else
                            {
                                wield = 250;
                            }
                            break;
                        case 3:
                            chance = r.Next(0, 100);
                            if (chance < 60)
                            {
                                wield = 0;
                            }
                            else if (chance < 90)
                            {
                                wield = 250;
                            }
                            else
                            {
                                wield = 270;
                            }
                            break;
                        case 4:
                            chance = r.Next(0, 100);
                            if (chance < 60)
                            {
                                wield = 0;
                            }
                            else if (chance < 90)
                            {
                                wield = 250;
                            }
                            else
                            {
                                wield = 270;
                            }
                            break;
                        case 5:
                            chance = r.Next(0, 100);
                            if (chance < 60)
                            {
                                wield = 270;
                            }
                            else if (chance < 90)
                            {
                                wield = 290;
                            }
                            else
                            {
                                wield = 315;
                            }
                            break;
                        case 6:
                            chance = r.Next(0, 100);
                            if (chance < 60)
                            {
                                wield = 315;
                            }
                            else if (chance < 90)
                            {
                                wield = 335;
                            }
                            else
                            {
                                wield = 360;
                            }
                            break;
                        case 7:
                            chance = r.Next(0, 100);
                            if (chance < 60)
                            {
                                wield = 335;
                            }
                            else if (chance < 90)
                            {
                                wield = 360;
                            }
                            else
                            {
                                wield = 375;
                            }
                            break;
                        case 8:
                            chance = r.Next(0, 100);
                            if (chance < 60)
                            {
                                wield = 360;
                            }
                            else if (chance < 90)
                            {
                                wield = 375;
                            }
                            else
                            {
                                wield = 385;
                            }
                            break;
                    }   
                    break;
                case 2:
                    switch (tier)
                    {
                        case 1:
                            wield = 0;
                            break;
                        case 2:
                            wield = 0;
                            break;
                        case 3:
                            wield = 0;
                            break;
                        case 4:
                            wield = 0;
                            break;
                        case 5:
                            chance = r.Next(0, 100);
                            if (chance < 60)
                            {
                                wield = 0;
                            }
                            else if (chance < 90)
                            {
                                wield = 290;
                            }
                            else
                            {
                                wield = 310;
                            }
                            break;
                        case 6:
                            chance = r.Next(0, 100);
                            if (chance < 40)
                            {
                                wield = 0;
                            }
                            else if (chance < 70)
                            {
                                wield = 310;
                            }
                            else if (chance < 90)
                            {
                                wield = 330;
                            }
                            else
                            {
                                wield = 365;
                            }
                            break;
                        case 7:
                            chance = r.Next(0, 100);
                            if (chance < 60)
                            {
                                wield = 330;
                            }
                            else if (chance < 90)
                            {
                                wield = 365;
                            }
                            else
                            {
                                wield = 375;
                            }
                            break;
                        case 8:
                            chance = r.Next(0, 100);
                            if (chance < 60)
                            {
                                wield = 355;
                            }
                            else if (chance < 90)
                            {
                                wield = 375;
                            }
                            else
                            {
                                wield = 385;
                            }
                            break;
                    }
                    break;
                case 3:
                    switch (tier)
                    {
                        case 1:
                            wield = 0;
                            break;
                        case 2:
                            chance = r.Next(0, 100);
                            if (chance < 60)
                            {
                                wield = 0;
                            }
                            else
                            {
                                wield = 250;
                            }
                            break;
                        case 3:
                            chance = r.Next(0, 100);
                            if (chance < 60)
                            {
                                wield = 0;
                            }
                            else if (chance < 90)
                            {
                                wield = 250;
                            }
                            else
                            {
                                wield = 300;
                            }
                            break;
                        case 4:
                            chance = r.Next(0, 100);
                            if (chance < 60)
                            {
                                wield = 0;
                            }
                            else if (chance < 90)
                            {
                                wield = 250;
                            }
                            else
                            {
                                wield = 300;
                            }
                            break;
                        case 5:
                            chance = r.Next(0, 100);
                            if (chance < 60)
                            {
                                wield = 300;
                            }
                            else if (chance < 90)
                            {
                                wield = 325;
                            }
                            else
                            {
                                wield = 350;
                            }
                            break;
                        case 6:
                            chance = r.Next(0, 100);
                            if (chance < 60)
                            {
                                wield = 350;
                            }
                            else if (chance < 90)
                            {
                                wield = 370;
                            }
                            else
                            {
                                wield = 400;
                            }
                            break;
                        case 7:
                            chance = r.Next(0, 100);
                            if (chance < 60)
                            {
                                wield = 370;
                            }
                            else if (chance < 90)
                            {
                                wield = 400;
                            }
                            else
                            {
                                wield = 420;
                            }
                            break;
                        case 8:
                            chance = r.Next(0, 100);
                            if (chance < 60)
                            {
                                wield = 400;
                            }
                            else if (chance < 90)
                            {
                                wield = 420;
                            }
                            else
                            {
                                wield = 430;
                            }
                            break;
                    }
                    break;
            }
            return wield;
        }

        public static double GetManaRate()
        {
            Random r = new Random();
            double manaRate = 1.0 / (double)(r.Next(10, 31));
            return manaRate;
        }

        public static int GetNumSpells(int tier)
        {
            Random r = new Random();
            int chance = 0;
            int numSpells = 0;
            switch (tier)
            {
                case 1:
                    ////1-3, minor cantrips
                    if (r.Next(0, 100) > 60)
                    {
                        chance = r.Next(1, 100);
                        if (chance < 50)
                        {
                            numSpells = 1;
                        }
                        else if (chance < 90)
                        {
                            numSpells = 2;
                        }
                        else
                        {
                            numSpells = 3;
                        }
                    }
                    break;
                case 2:
                    ////3-5 minor, and major
                    if (r.Next(0, 100) > 60)
                    {
                        chance = r.Next(1, 1000);
                        if (chance < 500)
                        {
                            numSpells = 1;
                        }
                        else if (chance < 900)
                        {
                            numSpells = 2;
                        }
                        else if (chance < 950)
                        {
                            numSpells = 3;
                        }
                        else if (chance < 998)
                        {
                            numSpells = 4;
                        }
                        else
                        {
                            numSpells = 5;
                        }
                    }
                    break;
                case 3:
                    //4-6, major/minor
                    if (r.Next(0, 100) > 60)
                    {
                        chance = r.Next(1, 1000);
                        if (chance < 500)
                        {
                            numSpells = 1;
                        }
                        else if (chance < 800)
                        {
                            numSpells = 2;
                        }
                        else if (chance < 900)
                        {
                            numSpells = 3;
                        }
                        else if (chance < 950)
                        {
                            numSpells = 4;
                        }
                        else if (chance < 985)
                        {
                            numSpells = 5;
                        }
                        else
                        {
                            numSpells = 6;
                        }
                    }
                    break;
                case 4:
                    //5-6, major and minor
                    if (r.Next(0, 100) > 60)
                    {
                        chance = r.Next(1, 1000);
                        if (chance < 500)
                        {
                            numSpells = 1;
                        }
                        else if (chance < 800)
                        {
                            numSpells = 2;
                        }
                        else if (chance < 900)
                        {
                            numSpells = 3;
                        }
                        else if (chance < 950)
                        {
                            numSpells = 4;
                        }
                        else if (chance < 985)
                        {
                            numSpells = 5;
                        }
                        else
                        {
                            numSpells = 6;
                        }
                    }
                    break;
                case 5:
                    //5-7 major/minor
                    if (r.Next(0, 100) > 60)
                    {
                        chance = r.Next(1, 1000);
                        if (chance < 500)
                        {
                            numSpells = 1;
                        }
                        else if (chance < 600)
                        {
                            numSpells = 2;
                        }
                        else if (chance < 700)
                        {
                            numSpells = 3;
                        }
                        else if (chance < 850)
                        {
                            numSpells = 4;
                        }
                        else if (chance < 940)
                        {
                            numSpells = 5;
                        }
                        else if (chance < 980)
                        {
                            numSpells = 6;
                        }
                        else
                        {
                            numSpells = 7;
                        }
                    }
                    break;
                case 6:
                    //6-7, minor(4 total) major(2 total)
                    if (r.Next(0, 100) > 60)
                    {
                        chance = r.Next(1, 1000);
                        if (chance < 200)
                        {
                            numSpells = 1;
                        }
                        else if (chance < 300)
                        {
                            numSpells = 2;
                        }
                        else if (chance < 400)
                        {
                            numSpells = 3;
                        }
                        else if (chance < 500)
                        {
                            numSpells = 4;
                        }
                        else if (chance < 600)
                        {
                            numSpells = 5;
                        }
                        else if (chance < 700)
                        {
                            numSpells = 6;
                        }
                        else if (chance < 950)
                        {
                            numSpells = 7;
                        }
                        else
                        {
                            numSpells = 8;
                        }
                    }
                    break;
                case 7:
                    ///6-8, minor(4), major(5), epic(3)
                    if (r.Next(0, 100) > 60)
                    {
                        chance = r.Next(1, 1000);
                        if (chance < 200)
                        {
                            numSpells = 1;
                        }
                        else if (chance < 300)
                        {
                            numSpells = 2;
                        }
                        else if (chance < 400)
                        {
                            numSpells = 3;
                        }
                        else if (chance < 500)
                        {
                            numSpells = 4;
                        }
                        else if (chance < 600)
                        {
                            numSpells = 5;
                        }
                        else if (chance < 700)
                        {
                            numSpells = 6;
                        }
                        else if (chance < 950)
                        {
                            numSpells = 7;
                        }
                        else
                        {
                            numSpells = 8;
                        }
                    }
                    break;
                case 8:
                    //6-8, minor(4), major(5), epic(3), legendary(2)
                    if (r.Next(0, 100) > 60)
                    {
                        chance = r.Next(1, 1000);
                        if (chance < 200)
                        {
                            numSpells = 1;
                        }
                        else if (chance < 300)
                        {
                            numSpells = 2;
                        }
                        else if (chance < 400)
                        {
                            numSpells = 3;
                        }
                        else if (chance < 500)
                        {
                            numSpells = 4;
                        }
                        else if (chance < 600)
                        {
                            numSpells = 5;
                        }
                        else if (chance < 700)
                        {
                            numSpells = 6;
                        }
                        else if (chance < 950)
                        {
                            numSpells = 7;
                        }
                        else
                        {
                            numSpells = 8;
                        }
                    }
                    break;
                default:
                    break;

            }
            return numSpells;
        }

        public static int GetNumCantrips(int spellAmount)
        {
            Random r = new Random();
            int chance = 0;
            int numSpells = 0;
            switch (spellAmount)
            {
                case 1:
                    if (r.Next(0, 100) > 90)
                    {
                        return 1; 
                    }
                    break;
                case 2:
                    if (r.Next(0, 100) > 90)
                    {
                        chance = r.Next(1, 1000);
                        if (chance < 750)
                        {
                            numSpells = 1;
                        }
                        else
                        {
                            numSpells = 2;
                        }
                    }
                    break;
                case 3:
                    if (r.Next(0, 100) > 60)
                    {
                        chance = r.Next(1, 1000);
                        if (chance < 750)
                        {
                            numSpells = 1;
                        }
                        else if (chance < 900)
                        {
                            numSpells = 2;
                        }
                        else
                        {
                            numSpells = 3;
                        }
                    }
                    break;
                case 4:
                    if (r.Next(0, 100) > 60)
                    {
                        chance = r.Next(1, 1000);
                        if (chance < 500)
                        {
                            numSpells = 1;
                        }
                        else if (chance < 800)
                        {
                            numSpells = 2;
                        }
                        else if (chance < 950)
                        {
                            numSpells = 3;
                        }
                        else
                        {
                            numSpells = 4;
                        }
                    }
                    break;
                case 5:
                    if (r.Next(0, 100) > 60)
                    {
                        chance = r.Next(1, 1000);
                        if (chance < 500)
                        {
                            numSpells = 1;
                        }
                        else if (chance < 600)
                        {
                            numSpells = 2;
                        }
                        else if (chance < 700)
                        {
                            numSpells = 3;
                        }
                        else if (chance < 850)
                        {
                            numSpells = 4;
                        }
                        else 
                        {
                            numSpells = 5;
                        }
                    }
                    break;
                case 6:
                    if (r.Next(0, 100) > 60)
                    {
                        chance = r.Next(1, 1000);
                        if (chance < 200)
                        {
                            numSpells = 1;
                        }
                        else if (chance < 300)
                        {
                            numSpells = 2;
                        }
                        else if (chance < 400)
                        {
                            numSpells = 3;
                        }
                        else if (chance < 500)
                        {
                            numSpells = 4;
                        }
                        else if (chance < 600)
                        {
                            numSpells = 5;
                        }
                        else if (chance < 700)
                        {
                            numSpells = 6;
                        }
                    }
                    break;
                case 7:
                    if (r.Next(0, 100) > 60)
                    {
                        chance = r.Next(1, 1000);
                        if (chance < 200)
                        {
                            numSpells = 1;
                        }
                        else if (chance < 300)
                        {
                            numSpells = 2;
                        }
                        else if (chance < 400)
                        {
                            numSpells = 3;
                        }
                        else if (chance < 500)
                        {
                            numSpells = 4;
                        }
                        else if (chance < 600)
                        {
                            numSpells = 5;
                        }
                        else if (chance < 700)
                        {
                            numSpells = 6;
                        }
                        else
                        {
                            numSpells = 7;
                        }
                    }
                    break;
                case 8:
                    //6-8, minor(4), major(5), epic(3), legendary(2)
                    if (r.Next(0, 100) > 60)
                    {
                        chance = r.Next(1, 1000);
                        if (chance < 200)
                        {
                            numSpells = 1;
                        }
                        else if (chance < 300)
                        {
                            numSpells = 2;
                        }
                        else if (chance < 400)
                        {
                            numSpells = 3;
                        }
                        else if (chance < 500)
                        {
                            numSpells = 4;
                        }
                        else if (chance < 600)
                        {
                            numSpells = 5;
                        }
                        else if (chance < 700)
                        {
                            numSpells = 6;
                        }
                        else if (chance < 950)
                        {
                            numSpells = 7;
                        }
                        else
                        {
                            numSpells = 8;
                        }
                    }
                    break;
                default:
                    break;

            }
            return numSpells;
        }

        public static double GetDamageModifier(int wield, int weaponType)
        {
            ///0 bow
            ///1 crossbow
            ///2 atlatl
            double damageMod = 0;
            if(wield == 0)
            {
                if(weaponType == 0)
                {
                    damageMod = 2.10;
                }
                else if(weaponType == 1)
                {
                    damageMod = 2.40;
                }
                else
                {
                    damageMod = 2.3;
                }
            }
            else
            {
                if (weaponType == 0)
                {
                    damageMod = 2.40;
                }
                else if (weaponType == 1)
                {
                    damageMod = 2.65;
                }
                else
                {
                    damageMod = 2.60;
                }
            }
            return damageMod;
        }

        public static WorldObject CreateMissileWeapon(int tier)
        {
            Random r = new Random();
            int[][] spells = LootHelper.MissileSpells;
            int[][] cantrips = LootHelper.MissileCantrips;
            int weaponWeenie = 0;
            ////Double Values

            double manaRate = -.04166667; ///done
            double maxVelocity = 0; ///done
            double weaponDefense = GetMeleeDMod(20, tier); // done
            double missileDefense = GetMissileDMod(tier);  //done
            double magicDefense = GetMissileDMod(tier);  //done
            double weaponOffense = 1;
            /////Int Valules
            int encumb = 0;
            int gemCount = r.Next(1, 6);
            int gemType = r.Next(10, 51);
            int numSpells = GetNumSpells(tier);
            int numCantrips = GetNumCantrips(numSpells);
            int itemMaxMana = GetMaxMana(numSpells, tier);
            int itemCurMana = itemMaxMana;
            int spellCraft = GetSpellcraft(numSpells, tier);
            int itemDifficulty = GetDifficulty(tier, spellCraft);
            int workmanship = GetWorkmanship(tier);
            int materialType = GetMaterialType(2, tier);
            int value = GetValue(tier);
            int weaponTime = 0; ///done
            int wieldDifficulty = GetWield(tier, 1); // done
            int itemSkillLevelLimit = 0;
            int weaponSkillInt = 0;
            int wieldRequirements = 2;
            int wieldSkillType = 47;
            double elemenatalBonus = GetElementalBonus(wieldDifficulty);
            int lowSpellTier = GetLowSpellTier(tier);
            int highSpellTier = GetHighSpellTier(tier);
            int subType = r.Next(0, 3);
            if (subType == 0)
            {
                ////There are 8 subtypes of Bows
                int subBowType = r.Next(0, 7);
                if (subBowType == 0)
                {
                    ////Longbow
                    encumb = r.Next(700, 1000);
                    weaponWeenie = 306;
                    maxVelocity = 27.3;
                    weaponTime = 45;
                }
                if (subBowType == 1)
                {
                    ////Yumi
                    weaponWeenie = 363;
                    maxVelocity = 27.3;
                    weaponTime = 45;
                    encumb = r.Next(700, 1000);
                }
                if (subBowType == 2)
                {
                    ////Nayin
                    weaponWeenie = 334;
                    maxVelocity = 27.3;
                    weaponTime = 40;
                    encumb = r.Next(700, 1000);
                }
                if (subBowType == 3)
                {
                    ////Shortbow
                    weaponWeenie = 307;
                    maxVelocity = 24.9;
                    weaponTime = 30;
                    encumb = r.Next(300, 400);
                }
                if (subBowType == 4)
                {
                    ////Shouyumi
                    weaponWeenie = 341;
                    maxVelocity = 24.9;
                    weaponTime = 29;
                    encumb = r.Next(300, 400);
                }
                if (subBowType == 5)
                {
                    ////War Bow
                    weaponWeenie = 30625;
                    maxVelocity = 27.3;
                    weaponTime = 43;
                    encumb = r.Next(700, 1000);
                }
                if (subBowType == 6)
                {
                    ////Yag
                    weaponWeenie = 360;
                    maxVelocity = 24.9;
                    weaponTime = 24;
                    encumb = r.Next(300, 400);
                }
            }
            if (subType == 1)
            {
                ////There are 4 subtypes of Crossbows
                int subXbowType = r.Next(0, 3);
                if (subXbowType == 0)
                {
                    ////Arbalest
                    weaponWeenie = 30616;
                    maxVelocity = 27.3;
                    weaponTime = 113;
                    encumb = r.Next(1400, 2000);
                }
                ////Compound Crossbow should go here, but not in the db
                if (subXbowType == 1)
                {
                    ////Heavy Crossbow
                    weaponWeenie = 311;
                    maxVelocity = 27.3;
                    weaponTime = 120;
                    encumb = r.Next(1400, 2000);
                }
                if (subXbowType == 2)
                {
                    ////Light Crossbow
                    weaponWeenie = 312;
                    maxVelocity = 24.9;
                    weaponTime = 58;
                    encumb = r.Next(700, 1000);
                }
            }
            if (subType == 2)
            {
                ////There are 3 subtypes of Atlatl
                int subAtlatlType = r.Next(0, 2);
                if (subAtlatlType == 0)
                {
                    ////Dart Flicker
                    weaponWeenie = 30345;
                    maxVelocity = 27.3;
                    weaponTime = 15;
                    encumb = r.Next(250, 500);
                }
                if (subAtlatlType == 1)
                {
                    ////Royal Atlatl
                    weaponWeenie = 20640;
                    maxVelocity = 27.3;
                    weaponTime = 15;
                    encumb = r.Next(250, 500);
                }
            }
            double damageMod = GetDamageModifier(wieldDifficulty, subType);
            WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)weaponWeenie);
            int[] shuffledValues = new int[spells.Length];
            for (int i = 0; i < spells.Length; i++)
            {
                shuffledValues[i] = i;
            }
            Shuffle(shuffledValues);
            if (numSpells - numCantrips > 0)
            {
                wo.SetProperty(PropertyInt.UiEffects, 1);
                for (int a = 0; a < numSpells - numCantrips; a++)
                {
                    int col = r.Next(lowSpellTier - 1, highSpellTier);
                    int spellID = spells[shuffledValues[a]][col];
                    var result = new BiotaPropertiesSpellBook { ObjectId = wo.Biota.Id, Spell = spellID, Object = wo.Biota };
                    wo.Biota.BiotaPropertiesSpellBook.Add(result);
                }
            }
            int minorCantrips = GetNumMinorCantrips(tier);
            int majorCantrips = GetNumMajorCantrips(tier);
            int epicCantrips = GetNumEpicCantrips(tier);
            int legendaryCantrips = GetNumLegendaryCantrips(tier);
            if (numCantrips > 0)
            {
                shuffledValues = new int[cantrips.Length];
                for (int i = 0; i < cantrips.Length; i++)
                {
                    shuffledValues[i] = i;
                }
                Shuffle(shuffledValues);
                int shuffledPlace = 0;
                wo.SetProperty(PropertyInt.UiEffects, 1);
                //minor cantripps
                for (int a = 0; a < minorCantrips; a++)
                {
                    int spellID = cantrips[shuffledValues[shuffledPlace]][0];
                    shuffledPlace++;
                    var result = new BiotaPropertiesSpellBook { ObjectId = wo.Biota.Id, Spell = spellID, Object = wo.Biota };
                    wo.Biota.BiotaPropertiesSpellBook.Add(result);
                }
                //major cantrips
                for (int a = 0; a < majorCantrips; a++)
                {
                    int spellID = cantrips[shuffledValues[shuffledPlace]][1];
                    shuffledPlace++;
                    var result = new BiotaPropertiesSpellBook { ObjectId = wo.Biota.Id, Spell = spellID, Object = wo.Biota };
                    wo.Biota.BiotaPropertiesSpellBook.Add(result);
                }
                // epic cantrips
                for (int a = 0; a < epicCantrips; a++)
                {
                    int spellID = cantrips[shuffledValues[shuffledPlace]][2];
                    shuffledPlace++;
                    var result = new BiotaPropertiesSpellBook { ObjectId = wo.Biota.Id, Spell = spellID, Object = wo.Biota };
                    wo.Biota.BiotaPropertiesSpellBook.Add(result);
                }
                //legendary cantrips
                for (int a = 0; a < legendaryCantrips; a++)
                {
                    int spellID = cantrips[shuffledValues[shuffledPlace]][3];
                    shuffledPlace++;
                    var result = new BiotaPropertiesSpellBook { ObjectId = wo.Biota.Id, Spell = spellID, Object = wo.Biota };
                    wo.Biota.BiotaPropertiesSpellBook.Add(result);
                }
            }
            int damageType = 0;
            int uiEffects = 0;
            String elementName = "";
            int chance = r.Next(0, 8);
            if (r.Next(1, 100) > 90)
            {
                switch (chance)
                {
                    case 0:
                        ///slash
                        damageType = 1;
                        uiEffects = 1024;
                        elementName = "Slashing";
                        break;
                    case 1:
                        ///pierce
                        damageType = 2;
                        uiEffects = 2049;
                        elementName = "Piercing";
                        break;
                    case 2:
                        //bludge
                        damageType = 4;
                        uiEffects = 513;
                        elementName = "Bludge";
                        break;
                    case 3:
                        //cold
                        damageType = 8;
                        uiEffects = 129;
                        elementName = "Frost";
                        break;
                    case 4:
                        //fire
                        damageType = 16;
                        uiEffects = 33;
                        elementName = "Fire";
                        break;
                    case 5:
                        //acid
                        damageType = 32;
                        uiEffects = 247;
                        elementName = "Acid";
                        break;
                    case 6:
                        //electric
                        damageType = 64;
                        uiEffects = 64;
                        elementName = "Electric";
                        break;
                }
            }

            String shortName = elementName + " " + wo.Name;
            wo.SetProperty(PropertyFloat.DamageMod, damageMod);
            wo.SetProperty(PropertyString.Name, shortName);
            wo.SetProperty(PropertyInt.UiEffects, uiEffects);
            wo.SetProperty(PropertyInt.DamageType, damageType);
            wo.SetProperty(PropertyFloat.ManaRate, manaRate);
            wo.SetProperty(PropertyFloat.MaximumVelocity, maxVelocity);
            wo.SetProperty(PropertyFloat.WeaponDefense, weaponDefense);
            wo.SetProperty(PropertyFloat.WeaponMissileDefense, missileDefense);
            wo.SetProperty(PropertyFloat.WeaponMagicDefense, magicDefense);
            wo.SetProperty(PropertyFloat.WeaponOffense, weaponOffense);
            wo.SetProperty(PropertyInt.EncumbranceVal, encumb);
            wo.SetProperty(PropertyInt.GemCount, gemCount);
            wo.SetProperty(PropertyInt.GemType, gemType);
            wo.SetProperty(PropertyInt.ItemMaxMana, itemMaxMana);
            wo.SetProperty(PropertyInt.ItemCurMana, itemCurMana);
            wo.SetProperty(PropertyInt.ItemSpellcraft, spellCraft);
            wo.SetProperty(PropertyInt.ItemDifficulty, itemDifficulty);
            wo.SetProperty(PropertyInt.ItemWorkmanship, workmanship);
            wo.SetProperty(PropertyInt.MaterialType, materialType);
            wo.SetProperty(PropertyInt.Value, value);
            wo.SetProperty(PropertyInt.WeaponTime, weaponTime);
            wo.SetProperty(PropertyInt.WieldDifficulty, wieldDifficulty);
            wo.SetProperty(PropertyInt.ItemSkillLevelLimit, itemSkillLevelLimit);
            wo.SetProperty(PropertyInt.WeaponSkill, weaponSkillInt);
            wo.SetProperty(PropertyInt.WieldRequirements, wieldRequirements);
            wo.SetProperty(PropertyInt.WieldSkilltype, wieldSkillType);
            if (numSpells == 0)
            {
                wo.RemoveProperty(PropertyInt.ItemManaCost);
                wo.RemoveProperty(PropertyInt.ItemMaxMana);
                wo.RemoveProperty(PropertyInt.ItemCurMana);
                wo.RemoveProperty(PropertyInt.ItemSpellcraft);
                wo.RemoveProperty(PropertyInt.ItemDifficulty);
            }
            else
            {
                wo.SetProperty(PropertyInt.UiEffects, 1);
            }
            if (wieldDifficulty == 0)
            {
                wo.RemoveProperty(PropertyInt.WieldDifficulty);
                wo.RemoveProperty(PropertyInt.WieldRequirements);
                wo.RemoveProperty(PropertyInt.WieldSkilltype);
            }
            return wo;
        }

        public static WorldObject CreateCaster(int tier)
        {
            int[][] WandSpells =
            {
                ////Focus
                new int[] { 1421, 1422, 1423, 1424, 1425, 1426, 2067, 4305 },
                ////Willpower
                new int[] { 1445, 1446, 1447, 1448, 1449, 1450, 2091, 4329 },
                ////Sneak Attack
                new int[] { 5867, 5868, 5869, 5870, 5871, 5872, 5881, 5882 },
                ////Arcane Enlight
                new int[] { 678, 679, 680, 681, 682, 683, 2195, 4510 },
                ////Mana C
                new int[] { 653, 654, 655, 656, 657, 658, 2287, 4602 },
                ////Creature Enchant
                new int[] { 557, 558, 559, 560, 561, 562 , 2215, 4530},
                ////Item Enchant
                new int[] { 581, 582, 583, 584, 585, 586, 2249, 4564 },
                ////Life Magic
                new int[] { 605, 606, 607, 608, 609, 610, 2267, 4582 },
                ////War Magic
                new int[] { 629, 630, 631, 632, 633, 634 , 2287, 4602},
                ////Defender
                new int[] { 1599, 1600, 1601, 1602, 1603, 1604, 2101, 4400 },
                ////Hermetic Link
                new int[] { 1475, 1476, 1477, 1478, 1479, 1480, 2117, 4418 },
             };
            Random r = new Random();
            String weaponName = "";
            String elementName = "";
            int casterWeenie = 0; //done
            int highSpellTier = 0; //done
            int lowSpellTier = 0; //done
            double elementalDamageMod = 0;
            double manaConMod = 0; //done
            double meleeDMod = 0;
            double missileDMod = 0; //done
            int appraisalDesc = 7;  //done
            int damageType = 0; //done
            int gemCount = 0; //done
            int gemType = 0; //done
            int itemDiff = 0; //done
            int itemMaxMana = 0; // done
            int spellcraft = 0; // done
            int workmanship = 0; //done
            int materialType = 0; //done
            int uiEffects = 0; //done
            int value = 0; //done
            int wieldReqs = 2; //done
            int wieldSkillType = 0; //done
            int wield = 0; //done
            int chance = 0; //done
            int numSpells = 0; //done
            switch(tier)
            {
                case 1:
                    wield = 0;
                    break;
                case 2:
                    wield = 0;
                    break;
                case 3:
                    chance = r.Next(0, 100);
                    if(chance < 80)
                    {
                        wield = 0;
                    }
                    else
                    {
                        wield = 290;
                    }
                    break;
                case 4:
                    chance = r.Next(0, 100);
                    if (chance < 60)
                    {
                        wield = 0;
                    }
                    else if(chance < 95)
                    {
                        wield = 290;
                    }
                    else
                    {
                        wield = 310;
                    }
                    break;
                case 5:
                    chance = r.Next(0, 100);
                    if (chance < 50)
                    {
                        wield = 0;
                    }
                    else if (chance < 70)
                    {
                        wield = 290;
                    }
                    else if ( chance < 90)
                    {
                        wield = 310;
                    }
                    else
                    {
                        wield = 330;
                    }
                    break;
                case 6:
                    chance = r.Next(0, 100);
                    if (chance < 40)
                    {
                        wield = 0;
                    }
                    else if (chance < 60)
                    {
                        wield = 290;
                    }
                    else if (chance < 80)
                    {
                        wield = 310;
                    }
                    else if (chance < 90)
                    {
                        wield = 330;
                    }
                    else
                    {
                        wield = 355;
                    }
                    break;
                case 7:
                    chance = r.Next(0, 100);
                    if (chance < 30)
                    {
                        wield = 0;
                    }
                    else if (chance < 50)
                    {
                        wield = 290;
                    }
                    else if (chance < 60)
                    {
                        wield = 310;
                    }
                    else if (chance < 70)
                    {
                        wield = 330;
                    }
                    else if ( chance  < 90)
                    {
                        wield = 355;
                    }
                    else
                    {
                        wield = 375;
                    }
                    break;
                case 8:
                    chance = r.Next(0, 100);
                    if (chance < 25)
                    {
                        wield = 0;
                    }
                    else if (chance < 50)
                    {
                        wield = 290;
                    }
                    else if (chance < 60)
                    {
                        wield = 310;
                    }
                    else if (chance < 70)
                    {
                        wield = 330;
                    }
                    else if (chance < 80)
                    {
                        wield = 355;
                    }
                    else if(chance < 90)
                    {
                        wield = 375;
                    }
                    else
                    {
                        wield = 385;
                    }
                    break;
            }
            ////Getting the caster Weenie needed.
            if(wield == 0)
            {
                int subType = r.Next(0, 4);
                if (subType == 0)
                {
                    ////Orb
                    casterWeenie = 2366;
                    weaponName = "Orb";
                }
                if (subType == 1)
                {
                    ////Sceptre
                    casterWeenie = 2548;
                    weaponName = "Sceptre";
                }
                if (subType == 2)
                {
                    ////staff
                    casterWeenie = 2547;
                    weaponName = "Staff";
                }
                if (subType == 3)
                {
                    ////wand
                    casterWeenie = 2472;
                    weaponName = "Wand";
                }
            }
            else
            {
                int subType = r.Next(0, 3);
                if (subType == 0)
                {
                    ////staff
                    casterWeenie = 29259;
                    weaponName = "Sceptre";
                }
                if (subType == 1)
                {
                    ////Sceptre
                    casterWeenie = 29259;
                    weaponName = "Sceptre";
                }
                if (subType == 2)
                {
                    ////baton
                    casterWeenie = 29259;
                    weaponName = "Sceptre";
                }
            }
            ////Setting the properties of the items here.
            
            if (wield > 0)
            {
                chance = r.Next(0, 5);
                switch (chance)
                {
                    case 0:
                        wieldSkillType = 43;
                        break;
                    case 1:
                        wieldSkillType = 34;
                        break;
                    case 2:
                        wieldSkillType = 31;
                        break;
                    case 3:
                        wieldSkillType = 32;
                        break;
                    case 4:
                        wieldSkillType = 33;
                        break;
                }
                chance = r.Next(0, 9);
                switch(chance)
                {
                    case 0:
                        ///slash
                        damageType = 1;
                        uiEffects = 1024;
                        elementName = "Slashing";
                        break;
                    case 1:
                        ///pierce
                        damageType = 2;
                        uiEffects = 2049;
                        elementName = "Piercing";
                        break;
                    case 2:
                        //bludge
                        damageType = 4;
                        uiEffects = 513;
                        elementName = "Blunt";
                        break;
                    case 3:
                        //cold
                        damageType = 8;
                        uiEffects = 129;
                        elementName = "Frost";
                        break;
                    case 4:
                        //fire
                        damageType = 16;
                        uiEffects = 33;
                        elementName = "Fire";
                        break;
                    case 5:
                        //acid
                        damageType = 32;
                        uiEffects = 247;
                        elementName = "Acid";
                        break;
                    case 6:
                        //electric
                        damageType = 64;
                        uiEffects = 64;
                        elementName = "Electric";
                        break;
                    case 7:
                        //nether
                        damageType = 1024;
                        uiEffects = 1;
                        elementName = "Nether";
                        break;
                }
            }
            if(r.Next(0, 100) > 95)
            {
                missileDMod = GetMissileDMod(tier);
            }
            WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)casterWeenie);
            meleeDMod = GetMeleeDMod(20, tier);
            workmanship = GetWorkmanship(tier);
            materialType = GetMaterialType(3, tier);
            gemCount = r.Next(1, 6);
            gemType = r.Next(10, 51);
            manaConMod = GetManaCMod(tier);
            lowSpellTier = GetLowSpellTier(tier);
            highSpellTier = GetHighSpellTier(tier);
            numSpells = GetNumSpells(tier);
            spellcraft = GetSpellcraft(numSpells, tier);
            itemMaxMana = GetMaxMana(numSpells, tier);
            itemDiff = GetDifficulty(tier, spellcraft);
            value = GetValue(tier);
            String shortDesc = elementName + " " + weaponName;
            elementalDamageMod = GetMaxDamageMod(tier, 18);
            wo.SetProperty(PropertyInt.MaterialType, materialType);
            wo.SetProperty(PropertyInt.Value, value);
            wo.SetProperty(PropertyInt.ItemWorkmanship, workmanship);
            wo.SetProperty(PropertyInt.ItemDifficulty, itemDiff);
            wo.SetProperty(PropertyFloat.ManaRate, GetManaRate());
            wo.SetProperty(PropertyFloat.WeaponMissileDefense, missileDMod);
            wo.SetProperty(PropertyFloat.WeaponDefense, meleeDMod);
            wo.SetProperty(PropertyFloat.ManaConversionMod, manaConMod);
            wo.SetProperty(PropertyFloat.ElementalDamageMod, elementalDamageMod);
            wo.SetProperty(PropertyInt.ItemMaxMana, itemMaxMana );
            wo.SetProperty(PropertyInt.AppraisalLongDescDecoration, appraisalDesc);
            wo.SetProperty(PropertyInt.ItemSpellcraft, spellcraft);
            wo.SetProperty(PropertyInt.DamageType, damageType);
            wo.SetProperty(PropertyInt.GemCount, gemCount);
            wo.SetProperty(PropertyInt.GemType, gemType);
            wo.SetProperty(PropertyString.Name, shortDesc);
            int minorCantrips = GetNumMinorCantrips(tier);
            int majorCantrips = GetNumMajorCantrips(tier);
            int epicCantrips = GetNumEpicCantrips(tier);
            int legendaryCantrips = GetNumLegendaryCantrips(tier);
            int numCantrips = minorCantrips + majorCantrips + epicCantrips + legendaryCantrips;
            if (wield == 0 && numSpells > 0)
            {
                uiEffects = 1;
            }
            wo.SetProperty(PropertyInt.UiEffects, uiEffects);
            if(wield > 0)
            {
                wo.SetProperty(PropertyInt.WieldRequirements, wieldReqs);
                wo.SetProperty(PropertyInt.WieldSkilltype, wieldSkillType);
                wo.SetProperty(PropertyInt.WieldDifficulty, wield);
            }
            else
            {
                wo.RemoveProperty(PropertyFloat.ElementalDamageMod);
            }
            wo.RemoveProperty(PropertyInt.ItemSkillLevelLimit);
            int[][] spells = LootHelper.WandSpells;
            int[][] cantrips = LootHelper.WandCantrips;
            int[] shuffledValues = new int[spells.Length];
            for (int i = 0; i < spells.Length; i++)
            {
                shuffledValues[i] = i;
            }
            Shuffle(shuffledValues);
            if (numSpells - numCantrips > 0)
            {
                wo.SetProperty(PropertyInt.UiEffects, 1);
                for (int a = 0; a < numSpells - numCantrips; a++)
                {
                    int col = r.Next(lowSpellTier - 1, highSpellTier);
                    int spellID = spells[shuffledValues[a]][col];
                    var result = new BiotaPropertiesSpellBook { ObjectId = wo.Biota.Id, Spell = spellID, Object = wo.Biota };
                    wo.Biota.BiotaPropertiesSpellBook.Add(result);
                }
            }
            if (numCantrips > 0)
            {
                shuffledValues = new int[cantrips.Length];
                for (int i = 0; i < cantrips.Length; i++)
                {
                    shuffledValues[i] = i;
                }
                Shuffle(shuffledValues);
                int shuffledPlace = 0;
                wo.SetProperty(PropertyInt.UiEffects, 1);
                //minor cantripps
                for (int a = 0; a < minorCantrips; a++)
                {
                    int spellID = cantrips[shuffledValues[shuffledPlace]][0];
                    shuffledPlace++;
                    var result = new BiotaPropertiesSpellBook { ObjectId = wo.Biota.Id, Spell = spellID, Object = wo.Biota };
                    wo.Biota.BiotaPropertiesSpellBook.Add(result);
                }
                //major cantrips
                for (int a = 0; a < majorCantrips; a++)
                {
                    int spellID = cantrips[shuffledValues[shuffledPlace]][1];
                    shuffledPlace++;
                    var result = new BiotaPropertiesSpellBook { ObjectId = wo.Biota.Id, Spell = spellID, Object = wo.Biota };
                    wo.Biota.BiotaPropertiesSpellBook.Add(result);
                }
                // epic cantrips
                for (int a = 0; a < epicCantrips; a++)
                {
                    int spellID = cantrips[shuffledValues[shuffledPlace]][2];
                    shuffledPlace++;
                    var result = new BiotaPropertiesSpellBook { ObjectId = wo.Biota.Id, Spell = spellID, Object = wo.Biota };
                    wo.Biota.BiotaPropertiesSpellBook.Add(result);
                }
                //legendary cantrips
                for (int a = 0; a < legendaryCantrips; a++)
                {
                    int spellID = cantrips[shuffledValues[shuffledPlace]][3];
                    shuffledPlace++;
                    var result = new BiotaPropertiesSpellBook { ObjectId = wo.Biota.Id, Spell = spellID, Object = wo.Biota };
                    wo.Biota.BiotaPropertiesSpellBook.Add(result);
                }
            }
            if (manaConMod <= 0)
            {
                wo.RemoveProperty(PropertyFloat.ManaConversionMod);
            }
            if(missileDMod <= 0)
            {
                wo.RemoveProperty(PropertyFloat.WeaponMissileDefense);
            }
            if (numSpells == 0)
            {
                wo.RemoveProperty(PropertyInt.ItemManaCost);
                wo.RemoveProperty(PropertyInt.ItemMaxMana);
                wo.RemoveProperty(PropertyInt.ItemCurMana);
                wo.RemoveProperty(PropertyInt.ItemSpellcraft);
                wo.RemoveProperty(PropertyInt.ItemDifficulty);
            }

            return wo;
        }

        public static double GetMaxDamageMod(int tier, int maxDamageMod)
        {
            Random r = new Random();
            double damageMod = 0;
            int chance = 0;
            switch (maxDamageMod)
            {
                case 15:
                    //tier 1
                    switch (tier)
                    {
                        case 1:
                            damageMod = 0;
                            break;
                        case 2:
                            chance = r.Next(0, 100);
                            if (chance < 80)
                            {
                                damageMod = .01;
                            }
                            else if (chance < 95)
                            {
                                damageMod = .02;
                            }
                            else
                            {
                                damageMod = .03;
                            }
                            break;
                        case 3:
                            chance = r.Next(0, 100);
                            if (chance < 50)
                            {
                                damageMod = .03;
                            }
                            else if (chance < 80)
                            {
                                damageMod = .04;
                            }
                            else if (chance < 95)
                            {
                                damageMod = .05;
                            }
                            else
                            {
                                damageMod = .06;
                            }
                            break;
                        case 4:
                            chance = r.Next(0, 100);
                            if (chance < 50)
                            {
                                damageMod = .06;
                            }
                            else if (chance < 80)
                            {
                                damageMod = .07;
                            }
                            else if (chance < 95)
                            {
                                damageMod = .08;
                            }
                            else
                            {
                                damageMod = .09;
                            }
                            break;
                        case 5:
                            chance = r.Next(0, 100);
                            if (chance < 50)
                            {
                                damageMod = .09;
                            }
                            else if (chance < 80)
                            {
                                damageMod = .10;
                            }
                            else if (chance < 95)
                            {
                                damageMod = .11;
                            }
                            else
                            {
                                damageMod = .12;
                            }
                            break;
                        case 6:
                            chance = r.Next(0, 100);
                            if (chance < 50)
                            {
                                damageMod = .09;
                            }
                            else if (chance < 80)
                            {
                                damageMod = .10;
                            }
                            else if (chance < 95)
                            {
                                damageMod = .11;
                            }
                            else
                            {
                                damageMod = .12;
                            }
                            break;
                        case 7:
                            chance = r.Next(0, 100);
                            if (chance < 50)
                            {
                                damageMod = .11;
                            }
                            else if (chance < 80)
                            {
                                damageMod = .12;
                            }
                            else
                            {
                                damageMod = .13;
                            }
                            break;
                        case 8:
                            chance = r.Next(0, 100);
                            if (chance < 50)
                            {
                                damageMod = .13;
                            }
                            else if (chance < 80)
                            {
                                damageMod = .14;
                            }
                            else
                            {
                                damageMod = .15;
                            }

                            break;

                    }
                    break;
                case 18:
                    //tier 1
                    switch (tier)
                    {
                        case 1:
                            damageMod = 0;
                            break;
                        case 2:
                            chance = r.Next(0, 100);
                             if (chance < 80)
                            {
                                damageMod = .01;
                            }
                            else if (chance < 95)
                            {
                                damageMod = .02;
                            }
                            else
                            {
                                damageMod = .03;
                            }
                            break;
                        case 3:
                            chance = r.Next(0, 100);
                            if (chance < 50)
                            {
                                damageMod = .03;
                            }
                            else if (chance < 80)
                            {
                                damageMod = .04;
                            }
                            else if (chance < 95)
                            {
                                damageMod = .05;
                            }
                            else
                            {
                                damageMod = .06;
                            }
                            break;
                        case 4:
                            chance = r.Next(0, 100);
                            if (chance < 50)
                            {
                                damageMod = .06;
                            }
                            else if (chance < 80)
                            {
                                damageMod = .07;
                            }
                            else if (chance < 95)
                            {
                                damageMod = .08;
                            }
                            else
                            {
                                damageMod = .09;
                            }
                            break;
                        case 5:
                            chance = r.Next(0, 100);
                            if (chance < 50)
                            {
                                damageMod = .09;
                            }
                            else if (chance < 80)
                            {
                                damageMod = .10;
                            }
                            else if (chance < 95)
                            {
                                damageMod = .11;
                            }
                            else
                            {
                                damageMod = .12;
                            }
                            break;
                        case 6:
                            chance = r.Next(0, 100);
                            if (chance < 50)
                            {
                                damageMod = .12;
                            }
                            else if (chance < 80)
                            {
                                damageMod = .13;
                            }
                            else if (chance < 95)
                            {
                                damageMod = .14;
                            }
                            else
                            {
                                damageMod = .15;
                            }
                            break;
                        case 7:
                            chance = r.Next(0, 100);
                            if (chance < 50)
                            {
                                damageMod = .15;
                            }
                            else if (chance < 80)
                            {
                                damageMod = .16;
                            }
                            else
                            {
                                damageMod = .17;
                            }
                            break;
                        case 8:
                            chance = r.Next(0, 100);
                            if (chance < 50)
                            {
                                damageMod = .16;
                            }
                            else if (chance < 80)
                            {
                                damageMod = .17;
                            }
                            else
                            {
                                damageMod = .18;
                            }

                            break;

                    }
                    break;
                case 20:
                    //tier 1
                    switch (tier)
                    {
                        case 1:
                            damageMod = 0;
                            break;
                        case 2:
                            chance = r.Next(0, 100);
                            if (chance < 80)
                            {
                                damageMod = .01;
                            }
                            else if (chance < 95)
                            {
                                damageMod = .02;
                            }
                            else
                            {
                                damageMod = .03;
                            }
                            break;
                        case 3:
                            chance = r.Next(0, 100);
                            if (chance < 50)
                            {
                                damageMod = .02;
                            }
                            else if (chance < 80)
                            {
                                damageMod = .03;
                            }
                            else if (chance < 95)
                            {
                                damageMod = .04;
                            }
                            else
                            {
                                damageMod = .05;
                            }
                            break;
                        case 4:
                            chance = r.Next(0, 100);
                            if (chance < 50)
                            {
                                damageMod = .05;
                            }
                            else if (chance < 80)
                            {
                                damageMod = .06;
                            }
                            else if (chance < 95)
                            {
                                damageMod = .07;
                            }
                            else
                            {
                                damageMod = .08;
                            }
                            break;
                        case 5:
                            chance = r.Next(0, 100);
                            if (chance < 50)
                            {
                                damageMod = .07;
                            }
                            else if (chance < 80)
                            {
                                damageMod = .08;
                            }
                            else if (chance < 95)
                            {
                                damageMod = .09;
                            }
                            else
                            {
                                damageMod = .10;
                            }
                            break;
                        case 6:
                            chance = r.Next(0, 100);
                            if (chance < 50)
                            {
                                damageMod = .11;
                            }
                            else if (chance < 80)
                            {
                                damageMod = .12;
                            }
                            else if (chance < 95)
                            {
                                damageMod = .13;
                            }
                            else
                            {
                                damageMod = .14;
                            }
                            break;
                        case 7:
                            chance = r.Next(0, 100);
                            if (chance < 50)
                            {
                                damageMod = .13;
                            }
                            else if (chance < 80)
                            {
                                damageMod = .14;
                            }
                            else if(chance < 90)
                            {
                                damageMod = .15;
                            }
                            else
                            {
                                damageMod = .16;
                            }
                            break;
                        case 8:
                            chance = r.Next(0, 100);
                            if (chance < 50)
                            {
                                damageMod = .17;
                            }
                            else if (chance < 80)
                            {
                                damageMod = .18;
                            }
                            else if(chance < 90)
                            {
                                damageMod = .19;
                            }
                            else
                            {
                                damageMod = .20;
                            }

                            break;

                    }
                    break;
                case 22:
                    //tier 1
                    switch (tier)
                    {
                        case 1:
                            damageMod = 0;
                            break;
                        case 2:
                            chance = r.Next(0, 100);
                            if (chance < 80)
                            {
                                damageMod = .01;
                            }
                            else if (chance < 95)
                            {
                                damageMod = .02;
                            }
                            else
                            {
                                damageMod = .03;
                            }
                            break;
                        case 3:
                            chance = r.Next(0, 100);
                            if (chance < 50)
                            {
                                damageMod = .02;
                            }
                            else if (chance < 80)
                            {
                                damageMod = .03;
                            }
                            else if (chance < 95)
                            {
                                damageMod = .04;
                            }
                            else
                            {
                                damageMod = .05;
                            }
                            break;
                        case 4:
                            chance = r.Next(0, 100);
                            if (chance < 50)
                            {
                                damageMod = .05;
                            }
                            else if (chance < 80)
                            {
                                damageMod = .06;
                            }
                            else if (chance < 95)
                            {
                                damageMod = .07;
                            }
                            else
                            {
                                damageMod = .08;
                            }
                            break;
                        case 5:
                            chance = r.Next(0, 100);
                            if (chance < 50)
                            {
                                damageMod = .07;
                            }
                            else if (chance < 80)
                            {
                                damageMod = .08;
                            }
                            else if (chance < 95)
                            {
                                damageMod = .09;
                            }
                            else
                            {
                                damageMod = .10;
                            }
                            break;
                        case 6:
                            chance = r.Next(0, 100);
                            if (chance < 50)
                            {
                                damageMod = .11;
                            }
                            else if (chance < 80)
                            {
                                damageMod = .12;
                            }
                            else if (chance < 95)
                            {
                                damageMod = .13;
                            }
                            else
                            {
                                damageMod = .14;
                            }
                            break;
                        case 7:
                            chance = r.Next(0, 100);
                            if (chance < 50)
                            {
                                damageMod = .14;
                            }
                            else if (chance < 80)
                            {
                                damageMod = .15;
                            }
                            else if (chance < 90)
                            {
                                damageMod = .16;
                            }
                            else
                            {
                                damageMod = .17;
                            }
                            break;
                        case 8:
                            chance = r.Next(0, 100);
                            if (chance < 50)
                            {
                                damageMod = .18;
                            }
                            else if (chance < 80)
                            {
                                damageMod = .19;
                            }
                            else if (chance < 90)
                            {
                                damageMod = .20;
                            }
                            else if(chance < 95)
                            {
                                damageMod = .21;
                            }
                            else
                            {
                                damageMod = .22;
                            }

                            break;

                    }
                    break;
                case 25:
                    //tier 1
                    switch (tier)
                    {
                        case 1:
                            damageMod = 0;
                            break;
                        case 2:
                            chance = r.Next(0, 100);
                            if (chance < 80)
                            {
                                damageMod = .01;
                            }
                            else if (chance < 70)
                            {
                                damageMod = .02;
                            }
                            else if (chance < 90)
                            {
                                damageMod = .03;
                            }
                            else
                            {
                                damageMod = .04;
                            }
                            break;
                        case 3:
                            chance = r.Next(0, 100);
                            if (chance < 50)
                            {
                                damageMod = .04;
                            }
                            else if (chance < 80)
                            {
                                damageMod = .05;
                            }
                            else if (chance < 95)
                            {
                                damageMod = .06;
                            }
                            else
                            {
                                damageMod = .07;
                            }
                            break;
                        case 4:
                            chance = r.Next(0, 100);
                            if (chance < 50)
                            {
                                damageMod = .07;
                            }
                            else if (chance < 70)
                            {
                                damageMod = .08;
                            }
                            else if (chance < 90)
                            {
                                damageMod = .09;
                            }
                            else if (chance < 96)
                            {
                                damageMod = .10;
                            }
                            else
                            {
                                damageMod = .11;
                            }
                            break;
                        case 5:
                            chance = r.Next(0, 100);
                            if (chance < 50)
                            {
                                damageMod = .11;
                            }
                            else if (chance < 70)
                            {
                                damageMod = .12;
                            }
                            else if (chance < 90)
                            {
                                damageMod = .13;
                            }
                            else if (chance < 96)
                            {
                                damageMod = .14;
                            }
                            else
                            {
                                damageMod = .15;
                            }
                            break;
                        case 6:
                            chance = r.Next(0, 100);
                            if (chance < 50)
                            {
                                damageMod = .15;
                            }
                            else if (chance < 70)
                            {
                                damageMod = .16;
                            }
                            else if (chance < 90)
                            {
                                damageMod = .17;
                            }
                            else
                            {
                                damageMod = .18;
                            }
                            break;
                        case 7:
                            chance = r.Next(0, 100);
                            if (chance < 50)
                            {
                                damageMod = .18;
                            }
                            else if (chance < 80)
                            {
                                damageMod = .19;
                            }
                            else if (chance < 90)
                            {
                                damageMod = .20;
                            }
                            else
                            {
                                damageMod = .21;
                            }
                            break;
                        case 8:
                            chance = r.Next(0, 100);
                            if (chance < 50)
                            {
                                damageMod = .21;
                            }
                            else if (chance < 80)
                            {
                                damageMod = .22;
                            }
                            else if (chance < 90)
                            {
                                damageMod = .23;
                            }
                            else if (chance < 95)
                            {
                                damageMod = .24;
                            }
                            else
                            {
                                damageMod = .25;
                            }

                            break;

                    }
                    break;
                default:
                    break;
            }
            double damageMod2 = 1.0 + damageMod;
            return damageMod2;
        }

        public static double GetElementalBonus(int wield)
        {
            Random r = new Random();
            int chance = 0;
            double eleMod = 0;
            switch(wield)
            {
                case 0:
                    break;
                case 250:
                    break;
                case 270:
                    break;
                case 315:
                    chance = r.Next(0, 100);
                    if(chance < 20)
                    {
                        eleMod = 1;
                    }
                    else if(chance < 40)
                    {
                        eleMod = 2;
                    }
                    else if (chance < 70)
                    {
                        eleMod = 3;
                    }
                    else if (chance < 95)
                    {
                        eleMod = 4;
                    }
                    else
                    {
                        eleMod = 5;
                    }
                    break;
                case 335:
                    chance = r.Next(0, 100);
                    if (chance < 20)
                    {
                        eleMod = 5;
                    }
                    else if (chance < 40)
                    {
                        eleMod = 6;
                    }
                    else if (chance < 70)
                    {
                        eleMod = 7;
                    }
                    else if (chance < 95)
                    {
                        eleMod = 8;
                    }
                    else
                    {
                        eleMod = 9;
                    }
                    break;
                case 360:
                    chance = r.Next(0, 100);
                    if (chance < 20)
                    {
                        eleMod = 10;
                    }
                    else if (chance < 30)
                    {
                        eleMod = 11;
                    }
                    else if (chance < 45)
                    {
                        eleMod =12;
                    }
                    else if (chance < 60)
                    {
                        eleMod = 13;
                    }
                    else if (chance < 75)
                    {
                        eleMod = 14;
                    }
                    else if (chance < 95)
                    {
                        eleMod = 15;
                    }
                    else
                    {
                        eleMod = 16;
                    }
                    break;
                case 375:
                    chance = r.Next(0, 100);
                    if (chance < 20)
                    {
                        eleMod = 12;
                    }
                    else if (chance < 30)
                    {
                        eleMod = 13;
                    }
                    else if (chance < 45)
                    {
                        eleMod = 14;
                    }
                    else if (chance < 60)
                    {
                        eleMod = 15;
                    }
                    else if (chance < 75)
                    {
                        eleMod = 16;
                    }
                    else if (chance < 95)
                    {
                        eleMod = 17;
                    }
                    else
                    {
                        eleMod = 18;
                    }
                    break;
                case 385:
                    chance = r.Next(0, 100);
                    if (chance < 20)
                    {
                        eleMod = 16;
                    }
                    else if (chance < 30)
                    {
                        eleMod = 17;
                    }
                    else if (chance < 45)
                    {
                        eleMod = 18;
                    }
                    else if (chance < 60)
                    {
                        eleMod = 19;
                    }
                    else if (chance < 75)
                    {
                        eleMod = 20;
                    }
                    else if (chance < 95)
                    {
                        eleMod = 21;
                    }
                    else
                    {
                        eleMod = 22;
                    }
                    break;
            }
            return eleMod;
        }

        //The percentages for variances need to be fixed
        public static double GetVariance(int category, int type)
        {
            Random r = new Random();
            int chance = 0;
            double variance = 0;
            switch (category)
            {
                case 1:
                    //Heavy Weapons
                    chance = r.Next(0, 100);
                    switch (type)
                    {
                        case 1:
                            //Axe
                            if (chance < 10)
                            {
                                variance = .90;
                            }
                            if (chance < 30)
                            {
                                variance = .93;
                            }
                            if(chance < 70)
                            {
                                variance = .95;
                            }
                            if(chance < 90)
                            {
                                variance = .97;
                            }
                            else
                            {
                                variance = .99;
                            }
                            break;
                        case 2:
                            //Dagger
                            if (chance < 10)
                            {
                                variance = .47;
                            }
                            if (chance < 30)
                            {
                                variance = .50;
                            }
                            if (chance < 70)
                            {
                                variance = .53;
                            }
                            if (chance < 90)
                            {
                                variance = .57;
                            }
                            else
                            {
                                variance = .62;
                            }
                            break;
                        case 3:
                            //Dagger MultiStrike
                            if (chance < 10)
                            {
                                variance = .40;
                            }
                            if (chance < 30)
                            {
                                variance = .43;
                            }
                            if (chance < 70)
                            {
                                variance = .48;
                            }
                            if (chance < 90)
                            {
                                variance = .53;
                            }
                            else
                            {
                                variance = .58;
                            }
                            break;
                        case 4:
                            //Mace
                            if (chance < 10)
                            {
                                variance = .30;
                            }
                            if (chance < 30)
                            {
                                variance = .33;
                            }
                            if (chance < 70)
                            {
                                variance = .37;
                            }
                            if (chance < 90)
                            {
                                variance = .42;
                            }
                            else
                            {
                                variance = .46;
                            }
                            break;
                        case 5:
                            //Spear
                            if (chance < 10)
                            {
                                variance = .59;
                            }
                            if (chance < 30)
                            {
                                variance = .63;
                            }
                            if (chance < 70)
                            {
                                variance = .68;
                            }
                            if (chance < 90)
                            {
                                variance = .72;
                            }
                            else
                            {
                                variance = .75;
                            }
                            break;
                        case 6:
                            //Staff
                            if (chance < 10)
                            {
                                variance = .38;
                            }
                            if (chance < 30)
                            {
                                variance = .42;
                            }
                            if (chance < 70)
                            {
                                variance = .45;
                            }
                            if (chance < 90)
                            {
                                variance = .50;
                            }
                            else
                            {
                                variance = .52;
                            }
                            break;
                        case 7:
                            //Sword
                            if (chance < 10)
                            {
                                variance = .47;
                            }
                            if (chance < 30)
                            {
                                variance = .50;
                            }
                            if (chance < 70)
                            {
                                variance = .53;
                            }
                            if (chance < 90)
                            {
                                variance = .57;
                            }
                            else
                            {
                                variance = .62;
                            }
                            break;
                        case 8:
                            //Sword Multistrike
                            if (chance < 10)
                            {
                                variance = .40;
                            }
                            if (chance < 30)
                            {
                                variance = .43;
                            }
                            if (chance < 70)
                            {
                                variance = .48;
                            }
                            if (chance < 90)
                            {
                                variance = .53;
                            }
                            else
                            {
                                variance = .60;
                            }
                            break;
                        case 9:
                            //UA
                            if (chance < 10)
                            {
                                variance = .44;
                            }
                            if (chance < 30)
                            {
                                variance = .48;
                            }
                            if (chance < 70)
                            {
                                variance = .53;
                            }
                            if (chance < 90)
                            {
                                variance = .58;
                            }
                            else
                            {
                                variance = .60;
                            }
                            break;
                    }
                    break;
                case 2:
                    //Finesse/Light Weapons
                    chance = r.Next(0, 100);
                    switch (type)
                    {
                        case 1:
                            //Axe
                            if (chance < 10)
                            {
                                variance = .80;
                            }
                            if (chance < 30)
                            {
                                variance = .83;
                            }
                            if (chance < 70)
                            {
                                variance = .85;
                            }
                            if (chance < 90)
                            {
                                variance = .90;
                            }
                            else
                            {
                                variance = .95;
                            }
                            break;
                        case 2:
                            //Dagger
                            if (chance < 10)
                            {
                                variance = .42;
                            }
                            if (chance < 30)
                            {
                                variance = .47;
                            }
                            if (chance < 70)
                            {
                                variance = .52;
                            }
                            if (chance < 90)
                            {
                                variance = .56;
                            }
                            else
                            {
                                variance = .60;
                            }
                            break;
                        case 3:
                            //Dagger MultiStrike
                            if (chance < 10)
                            {
                                variance = .24;
                            }
                            if (chance < 30)
                            {
                                variance = .28;
                            }
                            if (chance < 70)
                            {
                                variance = .35;
                            }
                            if (chance < 90)
                            {
                                variance = .40;
                            }
                            else
                            {
                                variance = .45;
                            }
                            break;
                        case 4:
                            //Mace
                            if (chance < 10)
                            {
                                variance = .23;
                            }
                            if (chance < 30)
                            {
                                variance = .28;
                            }
                            if (chance < 70)
                            {
                                variance = .32;
                            }
                            if (chance < 90)
                            {
                                variance = .37;
                            }
                            else
                            {
                                variance = .43;
                            }
                            break;
                        case 5:
                            //Jitte
                            if (chance < 10)
                            {
                                variance = .325;
                            }
                            if (chance < 30)
                            {
                                variance = .35;
                            }
                            if (chance < 70)
                            {
                                variance = .40;
                            }
                            if (chance < 90)
                            {
                                variance = .45;
                            }
                            else
                            {
                                variance = .50;
                            }
                            break;
                        case 6:
                            //Spear
                            if (chance < 10)
                            {
                                variance = .65;
                            }
                            if (chance < 30)
                            {
                                variance = .68;
                            }
                            if (chance < 70)
                            {
                                variance = .71;
                            }
                            if (chance < 90)
                            {
                                variance = .75;
                            }
                            else
                            {
                                variance = .80;
                            }
                            break;
                        case 7:
                            //Staff
                            if (chance < 10)
                            {
                                variance = .325;
                            }
                            if (chance < 30)
                            {
                                variance = .35;
                            }
                            if (chance < 70)
                            {
                                variance = .40;
                            }
                            if (chance < 90)
                            {
                                variance = .45;
                            }
                            else
                            {
                                variance = .50;
                            }
                            break;
                        case 8:
                            //Sword
                            if (chance < 10)
                            {
                                variance = .42;
                            }
                            if (chance < 30)
                            {
                                variance = .47;
                            }
                            if (chance < 70)
                            {
                                variance = .52;
                            }
                            if (chance < 90)
                            {
                                variance = .56;
                            }
                            else
                            {
                                variance = .60;
                            }
                            break;
                        case 9:
                            //Sword Multistrike
                            if (chance < 10)
                            {
                                variance = .24;
                            }
                            if (chance < 30)
                            {
                                variance = .28;
                            }
                            if (chance < 70)
                            {
                                variance = .35;
                            }
                            if (chance < 90)
                            {
                                variance = .40;
                            }
                            else
                            {
                                variance = .45;
                            }
                            break;
                        case 10:
                            //UA
                            if (chance < 10)
                            {
                                variance = .44;
                            }
                            if (chance < 30)
                            {
                                variance = .48;
                            }
                            if (chance < 70)
                            {
                                variance = .53;
                            }
                            if (chance < 90)
                            {
                                variance = .58;
                            }
                            else
                            {
                                variance = .60;
                            }
                            break;
                    }
                    break;
                case 3:
                    ///Two Handed, all have type 1, since there is only 1 set of variances
                    chance = r.Next(0, 100);
                    switch (type)
                    {
                        case 1:
                            //Axe
                            if (chance < 5)
                            {
                                variance = .30;
                            }
                            if (chance < 20)
                            {
                                variance = .35;
                            }
                            if (chance < 50)
                            {
                                variance = .40;
                            }
                            if (chance < 80)
                            {
                                variance = .45;
                            }
                            if (chance < 95)
                            {
                                variance = .50;
                            }
                            else
                            {
                                variance = .55;
                            }
                            break;
                    }
                    break;
                default:
                    break;
            }
            return variance;
        }

        public static int GetMaxDamage(int weaponType, int tier, int baseWeapon)
        {
            ///weaponType: 1 Heavy, 2 Finesse/Light, 3 two-handed
            ///baseWeapon: 1 Axe, 2 Dagger, 3 DaggerMulti, 4 Mace, 5 Spear, 6 Sword, 7 SwordMulti, 8 Staff, 9 UA
            int wieldPlace = 0;
            if(wieldPlace == 250)
            {
                wieldPlace = 1;
            }
            if (wieldPlace == 300)
            {
                wieldPlace = 2;
            }
            if (wieldPlace == 325)
            {
                wieldPlace = 3;
            }
            if (wieldPlace == 350)
            {
                wieldPlace = 4;
            }
            if (wieldPlace == 370)
            {
                wieldPlace = 5;
            }
            if (wieldPlace == 400)
            {
                wieldPlace = 6;
            }
            if (wieldPlace == 420)
            {
                wieldPlace = 7;
            }
            if (wieldPlace == 430)
            {
                wieldPlace = 8;
            }
            int[,] lightWeaponDamageTable =
            {
                { 22, 28, 33, 39, 44, 50, 55, 57, 61},
                { 18, 24, 29, 35, 40, 46, 51, 54, 58},
                {  7, 10, 13, 16, 18, 21, 24, 27, 28},
                { 19, 24, 29, 35, 40, 45, 50, 52, 57},
                { 21, 26, 32, 37, 42, 48, 53, 56, 60},
                { 20, 25, 31, 36, 41, 47, 52, 55, 58},
                {  7, 10, 13, 16, 18, 21, 24, 25, 28},
                { 19, 24, 30, 35, 40, 46, 51, 54, 57},
                { 17, 22, 26, 31, 35, 40, 44, 46, 48}
            };
            int[,] heavyWeaponDamageTable =
            {
                { 26, 33, 40, 47, 54, 61, 68, 71, 74 },
                { 24, 31, 38, 45, 51, 58, 65, 68, 71 },
                { 13, 16, 20, 23, 26, 30, 33, 36, 38 },
                { 22, 29, 36, 43, 49, 56, 63, 66, 69 },
                { 25, 32, 39, 46, 52, 59, 66, 69, 72 },
                { 24, 31, 38, 45, 51, 58, 65, 68, 71 },
                { 12, 16, 19, 23, 26, 30, 33, 36, 38 },
                { 23, 30, 36, 43, 50, 56, 63, 66, 70 },
                { 20, 26, 31, 37, 43, 48, 54, 56, 59 }
            };
            int[,] twohandedWeaponDamageTable =
            {
                { 13, 17, 22, 26, 30, 35, 39, 42, 45 },
                { 14, 19, 23, 28, 33, 37, 42, 45, 48 }
            };
            int tieredDamage = 0;
            switch(weaponType)
            {
                case 1:
                    tieredDamage = heavyWeaponDamageTable[baseWeapon - 1, wieldPlace];
                    break;
                case 2:
                    tieredDamage = lightWeaponDamageTable[baseWeapon - 1, wieldPlace];
                    break;
                case 3:
                    tieredDamage = twohandedWeaponDamageTable[baseWeapon - 1, wieldPlace];
                    break;
            }
            return tieredDamage;
        }

        public static int GetLowSpellTier(int tier)
        {
            int lowSpellTier = 0;
            switch (tier)
            {
                case 1:
                    lowSpellTier = 1;
                    break;
                case 2:
                    lowSpellTier = 3;
                    break;
                case 3:
                    lowSpellTier = 4;
                    break;
                case 4:
                    lowSpellTier = 5;
                    break;
                case 5:
                    lowSpellTier = 5;
                    break;
                case 6:
                    lowSpellTier = 6;
                    break;
                case 7:
                    lowSpellTier = 6;
                    break;
                case 8:
                    lowSpellTier = 6;
                    break;
                default:
                    break;
            }
            return lowSpellTier;
        }

        public static int GetHighSpellTier(int tier)
        {
            int highSpellTier = 0;
            switch (tier)
            {
                case 1:
                    highSpellTier = 3;
                    break;
                case 2:
                    highSpellTier = 5;
                    break;
                case 3:
                    highSpellTier = 6;
                    break;
                case 4:
                    highSpellTier = 6;
                    break;
                case 5:
                    highSpellTier = 7;
                    break;
                case 6:
                    highSpellTier = 7;
                    break;
                case 7:
                    highSpellTier = 8;
                    break;
                case 8:
                    highSpellTier = 8;
                    break;
                default:
                    break;
            }
            return highSpellTier;
        }

        public static int GetSkillLevelLimit(int wield)
        {
            Random r = new Random();
            double percentage = (double)r.Next(75, 99);
            int skill = (int)(percentage * (double)wield);
            return skill;
         }

        public static double GetManaCMod(int tier)
        {
            Random r = new Random();
            int magicMod = 0;
            int chance = 0;
            switch (tier)
            {
                case 1:
                    //tier 1
                    magicMod = 0;
                    break;
                case 2:
                    magicMod = 0;
                    break;
                case 3:
                    chance = r.Next(0, 1000);
                    if (chance > 900)
                    {
                        magicMod = 5;
                    }
                    else if (chance > 800)
                    {
                        magicMod = 4;
                    }
                    else if (chance > 700)
                    {
                        magicMod = 3;
                    }
                    else if (chance > 600)
                    {
                        magicMod = 2;
                    }
                    else if (chance > 500)
                    {
                        magicMod = 1;
                    }
                    break;
                case 4:
                    chance = r.Next(0, 1000);
                    if (chance > 900)
                    {
                        magicMod = 10;
                    }
                    else if (chance > 800)
                    {
                        magicMod = 9;
                    }
                    else if (chance > 700)
                    {
                        magicMod = 8;
                    }
                    else if (chance > 600)
                    {
                        magicMod = 7;
                    }
                    else if (chance > 500)
                    {
                        magicMod = 6;
                    }
                    else
                    {
                        magicMod = 5;
                    }
                    break;
                case 5:
                    chance = r.Next(0, 1000);
                    if (chance > 900)
                    {
                        magicMod = 10;
                    }
                    else if (chance > 800)
                    {
                        magicMod = 9;
                    }
                    else if (chance > 700)
                    {
                        magicMod = 8;
                    }
                    else if (chance > 600)
                    {
                        magicMod = 7;
                    }
                    else if (chance > 500)
                    {
                        magicMod = 6;
                    }
                    else
                    {
                        magicMod = 5;
                    }
                    break;
                case 6:
                    chance = r.Next(0, 1000);
                    if (chance > 900)
                    {
                        magicMod = 10;
                    }
                    else if (chance > 800)
                    {
                        magicMod = 9;
                    }
                    else if (chance > 700)
                    {
                        magicMod = 8;
                    }
                    else if (chance > 600)
                    {
                        magicMod = 7;
                    }
                    else if (chance > 500)
                    {
                        magicMod = 6;
                    }
                    else
                    {
                        magicMod = 5;
                    }
                    break;
                case 7:
                    chance = r.Next(0, 1000);
                    if (chance > 900)
                    {
                        magicMod = 10;
                    }
                    else if (chance > 800)
                    {
                        magicMod = 9;
                    }
                    else if (chance > 700)
                    {
                        magicMod = 8;
                    }
                    else if (chance > 600)
                    {
                        magicMod = 7;
                    }
                    else if (chance > 500)
                    {
                        magicMod = 6;
                    }
                    else
                    {
                        magicMod = 5;
                    }
                    break;
                case 8:
                    chance = r.Next(0, 1000);
                    if(chance > 900)
                    {
                        magicMod = 10;
                    }
                    else if (chance > 800)
                    {
                        magicMod = 9;
                    }
                    else if (chance > 700)
                    {
                        magicMod = 8;
                    }
                    else if (chance > 600)
                    {
                        magicMod = 7;
                    }
                    else if (chance > 500)
                    {
                        magicMod = 6;
                    }
                    else
                    {
                        magicMod = 5;
                    }
                    break;
                default:
                    break;
            }
            double manaDMod = magicMod / 100.0;
            return manaDMod;
        }

        public static double GetMissileDMod(int tier)
        {
            Random r = new Random();
            double missileMod = 0;
            switch (tier)
            {
                case 1:
                    //tier 1
                    missileMod = 0;
                    break;
                case 2:
                    missileMod = 0;
                    break;
                case 3:
                    int chance = r.Next(0, 100);
                    if (chance > 95)
                    {
                        missileMod = .005;
                    }
                    break;
                case 4:
                    chance = r.Next(0, 100);
                    if (chance > 95)
                    {
                        missileMod =.01;
                    }
                    else if (chance > 80)
                    {
                        missileMod = .005;
                    }
                    else
                    {
                        missileMod = 0;
                    }
                    break;
                case 5:
                    chance = r.Next(0, 1000);
                    if (chance > 950)
                    {
                        missileMod = .01;
                    }
                    else if (chance > 800)
                    {
                        missileMod = .005;
                    }
                    else
                    {
                        missileMod = 0;
                    }
                    break;
                case 6:
                    chance = r.Next(0, 1000);
                    if (chance > 975)
                    {
                        missileMod = .020;
                    }
                    else if (chance > 900)
                    {
                        missileMod = .015;
                    }
                    else if (chance > 800)
                    {
                        missileMod = .010;
                    }
                    else if (chance > 700)
                    {
                        missileMod = .005;
                    }
                    else
                    {
                        missileMod = 0;
                    }
                    break;
                case 7:
                    chance = r.Next(0, 1000);
                    if (chance > 990)
                    {
                        missileMod = .030;
                    }
                    else if (chance > 985)
                    {
                        missileMod = .025;
                    }
                    else if (chance > 950)
                    {
                        missileMod = .020;
                    }
                    else if (chance > 900)
                    {
                        missileMod = .015;
                    }
                    else if (chance > 850)
                    {
                        missileMod = .01;
                    }
                    else if (chance > 800)
                    {
                        missileMod = .005;
                    }
                    else
                    {
                        missileMod = 0;
                    }
                    break;
                case 8:
                    chance = r.Next(0, 1000);
                    if (chance > 998)
                    {
                        missileMod = .04;
                    }
                    else if (chance > 994)
                    {
                        missileMod = .035;
                    }
                    else if (chance > 990)
                    {
                        missileMod = .03;
                    }
                    else if (chance > 985)
                    {
                        missileMod = .025;
                    }
                    else if (chance > 950)
                    {
                        missileMod = .02;
                    }
                    else if (chance > 900)
                    {
                        missileMod = .015;
                    }
                    else if (chance > 850)
                    {
                        missileMod = .01;
                    }
                    else if (chance > 800)
                    {
                        missileMod = .005;
                    }
                    else
                    {
                        missileMod = 0;
                    }
                    break;
                default:
                    break;
            }
            double m2 = 1.0 + missileMod;
            return m2;
        }

        public static int GetValue(int tier)
        {
            ///This is just a placeholder. This doesnt return a final value used retail, just a quick vaue for now.
            ///Will use, tier, material type, amount of gems set into item, type of gems, spells on item
            Random r = new Random();
            int value = 0;
            switch (tier)
            {
                case 1:
                    //tier 1
                    value = (int)(tier * r.Next(1, 20) + r.Next(1, 50));
                    break;
                case 2:
                    value = (int)(tier * r.Next(1, 40) + r.Next(1, 50));
                    break;
                case 3:
                    value = (int)(tier * r.Next(1, 60) + r.Next(1, 50));
                    break;
                case 4:
                    value = (int)(tier * r.Next(1, 80) + r.Next(1, 50));
                    break;
                case 5:
                    value = (int)(tier * r.Next(1, 100) + r.Next(1, 50));
                    break;
                case 6:
                    value = (int)(tier * r.Next(1, 120) + r.Next(1, 50));
                    break;
                case 7:
                    value = (int)(tier * r.Next(1, 140) + r.Next(1, 50));
                    break;
                case 8:
                    value = (int)(tier * r.Next(1, 160) + r.Next(1, 50));
                    break;
                default:
                    break;
            }
            return value;
        }

        public static int GetWorkmanship(int tier)
        {
            Random r = new Random();
            int chance = 0;
            int workmanship = 0;
            switch (tier)
            {
                case 1:
                    //tier 1
                    chance = r.Next(0, 100);
                    if (chance < 50)
                    {
                        workmanship = 1;
                    }
                    else if (chance < 80)
                    {
                        workmanship = 2;
                    }
                    else
                    {
                        workmanship = 3;
                    }
                    break;
                case 2:
                    //tier 2
                    chance = r.Next(0, 100);
                    if (chance < 30)
                    {
                        workmanship = 2;
                    }
                    else if (chance < 50)
                    {
                        workmanship = 3;
                    }
                    else if (chance < 65)
                    {
                        workmanship = 4;
                    }
                    else if (chance < 85)
                    {
                        workmanship = 5;
                    }
                    else
                    {
                        workmanship = 6;
                    }
                    break;
                case 3:
                    //tier 3
                    chance = r.Next(0, 100);
                    if (chance < 30)
                    {
                        workmanship = 3;
                    }
                    else if (chance < 50)
                    {
                        workmanship = 4;
                    }
                    else if (chance < 65)
                    {
                        workmanship = 5;
                    }
                    else if (chance < 85)
                    {
                        workmanship = 6;
                    }
                    else
                    {
                        workmanship = 7;
                    }
                    break;
                case 4:
                    //tier 4
                    chance = r.Next(0, 100);
                    if (chance < 30)
                    {
                        workmanship = 3;
                    }
                    else if (chance < 50)
                    {
                        workmanship = 4;
                    }
                    else if (chance < 65)
                    {
                        workmanship = 5;
                    }
                    else if (chance < 80)
                    {
                        workmanship = 6;
                    }
                    else if (chance < 92)
                    {
                        workmanship = 7;
                    }
                    else
                    {
                        workmanship = 8;
                    }
                    break;
                case 5:
                    //tier 5
                    chance = r.Next(0, 100);
                    if (chance < 15)
                    {
                        workmanship = 3;
                    }
                    else if (chance < 30)
                    {
                        workmanship = 4;
                    }
                    else if (chance < 50)
                    {
                        workmanship = 5;
                    }
                    else if (chance < 65)
                    {
                        workmanship = 6;
                    }
                    else if (chance < 80)
                    {
                        workmanship = 7;
                    }
                    else if (chance < 92)
                    {
                        workmanship = 8;
                    }
                    else
                    {
                        workmanship = 9;
                    }
                    break;
                case 6:
                    //tier 6
                    chance = r.Next(0, 100);
                    if (chance < 15)
                    {
                        workmanship = 4;
                    }
                    else if (chance < 30)
                    {
                        workmanship = 5;
                    }
                    else if (chance < 50)
                    {
                        workmanship = 6;
                    }
                    else if (chance < 65)
                    {
                        workmanship = 7;
                    }
                    else if (chance < 80)
                    {
                        workmanship = 8;
                    }
                    else if (chance < 92)
                    {
                        workmanship = 9;
                    }
                    else
                    {
                        workmanship = 10;
                    }
                    break;
                case 7:
                    //tier 7
                    chance = r.Next(0, 100);
                    if (chance < 15)
                    {
                        workmanship = 4;
                    }
                    else if (chance < 30)
                    {
                        workmanship = 5;
                    }
                    else if (chance < 50)
                    {
                        workmanship = 6;
                    }
                    else if (chance < 65)
                    {
                        workmanship = 7;
                    }
                    else if (chance < 80)
                    {
                        workmanship = 8;
                    }
                    else if (chance < 92)
                    {
                        workmanship = 9;
                    }
                    else
                    {
                        workmanship = 10;
                    }
                    break;
                case 8:
                    //tier 8
                    chance = r.Next(0, 100);
                    if (chance < 15)
                    {
                        workmanship = 4;
                    }
                    else if (chance < 30)
                    {
                        workmanship = 5;
                    }
                    else if (chance < 50)
                    {
                        workmanship = 6;
                    }
                    else if (chance < 65)
                    {
                        workmanship = 7;
                    }
                    else if (chance < 80)
                    {
                        workmanship = 8;
                    }
                    else if (chance < 92)
                    {
                        workmanship = 9;
                    }
                    else
                    {
                        workmanship = 10;
                    }
                    break;
                default:
                    break;

            }
            return workmanship;
        }

        public static int GetSpellcraft(int spellAmount, int tier)
        {
            Random r = new Random();
            int spellcraft = 0;
            switch (tier)
            {
                case 1:
                    spellcraft = r.Next(1, 20) + spellAmount * r.Next(1, 4); //1-50
                    break;
                case 2:
                    spellcraft = r.Next(40, 70) + spellAmount * r.Next(1, 5); //40-90
                    break;
                case 3:
                    spellcraft = r.Next(70, 90) + spellAmount * r.Next(1, 6); //80 - 130
                    break;
                case 4:
                    spellcraft = r.Next(100, 120) + spellAmount * r.Next(1, 7); /// 120 - 160
                    break;
                case 5:
                    spellcraft = r.Next(130, 150) + spellAmount * r.Next(1, 8); ///150 - 210
                    break;
                case 6:
                    spellcraft = r.Next(160, 180) + spellAmount * r.Next(1, 9); /// 200-260
                    break;
                case 7:
                    spellcraft = r.Next(230, 260) + spellAmount * r.Next(1, 10); /// 250 - 310
                    break;
                case 8:
                    spellcraft = r.Next(280, 300) + spellAmount * r.Next(1, 11); //300-450
                    break;
                default:
                    break;
            }

                    return spellcraft;
        }

        public static int GetDifficulty(int tier, int spellcraft)
        {
            Random r = new Random();
            int difficulty = 0;
            switch (tier)
            {
                case 1:
                    difficulty = spellcraft + (r.Next(0, 10) * r.Next(1, 3));
                    break;
                case 2:
                    difficulty = spellcraft + (r.Next(0, 10) * r.Next(1, 3));
                    break;
                case 3:
                    difficulty = spellcraft + (r.Next(0, 10) * r.Next(1, 3));
                    break;
                case 4:
                    difficulty = spellcraft + (r.Next(0, 10) * r.Next(1, 3));
                    break;
                case 5:
                    difficulty = spellcraft + (r.Next(0, 10) * r.Next(1, 3));
                    break;
                case 6:
                    difficulty = spellcraft + (r.Next(0, 10) * r.Next(1, 3));
                    break;
                case 7:
                    difficulty = spellcraft + (r.Next(0, 10) * r.Next(1, 3));
                    break;
                case 8:
                    difficulty = spellcraft + (r.Next(0, 10) * r.Next(1, 3));
                    break;
                default:
                    break;
            }
            return difficulty;
        }

        public static int GetMaxMana(int spellAmount, int tier)
        {
            Random r = new Random();
            int maxmana = 0;
            switch (tier)
            {
                case 1:
                    maxmana = (r.Next(100, 500) + spellAmount * r.Next(1, 4)) * r.Next(3, 9); //1-50
                    break;
                case 2:
                    maxmana = (r.Next(400,700) + spellAmount * r.Next(1, 5)) * r.Next(3, 9); //40-90
                    break;
                case 3:
                    maxmana = (r.Next(700,900) + spellAmount * r.Next(1, 6)) *r.Next(3, 9); //80 - 130
                    break;
                case 4:
                    maxmana = (r.Next(1000,1200) + spellAmount * r.Next(1, 7)) *r.Next(3, 9); /// 120 - 160
                    break;
                case 5:
                    maxmana = (r.Next(1300, 1500) + spellAmount * r.Next(1, 8)) *r.Next(3, 9); ///150 - 210
                    break;
                case 6:
                    maxmana = (r.Next(1600, 1800) + spellAmount * r.Next(1, 9)) *r.Next(3, 9); /// 200-260
                    break;
                case 7:
                    maxmana = (r.Next(2300, 2600) + spellAmount * r.Next(1, 10)) *r.Next(3, 9); /// 250 - 310
                    break;
                case 8:
                    maxmana = (r.Next(2800, 3000) + spellAmount * r.Next(1, 11)) *r.Next(3, 9); //300-450
                    break;
                default:
                    break;
            }
            return maxmana;
        }

        public static int GetMaterialType(int type, int tier)
        {
            uint materialType = 0;
            Random r = new Random();
            ///Type = 1 for gems, 2 for weapons/bows, 3 for armor, 4 jewelry
            uint[] gems5 ={ 0x00000020, 0x0000002A, 0x0000002C, 0x0000002E, 0x0000000B, 0x0000001F, 0x0000000A, 0x0000000E, 0x00000011, 0x00000012, 0x00000013, 0x00000019,
                            0x0000001C, 0x0000001D, 0x0000001E, 0x00000024, 0x00000025, 0x00000028, 0x0000000C, 0x00000018, 0x0000002B, 0x0000002D, 0x00000030, 0x00000032,
                            0x0000000F, 0x0000001B, 0x00000023, 0x00000031, 0x0000000D, 0x00000017, 0x00000021, 0x0000001A, 0x0000002F, 0x00000010, 0x00000016, 0x00000029,
                            0x00000014, 0x00000026, 0x00000027, 0x00000015 };

            uint[] gems4 ={ 0x00000020, 0x0000002A, 0x0000002C, 0x0000002E, 0x0000000B, 0x0000001F, 0x0000000A, 0x0000000E, 0x00000011, 0x00000012, 0x00000013, 0x00000019,
                            0x0000001C, 0x0000001D, 0x0000001E, 0x00000024, 0x00000025, 0x00000028, 0x0000000C, 0x00000018, 0x0000002B, 0x0000002D, 0x00000030, 0x00000032,
                            0x0000000F, 0x0000001B, 0x00000023, 0x00000031, 0x0000000D, 0x00000017, 0x00000021, 0x0000001A, 0x0000002F, 0x00000010, 0x00000016, 0x00000029};

            uint[] gems3 ={ 0x00000020, 0x0000002A, 0x0000002C, 0x0000002E, 0x0000000B, 0x0000001F, 0x0000000A, 0x0000000E, 0x00000011, 0x00000012, 0x00000013, 0x00000019,
                            0x0000001C, 0x0000001D, 0x0000001E, 0x00000024, 0x00000025, 0x00000028, 0x0000000C, 0x00000018, 0x0000002B, 0x0000002D, 0x00000030, 0x00000032,
                            0x0000000F, 0x0000001B, 0x00000023, 0x00000031, 0x0000000D, 0x00000017, 0x00000021};

            uint[] gems2 ={ 0x00000020, 0x0000002A, 0x0000002C, 0x0000002E, 0x0000000B, 0x0000001F, 0x0000000A, 0x0000000E, 0x00000011, 0x00000012, 0x00000013, 0x00000019,
                            0x0000001C, 0x0000001D, 0x0000001E, 0x00000024, 0x00000025, 0x00000028, 0x0000000C, 0x00000018, 0x0000002B, 0x0000002D, 0x00000030, 0x00000032,
                            0x0000000F, 0x0000001B, 0x00000023};

            uint[] gems1 ={ 0x00000020, 0x0000002A, 0x0000002C, 0x0000002E, 0x0000000B, 0x0000001F, 0x0000000A, 0x0000000E, 0x00000011, 0x00000012, 0x00000013, 0x00000019,
                            0x0000001C, 0x0000001D, 0x0000001E, 0x00000024, 0x00000025, 0x00000028};

            uint[] materialTypes5 ={ 0x00000020, 0x0000002A, 0x0000002C, 0x0000002E, 0x0000000B, 0x0000001F, 0x0000000A, 0x0000000E, 0x00000011, 0x00000012, 0x00000013, 0x00000019,
                                     0x0000001C, 0x0000001D, 0x0000001E, 0x00000024, 0x00000025, 0x00000028, 0x0000000C, 0x00000018, 0x0000002B, 0x0000002D, 0x00000030, 0x00000032,
                                     0x0000000F, 0x0000001B, 0x00000023, 0x00000031, 0x0000000D, 0x00000017, 0x00000021, 0x0000001A, 0x0000002F, 0x00000010, 0x00000016, 0x00000029,
                                     0x00000014, 0x00000026, 0x00000027, 0x00000015, 0x00000033, 0x00000039, 0x0000003A, 0x0000003D, 0x00000040, 0x0000003E, 0x0000003C, 0x0000003F};

            uint[] materialTypes4 = { 0x00000020, 0x0000002A, 0x0000002C, 0x0000002E, 0x0000000B, 0x0000001F, 0x0000000A, 0x0000000E, 0x00000011, 0x00000012, 0x00000013, 0x00000019,
                                      0x0000001C, 0x0000001D, 0x0000001E, 0x00000024, 0x00000025, 0x00000028, 0x0000000C, 0x00000018, 0x0000002B, 0x0000002D, 0x00000030, 0x00000032,
                                      0x0000000F, 0x0000001B, 0x00000023, 0x00000031, 0x0000000D, 0x00000017, 0x00000021, 0x0000001A, 0x0000002F, 0x00000010, 0x00000016, 0x00000029,
                                      0x00000033, 0x00000039, 0x0000003A, 0x0000003D, 0x00000040, 0x0000003E, 0x0000003C, 0x0000003F};

            uint[] materialTypes3 ={ 0x00000020, 0x0000002A, 0x0000002C, 0x0000002E, 0x0000000B, 0x0000001F, 0x0000000A, 0x0000000E, 0x00000011, 0x00000012, 0x00000013, 0x00000019,
                                     0x0000001C, 0x0000001D, 0x0000001E, 0x00000024, 0x00000025, 0x00000028, 0x0000000C, 0x00000018, 0x0000002B, 0x0000002D, 0x00000030, 0x00000032,
                                     0x0000000F, 0x0000001B, 0x00000023, 0x00000031, 0x0000000D, 0x00000017, 0x00000021, 0x00000033, 0x00000039, 0x0000003A, 0x0000003D, 0x00000040,
                                     0x0000003C, 0x0000003F};

            uint[] materialTypes2 ={ 0x00000020, 0x0000002A, 0x0000002C, 0x0000002E, 0x0000000B, 0x0000001F, 0x0000000A, 0x0000000E, 0x00000011, 0x00000012, 0x00000013, 0x00000019,
                                     0x0000001C, 0x0000001D, 0x0000001E, 0x00000024, 0x00000025, 0x00000028, 0x0000000C, 0x00000018, 0x0000002B, 0x0000002D, 0x00000030, 0x00000032,
                                     0x0000000F, 0x0000001B, 0x00000023, 0x00000033, 0x00000039, 0x0000003A, 0x0000003D, 0x00000040, 0x0000003C, 0x0000003F};

            uint[] materialTypes1 ={ 0x00000020, 0x0000002A, 0x0000002C, 0x0000002E, 0x0000000B, 0x0000001F, 0x0000000A, 0x0000000E, 0x00000011, 0x00000012, 0x00000013, 0x00000019,
                                     0x0000001C, 0x0000001D, 0x0000001E, 0x00000024, 0x00000025, 0x00000028, 0x00000033, 0x00000039, 0x0000003A, 0x0000003D, 0x00000040, 0x0000003C,
                                     0x0000003F };

            uint[] bowTypes5 ={ 0x00000020, 0x0000002A, 0x0000002C, 0x0000002E, 0x0000000B, 0x0000001F, 0x0000000A, 0x0000000E, 0x00000011, 0x00000012, 0x00000013, 0x00000019,
                                     0x0000001C, 0x0000001D, 0x0000001E, 0x00000024, 0x00000025, 0x00000028, 0x0000000C, 0x00000018, 0x0000002B, 0x0000002D, 0x00000030, 0x00000032,
                                     0x0000000F, 0x0000001B, 0x00000023, 0x00000031, 0x0000000D, 0x00000017, 0x00000021, 0x0000001A, 0x0000002F, 0x00000010, 0x00000016, 0x00000029,
                                     0x00000014, 0x00000026, 0x00000027, 0x00000015, 0x00000033, 0x00000039, 0x0000003A, 0x0000003D, 0x00000040, 0x0000003E, 0x0000003C, 0x0000003F,
                                     0x0000004A, 0x00000049, 0x0000004B, 0x0000004C, 0x0000004D};

            uint[] bowTypes4 = { 0x00000020, 0x0000002A, 0x0000002C, 0x0000002E, 0x0000000B, 0x0000001F, 0x0000000A, 0x0000000E, 0x00000011, 0x00000012, 0x00000013, 0x00000019,
                                      0x0000001C, 0x0000001D, 0x0000001E, 0x00000024, 0x00000025, 0x00000028, 0x0000000C, 0x00000018, 0x0000002B, 0x0000002D, 0x00000030, 0x00000032,
                                      0x0000000F, 0x0000001B, 0x00000023, 0x00000031, 0x0000000D, 0x00000017, 0x00000021, 0x0000001A, 0x0000002F, 0x00000010, 0x00000016, 0x00000029,
                                      0x00000033, 0x00000039, 0x0000003A, 0x0000003D, 0x00000040, 0x0000003E, 0x0000003C, 0x0000003F, 0x0000004A, 0x00000049, 0x0000004B, 0x0000004C, 0x0000004D};

            uint[] bowTypes3 ={ 0x00000020, 0x0000002A, 0x0000002C, 0x0000002E, 0x0000000B, 0x0000001F, 0x0000000A, 0x0000000E, 0x00000011, 0x00000012, 0x00000013, 0x00000019,
                                     0x0000001C, 0x0000001D, 0x0000001E, 0x00000024, 0x00000025, 0x00000028, 0x0000000C, 0x00000018, 0x0000002B, 0x0000002D, 0x00000030, 0x00000032,
                                     0x0000000F, 0x0000001B, 0x00000023, 0x00000031, 0x0000000D, 0x00000017, 0x00000021, 0x00000033, 0x00000039, 0x0000003A, 0x0000003D, 0x00000040,
                                     0x0000003C, 0x0000003F, 0x0000004A, 0x00000049, 0x0000004B, 0x0000004C, 0x0000004D};

            uint[] bowTypes2 ={ 0x00000020, 0x0000002A, 0x0000002C, 0x0000002E, 0x0000000B, 0x0000001F, 0x0000000A, 0x0000000E, 0x00000011, 0x00000012, 0x00000013, 0x00000019,
                                     0x0000001C, 0x0000001D, 0x0000001E, 0x00000024, 0x00000025, 0x00000028, 0x0000000C, 0x00000018, 0x0000002B, 0x0000002D, 0x00000030, 0x00000032,
                                     0x0000000F, 0x0000001B, 0x00000023, 0x00000033, 0x00000039, 0x0000003A, 0x0000003D, 0x00000040, 0x0000003C, 0x0000003F, 0x0000004A, 0x00000049, 0x0000004B, 0x0000004C, 0x0000004D};

            uint[] bowTypes1 ={ 0x00000020, 0x0000002A, 0x0000002C, 0x0000002E, 0x0000000B, 0x0000001F, 0x0000000A, 0x0000000E, 0x00000011, 0x00000012, 0x00000013, 0x00000019,
                                     0x0000001C, 0x0000001D, 0x0000001E, 0x00000024, 0x00000025, 0x00000028, 0x00000033, 0x00000039, 0x0000003A, 0x0000003D, 0x00000040, 0x0000003C,
                                     0x0000003F, 0x0000004A, 0x00000049, 0x0000004B, 0x0000004C, 0x0000004D, };
            uint[] metalWeaponsAll = { 0x0000003B, 0x00000039, 0x0000003A, 0x0000003D, 0x00000040, 0x0000003C, 0x0000003F, 0x0000003E };

            uint[] leatherArmor = { 52, 53, 54, 55};

            uint[] clothArmor = {4, 5, 6, 7, 8 };

            uint[] metalArmor = { 0x0000003B, 0x00000039, 0x0000003A, 0x0000003D, 0x00000040, 0x0000003C, 0x0000003F, 0x0000003E };

            uint[] metalAndLeatherArmor = { 52, 53, 54, 55, 0x0000003B, 0x00000039, 0x0000003A, 0x0000003D, 0x00000040, 0x0000003C, 0x0000003F, 0x0000003E }; 

            ///Type = 1 for gems, 2 for weapons/bows, 3 for armor, 4 MetalWeapons, 5 jewelry, 
            switch (type)
            {
                case 1:
                    if(tier == 1)
                    {
                        materialType = gems1[r.Next(0, gems1.Length)];
                    }
                    if (tier == 2)
                    {
                        materialType = gems2[r.Next(0, gems2.Length)];
                    }
                    if (tier == 3)
                    {
                        materialType = gems3[r.Next(0, gems3.Length)];
                    }
                    if (tier == 4)
                    {
                        materialType = gems4[r.Next(0, gems4.Length)];
                    }
                    else
                    {
                        materialType = gems5[r.Next(0, gems5.Length)];
                    }
                    break;
                case 2:
                    if (tier == 1)
                    {
                        materialType = bowTypes1[r.Next(0, bowTypes1.Length)];
                    }
                    if (tier == 2)
                    {
                        materialType = bowTypes2[r.Next(0, bowTypes2.Length)];
                    }
                    if (tier == 3)
                    {
                        materialType = bowTypes3[r.Next(0, bowTypes3.Length)];
                    }
                    if (tier == 4)
                    {
                        materialType = bowTypes4[r.Next(0, bowTypes4.Length)];
                    }
                    else
                    {
                        materialType = bowTypes5[r.Next(0, bowTypes5.Length)];
                    }
                    break;
                case 3:
                    if (tier == 1)
                    {
                        materialType = materialTypes1[r.Next(0, materialTypes1.Length)];
                    }
                    if (tier == 2)
                    {
                        materialType = materialTypes2[r.Next(0, materialTypes2.Length)];
                    }
                    if (tier == 3)
                    {
                        materialType = materialTypes3[r.Next(0, materialTypes3.Length)];
                    }
                    if (tier == 4)
                    {
                        materialType = materialTypes4[r.Next(0, materialTypes4.Length)];
                    }
                    else
                    {
                        materialType = materialTypes5[r.Next(0, materialTypes5.Length)];
                    }
                    break;
                case 4:
                    materialType = metalWeaponsAll[r.Next(0, metalWeaponsAll.Length)];
                    break;
                case 5: materialType = leatherArmor[r.Next(0, leatherArmor.Length)];
                    break;
                case 6:
                    materialType = metalAndLeatherArmor[r.Next(0, metalAndLeatherArmor.Length)];
                    break;
                case 7:
                    materialType = metalArmor[r.Next(0, metalArmor.Length)];
                    break;
                case 8:
                    materialType = clothArmor[r.Next(0, clothArmor.Length)];
                    break;
                default:
                    break;

            }
            return (int) materialType;
        }

        public static double GetMeleeDMod(int maxMelee, int tier)
        {
            Random r = new Random();
            double meleeMod = 0;
            int chance = 0;
            switch (maxMelee)
            {
                case 20:
                    //tier 1
                    switch (tier)
                    {
                        case 1:
                            meleeMod = 0;
                            break;
                        case 2:
                            chance = r.Next(0, 100);
                            if (chance < 60)
                            {
                                meleeMod = 0;
                            }
                            else if (chance < 80)
                            {
                                meleeMod = .01;
                            }
                            else if (chance < 92)
                            {
                                meleeMod = .02;
                            }
                            else
                            {
                                meleeMod = .03;
                            }
                            break;
                        case 3:
                            chance = r.Next(0, 100);
                            if (chance < 60)
                            {
                                meleeMod = .03;
                            }
                            else if (chance < 80)
                            {
                                meleeMod = .04;
                            }
                            else if (chance < 92)
                            {
                                meleeMod = .05;
                            }
                            else
                            {
                                meleeMod = .06;
                            }
                            break;
                        case 4:
                            chance = r.Next(0, 100);
                            if (chance < 60)
                            {
                                meleeMod = .06;
                            }
                            else if (chance < 80)
                            {
                                meleeMod = .07;
                            }
                            else if (chance < 92)
                            {
                                meleeMod = .08;
                            }
                            else
                            {
                                meleeMod = .09;
                            }
                            break;
                        case 5:
                            chance = r.Next(0, 100);
                            if (chance < 60)
                            {
                                meleeMod = .09;
                            }
                            else if (chance < 80)
                            {
                                meleeMod = .1;
                            }
                            else if (chance < 92)
                            {
                                meleeMod = .11;
                            }
                            else
                            {
                                meleeMod = .12;
                            }
                            break;
                        case 6:
                            chance = r.Next(0, 100);
                            if (chance < 60)
                            {
                                meleeMod = .12;
                            }
                            else if (chance < 80)
                            {
                                meleeMod = .13;
                            }
                            else if (chance < 92)
                            {
                                meleeMod = .14;
                            }
                            else
                            {
                                meleeMod = .15;
                            }
                            break;
                        case 7:
                            chance = r.Next(0, 100);
                            if (chance < 60)
                            {
                                meleeMod = .15;
                            }
                            else if (chance < 80)
                            {
                                meleeMod = .16;
                            }
                            else if (chance < 92)
                            {
                                meleeMod = .17;
                            }
                            else
                            {
                                meleeMod = .18;
                            }
                            break;
                        case 8:
                            chance = r.Next(0, 100);
                            if (chance < 60)
                            {
                                meleeMod = .17;
                            }
                            else if (chance < 80)
                            {
                                meleeMod = .18;
                            }
                            else if (chance < 92)
                            {
                                meleeMod = .19;
                            }
                            else
                            {
                                meleeMod = .20;
                            }
                            break;
                    }
                    break;
            }
            meleeMod += 1.0;
            return meleeMod;
        }

        public static int GetNumLegendaryCantrips(int tier)
        {
            Random r = new Random();
            int amount = 0;
            switch(tier)
            {
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    break;
                case 7:
                    break;
                default:
                    if(r.Next(0,1000) == 0)
                    {
                        amount = 1;
                    }
                    if (r.Next(0, 5000) == 0)
                    {
                        amount = 2;
                    }
                    if (r.Next(0, 1000000) == 0)
                    {
                        amount = 1;
                    }
                    if (r.Next(0, 10000000) == 0)
                    {
                        amount = 1;
                    }
                    break;

            }
            return amount;
        }

        public static int GetNumEpicCantrips(int tier)
        {
            Random r = new Random();
            int amount = 0;
            switch (tier)
            {
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    break;
                case 7:
                    if (r.Next(0, 1000) == 0)
                    {
                        amount = 1;
                    }
                    if (r.Next(0, 10000) == 0)
                    {
                        amount = 2;
                    }
                    if (r.Next(0, 100000) == 0)
                    {
                        amount = 3;
                    }
                    break;
                default:
                    if (r.Next(0, 1000) == 0)
                    {
                        amount = 1;
                    }
                    if (r.Next(0, 10000) == 0)
                    {
                        amount = 2;
                    }
                    if (r.Next(0, 100000) == 0)
                    {
                        amount = 3;
                    }
                    break;

            }
            return amount;
        }

        public static int GetNumMajorCantrips(int tier)
        {
            Random r = new Random();
            int amount = 0;
            switch (tier)
            {
                case 1:
                    amount = 0;
                    break;
                case 2:
                    if (r.Next(0, 500) == 0)
                    {
                        amount = 1;
                    }
                    break;
                case 3:
                    if (r.Next(0, 500) == 0)
                    {
                        amount = 1;
                    }
                    if (r.Next(0, 10000) == 0)
                    {
                        amount = 2;
                    }
                    break;
                case 4:
                    if (r.Next(0, 500) == 0)
                    {
                        amount = 1;
                    }
                    if (r.Next(0, 5000) == 0)
                    {
                        amount = 2;
                    }
                    break;
                case 5:
                    if (r.Next(0, 500) == 0)
                    {
                        amount = 1;
                    }
                    if (r.Next(0, 5000) == 0)
                    {
                        amount = 2;
                    }
                    break;
                case 6:
                    if (r.Next(0, 500) == 0)
                    {
                        amount = 1;
                    }
                    if (r.Next(0, 5000) == 0)
                    {
                        amount = 2;
                    }
                    break;
                case 7:
                    if (r.Next(0, 500) == 0)
                    {
                        amount = 1;
                    }
                    if (r.Next(0, 5000) == 0)
                    {
                        amount = 2;
                    }
                    if (r.Next(0, 15000) == 0)
                    {
                        amount = 3;
                    }
                    break;
                default:
                    if (r.Next(0, 500) == 0)
                    {
                        amount = 1;
                    }
                    if (r.Next(0, 5000) == 0)
                    {
                        amount = 2;
                    }
                    if (r.Next(0, 15000) == 0)
                    {
                        amount = 3;
                    }
                    break;

            }
            return amount;
        }

        public static int GetNumMinorCantrips(int tier)
        {
            Random r = new Random();
            int amount = 0;
            switch (tier)
            {
                case 1:
                    if (r.Next(0, 100) == 0)
                    {
                        amount = 1;
                    }
                    break;
                case 2:
                    if (r.Next(0, 50) == 0)
                    {
                        amount = 1;
                    }
                    if (r.Next(0, 250) == 0)
                    {
                        amount = 2;
                    }
                    break;
                case 3:
                    if (r.Next(0, 50) == 0)
                    {
                        amount = 1;
                    }
                    if (r.Next(0, 250) == 0)
                    {
                        amount = 2;
                    }
                    break;
                case 4:
                    if (r.Next(0, 50) == 0)
                    {
                        amount = 1;
                    }
                    if (r.Next(0, 250) == 0)
                    {
                        amount = 2;
                    }
                    if (r.Next(0, 1000) == 0)
                    {
                        amount = 3;
                    }
                    break;
                case 5:
                    if (r.Next(0, 50) == 0)
                    {
                        amount = 1;
                    }
                    if (r.Next(0, 250) == 0)
                    {
                        amount = 2;
                    }
                    if (r.Next(0, 1000) == 0)
                    {
                        amount = 3;
                    }
                    break;
                case 6:
                    if (r.Next(0, 50) == 0)
                    {
                        amount = 1;
                    }
                    if (r.Next(0, 250) == 0)
                    {
                        amount = 2;
                    }
                    if (r.Next(0, 1000) == 0)
                    {
                        amount = 3;
                    }
                    if (r.Next(0, 5000) == 0)
                    {
                        amount = 4;
                    }
                    break;
                case 7:
                    if (r.Next(0, 50) == 0)
                    {
                        amount = 1;
                    }
                    if (r.Next(0, 250) == 0)
                    {
                        amount = 2;
                    }
                    if (r.Next(0, 1000) == 0)
                    {
                        amount = 3;
                    }
                    if (r.Next(0, 5000) == 0)
                    {
                        amount = 4;
                    }
                    break;
                default:
                    if (r.Next(0, 50) == 0)
                    {
                        amount = 1;
                    }
                    if (r.Next(0, 250) == 0)
                    {
                        amount = 2;
                    }
                    if (r.Next(0, 1000) == 0)
                    {
                        amount = 3;
                    }
                    if (r.Next(0, 5000) == 0)
                    {
                        amount = 4;
                    }
                    break;

            }
            return amount;
        }
    }
}
