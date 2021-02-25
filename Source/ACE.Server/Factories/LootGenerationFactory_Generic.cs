using ACE.Common;
using ACE.Database.Models.World;
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

        private static WorldObject CreateGenericObjects(TreasureDeath profile)
        {
            int chance;
            WorldObject wo;

            chance = ThreadSafeRandom.Next(1, 100);

            switch (chance)
            {
                case var rate when (rate < 2):
                    wo = WorldObjectFactory.CreateNewWorldObject(49485); // Encapsulated Spirit
                    break;
                case var rate when (rate < 10):
                    wo = CreateSummoningEssence(profile.Tier);
                    break;
                case var rate when (rate < 28):
                    wo = CreateRandomScroll(profile);
                    break;
                case var rate when (rate < 57):
                    wo = CreateFood();
                    break;
                default:
                    int genericLootMatrixIndex = profile.Tier - 1;
                    int upperLimit = LootTables.GenericLootMatrix[genericLootMatrixIndex].Length - 1;

                    chance = ThreadSafeRandom.Next(0, upperLimit);
                    uint id = (uint)LootTables.GenericLootMatrix[genericLootMatrixIndex][chance];

                    wo = WorldObjectFactory.CreateNewWorldObject(id);
                    break;
            }

            return wo;
        }
    }
}
