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
            int value = GetValue(tier, workmanship);
            int wieldDiff = GetWield(tier, 3);
            WieldRequirement wieldRequirments = WieldRequirement.RawSkill;

            int eleType = ThreadSafeRandom.Next(0, 4);
            int weaponType = ThreadSafeRandom.Next(0, 5);
            switch (weaponType)
            {
                case 0:
                    // Heavy Weapons
                    int heavyWeaponsType = ThreadSafeRandom.Next(0, 22);
                    weaponWeenie = LootTables.HeavyWeaponsMatrix[heavyWeaponsType][eleType];

                    switch (heavyWeaponsType)
                    {
                        case 0:
                        case 1:
                        case 2:
                            weaponDefense = GetMaxDamageMod(tier, 18);
                            weaponOffense = GetMaxDamageMod(tier, 22);
                            damage = GetMaxDamage(1, tier, wieldDiff, 1);
                            damageVariance = GetVariance(1, 1);
                            break;
                        case 3:
                        case 4:
                        case 5:
                            weaponDefense = GetMaxDamageMod(tier, 20);
                            weaponOffense = GetMaxDamageMod(tier, 20);

                            damage = GetMaxDamage(1, tier, wieldDiff, 2);

                            if (heavyWeaponsType == 3)
                                damageVariance = GetVariance(1, 2);
                            if (heavyWeaponsType == 4 || heavyWeaponsType == 5)
                            {
                                damage = GetMaxDamage(1, tier, wieldDiff, 3);
                                damageVariance = GetVariance(1, 3);
                            }
                            break;
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                            weaponDefense = GetMaxDamageMod(tier, 22);
                            weaponOffense = GetMaxDamageMod(tier, 18);
                            damage = GetMaxDamage(1, tier, wieldDiff, 4);
                            damageVariance = GetVariance(1, 4);
                            break;
                        case 10:
                        case 11:
                        case 12:
                            weaponDefense = GetMaxDamageMod(tier, 15);
                            weaponOffense = GetMaxDamageMod(tier, 25);
                            damage = GetMaxDamage(1, tier, wieldDiff, 5);
                            damageVariance = GetVariance(1, 5);
                            break;
                        case 13:
                        case 14:
                            weaponDefense = GetMaxDamageMod(tier, 25);
                            weaponOffense = GetMaxDamageMod(tier, 15);
                            damage = GetMaxDamage(1, tier, wieldDiff, 8);
                            damageVariance = GetVariance(1, 6);
                            break;
                        case 15:
                        case 16:
                        case 17:
                        case 18:
                        case 19:
                        case 20:
                            weaponDefense = GetMaxDamageMod(tier, 20);
                            weaponOffense = GetMaxDamageMod(tier, 20);

                            damage = GetMaxDamage(1, tier, wieldDiff, 6);
                            damageVariance = GetVariance(1, 7);

                            if (heavyWeaponsType == 20)
                            {
                                damage = GetMaxDamage(1, tier, wieldDiff, 7);
                                damageVariance = GetVariance(1, 8);
                            }
                            break;
                        case 21:
                        default:
                            damage = GetMaxDamage(1, tier, wieldDiff, 9);
                            weaponDefense = GetMaxDamageMod(tier, 20);
                            weaponOffense = GetMaxDamageMod(tier, 20);
                            damageVariance = GetVariance(1, 9);
                            break;
                    }
                    break;
                case 1:
                    // Light Weapons;
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
                            damage = GetMaxDamage(2, tier, wieldDiff, 1);
                            damageVariance = GetVariance(2, 1);
                            break;
                        case 4:
                        case 5:
                            weaponDefense = GetMaxDamageMod(tier, 20);
                            weaponOffense = GetMaxDamageMod(tier, 20);

                            if (lightWeaponsType == 4)
                            {
                                damage = GetMaxDamage(2, tier, wieldDiff, 2);
                                damageVariance = GetVariance(2, 2);
                            }

                            if (lightWeaponsType == 5)
                            {
                                damage = GetMaxDamage(2, tier, wieldDiff, 3);
                                damageVariance = GetVariance(2, 3);
                            }
                            break;
                        case 6:
                        case 7:
                        case 8:
                            weaponDefense = GetMaxDamageMod(tier, 22);
                            weaponOffense = GetMaxDamageMod(tier, 18);
                            damage = GetMaxDamage(2, tier, wieldDiff, 4);
                            damageVariance = GetVariance(2, 4);
                            break;
                        case 9:
                        case 10:
                            weaponDefense = GetMaxDamageMod(tier, 15);
                            weaponOffense = GetMaxDamageMod(tier, 25);
                            damage = GetMaxDamage(2, tier, wieldDiff, 5);
                            damageVariance = GetVariance(2, 6);
                            break;
                        case 11:
                            weaponDefense = GetMaxDamageMod(tier, 25);
                            weaponOffense = GetMaxDamageMod(tier, 15);
                            damage = GetMaxDamage(2, tier, wieldDiff, 8);
                            damageVariance = GetVariance(2, 7);
                            break;
                        case 12:
                        case 13:
                        case 14:
                        case 15:
                        case 16:
                        case 17:
                            weaponDefense = GetMaxDamageMod(tier, 20);
                            weaponOffense = GetMaxDamageMod(tier, 20);

                            damage = GetMaxDamage(2, tier, wieldDiff, 6);
                            damageVariance = GetVariance(2, 8);

                            if (lightWeaponsType == 14)
                            {
                                damage = GetMaxDamage(2, tier, wieldDiff, 7);
                                damageVariance = GetVariance(2, 9);
                            }
                            break;
                        case 18:
                        default:
                            weaponDefense = GetMaxDamageMod(tier, 20);
                            weaponOffense = GetMaxDamageMod(tier, 20);
                            damage = GetMaxDamage(2, tier, wieldDiff, 9);
                            damageVariance = GetVariance(2, 10);
                            break;
                    }
                    break;
                case 2:
                    // Finesse Weapons;
                    int finesseWeaponsType = ThreadSafeRandom.Next(0, 22);
                    weaponWeenie = LootTables.FinesseWeaponsMatrix[finesseWeaponsType][eleType];

                    switch (finesseWeaponsType)
                    {
                        case 0:
                        case 1:
                        case 2:
                            weaponDefense = GetMaxDamageMod(tier, 18);
                            weaponOffense = GetMaxDamageMod(tier, 22);
                            damage = GetMaxDamage(2, tier, wieldDiff, 1);
                            damageVariance = GetVariance(2, 1);
                            break;
                        case 3:
                        case 4:
                        case 5:
                            weaponDefense = GetMaxDamageMod(tier, 20);
                            weaponOffense = GetMaxDamageMod(tier, 20);
                            damage = GetMaxDamage(2, tier, wieldDiff, 2);
                            damageVariance = GetVariance(2, 2);

                            if (finesseWeaponsType == 3 || finesseWeaponsType == 4)
                            {
                                damageVariance = GetVariance(2, 3);
                                damage = GetMaxDamage(2, tier, wieldDiff, 3);
                            }
                            break;
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                            weaponDefense = GetMaxDamageMod(tier, 22);
                            weaponOffense = GetMaxDamageMod(tier, 18);
                            damage = GetMaxDamage(2, tier, wieldDiff, 4);
                            damageVariance = GetVariance(2, 4);

                            if (finesseWeaponsType == 9)
                                damageVariance = GetVariance(2, 5);
                            break;
                        case 11:
                        case 12:
                            weaponDefense = GetMaxDamageMod(tier, 15);
                            weaponOffense = GetMaxDamageMod(tier, 25);
                            damage = GetMaxDamage(2, tier, wieldDiff, 5);
                            damageVariance = GetVariance(2, 6);
                            break;
                        case 13:
                        case 14:
                            weaponDefense = GetMaxDamageMod(tier, 25);
                            weaponOffense = GetMaxDamageMod(tier, 15);
                            damage = GetMaxDamage(2, tier, wieldDiff, 8);
                            damageVariance = GetVariance(2, 7);
                            break;
                        case 15:
                        case 16:
                        case 17:
                        case 18:
                        case 19:
                        case 20:
                            weaponDefense = GetMaxDamageMod(tier, 20);
                            weaponOffense = GetMaxDamageMod(tier, 20);
                            damage = GetMaxDamage(2, tier, wieldDiff, 6);
                            damageVariance = GetVariance(2, 8);

                            if (finesseWeaponsType == 15)
                            {
                                damage = GetMaxDamage(2, tier, wieldDiff, 7);
                                damageVariance = GetVariance(2, 9);
                            }
                            break;
                        case 21:
                        default:
                            weaponDefense = GetMaxDamageMod(tier, 20);
                            weaponOffense = GetMaxDamageMod(tier, 20);
                            damage = GetMaxDamage(2, tier, wieldDiff, 9);
                            damageVariance = GetVariance(2, 10);
                            break;
                    }
                    break;
                case 3:
                    // Two handed
                    int twoHandedWeaponsType = ThreadSafeRandom.Next(0, 11);
                    weaponWeenie = LootTables.TwoHandedWeaponsMatrix[twoHandedWeaponsType][eleType];

                    damage = GetMaxDamage(3, tier, wieldDiff, 1);
                    damageVariance = GetVariance(3, 1);

                    switch (twoHandedWeaponsType)
                    {
                        case 0:
                        case 1:
                        case 2:
                            weaponDefense = GetMaxDamageMod(tier, 20);
                            weaponOffense = GetMaxDamageMod(tier, 20);
                            damageVariance = GetVariance(2, 1);
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
                            damage = GetMaxDamage(3, tier, wieldDiff, 2);
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
            wo.SetProperty(PropertyInt.Value, value);
            wo.SetProperty(PropertyInt.MaterialType, GetMaterialType(2, tier));
            wo.SetProperty(PropertyInt.ItemWorkmanship, workmanship);

            wo.SetProperty(PropertyInt.Damage, damage);
            wo.SetProperty(PropertyFloat.DamageVariance, damageVariance);

            wo.SetProperty(PropertyFloat.WeaponDefense, weaponDefense);
            wo.SetProperty(PropertyFloat.WeaponOffense, weaponOffense);
            wo.SetProperty(PropertyFloat.WeaponMissileDefense, missileD);
            wo.SetProperty(PropertyFloat.WeaponMagicDefense, magicD);

            wo.SetProperty(PropertyInt.WieldDifficulty, wieldDiff);
            wo.SetProperty(PropertyInt.WieldRequirements, (int)wieldRequirments);

            if (wieldDiff == 0)
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
            else
            {
                wo.RemoveProperty(PropertyInt.ItemManaCost);
                wo.RemoveProperty(PropertyInt.ItemMaxMana);
                wo.RemoveProperty(PropertyInt.ItemCurMana);
                wo.RemoveProperty(PropertyInt.ItemSpellcraft);
                wo.RemoveProperty(PropertyInt.ItemDifficulty);
            }

            return wo;
        }
    }
}
