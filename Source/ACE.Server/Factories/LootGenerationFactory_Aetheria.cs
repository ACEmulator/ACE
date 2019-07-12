using ACE.Server.Entity;
using ACE.Server.Managers;
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


            // Initial role for an Aetheria level 1 through 3
            wo.ItemMaxLevel = ThreadSafeRandom.Next(1, 3);

            // Perform an additional role check for a chance at a higher Aetheria level for tiers 6+
            if (tier > 5)
            {
                double dropRateSkew = PropertyManager.GetDouble("aetheria_level_drop_rate_mod").Item;
                if (dropRateSkew <= 0)
                    dropRateSkew = 1;

                int aetheriaHigherLevelChance = ThreadSafeRandom.Next(1, (int)(100 * dropRateSkew));
                switch (tier)
                {
                    case 6:
                        if (aetheriaHigherLevelChance <= 10)
                            wo.ItemMaxLevel = 4;
                        break;
                    case 7:
                    case 8:
                        if (aetheriaHigherLevelChance <= 10)
                            wo.ItemMaxLevel = 4;
                        else if (aetheriaHigherLevelChance <= 5)
                            wo.ItemMaxLevel = 5;
                        break;
                    default:
                        break;
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
