using System;
using System.IO;
using ACE.Entity.Enum;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.Structure
{
    /// <summary>
    /// All of the resistance levels for a piece of armor / clothing
    /// (natural, banes, lures)
    /// </summary>
    public class ArmorProfile
    {
        public float SlashingProtection;
        public float PiercingProtection;
        public float BludgeoningProtection;
        public float ColdProtection;
        public float FireProtection;
        public float AcidProtection;
        public float NetherProtection;
        public float LightningProtection;

        public ArmorProfile(WorldObject armor)
        {
            SlashingProtection = GetArmorMod(armor, DamageType.Slash);
            PiercingProtection = GetArmorMod(armor, DamageType.Pierce);
            BludgeoningProtection = GetArmorMod(armor, DamageType.Bludgeon);
            ColdProtection = GetArmorMod(armor,DamageType.Cold);
            FireProtection = GetArmorMod(armor, DamageType.Fire);
            AcidProtection = GetArmorMod(armor, DamageType.Acid);
            NetherProtection = GetArmorMod(armor, DamageType.Nether);
            LightningProtection = GetArmorMod(armor, DamageType.Electric);
        }

        /// <summary>
        /// Calculates the effective RL for a piece of armor or clothing
        /// against a particular damage type
        /// </summary>
        public float GetArmorMod(WorldObject armor, DamageType damageType)
        {
            var type = armor.EnchantmentManager.GetImpenBaneKey(damageType);
            var baseResistance = armor.GetProperty(type) ?? 1.0f;

            if (armor == null)
                return (float)baseResistance;

            // banes/lures
            var resistanceMod = armor != null ? armor.EnchantmentManager.GetArmorModVsType(damageType) : 0.0f;

            var effectiveRL = (float)(baseResistance + resistanceMod);

            // resistance clamp
            // TODO: this would be a good place to test with client values
            //if (effectiveRL > 2.0f)
                //effectiveRL = 2.0f;
            effectiveRL = Math.Clamp(effectiveRL, -2.0f, 2.0f);

            return effectiveRL;
        }
    }

    public static class ArmorProfileExtensions
    {
        /// <summary>
        /// Writes the ArmorProfile to the network stream
        /// </summary>
        public static void Write(this BinaryWriter writer, ArmorProfile profile)
        {
            writer.Write(profile.SlashingProtection);
            writer.Write(profile.PiercingProtection);
            writer.Write(profile.BludgeoningProtection);
            writer.Write(profile.ColdProtection);
            writer.Write(profile.FireProtection);
            writer.Write(profile.AcidProtection);
            writer.Write(profile.NetherProtection);
            writer.Write(profile.LightningProtection);
        }
    }
}
