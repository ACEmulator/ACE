using System;
using System.Text;

using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Managers;
using ACE.Server.Network.Enum;
using ACE.Server.Network.GameMessages;
using ACE.Server.Network.GameMessages.Messages;

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

                if (channelID == TurbineChatChannel.Society) // shouldn't ever be hit
                {
                    ChatPacket.SendServerMessage(session, "You do not belong to a society.", ChatMessageType.Broadcast); // I don't know if this is how it was done on the live servers
                    return;
                }

                var gameMessageTurbineChat = new GameMessageTurbineChat(ChatNetworkBlobType.NETBLOB_EVENT_BINARY, channelID, session.Player.Name, message, senderID, chatType);

                if (channelID > TurbineChatChannel.SocietyRadiantBlood) // Channel must be an allegiance channel
                {
                    var allegiance = AllegianceManager.FindAllegiance(channelID);
                    if (allegiance != null)
                    {
                        // is sender booted / gagged?
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
                }
                else if (channelID > TurbineChatChannel.Society) // Channel must be a society restricted channel
                {
                    //var senderSociety = session.Player.Society;

                    //var adjustedChatType = senderSociety switch
                    //{
                    //    FactionBits.CelestialHand => ChatType.SocietyCelHan,
                    //    FactionBits.EldrytchWeb => ChatType.SocietyEldWeb,
                    //    FactionBits.RadiantBlood => ChatType.SocietyRadBlo,
                    //    _ => ChatType.Society
                    //};

                    //gameMessageTurbineChat = new GameMessageTurbineChat(ChatNetworkBlobType.NETBLOB_EVENT_BINARY, channelID, session.Player.Name, message, senderID, adjustedChatType);

                    foreach (var recipient in PlayerManager.GetAllOnline())
                    {
                        // handle filters
                        if (session.Player.Society != recipient.Society)
                            continue;

                        if (!recipient.GetCharacterOption(CharacterOption.ListenToSocietyChat))
                            continue;

                        if (recipient.SquelchManager.Squelches.Contains(session.Player, ChatMessageType.AllChannels))
                            continue;

                        recipient.Session.Network.EnqueueSend(gameMessageTurbineChat);
                    }

                    session.Network.EnqueueSend(new GameMessageTurbineChat(ChatNetworkBlobType.NETBLOB_RESPONSE_BINARY, contextId, null, null, 0, chatType));
                }
                else if (channelID == TurbineChatChannel.Olthoi) // Channel must be the Olthoi play channel
                {
                    // todo: olthoi play chat (ha! yeah right...)
                }
                else // Channel must be available to all players
                {
                    foreach (var recipient in PlayerManager.GetAllOnline())
                    {
                        // handle filters
                        if (channelID == TurbineChatChannel.General && !recipient.GetCharacterOption(CharacterOption.ListenToGeneralChat) ||
                            channelID == TurbineChatChannel.Trade && !recipient.GetCharacterOption(CharacterOption.ListenToTradeChat) ||
                            channelID == TurbineChatChannel.LFG && !recipient.GetCharacterOption(CharacterOption.ListenToLFGChat) ||
                            channelID == TurbineChatChannel.Roleplay && !recipient.GetCharacterOption(CharacterOption.ListenToRoleplayChat))
                            continue;

                        if (recipient.SquelchManager.Squelches.Contains(session.Player, ChatMessageType.AllChannels))
                            continue;

                        recipient.Session.Network.EnqueueSend(gameMessageTurbineChat);
                    }

                    session.Network.EnqueueSend(new GameMessageTurbineChat(ChatNetworkBlobType.NETBLOB_RESPONSE_BINARY, contextId, null, null, 0, chatType));
                }
            }
            else
                Console.WriteLine($"Unhandled TurbineChatHandler ChatNetworkBlobType: 0x{(uint)chatBlobType:X4}");
        }
    }
}
