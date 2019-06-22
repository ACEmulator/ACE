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

            if (messageHandlers.TryGetValue(opcode, out var messageHandlerInfo))
            {
                if (messageHandlerInfo.Attribute.State == session.State)
                {
                    NetworkManager.InboundMessageQueue.EnqueueAction(new ActionEventDelegate(() =>
                    {
                        // It's possible that before this work is executed by WorldManager, and after it was enqueued here, the session.Player was set to null
                        // To avoid null reference exceptions, we make sure that the player is valid before the message handler is invoked.
                        if (messageHandlerInfo.Attribute.State == Enum.SessionState.WorldConnected && session.Player == null)
                            return;

                        try
                        {
                            messageHandlerInfo.Handler.Invoke(message, session);
                        }
                        catch (Exception ex)
                        {
                            log.Error($"Received GameMessage packet that threw an exception from account: {session.AccountId}:{session.Account}, player: {session.Player?.Name}, opcode: 0x{((int)opcode):X4}:{opcode}");
                            log.Error(ex);
                        }
                    }));
                }
            }
            else
            {
                log.Warn($"Received unhandled fragment opcode: 0x{(int)opcode:X4} - {opcode}");
            }
        }

        /// <summary>
        /// The call path for this function is as follows:
        /// InboundMessageManager.HandleClientMessage() queues work into NetworkManager.InboundMessageQueue that is run in WorldManager.UpdateWorld()
        /// That work invokes GameActionPacket.HandleGameAction() which calls this.
        /// </summary>
        public static void HandleGameAction(GameActionType opcode, ClientMessage message, Session session)
        {
            if (actionHandlers.TryGetValue(opcode, out var actionHandlerInfo))
            {
                // It's possible that before this work is executed by WorldManager, and after it was enqueued here, the session.Player was set to null
                // To avoid null reference exceptions, we make sure that the player is valid before the message handler is invoked.
                if (session.Player == null)
                    return;

                try
                {
                    actionHandlerInfo.Handler.Invoke(message, session);
                }
                catch (Exception ex)
                {
                    log.Error($"Received GameAction packet that threw an exception from account: {session.AccountId}:{session.Account}, player: {session.Player?.Name}, opcode: 0x{((int)opcode):X4}:{opcode}");
                    log.Error(ex);
                }
            }
            else
            {
                log.Warn($"Received unhandled GameActionType: 0x{(int)opcode:X4} - {opcode}");
            }
        }
    }
}
