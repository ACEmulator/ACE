using System.Threading.Tasks;

using ACE.Entity;

namespace ACE.Network.GameAction.Actions
{
    /// <summary>
    /// This method processes the Game Action (F7B1) Inventory_StackableMerge (0x0054) and calls
    /// the HandleActionStackableMerge method on the player object. Og II
    /// </summary>
    public static class GameActionStackableMerge
    {
        [GameAction(GameActionType.StackableMerge)]
        public static async Task Handle(ClientMessage message, Session session)
        {
            uint mergeFromId = message.Payload.ReadUInt32();
            uint mergeToId = message.Payload.ReadUInt32();
            int amount = message.Payload.ReadInt32();
            await session.Player.StackableMerge(session, new ObjectGuid(mergeFromId), new ObjectGuid(mergeToId), amount);
        }
    }
}
