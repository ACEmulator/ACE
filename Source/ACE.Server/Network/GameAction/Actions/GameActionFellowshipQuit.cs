
namespace ACE.Server.Network.GameAction.Actions
{
    public class GameActionFellowshipQuit
    {
        [GameAction(GameActionType.FellowshipQuit)]
        public static void Handle(ClientMessage message, Session session)
        {
            bool disbandFellowship = message.Payload.ReadUInt32() > 0;

            session.Player.FellowshipQuit(disbandFellowship);
        }
    }
}
