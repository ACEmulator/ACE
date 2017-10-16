using ACE.Common.Extensions;
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
        public static void Handle(ClientMessage message, Session session)
        {
            uint mergeFromId = message.Payload.ReadUInt32();
            uint mergeToId = message.Payload.ReadUInt32();
            uint amount = message.Payload.ReadUInt32();
            session.Player.HandleActionStackableMerge(session, new ObjectGuid(mergeFromId), new ObjectGuid(mergeToId), amount);
        }
    }
}
