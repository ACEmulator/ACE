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

            if (session.Player.CreatureMovementStates == MovementStates.Moving)
                session.Player.UpdateAutonomousMove();
        }
    }
}
