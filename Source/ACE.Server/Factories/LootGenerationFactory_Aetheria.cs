
using ACE.Common;
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

            int chance;
            uint aetheriaType;

            if (tier < 5) return null;

            // TODO: drop percentage tweaks between types within a given tier, if needed
            switch (tier)
            {
                case 5:
                    aetheriaType = Aetheria.AetheriaBlue;
                    break;
                case 6:
                    chance = ThreadSafeRandom.Next(1, 10);  // Example 50/50 split between color type
                    if (chance <= 5)
                        aetheriaType = Aetheria.AetheriaBlue;
                    else
                        aetheriaType = Aetheria.AetheriaYellow;
                    break;
                default:
                    chance = ThreadSafeRandom.Next(1, 9); // Example 33% between color type
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


            // Initial roll for an Aetheria level 1 through 3
            wo.ItemMaxLevel = 1;

            var rng = ThreadSafeRandom.Next(1, 7);

            if (rng > 4)
            {
                if (rng > 6)
                    wo.ItemMaxLevel = 3;
                else
                    wo.ItemMaxLevel = 2;
            }

            // Perform an additional roll check for a chance at a higher Aetheria level for tiers 6+
            if (tier > 5)
            {
                if (ThreadSafeRandom.Next(1, 50) == 1)
                {
                    wo.ItemMaxLevel = 4;
                    if (tier > 6 && ThreadSafeRandom.Next(1, 5) == 1)
                    {
                        wo.ItemMaxLevel = 5;
                    }
                }
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
