using System.Linq;

using ACE.Common;
using ACE.Entity.Enum;
using ACE.Server.Factories.Tables;
using ACE.Server.WorldObjects;

namespace ACE.Server.Factories
{
    public static partial class LootGenerationFactory
    {
        private static WorldObject CreateGem(int tier, bool isMagical, bool mutate = true)
        {
            int gemLootMatrixIndex = tier - 1;

            if (gemLootMatrixIndex > 4) gemLootMatrixIndex = 4;
            int upperLimit = LootTables.GemsMatrix[gemLootMatrixIndex].Length - 1;

            uint gemWCID = (uint)LootTables.GemsWCIDsMatrix[gemLootMatrixIndex][ThreadSafeRandom.Next(0, upperLimit)];

            WorldObject wo = WorldObjectFactory.CreateNewWorldObject(gemWCID) as Gem;

            if (wo != null && mutate)
                MutateGem(wo, tier, isMagical);

            return wo;
        }

        private static void MutateGem(WorldObject wo, int tier, bool isMagical)
        {
            uint gemType = 0;
            int workmanship = 0;
            int rank = 0;
            int difficulty = 0;
            SpellId spellDID = 0;
            int skill_level_limit = 0;

            gemType = (uint)wo.MaterialType;

            workmanship = GetWorkmanship(tier);
            wo.ItemWorkmanship = workmanship;
            int value = LootTables.gemValues[(int)gemType] + ThreadSafeRandom.Next(1, LootTables.gemValues[(int)gemType]);
            wo.Value = value;

            // TODO: here tier N always rolls a level N spell
            // in retail, each tier could roll different levels of spells. each tier had a spell level chance table
            // for example, tier 8 might have had a 75% chance to roll level 7 spells, and a 25% chance to roll level 8 spells

            var gemLootMatrixIndex = tier - 1;
            if (isMagical)
            {
                wo.ItemUseable = Usable.Contained;
                wo.UiEffects = UiEffects.Magical;

                int gemSpellIndex;
                int spellChance = 0;

                spellChance = ThreadSafeRandom.Next(0, 3);
                switch (spellChance)
                {
                    case 0:
                        gemSpellIndex = LootTables.GemSpellIndexMatrix[gemLootMatrixIndex][0];
                        spellDID = GemSpells.GemCreatureSpellMatrix[gemSpellIndex][ThreadSafeRandom.Next(0, GemSpells.GemCreatureSpellMatrix[gemSpellIndex].Length - 1)];
                        break;
                    case 1:
                        gemSpellIndex = LootTables.GemSpellIndexMatrix[gemLootMatrixIndex][0];
                        spellDID = GemSpells.GemLifeSpellMatrix[gemSpellIndex][ThreadSafeRandom.Next(0, GemSpells.GemLifeSpellMatrix[gemSpellIndex].Length - 1)];
                        break;
                    case 2:
                        gemSpellIndex = LootTables.GemSpellIndexMatrix[gemLootMatrixIndex][1];
                        spellDID = GemSpells.GemCreatureSpellMatrix[gemSpellIndex][ThreadSafeRandom.Next(0, GemSpells.GemCreatureSpellMatrix[gemSpellIndex].Length - 1)];
                        break;
                    default:
                        gemSpellIndex = LootTables.GemSpellIndexMatrix[gemLootMatrixIndex][1];
                        spellDID = GemSpells.GemLifeSpellMatrix[gemSpellIndex][ThreadSafeRandom.Next(0, GemSpells.GemLifeSpellMatrix[gemSpellIndex].Length - 1)];
                        break;
                }

                int manaCost = 50 * gemSpellIndex;
                int spellcraft = 50 * gemSpellIndex;
                int maxMana = ThreadSafeRandom.Next(manaCost, manaCost + 50);

                wo.SpellDID = (uint)spellDID;
                wo.ItemAllegianceRankLimit = rank;
                wo.ItemDifficulty = difficulty;
                wo.ItemManaCost = manaCost;
                wo.ItemMaxMana = maxMana;
                wo.ItemSkillLevelLimit = skill_level_limit;
                wo.ItemSpellcraft = spellcraft;
            }
            else
            {
                wo.ItemUseable = Usable.No;
                wo.SpellDID = null;
                wo.ItemManaCost = null;
                wo.ItemMaxMana = null;
                wo.ItemCurMana = null;
                wo.ItemSpellcraft = null;
                wo.ItemDifficulty = null;
                wo.ItemSkillLevelLimit = null;
                wo.ManaRate = null;

            }
            MutateColor(wo);
        }

        private static bool GetMutateGemData(uint wcid)
        {
            for (var gemLootMatrixIndex = 0; gemLootMatrixIndex < LootTables.GemsWCIDsMatrix.Length; gemLootMatrixIndex++)
            {
                if (LootTables.GemsWCIDsMatrix[gemLootMatrixIndex].Contains((int)wcid))
                    return true;
            }
            return false;
        }

        private static void MutateValue_Gem(WorldObject wo)
        {
            wo.Value = (int)(wo.Value * MaterialTable.GetValueMod(wo.MaterialType) * wo.ItemWorkmanship);
        }
    }
}
