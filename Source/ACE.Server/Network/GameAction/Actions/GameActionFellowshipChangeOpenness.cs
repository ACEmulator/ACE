
namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionFellowshipChangeOpenness
    {
        [GameAction(GameActionType.FellowshipChangeOpenness)]
        public static void Handle(ClientMessage message, Session session)
        {
            var isOpen = message.Payload.ReadUInt32() != 0;

            session.Player.HandleActionFellowshipChangeOpenness(isOpen);
        }
    }
}
