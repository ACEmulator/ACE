using System;
using System.Collections.Concurrent;
using System.Linq;

using log4net;

using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
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
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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

            if (!(House.HouseHooksVisible ?? true) && Item != null)
            {
                // redirect to item.CheckUseRequirements
                return Item.CheckUseRequirements(activator);
            }

            if (!House.RootHouse.HasPermission(player, true))
            {
                player.Session.Network.EnqueueSend(new GameEventCommunicationTransientString(player.Session, $"The {Name} is locked"));
                return new ActivationResult(false);
            }
            return new ActivationResult(true);
        }

        public override void ActOnUse(WorldObject wo)
        {
            if (!(House.HouseHooksVisible ?? true) && Item != null)
            {
                // redirect to item.ActOnUse
                Item.ActOnUse(wo);
                return;
            }

            base.ActOnUse(wo);
        }

        protected override void OnInitialInventoryLoadCompleted()
        {
            var hidden = Inventory.Count == 0 && !(House.HouseHooksVisible ?? true);

            NoDraw = hidden;
            UiHidden = hidden;

            if (Inventory.Count > 0)
                OnAddItem();
        }

        /// <summary>
        /// This event is raised when player adds item to hook
        /// </summary>
        protected override void OnAddItem()
        {
            //Console.WriteLine("Hook.OnAddItem()");

            var item = Inventory.Values.FirstOrDefault();

            if (item == null)
            {
                log.Error("OnAddItem() raised for Hook but Inventory collection has no values.");
                return;
            }

            NoDraw = false;
            UiHidden = false;

            SetupTableId = item.SetupTableId;
            MotionTableId = item.MotionTableId;
            PhysicsTableId = item.PhysicsTableId;
            SoundTableId = item.SoundTableId;
            ObjScale = item.ObjScale;
            Name = item.Name;

            Placement = (Placement)(item.HookPlacement ?? (int)ACE.Entity.Enum.Placement.Hook);

            // Here we explicilty save the hook to the database to prevent item loss.
            // If the player adds an item to the hook, and the server crashes before the hook has been saved, the item will be lost.
            SaveBiotaToDatabase();

            EnqueueBroadcast(new GameMessageUpdateObject(this));
        }

        private static readonly ConcurrentDictionary<uint, WorldObject> cachedHookReferences = new ConcurrentDictionary<uint, WorldObject>();

        /// <summary>
        /// This event is raised when player removes item from hook
        /// </summary>
        protected override void OnRemoveItem()
        {
            //Console.WriteLine("Hook.OnRemoveItem()");

            if (!cachedHookReferences.TryGetValue(WeenieClassId, out var hook))
            {
                var weenie = DatabaseManager.World.GetCachedWeenie(WeenieClassId);
                hook = WorldObjectFactory.CreateWorldObject(weenie, ObjectGuid.Invalid);

                cachedHookReferences[WeenieClassId] = hook;
            }

            SetupTableId = hook.SetupTableId;
            MotionTableId = hook.MotionTableId;
            PhysicsTableId = hook.PhysicsTableId;
            SoundTableId = hook.SoundTableId;
            Placement = hook.Placement;
            ObjScale = hook.ObjScale;
            Name = hook.Name;

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
    }
}
