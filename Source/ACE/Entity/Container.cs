using ACE.Entity.Enum;
using System.Collections.Generic;
using System.Linq;
using ACE.Network.Enum;
using log4net;

namespace ACE.Entity
{
    public class Container : WorldObject
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Dictionary<ObjectGuid, WorldObject> inventory = new Dictionary<ObjectGuid, WorldObject>();

        public override ushort? Burden
        {
            get { return (ushort?)(base.Burden  + UpdateBurden()) ?? (ushort?)0; }
        }

        public Container(ItemType type, ObjectGuid guid, string name, ushort weenieClassId, ObjectDescriptionFlag descriptionFlag, WeenieHeaderFlag weenieFlag, Position position)
            : base(guid)
        {
            Name = name;
            DescriptionFlags = descriptionFlag;
            WeenieFlags = weenieFlag;
            Location = position;
            WeenieClassId = weenieClassId;
        }

        public Container(AceObject aceObject, ObjectGuid guid, string name, ushort weenieClassId, ObjectDescriptionFlag descriptionFlag, WeenieHeaderFlag weenieFlag, Position position)
            : base(guid, aceObject)
        {
            Name = name;
            DescriptionFlags = descriptionFlag;
            WeenieFlags = weenieFlag;
            Location = position;
            WeenieClassId = weenieClassId;
        }

        public Container(AceObject aceObject)
            : base(aceObject)
        {
        }

        public Container(AceObject aceObject, ObjectGuid guid)
            : base(guid, aceObject)
        {
        }

        // Inventory Management Functions
        public virtual void AddToInventory(WorldObject inventoryItem)
        {
            if (!inventory.ContainsKey(inventoryItem.Guid))
            {
                inventory.Add(inventoryItem.Guid, inventoryItem);
                Burden = UpdateBurden();
            }
        }

        public bool HasItem(ObjectGuid inventoryItemGuid, bool includeSubContainers = true)
        {
            if (!includeSubContainers)
                return inventory.ContainsKey(inventoryItemGuid);

            var containers = inventory.Where(wo => wo.Value.ItemCapacity > 0).ToList();
            return containers.Any(cnt => ((Container)cnt.Value).inventory.ContainsKey(inventoryItemGuid));
        }

        public virtual void RemoveFromInventory(ObjectGuid inventoryItemGuid)
        {
            if (inventory.ContainsKey(inventoryItemGuid))
            {
                inventory.Remove(inventoryItemGuid);
                Burden = UpdateBurden();
            }
            else
            {
                // Ok maybe it is inventory in one of our packs
                var containers = inventory.Where(wo => wo.Value.ItemCapacity > 0).ToList();

                foreach (var cnt in containers)
                {
                    if (((Container)cnt.Value).inventory.ContainsKey(inventoryItemGuid))
                    {
                        ((Container)cnt.Value).inventory.Remove(inventoryItemGuid);
                        // update pack burden
                        ((Container)cnt.Value).Burden = ((Container)cnt.Value).UpdateBurden();
                        break;
                    }
                }
                Burden = UpdateBurden();
            }
        }

        public ushort UpdateBurden()
        {
            ushort calculatedBurden = 0;
            foreach (KeyValuePair<ObjectGuid, WorldObject> entry in inventory)
            {
                calculatedBurden += entry.Value.Burden ?? 0;
            }
            return calculatedBurden;
        }

        public void UpdateWieldedItem(Container container, uint itemId)
        {
            // TODO: need to make pack aware - just coding for main pack now.
            ObjectGuid itemGuid = new ObjectGuid(itemId);
            WorldObject inventoryItem = GetInventoryItem(itemGuid);
            if (inventoryItem.ContainerId != container.Guid.Full)
            {
                RemoveFromInventory(itemGuid);
                container.AddToInventory(inventoryItem);
            }
            switch (inventoryItem.ContainerId)
            {
                case null:
                    inventoryItem.ContainerId = container.Guid.Full;
                    inventoryItem.WielderId = null;
                    break;
                default:
                    inventoryItem.ContainerId = null;
                    inventoryItem.WielderId = container.Guid.Full;
                    break;
            }
        }

        public virtual List<KeyValuePair<ObjectGuid, WorldObject>> GetCurrentlyWieldedItems()
        {
            return inventory.Where(wo => wo.Value.WielderId != null && wo.Value.CurrentWieldedLocation < EquipMask.WristWearLeft).ToList();
        }

        public virtual WorldObject GetInventoryItem(ObjectGuid objectGuid)
        {
            var containers = inventory.Where(wo => wo.Value.ItemCapacity > 0).ToList();

            if (inventory.ContainsKey(objectGuid))
                return inventory[objectGuid];
            foreach (var cnt in containers)
            {
                if (((Container)cnt.Value).inventory.ContainsKey(objectGuid))
                    return ((Container)cnt.Value).inventory[objectGuid];
            }

            if (inventory.ContainsKey(objectGuid))
                return inventory[objectGuid];
            return null;
        }
    }
}
