using System;
using System.Text;

using ACE.Entity.Enum;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessageTurbineChat : GameMessage
    {
        public GameMessageTurbineChat(TurbineChatType turbineChatType, uint channel, string senderName, string message, uint senderID)
            : base(GameMessageOpcode.TurbineChat, GameMessageGroup.LoginQueue)
        {
            if (turbineChatType == TurbineChatType.InboundMessage)
            {
                var firstSizePos = Writer.BaseStream.Position;
                Writer.Write(0u); // Bytes to follow
                Writer.Write((uint)turbineChatType);
                Writer.Write(1u);
                Writer.Write(1u);
                Writer.Write(0x000B00B5); // Unique ID? Both ID's always match. These numbers change between 0x000B0000 - 0x000B00FF I think.
                Writer.Write(1u);
                Writer.Write(0x000B00B5); // Unique ID? Both ID's always match These numbers change between 0x000B0000 - 0x000B00FF I think.
                Writer.Write(0u);
                var secondSizePos = Writer.BaseStream.Position;
                Writer.Write(0u); // Bytes to follow

                Writer.Write(channel);

                Writer.Write((byte)senderName.Length);
                Writer.Write(Encoding.Unicode.GetBytes(senderName));

                Writer.Write((byte)message.Length);
                Writer.Write(Encoding.Unicode.GetBytes(message));

                Writer.Write(0x0Cu);
                Writer.Write(senderID);
                Writer.Write(0u);
                Writer.Write(1u);

                Writer.WritePosition((uint)(Writer.BaseStream.Position - firstSizePos + 4), firstSizePos);
                Writer.WritePosition((uint)(Writer.BaseStream.Position - secondSizePos + 4), secondSizePos);
            }
            else
                Console.WriteLine($"Unhandled GameMessageTurbineChat TurbineChatType: 0x{(uint)turbineChatType:X4}");
        }
    }
}
