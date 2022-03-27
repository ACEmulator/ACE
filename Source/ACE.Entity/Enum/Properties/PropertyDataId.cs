using System.ComponentModel;

namespace ACE.Entity.Enum.Properties
{
    public enum PropertyDataId : ushort
    {
        // properties marked as ServerOnly are properties we never saw in PCAPs, from here:
        // http://ac.yotesfan.com/ace_object/not_used_enums.php
        // source: @OptimShi
        // description attributes are used by the weenie editor for a cleaner display name

        [ServerOnly]
        Undef                      = 0,
        [ServerOnly]
        Setup                      = 1,
        [SendOnLogin]
        MotionTable                = 2,
        [ServerOnly]
        SoundTable                 = 3,
        [SendOnLogin]
        CombatTable                = 4,
        [ServerOnly]
        QualityFilter              = 5,
        [ServerOnly]
        PaletteBase                = 6,
        [ServerOnly]
        ClothingBase               = 7,
        [ServerOnly]
        Icon                       = 8,
        [AssessmentProperty]
        EyesTexture                = 9,
        [AssessmentProperty]
        NoseTexture                = 10,
        [AssessmentProperty]
        MouthTexture               = 11,
        [ServerOnly]
        DefaultEyesTexture         = 12,
        [ServerOnly]
        DefaultNoseTexture         = 13,
        [ServerOnly]
        DefaultMouthTexture        = 14,
        [AssessmentProperty]
        HairPalette                = 15,
        [AssessmentProperty]
        EyesPalette                = 16,
        [AssessmentProperty]
        SkinPalette                = 17,
        [ServerOnly]
        HeadObject                 = 18,
        [ServerOnly]
        ActivationAnimation        = 19,
        [ServerOnly]
        InitMotion                 = 20,
        [ServerOnly]
        ActivationSound            = 21,
        [ServerOnly]
        PhysicsEffectTable         = 22,
        [ServerOnly]
        UseSound                   = 23,
        [ServerOnly]
        UseTargetAnimation         = 24,
        [ServerOnly]
        UseTargetSuccessAnimation  = 25,
        [ServerOnly]
        UseTargetFailureAnimation  = 26,
        [ServerOnly]
        UseUserAnimation           = 27,
        [ServerOnly]
        Spell                      = 28,
        [ServerOnly]
        SpellComponent             = 29,
        [ServerOnly]
        PhysicsScript              = 30,
        [ServerOnly]
        LinkedPortalOne            = 31,
        [ServerOnly]
        WieldedTreasureType        = 32,
        [ServerOnly]
        InventoryTreasureType      = 33,
        [ServerOnly]
        ShowTreasureType           = 34,
        [ServerOnly]
        DeathTreasureType          = 35,
        [ServerOnly]
        MutateFilter               = 36,
        [ServerOnly]
        ItemSkillLimit             = 37,
        [ServerOnly]
        UseCreateItem              = 38,
        [ServerOnly]
        DeathSpell                 = 39,
        [ServerOnly]
        VendorsClassId             = 40,
        [ServerOnly]
        ItemSpecializedOnly        = 41,
        [ServerOnly]
        HouseId                    = 42,
        [ServerOnly]
        AccountHouseId             = 43,
        [ServerOnly]
        RestrictionEffect          = 44,
        [ServerOnly]
        CreationMutationFilter     = 45,
        [ServerOnly]
        TsysMutationFilter         = 46,
        [ServerOnly]
        LastPortal                 = 47,
        [ServerOnly]
        LinkedPortalTwo            = 48,
        [ServerOnly]
        OriginalPortal             = 49,
        [ServerOnly]
        IconOverlay                = 50,
        [ServerOnly]
        IconOverlaySecondary       = 51,
        [ServerOnly]
        IconUnderlay               = 52,
        [ServerOnly]
        AugmentationMutationFilter = 53,
        [ServerOnly]
        AugmentationEffect         = 54,
        ProcSpell                  = 55,
        [ServerOnly]
        AugmentationCreateItem     = 56,
        [ServerOnly]
        AlternateCurrency          = 57,
        [ServerOnly]
        BlueSurgeSpell             = 58,
        [ServerOnly]
        YellowSurgeSpell           = 59,
        [ServerOnly]
        RedSurgeSpell              = 60,
        [ServerOnly]
        OlthoiDeathTreasureType    = 61,

        [ServerOnly]
        PCAPRecordedWeenieHeader         = 8001,
        [ServerOnly]
        PCAPRecordedWeenieHeader2        = 8002,
        [ServerOnly]
        PCAPRecordedObjectDesc           = 8003,
        [ServerOnly]
        PCAPRecordedPhysicsDesc          = 8005,
        [ServerOnly]
        PCAPRecordedParentLocation       = 8009,
        [ServerOnly]
        PCAPRecordedDefaultScript        = 8019,
        [ServerOnly]
        PCAPRecordedTimestamp0           = 8020,
        [ServerOnly]
        PCAPRecordedTimestamp1           = 8021,
        [ServerOnly]
        PCAPRecordedTimestamp2           = 8022,
        [ServerOnly]
        PCAPRecordedTimestamp3           = 8023,
        [ServerOnly]
        PCAPRecordedTimestamp4           = 8024,
        [ServerOnly]
        PCAPRecordedTimestamp5           = 8025,
        [ServerOnly]
        PCAPRecordedTimestamp6           = 8026,
        [ServerOnly]
        PCAPRecordedTimestamp7           = 8027,
        [ServerOnly]
        PCAPRecordedTimestamp8           = 8028,
        [ServerOnly]
        PCAPRecordedTimestamp9           = 8029,
        [ServerOnly]
        PCAPRecordedMaxVelocityEstimated = 8030,
        [ServerOnly]
        PCAPPhysicsDIDDataTemplatedFrom  = 8044

        //[ServerOnly]
        //HairTexture                = 9001,
        //[ServerOnly]
        //DefaultHairTexture         = 9002,
    }

    public static class PropertyDataIdExtensions
    {
        public static string GetDescription(this PropertyDataId prop)
        {
            var description = prop.GetAttributeOfType<DescriptionAttribute>();
            return description?.Description ?? prop.ToString();
        }

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
                case PropertyDataId.LastPortal:
                case PropertyDataId.LinkedPortalOne:
                case PropertyDataId.LinkedPortalTwo:
                case PropertyDataId.OlthoiDeathTreasureType:
                case PropertyDataId.OriginalPortal:
                case PropertyDataId.PhysicsScript:
                case PropertyDataId.ProcSpell:
                case PropertyDataId.RedSurgeSpell:
                case PropertyDataId.RestrictionEffect:
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
