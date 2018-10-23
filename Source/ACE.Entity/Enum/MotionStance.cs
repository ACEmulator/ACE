namespace ACE.Entity.Enum
{
    /// <summary>
    /// The list of stances for players and creatures
    /// This is a subset of MotionCommand
    /// </summary>
    public enum MotionStance : uint
    {
        Invalid              = 0x80000000,
        HandCombat           = 0x8000003c,
        NonCombat            = 0x8000003d,
        SwordCombat          = 0x8000003e,
        BowCombat            = 0x8000003f,
        SwordShieldCombat    = 0x80000040,
        CrossbowCombat       = 0x80000041,
        UnusedCombat         = 0x80000042,
        SlingCombat          = 0x80000043,
        TwoHandedSwordCombat = 0x80000044, // 2HandedSwordCombat
        TwoHandedStaffCombat = 0x80000045, // 2HandedStaffCombat 
        DualWieldCombat      = 0x80000046,
        ThrownWeaponCombat   = 0x80000047,
        Graze                = 0x80000048,
        Magic                = 0x80000049,
        BowNoAmmo            = 0x800000e8,
        CrossBowNoAmmo       = 0x800000e9,
        AtlatlCombat         = 0x8000013b, // 138 in PY16
        ThrownShieldCombat   = 0x8000013c, // 139 in PY16
    }
}
