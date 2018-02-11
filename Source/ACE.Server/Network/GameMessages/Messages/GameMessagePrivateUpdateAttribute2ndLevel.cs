using ACE.Entity.Enum;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessagePrivateUpdateAttribute2ndLevel : GameMessage
    {
        public GameMessagePrivateUpdateAttribute2ndLevel(Session session, Vital vital, uint value)
            : base(GameMessageOpcode.PrivateUpdateAttribute2ndLevel, GameMessageGroup.UIQueue)
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
