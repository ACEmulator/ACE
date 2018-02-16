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
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Container(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
            SetProperty(PropertyInt.CoinValue, 0);
            SetProperty(PropertyInt.EncumbranceVal, 0);
            SetProperty(PropertyInt.Value, 0);

            // todo
            /*
            WieldedObjects = new Dictionary<ObjectGuid, WorldObject>();
            foreach (var wieldedItem in WieldedItems)
            {
                ObjectGuid woGuid = new ObjectGuid(wieldedItem.Value.AceObjectId);
                throw new System.NotImplementedException();
                //WieldedObjects.Add(woGuid, WorldObjectFactory.CreateWorldObject(wieldedItem.Value));

                Burden += wieldedItem.Value.EncumbranceVal;
                log.Debug($"{weenie.GetProperty(PropertyString.Name)} is wielding {wieldedItem.Value.Name}, adding {wieldedItem.Value.EncumbranceVal}, current Burden = {Burden}");

                Value += wieldedItem.Value.Value;
            }

            InventoryObjects = new Dictionary<ObjectGuid, WorldObject>();
            foreach (var inventoryItem in Inventory)
            {
                ObjectGuid woGuid = new ObjectGuid(inventoryItem.Value.AceObjectId);
                throw new System.NotImplementedException();
                WorldObject wo = WorldObjectFactory.CreateWorldObject(inventoryItem.Value);
                InventoryObjects.Add(woGuid, wo);

                if (wo.WeenieType == WeenieType.Coin)
                    CoinValue += wo.Value ?? 0;

                Burden += wo.Burden ?? 0;
                log.Debug($"{aceObject.Name} is has {wo.Name} in inventory, adding {wo.Burden}, current Burden = {Burden}");
            }
            */
        }


        // todo I want to rework the tracked equipment/wielded items

        /// <summary>
        /// Use EquipObject() and DequipObject() to manipulate this dictionary..
        /// </summary>
        public Dictionary<ObjectGuid, WorldObject> EquippedObjects { get; } = new Dictionary<ObjectGuid, WorldObject>();

        /// <summary>
        /// This will set the wielder property on worldObject to this guid, and will add it to the EquippedObjects dictionary.
        /// </summary>
        /// <param name="worldObject"></param>
        public bool TryEquipObject(WorldObject worldObject)
        {
            worldObject.SetProperty(PropertyInstanceId.Wielder, (int)Biota.Id);
            EquippedObjects[worldObject.Guid] = worldObject;

            return true;
        }

        /// <summary>
        /// This will set the CurrentWieldedLocation property to wieldedLocation and the Wielder property to this guid and will add it to the EquippedObjects dictionary.
        /// </summary>
        /// <param name="worldObject"></param>
        public bool TryEquipObject(WorldObject worldObject, int wieldedLocation)
        {
            // todo see if the wielded location is in use, if so, return false

            worldObject.SetProperty(PropertyInt.CurrentWieldedLocation, wieldedLocation);

            worldObject.SetProperty(PropertyInstanceId.Wielder, (int)Biota.Id);
            EquippedObjects[worldObject.Guid] = worldObject;

            return true;
        }

        /// <summary>
        /// This will remove the wielder property on worldObject and will remove it from the EquippedObjects dictionary.
        /// </summary>
        /// <param name="worldObject"></param>
        public bool TryDequipObject(WorldObject worldObject)
        {
            worldObject.RemoveProperty(PropertyInstanceId.Wielder);
            EquippedObjects.Remove(worldObject.Guid);

            return true;
        }


        public Dictionary<ObjectGuid, WorldObject> InventoryObjects { get; } = new Dictionary<ObjectGuid, WorldObject>();

        public bool TryAddToInventory(WorldObject worldObject)
        {
            // todo
            /*
            iouObj.SetProperty(PropertyInstanceId.Container, (int)player.Guid.Full);

            // FIXME: This is wrong and should also be unnecessary but we're not handling storing and reading back object placement within a container correctly so this is here to make it work.
            // TODO: fix placement (order or slot) issues within containers.
            iouObj.SetProperty(PropertyInt.Placement, 0);
            */

            return false;
        }









        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        // ******************************************************************* OLD CODE BELOW ********************************
        
        private int coinValue;
        public override int? CoinValue
        {
            get { return coinValue; }
            set
            {
                if (value != coinValue)
                {
                    coinValue = (int)value;
                    base.CoinValue = value;
                }
            }
        }

        private ushort burden;
        public override ushort? Burden
        {
            get { return burden; }
            set
            {
                if (value != burden)
                {
                    burden = (ushort)value;
                    base.Burden = burden;
                }
            }
        }

        private ushort usedPackSlots;
        private ushort maxPackSlots = 15;

        // Inventory Management Functions
        public virtual void AddToInventory(WorldObject inventoryItem, int placement = 0)
        {
            AddToInventoryEx(inventoryItem, placement);

            Burden += inventoryItem.Burden;
            log.Debug($"Add {inventoryItem.Name} in inventory, adding {inventoryItem.Burden}, current Burden = {Burden}");

            Value += inventoryItem.Value;
        }

        /// <summary>
        /// Adds a new item to the inventory collection AND NOTHING ELSE.  will not send updates to the client.  The
        /// primary use case here is as a helper function or for adding items prior to login (ie, char gen)
        /// </summary>
        public virtual void AddToInventoryEx(WorldObject inventoryItem, int placement = 0)
        {
            if (InventoryObjects.ContainsKey(inventoryItem.Guid))
            {
                // if item exists in the list, we are going to shift everything greater than the moving item down 1 to reflect its removal
                if (inventoryItem.UseBackpackSlot)
                    InventoryObjects.Where(i => InventoryObjects[inventoryItem.Guid].PlacementPosition != null &&
                                         i.Value.PlacementPosition > (uint)InventoryObjects[inventoryItem.Guid].PlacementPosition &&
                                         i.Value.UseBackpackSlot).ToList().ForEach(i => i.Value.PlacementPosition--);
                else
                    InventoryObjects.Where(i => InventoryObjects[inventoryItem.Guid].PlacementPosition != null &&
                                         i.Value.PlacementPosition > (uint)InventoryObjects[inventoryItem.Guid].PlacementPosition &&
                                         !i.Value.UseBackpackSlot).ToList().ForEach(i => i.Value.PlacementPosition--);

                InventoryObjects.Remove(inventoryItem.Guid);
            }
            // If not going on the very end (next open slot), make a hole.
            if (inventoryItem.UseBackpackSlot)
                InventoryObjects.Where(i => i.Value.PlacementPosition >= placement && i.Value.UseBackpackSlot).ToList().ForEach(i => i.Value.PlacementPosition++);
            else
                InventoryObjects.Where(i => i.Value.PlacementPosition >= placement && !i.Value.UseBackpackSlot).ToList().ForEach(i => i.Value.PlacementPosition++);

            inventoryItem.PlacementPosition = placement;
            inventoryItem.Location = null;
            InventoryObjects.Add(inventoryItem.Guid, inventoryItem);
        }

        public bool HasItem(ObjectGuid itemGuid)
        {
            bool foundItem = InventoryObjects.ContainsKey(itemGuid) || WieldedObjects.ContainsKey(itemGuid);
            if (foundItem)
                return true;

            var containers = InventoryObjects.Where(wo => wo.Value.WeenieType == WeenieType.Container).ToList();
            throw new System.NotImplementedException();/* Fix this to use the new inventory objects
            return containers.Any(cnt => (cnt.Value).InventoryObjects.ContainsKey(itemGuid));*/
        }

        public virtual void RemoveWorldObjectFromInventory(ObjectGuid objectguid)
        {
            // first search me / add all items of type.
            if (InventoryObjects.ContainsKey(objectguid))
            {
                // defrag the pack
                int placement = InventoryObjects[objectguid].PlacementPosition ?? 0;
                InventoryObjects.Where(i => i.Value.PlacementPosition > placement).ToList().ForEach(i => --i.Value.PlacementPosition);

                // todo calculate burdon / value / container properly

                // clear objects out maybe for db ?
                InventoryObjects[objectguid].ContainerId = null;
                InventoryObjects[objectguid].PlacementPosition = null;

                Burden -= InventoryObjects[objectguid].Burden;

                log.Debug($"Remove {InventoryObjects[objectguid].Name} in inventory, removing {InventoryObjects[objectguid].Burden}, current Burden = {Burden}");

                // TODO: research, should this only be done for pyreal and trade notes?   Does the value of your items add to the container value?   I am not sure.
                Value -= InventoryObjects[objectguid].Value;

                // decrease pack counter if item is not a container!
                if (InventoryObjects[objectguid].WeenieType != WeenieType.Container)
                    usedPackSlots -= 1;

                InventoryObjects.Remove(objectguid);
                return;
            }

            // next search all containers for item.. run function again for each container.
            var containers = InventoryObjects.Where(wo => wo.Value.WeenieType == WeenieType.Container).ToList();
            foreach (var container in containers)
            {
                ((Container)container.Value).RemoveWorldObjectFromInventory(objectguid);
            }
        }

    /// <summary>
    /// This method is used to get anything in our posession.   Inventory in main or any packs,
    /// </summary>
    public virtual WorldObject GetInventoryItem(ObjectGuid objectGuid)
        {
            // first search me for this item..
            if (InventoryObjects.ContainsKey(objectGuid))
            {
                if (InventoryObjects.TryGetValue(objectGuid, out var inventoryItem))
                    return inventoryItem;
            }

            // continue searching other packs..
            // next search all containers for item.. run function again for each container.
            var containers = InventoryObjects.Where(wo => wo.Value.WeenieType == WeenieType.Container).ToList();
            foreach (var container in containers)
            {
                if ((container.Value as Container).GetInventoryItem(objectGuid) != null)
                {
                    if ((container.Value as Container).GetInventoryItem(objectGuid) != null)
                    {
                        return (container.Value as Container).GetInventoryItem(objectGuid);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets Free Pack
        /// </summary>
        public virtual uint GetFreePackLocation()
        {
            // do I have enough space ?
            if (usedPackSlots <= maxPackSlots)
            {
                return Guid.Full;
            }

            // do any of my other containers have enough space ?
            var containers = InventoryObjects.Where(wo => wo.Value.WeenieType == WeenieType.Container).ToList();
            foreach (var container in containers)
            {
                if ((container.Value as Container).GetFreePackLocation() != 0)
                {
                    return (container.Value as Container).GetFreePackLocation();
                }
            }
            return 0;
        }

        /// <summary>
        /// This method is used to get all inventory items of Coin in this container (example of usage get all items of coin on player)
        /// </summary>
        public virtual List<WorldObject> GetInventoryItemsOfTypeWeenieType(WeenieType type)
        {
            List<WorldObject> items = new List<WorldObject>();

            // first search me / add all items of type.
            var localInventory = InventoryObjects.Where(wo => wo.Value.WeenieType == type).ToList();
            foreach (var wo in localInventory)
            {
                items.Add(wo.Value);
            }

            // next search all containers for coin.. run function again for each container.
            var containers = InventoryObjects.Where(wo => wo.Value.WeenieType == WeenieType.Container).ToList();
            foreach (var container in containers)
            {
                items.AddRange((container.Value as Container).GetInventoryItemsOfTypeWeenieType(type));
            }

            return items;
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
            Debug.Assert(toWo.Burden != null, "toWo.Burden != null");
            Debug.Assert(fromWo.Burden != null, "fromWo.Burden != null");

            int newValue = (int)(toWo.Value + ((fromWo.Value / fromWo.StackSize) * amount));
            uint newBurden = (uint)(toWo.Burden + ((fromWo.Burden / fromWo.StackSize) * amount));

            int oldStackSize = (int)toWo.StackSize;
            toWo.StackSize += (ushort)amount;
            toWo.Value = newValue;
            toWo.Burden = (ushort)newBurden;

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
            Debug.Assert(fromWo.Burden != null, "fromWo.Burden != null");

            int newFromValue = (int)(fromWo.Value + ((fromWo.Value / fromWo.StackSize) * -amount));
            uint newFromBurden = (uint)(fromWo.Burden + ((fromWo.Burden / fromWo.StackSize) * -amount));

            int oldFromStackSize = (int)fromWo.StackSize;
            fromWo.StackSize -= (ushort)amount;
            fromWo.Value = newFromValue;
            fromWo.Burden = (ushort)newFromBurden;

            // Build the needed messages to the client.
            GameMessagePrivateUpdatePropertyInt msgUpdateValue = new GameMessagePrivateUpdatePropertyInt(fromWo.Sequences, PropertyInt.Value, newFromValue);
            Debug.Assert(fromWo.StackSize != null, "fromWo.StackSize != null");
            GameMessageSetStackSize msgAdjustNewStackSize = new GameMessageSetStackSize(fromWo.Sequences, fromWo.Guid, (int)fromWo.StackSize, oldFromStackSize);

            CurrentLandblock.EnqueueBroadcast(Location, MaxObjectTrackingRange, msgUpdateValue, msgAdjustNewStackSize);
        }

        /// <summary>
        /// This method will remove a worldobject if we have consumed all of the amount in the merge.
        /// This checks inventory or wielded items (you could be pulling stackable ammo out of a wielded slot and into a stack in your pack
        /// It then creates and sends the remove object message.   Lastly, if the wo has ever been saved to the db, we clean up after ourselves.
        /// </summary>
        /// <param name="session">Session is used for sequence and target</param>
        /// <param name="fromWo">World object of the item are we merging from that needs to be destroyed.</param>
        public void RemoveWorldObject(Session session, WorldObject fromWo)
        {
            if (HasItem(fromWo.Guid))
                session.Player.RemoveWorldObjectFromInventory(fromWo.Guid);
            else
               session.Player.RemoveFromWieldedObjects(fromWo.Guid);
            GameMessageRemoveObject msgRemoveFrom = new GameMessageRemoveObject(fromWo);
            CurrentLandblock.EnqueueBroadcast(Location, MaxObjectTrackingRange, msgRemoveFrom);

            if (fromWo.SnapShotOfAceObject().HasEverBeenSavedToDatabase)
                DatabaseManager.Shard.DeleteObject(fromWo.SnapShotOfAceObject(), null);
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
                if (!HasItem(mergeFromGuid))
                {
                    // This is a pickup into our main pack.
                    session.Player.PutItemInContainer(mergeFromGuid, session.Player.Guid);
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
