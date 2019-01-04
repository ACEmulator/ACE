
namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionRemoveShortcut
    {
        [GameAction(GameActionType.RemoveShortCut)]
        public static void Handle(ClientMessage message, Session session)
        {
            var index = message.Payload.ReadUInt32();

            session.Player.HandleActionRemoveShortcut(index);
        }
    }
}
