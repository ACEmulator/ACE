
namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionQueryHealth
    {
        [GameAction(GameActionType.QueryHealth)]
        public static void Handle(ClientMessage message, Session session)
        {
            uint objectGuid = message.Payload.ReadUInt32();

            session.Player.HandleActionQueryHealth(objectGuid);
        }
    }
}
