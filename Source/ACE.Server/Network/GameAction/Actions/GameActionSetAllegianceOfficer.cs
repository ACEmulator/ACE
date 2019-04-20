using ACE.Common.Extensions;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionSetAllegianceOfficer
    {
        [GameAction(GameActionType.SetAllegianceOfficer)]
        public static void Handle(ClientMessage message, Session session)
        {
            var playerName = message.Payload.ReadString16L();     // The allegiance officer's name
            var officerLevel = message.Payload.ReadUInt32();

            session.Player.HandleActionSetAllegianceOfficer(playerName, officerLevel);
        }
    }
}
