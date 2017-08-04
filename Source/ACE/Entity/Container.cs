using ACE.Entity.Enum;
using System.Collections.Generic;
using System.Linq;
using log4net;
using ACE.Entity.Actions;
using ACE.Network;
using ACE.Network.GameMessages.Messages;

namespace ACE.Entity
{
    public class Container : WorldObject
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Dictionary<ObjectGuid, WorldObject> inventory = new Dictionary<ObjectGuid, WorldObject>();

        public override ushort? Burden
        {
            // FIXME : this is a temp fix, it works, but we need to refactor burden correctly.   It should only be
            // persisted when burden is actually changed ie via application of salvage.   All burden for containers should be a sum of
            // burden.  For example a pack is 65 bu.   It should always be 65 bu empty or full.   However we should report burden as below calculation
            // base burden + burden of contents as calculation. Og II
            get { return (ushort?)(base.Burden + UpdateBurden()) ?? (ushort?)0; }
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
            aceObject.Inventory.ForEach(i => inventory.Add(new ObjectGuid(i.AceObjectId), new GenericObject(i)));
        }

        public Container(AceObject aceObject)
            : base(aceObject)
        {
            aceObject.Inventory.ForEach(i => inventory.Add(new ObjectGuid(i.AceObjectId), new GenericObject(i)));
        }

        public void SendInventory(Session session)
        {
            foreach (var invItem in inventory.Values)
               session.Network.EnqueueSend(new GameMessageCreateObject(invItem));
        }

        // Inventory Management Functions
        public virtual void AddToInventory(WorldObject inventoryItem, uint placement = 0)
        {
            ActionChain actionChain = new ActionChain();
            actionChain.AddAction(this, () =>
            {
                if (inventory.ContainsKey(inventoryItem.Guid))
                {
                    // if item exists in the list, we are going to shift everything greater than the moving item down 1 to reflect its removal
                    inventory.Where(i => i.Value.Placement > (uint)inventory[inventoryItem.Guid].Placement).ToList().ForEach(i => i.Value.Placement--);
                    inventory.Remove(inventoryItem.Guid);
                }
                // If not going on the very end (next open slot), make a hole.
                inventory.Where(i => i.Value.Placement >= placement).ToList().ForEach(i => i.Value.Placement++);
                inventoryItem.Placement = placement;
                inventory.Add(inventoryItem.Guid, inventoryItem);
                // FIXME: I know there must be a better way, but not sure how to do it.
                // For now, sync up underlying aceObject
                AceObject.Inventory.Clear();
                inventory.ToList().ForEach(i => AceObject.Inventory.Add(i.Value.SnapShotOfAceObject()));
            });
            actionChain.EnqueueChain();
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
