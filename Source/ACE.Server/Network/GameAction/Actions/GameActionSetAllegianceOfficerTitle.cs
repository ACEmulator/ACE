using ACE.Common.Extensions;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionSetAllegianceOfficerTitle
    {
        [GameAction(GameActionType.SetAllegianceOfficerTitle)]
        public static void Handle(ClientMessage message, Session session)
        {
            var level = message.Payload.ReadUInt32();
            var title = message.Payload.ReadString16L();

            session.Player.HandleActionSetAllegianceOfficerTitle(level, title);
        }
    }
}
