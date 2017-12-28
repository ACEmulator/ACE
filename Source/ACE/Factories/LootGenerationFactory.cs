using ACE.Entity.Actions;
using ACE.Entity;
using ACE.Database;
using ACE.Managers;
using ACE.Network.GameEvent.Events;
using ACE.Network.Sequence;
using ACE.Entity.Enum;
using System.Collections.Generic;
using System;
using ACE.Entity.Enum.Properties;

namespace ACE.Factories
{
    public class LootGenerationFactory
    {
        // This is throw away code to understand the world object creation process.

        public static void Spawn(WorldObject inventoryItem, Position position)
        {
            GetSpawnChain(inventoryItem, position).EnqueueChain();
        }

        public static ActionChain GetSpawnChain(WorldObject inventoryItem, Position position)
        {
            inventoryItem.Sequences.GetNextSequence(SequenceType.ObjectTeleport);
            inventoryItem.Sequences.GetNextSequence(SequenceType.ObjectVector);
            inventoryItem.Location = position.InFrontOf(1.00f);
            inventoryItem.PhysicsDescriptionFlag |= PhysicsDescriptionFlag.Position;
            return LandblockManager.GetAddObjectChain(inventoryItem);
        }

        public static void CreateRandomTestWorldObjects(Player player, uint typeId, uint numItems)
        {
            var weenieList = DatabaseManager.World.GetRandomWeeniesOfType(typeId, numItems);
            List<WorldObject> items = new List<WorldObject>();
            for (int i = 0; i < numItems; i++)
            {
                WorldObject wo = WorldObjectFactory.CreateNewWorldObject(weenieList[i].WeenieClassId);
                items.Add(wo);
            }
            player.HandleAddNewWorldObjectsToInventory(items);
        }

        public static void CreateTierOneLootItems(Player player)
        {
            String[] gemName = { "Agate", "Amber", "Amethyst", "Aquamarine", "Azurite", "Black Garnet", "Black Opal", "Bloodstone", "Carnelian", "Citrine", "Diamond", "Emerald", "Fire Opal", "Geen Garnet", "Green Jade", "Hematite", "Imperial Topaz", "Jet", "Lapis Lazuli", "Lavender Jade", "Malachite", "Moonstone", "Onyx", "Opal", "Peridot", "Red Garnet", "Rose Quartz", "Ruby", "Sapphire", "Smokey Quartz", "Sunstone", "Tiger Eye", "Tourmaline", "Turquoise", "White Jade", "White Quartz", "White Sapphire", "Yellow Garnet", "Yellow Topaz", "Zircon" };
            int[] creature1 = { 2, 18, 256, 274, 298, 322, 346, 418, 467, 557, 581, 605, 629, 653, 678, 702, 726, 750, 774, 798, 824, 850, 874, 898, 922, 946, 970, 982, 1349, 1373, 1397, 1421, 1445, 1715, 1739, 1763, 5779, 5803, 5827, 5843, 5867, 6116 };
            int[] creature2 = { 1328, 245, 257, 275, 299, 323, 347, 419, 468, 558, 582, 606, 630, 654, 679, 703, 727, 751, 775, 799, 825, 851, 875, 899, 923, 947, 971, 983, 1350, 1374, 1398, 1422, 1446, 1716, 1740, 1764, 5780, 5804, 5828, 5844, 5868, 6117 };
            int[] creature3 = { 1329, 246, 258, 276, 300, 324, 348, 420, 469, 559, 583, 607, 631, 655, 680, 704, 728, 752, 776, 800, 826, 852, 876, 900, 924, 948, 972, 984, 1351, 1375, 1399, 1423, 1447, 1717, 1741, 1765, 5781, 5805, 5829, 5845, 5869, 6118 };
            int[] life1 = { 165, 54, 212, 515, 1018, 1030, 1066, 20, 1109, 1133, 24 };
            int[] life2 = { 166, 189, 213, 516, 1019, 1031, 1067, 1090, 1110, 1134, 1308 };
            int[] life3 = { 167, 190, 214, 517, 1020, 1032, 1068, 1091, 1111, 1135, 1309 };
            uint weenieType = 0;
            Random rnd = new Random();
            int numItems = rnd.Next(1, 4);
            List<WorldObject> items = new List<WorldObject>();
            for (int i = 0; i < numItems; i++)
            {
                int chance = rnd.Next(0, 4);
                ////Chance to be a Mundane Item.. 50%
                if (chance == 0)
                {
                    int itemNum = rnd.Next(1, 24);
                    switch (itemNum)
                    {
                        case 1:
                            weenieType = 8329;
                            break;
                        case 2:
                            weenieType = 27331;
                            break;
                        case 3:
                            weenieType = 2434;
                            break;
                        case 4:
                            weenieType = 378;
                            break;
                        case 5:
                            weenieType = 2457;
                            break;
                        case 6:
                            weenieType = 2460;
                            break;
                        case 7:
                            weenieType = 27326;
                            break;
                        case 8:
                            weenieType = 377;
                            break;
                        case 9:
                            weenieType = 379;
                            break;
                        case 10:
                            weenieType = 13233;
                            break;
                        case 11:
                            weenieType = 628;
                            break;
                        case 12:
                            weenieType = 511;
                            break;
                        case 13:
                            weenieType = 513;
                            break;
                        case 14:
                            weenieType = 42518;
                            break;
                        case 15:
                            ////apple
                            weenieType = 258;
                            break;
                        case 16:
                            ////bread
                            weenieType = 259;
                            break;
                        case 17:
                            ////brimstone cap mushroom
                            weenieType = 547;
                            break;
                        case 18:
                            ////cabbage
                            weenieType = 260;
                            break;
                        case 19:
                            ////cheese
                            weenieType = 261;
                            break;
                        case 20:
                            ////chicken(food)
                            weenieType = 262;
                            break;
                        case 21:
                            ////egg
                            weenieType = 546;
                            break;
                        case 22:
                            ////Fish
                            weenieType = 263;
                            break;
                        case 23:
                            ////grapes
                            weenieType = 264;
                            break;
                        case 24:
                            ////side of beef
                            weenieType = 4753;
                            break;
                    }
                    WorldObject wo = WorldObjectFactory.CreateNewWorldObject(weenieType);
                    items.Add(wo);
                }
                ////Chance to be gem
                if (chance == 1)
                {
                    int itemNum = rnd.Next(1, 19);
                    switch (itemNum)
                    {
                        case 1:
                            weenieType = 2432;
                            break;
                        case 2:
                            weenieType = 2418;
                            break;
                        case 3:
                            weenieType = 2419;
                            break;
                        case 4:
                            weenieType = 2420;
                            break;
                        case 5:
                            weenieType = 2426;
                            break;
                        case 6:
                            weenieType = 2431;
                            break;
                        case 7:
                            weenieType = 2413;
                            break;
                        case 8:
                            weenieType = 2414;
                            break;
                        case 9:
                            weenieType = 2427;
                            break;
                        case 10:
                            weenieType = 2428;
                            break;
                        case 11:
                            weenieType = 2429;
                            break;
                        case 12:
                            weenieType = 2430;
                            break;
                        case 13:
                            weenieType = 2415;
                            break;
                        case 14:
                            weenieType = 2405;
                            break;
                        case 15:
                            weenieType = 2416;
                            break;
                        case 16:
                            weenieType = 2406;
                            break;
                        case 17:
                            weenieType = 2433;
                            break;
                        case 18:
                            weenieType = 2417;
                            break;
                    }
                    int magicChance = rnd.Next(0, 2);
                    int spellNum = 0;
                    int[][] spells = LootHelper.GemSpells;
                    int spellCount = 1;
                    ////0 means we have a spell on our gem
                    ////Create item template
                    var wo = WorldObjectFactory.CreateNewWorldObject(weenieType) as Gem;
                    int spellLevel = 1;
                    wo.PropertiesSpellId.Clear();
                    if (magicChance == 0)
                    {
                        wo.UiEffects = UiEffects.Magical;
                        int[] itemSpell = new int[spellCount];
                        List<int> nums = new List<int>();
                        for (int num = 0; num < spells.Length; num++)
                        {
                            nums.Add(num);
                        }
                        for (int j = 0; j < spellCount; j++)
                        {
                            itemSpell[j] = rnd.Next(0, nums.Count);
                            int spellLevelChance = rnd.Next(0, 100);
                            if (spellLevelChance > 95)
                            {
                                spellLevel = 3;
                            }
                            else if (spellLevelChance > 70)
                            {
                                spellLevel = 2;
                            }
                            else
                            {
                                spellLevel = 1;
                            }
                            var propSpell = new AceObjectPropertiesSpell
                            {
                                AceObjectId = wo.Guid.Full,
                                SpellId = (uint)spells[itemSpell[j]][spellLevel]
                            };
                            spellNum = spells[itemSpell[j]][spellLevel];
                            Console.WriteLine("Writing a spell to the list...." + propSpell.SpellId);
                            wo.PropertiesSpellId.Add(propSpell);
                            Console.WriteLine(wo.PropertiesSpellId);
                            nums.RemoveAt(itemSpell[j]);
                        }
                    }
                    ////Affects the workmanship of an item
                    int workmanshipChance = rnd.Next(1, 101);
                    int workmanship = 0;
                    if (workmanshipChance > 95)
                    {
                        workmanship = 5;
                    }
                    else if (workmanshipChance > 80)
                    {
                        workmanship = 4;
                    }
                    else if (workmanshipChance > 50)
                    {
                        workmanship = 3;
                    }
                    else if (workmanshipChance > 20)
                    {
                        workmanship = 2;
                    }
                    else
                    {
                        workmanship = 1;
                    }
                    wo.Workmanship = workmanship;
                    uint spellNum2 = (uint)spellNum;
                    wo.Usable = Usable.Contained;
                    wo.SpellDID = spellNum2;
                    int gemValue = (int)workmanship * rnd.Next(5, 100);
                    wo.Value = gemValue;
                    wo.AppraisalLongDescDecoration = 5;
                    items.Add(wo);
                }
                if (chance == 2)
                {
                    int[][] spells;
                    int armorWeenie = 0;
                    ////How many spells?
                    int spellCount = 0;
                    int spellCountChance = rnd.Next(1, 101);
                    if (spellCountChance > 98)
                    {
                        spellCount = 4;
                    }
                    else if (spellCountChance > 85)
                    {
                        spellCount = 3;
                    }
                    else if (spellCountChance > 65)
                    {
                        spellCount = 2;
                    }
                    else if (spellCountChance > 20)
                    {
                        spellCount = 1;
                    }
                    else
                    {
                        spellCount = 0;
                    }
                    int armorType = rnd.Next(0, 4);
                    ////Leather Armor
                    if (armorType == 0)
                    {
                        ////Thinking an array would be more concise.
                        int armorPiece = rnd.Next(0, 16);
                        if (armorPiece == 0)
                        {
                            ////helm
                            armorWeenie = 25636;
                            spells = LootHelper.HeadSpells;
                        }
                        else if (armorPiece == 1)
                        {
                            ////head
                            armorWeenie = 25640;
                            spells = LootHelper.HeadSpells;
                        }
                        else if (armorPiece == 2)
                        {
                            ////Chest
                            armorWeenie = 25639;
                            spells = LootHelper.ChestSpells;
                        }
                        else if (armorPiece == 3)
                        {
                            ////Chest
                            armorWeenie = 25641;
                            spells = LootHelper.ChestSpells;
                        }
                        else if (armorPiece == 4)
                        {
                            ////Chest
                            armorWeenie = 25638;
                            spells = LootHelper.ChestSpells;
                        }
                        else if (armorPiece == 5)
                        {
                            ////arms
                            armorWeenie = 25651;
                            spells = LootHelper.UpperArmSpells;
                        }
                        else if (armorPiece == 6)
                        {
                            ////Hands
                            armorWeenie = 25642;
                            spells = LootHelper.HandSpells;
                        }
                        else if (armorPiece == 7)
                        {
                            ////Lower Arms
                            armorWeenie = 25637;
                            spells = LootHelper.LowerArmSpells;
                        }
                        else if (armorPiece == 8)
                        {
                            ////Upper arms
                            armorWeenie = 25648;
                            spells = LootHelper.UpperArmSpells;
                        }
                        else if (armorPiece == 9)
                        {
                            ////Abdomen
                            armorWeenie = 25643;
                            spells = LootHelper.AbdomenSpells;
                        }
                        else if (armorPiece == 10)
                        {
                            ////Abdomen
                            armorWeenie = 25650;
                            spells = LootHelper.AbdomenSpells;
                        }
                        else if (armorPiece == 11)
                        {
                            ////legs
                            armorWeenie = 25647;
                            spells = LootHelper.UpperLegSpells;
                        }
                        else if (armorPiece == 12)
                        {
                            ////legs
                            armorWeenie = 25645;
                            spells = LootHelper.UpperLegSpells;
                        }
                        else if (armorPiece == 13)
                        {
                            ////Upper legs
                            armorWeenie = 25652;
                            spells = LootHelper.UpperLegSpells;
                        }
                        else if (armorPiece == 14)
                        {
                            ////lower legs
                            armorWeenie = 25644;
                            spells = LootHelper.LowerLegSpells;
                        }
                        else
                        {
                            ////feet
                            armorWeenie = 25661;
                            spells = LootHelper.FeetSpells;
                        }
                        WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)armorWeenie) as Clothing;
                        wo.PropertiesSpellId.Clear();
                        if (spellCount > 0)
                        {
                            int[] itemSpell = new int[spellCount];
                            List<int> nums = new List<int>();
                            for (int num = 0; num < spells.Length; num++)
                            {
                                nums.Add(num);
                            }
                            for (int j = 0; j < spellCount; j++)
                            {
                                itemSpell[j] = rnd.Next(0, nums.Count);
                                int spellLevel;
                                int spellLevelChance = rnd.Next(0, 100);
                                if (spellLevelChance > 95)
                                {
                                    spellLevel = 3;
                                }
                                else if (spellLevelChance > 70)
                                {
                                    spellLevel = 2;
                                }
                                else
                                {
                                    spellLevel = 1;
                                }
                                var propSpell = new AceObjectPropertiesSpell
                                {
                                    AceObjectId = wo.Guid.Full,
                                    SpellId = (uint)spells[itemSpell[j]][spellLevel]
                                };
                                Console.WriteLine("Writing a spell to the list...." + propSpell.SpellId);
                                wo.PropertiesSpellId.Add(propSpell);
                                Console.WriteLine(wo.PropertiesSpellId);
                                nums.RemoveAt(itemSpell[j]);
                            }
                        }

                        ////Setting Material Type
                        int itemMaterial = rnd.Next(0, 4);
                        if (itemMaterial == 0)
                        {
                            wo.MaterialType = Material.Leather;
                        }
                        if (itemMaterial == 1)
                        {
                            wo.MaterialType = Material.ArmoredilloHide;
                        }
                        if (itemMaterial == 2)
                        {
                            wo.MaterialType = Material.ReedSharkHide;
                        }
                        if (itemMaterial == 3)
                        {
                            wo.MaterialType = Material.GromnieHide;
                        }
                        ////Armor Level Randomizer and set... Tier 1 Range is from 40-140
                        int armorLevel = rnd.Next(40, 141);
                        wo.ArmorLevel = armorLevel;
                        ////Setting ItemWorkmanship... Tier 1 Range is from 1-5
                        int itemWorkmanship = rnd.Next(1, 6);
                        wo.Workmanship = itemWorkmanship;
                        ////Value... Not sure what the range is, so just using arbitrary numbers based on workmanship
                        int itemValue = itemWorkmanship * rnd.Next(10, 40);
                        wo.Value = itemValue;
                        ////ArmorModVsSlash, with a value between 0.0-2.0.
                        double val = .1 * rnd.Next(1, 21);
                        wo.ArmorModVsSlash = val;
                        ////ArmorModVsPierce, with a value between 0.0-2.0.
                        val = .1 * rnd.Next(1, 21);
                        wo.ArmorModVsPierce = val;
                        ////ArmorModVsBludgeon, with a value between 0.0-2.0.
                        val = .1 * rnd.Next(1, 21);
                        wo.ArmorModVsBludgeon = val;
                        ////ArmorModVsCold, with a value between 0.0-2.0.
                        val = .1 * rnd.Next(1, 21);
                        wo.ArmorModVsCold = val;
                        ////ArmorModVsFire, with a value between 0.0-2.0.
                        val = .1 * rnd.Next(1, 21);
                        wo.ArmorModVsFire = val;
                        ////ArmorModVsAcid, with a value between 0.0-2.0.
                        val = .1 * rnd.Next(1, 21);
                        wo.ArmorModVsAcid = val;
                        ////ArmorModVsElectric, with a value between 0.0-2.0.
                        val = .1 * rnd.Next(1, 21);
                        wo.ArmorModVsElectric = val;
                        ////ArmorModVsNether, with a value between 0.0-2.0.
                        val = .1 * rnd.Next(1, 21);
                        wo.ArmorModVsNether = val;
                        ////Affects the workmanship of an item
                        int workmanshipChance = rnd.Next(1, 101);
                        float workmanship = 0;
                        if (workmanshipChance > 95)
                        {
                            workmanship = 5;
                        }
                        else if (workmanshipChance > 80)
                        {
                            workmanship = 4;
                        }
                        else if (workmanshipChance > 50)
                        {
                            workmanship = 3;
                        }
                        else if (workmanshipChance > 20)
                        {
                            workmanship = 2;
                        }
                        else
                        {
                            workmanship = 1;
                        }
                        wo.Workmanship = (int)workmanship;
                        items.Add(wo);
                    }
                    ////Studded Leather
                    if (armorType == 1)
                    {
                        ////Thinking a dictionary with random roll/WeenieID would be more concise.
                        int armorPiece = rnd.Next(0, 15);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 554;
                            spells = LootHelper.HeadSpells;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 116;
                            spells = LootHelper.FeetSpells;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 38;
                            spells = LootHelper.LowerArmSpells;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 42;
                            spells = LootHelper.ChestSpells;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 48;
                            spells = LootHelper.ChestSpells;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 723;
                            spells = LootHelper.HeadSpells;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 53;
                            spells = LootHelper.ChestSpells;
                        }
                        else if (armorPiece == 7)
                        {
                            armorWeenie = 59;
                            spells = LootHelper.HandSpells;
                        }
                        else if (armorPiece == 8)
                        {
                            armorWeenie = 63;
                            spells = LootHelper.AbdomenSpells;
                        }
                        else if (armorPiece == 9)
                        {
                            armorWeenie = 68;
                            spells = LootHelper.LowerLegSpells;
                        }
                        else if (armorPiece == 10)
                        {
                            armorWeenie = 45967;
                            spells = LootHelper.HeadSpells;
                        }
                        else if (armorPiece == 11)
                        {
                            armorWeenie = 89;
                            spells = LootHelper.LowerLegSpells;
                        }
                        else if (armorPiece == 12)
                        {
                            armorWeenie = 99;
                            spells = LootHelper.ChestSpells;
                        }
                        else if (armorPiece == 13)
                        {
                            armorWeenie = 105;
                            spells = LootHelper.UpperArmSpells;
                        }
                        else
                        {
                            armorWeenie = 112;
                            spells = LootHelper.UpperLegSpells;
                        }
                        WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)armorWeenie);
                        wo.PropertiesSpellId.Clear();
                        if (spellCount > 0)
                        {
                            wo.UiEffects = UiEffects.Magical;
                            int[] itemSpell = new int[spellCount];
                            List<int> nums = new List<int>();
                            for (int num = 0; num < spells.Length; num++)
                            {
                                nums.Add(num);
                            }
                            for (int j = 0; j < spellCount; j++)
                            {
                                itemSpell[j] = rnd.Next(0, nums.Count);
                                int spellLevel;
                                int spellLevelChance = rnd.Next(0, 100);
                                if (spellLevelChance > 95)
                                {
                                    spellLevel = 3;
                                }
                                else if (spellLevelChance > 70)
                                {
                                    spellLevel = 2;
                                }
                                else
                                {
                                    spellLevel = 1;
                                }
                                var propSpell = new AceObjectPropertiesSpell
                                {
                                    AceObjectId = wo.Guid.Full,
                                    SpellId = (uint)spells[itemSpell[j]][spellLevel]
                                };
                                Console.WriteLine("Writing a spell to the list...." + propSpell.SpellId);
                                wo.PropertiesSpellId.Add(propSpell);
                                Console.WriteLine(wo.PropertiesSpellId);
                                nums.RemoveAt(itemSpell[j]);
                            }
                        }
                        ////Setting Material Type
                        int itemMaterial = rnd.Next(0, 4);
                        if (itemMaterial == 0)
                        {
                            wo.MaterialType = Material.Leather;
                        }
                        if (itemMaterial == 1)
                        {
                            wo.MaterialType = Material.ArmoredilloHide;
                        }
                        if (itemMaterial == 2)
                        {
                            wo.MaterialType = Material.ReedSharkHide;
                        }
                        if (itemMaterial == 3)
                        {
                            wo.MaterialType = Material.GromnieHide;
                        }
                        ////Armor Level Randomizer and set... Tier 1 Range is from 40-140
                        int armorLevel = rnd.Next(50, 161);
                        wo.ArmorLevel = armorLevel;
                        ////Setting ItemWorkmanship... Tier 1 Range is from 1-5
                        int itemWorkmanship = rnd.Next(1, 6);
                        wo.Workmanship = itemWorkmanship;
                        ////Value... Not sure what the range is, so just using arbitrary numbers based on workmanship
                        int itemValue = itemWorkmanship * rnd.Next(10, 40);
                        wo.Value = itemValue;
                        ////ArmorModVsSlash, with a value between 0.0-2.0.
                        double val = .1 * rnd.Next(1, 21);
                        wo.ArmorModVsSlash = val;
                        ////ArmorModVsPierce, with a value between 0.0-2.0.
                        val = .1 * rnd.Next(1, 21);
                        wo.ArmorModVsPierce = val;
                        ////ArmorModVsBludgeon, with a value between 0.0-2.0.
                        val = .1 * rnd.Next(1, 21);
                        wo.ArmorModVsBludgeon = val;
                        ////ArmorModVsCold, with a value between 0.0-2.0.
                        val = .1 * rnd.Next(1, 21);
                        wo.ArmorModVsCold = val;
                        ////ArmorModVsFire, with a value between 0.0-2.0.
                        val = .1 * rnd.Next(1, 21);
                        wo.ArmorModVsFire = val;
                        ////ArmorModVsAcid, with a value between 0.0-2.0.
                        val = .1 * rnd.Next(1, 21);
                        wo.ArmorModVsAcid = val;
                        ////ArmorModVsElectric, with a value between 0.0-2.0.
                        val = .1 * rnd.Next(1, 21);
                        wo.ArmorModVsElectric = val;
                        ////ArmorModVsNether, with a value between 0.0-2.0.
                        val = .1 * rnd.Next(1, 21);
                        wo.ArmorModVsNether = val;
                        ////Affects the workmanship of an item
                        int workmanshipChance = rnd.Next(1, 101);
                        float workmanship = 0;
                        if (workmanshipChance > 95)
                        {
                            workmanship = 5;
                        }
                        else if (workmanshipChance > 80)
                        {
                            workmanship = 4;
                        }
                        else if (workmanshipChance > 50)
                        {
                            workmanship = 3;
                        }
                        else if (workmanshipChance > 20)
                        {
                            workmanship = 2;
                        }
                        else
                        {
                            workmanship = 1;
                        }
                        wo.Workmanship = (int)workmanship;
                        items.Add(wo);
                    }
                    ////Chainmail
                    if (armorType == 2)
                    {
                        ////Thinking a dictionary with random roll/WeenieID would be more concise.
                        int armorPiece = rnd.Next(0, 13);
                        if (armorPiece == 0)
                        {
                            armorWeenie = 35;
                            spells = LootHelper.HeadSpells;
                        }
                        else if (armorPiece == 1)
                        {
                            armorWeenie = 413;
                            spells = LootHelper.LowerArmSpells;
                        }
                        else if (armorPiece == 2)
                        {
                            armorWeenie = 414;
                            spells = LootHelper.ChestSpells;
                        }
                        else if (armorPiece == 3)
                        {
                            armorWeenie = 85;
                            spells = LootHelper.HeadSpells;
                        }
                        else if (armorPiece == 4)
                        {
                            armorWeenie = 30525;
                            spells = LootHelper.HandSpells;
                        }
                        else if (armorPiece == 5)
                        {
                            armorWeenie = 415;
                            spells = LootHelper.AbdomenSpells;
                        }
                        else if (armorPiece == 6)
                        {
                            armorWeenie = 2605;
                            spells = LootHelper.LowerLegSpells;
                        }
                        else if (armorPiece == 7)
                        {
                            armorWeenie = 71;
                            spells = LootHelper.ChestSpells;
                        }
                        else if (armorPiece == 8)
                        {
                            armorWeenie = 80;
                            spells = LootHelper.UpperLegSpells;
                        }
                        else if (armorPiece == 9)
                        {
                            armorWeenie = 416;
                            spells = LootHelper.UpperArmSpells;
                        }
                        else if (armorPiece == 10)
                        {
                            armorWeenie = 96;
                            spells = LootHelper.ChestSpells;
                        }
                        else if (armorPiece == 11)
                        {
                            armorWeenie = 101;
                            spells = LootHelper.UpperArmSpells;
                        }
                        else
                        {
                            armorWeenie = 108;
                            spells = LootHelper.UpperLegSpells;
                        }
                        WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)armorWeenie);
                        wo.PropertiesSpellId.Clear();
                        if (spellCount > 0)
                        {
                            wo.UiEffects = UiEffects.Magical;
                            int[] itemSpell = new int[spellCount];
                            List<int> nums = new List<int>();
                            for (int num = 0; num < spells.Length; num++)
                            {
                                nums.Add(num);
                            }
                            for (int j = 0; j < spellCount; j++)
                            {
                                itemSpell[j] = rnd.Next(0, nums.Count);
                                int spellLevel;
                                int spellLevelChance = rnd.Next(0, 100);
                                if (spellLevelChance > 95)
                                {
                                    spellLevel = 3;
                                }
                                else if (spellLevelChance > 70)
                                {
                                    spellLevel = 2;
                                }
                                else
                                {
                                    spellLevel = 1;
                                }
                                var propSpell = new AceObjectPropertiesSpell
                                {
                                    AceObjectId = wo.Guid.Full,
                                    SpellId = (uint)spells[itemSpell[j]][spellLevel]
                                };
                                Console.WriteLine("Writing a spell to the list...." + propSpell.SpellId);
                                wo.PropertiesSpellId.Add(propSpell);
                                Console.WriteLine(wo.PropertiesSpellId);
                                nums.RemoveAt(itemSpell[j]);
                            }
                        }
                        ////Setting Material Type
                        double materialValueMod;
                        int itemMaterial = rnd.Next(0, 7);
                        if (itemMaterial == 0)
                        {
                            wo.MaterialType = Material.Brass;
                            materialValueMod = 1.0;
                        }
                        if (itemMaterial == 1)
                        {
                            wo.MaterialType = Material.Bronze;
                            materialValueMod = 1.0;
                        }
                        if (itemMaterial == 2)
                        {
                            wo.MaterialType = Material.Copper;
                            materialValueMod = 1.0;
                        }
                        if (itemMaterial == 3)
                        {
                            wo.MaterialType = Material.Gold;
                            materialValueMod = 1.0;
                        }
                        if (itemMaterial == 4)
                        {
                            wo.MaterialType = Material.Iron;
                            materialValueMod = 1.0;
                        }
                        if (itemMaterial == 5)
                        {
                            wo.MaterialType = Material.Silver;
                            materialValueMod = 1.0;
                        }
                        if (itemMaterial == 6)
                        {
                            wo.MaterialType = Material.Steel;
                            materialValueMod = 1.0;
                        }
                        ////Armor Level Randomizer and set... Tier 1 Range is from 40-140
                        int armorLevel = rnd.Next(40, 141);
                        wo.ArmorLevel = armorLevel;
                        ////Setting ItemWorkmanship... Tier 1 Range is from 1-5
                        int itemWorkmanship = rnd.Next(1, 6);
                        wo.Workmanship = itemWorkmanship;
                        ////Value... Not sure what the range is, so just using arbitrary numbers based on workmanship
                        int itemValue = itemWorkmanship * rnd.Next(10, 40);
                        wo.Value = itemValue;
                        ////ArmorModVsSlash, with a value between 0.0-2.0.
                        double val = .1 * rnd.Next(1, 21);
                        wo.ArmorModVsSlash = val;
                        ////ArmorModVsPierce, with a value between 0.0-2.0.
                        val = .1 * rnd.Next(1, 21);
                        wo.ArmorModVsPierce = val;
                        ////ArmorModVsBludgeon, with a value between 0.0-2.0.
                        val = .1 * rnd.Next(1, 21);
                        wo.ArmorModVsBludgeon = val;
                        ////ArmorModVsCold, with a value between 0.0-2.0.
                        val = .1 * rnd.Next(1, 21);
                        wo.ArmorModVsCold = val;
                        ////ArmorModVsFire, with a value between 0.0-2.0.
                        val = .1 * rnd.Next(1, 21);
                        wo.ArmorModVsFire = val;
                        ////ArmorModVsAcid, with a value between 0.0-2.0.
                        val = .1 * rnd.Next(1, 21);
                        wo.ArmorModVsAcid = val;
                        ////ArmorModVsElectric, with a value between 0.0-2.0.
                        val = .1 * rnd.Next(1, 21);
                        wo.ArmorModVsElectric = val;
                        ////ArmorModVsNether, with a value between 0.0-2.0.
                        val = .1 * rnd.Next(1, 21);
                        wo.ArmorModVsNether = val;
                        ////Affects the workmanship of an item
                        int workmanshipChance = rnd.Next(1, 101);
                        float workmanship = 0;
                        if (workmanshipChance > 95)
                        {
                            workmanship = 5;
                        }
                        else if (workmanshipChance > 80)
                        {
                            workmanship = 4;
                        }
                        else if (workmanshipChance > 50)
                        {
                            workmanship = 3;
                        }
                        else if (workmanshipChance > 20)
                        {
                            workmanship = 2;
                        }
                        else
                        {
                            workmanship = 1;
                        }
                        wo.Workmanship = (int)workmanship;
                        items.Add(wo);
                    }
                    if (armorType == 3)
                    {
                        ////Jewelry
                        int jewelryPiece = rnd.Next(0, 6);
                        if (jewelryPiece == 0)
                        {
                            ////Amulet
                            armorWeenie = 294;
                        }
                        if (jewelryPiece == 1)
                        {
                            ////Bracelet
                            armorWeenie = 295;
                        }
                        if (jewelryPiece == 2)
                        {
                            ////ring
                            armorWeenie = 297;
                        }
                        if (jewelryPiece == 3)
                        {
                            ////necklace
                            armorWeenie = 622;
                        }
                        if (jewelryPiece == 4)
                        {
                            ////Heavy Bracelet
                            armorWeenie = 43930;
                        }
                        if (jewelryPiece == 5)
                        {
                            ////Heavy Necklace
                            armorWeenie = 415;
                        }
                        WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)armorWeenie);
                        spells = LootHelper.JewelrySpells;
                        wo.PropertiesSpellId.Clear();
                        if (spellCount > 0)
                        {
                            wo.UiEffects = UiEffects.Magical;
                            int[] itemSpell = new int[spellCount];
                            List<int> nums = new List<int>();
                            for (int num = 0; num < spells.Length; num++)
                            {
                                nums.Add(num);
                            }
                            for (int j = 0; j < spellCount; j++)
                            {
                                itemSpell[j] = rnd.Next(0, nums.Count);
                                int spellLevel;
                                int spellLevelChance = rnd.Next(0, 100);
                                if (spellLevelChance > 95)
                                {
                                    spellLevel = 3;
                                }
                                else if (spellLevelChance > 70)
                                {
                                    spellLevel = 2;
                                }
                                else
                                {
                                    spellLevel = 1;
                                }
                                var propSpell = new AceObjectPropertiesSpell
                                {
                                    AceObjectId = wo.Guid.Full,
                                    SpellId = (uint)spells[itemSpell[j]][spellLevel]
                                };
                                Console.WriteLine("Writing a spell to the list...." + propSpell.SpellId);
                                wo.PropertiesSpellId.Add(propSpell);
                                Console.WriteLine(wo.PropertiesSpellId);
                                nums.RemoveAt(itemSpell[j]);
                            }
                        }
                        ////Setting Material Type
                        double materialValueMod;
                        int itemMaterial = rnd.Next(0, 7);
                        if (itemMaterial == 0)
                        {
                            wo.MaterialType = Material.Brass;
                            materialValueMod = 1.0;
                        }
                        if (itemMaterial == 1)
                        {
                            wo.MaterialType = Material.Bronze;
                            materialValueMod = 1.0;
                        }
                        if (itemMaterial == 2)
                        {
                            wo.MaterialType = Material.Copper;
                            materialValueMod = 1.0;
                        }
                        if (itemMaterial == 3)
                        {
                            wo.MaterialType = Material.Gold;
                            materialValueMod = 1.0;
                        }
                        if (itemMaterial == 4)
                        {
                            wo.MaterialType = Material.Iron;
                            materialValueMod = 1.0;
                        }
                        if (itemMaterial == 5)
                        {
                            wo.MaterialType = Material.Silver;
                            materialValueMod = 1.0;
                        }
                        if (itemMaterial == 6)
                        {
                            wo.MaterialType = Material.Steel;
                            materialValueMod = 1.0;
                        }
                        ////Affects the workmanship of an item
                        int workmanshipChance = rnd.Next(1, 101);
                        float workmanship = 0;
                        if (workmanshipChance > 95)
                        {
                            workmanship = 5;
                        }
                        else if (workmanshipChance > 80)
                        {
                            workmanship = 4;
                        }
                        else if (workmanshipChance > 50)
                        {
                            workmanship = 3;
                        }
                        else if (workmanshipChance > 20)
                        {
                            workmanship = 2;
                        }
                        else
                        {
                            workmanship = 1;
                        }
                        wo.Workmanship = (int)workmanship;
                        ////Value... Not sure what the range is, so just using arbitrary numbers based on workmanship
                        int itemValue = (int)workmanship * rnd.Next(10, 40);
                        wo.Value = itemValue;
                        items.Add(wo);
                    }
                }
                if (chance == 3)
                {
                    ////What type of weapon... 6 choices 0-6
                    int[][] spells;
                    ////How many spells?
                    int spellCount = 0;
                    int spellCountChance = rnd.Next(1, 101);
                    if (spellCountChance > 98)
                    {
                        spellCount = 4;
                    }
                    else if (spellCountChance > 85)
                    {
                        spellCount = 3;
                    }
                    else if (spellCountChance > 65)
                    {
                        spellCount = 2;
                    }
                    else if (spellCountChance > 20)
                    {
                        spellCount = 1;
                    }
                    else
                    {
                        spellCount = 0;
                    }
                    int weaponType = rnd.Next(0, 6);
                    int weaponWeenie = 0;
                    if (weaponType == 0)
                    {
                        ////Heavy Weapons, of which there are 7 sub types
                        int subType = rnd.Next(0, 7);
                        if (subType == 0)
                        {
                            int subAxeType = rnd.Next(0, 3);
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
                            WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)weaponWeenie);
                            spells = LootHelper.MeleeSpells;
                            wo.PropertiesSpellId.Clear();
                            if (spellCount > 0)
                            {
                                wo.UiEffects = UiEffects.Magical;
                                int[] itemSpell = new int[spellCount];
                                List<int> nums = new List<int>();
                                for (int num = 0; num < spells.Length; num++)
                                {
                                    nums.Add(num);
                                }
                                for (int j = 0; j < spellCount; j++)
                                {
                                    itemSpell[j] = rnd.Next(0, nums.Count);
                                    int spellLevel;
                                    int spellLevelChance = rnd.Next(0, 100);
                                    if (spellLevelChance > 95)
                                    {
                                        spellLevel = 3;
                                    }
                                    else if (spellLevelChance > 70)
                                    {
                                        spellLevel = 2;
                                    }
                                    else
                                    {
                                        spellLevel = 1;
                                    }
                                    var propSpell = new AceObjectPropertiesSpell
                                    {
                                        AceObjectId = wo.Guid.Full,
                                        SpellId = (uint)spells[itemSpell[j]][spellLevel]
                                    };
                                    Console.WriteLine("Writing a spell to the list...." + propSpell.SpellId);
                                    wo.PropertiesSpellId.Add(propSpell);
                                    Console.WriteLine(wo.PropertiesSpellId);
                                    nums.RemoveAt(itemSpell[j]);
                                }
                            }
                            ////Set the damage, damagevariance, damagae attack, and damage defence
                            wo.DamageType = 1;
                            int damage = rnd.Next(6, 12);
                            wo.Damage = damage;
                            double damageVariance = .1 * rnd.Next(1, 11);
                            wo.DamageVariance = damageVariance;
                            wo.DamageMod = 1.0;
                            ////Sets the speed of the weapon
                            wo.WeaponTime = 50;
                            ////Setting Weapon Defense Mod
                            int defenseChance = rnd.Next(1, 101);
                            float weaponDefense = 0;
                            if (defenseChance > 98)
                            {
                                weaponDefense = 3;
                            }
                            else if (defenseChance > 90)
                            {
                                weaponDefense = 2;
                            }
                            else if (defenseChance > 75)
                            {
                                weaponDefense = 1;
                            }
                            else
                            {
                                weaponDefense = 0;
                            }
                            wo.WeaponDefense = 1.0 + (weaponDefense * .01);
                            ////Setting Weapon Defense Mod
                            int attackChance = rnd.Next(1, 101);
                            float attackDefense = 0;
                            if (attackChance > 98)
                            {
                                attackDefense = 3;
                            }
                            else if (attackChance > 90)
                            {
                                attackDefense = 2;
                            }
                            else if (attackChance > 75)
                            {
                                attackDefense = 1;
                            }
                            else
                            {
                                attackDefense = 0;
                            }
                            wo.WeaponOffense = 1.0 + (attackDefense * .01);
                            ////Sets how many gems and what type are in the description. May affect value later on.
                            wo.GemCount = rnd.Next(1, 6);
                            wo.GemType = rnd.Next(10, 51);
                            ////Setting Material Type
                            double materialValueMod;
                            int itemMaterial = rnd.Next(0, 7);
                            if (itemMaterial == 0)
                            {
                                wo.MaterialType = Material.Brass;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 1)
                            {
                                wo.MaterialType = Material.Bronze;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 2)
                            {
                                wo.MaterialType = Material.Copper;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 3)
                            {
                                wo.MaterialType = Material.Gold;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 4)
                            {
                                wo.MaterialType = Material.Iron;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 5)
                            {
                                wo.MaterialType = Material.Silver;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 6)
                            {
                                wo.MaterialType = Material.Steel;
                                materialValueMod = 1.0;
                            }
                            ////Affects the workmanship of an item
                            int workmanshipChance = rnd.Next(1, 101);
                            float workmanship = 0;
                            if (workmanshipChance > 95)
                            {
                                workmanship = 5;
                            }
                            else if (workmanshipChance > 80)
                            {
                                workmanship = 4;
                            }
                            else if (workmanshipChance > 50)
                            {
                                workmanship = 3;
                            }
                            else if (workmanshipChance > 20)
                            {
                                workmanship = 2;
                            }
                            else
                            {
                                workmanship = 1;
                            }
                            wo.Workmanship = (int)workmanship;
                            ////Value... Not sure what the range is, so just using arbitrary numbers based on workmanship
                            int itemValue = (int)workmanship * rnd.Next(10, 40);
                            wo.Value = itemValue;
                            items.Add(wo);
                        }
                        if (subType == 1)
                        {
                            ////There are three subtypes of daggers
                            int subDaggerType = rnd.Next(0, 3);
                            if (subDaggerType == 0)
                            {
                                ////Dirk
                                weaponWeenie = 22440;
                            }
                            if (subDaggerType == 1)
                            {
                                ////Stiletto
                                weaponWeenie = 7565;
                            }
                            if (subDaggerType == 2)
                            {
                                ////Jambiya
                                weaponWeenie = 319;
                            }
                            WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)weaponWeenie);
                            spells = LootHelper.MeleeSpells;
                            wo.PropertiesSpellId.Clear();
                            if (spellCount > 0)
                            {
                                wo.UiEffects = UiEffects.Magical;
                                int[] itemSpell = new int[spellCount];
                                List<int> nums = new List<int>();
                                for (int num = 0; num < spells.Length; num++)
                                {
                                    nums.Add(num);
                                }
                                for (int j = 0; j < spellCount; j++)
                                {
                                    itemSpell[j] = rnd.Next(0, nums.Count);
                                    int spellLevel;
                                    int spellLevelChance = rnd.Next(0, 100);
                                    if (spellLevelChance > 95)
                                    {
                                        spellLevel = 3;
                                    }
                                    else if (spellLevelChance > 70)
                                    {
                                        spellLevel = 2;
                                    }
                                    else
                                    {
                                        spellLevel = 1;
                                    }
                                    var propSpell = new AceObjectPropertiesSpell
                                    {
                                        AceObjectId = wo.Guid.Full,
                                        SpellId = (uint)spells[itemSpell[j]][spellLevel]
                                    };
                                    Console.WriteLine("Writing a spell to the list...." + propSpell.SpellId);
                                    wo.PropertiesSpellId.Add(propSpell);
                                    Console.WriteLine(wo.PropertiesSpellId);
                                    nums.RemoveAt(itemSpell[j]);
                                }
                            }
                            ////Set the damage, damagevariance, damagae attack, and damage defence
                            wo.DamageType = 3;
                            int damage = rnd.Next(1, 4);
                            wo.Damage = damage;
                            double damageVariance = .1 * rnd.Next(1, 11);
                            wo.DamageVariance = damageVariance;
                            wo.DamageMod = 1.0;
                            ////Sets the speed of the weapon
                            wo.WeaponTime = 10;
                            ////Setting Weapon Defense Mod
                            int defenseChance = rnd.Next(1, 101);
                            float weaponDefense = 0;
                            if (defenseChance > 98)
                            {
                                weaponDefense = 3;
                            }
                            else if (defenseChance > 90)
                            {
                                weaponDefense = 2;
                            }
                            else if (defenseChance > 75)
                            {
                                weaponDefense = 1;
                            }
                            else
                            {
                                weaponDefense = 0;
                            }
                            wo.WeaponDefense = 1.0 + (weaponDefense * .01);
                            ////Setting Weapon Defense Mod
                            int attackChance = rnd.Next(1, 101);
                            float attackDefense = 0;
                            if (attackChance > 98)
                            {
                                attackDefense = 3;
                            }
                            else if (attackChance > 90)
                            {
                                attackDefense = 2;
                            }
                            else if (attackChance > 75)
                            {
                                attackDefense = 1;
                            }
                            else
                            {
                                attackDefense = 0;
                            }
                            wo.WeaponOffense = 1.0 + (attackDefense * .01);
                            ////Sets how many gems and what type are in the description. May affect value later on.
                            wo.GemCount = rnd.Next(1, 6);
                            wo.GemType = rnd.Next(10, 51);
                            ////Setting Material Type
                            double materialValueMod;
                            int itemMaterial = rnd.Next(0, 7);
                            if (itemMaterial == 0)
                            {
                                wo.MaterialType = Material.Brass;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 1)
                            {
                                wo.MaterialType = Material.Bronze;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 2)
                            {
                                wo.MaterialType = Material.Copper;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 3)
                            {
                                wo.MaterialType = Material.Gold;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 4)
                            {
                                wo.MaterialType = Material.Iron;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 5)
                            {
                                wo.MaterialType = Material.Silver;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 6)
                            {
                                wo.MaterialType = Material.Steel;
                                materialValueMod = 1.0;
                            }
                            ////Affects the workmanship of an item
                            int workmanshipChance = rnd.Next(1, 101);
                            float workmanship = 0;
                            if (workmanshipChance > 95)
                            {
                                workmanship = 5;
                            }
                            else if (workmanshipChance > 80)
                            {
                                workmanship = 4;
                            }
                            else if (workmanshipChance > 50)
                            {
                                workmanship = 3;
                            }
                            else if (workmanshipChance > 20)
                            {
                                workmanship = 2;
                            }
                            else
                            {
                                workmanship = 1;
                            }
                            wo.Workmanship = (int)workmanship;
                            ////Value... Not sure what the range is, so just using arbitrary numbers based on workmanship
                            int itemValue = (int)workmanship * rnd.Next(10, 40);
                            wo.Value = itemValue;
                            items.Add(wo);
                        }
                        if (subType == 2)
                        {
                            ////There are 4 subtypes of maces
                            int subMaceType = rnd.Next(0, 4);
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
                            if (subMaceType == 2)
                            {
                                ////Morning Star
                                weaponWeenie = 332;
                            }
                            WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)weaponWeenie);
                            spells = LootHelper.MeleeSpells;
                            wo.PropertiesSpellId.Clear();
                            if (spellCount > 0)
                            {
                                wo.UiEffects = UiEffects.Magical;
                                int[] itemSpell = new int[spellCount];
                                List<int> nums = new List<int>();
                                for (int num = 0; num < spells.Length; num++)
                                {
                                    nums.Add(num);
                                }
                                for (int j = 0; j < spellCount; j++)
                                {
                                    itemSpell[j] = rnd.Next(0, nums.Count);
                                    int spellLevel;
                                    int spellLevelChance = rnd.Next(0, 100);
                                    if (spellLevelChance > 95)
                                    {
                                        spellLevel = 3;
                                    }
                                    else if (spellLevelChance > 70)
                                    {
                                        spellLevel = 2;
                                    }
                                    else
                                    {
                                        spellLevel = 1;
                                    }
                                    var propSpell = new AceObjectPropertiesSpell
                                    {
                                        AceObjectId = wo.Guid.Full,
                                        SpellId = (uint)spells[itemSpell[j]][spellLevel]
                                    };
                                    Console.WriteLine("Writing a spell to the list...." + propSpell.SpellId);
                                    wo.PropertiesSpellId.Add(propSpell);
                                    Console.WriteLine(wo.PropertiesSpellId);
                                    nums.RemoveAt(itemSpell[j]);
                                }
                            }
                            ////Set the damage, damagevariance, damagae attack, and damage defence
                            wo.DamageType = 4;
                            int damage = rnd.Next(8, 14);
                            wo.Damage = damage;
                            double damageVariance = .1 * rnd.Next(1, 11);
                            wo.DamageVariance = damageVariance;
                            wo.DamageMod = 1.0;
                            ////Sets the speed of the weapon
                            wo.WeaponTime = 40;
                            ////Setting Weapon Defense Mod
                            int defenseChance = rnd.Next(1, 101);
                            float weaponDefense = 0;
                            if (defenseChance > 98)
                            {
                                weaponDefense = 3;
                            }
                            else if (defenseChance > 90)
                            {
                                weaponDefense = 2;
                            }
                            else if (defenseChance > 75)
                            {
                                weaponDefense = 1;
                            }
                            else
                            {
                                weaponDefense = 0;
                            }
                            wo.WeaponDefense = 1.0 + (weaponDefense * .01);
                            ////Setting Weapon Defense Mod
                            int attackChance = rnd.Next(1, 101);
                            float attackDefense = 0;
                            if (attackChance > 98)
                            {
                                attackDefense = 3;
                            }
                            else if (attackChance > 90)
                            {
                                attackDefense = 2;
                            }
                            else if (attackChance > 75)
                            {
                                attackDefense = 1;
                            }
                            else
                            {
                                attackDefense = 0;
                            }
                            wo.WeaponOffense = 1.0 + (attackDefense * .01);
                            ////Sets how many gems and what type are in the description. May affect value later on.
                            wo.GemCount = rnd.Next(1, 6);
                            wo.GemType = rnd.Next(10, 51);
                            ////Setting Material Type
                            double materialValueMod;
                            int itemMaterial = rnd.Next(0, 7);
                            if (itemMaterial == 0)
                            {
                                wo.MaterialType = Material.Brass;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 1)
                            {
                                wo.MaterialType = Material.Bronze;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 2)
                            {
                                wo.MaterialType = Material.Copper;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 3)
                            {
                                wo.MaterialType = Material.Gold;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 4)
                            {
                                wo.MaterialType = Material.Iron;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 5)
                            {
                                wo.MaterialType = Material.Silver;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 6)
                            {
                                wo.MaterialType = Material.Steel;
                                materialValueMod = 1.0;
                            }
                            ////Affects the workmanship of an item
                            int workmanshipChance = rnd.Next(1, 101);
                            float workmanship = 0;
                            if (workmanshipChance > 95)
                            {
                                workmanship = 5;
                            }
                            else if (workmanshipChance > 80)
                            {
                                workmanship = 4;
                            }
                            else if (workmanshipChance > 50)
                            {
                                workmanship = 3;
                            }
                            else if (workmanshipChance > 20)
                            {
                                workmanship = 2;
                            }
                            else
                            {
                                workmanship = 1;
                            }
                            wo.Workmanship = (int)workmanship;
                            ////Value... Not sure what the range is, so just using arbitrary numbers based on workmanship
                            int itemValue = (int)workmanship * rnd.Next(10, 40);
                            wo.Value = itemValue;
                            items.Add(wo);
                        }
                        if (subType == 3)
                        {
                            ////There are three subtypes of spears
                            int subSpearType = rnd.Next(0, 3);
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
                            WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)weaponWeenie);
                            spells = LootHelper.MeleeSpells;
                            wo.PropertiesSpellId.Clear();
                            if (spellCount > 0)
                            {
                                wo.UiEffects = UiEffects.Magical;
                                int[] itemSpell = new int[spellCount];
                                List<int> nums = new List<int>();
                                for (int num = 0; num < spells.Length; num++)
                                {
                                    nums.Add(num);
                                }
                                for (int j = 0; j < spellCount; j++)
                                {
                                    itemSpell[j] = rnd.Next(0, nums.Count);
                                    int spellLevel;
                                    int spellLevelChance = rnd.Next(0, 100);
                                    if (spellLevelChance > 95)
                                    {
                                        spellLevel = 3;
                                    }
                                    else if (spellLevelChance > 70)
                                    {
                                        spellLevel = 2;
                                    }
                                    else
                                    {
                                        spellLevel = 1;
                                    }
                                    var propSpell = new AceObjectPropertiesSpell
                                    {
                                        AceObjectId = wo.Guid.Full,
                                        SpellId = (uint)spells[itemSpell[j]][spellLevel]
                                    };
                                    Console.WriteLine("Writing a spell to the list...." + propSpell.SpellId);
                                    wo.PropertiesSpellId.Add(propSpell);
                                    Console.WriteLine(wo.PropertiesSpellId);
                                    nums.RemoveAt(itemSpell[j]);
                                }
                            }
                            ////Set the damage, damagevariance, damagae attack, and damage defence
                            wo.DamageType = 2;
                            int damage = rnd.Next(6, 12);
                            wo.Damage = damage;
                            double damageVariance = .1 * rnd.Next(1, 11);
                            wo.DamageVariance = damageVariance;
                            wo.DamageMod = 1.0;
                            ////Sets the speed of the weapon
                            wo.WeaponTime = 50;
                            ////Setting Weapon Defense Mod
                            int defenseChance = rnd.Next(1, 101);
                            float weaponDefense = 0;
                            if (defenseChance > 98)
                            {
                                weaponDefense = 3;
                            }
                            else if (defenseChance > 90)
                            {
                                weaponDefense = 2;
                            }
                            else if (defenseChance > 75)
                            {
                                weaponDefense = 1;
                            }
                            else
                            {
                                weaponDefense = 0;
                            }
                            wo.WeaponDefense = 1.0 + (weaponDefense * .01);
                            ////Setting Weapon Defense Mod
                            int attackChance = rnd.Next(1, 101);
                            float attackDefense = 0;
                            if (attackChance > 98)
                            {
                                attackDefense = 3;
                            }
                            else if (attackChance > 90)
                            {
                                attackDefense = 2;
                            }
                            else if (attackChance > 75)
                            {
                                attackDefense = 1;
                            }
                            else
                            {
                                attackDefense = 0;
                            }
                            wo.WeaponOffense = 1.0 + (attackDefense * .01);
                            ////Sets how many gems and what type are in the description. May affect value later on.
                            wo.GemCount = rnd.Next(1, 6);
                            wo.GemType = rnd.Next(10, 51);
                            ////Setting Material Type
                            double materialValueMod;
                            int itemMaterial = rnd.Next(0, 7);
                            if (itemMaterial == 0)
                            {
                                wo.MaterialType = Material.Brass;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 1)
                            {
                                wo.MaterialType = Material.Bronze;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 2)
                            {
                                wo.MaterialType = Material.Copper;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 3)
                            {
                                wo.MaterialType = Material.Gold;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 4)
                            {
                                wo.MaterialType = Material.Iron;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 5)
                            {
                                wo.MaterialType = Material.Silver;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 6)
                            {
                                wo.MaterialType = Material.Steel;
                                materialValueMod = 1.0;
                            }
                            ////Affects the workmanship of an item
                            int workmanshipChance = rnd.Next(1, 101);
                            float workmanship = 0;
                            if (workmanshipChance > 95)
                            {
                                workmanship = 5;
                            }
                            else if (workmanshipChance > 80)
                            {
                                workmanship = 4;
                            }
                            else if (workmanshipChance > 50)
                            {
                                workmanship = 3;
                            }
                            else if (workmanshipChance > 20)
                            {
                                workmanship = 2;
                            }
                            else
                            {
                                workmanship = 1;
                            }
                            wo.Workmanship = (int)workmanship;
                            ////Value... Not sure what the range is, so just using arbitrary numbers based on workmanship
                            int itemValue = (int)workmanship * rnd.Next(10, 40);
                            wo.Value = itemValue;
                            items.Add(wo);
                        }
                        if (subType == 4)
                        {
                            ////There are two subtypes of staves
                            int subStaffType = rnd.Next(0, 2);
                            if (subStaffType == 0)
                            {
                                ////Nabut
                                weaponWeenie = 333;
                            }
                            if (subStaffType == 1)
                            {
                                ////Stick
                                weaponWeenie = 31788;
                            }
                            WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)weaponWeenie);
                            spells = LootHelper.MeleeSpells;
                            wo.PropertiesSpellId.Clear();
                            if (spellCount > 0)
                            {
                                wo.UiEffects = UiEffects.Magical;
                                int[] itemSpell = new int[spellCount];
                                List<int> nums = new List<int>();
                                for (int num = 0; num < spells.Length; num++)
                                {
                                    nums.Add(num);
                                }
                                for (int j = 0; j < spellCount; j++)
                                {
                                    itemSpell[j] = rnd.Next(0, nums.Count);
                                    int spellLevel;
                                    int spellLevelChance = rnd.Next(0, 100);
                                    if (spellLevelChance > 95)
                                    {
                                        spellLevel = 3;
                                    }
                                    else if (spellLevelChance > 70)
                                    {
                                        spellLevel = 2;
                                    }
                                    else
                                    {
                                        spellLevel = 1;
                                    }
                                    var propSpell = new AceObjectPropertiesSpell
                                    {
                                        AceObjectId = wo.Guid.Full,
                                        SpellId = (uint)spells[itemSpell[j]][spellLevel]
                                    };
                                    Console.WriteLine("Writing a spell to the list...." + propSpell.SpellId);
                                    wo.PropertiesSpellId.Add(propSpell);
                                    Console.WriteLine(wo.PropertiesSpellId);
                                    nums.RemoveAt(itemSpell[j]);
                                }
                            }
                            ////Set the damage, damagevariance, damagae attack, and damage defence
                            wo.DamageType = 4;
                            int damage = rnd.Next(6, 15);
                            wo.Damage = damage;
                            double damageVariance = .1 * rnd.Next(1, 11);
                            wo.DamageVariance = damageVariance;
                            wo.DamageMod = 1.0;
                            ////Sets the speed of the weapon
                            wo.WeaponTime = 45;
                            ////Setting Weapon Defense Mod
                            int defenseChance = rnd.Next(1, 101);
                            float weaponDefense = 0;
                            if (defenseChance > 98)
                            {
                                weaponDefense = 3;
                            }
                            else if (defenseChance > 90)
                            {
                                weaponDefense = 2;
                            }
                            else if (defenseChance > 75)
                            {
                                weaponDefense = 1;
                            }
                            else
                            {
                                weaponDefense = 0;
                            }
                            wo.WeaponDefense = 1.0 + (weaponDefense * .01);
                            ////Setting Weapon Defense Mod
                            int attackChance = rnd.Next(1, 101);
                            float attackDefense = 0;
                            if (attackChance > 98)
                            {
                                attackDefense = 3;
                            }
                            else if (attackChance > 90)
                            {
                                attackDefense = 2;
                            }
                            else if (attackChance > 75)
                            {
                                attackDefense = 1;
                            }
                            else
                            {
                                attackDefense = 0;
                            }
                            wo.WeaponOffense = 1.0 + (attackDefense * .01);
                            ////Sets how many gems and what type are in the description. May affect value later on.
                            wo.GemCount = rnd.Next(1, 6);
                            wo.GemType = rnd.Next(10, 51);
                            ////Setting Material Type
                            int itemMaterial = rnd.Next(0, 8);
                            if (itemMaterial == 0)
                            {
                                wo.MaterialType = Material.Ebony;
                            }
                            if (itemMaterial == 1)
                            {
                                wo.MaterialType = Material.Mahogany;
                            }
                            if (itemMaterial == 2)
                            {
                                wo.MaterialType = Material.Oak;
                            }
                            if (itemMaterial == 3)
                            {
                                wo.MaterialType = Material.Pine;
                            }
                            if (itemMaterial == 4)
                            {
                                wo.MaterialType = Material.Teak;
                            }
                            ////Affects the workmanship of an item
                            int workmanshipChance = rnd.Next(1, 101);
                            float workmanship = 0;
                            if (workmanshipChance > 95)
                            {
                                workmanship = 5;
                            }
                            else if (workmanshipChance > 80)
                            {
                                workmanship = 4;
                            }
                            else if (workmanshipChance > 50)
                            {
                                workmanship = 3;
                            }
                            else if (workmanshipChance > 20)
                            {
                                workmanship = 2;
                            }
                            else
                            {
                                workmanship = 1;
                            }
                            wo.Workmanship = (int)workmanship;
                            ////Value... Not sure what the range is, so just using arbitrary numbers based on workmanship
                            int itemValue = (int)workmanship * rnd.Next(10, 40);
                            wo.Value = itemValue;
                            items.Add(wo);
                        }
                        if (subType == 5)
                        {
                            ////There are six subtypes of swords
                            int subSwordType = rnd.Next(0, 6);
                            if (subSwordType == 0)
                            {
                                ////Flamberge
                                weaponWeenie = 30576;
                            }
                            if (subSwordType == 1)
                            {
                                ////Ken
                                weaponWeenie = 372;
                            }
                            if (subSwordType == 2)
                            {
                                ////Long Sword
                                weaponWeenie = 351;
                            }
                            if (subSwordType == 3)
                            {
                                ////Tachi
                                weaponWeenie = 353;
                            }
                            if (subSwordType == 4)
                            {
                                ////Takuba
                                weaponWeenie = 354;
                            }
                            if (subSwordType == 5)
                            {
                                ////Schlager
                                weaponWeenie = 45108;
                            }
                            WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)weaponWeenie);
                            spells = LootHelper.MeleeSpells;
                            wo.PropertiesSpellId.Clear();
                            if (spellCount > 0)
                            {
                                wo.UiEffects = UiEffects.Magical;
                                int[] itemSpell = new int[spellCount];
                                List<int> nums = new List<int>();
                                for (int num = 0; num < spells.Length; num++)
                                {
                                    nums.Add(num);
                                }
                                for (int j = 0; j < spellCount; j++)
                                {
                                    itemSpell[j] = rnd.Next(0, nums.Count);
                                    int spellLevel;
                                    int spellLevelChance = rnd.Next(0, 100);
                                    if (spellLevelChance > 95)
                                    {
                                        spellLevel = 3;
                                    }
                                    else if (spellLevelChance > 70)
                                    {
                                        spellLevel = 2;
                                    }
                                    else
                                    {
                                        spellLevel = 1;
                                    }
                                    var propSpell = new AceObjectPropertiesSpell
                                    {
                                        AceObjectId = wo.Guid.Full,
                                        SpellId = (uint)spells[itemSpell[j]][spellLevel]
                                    };
                                    Console.WriteLine("Writing a spell to the list...." + propSpell.SpellId);
                                    wo.PropertiesSpellId.Add(propSpell);
                                    Console.WriteLine(wo.PropertiesSpellId);
                                    nums.RemoveAt(itemSpell[j]);
                                }
                            }
                            ////Set the damage, damagevariance, damagae attack, and damage defence
                            wo.DamageType = 3;
                            int damage = rnd.Next(6, 12);
                            wo.Damage = damage;
                            double damageVariance = .1 * rnd.Next(1, 11);
                            wo.DamageVariance = damageVariance;
                            wo.DamageMod = 1.0;
                            ////Sets the speed of the weapon
                            wo.WeaponTime = 37;
                            ////Setting Weapon Defense Mod
                            int defenseChance = rnd.Next(1, 101);
                            float weaponDefense = 0;
                            if (defenseChance > 98)
                            {
                                weaponDefense = 3;
                            }
                            else if (defenseChance > 90)
                            {
                                weaponDefense = 2;
                            }
                            else if (defenseChance > 75)
                            {
                                weaponDefense = 1;
                            }
                            else
                            {
                                weaponDefense = 0;
                            }
                            wo.WeaponDefense = 1.0 + (weaponDefense * .01);
                            ////Setting Weapon Defense Mod
                            int attackChance = rnd.Next(1, 101);
                            float attackDefense = 0;
                            if (attackChance > 98)
                            {
                                attackDefense = 3;
                            }
                            else if (attackChance > 90)
                            {
                                attackDefense = 2;
                            }
                            else if (attackChance > 75)
                            {
                                attackDefense = 1;
                            }
                            else
                            {
                                attackDefense = 0;
                            }
                            wo.WeaponOffense = 1.0 + (attackDefense * .01);
                            ////Sets how many gems and what type are in the description. May affect value later on.
                            wo.GemCount = rnd.Next(1, 6);
                            wo.GemType = rnd.Next(10, 51);
                            ////Setting Material Type
                            double materialValueMod;
                            int itemMaterial = rnd.Next(0, 7);
                            if (itemMaterial == 0)
                            {
                                wo.MaterialType = Material.Brass;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 1)
                            {
                                wo.MaterialType = Material.Bronze;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 2)
                            {
                                wo.MaterialType = Material.Copper;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 3)
                            {
                                wo.MaterialType = Material.Gold;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 4)
                            {
                                wo.MaterialType = Material.Iron;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 5)
                            {
                                wo.MaterialType = Material.Silver;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 6)
                            {
                                wo.MaterialType = Material.Steel;
                                materialValueMod = 1.0;
                            }
                            ////Affects the workmanship of an item
                            int workmanshipChance = rnd.Next(1, 101);
                            float workmanship = 0;
                            if (workmanshipChance > 95)
                            {
                                workmanship = 5;
                            }
                            else if (workmanshipChance > 80)
                            {
                                workmanship = 4;
                            }
                            else if (workmanshipChance > 50)
                            {
                                workmanship = 3;
                            }
                            else if (workmanshipChance > 20)
                            {
                                workmanship = 2;
                            }
                            else
                            {
                                workmanship = 1;
                            }
                            wo.Workmanship = (int)workmanship;
                            ////Value... Not sure what the range is, so just using arbitrary numbers based on workmanship
                            int itemValue = (int)workmanship * rnd.Next(10, 40);
                            wo.Value = itemValue;
                            items.Add(wo);
                        }
                        if (subType == 6)
                        {
                            int damageType = 0;
                            ////There are 2 subtypes of UA
                            int subUAType = rnd.Next(0, 2);
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
                            WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)weaponWeenie);
                            spells = LootHelper.MeleeSpells;
                            wo.PropertiesSpellId.Clear();
                            wo.DamageType = damageType;
                            if (spellCount > 0)
                            {
                                wo.UiEffects = UiEffects.Magical;
                                int[] itemSpell = new int[spellCount];
                                List<int> nums = new List<int>();
                                for (int num = 0; num < spells.Length; num++)
                                {
                                    nums.Add(num);
                                }
                                for (int j = 0; j < spellCount; j++)
                                {
                                    itemSpell[j] = rnd.Next(0, nums.Count);
                                    int spellLevel;
                                    int spellLevelChance = rnd.Next(0, 100);
                                    if (spellLevelChance > 95)
                                    {
                                        spellLevel = 3;
                                    }
                                    else if (spellLevelChance > 70)
                                    {
                                        spellLevel = 2;
                                    }
                                    else
                                    {
                                        spellLevel = 1;
                                    }
                                    var propSpell = new AceObjectPropertiesSpell
                                    {
                                        AceObjectId = wo.Guid.Full,
                                        SpellId = (uint)spells[itemSpell[j]][spellLevel]
                                    };
                                    Console.WriteLine("Writing a spell to the list...." + propSpell.SpellId);
                                    wo.PropertiesSpellId.Add(propSpell);
                                    Console.WriteLine(wo.PropertiesSpellId);
                                    nums.RemoveAt(itemSpell[j]);
                                }
                            }
                            ////Set the damage, damagevariance, damagae attack, and damage defence
                            int damage = rnd.Next(2, 6);
                            wo.Damage = damage;
                            double damageVariance = .1 * rnd.Next(1, 11);
                            wo.DamageVariance = damageVariance;
                            wo.DamageMod = 1.0;
                            ////Sets the speed of the weapon
                            wo.WeaponTime = 16;
                            ////Setting Weapon Defense Mod
                            int defenseChance = rnd.Next(1, 101);
                            float weaponDefense = 0;
                            if (defenseChance > 98)
                            {
                                weaponDefense = 3;
                            }
                            else if (defenseChance > 90)
                            {
                                weaponDefense = 2;
                            }
                            else if (defenseChance > 75)
                            {
                                weaponDefense = 1;
                            }
                            else
                            {
                                weaponDefense = 0;
                            }
                            wo.WeaponDefense = 1.0 + (weaponDefense * .01);
                            ////Setting Weapon Defense Mod
                            int attackChance = rnd.Next(1, 101);
                            float attackDefense = 0;
                            if (attackChance > 98)
                            {
                                attackDefense = 3;
                            }
                            else if (attackChance > 90)
                            {
                                attackDefense = 2;
                            }
                            else if (attackChance > 75)
                            {
                                attackDefense = 1;
                            }
                            else
                            {
                                attackDefense = 0;
                            }
                            wo.WeaponOffense = 1.0 + (attackDefense * .01);
                            ////Sets how many gems and what type are in the description. May affect value later on.
                            wo.GemCount = rnd.Next(1, 6);
                            wo.GemType = rnd.Next(10, 51);
                            ////Setting Material Type
                            double materialValueMod;
                            int itemMaterial = rnd.Next(0, 7);
                            if (itemMaterial == 0)
                            {
                                wo.MaterialType = Material.Brass;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 1)
                            {
                                wo.MaterialType = Material.Bronze;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 2)
                            {
                                wo.MaterialType = Material.Copper;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 3)
                            {
                                wo.MaterialType = Material.Gold;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 4)
                            {
                                wo.MaterialType = Material.Iron;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 5)
                            {
                                wo.MaterialType = Material.Silver;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 6)
                            {
                                wo.MaterialType = Material.Steel;
                                materialValueMod = 1.0;
                            }
                            ////Affects the workmanship of an item
                            int workmanshipChance = rnd.Next(1, 101);
                            float workmanship = 0;
                            if (workmanshipChance > 95)
                            {
                                workmanship = 5;
                            }
                            else if (workmanshipChance > 80)
                            {
                                workmanship = 4;
                            }
                            else if (workmanshipChance > 50)
                            {
                                workmanship = 3;
                            }
                            else if (workmanshipChance > 20)
                            {
                                workmanship = 2;
                            }
                            else
                            {
                                workmanship = 1;
                            }
                            wo.Workmanship = (int)workmanship;
                            ////Value... Not sure what the range is, so just using arbitrary numbers based on workmanship
                            int itemValue = (int)workmanship * rnd.Next(10, 40);
                            wo.Value = itemValue;
                            items.Add(wo);
                        }
                    }
                    if (weaponType == 1)
                    {
                        ////Light Weapons, of which there are 7 sub types
                        int subType = rnd.Next(0, 7);
                        if (subType == 0)
                        {
                            int subAxeType = rnd.Next(0, 4);
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
                            if (subAxeType == 1)
                            {
                                ////Ono
                                weaponWeenie = 336;
                            }
                            if (subAxeType == 3)
                            {
                                ////War Hammer
                                weaponWeenie = 359;
                            }
                            ////Set the damage, damagevariance, damagae attack, and damage defence
                            WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)weaponWeenie);
                            spells = LootHelper.MeleeSpells;
                            wo.PropertiesSpellId.Clear();
                            if (spellCount > 0)
                            {
                                wo.UiEffects = UiEffects.Magical;
                                int[] itemSpell = new int[spellCount];
                                List<int> nums = new List<int>();
                                for (int num = 0; num < spells.Length; num++)
                                {
                                    nums.Add(num);
                                }
                                for (int j = 0; j < spellCount; j++)
                                {
                                    itemSpell[j] = rnd.Next(0, nums.Count);
                                    int spellLevel;
                                    int spellLevelChance = rnd.Next(0, 100);
                                    if (spellLevelChance > 95)
                                    {
                                        spellLevel = 3;
                                    }
                                    else if (spellLevelChance > 70)
                                    {
                                        spellLevel = 2;
                                    }
                                    else
                                    {
                                        spellLevel = 1;
                                    }
                                    var propSpell = new AceObjectPropertiesSpell
                                    {
                                        AceObjectId = wo.Guid.Full,
                                        SpellId = (uint)spells[itemSpell[j]][spellLevel]
                                    };
                                    Console.WriteLine("Writing a spell to the list...." + propSpell.SpellId);
                                    wo.PropertiesSpellId.Add(propSpell);
                                    Console.WriteLine(wo.PropertiesSpellId);
                                    nums.RemoveAt(itemSpell[j]);
                                }
                            }
                            if (subAxeType == 3)
                            {
                                ////War Hammer
                                wo.DamageType = 4;
                            }
                            else
                            {
                                wo.DamageType = 1;
                            }
                            int damage = rnd.Next(6, 12);
                            wo.Damage = damage;
                            double damageVariance = .1 * rnd.Next(1, 11);
                            wo.DamageVariance = damageVariance;
                            wo.DamageMod = 1.0;
                            ////Sets the speed of the weapon
                            wo.WeaponTime = 50;
                            ////Setting Weapon Defense Mod
                            int defenseChance = rnd.Next(1, 101);
                            float weaponDefense = 0;
                            if (defenseChance > 98)
                            {
                                weaponDefense = 3;
                            }
                            else if (defenseChance > 90)
                            {
                                weaponDefense = 2;
                            }
                            else if (defenseChance > 75)
                            {
                                weaponDefense = 1;
                            }
                            else
                            {
                                weaponDefense = 0;
                            }
                            wo.WeaponDefense = 1.0 + (weaponDefense * .01);
                            ////Setting Weapon Defense Mod
                            int attackChance = rnd.Next(1, 101);
                            float attackDefense = 0;
                            if (attackChance > 98)
                            {
                                attackDefense = 3;
                            }
                            else if (attackChance > 90)
                            {
                                attackDefense = 2;
                            }
                            else if (attackChance > 75)
                            {
                                attackDefense = 1;
                            }
                            else
                            {
                                attackDefense = 0;
                            }
                            wo.WeaponOffense = 1.0 + (attackDefense * .01);
                            ////Sets how many gems and what type are in the description. May affect value later on.
                            wo.GemCount = rnd.Next(1, 6);
                            wo.GemType = rnd.Next(10, 51);
                            ////Setting Material Type
                            double materialValueMod;
                            int itemMaterial = rnd.Next(0, 7);
                            if (itemMaterial == 0)
                            {
                                wo.MaterialType = Material.Brass;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 1)
                            {
                                wo.MaterialType = Material.Bronze;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 2)
                            {
                                wo.MaterialType = Material.Copper;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 3)
                            {
                                wo.MaterialType = Material.Gold;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 4)
                            {
                                wo.MaterialType = Material.Iron;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 5)
                            {
                                wo.MaterialType = Material.Silver;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 6)
                            {
                                wo.MaterialType = Material.Steel;
                                materialValueMod = 1.0;
                            }
                            ////Affects the workmanship of an item
                            int workmanshipChance = rnd.Next(1, 101);
                            float workmanship = 0;
                            if (workmanshipChance > 95)
                            {
                                workmanship = 5;
                            }
                            else if (workmanshipChance > 80)
                            {
                                workmanship = 4;
                            }
                            else if (workmanshipChance > 50)
                            {
                                workmanship = 3;
                            }
                            else if (workmanshipChance > 20)
                            {
                                workmanship = 2;
                            }
                            else
                            {
                                workmanship = 1;
                            }
                            wo.Workmanship = (int)workmanship;
                            ////Value... Not sure what the range is, so just using arbitrary numbers based on workmanship
                            int itemValue = (int)workmanship * rnd.Next(10, 40);
                            wo.Value = itemValue;
                            items.Add(wo);
                        }
                        if (subType == 1)
                        {
                            ////There are 2 subtypes of daggers
                            int subDaggerType = rnd.Next(0, 2);
                            if (subDaggerType == 0)
                            {
                                ////Dagger
                                weaponWeenie = 314;
                            }
                            if (subDaggerType == 1)
                            {
                                ////Khanjar
                                weaponWeenie = 328;
                            }
                            WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)weaponWeenie);
                            spells = LootHelper.MeleeSpells;
                            wo.PropertiesSpellId.Clear();
                            if (spellCount > 0)
                            {
                                wo.UiEffects = UiEffects.Magical;
                                int[] itemSpell = new int[spellCount];
                                List<int> nums = new List<int>();
                                for (int num = 0; num < spells.Length; num++)
                                {
                                    nums.Add(num);
                                }
                                for (int j = 0; j < spellCount; j++)
                                {
                                    itemSpell[j] = rnd.Next(0, nums.Count);
                                    int spellLevel;
                                    int spellLevelChance = rnd.Next(0, 100);
                                    if (spellLevelChance > 95)
                                    {
                                        spellLevel = 3;
                                    }
                                    else if (spellLevelChance > 70)
                                    {
                                        spellLevel = 2;
                                    }
                                    else
                                    {
                                        spellLevel = 1;
                                    }
                                    var propSpell = new AceObjectPropertiesSpell
                                    {
                                        AceObjectId = wo.Guid.Full,
                                        SpellId = (uint)spells[itemSpell[j]][spellLevel]
                                    };
                                    Console.WriteLine("Writing a spell to the list...." + propSpell.SpellId);
                                    wo.PropertiesSpellId.Add(propSpell);
                                    Console.WriteLine(wo.PropertiesSpellId);
                                    nums.RemoveAt(itemSpell[j]);
                                }
                            }
                            ////Set the damage, damagevariance, damagae attack, and damage defence
                            wo.DamageType = 3;
                            int damage = rnd.Next(1, 4);
                            wo.Damage = damage;
                            double damageVariance = .1 * rnd.Next(1, 11);
                            wo.DamageVariance = damageVariance;
                            wo.DamageMod = 1.0;
                            ////Sets the speed of the weapon
                            wo.WeaponTime = 10;
                            ////Setting Weapon Defense Mod
                            int defenseChance = rnd.Next(1, 101);
                            float weaponDefense = 0;
                            if (defenseChance > 98)
                            {
                                weaponDefense = 3;
                            }
                            else if (defenseChance > 90)
                            {
                                weaponDefense = 2;
                            }
                            else if (defenseChance > 75)
                            {
                                weaponDefense = 1;
                            }
                            else
                            {
                                weaponDefense = 0;
                            }
                            wo.WeaponDefense = 1.0 + (weaponDefense * .01);
                            ////Setting Weapon Defense Mod
                            int attackChance = rnd.Next(1, 101);
                            float attackDefense = 0;
                            if (attackChance > 98)
                            {
                                attackDefense = 3;
                            }
                            else if (attackChance > 90)
                            {
                                attackDefense = 2;
                            }
                            else if (attackChance > 75)
                            {
                                attackDefense = 1;
                            }
                            else
                            {
                                attackDefense = 0;
                            }
                            wo.WeaponOffense = 1.0 + (attackDefense * .01);
                            ////Sets how many gems and what type are in the description. May affect value later on.
                            wo.GemCount = rnd.Next(1, 6);
                            wo.GemType = rnd.Next(10, 51);
                            ////Setting Material Type
                            double materialValueMod;
                            int itemMaterial = rnd.Next(0, 7);
                            if (itemMaterial == 0)
                            {
                                wo.MaterialType = Material.Brass;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 1)
                            {
                                wo.MaterialType = Material.Bronze;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 2)
                            {
                                wo.MaterialType = Material.Copper;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 3)
                            {
                                wo.MaterialType = Material.Gold;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 4)
                            {
                                wo.MaterialType = Material.Iron;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 5)
                            {
                                wo.MaterialType = Material.Silver;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 6)
                            {
                                wo.MaterialType = Material.Steel;
                                materialValueMod = 1.0;
                            }
                            ////Affects the workmanship of an item
                            int workmanshipChance = rnd.Next(1, 101);
                            float workmanship = 0;
                            if (workmanshipChance > 95)
                            {
                                workmanship = 5;
                            }
                            else if (workmanshipChance > 80)
                            {
                                workmanship = 4;
                            }
                            else if (workmanshipChance > 50)
                            {
                                workmanship = 3;
                            }
                            else if (workmanshipChance > 20)
                            {
                                workmanship = 2;
                            }
                            else
                            {
                                workmanship = 1;
                            }
                            wo.Workmanship = (int)workmanship;
                            ////Value... Not sure what the range is, so just using arbitrary numbers based on workmanship
                            int itemValue = (int)workmanship * rnd.Next(10, 40);
                            wo.Value = itemValue;
                            items.Add(wo);
                        }
                        if (subType == 2)
                        {
                            ////There are 3 subtypes of maces
                            int subMaceType = rnd.Next(0, 3);
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
                            WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)weaponWeenie);
                            spells = LootHelper.MeleeSpells;
                            wo.PropertiesSpellId.Clear();
                            if (spellCount > 0)
                            {
                                wo.UiEffects = UiEffects.Magical;
                                int[] itemSpell = new int[spellCount];
                                List<int> nums = new List<int>();
                                for (int num = 0; num < spells.Length; num++)
                                {
                                    nums.Add(num);
                                }
                                for (int j = 0; j < spellCount; j++)
                                {
                                    itemSpell[j] = rnd.Next(0, nums.Count);
                                    int spellLevel;
                                    int spellLevelChance = rnd.Next(0, 100);
                                    if (spellLevelChance > 95)
                                    {
                                        spellLevel = 3;
                                    }
                                    else if (spellLevelChance > 70)
                                    {
                                        spellLevel = 2;
                                    }
                                    else
                                    {
                                        spellLevel = 1;
                                    }
                                    var propSpell = new AceObjectPropertiesSpell
                                    {
                                        AceObjectId = wo.Guid.Full,
                                        SpellId = (uint)spells[itemSpell[j]][spellLevel]
                                    };
                                    Console.WriteLine("Writing a spell to the list...." + propSpell.SpellId);
                                    wo.PropertiesSpellId.Add(propSpell);
                                    Console.WriteLine(wo.PropertiesSpellId);
                                    nums.RemoveAt(itemSpell[j]);
                                }
                            }
                            ////Set the damage, damagevariance, damagae attack, and damage defence
                            if (subMaceType == 2)
                            {
                                ////Spiked Club
                                wo.DamageType = 2;
                            }
                            else
                            {
                                wo.DamageType = 4;
                            }
                            int damage = rnd.Next(8, 14);
                            wo.Damage = damage;
                            double damageVariance = .1 * rnd.Next(1, 11);
                            wo.DamageVariance = damageVariance;
                            wo.DamageMod = 1.0;
                            ////Sets the speed of the weapon
                            wo.WeaponTime = 40;
                            ////Setting Weapon Defense Mod
                            int defenseChance = rnd.Next(1, 101);
                            float weaponDefense = 0;
                            if (defenseChance > 98)
                            {
                                weaponDefense = 3;
                            }
                            else if (defenseChance > 90)
                            {
                                weaponDefense = 2;
                            }
                            else if (defenseChance > 75)
                            {
                                weaponDefense = 1;
                            }
                            else
                            {
                                weaponDefense = 0;
                            }
                            wo.WeaponDefense = 1.0 + (weaponDefense * .01);
                            ////Setting Weapon Defense Mod
                            int attackChance = rnd.Next(1, 101);
                            float attackDefense = 0;
                            if (attackChance > 98)
                            {
                                attackDefense = 3;
                            }
                            else if (attackChance > 90)
                            {
                                attackDefense = 2;
                            }
                            else if (attackChance > 75)
                            {
                                attackDefense = 1;
                            }
                            else
                            {
                                attackDefense = 0;
                            }
                            wo.WeaponOffense = 1.0 + (attackDefense * .01);
                            ////Sets how many gems and what type are in the description. May affect value later on.
                            wo.GemCount = rnd.Next(1, 6);
                            wo.GemType = rnd.Next(10, 51);
                            ////Setting Material Type
                            int itemMaterial = rnd.Next(0, 8);
                            if (itemMaterial == 0)
                            {
                                wo.MaterialType = Material.Ebony;
                            }
                            if (itemMaterial == 1)
                            {
                                wo.MaterialType = Material.Mahogany;
                            }
                            if (itemMaterial == 2)
                            {
                                wo.MaterialType = Material.Oak;
                            }
                            if (itemMaterial == 3)
                            {
                                wo.MaterialType = Material.Pine;
                            }
                            if (itemMaterial == 4)
                            {
                                wo.MaterialType = Material.Teak;
                            }
                            ////Affects the workmanship of an item
                            int workmanshipChance = rnd.Next(1, 101);
                            float workmanship = 0;
                            if (workmanshipChance > 95)
                            {
                                workmanship = 5;
                            }
                            else if (workmanshipChance > 80)
                            {
                                workmanship = 4;
                            }
                            else if (workmanshipChance > 50)
                            {
                                workmanship = 3;
                            }
                            else if (workmanshipChance > 20)
                            {
                                workmanship = 2;
                            }
                            else
                            {
                                workmanship = 1;
                            }
                            wo.Workmanship = (int)workmanship;
                            ////Value... Not sure what the range is, so just using arbitrary numbers based on workmanship
                            int itemValue = (int)workmanship * rnd.Next(10, 40);
                            wo.Value = itemValue;
                            items.Add(wo);
                        }
                        if (subType == 3)
                        {
                            ////There are 2 subtypes of spears
                            int subSpearType = rnd.Next(0, 2);
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
                            WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)weaponWeenie);
                            spells = LootHelper.MeleeSpells;
                            wo.PropertiesSpellId.Clear();
                            if (spellCount > 0)
                            {
                                wo.UiEffects = UiEffects.Magical;
                                int[] itemSpell = new int[spellCount];
                                List<int> nums = new List<int>();
                                for (int num = 0; num < spells.Length; num++)
                                {
                                    nums.Add(num);
                                }
                                for (int j = 0; j < spellCount; j++)
                                {
                                    itemSpell[j] = rnd.Next(0, nums.Count);
                                    int spellLevel;
                                    int spellLevelChance = rnd.Next(0, 100);
                                    if (spellLevelChance > 95)
                                    {
                                        spellLevel = 3;
                                    }
                                    else if (spellLevelChance > 70)
                                    {
                                        spellLevel = 2;
                                    }
                                    else
                                    {
                                        spellLevel = 1;
                                    }
                                    var propSpell = new AceObjectPropertiesSpell
                                    {
                                        AceObjectId = wo.Guid.Full,
                                        SpellId = (uint)spells[itemSpell[j]][spellLevel]
                                    };
                                    Console.WriteLine("Writing a spell to the list...." + propSpell.SpellId);
                                    wo.PropertiesSpellId.Add(propSpell);
                                    Console.WriteLine(wo.PropertiesSpellId);
                                    nums.RemoveAt(itemSpell[j]);
                                }
                            }
                            ////Set the damage, damagevariance, damagae attack, and damage defence
                            wo.DamageType = 2;
                            int damage = rnd.Next(6, 12);
                            wo.Damage = damage;
                            double damageVariance = .1 * rnd.Next(1, 11);
                            wo.DamageVariance = damageVariance;
                            wo.DamageMod = 1.0;
                            ////Sets the speed of the weapon
                            wo.WeaponTime = 50;
                            ////Setting Weapon Defense Mod
                            int defenseChance = rnd.Next(1, 101);
                            float weaponDefense = 0;
                            if (defenseChance > 98)
                            {
                                weaponDefense = 3;
                            }
                            else if (defenseChance > 90)
                            {
                                weaponDefense = 2;
                            }
                            else if (defenseChance > 75)
                            {
                                weaponDefense = 1;
                            }
                            else
                            {
                                weaponDefense = 0;
                            }
                            wo.WeaponDefense = 1.0 + (weaponDefense * .01);
                            ////Setting Weapon Defense Mod
                            int attackChance = rnd.Next(1, 101);
                            float attackDefense = 0;
                            if (attackChance > 98)
                            {
                                attackDefense = 3;
                            }
                            else if (attackChance > 90)
                            {
                                attackDefense = 2;
                            }
                            else if (attackChance > 75)
                            {
                                attackDefense = 1;
                            }
                            else
                            {
                                attackDefense = 0;
                            }
                            wo.WeaponOffense = 1.0 + (attackDefense * .01);
                            ////Sets how many gems and what type are in the description. May affect value later on.
                            wo.GemCount = rnd.Next(1, 6);
                            wo.GemType = rnd.Next(10, 51);
                            ////Setting Material Type
                            double materialValueMod;
                            int itemMaterial = rnd.Next(0, 7);
                            if (itemMaterial == 0)
                            {
                                wo.MaterialType = Material.Brass;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 1)
                            {
                                wo.MaterialType = Material.Bronze;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 2)
                            {
                                wo.MaterialType = Material.Copper;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 3)
                            {
                                wo.MaterialType = Material.Gold;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 4)
                            {
                                wo.MaterialType = Material.Iron;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 5)
                            {
                                wo.MaterialType = Material.Silver;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 6)
                            {
                                wo.MaterialType = Material.Steel;
                                materialValueMod = 1.0;
                            }
                            ////Affects the workmanship of an item
                            int workmanshipChance = rnd.Next(1, 101);
                            float workmanship = 0;
                            if (workmanshipChance > 95)
                            {
                                workmanship = 5;
                            }
                            else if (workmanshipChance > 80)
                            {
                                workmanship = 4;
                            }
                            else if (workmanshipChance > 50)
                            {
                                workmanship = 3;
                            }
                            else if (workmanshipChance > 20)
                            {
                                workmanship = 2;
                            }
                            else
                            {
                                workmanship = 1;
                            }
                            wo.Workmanship = (int)workmanship;
                            ////Value... Not sure what the range is, so just using arbitrary numbers based on workmanship
                            int itemValue = (int)workmanship * rnd.Next(10, 40);
                            wo.Value = itemValue;
                            items.Add(wo);
                        }
                        if (subType == 4)
                        {
                            ////There is 1 subtypes of staves
                            int subStaffType = rnd.Next(0, 1);
                            if (subStaffType == 0)
                            {
                                ////Quarter Staff
                                weaponWeenie = 338;
                            }
                            WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)weaponWeenie);
                            spells = LootHelper.MeleeSpells;
                            wo.PropertiesSpellId.Clear();
                            if (spellCount > 0)
                            {
                                wo.UiEffects = UiEffects.Magical;
                                int[] itemSpell = new int[spellCount];
                                List<int> nums = new List<int>();
                                for (int num = 0; num < spells.Length; num++)
                                {
                                    nums.Add(num);
                                }
                                for (int j = 0; j < spellCount; j++)
                                {
                                    itemSpell[j] = rnd.Next(0, nums.Count);
                                    int spellLevel;
                                    int spellLevelChance = rnd.Next(0, 100);
                                    if (spellLevelChance > 95)
                                    {
                                        spellLevel = 3;
                                    }
                                    else if (spellLevelChance > 70)
                                    {
                                        spellLevel = 2;
                                    }
                                    else
                                    {
                                        spellLevel = 1;
                                    }
                                    var propSpell = new AceObjectPropertiesSpell
                                    {
                                        AceObjectId = wo.Guid.Full,
                                        SpellId = (uint)spells[itemSpell[j]][spellLevel]
                                    };
                                    Console.WriteLine("Writing a spell to the list...." + propSpell.SpellId);
                                    wo.PropertiesSpellId.Add(propSpell);
                                    Console.WriteLine(wo.PropertiesSpellId);
                                    nums.RemoveAt(itemSpell[j]);
                                }
                            }
                            ////Set the damage, damagevariance, damagae attack, and damage defence
                            wo.DamageType = 4;
                            int damage = rnd.Next(6, 15);
                            wo.Damage = damage;
                            double damageVariance = .1 * rnd.Next(1, 11);
                            wo.DamageVariance = damageVariance;
                            wo.DamageMod = 1.0;
                            ////Sets the speed of the weapon
                            wo.WeaponTime = 45;
                            ////Setting Weapon Defense Mod
                            int defenseChance = rnd.Next(1, 101);
                            float weaponDefense = 0;
                            if (defenseChance > 98)
                            {
                                weaponDefense = 3;
                            }
                            else if (defenseChance > 90)
                            {
                                weaponDefense = 2;
                            }
                            else if (defenseChance > 75)
                            {
                                weaponDefense = 1;
                            }
                            else
                            {
                                weaponDefense = 0;
                            }
                            wo.WeaponDefense = 1.0 + (weaponDefense * .01);
                            ////Setting Weapon Defense Mod
                            int attackChance = rnd.Next(1, 101);
                            float attackDefense = 0;
                            if (attackChance > 98)
                            {
                                attackDefense = 3;
                            }
                            else if (attackChance > 90)
                            {
                                attackDefense = 2;
                            }
                            else if (attackChance > 75)
                            {
                                attackDefense = 1;
                            }
                            else
                            {
                                attackDefense = 0;
                            }
                            wo.WeaponOffense = 1.0 + (attackDefense * .01);
                            ////Sets how many gems and what type are in the description. May affect value later on.
                            wo.GemCount = rnd.Next(1, 6);
                            wo.GemType = rnd.Next(10, 51);
                            ////Setting Material Type
                            int itemMaterial = rnd.Next(0, 8);
                            if (itemMaterial == 0)
                            {
                                wo.MaterialType = Material.Ebony;
                            }
                            if (itemMaterial == 1)
                            {
                                wo.MaterialType = Material.Mahogany;
                            }
                            if (itemMaterial == 2)
                            {
                                wo.MaterialType = Material.Oak;
                            }
                            if (itemMaterial == 3)
                            {
                                wo.MaterialType = Material.Pine;
                            }
                            if (itemMaterial == 4)
                            {
                                wo.MaterialType = Material.Teak;
                            }
                            ////Affects the workmanship of an item
                            int workmanshipChance = rnd.Next(1, 101);
                            float workmanship = 0;
                            if (workmanshipChance > 95)
                            {
                                workmanship = 5;
                            }
                            else if (workmanshipChance > 80)
                            {
                                workmanship = 4;
                            }
                            else if (workmanshipChance > 50)
                            {
                                workmanship = 3;
                            }
                            else if (workmanshipChance > 20)
                            {
                                workmanship = 2;
                            }
                            else
                            {
                                workmanship = 1;
                            }
                            wo.Workmanship = (int)workmanship;
                            ////Value... Not sure what the range is, so just using arbitrary numbers based on workmanship
                            int itemValue = (int)workmanship * rnd.Next(10, 40);
                            wo.Value = itemValue;
                            items.Add(wo);
                        }
                        if (subType == 5)
                        {
                            ////There are 6 subtypes of swords
                            int subSwordType = rnd.Next(0, 5);
                            if (subSwordType == 0)
                            {
                                ////Broad Sword
                                weaponWeenie = 350;
                            }
                            if (subSwordType == 1)
                            {
                                ////Dericost Blade
                                weaponWeenie = 40910;
                            }
                            if (subSwordType == 2)
                            {
                                ////Epee
                                weaponWeenie = 45099;
                            }
                            if (subSwordType == 3)
                            {
                                ////Kaskara
                                weaponWeenie = 324;
                            }
                            if (subSwordType == 4)
                            {
                                ////Spada
                                weaponWeenie = 30571;
                            }
                            if (subSwordType == 4)
                            {
                                ////Shamshir
                                weaponWeenie = 340;
                            }
                            WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)weaponWeenie);
                            spells = LootHelper.MeleeSpells;
                            wo.PropertiesSpellId.Clear();
                            if (spellCount > 0)
                            {
                                wo.UiEffects = UiEffects.Magical;
                                int[] itemSpell = new int[spellCount];
                                List<int> nums = new List<int>();
                                for (int num = 0; num < spells.Length; num++)
                                {
                                    nums.Add(num);
                                }
                                for (int j = 0; j < spellCount; j++)
                                {
                                    itemSpell[j] = rnd.Next(0, nums.Count);
                                    int spellLevel;
                                    int spellLevelChance = rnd.Next(0, 100);
                                    if (spellLevelChance > 95)
                                    {
                                        spellLevel = 3;
                                    }
                                    else if (spellLevelChance > 70)
                                    {
                                        spellLevel = 2;
                                    }
                                    else
                                    {
                                        spellLevel = 1;
                                    }
                                    var propSpell = new AceObjectPropertiesSpell
                                    {
                                        AceObjectId = wo.Guid.Full,
                                        SpellId = (uint)spells[itemSpell[j]][spellLevel]
                                    };
                                    Console.WriteLine("Writing a spell to the list...." + propSpell.SpellId);
                                    wo.PropertiesSpellId.Add(propSpell);
                                    Console.WriteLine(wo.PropertiesSpellId);
                                    nums.RemoveAt(itemSpell[j]);
                                }
                            }
                            ////Set the damage, damagevariance, damagae attack, and damage defence
                            wo.DamageType = 3;
                            int damage = rnd.Next(6, 12);
                            wo.Damage = damage;
                            double damageVariance = .1 * rnd.Next(1, 11);
                            wo.DamageVariance = damageVariance;
                            wo.DamageMod = 1.0;
                            ////Sets the speed of the weapon
                            wo.WeaponTime = 37;
                            ////Setting Weapon Defense Mod
                            int defenseChance = rnd.Next(1, 101);
                            float weaponDefense = 0;
                            if (defenseChance > 98)
                            {
                                weaponDefense = 3;
                            }
                            else if (defenseChance > 90)
                            {
                                weaponDefense = 2;
                            }
                            else if (defenseChance > 75)
                            {
                                weaponDefense = 1;
                            }
                            else
                            {
                                weaponDefense = 0;
                            }
                            wo.WeaponDefense = 1.0 + (weaponDefense * .01);
                            ////Setting Weapon Defense Mod
                            int attackChance = rnd.Next(1, 101);
                            float attackDefense = 0;
                            if (attackChance > 98)
                            {
                                attackDefense = 3;
                            }
                            else if (attackChance > 90)
                            {
                                attackDefense = 2;
                            }
                            else if (attackChance > 75)
                            {
                                attackDefense = 1;
                            }
                            else
                            {
                                attackDefense = 0;
                            }
                            wo.WeaponOffense = 1.0 + (attackDefense * .01);
                            ////Sets how many gems and what type are in the description. May affect value later on.
                            wo.GemCount = rnd.Next(1, 6);
                            wo.GemType = rnd.Next(10, 51);
                            ////Setting Material Type
                            double materialValueMod;
                            int itemMaterial = rnd.Next(0, 7);
                            if (itemMaterial == 0)
                            {
                                wo.MaterialType = Material.Brass;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 1)
                            {
                                wo.MaterialType = Material.Bronze;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 2)
                            {
                                wo.MaterialType = Material.Copper;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 3)
                            {
                                wo.MaterialType = Material.Gold;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 4)
                            {
                                wo.MaterialType = Material.Iron;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 5)
                            {
                                wo.MaterialType = Material.Silver;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 6)
                            {
                                wo.MaterialType = Material.Steel;
                                materialValueMod = 1.0;
                            }
                            ////Affects the workmanship of an item
                            int workmanshipChance = rnd.Next(1, 101);
                            float workmanship = 0;
                            if (workmanshipChance > 95)
                            {
                                workmanship = 5;
                            }
                            else if (workmanshipChance > 80)
                            {
                                workmanship = 4;
                            }
                            else if (workmanshipChance > 50)
                            {
                                workmanship = 3;
                            }
                            else if (workmanshipChance > 20)
                            {
                                workmanship = 2;
                            }
                            else
                            {
                                workmanship = 1;
                            }
                            wo.Workmanship = (int)workmanship;
                            ////Value... Not sure what the range is, so just using arbitrary numbers based on workmanship
                            int itemValue = (int)workmanship * rnd.Next(10, 40);
                            wo.Value = itemValue;
                            items.Add(wo);
                        }
                        if (subType == 6)
                        {
                            ////There are 2 subtypes of UA
                            int subUAType = rnd.Next(0, 2);
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
                            WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)weaponWeenie);
                            spells = LootHelper.MeleeSpells;
                            wo.PropertiesSpellId.Clear();
                            if (spellCount > 0)
                            {
                                wo.UiEffects = UiEffects.Magical;
                                int[] itemSpell = new int[spellCount];
                                List<int> nums = new List<int>();
                                for (int num = 0; num < spells.Length; num++)
                                {
                                    nums.Add(num);
                                }
                                for (int j = 0; j < spellCount; j++)
                                {
                                    itemSpell[j] = rnd.Next(0, nums.Count);
                                    int spellLevel;
                                    int spellLevelChance = rnd.Next(0, 100);
                                    if (spellLevelChance > 95)
                                    {
                                        spellLevel = 3;
                                    }
                                    else if (spellLevelChance > 70)
                                    {
                                        spellLevel = 2;
                                    }
                                    else
                                    {
                                        spellLevel = 1;
                                    }
                                    var propSpell = new AceObjectPropertiesSpell
                                    {
                                        AceObjectId = wo.Guid.Full,
                                        SpellId = (uint)spells[itemSpell[j]][spellLevel]
                                    };
                                    Console.WriteLine("Writing a spell to the list...." + propSpell.SpellId);
                                    wo.PropertiesSpellId.Add(propSpell);
                                    Console.WriteLine(wo.PropertiesSpellId);
                                    nums.RemoveAt(itemSpell[j]);
                                }
                            }
                            ////Set the damage, damagevariance, damagae attack, and damage defence
                            if (subUAType == 1)
                            {
                                ////Katar
                                wo.DamageType = 3;
                            }
                            else
                            {
                                wo.DamageType = 4;
                            }
                            int damage = rnd.Next(2, 6);
                            wo.Damage = damage;
                            double damageVariance = .1 * rnd.Next(1, 11);
                            wo.DamageVariance = damageVariance;
                            wo.DamageMod = 1.0;
                            ////Sets the speed of the weapon
                            wo.WeaponTime = 16;
                            ////Setting Weapon Defense Mod
                            int defenseChance = rnd.Next(1, 101);
                            float weaponDefense = 0;
                            if (defenseChance > 98)
                            {
                                weaponDefense = 3;
                            }
                            else if (defenseChance > 90)
                            {
                                weaponDefense = 2;
                            }
                            else if (defenseChance > 75)
                            {
                                weaponDefense = 1;
                            }
                            else
                            {
                                weaponDefense = 0;
                            }
                            wo.WeaponDefense = 1.0 + (weaponDefense * .01);
                            ////Setting Weapon Defense Mod
                            int attackChance = rnd.Next(1, 101);
                            float attackDefense = 0;
                            if (attackChance > 98)
                            {
                                attackDefense = 3;
                            }
                            else if (attackChance > 90)
                            {
                                attackDefense = 2;
                            }
                            else if (attackChance > 75)
                            {
                                attackDefense = 1;
                            }
                            else
                            {
                                attackDefense = 0;
                            }
                            wo.WeaponOffense = 1.0 + (attackDefense * .01);
                            ////Sets how many gems and what type are in the description. May affect value later on.
                            wo.GemCount = rnd.Next(1, 6);
                            wo.GemType = rnd.Next(10, 51);
                            ////Setting Material Type
                            double materialValueMod;
                            int itemMaterial = rnd.Next(0, 7);
                            if (itemMaterial == 0)
                            {
                                wo.MaterialType = Material.Brass;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 1)
                            {
                                wo.MaterialType = Material.Bronze;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 2)
                            {
                                wo.MaterialType = Material.Copper;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 3)
                            {
                                wo.MaterialType = Material.Gold;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 4)
                            {
                                wo.MaterialType = Material.Iron;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 5)
                            {
                                wo.MaterialType = Material.Silver;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 6)
                            {
                                wo.MaterialType = Material.Steel;
                                materialValueMod = 1.0;
                            }
                            ////Affects the workmanship of an item
                            int workmanshipChance = rnd.Next(1, 101);
                            float workmanship = 0;
                            if (workmanshipChance > 95)
                            {
                                workmanship = 5;
                            }
                            else if (workmanshipChance > 80)
                            {
                                workmanship = 4;
                            }
                            else if (workmanshipChance > 50)
                            {
                                workmanship = 3;
                            }
                            else if (workmanshipChance > 20)
                            {
                                workmanship = 2;
                            }
                            else
                            {
                                workmanship = 1;
                            }
                            wo.Workmanship = (int)workmanship;
                            ////Value... Not sure what the range is, so just using arbitrary numbers based on workmanship
                            int itemValue = (int)workmanship * rnd.Next(10, 40);
                            wo.Value = itemValue;
                            items.Add(wo);
                        }
                    }
                    if (weaponType == 2)
                    {
                        ////Finesse Weapons, of which there are 7 sub types
                        int subType = rnd.Next(0, 7);
                        if (subType == 0)
                        {
                            int subAxeType = rnd.Next(0, 4);
                            ////There are 4 subtypes of axes
                            if (subAxeType == 0)
                            {
                                ////Hammer
                                weaponWeenie = 41420;
                            }
                            if (subAxeType == 1)
                            {
                                ////Shou-ono
                                weaponWeenie = 342;
                            }
                            if (subAxeType == 2)
                            {
                                ////Hatchet
                                weaponWeenie = 30556;
                            }
                            if (subAxeType == 3)
                            {
                                ////Tungi
                                weaponWeenie = 357;
                            }
                            WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)weaponWeenie);
                            spells = LootHelper.MeleeSpells;
                            wo.PropertiesSpellId.Clear();
                            if (spellCount > 0)
                            {
                                wo.UiEffects = UiEffects.Magical;
                                int[] itemSpell = new int[spellCount];
                                List<int> nums = new List<int>();
                                for (int num = 0; num < spells.Length; num++)
                                {
                                    nums.Add(num);
                                }
                                for (int j = 0; j < spellCount; j++)
                                {
                                    itemSpell[j] = rnd.Next(0, nums.Count);
                                    int spellLevel;
                                    int spellLevelChance = rnd.Next(0, 100);
                                    if (spellLevelChance > 95)
                                    {
                                        spellLevel = 3;
                                    }
                                    else if (spellLevelChance > 70)
                                    {
                                        spellLevel = 2;
                                    }
                                    else
                                    {
                                        spellLevel = 1;
                                    }
                                    var propSpell = new AceObjectPropertiesSpell
                                    {
                                        AceObjectId = wo.Guid.Full,
                                        SpellId = (uint)spells[itemSpell[j]][spellLevel]
                                    };
                                    Console.WriteLine("Writing a spell to the list...." + propSpell.SpellId);
                                    wo.PropertiesSpellId.Add(propSpell);
                                    Console.WriteLine(wo.PropertiesSpellId);
                                    nums.RemoveAt(itemSpell[j]);
                                }
                            }
                            ////Set the damage, damagevariance, damagae attack, and damage defence
                            if (subAxeType == 0)
                            {
                                ////Hammer
                                wo.DamageType = 4;
                            }
                            else
                            {
                                wo.DamageType = 1;
                            }
                            int damage = rnd.Next(6, 12);
                            wo.Damage = damage;
                            double damageVariance = .1 * rnd.Next(1, 11);
                            wo.DamageVariance = damageVariance;
                            wo.DamageMod = 1.0;
                            ////Sets the speed of the weapon
                            wo.WeaponTime = 50;
                            ////Setting Weapon Defense Mod
                            int defenseChance = rnd.Next(1, 101);
                            float weaponDefense = 0;
                            if (defenseChance > 98)
                            {
                                weaponDefense = 3;
                            }
                            else if (defenseChance > 90)
                            {
                                weaponDefense = 2;
                            }
                            else if (defenseChance > 75)
                            {
                                weaponDefense = 1;
                            }
                            else
                            {
                                weaponDefense = 0;
                            }
                            wo.WeaponDefense = 1.0 + (weaponDefense * .01);
                            ////Setting Weapon Defense Mod
                            int attackChance = rnd.Next(1, 101);
                            float attackDefense = 0;
                            if (attackChance > 98)
                            {
                                attackDefense = 3;
                            }
                            else if (attackChance > 90)
                            {
                                attackDefense = 2;
                            }
                            else if (attackChance > 75)
                            {
                                attackDefense = 1;
                            }
                            else
                            {
                                attackDefense = 0;
                            }
                            wo.WeaponOffense = 1.0 + (attackDefense * .01);
                            ////Sets how many gems and what type are in the description. May affect value later on.
                            wo.GemCount = rnd.Next(1, 6);
                            wo.GemType = rnd.Next(10, 51);
                            ////Setting Material Type
                            double materialValueMod;
                            int itemMaterial = rnd.Next(0, 7);
                            if (itemMaterial == 0)
                            {
                                wo.MaterialType = Material.Brass;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 1)
                            {
                                wo.MaterialType = Material.Bronze;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 2)
                            {
                                wo.MaterialType = Material.Copper;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 3)
                            {
                                wo.MaterialType = Material.Gold;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 4)
                            {
                                wo.MaterialType = Material.Iron;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 5)
                            {
                                wo.MaterialType = Material.Silver;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 6)
                            {
                                wo.MaterialType = Material.Steel;
                                materialValueMod = 1.0;
                            }
                            ////Affects the workmanship of an item
                            int workmanshipChance = rnd.Next(1, 101);
                            float workmanship = 0;
                            if (workmanshipChance > 95)
                            {
                                workmanship = 5;
                            }
                            else if (workmanshipChance > 80)
                            {
                                workmanship = 4;
                            }
                            else if (workmanshipChance > 50)
                            {
                                workmanship = 3;
                            }
                            else if (workmanshipChance > 20)
                            {
                                workmanship = 2;
                            }
                            else
                            {
                                workmanship = 1;
                            }
                            wo.Workmanship = (int)workmanship;
                            ////Value... Not sure what the range is, so just using arbitrary numbers based on workmanship
                            int itemValue = (int)workmanship * rnd.Next(10, 40);
                            wo.Value = itemValue;
                            items.Add(wo);
                        }
                        if (subType == 1)
                        {
                            ////There are 3 subtypes of daggers
                            int subDaggerType = rnd.Next(0, 3);
                            if (subDaggerType == 0)
                            {
                                ////Knife
                                weaponWeenie = 329;
                            }
                            if (subDaggerType == 1)
                            {
                                ////Lancet
                                weaponWeenie = 31794;
                            }
                            if (subDaggerType == 2)
                            {
                                ////Poniard
                                weaponWeenie = 30946;
                            }
                            WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)weaponWeenie);
                            spells = LootHelper.MeleeSpells;
                            wo.PropertiesSpellId.Clear();
                            if (spellCount > 0)
                            {
                                wo.UiEffects = UiEffects.Magical;
                                int[] itemSpell = new int[spellCount];
                                List<int> nums = new List<int>();
                                for (int num = 0; num < spells.Length; num++)
                                {
                                    nums.Add(num);
                                }
                                for (int j = 0; j < spellCount; j++)
                                {
                                    itemSpell[j] = rnd.Next(0, nums.Count);
                                    int spellLevel;
                                    int spellLevelChance = rnd.Next(0, 100);
                                    if (spellLevelChance > 95)
                                    {
                                        spellLevel = 3;
                                    }
                                    else if (spellLevelChance > 70)
                                    {
                                        spellLevel = 2;
                                    }
                                    else
                                    {
                                        spellLevel = 1;
                                    }
                                    var propSpell = new AceObjectPropertiesSpell
                                    {
                                        AceObjectId = wo.Guid.Full,
                                        SpellId = (uint)spells[itemSpell[j]][spellLevel]
                                    };
                                    Console.WriteLine("Writing a spell to the list...." + propSpell.SpellId);
                                    wo.PropertiesSpellId.Add(propSpell);
                                    Console.WriteLine(wo.PropertiesSpellId);
                                    nums.RemoveAt(itemSpell[j]);
                                }
                            }
                            ////Set the damage, damagevariance, damagae attack, and damage defence
                            wo.DamageType = 3;
                            int damage = rnd.Next(2, 6);
                            wo.Damage = damage;
                            double damageVariance = .1 * rnd.Next(1, 11);
                            wo.DamageVariance = damageVariance;
                            wo.DamageMod = 1.0;
                            ////Sets the speed of the weapon
                            wo.WeaponTime = 10;
                            ////Setting Weapon Defense Mod
                            int defenseChance = rnd.Next(1, 101);
                            float weaponDefense = 0;
                            if (defenseChance > 98)
                            {
                                weaponDefense = 3;
                            }
                            else if (defenseChance > 90)
                            {
                                weaponDefense = 2;
                            }
                            else if (defenseChance > 75)
                            {
                                weaponDefense = 1;
                            }
                            else
                            {
                                weaponDefense = 0;
                            }
                            wo.WeaponDefense = 1.0 + (weaponDefense * .01);
                            ////Setting Weapon Defense Mod
                            int attackChance = rnd.Next(1, 101);
                            float attackDefense = 0;
                            if (attackChance > 98)
                            {
                                attackDefense = 3;
                            }
                            else if (attackChance > 90)
                            {
                                attackDefense = 2;
                            }
                            else if (attackChance > 75)
                            {
                                attackDefense = 1;
                            }
                            else
                            {
                                attackDefense = 0;
                            }
                            wo.WeaponOffense = 1.0 + (attackDefense * .01);
                            ////Sets how many gems and what type are in the description. May affect value later on.
                            wo.GemCount = rnd.Next(1, 6);
                            wo.GemType = rnd.Next(10, 51);
                            ////Setting Material Type
                            double materialValueMod;
                            int itemMaterial = rnd.Next(0, 7);
                            if (itemMaterial == 0)
                            {
                                wo.MaterialType = Material.Brass;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 1)
                            {
                                wo.MaterialType = Material.Bronze;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 2)
                            {
                                wo.MaterialType = Material.Copper;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 3)
                            {
                                wo.MaterialType = Material.Gold;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 4)
                            {
                                wo.MaterialType = Material.Iron;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 5)
                            {
                                wo.MaterialType = Material.Silver;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 6)
                            {
                                wo.MaterialType = Material.Steel;
                                materialValueMod = 1.0;
                            }
                            ////Affects the workmanship of an item
                            int workmanshipChance = rnd.Next(1, 101);
                            float workmanship = 0;
                            if (workmanshipChance > 95)
                            {
                                workmanship = 5;
                            }
                            else if (workmanshipChance > 80)
                            {
                                workmanship = 4;
                            }
                            else if (workmanshipChance > 50)
                            {
                                workmanship = 3;
                            }
                            else if (workmanshipChance > 20)
                            {
                                workmanship = 2;
                            }
                            else
                            {
                                workmanship = 1;
                            }
                            wo.Workmanship = (int)workmanship;
                            ////Value... Not sure what the range is, so just using arbitrary numbers based on workmanship
                            int itemValue = (int)workmanship * rnd.Next(10, 40);
                            wo.Value = itemValue;
                            items.Add(wo);
                        }
                        if (subType == 2)
                        {
                            ////There are 4 subtypes of maces
                            int subMaceType = rnd.Next(0, 4);
                            if (subMaceType == 0)
                            {
                                ////Board with Nail
                                weaponWeenie = 47248;
                            }
                            if (subMaceType == 1)
                            {
                                ////Dabus
                                weaponWeenie = 313;
                            }
                            if (subMaceType == 2)
                            {
                                ////Tofun
                                weaponWeenie = 356;
                            }
                            if (subMaceType == 3)
                            {
                                ////Jitte
                                weaponWeenie = 321;
                            }
                            WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)weaponWeenie);
                            spells = LootHelper.MeleeSpells;
                            wo.PropertiesSpellId.Clear();
                            if (spellCount > 0)
                            {
                                wo.UiEffects = UiEffects.Magical;
                                int[] itemSpell = new int[spellCount];
                                List<int> nums = new List<int>();
                                for (int num = 0; num < spells.Length; num++)
                                {
                                    nums.Add(num);
                                }
                                for (int j = 0; j < spellCount; j++)
                                {
                                    itemSpell[j] = rnd.Next(0, nums.Count);
                                    int spellLevel;
                                    int spellLevelChance = rnd.Next(0, 100);
                                    if (spellLevelChance > 95)
                                    {
                                        spellLevel = 3;
                                    }
                                    else if (spellLevelChance > 70)
                                    {
                                        spellLevel = 2;
                                    }
                                    else
                                    {
                                        spellLevel = 1;
                                    }
                                    var propSpell = new AceObjectPropertiesSpell
                                    {
                                        AceObjectId = wo.Guid.Full,
                                        SpellId = (uint)spells[itemSpell[j]][spellLevel]
                                    };
                                    Console.WriteLine("Writing a spell to the list...." + propSpell.SpellId);
                                    wo.PropertiesSpellId.Add(propSpell);
                                    Console.WriteLine(wo.PropertiesSpellId);
                                    nums.RemoveAt(itemSpell[j]);
                                }
                            }
                            ////Set the damage, damagevariance, damagae attack, and damage defence
                            if (subMaceType == 0)
                            {
                                ////Board with Nail
                                wo.DamageType = 2;
                            }
                            else
                            {
                                wo.DamageType = 4;
                            }
                            int damage = rnd.Next(8, 14);
                            wo.Damage = damage;
                            double damageVariance = .1 * rnd.Next(1, 11);
                            wo.DamageVariance = damageVariance;
                            wo.DamageMod = 1.0;
                            ////Sets the speed of the weapon
                            wo.WeaponTime = 40;
                            ////Setting Weapon Defense Mod
                            int defenseChance = rnd.Next(1, 101);
                            float weaponDefense = 0;
                            if (defenseChance > 98)
                            {
                                weaponDefense = 3;
                            }
                            else if (defenseChance > 90)
                            {
                                weaponDefense = 2;
                            }
                            else if (defenseChance > 75)
                            {
                                weaponDefense = 1;
                            }
                            else
                            {
                                weaponDefense = 0;
                            }
                            wo.WeaponDefense = 1.0 + (weaponDefense * .01);
                            ////Setting Weapon Defense Mod
                            int attackChance = rnd.Next(1, 101);
                            float attackDefense = 0;
                            if (attackChance > 98)
                            {
                                attackDefense = 3;
                            }
                            else if (attackChance > 90)
                            {
                                attackDefense = 2;
                            }
                            else if (attackChance > 75)
                            {
                                attackDefense = 1;
                            }
                            else
                            {
                                attackDefense = 0;
                            }
                            wo.WeaponOffense = 1.0 + (attackDefense * .01);
                            ////Sets how many gems and what type are in the description. May affect value later on.
                            wo.GemCount = rnd.Next(1, 6);
                            wo.GemType = rnd.Next(10, 51);
                            ////Setting Material Type
                            double materialValueMod;
                            int itemMaterial = rnd.Next(0, 7);
                            if (itemMaterial == 0)
                            {
                                wo.MaterialType = Material.Brass;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 1)
                            {
                                wo.MaterialType = Material.Bronze;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 2)
                            {
                                wo.MaterialType = Material.Copper;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 3)
                            {
                                wo.MaterialType = Material.Gold;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 4)
                            {
                                wo.MaterialType = Material.Iron;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 5)
                            {
                                wo.MaterialType = Material.Silver;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 6)
                            {
                                wo.MaterialType = Material.Steel;
                                materialValueMod = 1.0;
                            }
                            ////Affects the workmanship of an item
                            int workmanshipChance = rnd.Next(1, 101);
                            float workmanship = 0;
                            if (workmanshipChance > 95)
                            {
                                workmanship = 5;
                            }
                            else if (workmanshipChance > 80)
                            {
                                workmanship = 4;
                            }
                            else if (workmanshipChance > 50)
                            {
                                workmanship = 3;
                            }
                            else if (workmanshipChance > 20)
                            {
                                workmanship = 2;
                            }
                            else
                            {
                                workmanship = 1;
                            }
                            wo.Workmanship = (int)workmanship;
                            ////Value... Not sure what the range is, so just using arbitrary numbers based on workmanship
                            int itemValue = (int)workmanship * rnd.Next(10, 40);
                            wo.Value = itemValue;
                            items.Add(wo);
                        }
                        if (subType == 3)
                        {
                            ////There are 2 subtypes of spears
                            int subSpearType = rnd.Next(0, 2);
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
                            WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)weaponWeenie);
                            spells = LootHelper.MeleeSpells;
                            wo.PropertiesSpellId.Clear();
                            if (spellCount > 0)
                            {
                                wo.UiEffects = UiEffects.Magical;
                                int[] itemSpell = new int[spellCount];
                                List<int> nums = new List<int>();
                                for (int num = 0; num < spells.Length; num++)
                                {
                                    nums.Add(num);
                                }
                                for (int j = 0; j < spellCount; j++)
                                {
                                    itemSpell[j] = rnd.Next(0, nums.Count);
                                    int spellLevel;
                                    int spellLevelChance = rnd.Next(0, 100);
                                    if (spellLevelChance > 95)
                                    {
                                        spellLevel = 3;
                                    }
                                    else if (spellLevelChance > 70)
                                    {
                                        spellLevel = 2;
                                    }
                                    else
                                    {
                                        spellLevel = 1;
                                    }
                                    var propSpell = new AceObjectPropertiesSpell
                                    {
                                        AceObjectId = wo.Guid.Full,
                                        SpellId = (uint)spells[itemSpell[j]][spellLevel]
                                    };
                                    Console.WriteLine("Writing a spell to the list...." + propSpell.SpellId);
                                    wo.PropertiesSpellId.Add(propSpell);
                                    Console.WriteLine(wo.PropertiesSpellId);
                                    nums.RemoveAt(itemSpell[j]);
                                }
                            }
                            ////Set the damage, damagevariance, damagae attack, and damage defence
                            wo.DamageType = 2;
                            int damage = rnd.Next(6, 12);
                            wo.Damage = damage;
                            double damageVariance = .1 * rnd.Next(1, 11);
                            wo.DamageVariance = damageVariance;
                            wo.DamageMod = 1.0;
                            ////Sets the speed of the weapon
                            wo.WeaponTime = 50;
                            ////Setting Weapon Defense Mod
                            int defenseChance = rnd.Next(1, 101);
                            float weaponDefense = 0;
                            if (defenseChance > 98)
                            {
                                weaponDefense = 3;
                            }
                            else if (defenseChance > 90)
                            {
                                weaponDefense = 2;
                            }
                            else if (defenseChance > 75)
                            {
                                weaponDefense = 1;
                            }
                            else
                            {
                                weaponDefense = 0;
                            }
                            wo.WeaponDefense = 1.0 + (weaponDefense * .01);
                            ////Setting Weapon Defense Mod
                            int attackChance = rnd.Next(1, 101);
                            float attackDefense = 0;
                            if (attackChance > 98)
                            {
                                attackDefense = 3;
                            }
                            else if (attackChance > 90)
                            {
                                attackDefense = 2;
                            }
                            else if (attackChance > 75)
                            {
                                attackDefense = 1;
                            }
                            else
                            {
                                attackDefense = 0;
                            }
                            wo.WeaponOffense = 1.0 + (attackDefense * .01);
                            ////Sets how many gems and what type are in the description. May affect value later on.
                            wo.GemCount = rnd.Next(1, 6);
                            wo.GemType = rnd.Next(10, 51);
                            ////Setting Material Type
                            double materialValueMod;
                            int itemMaterial = rnd.Next(0, 7);
                            if (itemMaterial == 0)
                            {
                                wo.MaterialType = Material.Brass;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 1)
                            {
                                wo.MaterialType = Material.Bronze;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 2)
                            {
                                wo.MaterialType = Material.Copper;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 3)
                            {
                                wo.MaterialType = Material.Gold;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 4)
                            {
                                wo.MaterialType = Material.Iron;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 5)
                            {
                                wo.MaterialType = Material.Silver;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 6)
                            {
                                wo.MaterialType = Material.Steel;
                                materialValueMod = 1.0;
                            }
                            ////Affects the workmanship of an item
                            int workmanshipChance = rnd.Next(1, 101);
                            float workmanship = 0;
                            if (workmanshipChance > 95)
                            {
                                workmanship = 5;
                            }
                            else if (workmanshipChance > 80)
                            {
                                workmanship = 4;
                            }
                            else if (workmanshipChance > 50)
                            {
                                workmanship = 3;
                            }
                            else if (workmanshipChance > 20)
                            {
                                workmanship = 2;
                            }
                            else
                            {
                                workmanship = 1;
                            }
                            wo.Workmanship = (int)workmanship;
                            ////Value... Not sure what the range is, so just using arbitrary numbers based on workmanship
                            int itemValue = (int)workmanship * rnd.Next(10, 40);
                            wo.Value = itemValue;
                            items.Add(wo);
                        }
                        if (subType == 4)
                        {
                            ////There is 2 subtypes of staves
                            int subStaffType = rnd.Next(0, 2);
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
                            WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)weaponWeenie);
                            spells = LootHelper.MeleeSpells;
                            wo.PropertiesSpellId.Clear();
                            if (spellCount > 0)
                            {
                                wo.UiEffects = UiEffects.Magical;
                                int[] itemSpell = new int[spellCount];
                                List<int> nums = new List<int>();
                                for (int num = 0; num < spells.Length; num++)
                                {
                                    nums.Add(num);
                                }
                                for (int j = 0; j < spellCount; j++)
                                {
                                    itemSpell[j] = rnd.Next(0, nums.Count);
                                    int spellLevel;
                                    int spellLevelChance = rnd.Next(0, 100);
                                    if (spellLevelChance > 95)
                                    {
                                        spellLevel = 3;
                                    }
                                    else if (spellLevelChance > 70)
                                    {
                                        spellLevel = 2;
                                    }
                                    else
                                    {
                                        spellLevel = 1;
                                    }
                                    var propSpell = new AceObjectPropertiesSpell
                                    {
                                        AceObjectId = wo.Guid.Full,
                                        SpellId = (uint)spells[itemSpell[j]][spellLevel]
                                    };
                                    Console.WriteLine("Writing a spell to the list...." + propSpell.SpellId);
                                    wo.PropertiesSpellId.Add(propSpell);
                                    Console.WriteLine(wo.PropertiesSpellId);
                                    nums.RemoveAt(itemSpell[j]);
                                }
                            }
                            ////Set the damage, damagevariance, damagae attack, and damage defence
                            wo.DamageType = 4;
                            int damage = rnd.Next(6, 15);
                            wo.Damage = damage;
                            double damageVariance = .1 * rnd.Next(1, 11);
                            wo.DamageVariance = damageVariance;
                            wo.DamageMod = 1.0;
                            ////Sets the speed of the weapon
                            wo.WeaponTime = 45;
                            ////Setting Weapon Defense Mod
                            int defenseChance = rnd.Next(1, 101);
                            float weaponDefense = 0;
                            if (defenseChance > 98)
                            {
                                weaponDefense = 3;
                            }
                            else if (defenseChance > 90)
                            {
                                weaponDefense = 2;
                            }
                            else if (defenseChance > 75)
                            {
                                weaponDefense = 1;
                            }
                            else
                            {
                                weaponDefense = 0;
                            }
                            wo.WeaponDefense = 1.0 + (weaponDefense * .01);
                            ////Setting Weapon Defense Mod
                            int attackChance = rnd.Next(1, 101);
                            float attackDefense = 0;
                            if (attackChance > 98)
                            {
                                attackDefense = 3;
                            }
                            else if (attackChance > 90)
                            {
                                attackDefense = 2;
                            }
                            else if (attackChance > 75)
                            {
                                attackDefense = 1;
                            }
                            else
                            {
                                attackDefense = 0;
                            }
                            wo.WeaponOffense = 1.0 + (attackDefense * .01);
                            ////Sets how many gems and what type are in the description. May affect value later on.
                            wo.GemCount = rnd.Next(1, 6);
                            wo.GemType = rnd.Next(10, 51);
                            ////Setting Material Type
                            int itemMaterial = rnd.Next(0, 8);
                            if (itemMaterial == 0)
                            {
                                wo.MaterialType = Material.Ebony;
                            }
                            if (itemMaterial == 1)
                            {
                                wo.MaterialType = Material.Mahogany;
                            }
                            if (itemMaterial == 2)
                            {
                                wo.MaterialType = Material.Oak;
                            }
                            if (itemMaterial == 3)
                            {
                                wo.MaterialType = Material.Pine;
                            }
                            if (itemMaterial == 4)
                            {
                                wo.MaterialType = Material.Teak;
                            }
                            ////Affects the workmanship of an item
                            int workmanshipChance = rnd.Next(1, 101);
                            float workmanship = 0;
                            if (workmanshipChance > 95)
                            {
                                workmanship = 5;
                            }
                            else if (workmanshipChance > 80)
                            {
                                workmanship = 4;
                            }
                            else if (workmanshipChance > 50)
                            {
                                workmanship = 3;
                            }
                            else if (workmanshipChance > 20)
                            {
                                workmanship = 2;
                            }
                            else
                            {
                                workmanship = 1;
                            }
                            wo.Workmanship = (int)workmanship;
                            ////Value... Not sure what the range is, so just using arbitrary numbers based on workmanship
                            int itemValue = (int)workmanship * rnd.Next(10, 40);
                            wo.Value = itemValue;
                            items.Add(wo);
                        }
                        if (subType == 5)
                        {
                            ////There are 6 subtypes of swords
                            int subSwordType = rnd.Next(0, 6);
                            if (subSwordType == 0)
                            {
                                ////Rapier
                                weaponWeenie = 6853;
                            }
                            if (subSwordType == 1)
                            {
                                ////Sabra
                                weaponWeenie = 38934;
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
                            WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)weaponWeenie);
                            spells = LootHelper.MeleeSpells;
                            wo.PropertiesSpellId.Clear();
                            if (spellCount > 0)
                            {
                                wo.UiEffects = UiEffects.Magical;
                                int[] itemSpell = new int[spellCount];
                                List<int> nums = new List<int>();
                                for (int num = 0; num < spells.Length; num++)
                                {
                                    nums.Add(num);
                                }
                                for (int j = 0; j < spellCount; j++)
                                {
                                    itemSpell[j] = rnd.Next(0, nums.Count);
                                    int spellLevel;
                                    int spellLevelChance = rnd.Next(0, 100);
                                    if (spellLevelChance > 95)
                                    {
                                        spellLevel = 3;
                                    }
                                    else if (spellLevelChance > 70)
                                    {
                                        spellLevel = 2;
                                    }
                                    else
                                    {
                                        spellLevel = 1;
                                    }
                                    var propSpell = new AceObjectPropertiesSpell
                                    {
                                        AceObjectId = wo.Guid.Full,
                                        SpellId = (uint)spells[itemSpell[j]][spellLevel]
                                    };
                                    Console.WriteLine("Writing a spell to the list...." + propSpell.SpellId);
                                    wo.PropertiesSpellId.Add(propSpell);
                                    Console.WriteLine(wo.PropertiesSpellId);
                                    nums.RemoveAt(itemSpell[j]);
                                }
                            }
                            ////Set the damage, damagevariance, damagae attack, and damage defence
                            wo.DamageType = 3;
                            int damage = rnd.Next(6, 12);
                            wo.Damage = damage;
                            double damageVariance = .1 * rnd.Next(1, 11);
                            wo.DamageVariance = damageVariance;
                            wo.DamageMod = 1.0;
                            ////Sets the speed of the weapon
                            wo.WeaponTime = 37;
                            ////Setting Weapon Defense Mod
                            int defenseChance = rnd.Next(1, 101);
                            float weaponDefense = 0;
                            if (defenseChance > 98)
                            {
                                weaponDefense = 3;
                            }
                            else if (defenseChance > 90)
                            {
                                weaponDefense = 2;
                            }
                            else if (defenseChance > 75)
                            {
                                weaponDefense = 1;
                            }
                            else
                            {
                                weaponDefense = 0;
                            }
                            wo.WeaponDefense = 1.0 + (weaponDefense * .01);
                            ////Setting Weapon Defense Mod
                            int attackChance = rnd.Next(1, 101);
                            float attackDefense = 0;
                            if (attackChance > 98)
                            {
                                attackDefense = 3;
                            }
                            else if (attackChance > 90)
                            {
                                attackDefense = 2;
                            }
                            else if (attackChance > 75)
                            {
                                attackDefense = 1;
                            }
                            else
                            {
                                attackDefense = 0;
                            }
                            wo.WeaponOffense = 1.0 + (attackDefense * .01);
                            ////Sets how many gems and what type are in the description. May affect value later on.
                            wo.GemCount = rnd.Next(1, 6);
                            wo.GemType = rnd.Next(10, 51);
                            ////Setting Material Type
                            double materialValueMod;
                            int itemMaterial = rnd.Next(0, 7);
                            if (itemMaterial == 0)
                            {
                                wo.MaterialType = Material.Brass;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 1)
                            {
                                wo.MaterialType = Material.Bronze;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 2)
                            {
                                wo.MaterialType = Material.Copper;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 3)
                            {
                                wo.MaterialType = Material.Gold;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 4)
                            {
                                wo.MaterialType = Material.Iron;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 5)
                            {
                                wo.MaterialType = Material.Silver;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 6)
                            {
                                wo.MaterialType = Material.Steel;
                                materialValueMod = 1.0;
                            }
                            ////Affects the workmanship of an item
                            int workmanshipChance = rnd.Next(1, 101);
                            float workmanship = 0;
                            if (workmanshipChance > 95)
                            {
                                workmanship = 5;
                            }
                            else if (workmanshipChance > 80)
                            {
                                workmanship = 4;
                            }
                            else if (workmanshipChance > 50)
                            {
                                workmanship = 3;
                            }
                            else if (workmanshipChance > 20)
                            {
                                workmanship = 2;
                            }
                            else
                            {
                                workmanship = 1;
                            }
                            wo.Workmanship = (int)workmanship;
                            ////Value... Not sure what the range is, so just using arbitrary numbers based on workmanship
                            int itemValue = (int)workmanship * rnd.Next(10, 40);
                            wo.Value = itemValue;
                            items.Add(wo);
                        }
                        if (subType == 6)
                        {
                            ////There are 2 subtypes of UA
                            int subUAType = rnd.Next(0, 2);
                            if (subUAType == 0)
                            {
                                ////Claw
                                weaponWeenie = 31784;
                            }
                            if (subUAType == 1)
                            {
                                ////Hand Wraps
                                weaponWeenie = 45118;
                            }
                            WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)weaponWeenie);
                            spells = LootHelper.MeleeSpells;
                            wo.PropertiesSpellId.Clear();
                            if (spellCount > 0)
                            {
                                wo.UiEffects = UiEffects.Magical;
                                int[] itemSpell = new int[spellCount];
                                List<int> nums = new List<int>();
                                for (int num = 0; num < spells.Length; num++)
                                {
                                    nums.Add(num);
                                }
                                for (int j = 0; j < spellCount; j++)
                                {
                                    itemSpell[j] = rnd.Next(0, nums.Count);
                                    int spellLevel;
                                    int spellLevelChance = rnd.Next(0, 100);
                                    if (spellLevelChance > 95)
                                    {
                                        spellLevel = 3;
                                    }
                                    else if (spellLevelChance > 70)
                                    {
                                        spellLevel = 2;
                                    }
                                    else
                                    {
                                        spellLevel = 1;
                                    }
                                    var propSpell = new AceObjectPropertiesSpell
                                    {
                                        AceObjectId = wo.Guid.Full,
                                        SpellId = (uint)spells[itemSpell[j]][spellLevel]
                                    };
                                    Console.WriteLine("Writing a spell to the list...." + propSpell.SpellId);
                                    wo.PropertiesSpellId.Add(propSpell);
                                    Console.WriteLine(wo.PropertiesSpellId);
                                    nums.RemoveAt(itemSpell[j]);
                                }
                            }
                            ////Set the damage, damagevariance, damagae attack, and damage defence
                            if (subUAType == 1)
                            {
                                ////Hand Wraps
                                wo.DamageType = 4;
                            }
                            else
                            {
                                wo.DamageType = 3;
                            }
                            int damage = rnd.Next(2, 6);
                            wo.Damage = damage;
                            double damageVariance = .1 * rnd.Next(1, 11);
                            wo.DamageVariance = damageVariance;
                            wo.DamageMod = 1.0;
                            ////Sets the speed of the weapon
                            wo.WeaponTime = 16;
                            ////Setting Weapon Defense Mod
                            int defenseChance = rnd.Next(1, 101);
                            float weaponDefense = 0;
                            if (defenseChance > 98)
                            {
                                weaponDefense = 3;
                            }
                            else if (defenseChance > 90)
                            {
                                weaponDefense = 2;
                            }
                            else if (defenseChance > 75)
                            {
                                weaponDefense = 1;
                            }
                            else
                            {
                                weaponDefense = 0;
                            }
                            wo.WeaponDefense = 1.0 + (weaponDefense * .01);
                            ////Setting Weapon Defense Mod
                            int attackChance = rnd.Next(1, 101);
                            float attackDefense = 0;
                            if (attackChance > 98)
                            {
                                attackDefense = 3;
                            }
                            else if (attackChance > 90)
                            {
                                attackDefense = 2;
                            }
                            else if (attackChance > 75)
                            {
                                attackDefense = 1;
                            }
                            else
                            {
                                attackDefense = 0;
                            }
                            wo.WeaponOffense = 1.0 + (attackDefense * .01);
                            ////Sets how many gems and what type are in the description. May affect value later on.
                            wo.GemCount = rnd.Next(1, 6);
                            wo.GemType = rnd.Next(10, 51);
                            ////Setting Material Type
                            double materialValueMod;
                            int itemMaterial = rnd.Next(0, 7);
                            if (itemMaterial == 0)
                            {
                                wo.MaterialType = Material.Brass;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 1)
                            {
                                wo.MaterialType = Material.Bronze;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 2)
                            {
                                wo.MaterialType = Material.Copper;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 3)
                            {
                                wo.MaterialType = Material.Gold;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 4)
                            {
                                wo.MaterialType = Material.Iron;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 5)
                            {
                                wo.MaterialType = Material.Silver;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 6)
                            {
                                wo.MaterialType = Material.Steel;
                                materialValueMod = 1.0;
                            }
                            ////Affects the workmanship of an item
                            int workmanshipChance = rnd.Next(1, 101);
                            float workmanship = 0;
                            if (workmanshipChance > 95)
                            {
                                workmanship = 5;
                            }
                            else if (workmanshipChance > 80)
                            {
                                workmanship = 4;
                            }
                            else if (workmanshipChance > 50)
                            {
                                workmanship = 3;
                            }
                            else if (workmanshipChance > 20)
                            {
                                workmanship = 2;
                            }
                            else
                            {
                                workmanship = 1;
                            }
                            wo.Workmanship = (int)workmanship;
                            ////Value... Not sure what the range is, so just using arbitrary numbers based on workmanship
                            int itemValue = (int)workmanship * rnd.Next(10, 40);
                            wo.Value = itemValue;
                            items.Add(wo);
                        }
                    }
                    if (weaponType == 3)
                    {
                        ////Two Handed Weapons, of which there are 4 sub types
                        int subType = rnd.Next(0, 4);
                        if (subType == 0)
                        {
                            ////There are 4 subtypes of axes
                            int subSwordType = rnd.Next(0, 3);
                            if (subSwordType == 0)
                            {
                                ////Nodachi
                                weaponWeenie = 40760;
                            }
                            if (subSwordType == 1)
                            {
                                ////Shashqa
                                weaponWeenie = 41067;
                            }
                            if (subSwordType == 2)
                            {
                                ////Spadone
                                weaponWeenie = 29975;
                            }
                            WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)weaponWeenie);
                            spells = LootHelper.MeleeSpells;
                            wo.PropertiesSpellId.Clear();
                            if (spellCount > 0)
                            {
                                wo.UiEffects = UiEffects.Magical;
                                int[] itemSpell = new int[spellCount];
                                List<int> nums = new List<int>();
                                for (int num = 0; num < spells.Length; num++)
                                {
                                    nums.Add(num);
                                }
                                for (int j = 0; j < spellCount; j++)
                                {
                                    itemSpell[j] = rnd.Next(0, nums.Count);
                                    int spellLevel;
                                    int spellLevelChance = rnd.Next(0, 100);
                                    if (spellLevelChance > 95)
                                    {
                                        spellLevel = 3;
                                    }
                                    else if (spellLevelChance > 70)
                                    {
                                        spellLevel = 2;
                                    }
                                    else
                                    {
                                        spellLevel = 1;
                                    }
                                    var propSpell = new AceObjectPropertiesSpell
                                    {
                                        AceObjectId = wo.Guid.Full,
                                        SpellId = (uint)spells[itemSpell[j]][spellLevel]
                                    };
                                    Console.WriteLine("Writing a spell to the list...." + propSpell.SpellId);
                                    wo.PropertiesSpellId.Add(propSpell);
                                    Console.WriteLine(wo.PropertiesSpellId);
                                    nums.RemoveAt(itemSpell[j]);
                                }
                            }
                            ////Set the damage, damagevariance, damagae attack, and damage defence
                            wo.DamageType = 3;
                            int damage = rnd.Next(10, 20);
                            wo.Damage = damage;
                            double damageVariance = .1 * rnd.Next(1, 11);
                            wo.DamageVariance = damageVariance;
                            wo.DamageMod = 1.0;
                            ////Sets the speed of the weapon
                            wo.WeaponTime = 46;
                            ////Setting Weapon Defense Mod
                            int defenseChance = rnd.Next(1, 101);
                            float weaponDefense = 0;
                            if (defenseChance > 98)
                            {
                                weaponDefense = 3;
                            }
                            else if (defenseChance > 90)
                            {
                                weaponDefense = 2;
                            }
                            else if (defenseChance > 75)
                            {
                                weaponDefense = 1;
                            }
                            else
                            {
                                weaponDefense = 0;
                            }
                            wo.WeaponDefense = 1.0 + (weaponDefense * .01);
                            ////Setting Weapon Defense Mod
                            int attackChance = rnd.Next(1, 101);
                            float attackDefense = 0;
                            if (attackChance > 98)
                            {
                                attackDefense = 3;
                            }
                            else if (attackChance > 90)
                            {
                                attackDefense = 2;
                            }
                            else if (attackChance > 75)
                            {
                                attackDefense = 1;
                            }
                            else
                            {
                                attackDefense = 0;
                            }
                            wo.WeaponOffense = 1.0 + (attackDefense * .01);
                            ////Sets how many gems and what type are in the description. May affect value later on.
                            wo.GemCount = rnd.Next(1, 6);
                            wo.GemType = rnd.Next(10, 51);
                            ////Setting Material Type
                            double materialValueMod;
                            int itemMaterial = rnd.Next(0, 7);
                            if (itemMaterial == 0)
                            {
                                wo.MaterialType = Material.Brass;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 1)
                            {
                                wo.MaterialType = Material.Bronze;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 2)
                            {
                                wo.MaterialType = Material.Copper;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 3)
                            {
                                wo.MaterialType = Material.Gold;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 4)
                            {
                                wo.MaterialType = Material.Iron;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 5)
                            {
                                wo.MaterialType = Material.Silver;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 6)
                            {
                                wo.MaterialType = Material.Steel;
                                materialValueMod = 1.0;
                            }
                            ////Affects the workmanship of an item
                            int workmanshipChance = rnd.Next(1, 101);
                            float workmanship = 0;
                            if (workmanshipChance > 95)
                            {
                                workmanship = 5;
                            }
                            else if (workmanshipChance > 80)
                            {
                                workmanship = 4;
                            }
                            else if (workmanshipChance > 50)
                            {
                                workmanship = 3;
                            }
                            else if (workmanshipChance > 20)
                            {
                                workmanship = 2;
                            }
                            else
                            {
                                workmanship = 1;
                            }
                            wo.Workmanship = (int)workmanship;
                            ////Value... Not sure what the range is, so just using arbitrary numbers based on workmanship
                            int itemValue = (int)workmanship * rnd.Next(10, 40);
                            wo.Value = itemValue;
                            items.Add(wo);
                        }
                        if (subType == 1)
                        {
                            ////There are 4 subtypes of Maces
                            int subMaceType = rnd.Next(0, 4);
                            if (subMaceType == 0)
                            {
                                ////Great Star Mace
                                weaponWeenie = 41057;
                            }
                            if (subMaceType == 1)
                            {
                                ////Quadrelle
                                weaponWeenie = 29965;
                            }
                            if (subMaceType == 2)
                            {
                                ////Khanda-handled Mace
                                weaponWeenie = 41062;
                            }
                            if (subMaceType == 3)
                            {
                                ////Tetsubo
                                weaponWeenie = 46604;
                            }
                            WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)weaponWeenie);
                            spells = LootHelper.MeleeSpells;
                            wo.PropertiesSpellId.Clear();
                            if (spellCount > 0)
                            {
                                wo.UiEffects = UiEffects.Magical;
                                int[] itemSpell = new int[spellCount];
                                List<int> nums = new List<int>();
                                for (int num = 0; num < spells.Length; num++)
                                {
                                    nums.Add(num);
                                }
                                for (int j = 0; j < spellCount; j++)
                                {
                                    itemSpell[j] = rnd.Next(0, nums.Count);
                                    int spellLevel;
                                    int spellLevelChance = rnd.Next(0, 100);
                                    if (spellLevelChance > 95)
                                    {
                                        spellLevel = 3;
                                    }
                                    else if (spellLevelChance > 70)
                                    {
                                        spellLevel = 2;
                                    }
                                    else
                                    {
                                        spellLevel = 1;
                                    }
                                    var propSpell = new AceObjectPropertiesSpell
                                    {
                                        AceObjectId = wo.Guid.Full,
                                        SpellId = (uint)spells[itemSpell[j]][spellLevel]
                                    };
                                    Console.WriteLine("Writing a spell to the list...." + propSpell.SpellId);
                                    wo.PropertiesSpellId.Add(propSpell);
                                    Console.WriteLine(wo.PropertiesSpellId);
                                    nums.RemoveAt(itemSpell[j]);
                                }
                            }
                            ////Set the damage, damagevariance, damagae attack, and damage defence
                            wo.DamageType = 4;
                            int damage = rnd.Next(8, 18);
                            wo.Damage = damage;
                            double damageVariance = .1 * rnd.Next(1, 11);
                            wo.DamageVariance = damageVariance;
                            wo.DamageMod = 1.0;
                            ////Sets the speed of the weapon
                            wo.WeaponTime = 38;
                            ////Setting Weapon Defense Mod
                            int defenseChance = rnd.Next(1, 101);
                            float weaponDefense = 0;
                            if (defenseChance > 98)
                            {
                                weaponDefense = 3;
                            }
                            else if (defenseChance > 90)
                            {
                                weaponDefense = 2;
                            }
                            else if (defenseChance > 75)
                            {
                                weaponDefense = 1;
                            }
                            else
                            {
                                weaponDefense = 0;
                            }
                            wo.WeaponDefense = 1.0 + (weaponDefense * .01);
                            ////Setting Weapon Defense Mod
                            int attackChance = rnd.Next(1, 101);
                            float attackDefense = 0;
                            if (attackChance > 98)
                            {
                                attackDefense = 3;
                            }
                            else if (attackChance > 90)
                            {
                                attackDefense = 2;
                            }
                            else if (attackChance > 75)
                            {
                                attackDefense = 1;
                            }
                            else
                            {
                                attackDefense = 0;
                            }
                            wo.WeaponOffense = 1.0 + (attackDefense * .01);
                            ////Sets how many gems and what type are in the description. May affect value later on.
                            wo.GemCount = rnd.Next(1, 6);
                            wo.GemType = rnd.Next(10, 51);
                            ////Setting Material Type
                            double materialValueMod;
                            int itemMaterial = rnd.Next(0, 7);
                            if (itemMaterial == 0)
                            {
                                wo.MaterialType = Material.Brass;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 1)
                            {
                                wo.MaterialType = Material.Bronze;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 2)
                            {
                                wo.MaterialType = Material.Copper;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 3)
                            {
                                wo.MaterialType = Material.Gold;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 4)
                            {
                                wo.MaterialType = Material.Iron;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 5)
                            {
                                wo.MaterialType = Material.Silver;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 6)
                            {
                                wo.MaterialType = Material.Steel;
                                materialValueMod = 1.0;
                            }
                            ////Affects the workmanship of an item
                            int workmanshipChance = rnd.Next(1, 101);
                            float workmanship = 0;
                            if (workmanshipChance > 95)
                            {
                                workmanship = 5;
                            }
                            else if (workmanshipChance > 80)
                            {
                                workmanship = 4;
                            }
                            else if (workmanshipChance > 50)
                            {
                                workmanship = 3;
                            }
                            else if (workmanshipChance > 20)
                            {
                                workmanship = 2;
                            }
                            else
                            {
                                workmanship = 1;
                            }
                            wo.Workmanship = (int)workmanship;
                            ////Value... Not sure what the range is, so just using arbitrary numbers based on workmanship
                            int itemValue = (int)workmanship * rnd.Next(10, 40);
                            wo.Value = itemValue;
                            items.Add(wo);
                        }
                        if (subType == 2)
                        {
                            ////There are 1 subtypes of axes
                            int subUAType = rnd.Next(0, 1);
                            if (subUAType == 0)
                            {
                                ////Greataxe
                                weaponWeenie = 41052;
                            }
                            WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)weaponWeenie);
                            spells = LootHelper.MeleeSpells;
                            wo.PropertiesSpellId.Clear();
                            if (spellCount > 0)
                            {
                                wo.UiEffects = UiEffects.Magical;
                                int[] itemSpell = new int[spellCount];
                                List<int> nums = new List<int>();
                                for (int num = 0; num < spells.Length; num++)
                                {
                                    nums.Add(num);
                                }
                                for (int j = 0; j < spellCount; j++)
                                {
                                    itemSpell[j] = rnd.Next(0, nums.Count);
                                    int spellLevel;
                                    int spellLevelChance = rnd.Next(0, 100);
                                    if (spellLevelChance > 95)
                                    {
                                        spellLevel = 3;
                                    }
                                    else if (spellLevelChance > 70)
                                    {
                                        spellLevel = 2;
                                    }
                                    else
                                    {
                                        spellLevel = 1;
                                    }
                                    var propSpell = new AceObjectPropertiesSpell
                                    {
                                        AceObjectId = wo.Guid.Full,
                                        SpellId = (uint)spells[itemSpell[j]][spellLevel]
                                    };
                                    Console.WriteLine("Writing a spell to the list...." + propSpell.SpellId);
                                    wo.PropertiesSpellId.Add(propSpell);
                                    Console.WriteLine(wo.PropertiesSpellId);
                                    nums.RemoveAt(itemSpell[j]);
                                }
                            }
                            ////Set the damage, damagevariance, damagae attack, and damage defence
                            wo.DamageType = 1;
                            int damage = rnd.Next(8, 18);
                            wo.Damage = damage;
                            double damageVariance = .1 * rnd.Next(1, 11);
                            wo.DamageVariance = damageVariance;
                            wo.DamageMod = 1.0;
                            ////Sets the speed of the weapon
                            wo.WeaponTime = 48;
                            ////Setting Weapon Defense Mod
                            int defenseChance = rnd.Next(1, 101);
                            float weaponDefense = 0;
                            if (defenseChance > 98)
                            {
                                weaponDefense = 3;
                            }
                            else if (defenseChance > 90)
                            {
                                weaponDefense = 2;
                            }
                            else if (defenseChance > 75)
                            {
                                weaponDefense = 1;
                            }
                            else
                            {
                                weaponDefense = 0;
                            }
                            wo.WeaponDefense = 1.0 + (weaponDefense * .01);
                            ////Setting Weapon Defense Mod
                            int attackChance = rnd.Next(1, 101);
                            float attackDefense = 0;
                            if (attackChance > 98)
                            {
                                attackDefense = 3;
                            }
                            else if (attackChance > 90)
                            {
                                attackDefense = 2;
                            }
                            else if (attackChance > 75)
                            {
                                attackDefense = 1;
                            }
                            else
                            {
                                attackDefense = 0;
                            }
                            wo.WeaponOffense = 1.0 + (attackDefense * .01);
                            ////Sets how many gems and what type are in the description. May affect value later on.
                            wo.GemCount = rnd.Next(1, 6);
                            wo.GemType = rnd.Next(10, 51);
                            ////Setting Material Type
                            double materialValueMod;
                            int itemMaterial = rnd.Next(0, 7);
                            if (itemMaterial == 0)
                            {
                                wo.MaterialType = Material.Brass;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 1)
                            {
                                wo.MaterialType = Material.Bronze;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 2)
                            {
                                wo.MaterialType = Material.Copper;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 3)
                            {
                                wo.MaterialType = Material.Gold;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 4)
                            {
                                wo.MaterialType = Material.Iron;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 5)
                            {
                                wo.MaterialType = Material.Silver;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 6)
                            {
                                wo.MaterialType = Material.Steel;
                                materialValueMod = 1.0;
                            }
                            ////Affects the workmanship of an item
                            int workmanshipChance = rnd.Next(1, 101);
                            float workmanship = 0;
                            if (workmanshipChance > 95)
                            {
                                workmanship = 5;
                            }
                            else if (workmanshipChance > 80)
                            {
                                workmanship = 4;
                            }
                            else if (workmanshipChance > 50)
                            {
                                workmanship = 3;
                            }
                            else if (workmanshipChance > 20)
                            {
                                workmanship = 2;
                            }
                            else
                            {
                                workmanship = 1;
                            }
                            wo.Workmanship = (int)workmanship;
                            ////Value... Not sure what the range is, so just using arbitrary numbers based on workmanship
                            int itemValue = (int)workmanship * rnd.Next(10, 40);
                            wo.Value = itemValue;
                            items.Add(wo);
                        }
                        if (subType == 3)
                        {
                            ////There are 4 subtypes of spears
                            int subSpearType = rnd.Next(0, 4);
                            if (subSpearType == 0)
                            {
                                ////Assagai
                                weaponWeenie = 41036;
                            }
                            if (subSpearType == 1)
                            {
                                ////Pike
                                weaponWeenie = 41046;
                            }
                            if (subSpearType == 2)
                            {
                                ////Corsesca
                                weaponWeenie = 40818;
                            }
                            if (subSpearType == 3)
                            {
                                ////Magari Yari
                                weaponWeenie = 46605;
                            }
                            WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)weaponWeenie);
                            spells = LootHelper.MeleeSpells;
                            wo.PropertiesSpellId.Clear();
                            if (spellCount > 0)
                            {
                                wo.UiEffects = UiEffects.Magical;
                                int[] itemSpell = new int[spellCount];
                                List<int> nums = new List<int>();
                                for (int num = 0; num < spells.Length; num++)
                                {
                                    nums.Add(num);
                                }
                                for (int j = 0; j < spellCount; j++)
                                {
                                    itemSpell[j] = rnd.Next(0, nums.Count);
                                    int spellLevel;
                                    int spellLevelChance = rnd.Next(0, 100);
                                    if (spellLevelChance > 95)
                                    {
                                        spellLevel = 3;
                                    }
                                    else if (spellLevelChance > 70)
                                    {
                                        spellLevel = 2;
                                    }
                                    else
                                    {
                                        spellLevel = 1;
                                    }
                                    var propSpell = new AceObjectPropertiesSpell
                                    {
                                        AceObjectId = wo.Guid.Full,
                                        SpellId = (uint)spells[itemSpell[j]][spellLevel]
                                    };
                                    Console.WriteLine("Writing a spell to the list...." + propSpell.SpellId);
                                    wo.PropertiesSpellId.Add(propSpell);
                                    Console.WriteLine(wo.PropertiesSpellId);
                                    nums.RemoveAt(itemSpell[j]);
                                }
                            }
                            ////Set the damage, damagevariance, damagae attack, and damage defence
                            wo.DamageType = 2;
                            int damage = rnd.Next(8, 18);
                            wo.Damage = damage;
                            double damageVariance = .1 * rnd.Next(1, 11);
                            wo.DamageVariance = damageVariance;
                            wo.DamageMod = 1.0;
                            ////Sets the speed of the weapon
                            wo.WeaponTime = 50;
                            ////Setting Weapon Defense Mod
                            int defenseChance = rnd.Next(1, 101);
                            float weaponDefense = 0;
                            if (defenseChance > 98)
                            {
                                weaponDefense = 3;
                            }
                            else if (defenseChance > 90)
                            {
                                weaponDefense = 2;
                            }
                            else if (defenseChance > 75)
                            {
                                weaponDefense = 1;
                            }
                            else
                            {
                                weaponDefense = 0;
                            }
                            wo.WeaponDefense = 1.0 + (weaponDefense * .01);
                            ////Setting Weapon Defense Mod
                            int attackChance = rnd.Next(1, 101);
                            float attackDefense = 0;
                            if (attackChance > 98)
                            {
                                attackDefense = 3;
                            }
                            else if (attackChance > 90)
                            {
                                attackDefense = 2;
                            }
                            else if (attackChance > 75)
                            {
                                attackDefense = 1;
                            }
                            else
                            {
                                attackDefense = 0;
                            }
                            wo.WeaponOffense = 1.0 + (attackDefense * .01);
                            ////Sets how many gems and what type are in the description. May affect value later on.
                            wo.GemCount = rnd.Next(1, 6);
                            wo.GemType = rnd.Next(10, 51);
                            ////Setting Material Type
                            double materialValueMod;
                            int itemMaterial = rnd.Next(0, 7);
                            if (itemMaterial == 0)
                            {
                                wo.MaterialType = Material.Brass;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 1)
                            {
                                wo.MaterialType = Material.Bronze;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 2)
                            {
                                wo.MaterialType = Material.Copper;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 3)
                            {
                                wo.MaterialType = Material.Gold;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 4)
                            {
                                wo.MaterialType = Material.Iron;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 5)
                            {
                                wo.MaterialType = Material.Silver;
                                materialValueMod = 1.0;
                            }
                            if (itemMaterial == 6)
                            {
                                wo.MaterialType = Material.Steel;
                                materialValueMod = 1.0;
                            }
                            ////Affects the workmanship of an item
                            int workmanshipChance = rnd.Next(1, 101);
                            float workmanship = 0;
                            if (workmanshipChance > 95)
                            {
                                workmanship = 5;
                            }
                            else if (workmanshipChance > 80)
                            {
                                workmanship = 4;
                            }
                            else if (workmanshipChance > 50)
                            {
                                workmanship = 3;
                            }
                            else if (workmanshipChance > 20)
                            {
                                workmanship = 2;
                            }
                            else
                            {
                                workmanship = 1;
                            }
                            wo.Workmanship = (int)workmanship;
                            ////Value... Not sure what the range is, so just using arbitrary numbers based on workmanship
                            int itemValue = (int)workmanship * rnd.Next(10, 40);
                            wo.Value = itemValue;
                            items.Add(wo);
                        }
                    }
                    if (weaponType == 4)
                    {
                        ////Missile Weapons, of which there are 3 sub types
                        int subType = rnd.Next(0, 3);
                        if (subType == 0)
                        {
                            double maxVelocity = 0;
                            int weaponTime = 0;
                            ////There are 8 subtypes of Bows
                            int subBowType = rnd.Next(0, 7);
                            if (subBowType == 0)
                            {
                                ////Longbow
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
                            }
                            if (subBowType == 2)
                            {
                                ////Nayin
                                weaponWeenie = 334;
                                maxVelocity = 27.3;
                                weaponTime = 40;
                            }
                            if (subBowType == 3)
                            {
                                ////Shortbow
                                weaponWeenie = 307;
                                maxVelocity = 24.9;
                                weaponTime = 30;
                            }
                            if (subBowType == 4)
                            {
                                ////Shouyumi
                                weaponWeenie = 341;
                                maxVelocity = 24.9;
                                weaponTime = 29;
                            }
                            if (subBowType == 5)
                            {
                                ////War Bow
                                weaponWeenie = 30625;
                                maxVelocity = 27.3;
                                weaponTime = 43;
                            }
                            if (subBowType == 6)
                            {
                                ////Yag
                                weaponWeenie = 360;
                                maxVelocity = 24.9;
                                weaponTime = 24;
                            }
                            ////There is a Compound Bow that needs to be placed here but wasn't part of database
                            WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)weaponWeenie);
                            spells = LootHelper.MissileSpells;
                            wo.PropertiesSpellId.Clear();
                            if (spellCount > 0)
                            {
                                wo.UiEffects = UiEffects.Magical;
                                int[] itemSpell = new int[spellCount];
                                List<int> nums = new List<int>();
                                for (int num = 0; num < spells.Length; num++)
                                {
                                    nums.Add(num);
                                }
                                for (int j = 0; j < spellCount; j++)
                                {
                                    itemSpell[j] = rnd.Next(0, nums.Count);
                                    int spellLevel;
                                    int spellLevelChance = rnd.Next(0, 100);
                                    if (spellLevelChance > 95)
                                    {
                                        spellLevel = 3;
                                    }
                                    else if (spellLevelChance > 70)
                                    {
                                        spellLevel = 2;
                                    }
                                    else
                                    {
                                        spellLevel = 1;
                                    }
                                    var propSpell = new AceObjectPropertiesSpell
                                    {
                                        AceObjectId = wo.Guid.Full,
                                        SpellId = (uint)spells[itemSpell[j]][spellLevel]
                                    };
                                    Console.WriteLine("Writing a spell to the list...." + propSpell.SpellId);
                                    wo.PropertiesSpellId.Add(propSpell);
                                    Console.WriteLine(wo.PropertiesSpellId);
                                    nums.RemoveAt(itemSpell[j]);
                                }
                            }
                            wo.WeaponTime = weaponTime;
                            wo.MaximumVelocity = maxVelocity;
                            wo.DamageMod = 1.33;
                            wo.DamageVariance = 0;
                            wo.ObjScale = 0;
                            wo.WeaponOffense = 1.0;
                            wo.WeaponLength = 0;
                            wo.AmmoType = AmmoType.Arrow;
                            wo.AppraisalLongDescDecoration = 5;
                            wo.CombatUse = CombatUse.Missle;
                            wo.Damage = 0;
                            wo.DefaultCombatStyle = CombatStyle.Bow;
                            wo.Burden = (ushort)rnd.Next(200, 400);
                            wo.ParentLocation = 2;
                            wo.WeaponSkill = 47;
                            wo.WeaponType = 8;
                            ////Setting Weapon Defense Mod
                            int defenseChance = rnd.Next(1, 101);
                            float weaponDefense = 0;
                            if (defenseChance > 98)
                            {
                                weaponDefense = 3;
                            }
                            else if (defenseChance > 90)
                            {
                                weaponDefense = 2;
                            }
                            else if (defenseChance > 75)
                            {
                                weaponDefense = 1;
                            }
                            else
                            {
                                weaponDefense = 0;
                            }
                            wo.WeaponDefense = 1.0 + (weaponDefense * .01);
                            ////Setting Material Type
                            int itemMaterial = rnd.Next(0, 5);
                            if (itemMaterial == 0)
                            {
                                wo.MaterialType = Material.Ebony;
                            }
                            if (itemMaterial == 1)
                            {
                                wo.MaterialType = Material.Mahogany;
                            }
                            if (itemMaterial == 2)
                            {
                                wo.MaterialType = Material.Oak;
                            }
                            if (itemMaterial == 3)
                            {
                                wo.MaterialType = Material.Pine;
                            }
                            if (itemMaterial == 4)
                            {
                                wo.MaterialType = Material.Teak;
                            }
                            ////Affects the workmanship of an item
                            int workmanshipChance = rnd.Next(1, 101);
                            float workmanship = 0;
                            if (workmanshipChance > 95)
                            {
                                workmanship = 5;
                            }
                            else if (workmanshipChance > 80)
                            {
                                workmanship = 4;
                            }
                            else if (workmanshipChance > 50)
                            {
                                workmanship = 3;
                            }
                            else if (workmanshipChance > 20)
                            {
                                workmanship = 2;
                            }
                            else
                            {
                                workmanship = 1;
                            }
                            wo.Workmanship = (int)workmanship;
                            ////Sets how many gems and what type are in the description. May affect value later on.
                            wo.GemCount = rnd.Next(1, 6);
                            wo.GemType = rnd.Next(10, 51);
                            ////Value... Not sure what the range is, so just using arbitrary numbers based on workmanship
                            int itemValue = (int)workmanship * rnd.Next(10, 40);
                            wo.Value = itemValue;
                            items.Add(wo);
                        }
                        if (subType == 1)
                        {
                            double maxVelocity = 0;
                            int weaponTime = 0;
                            ////There are 4 subtypes of Crossbows
                            int subXbowType = rnd.Next(0, 3);
                            if (subXbowType == 0)
                            {
                                ////Arbalest
                                weaponWeenie = 47852;
                                maxVelocity = 27.3;
                                weaponTime = 113;
                            }
                            ////Compound Crossbow should go here, but not in the db
                            if (subXbowType == 1)
                            {
                                ////Heavy Crossbow
                                weaponWeenie = 311;
                                maxVelocity = 27.3;
                                weaponTime = 120;
                            }
                            if (subXbowType == 2)
                            {
                                ////Light Crossbow
                                weaponWeenie = 312;
                                maxVelocity = 24.9;
                                weaponTime = 58;
                            }
                            WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)weaponWeenie);
                            spells = LootHelper.MissileSpells;
                            wo.PropertiesSpellId.Clear();
                            if (spellCount > 0)
                            {
                                wo.UiEffects = UiEffects.Magical;
                                int[] itemSpell = new int[spellCount];
                                List<int> nums = new List<int>();
                                for (int num = 0; num < spells.Length; num++)
                                {
                                    nums.Add(num);
                                }
                                for (int j = 0; j < spellCount; j++)
                                {
                                    itemSpell[j] = rnd.Next(0, nums.Count);
                                    int spellLevel;
                                    int spellLevelChance = rnd.Next(0, 100);
                                    if (spellLevelChance > 95)
                                    {
                                        spellLevel = 3;
                                    }
                                    else if (spellLevelChance > 70)
                                    {
                                        spellLevel = 2;
                                    }
                                    else
                                    {
                                        spellLevel = 1;
                                    }
                                    var propSpell = new AceObjectPropertiesSpell
                                    {
                                        AceObjectId = wo.Guid.Full,
                                        SpellId = (uint)spells[itemSpell[j]][spellLevel]
                                    };
                                    Console.WriteLine("Writing a spell to the list...." + propSpell.SpellId);
                                    wo.PropertiesSpellId.Add(propSpell);
                                    Console.WriteLine(wo.PropertiesSpellId);
                                    nums.RemoveAt(itemSpell[j]);
                                }
                            }
                            wo.WeaponTime = weaponTime;
                            wo.MaximumVelocity = maxVelocity;
                            wo.DamageMod = 2.53;
                            wo.DamageVariance = 0;
                            wo.ObjScale = (float)1.25;
                            wo.WeaponOffense = 1.0;
                            wo.WeaponLength = 0;
                            wo.AmmoType = AmmoType.Bolt;
                            wo.AppraisalLongDescDecoration = 5;
                            wo.CombatUse = CombatUse.Missle;
                            wo.Damage = 0;
                            wo.DefaultCombatStyle = CombatStyle.Crossbow;
                            wo.Burden = (ushort)rnd.Next(800, 1400);
                            wo.ParentLocation = 2;
                            wo.WeaponSkill = 47;
                            wo.WeaponType = 9;
                            ////Setting Weapon Defense Mod
                            int defenseChance = rnd.Next(1, 101);
                            float weaponDefense = 0;
                            if (defenseChance > 98)
                            {
                                weaponDefense = 3;
                            }
                            else if (defenseChance > 90)
                            {
                                weaponDefense = 2;
                            }
                            else if (defenseChance > 75)
                            {
                                weaponDefense = 1;
                            }
                            else
                            {
                                weaponDefense = 0;
                            }
                            wo.WeaponDefense = 1.0 + (weaponDefense * .01);
                            ////Setting Material Type
                            int itemMaterial = rnd.Next(0, 5);
                            if (itemMaterial == 0)
                            {
                                wo.MaterialType = Material.Ebony;
                            }
                            if (itemMaterial == 1)
                            {
                                wo.MaterialType = Material.Mahogany;
                            }
                            if (itemMaterial == 2)
                            {
                                wo.MaterialType = Material.Oak;
                            }
                            if (itemMaterial == 3)
                            {
                                wo.MaterialType = Material.Pine;
                            }
                            if (itemMaterial == 4)
                            {
                                wo.MaterialType = Material.Teak;
                            }
                            ////Sets how many gems and what type are in the description. May affect value later on.
                            wo.GemCount = rnd.Next(1, 6);
                            wo.GemType = rnd.Next(10, 51);
                            ////Affects the workmanship of an item
                            int workmanshipChance = rnd.Next(1, 101);
                            float workmanship = 0;
                            if (workmanshipChance > 95)
                            {
                                workmanship = 5;
                            }
                            else if (workmanshipChance > 80)
                            {
                                workmanship = 4;
                            }
                            else if (workmanshipChance > 50)
                            {
                                workmanship = 3;
                            }
                            else if (workmanshipChance > 20)
                            {
                                workmanship = 2;
                            }
                            else
                            {
                                workmanship = 1;
                            }
                            wo.Workmanship = (int)workmanship;
                            ////Value... Not sure what the range is, so just using arbitrary numbers based on workmanship
                            int itemValue = (int)workmanship * rnd.Next(10, 40);
                            wo.Value = itemValue;
                            items.Add(wo);
                        }
                        if (subType == 2)
                        {
                            double maxVelocity = 0;
                            int weaponTime = 0;
                            ////There are 3 subtypes of Atlatl
                            int subAtlatlType = rnd.Next(0, 2);
                            if (subAtlatlType == 0)
                            {
                                ////Dart Flicker
                                weaponWeenie = 30345;
                                maxVelocity = 27.3;
                                weaponTime = 15;
                            }
                            if (subAtlatlType == 1)
                            {
                                ////Royal Atlatl
                                weaponWeenie = 20640;
                                maxVelocity = 27.3;
                                weaponTime = 15;
                            }
                            ////Regular slingshot should go here but not in the database
                            WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)weaponWeenie);
                            spells = LootHelper.MissileSpells;
                            wo.PropertiesSpellId.Clear();
                            if (spellCount > 0)
                            {
                                wo.UiEffects = UiEffects.Magical;
                                int[] itemSpell = new int[spellCount];
                                List<int> nums = new List<int>();
                                for (int num = 0; num < spells.Length; num++)
                                {
                                    nums.Add(num);
                                }
                                for (int j = 0; j < spellCount; j++)
                                {
                                    itemSpell[j] = rnd.Next(0, nums.Count);
                                    int spellLevel;
                                    int spellLevelChance = rnd.Next(0, 100);
                                    if (spellLevelChance > 95)
                                    {
                                        spellLevel = 3;
                                    }
                                    else if (spellLevelChance > 70)
                                    {
                                        spellLevel = 2;
                                    }
                                    else
                                    {
                                        spellLevel = 1;
                                    }
                                    var propSpell = new AceObjectPropertiesSpell
                                    {
                                        AceObjectId = wo.Guid.Full,
                                        SpellId = (uint)spells[itemSpell[j]][spellLevel]
                                    };
                                    Console.WriteLine("Writing a spell to the list...." + propSpell.SpellId);
                                    wo.PropertiesSpellId.Add(propSpell);
                                    Console.WriteLine(wo.PropertiesSpellId);
                                    nums.RemoveAt(itemSpell[j]);
                                }
                            }
                            wo.WeaponTime = weaponTime;
                            wo.MaximumVelocity = maxVelocity;
                            wo.DamageMod = 2;
                            wo.DamageVariance = 0;
                            wo.ObjScale = 0;
                            wo.WeaponOffense = 1.0;
                            wo.WeaponLength = 0;
                            wo.AmmoType = AmmoType.Atlatl;
                            wo.AppraisalLongDescDecoration = 5;
                            wo.CombatUse = CombatUse.Missle;
                            wo.Damage = 0;
                            wo.DefaultCombatStyle = CombatStyle.Atlatl;
                            wo.Burden = (ushort)rnd.Next(100, 300);
                            wo.ParentLocation = 2;
                            wo.WeaponSkill = 47;
                            wo.WeaponType = 10;
                            ////Setting Weapon Defense Mod
                            int defenseChance = rnd.Next(1, 101);
                            float weaponDefense = 0;
                            if (defenseChance > 98)
                            {
                                weaponDefense = 3;
                            }
                            else if (defenseChance > 90)
                            {
                                weaponDefense = 2;
                            }
                            else if (defenseChance > 75)
                            {
                                weaponDefense = 1;
                            }
                            else
                            {
                                weaponDefense = 0;
                            }
                            wo.WeaponDefense = 1.0 + (weaponDefense * .01);
                            ////Setting Material Type
                            int itemMaterial = rnd.Next(0, 5);
                            if (itemMaterial == 0)
                            {
                                wo.MaterialType = Material.Ebony;
                            }
                            if (itemMaterial == 1)
                            {
                                wo.MaterialType = Material.Mahogany;
                            }
                            if (itemMaterial == 2)
                            {
                                wo.MaterialType = Material.Oak;
                            }
                            if (itemMaterial == 3)
                            {
                                wo.MaterialType = Material.Pine;
                            }
                            if (itemMaterial == 4)
                            {
                                wo.MaterialType = Material.Teak;
                            }
                            ////Sets how many gems and what type are in the description. May affect value later on.
                            wo.GemCount = rnd.Next(1, 6);
                            wo.GemType = rnd.Next(10, 51);
                            ////Affects the workmanship of an item
                            int workmanshipChance = rnd.Next(1, 101);
                            float workmanship = 0;
                            if (workmanshipChance > 95)
                            {
                                workmanship = 5;
                            }
                            else if (workmanshipChance > 80)
                            {
                                workmanship = 4;
                            }
                            else if (workmanshipChance > 50)
                            {
                                workmanship = 3;
                            }
                            else if (workmanshipChance > 20)
                            {
                                workmanship = 2;
                            }
                            else
                            {
                                workmanship = 1;
                            }
                            wo.Workmanship = (int)workmanship;
                            ////Value... Not sure what the range is, so just using arbitrary numbers based on workmanship
                            int itemValue = (int)workmanship * rnd.Next(10, 40);
                            wo.Value = itemValue;
                            items.Add(wo);
                        }
                    }
                    if (weaponType == 5)
                    {
                        //// Magic Casters, of which there are 4 types.
                        int subType = rnd.Next(0, 4);
                        if (subType == 0)
                        {
                            ////Orb
                            weaponWeenie = 2366;
                        }
                        if (subType == 1)
                        {
                            ////Sceptre
                            weaponWeenie = 2548;
                        }
                        if (subType == 2)
                        {
                            ////staff
                            weaponWeenie = 2547;
                        }
                        if (subType == 3)
                        {
                            ////wand
                            weaponWeenie = 2472;
                        }
                        WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)weaponWeenie);
                        spells = LootHelper.WandSpells;
                        wo.PropertiesSpellId.Clear();
                        if (spellCount > 0)
                        {
                            wo.UiEffects = UiEffects.Magical;
                            int[] itemSpell = new int[spellCount];
                            List<int> nums = new List<int>();
                            for (int num = 0; num < spells.Length; num++)
                            {
                                nums.Add(num);
                            }
                            for (int j = 0; j < spellCount; j++)
                            {
                                itemSpell[j] = rnd.Next(0, nums.Count);
                                int spellLevel;
                                int spellLevelChance = rnd.Next(0, 100);
                                if (spellLevelChance > 95)
                                {
                                    spellLevel = 3;
                                }
                                else if (spellLevelChance > 70)
                                {
                                    spellLevel = 2;
                                }
                                else
                                {
                                    spellLevel = 1;
                                }
                                var propSpell = new AceObjectPropertiesSpell
                                {
                                    AceObjectId = wo.Guid.Full,
                                    SpellId = (uint)spells[itemSpell[j]][spellLevel]
                                };
                                Console.WriteLine("Writing a spell to the list...." + propSpell.SpellId);
                                wo.PropertiesSpellId.Add(propSpell);
                                Console.WriteLine(wo.PropertiesSpellId);
                                nums.RemoveAt(itemSpell[j]);
                            }
                        }
                        wo.ObjScale = (float)0.6;
                        wo.AppraisalLongDescDecoration = 3;
                        ////Setting Weapon Defense Mod
                        int manaChance = rnd.Next(1, 101);
                        double manaC = 0;
                        if (manaChance > 98)
                        {
                            manaC = 3;
                        }
                        else if (manaChance > 90)
                        {
                            manaC = 2;
                        }
                        else if (manaChance > 75)
                        {
                            manaC = 1;
                        }
                        else
                        {
                            manaC = 0;
                        }
                        if (manaC > 0)
                        {
                            wo.ManaConversionMod = manaC;
                        }
                        ////Setting Material Type
                        double materialValueMod;
                        int itemMaterial = rnd.Next(0, 7);
                        if (itemMaterial == 0)
                        {
                            wo.MaterialType = Material.Brass;
                            materialValueMod = 1.0;
                        }
                        if (itemMaterial == 1)
                        {
                            wo.MaterialType = Material.Bronze;
                            materialValueMod = 1.0;
                        }
                        if (itemMaterial == 2)
                        {
                            wo.MaterialType = Material.Copper;
                            materialValueMod = 1.0;
                        }
                        if (itemMaterial == 3)
                        {
                            wo.MaterialType = Material.Gold;
                            materialValueMod = 1.0;
                        }
                        if (itemMaterial == 4)
                        {
                            wo.MaterialType = Material.Iron;
                            materialValueMod = 1.0;
                        }
                        if (itemMaterial == 5)
                        {
                            wo.MaterialType = Material.Silver;
                            materialValueMod = 1.0;
                        }
                        if (itemMaterial == 6)
                        {
                            wo.MaterialType = Material.Steel;
                            materialValueMod = 1.0;
                        }
                        ////Sets how many gems and what type are in the description. May affect value later on.
                        wo.GemCount = rnd.Next(1, 6);
                        wo.GemType = rnd.Next(10, 51);
                        ////Affects the workmanship of an item
                        int workmanshipChance = rnd.Next(1, 101);
                        float workmanship = 0;
                        if (workmanshipChance > 95)
                        {
                            workmanship = 5;
                        }
                        else if (workmanshipChance > 80)
                        {
                            workmanship = 4;
                        }
                        else if (workmanshipChance > 50)
                        {
                            workmanship = 3;
                        }
                        else if (workmanshipChance > 20)
                        {
                            workmanship = 2;
                        }
                        else
                        {
                            workmanship = 1;
                        }
                        wo.Workmanship = (int)workmanship;
                        wo.LongDesc = wo.Name;
                        wo.ShortDesc = wo.Name;
                        if (wo.GemCount == 1)
                        {
                            wo.LongDesc += " set with one " + gemName[(int)wo.GemType - 10];
                        }
                        else
                        {
                            wo.LongDesc += " set with " + wo.GemCount + " " + gemName[(int)wo.GemType - 10];
                        }
                        ////Value... Not sure what the range is, so just using arbitrary numbers based on workmanship
                        int itemValue = (int)workmanship * rnd.Next(10, 40);
                        wo.Value = itemValue;
                        items.Add(wo);
                    }
                }
            }
            player.HandleAddNewWorldObjectsToInventory(items);
        }
    }
}
