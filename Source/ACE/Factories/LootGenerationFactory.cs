namespace ACE.Factories
{
    using Entity;
    using Entity.Enum;

    using Database;

    using Managers;
    using Network.Enum;
    using Network.Sequence;

    public class LootGenerationFactory
    {
        // This is throw away code to understand the world object creation process.

        public static void AddToContainer(WorldObject inventoryItem, Container container)
        {
            inventoryItem.GameData.ContainerId = container.Guid.Full;
            container.GameData.Burden += inventoryItem.GameData.Burden;
            container.AddToInventory(inventoryItem);
        }

        public static void Spawn(WorldObject inventoryItem, Position position)
        {
            inventoryItem.Sequences.GetNextSequence(SequenceType.ObjectTeleport);
            inventoryItem.Sequences.GetNextSequence(SequenceType.ObjectVector);
            inventoryItem.PhysicsData.Position = position.InFrontOf(1.00f);
            inventoryItem.PhysicsData.PhysicsDescriptionFlag |= PhysicsDescriptionFlag.Position;
            OpenWorldManager.OpenWorld.Register(inventoryItem);
        }

        public static WorldObject CreateTestWorldObject(ushort weenieId)
        {
            var aceObject = DatabaseManager.World.GetBaseAceObjectDataByWeenie(weenieId);
            WorldObject wo = new DebugObject(new ObjectGuid(CommonObjectFactory.DynamicObjectId, GuidType.None), aceObject);
            return wo;
        }

        public static WorldObject CreateRandomTestWorldObject(uint typeId)
        {
            var aceObject = DatabaseManager.World.GetRandomBaseAceObjectByTypeId(typeId);
            if (aceObject == null) return null;
            var wo = new DebugObject(new ObjectGuid(CommonObjectFactory.DynamicObjectId, GuidType.None), aceObject);
            return wo;
        }
    }
}