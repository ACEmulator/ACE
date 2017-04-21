using ACE.Command;
using ACE.Common.Extensions;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Managers;
using ACE.Network;
using ACE.Network.GameAction;
using ACE.Network.GameEvent.Events;
using ACE.Network.GameMessages;
using ACE.Network.GameMessages.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity
{
    public partial class Player
    {
        [GameAction(GameActionType.Talk)]
        private void TalkAction(ClientMessage clientMessage)
        {
            var message = clientMessage.Payload.ReadString16L();
            if (message.StartsWith("@"))
            {
                string command;
                string[] parameters;
                CommandManager.ParseCommand(message.Remove(0, 1), out command, out parameters);

                CommandHandlerInfo commandHandler;
                var response = CommandManager.GetCommandHandler(Session, command, parameters, out commandHandler);
                if (response == CommandHandlerResponse.Ok)
                    ((CommandHandler)commandHandler.Handler).Invoke(Session, parameters);
                else if (response == CommandHandlerResponse.SudoOk)
                {
                    string[] sudoParameters = new string[parameters.Length - 1];
                    for (int i = 1; i < parameters.Length; i++)
                        sudoParameters[i - 1] = parameters[i];

                    ((CommandHandler)commandHandler.Handler).Invoke(Session, sudoParameters);
                }
                else
                {
                    switch (response)
                    {
                        case CommandHandlerResponse.InvalidCommand:
                            Session.EnqueueSend(new GameMessageSystemChat($"Unknown command: {command}", ChatMessageType.Help));
                            break;
                        case CommandHandlerResponse.InvalidParameterCount:
                            Session.EnqueueSend(new GameMessageSystemChat($"Invalid parameter count, got {parameters.Length}, expected {commandHandler.Attribute.ParameterCount}!", ChatMessageType.Help));
                            Session.EnqueueSend(new GameMessageSystemChat($"@{commandHandler.Attribute.Command} - {commandHandler.Attribute.Description}", ChatMessageType.Broadcast));
                            Session.EnqueueSend(new GameMessageSystemChat($"Usage: @{commandHandler.Attribute.Command} {commandHandler.Attribute.Usage}", ChatMessageType.Broadcast));
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                var creatureMessage = new GameMessageCreatureMessage(message, Name, Guid.Full, ChatMessageType.Speech);

                // TODO: This needs to be changed to a different method. GetByRadius or GetNear, however we decide to do proximity updates...
                var targets = WorldManager.GetAll();

                foreach (var target in targets)
                    target.EnqueueSend(new GameMessage[] { creatureMessage });
            }
        }

        [GameAction(GameActionType.Tell)]
        private void TellAction(ClientMessage clientMessage)
        {
            var message = clientMessage.Payload.ReadString16L(); // The client seems to do the trimming for us
            var target = clientMessage.Payload.ReadString16L(); // Needs to be trimmed because it may contain white spaces after the name and before the ,
            target = target.Trim();
            var targetsession = WorldManager.FindByPlayerName(target);

            if (targetsession == null)
            {
                var statusMessage = new GameEventDisplayStatusMessage(Session, StatusMessageType1.CharacterNotAvailable);
                Session.EnqueueSend(statusMessage);
            }
            else
            {
                if (this != targetsession.Player)
                    Session.EnqueueSend(new GameMessageSystemChat($"You tell {target}, \"{message}\"", ChatMessageType.OutgoingTell));

                var tell = new GameEventTell(targetsession, message, Name, Guid.Full, targetsession.Player.Guid.Full, ChatMessageType.Tell);
                targetsession.EnqueueSend(tell);
            }
        }

        [GameAction(GameActionType.Emote)]
        public void EmoteAction(ClientMessage message)
        {
            var emote = message.Payload.ReadString16L();
            // TODO: send emote text to other characters
            // The emote text comes from the client ready to broadcast.
            // For example: *afk* comes as "decides to rest for a while."
        }

        [GameAction(GameActionType.ChatChannel)]
        private void ChatChannelAction(ClientMessage clientMessage)
        {
            var groupChatType = (GroupChatType)clientMessage.Payload.ReadUInt32();
            var message = clientMessage.Payload.ReadString16L();
            switch (groupChatType)
            {
                case GroupChatType.TellAbuse:
                    {
                        // TODO: Proper permissions check. This command should work for any character with AccessLevel.Advocate or higher
                        // if (!IsAdmin)
                        //    break;

                        // TODO This should check if the recipient is subscribed to the channel
                        foreach (var recipient in WorldManager.GetAll())
                            if (recipient != Session)
                                recipient.EnqueueSend(new GameEventChannelBroadcast(recipient, groupChatType, Name, message));
                            else
                                recipient.EnqueueSend(new GameEventChannelBroadcast(recipient, groupChatType, "", message));
                    }
                    break;
                case GroupChatType.TellAdmin:
                    {
                        if (!IsAdmin)
                            break;

                        // TODO This should check if the recipient is subscribed to the channel
                        foreach (var recipient in WorldManager.GetAll())
                            if (recipient != Session)
                                recipient.EnqueueSend(new GameEventChannelBroadcast(recipient, groupChatType, Name, message));
                            else
                                recipient.EnqueueSend(new GameEventChannelBroadcast(recipient, groupChatType, "", message));
                        // NetworkManager.SendWorldMessage(recipient, gameMessageSystemChat);
                    }
                    break;
                case GroupChatType.TellAudit:
                    {
                        // TODO: Proper permissions check. This command should work for any character AccessLevel.Sentinel or higher
                        // if (!IsAdmin)
                        //    break;

                        // TODO This should check if the recipient is subscribed to the channel
                        foreach (var recipient in WorldManager.GetAll())
                            if (recipient != Session)
                                recipient.EnqueueSend(new GameEventChannelBroadcast(recipient, groupChatType, Name, message));
                            else
                                recipient.EnqueueSend(new GameEventChannelBroadcast(recipient, groupChatType, "", message));
                    }
                    break;
                case GroupChatType.TellAdvocate:
                    {
                        // TODO: Proper permissions check. This command should work for any character AccessLevel.Advocate or higher
                        // if (!IsAdmin)
                        //    break;

                        // TODO This should check if the recipient is subscribed to the channel
                        foreach (var recipient in WorldManager.GetAll())
                            if (recipient != Session)
                                recipient.EnqueueSend(new GameEventChannelBroadcast(recipient, groupChatType, Name, message));
                            else
                                recipient.EnqueueSend(new GameEventChannelBroadcast(recipient, groupChatType, "", message));
                    }
                    break;
                case GroupChatType.TellAdvocate2:
                    {
                        // TODO: Proper permissions check. This command should work for any character AccessLevel.Advocate or higher
                        // if (!IsAdmin)
                        //    break;

                        // TODO This should check if the recipient is subscribed to the channel
                        foreach (var recipient in WorldManager.GetAll())
                            if (recipient != Session)
                                recipient.EnqueueSend(new GameEventChannelBroadcast(recipient, groupChatType, Name, message));
                            else
                                recipient.EnqueueSend(new GameEventChannelBroadcast(recipient, groupChatType, "", message));
                    }
                    break;
                case GroupChatType.TellAdvocate3:
                    {
                        // TODO: Proper permissions check. This command should work for any character AccessLevel.Advocate or higher
                        // if (!IsAdmin)
                        //    break;

                        // TODO This should check if the recipient is subscribed to the channel
                        foreach (var recipient in WorldManager.GetAll())
                            if (recipient != Session)
                                recipient.EnqueueSend(new GameEventChannelBroadcast(recipient, groupChatType, Name, message));
                            else
                                recipient.EnqueueSend(new GameEventChannelBroadcast(recipient, groupChatType, "", message));
                    }
                    break;
                case GroupChatType.TellSentinel:
                    {
                        // TODO: Proper permissions check. This command should work for any character with AccessLevel.Sentinel or higher
                        // if (!IsAdmin)
                        //    break;

                        // TODO This should check if the recipient is subscribed to the channel
                        foreach (var recipient in WorldManager.GetAll())
                            if (recipient != Session)
                                recipient.EnqueueSend(new GameEventChannelBroadcast(recipient, groupChatType, Name, message));
                            else
                                recipient.EnqueueSend(new GameEventChannelBroadcast(recipient, groupChatType, "", message));
                    }
                    break;
                case GroupChatType.TellHelp:
                    {
                        ChatPacket.SendServerMessage(Session, "GameActionChatChannel TellHelp Needs work.", ChatMessageType.Broadcast);
                        // TODO: I don't remember exactly how this was triggered. I don't think it sent back a "You say" message to the person who triggered it
                        // TODO: Proper permissions check. Requesting urgent help should work for any character but only displays the "says" mesage for those subscribed to the Help channel
                        //      which would be Advocates and above.
                        // if (!IsAdmin)
                        //    break;
                        string onTheWhatChannel = "on the " + System.Enum.GetName(typeof(GroupChatType), groupChatType).Replace("Tell", "") + " channel";
                        string whoSays = Name + " says ";

                        // ChatPacket.SendServerMessage(Session, $"You say {onTheWhatChannel}, \"{message}\"", ChatMessageType.OutgoingHelpSay);

                        var gameMessageSystemChat = new GameMessageSystemChat(whoSays + onTheWhatChannel + ", \"" + message + "\"", ChatMessageType.Help);

                        // TODO This should check if the recipient is subscribed to the channel
                        foreach (var recipient in WorldManager.GetAll())
                            if (recipient != Session)
                                recipient.EnqueueSend(gameMessageSystemChat);

                        // again not sure what way to go with this.. the code below was added after I realized I should be handling things differently
                        // and by handling differently I mean letting the client do all of the work it was already designed to do.

                        // foreach (var recipient in WorldManager.GetAll())
                        //    if (recipient != Session)
                        //        NetworkManager.SendWorldMessage(recipient, new GameEventChannelBroadcast(recipient, groupChatType, Name, message));
                        //    else
                        //        NetworkManager.SendWorldMessage(recipient, new GameEventChannelBroadcast(recipient, groupChatType, "", message));
                    }
                    break;

                case GroupChatType.TellFellowship:
                    {
                        var statusMessage = new GameEventDisplayStatusMessage(Session, StatusMessageType1.YouDoNotBelongToAFellowship);
                        Session.EnqueueSend(statusMessage);

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
