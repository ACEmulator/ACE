using ACE.Entity;
using ACE.Entity.Enum.Properties;
using ACE.Server.Network.Sequence;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessageUpdateInstanceId : GameMessage
    {
        public GameMessageUpdateInstanceId(SequenceManager sequences, ObjectGuid containerGuid, ObjectGuid itemGuid, PropertyInstanceId iidPropertyId)
            : base(GameMessageOpcode.PublicUpdateInstanceId, GameMessageGroup.UIQueue)
        {
            Writer.Write(sequences.GetNextSequence(SequenceType.PublicUpdatePropertyInstanceId));  // wts
            Writer.Write(itemGuid.Full); // sender
            Writer.Write((uint)iidPropertyId);
            Writer.Write(containerGuid.Full); // new value of the container id
        }

        //public GameMessageUpdateInstanceId(SequenceManager sequences, ObjectGuid senderGuid, PropertyInstanceId idPropertyId, ObjectGuid value)
        //    : base(GameMessageOpcode.PublicUpdateInstanceId, GameMessageGroup.UIQueue)
        //{
        //    Writer.Write(sequences.GetNextSequence(SequenceType.PublicUpdatePropertyInstanceId));  // wts
        //    Writer.Write(senderGuid.Full);
        //    Writer.Write((uint)idPropertyId);
        //    Writer.Write(value.Full);
        //}
    }
}
