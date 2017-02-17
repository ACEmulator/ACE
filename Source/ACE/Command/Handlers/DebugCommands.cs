using System;

using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Managers;
using ACE.Network;

namespace ACE.Command.Handlers
{
    public static class DebugCommands
    {
        [CommandHandler("gps", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld)]
        public static void HandleDebugGPS(Session session, params string[] parameters)
        {
            var position = session.Player.Position;
            ChatPacket.SendSystemMessage(session, $"Position: [Cell: 0x{position.Cell.ToString("X4")} | Offset: {position.Offset.X}, {position.Offset.Y}, {position.Offset.Z} | Facing: {position.Facing.X}, {position.Facing.Y}, {position.Facing.Z}, {position.Facing.W}]");
        }

        // teleloc location
        [CommandHandler("teleloc", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1)]
        public static void HandleDebugTeleportLocation(Session session, params string[] parameters)
        {
            var location = String.Join(" ", parameters);
            var teleportLocation = AssetManager.GetTeleport(location);
            if (teleportLocation == null)
                return;

            session.Player.Teleport(teleportLocation);
        }

        // telexyz cell x y z qx qy qz qw
        [CommandHandler("telexyz", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 8)]
        public static void HandleDebugTeleportXYZ(Session session, params string[] parameters)
        {
            uint cell;
            if (!uint.TryParse(parameters[0], out cell))
                return;

            var positionData = new float[7];
            for (uint i = 0u; i < 7u; i++)
            {
                float position;
                if (!float.TryParse(parameters[i + 1], out position))
                    return;

                positionData[i] = position;
            }

            session.Player.Teleport(new Position(cell, positionData[0], positionData[1], positionData[2], positionData[3], positionData[4], positionData[5], positionData[6]));
        }

        // Example /teleto 40.0n 55.0w
        [CommandHandler("teleto", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 2)]
        public static void HandleDebugTeleportCoords(Session session, params string[] parameters)
        {
            string northSouth = parameters[0].ToLower();
            string eastWest = parameters[1].ToLower();

            if (!northSouth.EndsWith("n") && !northSouth.EndsWith("s"))
            {
                ChatPacket.SendSystemMessage(session, "Missing n or s indicator on first parameter");
                return;
            }

            if (!eastWest.EndsWith("e") && !eastWest.EndsWith("w"))
            {
                ChatPacket.SendSystemMessage(session, "Missing e or w indicator on second parameter");
                return;
            }

            float coordNS;
            if (!float.TryParse(northSouth.Substring(0, northSouth.Length - 1), out coordNS))
            {
                ChatPacket.SendSystemMessage(session, "North/South coordinate is not a valid number.");
                return;
            }

            float coordEW;
            if (!float.TryParse(eastWest.Substring(0, eastWest.Length - 1), out coordEW))
            {
                ChatPacket.SendSystemMessage(session, "East/West coordinate is not a valid number.");
                return;
            }

            if (northSouth.EndsWith("s"))
                coordNS *= -1.0f;
            if (eastWest.EndsWith("w"))
                coordEW *= -1.0f;

            Position position = null;
            try
            {
                position = new Position(coordNS, coordEW);
            }
            catch (System.Exception)
            {
                ChatPacket.SendSystemMessage(session, "There was a problem teleporting to that location (bad coordinates?).");
                return;
            }

            // TODO: Check if water block?

            ChatPacket.SendSystemMessage(session, $"Position: [Cell: 0x{position.Cell.ToString("X4")} | Offset: {position.Offset.X}, {position.Offset.Y}, {position.Offset.Z} | Facing: {position.Facing.X}, {position.Facing.Y}, {position.Facing.Z}, {position.Facing.W}]");

            session.Player.Teleport(position);
        }


        [CommandHandler("grantxp", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1)]
        public static void HandleGrantXp(Session session, params string[] parameters)
        {
            uint xp = 0;

            if (parameters?.Length > 0 && uint.TryParse(parameters[0], out xp))
            {
                session.Player.GrantXp(xp);
            }
            else
            {
                ChatPacket.SendSystemMessage(session, "Usage: /grantxp 1234");
                return;
            }
        }
    }
}
