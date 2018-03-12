using System.Collections.Generic;
using System.Linq;

using ACE.Common;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Managers;
using ACE.Server.Network;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Motion;
using ACE.Server.Network.Sequence;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        public void PlayerEnterWorld()
        {
            IsAlive = true; // seems like something that should be handled differently...
            IsOnline = true;

            // Save the the LoginTimestamp
            SetProperty(PropertyFloat.LoginTimestamp, Time.GetTimestamp());

            var totalLogins = GetProperty(PropertyInt.TotalLogins) ?? 0;
            totalLogins++;
            SetProperty(PropertyInt.TotalLogins, totalLogins);

            Sequences.AddOrSetSequence(SequenceType.ObjectInstance, new UShortSequence((ushort)totalLogins));

            // SendSelf will trigger the entrance into portal space
            SendSelf();

            SendFriendStatusUpdates();

            // Init the client with the chat channel ID's, and then notify the player that they've choined the associated channels.
            var setTurbineChatChannels = new GameEventSetTurbineChatChannels(Session, 0, 1, 2, 3, 4, 6, 7, 0, 0, 0); // TODO these are hardcoded right now
            var general = new GameEventDisplayParameterizedStatusMessage(Session, StatusMessageType2.YouHaveEnteredThe_Channel, "General");
            var trade = new GameEventDisplayParameterizedStatusMessage(Session, StatusMessageType2.YouHaveEnteredThe_Channel, "Trade");
            var lfg = new GameEventDisplayParameterizedStatusMessage(Session, StatusMessageType2.YouHaveEnteredThe_Channel, "LFG");
            var roleplay = new GameEventDisplayParameterizedStatusMessage(Session, StatusMessageType2.YouHaveEnteredThe_Channel, "Roleplay");
            Session.Network.EnqueueSend(setTurbineChatChannels, general, trade, lfg, roleplay);
        }

        /// <summary>
        /// This is called prior to SendSelf to load up the child list for wielded items that are held in a hand.
        /// </summary>
        private void SetChildren()
        {
            // WE SHOULDNT SET THE CHILDEREN HERE
            // THIS SHOULD PROBABLY GO IN PlayerEnterWorld()
            // ALTERNATIVELY, WE CAN PULL THE EQUIPPED INVENTORY FROM THE DB IN THE CTOR

            Children.Clear();

            /* todo fix for new not use aceobj
            foreach (WorldObject wieldedObject in WieldedObjects.Values)
            {
                WorldObject wo = wieldedObject;
                int placementId;
                int childLocation;
                if ((wo.CurrentWieldedLocation != null) && (((EquipMask)wo.CurrentWieldedLocation & EquipMask.Selectable) != 0))
                    SetChild(this, wo, (int)wo.CurrentWieldedLocation, out placementId, out childLocation);
                else
                    log.Debug($"Error - item set as child that should not be set - no currentWieldedLocation {wo.Name} - {wo.Guid.Full:X}");
            }*/
        }

        private void SendSelf()
        {
            var player = new GameEventPlayerDescription(Session);
            var title = new GameEventCharacterTitle(Session);
            var friends = new GameEventFriendsListUpdate(Session);

            Session.Network.EnqueueSend(player, title, friends);

            SetChildren();

            Session.Network.EnqueueSend(new GameMessagePlayerCreate(Guid), new GameMessageCreateObject(this));

            SendInventoryAndWieldedItems(Session);

            // SendContractTrackerTable(); todo fix for new ef not use aceobj
        }

        /// <summary>
        /// This method iterates through your main pack, any packs and finds all the items contained
        /// It also iterates over your wielded items - it sends create object messages needed by the login process
        /// it is called from SendSelf as part of the login message traffic.   Og II
        /// </summary>
        /// <param name="session"></param>
        public void SendInventoryAndWieldedItems(Session session)
        {
            foreach (var item in Inventory.Values)
            {
                session.Network.EnqueueSend(new GameMessageCreateObject(item));

                // Was the item I just send a container? If so, we need to send the items in the container as well. Og II
                if (item is Container container)
                {
                    Session.Network.EnqueueSend(new GameEventViewContents(Session, container));

                    foreach (var itemsInContainer in container.Inventory.Values)
                        session.Network.EnqueueSend(new GameMessageCreateObject(itemsInContainer));
                }
            }

            foreach (var item in EquippedObjects.Values)
            {
                if ((item.CurrentWieldedLocation != null) && (((EquipMask)item.CurrentWieldedLocation & EquipMask.Selectable) != 0))
                {
                    int placementId;
                    int childLocation;
                    session.Player.SetChild(item, (int)item.CurrentWieldedLocation, out placementId, out childLocation);
                }
                session.Network.EnqueueSend(new GameMessageCreateObject(item));
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
            return;// todo fix
            List<Session> inverseFriends = WorldManager.FindInverseFriends(Guid);

            if (inverseFriends.Count > 0)
            {
                Friend playerFriend = new Friend();
                playerFriend.Id = Guid;
                playerFriend.Name = Name;

                foreach (var friendSession in inverseFriends)
                    friendSession.Network.EnqueueSend(new GameEventFriendsListUpdate(friendSession, GameEventFriendsListUpdate.FriendsUpdateTypeFlag.FriendStatusChanged, playerFriend, true, GetVirtualOnlineStatus()));
            }
        }


        public void RequestUpdatePosition(Position pos)
        {
            ExternalUpdatePosition(pos);
        }

        public void RequestUpdateMotion(uint holdKey, MovementData md, MotionItem[] commands)
        {
            new ActionChain(this, () =>
            {
                // Update our current style
                if ((md.MovementStateFlag & MovementStateFlag.CurrentStyle) != 0)
                {
                    MotionStance newStance = (MotionStance)md.CurrentStyle;

                    if (newStance != stance)
                        stance = (MotionStance)md.CurrentStyle;
                }

                md = md.ConvertToClientAccepted(holdKey, GetCreatureSkill(Skill.Run));
                UniversalMotion newMotion = new UniversalMotion(stance, md);

                // This is a hack to make walking work correctly.   Og II
                if (holdKey != 0 || (md.ForwardCommand == (uint)MotionCommand.WalkForward))
                    newMotion.IsAutonomous = true;

                // FIXME(ddevec): May need to de-dupe animation/commands from client -- getting multiple (e.g. wave)
                // FIXME(ddevec): This is the operation that should update our velocity (for physics later)
                newMotion.Commands.AddRange(commands);
                CurrentLandblock.EnqueueBroadcastMotion(this, newMotion);
            }).EnqueueChain();
        }

        private void ExternalUpdatePosition(Position newPosition)
        {
            //if (InWorld)
                PrepUpdatePosition(newPosition);
        }
    }
}
