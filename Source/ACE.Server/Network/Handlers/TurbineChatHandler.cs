using System;
using System.Reflection;
using System.Text;

using ACE.Entity.Enum;
using ACE.Server.Managers;
using ACE.Server.Network.Enum;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages;
using ACE.Server.Network.GameMessages.Messages;

using log4net;

namespace ACE.Server.Network.Handlers
{
    public static class TurbineChatHandler
    {
        [GameMessage(GameMessageOpcode.TurbineChat, SessionState.WorldConnected)]
        public static void TurbineChatReceived(ClientMessage clientMessage, Session session)
        {
            if (!PropertyManager.GetBool("use_turbine_chat").Item)
                return;

            clientMessage.Payload.ReadUInt32(); // Bytes to follow
            var chatBlobType = (ChatNetworkBlobType)clientMessage.Payload.ReadUInt32();
            clientMessage.Payload.ReadUInt32(); // Always 2
            clientMessage.Payload.ReadUInt32(); // Always 1
            clientMessage.Payload.ReadUInt32(); // Always 0
            clientMessage.Payload.ReadUInt32(); // Always 0
            clientMessage.Payload.ReadUInt32(); // Always 0
            clientMessage.Payload.ReadUInt32(); // Always 0
            clientMessage.Payload.ReadUInt32(); // Bytes to follow

            if (session.Player.IsGagged)
            {
                session.Player.SendGagError();
                return;
            }

            if (chatBlobType == ChatNetworkBlobType.NETBLOB_REQUEST_BINARY)
            {
                var contextId = clientMessage.Payload.ReadUInt32(); // 0x01 - 0x71 (maybe higher), typically though 0x01 - 0x0F
                clientMessage.Payload.ReadUInt32(); // Always 2
                clientMessage.Payload.ReadUInt32(); // Always 2
                var channelID = clientMessage.Payload.ReadUInt32();

                int messageLen = clientMessage.Payload.ReadByte();
                if ((messageLen & 0x80) > 0) // PackedByte
                {
                    byte lowbyte = clientMessage.Payload.ReadByte();
                    messageLen = ((messageLen & 0x7F) << 8) | lowbyte;
                }
                var messageBytes = clientMessage.Payload.ReadBytes(messageLen * 2);
                var message = Encoding.Unicode.GetString(messageBytes);

                clientMessage.Payload.ReadUInt32(); // Always 0x0C
                var senderID = clientMessage.Payload.ReadUInt32();
                clientMessage.Payload.ReadUInt32(); // Always 0
                var chatType = (ChatType)clientMessage.Payload.ReadUInt32();

                var gameMessageTurbineChat = new GameMessageTurbineChat(ChatNetworkBlobType.NETBLOB_EVENT_BINARY, channelID, session.Player.Name, message, senderID, chatType);

                switch (chatType)
                {
                    case ChatType.Allegiance:

                        //var allegiance = AllegianceManager.FindAllegiance(channelID);
                        var allegiance = AllegianceManager.GetAllegiance(session.Player);
                        if (allegiance != null)
                        {
                            // is sender booted / gagged?
                            if (!allegiance.IsMember(session.Player.Guid)) return;
                            if (allegiance.IsFiltered(session.Player.Guid)) return;

                            // iterate through all allegiance members
                            foreach (var member in allegiance.Members.Keys)
                            {
                                // is this allegiance member online?
                                var online = PlayerManager.GetOnlinePlayer(member);
                                if (online == null)
                                    continue;

                                // is this member booted / gagged?
                                if (allegiance.IsFiltered(member) || online.SquelchManager.Squelches.Contains(session.Player, ChatMessageType.Allegiance)) continue;

                                // does this player have allegiance chat filtered?
                                if (!online.GetCharacterOption(CharacterOption.ListenToAllegianceChat)) continue;

                                online.Session.Network.EnqueueSend(gameMessageTurbineChat);
                            }

                            session.Network.EnqueueSend(new GameMessageTurbineChat(ChatNetworkBlobType.NETBLOB_RESPONSE_BINARY, contextId, null, null, 0, chatType));
                        }

                        break;

                    case ChatType.General:
                    case ChatType.LFG:
                    case ChatType.Roleplay:
                    case ChatType.Trade:

                        if (PropertyManager.GetBool("chat_echo_only").Item)
                        {
                            session.Network.EnqueueSend(gameMessageTurbineChat);
                            session.Network.EnqueueSend(new GameMessageTurbineChat(ChatNetworkBlobType.NETBLOB_RESPONSE_BINARY, contextId, null, null, 0, chatType));
                            return;
                        }

                        if (PropertyManager.GetBool("chat_requires_account_15days").Item && !session.Player.Account15Days)
                        {
                            HandleChatReject(session, contextId, chatType, gameMessageTurbineChat, "because this account is not 15 days old");
                            return;
                        }

                        var chat_requires_account_time_seconds = PropertyManager.GetLong("chat_requires_account_time_seconds").Item;
                        if (chat_requires_account_time_seconds > 0 && (DateTime.UtcNow - session.Player.Account.CreateTime).TotalSeconds < chat_requires_account_time_seconds)
                        {
                            HandleChatReject(session, contextId, chatType, gameMessageTurbineChat, "because this account is not old enough");
                            return;
                        }

                        var chat_requires_player_age = PropertyManager.GetLong("chat_requires_player_age").Item;
                        if (chat_requires_player_age > 0 && session.Player.Age < chat_requires_player_age)
                        {
                            HandleChatReject(session, contextId, chatType, gameMessageTurbineChat, "because this character has not been played enough");
                            return;
                        }

                        var chat_requires_player_level = PropertyManager.GetLong("chat_requires_player_level").Item;
                        if (chat_requires_player_level > 0 && session.Player.Level < chat_requires_player_level)
                        {
                            HandleChatReject(session, contextId, chatType, gameMessageTurbineChat, $"because this character has reached level {chat_requires_player_level}");
                            return;
                        }

                        foreach (var recipient in PlayerManager.GetAllOnline())
                        {
                            // handle filters
                            if (chatType == ChatType.General && !recipient.GetCharacterOption(CharacterOption.ListenToGeneralChat) ||
                                chatType == ChatType.Trade && !recipient.GetCharacterOption(CharacterOption.ListenToTradeChat) ||
                                chatType == ChatType.LFG && !recipient.GetCharacterOption(CharacterOption.ListenToLFGChat) ||
                                chatType == ChatType.Roleplay && !recipient.GetCharacterOption(CharacterOption.ListenToRoleplayChat))
                                continue;

                            if ((chatType == ChatType.General && PropertyManager.GetBool("chat_disable_general").Item)
                                || (chatType == ChatType.Trade && PropertyManager.GetBool("chat_disable_trade").Item)
                                || (chatType == ChatType.LFG && PropertyManager.GetBool("chat_disable_lfg").Item)
                                || (chatType == ChatType.Roleplay && PropertyManager.GetBool("chat_disable_roleplay").Item))
                            {
                                HandleChatReject(session, contextId, chatType, gameMessageTurbineChat, null);
                                return;
                            }

                            if (recipient.SquelchManager.Squelches.Contains(session.Player, ChatMessageType.AllChannels))
                                continue;

                            recipient.Session.Network.EnqueueSend(gameMessageTurbineChat);
                        }

                        session.Network.EnqueueSend(new GameMessageTurbineChat(ChatNetworkBlobType.NETBLOB_RESPONSE_BINARY, contextId, null, null, 0, chatType));

                        break;

                    case ChatType.Society:
                    case ChatType.SocietyCelHan:
                    case ChatType.SocietyEldWeb:
                    case ChatType.SocietyRadBlo:

                        var senderSociety = session.Player.Society;

                        //var adjustedChatType = senderSociety switch
                        //{
                        //    FactionBits.CelestialHand => ChatType.SocietyCelHan,
                        //    FactionBits.EldrytchWeb => ChatType.SocietyEldWeb,
                        //    FactionBits.RadiantBlood => ChatType.SocietyRadBlo,
                        //    _ => ChatType.Society
                        //};

                        //gameMessageTurbineChat = new GameMessageTurbineChat(ChatNetworkBlobType.NETBLOB_EVENT_BINARY, channelID, session.Player.Name, message, senderID, adjustedChatType);

                        if (senderSociety == FactionBits.None)
                        {
                            ChatPacket.SendServerMessage(session, "You do not belong to a society.", ChatMessageType.Broadcast); // I don't know if this is how it was done on the live servers
                            return;
                        }

                        foreach (var recipient in PlayerManager.GetAllOnline())
                        {
                            // handle filters
                            if (senderSociety != recipient.Society && !recipient.IsAdmin)
                                continue;

                            if (!recipient.GetCharacterOption(CharacterOption.ListenToSocietyChat))
                                continue;

                            if (recipient.SquelchManager.Squelches.Contains(session.Player, ChatMessageType.AllChannels))
                                continue;

                            recipient.Session.Network.EnqueueSend(gameMessageTurbineChat);
                        }

                        session.Network.EnqueueSend(new GameMessageTurbineChat(ChatNetworkBlobType.NETBLOB_RESPONSE_BINARY, contextId, null, null, 0, chatType));

                        break;

                    case ChatType.Olthoi:

                        // todo: olthoi play chat (ha! yeah right...)

                        break;

                }                

                LogTurbineChat(channelID, session.Player.Name, message, senderID, chatType);
            }
            else
                Console.WriteLine($"Unhandled TurbineChatHandler ChatNetworkBlobType: 0x{(uint)chatBlobType:X4}");
        }

        private static void HandleChatReject(Session session, uint contextId, ChatType chatType, GameMessageTurbineChat gameMessageTurbineChat, string rejectReason)
        {
            if (PropertyManager.GetBool("chat_echo_reject").Item)
                session.Network.EnqueueSend(gameMessageTurbineChat);

            if (PropertyManager.GetBool("chat_inform_reject").Item)
            {
                session.Network.EnqueueSend(new GameEventCommunicationTransientString(session, $"{chatType} is currently disabled{(string.IsNullOrEmpty(rejectReason) ? "" : $" for you {rejectReason}")}."));
                session.Network.EnqueueSend(new GameMessageSystemChat($"{chatType} is currently disabled{(string.IsNullOrEmpty(rejectReason) ? "" : $" for you {rejectReason}")}.", ChatMessageType.Broadcast));
            }

            session.Network.EnqueueSend(new GameMessageTurbineChat(ChatNetworkBlobType.NETBLOB_RESPONSE_BINARY, contextId, null, null, 0, chatType));
        }

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static void LogTurbineChat(uint channelID, string name, string message, uint senderID, ChatType chatType)
        {
            switch (chatType)
            {
                case ChatType.Allegiance:
                    if (!PropertyManager.GetBool("chat_log_allegiance").Item)
                        return;
                    break;
                case ChatType.General:
                    if (!PropertyManager.GetBool("chat_log_general").Item)
                        return;
                    break;
                case ChatType.LFG:
                    if (!PropertyManager.GetBool("chat_log_lfg").Item)
                        return;
                    break;
                case ChatType.Olthoi:
                    if (!PropertyManager.GetBool("chat_log_olthoi").Item)
                        return;
                    break;
                case ChatType.Roleplay:
                    if (!PropertyManager.GetBool("chat_log_roleplay").Item)
                        return;
                    break;
                case ChatType.Society:
                case ChatType.SocietyCelHan:
                case ChatType.SocietyEldWeb:
                case ChatType.SocietyRadBlo:
                    if (!PropertyManager.GetBool("chat_log_society").Item)
                        return;
                    break;
                case ChatType.Trade:
                    if (!PropertyManager.GetBool("chat_log_trade").Item)
                        return;
                    break;
                default:
                    return;
            }

            log.Info($"[CHAT][{chatType}]{(chatType == ChatType.Allegiance ? $"[{channelID}]" : "")} {name} says, \"{message}\"");
        }
    }
}
