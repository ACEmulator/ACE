namespace ACE.Entity.Enum
{
    public enum SecurityLevel
    {
        // 0x00018b49 : Length = 530, Leaf = 0x1203 LF_FIELDLIST
        // list[0] = LF_ENUMERATE, public, value = 0, name = 'Undef_SecurityLevel'
        // list[1] = LF_ENUMERATE, public, value = 0, name = 'Player_SecurityLevel'
        // list[2] = LF_ENUMERATE, public, value = 1, name = 'Advocate1_SecurityLevel'
        // list[3] = LF_ENUMERATE, public, value = 2, name = 'Advocate2_SecurityLevel'
        // list[4] = LF_ENUMERATE, public, value = 3, name = 'Advocate3_SecurityLevel'
        // list[5] = LF_ENUMERATE, public, value = 4, name = 'Advocate4_SecurityLevel'
        // list[6] = LF_ENUMERATE, public, value = 5, name = 'Advocate5_SecurityLevel'
        // list[7] = LF_ENUMERATE, public, value = 5, name = 'MaxAdvocate_SecurityLevel'
        // list[8] = LF_ENUMERATE, public, value = 6, name = 'Sentinel1_SecurityLevel'
        // list[9] = LF_ENUMERATE, public, value = 7, name = 'Sentinel2_SecurityLevel'
        // list[10] = LF_ENUMERATE, public, value = 8, name = 'Sentinel3_SecurityLevel'
        // list[11] = LF_ENUMERATE, public, value = 8, name = 'MaxSentinel_SecurityLevel'
        // list[12] = LF_ENUMERATE, public, value = 9, name = 'Turbine_SecurityLevel'
        // list[13] = LF_ENUMERATE, public, value = 10, name = 'Arch_SecurityLevel'
        // list[14] = LF_ENUMERATE, public, value = 11, name = 'Admin_SecurityLevel'
        // list[15] = LF_ENUMERATE, public, value = 11, name = 'Max_SecurityLevel'
        // list[16] = LF_ENUMERATE, public, value = (LF_ULONG) 2147483647, name = 'FORCE_SecurityLevelEnum_32_BIT'

        Undef       = 0,
        Player      = Undef,
        Advocate1   = 1,
        Advocate2   = 2,
        Advocate3   = 3,
        Advocate4   = 4,
        Advocate5   = 5,
        MaxAdvocate = Advocate5,
        Sentinel1   = 6,
        Sentinel2   = 7,
        Sentinel3   = 8,
        MaxSentinel = Sentinel3,
        Turbine     = 9,
        Arch        = 10,
        Admin       = 11,
        Max         = Admin

        // Player = 0,
        // Advocate = 1,
        // Sentinel = 2,
        // Envoy = 3,
        // Developer = 4,
        // Admin = 5
    }
}
