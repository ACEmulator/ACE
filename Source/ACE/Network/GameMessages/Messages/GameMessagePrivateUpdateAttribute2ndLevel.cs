using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Network.Enum;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessagePrivateUpdateAttribute2ndLevel : GameMessage
    {
        public GameMessagePrivateUpdateAttribute2ndLevel(Session session, Vital vital, uint value)
            : base(GameMessageOpcode.PrivateUpdateAttribute2ndLevel, GameMessageGroup.Group09)
        {
            Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateAttribute2ndLevel));
            Writer.Write((uint)vital);
            Writer.Write(value);
        }
    }
}
