using ACE.Entity;
using ACE.Network.Sequence;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessageSetStackSize : GameMessage
    {
        public GameMessageSetStackSize(SequenceManager sequence, ObjectGuid itemGuid, int amount, int newValue)
            : base(GameMessageOpcode.SetStackSize, GameMessageGroup.Group09)
        {
            Writer.Write(sequence.GetNextSequence(SequenceType.ObjectInstance)[0]);
            Writer.Write(itemGuid.Full);
            Writer.Write(amount);
            Writer.Write(newValue);
        }
    }
}