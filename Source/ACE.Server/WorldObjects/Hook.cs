using System;
using System.Linq;
using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Factories;
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
            if (!House.HasPermission(player, true))
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

            var weenie = DatabaseManager.World.GetCachedWeenie(WeenieClassId);
            var hook = WorldObjectFactory.CreateWorldObject(weenie, new ObjectGuid(0));

            SetupTableId = hook.SetupTableId;
            MotionTableId = hook.MotionTableId;
            PhysicsTableId = hook.PhysicsTableId;
            SoundTableId = hook.SoundTableId;
            Placement = hook.Placement;
            ObjScale = hook.ObjScale;
            Name = hook.Name;

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
            Name = item.Name;

            Placement = (Placement)(item.HookPlacement ?? (int)ACE.Entity.Enum.Placement.Hook);

            EnqueueBroadcast(new GameMessageUpdateObject(this));
        }

        public override MotionCommand MotionPickup
        {
            get
            {
                var hookType = (HookType)(HookType ?? 0);

                switch (hookType)
                {
                    default:
                        return MotionCommand.Pickup;

                    case ACE.Entity.Enum.HookType.Wall:
                        return MotionCommand.Pickup10;

                    case ACE.Entity.Enum.HookType.Ceiling:
                    case ACE.Entity.Enum.HookType.Roof:
                        return MotionCommand.Pickup20;
                }
            }
        }

        public void OnLoad()
        {
            var hidden = Inventory.Count == 0 && !(House.HouseHooksVisible ?? true);

            NoDraw = hidden;
            UiHidden = hidden;

            OnAddItem();
        }
    }
}
