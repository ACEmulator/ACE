using System.Collections.Generic;
using System.Linq;

namespace ACE.Entity.Enum.Properties
{
    public static class ClientEnums
    {
        public static List<ushort> ClientPropertyInt = typeof(PropertyInt).GetFields().Select(x =>
                new { att = x.GetCustomAttributes(false).OfType<ServerOnlyAttribute>().FirstOrDefault(), member = x })
            .Where(x => x.att == null && x.member.Name != "value__").Select(x => (ushort)x.member.GetValue(null)).ToList();
    }

    public enum PropertyInt : ushort
    {
        // properties marked as ServerOnly are properties we never saw in PCAPs, from here:
        // http://ac.yotesfan.com/ace_object/not_used_enums.php
        // source: @OptimShi

        // description attributes are used by the weenie editor for a cleaner display name

        [ServerOnly]
        Undef                                    = 0,
        ItemType                                 = 1,
        CreatureType                             = 2,
        [ServerOnly]
        PaletteTemplate                          = 3,
        ClothingPriority                         = 4,
        EncumbranceVal                           = 5, // ENCUMB_VAL_INT,
        ItemsCapacity                            = 6,
        ContainersCapacity                       = 7,
        [ServerOnly]
        Mass                                     = 8,
        ValidLocations                           = 9, // LOCATIONS_INT
        CurrentWieldedLocation                   = 10,
        MaxStackSize                             = 11,
        StackSize                                = 12,
        [ServerOnly]
        StackUnitEncumbrance                     = 13,
        [ServerOnly]
        StackUnitMass                            = 14,
        [ServerOnly]
        StackUnitValue                           = 15,
        ItemUseable                              = 16,
        RareId                                   = 17,
        UiEffects                                = 18,
        Value                                    = 19,
        [ServerOnly]
        CoinValue                                = 20,
        [ServerOnly]
        TotalExperience                          = 21,
        [ServerOnly]
        AvailableCharacter                       = 22,
        [ServerOnly]
        TotalSkillCredits                        = 23,
        [ServerOnly]
        AvailableSkillCredits                    = 24,
        Level                                    = 25,
        AccountRequirements                      = 26,
        [ServerOnly]
        ArmorType                                = 27,
        ArmorLevel                               = 28,
        [ServerOnly]
        AllegianceCpPool                         = 29,
        AllegianceRank                           = 30,
        [ServerOnly]
        ChannelsAllowed                          = 31,
        [ServerOnly]
        ChannelsActive                           = 32,
        Bonded                                   = 33,
        [ServerOnly]
        MonarchsRank                             = 34,
        [ServerOnly]
        AllegianceFollowers                      = 35,
        ResistMagic                              = 36,
        [ServerOnly]
        ResistItemAppraisal                      = 37,
        ResistLockpick                           = 38,
        [ServerOnly]
        DeprecatedResistRepair                   = 39,
        [ServerOnly]
        CombatMode                               = 40,
        [ServerOnly]
        CurrentAttackHeight                      = 41,
        [ServerOnly]
        CombatCollisions                         = 42,
        NumDeaths                                = 43,
        Damage                                   = 44,
        DamageType                               = 45,
        DefaultCombatStyle                       = 46,
        AttackType                               = 47,
        WeaponSkill                              = 48,
        WeaponTime                               = 49,
        AmmoType                                 = 50,
        CombatUse                                = 51,
        ParentLocation                           = 52,
        PlacementPosition                        = 53,
        [ServerOnly]
        WeaponEncumbrance                        = 54,
        [ServerOnly]
        WeaponMass                               = 55,
        [ServerOnly]
        ShieldValue                              = 56,
        [ServerOnly]
        ShieldEncumbrance                        = 57,
        [ServerOnly]
        MissileInventoryLocation                 = 58,
        [ServerOnly]
        FullDamageType                           = 59,
        [ServerOnly]
        WeaponRange                              = 60,
        [ServerOnly]
        AttackersSkill                           = 61,
        [ServerOnly]
        DefendersSkill                           = 62,
        [ServerOnly]
        AttackersSkillValue                      = 63,
        [ServerOnly]
        AttackersClass                           = 64,
        [ServerOnly]
        Placement                                = 65,
        [ServerOnly]
        CheckpointStatus                         = 66,
        [ServerOnly]
        Tolerance                                = 67,
        [ServerOnly]
        TargetingTactic                          = 68,
        [ServerOnly]
        CombatTactic                             = 69,
        [ServerOnly]
        HomesickTargetingTactic                  = 70,
        [ServerOnly]
        NumFollowFailures                        = 71,
        [ServerOnly]
        FriendType                               = 72,
        [ServerOnly]
        FoeType                                  = 73,
        [ServerOnly]
        MerchandiseItemTypes                     = 74,
        [ServerOnly]
        MerchandiseMinValue                      = 75,
        [ServerOnly]
        MerchandiseMaxValue                      = 76,
        [ServerOnly]
        NumItemsSold                             = 77,
        [ServerOnly]
        NumItemsBought                           = 78,
        [ServerOnly]
        MoneyIncome                              = 79,
        [ServerOnly]
        MoneyOutflow                             = 80,
        MaxGeneratedObjects                      = 81,
        [ServerOnly]
        InitGeneratedObjects                     = 82,
        [ServerOnly]
        ActivationResponse                       = 83,
        [ServerOnly]
        OriginalValue                            = 84,
        [ServerOnly]
        NumMoveFailures                          = 85,
        MinLevel                                 = 86,
        MaxLevel                                 = 87,
        [ServerOnly]
        LockpickMod                              = 88,
        BoosterEnum                              = 89,
        BoostValue                               = 90,
        MaxStructure                             = 91,
        Structure                                = 92,
        PhysicsState                             = 93,
        TargetType                               = 94,
        RadarBlipColor                           = 95,
        [ServerOnly]
        EncumbranceCapacity                      = 96,
        [ServerOnly]
        LoginTimestamp                           = 97,
        CreationTimestamp                        = 98,
        [ServerOnly]
        PkLevelModifier                          = 99,
        GeneratorType                            = 100,
        [ServerOnly]
        AiAllowedCombatStyle                     = 101,
        [ServerOnly]
        LogoffTimestamp                          = 102,
        [ServerOnly]
        GeneratorDestructionType                 = 103,
        ActivationCreateClass                    = 104,
        ItemWorkmanship                          = 105,
        ItemSpellcraft                           = 106,
        ItemCurMana                              = 107,
        ItemMaxMana                              = 108,
        ItemDifficulty                           = 109,
        ItemAllegianceRankLimit                  = 110,
        PortalBitmask                            = 111,
        [ServerOnly]
        AdvocateLevel                            = 112,
        Gender                                   = 113,
        Attuned                                  = 114,
        ItemSkillLevelLimit                      = 115,
        [ServerOnly]
        GateLogic                                = 116,
        ItemManaCost                             = 117,
        [ServerOnly]
        Logoff                                   = 118,
        [ServerOnly]
        Active                                   = 119,
        [ServerOnly]
        AttackHeight                             = 120,
        [ServerOnly]
        NumAttackFailures                        = 121,
        [ServerOnly]
        AiCpThreshold                            = 122,
        [ServerOnly]
        AiAdvancementStrategy                    = 123,
        [ServerOnly]
        Version                                  = 124,
        Age                                      = 125,
        [ServerOnly]
        VendorHappyMean                          = 126,
        [ServerOnly]
        VendorHappyVariance                      = 127,
        [ServerOnly]
        CloakStatus                              = 128,
        [ServerOnly]
        VitaeCpPool                              = 129,
        [ServerOnly]
        NumServicesSold                          = 130,
        MaterialType                             = 131,
        [ServerOnly]
        NumAllegianceBreaks                      = 132,
        ShowableOnRadar                          = 133,
        PlayerKillerStatus                       = 134,
        [ServerOnly]
        VendorHappyMaxItems                      = 135,
        [ServerOnly]
        ScorePageNum                             = 136,
        [ServerOnly]
        ScoreConfigNum                           = 137,
        [ServerOnly]
        ScoreNumScores                           = 138,
        [ServerOnly]
        DeathLevel                               = 139,
        [ServerOnly]
        AiOptions                                = 140,
        [ServerOnly]
        OpenToEveryone                           = 141,
        GeneratorTimeType                        = 142,
        [ServerOnly]
        GeneratorStartTime                       = 143,
        [ServerOnly]
        GeneratorEndTime                         = 144,
        [ServerOnly]
        GeneratorEndDestructionType              = 145,
        [ServerOnly]
        XpOverride                               = 146,
        [ServerOnly]
        NumCrashAndTurns                         = 147,
        [ServerOnly]
        ComponentWarningThreshold                = 148,
        [ServerOnly]
        HouseStatus                              = 149,
        [ServerOnly]
        HookPlacement                            = 150,
        HookType                                 = 151,
        HookItemType                             = 152,
        [ServerOnly]
        AiPpThreshold                            = 153,
        [ServerOnly]
        GeneratorVersion                         = 154,
        [ServerOnly]
        HouseType                                = 155,
        [ServerOnly]
        PickupEmoteOffset                        = 156,
        [ServerOnly]
        WeenieIteration                          = 157,
        WieldRequirements                        = 158,
        WieldSkilltype                           = 159,
        WieldDifficulty                          = 160,
        [ServerOnly]
        HouseMaxHooksUsable                      = 161,
        [ServerOnly]
        HouseCurrentHooksUsable                  = 162,
        [ServerOnly]
        AllegianceMinLevel                       = 163,
        [ServerOnly]
        AllegianceMaxLevel                       = 164,
        [ServerOnly]
        HouseRelinkHookCount                     = 165,
        SlayerCreatureType                       = 166,
        [ServerOnly]
        ConfirmationInProgress                   = 167,
        [ServerOnly]
        ConfirmationTypeInProgress               = 168,
        [ServerOnly]
        TsysMutationData                         = 169,
        NumItemsInMaterial                       = 170,
        NumTimesTinkered                         = 171,
        AppraisalLongDescDecoration              = 172,
        AppraisalLockpickSuccessPercent          = 173,
        AppraisalPages                           = 174,
        AppraisalMaxPages                        = 175,
        AppraisalItemSkill                       = 176,
        GemCount                                 = 177,
        GemType                                  = 178,
        ImbuedEffect                             = 179,
        [ServerOnly]
        AttackersRawSkillValue                   = 180,
        ChessRank                                = 181,
        [ServerOnly]
        ChessTotalGames                          = 182,
        [ServerOnly]
        ChessGamesWon                            = 183,
        [ServerOnly]
        ChessGamesLost                           = 184,
        [ServerOnly]
        TypeOfAlteration                         = 185,
        [ServerOnly]
        SkillToBeAltered                         = 186,
        [ServerOnly]
        SkillAlterationCount                     = 187,
        HeritageGroup                            = 188,
        [ServerOnly]
        TransferFromAttribute                    = 189,
        [ServerOnly]
        TransferToAttribute                      = 190,
        [ServerOnly]
        AttributeTransferCount                   = 191,
        FakeFishingSkill                         = 192,
        NumKeys                                  = 193,
        [ServerOnly]
        DeathTimestamp                           = 194,
        [ServerOnly]
        PkTimestamp                              = 195,
        [ServerOnly]
        VictimTimestamp                          = 196,
        [ServerOnly]
        HookGroup                                = 197,
        [ServerOnly]
        AllegianceSwearTimestamp                 = 198,
        [ServerOnly]
        HousePurchaseTimestamp                   = 199,
        [ServerOnly]
        RedirectableEquippedArmorCount           = 200,
        [ServerOnly]
        MeleedefenseImbuedEffectTypeCache        = 201,
        [ServerOnly]
        MissileDefenseImbuedEffectTypeCache      = 202,
        [ServerOnly]
        MagicDefenseImbuedEffectTypeCache        = 203,
        ElementalDamageBonus                     = 204,
        [ServerOnly]
        ImbueAttempts                            = 205,
        [ServerOnly]
        ImbueSuccesses                           = 206,
        [ServerOnly]
        CreatureKills                            = 207,
        [ServerOnly]
        PlayerKillsPk                            = 208,
        [ServerOnly]
        PlayerKillsPkl                           = 209,
        [ServerOnly]
        RaresTierOne                             = 210,
        [ServerOnly]
        RaresTierTwo                             = 211,
        [ServerOnly]
        RaresTierThree                           = 212,
        [ServerOnly]
        RaresTierFour                            = 213,
        [ServerOnly]
        RaresTierFive                            = 214,
        [ServerOnly]
        AugmentationStat                         = 215,
        [ServerOnly]
        AugmentationFamilyStat                   = 216,
        [ServerOnly]
        AugmentationInnateFamily                 = 217,
        [ServerOnly]
        AugmentationInnateStrength               = 218,
        [ServerOnly]
        AugmentationInnateEndurance              = 219,
        [ServerOnly]
        AugmentationInnateCoordination           = 220,
        [ServerOnly]
        AugmentationInnateQuickness              = 221,
        [ServerOnly]
        AugmentationInnateFocus                  = 222,
        [ServerOnly]
        AugmentationInnateSelf                   = 223,
        [ServerOnly]
        AugmentationSpecializeSalvaging          = 224,
        [ServerOnly]
        AugmentationSpecializeItemTinkering      = 225,
        [ServerOnly]
        AugmentationSpecializeArmorTinkering     = 226,
        [ServerOnly]
        AugmentationSpecializeMagicItemTinkering = 227,
        [ServerOnly]
        AugmentationSpecializeWeaponTinkering    = 228,
        [ServerOnly]
        AugmentationExtraPackSlot                = 229,
        [ServerOnly]
        AugmentationIncreasedCarryingCapacity    = 230,
        [ServerOnly]
        AugmentationLessDeathItemLoss            = 231,
        [ServerOnly]
        AugmentationSpellsRemainPastDeath        = 232,
        [ServerOnly]
        AugmentationCriticalDefense              = 233,
        [ServerOnly]
        AugmentationBonusXp                      = 234,
        [ServerOnly]
        AugmentationBonusSalvage                 = 235,
        [ServerOnly]
        AugmentationBonusImbueChance             = 236,
        [ServerOnly]
        AugmentationFasterRegen                  = 237,
        [ServerOnly]
        AugmentationIncreasedSpellDuration       = 238,
        [ServerOnly]
        AugmentationResistanceFamily             = 239,
        [ServerOnly]
        AugmentationResistanceSlash              = 240,
        [ServerOnly]
        AugmentationResistancePierce             = 241,
        [ServerOnly]
        AugmentationResistanceBlunt              = 242,
        [ServerOnly]
        AugmentationResistanceAcid               = 243,
        [ServerOnly]
        AugmentationResistanceFire               = 244,
        [ServerOnly]
        AugmentationResistanceFrost              = 245,
        [ServerOnly]
        AugmentationResistanceLightning          = 246,
        [ServerOnly]
        RaresTierOneLogin                        = 247,
        [ServerOnly]
        RaresTierTwoLogin                        = 248,
        [ServerOnly]
        RaresTierThreeLogin                      = 249,
        [ServerOnly]
        RaresTierFourLogin                       = 250,
        [ServerOnly]
        RaresTierFiveLogin                       = 251,
        [ServerOnly]
        RaresLoginTimestamp                      = 252,
        [ServerOnly]
        RaresTierSix                             = 253,
        [ServerOnly]
        RaresTierSeven                           = 254,
        [ServerOnly]
        RaresTierSixLogin                        = 255,
        [ServerOnly]
        RaresTierSevenLogin                      = 256,
        ItemAttributeLimit                       = 257,
        ItemAttributeLevelLimit                  = 258,
        [ServerOnly]
        ItemAttribute2ndLimit                    = 259,
        [ServerOnly]
        ItemAttribute2ndLevelLimit               = 260,
        CharacterTitleId                         = 261,
        NumCharacterTitles                       = 262,
        ResistanceModifierType                   = 263,
        [ServerOnly]
        FreeTinkersBitfield                      = 264,
        EquipmentSetId                           = 265,
        [ServerOnly]
        PetClass                                 = 266,
        Lifespan                                 = 267,
        RemainingLifespan                        = 268,
        [ServerOnly]
        UseCreateQuantity                        = 269,
        WieldRequirements2                       = 270,
        WieldSkilltype2                          = 271,
        WieldDifficulty2                         = 272,
        WieldRequirements3                       = 273,
        WieldSkilltype3                          = 274,
        WieldDifficulty3                         = 275,
        WieldRequirements4                       = 276,
        WieldSkilltype4                          = 277,
        WieldDifficulty4                         = 278,
        Unique                                   = 279,
        SharedCooldown                           = 280,
        Faction1Bits                             = 281,
        [ServerOnly]
        Faction2Bits                             = 282,
        [ServerOnly]
        Faction3Bits                             = 283,
        [ServerOnly]
        Hatred1Bits                              = 284,
        [ServerOnly]
        Hatred2Bits                              = 285,
        [ServerOnly]
        Hatred3Bits                              = 286,
        SocietyRankCelhan                        = 287,
        SocietyRankEldweb                        = 288,
        SocietyRankRadblo                        = 289,
        [ServerOnly]
        HearLocalSignals                         = 290,
        [ServerOnly]
        HearLocalSignalsRadius                   = 291,
        Cleaving                                 = 292,
        [ServerOnly]
        AugmentationSpecializeGearcraft          = 293,
        [ServerOnly]
        AugmentationInfusedCreatureMagic         = 294,
        [ServerOnly]
        AugmentationInfusedItemMagic             = 295,
        [ServerOnly]
        AugmentationInfusedLifeMagic             = 296,
        [ServerOnly]
        AugmentationInfusedWarMagic              = 297,
        [ServerOnly]
        AugmentationCriticalExpertise            = 298,
        [ServerOnly]
        AugmentationCriticalPower                = 299,
        [ServerOnly]
        AugmentationSkilledMelee                 = 300,
        [ServerOnly]
        AugmentationSkilledMissile               = 301,
        [ServerOnly]
        AugmentationSkilledMagic                 = 302,
        ImbuedEffect2                            = 303,
        ImbuedEffect3                            = 304,
        ImbuedEffect4                            = 305,
        ImbuedEffect5                            = 306,
        DamageRating                             = 307,
        DamageResistRating                       = 308,
        [ServerOnly]
        AugmentationDamageBonus                  = 309,
        [ServerOnly]
        AugmentationDamageReduction              = 310,
        [ServerOnly]
        ImbueStackingBits                        = 311,
        [ServerOnly]
        HealOverTime                             = 312,
        CritRating                               = 313,
        CritDamageRating                         = 314,
        CritResistRating                         = 315,
        CritDamageResistRating                   = 316,
        [ServerOnly]
        HealingResistRating                      = 317,
        [ServerOnly]
        DamageOverTime                           = 318,
        ItemMaxLevel                             = 319,
        ItemXpStyle                              = 320,
        [ServerOnly]
        EquipmentSetExtra                        = 321,
        [ServerOnly]
        AetheriaBitfield                         = 322,
        [ServerOnly]
        HealingBoostRating                       = 323,
        HeritageSpecificArmor                    = 324,
        [ServerOnly]
        AlternateRacialSkills                    = 325,
        /// <summary>
        /// why was this defaulted to 1?  leaving comment
        /// </summary>
        [ServerOnly]
        AugmentationJackOfAllTrades              = 326,
        [ServerOnly]
        AugmentationResistanceNether             = 327,
        [ServerOnly]
        AugmentationInfusedVoidMagic             = 328,
        [ServerOnly]
        WeaknessRating                           = 329,
        [ServerOnly]
        NetherOverTime                           = 330,
        [ServerOnly]
        NetherResistRating                       = 331,
        [ServerOnly]
        LuminanceAward                           = 332,
        [ServerOnly]
        LumAugDamageRating                       = 333,
        [ServerOnly]
        LumAugDamageReductionRating              = 334,
        [ServerOnly]
        LumAugCritDamageRating                   = 335,
        [ServerOnly]
        LumAugCritReductionRating                = 336,
        [ServerOnly]
        LumAugSurgeEffectRating                  = 337,
        [ServerOnly]
        LumAugSurgeChanceRating                  = 338,
        [ServerOnly]
        LumAugItemManaUsage                      = 339,
        [ServerOnly]
        LumAugItemManaGain                       = 340,
        [ServerOnly]
        LumAugVitality                           = 341,
        [ServerOnly]
        LumAugHealingRating                      = 342,
        [ServerOnly]
        LumAugSkilledCraft                       = 343,
        [ServerOnly]
        LumAugSkilledSpec                        = 344,
        [ServerOnly]
        LumAugNoDestroyCraft                     = 345,
        [ServerOnly]
        RestrictInteraction                      = 346,
        [ServerOnly]
        OlthoiLootTimestamp                      = 347,
        [ServerOnly]
        OlthoiLootStep                           = 348,
        [ServerOnly]
        UseCreatesContractId                     = 349,
        [ServerOnly]
        DotResistRating                          = 350,
        LifeResistRating                         = 351,
        CloakWeaveProc                           = 352,
        WeaponType                               = 353,
        [ServerOnly]
        MeleeMastery                             = 354,
        [ServerOnly]
        RangedMastery                            = 355,
        [ServerOnly]
        SneakAttackRating                        = 356,
        [ServerOnly]
        RecklessnessRating                       = 357,
        [ServerOnly]
        DeceptionRating                          = 358,
        [ServerOnly]
        CombatPetRange                           = 359,
        [ServerOnly]
        WeaponAuraDamage                         = 360,
        [ServerOnly]
        WeaponAuraSpeed                          = 361,
        [ServerOnly]
        SummoningMastery                         = 362,
        [ServerOnly]
        HeartbeatLifespan                        = 363,
        [ServerOnly]
        UseLevelRequirement                      = 364,
        [ServerOnly]
        LumAugAllSkills                          = 365,
        UseRequiresSkill                         = 366,
        UseRequiresSkillLevel                    = 367,
        UseRequiresSkillSpec                     = 368,
        UseRequiresLevel                         = 369,
        GearDamage                               = 370,
        GearDamageResist                         = 371,
        GearCrit                                 = 372,
        GearCritResist                           = 373,
        GearCritDamage                           = 374,
        GearCritDamageResist                     = 375,
        GearHealingBoost                         = 376,
        GearNetherResist                         = 377,
        GearLifeResist                           = 378,
        GearMaxHealth                            = 379,
        Unknown380                               = 380,
        Unknown381                               = 381,
        Unknown382                               = 382,
        Unknown383                               = 383,
        Unknown384                               = 384,
        Unknown385                               = 385,
        Unknown386                               = 386,
        Unknown387                               = 387,
        Unknown388                               = 388,
        Unknown389                               = 389,
        Unknown390                               = 390,
        [ServerOnly]
        TotalLogins                              = 9001,
        [ServerOnly]
        DeletionTimestamp                        = 9002,
        [ServerOnly]
        CharacterOptions1                        = 9003,
        [ServerOnly]
        CharacterOptions2                        = 9004,
        [ServerOnly]
        LootTier                                 = 9005,
        GeneratorProbability                     = 9006,
        WeenieType                               = 9007
    }
}
