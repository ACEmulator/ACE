using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    public class Storage : Chest
    {
        public House House { get => ParentLink as House; }

        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public Storage(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Storage(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
            IsLocked = false;
            IsOpen = false;

            // unanimated objects will float in the air, and not be affected by gravity
            // unless we give it a bit of velocity to start
            // fixes floating storage chests
            Velocity = new AceVector3(0.0f, 0.0f, 0.5f);
        }

        public override ActivationResult CheckUseRequirements(WorldObject activator)
        {
            if (!(activator is Player player))
                return new ActivationResult(false);

            if (!House.RootHouse.HasPermission(player, true))
            {
                player.Session.Network.EnqueueSend(new GameEventCommunicationTransientString(player.Session, $"The {Name} is locked!"));
                EnqueueBroadcast(new GameMessageSound(Guid, Sound.OpenFailDueToLock, 1.0f));
                return new ActivationResult(false);
            }
            return new ActivationResult(true);
        }
    }
}
