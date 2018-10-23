using System;
using System.Linq;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    /// <summary>
    /// House hooks for item placement
    /// </summary>
    public class Hook : Container
    {
        public House House { get => ParentLink as House; }

        /// <summary>
        /// Default hook profiles
        /// </summary>
        public static uint FloorHook_SetupTableID = 0x2000A8D;
        public static uint WallHook_SetupTableID = 0x2000A8E;
        public static uint CeilingHook_SetupTableID = 0x2000A8C;

        public static uint Hook_PhysicsTableID = 0x3400002B;
        public static float Hook_ObjScale = 0.5f;

        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public Hook(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Hook(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
            IsLocked = false;
            IsOpen = false;
        }

        public override void ActOnUse(WorldObject worldObject)
        {
            //Console.WriteLine($"Hook.ActOnUse({worldObject.Name})");
            var player = worldObject as Player;
            if (player == null) return;

            // verify permissions to use hook
            if (HouseOwner == null || HouseOwner.Value != player.Guid.Full)
            {
                player.Session.Network.EnqueueSend(new GameEventCommunicationTransientString(player.Session, $"The {Name} is locked"));
                player.SendUseDoneEvent();
                return;
            }
            base.ActOnUse(worldObject);
        }

        /// <summary>
        /// This event is raised when player adds item to hook
        /// </summary>
        public override void OnAddItem()
        {
            // TODO: send new temporary dynamic guid item?
            //Console.WriteLine("Hook.OnAddItem()");
            OnAddRemoveItem();
        }

        /// <summary>
        /// This event is raised when player removes item from hook
        /// </summary>
        public override void OnRemoveItem()
        {
            //Console.WriteLine("Hook.OnRemoveItem()");
            OnAddRemoveItem();
        }

        public void OnAddRemoveItem()
        {
            SetItem();
        }

        /// <summary>
        /// Sets the hook profile to an empty hook
        /// </summary>
        public void SetNoItem()
        {
            //Console.WriteLine("SetNoItem()");

            if (Name.Equals("Floor Hook"))
                SetupTableId = FloorHook_SetupTableID;
            else if (Name.Equals("Wall Hook"))
                SetupTableId = WallHook_SetupTableID;
            else if (Name.Equals("Ceiling Hook"))
                SetupTableId = CeilingHook_SetupTableID;
            else
                Console.WriteLine($"Unknown hook: {Name}");

            MotionTableId = 0;
            PhysicsTableId = Hook_PhysicsTableID;
            SoundTableId = 0;
            ObjScale = Hook_ObjScale;

            EnqueueBroadcast(new GameMessageUpdateObject(this));
        }

        /// <summary>
        /// Sets the hook profile to the container item
        /// </summary>
        public void SetItem()
        {
            var item = Inventory.Values.FirstOrDefault();
            if (item == null)
            {
                SetNoItem();
                return;
            }
            Console.WriteLine("Setting hook item " + item.Guid);

            SetupTableId = item.SetupTableId;
            MotionTableId = item.MotionTableId;
            PhysicsTableId = item.PhysicsTableId;
            SoundTableId = item.SoundTableId;
            ObjScale = item.ObjScale;

            EnqueueBroadcast(new GameMessageUpdateObject(this));
        }
    }
}
