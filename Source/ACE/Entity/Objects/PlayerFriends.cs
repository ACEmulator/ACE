using ACE.Common.Extensions;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Network;
using ACE.Network.Enum;
using ACE.Network.GameAction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity.Objects
{
    public partial class Player
    {
        [GameAction(GameActionType.AddFriend)]
        private async void AddFriendAction(ClientMessage message)
        {
            var friendName = message.Payload.ReadString16L().Trim();
            var result = await AddFriend(friendName);

            switch (result)
            {
                case AddFriendResult.AlreadyInList:
                    ChatPacket.SendServerMessage(Session, "That character is already in your friends list", ChatMessageType.Broadcast);
                    break;

                case AddFriendResult.FriendWithSelf:
                    ChatPacket.SendServerMessage(Session, "Sorry, but you can't be friends with yourself.", ChatMessageType.Broadcast);
                    break;

                case AddFriendResult.CharacterDoesNotExist:
                    ChatPacket.SendServerMessage(Session, "That character does not exist", ChatMessageType.Broadcast);
                    break;
            }
        }

        [GameAction(GameActionType.RemoveFriend)]
        private async void RemoveFriendAction(ClientMessage message)
        {
            uint lowId = message.Payload.ReadUInt32() & 0xFFFFFF;
            var friendId = new ObjectGuid(lowId, GuidType.Player);
            var result = await RemoveFriend(friendId);

            if (result == RemoveFriendResult.NotInFriendsList)
                ChatPacket.SendServerMessage(Session, "That chracter is not in your friends list!", ChatMessageType.Broadcast);
        }

        [GameAction(GameActionType.RemoveAllFriends)]
        private void RemoveAllFriendsAction(ClientMessage message)
        {
            RemoveAllFriends();
        }
    }
}
