namespace ACE.Entity.Enum
{
    // TODO: Figure out what bitfield(s) these values map to and replace with OR's
    // Note: These IDs are from the last version of the client. Earlier versions of the client had different values for some of the enums.
    public enum MotionCommand : uint
    {
        Invalid                               = 0x0,
        HoldRun                               = 0x85000001,
        HoldSidestep                          = 0x85000002,
        Ready                                 = 0x41000003,
        Stop                                  = 0x40000004,
        WalkForward                           = 0x45000005,
        WalkBackwards                         = 0x45000006,
        RunForward                            = 0x44000007,
        Fallen                                = 0x40000008,
        Interpolating                         = 0x40000009,
        Hover                                 = 0x4000000a,
        On                                    = 0x4000000b,
        Off                                   = 0x4000000c,
        TurnRight                             = 0x6500000d,
        TurnLeft                              = 0x6500000e,
        SideStepRight                         = 0x6500000f,
        SideStepLeft                          = 0x65000010,
        Dead                                  = 0x40000011,
        Crouch                                = 0x41000012,
        Sitting                               = 0x41000013,
        Sleeping                              = 0x41000014,
        Falling                               = 0x40000015,
        Reload                                = 0x40000016,
        Unload                                = 0x40000017,
        Pickup                                = 0x40000018,
        StoreInBackpack                       = 0x40000019,
        Eat                                   = 0x4000001a,
        Drink                                 = 0x4000001b,
        Reading                               = 0x4000001c,
        JumpCharging                          = 0x4000001d,
        AimLevel                              = 0x4000001e,
        AimHigh15                             = 0x4000001f,
        AimHigh30                             = 0x40000020,
        AimHigh45                             = 0x40000021,
        AimHigh60                             = 0x40000022,
        AimHigh75                             = 0x40000023,
        AimHigh90                             = 0x40000024,
        AimLow15                              = 0x40000025,
        AimLow30                              = 0x40000026,
        AimLow45                              = 0x40000027,
        AimLow60                              = 0x40000028,
        AimLow75                              = 0x40000029,
        AimLow90                              = 0x4000002a,
        MagicBlast                            = 0x4000002b,
        MagicSelfHead                         = 0x4000002c,
        MagicSelfHeart                        = 0x4000002d,
        MagicBonus                            = 0x4000002e,
        MagicClap                             = 0x4000002f,
        MagicHarm                             = 0x40000030,
        MagicHeal                             = 0x40000031,
        MagicThrowMissile                     = 0x40000032,
        MagicRecoilMissile                    = 0x40000033,
        MagicPenalty                          = 0x40000034,
        MagicTransfer                         = 0x40000035,
        MagicVision                           = 0x40000036,
        MagicEnchantItem                      = 0x40000037,
        MagicPortal                           = 0x40000038,
        MagicPray                             = 0x40000039,
        StopTurning                           = 0x2000003a,
        Jump                                  = 0x2500003b,
        HandCombat                            = 0x8000003c,
        NonCombat                             = 0x8000003d,
        SwordCombat                           = 0x8000003e,
        BowCombat                             = 0x8000003f,
        SwordShieldCombat                     = 0x80000040,
        CrossbowCombat                        = 0x80000041,
        UnusedCombat                          = 0x80000042,
        SlingCombat                           = 0x80000043,
        TwoHandedSwordCombat                  = 0x80000044,
        TwoHandedStaffCombat                  = 0x80000045,
        DualWieldCombat                       = 0x80000046,
        ThrownWeaponCombat                    = 0x80000047,
        Graze                                 = 0x80000048,
        Magic                                 = 0x80000049,
        Hop                                   = 0x1000004a,
        Jumpup                                = 0x1000004b,
        Cheer                                 = 0x1300004c,
        ChestBeat                             = 0x1000004d,
        TippedLeft                            = 0x1000004e,
        TippedRight                           = 0x1000004f,
        FallDown                              = 0x10000050,
        Twitch1                               = 0x10000051,
        Twitch2                               = 0x10000052,
        Twitch3                               = 0x10000053,
        Twitch4                               = 0x10000054,
        StaggerBackward                       = 0x10000055,
        StaggerForward                        = 0x10000056,
        Sanctuary                             = 0x10000057,
        ThrustMed                             = 0x10000058,
        ThrustLow                             = 0x10000059,
        ThrustHigh                            = 0x1000005a,
        SlashHigh                             = 0x1000005b,
        SlashMed                              = 0x1000005c,
        SlashLow                              = 0x1000005d,
        BackhandHigh                          = 0x1000005e,
        BackhandMed                           = 0x1000005f,
        BackhandLow                           = 0x10000060,
        Shoot                                 = 0x10000061,
        AttackHigh1                           = 0x10000062,
        AttackMed1                            = 0x10000063,
        AttackLow1                            = 0x10000064,
        AttackHigh2                           = 0x10000065,
        AttackMed2                            = 0x10000066,
        AttackLow2                            = 0x10000067,
        AttackHigh3                           = 0x10000068,
        AttackMed3                            = 0x10000069,
        AttackLow3                            = 0x1000006a,
        HeadThrow                             = 0x1000006b,
        FistSlam                              = 0x1000006c,
        BreatheFlame                          = 0x1000006d,
        SpinAttack                            = 0x1000006e,
        MagicPowerUp01                        = 0x1000006f,
        MagicPowerUp02                        = 0x10000070,
        MagicPowerUp03                        = 0x10000071,
        MagicPowerUp04                        = 0x10000072,
        MagicPowerUp05                        = 0x10000073,
        MagicPowerUp06                        = 0x10000074,
        MagicPowerUp07                        = 0x10000075,
        MagicPowerUp08                        = 0x10000076,
        MagicPowerUp09                        = 0x10000077,
        MagicPowerUp10                        = 0x10000078,
        ShakeFist                             = 0x13000079,
        Beckon                                = 0x1300007a,
        BeSeeingYou                           = 0x1300007b,
        BlowKiss                              = 0x1300007c,
        BowDeep                               = 0x1300007d,
        ClapHands                             = 0x1300007e,
        Cry                                   = 0x1300007f,
        Laugh                                 = 0x13000080,
        MimeEat                               = 0x13000081,
        MimeDrink                             = 0x13000082,
        Nod                                   = 0x13000083,
        Point                                 = 0x13000084,
        ShakeHead                             = 0x13000085,
        Shrug                                 = 0x13000086,
        Wave                                  = 0x13000087,
        Akimbo                                = 0x13000088,
        HeartyLaugh                           = 0x13000089,
        Salute                                = 0x1300008a,
        ScratchHead                           = 0x1300008b,
        SmackHead                             = 0x1300008c,
        TapFoot                               = 0x1300008d,
        WaveHigh                              = 0x1300008e,
        WaveLow                               = 0x1300008f,
        YawnStretch                           = 0x13000090,
        Cringe                                = 0x13000091,
        Kneel                                 = 0x13000092,
        Plead                                 = 0x13000093,
        Shiver                                = 0x13000094,
        Shoo                                  = 0x13000095,
        Slouch                                = 0x13000096,
        Spit                                  = 0x13000097,
        Surrender                             = 0x13000098,
        Woah                                  = 0x13000099,
        Winded                                = 0x1300009a,
        YMCA                                  = 0x1200009b,
        EnterGame                             = 0x1000009c,
        ExitGame                              = 0x1000009d,
        OnCreation                            = 0x1000009e,
        OnDestruction                         = 0x1000009f,
        EnterPortal                           = 0x100000a0,
        ExitPortal                            = 0x100000a1,
        Cancel                                = 0x80000a2,
        UseSelected                           = 0x90000a3,
        AutosortSelected                      = 0x90000a4,
        DropSelected                          = 0x90000a5,
        GiveSelected                          = 0x90000a6,
        SplitSelected                         = 0x90000a7,
        ExamineSelected                       = 0x90000a8,
        CreateShortcutToSelected              = 0x80000a9,
        PreviousCompassItem                   = 0x90000aa,
        NextCompassItem                       = 0x90000ab,
        ClosestCompassItem                    = 0x90000ac,
        PreviousSelection                     = 0x90000ad,
        LastAttacker                          = 0x90000ae,
        PreviousFellow                        = 0x90000af,
        NextFellow                            = 0x90000b0,
        ToggleCombat                          = 0x90000b1,
        HighAttack                            = 0xd0000b2,
        MediumAttack                          = 0xd0000b3,
        LowAttack                             = 0xd0000b4,
        EnterChat                             = 0x80000b5,
        ToggleChat                            = 0x80000b6,
        SavePosition                          = 0x80000b7,
        OptionsPanel                          = 0x90000b8,
        ResetView                             = 0x90000b9,
        CameraLeftRotate                      = 0xd0000ba,
        CameraRightRotate                     = 0xd0000bb,
        CameraRaise                           = 0xd0000bc,
        CameraLower                           = 0xd0000bd,
        CameraCloser                          = 0xd0000be,
        CameraFarther                         = 0xd0000bf,
        FloorView                             = 0x90000c0,
        MouseLook                             = 0xc0000c1,
        PreviousItem                          = 0x90000c2,
        NextItem                              = 0x90000c3,
        ClosestItem                           = 0x90000c4,
        ShiftView                             = 0xd0000c5,
        MapView                               = 0x90000c6,
        AutoRun                               = 0x90000c7,
        DecreasePowerSetting                  = 0x90000c8,
        IncreasePowerSetting                  = 0x90000c9,
        Pray                                  = 0x130000ca,
        Mock                                  = 0x130000cb,
        Teapot                                = 0x130000cc,
        SpecialAttack1                        = 0x100000cd,
        SpecialAttack2                        = 0x100000ce,
        SpecialAttack3                        = 0x100000cf,
        MissileAttack1                        = 0x100000d0,
        MissileAttack2                        = 0x100000d1,
        MissileAttack3                        = 0x100000d2,
        CastSpell                             = 0x400000d3,
        Flatulence                            = 0x120000d4,
        FirstPersonView                       = 0x90000d5,
        AllegiancePanel                       = 0x90000d6,
        FellowshipPanel                       = 0x90000d7,
        SpellbookPanel                        = 0x90000d8,
        SpellComponentsPanel                  = 0x90000d9,
        HousePanel                            = 0x90000da,
        AttributesPanel                       = 0x90000db,
        SkillsPanel                           = 0x90000dc,
        MapPanel                              = 0x90000dd,
        InventoryPanel                        = 0x90000de,
        Demonet                               = 0x120000df,
        UseMagicStaff                         = 0x400000e0,
        UseMagicWand                          = 0x400000e1,
        Blink                                 = 0x100000e2,
        Bite                                  = 0x100000e3,
        TwitchSubstate1                       = 0x400000e4,
        TwitchSubstate2                       = 0x400000e5,
        TwitchSubstate3                       = 0x400000e6,
        CaptureScreenshotToFile               = 0x90000e7,
        BowNoAmmo                             = 0x800000e8,
        CrossBowNoAmmo                        = 0x800000e9,
        ShakeFistState                        = 0x430000ea,
        PrayState                             = 0x430000eb,
        BowDeepState                          = 0x430000ec,
        ClapHandsState                        = 0x430000ed,
        CrossArmsState                        = 0x430000ee,
        ShiverState                           = 0x430000ef,
        PointState                            = 0x430000f0,
        WaveState                             = 0x430000f1,
        AkimboState                           = 0x430000f2,
        SaluteState                           = 0x430000f3,
        ScratchHeadState                      = 0x430000f4,
        TapFootState                          = 0x430000f5,
        LeanState                             = 0x430000f6,
        KneelState                            = 0x430000f7,
        PleadState                            = 0x430000f8,
        ATOYOT                                = 0x420000f9,
        SlouchState                           = 0x430000fa,
        SurrenderState                        = 0x430000fb,
        WoahState                             = 0x430000fc,
        WindedState                           = 0x430000fd,
        AutoCreateShortcuts                   = 0x90000fe,
        AutoRepeatAttacks                     = 0x90000ff,
        AutoTarget                            = 0x9000100,
        AdvancedCombatInterface               = 0x9000101,
        IgnoreAllegianceRequests              = 0x9000102,
        IgnoreFellowshipRequests              = 0x9000103,
        InvertMouseLook                       = 0x9000104,
        LetPlayersGiveYouItems                = 0x9000105,
        AutoTrackCombatTargets                = 0x9000106,
        DisplayTooltips                       = 0x9000107,
        AttemptToDeceivePlayers               = 0x9000108,
        RunAsDefaultMovement                  = 0x9000109,
        StayInChatModeAfterSend               = 0x900010a,
        RightClickToMouseLook                 = 0x900010b,
        VividTargetIndicator                  = 0x900010c,
        SelectSelf                            = 0x900010d,
        SkillHealSelf                         = 0x1000010e,
        //WoahDuplicate1                      = 0x1000010f,  // ushort collision with NextMonster?
        SkillHealOther                        = 0x1000010f,
        //MimeDrinkDuplicate1                 = 0x10000110,  // ushort collision with PreviousMonster?
        //MimeDrinkDuplicate2                 = 0x10000111,  // ushort collision with ClosestMonster?
        //NextMonster                         = 0x900010f,
        PreviousMonster                       = 0x9000110,
        ClosestMonster                        = 0x9000111,
        NextPlayer                            = 0x9000112,
        PreviousPlayer                        = 0x9000113,
        ClosestPlayer                         = 0x9000114,
        SnowAngelState                        = 0x43000118,
        WarmHands                             = 0x13000119,
        CurtseyState                          = 0x4300011a,
        AFKState                              = 0x4300011b,
        MeditateState                         = 0x4300011c,
        TradePanel                            = 0x900011d,
        LogOut                                = 0x1000011e,
        DoubleSlashLow                        = 0x1000011f,
        DoubleSlashMed                        = 0x10000120,
        DoubleSlashHigh                       = 0x10000121,
        TripleSlashLow                        = 0x10000122,
        TripleSlashMed                        = 0x10000123,
        TripleSlashHigh                       = 0x10000124,
        DoubleThrustLow                       = 0x10000125,
        DoubleThrustMed                       = 0x10000126,
        DoubleThrustHigh                      = 0x10000127,
        TripleThrustLow                       = 0x10000128,
        TripleThrustMed                       = 0x10000129,
        TripleThrustHigh                      = 0x1000012a,
        MagicPowerUp01Purple                  = 0x1000012b,
        MagicPowerUp02Purple                  = 0x1000012c,
        MagicPowerUp03Purple                  = 0x1000012d,
        MagicPowerUp04Purple                  = 0x1000012e,
        MagicPowerUp05Purple                  = 0x1000012f,
        MagicPowerUp06Purple                  = 0x10000130,
        MagicPowerUp07Purple                  = 0x10000131,
        MagicPowerUp08Purple                  = 0x10000132,
        MagicPowerUp09Purple                  = 0x10000133,
        MagicPowerUp10Purple                  = 0x10000134,
        Helper                                = 0x13000135,
        Pickup5                               = 0x40000136,
        Pickup10                              = 0x40000137,
        Pickup15                              = 0x40000138,
        Pickup20                              = 0x40000139,
        HouseRecall                           = 0x1000013a,
        AtlatlCombat                          = 0x8000013b,
        ThrownShieldCombat                    = 0x8000013c,
        SitState                              = 0x4300013d,
        SitCrossleggedState                   = 0x4300013e,
        SitBackState                          = 0x4300013f,
        PointLeftState                        = 0x43000140,
        PointRightState                       = 0x43000141,
        TalktotheHandState                    = 0x43000142,
        PointDownState                        = 0x43000143,
        DrudgeDanceState                      = 0x43000144,
        PossumState                           = 0x43000145,
        ReadState                             = 0x43000146,
        ThinkerState                          = 0x43000147,
        HaveASeatState                        = 0x43000148,
        AtEaseState                           = 0x43000149,
        NudgeLeft                             = 0x1300014a,
        NudgeRight                            = 0x1300014b,
        PointLeft                             = 0x1300014c,
        PointRight                            = 0x1300014d,
        PointDown                             = 0x1300014e,
        Knock                                 = 0x1300014f,
        ScanHorizon                           = 0x13000150,
        DrudgeDance                           = 0x13000151,
        HaveASeat                             = 0x13000152,
        LifestoneRecall                       = 0x10000153,
        CharacterOptionsPanel                 = 0x9000154,
        SoundAndGraphicsPanel                 = 0x9000155,
        HelpfulSpellsPanel                    = 0x9000156,
        HarmfulSpellsPanel                    = 0x9000157,
        CharacterInformationPanel             = 0x9000158,
        LinkStatusPanel                       = 0x9000159,
        VitaePanel                            = 0x900015a,
        ShareFellowshipXP                     = 0x900015b,
        ShareFellowshipLoot                   = 0x900015c,
        AcceptCorpseLooting                   = 0x900015d,
        IgnoreTradeRequests                   = 0x900015e,
        DisableWeather                        = 0x900015f,
        DisableHouseEffect                    = 0x9000160,
        SideBySideVitals                      = 0x9000161,
        ShowRadarCoordinates                  = 0x9000162,
        ShowSpellDurations                    = 0x9000163,
        MuteOnLosingFocus                     = 0x9000164,
        Fishing                               = 0x10000165,
        MarketplaceRecall                     = 0x10000166,
        EnterPKLite                           = 0x10000167,
        AllegianceChat                        = 0x9000168,
        AutomaticallyAcceptFellowshipRequests = 0x9000169,
        Reply                                 = 0x900016a,
        MonarchReply                          = 0x900016b,
        PatronReply                           = 0x900016c,
        ToggleCraftingChanceOfSuccessDialog   = 0x900016d,
        UseClosestUnopenedCorpse              = 0x900016e,
        UseNextUnopenedCorpse                 = 0x900016f,
        IssueSlashCommand                     = 0x9000170,
        AllegianceHometownRecall              = 0x10000171,
        PKArenaRecall                         = 0x10000172,
        OffhandSlashHigh                      = 0x10000173,
        OffhandSlashMed                       = 0x10000174,
        OffhandSlashLow                       = 0x10000175,
        OffhandThrustHigh                     = 0x10000176,
        OffhandThrustMed                      = 0x10000177,
        OffhandThrustLow                      = 0x10000178,
        OffhandDoubleSlashLow                 = 0x10000179,
        OffhandDoubleSlashMed                 = 0x1000017a,
        OffhandDoubleSlashHigh                = 0x1000017b,
        OffhandTripleSlashLow                 = 0x1000017c,
        OffhandTripleSlashMed                 = 0x1000017d,
        OffhandTripleSlashHigh                = 0x1000017e,
        OffhandDoubleThrustLow                = 0x1000017f,
        OffhandDoubleThrustMed                = 0x10000180,
        OffhandDoubleThrustHigh               = 0x10000181,
        OffhandTripleThrustLow                = 0x10000182,
        OffhandTripleThrustMed                = 0x10000183,
        OffhandTripleThrustHigh               = 0x10000184,
        OffhandKick                           = 0x10000185,
        AttackHigh4                           = 0x10000186,
        AttackMed4                            = 0x10000187,
        AttackLow4                            = 0x10000188,
        AttackHigh5                           = 0x10000189,
        AttackMed5                            = 0x1000018a,
        AttackLow5                            = 0x1000018b,
        AttackHigh6                           = 0x1000018c,
        AttackMed6                            = 0x1000018d,
        AttackLow6                            = 0x1000018e,
        PunchFastHigh                         = 0x1000018f,
        PunchFastMed                          = 0x10000190,
        PunchFastLow                          = 0x10000191,
        PunchSlowHigh                         = 0x10000192,
        PunchSlowMed                          = 0x10000193,
        PunchSlowLow                          = 0x10000194,
        OffhandPunchFastHigh                  = 0x10000195,
        OffhandPunchFastMed                   = 0x10000196,
        OffhandPunchFastLow                   = 0x10000197,
        OffhandPunchSlowHigh                  = 0x10000198,
        OffhandPunchSlowMed                   = 0x10000199,
        OffhandPunchSlowLow                   = 0x1000019a,
        WoahDuplicate2                        = 0x1000019b, // Appears to be the same as Motion_Woah except it starts with 0x10 instead of 0x13
    }

    public static class MotionCommandHelper
    {
        public static MotionCommand GetMotion(MotionCommand motionCommand)
        {
            if ((int)motionCommand == 0x10000162)
                return MotionCommand.Fishing;

            return (MotionCommand)motionCommand;
        }

        public static bool IsMultiStrike(this MotionCommand motionCommand)
        {
            return motionCommand >= MotionCommand.DoubleSlashLow && motionCommand <= MotionCommand.TripleThrustHigh ||
                   motionCommand >= MotionCommand.OffhandDoubleSlashLow && motionCommand <= MotionCommand.OffhandTripleThrustHigh;
        }

        public static MotionCommand ReduceMultiStrike(this MotionCommand motionCommand)
        {
            if (!motionCommand.IsMultiStrike())
                return MotionCommand.Invalid;

            switch (motionCommand)
            {
                case MotionCommand.DoubleSlashLow:
                case MotionCommand.TripleSlashLow:
                    return MotionCommand.SlashLow;

                case MotionCommand.DoubleSlashMed:
                case MotionCommand.TripleSlashMed:
                    return MotionCommand.SlashMed;

                case MotionCommand.DoubleSlashHigh:
                case MotionCommand.TripleSlashHigh:
                    return MotionCommand.SlashHigh;

                case MotionCommand.DoubleThrustLow:
                case MotionCommand.TripleThrustLow:
                    return MotionCommand.ThrustLow;

                case MotionCommand.DoubleThrustMed:
                case MotionCommand.TripleThrustMed:
                    return MotionCommand.ThrustMed;

                case MotionCommand.DoubleThrustHigh:
                case MotionCommand.TripleThrustHigh:
                    return MotionCommand.ThrustHigh;

                case MotionCommand.OffhandDoubleSlashLow:
                case MotionCommand.OffhandTripleSlashLow:
                    return MotionCommand.SlashLow;

                case MotionCommand.OffhandDoubleSlashMed:
                case MotionCommand.OffhandTripleSlashMed:
                    return MotionCommand.SlashMed;

                case MotionCommand.OffhandDoubleSlashHigh:
                case MotionCommand.OffhandTripleSlashHigh:
                    return MotionCommand.SlashHigh;

                case MotionCommand.OffhandDoubleThrustLow:
                case MotionCommand.OffhandTripleThrustLow:
                    return MotionCommand.ThrustLow;

                case MotionCommand.OffhandDoubleThrustMed:
                case MotionCommand.OffhandTripleThrustMed:
                    return MotionCommand.ThrustMed;

                case MotionCommand.OffhandDoubleThrustHigh:
                case MotionCommand.OffhandTripleThrustHigh:
                    return MotionCommand.ThrustHigh;

                default:
                    return MotionCommand.Invalid;
            }
        }

        public static bool IsSubsequent(this MotionCommand motionCommand)
        {
            return motionCommand >= MotionCommand.AttackHigh2 && motionCommand <= MotionCommand.AttackLow3 ||
                   motionCommand >= MotionCommand.AttackHigh4 && motionCommand <= MotionCommand.AttackLow6;
        }

        public static MotionCommand ReduceSubsequent(this MotionCommand motionCommand)
        {
            if (!motionCommand.IsSubsequent())
                return MotionCommand.Invalid;

            switch (motionCommand)
            {
                case MotionCommand.AttackLow2:
                case MotionCommand.AttackLow3:
                case MotionCommand.AttackLow4:
                case MotionCommand.AttackLow5:
                case MotionCommand.AttackLow6:
                    return MotionCommand.AttackLow1;

                case MotionCommand.AttackMed2:
                case MotionCommand.AttackMed3:
                case MotionCommand.AttackMed4:
                case MotionCommand.AttackMed5:
                case MotionCommand.AttackMed6:
                    return MotionCommand.AttackMed1;

                case MotionCommand.AttackHigh2:
                case MotionCommand.AttackHigh3:
                case MotionCommand.AttackHigh4:
                case MotionCommand.AttackHigh5:
                case MotionCommand.AttackHigh6:
                    return MotionCommand.AttackHigh1;

                default:
                    return MotionCommand.Invalid;
            }
        }

        public static float GetAimAngle(this MotionCommand motion)
        {
            switch (motion)
            {
                default:
                    return 0.0f;

                case MotionCommand.AimHigh15:
                    return 15.0f;
                case MotionCommand.AimHigh30:
                    return 30.0f;
                case MotionCommand.AimHigh45:
                    return 45.0f;
                case MotionCommand.AimHigh60:
                    return 60.0f;
                case MotionCommand.AimHigh75:
                    return 75.0f;
                case MotionCommand.AimHigh90:
                    return 90.0f;

                case MotionCommand.AimLow15:
                    return -15.0f;
                case MotionCommand.AimLow30:
                    return -30.0f;
                case MotionCommand.AimLow45:
                    return -45.0f;
                case MotionCommand.AimLow60:
                    return -60.0f;
                case MotionCommand.AimLow75:
                    return -75.0f;
                case MotionCommand.AimLow90:
                    return -90.0f;
            }
        }
    }
}
