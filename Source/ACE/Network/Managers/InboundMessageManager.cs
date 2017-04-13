using System;
using System.Collections.Generic;
using System.Reflection;

using ACE.Network.GameAction;
using ACE.Network.GameMessages;
using log4net;

namespace ACE.Network.Managers
{
    public static class InboundMessageManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private class ActionHandlerInfo
        {
            public ActionHandler Handler { get; set; }
            public GameActionAttribute Attribute { get; set; }
        }

        public delegate void ActionHandler(ClientMessage message, Session session);

        private static Dictionary<GameActionType, ActionHandlerInfo> actionHandlers;

        public static void Initialise()
        {
            DefineActionHandlers();
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

        public static void HandleGameAction(GameActionType opcode, ClientMessage message, Session session)
        {
            if (!actionHandlers.ContainsKey(opcode))
                log.WarnFormat("Received unhandled GameActionType: 0x{0}", ((uint)opcode).ToString("X4"));
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
