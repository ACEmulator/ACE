﻿using ACE.Entity.Enum;
using ACE.Network.Enum;
using System.Collections.Generic;

namespace ACE.Entity
{
    public class Container : WorldObject
    {
        private readonly Dictionary<ObjectGuid, WorldObject> inventory = new Dictionary<ObjectGuid, WorldObject>();

        private readonly object inventoryMutex = new object();

        /// <summary>
        /// Load from template
        /// </summary>
        /// <param name="type"></param>
        /// <param name="guid"></param>
        /// <param name="name"></param>
        /// <param name="weenieClassId"></param>
        /// <param name="descriptionFlag"></param>
        /// <param name="weenieFlag"></param>
        /// <param name="position"></param>
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
            lock (inventoryMutex)
            {
                if (!inventory.ContainsKey(inventoryItem.Guid))
                {
                    inventory.Add(inventoryItem.Guid, inventoryItem);
                }

                Burden += inventoryItem.Burden;
                inventoryItem.ContainerId = Guid.Full;
                inventoryItem.PhysicsData.Position = null;
                inventoryItem.Location = null;
                inventoryItem.WeenieFlags = inventoryItem.SetWeenieHeaderFlag();
            }
        }

        public virtual void RemoveFromInventory(ObjectGuid inventoryItemGuid)
        {
            var inventoryItem = GetInventoryItem(inventoryItemGuid);
            if (Burden >= inventoryItem.Burden)
                Burden -= inventoryItem.Burden;

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

            lock (inventoryMutex)
            {
                if (inventory.ContainsKey(inventoryItemGuid))
                    inventory.Remove(inventoryItemGuid);
            }
        }

        public virtual WorldObject GetInventoryItem(ObjectGuid objectGuid)
        {
            lock (inventoryMutex)
            {
                if (inventory.ContainsKey(objectGuid))
                    return inventory[objectGuid];
                return null;
            }
        }
    }
}
