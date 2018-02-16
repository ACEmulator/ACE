using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

namespace ACE.Server.Entity.WorldObjects
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
            DescriptionFlags |= ObjectDescriptionFlag.Attackable;

            SetProperty(PropertyBool.Attackable, true);
        }

        /// <summary>
        /// This will also set the icon based on the palette
        /// </summary>
        public void SetProperties(uint palette, double shade)
        {
            var icon = DatManager.PortalDat.ReadFromDat<ClothingTable>(GetProperty(PropertyDataId.ClothingBase) ?? 0).GetIcon(palette);

            SetProperty(PropertyDataId.Icon, icon);
            SetProperty(PropertyDataId.PaletteBase, palette);
            SetProperty(PropertyDouble.Shade, shade);
        }
    }
}
