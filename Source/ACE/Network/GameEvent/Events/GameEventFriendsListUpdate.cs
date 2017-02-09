using ACE.Entity;
using ACE.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Network.GameEvent
{
    class GameEventFriendsListUpdate : GameEventPacket
    {
        [Flags]
        public enum FriendsUpdateTypeFlag
        {
            FullList = 0x0000,
            FriendAdded = 0x0001,
            FriendRemoved = 0x0002,
            FriendStatusChanged = 0x0004
        }

        public override GameEventOpcode Opcode { get { return GameEventOpcode.FriendsListUpdate; } }

        private FriendsUpdateTypeFlag updateType;
        private Friend friend = null;
        private bool? onlineOverride = null;
        
        /// <summary>
        /// This constructor should only be used for sending the full friend list
        /// </summary>
        /// <param name="session"></param>
        public GameEventFriendsListUpdate(Session session) : base(session)
        {
            this.updateType = FriendsUpdateTypeFlag.FullList;
        }

        /// <summary>
        /// This constructor is used for passing in a single friend for and add, remove, or update status.  It also allows you to override the online status so the WorldManager isn't checked.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="updateType"></param>
        /// <param name="friend"></param>
        /// <param name="onlineOverride">Send this as the online status of the friend.  Useful if you know the status and don't want to have the WorldManager check</param>
        public GameEventFriendsListUpdate(Session session, FriendsUpdateTypeFlag updateType, Friend friend, bool? onlineOverride = null) : base(session)
        {
            this.updateType = updateType;
            this.friend = friend;
            this.onlineOverride = onlineOverride;
        }

        protected override void WriteEventBody()
        {
            List<Friend> friendList = null;

            if (updateType == FriendsUpdateTypeFlag.FullList)
                friendList = session.Player.Character.Friends;
            else
                friendList = new List<Friend>() { friend };

            fragment.Payload.Write(friendList.Count);

            foreach (var f in friendList)
            {
                Session friendSession = WorldManager.Find(f.Id);
                bool isOnline = false;

                if (onlineOverride.HasValue)
                    isOnline = onlineOverride.Value;
                else if (friendSession != null && friendSession.Player.IsOnline)
                    isOnline = true;

                fragment.Payload.Write(f.Id.Full); // friend Object ID
                fragment.Payload.Write(isOnline ? 1u : 0u); // is Online               
                fragment.Payload.Write(0u); // Unknown
                fragment.Payload.WriteString16L(f.Name); // Friend Name

                fragment.Payload.Write(f.FriendIdList.Count); // Number of people on this persons friend's list.
                foreach(var fid in f.FriendIdList)
                    fragment.Payload.Write(fid.Full);
                
                fragment.Payload.Write(f.FriendOfIdList.Count); // Number of people that have this person as a friend.
                foreach (var fid in f.FriendOfIdList)
                    fragment.Payload.Write(fid.Full);                
            }

            fragment.Payload.Write((uint)updateType);            
                    
        }
    }
}
