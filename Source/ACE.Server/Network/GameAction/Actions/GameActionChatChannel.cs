using System;
using System.Collections.Generic;
using ACE.Common.Extensions;
using ACE.Database;
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
                        // Should anyone be able to post to this channel? If so should they just a response back stating their message has been posted.
                        // Should messages here also be written to a log?
                        if (session.AccessLevel < AccessLevel.Advocate)
                        {
                            var statusMessage = new GameEventWeenieError(session, WeenieError.YouCantUseThatChannel);
                            session.Network.EnqueueSend(statusMessage);
                            break;
                        }

                        foreach (var recipient in WorldManager.GetAll())
                            if (recipient != session)
                            {
                                if (recipient.AccessLevel >= AccessLevel.Advocate)
                                    recipient.Network.EnqueueSend(new GameEvent.Events.GameEventChannelBroadcast(recipient, groupChatType, session.Player.Name, message));
                            }
                            else
                            {
                                recipient.Network.EnqueueSend(new GameEvent.Events.GameEventChannelBroadcast(recipient, groupChatType, "", message));
                            }
                    }
                    break;
                case Channel.Admin:
                    {
                        if (!session.Player.IsAdmin)
                        {
                            var statusMessage = new GameEventWeenieError(session, WeenieError.YouCantUseThatChannel);
                            session.Network.EnqueueSend(statusMessage);
                            break;
                        }

                        foreach (var recipient in WorldManager.GetAll())
                            if (recipient != session)
                            {
                                if (recipient.AccessLevel == AccessLevel.Admin)
                                    recipient.Network.EnqueueSend(new GameEvent.Events.GameEventChannelBroadcast(recipient, groupChatType, session.Player.Name, message));
                            }
                            else
                            {
                                recipient.Network.EnqueueSend(new GameEvent.Events.GameEventChannelBroadcast(recipient, groupChatType, "", message));
                            }
                    }
                    break;
                case Channel.Audit:
                    {
                        if (session.AccessLevel < AccessLevel.Sentinel)
                        {
                            var statusMessage = new GameEventWeenieError(session, WeenieError.YouCantUseThatChannel);
                            session.Network.EnqueueSend(statusMessage);
                            break;
                        }

                        foreach (var recipient in WorldManager.GetAll())
                            if (recipient != session)
                            {
                                if (recipient.AccessLevel >= AccessLevel.Sentinel)
                                    recipient.Network.EnqueueSend(new GameEvent.Events.GameEventChannelBroadcast(recipient, groupChatType, session.Player.Name, message));
                            }
                            else
                            {
                                recipient.Network.EnqueueSend(new GameEvent.Events.GameEventChannelBroadcast(recipient, groupChatType, "", message));
                            }
                    }
                    break;
                case Channel.Advocate1:
                    {
                        if (session.AccessLevel < AccessLevel.Advocate)
                        {
                            var statusMessage = new GameEventWeenieError(session, WeenieError.YouCantUseThatChannel);
                            session.Network.EnqueueSend(statusMessage);
                            break;
                        }

                        foreach (var recipient in WorldManager.GetAll())
                            if (recipient != session)
                            {
                                if (recipient.AccessLevel >= AccessLevel.Advocate)
                                    recipient.Network.EnqueueSend(new GameEvent.Events.GameEventChannelBroadcast(recipient, groupChatType, session.Player.Name, message));
                            }
                            else
                            {
                                recipient.Network.EnqueueSend(new GameEvent.Events.GameEventChannelBroadcast(recipient, groupChatType, "", message));
                            }
                    }
                    break;
                case Channel.Advocate2:
                    {
                        if (session.AccessLevel < AccessLevel.Advocate)
                        {
                            var statusMessage = new GameEventWeenieError(session, WeenieError.YouCantUseThatChannel);
                            session.Network.EnqueueSend(statusMessage);
                            break;
                        }

                        foreach (var recipient in WorldManager.GetAll())
                            if (recipient != session)
                            {
                                if (recipient.AccessLevel >= AccessLevel.Advocate)
                                    recipient.Network.EnqueueSend(new GameEvent.Events.GameEventChannelBroadcast(recipient, groupChatType, session.Player.Name, message));
                            }
                            else
                            {
                                recipient.Network.EnqueueSend(new GameEvent.Events.GameEventChannelBroadcast(recipient, groupChatType, "", message));
                            }
                    }
                    break;
                case Channel.Advocate3:
                    {
                        if (session.AccessLevel < AccessLevel.Advocate)
                        {
                            var statusMessage = new GameEventWeenieError(session, WeenieError.YouCantUseThatChannel);
                            session.Network.EnqueueSend(statusMessage);
                            break;
                        }

                        foreach (var recipient in WorldManager.GetAll())
                            if (recipient != session)
                            {
                                if (recipient.AccessLevel >= AccessLevel.Advocate)
                                    recipient.Network.EnqueueSend(new GameEvent.Events.GameEventChannelBroadcast(recipient, groupChatType, session.Player.Name, message));
                            }
                            else
                            {
                                recipient.Network.EnqueueSend(new GameEvent.Events.GameEventChannelBroadcast(recipient, groupChatType, "", message));
                            }
                    }
                    break;
                case Channel.Sentinel:
                    {
                        if (session.AccessLevel < AccessLevel.Sentinel)
                        {
                            var statusMessage = new GameEventWeenieError(session, WeenieError.YouCantUseThatChannel);
                            session.Network.EnqueueSend(statusMessage);
                            break;
                        }

                        foreach (var recipient in WorldManager.GetAll())
                            if (recipient != session)
                            {
                                if (recipient.AccessLevel >= AccessLevel.Sentinel)
                                    recipient.Network.EnqueueSend(new GameEvent.Events.GameEventChannelBroadcast(recipient, groupChatType, session.Player.Name, message));
                            }
                            else
                            {
                                recipient.Network.EnqueueSend(new GameEvent.Events.GameEventChannelBroadcast(recipient, groupChatType, "", message));
                            }
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
                        if (session.Player.Fellowship == null)
                        {
                            var statusMessage = new GameEventWeenieError(session, WeenieError.YouDoNotBelongToAFellowship);
                            session.Network.EnqueueSend(statusMessage);
                            break;
                        }

                        foreach (var fellowmember in session.Player.Fellowship.FellowshipMembers)
                            if (fellowmember.Session != session)
                                fellowmember.Session.Network.EnqueueSend(new GameEvent.Events.GameEventChannelBroadcast(fellowmember.Session, groupChatType, session.Player.Name, message));
                            else
                                session.Network.EnqueueSend(new GameEvent.Events.GameEventChannelBroadcast(session, groupChatType, "", message));
                    }
                    break;
                case Channel.Vassals:
                    {
                        if (!session.Player.HasAllegiance)
                        {
                            var statusMessage = new GameEventWeenieError(session, WeenieError.YouAreNotInAllegiance);
                            session.Network.EnqueueSend(statusMessage);
                            break;
                        }

                        if (session.Player.AllegianceNode.TotalVassals == 0)
                        {
                            var statusMessage = new GameEventWeenieError(session, WeenieError.YouCantUseThatChannel);
                            session.Network.EnqueueSend(statusMessage);
                            break;
                        }

                        foreach (var vassal in session.Player.AllegianceNode.Vassals)
                        {
                            string vassalName = vassal.Player.Name;

                            if (DatabaseManager.Authentication.GetAccountById(vassal.Player.Character.AccountId).AccessLevel == 5)
                                vassalName = "+" + vassalName;

                            Session vassalSession = WorldManager.FindByPlayerName(vassalName);

                            if (vassalSession != null)
                                vassalSession.Network.EnqueueSend(new GameEvent.Events.GameEventChannelBroadcast(vassalSession.Player.Session, groupChatType, session.Player.Name, message));
                        }
                        session.Network.EnqueueSend(new GameEvent.Events.GameEventChannelBroadcast(session, groupChatType, "", message));
                    }
                    break;
                case Channel.Patron:
                    {
                        if (!session.Player.HasAllegiance)
                        {
                            var statusMessage = new GameEventWeenieError(session, WeenieError.YouAreNotInAllegiance);
                            session.Network.EnqueueSend(statusMessage);
                            break;
                        }

                        if (!session.Player.Patron.HasValue)
                        {
                            var statusMessage = new GameEventWeenieError(session, WeenieError.YouCantUseThatChannel);
                            session.Network.EnqueueSend(statusMessage);
                            break;
                        }

                        string patronName = session.Player.AllegianceNode.Patron.Player.Name;

                        if (DatabaseManager.Authentication.GetAccountById(session.Player.AllegianceNode.Patron.Player.Character.AccountId).AccessLevel == 5)
                            patronName = "+" + patronName;

                        Session patronSession = WorldManager.FindByPlayerName(patronName);

                        if (patronSession != null)
                            patronSession.Network.EnqueueSend(new GameEvent.Events.GameEventChannelBroadcast(patronSession.Player.Session, groupChatType, session.Player.Name, message));

                        session.Network.EnqueueSend(new GameEvent.Events.GameEventChannelBroadcast(session, groupChatType, "", message));
                    }
                    break;
                case Channel.Monarch:
                    {
                        if (!session.Player.HasAllegiance)
                        {
                            var statusMessage = new GameEventWeenieError(session, WeenieError.YouAreNotInAllegiance);
                            session.Network.EnqueueSend(statusMessage);
                            break;
                        }

                        if (!session.Player.Monarch.HasValue)
                        {
                            var statusMessage = new GameEventWeenieError(session, WeenieError.YouCantUseThatChannel);
                            session.Network.EnqueueSend(statusMessage);
                            break;
                        }

                        string monarchName = session.Player.AllegianceNode.Monarch.Player.Name;

                        if (DatabaseManager.Authentication.GetAccountById(session.Player.AllegianceNode.Monarch.Player.Character.AccountId).AccessLevel == 5)
                            monarchName = "+" + monarchName;

                        Session monarchSession = WorldManager.FindByPlayerName(monarchName);

                        if (monarchSession != null)
                            monarchSession.Network.EnqueueSend(new GameEvent.Events.GameEventChannelBroadcast(monarchSession.Player.Session, Channel.Patron, session.Player.Name, message));

                        session.Network.EnqueueSend(new GameEvent.Events.GameEventChannelBroadcast(session, groupChatType, "", message));
                    }
                    break;
                case Channel.CoVassals:
                    {
                        if (!session.Player.HasAllegiance)
                        {
                            var statusMessage = new GameEventWeenieError(session, WeenieError.YouAreNotInAllegiance);
                            session.Network.EnqueueSend(statusMessage);
                            break;
                        }

                        if (!session.Player.Patron.HasValue)
                        {
                            var statusMessage = new GameEventWeenieError(session, WeenieError.YouCantUseThatChannel);
                            session.Network.EnqueueSend(statusMessage);
                            break;
                        }

                        string patronName = session.Player.AllegianceNode.Patron.Player.Name;

                        if (DatabaseManager.Authentication.GetAccountById(session.Player.AllegianceNode.Patron.Player.Character.AccountId).AccessLevel == 5)
                            patronName = "+" + patronName;

                        Session patronSession = WorldManager.FindByPlayerName(patronName);

                        if (patronSession != null)
                            patronSession.Network.EnqueueSend(new GameEvent.Events.GameEventChannelBroadcast(patronSession.Player.Session, Channel.Patron, session.Player.Name, message));

                        foreach (var covassal in session.Player.AllegianceNode.Patron.Vassals)
                        {
                            string vassalName = covassal.Player.Name;

                            if (DatabaseManager.Authentication.GetAccountById(covassal.Player.Character.AccountId).AccessLevel == 5)
                                vassalName = "+" + vassalName;

                            Session covassalSession = WorldManager.FindByPlayerName(vassalName);

                            if (vassalName == session.Player.Name)
                            {
                                session.Network.EnqueueSend(new GameEvent.Events.GameEventChannelBroadcast(session, groupChatType, "", message));
                            }
                            else
                            {
                                if (covassalSession != null)
                                    covassalSession.Network.EnqueueSend(new GameEvent.Events.GameEventChannelBroadcast(covassalSession.Player.Session, groupChatType, session.Player.Name, message));
                            }
                        }
                    }
                    break;
                case Channel.AllegianceBroadcast:
                    {
                        // The client knows if we're in an allegiance or not, and will throw an error to the user if they try to /a, and no message will be dispatched to the server.
                        // 
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
