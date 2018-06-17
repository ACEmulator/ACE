using ACE.Entity;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionAllegianceBreakAllegiance
    {
        [GameAction(GameActionType.BreakAllegiance)]
        public static void Handle(ClientMessage message, Session session)
        {
            var target = message.Payload.ReadUInt32();
            var targetGuid = new ObjectGuid(target);

            session.Player.HandleActionBreakAllegiance(targetGuid);
        }
    }
}
