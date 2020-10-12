using System;
using System.Linq;

using ACE.Common;
using ACE.Database.Models.World;
using ACE.Entity.Enum;
using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Tables;
using ACE.Server.WorldObjects;

namespace ACE.Server.Factories
{
    public static partial class LootGenerationFactory
    {
        private static WorldObject CreateDinnerware(TreasureDeath profile, bool mutate = true)
        {
            var rng = ThreadSafeRandom.Next(0, LootTables.DinnerwareLootMatrix.Length - 1);

            var wcid = (uint)LootTables.DinnerwareLootMatrix[rng];

            var wo = WorldObjectFactory.CreateNewWorldObject(wcid);

            if (wo != null && mutate)
                MutateDinnerware(wo, profile);

            return wo;
        }

        private static void MutateDinnerware(WorldObject wo, TreasureDeath profile, TreasureRoll roll = null)
        {
            // Dinnerware has all these options (plates, tankards, etc)
            // This is just a short-term fix until Loot is overhauled
            // TODO - Doesn't handle damage/speed/etc that the mutate engine should for these types of items.

            // material type
            wo.MaterialType = (MaterialType)GetMaterialType(wo, profile.Tier);

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

            // TODO: dinnerware could get spells in retail

            wo.LongDesc = wo.Name;

            MutateDinnerware_ItemValue(wo);
        }

        private static void MutateDinnerware_ItemValue(WorldObject wo)
        {
            var materialMod = LootTables.getMaterialValueModifier(wo);
            var gemMaterialMod = LootTables.getGemMaterialValueModifier(wo);

            var baseValue = ThreadSafeRandom.Next(300, 600);

            var workmanship = wo.ItemWorkmanship ?? 1;

            wo.Value = (int)(baseValue * gemMaterialMod * materialMod * workmanship);
        }

        private static bool GetMutateDinnerwareData(uint wcid)
        {
            return LootTables.DinnerwareLootMatrix.Contains((int)wcid);
        }
    }
}

