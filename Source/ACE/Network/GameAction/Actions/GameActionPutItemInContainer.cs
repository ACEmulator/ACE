namespace ACE.Network.GameAction.Actions
{
    using global::ACE.Entity;
    using global::ACE.Network.GameAction.QueuedGameActions;

    public static class GameActionPutItemInContainer
    {
        [GameAction(GameActionType.PutItemInContainer)]
        public static void Handle(ClientMessage message, Session session)
        {
            var itemGuid = new ObjectGuid(message.Payload.ReadUInt32());
            var containerGuid = new ObjectGuid(message.Payload.ReadUInt32());
            QueuedGameAction action = new QueuedGameActionPutItemInContainer(containerGuid.Full, itemGuid.Full, session.Player.Location.LandblockId);
            session.Player.AddToActionQueue(action);
        }
    }
}