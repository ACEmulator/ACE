using ACE.Entity.Enum;
using System.Collections.Generic;
using System.Linq;
using log4net;
using ACE.Factories;
using System.Diagnostics;
using ACE.Entity.Actions;
using ACE.Entity.Enum.Properties;
using ACE.Network;
using ACE.Network.GameMessages.Messages;
using ACE.Database;

namespace ACE.Entity
{
    public class Container : WorldObject
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private uint coinValue = 0;
        public override uint? CoinValue
        {
            get { return coinValue; }
            set
            {
                if (value != coinValue)
                {
                    coinValue = (uint)value;
                    base.CoinValue = value;
                }
            }
        }

        private ushort burden = 0;
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

        /// <summary>
        /// On initial load, we will create all of the wielded items as world objects and add to dictionary for management.
        /// </summary>
        /// <param name="aceObject"></param>
        public Container(AceObject aceObject)
            : base(aceObject)
        {
            CoinValue = 0;
            log.Debug($"{aceObject.Name} CoinValue initialized to {CoinValue}");

            Burden = 0;
            log.Debug($"{aceObject.Name} Burden initialized to {Burden}");

            Burden += Weenie.EncumbranceVal ?? 0;
            log.Debug($"{aceObject.Name}'s weenie id is {Weenie.WeenieClassId} and its base burden is {Weenie.EncumbranceVal}, added to burden, Burden = {Burden}");

            Value = 0;
            Value += Weenie.Value ?? 0;

            WieldedObjects = new Dictionary<ObjectGuid, WorldObject>();
            foreach (var wieldedItem in WieldedItems)
            {
                ObjectGuid woGuid = new ObjectGuid(wieldedItem.Value.AceObjectId);
                WieldedObjects.Add(woGuid, WorldObjectFactory.CreateWorldObject(wieldedItem.Value));

                Burden += wieldedItem.Value.EncumbranceVal;
                log.Debug($"{aceObject.Name} is wielding {wieldedItem.Value.Name}, adding {wieldedItem.Value.EncumbranceVal}, current Burden = {Burden}");

                Value += wieldedItem.Value.Value;
            }

            InventoryObjects = new Dictionary<ObjectGuid, WorldObject>();
            foreach (var inventoryItem in Inventory)
            {
                ObjectGuid woGuid = new ObjectGuid(inventoryItem.Value.AceObjectId);
                WorldObject wo = WorldObjectFactory.CreateWorldObject(inventoryItem.Value);
                InventoryObjects.Add(woGuid, wo);

                Burden += wo.Burden ?? 0;
                log.Debug($"{aceObject.Name} is has {wo.Name} in inventory, adding {wo.Burden}, current Burden = {Burden}");

                Value += wo.Value ?? 0;

                if (wo.WeenieType == WeenieType.Coin)
                {
                    CoinValue += wo.Value ?? 0;
                    log.Debug($"{aceObject.Name} is has {wo.Name} in inventory, of WeenieType.Coin, adding {wo.Value}, current CoinValue = {CoinValue}");
                }
            }
        }

        // Inventory Management Functions
        public virtual void AddToInventory(WorldObject inventoryItem, uint placement = 0)
        {
            AddToInventoryEx(inventoryItem, placement);

            if (inventoryItem.WeenieType == WeenieType.Coin)
            {
                CoinValue += inventoryItem.Value ?? 0;
            }

            if (inventoryItem.WeenieType == WeenieType.Container)
            {
                CoinValue += inventoryItem.CoinValue ?? 0;
            }

            Burden += inventoryItem.Burden;
            log.Debug($"Add {inventoryItem.Name} in inventory, adding {inventoryItem.Burden}, current Burden = {Burden}");

            Value += inventoryItem.Value;
        }

        /// <summary>
        /// Adds a new item to the inventory collection AND NOTHING ELSE.  will not send updates to the client.  The
        /// primary use case here is as a helper function or for adding items prior to login (ie, char gen)
        /// </summary>
        public virtual void AddToInventoryEx(WorldObject inventoryItem, uint placement = 0)
        {
            if (InventoryObjects.ContainsKey(inventoryItem.Guid))
            {
                // if item exists in the list, we are going to shift everything greater than the moving item down 1 to reflect its removal
                if (inventoryItem.UseBackpackSlot)
                    InventoryObjects.Where(i => InventoryObjects[inventoryItem.Guid].Placement != null &&
                                         i.Value.Placement > (uint)InventoryObjects[inventoryItem.Guid].Placement &&
                                         i.Value.UseBackpackSlot).ToList().ForEach(i => i.Value.Placement--);
                else
                    InventoryObjects.Where(i => InventoryObjects[inventoryItem.Guid].Placement != null &&
                                         i.Value.Placement > (uint)InventoryObjects[inventoryItem.Guid].Placement &&
                                         !i.Value.UseBackpackSlot).ToList().ForEach(i => i.Value.Placement--);

                InventoryObjects.Remove(inventoryItem.Guid);
            }
            // If not going on the very end (next open slot), make a hole.
            if (inventoryItem.UseBackpackSlot)
                InventoryObjects.Where(i => i.Value.Placement >= placement && i.Value.UseBackpackSlot).ToList().ForEach(i => i.Value.Placement++);
            else
                InventoryObjects.Where(i => i.Value.Placement >= placement && !i.Value.UseBackpackSlot).ToList().ForEach(i => i.Value.Placement++);

            inventoryItem.Placement = placement;
            inventoryItem.Location = null;
            InventoryObjects.Add(inventoryItem.Guid, inventoryItem);
        }

        public bool HasItem(ObjectGuid itemGuid)
        {
            bool foundItem = InventoryObjects.ContainsKey(itemGuid) || WieldedObjects.ContainsKey(itemGuid);
            if (foundItem)
                return true;

            var containers = InventoryObjects.Where(wo => wo.Value.WeenieType == WeenieType.Container).ToList();
            return containers.Any(cnt => (cnt.Value).InventoryObjects.ContainsKey(itemGuid));
        }

        /// <summary>
        /// Remove item from inventory directory.   Also, defragment the placement values.
        /// </summary>
        /// <param name="inv"></param>
        /// <param name="itemGuid"></param>
        public virtual void RemoveFromInventory(Dictionary<ObjectGuid, WorldObject> inv, ObjectGuid itemGuid)
        {
            if (!inv.ContainsKey(itemGuid))
                return;

            uint placement = inv[itemGuid].Placement ?? 0u;
            inv.Where(i => i.Value.Placement > placement).ToList().ForEach(i => --i.Value.Placement);
            // Removed some unused code.   Og II

            ObjectGuid containerGuid = new ObjectGuid((uint)inv[itemGuid].ContainerId);
            if (!containerGuid.IsPlayer())
            {
                Container pack = (Container)GetInventoryItem(containerGuid);

                pack.Burden -= inv[itemGuid].Burden;
                pack.Value -= inv[itemGuid].Value;

                if (inv[itemGuid].WeenieType == WeenieType.Coin)
                {
                    pack.CoinValue -= inv[itemGuid].Value ?? 0;
                }
            }

            inv[itemGuid].ContainerId = null;
            inv[itemGuid].Placement = null;

            if (inv[itemGuid].WeenieType == WeenieType.Coin)
            {
                CoinValue -= inv[itemGuid].Value ?? 0;
            }

            if (inv[itemGuid].WeenieType == WeenieType.Container)
            {
                CoinValue -= inv[itemGuid].CoinValue ?? 0;
            }

            Burden -= inv[itemGuid].Burden;
            log.Debug($"Remove {inv[itemGuid].Name} in inventory, removing {inv[itemGuid].Burden}, current Burden = {Burden}");

            Value -= inv[itemGuid].Value;

            inv.Remove(itemGuid);
        }

        /// <summary>
        /// This method is used to get anything in our posession.   Inventory in main or any packs,
        /// as well as wielded items.   If we have it, this will return it.
        /// </summary>
        /// <param name="objectGuid"></param>
        /// <returns></returns>
        public virtual WorldObject GetInventoryItem(ObjectGuid objectGuid)
        {
            WorldObject inventoryItem;
            if (InventoryObjects.TryGetValue(objectGuid, out inventoryItem))
                return inventoryItem;

            WorldObject item = null;
            var containers = InventoryObjects.Where(wo => wo.Value.WeenieType == WeenieType.Container).ToList();
            foreach (var container in containers)
            {
                if (container.Value.InventoryObjects.TryGetValue(objectGuid, out item))
                {
                    break;
                }
            }

            // It is not in inventory - main pack or container - last check the wielded items.
            if (item == null)
                WieldedObjects.TryGetValue(objectGuid, out item);

            return item;
        }

        /// <summary>
        /// This method processes the Stackable Merge Game Action (F7B1) Stackable Merge (0x0054)
        /// </summary>
        /// <param name="session">Session is used for sequence and target</param>
        /// <param name="mergeFromGuid">Guid of the item are we merging from</param>
        /// <param name="mergeToGuid">Guid of the item we are merging into</param>
        /// <param name="amount">How many are we merging fromGuid into the toGuid</param>
        public void HandleActionStackableMerge(Session session, ObjectGuid mergeFromGuid, ObjectGuid mergeToGuid, uint amount)
        {
            new ActionChain(this, () =>
            {
                WorldObject fromWo = GetInventoryItem(mergeFromGuid);
                WorldObject toWo = GetInventoryItem(mergeToGuid);
                if (fromWo == null || toWo == null) return;

                Debug.Assert(toWo.StackSize != null, "toWo.StackSize != null");
                if (toWo.MaxStackSize >= (ushort)(toWo.StackSize + amount))
                {
                    // unless we have a data issue, these are valid asserts Og II
                    Debug.Assert(toWo.Value != null, "toWo.Value != null");
                    Debug.Assert(fromWo.Value != null, "fromWo.Value != null");
                    Debug.Assert(toWo.StackSize != null, "toWo.StackSize != null");
                    Debug.Assert(fromWo.StackSize != null, "fromWo.StackSize != null");
                    Debug.Assert(toWo.Burden != null, "toWo.Burden != null");
                    Debug.Assert(fromWo.Burden != null, "fromWo.Burden != null");

                    uint newValue = (uint)(toWo.Value + ((fromWo.Value / fromWo.StackSize) * amount));
                    uint newBurden = (uint)(toWo.Burden + ((fromWo.Burden / fromWo.StackSize) * amount));

                    int oldStackSize = (int)toWo.StackSize;
                    toWo.StackSize += (ushort)amount;
                    toWo.Value = newValue;
                    toWo.Burden = (ushort)newBurden;

                    // Build the needed messages to the client.
                    GameMessagePrivateUpdatePropertyInt msgUpdateValue = new GameMessagePrivateUpdatePropertyInt(Sequences, PropertyInt.Value, newValue);
                    GameMessagePutObjectInContainer msgPutObjectInContainer = new GameMessagePutObjectInContainer(session, Guid, toWo, toWo.Placement ?? 0u);
                    Debug.Assert(toWo.StackSize != null, "toWo.StackSize != null");
                    GameMessageSetStackSize msgAdjustNewStackSize = new GameMessageSetStackSize(toWo.Sequences, toWo.Guid, (int)toWo.StackSize, oldStackSize);

                    CurrentLandblock.EnqueueBroadcast(Location, MaxObjectTrackingRange, msgUpdateValue, msgPutObjectInContainer, msgAdjustNewStackSize);

                    // Ok did we merge it all?   If so, let's destroy the from item.
                    if (fromWo.StackSize == amount)
                    {
                        session.Player.RemoveFromInventory(InventoryObjects, fromWo.Guid);
                        GameMessageRemoveObject msgRemoveFrom = new GameMessageRemoveObject(fromWo);
                        CurrentLandblock.EnqueueBroadcast(Location, MaxObjectTrackingRange, msgRemoveFrom);
                        DatabaseManager.Shard.DeleteObject(fromWo.SnapShotOfAceObject(), null);
                    }
                }
            }).EnqueueChain();
        }
    }
}
