using System;

using log4net;

using ACE.Common.Extensions;
using ACE.Entity.Enum;
using ACE.Server.Command;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionTalk
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [GameAction(GameActionType.Talk)]
        public static void Handle(ClientMessage clientMessage, Session session)
        {
            var message = clientMessage.Payload.ReadString16L();
            
            if (message.StartsWith("@"))
            {
                string commandRaw = message.Remove(0, 1);
                CommandHandlerResponse response = CommandHandlerResponse.InvalidCommand;
                CommandHandlerInfo commandHandler = null;
                string command = null;
                string[] parameters = null;

                try
                {
                    CommandManager.ParseCommand(message.Remove(0, 1), out command, out parameters);
                }
                catch (Exception ex)
                {
                    log.Error($"Exception while parsing command: {commandRaw}", ex);
                    return;
                }

                try
                {
                    response = CommandManager.GetCommandHandler(session, command, parameters, out commandHandler);
                }
                catch (Exception ex)
                {
                    log.Error($"Exception while getting command handler for: {commandRaw}", ex);
                }

                if (response == CommandHandlerResponse.Ok)
                {
                    try
                    {
                        if (commandHandler.Attribute.IncludeRaw)
                        {
                            parameters = CommandManager.StuffRawIntoParameters(message.Remove(0, 1), command, parameters);
                        }
                        ((CommandHandler)commandHandler.Handler).Invoke(session, parameters);
                    }
                    catch (Exception ex)
                    {
                        log.Error($"Exception while invoking command handler for: {commandRaw}", ex);
                    }
                }
                else if (response == CommandHandlerResponse.SudoOk)
                {
                    string[] sudoParameters = new string[parameters.Length - 1];
                    for (int i = 1; i < parameters.Length; i++)
                        sudoParameters[i - 1] = parameters[i];
                    try
                    {
                        if (commandHandler.Attribute.IncludeRaw)
                        {
                            parameters = CommandManager.StuffRawIntoParameters(message.Remove(0, 1), command, parameters);
                        }
                        ((CommandHandler)commandHandler.Handler).Invoke(session, sudoParameters);
                    }
                    catch (Exception ex)
                    {
                        log.Error($"Exception while invoking command handler for: {commandRaw}", ex);
                    }
                }
                else
                {
                    switch (response)
                    {
                        case CommandHandlerResponse.InvalidCommand:
                            session.Network.EnqueueSend(new GameMessageSystemChat($"Unknown command: {command}", ChatMessageType.Help));
                            break;
                        case CommandHandlerResponse.InvalidParameterCount:
                            session.Network.EnqueueSend(new GameMessageSystemChat($"Invalid parameter count, got {parameters.Length}, expected {commandHandler.Attribute.ParameterCount}!", ChatMessageType.Help));
                            session.Network.EnqueueSend(new GameMessageSystemChat($"@{commandHandler.Attribute.Command} - {commandHandler.Attribute.Description}", ChatMessageType.Broadcast));
                            session.Network.EnqueueSend(new GameMessageSystemChat($"Usage: @{commandHandler.Attribute.Command} {commandHandler.Attribute.Usage}", ChatMessageType.Broadcast));
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                session.Player.HandleActionTalk(message);
            }
        }
    }
}
