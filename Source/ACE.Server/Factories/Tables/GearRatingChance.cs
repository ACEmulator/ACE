using log4net;

using ACE.Database.Models.World;
using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;
using ACE.Server.WorldObjects;

namespace ACE.Server.Factories.Tables
{
    public static class GearRatingChance
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly ChanceTable<bool> ArmorClothing_RatingChance = new ChanceTable<bool>()
        {
            ( false, 0.75f ),
            ( true,  0.25f ),
        };

        private static readonly ChanceTable<bool> Jewelry_RatingChance = new ChanceTable<bool>()
        {
            ( false, 0.85f ),
            ( true,  0.15f ),
        };

        private static readonly ChanceTable<int> ArmorRating = new ChanceTable<int>()
        {
            ( 1, 0.95f ),
            ( 2, 0.05f ),
        };

        private static readonly ChanceTable<int> ClothingJewelry_Rating = new ChanceTable<int>()
        {
            ( 1, 0.70f ),
            ( 2, 0.25f ),
            ( 3, 0.05f ),
        };

        public static int Roll(WorldObject wo, TreasureDeath profile, TreasureRoll roll)
        {
            // roll for rating chance
            ChanceTable<bool> chance = null;

            if (roll.HasArmorLevel(wo) || roll.IsClothing)
                chance = ArmorClothing_RatingChance;
            else if (roll.ItemType == TreasureItemType_Orig.Jewelry)
                chance = Jewelry_RatingChance;
            else
            {
                log.Error($"GearRatingChance.Roll({wo.Name}, {profile.TreasureType}, {roll.ItemType}): unknown item type");
                return 0;
            }

            var hasRating = chance.Roll(profile.LootQualityMod);

            if (!hasRating)
                return 0;

            // roll for the actual rating
            ChanceTable<int> rating = null;

            if (roll.HasArmorLevel(wo))
                rating = ArmorRating;
            else if (roll.IsClothing || roll.ItemType == TreasureItemType_Orig.Jewelry)
                rating = ClothingJewelry_Rating;

            return rating.Roll(profile.LootQualityMod);
        }
    }
}
