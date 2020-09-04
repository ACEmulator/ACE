using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using log4net;
using ACE.Common;
using ACE.Database;
using ACE.Database.Models.World;
using ACE.Database.Models.Shard;
using ACE.Entity.Enum;
using ACE.Server.Managers;
using ACE.Server.WorldObjects;
using ACE.Entity.Enum.Properties;

namespace ACE.Server.Factories
{
    public static class LootGenerationFactory_Test
    {
        public static string TestLootGen(int numItems, int tier, bool logstats, string displaytable)
        {
            string displayHeader = $"\n LootFactory Simulator - Items\n ---------------------\n";

            Console.WriteLine($"Creating {numItems} items, that are in tier {tier}");

            var ls = SetLootStatsDefaults(new LootStats(), logstats);

            // Create a dummy treasure profile for passing in tier value
            TreasureDeath profile = new TreasureDeath
            {
                Tier = tier,
                LootQualityMod = 0
            };

            // Loop depending on how many items you are creating
            for (int i = 0; i < numItems; i++)
            {
                var testItem = LootGenerationFactory.CreateRandomLootObjects(profile, true);
                ls = LootStats(testItem, ls, logstats);
            }
            Console.WriteLine(displayHeader);
            Console.WriteLine(DisplayStats(ls, displaytable));
            displayHeader += $" A total of {ls.TotalItems} items were generated in Tier {tier}. \n";
            if (logstats == true)
            {
                string myfilename = string.Format("LootSim-{0:hh-mm-ss-tt_MM-dd-yyyy}.csv", DateTime.Now);
                System.IO.File.WriteAllText(myfilename, displayHeader + DisplayStats(ls, displaytable));
            }

            return displayHeader;
        }
        public static string TestLootGenMonster(uint dtDID, int numberofcorpses, bool logstats, string displaytable)
        {
            string displayHeader = $"\n LootFactory Simulator - Corpses\n ---------------------\n";

            var corpseContainer = new List<WorldObject>();
            var ls = SetLootStatsDefaults(new LootStats(), logstats);

            Console.WriteLine($"Creating {numberofcorpses} corpses.");

            var deathTreasure = DatabaseManager.World.GetCachedDeathTreasure(dtDID);
            if (deathTreasure != null)
            {
                displayHeader += $" Loot profile {deathTreasure.Id} (a Tier {deathTreasure.Tier} profile) from DID {dtDID} was used for creating {numberofcorpses} corpses. \n";
            }
            else
            {
                displayHeader += $" DID {dtDID} you specified is invalid. \n";
                return displayHeader;
            }
            for (int i = 0; i < numberofcorpses; i++)
            {
                if (deathTreasure != null)
                {
                    corpseContainer = LootGenerationFactory.CreateRandomLootObjects(deathTreasure);
                    if (corpseContainer.Count < ls.MinItemsCreated)
                        ls.MinItemsCreated = corpseContainer.Count;
                    if (corpseContainer.Count > ls.MaxItemsCreated)
                        ls.MaxItemsCreated = corpseContainer.Count;

                    foreach (var lootItem in corpseContainer)
                    {
                        ls = LootStats(lootItem, ls, logstats);
                    }
                }
                else
                {
                }
            }
            Console.WriteLine(displayHeader);
            Console.WriteLine(DisplayStats(ls, displaytable));
            displayHeader += $" A total of {ls.TotalItems} unique items were generated. \n";
            if (logstats == true)
            {
                string myfilename = $"LootSim_DeathTreasureDID-{dtDID}" + string.Format("_{0:hh-mm-ss-tt_MM-dd-yyyy}.csv", DateTime.Now);
                System.IO.File.WriteAllText(myfilename, displayHeader + DisplayStats(ls, displaytable));
            }

            return displayHeader;
        }
        public static LootStats LootStats(WorldObject wo, LootStats ls, bool logstats)
        {
            // Weapon Properties 
            double missileDefMod = 0.00f;
            double magicDefMod = 0.00f;
            double wield = 0.00f;
            int value = 0;

            ls.TotalItems++;

            // Loop depending on how many items you are creating
            for (int i = 0; i < 1; i++)
            {
                var testItem = wo;
                if (testItem is null)
                {
                    ls.NullCount++;
                    continue;
                }
                string itemType = testItem.ItemType.ToString();
                if (itemType == null)
                {
                    ls.NullCount++;

                    continue;
                }

                switch (testItem.ItemType)
                {
                    case ItemType.None:
                        break;
                    case ItemType.MeleeWeapon:
                        ls.MeleeWeaponCount++;

                        bool cantrip = false;
                        if (testItem.EpicCantrips.Count > 0)
                        {
                            cantrip = true;
                        }
                        if (testItem.LegendaryCantrips.Count > 0)
                            cantrip = true;

                        string strikeType = "N";
                        if (testItem.WeaponMagicDefense != null)
                            magicDefMod = testItem.WeaponMagicDefense.Value;
                        if (testItem.Value != null)
                            value = testItem.Value.Value;
                        if (testItem.WeaponMissileDefense != null)
                            missileDefMod = testItem.WeaponMissileDefense.Value;
                        if (testItem.WieldDifficulty != null)
                            wield = testItem.WieldDifficulty.Value;
                        if (testItem.WeaponSkill == Skill.TwoHandedCombat)
                        {
                            if (logstats == true)
                            {
                                ls.MeleeWeapons += $"{testItem.WeaponSkill},{wield},{testItem.Damage.Value},{strikeType},{testItem.DamageVariance.Value},{testItem.WeaponDefense.Value},{magicDefMod},{missileDefMod},{cantrip},{value},{testItem.EncumbranceVal},{testItem.Name}\n";
                            }
                            else
                                ls.MeleeWeapons += $" {testItem.WeaponSkill}\t {wield}\t {testItem.Damage.Value}\t\t {strikeType} \t\t {testItem.DamageVariance.Value}\t\t {testItem.WeaponDefense.Value}\t\t {magicDefMod}\t\t {missileDefMod}\t\t {cantrip}\t{value}\t{testItem.EncumbranceVal} \t {testItem.Name}\n";
                        }
                        else
                        {
                            AttackType attackType = testItem.W_AttackType;
                            string at = attackType.ToString("F");
                            if (at.Contains("DoubleSlash") || at.Contains("DoubleThrust"))
                            {
                                strikeType = "2x";
                            }
                            else if (at.Contains("TripleSlash") || at.Contains("TripleThrust"))
                            {
                                strikeType = "3x";
                            }
                            if (logstats == true)
                            {
                                ls.MeleeWeapons += $"{testItem.WeaponSkill},{wield},{testItem.Damage.Value},{strikeType},{testItem.DamageVariance.Value},{testItem.WeaponDefense.Value},{magicDefMod},{missileDefMod},{cantrip},{value},{testItem.EncumbranceVal},{testItem.Name}\n";
                            }
                            else
                                ls.MeleeWeapons += $" {testItem.WeaponSkill}\t\t {wield}\t {testItem.Damage.Value}\t\t {strikeType}\t\t {testItem.DamageVariance.Value}\t\t {testItem.WeaponDefense.Value}\t\t {magicDefMod}\t\t {missileDefMod}\t\t {cantrip}\t{value}\t{testItem.EncumbranceVal} \t {testItem.Name}\n";
                        }
                        break;
                    case ItemType.Armor:
                        ls.ArmorCount++;
                        string equipmentSet = "None    ";
                        cantrip = false;
                        // float cantripSpells = 0;
                        var epicCantripSpells = testItem.EpicCantrips.Keys;

                        if (testItem.EpicCantrips.Count > 0)
                        {
                            cantrip = true;
                            
                        }
                            
                        if (testItem.LegendaryCantrips.Count > 0)
                            cantrip = true;

                        if (testItem.EquipmentSetId != null)
                            equipmentSet = System.Enum.GetName(typeof(EquipmentSet), testItem.EquipmentSetId);
                        if (logstats == true)
                        {
                            ls.Armor += $"{testItem.ArmorLevel},{testItem.ItemDifficulty},{testItem.Value.Value},{testItem.EncumbranceVal},{testItem.EpicCantrips.Count},{testItem.LegendaryCantrips.Count},{equipmentSet},{testItem.Name}\n";
                        }
                        else
                            ls.Armor += $" {testItem.ArmorLevel}\t{testItem.ItemDifficulty}\t{testItem.Value.Value}\t{testItem.EncumbranceVal}\t{testItem.EpicCantrips.Count}\t{testItem.LegendaryCantrips.Count}\t{equipmentSet}\t\t\t{testItem.Name}\n";
                        if (testItem.Name.Contains("Sheild"))
                            break;
                        if (testItem.ArmorLevel > ls.MaxAL)
                        {
                            ls.MaxAL = testItem.ArmorLevel.Value;
                            ls.MaxALItem = testItem.Name;
                        }
                        if (testItem.ArmorLevel < ls.MinAL)
                        {
                            ls.MinAL = testItem.ArmorLevel.Value;
                            ls.MinALItem = testItem.Name;
                        }
                        break;
                    case ItemType.Clothing:
                        if (testItem.Name.Contains("Cloak"))
                        {
                            string cloakSet = "None ";
                            if (testItem.EquipmentSetId != null)
                                cloakSet = System.Enum.GetName(typeof(EquipmentSet), testItem.EquipmentSetId);
                            ls.CloakCount++;
                            if (logstats == true)
                            {
                                ls.Cloaks += $"{testItem.ItemMaxLevel},{testItem.WieldDifficulty},{testItem.CloakWeaveProc.Value},{testItem.Value.Value},{cloakSet}\n";
                            }
                            else
                                ls.Cloaks += $" {testItem.ItemMaxLevel}\t {testItem.WieldDifficulty}\t {testItem.CloakWeaveProc.Value}\t {testItem.Value.Value}\t {cloakSet}\n";
                        }
                        else
                            ls.ClothingCount++;
                        break;
                    case ItemType.Jewelry:
                        ls.JewelryCount++;
                        string jewelrySlot = "";
                        if (testItem.Name.Contains("Necklace") || testItem.Name.Contains("Gorget") || testItem.Name.Contains("Amulet"))
                        {
                            ls.JewelryNecklaceCount++;
                            jewelrySlot = "Neck";
                        }
                        else if (testItem.Name.Contains("Bracelet"))
                        {
                            ls.JewelryBraceletCount++;
                            jewelrySlot = "Brace";
                        }
                        else if (testItem.Name.Contains("Ring"))
                        {
                            ls.JewelryRingCount++;
                            jewelrySlot = "Ring";
                        }
                        else if (testItem.Name.Contains("Compass") || testItem.Name.Contains("Goggles") || testItem.Name.Contains("Mechanical Scarab") || testItem.Name.Contains("Puzzle Box") || testItem.Name.Contains("Pocket Watch") || testItem.Name.Contains("Top"))
                        {
                            ls.JewelryTrinketCount++;
                            jewelrySlot = "Trink";
                        }
                        else
                        {
                            // Console.WriteLine(testItem.Name);                            
                        }
                        if (logstats == true)
                        {
                            ls.Jewelry += $"{jewelrySlot},{testItem.ItemDifficulty},{testItem.Value}\n";
                        }
                        else
                            ls.Jewelry += $" {jewelrySlot}\t {testItem.ItemDifficulty}\t {testItem.Value}\n";


                        break;
                    case ItemType.Creature:
                        break;
                    case ItemType.Food:
                        ls.Food++;
                        break;
                    case ItemType.Money:
                        break;
                    case ItemType.Misc:

                        string spirit = "Spirit";
                        string potionA = "Philtre";
                        string potionB = "Elixir";
                        string potionC = "Tonic";
                        string potionD = "Brew";
                        string potionE = "Potion";
                        string potionF = "Draught";
                        string potionG = "Tincture";

                        string healingKits = "Kit";
                        string spellcompGlyph = "Glyph";
                        string spellcompInk = "Ink";
                        string spellcompQuill = "Quill";

                        if (testItem.Name.Contains(spirit))
                        {
                            ls.Spirits++;
                        }
                        else if (testItem is PetDevice petDevice)
                        {
                            ls.PetsCount++;
                            int totalRatings = 0;
                            int damage = 0;
                            int damageResist = 0;
                            int crit = 0;
                            int critDamage = 0;
                            int critDamageResist = 0;
                            int critResist = 0;
                            int petLevel = 0;

                            if (petDevice.UseRequiresSkillLevel == 570)
                                petLevel = 200;
                            else if (petDevice.UseRequiresSkillLevel == 530)
                                petLevel = 180;
                            else if (petDevice.UseRequiresSkillLevel == 475)
                                petLevel = 150;
                            else if (petDevice.UseRequiresSkillLevel == 430)
                                petLevel = 125;
                            else if (petDevice.UseRequiresSkillLevel == 400)
                                petLevel = 100;
                            else if (petDevice.UseRequiresSkillLevel == 370)
                                petLevel = 80;
                            else if (petDevice.UseRequiresSkillLevel == 310)
                                petLevel = 50;

                            if (petDevice.GearDamage != null)
                            {
                                totalRatings += petDevice.GearDamage.Value;
                                damage = petDevice.GearDamage.Value;
                            }
                            if (petDevice.GearDamageResist != null)
                            {
                                totalRatings += petDevice.GearDamageResist.Value;
                                damageResist = petDevice.GearDamageResist.Value;
                            }
                            if (petDevice.GearCrit != null)
                            {
                                totalRatings += petDevice.GearCrit.Value;
                                crit = petDevice.GearCrit.Value;
                            }
                            if (petDevice.GearCritDamage != null)
                            {
                                totalRatings += petDevice.GearCritDamage.Value;
                                critDamage = petDevice.GearCritDamage.Value;
                            }
                            if (petDevice.GearCritDamageResist != null)
                            {
                                totalRatings += petDevice.GearCritDamageResist.Value;
                                critDamageResist = petDevice.GearCritDamageResist.Value;
                            }
                            if (petDevice.GearCritResist != null)
                            {
                                totalRatings += petDevice.GearCritResist.Value;
                                critResist = petDevice.GearCritResist.Value;
                            }
                            if (logstats == true)
                            {
                                ls.Pets += $"{petLevel},{damage},{damageResist},{crit},{critDamage},{critDamageResist},{critResist},{totalRatings}\n";
                            }
                            else
                                ls.Pets += $" {petLevel}\t {damage}\t {damageResist}\t {crit}\t {critDamage}\t {critDamageResist}\t {critResist}\t {totalRatings}\n";

                            if (totalRatings > 99)
                                ls.PetRatingsOverHundred++;
                            else if (totalRatings > 89)
                                ls.PetRatingsOverNinety++;
                            else if (totalRatings > 79)
                                ls.PetRatingsOverEighty++;
                            else if (totalRatings > 69)
                                ls.PetRatingsOverSeventy++;
                            else if (totalRatings > 59)
                                ls.PetRatingsOverSixty++;
                            else if (totalRatings > 49)
                                ls.PetRatingsOverFifty++;
                            else if (totalRatings > 39)
                                ls.PetRatingsOverForty++;
                            else if (totalRatings > 29)
                                ls.PetRatingsOverThirty++;
                            else if (totalRatings > 19)
                                ls.PetRatingsOverTwenty++;
                            else if (totalRatings > 9)
                                ls.PetRatingsOverTen++;
                            else if (totalRatings > 0)
                                ls.PetRatingsEqualOne++;
                            else if (totalRatings < 1)
                                ls.PetRatingsEqualZero++;
                        }
                        else if (testItem.Name.Contains(potionA) || testItem.Name.Contains(potionB) || testItem.Name.Contains(potionC) || testItem.Name.Contains(potionD) || testItem.Name.Contains(potionE) || testItem.Name.Contains(potionF) || testItem.Name.Contains(potionG))
                            ls.Poitions++;
                        else if (testItem.Name.Contains(spellcompGlyph) || testItem.Name.Contains(spellcompInk) || testItem.Name.Contains(spellcompQuill))
                            ls.LevelEightComp++;
                        else if (testItem.Name.Contains(healingKits))
                            ls.HealingKit++;
                        else
                        {
                            // Console.WriteLine($"ItemType.Misc Name={testItem.Name}");
                            ls.Misc++;
                        }
                        break;
                    case ItemType.MissileWeapon:
                        double eleBonus = 0.00f;
                        double damageMod = 0.00f;
                        string missileType = "Other";
                        if (testItem.AmmoType != null)
                        {
                            switch (testItem.AmmoType.Value)
                            {
                                case AmmoType.None:
                                    break;
                                case AmmoType.Arrow:
                                    missileType = "Bow";
                                    ls.MissileWeaponCount++;
                                    break;
                                case AmmoType.Bolt:
                                    missileType = "X Bow";
                                    ls.MissileWeaponCount++;
                                    break;
                                case AmmoType.Atlatl:
                                    missileType = "Thrown";
                                    ls.MissileWeaponCount++;
                                    break;
                                case AmmoType.ArrowCrystal:
                                    break;
                                case AmmoType.BoltCrystal:
                                    break;
                                case AmmoType.AtlatlCrystal:
                                    break;
                                case AmmoType.ArrowChorizite:
                                    break;
                                case AmmoType.BoltChorizite:
                                    break;
                                case AmmoType.AtlatlChorizite:
                                    break;
                                default:
                                    break;
                            }
                        }
                        if (testItem.WeaponMagicDefense != null)
                            magicDefMod = testItem.WeaponMagicDefense.Value;
                        if (testItem.Value != null)
                            value = testItem.Value.Value;
                        if (testItem.WeaponMissileDefense != null)
                            missileDefMod = testItem.WeaponMissileDefense.Value;
                        if (testItem.WieldDifficulty != null)
                            wield = testItem.WieldDifficulty.Value;
                        if (testItem.ElementalDamageBonus != null)
                            eleBonus = testItem.ElementalDamageBonus.Value;
                        if (testItem.DamageMod != null)
                            damageMod = testItem.DamageMod.Value;

                        if (missileType == "Other")
                        {
                            ls.DinnerWare++;
                        }
                        else
                        {
                            if (logstats == true)
                            {
                                ls.MissileWeapons += $"{missileType},{wield},{damageMod},{eleBonus},{testItem.WeaponDefense.Value},{magicDefMod},{missileDefMod},{value},{testItem.EncumbranceVal}\n";
                            }
                            else
                                ls.MissileWeapons += $"{missileType}\t {wield}\t {damageMod}\t\t{eleBonus}\t\t {testItem.WeaponDefense.Value}\t\t {magicDefMod}\t\t {missileDefMod}\t\t {value}\t {testItem.EncumbranceVal}\n";
                        }

                        break;
                    case ItemType.Container:
                        break;
                    case ItemType.Useless:
                        // Console.WriteLine($"ItemType.Useless Name={testItem.Name}");
                        break;
                    case ItemType.Gem:
                        string aetheriaColor = "None";
                        if (testItem.Name.Contains("Aetheria"))
                        {
                            ls.AetheriaCount++;
                            if (testItem.WieldDifficulty == 75)
                                aetheriaColor = "Blue  ";
                            else if (testItem.WieldDifficulty == 150)
                                aetheriaColor = "Yellow";
                            else if (testItem.WieldDifficulty == 225)
                                aetheriaColor = "Red   ";
                            if (logstats == true)
                            {
                                ls.Aetheria += $"{aetheriaColor},{testItem.ItemMaxLevel}\n";
                            }
                            else
                                ls.Aetheria += $" {aetheriaColor}\t {testItem.ItemMaxLevel}\n";
                        }
                        else
                            ls.GemCount++;
                        break;
                    case ItemType.SpellComponents:
                        ls.SpellComponents++;
                        break;
                    case ItemType.Writable:
                        string scrolls = "Scroll";

                        if (testItem.Name.Contains(scrolls))
                            ls.Scrolls++;
                        else
                            Console.WriteLine($"ItemType.Writeable Name={testItem.Name}");
                        break;
                    case ItemType.Key:
                        ls.Key++;
                        break;
                    case ItemType.Caster:
                        ls.CasterCount++;
                        double eleMod = 0.00f;
                        if (testItem.WeaponMagicDefense != null)
                            magicDefMod = testItem.WeaponMagicDefense.Value;
                        if (testItem.Value != null)
                            value = testItem.Value.Value;
                        if (testItem.WeaponMissileDefense != null)
                            missileDefMod = testItem.WeaponMissileDefense.Value;
                        if (testItem.WieldDifficulty != null)
                            wield = testItem.WieldDifficulty.Value;
                        if (testItem.ElementalDamageMod != null)
                            eleMod = testItem.ElementalDamageMod.Value;
                        if (testItem.ItemMaxMana != null)
                            ls.ItemMaxMana = testItem.ItemMaxMana.Value;
                        if (logstats == true)
                        {
                            ls.CasterWeapons += $"{wield},{eleMod},{testItem.WeaponDefense.Value},{magicDefMod},{missileDefMod},{value},{testItem.EncumbranceVal},{ls.ItemMaxMana}\n";
                        }
                        else
                            ls.CasterWeapons += $" {wield}\t {eleMod}\t\t {testItem.WeaponDefense.Value}\t\t  {magicDefMod}\t\t {missileDefMod}\t\t {value}\t {testItem.EncumbranceVal} \t {ls.ItemMaxMana}\n";
                        break;
                    case ItemType.Portal:
                        break;
                    case ItemType.Lockable:
                        break;
                    case ItemType.PromissoryNote:
                        break;
                    case ItemType.ManaStone:
                        ls.ManaStone++;
                        break;
                    case ItemType.Service:
                        break;
                    case ItemType.MagicWieldable:
                        break;
                    case ItemType.CraftCookingBase:
                        ls.OtherCount++;
                        break;
                    case ItemType.CraftAlchemyBase:
                        ls.OtherCount++;
                        break;
                    case ItemType.CraftFletchingBase:
                        ls.OtherCount++;
                        break;
                    case ItemType.CraftAlchemyIntermediate:
                        ls.OtherCount++;
                        break;
                    case ItemType.CraftFletchingIntermediate:
                        ls.OtherCount++;
                        break;
                    case ItemType.LifeStone:
                        break;
                    case ItemType.TinkeringTool:
                        ls.OtherCount++;
                        break;
                    case ItemType.TinkeringMaterial:
                        ls.OtherCount++;
                        break;
                    case ItemType.Gameboard:
                        break;
                    case ItemType.PortalMagicTarget:
                        break;
                    case ItemType.LockableMagicTarget:
                        break;
                    case ItemType.Vestements:
                        break;
                    case ItemType.Weapon:
                        break;
                    case ItemType.WeaponOrCaster:
                        break;
                    case ItemType.Item:
                        Console.WriteLine($"ItemType.item Name={testItem.Name}");
                        break;
                    case ItemType.RedirectableItemEnchantmentTarget:
                        break;
                    case ItemType.ItemEnchantableTarget:
                        break;
                    case ItemType.VendorShopKeep:
                        break;
                    case ItemType.VendorGrocer:
                        break;
                    default:
                        ls.OtherCount++;
                        break;
                }
                switch (itemType)
                {
                    case "Armor":
                        break;
                    case "MeleeWeapon":
                        break;
                    case "Caster":
                        break;
                    case "MissileWeapon":
                        break;
                    case "Jewelry":
                        break;
                    case "Gem":
                        break;
                    case "Clothing":
                        break;
                    default:

                        break;
                }
                if (testItem.ItemMaxMana != null)
                {
                    if (testItem.ItemMaxMana > ls.MaxMana)
                        ls.MaxMana = testItem.ItemMaxMana.Value;
                    if (testItem.ItemMaxMana < ls.MinMana)
                        ls.MinMana = testItem.ItemMaxMana.Value;
                    ls.HasManaCount++;
                    ls.TotalMaxMana += testItem.ItemMaxMana.Value;
                }
                if (testItem == null)
                {
                    Console.WriteLine("*Name is Null*");
                    continue;
                }
                else
                {
                }

                if (testItem.EpicCantrips.Count > 0)
                {
                    ls.EpicCantripCount++;
                }
                if (testItem.LegendaryCantrips.Count > 0)
                {
                    ls.LegendaryCantripCount++;
                }


            }
            return ls;
        }
        private static string DisplayStats(LootStats ls, string displaytable)
        {
            string displayStats = "";
            // Seeing if a table was specified to display
            switch (displaytable)
            {
                case "melee":
                    displayStats += ls.MeleeWeapons + $"\n";
                    break;
                case "missile":
                    displayStats += ls.MissileWeapons + $"\n";
                    break;
                case "caster":
                    displayStats += ls.CasterWeapons + $"\n";
                    break;
                case "jewelry":
                    displayStats += ls.Jewelry + $"\n";
                    break;
                case "armor":
                    displayStats += ls.Armor + $"\n";
                    break;
                case "cloak":
                    displayStats += ls.Cloaks + $"\n";
                    break;
                case "pet":
                    displayStats += ls.Pets + $"\n";
                    break;
                case "aetheria":
                    displayStats += ls.Aetheria + $"\n";
                    break;
                case "all":
                    displayStats += ls.MeleeWeapons + $"\n";
                    displayStats += ls.MissileWeapons + $"\n";
                    displayStats += ls.CasterWeapons + $"\n";
                    displayStats += ls.Jewelry + $"\n";
                    displayStats += ls.Armor + $"\n";
                    displayStats += ls.Cloaks + $"\n";
                    displayStats += ls.Pets + $"\n";
                    displayStats += ls.Aetheria + $"\n";
                    break;
                default:
                    displayStats += $"\n No Table(s) was selected to display, showing only general statistics";
                    break;
            }

            displayStats += ($" \n Treasure Items \n " +
                    $"---- \n " +
                    $"Armor={ls.ArmorCount} \n " +
                    $"MeleeWeapon={ls.MeleeWeaponCount} \n " +
                    $"Caster={ls.CasterCount} \n " +
                    $"MissileWeapon={ls.MissileWeaponCount} \n " +
                    $"Jewelry={ls.JewelryCount} \n " +
                    $"Gem={ls.GemCount} \n " +
                    $"Aetheria={ls.AetheriaCount} \n " +
                    $"Clothing={ls.ClothingCount} \n " +
                    $"Cloaks={ls.CloakCount} \n " +
                    $"\n Generic Items \n " +
                    $"---- \n " +
                    $"Food={ls.Food} \n " +
                    $"SpellComps={ls.SpellComponents} \n " +
                    $"Keys={ls.Key} \n " +
                    $"ManaStones={ls.ManaStone} \n " +
                    $"Pets={ls.PetsCount} \n " +
                    $"EncapSpirits={ls.Spirits} \n " +
                    $"Scrolls={ls.Scrolls} \n " +
                    $"Potions={ls.Poitions} \n " +
                    $"Healing Kits={ls.HealingKit} \n " +
                    $"Level 8 Comps={ls.LevelEightComp} \n " +
                    $"DinnerWare={ls.DinnerWare} \n " +
                    $"Misc={ls.Misc} \n " +
                    $"Other={ls.OtherCount} \n " +
                    $"NullCount={ls.NullCount} \n " +
                    $"Total Found={ls.ArmorCount + ls.MeleeWeaponCount + ls.CasterCount + ls.MissileWeaponCount + ls.JewelryCount + ls.GemCount + ls.ClothingCount + ls.Food + ls.SpellComponents + ls.Key + ls.ManaStone + ls.PetsCount + ls.Spirits + ls.Scrolls + ls.Poitions + ls.LevelEightComp + ls.HealingKit + ls.DinnerWare + ls.Misc + ls.OtherCount + ls.NullCount} \n " +
                    $"TotalGenerated={ls.TotalItems}\n");

            displayStats += $"\n Drop Rates \n ----\n " +
                                $"Armor= {ls.ArmorCount / ls.TotalItems * 100}% \n " +
                                $"MeleeWeapon= {ls.MeleeWeaponCount / ls.TotalItems * 100}% \n " +
                                $"Caster= {ls.CasterCount / ls.TotalItems * 100}% \n " +
                                $"MissileWeapon= {ls.MissileWeaponCount / ls.TotalItems * 100}% \n " +
                                $"Jewelry= {ls.JewelryCount / ls.TotalItems * 100}% \n " +
                                $"Gem= {ls.GemCount / ls.TotalItems * 100}% \n " +
                                $"Aetheria= {ls.AetheriaCount / ls.TotalItems * 100}% \n " +
                                $"Clothing= {ls.ClothingCount / ls.TotalItems * 100}% \n " +
                                $"Cloaks= {ls.CloakCount / ls.TotalItems * 100}% \n " +
                                $"Food= {ls.Food / ls.TotalItems * 100}% \n " +
                                $"SpellComps= {ls.SpellComponents / ls.TotalItems * 100}% \n " +
                                $"Keys= {ls.Key / ls.TotalItems * 100}% \n " +
                                $"Mana Stones= {ls.ManaStone / ls.TotalItems * 100}% \n " +
                                $"Pets= {ls.PetsCount / ls.TotalItems * 100}% \n " +
                                $"Encap. Spirits= {ls.Spirits / ls.TotalItems * 100}% \n " +
                                $"Scrolls= {ls.Scrolls / ls.TotalItems * 100}% \n " +
                                $"Potions= {ls.Poitions / ls.TotalItems * 100}% \n " +
                                $"Healing Kits= {ls.HealingKit / ls.TotalItems * 100}% \n " +
                                $"Level 8 Comps= {ls.LevelEightComp / ls.TotalItems * 100}% \n " +
                                $"DinnerWare= {ls.DinnerWare / ls.TotalItems * 100}% \n " +
                                $"Misc= {ls.Misc / ls.TotalItems * 100}% \n " +
                                $"Other={ls.OtherCount / ls.TotalItems * 100}% \n";

            // Armor Level Stats
            displayStats += $"\n Armor Levels \n ----\n MinAL = {ls.MinAL}\t {ls.MinALItem}\n MaxAL = {ls.MaxAL}\t {ls.MaxALItem}\n";

            // Pet Summons Stats
            displayStats += ($"\n Pets Ratings Stats \n ----\n " +
                                $"  100+ = {ls.PetRatingsOverHundred} \n" +
                                $"  90-99 = {ls.PetRatingsOverNinety} \n" +
                                $"  80-89 = {ls.PetRatingsOverEighty} \n" +
                                $"  70-79 = {ls.PetRatingsOverSeventy} \n" +
                                $"  60-69 = {ls.PetRatingsOverSixty} \n" +
                                $"  50-59 = {ls.PetRatingsOverFifty} \n" +
                                $"  40-49 = {ls.PetRatingsOverForty} \n" +
                                $"  30-39 = {ls.PetRatingsOverThirty} \n" +
                                $"  20-29 = {ls.PetRatingsOverTwenty} \n" +
                                $"  10-19 = {ls.PetRatingsOverTen} \n" +
                                $"    1-9 = {ls.PetRatingsEqualOne} \n" +
                                $"      0 = {ls.PetRatingsEqualZero} \n" +
                                $" Total Pets Generated = {ls.PetsCount} \n");
            // Jewelry
            displayStats += ($"\n Jewelry Counts Stats \n ----\n " +
                                $"Necklace = {ls.JewelryNecklaceCount}\t Droprate = {ls.JewelryNecklaceCount / ls.JewelryCount * 100}%\n" +
                                $" Bracelet = {ls.JewelryBraceletCount}\t Droprate = {ls.JewelryBraceletCount / ls.JewelryCount * 100}%\n" +
                                $"     Ring = {ls.JewelryRingCount}\t Droprate = {ls.JewelryRingCount / ls.JewelryCount * 100}%\n" +
                                $"  Trinket = {ls.JewelryTrinketCount}\t Droprate = {ls.JewelryTrinketCount / ls.JewelryCount * 100}%\n");
            // Cantrip Counts
            displayStats += ($"\n Cantip Counts Stats \n ----\n" +
                    //$"     Minor = {ls.MinorCantripCount}\t Droprate = {ls.MinorCantripCount / ls.TotalItems * 100}%\n" +
                    //$"     Major = {ls.MajorCantripCount}\t Droprate = {ls.MajorCantripCount / ls.TotalItems * 100}%\n" +
                    $"      Epic = {ls.EpicCantripCount}\t Droprate = {ls.EpicCantripCount / ls.TotalItems * 100}%\n" +
                    $" Legendary = {ls.LegendaryCantripCount}\t Droprate = {ls.LegendaryCantripCount / ls.TotalItems * 100}%\n");

            if (ls.HasManaCount == 0)
            {
            }
            else
            {
                displayStats += $"\n Mana capacity across all items Min={ls.MinMana}  Max={ls.MaxMana} Avg Mana={ls.TotalMaxMana / ls.HasManaCount}";
            }
            if (ls.MinItemsCreated == 100)
            { }
            else
            {
                displayStats += $"\n Min Items on a corpse = {ls.MinItemsCreated}, Max Items on coprse = {ls.MaxItemsCreated} \n";
            }
            return displayStats;
        }
        public static LootStats SetLootStatsDefaults(LootStats ls, bool logstats)
        {
            // Counters
            ls.ArmorCount = 0;
            ls.MeleeWeaponCount = 0;
            ls.CasterCount = 0;
            ls.MissileWeaponCount = 0;
            ls.JewelryCount = 0;
            ls.JewelryNecklaceCount = 0;
            ls.JewelryBraceletCount = 0;
            ls.JewelryRingCount = 0;
            ls.JewelryTrinketCount = 0;
            ls.GemCount = 0;
            ls.AetheriaCount = 0;
            ls.ClothingCount = 0;
            ls.OtherCount = 0;
            ls.NullCount = 0;
            ls.ItemMaxMana = 0;
            ls.MinMana = 50000;
            ls.MaxMana = 0;
            ls.HasManaCount = 0;
            ls.TotalMaxMana = 0;
            ls.MinItemsCreated = 100;
            ls.MaxItemsCreated = 0;
            ls.ManaStone = 0;
            ls.Key = 0;
            ls.Food = 0;
            ls.Misc = 0;
            ls.SpellComponents = 0;
            ls.TotalItems = 0;
            ls.Scrolls = 0;
            ls.PetsCount = 0;
            ls.Spirits = 0;
            ls.Poitions = 0;
            ls.HealingKit = 0;
            ls.DinnerWare = 0;
            ls.LevelEightComp = 0;
            ls.MinorCantripCount = 0;
            ls.MajorCantripCount = 0;
            ls.EpicCantripCount = 0;
            ls.LegendaryCantripCount = 0;
            ls.PetsTotalRatings = 0;
            ls.PetRatingsEqualZero = 0;
            ls.PetRatingsEqualOne = 0;
            ls.PetRatingsOverTen = 0;
            ls.PetRatingsOverTwenty = 0;
            ls.PetRatingsOverThirty = 0;
            ls.PetRatingsOverForty = 0;
            ls.PetRatingsOverFifty = 0;
            ls.PetRatingsOverSixty = 0;
            ls.PetRatingsOverSeventy = 0;
            ls.PetRatingsOverEighty = 0;
            ls.PetRatingsOverNinety = 0;
            ls.PetRatingsOverHundred = 0;
            ls.MinAL = 1000;
            ls.MaxAL = 0;
            ls.CloakCount = 0;

            // Tables
            if (logstats == true)
            {
                ls.MeleeWeapons = $"-----Melee Weapons----\nSkill,Wield,Damage,MStrike,Variance,DefenseMod,MagicDBonus,MissileDBonus,Cantrip,Value,Burden,Type\n";
            }
            else
                ls.MeleeWeapons = $"-----Melee Weapons----\n Skill \t\t\t Wield \t Damage \t MStrike \t Variance \t DefenseMod \t MagicDBonus \t MissileDBonus\tCantrip\t Value\t Burden\t Type \n";
            if (logstats == true)
            {
                ls.MissileWeapons = $"-----Missile Weapons----\nType,Wield,Modifier,ElementBonus,DefenseMod,MagicDBonus,MissileDBonus,Value,Burden\n";
            }
            else
                ls.MissileWeapons = $"-----Missile Weapons----\n Type \t Wield \t Modifier \tElementBonus \t DefenseMod \t MagicDBonus \t MissileDBonus\t Value\t Burden\n";
            if (logstats == true)
            {
                ls.CasterWeapons = $"-----Caster Weapons----\nWield,ElementBonus,DefenseMod,MagicDBonus,MissileDBonus,Value,MaxMana,Burden\n";
            }
            else
                ls.CasterWeapons = $"-----Caster Weapons----\n Wield \t ElementBonus \t DefenseMod \t MagicDBonus \t MissileDBonus \t Value\t Burden\t MaxMana\n";
            if (logstats == true)
            {
                ls.Armor = $"-----Armor----\nAL,Arcane,Value,Burden,Epic,Legendary,EquipmentSet,Type\n";
            }
            else
                ls.Armor = $"-----Armor----\n AL\tArcane\tValue\tBurden\tEpics\tLegend\tEquipment Set\t\t\tType\n";
            if (logstats == true)
            {
                ls.Pets = $"-----Pet Devices----\nLevel,Dmg,DmgR,Crit,CritD,CDR,CritR,Total\n";
            }
            else
                ls.Pets = $"-----Pet Devices----\n Level \t Dmg \t DmgR \t Crit \t CritD \t CDR \t CritR \t Total \n";
            if (logstats == true)
            {
                ls.Aetheria = $"-----Aetheria----\nColor,Level";
            }
            else
                ls.Aetheria = $"-----Aetheria----\n Color \t Level\n";
            if (logstats == true)
            {
                ls.Cloaks = $"-----Cloaks----\nLevel,Wield,Proc,Value,Set";
            }
            else
                ls.Cloaks = $"-----Cloaks----\n Level\t Wield\t Proc\t Value\t Set \n";
            if (logstats == true)
            {
                ls.Jewelry = $"-----Jewelry----\nSlot,Arcane,Value\n";
            }
            else
                ls.Jewelry = $"-----Jewelry----\n Slot\t Arcane\t Value\n";
            return ls;
        }
        public static string LogStats()
        {
            string test = "";
            return test;
        }
    }
}
