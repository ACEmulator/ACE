using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Network.Enum
{
    /// <summary>
    /// Different commands or styles of stance.
    /// Items that are listed as Unknown are included for completness, but are not found in PCAPS Og II
    /// </summary>
    public enum MotionStance
    {
        Invalid = 0x00,
        UANoShieldAttack = 0x3C,
        Standing = 0x3D,
        MeleeNoShieldAttack = 0x3E,
        BowAttack = 0x3F,
        MeleeShieldAttack = 0x40,
        CrossBowAttack = 0x41,
        Unknown1 = 0x42,
        SlingAttack = 0x43,
        TwoHandedSwordAttack = 0x44,
        TwoHandedStaffAttack = 0x45,
        DualWieldAttack = 0x46,
        ThrownWeaponAttack = 0x47,
        Unknown2 = 0x48,
        Spellcasting = 0x49,
        Unknown3 = 0xE8,
        Unknown4 = 0xE9,
        SitCrossLegged = 0x13B
    }
}
