
namespace ACE.Network.GameAction.Actions
{
    using global::ACE.Entity;

    public static class GameActionPutItemInContainer
    {
        [GameAction(GameActionType.PutItemInContainer)]
        public static void Handle(ClientMessage message, Session session)
        {
            var itemGuid = new ObjectGuid(message.Payload.ReadUInt32());
            var containerGuid = new ObjectGuid(message.Payload.ReadUInt32());
            session.Player.HandlePutItemInContainer(itemGuid, containerGuid, session);
        }
    }
}
