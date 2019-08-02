using ACE.Database;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
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
            Skill wieldSkillType = Skill.None;
            WieldRequirement wieldRequirement = WieldRequirement.RawSkill;
            int subType = 0;
            int wield = GetWield(tier, 2);

            ////Getting the caster Weenie needed.
            if (wield == 0)
            {
                // Determine plain caster type: 0 - Orb, 1 - Sceptre, 2 - Staff, 3 - Wand
                subType = ThreadSafeRandom.Next(0, 3);
                casterWeenie = LootTables.CasterWeaponsMatrix[wield][subType];

                if (tier > 6)
                {
                    wieldRequirement = WieldRequirement.Level;
                    wieldSkillType = Skill.Axe;  // Set by examples from PCAP data

                    switch (tier)
                    {
                        case 7:
                            wield = 150; // In this instance, used for indicating player level, rather than skill level
                            break;
                        default:
                            wield = 180; // In this instance, used for indicating player level, rather than skill level
                            break;
                    }
                }
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

                // If element is Nether, Void Magic is required, else War Magic is required for all other elements
                if (element == 7)
                    wieldSkillType = Skill.VoidMagic;
                else
                    wieldSkillType = Skill.WarMagic;
            }

            WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)casterWeenie);

            if (wo == null)
                return null;

            int workmanship = GetWorkmanship(tier);
            wo.SetProperty(PropertyInt.ItemWorkmanship, workmanship);
            int materialType = GetMaterialType(wo, tier);
            if (materialType > 0)
                wo.MaterialType = (MaterialType)materialType;
            wo.SetProperty(PropertyInt.MaterialType, GetMaterialType(wo, tier));
            wo.SetProperty(PropertyInt.GemCount, ThreadSafeRandom.Next(1, 5));

            wo.SetProperty(PropertyInt.GemType, ThreadSafeRandom.Next(10, 50));
            wo.SetProperty(PropertyString.LongDesc, wo.GetProperty(PropertyString.Name));

            double materialMod = LootTables.getMaterialValueModifier(wo);
            double gemMaterialMod = LootTables.getGemMaterialValueModifier(wo);
            var value = GetValue(tier, workmanship, gemMaterialMod, materialMod);
            wo.Value = value;

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

            if (wield > 0 || wieldRequirement == WieldRequirement.Level)
            {
                wo.SetProperty(PropertyInt.WieldRequirements, (int)wieldRequirement);
                wo.SetProperty(PropertyInt.WieldSkillType, (int)wieldSkillType);
                wo.SetProperty(PropertyInt.WieldDifficulty, wield);
            }
            else
            {
                wo.RemoveProperty(PropertyInt.WieldRequirements);
                wo.RemoveProperty(PropertyInt.WieldSkillType);
                wo.RemoveProperty(PropertyInt.WieldDifficulty);
            }

            wo.RemoveProperty(PropertyInt.ItemSkillLevelLimit);

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

            wo = RandomizeColor(wo);
            return wo;
        }
    }
}
