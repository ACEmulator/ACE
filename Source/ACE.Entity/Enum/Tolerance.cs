using System;

namespace ACE.Entity.Enum
{
    /// <summary>
    /// Determines when a monster will attack
    /// </summary>
    [Flags]
    public enum Tolerance
    {
        None        = 0,  // attack targets in range
        NoAttack    = 1,  // never attack
        Appraise    = 2,  // attack when ID'd or attacked
        Unknown     = 4,  // unused?
        Provoke     = 8,  // used in conjunction with 32
        Unknown2    = 16, // unused?
        Target      = 32, // only target original attacker
        Retaliate   = 64, // only attack after attacked

        Monster     = 128 // only attack other monsters
    };
}
