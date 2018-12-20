using ACE.Common.Extensions;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionAddPlayerPermission
    {
        [GameAction(GameActionType.AddPlayerPermission)]
        public static void Handle(ClientMessage message, Session session)
        {
            // player to grant corpse looting permissions to
            var playerName = message.Payload.ReadString16L();

            session.Player.HandleActionAddPlayerPermission(playerName);
        }
    }
}
