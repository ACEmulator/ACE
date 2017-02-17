using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE.Common.Extensions;

namespace ACE.Network.GameAction.Actions
{
    [GameAction(GameActionOpcode.AddFriend)]
    public class GameActionAddFriend : GameActionPacket
    {
        private string friendName;

        public GameActionAddFriend(Session session, ClientPacketFragment fragment) : base(session, fragment) { }

        public override void Read()
        {
            friendName = fragment.Payload.ReadString16L().Trim();
        }

        public async override void Handle()
        {
            string result = await session.Player.AddFriend(friendName);

            if(!string.IsNullOrEmpty(result))
                ChatPacket.SendSystemMessage(session, result);
        }
    }
}
