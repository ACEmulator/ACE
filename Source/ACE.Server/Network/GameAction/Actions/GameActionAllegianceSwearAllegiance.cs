
namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionAllegianceSwearAllegiance
    {
        [GameAction(GameActionType.SwearAllegiance)]
        public static void Handle(ClientMessage message, Session session)
        {
            var targetGuid = message.Payload.ReadUInt32();

            session.Player.HandleActionSwearAllegiance(targetGuid);
        }
    }
}
