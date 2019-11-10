using System;
using ACE.Server.Network.Structure;
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
        public static WeaponMask GetHighlightMask(WeaponProfile profile)
        {
            WeaponMask highlightMask = 0;

            var wielder = profile.Wielder;
            var weapon = profile.Weapon;

            // Enchant applies to all weapons
            if (profile.Enchantment_WeaponDefense != 0)
                highlightMask |= WeaponMask.MeleeDefense;

            // Following enchants do not apply to caster weapons
            if (weapon.WeenieType != ACE.Entity.Enum.WeenieType.Caster)
            {
                if (profile.Enchantment_WeaponOffense != 0)
                    highlightMask |= WeaponMask.AttackSkill;
                if (profile.Enchantment_WeaponTime != 0)
                    highlightMask |= WeaponMask.Speed;
                if (profile.Enchantment_Damage != 0)
                    highlightMask |= WeaponMask.Damage;
                if (profile.Enchantment_DamageVariance != 1.0f)
                    highlightMask |= WeaponMask.DamageVariance;
                if (profile.Enchantment_DamageMod != 0)
                    highlightMask |= WeaponMask.DamageMod;
            }

            return highlightMask;
        }

        public static WeaponMask GetColorMask(WeaponProfile profile)
        {
            WeaponMask colorMask = 0;

            var weapon = profile.Weapon;
            var wielder = profile.Wielder;

            // Enchant applies to all weapons
            if (profile.Enchantment_WeaponDefense > 0)
                colorMask |= WeaponMask.MeleeDefense;

            // Following enchants do not apply to caster weapons
            if (weapon.WeenieType != ACE.Entity.Enum.WeenieType.Caster)
            {
                // item enchanments are currently being cast on wielder
                if (profile.Enchantment_WeaponOffense > 0)
                    colorMask |= WeaponMask.AttackSkill;
                if (profile.Enchantment_WeaponTime < 0)
                    colorMask |= WeaponMask.Speed;
                if (profile.Enchantment_Damage > 0)
                    colorMask |= WeaponMask.Damage;
                if (profile.Enchantment_DamageVariance < 1.0f)
                    colorMask |= WeaponMask.DamageVariance;
                if (profile.Enchantment_DamageMod > 0)
                    colorMask |= WeaponMask.DamageMod;
            }

            return colorMask;
        }
    }
}
