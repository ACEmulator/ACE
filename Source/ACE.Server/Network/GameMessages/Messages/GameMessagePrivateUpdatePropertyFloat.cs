using ACE.Entity.Enum.Properties;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessagePrivateUpdatePropertyFloat : GameMessage
    {
        public GameMessagePrivateUpdatePropertyFloat(Session session, PropertyFloat property, double value)
            : base(GameMessageOpcode.PrivateUpdatePropertyFloat, GameMessageGroup.UIQueue)
        {
            Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdatePropertyDouble));
            Writer.Write((uint)property);
            Writer.Write(value);
        }
    }
}
