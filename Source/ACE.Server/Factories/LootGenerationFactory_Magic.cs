using ACE.Common;
using ACE.Database.Models.World;
using ACE.Database;
using ACE.Entity.Enum;
using ACE.Server.Factories.Entity;
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
                wield = RollWieldDifficulty(profile.Tier, WieldType.Caster);

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

        private static void MutateCaster(WorldObject wo, TreasureDeath profile, bool isMagical, int? wieldDifficulty = null)
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
            int materialType = GetMaterialType(wo, profile.Tier);
            if (materialType > 0)
                wo.MaterialType = (MaterialType)materialType;

            // item color
            MutateColor(wo);

            // gem count / gem material
            if (wo.GemCode != null)
                wo.GemCount = GemCountChance.Roll(wo.GemCode.Value, profile.Tier);
            else
                wo.GemCount = ThreadSafeRandom.Next(1, 5);

            wo.GemType = RollGemType(profile.Tier);

            // workmanship
            wo.ItemWorkmanship = GetWorkmanship(profile.Tier);

            // burden?

            // item value
            wo.Value = GetValue(profile.Tier, wo.ItemWorkmanship.Value, LootTables.getMaterialValueModifier(wo), LootTables.getGemMaterialValueModifier(wo));

            // missile defense / magic defense
            wo.WeaponMissileDefense = RollWeapon_MissileMagicDefense(profile.Tier);
            wo.WeaponMagicDefense = RollWeapon_MissileMagicDefense(profile.Tier);

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

                AssignMagic(wo, profile);
            }

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

            int minSpellLevel = GetLowSpellTier(profile.Tier);
            int maxSpellLevel = GetHighSpellTier(profile.Tier);

            var spellLevel = ThreadSafeRandom.Next(minSpellLevel, maxSpellLevel);

            wo.SpellDID = (uint)spellLevels[spellLevel - 1];

            var spell = new Server.Entity.Spell(wo.SpellDID.Value);

            wo.ItemManaCost = (int)spell.BaseMana * 5;

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

        private static WorldObject CreateSummoningEssence(int tier, bool mutate = true)
        {
            uint id = 0;

            // Adding a spread of Pet Device levels for each tier - Level 200 pets should only be dropping in T8 Loot - HQ 2/29/2020
            // The spread is from Optim's Data
            // T5-T8 20/35/30/15% split 
            // T8- 200,180,150,125
            // T7- 180,150,125,100
            // T6- 150,125,100,80
            // T5- 125,100,80,50
            // T4- 100,80,50
            // T3- 80,50
            // T2- 50
            // T1- 50

            // Tables are already 1-7, so removing them being Tier dependent

            int petLevel = 0;
            int chance = ThreadSafeRandom.Next(1, 100);
            if (chance > 80)
                petLevel = tier - 1;
            else if (chance > 45)
                petLevel = tier - 2;
            else if (chance > 15)
                petLevel = tier - 3;
            else
                petLevel = tier - 4;
            if (petLevel < 2)
                petLevel = 1;

            int summoningEssenceIndex = ThreadSafeRandom.Next(0, LootTables.SummoningEssencesMatrix.Length - 1);

            id = (uint)LootTables.SummoningEssencesMatrix[summoningEssenceIndex][petLevel - 1];

            var petDevice = WorldObjectFactory.CreateNewWorldObject(id) as PetDevice;

            if (petDevice != null && mutate)
                MutatePetDevice(petDevice, tier);

            return petDevice;
        }

        private static void MutatePetDevice(PetDevice petDevice, int tier)
        {
            var ratingChance = 0.5f;

            // add rng ratings to pet device
            // linear or biased?
            if (ratingChance > ThreadSafeRandom.Next(0.0f, 1.0f))
                petDevice.GearDamage = GeneratePetDeviceRating(tier);
            if (ratingChance > ThreadSafeRandom.Next(0.0f, 1.0f))
                petDevice.GearDamageResist = GeneratePetDeviceRating(tier);
            if (ratingChance > ThreadSafeRandom.Next(0.0f, 1.0f))
                petDevice.GearCritDamage = GeneratePetDeviceRating(tier);
            if (ratingChance > ThreadSafeRandom.Next(0.0f, 1.0f))
                petDevice.GearCritDamageResist = GeneratePetDeviceRating(tier);
            if (ratingChance > ThreadSafeRandom.Next(0.0f, 1.0f))
                petDevice.GearCrit = GeneratePetDeviceRating(tier);
            if (ratingChance > ThreadSafeRandom.Next(0.0f, 1.0f))
                petDevice.GearCritResist = GeneratePetDeviceRating(tier);

            petDevice.ItemWorkmanship = GetWorkmanship(tier);
        }

        public static int GeneratePetDeviceRating(int tier)
        {
            // thanks to morosity for this formula!
            var baseRating = ThreadSafeRandom.Next(1, 10);
            var rng = ThreadSafeRandom.Next(0.0f, 1.0f);
            var tierMod = 0.4f + tier * 0.02f;
            if (rng > tierMod)      // TODO: this might be backwards, review
                baseRating += ThreadSafeRandom.Next(1, 10);

            return baseRating;
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

            if (tier == 7)
            {
                // According to wiki, Tier 7 has a chance for level 8 spell components or level 7 spell scrolls
                // No indication of weighting in either direction, so assuming a 50/50 split
                int chance = ThreadSafeRandom.Next(1, 100);
                if (chance > 50)
                {
                    int id = CreateLevel8SpellComp();
                    wo = WorldObjectFactory.CreateNewWorldObject((uint)id);
                    return wo;
                }
            }

            if (tier < 1) tier = 1;

            int scrollLootMatrixIndex = tier - 1;
            int minSpellLevel = LootTables.ScrollLootMatrix[scrollLootMatrixIndex][0];
            int maxSpellLevel = LootTables.ScrollLootMatrix[scrollLootMatrixIndex][1];

            int scrollLootIndex = ThreadSafeRandom.Next(minSpellLevel, maxSpellLevel);
            var spellID = SpellId.Undef;

            while (spellID == SpellId.Undef)
                spellID = ScrollSpells.Table[ThreadSafeRandom.Next(0, ScrollSpells.Table.Length - 1)][scrollLootIndex];

            var weenie = DatabaseManager.World.GetScrollWeenie((uint)spellID);
            if (weenie == null)
            {
                log.DebugFormat("CreateRandomScroll for tier {0} and spellID of {1} returned null from the database.", tier, spellID);
                return null;
            }

            wo = WorldObjectFactory.CreateNewWorldObject(weenie.WeenieClassId);
            return wo;
        }

        private static int CreateLevel8SpellComp()
        {
            int upperLimit = LootTables.Level8SpellComps.Length - 1;
            int chance = ThreadSafeRandom.Next(0, upperLimit);

            return LootTables.Level8SpellComps[chance];
        }
    }
}
