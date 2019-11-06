using System;

using ACE.Entity;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionAutonomousPosition
    {
        /// <summary>
        /// Sent every ~1 second by the client when a player is moving,
        /// with the latest position from the client
        /// </summary>
        [GameAction(GameActionType.AutonomousPosition)]
        public static void Handle(ClientMessage message, Session session)
        {
            //Console.WriteLine($"{session.Player.Name}.AutoPos");

            var position = new Position(message.Payload);

            var instanceTimestamp = message.Payload.ReadUInt16();
            var serverControlTimestamp = message.Payload.ReadUInt16();
            var teleportTimestamp = message.Payload.ReadUInt16();
            var forcePositionTimestamp = message.Payload.ReadUInt16();

            session.Player.LastContact = message.Payload.ReadByte() != 0;   // TRUE if player is currently on ground

            if (session.Player.LastContact)
                session.Player.LastGroundPos = position;

            if (!session.Player.Teleporting)
                session.Player.SetRequestedLocation(position);

            message.Payload.Align();
        }
    }
}
