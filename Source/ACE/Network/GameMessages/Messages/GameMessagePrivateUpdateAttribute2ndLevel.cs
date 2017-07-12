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
            switch (vital)
            {
                case Vital.Health:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateAttribute2ndLevelHealth));
                    break;
                case Vital.Stamina:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateAttribute2ndLevelStamina));
                    break;
                case Vital.Mana:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateAttribute2ndLevelMana));
                    break;
                default:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateAttribute2ndLevel));
                    break;
            }
            Writer.Write((uint)vital);
            Writer.Write(value);
        }
    }
}
