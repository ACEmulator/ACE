using System;
using ACE.Entity.Enum.Properties;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessagePrivateUpdatePropertyBool : GameMessage
    {
        public GameMessagePrivateUpdatePropertyBool(Session session, PropertyBool property, bool value) : base(GameMessageOpcode.PrivateUpdatePropertyBool, GameMessageGroup.Group09)
        {
            Writer.Write(session.UpdatePropertyBoolSequence++);
            Writer.Write((uint)property);
            Writer.Write(Convert.ToUInt32(value));
        }
    }
}
