using System;
using System.Collections.Generic;
using System.Reflection;

using ACE.Managers;
using ACE.Network.Enum;
using ACE.Network.GameMessages;
using ACE.Network.GameMessages.Messages;
using ACE.Network.GameAction;
using ACE.Network.Handlers;

namespace ACE.Network.Managers
{
    public static class InboundMessageManager
    {
        private class MessageHandlerInfo
        {
            public FragmentHandler Handler { get; set; }
            public GameMessageAttribute Attribute { get; set; }
        }

        public delegate void FragmentHandler(ClientPacketFragment fragement, Session session);
        private static Dictionary<GameMessageOpcode, MessageHandlerInfo> fragmentHandlers;

        private static Dictionary<GameActionOpcode, Type> actionHandlers;

        public static void Initialise()
        {
            DefineMessageHandlers();
            DefineActionHandlers();
        }

        private static void DefineMessageHandlers()
        {
            fragmentHandlers = new Dictionary<GameMessageOpcode, MessageHandlerInfo>();
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                foreach (var methodInfo in type.GetMethods())
                {
                    foreach (var fragmentHandlerAttribute in methodInfo.GetCustomAttributes<GameMessageAttribute>())
                    {
                        var fragmentHandler = new MessageHandlerInfo()
                        {
                            Handler   = (FragmentHandler)Delegate.CreateDelegate(typeof(FragmentHandler), methodInfo),
                            Attribute = fragmentHandlerAttribute
                        };

                        fragmentHandlers[fragmentHandlerAttribute.Opcode] = fragmentHandler;
                    }
                }
            }
        }

        private static void DefineActionHandlers()
        {
            actionHandlers = new Dictionary<GameActionOpcode, Type>();
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
                foreach (var actionHandlerAttribute in type.GetCustomAttributes<GameActionAttribute>())
                    actionHandlers[actionHandlerAttribute.Opcode] = type;
        }

        public static void HandleClientFragment(ClientPacketFragment fragment, Session session)
        {
            var opcode = (GameMessageOpcode)fragment.Payload.ReadUInt32();
            if (!fragmentHandlers.ContainsKey(opcode))
                Console.WriteLine($"Received unhandled fragment opcode: 0x{(uint)opcode:X4}");
            else
            {
                MessageHandlerInfo fragmentHandlerInfo;
                if (fragmentHandlers.TryGetValue(opcode, out fragmentHandlerInfo))
                    if (fragmentHandlerInfo.Attribute.State == session.State)
                        fragmentHandlerInfo.Handler.Invoke(fragment, session);
            }
        }

        //TODO: This needs to be reworked. Activator.CreateInstance is not going to be performant.
        public static void HandleGameAction(GameActionOpcode opcode, ClientPacketFragment fragment, Session session)
        {
            if (!actionHandlers.ContainsKey(opcode))
                Console.WriteLine($"Received unhandled action opcode: 0x{(uint)opcode:X4}");
            else
            {
                Type actionType;
                if (actionHandlers.TryGetValue(opcode, out actionType))
                {
                    var gameAction = (GameActionPacket)Activator.CreateInstance(actionType, session, fragment);
                    gameAction.Read();
                    gameAction.Handle();
                }
            }
        }
    }
}
