using ACE.Entity.Enum.Properties;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessagePrivateUpdateDataID : GameMessage
    {
        public GameMessagePrivateUpdateDataID(Session session, PropertyDataId property, uint value)
            : base(GameMessageOpcode.PrivateUpdatePropertyDataID, GameMessageGroup.Group09)
        {
            Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdatePropertyDataID));
            Writer.Write((uint)property);
            Writer.Write(value);
        }
    }
}
