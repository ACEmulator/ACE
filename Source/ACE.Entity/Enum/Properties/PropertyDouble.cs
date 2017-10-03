namespace ACE.Entity.Enum.Properties
{
    public enum PropertyDouble : ushort
    {
        // properties marked as ServerOnly are properties we never saw in PCAPs, from here:
        // http://ac.yotesfan.com/ace_object/not_used_enums.php
        // source: @OptimShi

        // description attributes are used by the weenie editor for a cleaner display name

        [ServerOnly]
        Undef                          = 0,
        [ServerOnly]
        HeartbeatInterval              = 1,
        [ServerOnly]
        HeartbeatTimestamp             = 2,
        [ServerOnly]
        HealthRate                     = 3,
        [ServerOnly]
        StaminaRate                    = 4,
        ManaRate                       = 5,
        [ServerOnly]
        HealthUponResurrection         = 6,
        [ServerOnly]
        StaminaUponResurrection        = 7,
        [ServerOnly]
        ManaUponResurrection           = 8,
        [ServerOnly]
        StartTime                      = 9,
        [ServerOnly]
        StopTime                       = 10,
        [ServerOnly]
        ResetInterval                  = 11,
        [ServerOnly]
        Shade                          = 12,
        ArmorModVsSlash                = 13,
        ArmorModVsPierce               = 14,
        ArmorModVsBludgeon             = 15,
        ArmorModVsCold                 = 16,
        ArmorModVsFire                 = 17,
        ArmorModVsAcid                 = 18,
        ArmorModVsElectric             = 19,
        [ServerOnly]
        CombatSpeed                    = 20,
        WeaponLength                   = 21,
        DamageVariance                 = 22,
        [ServerOnly]
        CurrentPowerMod                = 23,
        [ServerOnly]
        AccuracyMod                    = 24,
        [ServerOnly]
        StrengthMod                    = 25,
        MaximumVelocity                = 26,
        [ServerOnly]
        RotationSpeed                  = 27,
        [ServerOnly]
        MotionTimestamp                = 28,
        WeaponDefense                  = 29,
        [ServerOnly]
        WimpyLevel                     = 30,
        [ServerOnly]
        VisualAwarenessRange           = 31,
        [ServerOnly]
        AuralAwarenessRange            = 32,
        [ServerOnly]
        PerceptionLevel                = 33,
        [ServerOnly]
        PowerupTime                    = 34,
        [ServerOnly]
        MaxChargeDistance              = 35,
        [ServerOnly]
        ChargeSpeed                    = 36,
        [ServerOnly]
        BuyPrice                       = 37,
        [ServerOnly]
        SellPrice                      = 38,
        DefaultScale                   = 39,
        [ServerOnly]
        LockpickMod                    = 40,
        RegenerationInterval           = 41,
        [ServerOnly]
        RegenerationTimestamp          = 42,
        [ServerOnly]
        GeneratorRadius                = 43,
        [ServerOnly]
        TimeToRot                      = 44,
        [ServerOnly]
        DeathTimestamp                 = 45,
        [ServerOnly]
        PkTimestamp                    = 46,
        [ServerOnly]
        VictimTimestamp                = 47,
        [ServerOnly]
        LoginTimestamp                 = 48,
        [ServerOnly]
        CreationTimestamp              = 49,
        [ServerOnly]
        MinimumTimeSincePk             = 50,
        [ServerOnly]
        DeprecatedHousekeepingPriority = 51,
        [ServerOnly]
        AbuseLoggingTimestamp          = 52,
        [ServerOnly]
        LastPortalTeleportTimestamp    = 53,
        UseRadius                      = 54,
        [ServerOnly]
        HomeRadius                     = 55,
        [ServerOnly]
        ReleasedTimestamp              = 56,
        [ServerOnly]
        MinHomeRadius                  = 57,
        [ServerOnly]
        Facing                         = 58,
        [ServerOnly]
        ResetTimestamp                 = 59,
        [ServerOnly]
        LogoffTimestamp                = 60,
        [ServerOnly]
        EconRecoveryInterval           = 61,
        WeaponOffense                  = 62,
        DamageMod                      = 63,
        [ServerOnly]
        ResistSlash                    = 64,
        [ServerOnly]
        ResistPierce                   = 65,
        [ServerOnly]
        ResistBludgeon                 = 66,
        [ServerOnly]
        ResistFire                     = 67,
        [ServerOnly]
        ResistCold                     = 68,
        [ServerOnly]
        ResistAcid                     = 69,
        [ServerOnly]
        ResistElectric                 = 70,
        [ServerOnly]
        ResistHealthBoost              = 71,
        [ServerOnly]
        ResistStaminaDrain             = 72,
        [ServerOnly]
        ResistStaminaBoost             = 73,
        [ServerOnly]
        ResistManaDrain                = 74,
        [ServerOnly]
        ResistManaBoost                = 75,
        Translucency                   = 76,
        PhysicsScriptIntensity         = 77,
        Friction                       = 78,
        Elasticity                     = 79,
        [ServerOnly]
        AiUseMagicDelay                = 80,
        [ServerOnly]
        ItemMinSpellcraftMod           = 81,
        [ServerOnly]
        ItemMaxSpellcraftMod           = 82,
        [ServerOnly]
        ItemRankProbability            = 83,
        [ServerOnly]
        Shade2                         = 84,
        [ServerOnly]
        Shade3                         = 85,
        [ServerOnly]
        Shade4                         = 86,
        ItemEfficiency                 = 87,
        [ServerOnly]
        ItemManaUpdateTimestamp        = 88,
        [ServerOnly]
        SpellGestureSpeedMod           = 89,
        [ServerOnly]
        SpellStanceSpeedMod            = 90,
        [ServerOnly]
        AllegianceAppraisalTimestamp   = 91,
        [ServerOnly]
        PowerLevel                     = 92,
        [ServerOnly]
        AccuracyLevel                  = 93,
        [ServerOnly]
        AttackAngle                    = 94,
        [ServerOnly]
        AttackTimestamp                = 95,
        [ServerOnly]
        CheckpointTimestamp            = 96,
        [ServerOnly]
        SoldTimestamp                  = 97,
        [ServerOnly]
        UseTimestamp                   = 98,
        [ServerOnly]
        UseLockTimestamp               = 99,
        HealkitMod                     = 100,
        [ServerOnly]
        FrozenTimestamp                = 101,
        [ServerOnly]
        HealthRateMod                  = 102,
        [ServerOnly]
        AllegianceSwearTimestamp       = 103,
        [ServerOnly]
        ObviousRadarRange              = 104,
        [ServerOnly]
        HotspotCycleTime               = 105,
        [ServerOnly]
        HotspotCycleTimeVariance       = 106,
        [ServerOnly]
        SpamTimestamp                  = 107,
        [ServerOnly]
        SpamRate                       = 108,
        [ServerOnly]
        BondWieldedTreasure            = 109,
        [ServerOnly]
        BulkMod                        = 110,
        [ServerOnly]
        SizeMod                        = 111,
        [ServerOnly]
        GagTimestamp                   = 112,
        [ServerOnly]
        GeneratorUpdateTimestamp       = 113,
        [ServerOnly]
        DeathSpamTimestamp             = 114,
        [ServerOnly]
        DeathSpamRate                  = 115,
        [ServerOnly]
        WildAttackProbability          = 116,
        [ServerOnly]
        FocusedProbability             = 117,
        [ServerOnly]
        CrashAndTurnProbability        = 118,
        [ServerOnly]
        CrashAndTurnRadius             = 119,
        [ServerOnly]
        CrashAndTurnBias               = 120,
        [ServerOnly]
        GeneratorInitialDelay          = 121,
        [ServerOnly]
        AiAcquireHealth                = 122,
        [ServerOnly]
        AiAcquireStamina               = 123,
        [ServerOnly]
        AiAcquireMana                  = 124,
        /// <summary>
        /// this had a default of "1" - leaving comment to investigate potential options for defaulting these things (125)
        /// </summary>
        [ServerOnly]
        ResistHealthDrain              = 125,
        [ServerOnly]
        LifestoneProtectionTimestamp   = 126,
        [ServerOnly]
        AiCounteractEnchantment        = 127,
        [ServerOnly]
        AiDispelEnchantment            = 128,
        [ServerOnly]
        TradeTimestamp                 = 129,
        [ServerOnly]
        AiTargetedDetectionRadius      = 130,
        [ServerOnly]
        EmotePriority                  = 131,
        [ServerOnly]
        LastTeleportStartTimestamp     = 132,
        [ServerOnly]
        EventSpamTimestamp             = 133,
        [ServerOnly]
        EventSpamRate                  = 134,
        [ServerOnly]
        InventoryOffset                = 135,
        CriticalMultiplier             = 136,
        ManaStoneDestroyChance         = 137,
        [ServerOnly]
        SlayerDamageBonus              = 138,
        [ServerOnly]
        AllegianceInfoSpamTimestamp    = 139,
        [ServerOnly]
        AllegianceInfoSpamRate         = 140,
        [ServerOnly]
        NextSpellcastTimestamp         = 141,
        [ServerOnly]
        AppraisalRequestedTimestamp    = 142,
        [ServerOnly]
        AppraisalHeartbeatDueTimestamp = 143,
        ManaConversionMod              = 144,
        [ServerOnly]
        LastPkAttackTimestamp          = 145,
        [ServerOnly]
        FellowshipUpdateTimestamp      = 146,
        CriticalFrequency              = 147,
        [ServerOnly]
        LimboStartTimestamp            = 148,
        WeaponMissileDefense           = 149,
        WeaponMagicDefense             = 150,
        [ServerOnly]
        IgnoreShield                   = 151,
        ElementalDamageMod             = 152,
        [ServerOnly]
        StartMissileAttackTimestamp    = 153,
        [ServerOnly]
        LastRareUsedTimestamp          = 154,
        IgnoreArmor                    = 155,
        [ServerOnly]
        ProcSpellRate                  = 156,
        ResistanceModifier             = 157,
        [ServerOnly]
        AllegianceGagTimestamp         = 158,
        AbsorbMagicDamage              = 159,
        [ServerOnly]
        CachedMaxAbsorbMagicDamage     = 160,
        [ServerOnly]
        GagDuration                    = 161,
        [ServerOnly]
        AllegianceGagDuration          = 162,
        [ServerOnly]
        GlobalXpMod                    = 163,
        [ServerOnly]
        HealingModifier                = 164,
        ArmorModVsNether               = 165,
        [ServerOnly]
        ResistNether                   = 166,
        CooldownDuration               = 167,
        [ServerOnly]
        WeaponAuraOffense              = 168,
        [ServerOnly]
        WeaponAuraDefense              = 169,
        [ServerOnly]
        WeaponAuraElemental            = 170,
        [ServerOnly]
        WeaponAuraManaConv             = 171
    }
}
