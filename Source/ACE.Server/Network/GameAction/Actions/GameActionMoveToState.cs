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
            //Console.WriteLine("MoveToState");

            var moveToState = new MoveToState(session.Player, message.Payload);

            if (!session.Player.Teleporting)
            {
                session.Player.OnMoveToState(moveToState);
                session.Player.LastMoveToState = moveToState;

                // do not do the AutoPos / update_object_server path here,
                // let update_object handle it naturally
                // update_object_server can leak deltaTime when spamming keypresses,
                // if deltaTime < minDeltaTime

                //session.Player.SetRequestedLocation(moveToState.Position);
            }

            //if (!moveToState.StandingLongJump)
            session.Player.BroadcastMovement(moveToState);

            if (session.Player.IsPlayerMovingTo)
                session.Player.StopExistingMoveToChains();
        }
    }
}
