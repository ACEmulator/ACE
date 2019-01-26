using System;
using System.Linq;
using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Entity;
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

        public bool HasItem => Inventory != null && Inventory.Count > 0;

        public WorldObject Item => Inventory != null ? Inventory.Values.FirstOrDefault() : null;

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

        public override ActivationResult CheckUseRequirements(WorldObject activator)
        {
            if (!(activator is Player player))
                return new ActivationResult(false);

            if (!House.RootHouse.HasPermission(player, true))
            {
                player.Session.Network.EnqueueSend(new GameEventCommunicationTransientString(player.Session, $"The {Name} is locked"));
                return new ActivationResult(false);
            }
            return new ActivationResult(true);
        }

        /// <summary>
        /// This event is raised when player adds item to hook
        /// </summary>
        public override void OnAddItem()
        {
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
