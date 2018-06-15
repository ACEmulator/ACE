using ACE.Common.Extensions;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Managers;
using ACE.Server.Network;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.WorldObjects;
using System;
using System.Linq;

namespace ACE.Server.Command.Handlers
{
    public static class AdvocateCommands
    {
        // attackable { on | off }
        [CommandHandler("attackable", AccessLevel.Advocate, CommandHandlerFlag.RequiresWorld, 1,
            "Sets whether monsters will attack you or not.",
            "[ on | off ]\n"
            + "This command sets whether monsters will attack you unprovoked.\n When turned on, monsters will attack you as if you are a normal player.\n When turned off, monsters will ignore you.")]
        public static void HandleAttackable(Session session, params string[] parameters)
        {
            // usage: @attackable { on,off}
            // This command sets whether monsters will attack you unprovoked.When turned on, monsters will attack you as if you are a normal player.  When turned off, monsters will ignore you.
            // @attackable - Sets whether monsters will attack you or not.

            if (session.Player.IsAdvocate && session.Player.AdvocateLevel < 5)
                return;

            var param = parameters[0];

            switch (param)
            {
                case "off":
                    session.Player.Attackable = false;
                    session.Player.CurrentLandblock.EnqueueBroadcast(session.Player.Location, new GameMessagePublicUpdatePropertyBool(session.Player, PropertyBool.Attackable, session.Player.Attackable ?? false));
                    session.Network.EnqueueSend(new GameMessageSystemChat("Monsters will only attack you if provoked by you first.", ChatMessageType.Broadcast));
                    break;
                case "on":
                default:
                    session.Player.Attackable = true;
                    session.Player.CurrentLandblock.EnqueueBroadcast(session.Player.Location, new GameMessagePublicUpdatePropertyBool(session.Player, PropertyBool.Attackable, session.Player.Attackable ?? false));
                    session.Network.EnqueueSend(new GameMessageSystemChat("Monsters will attack you normally.", ChatMessageType.Broadcast));
                    break;
            }
        }

        // bestow <name> <level>
        [CommandHandler("bestow", AccessLevel.Advocate, CommandHandlerFlag.RequiresWorld, 2)]
        public static void HandleBestow(Session session, params string[] parameters)
        {
            // usage: @bestow <name> <level>
            // @bestow - 

            // TODO: output
        }

        // remove <name>
        [CommandHandler("remove", AccessLevel.Advocate, CommandHandlerFlag.RequiresWorld, 1)]
        public static void HandleRemove(Session session, params string[] parameters)
        {
            // usage: @remove <name>
            // @remove - 

            // TODO: output
        }

        // tele [name,] <longitude> <latitude>
        [CommandHandler("tele", AccessLevel.Advocate, CommandHandlerFlag.RequiresWorld, 1,
            "Teleports you(or a player) to some location.",
            "[name] <longitude> <latitude>\nExample: /tele 0n0w\nExample: /tele plats4days 37s,67w\n"
            + "This command teleports yourself (or the specified character) to the given longitude and latitude.")]
        public static void HandleTele(Session session, params string[] parameters)
        {
            // Used PhatAC source to implement most of this.  Thanks Pea!

            // usage: @tele [name] longitude latitude
            // This command teleports yourself (or the specified character) to the given longitude and latitude.
            // @tele - Teleports you(or a player) to some location.

            // TODO: rework command to support name first then long, lat and then handle correctly.

            if (session.Player.IsAdvocate && session.Player.AdvocateLevel < 5)
                return;

            var singleparam = false;
            if (parameters.Length == 1)
                singleparam = true;

            if (!singleparam && parameters[parameters.Length - 2] == ",")
            {
                var p = parameters.ToList();
                p.RemoveAt(parameters.Length - 2);
                p[p.Count - 1] = "," + p[p.Count - 1];
                parameters = p.ToArray();
            }

            Player targetPlayer = null;
            Position position = null;
            string[] coords = null;
            int coordsParamCount = 2;
            var w = parameters[parameters.Length - 1].ToLower();
            var z = (w.Contains("n") || w.Contains("s")) && (w.Contains("e") || w.Contains("w"));  // accommodate no comma or space(s) in coordinates
            if ((parameters[parameters.Length - 1].Contains(",") && !parameters[parameters.Length - 1].StartsWith(",")) || z) // accommodate space comma space in coordinates
            {
                coordsParamCount = 1;
                if (z)
                {
                    var token1 = w.IndexOf("n");
                    var token2 = w.IndexOf("s");
                    var b = Math.Max(token1, token2) + 1;
                    if (b == w.Length)
                    {
                        ChatPacket.SendServerMessage(session, $"Coordinate longitude and latitude are transposed.", ChatMessageType.Broadcast);
                        return;
                    }
                    coords = new string[] { w.Substring(0, b), w.Substring(b) };
                }
                else
                {
                    coords = parameters.Last().Split(",");
                }
            }
            else
            {
                coords = parameters.TakeLast(2).ToArray();
                coordsParamCount = 2;
            }
            if (parameters.Length > 2 || coordsParamCount == 1 && parameters.Length > 1)
            {
                if (!CoordinateExtensions.TryParse(coords, out string errorMessage, out position))
                {
                    ChatPacket.SendServerMessage(session, errorMessage, ChatMessageType.Broadcast);
                    return;
                }
                var nameParts = new string[parameters.Length - coordsParamCount];
                Array.Copy(parameters, nameParts, parameters.Length - coordsParamCount);
                var charName = nameParts.Aggregate((a, b) => a + " " + b).Replace(",", "").Trim();
                var targetPlayerSession = WorldManager.FindByPlayerName(charName);
                if (targetPlayerSession == null)
                {
                    ChatPacket.SendServerMessage(session, $"Unable to find player {charName}", ChatMessageType.Broadcast);
                    return;
                }
                targetPlayer = targetPlayerSession.Player;
            }
            else
            {
                if (!CoordinateExtensions.TryParse(coords, out string errorMessage, out position))
                {
                    ChatPacket.SendServerMessage(session, errorMessage, ChatMessageType.Broadcast);
                    return;
                }
                targetPlayer = session.Player;
            }

            // TODO: Check if water block?

            ChatPacket.SendServerMessage(session, $"Position: [Cell: 0x{position.LandblockId.Landblock:X4} | Offset: {position.PositionX}, {position.PositionY}, {position.PositionZ} | Facing: {position.RotationX}, {position.RotationY}, {position.RotationZ}, {position.RotationW}]", ChatMessageType.Broadcast);

            targetPlayer.Teleport(position);
        }

    }
    public static class CoordinateExtensions
    {
        public static bool TryParse(string[] parameters, out string errorMessage, out Position position, int startingElement = 0)
        {
            errorMessage = string.Empty;
            position = null;
            if (parameters.Length - startingElement - 1 < 1)
            {
                errorMessage = "not enough parameters";
                return false;
            }

            string northSouth = parameters[startingElement].ToLower().Replace(",", "");
            string eastWest = parameters[startingElement + 1].ToLower().Replace(",", "");


            if (!northSouth.EndsWith("n") && !northSouth.EndsWith("s"))
            {
                errorMessage = "Missing n or s indicator on first parameter";
                return false;
            }

            if (!eastWest.EndsWith("e") && !eastWest.EndsWith("w"))
            {
                errorMessage = "Missing e or w indicator on second parameter";
                return false;
            }

            if (!float.TryParse(northSouth.Substring(0, northSouth.Length - 1), out var coordNS))
            {
                errorMessage = "North/South coordinate is not a valid number.";
                return false;
            }

            if (!float.TryParse(eastWest.Substring(0, eastWest.Length - 1), out var coordEW))
            {
                errorMessage = "East/West coordinate is not a valid number.";
                return false;
            }

            if (northSouth.EndsWith("s"))
                coordNS *= -1.0f;
            if (eastWest.EndsWith("w"))
                coordEW *= -1.0f;

            try
            {
                position = new Position(coordNS, coordEW);
                var cellLandblock = DatManager.CellDat.ReadFromDat<CellLandblock>(position.Cell >> 16 | 0xFFFF);
                position.PositionZ = cellLandblock.GetZ(position.PositionX, position.PositionY);
            }
            catch (System.Exception)
            {
                errorMessage = $"There was a problem with that location (bad coordinates?).";
                return false;
            }
            return true;
        }
    }
}
