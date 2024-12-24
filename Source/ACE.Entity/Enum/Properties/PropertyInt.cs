using System;
using System.ComponentModel;
using System.Globalization;

namespace ACE.Entity.Enum.Properties
{
    // No properties are sent to the client unless they featured an attribute.
    // SendOnLogin gets sent to players in the PlayerDescription event
    // AssessmentProperty gets sent in successful appraisal
    public enum PropertyInt : ushort
    {
        Undef                                    = 0,
        ItemType                                 = 1,
        [AssessmentProperty]
        CreatureType                             = 2,
        PaletteTemplate                          = 3,
        ClothingPriority                         = 4,
        EncumbranceVal                           = 5, // ENCUMB_VAL_INT,
        ItemsCapacity                            = 6,
        [SendOnLogin]
        ContainersCapacity                       = 7,
        Mass                                     = 8,
        ValidLocations                           = 9, // LOCATIONS_INT,
        CurrentWieldedLocation                   = 10,
        MaxStackSize                             = 11,
        StackSize                                = 12,
        StackUnitEncumbrance                     = 13,
        StackUnitMass                            = 14,
        StackUnitValue                           = 15,
        ItemUseable                              = 16,
        [AssessmentProperty]
        RareId                                   = 17,
        UiEffects                                = 18,
        [AssessmentProperty]
        Value                                    = 19,
        [SendOnLogin][Ephemeral]
        CoinValue                                = 20,
        TotalExperience                          = 21,
        AvailableCharacter                       = 22,
        TotalSkillCredits                        = 23,
        [SendOnLogin]
        AvailableSkillCredits                    = 24,
        [SendOnLogin][AssessmentProperty]
        Level                                    = 25,
        [AssessmentProperty]
        AccountRequirements                      = 26,
        ArmorType                                = 27,
        [AssessmentProperty]
        ArmorLevel                               = 28,
        AllegianceCpPool                         = 29,
        [SendOnLogin][AssessmentProperty]
        AllegianceRank                           = 30,
        ChannelsAllowed                          = 31,
        ChannelsActive                           = 32,
        [AssessmentProperty]
        Bonded                                   = 33,
        MonarchsRank                             = 34,
        [AssessmentProperty]
        AllegianceFollowers                      = 35,
        [AssessmentProperty]
        ResistMagic                              = 36,
        ResistItemAppraisal                      = 37,
        [AssessmentProperty]
        ResistLockpick                           = 38,
        DeprecatedResistRepair                   = 39,
        [SendOnLogin]
        CombatMode                               = 40,
        CurrentAttackHeight                      = 41,
        CombatCollisions                         = 42,
        [SendOnLogin][AssessmentProperty]
        NumDeaths                                = 43,
        Damage                                   = 44,
        [AssessmentProperty]
        DamageType                               = 45,
        DefaultCombatStyle                       = 46,
        [SendOnLogin][AssessmentProperty]
        AttackType                               = 47,
        WeaponSkill                              = 48,
        WeaponTime                               = 49,
        AmmoType                                 = 50,
        CombatUse                                = 51,
        ParentLocation                           = 52,
        /// <summary>
        /// TODO: Migrate inventory order away from this and instead use the new InventoryOrder property
        /// TODO: PlacementPosition is used (very sparingly) in cache.bin, so it has (or had) a meaning at one point before we hijacked it
        /// TODO: and used it for our own inventory order
        /// </summary>
        PlacementPosition                        = 53,
        WeaponEncumbrance                        = 54,
        WeaponMass                               = 55,
        ShieldValue                              = 56,
        ShieldEncumbrance                        = 57,
        MissileInventoryLocation                 = 58,
        FullDamageType                           = 59,
        WeaponRange                              = 60,
        AttackersSkill                           = 61,
        DefendersSkill                           = 62,
        AttackersSkillValue                      = 63,
        AttackersClass                           = 64,
        Placement                                = 65,
        CheckpointStatus                         = 66,
        Tolerance                                = 67,
        TargetingTactic                          = 68,
        CombatTactic                             = 69,
        HomesickTargetingTactic                  = 70,
        NumFollowFailures                        = 71,
        FriendType                               = 72,
        FoeType                                  = 73,
        MerchandiseItemTypes                     = 74,
        MerchandiseMinValue                      = 75,
        MerchandiseMaxValue                      = 76,
        NumItemsSold                             = 77,
        NumItemsBought                           = 78,
        MoneyIncome                              = 79,
        MoneyOutflow                             = 80,
        [Ephemeral]
        MaxGeneratedObjects                      = 81,
        [Ephemeral]
        InitGeneratedObjects                     = 82,
        ActivationResponse                       = 83,
        OriginalValue                            = 84,
        NumMoveFailures                          = 85,
        [AssessmentProperty]
        MinLevel                                 = 86,
        [AssessmentProperty]
        MaxLevel                                 = 87,
        LockpickMod                              = 88,
        [AssessmentProperty]
        BoosterEnum                              = 89,
        [AssessmentProperty]
        BoostValue                               = 90,
        [AssessmentProperty]
        MaxStructure                             = 91,
        [AssessmentProperty]
        Structure                                = 92,
        PhysicsState                             = 93,
        TargetType                               = 94,
        RadarBlipColor                           = 95,
        EncumbranceCapacity                      = 96,
        LoginTimestamp                           = 97,
        [SendOnLogin][AssessmentProperty]
        CreationTimestamp                        = 98,
        PkLevelModifier                          = 99,
        GeneratorType                            = 100,
        AiAllowedCombatStyle                     = 101,
        LogoffTimestamp                          = 102,
        GeneratorDestructionType                 = 103,
        ActivationCreateClass                    = 104,
        [AssessmentProperty]
        ItemWorkmanship                          = 105,
        [AssessmentProperty]
        ItemSpellcraft                           = 106,
        [AssessmentProperty]
        ItemCurMana                              = 107,
        [AssessmentProperty]
        ItemMaxMana                              = 108,
        [AssessmentProperty]
        ItemDifficulty                           = 109,
        [AssessmentProperty]
        ItemAllegianceRankLimit                  = 110,
        [AssessmentProperty]
        PortalBitmask                            = 111,
        AdvocateLevel                            = 112,
        [SendOnLogin][AssessmentProperty]
        Gender                                   = 113,
        [AssessmentProperty]
        Attuned                                  = 114,
        [AssessmentProperty]
        ItemSkillLevelLimit                      = 115,
        GateLogic                                = 116,
        [AssessmentProperty]
        ItemManaCost                             = 117,
        Logoff                                   = 118,
        Active                                   = 119,
        AttackHeight                             = 120,
        NumAttackFailures                        = 121,
        AiCpThreshold                            = 122,
        AiAdvancementStrategy                    = 123,
        Version                                  = 124,
        [SendOnLogin][AssessmentProperty]
        Age                                      = 125,
        VendorHappyMean                          = 126,
        VendorHappyVariance                      = 127,
        CloakStatus                              = 128,
        [SendOnLogin]
        VitaeCpPool                              = 129,
        NumServicesSold                          = 130,
        [AssessmentProperty]
        MaterialType                             = 131,
        [SendOnLogin]
        NumAllegianceBreaks                      = 132,
        [Ephemeral]
        ShowableOnRadar                          = 133,
        [SendOnLogin][AssessmentProperty]
        PlayerKillerStatus                       = 134,
        VendorHappyMaxItems                      = 135,
        ScorePageNum                             = 136,
        ScoreConfigNum                           = 137,
        ScoreNumScores                           = 138,
        [SendOnLogin]
        DeathLevel                               = 139,
        AiOptions                                = 140,
        OpenToEveryone                           = 141,
        GeneratorTimeType                        = 142,
        GeneratorStartTime                       = 143,
        GeneratorEndTime                         = 144,
        GeneratorEndDestructionType              = 145,
        XpOverride                               = 146,
        NumCrashAndTurns                         = 147,
        ComponentWarningThreshold                = 148,
        HouseStatus                              = 149,
        HookPlacement                            = 150,
        HookType                                 = 151,
        HookItemType                             = 152,
        AiPpThreshold                            = 153,
        GeneratorVersion                         = 154,
        HouseType                                = 155,
        PickupEmoteOffset                        = 156,
        WeenieIteration                          = 157,
        [AssessmentProperty]
        WieldRequirements                        = 158,
        [AssessmentProperty]
        WieldSkillType                           = 159,
        [AssessmentProperty]
        WieldDifficulty                          = 160,
        HouseMaxHooksUsable                      = 161,
        [Ephemeral]
        HouseCurrentHooksUsable                  = 162,
        AllegianceMinLevel                       = 163,
        AllegianceMaxLevel                       = 164,
        HouseRelinkHookCount                     = 165,
        [AssessmentProperty]
        SlayerCreatureType                       = 166,
        ConfirmationInProgress                   = 167,
        ConfirmationTypeInProgress               = 168,
        TsysMutationData                         = 169,
        [AssessmentProperty]
        NumItemsInMaterial                       = 170,
        [AssessmentProperty]
        NumTimesTinkered                         = 171,
        [AssessmentProperty]
        AppraisalLongDescDecoration              = 172,
        [AssessmentProperty]
        AppraisalLockpickSuccessPercent          = 173,
        [AssessmentProperty][Ephemeral]
        AppraisalPages                           = 174,
        [AssessmentProperty][Ephemeral]
        AppraisalMaxPages                        = 175,
        [AssessmentProperty]
        AppraisalItemSkill                       = 176,
        [AssessmentProperty]
        GemCount                                 = 177,
        [AssessmentProperty]
        GemType                                  = 178,
        [AssessmentProperty]
        ImbuedEffect                             = 179,
        AttackersRawSkillValue                   = 180,
        [SendOnLogin][AssessmentProperty]
        ChessRank                                = 181,
        ChessTotalGames                          = 182,
        ChessGamesWon                            = 183,
        ChessGamesLost                           = 184,
        TypeOfAlteration                         = 185,
        SkillToBeAltered                         = 186,
        SkillAlterationCount                     = 187,
        [SendOnLogin][AssessmentProperty]
        HeritageGroup                            = 188,
        TransferFromAttribute                    = 189,
        TransferToAttribute                      = 190,
        AttributeTransferCount                   = 191,
        [SendOnLogin][AssessmentProperty]
        FakeFishingSkill                         = 192,
        [AssessmentProperty]
        NumKeys                                  = 193,
        DeathTimestamp                           = 194,
        PkTimestamp                              = 195,
        VictimTimestamp                          = 196,
        HookGroup                                = 197,
        AllegianceSwearTimestamp                 = 198,
        [SendOnLogin]
        HousePurchaseTimestamp                   = 199,
        RedirectableEquippedArmorCount           = 200,
        MeleeDefenseImbuedEffectTypeCache        = 201,
        MissileDefenseImbuedEffectTypeCache      = 202,
        MagicDefenseImbuedEffectTypeCache        = 203,
        [AssessmentProperty]
        ElementalDamageBonus                     = 204,
        ImbueAttempts                            = 205,
        ImbueSuccesses                           = 206,
        CreatureKills                            = 207,
        PlayerKillsPk                            = 208,
        PlayerKillsPkl                           = 209,
        RaresTierOne                             = 210,
        RaresTierTwo                             = 211,
        RaresTierThree                           = 212,
        RaresTierFour                            = 213,
        RaresTierFive                            = 214,
        AugmentationStat                         = 215,
        AugmentationFamilyStat                   = 216,
        AugmentationInnateFamily                 = 217,
        [SendOnLogin]
        AugmentationInnateStrength               = 218,
        [SendOnLogin]
        AugmentationInnateEndurance              = 219,
        [SendOnLogin]
        AugmentationInnateCoordination           = 220,
        [SendOnLogin]
        AugmentationInnateQuickness              = 221,
        [SendOnLogin]
        AugmentationInnateFocus                  = 222,
        [SendOnLogin]
        AugmentationInnateSelf                   = 223,
        [SendOnLogin]
        AugmentationSpecializeSalvaging          = 224,
        [SendOnLogin]
        AugmentationSpecializeItemTinkering      = 225,
        [SendOnLogin]
        AugmentationSpecializeArmorTinkering     = 226,
        [SendOnLogin]
        AugmentationSpecializeMagicItemTinkering = 227,
        [SendOnLogin]
        AugmentationSpecializeWeaponTinkering    = 228,
        [SendOnLogin]
        AugmentationExtraPackSlot                = 229,
        [SendOnLogin]
        AugmentationIncreasedCarryingCapacity    = 230,
        [SendOnLogin]
        AugmentationLessDeathItemLoss            = 231,
        [SendOnLogin]
        AugmentationSpellsRemainPastDeath        = 232,
        [SendOnLogin]
        AugmentationCriticalDefense              = 233,
        [SendOnLogin]
        AugmentationBonusXp                      = 234,
        [SendOnLogin]
        AugmentationBonusSalvage                 = 235,
        [SendOnLogin]
        AugmentationBonusImbueChance             = 236,
        [SendOnLogin]
        AugmentationFasterRegen                  = 237,
        [SendOnLogin]
        AugmentationIncreasedSpellDuration       = 238,
        AugmentationResistanceFamily             = 239,
        [SendOnLogin]
        AugmentationResistanceSlash              = 240,
        [SendOnLogin]
        AugmentationResistancePierce             = 241,
        [SendOnLogin]
        AugmentationResistanceBlunt              = 242,
        [SendOnLogin]
        AugmentationResistanceAcid               = 243,
        [SendOnLogin]
        AugmentationResistanceFire               = 244,
        [SendOnLogin]
        AugmentationResistanceFrost              = 245,
        [SendOnLogin]
        AugmentationResistanceLightning          = 246,
        RaresTierOneLogin                        = 247,
        RaresTierTwoLogin                        = 248,
        RaresTierThreeLogin                      = 249,
        RaresTierFourLogin                       = 250,
        RaresTierFiveLogin                       = 251,
        RaresLoginTimestamp                      = 252,
        RaresTierSix                             = 253,
        RaresTierSeven                           = 254,
        RaresTierSixLogin                        = 255,
        RaresTierSevenLogin                      = 256,
        [AssessmentProperty]
        ItemAttributeLimit                       = 257,
        [AssessmentProperty]
        ItemAttributeLevelLimit                  = 258,
        [AssessmentProperty]
        ItemAttribute2ndLimit                    = 259,
        [AssessmentProperty]
        ItemAttribute2ndLevelLimit               = 260,
        [AssessmentProperty]
        CharacterTitleId                         = 261,
        [AssessmentProperty]
        NumCharacterTitles                       = 262,
        [AssessmentProperty]
        ResistanceModifierType                   = 263,
        FreeTinkersBitfield                      = 264,
        [AssessmentProperty]
        EquipmentSetId                           = 265,
        PetClass                                 = 266,
        [AssessmentProperty]
        Lifespan                                 = 267,
        [AssessmentProperty][Ephemeral]
        RemainingLifespan                        = 268,
        UseCreateQuantity                        = 269,
        [AssessmentProperty]
        WieldRequirements2                       = 270,
        [AssessmentProperty]
        WieldSkillType2                          = 271,
        [AssessmentProperty]
        WieldDifficulty2                         = 272,
        [AssessmentProperty]
        WieldRequirements3                       = 273,
        [AssessmentProperty]
        WieldSkillType3                          = 274,
        [AssessmentProperty]
        WieldDifficulty3                         = 275,
        [AssessmentProperty]
        WieldRequirements4                       = 276,
        [AssessmentProperty]
        WieldSkillType4                          = 277,
        [AssessmentProperty]
        WieldDifficulty4                         = 278,
        [AssessmentProperty]
        Unique                                   = 279,
        [AssessmentProperty]
        SharedCooldown                           = 280,
        [SendOnLogin][AssessmentProperty]
        Faction1Bits                             = 281,
        Faction2Bits                             = 282,
        Faction3Bits                             = 283,
        Hatred1Bits                              = 284,
        Hatred2Bits                              = 285,
        Hatred3Bits                              = 286,
        [SendOnLogin][AssessmentProperty]
        SocietyRankCelhan                        = 287,
        [SendOnLogin][AssessmentProperty]
        SocietyRankEldweb                        = 288,
        [SendOnLogin][AssessmentProperty]
        SocietyRankRadblo                        = 289,
        HearLocalSignals                         = 290,
        HearLocalSignalsRadius                   = 291,
        [AssessmentProperty]
        Cleaving                                 = 292,
        AugmentationSpecializeGearcraft          = 293,
        [SendOnLogin]
        AugmentationInfusedCreatureMagic         = 294,
        [SendOnLogin]
        AugmentationInfusedItemMagic             = 295,
        [SendOnLogin]
        AugmentationInfusedLifeMagic             = 296,
        [SendOnLogin]
        AugmentationInfusedWarMagic              = 297,
        [SendOnLogin]
        AugmentationCriticalExpertise            = 298,
        [SendOnLogin]
        AugmentationCriticalPower                = 299,
        [SendOnLogin]
        AugmentationSkilledMelee                 = 300,
        [SendOnLogin]
        AugmentationSkilledMissile               = 301,
        [SendOnLogin]
        AugmentationSkilledMagic                 = 302,
        [AssessmentProperty]
        ImbuedEffect2                            = 303,
        [AssessmentProperty]
        ImbuedEffect3                            = 304,
        [AssessmentProperty]
        ImbuedEffect4                            = 305,
        [AssessmentProperty]
        ImbuedEffect5                            = 306,
        [SendOnLogin][AssessmentProperty]
        DamageRating                             = 307,
        [SendOnLogin][AssessmentProperty]
        DamageResistRating                       = 308,
        [SendOnLogin]
        AugmentationDamageBonus                  = 309,
        [SendOnLogin]
        AugmentationDamageReduction              = 310,
        ImbueStackingBits                        = 311,
        [SendOnLogin]
        HealOverTime                             = 312,
        [SendOnLogin][AssessmentProperty]
        CritRating                               = 313,
        [SendOnLogin][AssessmentProperty]
        CritDamageRating                         = 314,
        [SendOnLogin][AssessmentProperty]
        CritResistRating                         = 315,
        [SendOnLogin][AssessmentProperty]
        CritDamageResistRating                   = 316,
        [SendOnLogin]
        HealingResistRating                      = 317,
        [SendOnLogin]
        DamageOverTime                           = 318,
        [AssessmentProperty]
        ItemMaxLevel                             = 319,
        [AssessmentProperty]
        ItemXpStyle                              = 320,
        EquipmentSetExtra                        = 321,
        [SendOnLogin]
        AetheriaBitfield                         = 322,
        [SendOnLogin][AssessmentProperty]
        HealingBoostRating                       = 323,
        [AssessmentProperty]
        HeritageSpecificArmor                    = 324,
        AlternateRacialSkills                    = 325,
        [SendOnLogin]
        AugmentationJackOfAllTrades              = 326,
        AugmentationResistanceNether             = 327,
        [SendOnLogin]
        AugmentationInfusedVoidMagic             = 328,
        [SendOnLogin]
        WeaknessRating                           = 329,
        [SendOnLogin]
        NetherOverTime                           = 330,
        [SendOnLogin]
        NetherResistRating                       = 331,
        LuminanceAward                           = 332,
        [SendOnLogin]
        LumAugDamageRating                       = 333,
        [SendOnLogin]
        LumAugDamageReductionRating              = 334,
        [SendOnLogin]
        LumAugCritDamageRating                   = 335,
        [SendOnLogin]
        LumAugCritReductionRating                = 336,
        [SendOnLogin]
        LumAugSurgeEffectRating                  = 337,
        [SendOnLogin]
        LumAugSurgeChanceRating                  = 338,
        [SendOnLogin]
        LumAugItemManaUsage                      = 339,
        [SendOnLogin]
        LumAugItemManaGain                       = 340,
        [SendOnLogin]
        LumAugVitality                           = 341,
        [SendOnLogin]
        LumAugHealingRating                      = 342,
        [SendOnLogin]
        LumAugSkilledCraft                       = 343,
        [SendOnLogin]
        LumAugSkilledSpec                        = 344,
        LumAugNoDestroyCraft                     = 345,
        RestrictInteraction                      = 346,
        [SendOnLogin]
        OlthoiLootTimestamp                      = 347,
        OlthoiLootStep                           = 348,
        UseCreatesContractId                     = 349,
        [SendOnLogin][AssessmentProperty]
        DotResistRating                          = 350,
        [SendOnLogin][AssessmentProperty]
        LifeResistRating                         = 351,
        [AssessmentProperty]
        CloakWeaveProc                           = 352,
        [AssessmentProperty]
        WeaponType                               = 353,
        [SendOnLogin]
        MeleeMastery                             = 354,
        [SendOnLogin]
        RangedMastery                            = 355,
        SneakAttackRating                        = 356,
        RecklessnessRating                       = 357,
        DeceptionRating                          = 358,
        CombatPetRange                           = 359,
        [SendOnLogin]
        WeaponAuraDamage                         = 360,
        [SendOnLogin]
        WeaponAuraSpeed                          = 361,
        [SendOnLogin]
        SummoningMastery                         = 362,
        HeartbeatLifespan                        = 363,
        UseLevelRequirement                      = 364,
        [SendOnLogin]
        LumAugAllSkills                          = 365,
        [AssessmentProperty]
        UseRequiresSkill                         = 366,
        [AssessmentProperty]
        UseRequiresSkillLevel                    = 367,
        [AssessmentProperty]
        UseRequiresSkillSpec                     = 368,
        [AssessmentProperty]
        UseRequiresLevel                         = 369,
        [SendOnLogin][AssessmentProperty]
        GearDamage                               = 370,
        [SendOnLogin][AssessmentProperty]
        GearDamageResist                         = 371,
        [SendOnLogin][AssessmentProperty]
        GearCrit                                 = 372,
        [SendOnLogin][AssessmentProperty]
        GearCritResist                           = 373,
        [SendOnLogin][AssessmentProperty]
        GearCritDamage                           = 374,
        [SendOnLogin][AssessmentProperty]
        GearCritDamageResist                     = 375,
        [SendOnLogin][AssessmentProperty]
        GearHealingBoost                         = 376,
        [SendOnLogin][AssessmentProperty]
        GearNetherResist                         = 377,
        [SendOnLogin][AssessmentProperty]
        GearLifeResist                           = 378,
        [SendOnLogin][AssessmentProperty]
        GearMaxHealth                            = 379,
        Unknown380                               = 380,
        [SendOnLogin][AssessmentProperty]
        PKDamageRating                           = 381,
        [SendOnLogin][AssessmentProperty]
        PKDamageResistRating                     = 382,
        [SendOnLogin][AssessmentProperty]
        GearPKDamageRating                       = 383,
        [SendOnLogin][AssessmentProperty]
        GearPKDamageResistRating                 = 384,
        Unknown385                               = 385,
        /// <summary>
        /// Overpower chance % for endgame creatures.
        /// </summary>
        [SendOnLogin][AssessmentProperty]
        Overpower                                = 386,
        [SendOnLogin][AssessmentProperty]
        OverpowerResist                          = 387,
        // Client does not display accurately
        [SendOnLogin][AssessmentProperty]
        GearOverpower                            = 388,
        // Client does not display accurately
        [SendOnLogin][AssessmentProperty]
        GearOverpowerResist                      = 389,
        // Number of times a character has enlightened
        [SendOnLogin][AssessmentProperty]
        Enlightenment                            = 390,

        /* Custom Properties */
        PCAPRecordedAutonomousMovement           = 8007,
        PCAPRecordedMaxVelocityEstimated         = 8030,
        PCAPRecordedPlacement                    = 8041,
        PCAPRecordedAppraisalPages               = 8042,
        PCAPRecordedAppraisalMaxPages            = 8043,

        // TotalLogins                           = 9001,
        // DeletionTimestamp                     = 9002,
        // CharacterOptions1                     = 9003,
        // CharacterOptions2                     = 9004,
        // LootTier                              = 9005,
        // GeneratorProbability                  = 9006,
        // WeenieType                            = 9007
        CurrentLoyaltyAtLastLogoff               = 9008,
        CurrentLeadershipAtLastLogoff            = 9009,
        AllegianceOfficerRank                    = 9010,
        HouseRentTimestamp                       = 9011,
        Hairstyle                                = 9012,
        [Ephemeral]
        VisualClothingPriority                   = 9013,
        SquelchGlobal                            = 9014,
        InventoryOrder                           = 9015,
    }

    public static class PropertyIntExtensions
    {
        public static string GetValueEnumName(this PropertyInt property, int value)
        {
            switch (property)
            {
                case PropertyInt.ActivationResponse:
                    return System.Enum.GetName(typeof(ActivationResponse), value);
                case PropertyInt.AetheriaBitfield:
                    return System.Enum.GetName(typeof(AetheriaBitfield), value);
                case PropertyInt.AttackHeight:
                    return System.Enum.GetName(typeof(AttackHeight), value);
                case PropertyInt.AttackType:
                    return System.Enum.GetName(typeof(AttackType), value);
                case PropertyInt.Attuned:
                    return System.Enum.GetName(typeof(AttunedStatus), value);
                case PropertyInt.AmmoType:
                    return System.Enum.GetName(typeof(AmmoType), value);
                case PropertyInt.Bonded:
                    return System.Enum.GetName(typeof(BondedStatus), value);
                case PropertyInt.ChannelsActive:
                case PropertyInt.ChannelsAllowed:
                    return System.Enum.GetName(typeof(Channel), value);
                case PropertyInt.CombatMode:
                    return System.Enum.GetName(typeof(CombatMode), value);
                case PropertyInt.DefaultCombatStyle:
                case PropertyInt.AiAllowedCombatStyle:
                    return System.Enum.GetName(typeof(CombatStyle), value);
                case PropertyInt.CombatUse:
                    return System.Enum.GetName(typeof(CombatUse), value);
                case PropertyInt.ClothingPriority:
                    return System.Enum.GetName(typeof(CoverageMask), value);
                case PropertyInt.CreatureType:
                case PropertyInt.SlayerCreatureType:
                case PropertyInt.FoeType:
                case PropertyInt.FriendType:
                    return System.Enum.GetName(typeof(CreatureType), value);
                case PropertyInt.DamageType:
                case PropertyInt.ResistanceModifierType:
                    return System.Enum.GetName(typeof(DamageType), value);
                case PropertyInt.CurrentWieldedLocation:
                case PropertyInt.ValidLocations:
                    return System.Enum.GetName(typeof(EquipMask), value);
                case PropertyInt.EquipmentSetId:
                    return System.Enum.GetName(typeof(EquipmentSet), value);
                case PropertyInt.Gender:
                    return System.Enum.GetName(typeof(Gender), value);
                case PropertyInt.GeneratorDestructionType:
                case PropertyInt.GeneratorEndDestructionType:
                    return System.Enum.GetName(typeof(GeneratorDestruct), value);
                case PropertyInt.GeneratorTimeType:
                    return System.Enum.GetName(typeof(GeneratorTimeType), value);
                case PropertyInt.GeneratorType:
                    return System.Enum.GetName(typeof(GeneratorType), value);
                case PropertyInt.HeritageGroup:
                case PropertyInt.HeritageSpecificArmor:
                    return System.Enum.GetName(typeof(HeritageGroup), value);
                case PropertyInt.HookType:
                    return System.Enum.GetName(typeof(HookType), value);
                case PropertyInt.HouseType:
                    return System.Enum.GetName(typeof(HouseType), value);
                case PropertyInt.ImbuedEffect:
                case PropertyInt.ImbuedEffect2:
                case PropertyInt.ImbuedEffect3:
                case PropertyInt.ImbuedEffect4:
                case PropertyInt.ImbuedEffect5:
                    return System.Enum.GetName(typeof(ImbuedEffectType), value);
                case PropertyInt.HookItemType:
                case PropertyInt.ItemType:
                case PropertyInt.MerchandiseItemTypes:
                case PropertyInt.TargetType:
                    return System.Enum.GetName(typeof(ItemType), value);
                case PropertyInt.ItemXpStyle:
                    return System.Enum.GetName(typeof(ItemXpStyle), value);
                case PropertyInt.MaterialType:
                    return System.Enum.GetName(typeof(MaterialType), value);
                case PropertyInt.PaletteTemplate:
                    return System.Enum.GetName(typeof(PaletteTemplate), value);
                case PropertyInt.PhysicsState:
                    return System.Enum.GetName(typeof(PhysicsState), value);
                case PropertyInt.HookPlacement:
                case PropertyInt.Placement:
                case PropertyInt.PCAPRecordedPlacement:
                    return System.Enum.GetName(typeof(Placement), value);
                case PropertyInt.PortalBitmask:
                    return System.Enum.GetName(typeof(PortalBitmask), value);
                case PropertyInt.PlayerKillerStatus:
                    return System.Enum.GetName(typeof(PlayerKillerStatus), value);
                case PropertyInt.BoosterEnum:
                    return System.Enum.GetName(typeof(PropertyAttribute2nd), value);
                case PropertyInt.ShowableOnRadar:
                    return System.Enum.GetName(typeof(RadarBehavior), value);
                case PropertyInt.RadarBlipColor:
                    return System.Enum.GetName(typeof(RadarColor), value);
                case PropertyInt.WeaponSkill:
                case PropertyInt.WieldSkillType:
                case PropertyInt.WieldSkillType2:
                case PropertyInt.WieldSkillType3:
                case PropertyInt.WieldSkillType4:
                case PropertyInt.AppraisalItemSkill:
                    return System.Enum.GetName(typeof(Skill), value);
                case PropertyInt.AccountRequirements:
                    return System.Enum.GetName(typeof(SubscriptionStatus), value);
                case PropertyInt.SummoningMastery:
                    return System.Enum.GetName(typeof(SummoningMastery), value);
                case PropertyInt.UiEffects:
                    return System.Enum.GetName(typeof(UiEffects), value);
                case PropertyInt.ItemUseable:
                    return System.Enum.GetName(typeof(Usable), value);
                case PropertyInt.WeaponType:
                    return System.Enum.GetName(typeof(WeaponType), value);
                case PropertyInt.WieldRequirements:
                case PropertyInt.WieldRequirements2:
                case PropertyInt.WieldRequirements3:
                case PropertyInt.WieldRequirements4:
                    return System.Enum.GetName(typeof(WieldRequirement), value);

                case PropertyInt.GeneratorStartTime:
                case PropertyInt.GeneratorEndTime:
                    return DateTimeOffset.FromUnixTimeSeconds(value).DateTime.ToString(CultureInfo.InvariantCulture);

                case PropertyInt.ArmorType:
                    return System.Enum.GetName(typeof(ArmorType), value);
                case PropertyInt.ParentLocation:
                    return System.Enum.GetName(typeof(ParentLocation), value);
                case PropertyInt.PlacementPosition:
                    return System.Enum.GetName(typeof(Placement), value);
                case PropertyInt.HouseStatus:
                    return System.Enum.GetName(typeof(HouseStatus), value);

                case PropertyInt.UseCreatesContractId:
                    return System.Enum.GetName(typeof(ContractId), value);

                case PropertyInt.Faction1Bits:
                case PropertyInt.Faction2Bits:
                case PropertyInt.Faction3Bits:
                case PropertyInt.Hatred1Bits:
                case PropertyInt.Hatred2Bits:
                case PropertyInt.Hatred3Bits:
                    return System.Enum.GetName(typeof(FactionBits), value);

                case PropertyInt.UseRequiresSkill:
                case PropertyInt.UseRequiresSkillSpec:
                case PropertyInt.SkillToBeAltered:
                    return System.Enum.GetName(typeof(Skill), value);

                case PropertyInt.HookGroup:
                    return System.Enum.GetName(typeof(HookGroupType), value);

                //case PropertyInt.TypeOfAlteration:
                //    return System.Enum.GetName(typeof(SkillAlterationType), value);
            }

            return null;
        }
    }
}
