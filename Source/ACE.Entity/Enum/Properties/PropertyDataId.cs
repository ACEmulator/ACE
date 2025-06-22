using System.ComponentModel;

namespace ACE.Entity.Enum.Properties
{
    // No properties are sent to the client unless they featured an attribute.
    // SendOnLogin gets sent to players in the PlayerDescription event
    // AssessmentProperty gets sent in successful appraisal
    public enum PropertyDataId : ushort
    {
        Undef                            = 0,
        Setup                            = 1,
        [SendOnLogin]
        MotionTable                      = 2,
        SoundTable                       = 3,
        [SendOnLogin]
        CombatTable                      = 4,
        QualityFilter                    = 5,
        PaletteBase                      = 6,
        ClothingBase                     = 7,
        Icon                             = 8,
        [AssessmentProperty]
        EyesTexture                      = 9,
        [AssessmentProperty]
        NoseTexture                      = 10,
        [AssessmentProperty]
        MouthTexture                     = 11,
        DefaultEyesTexture               = 12,
        DefaultNoseTexture               = 13,
        DefaultMouthTexture              = 14,
        [AssessmentProperty]
        HairPalette                      = 15,
        [AssessmentProperty]
        EyesPalette                      = 16,
        [AssessmentProperty]
        SkinPalette                      = 17,
        HeadObject                       = 18,
        ActivationAnimation              = 19,
        InitMotion                       = 20,
        ActivationSound                  = 21,
        PhysicsEffectTable               = 22,
        UseSound                         = 23,
        UseTargetAnimation               = 24,
        UseTargetSuccessAnimation        = 25,
        UseTargetFailureAnimation        = 26,
        UseUserAnimation                 = 27,
        Spell                            = 28,
        SpellComponent                   = 29,
        PhysicsScript                    = 30,
        LinkedPortalOne                  = 31,
        WieldedTreasureType              = 32,
        InventoryTreasureType            = 33,
        ShopTreasureType                 = 34,
        DeathTreasureType                = 35,
        MutateFilter                     = 36,
        ItemSkillLimit                   = 37,
        UseCreateItem                    = 38,
        DeathSpell                       = 39,
        VendorsClassId                   = 40,
        [AssessmentProperty]
        ItemSpecializedOnly              = 41,
        HouseId                          = 42,
        AccountHouseId                   = 43,
        RestrictionEffect                = 44,
        CreationMutationFilter           = 45,
        TsysMutationFilter               = 46,
        LastPortal                       = 47,
        LinkedPortalTwo                  = 48,
        OriginalPortal                   = 49,
        IconOverlay                      = 50,
        IconOverlaySecondary             = 51,
        IconUnderlay                     = 52,
        AugmentationMutationFilter       = 53,
        AugmentationEffect               = 54,
        [AssessmentProperty]
        ProcSpell                        = 55,
        AugmentationCreateItem           = 56,
        AlternateCurrency                = 57,
        BlueSurgeSpell                   = 58,
        YellowSurgeSpell                 = 59,
        RedSurgeSpell                    = 60,
        OlthoiDeathTreasureType          = 61,

        /* Custom Properties */
        PCAPRecordedWeenieHeader         = 8001,
        PCAPRecordedWeenieHeader2        = 8002,
        PCAPRecordedObjectDesc           = 8003,
        PCAPRecordedPhysicsDesc          = 8005,
        PCAPRecordedParentLocation       = 8009,
        PCAPRecordedDefaultScript        = 8019,
        PCAPRecordedTimestamp0           = 8020,
        PCAPRecordedTimestamp1           = 8021,
        PCAPRecordedTimestamp2           = 8022,
        PCAPRecordedTimestamp3           = 8023,
        PCAPRecordedTimestamp4           = 8024,
        PCAPRecordedTimestamp5           = 8025,
        PCAPRecordedTimestamp6           = 8026,
        PCAPRecordedTimestamp7           = 8027,
        PCAPRecordedTimestamp8           = 8028,
        PCAPRecordedTimestamp9           = 8029,
        PCAPRecordedMaxVelocityEstimated = 8030,
        PCAPPhysicsDIDDataTemplatedFrom  = 8044,
    }

    public static class PropertyDataIdExtensions
    {
        public static string GetValueEnumName(this PropertyDataId property, uint value)
        {
            switch (property)
            {
                case PropertyDataId.ActivationAnimation:
                case PropertyDataId.InitMotion:
                case PropertyDataId.UseTargetAnimation:
                case PropertyDataId.UseTargetFailureAnimation:
                case PropertyDataId.UseTargetSuccessAnimation:
                case PropertyDataId.UseUserAnimation:
                    return System.Enum.GetName(typeof(MotionCommand), value);
                case PropertyDataId.PhysicsScript:
                case PropertyDataId.RestrictionEffect:
                    return System.Enum.GetName(typeof(PlayScript), value);
                case PropertyDataId.ActivationSound:
                case PropertyDataId.UseSound:
                    return System.Enum.GetName(typeof(Sound), value);
                case PropertyDataId.WieldedTreasureType:
                case PropertyDataId.DeathTreasureType:
                case PropertyDataId.InventoryTreasureType:
                case PropertyDataId.ShopTreasureType:
                    // todo
                    break;
                case PropertyDataId.Spell:                
                case PropertyDataId.DeathSpell:
                case PropertyDataId.ProcSpell:
                case PropertyDataId.RedSurgeSpell:
                case PropertyDataId.BlueSurgeSpell:
                case PropertyDataId.YellowSurgeSpell:
                    return System.Enum.GetName(typeof(SpellId), value);

                case PropertyDataId.ItemSkillLimit:
                case PropertyDataId.ItemSpecializedOnly:
                    return System.Enum.GetName(typeof(Skill), value);

                case PropertyDataId.PCAPRecordedParentLocation:
                    return System.Enum.GetName(typeof(ParentLocation), value);
                case PropertyDataId.PCAPRecordedDefaultScript:
                    return System.Enum.GetName(typeof(MotionCommand), value);
            }

            return null;
        }

        public static bool IsHexData(this PropertyDataId property)
        {

            switch (property)
            {
                case PropertyDataId.AccountHouseId:
                case PropertyDataId.AlternateCurrency:
                case PropertyDataId.AugmentationCreateItem:
                case PropertyDataId.AugmentationEffect:
                case PropertyDataId.BlueSurgeSpell:
                case PropertyDataId.DeathSpell:
                case PropertyDataId.DeathTreasureType:
                case PropertyDataId.HouseId:
                case PropertyDataId.ItemSkillLimit:
                case PropertyDataId.ItemSpecializedOnly:
                case PropertyDataId.InventoryTreasureType:
                case PropertyDataId.LastPortal:
                case PropertyDataId.LinkedPortalOne:
                case PropertyDataId.LinkedPortalTwo:
                case PropertyDataId.OlthoiDeathTreasureType:
                case PropertyDataId.OriginalPortal:
                case PropertyDataId.PhysicsScript:
                case PropertyDataId.ProcSpell:
                case PropertyDataId.RedSurgeSpell:
                case PropertyDataId.RestrictionEffect:
                case PropertyDataId.ShopTreasureType:
                case PropertyDataId.Spell:
                case PropertyDataId.SpellComponent:
                case PropertyDataId.UseCreateItem:
                case PropertyDataId.UseSound:
                case PropertyDataId.VendorsClassId:
                case PropertyDataId.WieldedTreasureType:
                case PropertyDataId.YellowSurgeSpell:

                case PropertyDataId x when x >= PropertyDataId.PCAPRecordedWeenieHeader:
                    return false;

                default:
                    return true;
            }
        }
    }
}
