using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.WorldObjects;

namespace ACE.Server.Factories
{
    public static partial class LootGenerationFactory
    {
        private static int CreateFood()
        {
            int foodType = 0;
            foodType = LootTables.food[ThreadSafeRandom.Next(0, LootTables.food.Length - 1)];
            return foodType;
        }

        private static WorldObject CreateMundaneObjects(int tier)
        {
            uint id = 0;
            int chance;
            WorldObject wo;

            if (tier < 1) tier = 1;
            if (tier > 8) tier = 8;

            chance = ThreadSafeRandom.Next(0, 1);

            switch (chance)
            {
                case 0:
                    id = (uint)CreateFood();
                    break;
                default:
                    int mundaneLootMatrixIndex = tier - 1;
                    int upperLimit = LootTables.MundaneLootMatrix[mundaneLootMatrixIndex].Length - 1;

                    chance = ThreadSafeRandom.Next(0, upperLimit);
                    id = (uint)LootTables.MundaneLootMatrix[mundaneLootMatrixIndex][chance];
                    break;
            }

            if (id == 0)
                return null;

            wo = WorldObjectFactory.CreateNewWorldObject(id);

            // Dinnerware has all these options (plates, tankards, etc)
            // This is just a short-term fix until Loot is overhauled
            // TODO - Doesn't handle damage/speed/etc that the mutate engine should for these types of items.
            if (wo.TsysMutationData != null)
            {
                
                wo.SetProperty(PropertyInt.GemCount, ThreadSafeRandom.Next(1, 5));
                wo.SetProperty(PropertyInt.GemType, ThreadSafeRandom.Next(10, 50));

                wo.LongDesc = wo.Name;

                int materialType = GetMaterialType(wo, tier);
                wo.MaterialType = (MaterialType)materialType;
                int workmanship = GetWorkmanship(tier);
                wo.ItemWorkmanship = workmanship;

                wo = SetAppraisalLongDescDecoration(wo);

                wo = AssignValue(wo);
            }

            wo = RandomizeColor(wo);
            return wo;
        }
    }
}
