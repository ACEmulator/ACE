
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Entity;
using ACE.Entity.Enum.Properties;

namespace ACE.Server.WorldObjects
{
    public class Clothing : WorldObject
    {
        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public Clothing(ACE.Entity.Models.Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Clothing(Database.Models.Shard.Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Clothing(ACE.Entity.Models.Biota biota) : base(biota)
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
    }
}
