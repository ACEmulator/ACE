using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.WorldObjects;

namespace ACE.Server.Factories
{
    public static partial class LootGenerationFactory
    {
        private static WorldObject CreateMeleeWeapon(int tier, bool isMagical)
        {
            Skill wieldSkillType = Skill.None;

            int weaponWeenie = 0;
            int damage = 0;
            double damageVariance = 0;
            double weaponDefense = 0;
            double weaponOffense = 0;
            int longDescDecoration = 5;

            ///Properties for weapons
            double magicD = GetMissileDMod(tier);
            double missileD = GetMissileDMod(tier);
            int gemCount = ThreadSafeRandom.Next(1, 5);
            int gemType = ThreadSafeRandom.Next(10, 50);
            int workmanship = GetWorkmanship(tier);
            int wieldDiff = GetWield(tier, 3);
            WieldRequirement wieldRequirments = WieldRequirement.RawSkill;

            int eleType = ThreadSafeRandom.Next(0, 4);
            int weaponType = ThreadSafeRandom.Next(0, 3);
            switch (weaponType)
            {
                case 0:
                    // Heavy Weapons
                    wieldSkillType = Skill.HeavyWeapons;
                    int heavyWeaponsType = ThreadSafeRandom.Next(0, 22);
                    weaponWeenie = LootTables.HeavyWeaponsMatrix[heavyWeaponsType][eleType];

                    switch (heavyWeaponsType)
                    {
                        case 0:
                        case 1:
                        case 2:
                            weaponDefense = GetMaxDamageMod(tier, 18);
                            weaponOffense = GetMaxDamageMod(tier, 22);
                            damage = GetMeleeMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Axe);
                            damageVariance = GetVariance(wieldSkillType, LootWeaponType.Axe);
                            break;
                        case 3:
                        case 4:
                        case 5:
                            weaponDefense = GetMaxDamageMod(tier, 20);
                            weaponOffense = GetMaxDamageMod(tier, 20);

                            damage = GetMeleeMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Dagger);

                            if (heavyWeaponsType == 3)
                                damageVariance = GetVariance(wieldSkillType, LootWeaponType.Dagger);
                            if (heavyWeaponsType == 4 || heavyWeaponsType == 5)
                            {
                                damage = GetMeleeMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.DaggerMulti);
                                damageVariance = GetVariance(wieldSkillType, LootWeaponType.DaggerMulti);
                            }
                            break;
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                            weaponDefense = GetMaxDamageMod(tier, 22);
                            weaponOffense = GetMaxDamageMod(tier, 18);
                            damage = GetMeleeMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Mace);
                            damageVariance = GetVariance(wieldSkillType, LootWeaponType.Mace);
                            break;
                        case 10:
                        case 11:
                        case 12:
                            weaponDefense = GetMaxDamageMod(tier, 15);
                            weaponOffense = GetMaxDamageMod(tier, 25);
                            damage = GetMeleeMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Spear);
                            damageVariance = GetVariance(wieldSkillType, LootWeaponType.Spear);
                            break;
                        case 13:
                        case 14:
                            weaponDefense = GetMaxDamageMod(tier, 25);
                            weaponOffense = GetMaxDamageMod(tier, 15);
                            damage = GetMeleeMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Staff);
                            damageVariance = GetVariance(wieldSkillType, LootWeaponType.Staff);
                            break;
                        case 15:
                        case 16:
                        case 17:
                        case 18:
                        case 19:
                        case 20:
                            weaponDefense = GetMaxDamageMod(tier, 20);
                            weaponOffense = GetMaxDamageMod(tier, 20);

                            damage = GetMeleeMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Sword);
                            damageVariance = GetVariance(wieldSkillType, LootWeaponType.Sword);

                            if (heavyWeaponsType == 20)
                            {
                                damage = GetMeleeMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.SwordMulti);
                                damageVariance = GetVariance(wieldSkillType, LootWeaponType.SwordMulti);
                            }
                            break;
                        case 21:
                        default:
                            weaponDefense = GetMaxDamageMod(tier, 20);
                            weaponOffense = GetMaxDamageMod(tier, 20);
                            damage = GetMeleeMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.UA);
                            damageVariance = GetVariance(wieldSkillType, LootWeaponType.UA);
                            break;
                    }
                    break;
                case 1:
                    // Light Weapons;
                    wieldSkillType = Skill.LightWeapons;
                    int lightWeaponsType = ThreadSafeRandom.Next(0, 19);
                    weaponWeenie = LootTables.LightWeaponsMatrix[lightWeaponsType][eleType];

                    switch (lightWeaponsType)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                            weaponDefense = GetMaxDamageMod(tier, 18);
                            weaponOffense = GetMaxDamageMod(tier, 22);
                            damage = GetMeleeMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Axe);
                            damageVariance = GetVariance(wieldSkillType, LootWeaponType.Axe);
                            break;
                        case 4:
                        case 5:
                            weaponDefense = GetMaxDamageMod(tier, 20);
                            weaponOffense = GetMaxDamageMod(tier, 20);
                            damage = GetMeleeMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.DaggerMulti);
                            damageVariance = GetVariance(wieldSkillType, LootWeaponType.DaggerMulti);
                            break;
                        case 6:
                        case 7:
                        case 8:
                            weaponDefense = GetMaxDamageMod(tier, 22);
                            weaponOffense = GetMaxDamageMod(tier, 18);
                            damage = GetMeleeMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Mace);
                            damageVariance = GetVariance(wieldSkillType, LootWeaponType.Mace);
                            break;
                        case 9:
                        case 10:
                            weaponDefense = GetMaxDamageMod(tier, 15);
                            weaponOffense = GetMaxDamageMod(tier, 25);
                            damage = GetMeleeMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Spear);
                            damageVariance = GetVariance(wieldSkillType, LootWeaponType.Spear);
                            break;
                        case 11:
                            weaponDefense = GetMaxDamageMod(tier, 25);
                            weaponOffense = GetMaxDamageMod(tier, 15);
                            damage = GetMeleeMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Staff);
                            damageVariance = GetVariance(wieldSkillType, LootWeaponType.Staff);
                            break;
                        case 12:
                        case 13:
                        case 14:
                        case 15:
                        case 16:
                        case 17:
                            weaponDefense = GetMaxDamageMod(tier, 20);
                            weaponOffense = GetMaxDamageMod(tier, 20);

                            damage = GetMeleeMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Sword);
                            damageVariance = GetVariance(wieldSkillType, LootWeaponType.Sword);

                            if (lightWeaponsType == 14)
                            {
                                damage = GetMeleeMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.SwordMulti);
                                damageVariance = GetVariance(wieldSkillType, LootWeaponType.SwordMulti);
                            }
                            break;
                        case 18:
                        default:
                            weaponDefense = GetMaxDamageMod(tier, 20);
                            weaponOffense = GetMaxDamageMod(tier, 20);
                            damage = GetMeleeMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.UA);
                            damageVariance = GetVariance(wieldSkillType, LootWeaponType.UA);
                            break;
                    }
                    break;
                case 2:
                    // Finesse Weapons;
                    wieldSkillType = Skill.FinesseWeapons;
                    int finesseWeaponsType = ThreadSafeRandom.Next(0, 22);
                    weaponWeenie = LootTables.FinesseWeaponsMatrix[finesseWeaponsType][eleType];

                    switch (finesseWeaponsType)
                    {
                        case 0:
                        case 1:
                        case 2:
                            weaponDefense = GetMaxDamageMod(tier, 18);
                            weaponOffense = GetMaxDamageMod(tier, 22);
                            damage = GetMeleeMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Axe);
                            damageVariance = GetVariance(wieldSkillType, LootWeaponType.Axe);
                            break;
                        case 3:
                        case 4:
                        case 5:
                            weaponDefense = GetMaxDamageMod(tier, 20);
                            weaponOffense = GetMaxDamageMod(tier, 20);
                            damage = GetMeleeMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Dagger);
                            damageVariance = GetVariance(wieldSkillType, LootWeaponType.Dagger);

                            if (finesseWeaponsType == 3 || finesseWeaponsType == 4)
                            {
                                damageVariance = GetVariance(wieldSkillType, LootWeaponType.DaggerMulti);
                                damage = GetMeleeMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.DaggerMulti);
                            }
                            break;
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                            weaponDefense = GetMaxDamageMod(tier, 22);
                            weaponOffense = GetMaxDamageMod(tier, 18);
                            damage = GetMeleeMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Mace);
                            damageVariance = GetVariance(wieldSkillType, LootWeaponType.Mace);

                            if (finesseWeaponsType == 9)
                                damageVariance = GetVariance(wieldSkillType, LootWeaponType.Jitte);
                            break;
                        case 11:
                        case 12:
                            weaponDefense = GetMaxDamageMod(tier, 15);
                            weaponOffense = GetMaxDamageMod(tier, 25);
                            damage = GetMeleeMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Spear);
                            damageVariance = GetVariance(wieldSkillType, LootWeaponType.Spear);
                            break;
                        case 13:
                        case 14:
                            weaponDefense = GetMaxDamageMod(tier, 25);
                            weaponOffense = GetMaxDamageMod(tier, 15);
                            damage = GetMeleeMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Staff);
                            damageVariance = GetVariance(wieldSkillType, LootWeaponType.Staff);
                            break;
                        case 15:
                        case 16:
                        case 17:
                        case 18:
                        case 19:
                        case 20:
                            weaponDefense = GetMaxDamageMod(tier, 20);
                            weaponOffense = GetMaxDamageMod(tier, 20);
                            damage = GetMeleeMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Sword);
                            damageVariance = GetVariance(wieldSkillType, LootWeaponType.Sword);

                            if (finesseWeaponsType == 15)
                            {
                                damage = GetMeleeMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.SwordMulti);
                                damageVariance = GetVariance(wieldSkillType, LootWeaponType.SwordMulti);
                            }
                            break;
                        case 21:
                        default:
                            weaponDefense = GetMaxDamageMod(tier, 20);
                            weaponOffense = GetMaxDamageMod(tier, 20);
                            damage = GetMeleeMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.UA);
                            damageVariance = GetVariance(wieldSkillType, LootWeaponType.UA);
                            break;
                    }
                    break;
                default:
                    // Two handed
                    wieldSkillType = Skill.TwoHandedCombat;
                    int twoHandedWeaponsType = ThreadSafeRandom.Next(0, 11);
                    weaponWeenie = LootTables.TwoHandedWeaponsMatrix[twoHandedWeaponsType][eleType];

                    damage = GetMeleeMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Cleaving);
                    damageVariance = GetVariance(wieldSkillType, LootWeaponType.TwoHanded);

                    switch (twoHandedWeaponsType)
                    {
                        case 0:
                        case 1:
                        case 2:
                            weaponDefense = GetMaxDamageMod(tier, 20);
                            weaponOffense = GetMaxDamageMod(tier, 20);
                            break;
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                            weaponDefense = GetMaxDamageMod(tier, 22);
                            weaponOffense = GetMaxDamageMod(tier, 18);
                            break;
                        case 7:
                            weaponDefense = GetMaxDamageMod(tier, 18);
                            weaponOffense = GetMaxDamageMod(tier, 22);
                            break;
                        case 8:
                        case 9:
                        case 10:
                        default:
                            weaponDefense = GetMaxDamageMod(tier, 15);
                            weaponOffense = GetMaxDamageMod(tier, 25);
                            damage = GetMeleeMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Spears);
                            break;
                    }
                    break;
            }

            WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)weaponWeenie);

            if (wo == null)
                return null;

            wo.SetProperty(PropertyInt.AppraisalLongDescDecoration, longDescDecoration);
            wo.SetProperty(PropertyString.LongDesc, wo.GetProperty(PropertyString.Name));

            wo.SetProperty(PropertyInt.GemCount, gemCount);
            wo.SetProperty(PropertyInt.GemType, gemType);
            int materialType = GetMaterialType(wo, tier);
            if (materialType > 0)
                wo.MaterialType = (MaterialType)materialType;
            wo.SetProperty(PropertyInt.ItemWorkmanship, workmanship);

            wo.SetProperty(PropertyInt.Damage, damage);
            wo.SetProperty(PropertyFloat.DamageVariance, damageVariance);

            wo.SetProperty(PropertyFloat.WeaponDefense, weaponDefense);
            wo.SetProperty(PropertyFloat.WeaponOffense, weaponOffense);
            wo.SetProperty(PropertyFloat.WeaponMissileDefense, missileD);
            wo.SetProperty(PropertyFloat.WeaponMagicDefense, magicD);

            if (wieldDiff > 0)
            {
                wo.SetProperty(PropertyInt.WieldDifficulty, wieldDiff);
                wo.SetProperty(PropertyInt.WieldRequirements, (int)wieldRequirments);
                wo.SetProperty(PropertyInt.WieldSkillType, (int)wieldSkillType);
            }
            else
            {
                wo.RemoveProperty(PropertyInt.WieldDifficulty);
                wo.RemoveProperty(PropertyInt.WieldRequirements);
                wo.RemoveProperty(PropertyInt.WieldSkillType);
            }

            if (isMagical)
                wo = AssignMagic(wo, tier);
            else
            {
                wo.RemoveProperty(PropertyInt.ItemManaCost);
                wo.RemoveProperty(PropertyInt.ItemMaxMana);
                wo.RemoveProperty(PropertyInt.ItemCurMana);
                wo.RemoveProperty(PropertyInt.ItemSpellcraft);
                wo.RemoveProperty(PropertyInt.ItemDifficulty);
            }

            double materialMod = LootTables.getMaterialValueModifier(wo);
            double gemMaterialMod = LootTables.getGemMaterialValueModifier(wo);
            var value = GetValue(tier, workmanship, gemMaterialMod, materialMod);
            wo.Value = value;

            wo = RandomizeColor(wo);
            return wo;
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

        //The percentages for variances need to be fixed
        private static double GetVariance(Skill category, LootWeaponType type)
        {
            double variance = 0;
            int chance = ThreadSafeRandom.Next(0, 100);

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
                            //Axe
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
                            //Dagger
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
                            //Dagger MultiStrike
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
                            //Mace
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
                            //Jitte
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
                            //Spear
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
                            //Staff
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
                            //Sword
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
                            //Sword Multistrike
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
                            //UA
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
                    /// Two Handed only have one set of variances
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
            int maxDamageVariance = ThreadSafeRandom.Next(-4, 2);

            return damageTable + maxDamageVariance;
        }
    }
}
