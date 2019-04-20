using System;

namespace ACE.Entity.Enum
{
    [Flags]
    public enum ActivationResponse
    {
        Undef       = 0,
        Use         = 0x2,
        Animate     = 0x4,
        Talk        = 0x10,
        Emote       = 0x800,
        CastSpell   = 0x1000,
        Generate    = 0x10000
    }
}
