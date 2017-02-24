
using ACE.Entity;
using ACE.Entity.Enum;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessageSetState : GameMessage
    {
        public GameMessageSetState(ObjectGuid guid, PhysicsState state, uint logins, uint portals) : base(GameMessageOpcode.SetState, 0xA)
        {
            Writer.WriteGuid(guid);
            Writer.Write((uint)state);
            Writer.Write((ushort)logins);
            Writer.Write((ushort)portals);
        }
    }
}