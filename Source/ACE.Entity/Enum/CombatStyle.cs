namespace ACE.Entity.Enum
{
    /// <summary>
    /// exported from the decompiled client.
    /// </summary>
    public enum CombatStyle
    {
        Undef               = 0x00000,
        Unarmed             = 0x00001,
        OneHanded           = 0x00002,
        OneHandedAndShield  = 0x00004,
        TwoHanded           = 0x00008,
        Bow                 = 0x00010,
        Crossbow            = 0x00020,
        Sling               = 0x00040,
        ThrownWeapon        = 0x00080,
        DualWield           = 0x00100,
        Magic               = 0x00200,
        Atlatl              = 0x00400,
        ThrownShield        = 0x00800,
        Reserved1           = 0x01000,
        Reserved2           = 0x02000,
        Reserved3           = 0x04000,
        Reserved4           = 0x08000,
        StubbornMagic       = 0x10000,
        StubbornProjectile  = 0x20000,
        StubbornMelee       = 0x40000,
        StubbornMissile     = 0x80000,
        Melee               = Unarmed | OneHanded | OneHandedAndShield | TwoHanded | DualWield, // 271
        Missile             = Bow | Crossbow | Sling | ThrownWeapon | Atlatl | ThrownShield, // 3312
        // Magics           = Magic, // 512
        All                 = 65535
    }
}
