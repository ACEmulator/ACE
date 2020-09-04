using System;
using System.Collections.Generic;
using System.Globalization;

using ACE.Database;
using ACE.Database.Models.Auth;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Managers;
using ACE.Server.Network;
using ACE.Server.Network.Enum;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Managers;
using ACE.Server.WorldObjects;

using log4net;

namespace ACE.Server.Command.Handlers
{
    public static class SentinelCommands
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
            "Toggles the ability to bypass portal restrictions.",
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

        // boot { account | char | iid } who
        [CommandHandler("boot", AccessLevel.Sentinel, CommandHandlerFlag.None, 2,
            "Boots the character out of the game.",
            "[account | char | iid] who (, reason) \n"
            + "This command boots the specified character out of the game. You can specify who to boot by account, character name, or player instance id. 'who' is the account / character / instance id to actually boot. You can optionally include a reason for the boot.\n"
            + "Example: @boot char Character Name\n"
            + "         @boot account AccountName\n"
            + "         @boot iid 0x51234567\n"
            + "         @boot char Character Name, Reason for being booted\n")]
        public static void HandleBoot(Session session, params string[] parameters)
        {
            // usage: @boot { account,char, iid} who
            // This command boots the specified character out of the game.You can specify who to boot by account, character name, or player instance id.  'who' is the account / character / instance id to actually boot.
            // @boot - Boots the character out of the game.

            var whomToBoot = parameters[1];
            string specifiedReason = null;

            if (parameters.Length > 1)
            {
                var parametersAfterBootType = "";
                for (var i = 1; i < parameters.Length; i++)
                {
                    parametersAfterBootType += parameters[i] + " ";
                }
                parametersAfterBootType = parametersAfterBootType.Trim();
                var completeBootNamePlusCommaSeperatedReason = parametersAfterBootType.Split(",");
                whomToBoot = completeBootNamePlusCommaSeperatedReason[0].Trim();
                if (completeBootNamePlusCommaSeperatedReason.Length > 1)
                    specifiedReason = parametersAfterBootType.Replace($"{whomToBoot},", "").Trim();
            }

            string whatToBoot = null;
            Session sessionToBoot = null;
            switch (parameters[0].ToLower())
            {
                case "char":
                    whatToBoot = "character";
                    sessionToBoot = PlayerManager.GetOnlinePlayer(whomToBoot)?.Session;
                    break;
                case "account":
                    whatToBoot = "account";
                    sessionToBoot = NetworkManager.Find(whomToBoot);
                    break;
                case "iid":
                    whatToBoot = "instance id";
                    if (!whomToBoot.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                    {
                        CommandHandlerHelper.WriteOutputInfo(session, $"That is not a valid Instance ID (IID). IIDs must be between 0x{ObjectGuid.PlayerMin:X8} and 0x{ObjectGuid.PlayerMax:X8}", ChatMessageType.Broadcast);
                        return;
                    }    
                    if (uint.TryParse(whomToBoot.Substring(2), NumberStyles.HexNumber, CultureInfo.CurrentCulture, out var iid))
                    {
                        sessionToBoot = PlayerManager.GetOnlinePlayer(iid)?.Session;
                    }
                    else
                    {
                        CommandHandlerHelper.WriteOutputInfo(session, $"That is not a valid Instance ID (IID). IIDs must be between 0x{ObjectGuid.PlayerMin:X8} and 0x{ObjectGuid.PlayerMax:X8}", ChatMessageType.Broadcast);
                        return;
                    }
                    break;
                default:
                    CommandHandlerHelper.WriteOutputInfo(session, "You must specify what you are booting with char, account, or iid as the first parameter.", ChatMessageType.Broadcast);
                    return;
            }

            if (sessionToBoot == null)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Cannot boot \"{whomToBoot}\" because that {whatToBoot} is not currently online or cannot be found. Check syntax/spelling and try again.", ChatMessageType.Broadcast);
                return;
            }

            // Boot the player
            var bootText = $"Booting {whatToBoot} {whomToBoot}.{(specifiedReason != null ? $" Reason: {specifiedReason}" : "")}";
            CommandHandlerHelper.WriteOutputInfo(session, bootText, ChatMessageType.Broadcast);
            sessionToBoot.Terminate(SessionTerminationReason.AccountBooted, new GameMessageBootAccount($"{(specifiedReason != null ? $" - {specifiedReason}" : null)}"), null, specifiedReason);
            //CommandHandlerHelper.WriteOutputInfo(session, $"...Result: Success!", ChatMessageType.Broadcast);

            PlayerManager.BroadcastToAuditChannel(session?.Player, bootText);
        }

        // ban < acct > < days > < hours > < minutes >
        [CommandHandler("ban", AccessLevel.Sentinel, CommandHandlerFlag.None, 4,
            "Bans the specified player account.",
            "[accountname] [days] [hours] [minutes] (reason)\n"
            + "This command bans the specified player account for the specified time. This player will not be able to enter the game with any character until the time expires.\n"
            + "Example: @ban AccountName 0 0 5\n"
            + "Example: @ban AccountName 1 0 0 banned 1 day because reasons\n")]
        public static void HandleBanAccount(Session session, params string[] parameters)
        {
            // usage: @ban < acct > < days > < hours > < minutes >
            // This command bans the specified player account for the specified time.This player will not be able to enter the game with any character until the time expires.
            // @ban - Bans the specified player account.

            var accountName = parameters[0];
            var banDays     = parameters[1];
            var banHours    = parameters[2];
            var banMinutes  = parameters[3];

            var banReason = string.Empty;
            if (parameters.Length > 4)
            {
                var parametersAfterBanParams = "";
                for (var i = 4; i < parameters.Length; i++)
                {
                    parametersAfterBanParams += parameters[i] + " ";
                }
                parametersAfterBanParams = parametersAfterBanParams.Trim();
                banReason = parametersAfterBanParams;
            }

            var account = DatabaseManager.Authentication.GetAccountByName(accountName);

            if (account == null)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Cannot ban \"{accountName}\" because that account cannot be found in database. Check syntax/spelling and try again.", ChatMessageType.Broadcast);
                return;
            }

            if (!double.TryParse(banDays, out var days) || days < 0)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Days must not be less than 0.", ChatMessageType.Broadcast);
                return;
            }
            if (!double.TryParse(banHours, out var hours) || hours < 0)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Hours must not be less than 0.", ChatMessageType.Broadcast);
                return;
            }
            if (!double.TryParse(banMinutes, out var minutes) || minutes < 0)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Minutes must not be less than 0.", ChatMessageType.Broadcast);
                return;
            }

            var bannedOn = DateTime.UtcNow;
            var banExpires = DateTime.UtcNow.AddDays(days).AddHours(hours).AddMinutes(minutes);

            var bannedBy = 0u;
            if (session != null)
            {
                bannedBy = session.AccountId;
            }

            account.BannedTime = bannedOn;
            account.BanExpireTime = banExpires;
            account.BannedByAccountId = bannedBy;
            if (!string.IsNullOrWhiteSpace(banReason))
                account.BanReason = banReason;

            DatabaseManager.Authentication.UpdateAccount(account);

            // Boot the player
            if (NetworkManager.Find(accountName) != null)
            {
                var bootArgs = new List<string> { "account" };
                if (!string.IsNullOrWhiteSpace(banReason))
                {
                    bootArgs.Add($"{accountName},");
                    bootArgs.Add(banReason);
                }
                else
                    bootArgs.Add(accountName);
                HandleBoot(session, bootArgs.ToArray());
            }

            var banText = $"Banned account {accountName} for {days} days, {hours} hours and {minutes} minutes.{(!string.IsNullOrWhiteSpace(banReason) ? $" Reason: {banReason}" : "")}";
            CommandHandlerHelper.WriteOutputInfo(session, banText, ChatMessageType.Broadcast);
            PlayerManager.BroadcastToAuditChannel(session?.Player, banText);
        }

        // unban < acct >
        [CommandHandler("unban", AccessLevel.Sentinel, CommandHandlerFlag.None, 1,
            "Unbans the specified player account.",
            "[accountname]\n" +
            "This command removes the ban from the specified account. The player will then be able to log into the game.")]
        public static void HandleUnBanAccount(Session session, params string[] parameters)
        {
            // usage: @unban acct
            // This command removes the ban from the specified account.The player will then be able to log into the game.
            // @unban - Unbans the specified player account.

            var accountName = parameters[0];

            var account = DatabaseManager.Authentication.GetAccountByName(accountName);

            if (account == null)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Cannot unban \"{accountName}\" because that account cannot be found in database. Check spelling and try again.", ChatMessageType.Broadcast);
                return;
            }

            if (account.BanExpireTime == null)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Cannot unban\"{accountName}\" because that account is not banned.", ChatMessageType.Broadcast);
                return;
            }

            account.UnBan();
            var banText = $"UnBanned account {accountName}.";
            CommandHandlerHelper.WriteOutputInfo(session, banText, ChatMessageType.Broadcast);
            PlayerManager.BroadcastToAuditChannel(session?.Player, banText);
        }

        // banlist
        [CommandHandler("banlist", AccessLevel.Sentinel, CommandHandlerFlag.None, 0,
            "Lists all banned accounts on this world.",
            "")]
        public static void HandleBanlist(Session session, params string[] parameters)
        {
            // @banlist - Lists all banned accounts on this world.

            var bannedAccounts = DatabaseManager.Authentication.GetListofBannedAccounts();

            if (bannedAccounts.Count != 0)
            {
                var msg = "The following accounts are banned:\n";
                msg += "-------------------\n";
                foreach (var account in bannedAccounts)
                    msg += account + "\n";

                CommandHandlerHelper.WriteOutputInfo(session, msg, ChatMessageType.Broadcast);
            }
            else
                CommandHandlerHelper.WriteOutputInfo(session, $"There are no accounts currently banned.", ChatMessageType.Broadcast);
        }

        // deaf < on / off >
        [CommandHandler("deaf", AccessLevel.Sentinel, CommandHandlerFlag.RequiresWorld, 1)]
        public static void HandleDeaf(Session session, params string[] parameters)
        {
            // @deaf - Block @tells except for the player you are currently helping.
            // @deaf on -Make yourself deaf to players.
            // @deaf off -You can hear players again.

            // TODO: output
        }

        // deaf < hear | mute > < player >
        [CommandHandler("deaf", AccessLevel.Sentinel, CommandHandlerFlag.RequiresWorld, 2)]
        public static void HandleDeafHearOrMute(Session session, params string[] parameters)
        {
            // @deaf hear[name] -add a player to the list of players that you can hear.
            // @deaf mute[name] -remove a player from the list of players you can hear.

            // TODO: output
        }
    }
}
