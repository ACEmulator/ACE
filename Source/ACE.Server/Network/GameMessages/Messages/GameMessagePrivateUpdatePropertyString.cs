using ACE.Entity.Enum.Properties;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessagePrivateUpdatePropertyString : GameMessage
    {
        public GameMessagePrivateUpdatePropertyString(Session session, PropertyString property, string value)
            : base(GameMessageOpcode.PrivateUpdatePropertyString, GameMessageGroup.UIQueue)
        {
            Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdatePropertyString));
            Writer.Write((uint)property);
            Writer.WriteString16L(value);
        }
    }
}
