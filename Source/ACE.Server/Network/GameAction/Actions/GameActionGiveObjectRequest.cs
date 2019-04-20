
namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionGiveObjectRequest
    {
        [GameAction(GameActionType.GiveObjectRequest)]
        public static void Handle(ClientMessage message, Session session)
        {
            uint targetGuid = message.Payload.ReadUInt32();
            uint objectGuid = message.Payload.ReadUInt32();
            int amount = message.Payload.ReadInt32();

            session.Player.HandleActionGiveObjectRequest(targetGuid, objectGuid, amount);
        }
    }
}
