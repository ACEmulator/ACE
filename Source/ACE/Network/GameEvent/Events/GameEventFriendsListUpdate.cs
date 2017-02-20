using ACE.Entity;
using ACE.Managers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Network.GameEvent.Events
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
        private Friend friend = null;
        private bool overrideOnlineStatus = false;
        private bool onlineStatusVal = false;

        /// <summary>
        /// This constructor should only be used for sending the full friend list
        /// </summary>
        /// <param name="session"></param>
        public GameEventFriendsListUpdate(Session session) 
            : base (GameEventType.FriendsListUpdate, session)
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
        public GameEventFriendsListUpdate(Session session, FriendsUpdateTypeFlag updateType, Friend friend, bool overrideOnlineStatus = false, bool onlineStatusVal = false) 
            : base (GameEventType.FriendsListUpdate, session)
        {
            this.updateType = updateType;
            this.friend = friend;
            this.overrideOnlineStatus = overrideOnlineStatus;
            this.onlineStatusVal = onlineStatusVal;
            WriteEventBody();
        }

        private void WriteEventBody()
        {
            List<Friend> friendList = null;

            if (updateType == FriendsUpdateTypeFlag.FullList)
                friendList = Session.Player.Friends.ToList();
            else
                friendList = new List<Friend>() { friend };

            Writer.Write((uint)friendList.Count);

            foreach (var f in friendList)
            {
                bool isOnline = false;

                if (overrideOnlineStatus)
                    isOnline = onlineStatusVal;
                else
                {
                    Session friendSession = WorldManager.Find(f.Id);
                    if(friendSession != null && friendSession.Player?.GetVirtualOnlineStatus() == true)
                        isOnline = true;
                }                    

                Writer.Write(f.Id.Full); // friend Object ID
                Writer.Write(isOnline ? 1u : 0u); // is Online               
                Writer.Write(0u); // Unknown
                Writer.WriteString16L(f.Name); // Friend Name

                Writer.Write((uint)f.FriendIdList.Count); // Number of people on this persons friend's list.
                foreach (var fid in f.FriendIdList)
                    Writer.Write(fid.Full);

                Writer.Write((uint)f.FriendOfIdList.Count); // Number of people that have this person as a friend.
                foreach (var fid in f.FriendOfIdList)
                    Writer.Write(fid.Full);
            }

            Writer.Write((uint)updateType);
        }
    }
}
