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
        [CommandHandler("tele", AccessLevel.Advocate, CommandHandlerFlag.RequiresWorld, 2,
            "Teleports you(or a player) to some location.",
            "<longitude> <latitude> [name] \n"
            + "This command teleports yourself (or the specified character) to the given longitude and latitude.")]
        public static void HandleTele(Session session, params string[] parameters)
        {
            // Used PhatAC source to implement most of this.  Thanks Pea!

            // usage: @tele [name,] longitude latitude
            // This command teleports yourself (or the specified character) to the given longitude and latitude.
            // @tele - Teleports you(or a player) to some location.

            // TODO: rework command to support name first then long, lat and then handle correctly.

            if (session.Player.IsAdvocate && session.Player.AdvocateLevel < 5)
                return;

            Player targetPlayer = null;

            if (!CoordinateExtensions.TryParse(parameters, out string errorMessage, out Position position))
            {
                ChatPacket.SendServerMessage(session, errorMessage, ChatMessageType.Broadcast);
                return;
            }

            if (parameters.Length > 2)
            {
                var charName = CharacterNameExtensions.StringArrayToCharacterName(parameters, 2);
                var targetPlayerSession = WorldManager.FindByPlayerName(charName);
                if (targetPlayerSession == null)
                {
                    ChatPacket.SendServerMessage(session, $"Unable to find player {charName}", ChatMessageType.Broadcast);
                    return;
                }
                targetPlayer = targetPlayerSession.Player;
            }
            else
                targetPlayer = session.Player;

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
