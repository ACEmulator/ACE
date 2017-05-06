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
            // I think this is right Og II
            inventoryItem.PhysicsData.PhysicsDescriptionFlag |= PhysicsDescriptionFlag.Parent & ~PhysicsDescriptionFlag.Position;
            inventoryItem.PhysicsData.Position = null;
            container.GameData.Burden += inventoryItem.GameData.Burden;
            container.AddToInventory(inventoryItem);
        }

        public static void Spawn(WorldObject inventoryItem, Position position)
        {
            inventoryItem.Sequences.GetNextSequence(SequenceType.ObjectTeleport);
            inventoryItem.Sequences.GetNextSequence(SequenceType.ObjectVector);
            inventoryItem.PhysicsData.Position = position.InFrontOf(1.00f);
            inventoryItem.PhysicsData.PhysicsDescriptionFlag |= PhysicsDescriptionFlag.Position;
            LandblockManager.AddObject(inventoryItem);
        }

        public static WorldObject CreateTestWorldObject(Player player, ushort weenieId)
        {
            var aceObject = DatabaseManager.World.GetBaseAceObjectDataByWeenie(weenieId);

            var wo = new DebugObject(new ObjectGuid(CommonObjectFactory.DynamicObjectId, GuidType.None), aceObject);
            return wo;
        }
    }
}