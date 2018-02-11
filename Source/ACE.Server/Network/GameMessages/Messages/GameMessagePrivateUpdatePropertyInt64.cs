using ACE.Entity.Enum.Properties;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessagePrivateUpdatePropertyInt64 : GameMessage
    {
        public GameMessagePrivateUpdatePropertyInt64(Session session, PropertyInt64 property, ulong value)
            : base(GameMessageOpcode.PrivateUpdatePropertyInt64, GameMessageGroup.UIQueue)
        {
            Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdatePropertyInt64));
            Writer.Write((uint)property);
            Writer.Write(value);
        }
    }
}
