using ACE.Entity;
using ACE.StateMachines.Enum;
using System;

namespace ACE.Network.GameAction
{
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

            if (session.Player.Statemachine.CurrentState != (int)MovementStates.Moving) return;
            if ((Math.Abs(session.Player.PhysicsData.Position.SquaredDistanceTo(session.Player.MoveToPosition)) <= session.Player.ArrivedRadiusSquared))
            {
                session.Player.Statemachine.ChangeState((int)MovementStates.Arrived);
                session.Player.AddToActionQueue(session.Player.BlockedGameAction);
                session.Player.Statemachine.ChangeState((int)MovementStates.Idle);
                session.Player.BlockedGameAction = null;
                session.Player.MoveToPosition = null;
                session.Player.ArrivedRadiusSquared = 0.00f;
            }
        }
    }
}
