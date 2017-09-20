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
    using global::ACE.Database;

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

        public Container(AceObject aceObject)
            : base(aceObject)
        {
            WieldedObjects = new Dictionary<ObjectGuid, WorldObject>();
        }

        public void SendInventoryAndWieldedItems(Session session)
        {
            foreach (AceObject invItem in Inventory.Values)
            {
                WorldObject inv = WorldObjectFactory.CreateWorldObject(invItem);
                session.Network.EnqueueSend(new GameMessageCreateObject(inv));
                // Was the item I just send a container?   If so, we need to send the items in the container as well. Og II
                if (invItem.WeenieType != (uint)WeenieType.Container)
                    continue;
                foreach (AceObject itemsInContainer in invItem.Inventory.Values)
                {
                    WorldObject contItem = WorldObjectFactory.CreateWorldObject(itemsInContainer);
                    session.Network.EnqueueSend(new GameMessageCreateObject(contItem));
                }
            }

            foreach (AceObject wieldedItem in WieldedItems.Values)
            {
                WorldObject item = WorldObjectFactory.CreateWorldObject(wieldedItem);
                uint placementId;
                uint childLocation;
                if (item.CurrentWieldedLocation != null)
                    session.Player.SetParentPlacementChild(this, ref item, (uint)item.CurrentWieldedLocation, out placementId, out childLocation);
                session.Network.EnqueueSend(new GameMessageCreateObject(item));
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
            bool foundItem = Inventory.ContainsKey(itemGuid) || WieldedItems.ContainsKey(itemGuid);
            if (foundItem)
                return true;

            var containers = Inventory.Where(wo => wo.Value.WeenieType == (uint)WeenieType.Container).ToList();
            return containers.Any(cnt => (cnt.Value).Inventory.ContainsKey(itemGuid));
        }

        public virtual void RemoveFromInventory(ObjectGuid inventoryItemGuid)
        {
            if (Inventory.ContainsKey(inventoryItemGuid))
            {
                Inventory.Where(i =>
                    {
                        var placement = Inventory[inventoryItemGuid].Placement;
                        return placement != null && i.Value.Placement > (uint)placement;
                    }).ToList().ForEach(i => i.Value.Placement--);
                Inventory.Remove(inventoryItemGuid);
                Burden = UpdateBurden();
            }
            else
            {
                // Ok maybe it is inventory in one of our packs
                var containers = Inventory.Where(wo => wo.Value.WeenieType == (uint)WeenieType.Container).ToList();
                containers.ForEach(x => Inventory.Where(i =>
                    {
                        var placement = Inventory[i.Key].Placement;
                        return placement != null && i.Value.Placement > (uint)placement;
                    }).ToList().ForEach(i => i.Value.Placement--));
                containers.ForEach(i => Inventory.Remove(i.Key));
                Burden = UpdateBurden();
            }
        }

        public ushort UpdateBurden()
        {
            ushort calculatedBurden = 0;
            return calculatedBurden;
        }

        public virtual WorldObject GetInventoryItem(ObjectGuid objectGuid)
        {
            if (Inventory.ContainsKey(objectGuid))
                return WorldObjectFactory.CreateWorldObject(Inventory[objectGuid]);

            var containers = Inventory.Where(wo => wo.Value.WeenieType == (uint)WeenieType.Container).ToList();

            return (from cnt in containers where cnt.Value.Inventory.ContainsKey(objectGuid) select WorldObjectFactory.CreateWorldObject(cnt.Value.Inventory[objectGuid])).FirstOrDefault();
        }
    }
}
