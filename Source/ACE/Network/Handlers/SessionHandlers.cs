using ACE.Network.Enum;
using ACE.Network.GameAction;
using ACE.Network.GameMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Network
{
    public partial class Session
    {
        public delegate void MessageHandler(ClientMessage message);
        private static Dictionary<GameMessageOpcode, MessageHandlerMethodInfo> messageInfoHandlers;
        private class MessageHandlerMethodInfo
        {
            public MethodInfo MethodInfo { get; set; }
            public GameMessageAttribute Attribute { get; set; }
        }

        private static void DefineMessageInfoHandlers()
        {
            messageInfoHandlers = new Dictionary<GameMessageOpcode, MessageHandlerMethodInfo>();

            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                foreach (var methodInfo in typeof(Session).GetMethods())
                {
                    foreach (var messageHandlerAttribute in methodInfo.GetCustomAttributes<GameMessageAttribute>())
                    {
                        var messageHandler = new MessageHandlerMethodInfo()
                        {
                            MethodInfo = methodInfo,
                            Attribute = messageHandlerAttribute
                        };

                        messageInfoHandlers[messageHandlerAttribute.Opcode] = messageHandler;
                    }
                }
            }
        }

        static Session()
        {
            DefineMessageInfoHandlers();
        }

        private class MessageHandlerInfo
        {
            public MessageHandler Handler { get; set; }
            public GameMessageAttribute Attribute { get; set; }
        }

        private Dictionary<GameMessageOpcode, MessageHandlerInfo> messageHandlers = new Dictionary<GameMessageOpcode, MessageHandlerInfo>();

        private void DefineMessageHandlers()
        {
            foreach (MessageHandlerMethodInfo info in messageInfoHandlers.Values)
            {
                var messageHandler = new MessageHandlerInfo()
                {
                    Handler = (MessageHandler)info.MethodInfo.CreateDelegate(typeof(MessageHandler), this),
                    Attribute = info.Attribute
                };

                messageHandlers[info.Attribute.Opcode] = messageHandler;
            }
        }

        private void HandleClientMessage(ClientMessage message)
        {
            var opcode = (GameMessageOpcode)message.Opcode;
            MessageHandlerInfo messageHandlerInfo;
            if (messageHandlers.TryGetValue(opcode, out messageHandlerInfo))
            {
                if (messageHandlerInfo.Attribute.State == State)
                    messageHandlerInfo.Handler.Invoke(message);
            }
            else
            {
                log.WarnFormat("Received unhandled message opcode: 0x{0}", ((uint)opcode).ToString("X4"));
            }
        }

        [GameMessage(GameMessageOpcode.GameAction, SessionState.WorldConnected)]
        public void HandleGameAction(ClientMessage message)
        {
            // TODO: verify sequence
            uint sequence = message.Payload.ReadUInt32();
            uint opcode = message.Payload.ReadUInt32();

            Player.HandleGameAction((GameActionType)opcode, message);
        }
    }
}
