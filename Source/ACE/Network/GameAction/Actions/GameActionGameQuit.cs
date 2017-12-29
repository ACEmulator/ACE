using System.Threading.Tasks;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionGameQuit
    {
        [GameAction(GameActionType.Quit2)]
        #pragma warning disable 1998
        public static async Task Handle(ClientMessage message, Session session)
        {
            ////var dobEvent = new GameMessages.Messages.GameMessageSystemChat($"Quit request ack.", ChatMessageType.Broadcast);
            ////var gameId = 2056986625u;
            ////var msgGameOver = new GameEvent.Events.GameEventGameOver(session, gameId, -2);
            ////session.Network.EnqueueSend(dobEvent, msgGameOver);

            // This msg has no incoming data so the session will be the only way to find the game (gameboard) the player is in.
        }
        #pragma warning restore 1998
    }
}
