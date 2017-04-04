using System;
using System.Collections.Generic;
using System.Reflection;

using ACE.Network.GameAction;
using ACE.Network.GameMessages;

namespace ACE.Network.Managers
{
    public static class InboundMessageManager
    {
        private class MessageHandlerInfo
        {
            public MessageHandler Handler { get; set; }
            public GameMessageAttribute Attribute { get; set; }
        }

        private class ActionHandlerInfo
        {
            public ActionHandler Handler { get; set; }
            public GameActionAttribute Attribute { get; set; }
        }

        public delegate void MessageHandler(ClientMessage message, Session session);

        public delegate void ActionHandler(ClientMessage message, Session session);

        private static Dictionary<GameMessageOpcode, MessageHandlerInfo> messageHandlers;

        private static Dictionary<GameActionType, ActionHandlerInfo> actionHandlers;

        public static void Initialise()
        {
            DefineMessageHandlers();
            DefineActionHandlers();
        }

        private static void DefineMessageHandlers()
        {
            messageHandlers = new Dictionary<GameMessageOpcode, MessageHandlerInfo>();

            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                foreach (var methodInfo in type.GetMethods())
                {
                    foreach (var messageHandlerAttribute in methodInfo.GetCustomAttributes<GameMessageAttribute>())
                    {
                        var messageHandler = new MessageHandlerInfo()
                        {
                            Handler   = (MessageHandler)Delegate.CreateDelegate(typeof(MessageHandler), methodInfo),
                            Attribute = messageHandlerAttribute
                        };

                        messageHandlers[messageHandlerAttribute.Opcode] = messageHandler;
                    }
                }
            }
        }

        private static void DefineActionHandlers()
        {
            actionHandlers = new Dictionary<GameActionType, ActionHandlerInfo>();

            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                foreach (var methodInfo in type.GetMethods())
                {
                    foreach (var actionHandlerAttribute in methodInfo.GetCustomAttributes<GameActionAttribute>())
                    {
                        var actionhandler = new ActionHandlerInfo()
                        {
                            Handler = (ActionHandler)Delegate.CreateDelegate(typeof(ActionHandler), methodInfo),
                            Attribute = actionHandlerAttribute
                        };

                        actionHandlers[actionHandlerAttribute.Opcode] = actionhandler;
                    }
                }
            }
        }

        public static void HandleClientMessage(ClientMessage message, Session session)
        {
            var opcode = (GameMessageOpcode)message.Opcode;

            if (!messageHandlers.ContainsKey(opcode))
                Console.WriteLine($"Received unhandled fragment opcode: 0x{(uint)opcode:X4}");
            else
            {
                MessageHandlerInfo messageHandlerInfo;
                if (messageHandlers.TryGetValue(opcode, out messageHandlerInfo))
                {
                    if (messageHandlerInfo.Attribute.State == session.State)
                        messageHandlerInfo.Handler.Invoke(message, session);
                }
            }
        }

        public static void HandleGameAction(GameActionType opcode, ClientMessage message, Session session)
        {
            if (!actionHandlers.ContainsKey(opcode))
                Console.WriteLine($"Received unhandled GameActionType: 0x{(uint)opcode:X4}");
            else
            {
                ActionHandlerInfo actionHandlerInfo;
                if (actionHandlers.TryGetValue(opcode, out actionHandlerInfo))
                {
                    actionHandlerInfo.Handler.Invoke(message, session);
                }
            }
        }
    }
}
