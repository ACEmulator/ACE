using System.Linq;

using ACE.Common;
using ACE.Database.Models.World;
using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;
using ACE.Server.Factories.Tables;
using ACE.Server.WorldObjects;

namespace ACE.Server.Factories
{
    public static partial class LootGenerationFactory
    {
        /// <summary>
        /// Creates and optionally mutates a new MissileWeapon
        /// </summary>
        public static WorldObject CreateMissileWeapon(TreasureDeath profile, bool isMagical, bool mutate = true)
        {
            int weaponWeenie;

            int wieldDifficulty = RollWieldDifficulty(profile.Tier, TreasureWeaponType.MissileWeapon);

            // Changing based on wield, not tier. Refactored, less code, best results.  HarliQ 11/18/19
            if (wieldDifficulty < 315)
                weaponWeenie = GetNonElementalMissileWeapon();
            else
                weaponWeenie = GetElementalMissileWeapon();

            WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)weaponWeenie);

            if (wo != null && mutate)
                MutateMissileWeapon(wo, profile, isMagical, wieldDifficulty);
            
            return wo;
        }

        private static void MutateMissileWeapon(WorldObject wo, TreasureDeath profile, bool isMagical, int? wieldDifficulty = null, TreasureRoll roll = null)
        {
            if (roll == null)
            {
                // previous method

                // DamageMod
                wo.DamageMod = GetMissileDamageMod(wieldDifficulty.Value, wo.W_WeaponType);

                // ElementalDamageBonus
                if (wo.W_DamageType != DamageType.Undef)
                {
                    int elementalBonus = GetElementalDamageBonus(wieldDifficulty.Value);
                    if (elementalBonus > 0)
                        wo.ElementalDamageBonus = elementalBonus;
                }

                // Wield Requirements
                if (wieldDifficulty > 0)
                {
                    wo.WieldDifficulty = wieldDifficulty;
                    wo.WieldRequirements = WieldRequirement.RawSkill;
                    wo.WieldSkillType = (int)Skill.MissileWeapons;
                }
                else
                {
                    wo.WieldDifficulty = null;
                    wo.WieldRequirements = WieldRequirement.Invalid;
                    wo.WieldSkillType = null;
                }

                // WeaponDefense
                var meleeDMod = RollWeaponDefense(wieldDifficulty.Value, profile);
                if (meleeDMod > 0.0f)
                    wo.WeaponDefense = meleeDMod;
            }
            else
            {
                // new method / mutation scripts
                var isElemental = wo.W_DamageType != DamageType.Undef;

                var scriptName = GetMissileScript(roll.WeaponType, isElemental);

                // mutate DamageMod / ElementalDamageBonus / WieldRequirements
                var mutationFilter = MutationCache.GetMutation(scriptName);

                mutationFilter.TryMutate(wo, profile.Tier);

                // mutate WeaponDefense
                mutationFilter = MutationCache.GetMutation("MissileWeapons.weapon_defense.txt");

                mutationFilter.TryMutate(wo, profile.Tier);
            }

            // weapon speed
            if (wo.WeaponTime != null)
            {
                var weaponSpeedMod = RollWeaponSpeedMod(profile);
                wo.WeaponTime = (int)(wo.WeaponTime * weaponSpeedMod);
            }

            // material type
            var materialType = GetMaterialType(wo, profile.Tier);
            if (materialType > 0)
                wo.MaterialType = materialType;

            // item color
            MutateColor(wo);

            // gem count / gem material
            if (wo.GemCode != null)
                wo.GemCount = GemCountChance.Roll(wo.GemCode.Value, profile.Tier);
            else
                wo.GemCount = ThreadSafeRandom.Next(1, 5);

            wo.GemType = RollGemType(profile.Tier);

            // workmanship
            wo.ItemWorkmanship = WorkmanshipChance.Roll(profile.Tier);

            // burden
            MutateBurden(wo, profile, true);

            // missile / magic defense
            wo.WeaponMissileDefense = MissileMagicDefense.Roll(profile.Tier);
            wo.WeaponMagicDefense = MissileMagicDefense.Roll(profile.Tier);

            // spells
            if (!isMagical)
            {
                wo.ItemManaCost = null;
                wo.ItemMaxMana = null;
                wo.ItemCurMana = null;
                wo.ItemSpellcraft = null;
                wo.ItemDifficulty = null;
                wo.ManaRate = null;
            }
            else
                AssignMagic(wo, profile, roll);

            // item value
            //if (wo.HasMutateFilter(MutateFilter.Value))   // fixme: data
                MutateValue(wo, profile.Tier, roll);

            // long description
            wo.LongDesc = GetLongDesc(wo);
        }

        private static string GetMissileScript(TreasureWeaponType weaponType, bool isElemental = false)
        {
            var elementalStr = isElemental ? "elemental" : "non_elemental";

            return "MissileWeapons." + weaponType.GetScriptName() + "_" + elementalStr + ".txt";
        }

        private static bool GetMutateMissileWeaponData(uint wcid, int tier)
        {
            for (var isElemental = 0; isElemental < LootTables.MissileWeaponsMatrices.Count; isElemental++)
            {
                var table = LootTables.MissileWeaponsMatrices[isElemental];
                for (var missileType = 0; missileType < table.Length; missileType++)
                {
                    if (table[missileType].Contains((int)wcid))
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Get Missile Wield Index.
        /// </summary>
        private static int GetMissileWieldToIndex(int wieldDiff)
        {
            int index = 0;

            index = wieldDiff switch
            {
                250 => 1,
                270 => 2,
                290 => 3,
                315 => 4,
                335 => 5,
                360 => 6,
                375 => 7,
                385 => 8,
                _ => 0,  // Default/Else
            };
            return index;
        }

        /// <summary>
        /// Rolls for a DamageMod for missile weapons
        /// </summary>
        private static float GetMissileDamageMod(int wieldDiff, WeaponType weaponType)
        {
            // should this be setting defaults?
            if (weaponType == WeaponType.Undef)
                weaponType = WeaponType.Bow;

            var damageMod = weaponType switch
            {
                WeaponType.Bow => LootTables.MissileDamageMod[0][GetMissileWieldToIndex(wieldDiff)],
                WeaponType.Crossbow => LootTables.MissileDamageMod[1][GetMissileWieldToIndex(wieldDiff)],
                WeaponType.Thrown => LootTables.MissileDamageMod[2][GetMissileWieldToIndex(wieldDiff)],
                _ => 1.5f, // Default/Else
            };

            // Added variance for Damage Modifier.  Full Modifier was rare in retail
            int modChance = ThreadSafeRandom.Next(0, 99);
            if (modChance < 20)
                damageMod -= 0.09f;
            else if (modChance < 35)
                damageMod -= 0.08f;
            else if (modChance < 50)
                damageMod -= 0.07f;
            else if (modChance < 65)
                damageMod -= 0.06f;
            else if (modChance < 75)
                damageMod -= 0.05f;
            else if (modChance < 85)
                damageMod -= 0.04f;
            else if (modChance < 90)
                damageMod -= 0.03f;
            else if (modChance < 94)
                damageMod -= 0.02f;
            else if (modChance < 98)
                damageMod -= 0.01f;

            return damageMod;
        }

        /// <summary>
        /// Get Missile Elemental Damage based on Wield.
        /// </summary>
        /// <param name="wield"></param>
        /// <returns>Missile Weapon Wield Requirement</returns>
        private static int GetElementalDamageBonus(int wield)
        {
            int chance = 0;
            int eleMod = 0;
            switch (wield)
            {
                case 315:
                    chance = ThreadSafeRandom.Next(0, 99);
                    if (chance < 20)
                        eleMod = 1;
                    else if (chance < 40)
                        eleMod = 2;
                    else if (chance < 70)
                        eleMod = 3;
                    else if (chance < 95)
                        eleMod = 4;
                    else
                        eleMod = 5;
                    break;
                case 335:
                    chance = ThreadSafeRandom.Next(0, 99);
                    if (chance < 20)
                        eleMod = 5;
                    else if (chance < 40)
                        eleMod = 6;
                    else if (chance < 70)
                        eleMod = 7;
                    else if (chance < 95)
                        eleMod = 8;
                    else
                        eleMod = 9;
                    break;
                case 360:
                    chance = ThreadSafeRandom.Next(0, 99);
                    if (chance < 20)
                        eleMod = 10;
                    else if (chance < 30)
                        eleMod = 11;
                    else if (chance < 45)
                        eleMod = 12;
                    else if (chance < 60)
                        eleMod = 13;
                    else if (chance < 75)
                        eleMod = 14;
                    else if (chance < 95)
                        eleMod = 15;
                    else
                        eleMod = 16;
                    break;
                case 375: // Added +19 Elemental (like retail) and readjusted odds (odds are approximate, no hard data).  HarliQ 11/17/19  
                    chance = ThreadSafeRandom.Next(0, 99);
                    if (chance < 5)
                        eleMod = 12;
                    else if (chance < 15)
                        eleMod = 13;
                    else if (chance < 30)
                        eleMod = 14;
                    else if (chance < 50)
                        eleMod = 15;
                    else if (chance < 65)
                        eleMod = 16;
                    else if (chance < 80)
                        eleMod = 17;
                    else if (chance < 95)
                        eleMod = 18;
                    else
                        eleMod = 19;
                    break;
                case 385:
                    chance = ThreadSafeRandom.Next(0, 99);
                    if (chance < 20)
                        eleMod = 16;
                    else if (chance < 30)
                        eleMod = 17;
                    else if (chance < 45)
                        eleMod = 18;
                    else if (chance < 60)
                        eleMod = 19;
                    else if (chance < 75)
                        eleMod = 20;
                    else if (chance < 95)
                        eleMod = 21;
                    else
                        eleMod = 22;
                    break;
                default:
                    eleMod = 0;
                    break;
            }

            return eleMod;
        }

        /// <summary>
        /// Determines Type of Missile Weapon, and the element.
        /// </summary>
        /// <returns>Missile Type, Element</returns>
        private static int GetElementalMissileWeapon()
        {
            // Determine missile weapon type: 0 - Bow, 1 - Crossbows, 2 - Atlatl, 3 - Slingshot, 4 - Compound Bow, 5 - Compound Crossbow
            int missileType = ThreadSafeRandom.Next(0, 5);

            // Determine element type: 0 - Slashing, 1 - Piercing, 2 - Blunt, 3 - Frost, 4 - Fire, 5 - Acid, 6 - Electric
            int element = ThreadSafeRandom.Next(0, 6);

            return LootTables.ElementalMissileWeaponsMatrix[missileType][element];
        }

        /// <summary>
        /// Determines Non Elemental type of missile weapon (No Wields).
        /// </summary>
        /// <returns>Missile Weapon Type and SubType</returns>      
        private static int GetNonElementalMissileWeapon()
        {
            // Determine missile weapon type: 0 - Bow, 1 - Crossbows, 2 - Atlatl
            int missileType = ThreadSafeRandom.Next(0, 2);
            var subType = missileType switch
            {
                0 => ThreadSafeRandom.Next(0, 6),
                1 => ThreadSafeRandom.Next(0, 2),
                2 => ThreadSafeRandom.Next(0, 1),
                _ => 0, // Default/Else
            };
            return LootTables.NonElementalMissileWeaponsMatrix[missileType][subType];
        }
    }
}
