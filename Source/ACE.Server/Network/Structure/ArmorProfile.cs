using System;
using System.IO;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.Structure
{
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

        public ArmorProfile(WorldObject armor, WorldObject wielder)
        {
            SlashingProtection = GetArmorMod(armor, wielder, DamageType.Slash);
            PiercingProtection = GetArmorMod(armor, wielder, DamageType.Pierce);
            BludgeoningProtection = GetArmorMod(armor, wielder, DamageType.Bludgeon);
            ColdProtection = GetArmorMod(armor, wielder, DamageType.Cold);
            FireProtection = GetArmorMod(armor, wielder, DamageType.Fire);
            AcidProtection = GetArmorMod(armor, wielder, DamageType.Acid);
            NetherProtection = GetArmorMod(armor, wielder, DamageType.Nether);
            LightningProtection = GetArmorMod(armor, wielder, DamageType.Electric);
        }

        public float GetArmorMod(WorldObject armor, WorldObject wielder, DamageType damageType)
        {
            var type = armor.EnchantmentManager.GetImpenBaneKey(damageType);
            var baseResistance = armor.GetProperty(type) ?? 1.0f;

            if (wielder == null)
                return (float)baseResistance;

            // banes/lures
            var resistanceMod = wielder != null ? wielder.EnchantmentManager.GetArmorModVsType(damageType) : 0.0f;

            var effectiveRL = (float)(baseResistance + resistanceMod);

            // resistance cap
            if (effectiveRL > 2.0f)
                effectiveRL = 2.0f;

            return effectiveRL;
        }

        public bool HasModifier
        {
            get
            {
                // should be 1 or 0?
                return SlashingProtection != 0 || PiercingProtection != 0 || BludgeoningProtection != 0 ||
                    ColdProtection != 0 || FireProtection != 0 || AcidProtection != 0 || NetherProtection != 0 || LightningProtection != 0;
            }
        }
    }

    public static class ArmorProfileExtensions
    {
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
