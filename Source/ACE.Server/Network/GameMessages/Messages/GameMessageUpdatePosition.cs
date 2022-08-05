using System;
using ACE.Server.Network.Structure;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessageUpdatePosition : GameMessage
    {
        public PositionPack PositionPack;

        public GameMessageUpdatePosition(WorldObject worldObject, bool adminMove = false)
            : base(GameMessageOpcode.UpdatePosition, GameMessageGroup.SmartboxQueue)
        {
            //Console.WriteLine($"Sending UpdatePosition for {worldObject.Name}");

            // todo: avoid create intermediate object
            PositionPack = new PositionPack(worldObject, adminMove);

            Writer.WriteGuid(worldObject.Guid);
            Writer.Write(PositionPack);
        }
    }
}
