using ACE.Entity.Enum;
using ACE.Network.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity
{
    public class Container : WorldObject
    {
        private readonly Dictionary<ObjectGuid, WorldObject> inventory = new Dictionary<ObjectGuid, WorldObject>();

        private readonly object inventoryMutex = new object();

        public Container(ObjectType type, ObjectGuid guid, string name, ushort weenieClassId, ObjectDescriptionFlag descriptionFlag, WeenieHeaderFlag weenieFlag, Position position)
            : base(type, guid)
        {
            this.Name = name;
            this.DescriptionFlags = descriptionFlag;
            this.WeenieFlags = weenieFlag;
            this.Location = position;
            this.WeenieClassid = weenieClassId;
        }

        // Inventory Management Functions
        public virtual void AddToInventory(WorldObject inventoryItem)
        {
            lock (inventoryMutex)
            {
                if (!inventory.ContainsKey(inventoryItem.Guid))
                {
                    inventory.Add(inventoryItem.Guid, inventoryItem);
                }

                GameData.Burden += inventoryItem.GameData.Burden;
                inventoryItem.PositionFlag = UpdatePositionFlag.Contact | UpdatePositionFlag.ZeroQy | UpdatePositionFlag.ZeroQx;
                inventoryItem.GameData.ContainerId = Guid.Full;
                inventoryItem.PhysicsData.PhysicsDescriptionFlag &= PhysicsDescriptionFlag.Position;
            }
        }

        public virtual void RemoveFromInventory(ObjectGuid inventoryItemGuid)
        {
            var inventoryItem = GetInventoryItem(inventoryItemGuid);
            GameData.Burden -= inventoryItem.GameData.Burden;
            inventoryItem.PhysicsData.PhysicsDescriptionFlag = PhysicsDescriptionFlag.Stable | PhysicsDescriptionFlag.Petable
                                                                | PhysicsDescriptionFlag.CSetup | PhysicsDescriptionFlag.AnimationFrame
                                                                | PhysicsDescriptionFlag.Position;
            inventoryItem.PositionFlag = UpdatePositionFlag.Contact
                                           | UpdatePositionFlag.Placement
                                           | UpdatePositionFlag.ZeroQy
                                           | UpdatePositionFlag.ZeroQx;
            inventoryItem.PhysicsData.Position = PhysicsData.Position.InFrontOf(0.50f);
            inventoryItem.GameData.ContainerId = 0;
            inventoryItem.GameData.Wielder = 0;

            lock (inventoryMutex)
            {
                if (this.inventory.ContainsKey(inventoryItemGuid))
                    this.inventory.Remove(inventoryItemGuid);
            }
        }

        public virtual WorldObject GetInventoryItem(ObjectGuid objectGuid)
        {
            lock (inventoryMutex)
            {
                if (this.inventory.ContainsKey(objectGuid))
                    return this.inventory[objectGuid];
                else
                    return null;
            }
        }
    }
}
