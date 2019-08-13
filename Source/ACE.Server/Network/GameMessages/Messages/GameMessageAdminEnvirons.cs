using ACE.Entity.Enum;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessageAdminEnvirons : GameMessage
    {
        public GameMessageAdminEnvirons(Session session, EnvironChangeType environChange = EnvironChangeType.Clear)
            : base(GameMessageOpcode.AdminEnvirons, GameMessageGroup.UIQueue)
        {
            Writer.Write((uint)environChange);
        }
    }
}
