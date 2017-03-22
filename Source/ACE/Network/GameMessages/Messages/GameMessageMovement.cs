using ACE.Entity;

namespace ACE.Network.GameMessages.Messages
{

    public class GameMessageMovement : GameMessage
    {
        public GameMessageMovement(WorldObject worldObject, ushort instanceTimestamp, ushort movementTimestamp, ushort serverControlTimestamp, byte autonomous)
            : base(GameMessageOpcode.Motion, GameMessageGroup.Group0A)
        {
            var player = worldObject as Player;
            if (player == null) return;
            var p = player;
            this.Writer.WriteGuid(p.Guid);
            // p.Position.Serialize(this.Writer, true, false);
            this.Writer.Write(instanceTimestamp); // instance_timestamp - seems to be the same for a group of movements //TODO: research and implement
            this.Writer.Write(movementTimestamp); // movement_timestamp - sequenced within a instance - play order check in addition to order received 
            this.Writer.Write(serverControlTimestamp); // server_control_timestamp - seems to increment on each call 

            this.Writer.Write(autonomous);
            this.Writer.Write((byte)0);
            this.Writer.Write((byte)0);
            this.Writer.Write((byte)0);
            this.Writer.Write((ushort)0x03D); // command ?
            this.Writer.Write((byte)0);
            this.Writer.Write((byte)0);
            this.Writer.Write((byte)0);
            this.Writer.Write((ushort)1);
            this.Writer.Write((ushort)0);
            this.Writer.Write((ushort)0);
            this.Writer.Write((ushort)0);
            this.Writer.Write((ushort)0);
            this.Writer.Write((ushort)0);
            this.Writer.Write((ushort)0);
            this.Writer.Write((ushort)0);
            this.Writer.Write((ushort)0);
        }
    }
}
