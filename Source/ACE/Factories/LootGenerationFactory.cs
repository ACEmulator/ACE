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
            inventoryItem.Location = position.InFrontOf(1.00f);
            inventoryItem.PhysicsData.PhysicsDescriptionFlag |= PhysicsDescriptionFlag.Position;
            return LandblockManager.GetAddObjectChain(inventoryItem);
        }

        public static WorldObject CreateTestWorldObject(Player player, ushort weenieId)
        {
            var aceObject = DatabaseManager.World.GetBaseAceObjectDataByWeenie(weenieId);
            var wo = new DebugObject(new ObjectGuid(CommonObjectFactory.DynamicObjectId, GuidType.None), aceObject);
            return wo;
        }

        public static WorldObject CreateRandomTestWorldObject(Player player, uint typeId)
        {
            var aceObject = DatabaseManager.World.GetRandomWeenieOfType(typeId);
            if (aceObject == null) return null;
            var wo = new DebugObject(new ObjectGuid(CommonObjectFactory.DynamicObjectId, GuidType.None), aceObject);
            return wo;
        }
    }
}