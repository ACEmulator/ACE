using ACE.Factories;
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
            return wo;
        }
    }
}
