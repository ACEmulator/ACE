using ACE.Command;
using ACE.Extensions;

namespace ACE.Network.GameAction
{
    [GameAction(GameActionOpcode.Talk)]
    public class GameActionTalk : GameActionPacket
    {
        public GameActionTalk(Session session, ClientPacketFragment fragment) : base(session, fragment) { }

        private string message;

        public override void Read()
        {
            message = fragment.Payload.ReadString16L();
        }

        public override void Handle()
        {
            if (message.StartsWith("@"))
            {
                string command;
                string[] parameters;
                CommandManager.ParseCommand(message.Remove(0, 1), out command, out parameters);

                CommandHandlerInfo commandHandler;
                var response = CommandManager.GetCommandHandler(session, command, parameters, out commandHandler);
                if (response == CommandHandlerResponse.Ok)
                    ((CommandHandler)commandHandler.Handler).Invoke(session, parameters);
                else
                {
                    switch (response)
                    {
                        case CommandHandlerResponse.InvalidCommand:
                            ChatPacket.SendSystemMessage(session, $"Invalid command {command}!");
                            break;
                        case CommandHandlerResponse.InvalidParameterCount:
                            ChatPacket.SendSystemMessage(session, $"Invalid parameter count, got {parameters.Length}, expected {commandHandler.Attribute.ParameterCount}!");
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                // TODO: broadcast message
            }
        }
    }
}
