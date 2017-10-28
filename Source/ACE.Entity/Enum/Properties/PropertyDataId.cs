﻿using System.ComponentModel;

namespace ACE.Entity.Enum.Properties
{
    public enum PropertyDataId : ushort
    {
        // properties marked as ServerOnly are properties we never saw in PCAPs, from here:
        // http://ac.yotesfan.com/ace_object/not_used_enums.php
        // source: @OptimShi
        // description attributes are used by the weenie editor for a cleaner display name

        Undef                      = 0,
        Setup                      = 1,
        MotionTable                = 2,
        SoundTable                 = 3,
        CombatTable                = 4,
        QualityFilter              = 5,
        PaletteBase                = 6,
        ClothingBase               = 7,
        Icon                       = 8,
        EyesTexture                = 9,
        NoseTexture                = 10,
        MouthTexture               = 11,
        DefaultEyesTexture         = 12,
        DefaultNoseTexture         = 13,
        DefaultMouthTexture        = 14,
        HairPalette                = 15,
        EyesPalette                = 16,
        SkinPalette                = 17,
        HeadObject                 = 18,
        ActivationAnimation        = 19,
        InitMotion                 = 20,
        ActivationSound            = 21,
        PhysicsEffectTable         = 22,
        UseSound                   = 23,
        UseTargetAnimation         = 24,
        UseTargetSuccessAnimation  = 25,
        UseTargetFailureAnimation  = 26,
        UseUserAnimation           = 27,
        Spell                      = 28,
        SpellComponent             = 29,
        PhysicsScript              = 30,
        LinkedPortalOne            = 31,
        WieldedTreasureType        = 32,
        UnknownGuessedname         = 33,
        UnknownGuessedname2        = 34,
        DeathTreasureType          = 35,
        MutateFilter               = 36,
        ItemSkillLimit             = 37,
        UseCreateItem              = 38,
        DeathSpell                 = 39,
        VendorsClassId             = 40,
        ItemSpecializedOnly        = 41,
        HouseId                    = 42,
        AccountHouseId             = 43,
        RestrictionEffect          = 44,
        CreationMutationFilter     = 45,
        TsysMutationFilter         = 46,
        LastPortal                 = 47,
        LinkedPortalTwo            = 48,
        OriginalPortal             = 49,
        IconOverlay                = 50,
        IconOverlaySecondary       = 51,
        IconUnderlay               = 52,
        AugmentationMutationFilter = 53,
        AugmentationEffect         = 54,
        ProcSpell                  = 55,
        AugmentationCreateItem     = 56,
        AlternateCurrency          = 57,
        BlueSurgeSpell             = 58,
        YellowSurgeSpell           = 59,
        RedSurgeSpell              = 60,
        OlthoiDeathTreasureType    = 61,
        [ServerOnly]
        HairTexture                = 9001,
        [ServerOnly]
        DefaultHairTexture         = 9002,
    }

    public static class PropertyDataIdExtensions
    {
        public static string GetDescription(this PropertyDataId prop)
        {
            var description = EnumHelper.GetAttributeOfType<DescriptionAttribute>(prop);
            return description?.Description ?? prop.ToString();
        }
    }
}
