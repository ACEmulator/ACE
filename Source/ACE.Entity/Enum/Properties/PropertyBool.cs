namespace ACE.Entity.Enum.Properties
{
    public enum PropertyBool : ushort
    {
        [ServerOnly]
        Undef                            = 0,
        Stuck                            = 1,
        Open                             = 2,
        Locked                           = 3,
        [ServerOnly]
        RotProof                         = 4,
        [ServerOnly]
        AllegianceUpdateRequest          = 5,
        [ServerOnly]
        AiUsesMana                       = 6,
        [ServerOnly]
        AiUseHumanMagicAnimations        = 7,
        [ServerOnly]
        AllowGive                        = 8,
        [ServerOnly]
        CurrentlyAttacking               = 9,
        [ServerOnly]
        AttackerAi                       = 10,
        IgnoreCollisions                 = 11,
        ReportCollisions                 = 12,
        Ethereal                         = 13,
        GravityStatus                    = 14,
        LightsStatus                     = 15,
        ScriptedCollision                = 16,
        Inelastic                        = 17,
        Visibility                       = 18,
        Attackable                       = 19,
        [ServerOnly]
        SafeSpellComponents              = 20,
        [ServerOnly]
        AdvocateState                    = 21,
        Inscribable                      = 22,
        [ServerOnly]
        DestroyOnSell                    = 23,
        UiHidden                         = 24,
        IgnoreHouseBarriers              = 25,
        HiddenAdmin                      = 26,
        [ServerOnly]
        PkWounder                        = 27,
        PkKiller                         = 28,
        [ServerOnly]
        NoCorpse                         = 29,
        [ServerOnly]
        UnderLifestoneProtection         = 30,
        [ServerOnly]
        ItemManaUpdatePending            = 31,
        GeneratorStatus                  = 32,
        [ServerOnly]
        ResetMessagePending              = 33,
        [ServerOnly]
        DefaultOpen                      = 34,
        [ServerOnly]
        DefaultLocked                    = 35,
        [ServerOnly]
        DefaultOn                        = 36,
        [ServerOnly]
        OpenForBusiness                  = 37,
        [ServerOnly]
        IsFrozen                         = 38,
        [ServerOnly]
        DealMagicalItems                 = 39,
        [ServerOnly]
        LogoffImDead                     = 40,
        ReportCollisionsAsEnvironment    = 41,
        AllowEdgeSlide                   = 42,
        [ServerOnly]
        AdvocateQuest                    = 43,
        [ServerOnly]
        IsAdmin                          = 44,
        [ServerOnly]
        IsArch                           = 45,
        [ServerOnly]
        IsSentinel                       = 46,
        [ServerOnly]
        IsAdvocate                       = 47,
        [ServerOnly]
        CurrentlyPoweringUp              = 48,
        [ServerOnly]
        GeneratorEnteredWorld            = 49,
        [ServerOnly]
        NeverFailCasting                 = 50,
        VendorService                    = 51,
        [ServerOnly]
        AiImmobile                       = 52,
        [ServerOnly]
        DamagedByCollisions              = 53,
        [ServerOnly]
        IsDynamic                        = 54,
        [ServerOnly]
        IsHot                            = 55,
        [ServerOnly]
        IsAffecting                      = 56,
        [ServerOnly]
        AffectsAis                       = 57,
        [ServerOnly]
        SpellQueueActive                 = 58,
        [ServerOnly]
        GeneratorDisabled                = 59,
        [ServerOnly]
        IsAcceptingTells                 = 60,
        [ServerOnly]
        LoggingChannel                   = 61,
        [ServerOnly]
        OpensAnyLock                     = 62,
        UnlimitedUse                     = 63,
        [ServerOnly]
        GeneratedTreasureItem            = 64,
        [ServerOnly]
        IgnoreMagicResist                = 65,
        [ServerOnly]
        IgnoreMagicArmor                 = 66,
        [ServerOnly]
        AiAllowTrade                     = 67,
        [ServerOnly]
        SpellComponentsRequired          = 68,
        IsSellable                       = 69,
        [ServerOnly]
        IgnoreShieldsBySkill             = 70,
        NoDraw                           = 71,
        [ServerOnly]
        ActivationUntargeted             = 72,
        [ServerOnly]
        HouseHasGottenPriorityBootPos    = 73,
        [ServerOnly]
        GeneratorAutomaticDestruction    = 74,
        [ServerOnly]
        HouseHooksVisible                = 75,
        [ServerOnly]
        HouseRequiresMonarch             = 76,
        [ServerOnly]
        HouseHooksEnabled                = 77,
        [ServerOnly]
        HouseNotifiedHudOfHookCount      = 78,
        [ServerOnly]
        AiAcceptEverything               = 79,
        [ServerOnly]
        IgnorePortalRestrictions         = 80,
        RequiresBackpackSlot             = 81,
        [ServerOnly]
        DontTurnOrMoveWhenGiving         = 82,
        [ServerOnly]
        NpcLooksLikeObject               = 83,
        [ServerOnly]
        IgnoreCloIcons                   = 84,
        AppraisalHasAllowedWielder       = 85,
        [ServerOnly]
        ChestRegenOnClose                = 86,
        [ServerOnly]
        LogoffInMinigame                 = 87,
        [ServerOnly]
        PortalShowDestination            = 88,
        [ServerOnly]
        PortalIgnoresPkAttackTimer       = 89,
        [ServerOnly]
        NpcInteractsSilently             = 90,
        Retained                         = 91,
        [ServerOnly]
        IgnoreAuthor                     = 92,
        [ServerOnly]
        Limbo                            = 93,
        AppraisalHasAllowedActivator     = 94,
        [ServerOnly]
        ExistedBeforeAllegianceXpChanges = 95,
        [ServerOnly]
        IsDeaf                           = 96,
        [ServerOnly]
        IsPsr                            = 97,
        [ServerOnly]
        Invincible                       = 98,
        Ivoryable                        = 99,
        Dyable                           = 100,
        [ServerOnly]
        CanGenerateRare                  = 101,
        [ServerOnly]
        CorpseGeneratedRare              = 102,
        [ServerOnly]
        NonProjectileMagicImmune         = 103,
        [ServerOnly]
        ActdReceivedItems                = 104,
        [ServerOnly]
        Unknown105                       = 105,
        [ServerOnly]
        FirstEnterWorldDone              = 106,
        [ServerOnly]
        RecallsDisabled                  = 107,
        RareUsesTimer                    = 108,
        [ServerOnly]
        ActdPreorderReceivedItems        = 109,
        [ServerOnly]
        Afk                              = 110,
        [ServerOnly]
        IsGagged                         = 111,
        [ServerOnly]
        ProcSpellSelfTargeted            = 112,
        [ServerOnly]
        IsAllegianceGagged               = 113,
        [ServerOnly]
        EquipmentSetTriggerPiece         = 114,
        [ServerOnly]
        Uninscribe                       = 115,
        WieldOnUse                       = 116,
        [ServerOnly]
        ChestClearedWhenClosed           = 117,
        [ServerOnly]
        NeverAttack                      = 118,
        [ServerOnly]
        SuppressGenerateEffect           = 119,
        [ServerOnly]
        TreasureCorpse                   = 120,
        [ServerOnly]
        EquipmentSetAddLevel             = 121,
        [ServerOnly]
        BarberActive                     = 122,
        [ServerOnly]
        TopLayerPriority                 = 123,
        [ServerOnly]
        NoHeldItemShown                  = 124,
        [ServerOnly]
        LoginAtLifestone                 = 125,
        [ServerOnly]
        OlthoiPk                         = 126,
        [ServerOnly]
        Account15Days                    = 127,
        [ServerOnly]
        HadNoVitae                       = 128,
        [ServerOnly]
        NoOlthoiTalk                     = 129,
        AutowieldLeft                    = 130,
        [ServerOnly]
        IsDeleted                        = 9001
    }
}
