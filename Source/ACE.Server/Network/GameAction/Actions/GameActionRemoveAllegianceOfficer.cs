using ACE.Common.Extensions;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionRemoveAllegianceOfficer
    {
        [GameAction(GameActionType.RemoveAllegianceOfficer)]
        public static void Handle(ClientMessage message, Session session)
        {
            var officerName = message.Payload.ReadString16L();

            session.Player.HandleActionRemoveAllegianceOfficer(officerName);
        }
    }
}
