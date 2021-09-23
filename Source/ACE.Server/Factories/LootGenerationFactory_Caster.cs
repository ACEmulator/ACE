using ACE.Common;
using ACE.Database.Models.World;
using ACE.Entity.Enum;
using ACE.Server.Entity.Mutations;
using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;
using ACE.Server.Factories.Tables;
using ACE.Server.WorldObjects;

namespace ACE.Server.Factories
{
    public static partial class LootGenerationFactory
    {
        /// <summary>
        /// Creates Caster (Wand, Staff, Orb)
        /// </summary>
        public static WorldObject CreateCaster(TreasureDeath profile, bool isMagical, int wield = -1, bool forceWar = false, bool mutate = true)
        {
            // Refactored 11/20/19  - HarliQ
            int casterWeenie = 0;
            int subType = 0;
            int element = 0;

            if (wield == -1)
                wield = RollWieldDifficulty(profile.Tier, TreasureWeaponType.Caster);

            // Getting the caster Weenie needed.
            if (wield == 0)
            {
                // Determine plain caster type: 0 - Orb, 1 - Sceptre, 2 - Staff, 3 - Wand
                subType = ThreadSafeRandom.Next(0, 3);
                casterWeenie = LootTables.CasterWeaponsMatrix[wield][subType];
            }
            else
            {
                // Determine caster type: 1 - Sceptre, 2 - Baton, 3 - Staff
                int casterType = ThreadSafeRandom.Next(1, 3);

                // Determine element type: 0 - Slashing, 1 - Piercing, 2 - Blunt, 3 - Frost, 4 - Fire, 5 - Acid, 6 - Electric, 7 - Nether
                element = forceWar ? ThreadSafeRandom.Next(0, 6) : ThreadSafeRandom.Next(0, 7);
                casterWeenie = LootTables.CasterWeaponsMatrix[casterType][element];
            }

            WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)casterWeenie);

            if (wo != null && mutate)
                MutateCaster(wo, profile, isMagical, wield);

            return wo;
        }

        private static void MutateCaster(WorldObject wo, TreasureDeath profile, bool isMagical, int? wieldDifficulty = null, TreasureRoll roll = null)
        {
            if (wieldDifficulty != null)
            {
                // previous method

                var wieldRequirement = WieldRequirement.RawSkill;
                var wieldSkillType = Skill.None;

                double elementalDamageMod = 0;

                if (wieldDifficulty == 0)
                {
                    if (profile.Tier > 6)
                    {
                        wieldRequirement = WieldRequirement.Level;
                        wieldSkillType = Skill.Axe;  // Set by examples from PCAP data

                        wieldDifficulty = profile.Tier switch
                        {
                            7 => 150, // In this instance, used for indicating player level, rather than skill level
                            _ => 180, // In this instance, used for indicating player level, rather than skill level
                        };
                    }
                }
                else
                {
                    elementalDamageMod = RollElementalDamageMod(wieldDifficulty.Value);

                    if (wo.W_DamageType == DamageType.Nether)
                        wieldSkillType = Skill.VoidMagic;
                    else
                        wieldSkillType = Skill.WarMagic;
                }

                // ManaConversionMod
                var manaConversionMod = RollManaConversionMod(profile.Tier);
                if (manaConversionMod > 0.0f)
                    wo.ManaConversionMod = manaConversionMod;

                // ElementalDamageMod
                if (elementalDamageMod > 1.0f)
                    wo.ElementalDamageMod = elementalDamageMod;

                // WieldRequirements
                if (wieldDifficulty > 0 || wieldRequirement == WieldRequirement.Level)
                {
                    wo.WieldRequirements = wieldRequirement;
                    wo.WieldSkillType = (int)wieldSkillType;
                    wo.WieldDifficulty = wieldDifficulty;
                }
                else
                {
                    wo.WieldRequirements = WieldRequirement.Invalid;
                    wo.WieldSkillType = null;
                    wo.WieldDifficulty = null;
                }

                // WeaponDefense
                wo.WeaponDefense = RollWeaponDefense(wieldDifficulty.Value, profile);
            }
            else
            {
                // new method - mutation scripts

                // mutate ManaConversionMod
                var mutationFilter = MutationCache.GetMutation("Casters.caster.txt");
                mutationFilter.TryMutate(wo, profile.Tier);

                // mutate ElementalDamageMod / WieldRequirements
                var isElemental = wo.W_DamageType != DamageType.Undef;
                var scriptName = GetCasterScript(isElemental);

                mutationFilter = MutationCache.GetMutation(scriptName);
                mutationFilter.TryMutate(wo, profile.Tier);

                // this part was not handled by mutation filter
                if (wo.WieldRequirements == WieldRequirement.RawSkill)
                {
                    if (wo.W_DamageType == DamageType.Nether)
                        wo.WieldSkillType = (int)Skill.VoidMagic;
                    else
                        wo.WieldSkillType = (int)Skill.WarMagic;
                }

                // mutate WeaponDefense
                mutationFilter = MutationCache.GetMutation("Casters.weapon_defense.txt");
                mutationFilter.TryMutate(wo, profile.Tier);
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

            // burden?

            // missile defense / magic defense
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
            }
            else
            {
                // if a caster was from a MagicItem profile, it always had a SpellDID
                MutateCaster_SpellDID(wo, profile);

                AssignMagic(wo, profile, roll);
            }

            // item value
            //if (wo.HasMutateFilter(MutateFilter.Value))   // fixme: data
                MutateValue(wo, profile.Tier, roll);

            // long description
            wo.LongDesc = GetLongDesc(wo);
        }

        private static void MutateCaster_SpellDID(WorldObject wo, TreasureDeath profile)
        {
            var firstSpell = CasterSlotSpells.Roll(wo);

            var spellLevels = SpellLevelProgression.GetSpellLevels(firstSpell);

            if (spellLevels == null)
            {
                log.Error($"MutateCaster_SpellDID: couldn't find {firstSpell}");
                return;
            }

            if (spellLevels.Count != 8)
            {
                log.Error($"MutateCaster_SpellDID: found {spellLevels.Count} spell levels for {firstSpell}, expected 8");
                return;
            }

            var spellLevel = SpellLevelChance.Roll(profile.Tier);

            wo.SpellDID = (uint)spellLevels[spellLevel - 1];

            var spell = new Server.Entity.Spell(wo.SpellDID.Value);

            var castableMod = CasterSlotSpells.IsOrb(wo) ? 5.0f : 2.5f;

            wo.ItemManaCost = (int)(spell.BaseMana * castableMod);

            wo.ItemUseable = Usable.SourceWieldedTargetRemoteNeverWalk;
        }

        private static string GetCasterScript(bool isElemental = false)
        {
            var elementalStr = isElemental ? "elemental" : "non_elemental";

            return $"Casters.caster_{elementalStr}.txt";
        }

        private static bool GetMutateCasterData(uint wcid)
        {
            for (var i = 0; i < LootTables.CasterWeaponsMatrix.Length; i++)
            {
                var table = LootTables.CasterWeaponsMatrix[i];

                for (var element = 0; element < table.Length; element++)
                {
                    if (wcid == table[element])
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Rolls for the ManaConversionMod for casters
        /// </summary>
        private static double RollManaConversionMod(int tier)
        {
            int magicMod = 0;

            int chance = 0;
            switch (tier)
            {
                case 1:
                case 2:
                    magicMod = 0;
                    break;
                case 3:
                    chance = ThreadSafeRandom.Next(1, 1000);
                    if (chance > 900)
                        magicMod = 5;
                    else if (chance > 800)
                        magicMod = 4;
                    else if (chance > 700)
                        magicMod = 3;
                    else if (chance > 600)
                        magicMod = 2;
                    else if (chance > 500)
                        magicMod = 1;
                    break;
                case 4:
                    chance = ThreadSafeRandom.Next(1, 1000);
                    if (chance > 900)
                        magicMod = 10;
                    else if (chance > 800)
                        magicMod = 9;
                    else if (chance > 700)
                        magicMod = 8;
                    else if (chance > 600)
                        magicMod = 7;
                    else if (chance > 500)
                        magicMod = 6;
                    else
                        magicMod = 5;
                    break;
                case 5:
                    chance = ThreadSafeRandom.Next(1, 1000);
                    if (chance > 900)
                        magicMod = 10;
                    else if (chance > 800)
                        magicMod = 9;
                    else if (chance > 700)
                        magicMod = 8;
                    else if (chance > 600)
                        magicMod = 7;
                    else if (chance > 500)
                        magicMod = 6;
                    else
                        magicMod = 5;
                    break;
                case 6:
                    chance = ThreadSafeRandom.Next(1, 1000);
                    if (chance > 900)
                        magicMod = 10;
                    else if (chance > 800)
                        magicMod = 9;
                    else if (chance > 700)
                        magicMod = 8;
                    else if (chance > 600)
                        magicMod = 7;
                    else if (chance > 500)
                        magicMod = 6;
                    else
                        magicMod = 5;
                    break;
                case 7:
                    chance = ThreadSafeRandom.Next(1, 1000);
                    if (chance > 900)
                        magicMod = 10;
                    else if (chance > 800)
                        magicMod = 9;
                    else if (chance > 700)
                        magicMod = 8;
                    else if (chance > 600)
                        magicMod = 7;
                    else if (chance > 500)
                        magicMod = 6;
                    else
                        magicMod = 5;
                    break;
                default:
                    chance = ThreadSafeRandom.Next(1, 1000);
                    if (chance > 900)
                        magicMod = 10;
                    else if (chance > 800)
                        magicMod = 9;
                    else if (chance > 700)
                        magicMod = 8;
                    else if (chance > 600)
                        magicMod = 7;
                    else if (chance > 500)
                        magicMod = 6;
                    else
                        magicMod = 5;
                    break;
            }

            double manaDMod = magicMod / 100.0;

            return manaDMod;
        }

        /// <summary>
        /// Rolls for ElementalDamageMod for caster weapons
        /// </summary>
        private static double RollElementalDamageMod(int wield)
        {
            double elementBonus = 0;

            int chance = ThreadSafeRandom.Next(1, 100);
            switch (wield)
            {
                case 290:
                    if (chance > 95)
                        elementBonus = 0.03;
                    else if (chance > 65)
                        elementBonus = 0.02;
                    else
                        elementBonus = 0.01;
                    break;
                case 310:
                    if (chance > 95)
                        elementBonus = 0.06;
                    else if (chance > 65)
                        elementBonus = 0.05;
                    else
                        elementBonus = 0.04;
                    break;

                case 330:
                    if (chance > 95)
                        elementBonus = 0.09;
                    else if (chance > 65)
                        elementBonus = 0.08;
                    else
                        elementBonus = 0.07;
                    break;

                case 355:
                    if (chance > 95)
                        elementBonus = 0.13;
                    else if (chance > 80)
                        elementBonus = 0.12;
                    else if (chance > 55)
                        elementBonus = 0.11;
                    else if (chance > 20)
                        elementBonus = 0.10;
                    else
                        elementBonus = 0.09;
                    break;

                case 375:
                    if (chance > 95)
                        elementBonus = 0.16;
                    else if (chance > 85)
                        elementBonus = 0.15;
                    else if (chance > 60)
                        elementBonus = 0.14;
                    else if (chance > 30)
                        elementBonus = 0.13;
                    else if (chance > 10)
                        elementBonus = 0.12;
                    else
                        elementBonus = 0.11;
                    break;

                default:
                    // 385
                    if (chance > 95)
                        elementBonus = 0.18;
                    else if (chance > 65)
                        elementBonus = 0.17;
                    else
                        elementBonus = 0.16;
                    break;
            }

            elementBonus += 1;

            return elementBonus;
        }
    }
}
