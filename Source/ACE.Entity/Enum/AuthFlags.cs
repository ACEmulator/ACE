using System;

namespace ACE.Entity.Enum
{
    [Flags]
    public enum AuthFlags
    {
        None                 = 0x0,
        EnableCrypto         = 0x1,
        AdminAccountOverride = 0x2,   // admin logging in as another account
        ExtraData            = 0x4,
        LastDefault          = 0x4
    };
}
