using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Managers;
using ACE.Server.Network;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Physics.Common;
using ACE.Server.WorldObjects;
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
                    session.Player.UpdateProperty(session.Player, PropertyBool.Attackable, false, true);
                    session.Network.EnqueueSend(new GameMessageSystemChat("Monsters will only attack you if provoked by you first.", ChatMessageType.Broadcast));
                    break;
                case "on":
                default:
                    session.Player.UpdateProperty(session.Player, PropertyBool.Attackable, true, true);
                    session.Network.EnqueueSend(new GameMessageSystemChat("Monsters will attack you normally.", ChatMessageType.Broadcast));
                    break;
            }
        }

        // bestow <name> <level>
        [CommandHandler("bestow", AccessLevel.Advocate, CommandHandlerFlag.RequiresWorld, 2,
            "Sets a character's Advocate Level.",
            "<name> <level>\nAdvocates can bestow any level less than their own.")]
        public static void HandleBestow(Session session, params string[] parameters)
        {
            var charName = string.Join(" ", parameters).Trim();

            var level = parameters[parameters.Length - 1];

            if (!int.TryParse(level, out var advocateLevel) || advocateLevel < 1 || advocateLevel > 7)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"{level} is not a valid advocate level.", ChatMessageType.Broadcast));
                return;
            }

            var advocateName = charName.TrimEnd((" " + level).ToCharArray());

            var playerToFind = PlayerManager.FindByName(advocateName);

            if (playerToFind != null)
            {
                if (playerToFind is Player player)
                {
                    //if (!Advocate.IsAdvocate(player))
                    //{
                    //    session.Network.EnqueueSend(new GameMessageSystemChat($"{playerToFind.Name} is not an Advocate.", ChatMessageType.Broadcast));
                    //    return;
                    //}

                    if (player.IsPK || PropertyManager.GetBool("pk_server").Item)
                    {
                        session.Network.EnqueueSend(new GameMessageSystemChat($"{playerToFind.Name} in a Player Killer and cannot be an Advocate.", ChatMessageType.Broadcast));
                        return;
                    }

                    if (session.Player.AdvocateLevel <= player.AdvocateLevel)
                    {
                        session.Network.EnqueueSend(new GameMessageSystemChat($"You cannot change {playerToFind.Name}'s Advocate status because they are equal to or out rank you.", ChatMessageType.Broadcast));
                        return;
                    }

                    if (advocateLevel >= session.Player.AdvocateLevel && !session.Player.IsAdmin)
                    {
                        session.Network.EnqueueSend(new GameMessageSystemChat($"You cannot bestow {playerToFind.Name}'s Advocate rank to {advocateLevel} because that is equal to or higher than your rank.", ChatMessageType.Broadcast));
                        return;
                    }

                    if (advocateLevel == player.AdvocateLevel)
                    {
                        session.Network.EnqueueSend(new GameMessageSystemChat($"{playerToFind.Name}'s Advocate rank is already at level {advocateLevel}.", ChatMessageType.Broadcast));
                        return;
                    }

                    if (!Advocate.CanAcceptAdvocateItems(player, advocateLevel))
                    {
                        session.Network.EnqueueSend(new GameMessageSystemChat($"You cannot change {playerToFind.Name}'s Advocate status because they do not have capacity for the advocate items.", ChatMessageType.Broadcast));
                        return;
                    }

                    if (Advocate.Bestow(player, advocateLevel))
                        session.Network.EnqueueSend(new GameMessageSystemChat($"{playerToFind.Name} is now an Advocate, level {advocateLevel}.", ChatMessageType.Broadcast));
                    else
                        session.Network.EnqueueSend(new GameMessageSystemChat($"Advocate bestowal of {playerToFind.Name} failed.", ChatMessageType.Broadcast));
                }
                else
                    session.Network.EnqueueSend(new GameMessageSystemChat($"{playerToFind.Name} is not online. Cannot complete bestowal process.", ChatMessageType.Broadcast));
            }
            else
                session.Network.EnqueueSend(new GameMessageSystemChat($"{advocateName} was not found in the database.", ChatMessageType.Broadcast));
        }

        // remove <name>
        [CommandHandler("remove", AccessLevel.Advocate, CommandHandlerFlag.RequiresWorld, 1,
            "Removes the specified character from the Advocate ranks.",
            "<character name>\nAdvocates can remove Advocate status for any Advocate of lower level than their own.")]
        public static void HandleRemove(Session session, params string[] parameters)
        {
            var charName = string.Join(" ", parameters).Trim();

            var playerToFind = PlayerManager.FindByName(charName);

            if (playerToFind != null)
            {
                if (playerToFind is Player player)
                {
                    if (!Advocate.IsAdvocate(player))
                    {
                        session.Network.EnqueueSend(new GameMessageSystemChat($"{playerToFind.Name} is not an Advocate.", ChatMessageType.Broadcast));
                        return;
                    }

                    if (session.Player.AdvocateLevel < player.AdvocateLevel)
                    {
                        session.Network.EnqueueSend(new GameMessageSystemChat($"You cannot remove {playerToFind.Name}'s Advocate status because they out rank you.", ChatMessageType.Broadcast));
                        return;
                    }

                    if (Advocate.Remove(player))
                        session.Network.EnqueueSend(new GameMessageSystemChat($"{playerToFind.Name} is no longer an Advocate.", ChatMessageType.Broadcast));
                    else
                        session.Network.EnqueueSend(new GameMessageSystemChat($"Advocate removal of {playerToFind.Name} failed.", ChatMessageType.Broadcast));
                }
                else
                    session.Network.EnqueueSend(new GameMessageSystemChat($"{playerToFind.Name} is not online. Cannot complete removal process.", ChatMessageType.Broadcast));
            }
            else
                session.Network.EnqueueSend(new GameMessageSystemChat($"{charName} was not found in the database.", ChatMessageType.Broadcast));
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
                    Type = CommandParameterHelpers.ACECommandParameterType.OnlinePlayerNameOrIid,
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
