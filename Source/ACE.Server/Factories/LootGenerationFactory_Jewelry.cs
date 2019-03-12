using ACE.Database.Models.Shard;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Factories;
using ACE.Server.WorldObjects;

namespace ACE.Server.Factories
{
    public static partial class LootGenerationFactory
    {
        private static WorldObject CreateJewels(int tier, bool isMagical)
        {
            uint gemType = 0;
            int workmanship = 0;
            int rank = 0;
            int difficulty = 0;
            int spellDID = 0;
            int skill_level_limit = 0;

            int gemLootMatrixIndex = tier - 1;

            if (gemLootMatrixIndex > 4) gemLootMatrixIndex = 4;
            int upperLimit = LootTables.GemsMatrix[gemLootMatrixIndex].Length - 1;

            gemType = (uint)LootTables.GemsMatrix[gemLootMatrixIndex][ThreadSafeRandom.Next(0, upperLimit)];

            WorldObject wo = WorldObjectFactory.CreateNewWorldObject(gemType) as Gem;

            if (wo == null)
                return null;

            workmanship = GetWorkmanship(tier);
            wo.SetProperty(PropertyInt.ItemWorkmanship, workmanship);
            wo.SetProperty(PropertyInt.Value, GetValue(tier, workmanship));

            gemLootMatrixIndex = tier - 1;
            if (isMagical)
            {
                wo.SetProperty(PropertyInt.ItemUseable, 8);
                wo.SetProperty(PropertyInt.UiEffects, (int)UiEffects.Magical);

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

                wo.SetProperty(PropertyDataId.Spell, (uint)spellDID);
                wo.SetProperty(PropertyInt.ItemAllegianceRankLimit, rank);
                wo.SetProperty(PropertyInt.ItemDifficulty, difficulty);
                wo.SetProperty(PropertyInt.ItemManaCost, manaCost);
                wo.SetProperty(PropertyInt.ItemMaxMana, maxMana);
                wo.SetProperty(PropertyInt.ItemSkillLevelLimit, skill_level_limit);
                wo.SetProperty(PropertyInt.ItemSpellcraft, spellcraft);
            }
            else
            {
                wo.SetProperty(PropertyInt.ItemUseable, 1);

                wo.RemoveProperty(PropertyInt.ItemManaCost);
                wo.RemoveProperty(PropertyInt.ItemMaxMana);
                wo.RemoveProperty(PropertyInt.ItemCurMana);
                wo.RemoveProperty(PropertyInt.ItemSpellcraft);
                wo.RemoveProperty(PropertyInt.ItemDifficulty);
                wo.RemoveProperty(PropertyInt.ItemSkillLevelLimit);
                wo.RemoveProperty(PropertyDataId.Spell);
            }

            return wo;
        }

        private static WorldObject CreateJewelry(int tier, bool isMagical)
        {

            int[][] JewelrySpells = LootTables.JewelrySpells;
            int[][] JewelryCantrips = LootTables.JewelryCantrips;
            int[] jewelryItems = { 621, 295, 297, 294, 623, 622 };
            int jewelType = jewelryItems[ThreadSafeRandom.Next(0, jewelryItems.Length - 1)];

            //int rank = 0;
            //int skill_level_limit = 0;

            WorldObject wo = WorldObjectFactory.CreateNewWorldObject((uint)jewelType);

            if (wo == null)
                return null;

            wo.SetProperty(PropertyInt.AppraisalLongDescDecoration, 1);
            wo.SetProperty(PropertyString.LongDesc, wo.GetProperty(PropertyString.Name));

            int workmanship = GetWorkmanship(tier);
            int value = GetValue(tier, workmanship);
            wo.SetProperty(PropertyInt.Value, value);
            wo.SetProperty(PropertyInt.ItemWorkmanship, workmanship);

            int mT = GetMaterialType(1, tier);
            wo.SetProperty(PropertyInt.MaterialType, mT);

            int gemCount = ThreadSafeRandom.Next(1, 5);
            int gemType = ThreadSafeRandom.Next(10, 50);
            wo.SetProperty(PropertyInt.GemCount, gemCount);
            wo.SetProperty(PropertyInt.GemType, gemType);

            wo.RemoveProperty(PropertyInt.ItemSkillLevelLimit);

            if (isMagical)
            {
                wo.SetProperty(PropertyInt.UiEffects, (int)UiEffects.Magical);
                int numSpells = GetNumSpells(tier);

                int spellcraft = GetSpellcraft(numSpells, tier);
                int maxMana = GetMaxMana(numSpells, tier);
                wo.SetProperty(PropertyFloat.ManaRate, GetManaRate());
                wo.SetProperty(PropertyInt.ItemMaxMana, maxMana);
                wo.SetProperty(PropertyInt.ItemCurMana, maxMana);
                wo.SetProperty(PropertyInt.ItemSpellcraft, spellcraft);

                int difficulty = GetDifficulty(tier, spellcraft);
                wo.SetProperty(PropertyInt.ItemDifficulty, difficulty);

                int minorCantrips = GetNumMinorCantrips(tier);
                int majorCantrips = GetNumMajorCantrips(tier);
                int epicCantrips = GetNumEpicCantrips(tier);
                int legendaryCantrips = GetNumLegendaryCantrips(tier);

                int numCantrips = minorCantrips + majorCantrips + epicCantrips + legendaryCantrips;
                if (numCantrips > 10)
                    minorCantrips = 0;

                int[] shuffledValues = new int[JewelrySpells.Length];
                for (int i = 0; i < JewelrySpells.Length; i++)
                {
                    shuffledValues[i] = i;
                }
                Shuffle(shuffledValues);

                int lowSpellTier = GetLowSpellTier(tier);
                int highSpellTier = GetHighSpellTier(tier);
                if (numSpells - numCantrips > 0)
                {
                    for (int a = 0; a < numSpells - numCantrips; a++)
                    {
                        int col = ThreadSafeRandom.Next(lowSpellTier - 1, highSpellTier - 1);
                        int spellID = JewelrySpells[shuffledValues[a]][col];
                        var result = new BiotaPropertiesSpellBook { ObjectId = wo.Biota.Id, Spell = spellID, Object = wo.Biota };
                        wo.Biota.BiotaPropertiesSpellBook.Add(result);
                    }
                }

                if (numCantrips > 0)
                {
                    shuffledValues = new int[JewelryCantrips.Length];
                    for (int i = 0; i < JewelryCantrips.Length; i++)
                    {
                        shuffledValues[i] = i;
                    }
                    Shuffle(shuffledValues);
                    int shuffledPlace = 0;
                    //minor cantripps
                    for (int a = 0; a < minorCantrips; a++)
                    {
                        int spellID = JewelryCantrips[shuffledValues[shuffledPlace]][0];
                        shuffledPlace++;
                        var result = new BiotaPropertiesSpellBook { ObjectId = wo.Biota.Id, Spell = spellID, Object = wo.Biota };
                        wo.Biota.BiotaPropertiesSpellBook.Add(result);
                    }
                    //major cantrips
                    for (int a = 0; a < majorCantrips; a++)
                    {
                        int spellID = JewelryCantrips[shuffledValues[shuffledPlace]][1];
                        shuffledPlace++;
                        var result = new BiotaPropertiesSpellBook { ObjectId = wo.Biota.Id, Spell = spellID, Object = wo.Biota };
                        wo.Biota.BiotaPropertiesSpellBook.Add(result);
                    }
                    // epic cantrips
                    for (int a = 0; a < epicCantrips; a++)
                    {
                        int spellID = JewelryCantrips[shuffledValues[shuffledPlace]][2];
                        shuffledPlace++;
                        var result = new BiotaPropertiesSpellBook { ObjectId = wo.Biota.Id, Spell = spellID, Object = wo.Biota };
                        wo.Biota.BiotaPropertiesSpellBook.Add(result);
                    }
                    //legendary cantrips
                    for (int a = 0; a < legendaryCantrips; a++)
                    {
                        int spellID = JewelryCantrips[shuffledValues[shuffledPlace]][3];
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
                wo.RemoveProperty(PropertyFloat.ManaRate);
            }

            return wo;
        }
    }
}
