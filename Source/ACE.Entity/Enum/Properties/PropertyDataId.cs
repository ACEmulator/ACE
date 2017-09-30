namespace ACE.Entity.Enum.Properties
{
    public enum PropertyDataId : uint
    {
        // properties marked as ServerOnly are properties we never saw in PCAPs, from here:
        // http://ac.yotesfan.com/ace_object/not_used_enums.php
        // source: @OptimShi

        // description attributes are used by the weenie editor for a cleaner display name

        [ServerOnly]
        Undef                      = 0,
        Setup                      = 1,
        MotionTable                = 2,
        SoundTable                 = 3,
        [ServerOnly]
        CombatTable                = 4,
        [ServerOnly]
        QualityFilter              = 5,
        PaletteBase                = 6,
        ClothingBase               = 7,
        Icon                       = 8,
        EyesTexture                = 9,
        NoseTexture                = 10,
        MouthTexture               = 11,
        [ServerOnly]
        DefaultEyesTexture         = 12,
        [ServerOnly]
        DefaultNoseTexture         = 13,
        [ServerOnly]
        DefaultMouthTexture        = 14,
        HairPalette                = 15,
        EyesPalette                = 16,
        SkinPalette                = 17,
        [ServerOnly]
        HeadObject                 = 18,
        [ServerOnly]
        ActivationAnimation        = 19,
        [ServerOnly]
        InitMotion                 = 20,
        [ServerOnly]
        ActivationSound            = 21,
        PhysicsEffectTable         = 22,
        [ServerOnly]
        UseSound                   = 23,
        [ServerOnly]
        UseTargetAnimation         = 24,
        [ServerOnly]
        UseTargetSuccessAnimation  = 25,
        [ServerOnly]
        UseTargetFailureAnimation  = 26,
        UseUserAnimation           = 27,
        Spell                      = 28,
        [ServerOnly]
        SpellComponent             = 29,
        PhysicsScript              = 30,
        [ServerOnly]
        LinkedPortalOne            = 31,
        [ServerOnly]
        WieldedTreasureType        = 32,
        [ServerOnly]
        UnknownGuessedname         = 33,
        [ServerOnly]
        UnknownGuessedname2        = 34,
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
        IconOverlay                = 50,
        [ServerOnly]
        IconOverlaySecondary       = 51,
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
        HairTexture                = 9001,
        [ServerOnly]
        DefaultHairTexture         = 9002,
    }
}
