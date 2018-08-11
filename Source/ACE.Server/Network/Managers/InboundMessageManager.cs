using System;
using System.Collections.Generic;
using System.Reflection;

using log4net;

using ACE.Server.Entity.Actions;
using ACE.Server.Managers;
using ACE.Server.Network.GameAction;
using ACE.Server.Network.GameMessages;

namespace ACE.Server.Network.Managers
{
    public static class InboundMessageManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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

        public static void Initialize()
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
                log.WarnFormat("Received unhandled fragment opcode: 0x{0:X4}", ((uint)opcode));
            else
            {
                if (messageHandlers.TryGetValue(opcode, out var messageHandlerInfo))
                {
                    if (messageHandlerInfo.Attribute.State == session.State)
                    {
                        WorldManager.InboundMessageQueue.EnqueueAction(new ActionEventDelegate(() =>
                        {
                            messageHandlerInfo.Handler.Invoke(message, session);
                        }));
                    }
                }
            }
        }

        public static void HandleGameAction(GameActionType opcode, ClientMessage message, Session session)
        {
            if (!actionHandlers.ContainsKey(opcode))
                log.WarnFormat("Received unhandled GameActionType: 0x{0:X4}", ((uint)opcode));
            else
            {
                if (actionHandlers.TryGetValue(opcode, out var actionHandlerInfo))
                {
                    WorldManager.InboundMessageQueue.EnqueueAction(new ActionEventDelegate(() =>
                    {
                        actionHandlerInfo.Handler.Invoke(message, session);
                    }));
                }
            }
        }
    }
}
