
using ACE.Common.Extensions;
using ACE.Entity.Enum;

namespace ACE.Network.GameAction.Actions
{
    [GameAction(GameActionType.AddFriend)]
    public class GameActionAddFriend : GameActionPacket
    {
        private string friendName;

        public GameActionAddFriend(Session session, ClientPacketFragment fragment) : base(session, fragment) { }

        public override void Read()
        {
            friendName = Fragment.Payload.ReadString16L().Trim();
        }

        public async override void Handle()
        {
            var result = await Session.Player.AddFriend(friendName);

            switch(result)
            {
                case Enum.AddFriendResult.AlreadyInList:
                    ChatPacket.SendServerMessage(Session, "That character is already in your friends list", ChatMessageType.Broadcast);
                    break;

                case Enum.AddFriendResult.FriendWithSelf:
                    ChatPacket.SendServerMessage(Session, "Sorry, but you can't be friends with yourself.", ChatMessageType.Broadcast);
                    break;

                case Enum.AddFriendResult.CharacterDoesNotExist:
                    ChatPacket.SendServerMessage(Session, "That character does not exist", ChatMessageType.Broadcast);
                    break;
            }
        }
    }
}
