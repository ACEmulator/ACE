using ACE.Common;
using ACE.Database.Models.World;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.WorldObjects;

namespace ACE.Server.Factories
{
    public static partial class LootGenerationFactory
    {
        public static WorldObject CreateMissileWeapon(TreasureDeath profile, bool isMagical)
        {
            int weaponWeenie;
            int elemenatalBonus = 0;

            int wieldDifficulty = GetWield(profile.Tier, 1);

            // Changing based on wield, not tier. Refactored, less code, best results.  HarliQ 11/18/19
            if (wieldDifficulty < 315)
                weaponWeenie = GetNonElementalMissileWeapon();
            else
            {
                elemenatalBonus = GetElementalBonus(wieldDifficulty);
                weaponWeenie = GetElementalMissileWeapon();
            }

            WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)weaponWeenie);

            if (wo == null)
                return null;

            int workmanship = GetWorkmanship(profile.Tier);
            wo.SetProperty(PropertyInt.ItemWorkmanship, workmanship);
            int materialType = GetMaterialType(wo, profile.Tier);
            if (materialType > 0)
                wo.MaterialType = (MaterialType)materialType;
            wo.SetProperty(PropertyInt.GemCount, ThreadSafeRandom.Next(1, 5));
            wo.SetProperty(PropertyInt.GemType, ThreadSafeRandom.Next(10, 50));
            wo.SetProperty(PropertyString.LongDesc, wo.GetProperty(PropertyString.Name));

            double meleeDMod = GetWieldReqMeleeDMod(wieldDifficulty);
            // double meleeDMod = GetMeleeDMod(tier);
            if (meleeDMod > 0.0f)
                wo.SetProperty(PropertyFloat.WeaponDefense, meleeDMod);

            // MagicD/Missile Bonus
            wo.WeaponMagicDefense = GetMagicMissileDMod(profile.Tier); 
            wo.WeaponMissileDefense = GetMagicMissileDMod(profile.Tier);

            wo.SetProperty(PropertyFloat.DamageMod, GetMissileDamageMod(wieldDifficulty, wo.GetProperty(PropertyInt.WeaponType)));

            if (elemenatalBonus > 0)
                wo.SetProperty(PropertyInt.ElementalDamageBonus, elemenatalBonus);

            if (wieldDifficulty > 0)
            {
                wo.SetProperty(PropertyInt.WieldDifficulty, wieldDifficulty);
                wo.SetProperty(PropertyInt.WieldRequirements, (int)WieldRequirement.RawSkill);
                wo.SetProperty(PropertyInt.WieldSkillType, (int)Skill.MissileWeapons);
            }
            else
            {
                wo.RemoveProperty(PropertyInt.WieldDifficulty);
                wo.RemoveProperty(PropertyInt.WieldRequirements);
                wo.RemoveProperty(PropertyInt.WieldSkillType);
            }

            if (isMagical)
                wo = AssignMagic(wo, profile);
            else
            {
                wo.RemoveProperty(PropertyInt.ItemManaCost);
                wo.RemoveProperty(PropertyInt.ItemMaxMana);
                wo.RemoveProperty(PropertyInt.ItemCurMana);
                wo.RemoveProperty(PropertyInt.ItemSpellcraft);
                wo.RemoveProperty(PropertyInt.ItemDifficulty);
                wo.RemoveProperty(PropertyFloat.ManaRate);
            }

            double materialMod = LootTables.getMaterialValueModifier(wo);
            double gemMaterialMod = LootTables.getGemMaterialValueModifier(wo);
            var value = GetValue(profile.Tier, workmanship, gemMaterialMod, materialMod);
            wo.Value = value;

            wo = RandomizeColor(wo);
            return wo;
        }

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
                _ => 0,
            };
            return index;
        }

        private static float GetMissileDamageMod(int wieldDiff, int? missileType)
        {
            WeaponType weaponType = (WeaponType)(missileType ?? 8);

            const int bow = 0;
            const int crossbow = 1;
            const int thrown = 2;
            var damageMod = weaponType switch
            {
                WeaponType.Bow => LootTables.MissileDamageMod[bow][GetMissileWieldToIndex(wieldDiff)],
                WeaponType.Crossbow => LootTables.MissileDamageMod[crossbow][GetMissileWieldToIndex(wieldDiff)],
                WeaponType.Thrown => LootTables.MissileDamageMod[thrown][GetMissileWieldToIndex(wieldDiff)],
                _ => 1.5f,
            };
            // Added varaiance for Damage Modifier.  Full Modifier was rare in retail
            int modChance = ThreadSafeRandom.Next(0, 100);
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
        private static int GetElementalBonus(int wield)
        {
            int chance = 0;
            int eleMod = 0;
            switch (wield)
            {
                case 315:
                    chance = ThreadSafeRandom.Next(0, 100);
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
                    chance = ThreadSafeRandom.Next(0, 100);
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
                    chance = ThreadSafeRandom.Next(0, 100);
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
                    chance = ThreadSafeRandom.Next(0, 100);
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
                    chance = ThreadSafeRandom.Next(0, 100);
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

        private static int GetElementalMissileWeapon()
        {
            // Determine missile weapon type: 0 - Bow, 1 - Crossbows, 2 - Atlatl, 3 - Slingshot, 4 - Compound Bow, 5 - Compound Crossbow
            int missileType = ThreadSafeRandom.Next(0, 5);

            // Determine element type: 0 - Slashing, 1 - Piercing, 2 - Blunt, 3 - Frost, 4 - Fire, 5 - Acid, 6 - Electric
            int element = ThreadSafeRandom.Next(0, 6);

            return LootTables.ElementalMissileWeaponsMatrix[missileType][element];
        }

        private static int GetNonElementalMissileWeapon()
        {

            // Determine missile weapon type: 0 - Bow, 1 - Crossbows, 2 - Atlatl
            int missileType = ThreadSafeRandom.Next(0, 2);
            var subType = missileType switch
            {
                0 => ThreadSafeRandom.Next(0, 6),
                1 => ThreadSafeRandom.Next(0, 2),
                _ => ThreadSafeRandom.Next(0, 1),
            };
            return LootTables.NonElementalMissileWeaponsMatrix[missileType][subType];
        }
    }
}
