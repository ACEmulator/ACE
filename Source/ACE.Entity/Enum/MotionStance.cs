using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity.Enum
{
    /// <summary>
    /// Different commands or styles of stance.
    /// Items that are listed as Unknown are included for completeness, but are not found in PCAPS Og II
    /// </summary>
    public enum MotionStance
    {
        Invalid              = 0x00,
        UaNoShieldAttack     = 0x3C,
        Standing             = 0x3D,
        MeleeNoShieldAttack  = 0x3E,
        BowAttack            = 0x3F,
        MeleeShieldAttack    = 0x40,
        CrossBowAttack       = 0x41,
        Unknown1             = 0x42,
        SlingAttack          = 0x43,
        TwoHandedSwordAttack = 0x44,
        TwoHandedStaffAttack = 0x45,
        DualWieldAttack      = 0x46,
        ThrownWeaponAttack   = 0x47,
        Graze                = 0x48,
        Spellcasting         = 0x49,
        BowNoAmmo            = 0xE8,
        CrossBowNoAmmo       = 0xE9,
        AtlatlCombat         = 0x138,
        ThrownShieldCombat   = 0x138,
        SitCrossLegged       = 0x13B
    }
}
