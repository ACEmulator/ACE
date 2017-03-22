using ACE.Entity.Enum.Properties;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessagePrivateUpdatePropertyInt : GameMessage
    {
        public GameMessagePrivateUpdatePropertyInt(Session session, PropertyInt property, uint value)
            : base(GameMessageOpcode.PrivateUpdatePropertyInt, GameMessageGroup.Group09)
        {
            Writer.Write(session.UpdatePropertyIntSequence++);
            Writer.Write((uint)property);
            Writer.Write(value);
        }
    }
}
