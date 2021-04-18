using System;
using System.Collections.Concurrent;
using System.Linq;

using log4net;

using ACE.Database;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;
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

            if (player.IgnoreHouseBarriers)
                return new ActivationResult(true);

            var rootHouse = House.RootHouse;
            var houseOwner = rootHouse.HouseOwner;

            var houseHooksVisible = rootHouse.HouseHooksVisible ?? true;

            if (!houseHooksVisible)
            {
                if (Item is Hooker || Item is Book)
                {
                    // redirect to item.CheckUseRequirements
                    return Item.CheckUseRequirements(activator);
                }

                if (houseOwner != null && (player.Guid.Full == houseOwner || player.House?.HouseOwner == houseOwner))
                    return new ActivationResult(new GameEventWeenieErrorWithString(player.Session, WeenieErrorWithString.ItemUnusableOnHook_CanOpen, Name));
                else
                    return new ActivationResult(new GameEventWeenieErrorWithString(player.Session, WeenieErrorWithString.ItemUnusableOnHook_CannotOpen, Name));
            }

            if (player.House == null || houseOwner == null || (player.Guid.Full != houseOwner && player.House?.HouseOwner != houseOwner))
            {
                if (Item == null)
                    return new ActivationResult(new GameEventWeenieError(player.Session, WeenieError.HookItemNotUsable_CannotOpen));
                else if (Item is Hooker)
                    return new ActivationResult(new GameEventWeenieError(player.Session, WeenieError.YouAreNotPermittedToUseThatHook));
                else
                    return new ActivationResult(new GameEventWeenieErrorWithString(player.Session, WeenieErrorWithString.ItemUnusableOnHook_CannotOpen, Name));
            }

            return new ActivationResult(true);
        }

        public override void ActOnUse(WorldObject wo)
        {
            if (!(House.RootHouse.HouseHooksVisible ?? true) && Item != null)
            {
                if (wo is Player player)
                    player.LasUsedHookId = Guid;

                // redirect to item.ActOnUse
                Item.OnActivate(wo);

                return;
            }

            base.ActOnUse(wo);
        }

        protected override void OnInitialInventoryLoadCompleted()
        {
            var hidden = !(House.RootHouse.HouseHooksVisible ?? true);

            Ethereal = !HasItem;
            if (!HasItem)
            {
                NoDraw = hidden;
                UiHidden = hidden;
            }
            else
            {
                NoDraw = false;
                UiHidden = false;
            }

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
            Ethereal = false;

            SetupTableId = item.SetupTableId;
            MotionTableId = item.MotionTableId;
            PhysicsTableId = item.PhysicsTableId;
            SoundTableId = item.SoundTableId;
            ObjScale = item.ObjScale;
            Name = item.Name;

            if (MotionTableId != 0)
                CurrentMotionState = new Motion(MotionStance.Invalid);

            Placement = (Placement)(item.HookPlacement ?? (int)ACE.Entity.Enum.Placement.Hook);

            item.EmoteManager.SetProxy(this);

            House.HouseCurrentHooksUsable--;

            // Here we explicitly save the hook to the database to prevent item loss.
            // If the player adds an item to the hook, and the server crashes before the hook has been saved, the item will be lost.
            SaveBiotaToDatabase();

            EnqueueBroadcast(new GameMessageUpdateObject(this));
        }

        private static readonly ConcurrentDictionary<uint, WorldObject> cachedHookReferences = new ConcurrentDictionary<uint, WorldObject>();

        /// <summary>
        /// This event is raised when player removes item from hook
        /// </summary>
        protected override void OnRemoveItem(WorldObject removedItem)
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

            NoDraw = false;
            UiHidden = false;
            Ethereal = true;

            if (MotionTableId == 0)
                CurrentMotionState = null;

            removedItem.EmoteManager.ClearProxy();

            House.HouseCurrentHooksUsable++;

            EnqueueBroadcast(new GameMessageUpdateObject(this));

            // Here we explicitly save the storage to the database to prevent property desync.
            SaveBiotaToDatabase();
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

        public void UpdateHookVisibility()
        {
            if (!HasItem)
            {
                if (!(House.HouseHooksVisible ?? false))
                {
                    NoDraw = true;
                    UiHidden = true;
                    Ethereal = true;
                }
                else
                {
                    NoDraw = false;
                    UiHidden = false;
                    Ethereal = true;
                }

                EnqueueBroadcastPhysicsState();
                EnqueueBroadcast(new GameMessagePublicUpdatePropertyBool(this, PropertyBool.UiHidden, UiHidden));
            }
        }
    }
}
