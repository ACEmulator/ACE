using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Network.Enum;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessageTestWorldPackage : GameMessage
    {
        private static uint nextObjectId = 100;

        public GameMessageTestWorldPackage(Player player) : base(GameMessageOpcode.ObjectCreate, GameMessageGroup.Group0A)
        {
            Position newPosition = new Position(player.Position.Cell, player.Position.Offset.X, player.Position.Offset.Y, player.Position.Offset.Z, player.Position.Facing.X, player.Position.Facing.Y, player.Position.Facing.Z, player.Position.Facing.W);
            newPosition.Offset = new System.Numerics.Vector3(player.Position.Offset.X, player.Position.Offset.Y, player.Position.Offset.Z + 1.5f);
            newPosition.Facing = new System.Numerics.Quaternion(0, 0, 0, 0);

            var weenie = WeenieHeaderFlag.Useability | WeenieHeaderFlag.BlipColour | WeenieHeaderFlag.Radar | WeenieHeaderFlag.UseRadius;
            ImmutableWorldObject wo = new ImmutableWorldObject(ObjectType.LifeStone, new ObjectGuid(nextObjectId++, GuidType.None), "Life Stone", ObjectDescriptionFlag.LifeStone, weenie, newPosition);

            // model id 0x000026
            wo.PhysicsData.MTableResourceId = 0x09000026u;
            wo.PhysicsData.Stable = 0x20000014u;
            wo.PhysicsData.CSetup = (uint)0x020002EEu;

            //game data min required flags;
            wo.GameData.Type = (ushort)0x1355;
            wo.GameData.Icon = (ushort)0x1036;

            wo.GameData.Useability = (uint)32;
            wo.GameData.RadarColour = RadarColor.Blue;
            wo.GameData.RadarBehavior = RadarBehavior.ShowAlways;
            wo.GameData.UseRadius = 4f;

            wo.SerializeCreateObject(Writer);
        }
    }
}