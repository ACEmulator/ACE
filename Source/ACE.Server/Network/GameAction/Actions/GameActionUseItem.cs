
namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionUseItem
    {
        [GameAction(GameActionType.Use)]
        public static void Handle(ClientMessage message, Session session)
        {
            uint itemGuid = message.Payload.ReadUInt32();

            session.Player.HandleActionUseItem(itemGuid);
        }
    }
}
