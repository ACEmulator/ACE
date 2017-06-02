using ACE.Entity.Actions;

using ACE.Entity;
using ACE.Database;
using ACE.Managers;
using ACE.Network.Enum;
using ACE.Network.Sequence;

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
            inventoryItem.PhysicsData.Position = position.InFrontOf(1.00f);
            inventoryItem.PhysicsData.PhysicsDescriptionFlag |= PhysicsDescriptionFlag.Position;
            return LandblockManager.GetAddObjectChain(inventoryItem);
        }

        public static WorldObject CreateTestWorldObject(Player player, uint weenieId)
        {
            AceObject aceObject = DatabaseManager.World.GetAceObjectByWeenie(weenieId);
            DebugObject wo = new DebugObject(new ObjectGuid(CommonObjectFactory.DynamicObjectId, GuidType.None), aceObject);
            return wo;
        }

        public static void CreateRandomTestWorldObjects(Player player, uint typeId, uint numItems)
        {
            var weenieList = DatabaseManager.World.GetRandomWeeniesOfType(typeId, numItems);
            for (int i = 0; i < numItems; i++)
            {
                WorldObject wo = CreateTestWorldObject(player, weenieList[i].WeenieClassId);
                player.AddToInventory(wo);
                player.TrackObject(wo);
            }
            player.UpdatePlayerBurden();
        }
    }
}