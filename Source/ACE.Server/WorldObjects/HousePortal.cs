using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Network.GameEvent.Events;

namespace ACE.Server.WorldObjects
{
    public sealed class HousePortal : Portal
    {
        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public HousePortal(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public HousePortal(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        public override void OnCollideObject(Player player)
        {
            if (HouseOwner == player.Guid.Full) // TODO: Expand check to include guest ACL; currently only checking owner
                base.OnCollideObject(player);
            else
                player.Session.Network.EnqueueSend(new GameEventWeenieError(player.Session, WeenieError.YouMustBeHouseGuestToUsePortal));
        }
    }
}
