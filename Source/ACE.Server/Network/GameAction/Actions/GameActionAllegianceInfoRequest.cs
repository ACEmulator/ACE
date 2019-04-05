using ACE.Common.Extensions;

namespace ACE.Server.Network.GameAction.Actions
{
    /// <summary>
    /// Requests allegiance info for a player
    /// </summary>
    public static class GameActionAllegianceInfoRequest
    {
        [GameAction(GameActionType.AllegianceInfoRequest)]
        public static void Handle(ClientMessage message, Session session)
        {
            var playerName = message.Payload.ReadString16L();

            session.Player.HandleActionAllegianceInfoRequest(playerName);
        }
    }
}
