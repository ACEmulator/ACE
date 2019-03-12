using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Network;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Physics.Common;
using System.Collections.Generic;

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
                    session.Player.EnqueueBroadcast(new GameMessagePublicUpdatePropertyBool(session.Player, PropertyBool.Attackable, session.Player.Attackable ?? false));
                    session.Network.EnqueueSend(new GameMessageSystemChat("Monsters will only attack you if provoked by you first.", ChatMessageType.Broadcast));
                    break;
                case "on":
                default:
                    session.Player.Attackable = true;
                    session.Player.EnqueueBroadcast(new GameMessagePublicUpdatePropertyBool(session.Player, PropertyBool.Attackable, session.Player.Attackable ?? false));
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

            if (session.Player.IsAdvocate && session.Player.AdvocateLevel < 5)
                return;

            List<CommandParameterHelpers.ACECommandParameter> aceParams = new List<CommandParameterHelpers.ACECommandParameter>()
            {
                new CommandParameterHelpers.ACECommandParameter() {
                    Type = CommandParameterHelpers.ACECommandParameterType.OnlinePlayerName,
                    Required = false,
                    DefaultValue = session.Player
                },
                new CommandParameterHelpers.ACECommandParameter()
                {
                    Type = CommandParameterHelpers.ACECommandParameterType.Location,
                    Required = true,
                    ErrorMessage = "You must supply a location to teleport to.\nExample: /tele 37s,67w"
                }
            };
            if (!CommandParameterHelpers.ResolveACEParameters(session, parameters, aceParams)) return;

            // Check if water block
            var landblock = LScape.get_landblock(aceParams[1].AsPosition.LandblockId.Raw);
            if (landblock.WaterType == LandDefs.WaterType.EntirelyWater)
            {
                ChatPacket.SendServerMessage(session, $"Landblock 0x{aceParams[1].AsPosition.LandblockId.Landblock:X4} is entirely filled with water, and is impassable", ChatMessageType.Broadcast);
                return;
            }

            ChatPacket.SendServerMessage(session, $"Position: [Cell: 0x{aceParams[1].AsPosition.LandblockId.Landblock:X4} | Offset: {aceParams[1].AsPosition.PositionX}, "+
                $"{aceParams[1].AsPosition.PositionY}, {aceParams[1].AsPosition.PositionZ} | Facing: {aceParams[1].AsPosition.RotationX}, {aceParams[1].AsPosition.RotationY}, " +
                $"{ aceParams[1].AsPosition.RotationZ}, {aceParams[1].AsPosition.RotationW}]", ChatMessageType.Broadcast);

            aceParams[0].AsPlayer.Teleport(aceParams[1].AsPosition);
        }
    }
}
