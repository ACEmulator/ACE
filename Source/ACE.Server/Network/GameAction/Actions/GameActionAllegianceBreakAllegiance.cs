
namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionAllegianceBreakAllegiance
    {
        [GameAction(GameActionType.BreakAllegiance)]
        public static void Handle(ClientMessage message, Session session)
        {
            var targetGuid = message.Payload.ReadUInt32();

            session.Player.HandleActionBreakAllegiance(targetGuid);
        }
    }
}
