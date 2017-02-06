using ACE.Entity;
using ACE.Managers;
using ACE.Network;
using System;

namespace ACE.Command
{
    public static class DebugCommands
    {
        [CommandHandler("gps", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld)]
        public static void HandleDebugGPS(Session session, params string[] parameters)
        {
            var position = session.Character.Position;
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

            session.Character.Teleport(teleportLocation);
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

            session.Character.Teleport(new Position(cell, positionData[0], positionData[1], positionData[2], positionData[3], positionData[4], positionData[5], positionData[6]));
        }
        
        [CommandHandler("teleto", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 2)]
        public static void HandleDebugTeleportCoords(Session session, params string[] parameters)
        {
            string ns = parameters[0].ToLower();
            string ew = parameters[1].ToLower();

            if (!ns.EndsWith("n") && !ns.EndsWith("s"))
            {
                ChatPacket.SendSystemMessage(session, "Missing n or s indicator on first parameter");
                return;
            }
                
            if (!ew.EndsWith("e") && !ew.EndsWith("w"))
            {
                ChatPacket.SendSystemMessage(session, "Missing e or w indicator on second parameter");
                return;
            }            

            float coordNS;
            if (!float.TryParse(ns.Substring(0, ns.Length - 1), out coordNS))
            {
                ChatPacket.SendSystemMessage(session, "North/South coordinate is not a valid number.");
                return;
            }
            
            float coordEW;
            if (!float.TryParse(ew.Substring(0, ew.Length - 1), out coordEW))
            {
                ChatPacket.SendSystemMessage(session, "East/West coordinate is not a valid number.");
                return;
            }

            if (ns.EndsWith("s"))
                coordNS *= -1.0f;
            if (ew.EndsWith("w"))
                coordEW *= -1.0f;

            Position position = GetPositionFromCoords(coordNS, coordEW);

            if(position == null)
            {
                ChatPacket.SendSystemMessage(session, "Bad coordinate location.");
                return;
            }

            // TODO: Check if water block?

            ChatPacket.SendSystemMessage(session, $"Position: [Cell: 0x{position.Cell.ToString("X4")} | Offset: {position.Offset.X}, {position.Offset.Y}, {position.Offset.Z} | Facing: {position.Facing.X}, {position.Facing.Y}, {position.Facing.Z}, {position.Facing.W}]");

            session.Character.Teleport(position);            
        }

        // TODO: Need to move this to another file
        private static Position GetPositionFromCoords(float NorthSouth, float EastWest)
        {
            NorthSouth -= 0.5f;
            EastWest -= 0.5f;
            NorthSouth *= 10.0f;
            EastWest *= 10.0f;

            uint basex = (uint)(EastWest + 0x400);
            uint basey = (uint)(NorthSouth + 0x400);

            if (basex < 0 || basex >= 0x7F8 || basey < 0 || basey >= 0x7F8)
                return null;

            float xOffset = ((basex & 7) * 24.0f) + 12;
            float yOffset = ((basey & 7) * 24.0f) + 12;
            float z = 200.0f;  // TODO: calculate appropriate z from file.

            return new Position(GetCellFromBase(basex, basey), xOffset, yOffset, z, 0.0f, 0.0f, 0.0f, 1.0f);
        }

        // TODO: Need to move this to another file
        private static uint GetCellFromBase(uint basex, uint basey)
        {
            byte blockx = (byte)(basex >> 3);
            byte blocky = (byte)(basey >> 3);
            byte cellx = (byte)(basex & 7);
            byte celly = (byte)(basey & 7);

            uint block = (uint)((blockx << 8) | blocky);
            uint cell = (uint)((cellx << 3) | celly);

            return (block << 16) | (cell + 1);
        }
    }
}
