
namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionIdentifyObject
    {
        [GameAction(GameActionType.IdentifyObject)]
        public static void Handle(ClientMessage message, Session session)
        {
            var objectGuid = message.Payload.ReadUInt32();

            session.Player.HandleActionIdentifyObject(objectGuid);
        }
    }
}
