using ACE.Common;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.WorldObjects;

namespace ACE.Server.Factories
{
    public static partial class LootGenerationFactory
    {
        private static WorldObject CreateDinnerware(int tier)
        {
            uint id = 0;
            int chance;
            WorldObject wo;

            if (tier < 1) tier = 1;
            if (tier > 8) tier = 8;

            int genericLootMatrixIndex = tier - 1;
            int upperLimit = LootTables.DinnerwareLootMatrix.Length - 1;

            chance = ThreadSafeRandom.Next(0, upperLimit);
            id = (uint)LootTables.DinnerwareLootMatrix[chance];

            if (id == 0)
                return null;

            wo = WorldObjectFactory.CreateNewWorldObject(id);

            // Dinnerware has all these options (plates, tankards, etc)
            // This is just a short-term fix until Loot is overhauled
            // TODO - Doesn't handle damage/speed/etc that the mutate engine should for these types of items.
            wo.SetProperty(PropertyInt.GemCount, ThreadSafeRandom.Next(1, 5));
            wo.SetProperty(PropertyInt.GemType, ThreadSafeRandom.Next(10, 50));

            wo.LongDesc = wo.Name;

            int materialType = GetMaterialType(wo, tier);
            wo.MaterialType = (MaterialType)materialType;
            int workmanship = GetWorkmanship(tier);
            wo.ItemWorkmanship = workmanship;

            wo = SetAppraisalLongDescDecoration(wo);

            wo = AssignValue(wo);

            wo = RandomizeColor(wo);

            return wo;
        }
    }
}

