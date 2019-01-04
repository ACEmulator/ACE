using ACE.Entity;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionGiveObjectRequest
    {
        [GameAction(GameActionType.GiveObjectRequest)]
        public static void Handle(ClientMessage message, Session session)
        {
            uint targetID = message.Payload.ReadUInt32();
            uint objectID = message.Payload.ReadUInt32();
            int amount = message.Payload.ReadInt32();
            ObjectGuid targetGuid = new ObjectGuid(targetID);
            ObjectGuid objectGuid = new ObjectGuid(objectID);

            session.Player.HandleActionGiveObjectRequest(targetGuid, objectGuid, amount);
        }
    }
}
