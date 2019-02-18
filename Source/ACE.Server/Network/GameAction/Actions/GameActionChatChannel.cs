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
                        // Should anyone be able to post to this channel? If so should they just a response back stating their message has been posted.
                        // Should messages here also be written to a log?
                        if (session.AccessLevel < AccessLevel.Advocate)
                        {
                            var statusMessage = new GameEventWeenieError(session, WeenieError.YouCantUseThatChannel);
                            session.Network.EnqueueSend(statusMessage);
                            break;
                        }

                        foreach (var recipient in PlayerManager.GetAllOnline())
                            if (recipient.Session != session)
                            {
                                if (recipient.Session.AccessLevel >= AccessLevel.Advocate && !recipient.Squelches.Contains(session.Player))
                                    recipient.Session.Network.EnqueueSend(new GameEventChannelBroadcast(recipient.Session, groupChatType, session.Player.Name, message));
                            }
                            else
                            {
                                recipient.Session.Network.EnqueueSend(new GameEventChannelBroadcast(recipient.Session, groupChatType, "", message));
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

                        foreach (var recipient in PlayerManager.GetAllOnline())
                            if (recipient.Session != session)
                            {
                                if (recipient.Session.AccessLevel == AccessLevel.Admin && !recipient.Squelches.Contains(session.Player))
                                    recipient.Session.Network.EnqueueSend(new GameEventChannelBroadcast(recipient.Session, groupChatType, session.Player.Name, message));
                            }
                            else
                            {
                                recipient.Session.Network.EnqueueSend(new GameEventChannelBroadcast(recipient.Session, groupChatType, "", message));
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

                        foreach (var recipient in PlayerManager.GetAllOnline())
                            if (recipient.Session != session)
                            {
                                if (recipient.Session.AccessLevel >= AccessLevel.Sentinel && !recipient.Squelches.Contains(session.Player))
                                    recipient.Session.Network.EnqueueSend(new GameEventChannelBroadcast(recipient.Session, groupChatType, session.Player.Name, message));
                            }
                            else
                            {
                                recipient.Session.Network.EnqueueSend(new GameEventChannelBroadcast(recipient.Session, groupChatType, "", message));
                            }
                    }
                    break;
                case Channel.Advocate1:
                case Channel.Advocate2:
                case Channel.Advocate3:
                    {
                        if (session.AccessLevel < AccessLevel.Advocate)
                        {
                            var statusMessage = new GameEventWeenieError(session, WeenieError.YouCantUseThatChannel);
                            session.Network.EnqueueSend(statusMessage);
                            break;
                        }

                        foreach (var recipient in PlayerManager.GetAllOnline())
                            if (recipient.Session != session)
                            {
                                if (recipient.Session.AccessLevel >= AccessLevel.Advocate && !recipient.Squelches.Contains(session.Player))
                                    recipient.Session.Network.EnqueueSend(new GameEventChannelBroadcast(recipient.Session, groupChatType, session.Player.Name, message));
                            }
                            else
                            {
                                recipient.Session.Network.EnqueueSend(new GameEventChannelBroadcast(recipient.Session, groupChatType, "", message));
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

                        foreach (var recipient in PlayerManager.GetAllOnline())
                            if (recipient.Session != session)
                            {
                                if (recipient.Session.AccessLevel >= AccessLevel.Sentinel && !recipient.Squelches.Contains(session.Player))
                                    recipient.Session.Network.EnqueueSend(new GameEventChannelBroadcast(recipient.Session, groupChatType, session.Player.Name, message));
                            }
                            else
                            {
                                recipient.Session.Network.EnqueueSend(new GameEventChannelBroadcast(recipient.Session, groupChatType, "", message));
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
                        foreach (var recipient in PlayerManager.GetAllOnline())
                            if (recipient.Session != session && !recipient.Squelches.Contains(session.Player))
                                recipient.Session.Network.EnqueueSend(gameMessageSystemChat);

                        // again not sure what way to go with this.. the code below was added after I realized I should be handling things differently
                        // and by handling differently I mean letting the client do all of the work it was already designed to do.

                        // foreach (var recipient in WorldManager.GetAll())
                        //    if (recipient != session)
                        //        NetworkManager.SendWorldMessage(recipient, new GameEventChannelBroadcast(recipient, groupChatType, session.Player.Name, message));
                        //    else
                        //        NetworkManager.SendWorldMessage(recipient, new GameEventChannelBroadcast(recipient, groupChatType, "", message));
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
                            if (fellowmember.Session != session && !fellowmember.Squelches.Contains(session.Player))
                                fellowmember.Session.Network.EnqueueSend(new GameEventChannelBroadcast(fellowmember.Session, groupChatType, session.Player.Name, message));
                            else
                                session.Network.EnqueueSend(new GameEventChannelBroadcast(session, groupChatType, "", message));
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
                            var vassalPlayer = PlayerManager.GetOnlinePlayer(vassal.PlayerGuid);

                            if (vassalPlayer != null && !vassalPlayer.Squelches.Contains(session.Player))
                                vassalPlayer.Session.Network.EnqueueSend(new GameEventChannelBroadcast(vassalPlayer.Session, groupChatType, session.Player.Name, message));
                        }
                        session.Network.EnqueueSend(new GameEventChannelBroadcast(session, groupChatType, "", message));
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

                        if (!session.Player.PatronId.HasValue)
                        {
                            var statusMessage = new GameEventWeenieError(session, WeenieError.YouCantUseThatChannel);
                            session.Network.EnqueueSend(statusMessage);
                            break;
                        }

                        var patronPlayer = PlayerManager.GetOnlinePlayer(session.Player.AllegianceNode.Patron.PlayerGuid);

                        if (patronPlayer != null && !patronPlayer.Squelches.Contains(session.Player))
                            patronPlayer.Session.Network.EnqueueSend(new GameEventChannelBroadcast(patronPlayer.Session, groupChatType, session.Player.Name, message));

                        session.Network.EnqueueSend(new GameEventChannelBroadcast(session, groupChatType, "", message));
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

                        if (!session.Player.MonarchId.HasValue)
                        {
                            var statusMessage = new GameEventWeenieError(session, WeenieError.YouCantUseThatChannel);
                            session.Network.EnqueueSend(statusMessage);
                            break;
                        }

                        var monarchPlayer = PlayerManager.GetOnlinePlayer(session.Player.AllegianceNode.Monarch.PlayerGuid);

                        if (monarchPlayer != null && !monarchPlayer.Squelches.Contains(session.Player))
                            monarchPlayer.Session.Network.EnqueueSend(new GameEventChannelBroadcast(monarchPlayer.Session, Channel.Monarch, session.Player.Name, message));

                        session.Network.EnqueueSend(new GameEventChannelBroadcast(session, groupChatType, "", message));
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

                        if (!session.Player.PatronId.HasValue)
                        {
                            var statusMessage = new GameEventWeenieError(session, WeenieError.YouCantUseThatChannel);
                            session.Network.EnqueueSend(statusMessage);
                            break;
                        }

                        var patronPlayer = PlayerManager.GetOnlinePlayer(session.Player.AllegianceNode.Patron.PlayerGuid);

                        if (patronPlayer != null && !patronPlayer.Squelches.Contains(session.Player))
                            patronPlayer.Session.Network.EnqueueSend(new GameEventChannelBroadcast(patronPlayer.Session, Channel.Patron, session.Player.Name, message));

                        foreach (var covassal in session.Player.AllegianceNode.Patron.Vassals)
                        {
                            if (covassal.PlayerGuid.Full == session.Player.Guid.Full)
                            {
                                session.Network.EnqueueSend(new GameEventChannelBroadcast(session, groupChatType, "", message));
                            }
                            else
                            {
                                var covassalPlayer = PlayerManager.GetOnlinePlayer(covassal.PlayerGuid);

                                if (covassalPlayer != null && !covassalPlayer.Squelches.Contains(session.Player))
                                    covassalPlayer.Session.Network.EnqueueSend(new GameEventChannelBroadcast(covassalPlayer.Session, groupChatType, session.Player.Name, message));
                            }
                        }
                    }
                    break;
                case Channel.AllegianceBroadcast:
                    {
                        // The client knows if we're in an allegiance or not, and will throw an error to the user if they try to /ab, and no message will be dispatched to the server.
                        // Check anyway
                        var player = session.Player;
                        if (player.Allegiance == null)
                        {
                            session.Network.EnqueueSend(new GameEventWeenieError(session, WeenieError.YouAreNotInAllegiance));
                            break;
                        }

                        if (player.AllegiancePermissionLevel < AllegiancePermissionLevel.Speaker)
                        {
                            session.Network.EnqueueSend(new GameEventWeenieError(session, WeenieError.YouDoNotHaveAuthorityInAllegiance));
                            break;
                        }

                        // iterate through all allegiance members
                        foreach (var member in player.Allegiance.Members.Keys)
                        {
                            // is this allegiance member online?
                            var online = PlayerManager.GetOnlinePlayer(member);
                            if (online == null || online.Squelches.Contains(session.Player))
                                continue;

                            online.Session.Network.EnqueueSend(new GameEventChannelBroadcast(online.Session, groupChatType, session.Player.Name, message));
                        }
                    }
                    break;
                default:
                    Console.WriteLine($"Unhandled ChatChannel GroupChatType: 0x{(uint)groupChatType:X4}");
                    break;
            }
        }
    }
}
