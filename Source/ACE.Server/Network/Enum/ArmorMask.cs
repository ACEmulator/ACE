using ACE.Entity.Enum;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.Enum
{
    public enum ArmorMask
    {
        ArmorLevel              = 0x1,
        SlashingProtection      = 0x2,
        PiercingProtection      = 0x4,
        BludgeoningProtection   = 0x8,
        ColdProtection          = 0x10,
        FireProtection          = 0x20,
        AcidProtection          = 0x40,
        LightningProtection     = 0x80
    };

    public static class ArmorMaskHelper
    {
        /// <summary>
        /// Determines if there is a highlight for each armor protection vs. damage type
        /// </summary>
        public static ArmorMask GetHighlightMask(WorldObject armor)
        {
            ArmorMask highlightMask = 0;

            if (armor == null)
                return highlightMask;

            // item enchanments are currently being cast on wielder
            if (armor.EnchantmentManager.GetArmorMod() != 0)
                highlightMask |= ArmorMask.ArmorLevel;
            if (armor.EnchantmentManager.GetArmorModVsType(DamageType.Slash) != 0)
                highlightMask |= ArmorMask.SlashingProtection;
            if (armor.EnchantmentManager.GetArmorModVsType(DamageType.Pierce) != 0)
                highlightMask |= ArmorMask.PiercingProtection;
            if (armor.EnchantmentManager.GetArmorModVsType(DamageType.Bludgeon) != 0)
                highlightMask |= ArmorMask.BludgeoningProtection;
            if (armor.EnchantmentManager.GetArmorModVsType(DamageType.Cold) != 0)
                highlightMask |= ArmorMask.ColdProtection;
            if (armor.EnchantmentManager.GetArmorModVsType(DamageType.Fire) != 0)
                highlightMask |= ArmorMask.FireProtection;
            if (armor.EnchantmentManager.GetArmorModVsType(DamageType.Acid) != 0)
                highlightMask |= ArmorMask.AcidProtection;
            if (armor.EnchantmentManager.GetArmorModVsType(DamageType.Electric) != 0)
                highlightMask |= ArmorMask.LightningProtection;

            return highlightMask;
        }

        /// <summary>
        /// Determines the red/green color for each armor protection vs. damage type
        /// </summary>
        public static ArmorMask GetColorMask(WorldObject armor)
        {
            ArmorMask colorMask = 0;

            if (armor == null)
                return colorMask;

            if (armor.EnchantmentManager.GetArmorMod() > 0)
                colorMask |= ArmorMask.ArmorLevel;
            if (armor.EnchantmentManager.GetArmorModVsType(DamageType.Slash) > 0)
                colorMask |= ArmorMask.SlashingProtection;
            if (armor.EnchantmentManager.GetArmorModVsType(DamageType.Pierce) > 0)
                colorMask |= ArmorMask.PiercingProtection;
            if (armor.EnchantmentManager.GetArmorModVsType(DamageType.Bludgeon) > 0)
                colorMask |= ArmorMask.BludgeoningProtection;
            if (armor.EnchantmentManager.GetArmorModVsType(DamageType.Cold) > 0)
                colorMask |= ArmorMask.ColdProtection;
            if (armor.EnchantmentManager.GetArmorModVsType(DamageType.Fire) > 0)
                colorMask |= ArmorMask.FireProtection;
            if (armor.EnchantmentManager.GetArmorModVsType(DamageType.Acid) > 0)
                colorMask |= ArmorMask.AcidProtection;
            if (armor.EnchantmentManager.GetArmorModVsType(DamageType.Electric) > 0)
                colorMask |= ArmorMask.LightningProtection;

            return colorMask;
        }
    }
}
