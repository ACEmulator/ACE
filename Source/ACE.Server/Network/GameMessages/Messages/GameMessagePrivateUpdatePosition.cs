using ACE.Entity;
using ACE.Entity.Enum.Properties;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessagePrivateUpdatePosition : GameMessage
    {
        public GameMessagePrivateUpdatePosition(WorldObject worldObject, PositionType positionType, Position pos)
            : base(GameMessageOpcode.PrivateUpdatePosition, GameMessageGroup.UIQueue, 41)
        {
            Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.UpdatePosition, positionType));
            Writer.Write((uint)positionType);
            pos.Serialize(Writer);
        }
    }
}
