
using ACE.Command;
using ACE.Common.Extensions;
using ACE.Entity.Enum;
using ACE.Managers;
using ACE.Network.GameMessages;
using ACE.Network.GameMessages.Messages;
using ACE.Network.Managers;

namespace ACE.Network.GameAction.Actions
{
    [GameAction(GameActionOpcode.Talk)]
    public class GameActionTalk : GameActionPacket
    {
        public GameActionTalk(Session session, ClientPacketFragment fragment) : base(session, fragment) { }

        private string message;

        public override void Read()
        {
            message = Fragment.Payload.ReadString16L();
        }

        public override void Handle()
        {
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
                            ChatPacket.SendServerMessage(Session, $"Invalid command {command}!", ChatMessageType.Broadcast);
                            break;
                        case CommandHandlerResponse.InvalidParameterCount:
                            ChatPacket.SendServerMessage(Session, $"Invalid parameter count, got {parameters.Length}, expected {commandHandler.Attribute.ParameterCount}!", ChatMessageType.Broadcast);
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                var creatureMessage = new GameMessageCreatureMessage(message, Session.Player.Name, Session.Player.Guid.Full, ChatMessageType.PublicChat);

                // TODO: This needs to be changed to a different method. GetByRadius or GetNear, however we decide to do proximity updates...
                var targets = WorldManager.GetAll();
                
                foreach (var target in targets)
                    target.WorldSession.Enqueue(new GameMessage[] { creatureMessage });
            }
        }
    }
}