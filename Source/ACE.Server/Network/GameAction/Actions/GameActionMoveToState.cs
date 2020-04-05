using ACE.Server.Network.Structure;

namespace ACE.Server.Network.GameAction.Actions
{
    /// <summary>
    /// Sent by the client when a movement key is pressed / released
    /// </summary>
    public static class GameActionMoveToState
    {
        [GameAction(GameActionType.MoveToState)]
        public static void Handle(ClientMessage message, Session session)
        {
            //Console.WriteLine($"{session.Player.Name}.MoveToState");

            var moveToState = new MoveToState(session.Player, message.Payload);
            session.Player.CurrentMoveToState = moveToState;

            if (session.Player.IsPlayerMovingTo)
                session.Player.StopExistingMoveToChains();

            if (session.Player.IsPlayerMovingTo2)
                session.Player.StopExistingMoveToChains2();

            if (!session.Player.Teleporting)
            {
                session.Player.OnMoveToState(moveToState);
                session.Player.LastMoveToState = moveToState;

                // MoveToState - UpdatePosition broadcasts were capped to 1 per second in retail
                session.Player.SetRequestedLocation(moveToState.Position, false);
            }

            //if (!moveToState.StandingLongJump)
                session.Player.BroadcastMovement(moveToState);
        }
    }
}
