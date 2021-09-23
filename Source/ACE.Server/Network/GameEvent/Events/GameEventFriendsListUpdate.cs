using System;
using System.Linq;
using System.Collections.Generic;

using ACE.Database.Models.Shard;
using ACE.Server.Managers;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventFriendsListUpdate : GameEventMessage
    {
        [Flags]
        public enum FriendsUpdateTypeFlag
        {
            FullList            = 0x0000,
            FriendAdded         = 0x0001,
            FriendRemoved       = 0x0002,
            FriendStatusChanged = 0x0004
        }

        private readonly FriendsUpdateTypeFlag updateType;
        private readonly CharacterPropertiesFriendList friend;
        private readonly bool overrideOnlineStatus;
        private readonly bool onlineStatusVal;

        /// <summary>
        /// This constructor should only be used for sending the full friend list
        /// </summary>
        /// <param name="session"></param>
        public GameEventFriendsListUpdate(Session session)
            : base(GameEventType.FriendsListUpdate, GameMessageGroup.UIQueue, session)
        {
            updateType = FriendsUpdateTypeFlag.FullList;
            WriteEventBody();
        }

        /// <summary>
        /// This constructor is used for passing in a single friend for and add, remove, or update status.  It also allows you to override the online status so the WorldManager isn't checked.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="updateType"></param>
        /// <param name="friend"></param>
        /// <param name="overrideOnlineStatus">Set to true if you want to force a value for the online status of the friend.  Useful if you know the status and don't want to have the WorldManager check</param>
        /// <param name="onlineStatusVal">If overrideOnlineStatus is true, then this is the online status value that you want to force in the packet</param>
        public GameEventFriendsListUpdate(Session session, FriendsUpdateTypeFlag updateType, CharacterPropertiesFriendList friend, bool overrideOnlineStatus = false, bool onlineStatusVal = false)
            : base(GameEventType.FriendsListUpdate, GameMessageGroup.UIQueue, session)
        {
            this.updateType = updateType;
            this.friend = friend;
            this.overrideOnlineStatus = overrideOnlineStatus;
            this.onlineStatusVal = onlineStatusVal;

            WriteEventBody();
        }

        private void WriteEventBody()
        {
            List<CharacterPropertiesFriendList> friendList;

            if (updateType == FriendsUpdateTypeFlag.FullList)
                friendList = Session.Player.Character.GetFriends(Session.Player.CharacterDatabaseLock);
            else
                friendList = new List<CharacterPropertiesFriendList>() { friend };

            Writer.Write((uint)friendList.Count);

            foreach (var f in friendList)
            {
                var player = PlayerManager.FindByGuid(f.FriendId, out var isOnline);
                var friendName = (player != null) ? player.Name : "";

                if (overrideOnlineStatus)
                    isOnline = onlineStatusVal;
                else if (isOnline)
                {
                    // Does this friend want to appear offline?
                    var onlineFriend = PlayerManager.GetOnlinePlayer(f.FriendId);
                    if (onlineFriend != null && onlineFriend.GetAppearOffline())
                        isOnline = false;
                }

                Writer.Write(f.FriendId);           // Friend's ID
                Writer.Write(isOnline ? 1u : 0u);   // Whether this friend is online
                Writer.Write(0u);                   // Whether the friend should appear to be offline
                Writer.WriteString16L(friendName);  // Name of the friend

                // send the list of friend's friends
                Writer.Write((uint)0/* TODO player.Character.CharacterPropertiesFriendList.Count*/);
                /*foreach (var friendFriend in player.Character.CharacterPropertiesFriendList)
                    Writer.Write(friendFriend.FriendId);*/

                // todo: send the inverse list of friend's friends
                Writer.Write((uint)0/* TODO playersFriend.Character.CharacterPropertiesFriendList.Count*/);
                /*foreach (var friendFriend in playersFriend.Character.CharacterPropertiesFriendList)
                    Writer.Write(friendFriend.FriendId);*/
            }

            Writer.Write((uint)updateType);
        }
    }
}
