using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using System.IO;

namespace ACE.Server.WorldObjects
{
    public class Clothing : WorldObject
    {
        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public Clothing(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Clothing(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
        }

        /// <summary>
        /// This will also set the icon based on the palette
        /// </summary>
        public void SetProperties(int palette, double shade)
        {
            var icon = DatManager.PortalDat.ReadFromDat<ClothingTable>(GetProperty(PropertyDataId.ClothingBase) ?? 0).GetIcon((uint)palette);

            SetProperty(PropertyDataId.Icon, icon);
            SetProperty(PropertyInt.PaletteTemplate, palette);
            SetProperty(PropertyFloat.Shade, shade);
        }

        public override void SerializeIdentifyObjectResponse(BinaryWriter writer, bool success, IdentifyResponseFlags flags = IdentifyResponseFlags.None)
        {
                flags |= IdentifyResponseFlags.ArmorProfile;

            base.SerializeIdentifyObjectResponse(writer, success, flags);

            WriteIdentifyObjectArmorProfile(writer, this, success);
        }

        //protected static void WriteIdentifyObjectArmorProfile(BinaryWriter writer, IdentifyResponseFlags flags, List<AceObjectPropertiesDouble> propertiesArmor)
        protected static void WriteIdentifyObjectArmorProfile(BinaryWriter writer, WorldObject wo, bool success)
        {
            //var notNull = propertiesArmor.Where(p => p.PropertyValue != null).ToList();
            //if ((flags & IdentifyResponseFlags.ArmorProfile) == 0 || (notNull.Count == 0)) return;

            //foreach (AceObjectPropertiesDouble x in notNull)
            //    writer.Write((float)x.PropertyValue.Value);

            writer.Write((float)(wo.GetProperty(PropertyFloat.ResistSlash) ?? 0));
            writer.Write((float)(wo.GetProperty(PropertyFloat.ResistPierce) ?? 0));
            writer.Write((float)(wo.GetProperty(PropertyFloat.ResistBludgeon) ?? 0));
            writer.Write((float)(wo.GetProperty(PropertyFloat.ResistCold) ?? 0));
            writer.Write((float)(wo.GetProperty(PropertyFloat.ResistFire) ?? 0));
            writer.Write((float)(wo.GetProperty(PropertyFloat.ResistAcid) ?? 0));
            writer.Write((float)(wo.GetProperty(PropertyFloat.ResistNether) ?? 0));
            writer.Write((float)(wo.GetProperty(PropertyFloat.ResistElectric) ?? 0));
        }
    }
}
