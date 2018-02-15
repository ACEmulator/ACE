using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

namespace ACE.Server.Entity.WorldObjects
{
    public class Clothing : WorldObject
    {
        /// <summary>
        /// If biota is null, one will be created with default values for this WorldObject type.
        /// </summary>
        public Clothing(Weenie weenie, Biota biota = null) : base(weenie, biota)
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
