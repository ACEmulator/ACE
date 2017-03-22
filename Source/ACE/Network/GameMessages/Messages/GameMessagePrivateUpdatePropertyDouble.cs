using ACE.Entity.Enum.Properties;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessagePrivateUpdatePropertyDouble : GameMessage
    {
        public GameMessagePrivateUpdatePropertyDouble(Session session, PropertyDouble property, double value) 
            : base(GameMessageOpcode.PrivateUpdatePropertyDouble, GameMessageGroup.Group09)
        {
            Writer.Write(session.UpdatePropertyDoubleSequence++);
            Writer.Write((uint)property);
            Writer.Write(value);
        }
    }
}
