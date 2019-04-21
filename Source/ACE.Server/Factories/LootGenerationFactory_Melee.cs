using ACE.Database.Models.Shard;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Factories;
using ACE.Server.WorldObjects;

namespace ACE.Server.Factories
{
    public static partial class LootGenerationFactory
    {
        private static WorldObject CreateWeapon(int tier, bool isMagical)
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
            int materialType = GetMaterialType(2, tier);
            int workmanship = GetWorkmanship(tier);
            int wieldDiff = GetWield(tier, 3);
            WieldRequirement wieldRequirments = WieldRequirement.RawSkill;

            int eleType = ThreadSafeRandom.Next(0, 4);
            int weaponType = ThreadSafeRandom.Next(0, 5);
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
                            damage = GetMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Axe);
                            damageVariance = GetVariance(wieldSkillType, LootWeaponType.Axe);
                            break;
                        case 3:
                        case 4:
                        case 5:
                            weaponDefense = GetMaxDamageMod(tier, 20);
                            weaponOffense = GetMaxDamageMod(tier, 20);

                            damage = GetMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Dagger);

                            if (heavyWeaponsType == 3)
                                damageVariance = GetVariance(wieldSkillType, LootWeaponType.Dagger);
                            if (heavyWeaponsType == 4 || heavyWeaponsType == 5)
                            {
                                damage = GetMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.DaggerMulti);
                                damageVariance = GetVariance(wieldSkillType, LootWeaponType.DaggerMulti);
                            }
                            break;
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                            weaponDefense = GetMaxDamageMod(tier, 22);
                            weaponOffense = GetMaxDamageMod(tier, 18);
                            damage = GetMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Mace);
                            damageVariance = GetVariance(wieldSkillType, LootWeaponType.Mace);
                            break;
                        case 10:
                        case 11:
                        case 12:
                            weaponDefense = GetMaxDamageMod(tier, 15);
                            weaponOffense = GetMaxDamageMod(tier, 25);
                            damage = GetMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Spear);
                            damageVariance = GetVariance(wieldSkillType, LootWeaponType.Spear);
                            break;
                        case 13:
                        case 14:
                            weaponDefense = GetMaxDamageMod(tier, 25);
                            weaponOffense = GetMaxDamageMod(tier, 15);
                            damage = GetMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Staff);
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

                            damage = GetMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Sword);
                            damageVariance = GetVariance(wieldSkillType, LootWeaponType.Sword);

                            if (heavyWeaponsType == 20)
                            {
                                damage = GetMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.SwordMulti);
                                damageVariance = GetVariance(wieldSkillType, LootWeaponType.SwordMulti);
                            }
                            break;
                        case 21:
                        default:
                            weaponDefense = GetMaxDamageMod(tier, 20);
                            weaponOffense = GetMaxDamageMod(tier, 20);
                            damage = GetMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.UA);
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
                            damage = GetMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Axe);
                            damageVariance = GetVariance(wieldSkillType, LootWeaponType.Axe);
                            break;
                        case 4:
                        case 5:
                            weaponDefense = GetMaxDamageMod(tier, 20);
                            weaponOffense = GetMaxDamageMod(tier, 20);

                            if (lightWeaponsType == 4)
                            {
                                damage = GetMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Dagger);
                                damageVariance = GetVariance(wieldSkillType, LootWeaponType.Dagger);
                            }

                            if (lightWeaponsType == 5)
                            {
                                damage = GetMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.DaggerMulti);
                                damageVariance = GetVariance(wieldSkillType, LootWeaponType.DaggerMulti);
                            }
                            break;
                        case 6:
                        case 7:
                        case 8:
                            weaponDefense = GetMaxDamageMod(tier, 22);
                            weaponOffense = GetMaxDamageMod(tier, 18);
                            damage = GetMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Mace);
                            damageVariance = GetVariance(wieldSkillType, LootWeaponType.Mace);
                            break;
                        case 9:
                        case 10:
                            weaponDefense = GetMaxDamageMod(tier, 15);
                            weaponOffense = GetMaxDamageMod(tier, 25);
                            damage = GetMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Spear);
                            damageVariance = GetVariance(wieldSkillType, LootWeaponType.Spear);
                            break;
                        case 11:
                            weaponDefense = GetMaxDamageMod(tier, 25);
                            weaponOffense = GetMaxDamageMod(tier, 15);
                            damage = GetMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Staff);
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

                            damage = GetMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Sword);
                            damageVariance = GetVariance(wieldSkillType, LootWeaponType.Sword);

                            if (lightWeaponsType == 14)
                            {
                                damage = GetMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.SwordMulti);
                                damageVariance = GetVariance(wieldSkillType, LootWeaponType.SwordMulti);
                            }
                            break;
                        case 18:
                        default:
                            weaponDefense = GetMaxDamageMod(tier, 20);
                            weaponOffense = GetMaxDamageMod(tier, 20);
                            damage = GetMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.UA);
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
                            damage = GetMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Axe);
                            damageVariance = GetVariance(wieldSkillType, LootWeaponType.Axe);
                            break;
                        case 3:
                        case 4:
                        case 5:
                            weaponDefense = GetMaxDamageMod(tier, 20);
                            weaponOffense = GetMaxDamageMod(tier, 20);
                            damage = GetMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Dagger);
                            damageVariance = GetVariance(wieldSkillType, LootWeaponType.Dagger);

                            if (finesseWeaponsType == 3 || finesseWeaponsType == 4)
                            {
                                damageVariance = GetVariance(wieldSkillType, LootWeaponType.DaggerMulti);
                                damage = GetMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.DaggerMulti);
                            }
                            break;
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                            weaponDefense = GetMaxDamageMod(tier, 22);
                            weaponOffense = GetMaxDamageMod(tier, 18);
                            damage = GetMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Mace);
                            damageVariance = GetVariance(wieldSkillType, LootWeaponType.Mace);

                            if (finesseWeaponsType == 9)
                                damageVariance = GetVariance(wieldSkillType, LootWeaponType.Jitte);
                            break;
                        case 11:
                        case 12:
                            weaponDefense = GetMaxDamageMod(tier, 15);
                            weaponOffense = GetMaxDamageMod(tier, 25);
                            damage = GetMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Spear);
                            damageVariance = GetVariance(wieldSkillType, LootWeaponType.Spear);
                            break;
                        case 13:
                        case 14:
                            weaponDefense = GetMaxDamageMod(tier, 25);
                            weaponOffense = GetMaxDamageMod(tier, 15);
                            damage = GetMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Staff);
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
                            damage = GetMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Sword);
                            damageVariance = GetVariance(wieldSkillType, LootWeaponType.Sword);

                            if (finesseWeaponsType == 15)
                            {
                                damage = GetMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.SwordMulti);
                                damageVariance = GetVariance(wieldSkillType, LootWeaponType.SwordMulti);
                            }
                            break;
                        case 21:
                        default:
                            weaponDefense = GetMaxDamageMod(tier, 20);
                            weaponOffense = GetMaxDamageMod(tier, 20);
                            damage = GetMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.UA);
                            damageVariance = GetVariance(wieldSkillType, LootWeaponType.UA);
                            break;
                    }
                    break;
                case 3:
                    // Two handed
                    wieldSkillType = Skill.TwoHandedCombat;
                    int twoHandedWeaponsType = ThreadSafeRandom.Next(0, 11);
                    weaponWeenie = LootTables.TwoHandedWeaponsMatrix[twoHandedWeaponsType][eleType];

                    damage = GetMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Cleaving);
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
                            damage = GetMaxDamage(wieldSkillType, wieldDiff, LootWeaponType.Spears);
                            break;
                    }
                    break;
                case 4:
                    return CreateMissileWeapon(tier, isMagical);
                default:
                    return CreateCaster(tier, isMagical);
            }

            WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)weaponWeenie);

            if (wo == null)
                return null;

            wo.SetProperty(PropertyInt.AppraisalLongDescDecoration, longDescDecoration);
            wo.SetProperty(PropertyString.LongDesc, wo.GetProperty(PropertyString.Name));

            wo.SetProperty(PropertyInt.GemCount, gemCount);
            wo.SetProperty(PropertyInt.GemType, gemType);
            wo.SetProperty(PropertyInt.MaterialType, GetMaterialType(2, tier));
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
            {
                wo.SetProperty(PropertyInt.UiEffects, (int)UiEffects.Magical);

                int numSpells = GetNumSpells(tier);

                int lowSpellTier = GetLowSpellTier(tier);
                int highSpellTier = GetHighSpellTier(tier);
                int minorCantrips = GetNumMinorCantrips(tier);
                int majorCantrips = GetNumMajorCantrips(tier);
                int epicCantrips = GetNumEpicCantrips(tier);
                int legendaryCantrips = GetNumLegendaryCantrips(tier);
                int numCantrips = minorCantrips + majorCantrips + epicCantrips + legendaryCantrips;

                int spellCraft = GetSpellcraft(numSpells, tier);
                int itemDifficulty = GetDifficulty(tier, spellCraft);
                int maxMana = GetMaxMana(numSpells, tier);

                wo.SetProperty(PropertyInt.ItemSpellcraft, spellCraft);

                wo.SetProperty(PropertyInt.ItemDifficulty, itemDifficulty);
                wo.SetProperty(PropertyInt.ItemMaxMana, maxMana);
                wo.SetProperty(PropertyInt.ItemCurMana, maxMana);
                wo.SetProperty(PropertyFloat.ManaRate, GetManaRate());

                int[][] spells = LootTables.MeleeSpells;
                int[][] cantrips = LootTables.MeleeCantrips;
                int[] shuffledValues = new int[spells.Length];
                for (int i = 0; i < spells.Length; i++)
                {
                    shuffledValues[i] = i;
                }
                Shuffle(shuffledValues);

                if (numSpells - numCantrips > 0)
                {
                    for (int a = 0; a < numSpells - numCantrips; a++)
                    {
                        int col = ThreadSafeRandom.Next(lowSpellTier - 1, highSpellTier - 1);
                        int spellID = spells[shuffledValues[a]][col];
                        wo.Biota.GetOrAddKnownSpell(spellID, wo.BiotaDatabaseLock, wo.BiotaPropertySpells, out _);
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
                        wo.Biota.GetOrAddKnownSpell(spellID, wo.BiotaDatabaseLock, wo.BiotaPropertySpells, out _);
                    }
                    //major cantrips
                    for (int a = 0; a < majorCantrips; a++)
                    {
                        int spellID = cantrips[shuffledValues[shuffledPlace]][1];
                        shuffledPlace++;
                        wo.Biota.GetOrAddKnownSpell(spellID, wo.BiotaDatabaseLock, wo.BiotaPropertySpells, out _);
                    }
                    // epic cantrips
                    for (int a = 0; a < epicCantrips; a++)
                    {
                        int spellID = cantrips[shuffledValues[shuffledPlace]][2];
                        shuffledPlace++;
                        wo.Biota.GetOrAddKnownSpell(spellID, wo.BiotaDatabaseLock, wo.BiotaPropertySpells, out _);
                    }
                    //legendary cantrips
                    for (int a = 0; a < legendaryCantrips; a++)
                    {
                        int spellID = cantrips[shuffledValues[shuffledPlace]][3];
                        shuffledPlace++;
                        wo.Biota.GetOrAddKnownSpell(spellID, wo.BiotaDatabaseLock, wo.BiotaPropertySpells, out _);
                    }
                }
            }
            else
            {
                wo.RemoveProperty(PropertyInt.ItemManaCost);
                wo.RemoveProperty(PropertyInt.ItemMaxMana);
                wo.RemoveProperty(PropertyInt.ItemCurMana);
                wo.RemoveProperty(PropertyInt.ItemSpellcraft);
                wo.RemoveProperty(PropertyInt.ItemDifficulty);
            }
            wo.SetProperty(PropertyInt.Value, GetValue(tier, workmanship, LootTables.materialModifier[(int)wo.GetProperty(PropertyInt.GemType)], LootTables.materialModifier[(int)wo.GetProperty(PropertyInt.MaterialType)]));

            return wo;
        }
    }
}
