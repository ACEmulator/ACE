using System;
using System.Collections.Generic;
using System.Linq;

using log4net;

using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Entity.Actions;
using ACE.Server.Factories;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Motion;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages;

namespace ACE.Server.WorldObjects
{
    public partial class Container : WorldObject
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public Container(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();

            // A player has their possessions passed via the ctor. All other world objects must load their own inventory
            if (!(this is Player) && !(new ObjectGuid(ContainerId ?? 0).IsPlayer()))
                DatabaseManager.Shard.GetInventory(guid.Full, false, SortBiotasIntoInventory);
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Container(Biota biota) : base(biota)
        {
            SetEphemeralValues();

            // A player has their possessions passed via the ctor. All other world objects must load their own inventory
            if (!(this is Player) && !(new ObjectGuid(ContainerId ?? 0).IsPlayer()))
                DatabaseManager.Shard.GetInventory(biota.Id, false, SortBiotasIntoInventory);
        }

        private void SetEphemeralValues()
        {
            EncumbranceVal = EncumbranceVal ?? 0; // Containers are init at 0 burden or their initial value from database. As inventory/equipment is added the burden will be increased
            Value = Value ?? 0;

            //CurrentMotionState = motionStateClosed; // What container defaults to open?

            if (UseRadius < 2)
                UseRadius = 2; // Until DoMoveTo (Physics, Indoor/Outside range variance) is smarter, use 2 is safest.

            GenerateContainList();
        }


        public bool InventoryLoaded { get; private set; }

        /// <summary>
        /// This will contain all main pack items, and all side slot items.<para />
        /// To access items inside of the side slot items, you'll need to access that items.Inventory dictionary.<para />
        /// Do not manipulate this dictionary directly.
        /// </summary>
        public Dictionary<ObjectGuid, WorldObject> Inventory { get; } = new Dictionary<ObjectGuid, WorldObject>();

        /// <summary>
        /// The only time this should be used is to populate Inventory from the ctor.
        /// </summary>
        protected void SortBiotasIntoInventory(IEnumerable<Biota> biotas)
        {
            var worldObjects = new List<WorldObject>();

            foreach (var biota in biotas)
                worldObjects.Add(WorldObjectFactory.CreateWorldObject(biota));

            SortWorldObjectsIntoInventory(worldObjects);

            if (worldObjects.Count > 0)
                log.Error("Inventory detected without a container to put it in to.");
        }

        /// <summary>
        /// The only time this should be used is to populate Inventory from the ctor.
        /// This will remove from worldObjects as they're sorted.
        /// </summary>
        private void SortWorldObjectsIntoInventory(IList<WorldObject> worldObjects)
        {
            // This will pull out all of our main pack items and side slot items (foci & containers)
            for (int i = worldObjects.Count - 1; i >= 0; i--)
            {
                if ((worldObjects[i].ContainerId ?? 0) == Biota.Id)
                {
                    Inventory[worldObjects[i].Guid] = worldObjects[i];
                    if (worldObjects[i].WeenieType != WeenieType.Container) // We skip over containers because we'll add their burden/value in the next loop.
                    {
                        EncumbranceVal += worldObjects[i].EncumbranceVal;
                        Value += worldObjects[i].Value;
                    }

                    worldObjects.RemoveAt(i);
                }
            }

            InventoryLoaded = true;

            // All that should be left are side pack sub contents.

            var sideContainers = Inventory.Values.Where(i => i.WeenieType == WeenieType.Container).ToList();
            foreach (var container in sideContainers)
            {
                ((Container)container).SortWorldObjectsIntoInventory(worldObjects); // This will set the InventoryLoaded flag for this sideContainer
                EncumbranceVal += container.EncumbranceVal; // This value includes the containers burden itself + all child items
                Value += container.Value; // This value includes the containers value itself + all child items
            }
        }

        /// <summary>
        /// This method will check all containers in our possession. Inventory in main or any packs or equipped.
        /// </summary>
        public bool HasInventoryItem(ObjectGuid objectGuid)
        {
            return GetInventoryItem(objectGuid) != null;
        }

        /// <summary>
        /// This method is used to get anything in our posession. Inventory in main or any packs or equipped.
        /// </summary>
        public WorldObject GetInventoryItem(ObjectGuid objectGuid)
        {
            return GetInventoryItem(objectGuid, out _);
        }

        /// <summary>
        /// This method is used to get anything in our posession. Inventory in main or any packs or equipped.
        /// </summary>
        public WorldObject GetInventoryItem(ObjectGuid objectGuid, out Container container)
        {
            // First search my wielded items..
            var wielded = (this as Player)?.GetWieldedItem(objectGuid);
            if (wielded != null)
            {
                container = this;
                return wielded;
            }

            // Next search my main pack for this item..
            if (Inventory.TryGetValue(objectGuid, out var value))
            {
                container = this;
                return value;
            }

            // Next search all containers for item.. run function again for each container.
            var sideContainers = Inventory.Values.Where(i => i.WeenieType == WeenieType.Container).ToList();
            foreach (var sideContainer in sideContainers)
            {
                var containerItem = ((Container)sideContainer).GetInventoryItem(objectGuid);

                if (containerItem != null)
                {
                    container = (Container)sideContainer;
                    return containerItem;
                }
            }

            var ammo = (this as Player)?.GetEquippedAmmo();
            if (ammo != null)
            {
                if (ammo.Guid == objectGuid)
                {
                    container = null;
                    return ammo;
                }
            }

            container = null;
            return null;
        }

        /// <summary>
        /// This method is used to get all inventory items of a type in this container (example of usage get all items of coin on player)
        /// </summary>
        public List<WorldObject> GetInventoryItemsOfTypeWeenieType(WeenieType type)
        {
            var items = new List<WorldObject>();

            // first search me / add all items of type.
            var localInventory = Inventory.Values.Where(wo => wo.WeenieType == type).ToList();

            items.AddRange(localInventory);

            // next search all containers for coin.. run function again for each container.
            var sideContainers = Inventory.Values.Where(i => i.WeenieType == WeenieType.Container).ToList();
            foreach (var container in sideContainers)
                items.AddRange(((Container)container).GetInventoryItemsOfTypeWeenieType(type));

            return items;
        }

        /// <summary>
        /// If enough burden is available, this will try to add an item to the main pack. If the main pack is full, it will try to add it to the first side pack with room.<para />
        /// It will also increase the EncumbranceVal and Value.
        /// </summary>
        public bool TryAddToInventory(WorldObject worldObject, int placementPosition = 0, bool limitToMainPackOnly = false)
        {
            return TryAddToInventory(worldObject, out _, placementPosition, limitToMainPackOnly);
        }

        /// <summary>
        /// If enough burden is available, this will try to add an item to the main pack. If the main pack is full, it will try to add it to the first side pack with room.<para />
        /// It will also increase the EncumbranceVal and Value.
        /// </summary>
        public bool TryAddToInventory(WorldObject worldObject, out Container container, int placementPosition = 0, bool limitToMainPackOnly = false)
        {
            if (this is Player player)
            {
                if (!player.HasEnoughBurdenToAddToInventory(worldObject))
                {
                    container = null;
                    return false;
                }
            }

            IList<WorldObject> containerItems;

            if (worldObject.UseBackpackSlot)
            {
                containerItems = Inventory.Values.Where(i => i.UseBackpackSlot).ToList();

                if ((ContainerCapacity ?? 0) <= containerItems.Count())
                {
                    container = null;
                    return false;
                }
            }
            else
            {
                containerItems = Inventory.Values.Where(i => !i.UseBackpackSlot).ToList();

                if ((ItemCapacity ?? 0) <= containerItems.Count())
                {
                    // Can we add this to any side pack?
                    if (!limitToMainPackOnly)
                    {
                        var containers = Inventory.Values.OfType<Container>().ToList();
                        containers.Sort((a, b) => (a.Placement ?? 0).CompareTo(b.Placement ?? 0));

                        foreach (var sidePack in containers)
                        {
                            if (sidePack.TryAddToInventory(worldObject, out container, placementPosition, true))
                            {
                                EncumbranceVal += worldObject.EncumbranceVal;
                                Value += worldObject.Value;

                                return true;
                            }
                        }
                    }

                    container = null;
                    return false;
                }
            }

            worldObject.OwnerId = Guid.Full;
            worldObject.ContainerId = Guid.Full;
            worldObject.PlacementPosition = placementPosition; // Server only variable that we use to remember/restore the order in which items exist in a container

            // Move all the existing items PlacementPosition over.
            if (!worldObject.UseBackpackSlot)
                containerItems.Where(i => !i.UseBackpackSlot && i.PlacementPosition >= placementPosition).ToList().ForEach(i => i.PlacementPosition++);
            else
                containerItems.Where(i => i.UseBackpackSlot && i.PlacementPosition >= placementPosition).ToList().ForEach(i => i.PlacementPosition++);

            Inventory.Add(worldObject.Guid, worldObject);

            EncumbranceVal += worldObject.EncumbranceVal;
            Value += worldObject.Value;

            container = this;
            return true;
        }

        /// <summary>
        /// This will clear the ContainerId and PlacementPosition properties.<para />
        /// It will also subtract the EncumbranceVal and Value.
        /// </summary>
        public bool TryRemoveFromInventory(ObjectGuid objectGuid)
        {
            return TryRemoveFromInventory(objectGuid, out _);
        }

        /// <summary>
        /// This will clear the ContainerId and PlacementPosition properties and remove the object from the Inventory dictionary.<para />
        /// It will also subtract the EncumbranceVal and Value.
        /// </summary>
        public bool TryRemoveFromInventory(ObjectGuid objectGuid, out WorldObject item)
        {
            // first search me / add all items of type.
            if (Inventory.Remove(objectGuid, out item))
            {
                int removedItemsPlacementPosition = item.PlacementPosition ?? 0;

                item.OwnerId = null;
                item.ContainerId = null;
                item.PlacementPosition = null;

                // Move all the existing items PlacementPosition over.
                if (!item.UseBackpackSlot)
                    Inventory.Values.Where(i => !i.UseBackpackSlot && i.PlacementPosition > removedItemsPlacementPosition).ToList().ForEach(i => i.PlacementPosition--);
                else
                    Inventory.Values.Where(i => i.UseBackpackSlot && i.PlacementPosition > removedItemsPlacementPosition).ToList().ForEach(i => i.PlacementPosition--);

                EncumbranceVal -= item.EncumbranceVal;
                Value -= item.Value;

                return true;
            }

            // next search all containers for item.. run function again for each container.
            var sideContainers = Inventory.Values.Where(i => i.WeenieType == WeenieType.Container).ToList();
            foreach (var container in sideContainers)
            {
                if (((Container)container).TryRemoveFromInventory(objectGuid, out item))
                {
                    EncumbranceVal -= item.EncumbranceVal;
                    Value -= item.Value;

                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// This is raised by Player.HandleActionUseItem, and is wrapped in ActionChain.<para />
        /// The actor of the ActionChain is the item being used.<para />
        /// The item does not exist in the players possession.<para />
        /// If the item was outside of range, the player will have been commanded to move using DoMoveTo before ActOnUse is called.<para />
        /// When this is called, it should be assumed that the player is within range.
        /// </summary>
        public override void ActOnUse(WorldObject worldObject)
        {
            if (worldObject is Player)
            {
                var player = worldObject as Player;
                if (!(IsOpen ?? false))
                {
                    var turnToMotion = new UniversalMotion(MotionStance.Standing, Location, Guid);
                    turnToMotion.MovementTypes = MovementTypes.TurnToObject;

                    var turnToTimer = new ActionChain();
                    turnToTimer.AddAction(this, () => player.CurrentLandblock.EnqueueBroadcastMotion(player, turnToMotion));
                    turnToTimer.AddDelaySeconds(1);
                    turnToTimer.AddAction(this, () => Open(player));
                    turnToTimer.EnqueueChain();

                    return;
                }

                if (Viewer == player.Guid.Full)
                    Close(player);

                // else error msg?

                player.SendUseDoneEvent();
            }
        }

        public virtual void Open(Player player)
        {
            if (IsOpen ?? false)
                return;

            IsOpen = true;

            Viewer = player.Guid.Full;

            DoOnOpenMotionChanges();

            player.Session.Network.EnqueueSend(new GameEventViewContents(player.Session, this));

            // send createobject for all objects in this container's inventory to player
            var itemsToSend = new List<GameMessage>();
            var woToExamine = new List<WorldObject>();

            foreach (var item in Inventory.Values)
            {
                itemsToSend.Add(new GameMessageCreateObject(item));
                woToExamine.Add(item);
            }

            player.Session.Network.EnqueueSend(itemsToSend.ToArray());
            player.TrackInteractiveObjects(woToExamine);

            player.SendUseDoneEvent();
        }

        protected virtual void DoOnOpenMotionChanges()
        {
        }

        public void Close(Player player)
        {
            if (!(IsOpen ?? false))
                return;

            player.Session.Network.EnqueueSend(new GameEventCloseGroundContainer(player.Session, this));

            // send deleteobject for all objects in this container's inventory to player
            var itemsToSend = new List<GameMessage>();

            foreach (var item in Inventory.Values)
                itemsToSend.Add(new GameMessageDeleteObject(item));

            player.Session.Network.EnqueueSend(itemsToSend.ToArray());

            DoOnCloseMotionChanges();

            Viewer = null;

            IsOpen = false;
        }

        protected virtual void DoOnCloseMotionChanges()
        {
        }

        public virtual void Reset()
        {
            // do reset stuff here
        }

        private void GenerateContainList()
        {
            foreach(var item in Biota.BiotaPropertiesCreateList.Where(x => x.DestinationType == (sbyte)DestinationType.Contain || x.DestinationType == (sbyte)DestinationType.ContainTreasure))
            {
                var wo = WorldObjectFactory.CreateNewWorldObject(item.WeenieClassId);

                if (wo == null)
                    continue;

                if (item.Palette > 0)
                    wo.PaletteTemplate = item.Palette;
                if (item.Shade > 0)
                    wo.Shade = item.Shade;
                if (item.StackSize > 1)
                    wo.StackSize = (ushort)item.StackSize;

                TryAddToInventory(wo);
            }
        }


        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************

        // Inventory Management Functions
        [Obsolete("This needs to be refactored into the new system")]
        protected void AddToInventory(WorldObject inventoryItem, int placement = 0)
        {
            AddToInventoryEx(inventoryItem, placement);

            //Burden += inventoryItem.Burden;
            log.Debug($"Add {inventoryItem.Name} in inventory, adding {inventoryItem.EncumbranceVal}, current EncumbranceVal = {EncumbranceVal}");

            Value += inventoryItem.Value;
        }

        /// <summary>
        /// Adds a new item to the inventory collection AND NOTHING ELSE.  will not send updates to the client.  The
        /// primary use case here is as a helper function or for adding items prior to login (ie, char gen)
        /// </summary>
        [Obsolete("This needs to be refactored into the new system")]
        private void AddToInventoryEx(WorldObject inventoryItem, int placement = 0)
        {
            //if (InventoryObjects.ContainsKey(inventoryItem.Guid))
            //{
            //    // if item exists in the list, we are going to shift everything greater than the moving item down 1 to reflect its removal
            //    if (inventoryItem.UseBackpackSlot)
            //        InventoryObjects.Where(i => InventoryObjects[inventoryItem.Guid].PlacementPosition != null &&
            //                             i.Value.PlacementPosition > (uint)InventoryObjects[inventoryItem.Guid].PlacementPosition &&
            //                             i.Value.UseBackpackSlot).ToList().ForEach(i => i.Value.PlacementPosition--);
            //    else
            //        InventoryObjects.Where(i => InventoryObjects[inventoryItem.Guid].PlacementPosition != null &&
            //                             i.Value.PlacementPosition > (uint)InventoryObjects[inventoryItem.Guid].PlacementPosition &&
            //                             !i.Value.UseBackpackSlot).ToList().ForEach(i => i.Value.PlacementPosition--);

            //    InventoryObjects.Remove(inventoryItem.Guid);
            //}
            //// If not going on the very end (next open slot), make a hole.
            //if (inventoryItem.UseBackpackSlot)
            //    InventoryObjects.Where(i => i.Value.PlacementPosition >= placement && i.Value.UseBackpackSlot).ToList().ForEach(i => i.Value.PlacementPosition++);
            //else
            //    InventoryObjects.Where(i => i.Value.PlacementPosition >= placement && !i.Value.UseBackpackSlot).ToList().ForEach(i => i.Value.PlacementPosition++);

            inventoryItem.PlacementPosition = placement;
            inventoryItem.Location = null;
            Inventory.Add(inventoryItem.Guid, inventoryItem);
        }
    }
}
