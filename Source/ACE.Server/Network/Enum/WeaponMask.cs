using System;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.Enum
{
    [Flags]
    public enum WeaponMask
    {
        AttackSkill     = 0x1,
        MeleeDefense    = 0x2,
        Speed           = 0x4,
        Damage          = 0x8,
        DamageVariance  = 0x10,
        DamageMod       = 0x20
    }

    public static class WeaponMaskHelper
    {
        public static WeaponMask GetHighlightMask(WorldObject weapon)
        {
            WeaponMask highlightMask = 0;

            return highlightMask;
        }

        public static WeaponMask GetColorMask(WorldObject weapon)
        {
            WeaponMask colorMask = 0;

            return colorMask;
        }
    }
}
