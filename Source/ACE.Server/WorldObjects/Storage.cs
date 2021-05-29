using log4net;

using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Models;
using ACE.Server.Entity;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    public class Storage : Chest
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public House House { get => ParentLink as House; }

        public override double Default_ChestResetInterval => double.PositiveInfinity;

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
            //Velocity = new Vector3(0, 0, 0.5f);
            BumpVelocity = true;
        }

        public override ActivationResult CheckUseRequirements(WorldObject activator)
        {
            var baseRequirements = base.CheckUseRequirements(activator);
            if (!baseRequirements.Success)
                return baseRequirements;

            if (!(activator is Player player))
                return new ActivationResult(false);

            if (player.IgnoreHouseBarriers)
                return new ActivationResult(true);

            var rootHouse = House?.RootHouse;

            if (rootHouse == null)
            {
                log.Error($"[HOUSE] {player.Name} tried to use Storage chest @ {Location}, couldn't find RootHouse (this shouldn't happen)");
                return new ActivationResult(false);
            }

            if (!rootHouse.HasPermission(player, true))
            {
                player.Session.Network.EnqueueSend(new GameEventCommunicationTransientString(player.Session, $"You do not have permission to access {Name}"));
                EnqueueBroadcast(new GameMessageSound(Guid, Sound.OpenFailDueToLock, 1.0f));
                return new ActivationResult(false);
            }
            return new ActivationResult(true);
        }

        /// <summary>
        /// This event is raised when player adds item to storage
        /// </summary>
        protected override void OnAddItem()
        {
            //Console.WriteLine("Storage.OnAddItem()");

            if (Inventory.Count > 0)
            {
                // Here we explicitly save the storage to the database to prevent item loss.
                // If the player adds an item to the storage, and the server crashes before the storage has been saved, the item will be lost.
                SaveBiotaToDatabase();
            }
        }

        /// <summary>
        /// This event is raised when player removes item from storage
        /// </summary>
        protected override void OnRemoveItem(WorldObject removedItem)
        {
            //Console.WriteLine("Storage.OnRemoveItem()");

            // Here we explicitly save the storage to the database to prevent property desync.
            SaveBiotaToDatabase();
        }
    }
}
