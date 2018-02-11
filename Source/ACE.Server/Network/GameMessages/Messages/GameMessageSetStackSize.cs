using ACE.Entity;
using ACE.Server.Network.Sequence;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessageSetStackSize : GameMessage
    {
        public GameMessageSetStackSize(SequenceManager sequence, ObjectGuid itemGuid, int amount, int newValue)
            : base(GameMessageOpcode.SetStackSize, GameMessageGroup.UIQueue)
        {
            // TODO: need to understand what we really need to send here for sequence it is only a byte
            // and it increments by the number of items popped off the stack in the case of comp burn. Og II
            Writer.Write(sequence.GetNextSequence(SequenceType.SetStackSize)[0]);
            Writer.Write(itemGuid.Full);
            Writer.Write(amount);
            Writer.Write(newValue);
        }
    }
}