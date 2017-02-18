
using ACE.Entity.Enum.Properties;

namespace ACE.Network.Messages
{
    public class GameMessagePrivateUpdatePropertyInt64 : GameMessage
    {
        private PropertyInt64 property;
        private ulong value;

        public GameMessagePrivateUpdatePropertyInt64(Session session, PropertyInt64 property, ulong value) 
            : base(GameMessageOpcode.PrivateUpdatePropertyInt64)
        {
            this.property = property;
            this.value = value;
            writer.Write(session.UpdatePropertyInt64Sequence++);
            writer.Write((uint)property);
            writer.Write(value);
        }
    }
}
