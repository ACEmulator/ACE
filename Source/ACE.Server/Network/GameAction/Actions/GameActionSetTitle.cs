
namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionSetTitle
    {
        [GameAction(GameActionType.TitleSet)]
        public static void Handle(ClientMessage message, Session session)
        {
            var title = message.Payload.ReadUInt32();

            session.Player.HandleActionSetTitle(title);
        }
    }
}
