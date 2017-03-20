using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Network.Enum;

namespace ACE.Factories
{
    /// <summary>
    /// factory class for creating objects related to administration or content-recreation
    /// </summary>
    public class LifestoneObjectFactory
    {
        /// <summary>
        /// creates a lifestone directly in fron of the player's position provided
        /// </summary>
        public static ImmutableWorldObject CreateLifestone(ushort weenieClassId, Position newPosition, LifestoneType lifestoneType)
        {
<<<<<<< HEAD:Source/ACE/Factories/AdminObjectFactory.cs
            var weenie = WeenieHeaderFlag.Usable | WeenieHeaderFlag.BlipColour | WeenieHeaderFlag.Radar |
                         WeenieHeaderFlag.UseRadius;
            ImmutableWorldObject wo = new ImmutableWorldObject(ObjectType.LifeStone,
                new ObjectGuid(CommonObjectFactory.DynamicObjectId, GuidType.None), "Life Stone", 0,
                ObjectDescriptionFlag.LifeStone, weenie, newPosition);
=======
            var weenie = WeenieHeaderFlag.Usable | WeenieHeaderFlag.BlipColour | WeenieHeaderFlag.Radar | WeenieHeaderFlag.UseRadius;
            ImmutableWorldObject wo = new ImmutableWorldObject(ObjectType.LifeStone, new ObjectGuid(CommonObjectFactory.DynamicObjectId, GuidType.None), "Life Stone", weenieClassId, ObjectDescriptionFlag.LifeStone, weenie, newPosition);
>>>>>>> upstream/master:Source/ACE/Factories/LifestoneObjectFactory.cs

            // model id 0x000026 is one of several lifestone IDs
            wo.PhysicsData.MTableResourceId = 0x09000026u;
            wo.PhysicsData.Stable = 0x20000014u;
<<<<<<< HEAD:Source/ACE/Factories/AdminObjectFactory.cs
            wo.PhysicsData.CSetup = (uint) 0x020002EEu;
=======
            wo.PhysicsData.CSetup = (uint)lifestoneType;
>>>>>>> upstream/master:Source/ACE/Factories/LifestoneObjectFactory.cs

            wo.PhysicsData.PhysicsDescriptionFlag = PhysicsDescriptionFlag.CSetup | PhysicsDescriptionFlag.MTable |
                                                    PhysicsDescriptionFlag.Stable | PhysicsDescriptionFlag.Position;

            wo.PhysicsData.PhysicsState = PhysicsState.IgnoreCollision | PhysicsState.Gravity;

            //game data min required flags;
            wo.GameData.Type = (ushort) 0x1355;
            wo.GameData.Icon = (ushort) 0x1036;

            wo.GameData.Usable = Usable.UsableRemote;
            wo.GameData.RadarColour = RadarColor.Blue;
            wo.GameData.RadarBehavior = RadarBehavior.ShowAlways;
            wo.GameData.UseRadius = 4f;

            return wo;
        }

        public static WorldObject CreateTrainingWand(Player newPlayer)
        {
            const WeenieHeaderFlag weenie =
                WeenieHeaderFlag.Usable | WeenieHeaderFlag.HookItemTypes | WeenieHeaderFlag.Value |
                WeenieHeaderFlag.TargetType | WeenieHeaderFlag.Container | WeenieHeaderFlag.Wielder |
                WeenieHeaderFlag.Burden;

            var wo = new ImmutableWorldObject(ObjectType.Caster,
                new ObjectGuid(CommonObjectFactory.DynamicObjectId, GuidType.None), "Training Wand", 1,
                ObjectDescriptionFlag.Inscribable, weenie, newPlayer.Position);

            wo.GameData.ContainerId = newPlayer.Guid.Full;
            wo.GameData.Icon = 0x2A3C;
            wo.Icon = 0x2A3C;
            wo.GameData.Value = 25;
            wo.GameData.Burden = 50;
            wo.GameData.TargetType = 16;
            wo.GameData.Type = 0x31CC;

            wo.GameData.Usable = Usable.UsableNo;

            wo.PhysicsData.PhysicsDescriptionFlag = PhysicsDescriptionFlag.Stable | PhysicsDescriptionFlag.Petable
                                                    | PhysicsDescriptionFlag.CSetup |
                                                    PhysicsDescriptionFlag.AnimationFrame;

            wo.PhysicsData.AnimationFrame = 0x00000065;

            wo.PhysicsData.EquipperPhysicsDescriptionFlag = EquipMask.Wand;
            wo.PhysicsData.Stable = 0x20000014;
            wo.PhysicsData.Petable = 0x3400002B;
            wo.PhysicsData.CSetup = 0x2000ED7;
            //wo.ModelData.
            wo.ModelData.AddTexture(0, 0xC3, 0xC4);
            wo.ModelData.AddModel(0, 0x2D7C);
            wo.PhysicsData.PhysicsState = PhysicsState.Gravity | PhysicsState.IgnoreCollision | PhysicsState.Ethereal;
            
            // This is probably not the right way to do this all of this infromation is sent in the message - OG II
            newPlayer.GameData.Burden += wo.GameData.Burden;

            return wo;
        }

    }
}