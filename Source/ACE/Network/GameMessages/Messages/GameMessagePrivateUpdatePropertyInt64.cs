
using ACE.Entity.Enum.Properties;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessagePrivateUpdatePropertyInt64 : GameMessage
    {
        public GameMessagePrivateUpdatePropertyInt64(Session session, PropertyInt64 property, ulong value) : base(GameMessageOpcode.PrivateUpdatePropertyInt64)
        {
            Writer.Write(session.UpdatePropertyInt64Sequence++);
            Writer.Write((uint)property);
            Writer.Write(value);
        }
    }
}
