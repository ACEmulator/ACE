
using ACE.Entity;
using ACE.Entity.Enum;

namespace ACE.Network.GameAction.Actions
{
    [GameAction(GameActionOpcode.RemoveFriend)]
    public class GameActionRemoveFriend : GameActionPacket
    {
        private ObjectGuid friendId;

        public GameActionRemoveFriend(Session session, ClientPacketFragment fragment) : base(session, fragment) { }

        public override void Read()
        {
            uint lowId = Fragment.Payload.ReadUInt32() & 0xFFFFFF;
            friendId = new ObjectGuid(lowId, GuidType.Player);
        }

        public async override void Handle()
        {
            var result = await Session.Player.RemoveFriend(friendId);

            if(result == Enum.RemoveFriendResult.NotInFriendsList)
                ChatPacket.SendServerMessage(Session, "That chracter is not in your friends list!", ChatMessageType.Broadcast);                            
        }
    }
}
