using System.Linq;

using ACE.Common;
using ACE.Database.Models.Shard;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity.Actions;
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
            PlayerManager.SwitchPlayerFromOfflineToOnline(this);

            // Save the the LoginTimestamp
            var lastLoginTimestamp = Time.GetUnixTime();

            SetProperty(PropertyInt.LoginTimestamp, (int)lastLoginTimestamp);

            Character.LastLoginTimestamp = lastLoginTimestamp;
            Character.TotalLogins++;
            CharacterChangesDetected = true;

            Sequences.SetSequence(SequenceType.ObjectInstance, new UShortSequence((ushort)Character.TotalLogins));

            if (BarberActive)
                BarberActive = false;

            if (AllegianceNode != null)
                AllegianceRank = (int)AllegianceNode.Rank;
            else
                AllegianceRank = null;

            // SendSelf will trigger the entrance into portal space
            SendSelf();

            // Init the client with the chat channel ID's, and then notify the player that they've choined the associated channels.
            UpdateChatChannels();

            var general = new GameEventWeenieErrorWithString(Session, WeenieErrorWithString.YouHaveEnteredThe_Channel, "General");
            var trade = new GameEventWeenieErrorWithString(Session, WeenieErrorWithString.YouHaveEnteredThe_Channel, "Trade");
            var lfg = new GameEventWeenieErrorWithString(Session, WeenieErrorWithString.YouHaveEnteredThe_Channel, "LFG");
            var roleplay = new GameEventWeenieErrorWithString(Session, WeenieErrorWithString.YouHaveEnteredThe_Channel, "Roleplay");
            Session.Network.EnqueueSend(general, trade, lfg, roleplay);

            // check if vassals earned XP while offline
            HandleAllegianceOnLogin();
            HandleHouseOnLogin();

            AuditItemSpells();

            if (PlayerKillerStatus == PlayerKillerStatus.PKLite && !PropertyManager.GetBool("pkl_server").Item)
            {
                var actionChain = new ActionChain();
                actionChain.AddDelaySeconds(3.0f);
                actionChain.AddAction(this, () =>
                {
                    UpdateProperty(this, PropertyInt.PlayerKillerStatus, (int)PlayerKillerStatus.NPK, true);

                    Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouAreNonPKAgain));
                });
                actionChain.EnqueueChain();
            }

            HandleDBUpdates();
        }

        public void UpdateChatChannels()
        {
            var allegianceChannel = Allegiance != null ? Allegiance.Biota.Id : 0u;

            Session.Network.EnqueueSend(new GameEventSetTurbineChatChannels(Session, allegianceChannel));
        }

        private void SendSelf()
        {
            var player = new GameEventPlayerDescription(Session);
            var title = new GameEventCharacterTitle(Session);
            var friends = new GameEventFriendsListUpdate(Session);

            Session.Network.EnqueueSend(player, title, friends);

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
                item.Wielder = this;
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
        public void SendFriendStatusUpdates()
        {
            var inverseFriends = PlayerManager.GetOnlineInverseFriends(Guid);

            foreach (var friend in inverseFriends)
            {
                var playerFriend = new CharacterPropertiesFriendList { CharacterId = friend.Guid.Full, FriendId = Guid.Full };
                friend.Session.Network.EnqueueSend(new GameEventFriendsListUpdate(friend.Session, GameEventFriendsListUpdate.FriendsUpdateTypeFlag.FriendStatusChanged, playerFriend, true, !GetAppearOffline()));
            }
        }

        /// <summary>
        /// Will send out GameEventFriendsListUpdate packets to everyone online that has this player as a friend.
        /// </summary>
        public void SendFriendStatusUpdates(bool onlineStatus)
        {
            var inverseFriends = PlayerManager.GetOnlineInverseFriends(Guid);

            foreach (var friend in inverseFriends)
            {
                var playerFriend = new CharacterPropertiesFriendList { CharacterId = friend.Guid.Full, FriendId = Guid.Full };
                friend.Session.Network.EnqueueSend(new GameEventFriendsListUpdate(friend.Session, GameEventFriendsListUpdate.FriendsUpdateTypeFlag.FriendStatusChanged, playerFriend, true, onlineStatus));
            }
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

            // update CurrentMotionState here for substates?
            if ((state.Flags & RawMotionFlags.ForwardCommand) != 0)
            {
                if (((uint)state.ForwardCommand & (uint)CommandMask.SubState) != 0)
                    CurrentMotionState.SetForwardCommand(state.ForwardCommand);
            }
            else
                CurrentMotionState.SetForwardCommand(MotionCommand.Ready);

            if (state.CommandListLength > 0)
            {
                if (((uint)state.Commands[0].MotionCommand & (uint)CommandMask.SubState) != 0)
                    CurrentMotionState.SetForwardCommand(state.Commands[0].MotionCommand);
            }

            var movementData = new MovementData(this, moveToState);

            var movementEvent = new GameMessageUpdateMotion(this, movementData);
            EnqueueBroadcast(movementEvent);    // shouldn't need to go to originating player?

            // TODO: use real motion / animation system from physics
            CurrentMotionCommand = movementData.Invalid.State.ForwardCommand;
        }
    }
}
