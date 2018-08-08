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

        private FriendsUpdateTypeFlag updateType;
        private CharacterPropertiesFriendList friend;
        private bool overrideOnlineStatus;
        private bool onlineStatusVal;

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
                friendList = Session.Player.Character.CharacterPropertiesFriendList.ToList();
            else
                friendList = new List<CharacterPropertiesFriendList>() { friend };

            Writer.Write((uint)friendList.Count);

            foreach (var f in friendList)
            {
                bool isOnline = false;

                if (overrideOnlineStatus)
                    isOnline = onlineStatusVal;
                else
                {
                    // lookup by player id or account id?
                    //Session friendSession = WorldManager.Find(f.FriendId);
                    var onlineFriend = WorldManager.GetPlayerByGuidId(f.FriendId);
                    if (onlineFriend != null && onlineFriend.GetVirtualOnlineStatus() == true)
                        isOnline = true;
                }

                var offlinePlayer = WorldManager.AllPlayers.FirstOrDefault(p => p.Guid.Full == f.FriendId);
                var friendName = offlinePlayer != null ? offlinePlayer.Name : "";

                Writer.Write(f.FriendId);           // Friend's ID
                Writer.Write(isOnline ? 1u : 0u);   // Whether this friend is online
                Writer.Write(0u);                   // Whether the friend should appear to be offline
                Writer.WriteString16L(friendName);  // Name of the friend

                // send the list of friend's friends
                Writer.Write((uint)offlinePlayer.Character.CharacterPropertiesFriendList.Count);
                foreach (var friendFriend in offlinePlayer.Character.CharacterPropertiesFriendList)
                    Writer.Write(friendFriend.FriendId);

                // todo: send the inverse list of friend's friends
                Writer.Write((uint)offlinePlayer.Character.CharacterPropertiesFriendList.Count);
                foreach (var friendFriend in offlinePlayer.Character.CharacterPropertiesFriendList)
                    Writer.Write(friendFriend.FriendId);
            }

            Writer.Write((uint)updateType);
        }
    }
}
