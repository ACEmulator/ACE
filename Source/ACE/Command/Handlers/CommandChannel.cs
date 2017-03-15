using System;
using ACE.Network;
using ACE.Entity.Enum;

namespace ACE.Command.Handlers
{
    /// <summary>
    /// Code to encapsulate sending messages to the console or to the player chat window
    /// </summary>
    public class CommandChannel
    {
        public static ClientCommandChannel TheClientChannel = new ClientCommandChannel();
        public static ServerCommandChannel TheServerChannel = new ServerCommandChannel();
        public class ClientCommandChannel : ICommandChannel
        {
            public void SendMsg(Session session, string msg)
            {
                ChatPacket.SendServerMessage(session, msg, ChatMessageType.Broadcast, flush: true);
            }
        }
        public class ServerCommandChannel : ICommandChannel
        {
            public void SendMsg(Session session, string msg)
            {
                Console.WriteLine(msg);
            }
        }
    }
    public interface ICommandChannel
    {
        void SendMsg(Session session, string msg);

    }
}
