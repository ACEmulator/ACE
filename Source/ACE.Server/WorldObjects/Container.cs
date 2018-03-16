using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using ACE.Server.Network;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    public class Container : WorldObject
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public Container(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();

            InventoryLoaded = true;
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Container(Biota biota) : base(biota)
        {
            SetEphemeralValues();

            // A player has their possessions passed via the ctor. All other world objects must load their own inventory
            if (!(this is Player))
                DatabaseManager.Shard.GetInventory(biota.Id, true, SortBiotasIntoInventory);
        }

        private void SetEphemeralValues()
        {
            EncumbranceVal = EncumbranceVal ?? 0; // Containers are init at 0 burden or their initial value from database. As inventory/equipment is added the burden will be increased
            Value = Value ?? 0;
            // todo CoinValue
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
        /// This method will check all containers in our possession. Inventory in main or any packs,
        /// </summary>
        public bool HasInventoryItem(ObjectGuid objectGuid)
        {
            return GetInventoryItem(objectGuid) != null;
        }

        /// <summary>
        /// This method is used to get anything in our posession. Inventory in main or any packs,
        /// </summary>
        public WorldObject GetInventoryItem(ObjectGuid objectGuid)
        {
            return GetInventoryItem(objectGuid, out _);
        }

        /// <summary>
        /// This method is used to get anything in our posession. Inventory in main or any packs,
        /// </summary>
        public WorldObject GetInventoryItem(ObjectGuid objectGuid, out Container container)
        {
            // First search me for this item..
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

            // next search all containers for coin.. run function again for each container.
            var sideContainers = Inventory.Values.Where(i => i.WeenieType == WeenieType.Container).ToList();
            foreach (var container in sideContainers)
                items.AddRange(((Container)container).GetInventoryItemsOfTypeWeenieType(type));

            return items;
        }

        /// <summary>
        /// If enough burden is available, this will try to add an item to the main pack. If the main pack is full, it will try to add it to the first side pack with room.
        /// </summary>
        public bool TryAddToInventory(WorldObject worldObject, int placementPosition = 0, bool limitToMainPackOnly = false)
        {
            return TryAddToInventory(worldObject, out _, placementPosition, limitToMainPackOnly);
        }

        /// <summary>
        /// If enough burden is available, this will try to add an item to the main pack. If the main pack is full, it will try to add it to the first side pack with room.
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
                                // todo CoinValue

                                return true;
                            }
                        }
                    }

                    container = null;
                    return false;
                }
            }

            worldObject.ContainerId = Guid.Full;
            worldObject.PlacementPosition = placementPosition; // Server only variable that we use to remember/restore the order in which items exist in a container

            // Move all the existing items PlacementPosition over.
            containerItems.Where(i => i.PlacementPosition >= worldObject.PlacementPosition).ToList().ForEach(i => i.PlacementPosition++);

            Inventory.Add(worldObject.Guid, worldObject);

            EncumbranceVal += worldObject.EncumbranceVal;
            Value += worldObject.Value;
            // todo CoinValue

            container = this;
            return true;
        }

        public bool TryRemoveFromInventory(ObjectGuid objectGuid)
        {
            return TryRemoveFromInventory(objectGuid, out _);
        }

        public bool TryRemoveFromInventory(ObjectGuid objectGuid, out WorldObject item)
        {
            // first search me / add all items of type.
            if (Inventory.Remove(objectGuid, out item))
            {
                item.ContainerId = null;
                item.PlacementPosition = null;

                // Move all the existing items PlacementPosition over.
                int placement = item.PlacementPosition ?? 0;
                Inventory.Where(i => i.Value.PlacementPosition > placement).ToList().ForEach(i => --i.Value.PlacementPosition);

                EncumbranceVal -= item.EncumbranceVal;
                Value -= item.Value;
                // todo CoinValue
                //if (item.WeenieType == WeenieType.Coin || item.WeenieType == WeenieType.Container)
                //    UpdateCurrencyClientCalculations(WeenieType.Coin);

                item.SaveBiotaToDatabase();

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
                    // todo CoinValue

                    return true;
                }
            }

            return false;
        }









        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************

        // Inventory Management Functions
        public void AddToInventory(WorldObject inventoryItem, int placement = 0)
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
        public void AddToInventoryEx(WorldObject inventoryItem, int placement = 0)
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






        /// <summary>
        /// This method handles the first part of the merge - split out for code reuse.  It calculates
        /// the updated values for stack size, value and burden, creates the needed client messages
        /// and sends them.   This must be called from within an action chain. Og II
        /// </summary>
        /// <param name="session">Session is used for sequence and target</param>
        /// <param name="fromWo">World object of the item are we merging from</param>
        /// <param name="toWo">World object of the item we are merging into</param>
        /// <param name="amount">How many are we merging fromWo into the toWo</param>
        public void UpdateToStack(Session session, WorldObject fromWo, WorldObject toWo, int amount)
        {
            // unless we have a data issue, these are valid asserts Og II
            Debug.Assert(toWo.Value != null, "toWo.Value != null");
            Debug.Assert(fromWo.Value != null, "fromWo.Value != null");
            Debug.Assert(toWo.StackSize != null, "toWo.StackSize != null");
            Debug.Assert(fromWo.StackSize != null, "fromWo.StackSize != null");
            Debug.Assert(toWo.EncumbranceVal != null, "toWo.EncumbranceVal != null");
            Debug.Assert(fromWo.EncumbranceVal != null, "fromWo.EncumbranceVal != null");

            int newValue = (int)(toWo.Value + ((fromWo.Value / fromWo.StackSize) * amount));
            uint newBurden = (uint)(toWo.EncumbranceVal + ((fromWo.EncumbranceVal / fromWo.StackSize) * amount));

            int oldStackSize = (int)toWo.StackSize;
            toWo.StackSize += (ushort)amount;
            toWo.Value = newValue;
            toWo.EncumbranceVal = (int)newBurden;

            // Build the needed messages to the client.
            GameMessagePrivateUpdatePropertyInt msgUpdateValue = new GameMessagePrivateUpdatePropertyInt(toWo.Sequences, PropertyInt.Value, newValue);
            GameMessagePutObjectInContainer msgPutObjectInContainer = new GameMessagePutObjectInContainer(session, Guid, toWo, toWo.PlacementPosition ?? 0);
            Debug.Assert(toWo.StackSize != null, "toWo.StackSize != null");
            GameMessageSetStackSize msgAdjustNewStackSize = new GameMessageSetStackSize(toWo.Sequences, toWo.Guid, (int)toWo.StackSize, oldStackSize);

            CurrentLandblock.EnqueueBroadcast(Location, MaxObjectTrackingRange, msgUpdateValue, msgPutObjectInContainer, msgAdjustNewStackSize);
        }

        /// <summary>
        /// This method handles the second part of the merge if we have not merged ALL of the fromWo into the toWo - split out for code reuse.  It calculates
        /// the updated values for stack size, value and burden, creates the needed client messages
        /// and sends them.   This must be called from within an action chain. Og II
        /// </summary>
        /// <param name="session">Session is used for sequence and target</param>
        /// <param name="fromWo">World object of the item are we merging from</param>
        /// <param name="amount">How many are we merging fromWo into the toWo</param>
        public void UpdateFromStack(Session session, WorldObject fromWo,  int amount)
        {
            // ok, there are some left, we need up update the stack size, value and burden of the fromWo
            // unless we have a data issue, these are valid asserts Og II

            Debug.Assert(fromWo.Value != null, "fromWo.Value != null");
            Debug.Assert(fromWo.StackSize != null, "fromWo.StackSize != null");
            Debug.Assert(fromWo.EncumbranceVal != null, "fromWo.Burden != null");

            int newFromValue = (int)(fromWo.Value + ((fromWo.Value / fromWo.StackSize) * -amount));
            uint newFromBurden = (uint)(fromWo.EncumbranceVal + ((fromWo.EncumbranceVal / fromWo.StackSize) * -amount));

            int oldFromStackSize = (int)fromWo.StackSize;
            fromWo.StackSize -= (ushort)amount;
            fromWo.Value = newFromValue;
            fromWo.EncumbranceVal = (int)newFromBurden;

            // Build the needed messages to the client.
            GameMessagePrivateUpdatePropertyInt msgUpdateValue = new GameMessagePrivateUpdatePropertyInt(fromWo.Sequences, PropertyInt.Value, newFromValue);
            Debug.Assert(fromWo.StackSize != null, "fromWo.StackSize != null");
            GameMessageSetStackSize msgAdjustNewStackSize = new GameMessageSetStackSize(fromWo.Sequences, fromWo.Guid, (int)fromWo.StackSize, oldFromStackSize);

            CurrentLandblock.EnqueueBroadcast(Location, MaxObjectTrackingRange, msgUpdateValue, msgAdjustNewStackSize);
        }

        /// <summary>
        /// This method will remove a worldobject if we have consumed all of the amount in the merge.
        /// This checks inventory or wielded items (you could be pulling stackable ammo out of a wielded slot and into a stack in your pack
        /// It then creates and sends the remove object message. Lastly, if the wo has ever been saved to the db, we clean up after ourselves.
        /// </summary>
        /// <param name="session">Session is used for sequence and target</param>
        /// <param name="fromWo">World object of the item are we merging from that needs to be destroyed.</param>
        public void RemoveWorldObject(Session session, WorldObject fromWo)
        {
            if (HasInventoryItem(fromWo.Guid))
                session.Player.TryRemoveFromInventory(fromWo.Guid);

            GameMessageRemoveObject msgRemoveFrom = new GameMessageRemoveObject(fromWo);
            CurrentLandblock.EnqueueBroadcast(Location, MaxObjectTrackingRange, msgRemoveFrom);

            // todo fix for EF
            throw new NotImplementedException();
            //if (fromWo.SnapShotOfAceObject().HasEverBeenSavedToDatabase)
            //   DatabaseManager.Shard.DeleteObject(fromWo.SnapShotOfAceObject(), null);
        }

        /// <summary>
        /// This method processes the Stackable Merge Game Action (F7B1) Stackable Merge (0x0054)
        /// </summary>
        /// <param name="session">Session is used for sequence and target</param>
        /// <param name="mergeFromGuid">Guid of the item are we merging from</param>
        /// <param name="mergeToGuid">Guid of the item we are merging into</param>
        /// <param name="amount">How many are we merging fromGuid into the toGuid</param>
        public void HandleActionStackableMerge(Session session, ObjectGuid mergeFromGuid, ObjectGuid mergeToGuid, int amount)
        {
            new ActionChain(this, () =>
            {
                // is this something I already have? If not, it has to be a pickup - do the pickup and out.
                if (!HasInventoryItem(mergeFromGuid))
                {
                    // This is a pickup into our main pack.
                    session.Player.HandleActionPutItemInContainer(mergeFromGuid, session.Player.Guid);
                }

                WorldObject fromWo = GetInventoryItem(mergeFromGuid);
                WorldObject toWo = GetInventoryItem(mergeToGuid);

                if (fromWo == null || toWo == null) return;

                // Check to see if we are trying to merge into a full stack. If so, nothing to do here.
                // Check this and see if I need to call UpdateToStack to clear the action with an amount of 0 Og II
                if (toWo.MaxStackSize == toWo.StackSize)
                    return;

                Debug.Assert(toWo.StackSize != null, "toWo.StackSize != null");
                if (toWo.MaxStackSize >= (ushort)(toWo.StackSize + amount))
                {
                    UpdateToStack(session, fromWo, toWo, amount);
                    // Ok did we merge it all?   If so, let's destroy the from item.
                    if (fromWo.StackSize == amount)
                        RemoveWorldObject(session, fromWo);
                    else
                        UpdateFromStack(session, fromWo, amount);
                }
                else
                {
                    // ok we have more than the max stack size on the to object, just add what we can and adjust both.
                    Debug.Assert(toWo.MaxStackSize != null, "toWo.MaxStackSize != null");
                    int amtToFill = (int)(toWo.MaxStackSize - toWo.StackSize);
                    UpdateToStack(session, fromWo, toWo, amtToFill);
                    UpdateFromStack(session, toWo, amtToFill);
                }
            }).EnqueueChain();
        }
    }
}
