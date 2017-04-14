using System;
using System.Text;

using ACE.Entity.Enum;
using ACE.Managers;
using ACE.Network.Enum;
using ACE.Network.GameMessages;
using ACE.Network.GameMessages.Messages;
using ACE.Network.Managers;

namespace ACE.Network.Handlers
{
    public static class TurbineChatHandler
    {
        [GameMessage(GameMessageOpcode.TurbineChat, SessionState.WorldConnected)]
        public static void TurbineChatReceived(ClientMessage clientMessage, Session session)
        {
            clientMessage.Payload.ReadUInt32(); // Bytes to follow
            var turbineChatType = (TurbineChatType)clientMessage.Payload.ReadUInt32();
            clientMessage.Payload.ReadUInt32(); // Always 2
            clientMessage.Payload.ReadUInt32(); // Always 1
            clientMessage.Payload.ReadUInt32(); // Always 0
            clientMessage.Payload.ReadUInt32(); // Always 0
            clientMessage.Payload.ReadUInt32(); // Always 0
            clientMessage.Payload.ReadUInt32(); // Always 0
            clientMessage.Payload.ReadUInt32(); // Bytes to follow

            if (turbineChatType == TurbineChatType.OutboundMessage)
            {
                clientMessage.Payload.ReadUInt32(); // 0x01 - 0x71 (maybe higher), typically though 0x01 - 0x0F
                clientMessage.Payload.ReadUInt32(); // Always 2
                clientMessage.Payload.ReadUInt32(); // Always 2
                var channelID = clientMessage.Payload.ReadUInt32();

                var messageLen = clientMessage.Payload.ReadByte();
                var messageBytes = clientMessage.Payload.ReadBytes(messageLen * 2);
                var message = Encoding.Unicode.GetString(messageBytes);

                clientMessage.Payload.ReadUInt32(); // Always 0x0C
                var senderID = clientMessage.Payload.ReadUInt32();
                clientMessage.Payload.ReadUInt32(); // Always 0
                clientMessage.Payload.ReadUInt32(); // Always 1 or 2

                if (channelID == 7) // TODO this is hardcoded right now
                {
                    ChatPacket.SendServerMessage(session, "You do not belong to a society.", ChatMessageType.Broadcast); // I don't know if this is how it was done on the live servers
                    return;
                }

                var gameMessageTurbineChat = new GameMessageTurbineChat(TurbineChatType.InboundMessage, channelID, session.Player.Name, message, senderID);

                // TODO This should check if the recipient is subscribed to the channel
                foreach (var recipient in WorldManager.GetAll())
                    recipient.EnqueueSend(gameMessageTurbineChat);
            }
            else
                Console.WriteLine($"Unhandled TurbineChatHandler TurbineChatType: 0x{(uint)turbineChatType:X4}");
        }
    }
}
