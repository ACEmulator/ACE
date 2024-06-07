using log4net;

using ACE.Database.Models.World;
using ACE.Server.Factories.Entity;
using ACE.Server.WorldObjects;
using ACE.Common;

namespace ACE.Server.Factories.Tables
{
    public static class GearRatingChance
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        private static ChanceTable<bool> RatingChance = new ChanceTable<bool>()
        {
            ( false, 0.6f ),
            ( true,  0.4f )
        };

        private static ChanceTable<int> ArmorRating = new ChanceTable<int>()
        {
            ( 1, 0.20f ),
            ( 2, 0.20f ),
            ( 3, 0.15f ),
            ( 4, 0.15f ),
            ( 5, 0.075f ),
            ( 6, 0.075f ),
            ( 7, 0.0575f ),
            ( 8, 0.055f ),
            ( 9, 0.025f ),
            ( 10, 0.0125f )
        };

        private static ChanceTable<int> ClothingJewelryRating = new ChanceTable<int>()
        {
            ( 1, 0.20f ),
            ( 2, 0.20f ),
            ( 3, 0.15f ),
            ( 4, 0.15f ),
            ( 5, 0.075f ),
            ( 6, 0.075f ),
            ( 7, 0.0575f ),
            ( 8, 0.055f ),
            ( 9, 0.025f ),
            ( 10, 0.0125f )
        };

        private static ChanceTable<int> TierRatingMod9 = new ChanceTable<int>()
        {
            ( 0, 0.025f ),
            ( 1, 0.075f ),
            ( 2, 0.425f ),
            ( 3, 0.425f ),
            ( 4, 0.025f ),
            ( 5, 0.025f )
        };

        private static ChanceTable<int> TierRatingMod10 = new ChanceTable<int>()
        {
            ( 9, 0.8f ),
            ( 10, 0.065f ),
            ( 11, 0.045f ),
            ( 12, 0.040f ),
            ( 13, 0.0375f ),
            ( 14, 0.0125f )
        };

        private static ChanceTable<int> TierRatingMod11 = new ChanceTable<int>()
        {
            ( 15, 0.8f ),
            ( 16, 0.065f ),
            ( 17, 0.045f ),
            ( 18, 0.040f ),
            ( 19, 0.0375f ),
            ( 20, 0.0125f )
        };

        private static ChanceTable<int> TierRatingMod12 = new ChanceTable<int>()
        {
            ( 20, 0.75f ),
            ( 21, 0.225f ),
            ( 22, 0.025f ),
        };


        public static int Roll(WorldObject wo, TreasureDeath profile, TreasureRoll roll)
        {
            // initial roll for rating chance
            if (!RatingChance.Roll(profile.LootQualityMod) && profile.Tier < 9)
                return 0;

            // roll for the actual rating
            ChanceTable<int> rating = null;

            ChanceTable<int> ratingTierMod = null;


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

            switch (profile.Tier)
            {
                case 9:
                    ratingTierMod = TierRatingMod9;
                    break;
                case 10:
                    ratingTierMod = TierRatingMod10;
                    break;
                case 11:
                    ratingTierMod = TierRatingMod11;
                    break;
                case 12:
                    ratingTierMod = TierRatingMod12;
                    break;
                default:
                    break;
            }

            if (ratingTierMod != null)
            {
                return (rating.Roll(profile.LootQualityMod) + ratingTierMod.Roll(profile.LootQualityMod));
            }
            else
            {
                return rating.Roll(profile.LootQualityMod);
            }
        }
    }
}
