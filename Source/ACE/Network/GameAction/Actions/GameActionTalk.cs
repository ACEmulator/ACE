using ACE.Command;
using ACE.Common.Extensions;
using ACE.Entity.Enum;
using ACE.Managers;
using ACE.Network.GameMessages;
using ACE.Network.GameMessages.Messages;
using ACE.Network.Managers;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionTalk
    {
        [GameAction(GameActionType.Talk)]
        public static void Handle(ClientMessage clientMessage, Session session)
        {
            var message = clientMessage.Payload.ReadString16L();
            if (message.StartsWith("@"))
            {
                string command;
                string[] parameters;
                CommandManager.ParseCommand(message.Remove(0, 1), out command, out parameters);

                CommandHandlerInfo commandHandler;
                var response = CommandManager.GetCommandHandler(session, command, parameters, out commandHandler);
                if (response == CommandHandlerResponse.Ok)
                    ((CommandHandler)commandHandler.Handler).Invoke(session, parameters);
                else if (response == CommandHandlerResponse.SudoOk)
                {
                    string[] sudoParameters = new string[parameters.Length - 1];
                    for (int i = 1; i < parameters.Length; i++)
                        sudoParameters[i - 1] = parameters[i];

                    ((CommandHandler)commandHandler.Handler).Invoke(session, sudoParameters);
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
                var creatureMessage = new GameMessageCreatureMessage(message, session.Player.Name, session.Player.Guid.Full, ChatMessageType.Speech);

                // TODO: This needs to be changed to a different method. GetByRadius or GetNear, however we decide to do proximity updates...
                var targets = WorldManager.GetAll();

                foreach (var target in targets)
                    target.Network.EnqueueSend(new GameMessage[] { creatureMessage });
            }
        }
    }
}