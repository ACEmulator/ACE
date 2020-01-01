using System;

namespace ACE.Entity.Enum
{
    [Flags]
    public enum AttackType
    {
        Undef               = 0x0,
        Punch               = 0x0001,
        Thrust              = 0x0002,
        Slash               = 0x0004,
        Kick                = 0x0008,
        OffhandPunch        = 0x0010,
        DoubleSlash         = 0x0020,
        TripleSlash         = 0x0040,
        DoubleThrust        = 0x0080,
        TripleThrust        = 0x0100,
        OffhandThrust       = 0x0200,
        OffhandSlash        = 0x0400,
        OffhandDoubleSlash  = 0x0800,
        OffhandTripleSlash  = 0x1000,
        OffhandDoubleThrust = 0x2000,
        OffhandTripleThrust = 0x4000,

        Unarmed             = Punch | Kick | OffhandPunch,

        DoubleStrike        = DoubleSlash | DoubleThrust | OffhandDoubleSlash | OffhandDoubleThrust,
        TripleStrike        = TripleSlash | TripleThrust | OffhandTripleSlash | OffhandTripleThrust,

        Offhand             = OffhandThrust | OffhandSlash | OffhandDoubleSlash | OffhandTripleSlash | OffhandDoubleThrust | OffhandTripleThrust,
        Thrusts             = Thrust | DoubleThrust | TripleThrust | OffhandThrust | OffhandDoubleThrust | OffhandTripleThrust,
        Slashes             = Slash | DoubleSlash | TripleSlash | OffhandSlash | OffhandDoubleSlash | OffhandTripleSlash,
        Punches             = Punch | OffhandPunch,

        MultiStrike         = DoubleStrike | TripleStrike
    }

    public static class AttackTypeExtensions
    {
        public static bool IsMultiStrike(this AttackType attackType)
        {
            return (attackType & AttackType.MultiStrike) != 0;
        }

        public static AttackType ReduceMultiStrike(this AttackType attackType)
        {
            if (!attackType.IsMultiStrike())
                return AttackType.Undef;

            switch (attackType)
            {
                case AttackType.DoubleThrust:
                case AttackType.TripleThrust:
                    return AttackType.Thrust;

                case AttackType.DoubleSlash:
                case AttackType.TripleSlash:
                    return AttackType.Slash;

                case AttackType.OffhandDoubleThrust:
                case AttackType.OffhandTripleThrust:
                    return AttackType.OffhandThrust;

                case AttackType.OffhandDoubleSlash:
                case AttackType.OffhandTripleSlash:
                    return AttackType.OffhandSlash;

                default:
                    return AttackType.Undef;
            }
        }
    }
}
