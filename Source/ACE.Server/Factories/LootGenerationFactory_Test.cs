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
        public static string TestLootGen(int numItems, int tier)
        {
            string dataToPrint = "Hits TestLootGen Class";

            Console.WriteLine($"Creating {numItems} items, that are in tier {tier}");

            // Counters
            float armorCount = 0;
            float meleeWeaponCount = 0;
            float casterCount = 0;
            float missileWeaponCount = 0;
            float jewelryCount = 0;
            float gemCount = 0;
            float clothingCount = 0;
            float otherCount = 0;
            float nullCount = 0;

            // Weapon Properties 
            double missileDefMod = 0.00f;
            double magicDefMod = 0.00f;
            double wield = 0.00f;
            int value = 0;
            int itemMaxMana = 0;
            int minMana = 50000;
            int maxMana = 0;
            int hasManaCount = 0;
            int totalMaxMana = 0;

            string meleeWeapons = $"-----Melee Weapons----\n Skill \t\t\t Wield \t Damage \t Variance \t DefenseMod \t MagicDBonus \t MissileDBonus\t Value\t Type \n";
            string missileWeapons = $"-----Missile Weapons----\n Type \t Wield \t Modifier \tElementBonus \t DefenseMod \t MagicDBonus \t MissileDBonus\t Value\n";
            string casterWeapons = $"-----Caster Weapons----\n Wield \t ElementBonus \t DefenseMod \t MagicDBonus \t MissileDBonus \t Value \t MaxMana\n";

            // Loop depending on how many items you are creating
            // string fileName = null;
            for (int i = 0; i < numItems; i++)
            {
                var testItem = LootGenerationFactory.CreateRandomLootObjects(tier, true);              
                if (testItem is null)
                {
                    nullCount++;
                    continue;
                }
                string itemType = testItem.ItemType.ToString();
                if (itemType == null)
                {
                    nullCount++;

                    continue;
                }

                switch (itemType)
                {
                    case "Armor":
                        armorCount++;
                        break;
                    case "MeleeWeapon":
                        meleeWeaponCount++;
                        if (testItem.WeaponMagicDefense != null)
                            magicDefMod = testItem.WeaponMagicDefense.Value;
                        if (testItem.Value != null)
                            value = testItem.Value.Value;
                        if (testItem.WeaponMissileDefense != null)
                            missileDefMod = testItem.WeaponMissileDefense.Value;
                        if (testItem.WieldDifficulty != null)
                            wield = testItem.WieldDifficulty.Value;
                        if (testItem.WeaponSkill == Skill.TwoHandedCombat)
                            meleeWeapons = meleeWeapons + $" {testItem.WeaponSkill}\t {wield}\t {testItem.Damage.Value}\t\t {testItem.DamageVariance.Value}\t\t {testItem.WeaponDefense.Value}\t\t {magicDefMod}\t\t {missileDefMod}\t\t {value}\t {testItem.Name}\n";
                        else
                            meleeWeapons = meleeWeapons + $" {testItem.WeaponSkill}\t\t {wield}\t {testItem.Damage.Value}\t\t {testItem.DamageVariance.Value}\t\t {testItem.WeaponDefense.Value}\t\t {magicDefMod}\t\t {missileDefMod}\t\t {value}\t {testItem.Name}\n";

                        break;
                    case "Caster":
                        casterCount++;
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
                            itemMaxMana = testItem.ItemMaxMana.Value;

                                casterWeapons = casterWeapons + $" {wield}\t {eleMod}\t\t {testItem.WeaponDefense.Value}\t\t  {magicDefMod}\t\t {missileDefMod}\t\t {value}\t {itemMaxMana}\n";
                        break;
                    case "MissileWeapon":
                        missileWeaponCount++;
                        double eleBonus = 0.00f;
                        string missileType = "Other";
                        switch (testItem.AmmoType.Value)
                        {
                            case ACE.Entity.Enum.AmmoType.None:
                                break;
                            case ACE.Entity.Enum.AmmoType.Arrow:
                                missileType = " Bow";
                                break;
                            case ACE.Entity.Enum.AmmoType.Bolt:
                                missileType = " X Bow";
                                break;
                            case ACE.Entity.Enum.AmmoType.Atlatl:
                                missileType = " Thrown";
                                break;
                            case ACE.Entity.Enum.AmmoType.ArrowCrystal:
                                break;
                            case ACE.Entity.Enum.AmmoType.BoltCrystal:
                                break;
                            case ACE.Entity.Enum.AmmoType.AtlatlCrystal:
                                break;
                            case ACE.Entity.Enum.AmmoType.ArrowChorizite:
                                break;
                            case ACE.Entity.Enum.AmmoType.BoltChorizite:
                                break;
                            case ACE.Entity.Enum.AmmoType.AtlatlChorizite:
                                break;
                            default:
                                break;
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

                        missileWeapons = missileWeapons + $"{missileType}\t {wield}\t {Math.Round(testItem.DamageMod.Value, 2)}\t\t{eleBonus}\t\t {testItem.WeaponDefense.Value}\t\t {magicDefMod}\t\t {missileDefMod}\t\t {value}\n";
                        break;
                    case "Jewelry":
                        jewelryCount++;
                        break;
                    case "Gem":
                        gemCount++;
                        break;
                    case "Clothing":
                        clothingCount++;
                        break;
                    default:
                        otherCount++;

                        break;
                }

                if (testItem.ItemMaxMana != null)
                {
                    if (testItem.ItemMaxMana > maxMana)
                        maxMana = testItem.ItemMaxMana.Value;
                    if (testItem.ItemMaxMana < minMana)
                        minMana = testItem.ItemMaxMana.Value;
                    hasManaCount++;
                    totalMaxMana = totalMaxMana + testItem.ItemMaxMana.Value;

                }

                if (testItem == null)
                {
                    Console.WriteLine("*Name is Null*");
                    continue;
                }
                else
                {
                }
            }
            float totalItemsGenerated = armorCount + meleeWeaponCount + casterCount + missileWeaponCount + jewelryCount + gemCount + clothingCount + otherCount;
            Console.WriteLine($" Armor={armorCount} \n " +
                                $"MeleeWeapon={meleeWeaponCount} \n " +
                                $"Caster={casterCount} \n " +
                                $"MissileWeapon={missileWeaponCount} \n " +
                                $"Jewelry={jewelryCount} \n " +
                                $"Gem={gemCount} \n " +
                                $"Clothing={clothingCount} \n " +
                                $"Other={otherCount} \n " +
                                $"NullCount={nullCount} \n " +
                                $"TotalGenerated={totalItemsGenerated}");
            Console.WriteLine();
            Console.WriteLine($" Drop Rates \n " +
                                $"Armor= {armorCount / totalItemsGenerated * 100}% \n " +
                                $"MeleeWeapon= {meleeWeaponCount / totalItemsGenerated * 100}% \n " +
                                $"Caster= {casterCount / totalItemsGenerated * 100}% \n " +
                                $"MissileWeapon= {missileWeaponCount / totalItemsGenerated * 100}% \n " +
                                $"Jewelry= {jewelryCount / totalItemsGenerated * 100}% \n " +
                                $"Gem= {gemCount / totalItemsGenerated * 100}% \n " +
                                $"Clothing= {clothingCount / totalItemsGenerated * 100}% \n " +
                                $"Other={otherCount / totalItemsGenerated * 100}% \n  ");

            Console.WriteLine(meleeWeapons);
            Console.WriteLine(missileWeapons);
            Console.WriteLine(casterWeapons);
            Console.WriteLine($" Mana capacity across all items Min={minMana}  Max={maxMana} Avg Mana={totalMaxMana / hasManaCount}");

            return dataToPrint;
        }
    }
}
