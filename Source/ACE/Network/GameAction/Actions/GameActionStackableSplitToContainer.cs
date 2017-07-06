using ACE.Network.Enum;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionStackableSplitToContainer
    {
        [GameAction(GameActionType.StackableSplitToContainer)]
        public static void Handle(ClientMessage message, Session session)
        {
            // Read in the applicable data.
            uint stackId = message.Payload.ReadUInt32();
            uint containerId = message.Payload.ReadUInt32();
            uint place = message.Payload.ReadUInt32();
            ushort amount = (ushort)message.Payload.ReadUInt32();
            session.Player.HandleActionStackableSplitToContainer(stackId, containerId, place, amount);
        }
    }
}
