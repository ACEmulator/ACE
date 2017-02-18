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
            var result = await session.Player.AddFriend(friendName);

            switch(result)
            {
                case Enum.AddFriendResult.AlreadyInList:
                    ChatPacket.SendSystemMessage(session, "That character is already in your friends list");
                    break;

                case Enum.AddFriendResult.FriendWithSelf:
                    ChatPacket.SendSystemMessage(session, "Sorry, but you can't be friends with yourself.");
                    break;

                case Enum.AddFriendResult.CharacterDoesNotExist:
                    ChatPacket.SendSystemMessage(session, "That character does not exist");
                    break;
            }
        }
    }
}
