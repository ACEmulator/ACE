using ACE.Common.Extensions;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionRemoveFromPlayerConsentList
    {
        [GameAction(GameActionType.RemoveFromPlayerConsentList)]
        public static void Handle(ClientMessage message, Session session)
        {
            // the granter we are removing from consent list
            var playerName = message.Payload.ReadString16L();

            session.Player.HandleActionRemoveFromPlayerConsentList(playerName);
        }
    }
}
