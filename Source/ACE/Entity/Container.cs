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

                GameData.Burden += inventoryItem.GameData.Burden;
                inventoryItem.GameData.ContainerId = Guid.Full;
                if (inventoryItem.Location != null)
                    LandblockManager.RemoveObject(inventoryItem);
                inventoryItem.PhysicsData.PhysicsDescriptionFlag &= ~PhysicsDescriptionFlag.Position;
                inventoryItem.PositionFlag = UpdatePositionFlag.None;
                inventoryItem.PhysicsData.Position = null;
            }
        }

        public virtual void RemoveFromInventory(ObjectGuid inventoryItemGuid)
        {
            var inventoryItem = GetInventoryItem(inventoryItemGuid);
            Burden -= inventoryItem.Burden;

            inventoryItem.PhysicsData.Position = PhysicsData.Position.InFrontOf(1.0f);
            // TODO: Write a method to set this based on data.
            inventoryItem.PositionFlag = UpdatePositionFlag.Contact
                                         | UpdatePositionFlag.Placement
                                         | UpdatePositionFlag.ZeroQy
                                         | UpdatePositionFlag.ZeroQx;

            inventoryItem.PhysicsData.PhysicsDescriptionFlag = inventoryItem.PhysicsData.SetPhysicsDescriptionFlag(inventoryItem);
            inventoryItem.ContainerId = null;
            inventoryItem.Wielder = null;
            inventoryItem.WeenieFlags = inventoryItem.SetWeenieHeaderFlag();

            if (this.inventory.ContainsKey(inventoryItemGuid))
                this.inventory.Remove(inventoryItemGuid);
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
