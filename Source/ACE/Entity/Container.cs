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
        }

        public Container(AceObject aceObject)
            : base(aceObject)
        {
        }

        public void SendInventory(Session session)
        {
            foreach (var invItem in Inventory.Values)
            {
                var inv = new GenericObject(invItem);
                session.Network.EnqueueSend(new GameMessageCreateObject(inv));
            }
        }

        // Inventory Management Functions
        public virtual void AddToInventory(WorldObject inventoryItem, uint placement = 0)
        {
            ActionChain actionChain = new ActionChain();
            actionChain.AddAction(this, () =>
            {
                if (Inventory.ContainsKey(inventoryItem.Guid))
                {
                    // if item exists in the list, we are going to shift everything greater than the moving item down 1 to reflect its removal
                    Inventory.Where(i => i.Value.Placement > (uint)Inventory[inventoryItem.Guid].Placement).ToList().ForEach(i => i.Value.Placement--);
                    Inventory.Remove(inventoryItem.Guid);
                }
                // If not going on the very end (next open slot), make a hole.
                Inventory.Where(i => i.Value.Placement >= placement).ToList().ForEach(i => i.Value.Placement++);
                inventoryItem.Placement = placement;
                var aceO = inventoryItem.SnapShotOfAceObject();
                Inventory.Add(inventoryItem.Guid, aceO);
            });
            actionChain.EnqueueChain();
        }

        public bool HasItem(ObjectGuid inventoryItemGuid, bool includeSubContainers = true)
        {
            if (!includeSubContainers)
                return Inventory.ContainsKey(inventoryItemGuid);

            var containers = Inventory.Where(wo => wo.Value.ItemsCapacity > 0).ToList();
            return containers.Any(cnt => (cnt.Value).Inventory.ContainsKey(inventoryItemGuid));
        }

        public virtual void RemoveFromInventory(ObjectGuid inventoryItemGuid)
        {
            if (Inventory.ContainsKey(inventoryItemGuid))
            {
                Inventory.Where(i => i.Value.Placement > (uint)Inventory[inventoryItemGuid].Placement).ToList().ForEach(i => i.Value.Placement--);
                Inventory.Remove(inventoryItemGuid);
                Burden = UpdateBurden();
            }
            else
            {
                // Ok maybe it is inventory in one of our packs
                var containers = Inventory.Where(wo => wo.Value.ItemsCapacity > 0).ToList();
                containers.ForEach(x => Inventory.Where(i => i.Value.Placement > (uint)Inventory[i.Key].Placement).ToList().ForEach(i => i.Value.Placement--));
                containers.ForEach(i => Inventory.Remove(i.Key));
                Burden = UpdateBurden();
            }
        }

        public ushort UpdateBurden()
        {
            ushort calculatedBurden = 0;
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
            // return Inventory.Where(wo => wo.Value.IntProperties != null && wo.Value.CurrentWieldedLocation < EquipMask.WristWearLeft).ToList();
            return null;
        }

        public virtual WorldObject GetInventoryItem(ObjectGuid objectGuid)
        {
            if (Inventory.ContainsKey(objectGuid))
                return new GenericObject(Inventory[objectGuid]);

            var containers = Inventory.Where(wo => wo.Value.ItemsCapacity > 0).ToList();

            foreach (var cnt in containers)
            {
                if (cnt.Value.Inventory.ContainsKey(objectGuid))
                    return new GenericObject(cnt.Value.Inventory[objectGuid]);
            }
            return null;
        }
    }
}
