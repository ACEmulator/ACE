using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

namespace ACE.Server.Entity.WorldObjects
{
    public class Bindstone : WorldObject
    {
        /// <summary>
        /// If biota is null, one will be created with default values for this WorldObject type.
        /// </summary>
        public Bindstone(Weenie weenie, Biota biota = null) : base(weenie, biota)
        {
            DescriptionFlags |= ObjectDescriptionFlag.Stuck | ObjectDescriptionFlag.Attackable;

            SetProperty(PropertyBool.Stuck, true);
            SetProperty(PropertyBool.Attackable, true);

            SetProperty(PropertyInt.ShowableOnRadar, (int)ACE.Entity.Enum.RadarBehavior.ShowAlways);
            SetProperty(PropertyInt.RadarBlipColor, (int)ACE.Entity.Enum.RadarColor.LifeStone);
        }
    }
}
