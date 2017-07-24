using System;
using ACE.Entity.Enum.Properties;
using ACE.Entity;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessageUpdatePropertyBool : GameMessage
    {
        public GameMessageUpdatePropertyBool(WorldObject worldObject, PropertyBool property, bool value) : base(GameMessageOpcode.PublicUpdatePropertyBool, GameMessageGroup.Group09)
        {
            // Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdatePropertyBool));
            Writer.Write(0u); // Public updates seem to always be 0? It's possible they're worldObject.Sequences.GetCurrentSequence(Sequence.SequenceType.PrivateUpdatePropertyBool) as well
            Writer.Write(worldObject.Guid.Full);
            Writer.Write((uint)property);
            Writer.Write(Convert.ToUInt32(value));
        }
    }
}
