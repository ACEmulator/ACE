using ACE.Entity.Enum.Properties;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessagePrivateUpdatePropertyDouble : GameMessage
    {
        public GameMessagePrivateUpdatePropertyDouble(Session session, PropertyFloat property, double value)
            : base(GameMessageOpcode.PrivateUpdatePropertyDouble, GameMessageGroup.UIQueue)
        {
            Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdatePropertyDouble));
            Writer.Write((uint)property);
            Writer.Write(value);
        }
    }
}
