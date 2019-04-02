using ACE.Server.Entity;
using ACE.Server.WorldObjects;

namespace ACE.Server.Factories
{
    public static partial class LootGenerationFactory
    {
        private static WorldObject CreateAetheria(int tier)
        {
            const uint aetheriaIconOverlayOne   = 100690996;
            const uint aetheriaIconOverlayTwo   = 100690997;
            const uint aetheriaIconOverlayThree = 100690998;
            const uint aetheriaIconOverlayFour  = 100690999;
            const uint aetheriaIconOverlayFive  = 100691000;

            uint chance;
            uint aetheriaType;

            if (tier < 5) return null;

            switch (tier)
            {
                case 5:
                    aetheriaType = Aetheria.AetheriaBlue;
                    break;
                case 6:
                    chance = ThreadSafeRandom.Next(1u, 10u);
                    if (chance <= 5)
                        aetheriaType = Aetheria.AetheriaBlue;
                    else
                        aetheriaType = Aetheria.AetheriaYellow;
                    break;
                default:
                    chance = ThreadSafeRandom.Next(1u, 9u);
                    if (chance <= 3)
                        aetheriaType = Aetheria.AetheriaBlue;
                    else if (chance <= 6)
                        aetheriaType = Aetheria.AetheriaYellow;
                    else
                        aetheriaType = Aetheria.AetheriaRed;
                    break;
            }

            WorldObject wo = WorldObjectFactory.CreateNewWorldObject(aetheriaType) as Gem;

            if (wo == null)
                return null;

            switch (tier)
            {
                case 5:
                    wo.ItemMaxLevel = ThreadSafeRandom.Next(1, 3);
                    break;
                case 6:
                    wo.ItemMaxLevel = ThreadSafeRandom.Next(1, 4);
                    break;
                default:
                    wo.ItemMaxLevel = ThreadSafeRandom.Next(1, 5);
                    break;
            }

            switch (wo.ItemMaxLevel)
            {
                case 1:
                    wo.IconOverlayId = aetheriaIconOverlayOne;
                    break;
                case 2:
                    wo.IconOverlayId = aetheriaIconOverlayTwo;
                    break;
                case 3:
                    wo.IconOverlayId = aetheriaIconOverlayThree;
                    break;
                case 4:
                    wo.IconOverlayId = aetheriaIconOverlayFour;
                    break;
                default:
                    wo.IconOverlayId = aetheriaIconOverlayFive;
                    break;
            }

            return wo;
        }
    }
}
