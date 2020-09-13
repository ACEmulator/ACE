using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using ACE.Database;
using ACE.Database.Models.World;
using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.WorldObjects;

namespace ACE.Server.Factories
{
    public static class LootGenerationFactory_Test
    {
        public static string TestLootGen(int numItems, int tier, bool logstats, string displaytable)
        {
            string displayHeader = $"\n LootFactory Simulator - Items\n ---------------------\n";

            Console.WriteLine($"Creating {numItems} items, that are in tier {tier}");

            var ls = new LootStats(logstats);

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
            if (logstats)
            {
                string myfilename = string.Format("LootSim-{0:hh-mm-ss-tt_MM-dd-yyyy}.csv", DateTime.Now);
                File.WriteAllText(myfilename, displayHeader + DisplayStats(ls, displaytable));
            }

            return displayHeader;
        }
        public static string TestLootGenMonster(uint dtDID, int numberofcorpses, bool logstats, string displaytable)
        {
            string displayHeader = $"\n LootFactory Simulator - Corpses\n ---------------------\n";

            var corpseContainer = new List<WorldObject>();
            var ls = new LootStats(logstats);

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
                        ls = LootStats(lootItem, ls, logstats);
                }
            }

            Console.WriteLine(displayHeader);
            Console.WriteLine(DisplayStats(ls, displaytable));
            displayHeader += $" A total of {ls.TotalItems} unique items were generated. \n";
            if (logstats)
            {
                string myfilename = $"LootSim_DeathTreasureDID-{dtDID}" + string.Format("_{0:hh-mm-ss-tt_MM-dd-yyyy}.csv", DateTime.Now);
                File.WriteAllText(myfilename, displayHeader + DisplayStats(ls, displaytable));
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
                if (testItem == null)
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
                            if (logstats)
                            {
                                ls.MeleeWeapons.Add($"{testItem.WeaponSkill},{wield},{testItem.Damage.Value},{strikeType},{testItem.DamageVariance.Value},{testItem.WeaponDefense.Value},{magicDefMod},{missileDefMod},{cantrip},{value},{testItem.EncumbranceVal},{testItem.Name}");
                            }
                            else
                                ls.MeleeWeapons.Add($"{testItem.WeaponSkill}\t {wield}\t {testItem.Damage.Value}\t\t {strikeType} \t\t {testItem.DamageVariance.Value}\t\t {testItem.WeaponDefense.Value}\t\t {magicDefMod}\t\t {missileDefMod}\t\t {cantrip}\t{value}\t{testItem.EncumbranceVal} \t {testItem.Name}");
                        }
                        else
                        {
                            if ((testItem.W_AttackType & AttackType.TripleStrike) != 0) 
                            {
                                strikeType = "3x";
                            }
                            else if ((testItem.W_AttackType & AttackType.DoubleStrike) != 0)
                            {
                                strikeType = "2x";
                            }
                            if (logstats)
                            {
                                ls.MeleeWeapons.Add($"{testItem.WeaponSkill},{wield},{testItem.Damage.Value},{strikeType},{testItem.DamageVariance.Value},{testItem.WeaponDefense.Value},{magicDefMod},{missileDefMod},{cantrip},{value},{testItem.EncumbranceVal},{testItem.Name}");
                            }
                            else
                                ls.MeleeWeapons.Add($" {testItem.WeaponSkill}\t\t {wield}\t {testItem.Damage.Value}\t\t {strikeType}\t\t {testItem.DamageVariance.Value}\t\t {testItem.WeaponDefense.Value}\t\t {magicDefMod}\t\t {missileDefMod}\t\t {cantrip}\t{value}\t{testItem.EncumbranceVal} \t {testItem.Name}");
                        }
                        break;
                    case ItemType.Armor:
                        ls.ArmorCount++;
                        string equipmentSet = "None    ";
                        cantrip = false;
                        // float cantripSpells = 0;
                        var epicCantripSpells = testItem.EpicCantrips.Keys;

                        if (testItem.EpicCantrips.Count > 0)
                            cantrip = true;
                            
                        if (testItem.LegendaryCantrips.Count > 0)
                            cantrip = true;

                        if (testItem.EquipmentSetId != null)
                            equipmentSet = testItem.EquipmentSetId.ToString();
                        if (logstats)
                        {
                            ls.Armor.Add($"{testItem.ArmorLevel},{testItem.ItemDifficulty},{testItem.Value.Value},{testItem.EncumbranceVal},{testItem.EpicCantrips.Count},{testItem.LegendaryCantrips.Count},{equipmentSet},{testItem.Name}");
                        }
                        else
                            ls.Armor.Add($" {testItem.ArmorLevel}\t{testItem.ItemDifficulty}\t{testItem.Value.Value}\t{testItem.EncumbranceVal}\t{testItem.EpicCantrips.Count}\t{testItem.LegendaryCantrips.Count}\t{equipmentSet}\t\t\t{testItem.Name}");
                        if (testItem.Name.Contains("Sheild"))   // typo?
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
                                cloakSet = testItem.EquipmentSetId.ToString();
                            ls.CloakCount++;
                            if (logstats)
                            {
                                ls.Cloaks.Add($"{testItem.ItemMaxLevel},{testItem.WieldDifficulty},{testItem.CloakWeaveProc.Value},{testItem.Value.Value},{cloakSet}");
                            }
                            else
                                ls.Cloaks.Add($" {testItem.ItemMaxLevel}\t {testItem.WieldDifficulty}\t {testItem.CloakWeaveProc.Value}\t {testItem.Value.Value}\t {cloakSet}");
                        }
                        else
                            ls.ClothingCount++;
                        break;
                    case ItemType.Jewelry:
                        ls.JewelryCount++;
                        string jewelrySlot = "";
                        switch (testItem.ValidLocations)
                        {
                            case EquipMask.NeckWear:
                                ls.JewelryNecklaceCount++;
                                jewelrySlot = "Neck";
                                break;
                            case EquipMask.WristWear:
                                ls.JewelryBraceletCount++;
                                jewelrySlot = "Brace";
                                break;
                            case EquipMask.FingerWear:
                                ls.JewelryRingCount++;
                                jewelrySlot = "Ring";
                                break;
                            case EquipMask.TrinketOne:
                                ls.JewelryTrinketCount++;
                                jewelrySlot = "Trink";
                                break;
                            default:
                                // Console.WriteLine(testItem.Name);                            
                                break;
                        }
                        if (logstats)
                        {
                            ls.Jewelry.Add($"{jewelrySlot},{testItem.ItemDifficulty},{testItem.Value}");
                        }
                        else
                            ls.Jewelry.Add($" {jewelrySlot}\t {testItem.ItemDifficulty}\t {testItem.Value}");


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
                            if (logstats)
                            {
                                ls.Pets.Add($"{petLevel},{damage},{damageResist},{crit},{critDamage},{critDamageResist},{critResist},{totalRatings}");
                            }
                            else
                                ls.Pets.Add($" {petLevel}\t {damage}\t {damageResist}\t {crit}\t {critDamage}\t {critDamageResist}\t {critResist}\t {totalRatings}");

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
                            ls.Potions++;
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
                            if (logstats)
                            {
                                ls.MissileWeapons.Add($"{missileType},{wield},{damageMod},{eleBonus},{testItem.WeaponDefense.Value},{magicDefMod},{missileDefMod},{value},{testItem.EncumbranceVal}");
                            }
                            else
                                ls.MissileWeapons.Add($"{missileType}\t {wield}\t {damageMod}\t\t{eleBonus}\t\t {testItem.WeaponDefense.Value}\t\t {magicDefMod}\t\t {missileDefMod}\t\t {value}\t {testItem.EncumbranceVal}");
                        }

                        break;
                    case ItemType.Container:
                        break;
                    case ItemType.Useless:
                        // Console.WriteLine($"ItemType.Useless Name={testItem.Name}");
                        break;
                    case ItemType.Gem:
                        string aetheriaColor = "None";
                        if (Aetheria.IsAetheria(testItem.WeenieClassId))
                        {
                            ls.AetheriaCount++;
                            if (testItem.WieldDifficulty == 75)
                                aetheriaColor = "Blue  ";
                            else if (testItem.WieldDifficulty == 150)
                                aetheriaColor = "Yellow";
                            else if (testItem.WieldDifficulty == 225)
                                aetheriaColor = "Red   ";
                            if (logstats)
                            {
                                ls.Aetheria.Add($"{aetheriaColor},{testItem.ItemMaxLevel}");
                            }
                            else
                                ls.Aetheria.Add($" {aetheriaColor}\t {testItem.ItemMaxLevel}");
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
                        if (logstats)
                        {
                            ls.CasterWeapons.Add($"{wield},{eleMod},{testItem.WeaponDefense.Value},{magicDefMod},{missileDefMod},{value},{testItem.EncumbranceVal},{ls.ItemMaxMana}");
                        }
                        else
                            ls.CasterWeapons.Add($" {wield}\t {eleMod}\t\t {testItem.WeaponDefense.Value}\t\t  {magicDefMod}\t\t {missileDefMod}\t\t {value}\t {testItem.EncumbranceVal} \t {ls.ItemMaxMana}");
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
                /*switch (testItem.ItemType)
                {
                    case ItemType.Armor:
                        break;
                    case ItemType.MeleeWeapon:
                        break;
                    case ItemType.Caster:
                        break;
                    case ItemType.MissileWeapon:
                        break;
                    case ItemType.Jewelry:
                        break;
                    case ItemType.Gem:
                        break;
                    case ItemType.Clothing:
                        break;
                    default:
                        break;
                }*/
                if (testItem.ItemMaxMana != null)
                {
                    if (testItem.ItemMaxMana > ls.MaxMana)
                        ls.MaxMana = testItem.ItemMaxMana.Value;
                    if (testItem.ItemMaxMana < ls.MinMana)
                        ls.MinMana = testItem.ItemMaxMana.Value;
                    ls.HasManaCount++;
                    ls.TotalMaxMana += testItem.ItemMaxMana.Value;
                }
                if (testItem.Name == null)
                {
                    Console.WriteLine("*Name is Null*");
                    continue;
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
            // fixme
            switch (displaytable)
            {
                case "melee":
                    displayStats += string.Join("\n", ls.MeleeWeapons) + $"\n";
                    break;
                case "missile":
                    displayStats += string.Join("\n", ls.MissileWeapons) + $"\n";
                    break;
                case "caster":
                    displayStats += string.Join("\n", ls.CasterWeapons) + $"\n";
                    break;
                case "jewelry":
                    displayStats += string.Join("\n", ls.Jewelry) + $"\n";
                    break;
                case "armor":
                    displayStats += string.Join("\n", ls.Armor) + $"\n";
                    break;
                case "cloak":
                    displayStats += string.Join("\n", ls.Cloaks) + $"\n";
                    break;
                case "pet":
                    displayStats += string.Join("\n", ls.Pets) + $"\n";
                    break;
                case "aetheria":
                    displayStats += string.Join("\n", ls.Aetheria) + $"\n";
                    break;
                case "all":
                    displayStats += string.Join("\n", ls.MeleeWeapons) + $"\n";
                    displayStats += string.Join("\n", ls.MissileWeapons) + $"\n";
                    displayStats += string.Join("\n", ls.CasterWeapons) + $"\n";
                    displayStats += string.Join("\n", ls.Jewelry) + $"\n";
                    displayStats += string.Join("\n", ls.Armor) + $"\n";
                    displayStats += string.Join("\n", ls.Cloaks) + $"\n";
                    displayStats += string.Join("\n", ls.Pets) + $"\n";
                    displayStats += string.Join("\n", ls.Aetheria) + $"\n";
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
                    $"Potions={ls.Potions} \n " +
                    $"Healing Kits={ls.HealingKit} \n " +
                    $"Level 8 Comps={ls.LevelEightComp} \n " +
                    $"DinnerWare={ls.DinnerWare} \n " +
                    $"Misc={ls.Misc} \n " +
                    $"Other={ls.OtherCount} \n " +
                    $"NullCount={ls.NullCount} \n " +
                    $"Total Found={ls.ArmorCount + ls.MeleeWeaponCount + ls.CasterCount + ls.MissileWeaponCount + ls.JewelryCount + ls.GemCount + ls.ClothingCount + ls.Food + ls.SpellComponents + ls.Key + ls.ManaStone + ls.PetsCount + ls.Spirits + ls.Scrolls + ls.Potions + ls.LevelEightComp + ls.HealingKit + ls.DinnerWare + ls.Misc + ls.OtherCount + ls.NullCount} \n " +
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
                                $"Potions= {ls.Potions / ls.TotalItems * 100}% \n " +
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
    }
}
