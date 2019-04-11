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
        public static ResistMask GetHighlightMask(WorldObject wo)
        {
            ResistMask highlightMask = 0;

            if (wo.EnchantmentManager.GetResistanceMod(DamageType.Slash) != 1.0f)
                highlightMask |= ResistMask.ResistSlash;
            if (wo.EnchantmentManager.GetResistanceMod(DamageType.Pierce) != 1.0f)
                highlightMask |= ResistMask.ResistPierce;
            if (wo.EnchantmentManager.GetResistanceMod(DamageType.Bludgeon) != 1.0f)
                highlightMask |= ResistMask.ResistBludgeon;
            if (wo.EnchantmentManager.GetResistanceMod(DamageType.Fire) != 1.0f)
                highlightMask |= ResistMask.ResistFire;
            if (wo.EnchantmentManager.GetResistanceMod(DamageType.Cold) != 1.0f)
                highlightMask |= ResistMask.ResistCold;
            if (wo.EnchantmentManager.GetResistanceMod(DamageType.Electric) != 1.0f)
                highlightMask |= ResistMask.ResistElectric;
            if (wo.EnchantmentManager.GetResistanceMod(DamageType.Health) != 1.0f)      // ??
                highlightMask |= ResistMask.ResistHealthBoost;
            if (wo.EnchantmentManager.GetResistanceMod(DamageType.Stamina) != 1.0f)
                highlightMask |= ResistMask.ResistStaminaDrain;
            if (wo.EnchantmentManager.GetResistanceMod(DamageType.Stamina) != 1.0f)
                highlightMask |= ResistMask.ResistStaminaBoost;
            if (wo.EnchantmentManager.GetResistanceMod(DamageType.Mana) != 1.0f)
                highlightMask |= ResistMask.ResistManaDrain;
            if (wo.EnchantmentManager.GetResistanceMod(DamageType.Mana) != 1.0f)
                highlightMask |= ResistMask.ResistManaBoost;
            if (wo.EnchantmentManager.GetManaConvMod() != 1.0f)     // only for items?
                highlightMask |= ResistMask.ManaConversionMod;
            if (wo.EnchantmentManager.GetElementalDamageMod() != 0.0f)
                highlightMask |= ResistMask.ElementalDamageMod;
            if (wo.EnchantmentManager.GetResistanceMod(DamageType.Nether) != 1.0f)
                highlightMask |= ResistMask.ResistNether;

            return highlightMask;
        }

        public static ResistMask GetColorMask(WorldObject wo)
        {
            ResistMask colorMask = 0;

            if (wo.EnchantmentManager.GetResistanceMod(DamageType.Slash) > 1.0f)
                colorMask |= ResistMask.ResistSlash;
            if (wo.EnchantmentManager.GetResistanceMod(DamageType.Pierce) > 1.0f)
                colorMask |= ResistMask.ResistPierce;
            if (wo.EnchantmentManager.GetResistanceMod(DamageType.Bludgeon) > 1.0f)
                colorMask |= ResistMask.ResistBludgeon;
            if (wo.EnchantmentManager.GetResistanceMod(DamageType.Fire) > 1.0f)
                colorMask |= ResistMask.ResistFire;
            if (wo.EnchantmentManager.GetResistanceMod(DamageType.Cold) > 1.0f)
                colorMask |= ResistMask.ResistCold;
            if (wo.EnchantmentManager.GetResistanceMod(DamageType.Electric) > 1.0f)
                colorMask |= ResistMask.ResistElectric;
            if (wo.EnchantmentManager.GetResistanceMod(DamageType.Health) > 1.0f)      // ??
                colorMask |= ResistMask.ResistHealthBoost;
            if (wo.EnchantmentManager.GetResistanceMod(DamageType.Stamina) > 1.0f)
                colorMask |= ResistMask.ResistStaminaDrain;
            if (wo.EnchantmentManager.GetResistanceMod(DamageType.Stamina) > 1.0f)
                colorMask |= ResistMask.ResistStaminaBoost;
            if (wo.EnchantmentManager.GetResistanceMod(DamageType.Mana) > 1.0f)
                colorMask |= ResistMask.ResistManaDrain;
            if (wo.EnchantmentManager.GetResistanceMod(DamageType.Mana) > 1.0f)
                colorMask |= ResistMask.ResistManaBoost;
            if (wo.EnchantmentManager.GetManaConvMod() > 1.0f)      // only for items?
                colorMask |= ResistMask.ManaConversionMod;
            if (wo.EnchantmentManager.GetElementalDamageMod() > 0.0f)
                colorMask |= ResistMask.ElementalDamageMod;
            if (wo.EnchantmentManager.GetResistanceMod(DamageType.Nether) > 1.0f)
                colorMask |= ResistMask.ResistNether;

            return colorMask;
        }
    }
}
