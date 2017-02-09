using ACE.Database;
using ACE.Entity;
using ACE.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Network.GameAction
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
            if (string.Equals(friendName, session.Player.Character.Name, StringComparison.CurrentCultureIgnoreCase))
            {
                ChatPacket.SendSystemMessage(session, "Sorry, but you can't be friends with yourself.");
                return;
            }

            // Check if already a friend
            if (session.Player.Character.Friends.FirstOrDefault(f => string.Equals(f.Name, friendName, StringComparison.CurrentCultureIgnoreCase)) != null)
            {
                ChatPacket.SendSystemMessage(session, "That character is already in your friends list!");
                return;
            }

            // Pull back record from db            
            Character c = await DatabaseManager.Character.GetCharacterByName(friendName);

            if (c == null)
            {
                ChatPacket.SendSystemMessage(session, "That character doesn't exist.");
                return;
            }
    
            Friend newFriend = new Friend();
            newFriend.Name = c.Name;
            newFriend.Id = new ObjectGuid(c.Id, GuidType.Player);
            newFriend.FriendIdList = new List<ObjectGuid>();
            newFriend.FriendOfIdList = new List<ObjectGuid>();

            // Add to db
            await DatabaseManager.Character.AddFriend(session.Player.Guid.GetLow(), newFriend.Id.GetLow());

            // Add to character object
            session.Player.Character.Friends.Add(newFriend);

            // send packet back
            new GameEvent.GameEventFriendsListUpdate(session, GameEvent.GameEventFriendsListUpdate.FriendsUpdateTypeFlag.FriendAdded, newFriend).Send();
        }
    }
}
