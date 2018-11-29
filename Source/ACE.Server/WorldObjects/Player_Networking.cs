using System.Linq;
using ACE.Common;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Managers;
using ACE.Server.Network.Enum;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Sequence;
using ACE.Server.Network.Structure;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        public void PlayerEnterWorld()
        {
            IsOnline = true;

            // Save the the LoginTimestamp
            var lastLoginTimestamp = Time.GetUnixTime();

            SetProperty(PropertyInt.LoginTimestamp, (int)lastLoginTimestamp);

            Character.LastLoginTimestamp = lastLoginTimestamp;
            Character.TotalLogins++;
            CharacterChangesDetected = true;

            Sequences.SetSequence(SequenceType.ObjectInstance, new UShortSequence((ushort)Character.TotalLogins));

            // SendSelf will trigger the entrance into portal space
            SendSelf();

            SendFriendStatusUpdates();

            // Init the client with the chat channel ID's, and then notify the player that they've choined the associated channels.
            var setTurbineChatChannels = new GameEventSetTurbineChatChannels(Session, 0, 1, 2, 3, 4, 6, 7, 0, 0, 0); // TODO these are hardcoded right now
            var general = new GameEventWeenieErrorWithString(Session, WeenieErrorWithString.YouHaveEnteredThe_Channel, "General");
            var trade = new GameEventWeenieErrorWithString(Session, WeenieErrorWithString.YouHaveEnteredThe_Channel, "Trade");
            var lfg = new GameEventWeenieErrorWithString(Session, WeenieErrorWithString.YouHaveEnteredThe_Channel, "LFG");
            var roleplay = new GameEventWeenieErrorWithString(Session, WeenieErrorWithString.YouHaveEnteredThe_Channel, "Roleplay");
            Session.Network.EnqueueSend(setTurbineChatChannels, general, trade, lfg, roleplay);

            // check if vassals earned XP while offline
            var offlinePlayer = WorldManager.GetOfflinePlayer(Guid);
            if (offlinePlayer != null)
                offlinePlayer.AddCPPoolToUnload(true);

            HandleDBUpdates();
        }

        /// <summary>
        /// This is called prior to SendSelf to load up the child list for wielded items that are held in a hand.
        /// </summary>
        private void SetChildren()
        {
            Children.Clear();

            foreach (var item in EquippedObjects.Values)
            {
                if ((item.CurrentWieldedLocation != null) && (((EquipMask)item.CurrentWieldedLocation & EquipMask.Selectable) != 0))
                {
                    int placementId;
                    int parentLocation;
                    SetChild(item, (int)item.CurrentWieldedLocation, out placementId, out parentLocation);
                }
            }
        }

        private void SendSelf()
        {
            var player = new GameEventPlayerDescription(Session);
            var title = new GameEventCharacterTitle(Session);
            var friends = new GameEventFriendsListUpdate(Session);

            Session.Network.EnqueueSend(player, title, friends);

            SetChildren();
            // Player objects don't get a placement
            Placement = null;
            Session.Network.EnqueueSend(new GameMessagePlayerCreate(Guid), new GameMessageCreateObject(this));

            SendInventoryAndWieldedItems();

            // SendContractTrackerTable(); todo fix for new ef not use aceobj
        }

        /// <summary>
        /// This method iterates through your main pack, any packs and finds all the items contained
        /// It also iterates over your wielded items - it sends create object messages needed by the login process
        /// it is called from SendSelf as part of the login message traffic.   Og II
        /// </summary>
        public void SendInventoryAndWieldedItems()
        {
            foreach (var item in Inventory.Values)
            {
                Session.Network.EnqueueSend(new GameMessageCreateObject(item));

                // Was the item I just send a container? If so, we need to send the items in the container as well. Og II
                if (item is Container container)
                {
                    Session.Network.EnqueueSend(new GameEventViewContents(Session, container));

                    foreach (var itemsInContainer in container.Inventory.Values)
                        Session.Network.EnqueueSend(new GameMessageCreateObject(itemsInContainer));
                }
            }

            foreach (var item in EquippedObjects.Values)
            {
                if (item.CurrentWieldedLocation != null
                    && ((EquipMask)item.CurrentWieldedLocation & EquipMask.Selectable) != 0
                    && item.CurrentWieldedLocation != EquipMask.MissileAmmo)
                {
                    int placementId;
                    int parentLocation;
                    SetChild(item, (int)item.CurrentWieldedLocation, out placementId, out parentLocation);
                    item.CurrentMotionState = null;
                }

                // We don't want missile ammo to appear in the players right hand on login.
                if (item.CurrentWieldedLocation == EquipMask.MissileAmmo)
                {
                    item.ParentLocation = null;
                    item.Placement = ACE.Entity.Enum.Placement.Resting;
                    item.Location = null;
                }
                Session.Network.EnqueueSend(new GameMessageCreateObject(item));
            }
        }

        /// <summary>
        /// This method is used to take our persisted tracked contracts and send them on to the client. Pg II
        /// </summary>
        public void SendContractTrackerTable()
        {
            Session.Network.EnqueueSend(new GameEventSendClientContractTrackerTable(Session, TrackedContracts.Select(x => x.Value).ToList()));
        }

        /// <summary>
        /// Will send out GameEventFriendsListUpdate packets to everyone online that has this player as a friend.
        /// </summary>
        private void SendFriendStatusUpdates()
        {
            return; // todo fix

            /*List<Session> inverseFriends = WorldManager.FindInverseFriends(Guid);

            if (inverseFriends.Count > 0)
            {
                Friend playerFriend = new Friend();
                playerFriend.Id = Guid;
                playerFriend.Name = Name;

                foreach (var friendSession in inverseFriends)
                    friendSession.Network.EnqueueSend(new GameEventFriendsListUpdate(friendSession, GameEventFriendsListUpdate.FriendsUpdateTypeFlag.FriendStatusChanged, playerFriend, true, GetVirtualOnlineStatus()));
            }*/
        }


        /// <summary>
        /// Records where the client thinks we are, for use by physics engine later
        /// </summary>
        public void SetRequestedLocation(Position pos)
        {
            RequestedLocation = pos;
        }

        public void BroadcastMovement(MoveToState moveToState)
        {
            var state = moveToState.RawMotionState;

            // update current style
            if ((state.Flags & RawMotionFlags.CurrentStyle) != 0)
            {
                // this lowercase stance field in Player doesn't really seem to be used anywhere
                stance = state.CurrentStyle;
            }

            var movementData = new MovementData(this, moveToState);

            var movementEvent = new GameMessageUpdateMotion(this, movementData);
            EnqueueBroadcast(movementEvent);    // shouldn't need to go to originating player?

            // TODO: use real motion / animation system from physics
            CurrentMotionCommand = movementData.Invalid.State.ForwardCommand;
        }
    }
}
