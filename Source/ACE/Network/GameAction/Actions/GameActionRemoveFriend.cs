using ACE.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Network.GameAction.Actions
{
    [GameAction(GameActionOpcode.RemoveFriend)]
    public class GameActionRemoveFriend : GameActionPacket
    {
        private ObjectGuid friendId;

        public GameActionRemoveFriend(Session session, ClientPacketFragment fragment) : base(session, fragment) { }

        public override void Read()
        {
            uint lowId = fragment.Payload.ReadUInt32() & 0xFFFFFF;
            friendId = new ObjectGuid(lowId, GuidType.Player);
        }

        public async override void Handle()
        {
            var result = await session.Player.RemoveFriend(friendId);

            if(result == Enum.RemoveFriendResult.NotInFriendsList)
                ChatPacket.SendSystemMessage(session, "That chracter is not in your friends list!");                            
        }
    }
}
