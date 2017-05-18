using ACE.Entity;
using ACE.StateMachines.Enum;

namespace ACE.Network.GameAction
{
    public static class GameActionAutonomousPosition
    {
        [GameAction(GameActionType.AutonomousPosition)]
        public static void Handle(ClientMessage message, Session session)
        {
            var position = new Position(message.Payload);
            message.Payload.ReadByte();
            session.Player.UpdatePosition(position);
            /*
            if (session.Player.CreatureMovementStates == MovementStates.Moving)
                session.Player.UpdateAutonomousMove();
            */
        }
    }
}
