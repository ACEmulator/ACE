using ACE.Entity.Enum;
using ACE.Managers;
using ACE.Network.Enum;
using System.Collections.Generic;

namespace ACE.Entity
{
    public class Container : WorldObject
    {
        private readonly Dictionary<ObjectGuid, WorldObject> inventory = new Dictionary<ObjectGuid, WorldObject>();

        private readonly object inventoryMutex = new object();

        public Container(ObjectType type, ObjectGuid guid, string name, ushort weenieClassId, ObjectDescriptionFlag descriptionFlag, WeenieHeaderFlag weenieFlag, Position position)
            : base(type, guid)
        {
            Name = name;
            DescriptionFlags = descriptionFlag;
            WeenieFlags = weenieFlag;
            Location = position;
            WeenieClassid = weenieClassId;
        }

        /// <summary>
        /// Load from saved object
        /// </summary>
        /// <param name="baseObject"></param>
        public Container(AceObject baseObject)
            : base(baseObject) { }

        // Inventory Management Functions
        public virtual void AddToInventory(WorldObject inventoryItem)
        {
            if (!inventory.ContainsKey(inventoryItem.Guid))
            {
                inventory.Add(inventoryItem.Guid, inventoryItem);
            }

            Burden += inventoryItem.Burden;
            inventoryItem.ContainerId = Guid.Full;
            if (inventoryItem.Location != null)
                LandblockManager.RemoveObject(inventoryItem);
            inventoryItem.PhysicsData.PhysicsDescriptionFlag &= ~PhysicsDescriptionFlag.Position;
            inventoryItem.PositionFlag = UpdatePositionFlag.None;
            inventoryItem.PhysicsData.Position = null;
            inventoryItem.Location = null;
            inventoryItem.WeenieFlags = inventoryItem.SetWeenieHeaderFlag();
        }

        public virtual void RemoveFromInventory(ObjectGuid inventoryItemGuid)
        {
            WorldObject inventoryItem = GetInventoryItem(inventoryItemGuid);
            if (Burden >= inventoryItem.Burden)
                Burden -= inventoryItem.Burden;
            else
                Burden = 0;

            // FIXME(ddevec): RemoveFromInventory should only be responsible for removing the item from the inventory, not placing it on the world!
            inventoryItem.Location = Location.InFrontOf(1.0f);
            // TODO: Write a method to set this based on data.
            inventoryItem.PositionFlag = UpdatePositionFlag.Contact
                                         | UpdatePositionFlag.Placement
                                         | UpdatePositionFlag.ZeroQy
                                         | UpdatePositionFlag.ZeroQx;

            inventoryItem.PhysicsData.PhysicsDescriptionFlag = inventoryItem.PhysicsData.SetPhysicsDescriptionFlag(inventoryItem);
            inventoryItem.ContainerId = null;
            inventoryItem.Wielder = null;
            inventoryItem.WeenieFlags = inventoryItem.SetWeenieHeaderFlag();
            if (inventory.ContainsKey(inventoryItemGuid))
            {
                inventory.Remove(inventoryItemGuid);
            }
        }

        public ushort UpdateBurden()
        {
            ushort calculatedBurden = 0;
            lock (inventoryMutex)
            {
                foreach (KeyValuePair<ObjectGuid, WorldObject> entry in inventory)
                {
                    calculatedBurden += entry.Value.Burden ?? 0;
                }
            }
            return calculatedBurden;
        }

        public virtual WorldObject GetInventoryItem(ObjectGuid objectGuid)
        {
            if (this.inventory.ContainsKey(objectGuid))
                return this.inventory[objectGuid];
            else
                return null;
        }
    }
}
