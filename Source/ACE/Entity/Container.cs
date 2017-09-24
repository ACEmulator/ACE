using ACE.Entity.Enum;
using System.Collections.Generic;
using System.Linq;
using log4net;
using ACE.Entity.Actions;
using ACE.Factories;
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

        protected Dictionary<ObjectGuid, AceObject> Inventory
        {
            get { return AceObject.Inventory; }
        }

        // This dictionary is only used to load WieldedObjects and to save them.   Other than the load and save, it should never be added to or removed from.
        protected Dictionary<ObjectGuid, AceObject> WieldedItems
        {
            get { return AceObject.WieldedItems; }
        }

        protected Dictionary<ObjectGuid, WorldObject> WieldedObjects { get; set; }

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
        }

        // Inventory Management Functions
        public virtual void AddToInventory(WorldObject inventoryItem, uint placement = 0)
        {
            ActionChain actionChain = new ActionChain();
            actionChain.AddAction(this, () =>
            {
                AddToInventoryEx(inventoryItem, placement);
            });
            actionChain.EnqueueChain();
        }

        /// <summary>
        /// Adds a new item to the inventory collection AND NOTHING ELSE.  will not send updates to the client.  The
        /// primary use case here is as a helper function or for adding items prior to login (ie, char gen)
        /// </summary>
        public virtual void AddToInventoryEx(WorldObject inventoryItem, uint placement = 0)
        {
            if (Inventory.ContainsKey(inventoryItem.Guid))
            {
                // if item exists in the list, we are going to shift everything greater than the moving item down 1 to reflect its removal
                if (inventoryItem.UseBackpackSlot)
                    Inventory.Where(i => Inventory[inventoryItem.Guid].Placement != null &&
                                         i.Value.Placement > (uint)Inventory[inventoryItem.Guid].Placement &&
                                         i.Value.UseBackpackSlot).ToList().ForEach(i => i.Value.Placement--);
                else
                    Inventory.Where(i => Inventory[inventoryItem.Guid].Placement != null &&
                                         i.Value.Placement > (uint)Inventory[inventoryItem.Guid].Placement &&
                                         !i.Value.UseBackpackSlot).ToList().ForEach(i => i.Value.Placement--);

                Inventory.Remove(inventoryItem.Guid);
            }
            // If not going on the very end (next open slot), make a hole.
            if (inventoryItem.UseBackpackSlot)
                Inventory.Where(i => i.Value.Placement >= placement &&
                                     i.Value.UseBackpackSlot).ToList().ForEach(i => i.Value.Placement++);
            else
                Inventory.Where(i => i.Value.Placement >= placement &&
                 !i.Value.UseBackpackSlot).ToList().ForEach(i => i.Value.Placement++);

            inventoryItem.Placement = placement;
            inventoryItem.Location = null;
            AceObject aceO = inventoryItem.SnapShotOfAceObject();
            Inventory.Add(inventoryItem.Guid, aceO);
        }

        public bool HasItem(ObjectGuid itemGuid)
        {
            bool foundItem = Inventory.ContainsKey(itemGuid) || WieldedObjects.ContainsKey(itemGuid);
            if (foundItem)
                return true;

            var containers = Inventory.Where(wo => wo.Value.WeenieType == (uint)WeenieType.Container).ToList();
            return containers.Any(cnt => (cnt.Value).Inventory.ContainsKey(itemGuid));
        }

        /// <summary>
        /// Remove item from inventory directory.   Also, defragment the placement values.
        /// </summary>
        /// <param name="itemGuid"></param>
        public virtual void RemoveFromInventory(ObjectGuid itemGuid)
        {
            if (!Inventory.ContainsKey(itemGuid)) return;

            uint placement = Inventory[itemGuid].Placement ?? 0u;
            uint? containerId = GetContainer(itemGuid);
            if (containerId == null) return;

            ObjectGuid containerGuid = new ObjectGuid((uint)containerId);
            AceObject container;
            if (containerGuid.IsPlayer())
                container = this.AceObject;
            else
                container = Inventory[containerGuid];

            container.Inventory.Where(i => i.Value.Placement > placement).ToList().ForEach(i => --i.Value.Placement);
            container.Inventory[itemGuid].ContainerIID = null;
            container.Inventory[itemGuid].Placement = null;
            container.Inventory.Remove(itemGuid);
            Burden = UpdateBurden();
        }

        public ushort UpdateBurden()
        {
            ushort calculatedBurden = 0;
            return calculatedBurden;
        }

        public uint? GetContainer(ObjectGuid itemGuid)
        {
            return GetInventoryItem(itemGuid).ContainerId;
        }

        public virtual WorldObject GetInventoryItem(ObjectGuid objectGuid)
        {
            if (Inventory.ContainsKey(objectGuid))
                return WorldObjectFactory.CreateWorldObject(Inventory[objectGuid]);

            var containers = Inventory.Where(wo => wo.Value.WeenieType == (uint)WeenieType.Container).ToList();
            WorldObject item = (from cnt in containers where cnt.Value.Inventory.ContainsKey(objectGuid) select WorldObjectFactory.CreateWorldObject(cnt.Value.Inventory[objectGuid])).FirstOrDefault();

            // It is not in inventory - main pack or container - last check the wielded items.
            if (item == null)
                WieldedObjects.TryGetValue(objectGuid, out item);

            return item;
        }
    }
}
