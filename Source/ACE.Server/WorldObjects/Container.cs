using System;
using System.Collections.Generic;
using System.Linq;

using log4net;

using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity.Actions;
using ACE.Server.Factories;
using ACE.Server.Network.GameMessages.Messages;
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
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Container(Biota biota) : base(biota)
        {
            SetEphemeralValues();

            // A player has their possessions passed via the ctor. All other world objects must load their own inventory
            if (!(this is Player) && !ObjectGuid.IsPlayer(ContainerId ?? 0))
            {
                DatabaseManager.Shard.GetInventoryInParallel(biota.Id, false, biotas =>
                {
                    EnqueueAction(new ActionEventDelegate(() => SortBiotasIntoInventory(biotas)));
                });
            }
        }

        private void SetEphemeralValues()
        {
            ephemeralPropertyInts.TryAdd(PropertyInt.EncumbranceVal, EncumbranceVal ?? 0); // Containers are init at 0 burden or their initial value from database. As inventory/equipment is added the burden will be increased
            if (!(this is Creature)) // Creatures do not have a value
                ephemeralPropertyInts.TryAdd(PropertyInt.Value, Value ?? 0);

            //CurrentMotionState = motionStateClosed; // What container defaults to open?

            var creature = this as Creature;
            if (creature == null)
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
                        EncumbranceVal += (worldObjects[i].EncumbranceVal ?? 0);
                        Value += (worldObjects[i].Value ?? 0);
                    }

                    worldObjects.RemoveAt(i);
                }
            }

            // Make sure placement positions are correct. They could get out of sync from a client issue, server issue, or orphaned biota
            var mainPackItems = Inventory.Values.Where(wo => !wo.UseBackpackSlot).OrderBy(wo => wo.PlacementPosition).ToList();
            for (int i = 0; i < mainPackItems.Count; i++)
                mainPackItems[i].PlacementPosition = i;
            var sidPackItems = Inventory.Values.Where(wo => wo.UseBackpackSlot).OrderBy(wo => wo.PlacementPosition).ToList();
            for (int i = 0; i < sidPackItems.Count; i++)
                sidPackItems[i].PlacementPosition = i;

            InventoryLoaded = true;

            // All that should be left are side pack sub contents.

            var sideContainers = Inventory.Values.Where(i => i.WeenieType == WeenieType.Container).ToList();
            foreach (var container in sideContainers)
            {
                ((Container)container).SortWorldObjectsIntoInventory(worldObjects); // This will set the InventoryLoaded flag for this sideContainer
                EncumbranceVal += container.EncumbranceVal; // This value includes the containers burden itself + all child items
                Value += container.Value; // This value includes the containers value itself + all child items
            }

            OnInitialInventoryLoadCompleted();
        }

        public int GetFreeInventorySlots(bool includeSidePacks = true)
        {
            int freeSlots = (ItemCapacity ?? 0) - Inventory.Count;

            if (includeSidePacks)
            {
                foreach (var sidePack in Inventory.Values.OfType<Container>())
                    freeSlots += (sidePack.ItemCapacity ?? 0) - sidePack.Inventory.Count;
            }

            return freeSlots;
        }

        /// <summary>
        /// This method will check all containers in our possession
        /// in main inventory or any side packs
        /// </summary>
        public bool HasInventoryItem(ObjectGuid objectGuid)
        {
            return GetInventoryItem(objectGuid) != null;
        }

        /// <summary>
        /// This method will check all containers in our possession
        /// in main inventory or any side packs
        /// </summary>
        public WorldObject GetInventoryItem(ObjectGuid objectGuid)
        {
            return GetInventoryItem(objectGuid, out _);
        }

        /// <summary>
        /// This method will check all containers in our possession
        /// in main inventory or any side packs
        /// </summary>
        public WorldObject GetInventoryItem(uint objectGuid)
        {
            return GetInventoryItem(new ObjectGuid(objectGuid), out _); // todo remove this so it doesnt' create a new ObjectGuid
        }

        /// <summary>
        /// This method will check all containers in our possession
        /// in main inventory or any side packs
        /// </summary>
        public WorldObject GetInventoryItem(ObjectGuid objectGuid, out Container container)
        {
            // First search my main pack for this item..
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

            // next search all containers for type.. run function again for each container.
            var sideContainers = Inventory.Values.Where(i => i.WeenieType == WeenieType.Container).ToList();
            foreach (var container in sideContainers)
                items.AddRange(((Container)container).GetInventoryItemsOfTypeWeenieType(type));

            return items;
        }

        /// <summary>
        /// Returns the inventory items matching a weenie class id
        /// </summary>
        public List<WorldObject> GetInventoryItemsOfWCID(uint weenieClassId)
        {
            var items = new List<WorldObject>();

            // search main pack / creature
            var localInventory = Inventory.Values.Where(i => i.WeenieClassId == weenieClassId).ToList();

            items.AddRange(localInventory);

            // next search any side containers
            var sideContainers = Inventory.Values.Where(i => i.WeenieType == WeenieType.Container).Select(i => i as Container).ToList();
            foreach (var container in sideContainers)
                items.AddRange(container.GetInventoryItemsOfWCID(weenieClassId));

            return items;
        }

        /// <summary>
        /// Returns all of the trade notes from inventory + side packs
        /// </summary>
        public List<WorldObject> GetTradeNotes()
        {
            // FIXME: search by classname performance
            var items = new List<WorldObject>();

            // search main pack / creature
            var localInventory = Inventory.Values.Where(i => i.WeenieClassName.StartsWith("tradenote")).ToList();

            items.AddRange(localInventory);

            // next search any side containers
            var sideContainers = Inventory.Values.Where(i => i.WeenieType == WeenieType.Container).Select(i => i as Container).ToList();
            foreach (var container in sideContainers)
                items.AddRange(container.GetTradeNotes());

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

                if ((ContainerCapacity ?? 0) <= containerItems.Count)
                {
                    container = null;
                    return false;
                }
            }
            else
            {
                containerItems = Inventory.Values.Where(i => !i.UseBackpackSlot).ToList();

                if ((ItemCapacity ?? 0) <= containerItems.Count)
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
                                EncumbranceVal += (worldObject.EncumbranceVal ?? 0);
                                Value += (worldObject.Value ?? 0);

                                return true;
                            }
                        }
                    }

                    container = null;
                    return false;
                }
            }

            worldObject.Location = null;
            worldObject.Placement = null;

            worldObject.OwnerId = Guid.Full;
            worldObject.ContainerId = Guid.Full;
            worldObject.PlacementPosition = placementPosition; // Server only variable that we use to remember/restore the order in which items exist in a container

            // Move all the existing items PlacementPosition over.
            if (!worldObject.UseBackpackSlot)
                containerItems.Where(i => !i.UseBackpackSlot && i.PlacementPosition >= placementPosition).ToList().ForEach(i => i.PlacementPosition++);
            else
                containerItems.Where(i => i.UseBackpackSlot && i.PlacementPosition >= placementPosition).ToList().ForEach(i => i.PlacementPosition++);

            Inventory.Add(worldObject.Guid, worldObject);

            EncumbranceVal += (worldObject.EncumbranceVal ?? 0);
            Value += (worldObject.Value ?? 0);

            container = this;

            OnAddItem();

            return true;
        }

        /// <summary>
        /// Removes all items from an inventory
        /// </summary>
        /// <returns>TRUE if all items were removed successfully</returns>
        public bool ClearInventory(bool forceSave = false)
        {
            var success = true;
            var itemGuids = Inventory.Keys.ToList();
            foreach (var itemGuid in itemGuids)
            {
                if (!TryRemoveFromInventory(itemGuid, forceSave))
                    success = false;
            }
            if (forceSave)
                SaveBiotaToDatabase();

            return success;
        }

        /// <summary>
        /// This will clear the ContainerId and PlacementPosition properties.<para />
        /// It will also subtract the EncumbranceVal and Value.
        /// </summary>
        public bool TryRemoveFromInventory(ObjectGuid objectGuid, bool forceSave = false)
        {
            return TryRemoveFromInventory(objectGuid, out _, forceSave);
        }

        /// <summary>
        /// This will clear the ContainerId and PlacementPosition properties and remove the object from the Inventory dictionary.<para />
        /// It will also subtract the EncumbranceVal and Value.
        /// </summary>
        public bool TryRemoveFromInventory(ObjectGuid objectGuid, out WorldObject item, bool forceSave = false)
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

                EncumbranceVal -= (item.EncumbranceVal ?? 0);
                Value -= (item.Value ?? 0);

                if (forceSave)
                    item.SaveBiotaToDatabase();

                OnRemoveItem();

                return true;
            }

            // next search all containers for item.. run function again for each container.
            var sideContainers = Inventory.Values.Where(i => i.WeenieType == WeenieType.Container).ToList();
            foreach (var container in sideContainers)
            {
                if (((Container)container).TryRemoveFromInventory(objectGuid, out item))
                {
                    EncumbranceVal -= (item.EncumbranceVal ?? 0);
                    Value -= (item.Value ?? 0);

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// This is raised by Player.HandleActionUseItem.<para />
        /// The item does not exist in the players possession.<para />
        /// If the item was outside of range, the player will have been commanded to move using DoMoveTo before ActOnUse is called.<para />
        /// When this is called, it should be assumed that the player is within range.
        /// </summary>
        public override void ActOnUse(WorldObject wo)
        {
            if (!(wo is Player player))
                return;

            if (!IsOpen)
            {
                Open(player);
            }
            else
            {
                if (Viewer == 0)
                    Close(null);
                else if (Viewer == player.Guid.Full)
                    Close(player);

                // else error msg?
            }
        }

        public virtual void Open(Player player)
        {
            if (IsOpen) return;

            IsOpen = true;

            Viewer = player.Guid.Full;

            DoOnOpenMotionChanges();

            SendInventory(player);
        }

        public void SendInventory(Player player)
        {
            // send createobject for all objects in this container's inventory to player
            var itemsToSend = new List<GameMessage>();

            foreach (var item in Inventory.Values)
            {
                // FIXME: only send messages for unknown objects
                itemsToSend.Add(new GameMessageCreateObject(item));

                if (item is Container container)
                {
                    foreach (var containerItem in container.Inventory.Values)
                        itemsToSend.Add(new GameMessageCreateObject(containerItem));
                }
            }

            player.Session.Network.EnqueueSend(itemsToSend.ToArray());

            player.Session.Network.EnqueueSend(new GameEventViewContents(player.Session, this));

            // send sub-containers
            foreach (var container in Inventory.Values.Where(i => i is Container))
                player.Session.Network.EnqueueSend(new GameEventViewContents(player.Session, (Container)container));
        }

        protected virtual float DoOnOpenMotionChanges()
        {
            return 0;
        }

        public virtual void Close(Player player)
        {
            if (!IsOpen) return;

            var animTime = DoOnCloseMotionChanges();
            var actionChain = new ActionChain();
            actionChain.AddDelaySeconds(animTime / 2.0f);
            actionChain.AddAction(this, () =>
            {
                IsOpen = false;
                Viewer = 0;

                if (player != null)
                {
                    player.Session.Network.EnqueueSend(new GameEventCloseGroundContainer(player.Session, this));

                    if (player.LastUsedContainerId == Guid)
                        player.LastUsedContainerId = ObjectGuid.Invalid;

                    // send deleteobject for all objects in this container's inventory to player
                    // this seems logical, but it bugs out the client for re-opening chests w/ respawned items
                    /*var itemsToSend = new List<GameMessage>();

                    foreach (var item in Inventory.Values)
                        itemsToSend.Add(new GameMessageDeleteObject(item));

                    player.Session.Network.EnqueueSend(itemsToSend.ToArray());*/
                }
            });
            actionChain.EnqueueChain();
        }

        protected virtual float DoOnCloseMotionChanges()
        {
            return 0;
        }

        public virtual void Reset()
        {
            // do reset stuff here
        }

        private void GenerateContainList()
        {
            foreach (var item in Biota.BiotaPropertiesCreateList.Where(x => x.DestinationType == (sbyte)DestinationType.Contain || x.DestinationType == (sbyte)DestinationType.ContainTreasure))
            {
                var wo = WorldObjectFactory.CreateNewWorldObject(item.WeenieClassId);

                if (wo == null)
                    continue;

                if (item.Palette > 0)
                    wo.PaletteTemplate = item.Palette;
                if (item.Shade > 0)
                    wo.Shade = item.Shade;
                if (item.StackSize > 1)
                    wo.SetStackSize(item.StackSize);

                TryAddToInventory(wo);
            }
        }

        public void MergeAllStackables()
        {
            var inventory = Inventory.Values.ToList();

            for (int i = inventory.Count - 1; i > 0; i--)
            {
                var sourceItem = inventory[i];

                if (sourceItem.MaxStackSize == null || sourceItem.MaxStackSize <= 1)
                    continue;

                for (int j = 0; j < i; j++)
                {
                    var destinationItem = inventory[j];

                    if (destinationItem.WeenieClassId != sourceItem.WeenieClassId || destinationItem.StackSize == destinationItem.MaxStackSize)
                        continue;

                    var amount = Math.Min(sourceItem.StackSize ?? 0, (destinationItem.MaxStackSize - destinationItem.StackSize) ?? 0);

                    sourceItem.SetStackSize(sourceItem.StackSize - amount);

                    destinationItem.SetStackSize(destinationItem.StackSize + amount);

                    if (sourceItem.StackSize == 0)
                    {
                        TryRemoveFromInventory(sourceItem.Guid);
                        sourceItem.Destroy();
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// This event is raised after the containers items have been completely loaded from the database
        /// </summary>
        protected virtual void OnInitialInventoryLoadCompleted()
        {
            // empty base
        }

        /// <summary>
        /// This event is raised when player adds item to container
        /// </summary>
        protected virtual void OnAddItem()
        {
            // empty base
        }

        /// <summary>
        /// This event is raised when player removes item from container
        /// </summary>
        protected virtual void OnRemoveItem()
        {
            // empty base
        }

        public virtual MotionCommand MotionPickup => MotionCommand.Pickup;
    }
}
