using System;
using System.Text;
using ACE.Entity.Enum;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessageTurbineChat : GameMessage
    {
        public GameMessageTurbineChat(ChatNetworkBlobType chatNetworkBlobType, ChatNetworkBlobDispatchType chatNetworkBlobDispatchType, uint channel, string senderName, string message, uint senderID, ChatType chatType)
            : base(GameMessageOpcode.TurbineChat, GameMessageGroup.LoginQueue)
        {
            /*uint messageSize;       // the number of bytes that follow after this DWORD
            ChatNetworkBlobType type;   // the type of data contained in this message
            uint blobDispatchType;  // 1?
            int targetType;         // 1?
            int targetID;           // Unique ID? Both ID's always match. These numbers change between 0x000B0000 - 0x000B00FF I think.
            int transportType;      // 1?
            int transportID;        // Unique ID? Both ID's always match. These numbers change between 0x000B0000 - 0x000B00FF I think.
            int cookie;             // 0?
            uint payloadSize;       // the number of bytes that follow after this DWORD

            // Select one section based on the value of ChatNetworkBlobType

            // 0x1 - ChatNetworkBlobType.NETBLOB_EVENT_BINARY

                // Select one section based on the value of blobDispatchType

                // 0x1 - SendToRoomChatEvent
                    uint roomID;            // the channel number of the message
                    string displayName;     // the name of the player sending the message
                    string text;            // the message text
                    uint extraDataSize;     // the number of bytes that follow after this DWORD
                    uint speakerGuid;       // the object ID of the player sending the message
                    int hResult;
                    uint chatType;

            // 0x3 - ChatNetworkBlobType.NETBLOB_REQUEST_BINARY

                // Select one section based on the value of blobDispatchType

                // 0x1 - SendToRoomByNameRequest
                    uint contextID;
                    uint responseID = 1;
                    uint methodID = 1;
                    string roomName;        // wstring - the channel name of the message
                    string text;            // wstring - the message text
                    uint extraDataSize;     // the number of bytes that follow after this DWORD
                    uint speakerGuid;       // the object ID of the player sending the message (should be you)
                    int hResult;
                    uint chatType;

                // 0x2 - SendToRoomByIDRequest, this is the only one used by the client I believe
                    uint contextID;
                    uint responseID = 2;
                    uint methodID = 2;
                    uint roomID;            // the channel number of the message
                    string text;            // the message text
                    uint extraDataSize;     // the numbe rof bytes that follow after this DWORD
                    uint speakerID;         // the object ID of the player sending the message (should be you)
                    int hResult;
                    uint chatType;

            // 0x5 - ChatNetworkBlobType.NETBLOB_RESPONSE_BINARY - inbound acknowledgement to client of outbound message

                // Select one section based on the value of blobDispatchType

                // 0x1 - SendToRoomByNameRequest
                    uint contextID;
                    uint responseID = 1;    // type of response, should be 1 here
                    uint methodID = 1;      // type of request, should be 1 here
                    int hResult;

                // 0x2 - SendToRoomByIDResponse
                    uint contextID;
                    uint responseID = 2;    // type of response, should be 2 here
                    uint methodID = 2;      // type of request, should be 2 here
                    int hResult;*/

            if (chatNetworkBlobType == ChatNetworkBlobType.NETBLOB_EVENT_BINARY)
            {
                var firstSizePos = Writer.BaseStream.Position;
                Writer.Write(0u); // Bytes to follow
                Writer.Write((uint)chatNetworkBlobType);
                Writer.Write((uint)chatNetworkBlobDispatchType);
                Writer.Write(1u);
                Writer.Write(0x000B00B5); // Unique ID? Both ID's always match. These numbers change between 0x000B0000 - 0x000B00FF I think.
                Writer.Write(1u);
                Writer.Write(0x000B00B5); // Unique ID? Both ID's always match. These numbers change between 0x000B0000 - 0x000B00FF I think.
                Writer.Write(0u);
                var secondSizePos = Writer.BaseStream.Position;
                Writer.Write(0u); // Bytes to follow

                Writer.Write(channel);

                if (senderName.Length < 128)
                    Writer.Write((byte)senderName.Length);
                else
                    Writer.Write((ushort)(0x80 | (message.Length << 8)));
                Writer.Write(Encoding.Unicode.GetBytes(senderName));

                if (message.Length < 128)
                    Writer.Write((byte)message.Length);
                else
                    Writer.Write((ushort)(0x80 | (message.Length << 8)));
                Writer.Write(Encoding.Unicode.GetBytes(message));

                Writer.Write(0x0Cu);
                Writer.Write(senderID);
                Writer.Write(0u);
                Writer.Write((uint)chatType);

                Writer.WritePosition((uint)(Writer.BaseStream.Position - firstSizePos + 4), firstSizePos);
                Writer.WritePosition((uint)(Writer.BaseStream.Position - secondSizePos + 4), secondSizePos);
            }
            else if (chatNetworkBlobType == ChatNetworkBlobType.NETBLOB_RESPONSE_BINARY)
            {
                var firstSizePos = Writer.BaseStream.Position;
                Writer.Write(0u); // Bytes to follow
                Writer.Write((uint)chatNetworkBlobType);
                Writer.Write((uint)chatNetworkBlobDispatchType);
                Writer.Write(1u);
                Writer.Write(0x000B00B5); // Unique ID? Both ID's always match. These numbers change between 0x000B0000 - 0x000B00FF I think.
                Writer.Write(1u);
                Writer.Write(0x000B00B5); // Unique ID? Both ID's always match. These numbers change between 0x000B0000 - 0x000B00FF I think.
                Writer.Write(0u);
                var secondSizePos = Writer.BaseStream.Position;
                Writer.Write(0u); // Bytes to follow

                Writer.Write(channel);
                Writer.Write(2u);
                Writer.Write(2u);
                Writer.Write(0u);

                Writer.WritePosition((uint)(Writer.BaseStream.Position - firstSizePos + 4), firstSizePos);
                Writer.WritePosition((uint)(Writer.BaseStream.Position - secondSizePos + 4), secondSizePos);
            }
            else
                Console.WriteLine($"Unhandled GameMessageTurbineChat ChatNetworkBlobType: 0x{(uint)chatNetworkBlobType:X4}");
        }
    }
}
