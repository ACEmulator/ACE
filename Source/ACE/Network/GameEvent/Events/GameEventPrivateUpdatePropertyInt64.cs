
using ACE.Entity.Enum.Properties;

namespace ACE.Network.GameEvent.Events
{
    public class GameEventPrivateUpdatePropertyInt64 : GameEventPacket
    {
        private PropertyInt64 property;
        private ulong value;

        public override GameEventOpcode Opcode { get { return GameEventOpcode.PrivateUpdatePropertyInt64; } }

        public GameEventPrivateUpdatePropertyInt64(Session session, PropertyInt64 property, ulong value) 
            : base(session)
        {
            this.property = property;
            this.value = value;
        }

        protected override void WriteEventBody()
        {
            fragment.Payload.Write(session.UpdatePropertyInt64Sequence++);
            fragment.Payload.Write((uint)property);
            fragment.Payload.Write(value);
        }
    }
}
