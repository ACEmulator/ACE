using ACE.Network;
using ACE.Network.Enum;
using ACE.Network.GameAction;
using ACE.Network.GameMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity.Objects
{
    public partial class Player
    {
        public delegate void ActionHandler(ClientMessage message);

        private static Dictionary<GameActionType, ActionHandlerMethodInfo> actionInfoHandlers;

        private class ActionHandlerMethodInfo
        {
            public MethodInfo MethodInfo { get; set; }
            public GameActionAttribute Attribute { get; set; }
        }

        private static void DefineActionInfoHandlers()
        {
            actionInfoHandlers = new Dictionary<GameActionType, ActionHandlerMethodInfo>();

            foreach (var methodInfo in typeof(Player).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic))
            {
                foreach (var messageHandlerAttribute in methodInfo.GetCustomAttributes<GameActionAttribute>())
                {
                    var messageHandler = new ActionHandlerMethodInfo()
                    {
                        MethodInfo = methodInfo,
                        Attribute = messageHandlerAttribute
                    };

                    actionInfoHandlers[messageHandlerAttribute.Opcode] = messageHandler;
                }
            }
        }

        static Player()
        {
            DefineActionInfoHandlers();
        }

        private class ActionHandlerInfo
        {
            public ActionHandler Handler { get; set; }
            public GameActionAttribute Attribute { get; set; }
        }

        private Dictionary<GameActionType, ActionHandlerInfo> actionHandlers = new Dictionary<GameActionType, ActionHandlerInfo>();

        private void DefineActionHandlers()
        {
            foreach (ActionHandlerMethodInfo info in actionInfoHandlers.Values)
            {
                var actionHandler = new ActionHandlerInfo()
                {
                    Handler = (ActionHandler)info.MethodInfo.CreateDelegate(typeof(ActionHandler), this),
                    Attribute = info.Attribute
                };

                actionHandlers[info.Attribute.Opcode] = actionHandler;
            }
        }

        public void HandleGameAction(GameActionType type, ClientMessage message)
        {
            ActionHandlerInfo actionHandlerInfo;
            if (actionHandlers.TryGetValue(type, out actionHandlerInfo))
            {
                actionHandlerInfo.Handler.Invoke(message);
            }
            else
            {
                log.WarnFormat("Received unhandled action type: 0x{0}", ((uint)type).ToString("X4"));
            }
        }
    }
}
