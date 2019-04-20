using ACE.Common.Extensions;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionSetMotd
    {
        [GameAction(GameActionType.SetMotd)]
        public static void Handle(ClientMessage message, Session session)
        {
            var motd = message.Payload.ReadString16L();

            session.Player.HandleActionSetMotd(motd);
        }
    }
}
