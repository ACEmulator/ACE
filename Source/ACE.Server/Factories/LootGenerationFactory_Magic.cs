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
        private static WorldObject CreateSummoningEssence(int tier)
        {
            uint id = 0;

            if (tier < 1) tier = 1;
            if (tier > 8) tier = 8;

            int summoningEssenceIndex = ThreadSafeRandom.Next(0, LootTables.SummoningEssencesMatrix.Length - 1);

            id = (uint)LootTables.SummoningEssencesMatrix[summoningEssenceIndex][tier - 1];

            if (id == 0)
                return null;

            if (!(WorldObjectFactory.CreateNewWorldObject(id) is PetDevice petDevice))
                return null;

            var ratingChance = 0.5f;

            // add rng ratings to pet device
            // linear or biased?
            if (ratingChance > ThreadSafeRandom.Next(0.0f, 1.0f))
                petDevice.GearDamage = ThreadSafeRandom.Next(1, 20);
            if (ratingChance > ThreadSafeRandom.Next(0.0f, 1.0f))
                petDevice.GearDamageResist = ThreadSafeRandom.Next(1, 20);
            if (ratingChance > ThreadSafeRandom.Next(0.0f, 1.0f))
                petDevice.GearCritDamage = ThreadSafeRandom.Next(1, 20);
            if (ratingChance > ThreadSafeRandom.Next(0.0f, 1.0f))
                petDevice.GearCritDamageResist = ThreadSafeRandom.Next(1, 20);
            if (ratingChance > ThreadSafeRandom.Next(0.0f, 1.0f))
                petDevice.GearCrit = ThreadSafeRandom.Next(1, 20);
            if (ratingChance > ThreadSafeRandom.Next(0.0f, 1.0f))
                petDevice.GearCritResist = ThreadSafeRandom.Next(1, 20);

            var workmanship = GetWorkmanship(tier);
            petDevice.SetProperty(PropertyInt.ItemWorkmanship, workmanship);

            return petDevice;
        }

        private static WorldObject CreateRandomScroll(int tier)
        {
            WorldObject wo;

            if (tier > 7)
            {
                int id = CreateLevel8SpellComp();
                wo = WorldObjectFactory.CreateNewWorldObject((uint)id);
                return wo;
            }

            if (tier < 1) tier = 1;

            int scrollLootMatrixIndex = tier - 1;
            int minSpellLevel = LootTables.ScrollLootMatrix[scrollLootMatrixIndex][0];
            int maxSpellLevel = LootTables.ScrollLootMatrix[scrollLootMatrixIndex][1];

            int scrollLootIndex = ThreadSafeRandom.Next(minSpellLevel, maxSpellLevel);
            uint spellID = 0;

            while (spellID == 0)
                spellID = (uint)LootTables.ScrollSpells[ThreadSafeRandom.Next(0, LootTables.ScrollSpells.Length - 1)][scrollLootIndex];

            var weenie = DatabaseManager.World.GetScrollWeenie(spellID);
            if (weenie == null)
            {
                log.DebugFormat("CreateRandomScroll for tier {0} and spellID of {1} returned null from the database.", tier, spellID);
                return null;
            }

            wo = WorldObjectFactory.CreateNewWorldObject(weenie.ClassId);
            return wo;
        }

        private static int CreateLevel8SpellComp()
        {
            int upperLimit = LootTables.Level8SpellComps.Length - 1;
            int chance = ThreadSafeRandom.Next(0, upperLimit);

            return LootTables.Level8SpellComps[chance];
        }

        private static WorldObject CreateCaster(int tier, bool isMagical)
        {
            int casterWeenie = 0; //done
            double elementalDamageMod = 0;
            int wield = 0; //done
            Skill wieldSkillType = Skill.None;
            int chance = 0;
            int subType = 0;

            switch (tier)
            {
                case 1:
                case 2:
                    wield = 0;
                    break;
                case 3:
                    chance = ThreadSafeRandom.Next(0, 100);
                    if (chance < 80)
                        wield = 0;
                    else
                        wield = 290;
                    break;
                case 4:
                    chance = ThreadSafeRandom.Next(0, 100);
                    if (chance < 60)
                        wield = 0;
                    else if (chance < 95)
                        wield = 290;
                    else
                        wield = 310;
                    break;
                case 5:
                    chance = ThreadSafeRandom.Next(0, 100);
                    if (chance < 50)
                        wield = 0;
                    else if (chance < 70)
                        wield = 290;
                    else if (chance < 90)
                        wield = 310;
                    else
                        wield = 330;
                    break;
                case 6:
                    chance = ThreadSafeRandom.Next(0, 100);
                    if (chance < 40)
                        wield = 0;
                    else if (chance < 60)
                        wield = 290;
                    else if (chance < 80)
                        wield = 310;
                    else if (chance < 90)
                        wield = 330;
                    else
                        wield = 355;
                    break;
                case 7:
                    chance = ThreadSafeRandom.Next(0, 100);
                    if (chance < 30)
                        wield = 0;
                    else if (chance < 50)
                        wield = 290;
                    else if (chance < 60)
                        wield = 310;
                    else if (chance < 70)
                        wield = 330;
                    else if (chance < 90)
                        wield = 355;
                    else
                        wield = 375;
                    break;
                default:
                    chance = ThreadSafeRandom.Next(0, 100);
                    if (chance < 25)
                        wield = 0;
                    else if (chance < 50)
                        wield = 290;
                    else if (chance < 60)
                        wield = 310;
                    else if (chance < 70)
                        wield = 330;
                    else if (chance < 80)
                        wield = 355;
                    else if (chance < 90)
                        wield = 375;
                    else
                        wield = 385;
                    break;
            }

            ////Getting the caster Weenie needed.
            if (wield == 0)
            {
                // Determine plain caster type: 0 - Orb, 1 - Sceptre, 2 - Staff, 3 - Wand
                subType = ThreadSafeRandom.Next(0, 3);
                casterWeenie = LootTables.CasterWeaponsMatrix[wield][subType];
            }
            else
            {
                // Determine the Elemental Damage Mod amount
                elementalDamageMod = GetMaxDamageMod(tier, 18);

                // Determine caster type: 1 - Sceptre, 2 - Baton, 3 - Staff
                int casterType = ThreadSafeRandom.Next(1, 3);

                // Determine element type: 0 - Slashing, 1 - Piercing, 2 - Blunt, 3 - Frost, 4 - Fire, 5 - Acid, 6 - Electric, 7 - Nether
                int element = ThreadSafeRandom.Next(0, 7);
                casterWeenie = LootTables.CasterWeaponsMatrix[casterType][element];

                if (element == 7)
                {
                    wieldSkillType = Skill.VoidMagic;
                }
                else
                {
                    // Determine skill of wield requirement
                    chance = ThreadSafeRandom.Next(0, 3);
                    switch (chance)
                    {
                        case 0:
                            wieldSkillType = Skill.WarMagic;
                            break;
                        case 2:
                            wieldSkillType = Skill.CreatureEnchantment;
                            break;
                        case 3:
                            wieldSkillType = Skill.ItemEnchantment;
                            break;
                        default:
                            wieldSkillType = Skill.LifeMagic;
                            break;
                    }
                }
            }

            WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)casterWeenie);

            if (wo == null)
                return null;

            int workmanship = GetWorkmanship(tier);
            wo.SetProperty(PropertyInt.ItemWorkmanship, workmanship);
            wo.SetProperty(PropertyInt.MaterialType, GetMaterialType(3, tier));
            wo.SetProperty(PropertyInt.GemCount, ThreadSafeRandom.Next(1, 5));
            wo.SetProperty(PropertyInt.GemType, ThreadSafeRandom.Next(10, 50));
            wo.SetProperty(PropertyString.LongDesc, wo.GetProperty(PropertyString.Name));
            wo.SetProperty(PropertyInt.Value, GetValue(tier, workmanship, LootTables.materialModifier[(int)wo.GetProperty(PropertyInt.GemType)], LootTables.materialModifier[(int)wo.GetProperty(PropertyInt.MaterialType)]));

            if (ThreadSafeRandom.Next(0, 100) > 95)
            {
                double missileDMod = GetMissileDMod(tier);
                if (missileDMod > 0.0f)
                    wo.SetProperty(PropertyFloat.WeaponMissileDefense, missileDMod);
            }
            else
            {
                double meleeDMod = GetMeleeDMod(tier);
                if (meleeDMod > 0.0f)
                    wo.SetProperty(PropertyFloat.WeaponDefense, meleeDMod);
            }

            double manaConMod = GetManaCMod(tier);
            if (manaConMod > 0.0f)
                wo.SetProperty(PropertyFloat.ManaConversionMod, manaConMod);

            if (elementalDamageMod > 1.0f)
                wo.SetProperty(PropertyFloat.ElementalDamageMod, elementalDamageMod);

            if (wield > 0)
            {
                wo.SetProperty(PropertyInt.WieldRequirements, (int)WieldRequirement.RawSkill);
                wo.SetProperty(PropertyInt.WieldDifficulty, wield);
                wo.SetProperty(PropertyInt.WieldSkillType, (int)wieldSkillType);
            }

            wo.RemoveProperty(PropertyInt.ItemSkillLevelLimit);

            if (isMagical)
            {
                wo.SetProperty(PropertyInt.UiEffects, (int)UiEffects.Magical);

                int lowSpellTier = GetLowSpellTier(tier);
                int highSpellTier = GetHighSpellTier(tier);
                int numSpells = GetNumSpells(tier);
                int spellcraft = GetSpellcraft(numSpells, tier);
                int itemMaxMana = GetMaxMana(numSpells, tier);

                wo.SetProperty(PropertyInt.ItemDifficulty, GetDifficulty(tier, spellcraft));
                wo.SetProperty(PropertyFloat.ManaRate, GetManaRate());

                wo.SetProperty(PropertyInt.ItemMaxMana, itemMaxMana);
                wo.SetProperty(PropertyInt.ItemCurMana, itemMaxMana);
                wo.SetProperty(PropertyInt.AppraisalLongDescDecoration, 7);
                wo.SetProperty(PropertyInt.ItemSpellcraft, spellcraft);

                int minorCantrips = GetNumMinorCantrips(tier);
                int majorCantrips = GetNumMajorCantrips(tier);
                int epicCantrips = GetNumEpicCantrips(tier);
                int legendaryCantrips = GetNumLegendaryCantrips(tier);
                int numCantrips = minorCantrips + majorCantrips + epicCantrips + legendaryCantrips;

                int[][] spells = LootTables.WandSpells;
                int[][] cantrips = LootTables.WandCantrips;
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

            return wo;
        }
    }
}
