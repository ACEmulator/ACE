using System.Threading.Tasks;

using ACE.Entity;

namespace ACE.Network.GameAction
{
    public static class GameActionAutonomousPosition
    {
        [GameAction(GameActionType.AutonomousPosition)]
        #pragma warning disable 1998
        public static async Task Handle(ClientMessage message, Session session)
        {
            var position = new Position(message.Payload);
            var instanceTimestamp = message.Payload.ReadUInt16();
            var serverControlTimestamp = message.Payload.ReadUInt16();
            var teleportTimestamp = message.Payload.ReadUInt16();
            var forcePositionTimestamp = message.Payload.ReadUInt16();
            message.Payload.ReadByte();
            session.Player.RequestUpdatePosition(position);
            /*
            if (session.Player.CreatureMovementStates == MovementStates.Moving)
                session.Player.UpdateAutonomousMove();
            */
        }
        #pragma warning restore 1998
    }
}
