using System.IO;
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

        public ArmorProfile(WorldObject wo)
        {
            SlashingProtection = (float)(wo.GetProperty(PropertyFloat.ArmorModVsSlash) ?? 0);
            PiercingProtection = (float)(wo.GetProperty(PropertyFloat.ArmorModVsPierce) ?? 0);
            BludgeoningProtection = (float)(wo.GetProperty(PropertyFloat.ArmorModVsBludgeon) ?? 0);
            ColdProtection = (float)(wo.GetProperty(PropertyFloat.ArmorModVsCold) ?? 0);
            FireProtection = (float)(wo.GetProperty(PropertyFloat.ArmorModVsFire) ?? 0);
            AcidProtection = (float)(wo.GetProperty(PropertyFloat.ArmorModVsAcid) ?? 0);
            NetherProtection = (float)(wo.GetProperty(PropertyFloat.ArmorModVsNether) ?? 0);
            LightningProtection = (float)(wo.GetProperty(PropertyFloat.ArmorModVsElectric) ?? 0);
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
