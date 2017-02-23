using System;

using ACE.Common.Extensions;
using ACE.Entity.Enum;
using ACE.Network.GameEvent.Events;
using ACE.Network.GameMessages;
using ACE.Network.Managers;

namespace ACE.Network.GameAction.Actions
{
    [GameAction(GameActionOpcode.ChatChannel)]
    public class GameActionChatChannel : GameActionPacket
    {
        public GameActionChatChannel(Session session, ClientPacketFragment fragment) : base(session, fragment) { }

        private GroupChatType groupChatType;
        private string message;

        public override void Read()
        {
            groupChatType = (GroupChatType)Fragment.Payload.ReadUInt32();
            message = Fragment.Payload.ReadString16L();
        }

        public override void Handle()
        {
            switch (groupChatType)
            {
                case GroupChatType.TellFellowship:
                    {
                        var statusMessage = new GameEventDisplayStatusMessage(Session, StatusMessageType1.YouDoNotBelongToAFellowship);
                        NetworkManager.SendWorldMessages(Session, new GameMessage[] { statusMessage });

                        ChatPacket.SendServerMessage(Session, "GameActionChatChannel TellFellowship Needs work.", ChatMessageType.Broadcast);
                    }
                    break;

                case GroupChatType.TellVassals:
                    {
                        ChatPacket.SendServerMessage(Session, "GameActionChatChannel TellVassals Needs work.", ChatMessageType.Broadcast);
                    }
                    break;

                case GroupChatType.TellPatron:
                    {
                        ChatPacket.SendServerMessage(Session, "GameActionChatChannel TellPatron Needs work.", ChatMessageType.Broadcast);
                    }
                    break;

                case GroupChatType.TellMonarch:
                    {
                        ChatPacket.SendServerMessage(Session, "GameActionChatChannel TellMonarch Needs work.", ChatMessageType.Broadcast);
                    }
                    break;

                case GroupChatType.TellCoVassals:
                    {
                        ChatPacket.SendServerMessage(Session, "GameActionChatChannel TellCoVassals Needs work.", ChatMessageType.Broadcast);
                    }
                    break;

                case GroupChatType.AllegianceBroadcast:
                    {
                        // The client knows if we're in an allegiance or not, and will throw an error to the user if they try to /a, and no message will be dispatched to the server.

                        ChatPacket.SendServerMessage(Session, "GameActionChatChannel AllegianceBroadcast Needs work.", ChatMessageType.Broadcast);
                    }
                    break;

                default:
                    Console.WriteLine($"Unhandled ChatChannel GroupChatType: 0x{(uint)groupChatType:X4}");
                    break;
            }
        }
    }
}
