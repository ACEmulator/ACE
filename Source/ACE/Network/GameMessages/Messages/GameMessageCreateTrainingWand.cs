using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Network.Enum;

namespace ACE.Network.GameMessages.Messages
{
    using System.Net;

    public class GameMessageCreateTrainingWand : GameMessage
    {
        private static uint nextObjectId = 100;

        /// <summary>
        /// creates a Training Wand in the users main backpack.
        /// </summary>
        public GameMessageCreateTrainingWand(Player player) : base(GameMessageOpcode.ObjectCreate, GameMessageGroup.Group0A)
        {
            var containerId = player.Guid.Full;
            const WeenieHeaderFlag Weenie =
                WeenieHeaderFlag.Usable | WeenieHeaderFlag.HookItemTypes | WeenieHeaderFlag.Value | WeenieHeaderFlag.TargetType
                | WeenieHeaderFlag.Container | WeenieHeaderFlag.Wielder | WeenieHeaderFlag.Burden;

            var wo = new ImmutableWorldObject(ObjectType.Caster, new ObjectGuid(nextObjectId++, GuidType.None), "Training Wand", 1, ObjectDescriptionFlag.Inscribable, Weenie, null);
            
            wo.GameData.ContainerId = containerId;
            wo.GameData.Icon = 0x2A3C;
            wo.Icon = 0x2A3C;
            wo.GameData.Value = 25;
            wo.GameData.Burden = 50;
            wo.GameData.TargetType = 16;
            wo.GameData.Type = 12748;

            wo.GameData.Usable = Usable.UsableNo;

            wo.PhysicsData.PhysicsDescriptionFlag = PhysicsDescriptionFlag.Stable | PhysicsDescriptionFlag.Petable
                                                    | PhysicsDescriptionFlag.CSetup | PhysicsDescriptionFlag.AnimationFrame;

            wo.PhysicsData.AnimationFrame = 0x00000065;

            wo.PhysicsData.EquipperPhysicsDescriptionFlag = EquipMask.Wand;
            wo.PhysicsData.Stable = 536870932u;
            wo.PhysicsData.Petable = 872415275u;
            wo.PhysicsData.CSetup = 33558231u;

            wo.ModelData.AddTexture(0, 0xC3, 0xC4);
            wo.ModelData.AddModel(0, 0x2D7C);
            wo.PhysicsData.PhysicsState = PhysicsState.Gravity | PhysicsState.IgnoreCollision | PhysicsState.Ethereal;

            wo.SerializeCreateObject(this.Writer);
                       
        }
    }
}