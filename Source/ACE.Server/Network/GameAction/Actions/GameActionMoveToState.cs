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
            var moveToState = new MoveToState(session.Player, message.Payload);

            session.Player.SetRequestedLocation(moveToState.Position);

            //if (!moveToState.StandingLongJump)
                session.Player.BroadcastMovement(moveToState);

            if (session.Player.IsPlayerMovingTo)
                session.Player.StopExistingMoveToChains();
        }
    }
}
