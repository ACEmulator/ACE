using ACE.Database;
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
            // Check if in friend list
            Friend friend = session.Player.Character.Friends.FirstOrDefault(f => f.Id.GetLow() == friendId.GetLow());
            if (friend == null)
            {
                ChatPacket.SendSystemMessage(session, "That friend is not in your list!");
                return;
            }

            // Remove from db
            await DatabaseManager.Character.DeleteFriend(session.Player.Guid.GetLow(), friend.Id.GetLow());

            // Remove from in memory list
            session.Player.Character.Friends.Remove(friend);

            // Send response
            new GameEvent.GameEventFriendsListUpdate(session, GameEvent.GameEventFriendsListUpdate.FriendsUpdateTypeFlag.FriendRemoved, friend).Send();
        }
    }
}
