using System.Collections.Generic;

using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Network;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.WorldObjects;

namespace ACE.Server.Command.Handlers
{
    public static class SentinelCommands
    {
        // cloak < on / off / player / creature >
        [CommandHandler("cloak", AccessLevel.Sentinel, CommandHandlerFlag.RequiresWorld, 1,
            "Sets your cloaking state.",
            "< on / off / player / creature >\n" +
            "This command sets your current cloaking state\n" +
            "< on > You will be completely invisible to players.\n" +
            "< off > You will show up as a normal.\n" +
            "< player > You will appear as a player. (No + and a white radar dot.)\n" +
            "< creature > You will appear as a creature. (No + and an orange radar dot.)")]
        public static void HandleCloak(Session session, params string[] parameters)
        {
            // Please specify if you want cloaking on or off.usage: @cloak < on / off / player / creature >
            // This command sets your current cloaking state.
            // < on > You will be completely invisible to players.
            // < off > You will show up as a normal.
            // < player > You will appear as a player. (No + and a white radar dot.)
            // < creature > You will appear as a creature. (No + and an orange radar dot.)
            // @cloak - Sets your cloaking state.

            // TODO: investigate translucensy/visbility of other cloaked admins.

            switch (parameters?[0].ToLower())
            {
                case "off":
                    if (session.Player.CloakStatus == CloakStatus.Off)
                        return;

                    session.Player.DeCloak();

                    session.Player.SetProperty(PropertyInt.CloakStatus, (int)CloakStatus.Off);

                    CommandHandlerHelper.WriteOutputInfo(session, $"You are no longer cloaked, can no longer pass through doors and will appear as an admin.", ChatMessageType.Broadcast);
                    break;
                case "on":
                    if (session.Player.CloakStatus == CloakStatus.On)
                        return;

                    session.Player.HandleCloak();

                    session.Player.SetProperty(PropertyInt.CloakStatus, (int)CloakStatus.On);

                    CommandHandlerHelper.WriteOutputInfo(session, $"You are now cloaked.\nYou are now ethereal and can pass through doors.", ChatMessageType.Broadcast);
                    break;
                case "player":
                    if (session.AccessLevel > AccessLevel.Envoy)
                    {
                        if (session.Player.CloakStatus == CloakStatus.Player)
                            return;

                        session.Player.SetProperty(PropertyInt.CloakStatus, (int)CloakStatus.Player);

                        session.Player.DeCloak();
                        CommandHandlerHelper.WriteOutputInfo(session, $"You will now appear as a player.", ChatMessageType.Broadcast);
                    }
                    else
                        CommandHandlerHelper.WriteOutputInfo(session, $"You do not have permission to do that state", ChatMessageType.Broadcast);
                    break;
                case "creature":
                    if (session.AccessLevel > AccessLevel.Envoy)
                    {
                        if (session.Player.CloakStatus == CloakStatus.Creature)
                            return;

                        session.Player.SetProperty(PropertyInt.CloakStatus, (int)CloakStatus.Creature);
                        session.Player.Attackable = true;

                        session.Player.DeCloak();
                        CommandHandlerHelper.WriteOutputInfo(session, $"You will now appear as a creature.\nUse @pk free to be allowed to attack all living things.", ChatMessageType.Broadcast);
                    }
                    else
                        CommandHandlerHelper.WriteOutputInfo(session, $"You do not have permission to do that state", ChatMessageType.Broadcast);
                    break;
                default:
                    session.Network.EnqueueSend(new GameMessageSystemChat("Please specify if you want cloaking on or off.", ChatMessageType.Broadcast));
                    break;
            }
        }

        // neversaydie [on/off]
        [CommandHandler("neversaydie", AccessLevel.Sentinel, CommandHandlerFlag.RequiresWorld, 0,
            "Turn immortality on or off.",
            "[ on | off ]\n" + "Defaults to on.")]
        public static void HandleNeverSayDie(Session session, params string[] parameters)
        {
            // @neversaydie [on/off] - Turn immortality on or off. Defaults to on.

            string param;

            if (parameters.Length > 0)
                param = parameters[0];
            else
                param = "on";

            switch (param)
            {
                case "off":
                    session.Player.Invincible = false;
                    session.Network.EnqueueSend(new GameMessageSystemChat("You are once again mortal.", ChatMessageType.Broadcast));
                    break;
                case "on":
                default:
                    session.Player.Invincible = true;
                    session.Network.EnqueueSend(new GameMessageSystemChat("You are now immortal.", ChatMessageType.Broadcast));
                    break;
            }
        }

        // portal_bypass
        [CommandHandler("portal_bypass", AccessLevel.Sentinel, CommandHandlerFlag.RequiresWorld, 0,
            "Toggles the ability to bypass portal restrictions",
            "")]
        public static void HandlePortalBypass(Session session, params string[] parameters)
        {
            // @portal_bypass - Toggles the ability to bypass portal restrictions.

            var param = session.Player.IgnorePortalRestrictions;

            switch (param)
            {
                case true:
                    session.Player.IgnorePortalRestrictions = false;
                    session.Network.EnqueueSend(new GameMessageSystemChat("You are once again bound by portal restrictions.", ChatMessageType.Broadcast));
                    break;
                case false:
                    session.Player.IgnorePortalRestrictions = true;
                    session.Network.EnqueueSend(new GameMessageSystemChat("You are no longer bound by portal restrictions.", ChatMessageType.Broadcast));
                    break;
            }
        }

        // fellowbuff [name]
        [CommandHandler("fellowbuff", AccessLevel.Sentinel, CommandHandlerFlag.RequiresWorld, 0,
            "Buffs your fellowship (or a player's fellowship) with all beneficial spells.",
            "[name]\n"
            + "This command buffs your fellowship (or the fellowship of the specified character).")]
        public static void HandleFellowBuff(Session session, params string[] parameters)
        {
            List<CommandParameterHelpers.ACECommandParameter> aceParams = new List<CommandParameterHelpers.ACECommandParameter>()
            {
                new CommandParameterHelpers.ACECommandParameter() {
                    Type = CommandParameterHelpers.ACECommandParameterType.OnlinePlayerNameOrIid,
                    Required = false,
                    DefaultValue = session.Player
                }
            };
            if (!CommandParameterHelpers.ResolveACEParameters(session, parameters, aceParams)) return;
            if (aceParams[0].AsPlayer.Fellowship == null)
            {
                session.Player.CreateSentinelBuffPlayers(new Player[] { aceParams[0].AsPlayer }, aceParams[0].AsPlayer == session.Player);
                return;
            }

            var fellowshipMembers = aceParams[0].AsPlayer.Fellowship.GetFellowshipMembers();

            session.Player.CreateSentinelBuffPlayers(fellowshipMembers.Values,
                fellowshipMembers.Count == 1 && aceParams[0].AsPlayer.Fellowship.FellowshipLeaderGuid == session.Player.Guid.Full);
        }

        // buff [name]
        [CommandHandler("buff", AccessLevel.Sentinel, CommandHandlerFlag.RequiresWorld, 0,
            "Buffs you (or a player) with all beneficial spells.",
            "[name] [maxLevel]\n"
            + "This command buffs yourself (or the specified character).")]
        public static void HandleBuff(Session session, params string[] parameters)
        {
            List<CommandParameterHelpers.ACECommandParameter> aceParams = new List<CommandParameterHelpers.ACECommandParameter>()
            {
                new CommandParameterHelpers.ACECommandParameter() {
                    Type = CommandParameterHelpers.ACECommandParameterType.OnlinePlayerNameOrIid,
                    Required = false,
                    DefaultValue = session.Player
                },
                new CommandParameterHelpers.ACECommandParameter()
                {
                    Type = CommandParameterHelpers.ACECommandParameterType.ULong,
                    Required = false,
                    DefaultValue = (ulong)8
                }
            };
            if (!CommandParameterHelpers.ResolveACEParameters(session, parameters, aceParams)) return;
            session.Player.CreateSentinelBuffPlayers(new Player[] { aceParams[0].AsPlayer }, aceParams[0].AsPlayer == session.Player, aceParams[1].AsULong);
        }

        // run < on | off | toggle | check >
        [CommandHandler("run", AccessLevel.Sentinel, CommandHandlerFlag.RequiresWorld, 0,
            "Temporarily boosts your run skill.",
            "( on | off | toggle | check )\n"
            + "Boosts the run skill of the PSR so they can pursue the \"bad folks\". The enchantment will wear off after a while. This command defaults to toggle.")]
        public static void HandleRun(Session session, params string[] parameters)
        {
            // usage: @run on| off | toggle | check
            // Boosts the run skill of the PSR so they can pursue the "bad folks".The enchantment will wear off after a while.This command defaults to toggle.
            // @run - Temporarily boosts your run skill.

            string param;

            if (parameters.Length > 0)
                param = parameters[0];
            else
                param = "toggle";

            var spellID = (uint)SpellId.SentinelRun;

            switch (param)
            {
                case "toggle":
                    if (session.Player.EnchantmentManager.HasSpell(spellID))
                        goto case "off";
                    else
                        goto case "on";
                case "check":
                    session.Network.EnqueueSend(new GameMessageSystemChat($"Run speed boost is currently {(session.Player.EnchantmentManager.HasSpell(spellID) ? "ACTIVE" : "INACTIVE")}", ChatMessageType.Broadcast));
                    break;
                case "off":
                    var runBoost = session.Player.EnchantmentManager.GetEnchantment(spellID);
                    if (runBoost != null)
                        session.Player.EnchantmentManager.Remove(runBoost);
                    else
                        session.Network.EnqueueSend(new GameMessageSystemChat("Run speed boost is currently INACTIVE", ChatMessageType.Broadcast));
                    break;
                case "on":
                    session.Player.CreateSingleSpell(spellID);
                    session.Network.EnqueueSend(new GameMessageSystemChat("Run forrest, run!", ChatMessageType.Broadcast));
                    break;
            }
        }
    }
}
