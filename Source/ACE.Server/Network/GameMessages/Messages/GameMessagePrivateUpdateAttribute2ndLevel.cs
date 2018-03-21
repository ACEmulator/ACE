using ACE.Entity.Enum;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessagePrivateUpdateAttribute2ndLevel : GameMessage
    {
        public GameMessagePrivateUpdateAttribute2ndLevel(WorldObject worldObject, Vital vital, uint value)
            : base(GameMessageOpcode.PrivateUpdateAttribute2ndLevel, GameMessageGroup.UIQueue)
        {
            switch (vital)
            {
                case Vital.Health:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateAttribute2ndLevelHealth));
                    break;
                case Vital.Stamina:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateAttribute2ndLevelStamina));
                    break;
                case Vital.Mana:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateAttribute2ndLevelMana));
                    break;
                default:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateAttribute2ndLevel));
                    break;
            }
            Writer.Write((uint)vital);
            Writer.Write(value);
        }
    }
}
