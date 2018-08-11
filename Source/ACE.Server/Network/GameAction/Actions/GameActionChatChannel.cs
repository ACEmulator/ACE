using System;
using ACE.Common.Extensions;
using ACE.Entity.Enum;
using ACE.Server.Managers;
using ACE.Server.Network.GameEvent.Events;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionChatChannel
    {
        [GameAction(GameActionType.ChatChannel)]
        public static void Handle(ClientMessage clientMessage, Session session)
        {
            var groupChatType = (Channel)clientMessage.Payload.ReadUInt32();
            var message = clientMessage.Payload.ReadString16L();
            switch (groupChatType)
            {
                case Channel.Abuse:
                    {
                        // TODO: Proper permissions check. This command should work for any character with AccessLevel.Advocate or higher
                        // if (!session.Player.IsAdmin)
                        //    break;

                        // TODO This should check if the recipient is subscribed to the channel
                        foreach (var recipient in WorldManager.GetAll())
                            if (recipient != session)
                                recipient.Network.EnqueueSend(new GameEvent.Events.GameEventChannelBroadcast(recipient, groupChatType, session.Player.Name, message));
                            else
                                recipient.Network.EnqueueSend(new GameEvent.Events.GameEventChannelBroadcast(recipient, groupChatType, "", message));
                    }
                    break;
                case Channel.Admin:
                    {
                        if (!session.Player.IsAdmin)
                            break;

                        // TODO This should check if the recipient is subscribed to the channel
                        foreach (var recipient in WorldManager.GetAll())
                            if (recipient != session)
                                recipient.Network.EnqueueSend(new GameEvent.Events.GameEventChannelBroadcast(recipient, groupChatType, session.Player.Name, message));
                            else
                                recipient.Network.EnqueueSend(new GameEvent.Events.GameEventChannelBroadcast(recipient, groupChatType, "", message));
                        // NetworkManager.SendWorldMessage(recipient, gameMessageSystemChat);
                    }
                    break;
                case Channel.Audit:
                    {
                        // TODO: Proper permissions check. This command should work for any character AccessLevel.Sentinel or higher
                        // if (!session.Player.IsAdmin)
                        //    break;

                        // TODO This should check if the recipient is subscribed to the channel
                        foreach (var recipient in WorldManager.GetAll())
                            if (recipient != session)
                                recipient.Network.EnqueueSend(new GameEvent.Events.GameEventChannelBroadcast(recipient, groupChatType, session.Player.Name, message));
                            else
                                recipient.Network.EnqueueSend(new GameEvent.Events.GameEventChannelBroadcast(recipient, groupChatType, "", message));
                    }
                    break;
                case Channel.Advocate1:
                    {
                        // TODO: Proper permissions check. This command should work for any character AccessLevel.Advocate or higher
                        // if (!session.Player.IsAdmin)
                        //    break;

                        // TODO This should check if the recipient is subscribed to the channel
                        foreach (var recipient in WorldManager.GetAll())
                            if (recipient != session)
                                recipient.Network.EnqueueSend(new GameEvent.Events.GameEventChannelBroadcast(recipient, groupChatType, session.Player.Name, message));
                            else
                                recipient.Network.EnqueueSend(new GameEvent.Events.GameEventChannelBroadcast(recipient, groupChatType, "", message));
                    }
                    break;
                case Channel.Advocate2:
                    {
                        // TODO: Proper permissions check. This command should work for any character AccessLevel.Advocate or higher
                        // if (!session.Player.IsAdmin)
                        //    break;

                        // TODO This should check if the recipient is subscribed to the channel
                        foreach (var recipient in WorldManager.GetAll())
                            if (recipient != session)
                                recipient.Network.EnqueueSend(new GameEvent.Events.GameEventChannelBroadcast(recipient, groupChatType, session.Player.Name, message));
                            else
                                recipient.Network.EnqueueSend(new GameEvent.Events.GameEventChannelBroadcast(recipient, groupChatType, "", message));
                    }
                    break;
                case Channel.Advocate3:
                    {
                        // TODO: Proper permissions check. This command should work for any character AccessLevel.Advocate or higher
                        // if (!session.Player.IsAdmin)
                        //    break;

                        // TODO This should check if the recipient is subscribed to the channel
                        foreach (var recipient in WorldManager.GetAll())
                            if (recipient != session)
                                recipient.Network.EnqueueSend(new GameEvent.Events.GameEventChannelBroadcast(recipient, groupChatType, session.Player.Name, message));
                            else
                                recipient.Network.EnqueueSend(new GameEvent.Events.GameEventChannelBroadcast(recipient, groupChatType, "", message));
                    }
                    break;
                case Channel.Sentinel:
                    {
                        // TODO: Proper permissions check. This command should work for any character with AccessLevel.Sentinel or higher
                        // if (!session.Player.IsAdmin)
                        //    break;

                        // TODO This should check if the recipient is subscribed to the channel
                        foreach (var recipient in WorldManager.GetAll())
                            if (recipient != session)
                                recipient.Network.EnqueueSend(new GameEvent.Events.GameEventChannelBroadcast(recipient, groupChatType, session.Player.Name, message));
                            else
                                recipient.Network.EnqueueSend(new GameEvent.Events.GameEventChannelBroadcast(recipient, groupChatType, "", message));
                    }
                    break;
                case Channel.Help:
                    {
                        ChatPacket.SendServerMessage(session, "GameActionChatChannel TellHelp Needs work.", ChatMessageType.Broadcast);
                        // TODO: I don't remember exactly how this was triggered. I don't think it sent back a "You say" message to the person who triggered it
                        // TODO: Proper permissions check. Requesting urgent help should work for any character but only displays the "says" mesage for those subscribed to the Help channel
                        //      which would be Advocates and above.
                        // if (!session.Player.IsAdmin)
                        //    break;
                        string onTheWhatChannel = "on the " + System.Enum.GetName(typeof(Channel), groupChatType).Replace("Tell", "") + " channel";
                        string whoSays = session.Player.Name + " says ";

                        // ChatPacket.SendServerMessage(session, $"You say {onTheWhatChannel}, \"{message}\"", ChatMessageType.OutgoingHelpSay);

                        var gameMessageSystemChat = new GameMessages.Messages.GameMessageSystemChat(whoSays + onTheWhatChannel + ", \"" + message + "\"", ChatMessageType.Help);

                        // TODO This should check if the recipient is subscribed to the channel
                        foreach (var recipient in WorldManager.GetAll())
                            if (recipient != session)
                                recipient.Network.EnqueueSend(gameMessageSystemChat);

                        // again not sure what way to go with this.. the code below was added after I realized I should be handling things differently
                        // and by handling differently I mean letting the client do all of the work it was already designed to do.

                        // foreach (var recipient in WorldManager.GetAll())
                        //    if (recipient != session)
                        //        NetworkManager.SendWorldMessage(recipient, new GameEvent.Events.GameEventChannelBroadcast(recipient, groupChatType, session.Player.Name, message));
                        //    else
                        //        NetworkManager.SendWorldMessage(recipient, new GameEvent.Events.GameEventChannelBroadcast(recipient, groupChatType, "", message));
                    }
                    break;

                case Channel.Fellow:
                    {
                        var statusMessage = new GameEventWeenieError(session, WeenieError.YouDoNotBelongToAFellowship);
                        session.Network.EnqueueSend(statusMessage);

                        ChatPacket.SendServerMessage(session, "GameActionChatChannel TellFellowship Needs work.", ChatMessageType.Broadcast);
                    }
                    break;

                case Channel.Vassals:
                    {
                        ChatPacket.SendServerMessage(session, "GameActionChatChannel TellVassals Needs work.", ChatMessageType.Broadcast);
                    }
                    break;

                case Channel.Patron:
                    {
                        ChatPacket.SendServerMessage(session, "GameActionChatChannel TellPatron Needs work.", ChatMessageType.Broadcast);
                    }
                    break;

                case Channel.Monarch:
                    {
                        ChatPacket.SendServerMessage(session, "GameActionChatChannel TellMonarch Needs work.", ChatMessageType.Broadcast);
                    }
                    break;

                case Channel.CoVassals:
                    {
                        ChatPacket.SendServerMessage(session, "GameActionChatChannel TellCoVassals Needs work.", ChatMessageType.Broadcast);
                    }
                    break;

                case Channel.AllegianceBroadcast:
                    {
                        // The client knows if we're in an allegiance or not, and will throw an error to the user if they try to /a, and no message will be dispatched to the server.

                        ChatPacket.SendServerMessage(session, "GameActionChatChannel AllegianceBroadcast Needs work.", ChatMessageType.Broadcast);
                    }
                    break;

                default:
                    Console.WriteLine($"Unhandled ChatChannel GroupChatType: 0x{(uint)groupChatType:X4}");
                    break;
            }
        }
    }
}
