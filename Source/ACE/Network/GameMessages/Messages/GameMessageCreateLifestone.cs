using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Network.Enum;
using System;
using System.Numerics;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessageCreateLifestone : GameMessage
    {
        private static uint nextObjectId = 100;
        private static readonly float maxXY = Convert.ToSingle(Math.Sqrt(2) / 2);
        private static readonly float minXY = -1 * Convert.ToSingle(Math.Sqrt(2) / 2);

        /// <summary>
        /// creates a lifesone 3 units (just in front of) the player.
        /// </summary>
        public GameMessageCreateLifestone(Player player) : base(GameMessageOpcode.ObjectCreate, GameMessageGroup.Group0A)
        {
            float qw = player.Position.Facing.W; // north
            float qz = player.Position.Facing.Z; // south
            
            double x = 2 * qw * qz;
            double y = 1 - 2 * qz * qz;

            var heading = Math.Atan2(x, y);
            double scalar = 3.0f;
            var dx = -1 * Convert.ToSingle(Math.Sin(heading) * scalar);
            var dy = Convert.ToSingle(Math.Cos(heading) * scalar);

            Position newPosition = new Position(player.Position.Cell, player.Position.Offset.X + dx, player.Position.Offset.Y + dy, player.Position.Offset.Z + 0.5f, 0f, 0f, 0f, 0f);
            
            var weenie = WeenieHeaderFlag.Usable | WeenieHeaderFlag.BlipColour | WeenieHeaderFlag.Radar | WeenieHeaderFlag.UseRadius;
            ImmutableWorldObject wo = new ImmutableWorldObject(ObjectType.LifeStone, new ObjectGuid(nextObjectId++, GuidType.None), "Life Stone", ObjectDescriptionFlag.LifeStone, weenie, newPosition);

            // model id 0x000026
            wo.PhysicsData.MTableResourceId = 0x09000026u;
            wo.PhysicsData.Stable = 0x20000014u;
            wo.PhysicsData.CSetup = (uint)0x020002EEu;

            wo.PhysicsData.PhysicsDescriptionFlag = PhysicsDescriptionFlag.CSetup | PhysicsDescriptionFlag.MTable | PhysicsDescriptionFlag.Stable | PhysicsDescriptionFlag.Position;

            //game data min required flags;
            wo.GameData.Type = (ushort)0x1355;
            wo.GameData.Icon = (ushort)0x1036;

            wo.GameData.Usable = Usable.UsableRemote;
            wo.GameData.RadarColour = RadarColor.LifeStone;
            wo.GameData.RadarBehavior = RadarBehavior.ShowAlways;
            wo.GameData.UseRadius = 4f;

            wo.SerializeCreateObject(Writer);
        }
    }
}