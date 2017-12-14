using ACE.Entity.Actions;
using ACE.Entity;
using ACE.Database;
using ACE.Managers;
using ACE.Network.GameEvent.Events;
using ACE.Network.Sequence;
using ACE.Entity.Enum;
using System.Collections.Generic;

namespace ACE.Factories
{
    public class LootGenerationFactory
    {
        // This is throw away code to understand the world object creation process.

        public static void Spawn(WorldObject inventoryItem, Position position)
        {
            inventoryItem.Sequences.GetNextSequence(SequenceType.ObjectTeleport);
            inventoryItem.Sequences.GetNextSequence(SequenceType.ObjectVector);
            inventoryItem.Location = position.InFrontOf(1.00f);
            inventoryItem.PhysicsDescriptionFlag |= PhysicsDescriptionFlag.Position;
            LandblockManager.AddObject(inventoryItem);
        }

        public static void CreateRandomTestWorldObjects(Player player, uint typeId, uint numItems)
        {
            var weenieList = DatabaseManager.World.GetRandomWeeniesOfType(typeId, numItems);
            List<WorldObject> items = new List<WorldObject>();
            for (int i = 0; i < numItems; i++)
            {
                WorldObject wo = WorldObjectFactory.CreateNewWorldObject(weenieList[i].WeenieClassId);
                items.Add(wo);
            }
            player.HandleAddNewWorldObjectsToInventory(items);
        }
    }
}
