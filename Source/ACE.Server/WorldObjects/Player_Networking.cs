using System;

using ACE.Common;
using ACE.Database.Models.Shard;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
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
            Teleporting = true;

            // Save the the LoginTimestamp
            var lastLoginTimestamp = Time.GetUnixTime();

            LoginTimestamp = lastLoginTimestamp;
            LastTeleportStartTimestamp = lastLoginTimestamp;

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

            if (!Account15Days)
            {
                var accountAge = DateTime.UtcNow - Account.CreateTime;

                if (accountAge.TotalDays >= 15)
                    Account15Days = true;

                ManageAccount15Days_HousePurchaseTimestamp();
            }

            if (PlayerKillerStatus == PlayerKillerStatus.PKLite && !PropertyManager.GetBool("pkl_server").Item)
            {
                PlayerKillerStatus = PlayerKillerStatus.NPK;

                var actionChain = new ActionChain();
                actionChain.AddDelaySeconds(3.0f);
                actionChain.AddAction(this, () =>
                {
                    Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouAreNonPKAgain));
                });
                actionChain.EnqueueChain();
            }

            HandlePreOrderItems();

            // SendSelf will trigger the entrance into portal space
            SendSelf();

            // Update or override certain properties sent to client.

            // bugged: do not send this here, or else a freshly loaded acclient will overrwrite the values
            // wait until first enter world is completed

            //SendPropertyUpdatesAndOverrides();

            if (PropertyManager.GetBool("use_turbine_chat").Item)
            {
                // Init the client with the chat channel ID's, and then notify the player that they've joined the associated channels.
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.TurbineChatIsEnabled));

                if (IsOlthoiPlayer)
                {
                    JoinTurbineChatChannel("Olthoi");
                }
                else
                {
                    if (GetCharacterOption(CharacterOption.ListenToAllegianceChat) && Allegiance != null)
                        JoinTurbineChatChannel("Allegiance");
                    if (GetCharacterOption(CharacterOption.ListenToGeneralChat))
                        JoinTurbineChatChannel("General");
                    if (GetCharacterOption(CharacterOption.ListenToTradeChat))
                        JoinTurbineChatChannel("Trade");
                    if (GetCharacterOption(CharacterOption.ListenToLFGChat))
                        JoinTurbineChatChannel("LFG");
                    if (GetCharacterOption(CharacterOption.ListenToRoleplayChat))
                        JoinTurbineChatChannel("Roleplay");
                    if (GetCharacterOption(CharacterOption.ListenToSocietyChat) && Society != FactionBits.None)
                        JoinTurbineChatChannel("Society");
                }
            }

            // check if vassals earned XP while offline
            HandleAllegianceOnLogin();
            HandleHouseOnLogin();

            // retail appeared to send the squelch list very early,
            // even before the CreatePlayer, but doing it here
            if (SquelchManager.HasSquelches)
                SquelchManager.SendSquelchDB();

            AuditItemSpells();
            AuditEquippedItems();

            HandleMissingXp();
            HandleSkillCreditRefund();
            HandleSkillTemplesReset();
            HandleSkillSpecCreditRefund();
            HandleFreeSkillResetRenewal();
            HandleFreeAttributeResetRenewal();
            HandleFreeMasteryResetRenewal();

            HandleDBUpdates();

            if (ServerManager.ShutdownInitiated)
            {
                var actionChain = new ActionChain();
                actionChain.AddDelaySeconds(10.0f);
                actionChain.AddAction(this, () =>
                {
                    SendMessage(ServerManager.ShutdownNoticeText(), ChatMessageType.WorldBroadcast);
                });
                actionChain.EnqueueChain();
            }

            log.Debug($"[LOGIN] Account {Account.AccountName} entered the world with character {Name} (0x{Guid}) at {DateTime.Now}.");
        }

        public void SendTurbineChatChannels(bool breakAllegiance = false)
        {
            var allegianceChannel = Allegiance != null && !breakAllegiance ? Allegiance.Biota.Id : 0u;

            var societyChannel = Society switch
            {
                FactionBits.CelestialHand => TurbineChatChannel.SocietyCelestialHand,
                FactionBits.EldrytchWeb => TurbineChatChannel.SocietyEldrytchWeb,
                FactionBits.RadiantBlood => TurbineChatChannel.SocietyRadiantBlood,
                _ => 0u
            };

            Session.Network.EnqueueSend(new GameEventSetTurbineChatChannels(Session, allegianceChannel, societyChannel));
        }

        public void JoinTurbineChatChannel(string channelName)
        {
            if (channelName == "Allegiance" && Allegiance == null)
                return;
            else if (channelName == "Society")
            {
                if (Society == FactionBits.None)
                    return;

                channelName = Society switch
                {
                    FactionBits.CelestialHand => "Celestial Hand",
                    FactionBits.EldrytchWeb => "Eldrytch Web",
                    FactionBits.RadiantBlood => "Radiant Blood",
                    _ => channelName
                };
            }
            else if (channelName == "Olthoi" && (!IsOlthoiPlayer || !IsAdmin))
                return;
            else if (IsOlthoiPlayer && !IsAdmin && channelName != "Olthoi")
                return;

            Session.Network.EnqueueSend(new GameEventWeenieErrorWithString(Session, WeenieErrorWithString.YouHaveEnteredThe_Channel, channelName));

            SendTurbineChatChannels();
        }

        public void LeaveTurbineChatChannel(string channelName, bool breakAllegiance = false)
        {
            if (channelName == "Allegiance" && !breakAllegiance && Allegiance == null)
                return;
            else if (channelName == "Society")
            {
                if (Society == FactionBits.None)
                    return;

                channelName = Society switch
                {
                    FactionBits.CelestialHand => "Celestial Hand",
                    FactionBits.EldrytchWeb => "Eldrytch Web",
                    FactionBits.RadiantBlood => "Radiant Blood",
                    _ => channelName
                };
            }
            else if (channelName == "Olthoi" && (!IsOlthoiPlayer || !IsAdmin))
                return;
            else if (IsOlthoiPlayer && !IsAdmin && channelName != "Olthoi")
                return;

            Session.Network.EnqueueSend(new GameEventWeenieErrorWithString(Session, WeenieErrorWithString.YouHaveLeftThe_Channel, channelName));

            SendTurbineChatChannels(breakAllegiance);
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

            SendContractTrackerTable();
        }

        public void SendPropertyUpdatesAndOverrides()
        {
            if (!PropertyManager.GetBool("require_spell_comps").Item)
                Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyBool(this, PropertyBool.SpellComponentsRequired, false));
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

        public void SendContractTrackerTable()
        {
            if (Character.GetContractsCount(CharacterDatabaseLock) > 0)
                Session.Network.EnqueueSend(new GameEventSendClientContractTrackerTable(Session));
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
        public void SetRequestedLocation(Position pos, bool broadcast = true)
        {
            RequestedLocation = pos;
            RequestedLocationBroadcast = broadcast;
        }

        public MotionCommand LastSoulEmote;
        public DateTime LastSoulEmoteEndTime;

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

            if (state.HasSoulEmote(false))
            {
                // prevent soul emote spam / bug where client sends multiples
                var soulEmote = state.Commands[0].MotionCommand;
                if (soulEmote == LastSoulEmote && DateTime.UtcNow < LastSoulEmoteEndTime)
                {
                    state.Commands.Clear();
                    state.CommandListLength = 0;
                }
                else
                {
                    var animLength = Physics.Animation.MotionTable.GetAnimationLength(MotionTableId, CurrentMotionState.Stance, soulEmote, state.Commands[0].Speed);

                    LastSoulEmote = soulEmote;
                    LastSoulEmoteEndTime = DateTime.UtcNow + TimeSpan.FromSeconds(animLength);
                }
            }

            var movementData = new MovementData(this, moveToState);

            // copy some fields to CurrentMotionState?
            // this is a mess, fix this whole architecture.
            CurrentMotionState.MotionState.ForwardCommand = movementData.Invalid.State.ForwardCommand;
            CurrentMotionState.MotionState.ForwardSpeed = movementData.Invalid.State.ForwardSpeed;
            CurrentMotionState.MotionState.TurnCommand = movementData.Invalid.State.TurnCommand;
            CurrentMotionState.MotionState.TurnSpeed = movementData.Invalid.State.TurnSpeed;
            CurrentMotionState.MotionState.SidestepCommand = movementData.Invalid.State.SidestepCommand;
            CurrentMotionState.MotionState.SidestepSpeed = movementData.Invalid.State.SidestepSpeed;

            var movementEvent = new GameMessageUpdateMotion(this, movementData);
            EnqueueBroadcast(true, movementEvent);    // shouldn't need to go to originating player?

            // TODO: use real motion / animation system from physics
            //CurrentMotionCommand = movementData.Invalid.State.ForwardCommand;
            CurrentMovementData = movementData;
        }

        private EnvironChangeType? currentFogColor;

        public void SetFogColor(EnvironChangeType fogColor)
        {
            if (fogColor == EnvironChangeType.Clear && !currentFogColor.HasValue)
                return;                

            if (LandblockManager.GlobalFogColor.HasValue && currentFogColor != fogColor)
            {
                currentFogColor = LandblockManager.GlobalFogColor;
                SendEnvironChange(currentFogColor.Value);
            }
            else if (currentFogColor != fogColor)
            {
                currentFogColor = fogColor;
                SendEnvironChange(currentFogColor.Value);
            }

            if (currentFogColor == EnvironChangeType.Clear)
                currentFogColor = null;
        }

        public void ClearFogColor()
        {
            SetFogColor(EnvironChangeType.Clear);
        }

        public void SendEnvironChange(EnvironChangeType environChangeType)
        {
            Session.Network.EnqueueSend(new GameMessageAdminEnvirons(Session, environChangeType));
        }

        public void SetPlayerKillerStatus(PlayerKillerStatus playerKillerStatus, bool broadcast = false)
        {
            switch (playerKillerStatus)
            {
                case PlayerKillerStatus.NPK:
                case PlayerKillerStatus.PK:
                case PlayerKillerStatus.PKLite:
                    PlayerKillerStatus = PlayerKillerStatus.NPK;
                    MinimumTimeSincePk = 0;
                    break;
                case PlayerKillerStatus.Free:
                    PlayerKillerStatus = PlayerKillerStatus.Free;
                    break;
            }

            if (broadcast)
                EnqueueBroadcast(new GameMessagePublicUpdatePropertyInt(this, PropertyInt.PlayerKillerStatus, (int)PlayerKillerStatus));
        }

        public void SendWeenieError(WeenieError error)
        {
            Session.Network.EnqueueSend(new GameEventWeenieError(Session, error));
        }

        public void SendWeenieErrorWithString(WeenieErrorWithString error, string str)
        {
            Session.Network.EnqueueSend(new GameEventWeenieErrorWithString(Session, error, str));
        }

        public void SendTransientError(string msg)
        {
            Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, msg));
        }

        public void HandleActionSetAFKMode(bool afkStatus)
        {
            IsAfk = afkStatus;

            Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyBool(this, PropertyBool.Afk, IsAfk));
        }

        public static string DefaultAFKMessage => "I am currently away from the keyboard."; // client default (/afk msg)

        public void HandleActionSetAFKMessage(string afkMessage)
        {
            if (string.IsNullOrWhiteSpace(afkMessage))
                afkMessage = DefaultAFKMessage; // client default

            AfkMessage = afkMessage;
        }

        public void HandlePreOrderItems()
        {
            var subscriptionStatus = (SubscriptionStatus)PropertyManager.GetLong("default_subscription_level").Item;

            string status;
            bool success;
            switch (subscriptionStatus)
            {
                default:
                    status = "purchasing";
                    success = TryCreatePreOrderItem(PropertyBool.ActdReceivedItems, ACE.Entity.Enum.WeenieClassName.W_GEMACTDPURCHASEREWARDARMOR_CLASS);
                    break;
                case SubscriptionStatus.ThroneOfDestiny_Preordered:
                    status = "pre-ordering";
                    TryCreatePreOrderItem(PropertyBool.ActdReceivedItems, ACE.Entity.Enum.WeenieClassName.W_GEMACTDPURCHASEREWARDARMOR_CLASS); // pcaps show this actually didn't occur on retail. odd
                    success = TryCreatePreOrderItem(PropertyBool.ActdPreorderReceivedItems, ACE.Entity.Enum.WeenieClassName.W_GEMACTDPURCHASEREWARDHEALTH_CLASS);
                    break;
            }

            var msg = $"Thank you for {status} the Throne of Destiny expansion! A special gift has been placed in your backpack.";

            if (PropertyManager.GetBool("show_first_login_gift").Item && success)
                Session.Network.EnqueueSend(new GameMessageSystemChat(msg, ChatMessageType.Magic));

            AccountRequirements = subscriptionStatus;
        }

        private bool TryCreatePreOrderItem(PropertyBool propertyBool, WeenieClassName weenieClassName)
        {
            var rcvdBlackmoorsFavor = GetProperty(propertyBool) ?? false;
            if (!rcvdBlackmoorsFavor)
            {
                if (GetInventoryItemsOfWCID((uint)weenieClassName).Count == 0)
                {
                    var cachedWeenie = Database.DatabaseManager.World.GetCachedWeenie((uint)weenieClassName);
                    if (cachedWeenie == null)
                        return false;

                    var wo = Factories.WorldObjectFactory.CreateNewWorldObject(cachedWeenie);
                    if (wo == null)
                        return false;

                    if (TryAddToInventory(wo))
                    {
                        SetProperty(propertyBool, true);
                        return true;
                    }
                }
                else
                    SetProperty(propertyBool, true); // already had the item, set the property to reflect item was received
            }

            return false;
        }
    }
}
