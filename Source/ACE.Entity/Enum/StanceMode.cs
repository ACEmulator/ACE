namespace ACE.Entity.Enum
{
    /// <summary>
    /// This should be the same as MotionStance & 0xFFFF
    /// </summary>
    public enum StanceMode: ushort
    {
        Invalid               = 0x0,
        HandCombat            = 0x3c,
        NonCombat             = 0x3d,
        SwordCombat           = 0x3e,
        BowCombat             = 0x3f,
        SwordShieldCombat     = 0x40,
        CrossbowCombat        = 0x41,
        UnusedCombat          = 0x42,
        SlingCombat           = 0x43,
        TwoHandedSwordCombat  = 0x44,   // 2HandedSwordCombat
        TwoHandedStaffCombat  = 0x45,   // 2HandedStaffCombat 
        DualWieldCombat       = 0x46,
        ThrownWeaponCombat    = 0x47,
        Graze                 = 0x48,   // unused?
        Magic                 = 0x49,
        BowNoAmmo             = 0xe8,
        CrossBowNoAmmo        = 0xe9,
        AtlatlCombat          = 0x13b,  // 138 in PY16
        ThrownShieldCombat    = 0x13c,  // 139 in PY16
    }
}
