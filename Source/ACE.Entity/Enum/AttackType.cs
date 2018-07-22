using System;

namespace ACE.Entity.Enum
{
    [Flags]
    public enum AttackType
    {
        Undef               = 0,
        Punch               = (1 << 0),
        Thrust              = (1 << 1),
        Slash               = (1 << 2),
        Kick                = (1 << 3),
        OffhandPunch        = (1 << 4),
        DoubleSlash         = (1 << 5),
        TripleSlash         = (1 << 6),
        DoubleThrust        = (1 << 7),
        TripleThrust        = (1 << 8),
        OffhandThrust       = (1 << 9),
        OffhandSlash        = (1 << 10),
        OffhandDoubleSlash  = (1 << 11),
        OffhandTripleSlash  = (1 << 12),
        OffhandDoubleThrust = (1 << 13),
        OffhandTripleThrust = (1 << 14),

        Unarmed             = Punch | Kick | OffhandPunch, // 25
        MultiStrike         = DoubleSlash | TripleSlash | DoubleThrust | TripleThrust | OffhandDoubleSlash | OffhandTripleSlash | OffhandDoubleThrust | OffhandTripleThrust // 31200
    }
}
