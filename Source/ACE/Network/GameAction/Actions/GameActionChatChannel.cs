using System;

using ACE.Common.Extensions;
using ACE.Entity.Enum;
using ACE.Managers;
using ACE.Network.GameEvent.Events;
using ACE.Network.GameMessages;
using ACE.Network.Managers;

namespace ACE.Network.GameAction.Actions
{
    [GameAction(GameActionType.ChatChannel)]
    public class GameActionChatChannel : GameActionPacket
    {
        public GameActionChatChannel(Session session, ClientPacketFragment fragment) : base(session, fragment) { }

        private GroupChatType groupChatType;
        private string message;

        public override void Read()
        {
            groupChatType = (GroupChatType)Fragment.Payload.ReadUInt32();
            message = Fragment.Payload.ReadString16L();
        }

        public override void Handle()
        {
            switch (groupChatType)
            {
                case GroupChatType.TellAbuse:
                    {
                        //TODO: Proper permissions check. This command should work for any character with AccessLevel.Advocate or higher
                        //if (!Session.Player.IsAdmin)
                        //    break;

                        // TODO This should check if the recipient is subscribed to the channel
                        foreach (var recipient in WorldManager.GetAll())
                            if (recipient != Session)
                                recipient.Network.EnqueueSend(new GameEvent.Events.GameEventChannelBroadcast(recipient, groupChatType, Session.Player.Name, message));
                            else
                                recipient.Network.EnqueueSend(new GameEvent.Events.GameEventChannelBroadcast(recipient, groupChatType, "", message));
                    }
                    break;
                case GroupChatType.TellAdmin:
                    {
                        if (!Session.Player.IsAdmin)
                            break;

                        // TODO This should check if the recipient is subscribed to the channel
                        foreach (var recipient in WorldManager.GetAll())
                            if (recipient != Session)
                                recipient.Network.EnqueueSend(new GameEvent.Events.GameEventChannelBroadcast(recipient, groupChatType, Session.Player.Name, message));
                            else
                                recipient.Network.EnqueueSend(new GameEvent.Events.GameEventChannelBroadcast(recipient, groupChatType, "", message));
                        //NetworkManager.SendWorldMessage(recipient, gameMessageSystemChat);
                    }
                    break;
                case GroupChatType.TellAudit:
                    {
                        //TODO: Proper permissions check. This command should work for any character AccessLevel.Sentinel or higher
                        //if (!Session.Player.IsAdmin)
                        //    break;

                        // TODO This should check if the recipient is subscribed to the channel
                        foreach (var recipient in WorldManager.GetAll())
                            if (recipient != Session)
                                recipient.Network.EnqueueSend(new GameEvent.Events.GameEventChannelBroadcast(recipient, groupChatType, Session.Player.Name, message));
                            else
                                recipient.Network.EnqueueSend(new GameEvent.Events.GameEventChannelBroadcast(recipient, groupChatType, "", message));
                    }
                    break;
                case GroupChatType.TellAdvocate:
                    {
                        //TODO: Proper permissions check. This command should work for any character AccessLevel.Advocate or higher
                        //if (!Session.Player.IsAdmin)
                        //    break;

                        // TODO This should check if the recipient is subscribed to the channel
                        foreach (var recipient in WorldManager.GetAll())
                            if (recipient != Session)
                                recipient.Network.EnqueueSend(new GameEvent.Events.GameEventChannelBroadcast(recipient, groupChatType, Session.Player.Name, message));
                            else
                                recipient.Network.EnqueueSend(new GameEvent.Events.GameEventChannelBroadcast(recipient, groupChatType, "", message));
                    }
                    break;
                case GroupChatType.TellAdvocate2:
                    {
                        //TODO: Proper permissions check. This command should work for any character AccessLevel.Advocate or higher
                        //if (!Session.Player.IsAdmin)
                        //    break;

                        // TODO This should check if the recipient is subscribed to the channel
                        foreach (var recipient in WorldManager.GetAll())
                            if (recipient != Session)
                                recipient.Network.EnqueueSend(new GameEvent.Events.GameEventChannelBroadcast(recipient, groupChatType, Session.Player.Name, message));
                            else
                                recipient.Network.EnqueueSend(new GameEvent.Events.GameEventChannelBroadcast(recipient, groupChatType, "", message));
                    }
                    break;
                case GroupChatType.TellAdvocate3:
                    {
                        //TODO: Proper permissions check. This command should work for any character AccessLevel.Advocate or higher
                        //if (!Session.Player.IsAdmin)
                        //    break;

                        // TODO This should check if the recipient is subscribed to the channel
                        foreach (var recipient in WorldManager.GetAll())
                            if (recipient != Session)
                                recipient.Network.EnqueueSend(new GameEvent.Events.GameEventChannelBroadcast(recipient, groupChatType, Session.Player.Name, message));
                            else
                                recipient.Network.EnqueueSend(new GameEvent.Events.GameEventChannelBroadcast(recipient, groupChatType, "", message));
                    }
                    break;
                case GroupChatType.TellSentinel:
                    {
                        //TODO: Proper permissions check. This command should work for any character with AccessLevel.Sentinel or higher
                        //if (!Session.Player.IsAdmin)
                        //    break;

                        // TODO This should check if the recipient is subscribed to the channel
                        foreach (var recipient in WorldManager.GetAll())
                            if (recipient != Session)
                                recipient.Network.EnqueueSend(new GameEvent.Events.GameEventChannelBroadcast(recipient, groupChatType, Session.Player.Name, message));
                            else
                                recipient.Network.EnqueueSend(new GameEvent.Events.GameEventChannelBroadcast(recipient, groupChatType, "", message));
                    }
                    break;
                case GroupChatType.TellHelp:
                    {
                        ChatPacket.SendServerMessage(Session, "GameActionChatChannel TellHelp Needs work.", ChatMessageType.Broadcast);
                        //TODO: I don't remember exactly how this was triggered. I don't think it sent back a "You say" message to the person who triggered it
                        //TODO: Proper permissions check. Requesting urgent help should work for any character but only displays the "says" mesage for those subscribed to the Help channel
                        //      which would be Advocates and above.
                        //if (!Session.Player.IsAdmin)
                        //    break;
                        string onTheWhatChannel = "on the " + System.Enum.GetName(typeof(GroupChatType), groupChatType).Replace("Tell", "") + " channel";
                        string whoSays = Session.Player.Name + " says ";

                        //ChatPacket.SendServerMessage(Session, $"You say {onTheWhatChannel}, \"{message}\"", ChatMessageType.OutgoingHelpSay);

                        var gameMessageSystemChat = new GameMessages.Messages.GameMessageSystemChat(whoSays + onTheWhatChannel + ", \"" + message + "\"", ChatMessageType.Help);

                        // TODO This should check if the recipient is subscribed to the channel
                        foreach (var recipient in WorldManager.GetAll())
                            if (recipient != Session)
                                recipient.Network.EnqueueSend(gameMessageSystemChat);

                        // again not sure what way to go with this.. the code below was added after I realized I should be handling things differently
                        // and by handling differently I mean letting the client do all of the work it was already designed to do.
                        
                        //foreach (var recipient in WorldManager.GetAll())
                        //    if (recipient != Session)
                        //        NetworkManager.SendWorldMessage(recipient, new GameEvent.Events.GameEventChannelBroadcast(recipient, groupChatType, Session.Player.Name, message));
                        //    else
                        //        NetworkManager.SendWorldMessage(recipient, new GameEvent.Events.GameEventChannelBroadcast(recipient, groupChatType, "", message));
                    }
                    break;


                case GroupChatType.TellFellowship:
                    {
                        var statusMessage = new GameEventDisplayStatusMessage(Session, StatusMessageType1.YouDoNotBelongToAFellowship);
                        Session.Network.EnqueueSend(statusMessage);

                        ChatPacket.SendServerMessage(Session, "GameActionChatChannel TellFellowship Needs work.", ChatMessageType.Broadcast);
                    }
                    break;

                case GroupChatType.TellVassals:
                    {
                        ChatPacket.SendServerMessage(Session, "GameActionChatChannel TellVassals Needs work.", ChatMessageType.Broadcast);
                    }
                    break;

                case GroupChatType.TellPatron:
                    {
                        ChatPacket.SendServerMessage(Session, "GameActionChatChannel TellPatron Needs work.", ChatMessageType.Broadcast);
                    }
                    break;

                case GroupChatType.TellMonarch:
                    {
                        ChatPacket.SendServerMessage(Session, "GameActionChatChannel TellMonarch Needs work.", ChatMessageType.Broadcast);
                    }
                    break;

                case GroupChatType.TellCoVassals:
                    {
                        ChatPacket.SendServerMessage(Session, "GameActionChatChannel TellCoVassals Needs work.", ChatMessageType.Broadcast);
                    }
                    break;

                case GroupChatType.AllegianceBroadcast:
                    {
                        // The client knows if we're in an allegiance or not, and will throw an error to the user if they try to /a, and no message will be dispatched to the server.

                        ChatPacket.SendServerMessage(Session, "GameActionChatChannel AllegianceBroadcast Needs work.", ChatMessageType.Broadcast);
                    }
                    break;

                default:
                    Console.WriteLine($"Unhandled ChatChannel GroupChatType: 0x{(uint)groupChatType:X4}");
                    break;
            }
        }
    }
}
