using log4net;

using ACE.Database.Models.World;
using ACE.Server.Factories.Entity;
using ACE.Server.WorldObjects;

namespace ACE.Server.Factories.Tables
{
    public static class GearRatingChance
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        private static ChanceTable<bool> RatingChance = new ChanceTable<bool>()
        {
            ( false, 0.75f ),
            ( true,  0.25f ),
        };

        private static ChanceTable<int> ArmorRating = new ChanceTable<int>()
        {
            ( 1, 0.95f ),
            ( 2, 0.05f ),
        };

        private static ChanceTable<int> ClothingJewelryRating = new ChanceTable<int>()
        {
            ( 1, 0.70f ),
            ( 2, 0.25f ),
            ( 3, 0.05f ),
        };

        public static int Roll(WorldObject wo, TreasureDeath profile, TreasureRoll roll)
        {
            // initial roll for rating chance
            if (!RatingChance.Roll(profile.LootQualityMod))
                return 0;

            // roll for the actual rating
            ChanceTable<int> rating = null;

            if (roll.HasArmorLevel(wo))
            {
                rating = ArmorRating;
            }
            else if (roll.IsClothing || roll.IsJewelry || roll.IsCloak)
            {
                rating = ClothingJewelryRating;
            }
            else
            {
                log.Error($"GearRatingChance.Roll({wo.Name}, {profile.TreasureType}, {roll.ItemType}): unknown item type");
                return 0;
            }

            return rating.Roll(profile.LootQualityMod);
        }
    }
}
