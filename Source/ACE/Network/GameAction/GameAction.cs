using ACE.Network.Enum;
using ACE.Network.GameMessages;

namespace ACE.Network.GameAction
{
    public abstract class GameAction
    {
        public GameActionType Type { get; }
        public uint Sequence { get; }

        public GameAction(ClientMessage message)
        {
            Sequence = message.Payload.ReadUInt32();
            Type = (GameActionType)message.Payload.ReadUInt32();
        }
    }
}