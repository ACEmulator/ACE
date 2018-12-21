using ACE.Entity;
using ACE.Entity.Enum.Properties;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessagePublicUpdatePosition : GameMessage
    {
        public GameMessagePublicUpdatePosition(WorldObject worldObject, PositionType positionType, Position pos)
            : base(GameMessageOpcode.PublicUpdatePosition, GameMessageGroup.UIQueue)
        {
            Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.UpdatePosition, positionType));
            Writer.WriteGuid(worldObject.Guid);
            Writer.Write((uint)positionType);
            pos.Serialize(Writer);
        }
    }
}
