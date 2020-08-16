using System;
using System.Linq;

using ACE.Common;
using ACE.Database.Models.World;
using ACE.Entity.Enum;
using ACE.Server.WorldObjects;

namespace ACE.Server.Factories
{
    public static partial class LootGenerationFactory
    {
        /// <summary>
        /// Creates a Melee weapon object.
        /// </summary>
        /// <param name="profile"></param><param name="isMagical"></param>
        /// <returns>Returns Melee Weapon WO</returns>
        public static WorldObject CreateMeleeWeapon(TreasureDeath profile, bool isMagical, int weaponType = -1, bool mutate = true)
        {
            int weaponWeenie = 0;
            int subtype = 0;

            int eleType = ThreadSafeRandom.Next(0, 4);
            if (weaponType == -1)
                weaponType = ThreadSafeRandom.Next(0, 3);

            // Weapon Types
            // 0 = Heavy
            // 1 = Light
            // 2 = Finesse
            // default = Two Handed
            switch (weaponType)                
            {
                case 0:
                    // Heavy Weapons
                    subtype = ThreadSafeRandom.Next(0, LootTables.HeavyWeaponsMatrix.Length - 1);
                    weaponWeenie = LootTables.HeavyWeaponsMatrix[subtype][eleType];
                    break;
                case 1:
                    // Light Weapons
                    subtype = ThreadSafeRandom.Next(0, LootTables.LightWeaponsMatrix.Length - 1);
                    weaponWeenie = LootTables.LightWeaponsMatrix[subtype][eleType];
                    break;
                case 2:
                    // Finesse Weapons;
                    subtype = ThreadSafeRandom.Next(0, LootTables.FinesseWeaponsMatrix.Length - 1);
                    weaponWeenie = LootTables.FinesseWeaponsMatrix[subtype][eleType];
                    break;
                default:
                    // Two handed
                    subtype = ThreadSafeRandom.Next(0, LootTables.TwoHandedWeaponsMatrix.Length - 1);
                    weaponWeenie = LootTables.TwoHandedWeaponsMatrix[subtype][eleType];
                    break;
            }

            var wo = WorldObjectFactory.CreateNewWorldObject((uint)weaponWeenie);

            if (wo != null && mutate)
                if (!MutateMeleeWeapon(wo, profile, isMagical))
                    wo = null;

            return wo;
        }

        private static bool MutateMeleeWeapon(WorldObject wo, TreasureDeath profile, bool isMagical)
        {
            if ((wo.WeaponSkill != Skill.HeavyWeapons || wo.WeaponSkill != Skill.LightWeapons || wo.WeaponSkill != Skill.FinesseWeapons || wo.WeaponSkill != Skill.TwoHandedCombat)
                || (wo.W_WeaponType != WeaponType.Axe || wo.W_WeaponType != WeaponType.Mace || wo.W_WeaponType != WeaponType.Dagger || wo.W_WeaponType != WeaponType.Staff
                || wo.W_WeaponType != WeaponType.Sword || wo.W_WeaponType != WeaponType.Spear || wo.W_WeaponType != WeaponType.Unarmed || wo.W_WeaponType != WeaponType.TwoHanded)
                || (wo.W_AttackType == 0))
            {
                log.Warn($"[LOOT] Missing melee weapon properties on loot item {wo.WeenieClassId} - {wo.Name}");
                return false;
            }

            Skill wieldSkillType = wo.WeaponSkill;

            int damage = 0;
            double damageVariance = 0;
            double weaponDefense = 0;
            double weaponOffense = 0;

            // Properties for weapons
            double magicD = GetMagicMissileDMod(profile.Tier);
            double missileD = GetMagicMissileDMod(profile.Tier);
            int gemCount = ThreadSafeRandom.Next(1, 5);
            int gemType = ThreadSafeRandom.Next(10, 50);
            int workmanship = GetWorkmanship(profile.Tier);
            int wieldDiff = GetWieldDifficulty(profile.Tier, WieldType.MeleeWeapon);
            WieldRequirement wieldRequirments = WieldRequirement.RawSkill;

            switch (wieldSkillType)
            {
                case Skill.HeavyWeapons:
                    switch (wo.W_WeaponType)
                    {
                        case WeaponType.Axe:
                            weaponDefense = GetMaxDamageMod(profile.Tier, 18);
                            weaponOffense = GetMaxDamageMod(profile.Tier, 22);
                            damage = GetMeleeMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Axe);
                            damageVariance = GetVariance(wieldSkillType, LootWeaponType.Axe);
                            break;
                        case WeaponType.Dagger:
                            weaponDefense = GetMaxDamageMod(profile.Tier, 20);
                            weaponOffense = GetMaxDamageMod(profile.Tier, 20);

                            damage = GetMeleeMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Dagger);

                            if (wo.W_AttackType.IsMultiStrike())
                            {
                                damage = GetMeleeMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.DaggerMulti);
                                damageVariance = GetVariance(wieldSkillType, LootWeaponType.DaggerMulti);
                            }
                            else
                                damageVariance = GetVariance(wieldSkillType, LootWeaponType.Dagger);
                            break;
                        case WeaponType.Mace:
                            weaponDefense = GetMaxDamageMod(profile.Tier, 22);
                            weaponOffense = GetMaxDamageMod(profile.Tier, 18);
                            damage = GetMeleeMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Mace);
                            damageVariance = GetVariance(wieldSkillType, LootWeaponType.Mace);
                            break;
                        case WeaponType.Spear:
                            weaponDefense = GetMaxDamageMod(profile.Tier, 15);
                            weaponOffense = GetMaxDamageMod(profile.Tier, 25);
                            damage = GetMeleeMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Spear);
                            damageVariance = GetVariance(wieldSkillType, LootWeaponType.Spear);
                            break;
                        case WeaponType.Staff:
                            weaponDefense = GetMaxDamageMod(profile.Tier, 25);
                            weaponOffense = GetMaxDamageMod(profile.Tier, 15);
                            damage = GetMeleeMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Staff);
                            damageVariance = GetVariance(wieldSkillType, LootWeaponType.Staff);
                            break;
                        case WeaponType.Sword:
                            weaponDefense = GetMaxDamageMod(profile.Tier, 20);
                            weaponOffense = GetMaxDamageMod(profile.Tier, 20);

                            damage = GetMeleeMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Sword);
                            damageVariance = GetVariance(wieldSkillType, LootWeaponType.Sword);

                            if (wo.W_AttackType.IsMultiStrike())
                            {
                                damage = GetMeleeMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.SwordMulti);
                                damageVariance = GetVariance(wieldSkillType, LootWeaponType.SwordMulti);
                            }
                            break;
                        default:
                            weaponDefense = GetMaxDamageMod(profile.Tier, 20);
                            weaponOffense = GetMaxDamageMod(profile.Tier, 20);
                            damage = GetMeleeMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.UA);
                            damageVariance = GetVariance(wieldSkillType, LootWeaponType.UA);
                            break;
                    }
                    break;
                case Skill.LightWeapons:
                    switch (wo.W_WeaponType)
                    {
                        case WeaponType.Axe:
                            weaponDefense = GetMaxDamageMod(profile.Tier, 18);
                            weaponOffense = GetMaxDamageMod(profile.Tier, 22);
                            damage = GetMeleeMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Axe);
                            damageVariance = GetVariance(wieldSkillType, LootWeaponType.Axe);
                            break;
                        case WeaponType.Dagger:
                            weaponDefense = GetMaxDamageMod(profile.Tier, 20);
                            weaponOffense = GetMaxDamageMod(profile.Tier, 20);
                            damage = GetMeleeMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.DaggerMulti);
                            damageVariance = GetVariance(wieldSkillType, LootWeaponType.DaggerMulti);
                            break;
                        case WeaponType.Mace:
                            weaponDefense = GetMaxDamageMod(profile.Tier, 22);
                            weaponOffense = GetMaxDamageMod(profile.Tier, 18);
                            damage = GetMeleeMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Mace);
                            damageVariance = GetVariance(wieldSkillType, LootWeaponType.Mace);
                            break;
                        case WeaponType.Spear:
                            weaponDefense = GetMaxDamageMod(profile.Tier, 15);
                            weaponOffense = GetMaxDamageMod(profile.Tier, 25);
                            damage = GetMeleeMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Spear);
                            damageVariance = GetVariance(wieldSkillType, LootWeaponType.Spear);
                            break;
                        case WeaponType.Staff:
                            weaponDefense = GetMaxDamageMod(profile.Tier, 25);
                            weaponOffense = GetMaxDamageMod(profile.Tier, 15);
                            damage = GetMeleeMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Staff);
                            damageVariance = GetVariance(wieldSkillType, LootWeaponType.Staff);
                            break;
                        case WeaponType.Sword:
                            weaponDefense = GetMaxDamageMod(profile.Tier, 20);
                            weaponOffense = GetMaxDamageMod(profile.Tier, 20);

                            damage = GetMeleeMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Sword);
                            damageVariance = GetVariance(wieldSkillType, LootWeaponType.Sword);

                            if (wo.W_AttackType.IsMultiStrike())
                            {
                                damage = GetMeleeMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.SwordMulti);
                                damageVariance = GetVariance(wieldSkillType, LootWeaponType.SwordMulti);
                            }
                            break;
                        default:
                            weaponDefense = GetMaxDamageMod(profile.Tier, 20);
                            weaponOffense = GetMaxDamageMod(profile.Tier, 20);
                            damage = GetMeleeMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.UA);
                            damageVariance = GetVariance(wieldSkillType, LootWeaponType.UA);
                            break;
                    }
                    break;
                case Skill.FinesseWeapons:
                    switch (wo.W_WeaponType)
                    {
                        case WeaponType.Axe:
                            weaponDefense = GetMaxDamageMod(profile.Tier, 18);
                            weaponOffense = GetMaxDamageMod(profile.Tier, 22);
                            damage = GetMeleeMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Axe);
                            damageVariance = GetVariance(wieldSkillType, LootWeaponType.Axe);
                            break;
                        case WeaponType.Dagger:
                            weaponDefense = GetMaxDamageMod(profile.Tier, 20);
                            weaponOffense = GetMaxDamageMod(profile.Tier, 20);
                            damage = GetMeleeMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Dagger);
                            damageVariance = GetVariance(wieldSkillType, LootWeaponType.Dagger);

                            if (wo.W_AttackType.IsMultiStrike())
                            {
                                damageVariance = GetVariance(wieldSkillType, LootWeaponType.DaggerMulti);
                                damage = GetMeleeMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.DaggerMulti);
                            }
                            break;
                        case WeaponType.Mace:
                            weaponDefense = GetMaxDamageMod(profile.Tier, 22);
                            weaponOffense = GetMaxDamageMod(profile.Tier, 18);
                            damage = GetMeleeMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Mace);
                            damageVariance = GetVariance(wieldSkillType, LootWeaponType.Mace);

                            if (wo.Name.Any() != false && wo.Name.Contains("Jitte"))
                                damageVariance = GetVariance(wieldSkillType, LootWeaponType.Jitte);
                            break;
                        case WeaponType.Spear:
                            weaponDefense = GetMaxDamageMod(profile.Tier, 15);
                            weaponOffense = GetMaxDamageMod(profile.Tier, 25);
                            damage = GetMeleeMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Spear);
                            damageVariance = GetVariance(wieldSkillType, LootWeaponType.Spear);
                            break;
                        case WeaponType.Staff:
                            weaponDefense = GetMaxDamageMod(profile.Tier, 25);
                            weaponOffense = GetMaxDamageMod(profile.Tier, 15);
                            damage = GetMeleeMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Staff);
                            damageVariance = GetVariance(wieldSkillType, LootWeaponType.Staff);
                            break;
                        case WeaponType.Sword:
                            weaponDefense = GetMaxDamageMod(profile.Tier, 20);
                            weaponOffense = GetMaxDamageMod(profile.Tier, 20);
                            damage = GetMeleeMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Sword);
                            damageVariance = GetVariance(wieldSkillType, LootWeaponType.Sword);

                            if (wo.W_AttackType.IsMultiStrike())
                            {
                                damage = GetMeleeMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.SwordMulti);
                                damageVariance = GetVariance(wieldSkillType, LootWeaponType.SwordMulti);
                            }
                            break;
                        default:
                            weaponDefense = GetMaxDamageMod(profile.Tier, 20);
                            weaponOffense = GetMaxDamageMod(profile.Tier, 20);
                            damage = GetMeleeMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.UA);
                            damageVariance = GetVariance(wieldSkillType, LootWeaponType.UA);
                            break;
                    }
                    break;
                default: // Two handed
                    damage = GetMeleeMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Cleaving);
                    damageVariance = GetVariance(wieldSkillType, LootWeaponType.TwoHanded);

                    switch (wo.W_AttackType)
                    {
                        case AttackType.Thrust: // Assagai | Pike | Corsesca | Magari Yari
                            weaponDefense = GetMaxDamageMod(profile.Tier, 15);
                            weaponOffense = GetMaxDamageMod(profile.Tier, 25);
                            damage = GetMeleeMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Spears);
                            break;
                        case AttackType.Slash:
                            // Great Star Mace | Quadrelle | Khanda-handled Mace | Tetsubo
                            weaponDefense = GetMaxDamageMod(profile.Tier, 22);
                            weaponOffense = GetMaxDamageMod(profile.Tier, 18);

                            if ((wo.Name.Any() != false) && (wo.Name.Contains("Nodachi") || wo.Name.Contains("Shashqa") || wo.Name.Contains("Spadone")))
                            {
                                weaponDefense = GetMaxDamageMod(profile.Tier, 20);
                                weaponOffense = GetMaxDamageMod(profile.Tier, 20);
                            }

                            if (wo.SetupTableId >= 33560802 && wo.SetupTableId <= 33560807) // Greataxe
                            {
                                weaponDefense = GetMaxDamageMod(profile.Tier, 18);
                                weaponOffense = GetMaxDamageMod(profile.Tier, 22);
                            }
                            break;
                    }
                    break;
            }

            // Description
            //wo.AppraisalLongDescDecoration = AppraisalLongDescDecorations.PrependWorkmanship | AppraisalLongDescDecorations.AppendGemInfo;
            wo.LongDesc = wo.Name;

            // GemTypes, Material, Workmanship
            wo.GemCount = gemCount;
            wo.GemType = (MaterialType)gemType;
            int materialType = GetMaterialType(wo, profile.Tier);
            if (materialType > 0)
                wo.MaterialType = (MaterialType)materialType;
            wo.ItemWorkmanship = workmanship;

            // Burden
            MutateBurden(wo, profile.Tier, true);

            // Weapon Stats
            wo.Damage = damage;
            wo.DamageVariance = damageVariance;
            wo.WeaponDefense = weaponDefense;
            wo.WeaponOffense = weaponOffense;
            wo.WeaponMissileDefense = missileD;
            wo.WeaponMagicDefense = magicD;

            // Adding Wield Reqs if required
            if (wieldDiff > 0)
            {
                wo.WieldDifficulty = wieldDiff;
                wo.WieldRequirements = wieldRequirments;
                wo.WieldSkillType = (int)wieldSkillType;

            }
            else
            {
                // If no wield, remove wield reqs
                wo.WieldDifficulty = null;
                wo.WieldRequirements = WieldRequirement.Invalid;
                wo.WieldSkillType = null;
            }

            // Adding Magic Spells
            if (isMagical)
                wo = AssignMagic(wo, profile);
            else
            {
                // If no spells remove magic properites
                wo.ItemManaCost = null;
                wo.ItemMaxMana = null;
                wo.ItemCurMana = null;
                wo.ItemSpellcraft = null;
                wo.ItemDifficulty = null;

            }

            double materialMod = LootTables.getMaterialValueModifier(wo);
            double gemMaterialMod = LootTables.getGemMaterialValueModifier(wo);
            var value = GetValue(profile.Tier, workmanship, gemMaterialMod, materialMod);
            wo.Value = value;

            RandomizeColor(wo);

            return true;
        }

        private enum LootWeaponType
        {
            Axe,
            Dagger,
            DaggerMulti,
            Mace,
            Spear,
            Sword,
            SwordMulti,
            Staff,
            UA,
            Jitte,
            TwoHanded = 0,
            Cleaving = 0,
            Spears,
        }

        // The percentages for variances need to be fixed
        /// <summary>
        /// Gets Melee Weapon Variance
        /// </summary>
        /// <param name="category"></param><param name="type"></param>
        /// <returns>Returns Melee Weapon Variance</returns>
        private static double GetVariance(Skill category, LootWeaponType type)
        {
            double variance = 0;
            int chance = ThreadSafeRandom.Next(0, 99);

            switch (category)
            {
                case Skill.HeavyWeapons:
                    switch (type)
                    {
                        case LootWeaponType.Axe:
                            if (chance < 10)
                                variance = .90;
                            else if (chance < 30)
                                variance = .93;
                            else if (chance < 70)
                                variance = .95;
                            else if (chance < 90)
                                variance = .97;
                            else
                                variance = .99;
                            break;
                        case LootWeaponType.Dagger:
                            if (chance < 10)
                                variance = .47;
                            else if (chance < 30)
                                variance = .50;
                            else if (chance < 70)
                                variance = .53;
                            else if (chance < 90)
                                variance = .57;
                            else
                                variance = .62;
                            break;
                        case LootWeaponType.DaggerMulti:
                            if (chance < 10)
                                variance = .40;
                            else if (chance < 30)
                                variance = .43;
                            else if (chance < 70)
                                variance = .48;
                            else if (chance < 90)
                                variance = .53;
                            else
                                variance = .58;
                            break;
                        case LootWeaponType.Mace:
                            if (chance < 10)
                                variance = .30;
                            else if (chance < 30)
                                variance = .33;
                            else if (chance < 70)
                                variance = .37;
                            else if (chance < 90)
                                variance = .42;
                            else
                                variance = .46;
                            break;
                        case LootWeaponType.Spear:
                            if (chance < 10)
                                variance = .59;
                            else if (chance < 30)
                                variance = .63;
                            else if (chance < 70)
                                variance = .68;
                            else if (chance < 90)
                                variance = .72;
                            else
                                variance = .75;
                            break;
                        case LootWeaponType.Staff:
                            if (chance < 10)
                                variance = .38;
                            else if (chance < 30)
                                variance = .42;
                            else if (chance < 70)
                                variance = .45;
                            else if (chance < 90)
                                variance = .50;
                            else
                                variance = .52;
                            break;
                        case LootWeaponType.Sword:
                            if (chance < 10)
                                variance = .47;
                            else if (chance < 30)
                                variance = .50;
                            else if (chance < 70)
                                variance = .53;
                            else if (chance < 90)
                                variance = .57;
                            else
                                variance = .62;
                            break;
                        case LootWeaponType.SwordMulti:
                            if (chance < 10)
                                variance = .40;
                            else if (chance < 30)
                                variance = .43;
                            else if (chance < 70)
                                variance = .48;
                            else if (chance < 90)
                                variance = .53;
                            else
                                variance = .60;
                            break;
                        case LootWeaponType.UA:
                            if (chance < 10)
                                variance = .44;
                            else if (chance < 30)
                                variance = .48;
                            else if (chance < 70)
                                variance = .53;
                            else if (chance < 90)
                                variance = .58;
                            else
                                variance = .60;
                            break;
                    }
                    break;
                case Skill.LightWeapons:
                case Skill.FinesseWeapons:
                    switch (type)
                    {
                        case LootWeaponType.Axe:
                            // Axe
                            if (chance < 10)
                                variance = .80;
                            else if (chance < 30)
                                variance = .83;
                            else if (chance < 70)
                                variance = .85;
                            else if (chance < 90)
                                variance = .90;
                            else
                                variance = .95;
                            break;
                        case LootWeaponType.Dagger:
                            // Dagger
                            if (chance < 10)
                                variance = .42;
                            else if (chance < 30)
                                variance = .47;
                            else if (chance < 70)
                                variance = .52;
                            else if (chance < 90)
                                variance = .56;
                            else
                                variance = .60;
                            break;
                        case LootWeaponType.DaggerMulti:
                            // Dagger MultiStrike
                            if (chance < 10)
                                variance = .24;
                            else if (chance < 30)
                                variance = .28;
                            else if (chance < 70)
                                variance = .35;
                            else if (chance < 90)
                                variance = .40;
                            else
                                variance = .45;
                            break;
                        case LootWeaponType.Mace:
                            // Mace
                            if (chance < 10)
                                variance = .23;
                            else if (chance < 30)
                                variance = .28;
                            else if (chance < 70)
                                variance = .32;
                            else if (chance < 90)
                                variance = .37;
                            else
                                variance = .43;
                            break;
                        case LootWeaponType.Jitte:
                            // Jitte
                            if (chance < 10)
                                variance = .325;
                            else if (chance < 30)
                                variance = .35;
                            else if (chance < 70)
                                variance = .40;
                            else if (chance < 90)
                                variance = .45;
                            else
                                variance = .50;
                            break;
                        case LootWeaponType.Spear:
                            // Spear
                            if (chance < 10)
                                variance = .65;
                            else if (chance < 30)
                                variance = .68;
                            else if (chance < 70)
                                variance = .71;
                            else if (chance < 90)
                                variance = .75;
                            else
                                variance = .80;
                            break;
                        case LootWeaponType.Staff:
                            // Staff
                            if (chance < 10)
                                variance = .325;
                            else if (chance < 30)
                                variance = .35;
                            else if (chance < 70)
                                variance = .40;
                            else if (chance < 90)
                                variance = .45;
                            else
                                variance = .50;
                            break;
                        case LootWeaponType.Sword:
                            // Sword
                            if (chance < 10)
                                variance = .42;
                            else if (chance < 30)
                                variance = .47;
                            else if (chance < 70)
                                variance = .52;
                            else if (chance < 90)
                                variance = .56;
                            else
                                variance = .60;
                            break;
                        case LootWeaponType.SwordMulti:
                            // Sword Multistrike
                            if (chance < 10)
                                variance = .24;
                            else if (chance < 30)
                                variance = .28;
                            else if (chance < 70)
                                variance = .35;
                            else if (chance < 90)
                                variance = .40;
                            else
                                variance = .45;
                            break;
                        case LootWeaponType.UA:
                            // UA
                            if (chance < 10)
                                variance = .44;
                            else if (chance < 30)
                                variance = .48;
                            else if (chance < 70)
                                variance = .53;
                            else if (chance < 90)
                                variance = .58;
                            else
                                variance = .60;
                            break;
                    }
                    break;
                case Skill.TwoHandedCombat:
                    // Two Handed only have one set of variances
                    if (chance < 5)
                        variance = .30;
                    else if (chance < 20)
                        variance = .35;
                    else if (chance < 50)
                        variance = .40;
                    else if (chance < 80)
                        variance = .45;
                    else if (chance < 95)
                        variance = .50;
                    else
                        variance = .55;
                    break;
                default:
                    return 0;
            }

            return variance;
        }

        /// <summary>
        /// Gets Melee Weapon Index
        /// </summary>
        private static int GetMeleeWieldToIndex(int wieldDiff)
        {
            int index = 0;

            switch (wieldDiff)
            {
                case 250:
                    index = 1;
                    break;
                case 300:
                    index = 2;
                    break;
                case 325:
                    index = 3;
                    break;
                case 350:
                    index = 4;
                    break;
                case 370:
                    index = 5;
                    break;
                case 400:
                    index = 6;
                    break;
                case 420:
                    index = 7;
                    break;
                case 430:
                    index = 8;
                    break;
                default:
                    index = 0;
                    break;
            }

            return index;
        }

        /// <summary>
        /// Gets Melee Weapon Max Damage
        /// </summary>
        /// <param name="weaponType"></param><param name="wieldDiff"></param><param name="baseWeapon"></param>
        /// <returns>Melee Weapon Max Damage</returns>
        private static int GetMeleeMaxDamage(Skill weaponType, int wieldDiff, LootWeaponType baseWeapon)
        {
            int damageTable = 0;

            switch (weaponType)
            {
                case Skill.HeavyWeapons:
                    damageTable = LootTables.HeavyWeaponDamageTable[(int)baseWeapon, GetMeleeWieldToIndex(wieldDiff)];
                    break;
                case Skill.FinesseWeapons:
                case Skill.LightWeapons:
                    damageTable = LootTables.LightWeaponDamageTable[(int)baseWeapon, GetMeleeWieldToIndex(wieldDiff)];
                    break;
                case Skill.TwoHandedCombat:
                    damageTable = LootTables.TwoHandedWeaponDamageTable[(int)baseWeapon, GetMeleeWieldToIndex(wieldDiff)];
                    break;
                default:
                    return 0;
            }

            // To add a little bit of randomness to Max weapon damage
            int maxDamageVariance = ThreadSafeRandom.Next(-4, 0);

            return damageTable + maxDamageVariance;
        }

        private static bool GetMutateMeleeWeaponData(uint wcid)
        {
            // linear search = slow... but this is only called for /lootgen
            // if this ever needs to be fast, create a lookup table

            for (int weaponType = 0; weaponType < LootTables.MeleeWeaponsMatrices.Count; weaponType++)
            {
                var lootTable = LootTables.MeleeWeaponsMatrices[weaponType];
                for (int subtype = 0; subtype < lootTable.Length; subtype++)
                {
                    if (lootTable[subtype].Contains((int)wcid))
                        return true;
                }
            }
            return false;
        }
    }
}
