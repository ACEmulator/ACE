using System;
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

            if (!session.Player.Teleporting)
            {
                session.Player.OnMoveToState(moveToState);
                session.Player.LastMoveToState = moveToState;

                // should the server broadcast position updates on MoveToState?
                session.Player.SetRequestedLocation(moveToState.Position, false);
            }

            //if (!moveToState.StandingLongJump)
            session.Player.BroadcastMovement(moveToState);

            if (session.Player.IsPlayerMovingTo)
                session.Player.StopExistingMoveToChains();
        }
    }
}
