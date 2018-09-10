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

        /// <summary>
        /// House Portals are on Use activated, rather than collision based activation
        /// The actual portal process is wrapped to the base portal class ActOnUse, after ACL check are performed
        /// </summary>
        /// <param name="worldObject"></param>
        public override void ActOnUse(WorldObject worldObject)
        {
            if (worldObject is Player)
            {
                var player = worldObject as Player;

                if (HouseOwner == player.Guid.Full) // TODO: Expand check to include guest ACL; currently only checking owner
                    base.ActOnUse(player);
                else
                {
                    player.Session.Network.EnqueueSend(new GameEventWeenieError(player.Session, WeenieError.YouMustBeHouseGuestToUsePortal));
                    player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session));
                }
            }
        }
    }
}
