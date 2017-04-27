using ACE.Entity;

namespace ACE.Network.GameAction
{
    using System;

    using global::ACE.StateMachines.Enum;

    public static class GameActionAutonomousPosition
    {
        [GameAction(GameActionType.AutonomousPosition)]
        public static void Handle(ClientMessage message, Session session)
        {
            var position = new Position(message.Payload);
            var instanceTimestamp = message.Payload.ReadUInt16();
            var serverControlTimestamp = message.Payload.ReadUInt16();
            var teleportTimestamp = message.Payload.ReadUInt16();
            var forcePositionTimestamp = message.Payload.ReadUInt16();
            message.Payload.ReadByte();
            session.Player.UpdatePosition(position);
            if (session.Player.Statemachine.CurrentState == (int)MovementStates.Moving
               || session.Player.Statemachine.CurrentState == (int)MovementStates.Arrived)
            {
                if (
                    session.Player.MoveToPosition.SquaredDistanceTo(
                        session.Player.PhysicsData.Position)
                    <= 1.0f)
                {
                    session.Player.Statemachine.ChangeState((int)MovementStates.Arrived);
                    session.Player.AddToActionQueue(session.Player.BlockedGameAction);
                    session.Player.Statemachine.ChangeState((int)MovementStates.Idle);
                }
            }
        }
    }
}