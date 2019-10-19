using System;

using ACE.Entity.Enum;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.Enum
{
    [Flags]
    public enum ResistMask
    {
        ResistSlash         = 0x1,
        ResistPierce        = 0x2,
        ResistBludgeon      = 0x4,
        ResistFire          = 0x8,
        ResistCold          = 0x10,
        ResistAcid          = 0x20,
        ResistElectric      = 0x40,
        ResistHealthBoost   = 0x80,
        ResistStaminaDrain  = 0x100,
        ResistStaminaBoost  = 0x200,
        ResistManaDrain     = 0x400,
        ResistManaBoost     = 0x800,
        ManaConversionMod   = 0x1000,
        ElementalDamageMod  = 0x2000,
        ResistNether        = 0x4000
    };

    public static class ResistMaskHelper
    {
        public static ResistMask GetHighlightMask(WorldObject wielder, WorldObject weapon = null)
        {
            ResistMask highlightMask = 0;

            if (wielder.EnchantmentManager.GetResistanceMod(DamageType.Slash) != 1.0f)
                highlightMask |= ResistMask.ResistSlash;
            if (wielder.EnchantmentManager.GetResistanceMod(DamageType.Pierce) != 1.0f)
                highlightMask |= ResistMask.ResistPierce;
            if (wielder.EnchantmentManager.GetResistanceMod(DamageType.Bludgeon) != 1.0f)
                highlightMask |= ResistMask.ResistBludgeon;
            if (wielder.EnchantmentManager.GetResistanceMod(DamageType.Fire) != 1.0f)
                highlightMask |= ResistMask.ResistFire;
            if (wielder.EnchantmentManager.GetResistanceMod(DamageType.Cold) != 1.0f)
                highlightMask |= ResistMask.ResistCold;
            if (wielder.EnchantmentManager.GetResistanceMod(DamageType.Electric) != 1.0f)
                highlightMask |= ResistMask.ResistElectric;
            if (wielder.EnchantmentManager.GetResistanceMod(DamageType.Health) != 1.0f)      // ??
                highlightMask |= ResistMask.ResistHealthBoost;
            if (wielder.EnchantmentManager.GetResistanceMod(DamageType.Stamina) != 1.0f)
                highlightMask |= ResistMask.ResistStaminaDrain;
            if (wielder.EnchantmentManager.GetResistanceMod(DamageType.Stamina) != 1.0f)
                highlightMask |= ResistMask.ResistStaminaBoost;
            if (wielder.EnchantmentManager.GetResistanceMod(DamageType.Mana) != 1.0f)
                highlightMask |= ResistMask.ResistManaDrain;
            if (wielder.EnchantmentManager.GetResistanceMod(DamageType.Mana) != 1.0f)
                highlightMask |= ResistMask.ResistManaBoost;
            if (wielder.EnchantmentManager.GetManaConvMod() != 1.0f)     // only for items?
                highlightMask |= ResistMask.ManaConversionMod;

            var wielderElementalDamageMod = wielder.EnchantmentManager.GetElementalDamageMod();
            var weaponElementalDamageMod = weapon != null ? weapon.EnchantmentManager.GetElementalDamageMod() : 0.0f;

            var elementalDamageMod = wielderElementalDamageMod + weaponElementalDamageMod;

            if (elementalDamageMod != 0.0f)
                highlightMask |= ResistMask.ElementalDamageMod;

            if (wielder.EnchantmentManager.GetResistanceMod(DamageType.Nether) != 1.0f)
                highlightMask |= ResistMask.ResistNether;

            return highlightMask;
        }

        public static ResistMask GetColorMask(WorldObject wielder, WorldObject weapon = null)
        {
            ResistMask colorMask = 0;

            if (wielder.EnchantmentManager.GetResistanceMod(DamageType.Slash) > 1.0f)
                colorMask |= ResistMask.ResistSlash;
            if (wielder.EnchantmentManager.GetResistanceMod(DamageType.Pierce) > 1.0f)
                colorMask |= ResistMask.ResistPierce;
            if (wielder.EnchantmentManager.GetResistanceMod(DamageType.Bludgeon) > 1.0f)
                colorMask |= ResistMask.ResistBludgeon;
            if (wielder.EnchantmentManager.GetResistanceMod(DamageType.Fire) > 1.0f)
                colorMask |= ResistMask.ResistFire;
            if (wielder.EnchantmentManager.GetResistanceMod(DamageType.Cold) > 1.0f)
                colorMask |= ResistMask.ResistCold;
            if (wielder.EnchantmentManager.GetResistanceMod(DamageType.Electric) > 1.0f)
                colorMask |= ResistMask.ResistElectric;
            if (wielder.EnchantmentManager.GetResistanceMod(DamageType.Health) > 1.0f)      // ??
                colorMask |= ResistMask.ResistHealthBoost;
            if (wielder.EnchantmentManager.GetResistanceMod(DamageType.Stamina) > 1.0f)
                colorMask |= ResistMask.ResistStaminaDrain;
            if (wielder.EnchantmentManager.GetResistanceMod(DamageType.Stamina) > 1.0f)
                colorMask |= ResistMask.ResistStaminaBoost;
            if (wielder.EnchantmentManager.GetResistanceMod(DamageType.Mana) > 1.0f)
                colorMask |= ResistMask.ResistManaDrain;
            if (wielder.EnchantmentManager.GetResistanceMod(DamageType.Mana) > 1.0f)
                colorMask |= ResistMask.ResistManaBoost;
            if (wielder.EnchantmentManager.GetManaConvMod() > 1.0f)      // only for items?
                colorMask |= ResistMask.ManaConversionMod;

            var wielderElementalDamageMod = wielder.EnchantmentManager.GetElementalDamageMod();
            var weaponElementalDamageMod = weapon != null ? weapon.EnchantmentManager.GetElementalDamageMod() : 0.0f;

            var elementalDamageMod = wielderElementalDamageMod + weaponElementalDamageMod;

            if (elementalDamageMod > 0.0f)
                colorMask |= ResistMask.ElementalDamageMod;

            if (wielder.EnchantmentManager.GetResistanceMod(DamageType.Nether) > 1.0f)
                colorMask |= ResistMask.ResistNether;

            return colorMask;
        }
    }
}
