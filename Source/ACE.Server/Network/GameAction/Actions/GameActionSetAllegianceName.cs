using ACE.Common.Extensions;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionSetAllegianceName
    {
        [GameAction(GameActionType.SetAllegianceName)]
        public static void Handle(ClientMessage message, Session session)
        {
            var allegianceName = message.Payload.ReadString16L();

            session.Player.HandleActionSetAllegianceName(allegianceName);
        }
    }
}
