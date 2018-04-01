using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Network;
using ACE.Server.Network.GameMessages.Messages;

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
            "[name,] <longitude> <latitude>\n"
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

            string northSouth = parameters[0].ToLower().Replace(",", "");
            string eastWest = parameters[1].ToLower().Replace(",", "");

            if (!northSouth.EndsWith("n") && !northSouth.EndsWith("s"))
            {
                ChatPacket.SendServerMessage(session, "Missing n or s indicator on first parameter", ChatMessageType.Broadcast);
                return;
            }

            if (!eastWest.EndsWith("e") && !eastWest.EndsWith("w"))
            {
                ChatPacket.SendServerMessage(session, "Missing e or w indicator on second parameter", ChatMessageType.Broadcast);
                return;
            }

            if (!float.TryParse(northSouth.Substring(0, northSouth.Length - 1), out var coordNS))
            {
                ChatPacket.SendServerMessage(session, "North/South coordinate is not a valid number.", ChatMessageType.Broadcast);
                return;
            }

            if (!float.TryParse(eastWest.Substring(0, eastWest.Length - 1), out var coordEW))
            {
                ChatPacket.SendServerMessage(session, "East/West coordinate is not a valid number.", ChatMessageType.Broadcast);
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
                var cellLandblock = DatManager.CellDat.ReadFromDat<CellLandblock>(position.Cell >> 16 | 0xFFFF);
                position.PositionZ = cellLandblock.GetZ(position.PositionX, position.PositionY);
            }
            catch (System.Exception)
            {
                ChatPacket.SendServerMessage(session, "There was a problem teleporting to that location (bad coordinates?).", ChatMessageType.Broadcast);
                return;
            }

            // TODO: Check if water block?

            ChatPacket.SendServerMessage(session, $"Position: [Cell: 0x{position.LandblockId.Landblock:X4} | Offset: {position.PositionX}, {position.PositionY}, {position.PositionZ} | Facing: {position.RotationX}, {position.RotationY}, {position.RotationZ}, {position.RotationW}]", ChatMessageType.Broadcast);

            session.Player.Teleport(position);
        }
    }
}
