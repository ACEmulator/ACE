using System.Collections.Generic;
using System;

using log4net;

using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Factories;
using ACE.Server.WorldObjects;

namespace ACE.Server.Factories
{
    public static partial class LootGenerationFactory
    {
        private static WorldObject CreateMissileWeapon(int tier, bool isMagical)
        {
            int[][] spells = LootTables.MissileSpells;
            int[][] cantrips = LootTables.MissileCantrips;

            ////Double Values
            double manaRate = -.04166667; ///done

            int weaponWeenie;
            int chance;
            int elemenatalBonus = 0;

            int wieldDifficulty = GetWield(tier, 1);

            if (tier < 4)
                weaponWeenie = GetNonElementalMissileWeapon();
            else
            {
                chance = ThreadSafeRandom.Next(0, 1);
                switch (chance)
                {
                    case 0:
                        weaponWeenie = GetNonElementalMissileWeapon();
                        break;
                    default:
                        elemenatalBonus = GetElementalBonus(wieldDifficulty);
                        weaponWeenie = GetElementalMissileWeapon();
                        break;
                }
            }

            WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)weaponWeenie);

            if (wo == null)
                return null;

            int workmanship = GetWorkmanship(tier);
            wo.SetProperty(PropertyInt.ItemWorkmanship, workmanship);
            wo.SetProperty(PropertyInt.MaterialType, GetMaterialType(2, tier));
            wo.SetProperty(PropertyInt.GemCount, ThreadSafeRandom.Next(1, 5));
            wo.SetProperty(PropertyInt.GemType, ThreadSafeRandom.Next(10, 50));
            wo.SetProperty(PropertyString.LongDesc, wo.GetProperty(PropertyString.Name));

            double meleeDMod = GetMeleeDMod(tier);
            if (meleeDMod > 0.0f)
                wo.SetProperty(PropertyFloat.WeaponDefense, meleeDMod);

            double missileDMod = GetMissileDMod(tier);
            if (missileDMod > 0.0f)
                wo.SetProperty(PropertyFloat.WeaponMissileDefense, missileDMod);

            // wo.SetProperty(PropertyFloat.WeaponMagicDefense, magicDefense);

            if (elemenatalBonus > 0)
                wo.SetProperty(PropertyInt.ElementalDamageBonus, elemenatalBonus);

            if (wieldDifficulty > 0)
            {
                wo.SetProperty(PropertyInt.WieldDifficulty, wieldDifficulty);
                wo.SetProperty(PropertyInt.WieldRequirements, (int)WieldRequirement.RawSkill);
                wo.SetProperty(PropertyInt.WieldSkillType, (int)Skill.MissileWeapons);
            }

            if (isMagical)
            {
                wo.SetProperty(PropertyInt.UiEffects, (int)UiEffects.Magical);

                wo.SetProperty(PropertyFloat.ManaRate, manaRate);

                int numSpells = GetNumSpells(tier);
                int spellCraft = GetSpellcraft(numSpells, tier);
                int lowSpellTier = GetLowSpellTier(tier);
                int highSpellTier = GetHighSpellTier(tier);
                int itemSkillLevelLimit = 0;

                wo.SetProperty(PropertyInt.ItemMaxMana, GetMaxMana(numSpells, tier));
                wo.SetProperty(PropertyInt.ItemCurMana, GetMaxMana(numSpells, tier));
                wo.SetProperty(PropertyInt.ItemSpellcraft, spellCraft);
                wo.SetProperty(PropertyInt.ItemDifficulty, GetDifficulty(tier, spellCraft));
                wo.SetProperty(PropertyInt.ItemSkillLevelLimit, itemSkillLevelLimit);

                int[] shuffledValues = new int[spells.Length];
                for (int i = 0; i < spells.Length; i++)
                {
                    shuffledValues[i] = i;
                }
                Shuffle(shuffledValues);

                int minorCantrips = GetNumMinorCantrips(tier);
                int majorCantrips = GetNumMajorCantrips(tier);
                int epicCantrips = GetNumEpicCantrips(tier);
                int legendaryCantrips = GetNumLegendaryCantrips(tier);
                int numCantrips = minorCantrips + majorCantrips + epicCantrips + legendaryCantrips;

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
                wo.RemoveProperty(PropertyFloat.ManaRate);
            }
            wo.SetProperty(PropertyInt.Value, GetValue(tier, workmanship, LootTables.materialModifier[(int)wo.GetProperty(PropertyInt.GemType)], LootTables.materialModifier[(int)wo.GetProperty(PropertyInt.MaterialType)]));

            return wo;
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
            int subType;

            // Determine missile weapon type: 0 - Bow, 1 - Crossbows, 2 - Atlatl
            int missileType = ThreadSafeRandom.Next(0, 2);
            switch (missileType)
            {
                case 0:
                    subType = ThreadSafeRandom.Next(0, 6);
                    break;
                case 1:
                    subType = ThreadSafeRandom.Next(0, 2);
                    break;
                default:
                    subType = ThreadSafeRandom.Next(0, 1);
                    break;
            }

            return LootTables.NonElementalMissileWeaponsMatrix[missileType][subType];
        }
    }
}
