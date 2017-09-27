using ACE.Entity.Enum;
using System.Collections.Generic;
using System.Linq;
using log4net;
using ACE.Factories;
using ACE.Database;

namespace ACE.Entity
{
    public class Container : WorldObject
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override ushort? Burden
        {
            // FIXME : this is a temp fix, it works, but we need to refactor burden correctly.   It should only be
            // persisted when burden is actually changed ie via application of salvage.   All burden for containers should be a sum of
            // burden.  For example a pack is 65 bu.   It should always be 65 bu empty or full.   However we should report burden as below calculation
            // base burden + burden of contents as calculation. Og II
            get { return (ushort?)(base.Burden + UpdateBurden()) ?? (ushort?)0; }
        }

        public Container(AceObject aceObject, ObjectGuid guid, string name, ushort weenieClassId, ObjectDescriptionFlag descriptionFlag, WeenieHeaderFlag weenieFlag, Position position)
            : this(aceObject)
        {
            Name = name;
            DescriptionFlags = descriptionFlag;
            WeenieFlags = weenieFlag;
            Location = position;
            WeenieClassId = weenieClassId;
        }

        /// <summary>
        /// On initial load, we will create all of the wielded items as world objects and add to dictionary for management.
        /// </summary>
        /// <param name="aceObject"></param>
        public Container(AceObject aceObject)
            : base(aceObject)
        {
            WieldedObjects = new Dictionary<ObjectGuid, WorldObject>();
            foreach (var wieldedItem in WieldedItems)
            {
                ObjectGuid woGuid = new ObjectGuid(wieldedItem.Value.AceObjectId);
                WieldedObjects.Add(woGuid, new GenericObject(WieldedItems[woGuid]));
            }

            InventoryObjects = new Dictionary<ObjectGuid, WorldObject>();
            foreach (var inventoryItem in Inventory)
            {
                ObjectGuid woGuid = new ObjectGuid(inventoryItem.Value.AceObjectId);
                InventoryObjects.Add(woGuid, WorldObjectFactory.CreateWorldObject(inventoryItem.Value));
                if (InventoryObjects[woGuid].WeenieType == WeenieType.Container)
                {
                    InventoryObjects[woGuid].InventoryObjects = new Dictionary<ObjectGuid, WorldObject>();
                    foreach (var item in Inventory[woGuid].Inventory)
                    {
                        ObjectGuid cwoGuid = new ObjectGuid(item.Value.AceObjectId);
                        InventoryObjects[woGuid].InventoryObjects.Add(cwoGuid, WorldObjectFactory.CreateWorldObject(item.Value));
                    }
                }
            }
        }

        // Inventory Management Functions
        public virtual void AddToInventory(WorldObject inventoryItem, uint placement = 0)
        {
            AddToInventoryEx(inventoryItem, placement);
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
            if (!inv.ContainsKey(itemGuid)) return;

            uint placement = inv[itemGuid].Placement ?? 0u;
            inv.Where(i => i.Value.Placement > placement).ToList().ForEach(i => --i.Value.Placement);
            inv[itemGuid].ContainerId = null;
            inv[itemGuid].Placement = null;
            inv.Remove(itemGuid);
        }

        public ushort UpdateBurden()
        {
            // TODO: reimplement this.   Og II
            ushort calculatedBurden = 0;
            return calculatedBurden;
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
    }
}
