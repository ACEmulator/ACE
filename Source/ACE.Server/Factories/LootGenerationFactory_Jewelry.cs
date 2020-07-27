using System.Linq;

using ACE.Common;
using ACE.Database.Models.World;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.WorldObjects;

namespace ACE.Server.Factories
{
    public static partial class LootGenerationFactory
    {
        private static WorldObject CreateJewels(int tier, bool isMagical, bool mutate = true)
        {
            int gemLootMatrixIndex = tier - 1;

            if (gemLootMatrixIndex > 4) gemLootMatrixIndex = 4;
            int upperLimit = LootTables.GemsMatrix[gemLootMatrixIndex].Length - 1;

            uint gemWCID = (uint)LootTables.GemsWCIDsMatrix[gemLootMatrixIndex][ThreadSafeRandom.Next(0, upperLimit)];

            WorldObject wo = WorldObjectFactory.CreateNewWorldObject(gemWCID) as Gem;

            if (wo != null && mutate)
                MutateJewels(wo, tier, isMagical, gemLootMatrixIndex);

            return wo;
        }

        private static void MutateJewels(WorldObject wo, int tier, bool isMagical, int gemLootMatrixIndex)
        {
            uint gemType = 0;
            int workmanship = 0;
            int rank = 0;
            int difficulty = 0;
            int spellDID = 0;
            int skill_level_limit = 0;

            gemType = (uint)wo.MaterialType;

            workmanship = GetWorkmanship(tier);
            wo.ItemWorkmanship = workmanship;
            int value = LootTables.gemValues[(int)gemType] + ThreadSafeRandom.Next(1, LootTables.gemValues[(int)gemType]);
            wo.Value = value;

            gemLootMatrixIndex = tier - 1;
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
                        spellDID = LootTables.GemCreatureSpellMatrix[gemSpellIndex][ThreadSafeRandom.Next(0, LootTables.GemCreatureSpellMatrix[gemSpellIndex].Length - 1)];
                        break;
                    case 1:
                        gemSpellIndex = LootTables.GemSpellIndexMatrix[gemLootMatrixIndex][0];
                        spellDID = LootTables.GemLifeSpellMatrix[gemSpellIndex][ThreadSafeRandom.Next(0, LootTables.GemLifeSpellMatrix[gemSpellIndex].Length - 1)];
                        break;
                    case 2:
                        gemSpellIndex = LootTables.GemSpellIndexMatrix[gemLootMatrixIndex][1];
                        spellDID = LootTables.GemCreatureSpellMatrix[gemSpellIndex][ThreadSafeRandom.Next(0, LootTables.GemCreatureSpellMatrix[gemSpellIndex].Length - 1)];
                        break;
                    default:
                        gemSpellIndex = LootTables.GemSpellIndexMatrix[gemLootMatrixIndex][1];
                        spellDID = LootTables.GemLifeSpellMatrix[gemSpellIndex][ThreadSafeRandom.Next(0, LootTables.GemLifeSpellMatrix[gemSpellIndex].Length - 1)];
                        break;
                }

                int manaCost = 50 * gemSpellIndex;
                int spellcraft = 50 * gemSpellIndex;
                int maxMana = ThreadSafeRandom.Next(manaCost, manaCost + 50);

                wo.SpellDID = (uint)spellDID;
                wo.ItemAllegianceRankLimit= rank;
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
            RandomizeColor(wo);
        }

        private static bool GetMutateJewelsData(uint wcid, out int gemLootMatrixIndex)
        {
            for (gemLootMatrixIndex = 0; gemLootMatrixIndex < LootTables.GemsWCIDsMatrix.Length; gemLootMatrixIndex++)
            {
                if (LootTables.GemsWCIDsMatrix[gemLootMatrixIndex].Contains((int)wcid))
                    return true;
            }
            gemLootMatrixIndex = -1;
            return false;
        }

        private static WorldObject CreateJewelry(TreasureDeath profile, bool isMagical, bool mutate = true)
        {
            // 31% chance ring, 31% chance bracelet, 30% chance necklace 8% chance Trinket

            int jewelrySlot = ThreadSafeRandom.Next(1, 100);
            int jewelType;

            // Made this easier to read (switch -> if statement)
            if (jewelrySlot <= 31)
                jewelType = LootTables.ringItems[ThreadSafeRandom.Next(0, LootTables.ringItems.Length - 1)];
            else if (jewelrySlot <= 62)
                jewelType = LootTables.braceletItems[ThreadSafeRandom.Next(0, LootTables.braceletItems.Length - 1)];
            else if (jewelrySlot <= 92)
                jewelType = LootTables.necklaceItems[ThreadSafeRandom.Next(0, LootTables.necklaceItems.Length - 1)];
            else
                jewelType = LootTables.trinketItems[ThreadSafeRandom.Next(0, LootTables.trinketItems.Length - 1)];

            WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)jewelType);

            if (wo != null && mutate)
                MutateJewelry(wo, profile, isMagical);

            return wo;
        }

        private static void MutateJewelry(WorldObject wo, TreasureDeath profile, bool isMagical)
        {
            //wo.AppraisalLongDescDecoration = AppraisalLongDescDecorations.PrependWorkmanship;
            wo.LongDesc = wo.Name;
            int materialType = GetMaterialType(wo, profile.Tier);
            if (materialType > 0)
                wo.MaterialType = (MaterialType)materialType;
            int gemCount = ThreadSafeRandom.Next(1, 5);
            int gemType = ThreadSafeRandom.Next(10, 50);
            wo.GemCount = gemCount;
            wo.GemType = (MaterialType)gemType;
            int workmanship = GetWorkmanship(profile.Tier);

            double materialMod = LootTables.getMaterialValueModifier(wo);
            double gemMaterialMod = LootTables.getGemMaterialValueModifier(wo);
            var value = GetValue(profile.Tier, workmanship, gemMaterialMod, materialMod);
            wo.Value = value;
            wo.ItemWorkmanship = workmanship;

            wo.ItemSkillLevelLimit = null;

            if (profile.Tier > 6)
            {
                wo.WieldRequirements = WieldRequirement.Level;
                wo.WieldSkillType = (int)Skill.Axe;  // Set by examples from PCAP data

                var wield = profile.Tier switch
                {
                    7 => 150,// In this instance, used for indicating player level, rather than skill level
                    _ => 180,// In this instance, used for indicating player level, rather than skill level
                };
                wo.WieldDifficulty = wield;
            }

            if (isMagical)
                wo = AssignMagic(wo, profile);
            else
            {
                wo.ItemManaCost = null;
                wo.ItemMaxMana = null;
                wo.ItemCurMana = null;
                wo.ItemSpellcraft = null;
                wo.ItemDifficulty = null;
                wo.ManaRate = null;
            }

            RandomizeColor(wo);
        }

        private static bool GetMutateJewelryData(uint wcid)
        {
            foreach (var jewelryTable in LootTables.jewelryTables)
            {
                if (jewelryTable.Contains((int)wcid))
                    return true;
            }
            return false;
        }
    }
}
