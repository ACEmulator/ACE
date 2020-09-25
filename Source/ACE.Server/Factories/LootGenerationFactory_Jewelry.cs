using System.Linq;

using ACE.Common;
using ACE.Database.Models.World;
using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Factories.Tables;
using ACE.Server.WorldObjects;

namespace ACE.Server.Factories
{
    public static partial class LootGenerationFactory
    {
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
            int materialType = GetMaterialType(wo, profile.Tier);
            if (materialType > 0)
                wo.MaterialType = (MaterialType)materialType;

            if (wo.GemCode != null)
                wo.GemCount = GemCountChance.Roll(wo.GemCode.Value, profile.Tier);
            else
                wo.GemCount = ThreadSafeRandom.Next(1, 5);

            wo.GemType = RollGemType(profile.Tier);

            wo.ItemWorkmanship = GetWorkmanship(profile.Tier);

            double materialMod = LootTables.getMaterialValueModifier(wo);
            double gemMaterialMod = LootTables.getGemMaterialValueModifier(wo);
            var value = GetValue(profile.Tier, wo.ItemWorkmanship.Value, gemMaterialMod, materialMod);
            wo.Value = value;

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
                AssignMagic(wo, profile);
            else
            {
                wo.ItemManaCost = null;
                wo.ItemMaxMana = null;
                wo.ItemCurMana = null;
                wo.ItemSpellcraft = null;
                wo.ItemDifficulty = null;
                wo.ManaRate = null;
            }

            //wo.AppraisalLongDescDecoration = AppraisalLongDescDecorations.PrependWorkmanship;
            wo.LongDesc = GetLongDesc(wo);

            MutateColor(wo);
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
