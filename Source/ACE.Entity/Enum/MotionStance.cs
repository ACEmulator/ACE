using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity.Enum
{
    /// <summary>
    /// Different commands or styles of stance.
    /// Items that are listed as Unused are included for completeness, but are not found in PCAPS Og II
    /// Comments are the MotionStyle Enum as listed in the client
    /// </summary>
    public enum MotionStance : uint
    {
        Invalid              = 0x80000000,
        UaNoShieldAttack     = 0x8000003c, // HandCombat 
        Standing             = 0x8000003d, // NonCombat
        MeleeNoShieldAttack  = 0x8000003e, // SwordCombat
        BowAttack            = 0x8000003f, // BowCombat
        MeleeShieldAttack    = 0x80000040, // SwordShieldCombat
        CrossBowAttack       = 0x80000041, // CrossbowCombat
        Unused               = 0x80000042, // Unused Combat
        SlingAttack          = 0x80000043, // SlingCombat 
        TwoHandedSwordAttack = 0x80000044, // 2HandedSwordCombat 
        TwoHandedStaffAttack = 0x80000045, // 2HandedStaffCombat 
        DualWieldAttack      = 0x80000046, // DualWieldCombat 
        ThrownWeaponAttack   = 0x80000047, // ThrownWeaponCombat 
        Graze                = 0x80000048,
        Spellcasting         = 0x80000049, // Magic 
        BowNoAmmo            = 0x800000e8,
        CrossBowNoAmmo       = 0x800000e9,
        AtlatlCombat         = 0x80000138,
        ThrownShieldCombat   = 0x80000139,
    }
}
