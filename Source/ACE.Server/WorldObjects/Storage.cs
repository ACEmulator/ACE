using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
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
        }

        /// <summary>
        /// Called when player attempts to open a storage chest in a house
        /// </summary>
        public override void ActOnUse(WorldObject worldObject)
        {
            var player = worldObject as Player;
            if (player == null) return;

            // verify permissions to use storage
            if (!House.HasPermission(player, true))
            {
                player.Session.Network.EnqueueSend(new GameEventCommunicationTransientString(player.Session, $"The {Name} is locked!"));
                EnqueueBroadcast(new GameMessageSound(Guid, Sound.OpenFailDueToLock, 1.0f));
                player.SendUseDoneEvent();
                return;
            }

            base.ActOnUse(worldObject);
        }

        /// <summary>
        /// This event is raised when player adds item to house storage chest
        /// </summary>
        public override void OnAddItem()
        {
            //Console.WriteLine("Storage.OnAddItem()");
            OnAddRemoveItem();
        }

        /// <summary>
        /// This event is raised when player removes item from house storage chest
        /// </summary>
        public override void OnRemoveItem()
        {
            //Console.WriteLine("Storage.OnRemoveItem()");
            OnAddRemoveItem();
        }

        public void OnAddRemoveItem()
        {
        }
    }
}
