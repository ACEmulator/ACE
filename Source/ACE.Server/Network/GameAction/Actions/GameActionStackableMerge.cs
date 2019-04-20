
namespace ACE.Server.Network.GameAction.Actions
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
            uint mergeFromGuid = message.Payload.ReadUInt32();
            uint mergeToGuid = message.Payload.ReadUInt32();
            int amount = message.Payload.ReadInt32();

            session.Player.HandleActionStackableMerge(mergeFromGuid, mergeToGuid, amount);
        }
    }
}
