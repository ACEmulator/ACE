using ACE.Entity.Actions;

using ACE.Entity;
using ACE.Database;
using ACE.Managers;
using ACE.Network.Enum;
using ACE.Network.GameEvent.Events;
using ACE.Network.Sequence;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

namespace ACE.Factories
{
    public class LootGenerationFactory
    {
        // This is throw away code to understand the world object creation process.

        public static void Spawn(WorldObject inventoryItem, Position position)
        {
            GetSpawnChain(inventoryItem, position).EnqueueChain();
        }

        public static ActionChain GetSpawnChain(WorldObject inventoryItem, Position position)
        {
            inventoryItem.Sequences.GetNextSequence(SequenceType.ObjectTeleport);
            inventoryItem.Sequences.GetNextSequence(SequenceType.ObjectVector);
            inventoryItem.Location = position.InFrontOf(1.00f);
            inventoryItem.PhysicsDescriptionFlag |= PhysicsDescriptionFlag.Position;
            return LandblockManager.GetAddObjectChain(inventoryItem);
        }

        public static Container CreateTestContainerObject(Player player, uint weenieId)
        {
            AceObject aceObject = DatabaseManager.World.GetAceObjectByWeenie(weenieId);
            Container wo = new Container(aceObject);
            player.Session.Network.EnqueueSend(new GameEventViewContents(player.Session, wo.Guid.Full));
            return wo;
        }

        public static WorldObject CreateTestWorldObject(Player player, uint weenieId)
        {
            AceObject aceObject = DatabaseManager.World.GetAceObjectByWeenie(weenieId);
            if (aceObject.GetIntProperty(PropertyInt.ItemsCapacity) >= 1)
            {
                return CreateTestContainerObject(player, weenieId);
            }
            return new DebugObject(aceObject);
        }

        public static void CreateRandomTestWorldObjects(Player player, uint typeId, uint numItems)
        {
            var weenieList = DatabaseManager.World.GetRandomWeeniesOfType(typeId, numItems);
            for (int i = 0; i < numItems; i++)
            {
                WorldObject wo = CreateTestWorldObject(player, weenieList[i].WeenieClassId);
                wo.ContainerId = player.Guid.Full;
                player.HandleAddToInventory(wo);
            }
        }
    }
}