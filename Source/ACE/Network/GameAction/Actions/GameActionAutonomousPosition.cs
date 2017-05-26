using ACE.Entity;
using ACE.StateMachines.Enum;

namespace ACE.Network.GameAction
{
    public static class GameActionAutonomousPosition
    {
        [GameAction(GameActionType.AutonomousPosition)]
        public static void Handle(ClientMessage message, Session session)
        {
            // FIXME(ddevec): This should be synchronized -- nonblocking action?
            var position = new Position(message.Payload);
            message.Payload.ReadByte();
            session.Player.AddNonBlockingAction(() => session.Player.ActUpdatePosition(position));
            // session.Player.UpdatePosition(position);
            /*
            if (session.Player.CreatureMovementStates == MovementStates.Moving)
                session.Player.UpdateAutonomousMove();
            */
        }
    }
}
