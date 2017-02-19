
using ACE.Command;
using ACE.Common.Extensions;

namespace ACE.Network.GameAction.Actions
{
    [GameAction(GameActionOpcode.ChatChannel)]
    public class GameActionChatChannel : GameActionPacket
    {
        public GameActionChatChannel(Session session, ClientPacketFragment fragment) : base(session, fragment) { }

        public override void Handle()
        {
            throw new System.NotImplementedException();
        }
    }
}
