using ACE.Entity;
namespace ACE.Network.GameAction.Actions
{
    public static class GameActionPutItemInContainer
    {
        [GameAction(GameActionType.PutItemInContainer)]
        public static void Handle(ClientMessage message, Session session)
        {
            var itemGuid = new ObjectGuid(message.Payload.ReadUInt32());
            var containerGuid = new ObjectGuid(message.Payload.ReadUInt32());
            var placement = message.Payload.ReadUInt32();
            session.Player.HandleActionPutItemInContainer(itemGuid, containerGuid, placement);
        }
    }
}
