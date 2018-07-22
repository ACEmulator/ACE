using System;

namespace ACE.Entity.Enum
{
    [Flags]
    public enum AttackType
    {
        Undef_AttackType                = 0,
        Punch_AttackType                = (1 << 0),
        Thrust_AttackType               = (1 << 1),
        Slash_AttackType                = (1 << 2),
        Kick_AttackType                 = (1 << 3),
        OffhandPunch_AttackType         = (1 << 4),
        DoubleSlash_AttackType          = (1 << 5),
        TripleSlash_AttackType          = (1 << 6),
        DoubleThrust_AttackType         = (1 << 7),
        TripleThrust_AttackType         = (1 << 8),
        OffhandThrust_AttackType        = (1 << 9),
        OffhandSlash_AttackType         = (1 << 10),
        OffhandDoubleSlash_AttackType   = (1 << 11),
        OffhandTripleSlash_AttackType   = (1 << 12),
        OffhandDoubleThrust_AttackType  = (1 << 13),
        OffhandTripleThrust_AttackType  = (1 << 14),

        Unarmed_AttackType              = Punch_AttackType | Kick_AttackType | OffhandPunch_AttackType, // 25
        MultiStrike_AttackType          = DoubleSlash_AttackType | TripleSlash_AttackType | DoubleThrust_AttackType | TripleThrust_AttackType | OffhandDoubleSlash_AttackType | OffhandTripleSlash_AttackType | OffhandDoubleThrust_AttackType | OffhandTripleThrust_AttackType // 31200
    }
}
