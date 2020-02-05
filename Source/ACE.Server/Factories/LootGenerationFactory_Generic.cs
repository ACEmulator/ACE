using ACE.Common;
using ACE.Server.WorldObjects;

namespace ACE.Server.Factories
{
    public static partial class LootGenerationFactory
    {
        private static WorldObject CreateFood()
        {
            uint foodType = (uint)LootTables.food[ThreadSafeRandom.Next(0, LootTables.food.Length - 1)];
            return WorldObjectFactory.CreateNewWorldObject(foodType);
        }

        private static WorldObject CreateGenericObjects(int tier)
        {
            int chance;
            WorldObject wo;

            if (tier < 1) tier = 1;
            if (tier > 8) tier = 8;

            chance = ThreadSafeRandom.Next(1, 100);

            switch (chance)
            {
                case var rate when (rate < 2):
                    wo = WorldObjectFactory.CreateNewWorldObject(49485); // Encapsulated Spirit
                    break;
                case var rate when (rate < 10):
                    wo = CreateSummoningEssence(tier);
                    break;
                case var rate when (rate < 28):
                    wo = CreateRandomScroll(tier);
                    break;
                case var rate when (rate < 57):
                    wo = CreateFood();
                    break;
                default:
                    int genericLootMatrixIndex = tier - 1;
                    int upperLimit = LootTables.GenericLootMatrix[genericLootMatrixIndex].Length - 1;

                    chance = ThreadSafeRandom.Next(0, upperLimit);
                    uint id = (uint)LootTables.GenericLootMatrix[genericLootMatrixIndex][chance];

                    if (id == 0)
                        return null;

                    wo = WorldObjectFactory.CreateNewWorldObject(id);
                    break;
            }

            return wo;
        }
    }
}
