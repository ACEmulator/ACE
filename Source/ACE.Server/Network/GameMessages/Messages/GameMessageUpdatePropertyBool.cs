using System;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessageUpdatePropertyBool : GameMessage
    {
        public GameMessageUpdatePropertyBool(WorldObject worldObject, PropertyBool property, bool value) : base(GameMessageOpcode.PublicUpdatePropertyBool, GameMessageGroup.Group09)
        {
            Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdatePropertyBool)); // This might need to change to GetCurrentSequence.. 
            Writer.Write(worldObject.Guid.Full);
            Writer.Write((uint)property);
            Writer.Write(Convert.ToUInt32(value));
        }
    }
}
