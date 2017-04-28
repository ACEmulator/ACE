namespace ACE.Factories
{
    using Entity;
    using Entity.Enum;
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
            // sending positon of the container so we know what landblock to register with.
            inventoryItem.PhysicsData.Position = container.PhysicsData.Position;
        }

        public static void Spawn(WorldObject inventoryItem, Position position)
        {
            inventoryItem.Sequences.GetNextSequence(SequenceType.ObjectTeleport);
            inventoryItem.Sequences.GetNextSequence(SequenceType.ObjectVector);
            inventoryItem.PhysicsData.Position = position.InFrontOf(1.00f);
            inventoryItem.PhysicsData.PhysicsDescriptionFlag = PhysicsDescriptionFlag.Position |
                                                               inventoryItem.PhysicsData.PhysicsDescriptionFlag;
            LandblockManager.AddObject(inventoryItem);
        }

        public static WorldObject CreateTrainingWand(Player player)
        {
            const WeenieHeaderFlag weenie =
                WeenieHeaderFlag.Usable | WeenieHeaderFlag.HookItemTypes | WeenieHeaderFlag.Value |
                WeenieHeaderFlag.TargetType | WeenieHeaderFlag.Container | WeenieHeaderFlag.Wielder |
                WeenieHeaderFlag.Burden;

            const ushort WandTraining = 12748;

            var wo = new UsableObject(ObjectType.Caster,
                new ObjectGuid(CommonObjectFactory.DynamicObjectId, GuidType.None), "Training Wand", WandTraining,
                ObjectDescriptionFlag.Inscribable, weenie, null);

            // Todo: Swap this out and read from Weenie database

            wo.Icon = 0x2A3C;
            wo.GameData.Value = 25;
            wo.GameData.Burden = 50;
            wo.GameData.TargetType = 16;
            wo.GameData.Type = 0x31CC;  // TODO: Magic number need to research

            wo.GameData.Usable = Usable.UsableNo;

            wo.PhysicsData.PhysicsDescriptionFlag = PhysicsDescriptionFlag.Stable | PhysicsDescriptionFlag.Petable
                                                    | PhysicsDescriptionFlag.CSetup |
                                                    PhysicsDescriptionFlag.AnimationFrame;

            wo.PhysicsData.AnimationFrame = 0x00000065;

            wo.PhysicsData.EquipperPhysicsDescriptionFlag = EquipMask.Wand;
            wo.PhysicsData.Stable = 0x20000014;
            wo.PhysicsData.Petable = 0x3400002B;
            wo.PhysicsData.CSetup = 0x2000ED7;

            wo.ModelData.AddTexture(0, 0xC3, 0xC4);
            wo.ModelData.AddModel(0, 0x2D7C);
            wo.PhysicsData.PhysicsState = PhysicsState.Gravity | PhysicsState.IgnoreCollision | PhysicsState.Ethereal;

            return wo;
        }
    }
}