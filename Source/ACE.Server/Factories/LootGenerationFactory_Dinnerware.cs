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
            uint id = 0;
            int chance;
            WorldObject wo;

            var tier = Math.Clamp(profile.Tier, 1, 8);

            int genericLootMatrixIndex = tier - 1;
            int upperLimit = LootTables.DinnerwareLootMatrix.Length - 1;

            chance = ThreadSafeRandom.Next(0, upperLimit);
            id = (uint)LootTables.DinnerwareLootMatrix[chance];

            wo = WorldObjectFactory.CreateNewWorldObject(id);

            if (wo != null && mutate)
                MutateDinnerware(wo, profile);

            return wo;
        }

        private static void MutateDinnerware(WorldObject wo, TreasureDeath profile, TreasureRoll roll = null)
        {
            // Dinnerware has all these options (plates, tankards, etc)
            // This is just a short-term fix until Loot is overhauled
            // TODO - Doesn't handle damage/speed/etc that the mutate engine should for these types of items.

            if (wo.GemCode != null)
                wo.GemCount = GemCountChance.Roll(wo.GemCode.Value, profile.Tier);
            else
                wo.GemCount = ThreadSafeRandom.Next(1, 5);

            wo.GemType = RollGemType(profile.Tier);

            wo.LongDesc = wo.Name;

            int materialType = GetMaterialType(wo, profile.Tier);
            wo.MaterialType = (MaterialType)materialType;
            int workmanship = GetWorkmanship(profile.Tier);
            wo.ItemWorkmanship = workmanship;

            //wo = SetAppraisalLongDescDecoration(wo);

            wo = AssignValue(wo);

            MutateColor(wo);

            // TODO: dinnerware could get spells?
        }

        private static bool GetMutateDinnerwareData(uint wcid)
        {
            return LootTables.DinnerwareLootMatrix.Contains((int)wcid);
        }
    }
}

