using ACE.Entity;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionIdentifyObject
    {
        [GameAction(GameActionType.IdentifyObject)]
        public static void Handle(ClientMessage message, Session session)
        {
            var id = message.Payload.ReadUInt32();

            ObjectGuid guid = new ObjectGuid(id);

            session.Player.ExamineObject(guid);
        }
    }
}
