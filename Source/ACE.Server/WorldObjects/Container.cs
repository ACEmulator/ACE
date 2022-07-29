using System;
using System.Collections.Generic;
using System.Linq;

using log4net;

using ACE.Database;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;
using ACE.Server.Entity.Actions;
using ACE.Server.Factories;
using ACE.Server.Managers;
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
            InitializePropertyDictionaries();
            SetEphemeralValues(false);

            InventoryLoaded = true;
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Container(Biota biota) : base(biota)
        {
            if (Biota.TryRemoveProperty(PropertyBool.Open, BiotaDatabaseLock))
                ChangesDetected = true;

            // This is a temporary fix for objects that were loaded with this PR when EncumbranceVal was not treated as ephemeral. 2020-03-28
            // This can be removed later.
            if (Biota.PropertiesInt.ContainsKey(PropertyInt.EncumbranceVal))
            {
                var weenie = DatabaseManager.World.GetCachedWeenie(biota.WeenieClassId);

                if (weenie != null && weenie.PropertiesInt.TryGetValue(PropertyInt.EncumbranceVal, out var value))
                {
                    if (biota.PropertiesInt[PropertyInt.EncumbranceVal] != value)
                    {
                        biota.PropertiesInt[PropertyInt.EncumbranceVal] = value;
                        ChangesDetected = true;
                    }
                }
                else
                {
                    biota.PropertiesInt.Remove(PropertyInt.EncumbranceVal);
                    ChangesDetected = true;
                }
            }

            // This is a temporary fix for objects that were loaded with this PR when Value was not treated as ephemeral. 2020-03-28
            // This can be removed later.
            if (!(this is Creature) && Biota.PropertiesInt.ContainsKey(PropertyInt.Value))
            {
                var weenie = DatabaseManager.World.GetCachedWeenie(biota.WeenieClassId);

                if (weenie != null && weenie.PropertiesInt.TryGetValue(PropertyInt.Value, out var value))
                {
                    if (biota.PropertiesInt[PropertyInt.Value] != value)
                    {
                        biota.PropertiesInt[PropertyInt.Value] = value;
                        ChangesDetected = true;
                    }
                }
                else
                {
                    biota.PropertiesInt.Remove(PropertyInt.Value);
                    ChangesDetected = true;
                }
            }

            InitializePropertyDictionaries();
            SetEphemeralValues(true);

            // A player has their possessions passed via the ctor. All other world objects must load their own inventory
            if (!(this is Player) && !ObjectGuid.IsPlayer(ContainerId ?? 0))
            {
                DatabaseManager.Shard.GetInventoryInParallel(biota.Id, false, biotas =>
                {
                    EnqueueAction(new ActionEventDelegate(() => SortBiotasIntoInventory(biotas)));
                });
            }
        }

        private void InitializePropertyDictionaries()
        {
            if (ephemeralPropertyInts == null)
                ephemeralPropertyInts = new Dictionary<PropertyInt, int?>();
        }

        private void SetEphemeralValues(bool fromBiota)
        {
            ephemeralPropertyInts.TryAdd(PropertyInt.EncumbranceVal, EncumbranceVal ?? 0); // Containers are init at 0 burden or their initial value from database. As inventory/equipment is added the burden will be increased
            if (!(this is Creature) && !(this is Corpse)) // Creatures/Corpses do not have a value
                ephemeralPropertyInts.TryAdd(PropertyInt.Value, Value ?? 0);

            //CurrentMotionState = motionStateClosed; // What container defaults to open?

            if (!fromBiota && !(this is Creature))
                GenerateContainList();

            if (!ContainerCapacity.HasValue)
                ContainerCapacity = 0;

            if (!UseRadius.HasValue)
                UseRadius = 0.5f;

            IsOpen = false;
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
        protected void SortBiotasIntoInventory(IEnumerable<ACE.Database.Models.Shard.Biota> biotas)
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
                    worldObjects[i].Container = this;

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

        /// <summary>
        /// Counts the number of actual inventory items, ignoring Packs/Foci.
        /// </summary>
        private int CountPackItems()
        {
            return Inventory.Values.Count(wo => !wo.UseBackpackSlot);
        }

        /// <summary>
        /// Counts the number of containers in inventory, including Foci.
        /// </summary>
        private int CountContainers()
        {
            return Inventory.Values.Count(wo => wo.UseBackpackSlot);
        }

        public int GetFreeInventorySlots(bool includeSidePacks = true)
        {
            int freeSlots = (ItemCapacity ?? 0) - CountPackItems();

            if (includeSidePacks)
            {
                foreach (var sidePack in Inventory.Values.OfType<Container>())
                    freeSlots += (sidePack.ItemCapacity ?? 0) - sidePack.CountPackItems();
            }

            return freeSlots;
        }

        public int GetFreeContainerSlots()
        {
            return (ContainerCapacity ?? 0) - CountContainers();
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
            var localInventory = Inventory.Values.Where(wo => wo.WeenieType == type).OrderBy(i => i.PlacementPosition).ToList();

            items.AddRange(localInventory);

            // next search all containers for type.. run function again for each container.
            var sideContainers = Inventory.Values.Where(i => i.WeenieType == WeenieType.Container).OrderBy(i => i.PlacementPosition).ToList();
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
            var localInventory = Inventory.Values.Where(i => i.WeenieClassId == weenieClassId).OrderBy(i => i.PlacementPosition).ToList();

            items.AddRange(localInventory);

            // next search any side containers
            var sideContainers = Inventory.Values.Where(i => i.WeenieType == WeenieType.Container).Select(i => i as Container).OrderBy(i => i.PlacementPosition).ToList();
            foreach (var container in sideContainers)
                items.AddRange(container.GetInventoryItemsOfWCID(weenieClassId));

            return items;
        }

        /// <summary>
        /// Returns the total # of inventory items matching a wcid
        /// </summary>
        public int GetNumInventoryItemsOfWCID(uint weenieClassId)
        {
            return GetInventoryItemsOfWCID(weenieClassId).Select(i => i.StackSize ?? 1).Sum();
        }

        /// <summary>
        /// Returns the inventory items matching a weenie class name
        /// </summary>
        public List<WorldObject> GetInventoryItemsOfWeenieClass(string weenieClassName)
        {
            var items = new List<WorldObject>();

            // search main pack / creature
            var localInventory = Inventory.Values.Where(i => i.WeenieClassName.Equals(weenieClassName, StringComparison.OrdinalIgnoreCase)).OrderBy(i => i.PlacementPosition).ToList();

            items.AddRange(localInventory);

            // next search any side containers
            var sideContainers = Inventory.Values.Where(i => i.WeenieType == WeenieType.Container).Select(i => i as Container).OrderBy(i => i.PlacementPosition).ToList();
            foreach (var container in sideContainers)
                items.AddRange(container.GetInventoryItemsOfWeenieClass(weenieClassName));

            return items;
        }

        /// <summary>
        /// Returns the total # of inventory items matching a weenie class name
        /// </summary>
        public int GetNumInventoryItemsOfWeenieClass(string weenieClassName)
        {
            return GetInventoryItemsOfWeenieClass(weenieClassName).Select(i => i.StackSize ?? 1).Sum();
        }

        /// <summary>
        /// Returns all of the trade notes from inventory + side packs
        /// </summary>
        public List<WorldObject> GetTradeNotes()
        {
            // FIXME: search by classname performance
            var items = new List<WorldObject>();

            // search main pack / creature
            var localInventory = Inventory.Values.Where(i => i.WeenieClassName.StartsWith("tradenote")).OrderBy(i => i.PlacementPosition).ToList();

            items.AddRange(localInventory);

            // next search any side containers
            var sideContainers = Inventory.Values.Where(i => i.WeenieType == WeenieType.Container).Select(i => i as Container).OrderBy(i => i.PlacementPosition).ToList();
            foreach (var container in sideContainers)
                items.AddRange(container.GetTradeNotes());

            return items;
        }

        /// <summary>
        /// If enough burden is available, this will try to add an item to the main pack. If the main pack is full, it will try to add it to the first side pack with room.<para />
        /// It will also increase the EncumbranceVal and Value.
        /// </summary>
        public bool TryAddToInventory(WorldObject worldObject, int placementPosition = 0, bool limitToMainPackOnly = false, bool burdenCheck = true)
        {
            if (worldObject == null) return false;

            return TryAddToInventory(worldObject, out _, placementPosition, limitToMainPackOnly, burdenCheck);
        }

        /// <summary>
        /// Returns TRUE if there are enough free inventory slots and burden available to add items
        /// </summary>
        public bool CanAddToInventory(int totalContainerObjectsToAdd, int totalInventoryObjectsToAdd, int totalBurdenToAdd)
        {
            if (this is Player player && !player.HasEnoughBurdenToAddToInventory(totalBurdenToAdd))
                return false;

            return (GetFreeContainerSlots() >= totalContainerObjectsToAdd) && (GetFreeInventorySlots() >= totalInventoryObjectsToAdd);
        }

        /// <summary>
        /// Returns TRUE if there are enough free inventory slots and burden available to add item
        /// </summary>
        public bool CanAddToInventory(WorldObject worldObject)
        {
            if (this is Player player && !player.HasEnoughBurdenToAddToInventory(worldObject))
                return false;

            if (worldObject.UseBackpackSlot)
                return GetFreeContainerSlots() > 0;
            else
                return GetFreeInventorySlots() > 0;
        }

        /// <summary>
        /// Returns TRUE if there are enough free inventory slots and burden available to add all items
        /// </summary>
        public bool CanAddToInventory(List<WorldObject> worldObjects)
        {
            return CanAddToInventory(worldObjects, out _, out _);
        }

        /// <summary>
        /// Returns TRUE if there are enough free inventory slots and burden available to add all items
        /// </summary>
        public bool CanAddToInventory(List<WorldObject> worldObjects, out bool TooEncumbered, out bool NotEnoughFreeSlots)
        {
            TooEncumbered = false;
            NotEnoughFreeSlots = false;

            if (worldObjects.Count == 0) // There are no objects to add (e.g. 1 way trade)
                return true;

            if (this is Player player && !player.HasEnoughBurdenToAddToInventory(worldObjects))
            {
                TooEncumbered = true;
                return false;
            }

            var containers = worldObjects.Where(w => w.UseBackpackSlot).ToList();
            if (containers.Count > 0)
            {
                if (GetFreeContainerSlots() < containers.Count)
                {
                    NotEnoughFreeSlots = true;
                    return false;
                }
            }

            if (GetFreeInventorySlots() < (worldObjects.Count - containers.Count))
            {
                NotEnoughFreeSlots = true;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Returns TRUE if there are enough free inventory slots and burden available to add item
        /// </summary>
        public bool CanAddToContainer(WorldObject worldObject, bool includeSidePacks = true)
        {
            if (this is Player player && !player.HasEnoughBurdenToAddToInventory(worldObject))
                return false;

            if (worldObject.UseBackpackSlot)
                return GetFreeContainerSlots() > 0;
            else
                return GetFreeInventorySlots(includeSidePacks) > 0;
        }

        /// <summary>
        /// Returns TRUE if there are enough free burden available to merge item and merge target will not exceed maximum stack size
        /// </summary>
        public bool CanMergeToInventory(WorldObject worldObject, WorldObject mergeTarget, int mergeAmout)
        {
            if (this is Player player && !player.HasEnoughBurdenToAddToInventory(worldObject))
                return false;

            var currentStackSize = mergeTarget.StackSize;
            var maxStackSize = mergeTarget.MaxStackSize;
            var newStackSize = currentStackSize + mergeAmout;

            return newStackSize <= maxStackSize;
        }

        /// <summary>
        /// If enough burden is available, this will try to add an item to the main pack. If the main pack is full, it will try to add it to the first side pack with room.<para />
        /// It will also increase the EncumbranceVal and Value.
        /// </summary>
        public bool TryAddToInventory(WorldObject worldObject, out Container container, int placementPosition = 0, bool limitToMainPackOnly = false, bool burdenCheck = true)
        {
            // bug: should be root owner
            if (this is Player player && burdenCheck)
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

            if (Inventory.ContainsKey(worldObject.Guid))
            {
                container = null;
                return false;
            }

            worldObject.Location = null;
            worldObject.Placement = ACE.Entity.Enum.Placement.Resting;

            worldObject.OwnerId = Guid.Full;
            worldObject.ContainerId = Guid.Full;
            worldObject.Container = this;
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
                if (!TryRemoveFromInventory(itemGuid, out var item, forceSave))
                    success = false;

                if (success)
                    item.Destroy();
            }
            if (forceSave)
                SaveBiotaToDatabase();

            return success;
        }

        /// <summary>
        /// Removes all items from an inventory that are unmanaged/controlled
        /// </summary>
        /// <returns>TRUE if all unmanaged items were removed successfully</returns>
        public bool ClearUnmanagedInventory(bool forceSave = false)
        {
            if (this is Storage || WeenieClassId == (uint)ACE.Entity.Enum.WeenieClassName.W_STORAGE_CLASS)
                return false; // Do not clear storage, ever.

            var success = true;
            var itemGuids = Inventory.Where(i => i.Value.GeneratorId == null).Select(i => i.Key).ToList();
            foreach (var itemGuid in itemGuids)
            {
                if (!TryRemoveFromInventory(itemGuid, out var item, forceSave))
                    success = false;

                if (success)
                    item.Destroy();
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
                item.Container = null;
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

                OnRemoveItem(item);

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

            // If we have a previous container open, let's close it
            if (player.LastOpenedContainerId != ObjectGuid.Invalid && player.LastOpenedContainerId != Guid)
            {
                var lastOpenedContainer = CurrentLandblock?.GetObject(player.LastOpenedContainerId) as Container;

                if (lastOpenedContainer != null && lastOpenedContainer.IsOpen && lastOpenedContainer.Viewer == player.Guid.Full)
                    lastOpenedContainer.Close(player);
            }

            if ((OwnerId.HasValue && OwnerId.Value > 0) || (ContainerId.HasValue && ContainerId.Value > 0))
                return; // Do nothing else if container is owned by something.

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
                else
                    player.SendTransientError(InUseMessage);
            }
        }

        public string InUseMessage
        {
            get
            {
                // verified this message was sent for corpses, instead of WeenieErrorWithString.The_IsCurrentlyInUse
                var currentViewer = "someone else";

                if (PropertyManager.GetBool("container_opener_name").Item)
                {
                    var name = CurrentLandblock?.GetObject(Viewer)?.Name;
                    if (name != null)
                        currentViewer = name;
                }
                return $"The {Name} is already in use by {currentViewer}!";
            }
        }

        public virtual void Open(Player player)
        {
            if (IsOpen)
            {
                player.SendTransientError(InUseMessage);
                return;
            }

            player.LastOpenedContainerId = Guid;

            IsOpen = true;

            Viewer = player.Guid.Full;

            DoOnOpenMotionChanges();

            SendInventory(player);

            if (!(this is Chest) && !ResetMessagePending && ResetInterval.HasValue)
            {
                var actionChain = new ActionChain();
                if (ResetInterval.Value < 15)
                    actionChain.AddDelaySeconds(15);
                else
                    actionChain.AddDelaySeconds(ResetInterval.Value);
                actionChain.AddAction(this, Reset);
                //actionChain.AddAction(this, () =>
                //{
                //    Close(player);
                //});
                actionChain.EnqueueChain();

                ResetMessagePending = true;
            }
        }

        protected virtual float DoOnOpenMotionChanges()
        {
            return 0;
        }

        private void SendInventory(Player player)
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

            player.Session.Network.EnqueueSend(new GameEventViewContents(player.Session, this));

            // send sub-containers
            foreach (var container in Inventory.Values.Where(i => i is Container))
                player.Session.Network.EnqueueSend(new GameEventViewContents(player.Session, (Container)container));

            player.Session.Network.EnqueueSend(itemsToSend.ToArray());
        }

        private void SendDeletesForMyInventory(Player player)
        {
            // send deleteobjects for all objects in this container's inventory to player
            var itemsToSend = new List<GameMessage>();

            foreach (var item in Inventory.Values)
            {
                // FIXME: only send messages for known objects
                itemsToSend.Add(new GameMessageDeleteObject(item));

                if (item is Container container)
                {
                    foreach (var containerItem in container.Inventory.Values)
                        itemsToSend.Add(new GameMessageDeleteObject(containerItem));
                }
            }

            player.Session.Network.EnqueueSend(itemsToSend.ToArray());
        }

        public virtual void Close(Player player)
        {
            if (!IsOpen) return;

            var animTime = DoOnCloseMotionChanges();

            if (animTime <= 0)
                FinishClose(player);
            else
            {
                var actionChain = new ActionChain();
                actionChain.AddDelaySeconds(animTime / 2.0f);
                actionChain.AddAction(this, () => FinishClose(player));
                actionChain.EnqueueChain();
            }
        }

        protected virtual float DoOnCloseMotionChanges()
        {
            return 0;
        }

        public virtual void FinishClose(Player player)
        {
            IsOpen = false;
            Viewer = 0;

            if (player != null)
            {
                player.Session.Network.EnqueueSend(new GameEventCloseGroundContainer(player.Session, this));

                if (player.LastOpenedContainerId == Guid)
                    player.LastOpenedContainerId = ObjectGuid.Invalid;

                // send deleteobject for all objects in this container's inventory to player
                // this seems logical, but it bugs out the client for re-opening chests w/ respawned items
                /*var itemsToSend = new List<GameMessage>();

                foreach (var item in Inventory.Values)
                    itemsToSend.Add(new GameMessageDeleteObject(item));

                player.Session.Network.EnqueueSend(itemsToSend.ToArray());*/
            }

        }

        public virtual void Reset()
        {
            var player = CurrentLandblock.GetObject(Viewer) as Player;

            if (IsOpen)
                Close(player);

            //if (IsGenerator)
            //{
            //    ResetGenerator();
            //    if (InitCreate > 0)
            //        Generator_Regeneration();
            //}

            ClearUnmanagedInventory();

            ResetMessagePending = false;
        }

        public void GenerateContainList()
        {
            if (Biota.PropertiesCreateList == null)
                return;

            foreach (var item in Biota.PropertiesCreateList.Where(x => x.DestinationType == DestinationType.Contain || x.DestinationType == DestinationType.ContainTreasure))
            {
                var wo = WorldObjectFactory.CreateNewWorldObject(item.WeenieClassId);

                if (wo == null)
                    continue;

                if (!Guid.IsPlayer())
                    wo.GeneratorId = Guid.Full; // add this to mark item as "managed" so container resets don't delete it.

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
                        if (!sourceItem.IsDestroyed)
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
        protected virtual void OnRemoveItem(WorldObject worldObject)
        {
            // empty base
        }

        public virtual MotionCommand MotionPickup => MotionCommand.Pickup;

        public override bool IsAttunedOrContainsAttuned => base.IsAttunedOrContainsAttuned || Inventory.Values.Any(i => i.IsAttunedOrContainsAttuned);

        public override bool IsStickyAttunedOrContainsStickyAttuned => base.IsStickyAttunedOrContainsStickyAttuned || Inventory.Values.Any(i => i.IsStickyAttunedOrContainsStickyAttuned);

        public override bool IsUniqueOrContainsUnique => base.IsUniqueOrContainsUnique || Inventory.Values.Any(i => i.IsUniqueOrContainsUnique);

        public override bool IsBeingTradedOrContainsItemBeingTraded(HashSet<ObjectGuid> guidList) => base.IsBeingTradedOrContainsItemBeingTraded(guidList) || Inventory.Values.Any(i => i.IsBeingTradedOrContainsItemBeingTraded(guidList));

        public override List<WorldObject> GetUniqueObjects()
        {
            var uniqueObjects = new List<WorldObject>();

            if (Unique != null)
                uniqueObjects.Add(this);

            foreach (var item in Inventory.Values)
                uniqueObjects.AddRange(item.GetUniqueObjects());

            return uniqueObjects;
        }

        public override void OnTalk(WorldObject activator)
        {
            if (activator is Player player)
            {
                if (IsOpen)
                    player.Session.Network.EnqueueSend(new GameMessageSystemChat(ActivationTalk, ChatMessageType.Broadcast));
            }
        }
    }
}
