using System;
using ACE.Entity.Enum.Properties;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessagePrivateUpdateBool : GameMessage
    {
        public GameMessagePrivateUpdateBool(Session session, PropertyBool property, bool value) : base(GameMessageOpcode.PrivateUpdatePropertyBool)
        {
            Writer.Write(session.UpdatePropertyBoolSequence++);
            Writer.Write(0x19u);
            Writer.Write(1u);
        }
    }
}
