using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using System.IO;

namespace ACE.Server.WorldObjects
{
    public class GenericObject : WorldObject
    {
        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public GenericObject(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public GenericObject(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
        }

        public override void HandleActionUseOnTarget(Player player, WorldObject target)
        {
            // TODO: does this have to be generic object?
            if (PetDevice.IsEncapsulatedSpirit(this) && target is PetDevice petDevice)
            {
                petDevice.Refill(player, this);
                return;
            }

            // fallback on recipe manager?
            base.HandleActionUseOnTarget(player, target);
        }
    }
}
