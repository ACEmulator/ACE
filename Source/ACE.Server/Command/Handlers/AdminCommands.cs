using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading;

using log4net;

using ACE.Common.Extensions;
using ACE.Database;
using ACE.Database.Models.Auth;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;
using ACE.Server.Entity;
using ACE.Server.Factories;
using ACE.Server.Managers;
using ACE.Server.Network;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.WorldObjects;
using ACE.Server.WorldObjects.Entity;

using Position = ACE.Entity.Position;

namespace ACE.Server.Command.Handlers
{
    public static class AdminCommands
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // // commandname parameters
        // [CommandHandler("commandname", AccessLevel.Admin, CommandHandlerFlag.RequiresWorld, 0)]
        // public static void HandleHelp(Session session, params string[] parameters)
        // {
        //     //TODO: output
        // }

        // adminvision { on | off | toggle | check}
        [CommandHandler("adminvision", AccessLevel.Sentinel, CommandHandlerFlag.RequiresWorld, 1,
            "Allows the admin to see admin-only visible items.", "{ on | off | toggle | check }\n" +
            "Controls whether or not the admin can see admin-only visible items. Note that if you turn this feature off, you will need to log out and back in before the visible items become invisible.")]
        public static void HandleAdminvision(Session session, params string[] parameters)
        {
            // @adminvision { on | off | toggle | check}
            // Controls whether or not the admin can see admin-only visible items. Note that if you turn this feature off, you will need to log out and back in before the visible items become invisible.
            // @adminvision - Allows the admin to see admin - only visible items.

            switch (parameters?[0].ToLower())
            {
                case "1":
                case "on":
                    session.Player.HandleAdminvisionToggle(1);
                    break;
                case "0":
                case "off":
                    session.Player.HandleAdminvisionToggle(0);
                    break;
                case "toggle":
                    session.Player.HandleAdminvisionToggle(2);
                    break;
                case "check":
                default:
                    session.Player.HandleAdminvisionToggle(-1);
                    break;
            }
        }

        // adminui
        [CommandHandler("adminui", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0)]
        public static void HandleAdminui(Session session, params string[] parameters)
        {
            // usage: @adminui
            // This command toggles whether the Admin UI is visible.

            // just a placeholder, probably not needed or should be handled by a decal plugin to replicate the admin ui
        }

        // delete
        [CommandHandler("delete", AccessLevel.Envoy, CommandHandlerFlag.RequiresWorld, 0, "Deletes the selected object.", "Players may not be deleted this way.")]
        public static void HandleDeleteSelected(Session session, params string[] parameters)
        {
            // @delete - Deletes the selected object. Players may not be deleted this way.

            var objectId = ObjectGuid.Invalid;

            if (session.Player.HealthQueryTarget.HasValue)
                objectId = new ObjectGuid(session.Player.HealthQueryTarget.Value);
            else if (session.Player.ManaQueryTarget.HasValue)
                objectId = new ObjectGuid(session.Player.ManaQueryTarget.Value);
            else if (session.Player.CurrentAppraisalTarget.HasValue)
                objectId = new ObjectGuid(session.Player.CurrentAppraisalTarget.Value);

            if (objectId == ObjectGuid.Invalid)
                ChatPacket.SendServerMessage(session, "Delete failed. Please identify the object you wish to delete first.", ChatMessageType.Broadcast);

            if (objectId.IsPlayer())
            {
                ChatPacket.SendServerMessage(session, "Delete failed. Players cannot be deleted.", ChatMessageType.Broadcast);
                return;
            }

            var wo = session.Player.FindObject(objectId.Full, Player.SearchLocations.Everywhere, out _, out Container rootOwner, out bool wasEquipped);

            if (wo == null)
            {
                ChatPacket.SendServerMessage(session, "Delete failed. Object not found.", ChatMessageType.Broadcast);
                return;
            }

            if (parameters.Length == 1)
            {
                var objectType = parameters[0].ToLower();

                if (objectType != wo.GetType().Name.ToLower() && objectType != wo.WeenieType.ToString().ToLower())
                {
                    ChatPacket.SendServerMessage(session, $"Delete failed. Object type specified ({parameters[0]}) does not match object type ({wo.GetType().Name}) or weenie type ({wo.WeenieType.ToString()}) for 0x{wo.Guid}:{wo.Name}.", ChatMessageType.Broadcast);
                    return;
                }
            }

            wo.DeleteObject(rootOwner);
            session.Network.EnqueueSend(new GameMessageDeleteObject(wo));

            PlayerManager.BroadcastToAuditChannel(session.Player, $"{session.Player.Name} has deleted 0x{wo.Guid}:{wo.Name}");
        }

        // draw
        [CommandHandler("draw", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0)]
        public static void HandleDraw(Session session, params string[] parameters)
        {
            // @draw - Draws undrawable things.

            // TODO: output
        }

        // finger [ [-a] character] [-m account]
        [CommandHandler("finger", AccessLevel.Sentinel, CommandHandlerFlag.None, 1,
            "Show the given character's account name or vice-versa.",
            "[ [-a] character] [-m account]\n"
            + "Given a character name, this command displays the name of the owning account.\nIf the -m option is specified, the argument is considered an account name and the characters owned by that account are displayed.\nIf the -a option is specified, then the character name is fingered but their account is implicitly fingered as well.")]
        public static void HandleFinger(Session session, params string[] parameters)
        {
            // usage: @finger[ [-a] character] [-m account]
            // Given a character name, this command displays the name of the owning account.If the -m option is specified, the argument is considered an account name and the characters owned by that account are displayed.If the -a option is specified, then the character name is fingered but their account is implicitly fingered as well.
            // @finger - Show the given character's account name or vice-versa.

            var lookupCharAndAccount = parameters.Contains("-a");
            var lookupByAccount = parameters.Contains("-m");

            var charName = "";
            if (lookupByAccount || lookupCharAndAccount)
                charName = string.Join(" ", parameters.Skip(1));
            else
                charName = string.Join(" ", parameters);

            var message = "";
            if (!lookupByAccount && !lookupCharAndAccount)
            {
                var character = PlayerManager.FindByName(charName);

                if (character != null)
                {
                    if (character.Account != null)
                        message = $"Login name: {character.Account.AccountName}      Character: {character.Name}\n";
                    else
                        message = $"Login name: account not found, character is orphaned.      Character: {character.Name}\n";
                }
                else
                    message = $"There was no active character named \"{charName}\" found in the database.\n";
            }
            else
            {
                Account account;
                if (lookupCharAndAccount)
                {
                    var character = PlayerManager.FindByName(charName);

                    if (character == null)
                    {
                        message = $"There was no active character named \"{charName}\" found in the database.\n";
                        CommandHandlerHelper.WriteOutputInfo(session, message, ChatMessageType.WorldBroadcast);
                        return;
                    }
                    account = character.Account;
                    if (account == null)
                    {
                        message = $"Login name: account not found, character is orphaned.      Character: {character.Name}\n";
                        CommandHandlerHelper.WriteOutputInfo(session, message, ChatMessageType.WorldBroadcast);
                        return;
                    }
                    else // get updated data from db.
                        account = DatabaseManager.Authentication.GetAccountById(account.AccountId);
                }
                else
                    account = DatabaseManager.Authentication.GetAccountByName(charName);

                if (account != null)
                {
                    if (account.BannedTime != null)
                    {
                        var bannedbyAccount = account.BannedByAccountId > 0 ? $"account {DatabaseManager.Authentication.GetAccountById(account.BannedByAccountId.Value).AccountName}" : "CONSOLE";

                        message = $"Account '{account.AccountName}' was banned by {bannedbyAccount} until server time {account.BanExpireTime.Value.ToLocalTime():MMM dd yyyy  h:mmtt}.\n";
                    }
                    else
                        message = $"Account '{account.AccountName}' is not banned.\n";
                    if (account.AccessLevel > (int)AccessLevel.Player)
                        message += $"Account '{account.AccountName}' has been granted AccessLevel.{((AccessLevel)account.AccessLevel).ToString()} rights.\n";
                    message += $"Account created on {account.CreateTime.ToLocalTime()} by IP: {(account.CreateIP != null ? new IPAddress(account.CreateIP).ToString() : "N/A")} \n";
                    message += $"Account last logged on at {(account.LastLoginTime.HasValue ? account.LastLoginTime.Value.ToLocalTime().ToString() : "N/A")} by IP: {(account.LastLoginIP != null ? new IPAddress(account.LastLoginIP).ToString() : "N/A")}\n";
                    message += $"Account total times logged on {account.TotalTimesLoggedIn}\n";
                    var characters = DatabaseManager.Shard.BaseDatabase.GetCharacters(account.AccountId, true);
                    message += $"{characters.Count} Character(s) owned by: {account.AccountName}\n";
                    message += "-------------------\n";
                    foreach (var character in characters.Where(x => !x.IsDeleted && x.DeleteTime == 0))
                        message += $"\"{(character.IsPlussed ? "+" : "")}{character.Name}\", ID 0x{character.Id.ToString("X8")}\n";
                    var pendingDeletedCharacters = characters.Where(x => !x.IsDeleted && x.DeleteTime > 0).ToList();
                    if (pendingDeletedCharacters.Count > 0)
                    {
                        message += "-------------------\n";
                        foreach (var character in pendingDeletedCharacters)
                            message += $"\"{(character.IsPlussed ? "+" : "")}{character.Name}\", ID 0x{character.Id.ToString("X8")} -- Will be deleted at server time {new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(character.DeleteTime).ToLocalTime().ToString("MMM d yyyy h:mm tt")}\n";
                    }
                    message += "-------------------\n";
                    var deletedCharacters = characters.Where(x => x.IsDeleted).ToList();
                    if (deletedCharacters.Count > 0)
                    {
                        foreach (var character in deletedCharacters)
                            message += $"\"{(character.IsPlussed ? "+" : "")}{character.Name}\", ID 0x{character.Id.ToString("X8")} -- Deleted at server time {new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(character.DeleteTime).ToLocalTime().ToString("MMM d yyyy h:mm tt")}\n";
                    }
                    else
                        message += "No deleted characters.\n";
                }
                else
                    message = $"There was no account named \"{charName}\" found in the database.\n";
            }

            CommandHandlerHelper.WriteOutputInfo(session, message, ChatMessageType.WorldBroadcast);
        }

        // freeze
        [CommandHandler("freeze", AccessLevel.Sentinel, CommandHandlerFlag.RequiresWorld, 0)]
        public static void HandleFreeze(Session session, params string[] parameters)
        {
            // @freeze - Freezes the selected target for 10 minutes or until unfrozen.

            // TODO: output
        }

        // unfreeze
        [CommandHandler("unfreeze", AccessLevel.Sentinel, CommandHandlerFlag.RequiresWorld, 0)]
        public static void HandleUnFreeze(Session session, params string[] parameters)
        {
            // @unfreeze - Unfreezes the selected target.

            // TODO: output
        }

        // gag < char name >
        [CommandHandler("gag", AccessLevel.Sentinel, CommandHandlerFlag.RequiresWorld, 1,
            "Prevents a character from talking.",
            "< char name >\nThe character will not be able to @tell or use chat normally.")]
        public static void HandleGag(Session session, params string[] parameters)
        {
            // usage: @gag < char name >
            // This command gags the specified character for five minutes.  The character will not be able to @tell or use chat normally.
            // @gag - Prevents a character from talking.
            // @ungag -Allows a gagged character to talk again.

            if (parameters.Length > 0)
            {
                var playerName = string.Join(" ", parameters);

                var msg = "";
                if (PlayerManager.GagPlayer(session.Player, playerName))
                {
                    msg = $"{playerName} has been gagged for five minutes.";
                }
                else
                {
                    msg = $"Unable to gag a character named {playerName}, check the name and re-try the command.";
                }

                CommandHandlerHelper.WriteOutputInfo(session, msg, ChatMessageType.WorldBroadcast);
            }
        }

        // ungag < char name >
        [CommandHandler("ungag", AccessLevel.Sentinel, CommandHandlerFlag.RequiresWorld, 1,
            "Allows a gagged character to talk again.",
            "< char name >\nThe character will again be able to @tell and use chat normally.")]
        public static void HandleUnGag(Session session, params string[] parameters)
        {
            // usage: @ungag < char name >
            // @ungag -Allows a gagged character to talk again.

            if (parameters.Length > 0)
            {
                var playerName = string.Join(" ", parameters);

                var msg = "";
                if (PlayerManager.UnGagPlayer(session.Player, playerName))
                {
                    msg = $"{playerName} has been ungagged.";
                }
                else
                {
                    msg = $"Unable to ungag a character named {playerName}, check the name and re-try the command.";
                }

                CommandHandlerHelper.WriteOutputInfo(session, msg, ChatMessageType.WorldBroadcast);
            }
        }

        /// <summary>
        /// Teleports an admin to their sanctuary position. If a single uint value from 1 to 9 is provided as a parameter then the admin is teleported to the cooresponding named recall point.
        /// </summary>
        /// <param name="parameters">A single uint value from 0 to 9. Value 0 recalls to Sanctuary, values 1 through 9 teleports too the corresponding saved recall point.</param>
        [CommandHandler("home", AccessLevel.Sentinel, CommandHandlerFlag.RequiresWorld, 0,
            "Teleports you to your sanctuary position.",
            "< recall number > - Recalls to a saved position, valid values are 1 - 9.\n" +
            "NOTE: Calling @home without a number recalls your sanctuary position; calling it with a number will teleport you to the corresponding saved position.")]
        public static void HandleHome(Session session, params string[] parameters)
        {
            // @home has the alias @recall
            string parsePositionString = "0";

            // Limit the incoming parameter to 1 character
            if (parameters?.Length >= 1)
                parsePositionString = parameters[0].Length > 1 ? parameters[0].Substring(0, 1) : parameters[0];

            // Attempt to parse the integer
            if (uint.TryParse(parsePositionString, out var parsedPositionInt))
            {
                // parsedPositionInt value should be limited too a value from, 0-9
                // Create a new position from the current player location
                PositionType positionType = new PositionType();
                // Transform too the correct PositionType, based on the "Saved Positions" subset:
                switch (parsedPositionInt)
                {
                    case 0:
                        {
                            positionType = PositionType.Sanctuary;
                            break;
                        }
                    case 1:
                        {
                            positionType = PositionType.Save1;
                            break;
                        }
                    case 2:
                        {
                            positionType = PositionType.Save2;
                            break;
                        }
                    case 3:
                        {
                            positionType = PositionType.Save3;
                            break;
                        }
                    case 4:
                        {
                            positionType = PositionType.Save4;
                            break;
                        }
                    case 5:
                        {
                            positionType = PositionType.Save5;
                            break;
                        }
                    case 6:
                        {
                            positionType = PositionType.Save6;
                            break;
                        }
                    case 7:
                        {
                            positionType = PositionType.Save7;
                            break;
                        }
                    case 8:
                        {
                            positionType = PositionType.Save8;
                            break;
                        }
                    case 9:
                        {
                            positionType = PositionType.Save9;
                            break;
                        }
                }

                // If we have the position, teleport the player
                var position = session.Player.GetPosition(positionType);
                if (position != null)
                {
                    session.Player.TeleToPosition(positionType);
                    var positionMessage = new GameMessageSystemChat($"Recalling to {positionType}", ChatMessageType.Broadcast);
                    session.Network.EnqueueSend(positionMessage);
                    return;
                }
            }
            // Invalid character was receieved in the input (it was not 0-9)
            var homeErrorMessage = new GameMessageSystemChat("Could not find a valid recall position.", ChatMessageType.Broadcast);
            session.Network.EnqueueSend(homeErrorMessage);
        }

        // mrt
        [CommandHandler("mrt", AccessLevel.Sentinel, CommandHandlerFlag.RequiresWorld, 0,
            "Toggles the ability to bypass housing boundaries",
            "")]
        public static void HandleMRT(Session session, params string[] parameters)
        {
            // @mrt - Toggles the ability to bypass housing boundaries.
            session.Player.HandleMRT();
        }

        // limbo [on / off]
        [CommandHandler("limbo", AccessLevel.Sentinel, CommandHandlerFlag.RequiresWorld, 0)]
        public static void HandleLimbo(Session session, params string[] parameters)
        {
            // @limbo[on / off] - Puts the targeted player in 'limbo' which means that the player cannot damage anything or be damaged by anything.The player will not recieve direct tells, or channel messages, such as fellowship messages and allegiance chat.  The player will be unable to salvage.This status times out after 15 minutes, use '@limbo on' again on the player to reset the timer. You and the player will be notifed when limbo wears off.If neither on or off are specified, on is assumed.
            // @limbo - Puts the selected target in limbo.

            // TODO: output
        }

        // myiid
        [CommandHandler("myiid", AccessLevel.Envoy, CommandHandlerFlag.RequiresWorld, 0,
            "Displays your Instance ID (IID)")]
        public static void HandleMyIID(Session session, params string[] parameters)
        {
            // @myiid - Displays your Instance ID(IID).

            session.Network.EnqueueSend(new GameMessageSystemChat($"GUID: {session.Player.Guid.Full}  - Low: {session.Player.Guid.Low} - High: {session.Player.Guid.High} - (0x{session.Player.Guid.Full:X})", ChatMessageType.Broadcast));
        }

        // myserver
        [CommandHandler("myserver", AccessLevel.Envoy, CommandHandlerFlag.RequiresWorld, 0)]
        public static void HandleMyServer(Session session, params string[] parameters)
        {
            // @myserver - Displays the number of the game server on which you are currently located.

            // TODO: output
        }

        // pk
        [CommandHandler("pk", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0,
            "sets your own PK state.",
            "< npk / pk / pkl / free >\n" +
            "This command sets your current player killer state\n" +
            "This command also expects to be used '@cloak player' to properly reflect to other players\n" +
            "< npk > You can only attack monsters.\n" +
            "< pk > You can attack player killers and monsters.\n" +
            "< pkl > You can attack player killer lites and monsters\n" +
            "< free > You can attack players, player killers, player killer lites, monsters, and npcs")]
        public static void HandlePk(Session session, params string[] parameters)
        {
            // @pk - Toggles or sets your own PK state.

            if (parameters.Length == 0)
            {
                //player.EnqueueBroadcast(new GameMessagePublicUpdatePropertyInt(player, PropertyInt.PlayerKillerStatus, (int)player.PlayerKillerStatus));
                var message = $"Your current PK state is: {session.Player.PlayerKillerStatus.ToString()}\n"
                    + "You can change it to the following:\n"
                    + "NPK      = Non-Player Killer\n"
                    + "PK       = Player Killer\n"
                    + "PKL      = Player Killer Lite\n"
                    + "Free     = Can kill anything\n";
                CommandHandlerHelper.WriteOutputInfo(session, message, ChatMessageType.Broadcast);
            }
            else
            {
                switch (parameters[0].ToLower())
                {
                    case "npk":
                        session.Player.PlayerKillerStatus = PlayerKillerStatus.NPK;
                        session.Player.PkLevel = PKLevel.NPK;
                        break;
                    case "pk":
                        session.Player.PlayerKillerStatus = PlayerKillerStatus.PK;
                        session.Player.PkLevel = PKLevel.PK;
                        break;
                    case "pkl":
                        session.Player.PlayerKillerStatus = PlayerKillerStatus.PKLite;
                        session.Player.PkLevel = PKLevel.NPK;
                        break;
                    case "free":
                        session.Player.PlayerKillerStatus = PlayerKillerStatus.Free;
                        session.Player.PkLevel = PKLevel.Free;
                        break;
                }
                session.Player.EnqueueBroadcast(new GameMessagePublicUpdatePropertyInt(session.Player, PropertyInt.PlayerKillerStatus, (int)session.Player.PlayerKillerStatus));
                CommandHandlerHelper.WriteOutputInfo(session, $"Your current PK state is now set to: {session.Player.PlayerKillerStatus.ToString()}", ChatMessageType.Broadcast);

                PlayerManager.BroadcastToAuditChannel(session.Player, $"{session.Player.Name} changed their PK state to {session.Player.PlayerKillerStatus.ToString()}.");
            }
        }

        // querypluginlist
        [CommandHandler("querypluginlist", AccessLevel.Envoy, CommandHandlerFlag.RequiresWorld, 0)]
        public static void HandleQuerypluginlist(Session session, params string[] parameters)
        {
            // @querypluginlist (-fresh) - View list of plug-ins the selected character is running. If you do not use the -fresh paramater, you will get results that where cached at login. That way, you will not be sending an alert to the player that you are querying thier plugin list.  If you use -fresh, then you will get fresh data from the player's client and they will recieve notification that you have asked for thier plugin list.NOTE: Results are dependent upon 3rd party authors providing correct information.
            // @querypluginlist - View list of plug - ins the selected character is running.
            // @querypluginlist<pluginname> - View information about a specific plugin.NOTE: Results are dependent upon 3rd party authors providing correct information.
            // @queryplugin < pluginname > -View information about a specific plugin.

            // TODO: output
        }

        // queryplugin
        [CommandHandler("queryplugin", AccessLevel.Envoy, CommandHandlerFlag.RequiresWorld, 1)]
        public static void HandleQueryplugin(Session session, params string[] parameters)
        {
            // @queryplugin < pluginname > -View information about a specific plugin.

            // TODO: output
        }

        // repeat < Num > < Command >
        [CommandHandler("repeat", AccessLevel.Sentinel, CommandHandlerFlag.RequiresWorld, 2)]
        public static void HandleRepeat(Session session, params string[] parameters)
        {
            // @repeat < Num > < Command > -Repeat a command a number of times.
            // EX: "@repeat 5 @say Hi" - say Hi 5 times.
            // @repeat < Num > < Command > -Repeat a command a number of times.

            // TODO: output
        }

        // regen
        [CommandHandler("regen", AccessLevel.Envoy, CommandHandlerFlag.RequiresWorld, 0,
            "Sends the selected generator a regeneration message.",
            "")]
        public static void HandleRegen(Session session, params string[] parameters)
        {
            // @regen - Sends the selected generator a regeneration message.

            var objectId = new ObjectGuid();

            if (session.Player.HealthQueryTarget.HasValue || session.Player.ManaQueryTarget.HasValue || session.Player.CurrentAppraisalTarget.HasValue)
            {
                if (session.Player.HealthQueryTarget.HasValue)
                    objectId = new ObjectGuid((uint)session.Player.HealthQueryTarget);
                else if (session.Player.HealthQueryTarget.HasValue)
                    objectId = new ObjectGuid((uint)session.Player.ManaQueryTarget);
                else
                    objectId = new ObjectGuid((uint)session.Player.CurrentAppraisalTarget);

                var wo = session.Player.CurrentLandblock?.GetObject(objectId);

                if (objectId.IsPlayer())
                    return;

                if (wo != null & wo.IsGenerator)
                {
                    wo.ResetGenerator();
                    wo.GeneratorEnteredWorld = false;
                }
            }
        }

        // reportbug < code | content > < description >
        [CommandHandler("reportbug", AccessLevel.Envoy, CommandHandlerFlag.RequiresWorld, 2)]
        public static void HandleReportbug(Session session, params string[] parameters)
        {
            // @reportbug < code | content > < description >
            // This command emails your report to the AC1 Bugs folder at Turbine.
            // @reportbug - Email a bug report to Turbine.

            // LOL

            // TODO: output
        }

        /// <summary>
        /// Command for saving the Admin's current location as the sanctuary position. If a uint between 1-9 is provided as a parameter, the corresponding named recall is saved.
        /// </summary>
        /// <param name="parameters">A single uint value from 0 to 9. Value 0 saves the Sanctuary recall (default), values 1 through 9 save the corresponding named recall point.</param>
        [CommandHandler("save", AccessLevel.Sentinel, CommandHandlerFlag.RequiresWorld, 0,
            "Sets your sanctuary position or a named recall point.",
            "< recall number > - Saves your position into the numbered recall, valid values are 1 - 9.\n" +
            "NOTE: Calling @save without a number saves your sanctuary (Lifestone Recall) position.")]
        public static void HandleSave(Session session, params string[] parameters)
        {
            // Set the default of 0 to save the sanctuary portal if no parameter is passed.
            string parsePositionString = "0";

            // Limit the incoming parameter to 1 character
            if (parameters?.Length >= 1)
                parsePositionString = parameters[0].Length > 1 ? parameters[0].Substring(0, 1) : parameters[0];

            // Attempt to parse the integer
            if (uint.TryParse(parsePositionString, out var parsedPositionInt))
            {
                // parsedPositionInt value should be limited too a value from, 0-9
                // Create a new position from the current player location
                Position playerPosition = session.Player.Location;
                PositionType positionType = PositionType.Sanctuary;
                // Set the correct PositionType, based on the "Saved Positions" position type subset:
                switch (parsedPositionInt)
                {
                    case 0:
                        {
                            positionType = PositionType.Sanctuary;
                            break;
                        }
                    case 1:
                        {
                            positionType = PositionType.Save1;
                            break;
                        }
                    case 2:
                        {
                            positionType = PositionType.Save2;
                            break;
                        }
                    case 3:
                        {
                            positionType = PositionType.Save3;
                            break;
                        }
                    case 4:
                        {
                            positionType = PositionType.Save4;
                            break;
                        }
                    case 5:
                        {
                            positionType = PositionType.Save5;
                            break;
                        }
                    case 6:
                        {
                            positionType = PositionType.Save6;
                            break;
                        }
                    case 7:
                        {
                            positionType = PositionType.Save7;
                            break;
                        }
                    case 8:
                        {
                            positionType = PositionType.Save8;
                            break;
                        }
                    case 9:
                        {
                            positionType = PositionType.Save9;
                            break;
                        }
                }

                // Save the position
                session.Player.SetPosition(positionType, new Position(playerPosition));
                // Report changes to client
                var positionMessage = new GameMessageSystemChat($"Set: {positionType} to Loc: {playerPosition}", ChatMessageType.Broadcast);
                session.Network.EnqueueSend(positionMessage);
                return;
            }
            // Error parsing the text input, from parameter[0]
            var positionErrorMessage = new GameMessageSystemChat("Could not determine the correct PositionType. Please use an integer value from 1 to 9; or omit the parmeter entirely.", ChatMessageType.Broadcast);
            session.Network.EnqueueSend(positionErrorMessage);
        }

        // serverlist
        [CommandHandler("serverlist", AccessLevel.Envoy, CommandHandlerFlag.RequiresWorld, 0)]
        public static void HandleServerlist(Session session, params string[] parameters)
        {
            // The @serverlist command shows a list of the servers in the current server farm. The format is as follows:
            // -The server ID
            // -The server's speed relative to the master server
            // - Number of users reported by ObjLoc and LoadBalancing
            // - Current total load reported by LoadBalancing
            // - Total load from blocks with no players in them
            // - Blocks with players / blocks loaded / blocks owned
            // - The owned block range from low to high(in hex)
            // - The external IP address to talk to clients on
            // -If the server is your current server or the master
            // @serverlist - Shows a list of the logical servers that control this world.

            // TODO: output
        }

        // snoop [start / stop] [Character Name]
        [CommandHandler("snoop", AccessLevel.Envoy, CommandHandlerFlag.RequiresWorld, 2)]
        public static void HandleSnoop(Session session, params string[] parameters)
        {
            // @snoop[start / stop][Character Name]
            // - If no character name is supplied, the currently selected character will be used.If neither start nor stop is specified, start will be assumed.
            // @snoop - Listen in on a player's private communication.

            // TODO: output
        }

        [CommandHandler("smite", AccessLevel.Envoy, CommandHandlerFlag.RequiresWorld, 0, "Kills the selected target or all monsters in radar range if \"all\" is specified.", "[all, Player's Name]")]
        public static void HandleSmite(Session session, params string[] parameters)
        {
            // @smite [all] - Kills the selected target or all monsters in radar range if "all" is specified.

            if (parameters?.Length > 0)
            {
                if (parameters[0] == "all")
                {
                    foreach (var obj in session.Player.PhysicsObj.ObjMaint.GetVisibleObjectsValues())
                    {
                        var wo = obj.WeenieObj.WorldObject;

                        if (wo is Player) // I don't recall if @smite all would kill players in range, assuming it didn't
                            continue;

                        var useTakeDamage = PropertyManager.GetBool("smite_uses_takedamage").Item;

                        if (wo is Creature creature && creature.Attackable)
                            creature.Smite(session.Player, useTakeDamage);
                    }

                    PlayerManager.BroadcastToAuditChannel(session.Player, $"{session.Player.Name} used smite all.");
                }
                else
                {
                    string characterName = "";

                    // if parameters are greater then 1, we may have a space in a character name
                    if (parameters.Length > 1)
                    {
                        foreach (string name in parameters)
                        {
                            // adds a space back inbetween each parameter
                            if (characterName.Length > 0)
                                characterName += " " + name;
                            else
                                characterName = name;
                        }
                    }
                    // if there are no spaces, just set the characterName to the first paramter
                    else
                        characterName = parameters[0];

                    // look up session
                    var player = PlayerManager.GetOnlinePlayer(characterName);

                    // playerSession will be null when the character is not found
                    if (player != null)
                    {
                        player.Smite(session.Player, PropertyManager.GetBool("smite_uses_takedamage").Item);

                        PlayerManager.BroadcastToAuditChannel(session.Player, $"{session.Player.Name} used smite on {player.Name}");
                        return;
                    }

                    ChatPacket.SendServerMessage(session, "Select a target and use @smite, or use @smite all to kill all creatures in radar range or @smite [players' name].", ChatMessageType.Broadcast);
                }
            }
            else
            {
                if (session.Player.HealthQueryTarget.HasValue) // Only Creatures will trigger this.. Excludes vendors automatically as a result (Can change design to mimic @delete command)
                {
                    var objectId = new ObjectGuid((uint)session.Player.HealthQueryTarget);

                    var wo = session.Player.CurrentLandblock?.GetObject(objectId) as Creature;

                    if (objectId == session.Player.Guid) // don't kill yourself
                        return;

                    if (wo != null)
                        wo.Smite(session.Player, PropertyManager.GetBool("smite_uses_takedamage").Item);

                    PlayerManager.BroadcastToAuditChannel(session.Player, $"{session.Player.Name} used smite on {wo.Name} (0x{wo.Guid:X8})");
                }
                else
                {
                    ChatPacket.SendServerMessage(session, "Select a target and use @smite, or use @smite all to kill all creatures in radar range or @smite [players' name].", ChatMessageType.Broadcast);
                }
            }
        }

        // teleto [char]
        [CommandHandler("teleto", AccessLevel.Sentinel, CommandHandlerFlag.RequiresWorld, 1,
            "Teleport yourself to a player",
            "[Player's Name]\n")]
        public static void HandleTeleto(Session session, params string[] parameters)
        {
            // @teleto - Teleports you to the specified character.
            var playerName = string.Join(" ", parameters);
            // Lookup the player in the world
            var player = PlayerManager.GetOnlinePlayer(playerName);
            // If the player is found, teleport the admin to the Player's location
            if (player != null)
                session.Player.Teleport(player.Location);
            else
                session.Network.EnqueueSend(new GameMessageSystemChat($"Player {playerName} was not found.", ChatMessageType.Broadcast));
        }

        /// <summary>
        /// Teleports a player to your current location
        /// </summary>
        [CommandHandler("teletome", AccessLevel.Sentinel, CommandHandlerFlag.RequiresWorld, 1, "Teleports a player to your current location.", "PlayerName")]
        public static void HandleTeleToMe(Session session, params string[] parameters)
        {
            var playerName = string.Join(" ", parameters);
            var player = PlayerManager.GetOnlinePlayer(playerName);
            if (player == null)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Player {playerName} was not found.", ChatMessageType.Broadcast));
                return;
            }
            var currentPos = new Position(player.Location);
            player.Teleport(session.Player.Location);
            player.SetPosition(PositionType.TeleportedCharacter, currentPos);
            player.Session.Network.EnqueueSend(new GameMessageSystemChat($"{session.Player.Name} has teleported you.", ChatMessageType.Magic));

            PlayerManager.BroadcastToAuditChannel(session.Player, $"{session.Player.Name} has teleported {player.Name} to them.");
        }

        /// <summary>
        /// Teleports a player to their previous position
        /// </summary>
        [CommandHandler("telereturn", AccessLevel.Sentinel, CommandHandlerFlag.RequiresWorld, 1, "Return a player to their previous location.", "PlayerName")]
        public static void HandleTeleReturn(Session session, params string[] parameters)
        {
            var playerName = string.Join(" ", parameters);
            var player = PlayerManager.GetOnlinePlayer(playerName);
            if (player == null)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Player {playerName} was not found.", ChatMessageType.Broadcast));
                return;
            }

            if (player.TeleportedCharacter == null)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Player {playerName} does not have a return position saved.", ChatMessageType.Broadcast));
                return;
            }

            player.Teleport(new Position(player.TeleportedCharacter));
            player.SetPosition(PositionType.TeleportedCharacter, null);
            player.Session.Network.EnqueueSend(new GameMessageSystemChat($"{session.Player.Name} has returned you to your previous location.", ChatMessageType.Magic));

            PlayerManager.BroadcastToAuditChannel(session.Player, $"{session.Player.Name} has returned {player.Name} to their previous location.");
        }

        // teleallto [char]
        [CommandHandler("teleallto", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0, "Teleports all players to a player. If no target is specified, all players will be teleported to you.", "[Player's Name]\n")]
        public static void HandleTeleAllTo(Session session, params string[] parameters)
        {
            Player destinationPlayer = null;

            if (parameters.Length > 0)
                destinationPlayer = PlayerManager.GetOnlinePlayer(parameters[0]);

            if (destinationPlayer == null)
                destinationPlayer = session.Player;

            foreach (var player in PlayerManager.GetAllOnline())
            {
                if (player == destinationPlayer)
                    continue;

                player.SetPosition(PositionType.TeleportedCharacter, new Position(player.Location));

                player.Teleport(new Position(destinationPlayer.Location));
            }

            PlayerManager.BroadcastToAuditChannel(session.Player, $"{session.Player.Name} has teleported all online players to their location.");
        }

        // telepoi location
        [CommandHandler("telepoi", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1,
            "Teleport yourself to a named Point of Interest",
            "[POI|list]\n" +
            "@telepoi Arwic\n" +
            "Get the list of POIs\n" +
            "@telepoi list")]
        public static void HandleTeleportPoi(Session session, params string[] parameters)
        {
            var poi = String.Join(" ", parameters);

            if (poi.ToLower() == "list")
            {
                DatabaseManager.World.CacheAllPointsOfInterest();
                var pois = DatabaseManager.World.GetPointsOfInterestCache();
                var list = pois
                    .Select(k => k.Key)
                    .OrderBy(k => k)
                    .DefaultIfEmpty()
                    .Aggregate((a, b) => a + ", " + b);
                session.Network.EnqueueSend(new GameMessageSystemChat($"All POIs: {list}", ChatMessageType.Broadcast));
            }
            else
            {
                var teleportPOI = DatabaseManager.World.GetCachedPointOfInterest(poi);
                if (teleportPOI == null)
                    return;
                var weenie = DatabaseManager.World.GetCachedWeenie(teleportPOI.WeenieClassId);
                var portalDest = new Position(weenie.GetPosition(PositionType.Destination));
                WorldObject.AdjustDungeon(portalDest);
                session.Player.Teleport(portalDest);
            }
        }

        // teleloc cell x y z [qx qy qz qw]
        [CommandHandler("teleloc", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 4,
            "Teleport yourself to the specified location.",
            "cell [x y z] (qw qx qy qz)\n" +
            "@teleloc follows the same number order as displayed from @loc output\n" +
            "Example: @teleloc 0x7F0401AD [12.319900 -28.482000 0.005000] -0.338946 0.000000 0.000000 -0.940806\n" +
            "Example: @teleloc 0x7F0401AD 12.319900 -28.482000 0.005000 -0.338946 0.000000 0.000000 -0.940806\n" +
            "Example: @teleloc 7F0401AD 12.319900 - 28.482000 0.005000")]
        public static void HandleTeleportLOC(Session session, params string[] parameters)
        {
            try
            {
                uint cell;

                if (parameters[0].StartsWith("0x"))
                {
                    string strippedcell = parameters[0].Substring(2);
                    cell = (uint)int.Parse(strippedcell, System.Globalization.NumberStyles.HexNumber);
                }
                else
                    cell = (uint)int.Parse(parameters[0], System.Globalization.NumberStyles.HexNumber);

                var positionData = new float[7];
                for (uint i = 0u; i < 7u; i++)
                {
                    if (!float.TryParse(parameters[i + 1].Trim(new Char[] { ' ', '[', ']' }), out var position))
                        return;

                    positionData[i] = position;
                }

                session.Player.Teleport(new Position(cell, positionData[0], positionData[1], positionData[2], positionData[4], positionData[5], positionData[6], positionData[3]));
            }
            catch (Exception)
            {
                ChatPacket.SendServerMessage(session, "Invalid arguments for @teleloc", ChatMessageType.Broadcast);
                ChatPacket.SendServerMessage(session, "Hint: @teleloc follows the same number order as displayed from @loc output", ChatMessageType.Broadcast);
                ChatPacket.SendServerMessage(session, "Usage: @teleloc cell [x y z] (qw qx qy qz)", ChatMessageType.Broadcast);
                ChatPacket.SendServerMessage(session, "Example: @teleloc 0x7F0401AD [12.319900 -28.482000 0.005000] -0.338946 0.000000 0.000000 -0.940806", ChatMessageType.Broadcast);
                ChatPacket.SendServerMessage(session, "Example: @teleloc 0x7F0401AD 12.319900 -28.482000 0.005000 -0.338946 0.000000 0.000000 -0.940806", ChatMessageType.Broadcast);
                ChatPacket.SendServerMessage(session, "Example: @teleloc 7F0401AD 12.319900 -28.482000 0.005000", ChatMessageType.Broadcast);
            }
        }

        // time
        [CommandHandler("time", AccessLevel.Envoy, CommandHandlerFlag.None, 0,
            "Displays the server's current game time.")]
        public static void HandleTime(Session session, params string[] parameters)
        {
            // @time - Displays the server's current game time.

            var messageUTC = "The current server time in UtcNow is: " + DateTime.UtcNow;
            //var messagePY = "The current server time translated to DerethDateTime is:\n" + Timers.CurrentLoreTime;
            var messageIGPY = "The current server time shown in game client is:\n" + Timers.CurrentInGameTime;
            var messageTOD = $"It is currently {Timers.CurrentInGameTime.TimeOfDay} in game right now.";

            CommandHandlerHelper.WriteOutputInfo(session, messageUTC, ChatMessageType.WorldBroadcast);
            //CommandHandlerHelper.WriteOutputInfo(session, messagePY, ChatMessageType.WorldBroadcast);
            CommandHandlerHelper.WriteOutputInfo(session, messageIGPY, ChatMessageType.WorldBroadcast);
            CommandHandlerHelper.WriteOutputInfo(session, messageTOD, ChatMessageType.WorldBroadcast);
        }

        // trophies
        [CommandHandler("trophies", AccessLevel.Envoy, CommandHandlerFlag.RequiresWorld, 0,
            "Shows a list of the trophies dropped by the target creature, and the percentage chance of dropping.",
            "")]
        public static void HandleTrophies(Session session, params string[] parameters)
        {
            // @trophies - Shows a list of the trophies dropped by the target creature, and the percentage chance of dropping.

            var objectId = new ObjectGuid();

            if (session.Player.HealthQueryTarget.HasValue || session.Player.ManaQueryTarget.HasValue || session.Player.CurrentAppraisalTarget.HasValue)
            {
                if (session.Player.HealthQueryTarget.HasValue)
                    objectId = new ObjectGuid((uint)session.Player.HealthQueryTarget);
                else if (session.Player.HealthQueryTarget.HasValue)
                    objectId = new ObjectGuid((uint)session.Player.ManaQueryTarget);
                else
                    objectId = new ObjectGuid((uint)session.Player.CurrentAppraisalTarget);

                var wo = session.Player.CurrentLandblock?.GetObject(objectId);

                if (objectId.IsPlayer())
                    return;

                var msg = "";
                if (wo is Creature creature && wo.Biota.PropertiesCreateList != null && wo.Biota.PropertiesCreateList.Count > 0)
                {
                    var createList = creature.Biota.PropertiesCreateList.Where(i => (i.DestinationType & DestinationType.Contain) != 0 ||
                        (i.DestinationType & DestinationType.Treasure) != 0 && (i.DestinationType & DestinationType.Wield) == 0).ToList();

                    var wieldedTreasure = creature.Inventory.Values.Concat(creature.EquippedObjects.Values).Where(i => i.DestinationType.HasFlag(DestinationType.Treasure)).ToList();

                    msg = $"Trophy Dump for {creature.Name} (0x{creature.Guid})\n";
                    msg += $"WCID: {creature.WeenieClassId}\n";
                    msg += $"WeenieClassName: {creature.WeenieClassName}\n";

                    if (createList.Count > 0)
                    {
                        foreach (var item in createList)
                        {
                            if (item.WeenieClassId == 0)
                            {
                                msg += $"{((DestinationType)item.DestinationType).ToString()}: {item.Shade,7:P2} - {item.WeenieClassId,5} - Nothing\n";
                                continue;
                            }

                            var weenie = DatabaseManager.World.GetCachedWeenie(item.WeenieClassId);
                            msg += $"{((DestinationType)item.DestinationType).ToString()}: {item.Shade,7:P2} - {item.WeenieClassId,5} - {(weenie != null ? weenie.ClassName : "Item not found in DB")} - {(weenie != null ? weenie.GetProperty(PropertyString.Name) : "Item not found in DB")}\n";
                        }
                    }
                    else
                        msg += "Creature has no trophies to drop.\n";

                    if (wieldedTreasure.Count > 0)
                    {
                        foreach (var item in wieldedTreasure)
                        {
                            msg += $"{item.DestinationType.ToString()}: 100.00% - {item.WeenieClassId,5} - {item.WeenieClassName} - {item.Name}\n";
                        }
                    }
                    else
                        msg += "Creature has no wielded items to drop.\n";
                }
                else
                    msg = $"{wo.Name} (0x{wo.Guid}) has no trophies.";

                session.Network.EnqueueSend(new GameMessageSystemChat(msg, ChatMessageType.System));
            }
        }

        // unlock {-all | IID}
        [CommandHandler("unlock", AccessLevel.Envoy, CommandHandlerFlag.RequiresWorld, 1)]
        public static void HandleUnlock(Session session, params string[] parameters)
        {
            // usage: @unlock {-all | IID}
            // Cleans the SQL lock on either everyone or the given player.
            // @unlock - Cleans the SQL lock on either everyone or the given player.

            // TODO: output
        }

        // gamecast <message>
        [CommandHandler("gamecast", AccessLevel.Envoy, CommandHandlerFlag.RequiresWorld, 1,
            "Sends a world-wide broadcast.",
            "<message>\n" +
            "This command sends a world-wide broadcast to everyone in the game. Text is prefixed with 'Broadcast from (admin-name)> '.\n" +
            "See Also: @gamecast, @gamecastemote, @gamecastlocal, @gamecastlocalemote.")]
        public static void HandleGamecast(Session session, params string[] parameters)
        {
            // > Broadcast from usage: @gamecast<message>
            // This command sends a world-wide broadcast to everyone in the game. Text is prefixed with 'Broadcast from (admin-name)> '.
            // See Also: @gamecast, @gamecastemote, @gamecastlocal, @gamecastlocalemote.
            // @gamecast - Sends a world-wide broadcast.

            session.Player.HandleActionWorldBroadcast($"Broadcast from {session.Player.Name}> {string.Join(" ", parameters)}", ChatMessageType.WorldBroadcast);
        }

        // add <spell>
        [CommandHandler("addspell", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1, "Adds the specified spell to your own spellbook.", "<spellid>")]
        public static void HandleAddSpell(Session session, params string[] parameters)
        {
            if (Enum.TryParse(parameters[0], true, out SpellId spellId))
            {
                if (Enum.IsDefined(typeof(SpellId), spellId))
                    session.Player.LearnSpellWithNetworking((uint)spellId);
            }
        }

        [CommandHandler("removespell", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1, "Removes the specified spell to your own spellbook.", "<spellid>")]
        public static void HandleRemoveSpell(Session session, params string[] parameters)
        {
            if (!Enum.TryParse(parameters[0], true, out SpellId spellId))
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Unknown spell {parameters[0]}", ChatMessageType.Broadcast));
                return;
            }
            if (session.Player.RemoveKnownSpell((uint)spellId))
            {
                var spell = new Entity.Spell(spellId, false);
                session.Network.EnqueueSend(new GameMessageSystemChat($"{spell.Name} removed from spellbook.", ChatMessageType.Broadcast));
            }
            else
                session.Network.EnqueueSend(new GameMessageSystemChat($"You don't know that spell!", ChatMessageType.Broadcast));
        }

        // adminhouse
        [CommandHandler("adminhouse", AccessLevel.Admin, CommandHandlerFlag.RequiresWorld, 0)]
        public static void HandleAdminhouse(Session session, params string[] parameters)
        {
            // @adminhouse dump: dumps info about currently selected house or house owned by currently selected player.
            // @adminhouse dump name<name>: dumps info about house owned by the account of the named player.
            // @adminhouse dump account<account_name>: dumps info about house owned by named account.
            // @adminhouse dump hid<houseID>: dumps info about specified house.
            // @adminhouse dump_all: dumps one line about each house in the world.
            // @adminhouse dump_all summary: dumps info about total houses owned for each house type.
            // @adminhouse dump_all dangerous: dumps full info about all houses.Use with caution.
            // @adminhouse rent pay: fully pay the rent of the selected house.
            // @adminhouse rent warn: rent timestamp is pushed far enough back in time to cause the rent to be almost due for the selected house.
            // @adminhouse rent due: rent timestamp is pushed back to cause the rent to be due for the selected house.
            // @adminhouse rent overdue: sets the rent timestamp far enough back to cause the selected house's rent to be overdue.
            // @adminhouse rent payall: fully pay the rent for all houses.
            // @adminhouse payrent on / off: sets the targeted house to not require / require normal maintenance payments.
            // @adminhouse - House management tools for admins.

            // TODO: output
        }

        // bornagain deletedCharID[, newCharName[, accountName]]
        [CommandHandler("bornagain", AccessLevel.Admin, CommandHandlerFlag.RequiresWorld, 1)]
        public static void HandleBornAgain(Session session, params string[] parameters)
        {
            // usage: @bornagain deletedCharID[, newCharName[, accountName]]
            // Given the ID of a deleted character, this command restores that character to its owner.  (You can find the ID of a deleted character using the @finger command.)
            // If the deleted character's name has since been taken by a new character, you can specify a new name for the restored character as the second parameter.  (You can find if the name has been taken by also using the @finger command.)  Use a comma to separate the arguments.
            // If needed, you can specify an account name as a third parameter if the character should be restored to an account other than its original owner.  Again, use a comma between the arguments.
            // @bornagain - Restores a deleted character to an account.

            // TODO: output
        }

        // copychar < character name >, < copy name >
        [CommandHandler("copychar", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 2)]
        public static void HandleCopychar(Session session, params string[] parameters)
        {
            // usage: @copychar < character name >, < copy name >
            // Given the name of an existing character "character name", this command makes a copy of that character with the name "copy name" and places it into your character list.
            // @copychar - Copies an existing character into your character list.

            // TODO: output
        }

        public static Position LastSpawnPos;

        // ??
        public const uint WEENIE_MAX = uint.MaxValue;

        static WorldObject CreateObjectForCommand(Session session, string weenieClassDescription, bool forInventory = false)
        {
            bool wcid = uint.TryParse(weenieClassDescription, out uint weenieClassId);
            if (wcid)
            {
                if (weenieClassId < 1 || weenieClassId > WEENIE_MAX)
                {
                    session.Network.EnqueueSend(new GameMessageSystemChat($"Not a valid weenie id. Must be a number between 1 and {WEENIE_MAX}.", ChatMessageType.Broadcast));
                    return null;
                }
            }

            Weenie weenie;
            if (wcid)
                weenie = DatabaseManager.World.GetCachedWeenie(weenieClassId);
            else
                weenie = DatabaseManager.World.GetCachedWeenie(weenieClassDescription);

            if (weenie == null)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"{weenieClassDescription} is not a valid weenie.", ChatMessageType.Broadcast));
                return null;
            }

            var weenieType = weenie.WeenieType;
            if (   weenieType == WeenieType.Admin
                || weenieType == WeenieType.AI
                || weenieType == WeenieType.Allegiance
                || weenieType == WeenieType.BootSpot
                || weenieType == WeenieType.Channel
                || weenieType == WeenieType.CombatPet
                || weenieType == WeenieType.Deed
                || weenieType == WeenieType.Entity
                || weenieType == WeenieType.EventCoordinator
                || weenieType == WeenieType.Game
                || weenieType == WeenieType.GamePiece
                || weenieType == WeenieType.GScoreGatherer
                || weenieType == WeenieType.GScoreKeeper
                || weenieType == WeenieType.GSpellEconomy
                || weenieType == WeenieType.Hook
                || weenieType == WeenieType.House
                || weenieType == WeenieType.HousePortal
                || weenieType == WeenieType.HUD                
                || weenieType == WeenieType.InGameStatKeeper
                || weenieType == WeenieType.LScoreKeeper
                || weenieType == WeenieType.LSpellEconomy
                || weenieType == WeenieType.Machine
                || weenieType == WeenieType.Pet
                || weenieType == WeenieType.ProjectileSpell
                || weenieType == WeenieType.Sentinel
                || weenieType == WeenieType.SlumLord
                || weenieType == WeenieType.SocialManager
                || weenieType == WeenieType.Storage
                || weenieType == WeenieType.Undef
                || weenieType == WeenieType.UNKNOWN__GUESSEDNAME32
                )
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"You cannot spawn {weenie.ClassName} because it is a {weenieType}", ChatMessageType.Broadcast));
                return null;
            }

            if (forInventory && weenie.IsStuck())
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"You cannot spawn {weenie.ClassName} in your inventory because it cannot be picked up", ChatMessageType.Broadcast));
                return null;
            }

            var obj = WorldObjectFactory.CreateNewWorldObject(weenie);

            if (!obj.TimeToRot.HasValue)
                obj.TimeToRot = Double.MaxValue;

            if (obj.WeenieType == WeenieType.Creature)
                obj.Location = session.Player.Location.InFrontOf(5f, true);
            else
                obj.Location = session.Player.Location.InFrontOf((obj.UseRadius ?? 2) > 2 ? obj.UseRadius.Value : 2);

            obj.Location.LandblockId = new LandblockId(obj.Location.GetCell());

            LastSpawnPos = obj.Location;

            return obj;
        }

        // create wclassid (number)
        [CommandHandler("create", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1,
            "Creates an object or objects in the world.",
            "wclassid (string or number), Amount to Spawn (optional [default:1]), Palette (optional), Shade (optional)\n" +
            "This will attempt to spawn the weenie you specify. If you include an amount to spawn parameter it will attempt to spawn that many of the weenie.\n" +
            "Stackable items will spawn in stacks of their max stack size. All spawns will be limited by the physics engine placement algorithim which may prevent the number you specify from actually spawning." +
            "Be careful with large numbers, especially with ethereal weenies...")]
        public static void HandleCreate(Session session, params string[] parameters)
        {
            string weenieClassDescription = parameters[0];
            int palette = 0;
            bool hasPalette = false;
            float shade = 0;
            bool hasShade = false;
            int numToSpawn = 1;
            WorldObject weenieToSpawn;
            List<WorldObject> weeniesToSpawn = new List<WorldObject>();

            if (parameters.Length > 1)
            {
                if (!int.TryParse(parameters[1], out numToSpawn))
                {
                    session.Network.EnqueueSend(new GameMessageSystemChat($"Amount to spawn must be a number between {int.MinValue} - {int.MaxValue}.", ChatMessageType.Broadcast));
                    return;
                }
            }
            if (parameters.Length > 2)
            {
                if (!int.TryParse(parameters[2], out palette))
                {
                    session.Network.EnqueueSend(new GameMessageSystemChat($"Palette must be number between {int.MinValue} - {int.MaxValue}.", ChatMessageType.Broadcast));
                    return;
                }
                else
                    hasPalette = true;
            }
            if (parameters.Length > 3)
            {
                if (!float.TryParse(parameters[3], out shade))
                {
                    session.Network.EnqueueSend(new GameMessageSystemChat($"Shade must be number between {float.MinValue} - {float.MaxValue}.", ChatMessageType.Broadcast));
                    return;
                }
                else
                    hasShade = true;
            }

            weenieToSpawn = CreateObjectForCommand(session, weenieClassDescription);

            // The number of weenies to spawn will be limited by the physics engine.
            if (weenieToSpawn != null && numToSpawn == 1)
            {
                weeniesToSpawn.Add(weenieToSpawn);
            }
            else if (weenieToSpawn != null && numToSpawn > 1)
            {
                if (weenieToSpawn.WeenieType is WeenieType.Stackable)
                {
                    if (weenieToSpawn.MaxStackSize != null && numToSpawn > weenieToSpawn.MaxStackSize)
                    {
                        int numWeeniesRequired = (numToSpawn / (int)weenieToSpawn.MaxStackSize);
                        int lastStackAmount = numToSpawn % (int)weenieToSpawn.MaxStackSize;
                        for (int i = 0; i < numWeeniesRequired; i++)
                        {
                            weeniesToSpawn.Add(CreateObjectForCommand(session, weenieClassDescription));
                        }
                        foreach (var w in weeniesToSpawn)
                        {
                            w.SetStackSize(weenieToSpawn.MaxStackSize);
                        }
                        weenieToSpawn.SetStackSize(lastStackAmount);
                        weeniesToSpawn.Add(weenieToSpawn);
                    }
                    else if (weenieToSpawn.MaxStackSize != null && numToSpawn <= weenieToSpawn.MaxStackSize)
                    {
                        weenieToSpawn.SetStackSize(numToSpawn);
                        weeniesToSpawn.Add(weenieToSpawn);
                    }
                }
                else
                {
                    for (int i = 0; i < numToSpawn; i++)
                    {
                        weeniesToSpawn.Add(CreateObjectForCommand(session, weenieClassDescription));
                    }
                }
            }
            else
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"No object was created.", ChatMessageType.Broadcast));
                return;
            }

            foreach (var w in weeniesToSpawn)
            {
                if (hasPalette)
                    w.PaletteTemplate = palette;
                if (hasShade)
                    w.Shade = shade;
                w.EnterWorld();
            }

            if (numToSpawn > 1)
                PlayerManager.BroadcastToAuditChannel(session.Player, $"{session.Player.Name} has created {numToSpawn} {weenieToSpawn.Name} (0x{weenieToSpawn.Guid:X8}) near {weenieToSpawn.Location.ToLOCString()}.");
            else
                PlayerManager.BroadcastToAuditChannel(session.Player, $"{session.Player.Name} has created {weenieToSpawn.Name} (0x{weenieToSpawn.Guid:X8}) at {weenieToSpawn.Location.ToLOCString()}.");
        }

        [CommandHandler("createnamed", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 3,
            "Creates a named object in the world.", "<wclassid(string or number)> <count> <name ... >")]
        public static void HandleCreateNamed(Session session, params string[] parameters)
        {
            if (!Int32.TryParse(parameters[1], out int count))
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"count must be an integer value", ChatMessageType.Broadcast));
                return;
            }
            if (count < 1 || count > ushort.MaxValue)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"count must be a between 1 and {ushort.MaxValue}", ChatMessageType.Broadcast));
                return;
            }
            string predefName = null;
            if (parameters.Length > 1)
            {
                predefName = String.Join(" ", parameters, 2, parameters.Length - 2);
            }

            WorldObject first = null;
            for (int i = 0; i < count; i++)
            {
                WorldObject obj = CreateObjectForCommand(session, parameters[0]);
                if (obj == null)
                {
                    // should have already emitted an error message
                    return;
                }
                if (first == null)
                {
                    first = obj;
                }

                // Rename object if specified
                if (predefName != null)
                    obj.Name = predefName;

                obj.EnterWorld();
            }

            if (count == 1)
                PlayerManager.BroadcastToAuditChannel(session.Player, $"{session.Player.Name} has created {first.Name} (0x{first.Guid:X8}) at {first.Location.ToLOCString()}.");
            else
                PlayerManager.BroadcastToAuditChannel(session.Player, $"{session.Player.Name} has created {count}x {first.Name} at {first.Location.ToLOCString()}.");
        }

        // ci wclassid (number)
        [CommandHandler("ci", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1, "Creates an object in your inventory.", "wclassid (string or number), Amount to Spawn (optional [default:1]), Palette (optional), Shade (optional)\n")]
        public static void HandleCI(Session session, params string[] parameters)
        {
            string weenieClassDescription = parameters[0];
            ushort stackSize = 0;
            if (parameters.Length > 1)
            {
                var isValidStackSize = ushort.TryParse(parameters[1], out stackSize);
                if (!isValidStackSize || stackSize == 0)
                {
                    session.Network.EnqueueSend(new GameMessageSystemChat($"stacksize must be number between 1 - {ushort.MaxValue}", ChatMessageType.Broadcast));
                    return;
                }
            }

            bool hasPalette = false;
            int palette = 0;
            bool hasShade = false;
            float shade = 0f;
            if (parameters.Length > 2)
            {
                if (!int.TryParse(parameters[2], out palette))
                {
                    session.Network.EnqueueSend(new GameMessageSystemChat($"palette must be number between {int.MinValue} - {int.MaxValue}", ChatMessageType.Broadcast));
                    return;
                }
                else
                    hasPalette = true;
            }
            if (parameters.Length > 3)
            {
                if (!float.TryParse(parameters[3], out shade))
                {
                    session.Network.EnqueueSend(new GameMessageSystemChat($"shade must be number between {float.MinValue} - {float.MaxValue}", ChatMessageType.Broadcast));
                    return;
                }
                else
                    hasShade = true;
            }

            WorldObject obj = CreateObjectForCommand(session, weenieClassDescription, true);
            if (obj == null)
            {
                // already sent an error message
                return;
            }

            if (stackSize != 0)
            {
                if (obj.MaxStackSize != null && stackSize > obj.MaxStackSize)
                    obj.SetStackSize(obj.MaxStackSize);
                else if (obj.MaxStackSize != null && stackSize <= obj.MaxStackSize)
                    obj.SetStackSize(stackSize);
            }

            if (hasPalette)
                obj.PaletteTemplate = palette;
            if (hasShade)
                obj.Shade = shade;

            session.Player.TryCreateInInventoryWithNetworking(obj);

            PlayerManager.BroadcastToAuditChannel(session.Player, $"{session.Player.Name} has created {obj.Name} (0x{obj.Guid:X8}) in their inventory.");
        }

        [CommandHandler("crack", AccessLevel.Envoy, CommandHandlerFlag.RequiresWorld, 0, "Cracks the most recently appraised locked target.", "[. open it too]")]
        public static void HandleCrack(Session session, params string[] parameters)
        {
            bool openIt = (parameters?.Length > 0 && parameters[0] == ".");
            bool notOK = false;
            if (session.Player.CurrentAppraisalTarget.HasValue)
            {
                var objectId = new ObjectGuid((uint)session.Player.CurrentAppraisalTarget);
                var wo = session.Player.CurrentLandblock?.GetObject(objectId);
                if (wo is Lock @lock)
                {
                    var opening = openIt ? $" Opening {wo.WeenieType}." : "";
                    string lockCode = LockHelper.GetLockCode(wo);
                    int? resistLockpick = LockHelper.GetResistLockpick(wo);
                    UnlockResults res = UnlockResults.IncorrectKey;

                    if (!string.IsNullOrWhiteSpace(lockCode))
                    {
                        res = @lock.Unlock(session.Player.Guid.Full, null, lockCode);
                        ChatPacket.SendServerMessage(session, $"Crack {wo.WeenieType} via {lockCode} result: {res}.{opening}", ChatMessageType.Broadcast);
                    }
                    else if (resistLockpick.HasValue && resistLockpick > 0)
                    {
                        var difficulty = 0;
                        res = @lock.Unlock(session.Player.Guid.Full, (uint)(resistLockpick * 2), ref difficulty);
                        ChatPacket.SendServerMessage(session, $"Crack {wo.WeenieType} with skill {resistLockpick}*2 result: {res}.{opening}", ChatMessageType.Broadcast);
                    }
                    else
                        ChatPacket.SendServerMessage(session, $"The {wo.WeenieType} has no key code or lockpick difficulty.  Unable to crack it.{opening}", ChatMessageType.Broadcast);

                    if (openIt)
                    {
                        if (wo is Door woDoor)
                            woDoor.Open(session.Player.Guid);
                        else if (wo is Chest woChest)
                            ChatPacket.SendServerMessage(session, $"The {wo.WeenieType} cannot be opened because it is not implemented yet!", ChatMessageType.Broadcast);
                    }
                }
                else notOK = true;
            }
            else notOK = true;
            if (notOK) ChatPacket.SendServerMessage(session, "Appraise a locked target before using @crack", ChatMessageType.Broadcast);
        }

        /// <summary>
        /// Displays how much experience the last appraised creature is worth when killed.
        /// </summary>
        [CommandHandler("deathxp", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0, "Displays how much experience the last appraised creature is worth when killed.")]
        public static void HandleDeathxp(Session session, params string[] parameters)
        {
            var creature = CommandHandlerHelper.GetLastAppraisedObject(session);
            if (creature == null) return;

            CommandHandlerHelper.WriteOutputInfo(session, $"{creature.Name} XP: {creature.XpOverride}");
        }

        // de_n name, text
        [CommandHandler("de_n", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 2, "Sends text to named player, formatted exactly as entered.", "<name> <text>")]
        public static void Handlede_n(Session session, params string[] parameters)
        {
            // usage: @de_n name, text
            // usage: @direct_emote_name name, text
            // Sends text to named player, formatted exactly as entered, with no prefix of any kind.
            // @direct_emote_name - Sends text to named player, formatted exactly as entered.

            // TODO: output
        }

        // direct_emote_name name, text
        [CommandHandler("direct_emote_name", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 2, "Sends text to named player, formatted exactly as entered.", "<name> <text>")]
        public static void Handledirect_emote_name(Session session, params string[] parameters)
        {
            // usage: @de_n name, text
            // usage: @direct_emote_name name, text
            // Sends text to named player, formatted exactly as entered, with no prefix of any kind.
            // @direct_emote_name - Sends text to named player, formatted exactly as entered.

            // TODO: output
        }

        // de_s text
        [CommandHandler("de_s", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1, "Sends text to selected player, formatted exactly as entered, with no prefix of any kind.", "<text>")]
        public static void Handlede_s(Session session, params string[] parameters)
        {
            // usage: @de_s text
            // usage: @direct_emote_select text
            // Sends text to selected player, formatted exactly as entered, with no prefix of any kind.
            // @direct_emote_select - Sends text to selected player, formatted exactly as entered.

            // TODO: output
        }

        // direct_emote_select text
        [CommandHandler("direct_emote_select", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1, "Sends text to selected player, formatted exactly as entered, with no prefix of any kind.", "<text>")]
        public static void Handledirect_emote_select(Session session, params string[] parameters)
        {
            // usage: @de_s text
            // usage: @direct_emote_select text
            // Sends text to selected player, formatted exactly as entered, with no prefix of any kind.
            // @direct_emote_select - Sends text to selected player, formatted exactly as entered.

            // TODO: output
        }

        // dispel
        [CommandHandler("dispel", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0, "Removes all enchantments from the player", "/dispel")]
        public static void HandleDispel(Session session, params string[] parameters)
        {
            session.Player.EnchantmentManager.DispelAllEnchantments();

            // remove all enchantments from equipped items for now
            foreach (var item in session.Player.EquippedObjects.Values)
                item.EnchantmentManager.DispelAllEnchantments();
        }

        // event
        [CommandHandler("event", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 2,
            "Maniuplates the state of an event",
            "[ start | stop | disable | enable | clear | status ] (name)\n"
            + "@event clear < name > - clears event with name <name> or all events if you put in 'all' (All clears registered generators, <name> does not)\n"
            + "@event status <eventSubstring> - get the status of all registered events or get all of the registered events that have <eventSubstring> in the name.")]
        public static void HandleEvent(Session session, params string[] parameters)
        {
            // usage: @event start| stop | disable | enable name
            // @event clear < name > -clears event with name <name> or all events if you put in 'all' (All clears registered generators, <name> does not).
            // @event status<eventSubstring> - get the status of all registered events or get all of the registered events that have <eventSubstring> in the name.
            // @event - Maniuplates the state of an event.

            // TODO: output

            var eventCmd = parameters?[0].ToLower();

            var eventName = parameters?[1];

            switch (eventCmd)
            {
                case "start":
                    if (EventManager.StartEvent(eventName))
                        session.Network.EnqueueSend(new GameMessageSystemChat($"Event {eventName} started successfully.", ChatMessageType.Broadcast));
                    else
                        session.Network.EnqueueSend(new GameMessageSystemChat($"Unable to start event named {eventName} .", ChatMessageType.Broadcast));
                    break;
                case "stop":
                    if (EventManager.StopEvent(eventName))
                        session.Network.EnqueueSend(new GameMessageSystemChat($"Event {eventName} stopped successfully.", ChatMessageType.Broadcast));
                    else
                        session.Network.EnqueueSend(new GameMessageSystemChat($"Unable to stop event named {eventName} .", ChatMessageType.Broadcast));
                    break;
                case "disable":
                    break;
                case "enable":
                    break;
                case "clear":
                    break;
                case "status":
                    if (eventName != "all" && eventName != "")
                    {
                        session.Network.EnqueueSend(new GameMessageSystemChat($"Event {eventName} - GameEventState.{EventManager.GetEventStatus(eventName)}", ChatMessageType.Broadcast));
                    }
                    break;
                default:
                    session.Network.EnqueueSend(new GameMessageSystemChat("That is not a valid event command", ChatMessageType.Broadcast));
                    break;
            }
        }

        // fumble
        [CommandHandler("fumble", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0)]
        public static void HandleFumble(Session session, params string[] parameters)
        {
            // @fumble - Forces the selected target to drop everything they contain to the ground.

            // TODO: output
        }

        // god
        [CommandHandler("god", AccessLevel.Sentinel, CommandHandlerFlag.RequiresWorld, 0,
            "Turns current character into a god!",
            "Sets attributes and skills to higher than max levels.\n"
            + "To return to a mortal state, use the /ungod command.\n"
            + "Use the /god command with caution. While unlikely, there is a possibility that the character that runs the command will not be able to return to normal or will end up in a state that is unrecoverable.")]
        public static void HandleGod(Session session, params string[] parameters)
        {
            // @god - Sets your own stats to a godly level.
            // need to save stats so that we can return with /ungod
            DatabaseManager.Shard.SaveBiota(session.Player.Biota, session.Player.BiotaDatabaseLock, result => DoGodMode(result, session));
        }

        private static void DoGodMode(bool playerSaved, Session session, bool exceptionReturn = false)
        {
            if (!playerSaved)
            {
                ChatPacket.SendServerMessage(session, "Error saving player. Godmode not available.", ChatMessageType.Broadcast);
                Console.WriteLine($"Player {session.Player.Name} tried to enter god mode but there was an error saving player. Godmode not available.");
                return;
            }

            var biota = session.Player.Biota;

            string godString = session.Player.GodState;

            if (!exceptionReturn)
            {
                // if godstate starts with 1, you are in godmode

                if (godString != null)
                {
                    if (godString.StartsWith("1"))
                    {
                        ChatPacket.SendServerMessage(session, "You are already a god.", ChatMessageType.Broadcast);
                        return;
                    }
                }

                string returnState = "1=";
                returnState += $"{DateTime.UtcNow}=";

                // need level 25, available skill credits 24
                returnState += $"24={session.Player.AvailableSkillCredits}=25={session.Player.Level}=";

                // need total xp 1, unassigned xp 2
                returnState += $"1={session.Player.TotalExperience}=2={session.Player.AvailableExperience}=";

                // need all attributes
                // 1 through 6 str, end, coord, quick, focus, self
                foreach (var kvp in biota.PropertiesAttribute)
                {
                    var att = kvp.Value;

                    if (kvp.Key > 0 && (int)kvp.Key <= 6)
                    {
                        returnState += $"{(int)kvp.Key}=";
                        returnState += $"{att.InitLevel}=";
                        returnState += $"{att.LevelFromCP}=";
                        returnState += $"{att.CPSpent}=";
                    }
                }

                // need all vitals
                // 1, 3, 5 H,S,M (2,4,6 are current values and are not stored since they will be maxed entering/exiting godmode)
                foreach (var kvp in biota.PropertiesAttribute2nd)
                {
                    var attSec = kvp.Value;

                    if ((int)kvp.Key == 1 || (int)kvp.Key == 3 || (int)kvp.Key == 5)
                    {
                        returnState += $"{(int)kvp.Key}=";
                        returnState += $"{attSec.InitLevel}=";
                        returnState += $"{attSec.LevelFromCP}=";
                        returnState += $"{attSec.CPSpent}=";
                        returnState += $"{attSec.CurrentLevel}=";
                    }
                }

                // need all skills
                foreach (var kvp in biota.PropertiesSkill)
                {
                    var sk = kvp.Value;

                    if (SkillHelper.ValidSkills.Contains(kvp.Key))
                    {
                        returnState += $"{(int)kvp.Key}=";
                        returnState += $"{sk.LevelFromPP}=";
                        returnState += $"{(uint)sk.SAC}=";
                        returnState += $"{sk.PP}=";
                        returnState += $"{sk.InitLevel}=";
                    }
                }

                // Check string is correctly formatted before altering stats
                // correctly formatted return string should have 240 entries
                // if the construction of the string changes - this will need to be updated to match
                if (returnState.Split("=").Length != 240)
                {
                    ChatPacket.SendServerMessage(session, "Godmode is not available at this time.", ChatMessageType.Broadcast);
                    Console.WriteLine($"Player {session.Player.Name} tried to enter god mode but there was an error with the godString length. (length = {returnState.Split("=").Length}) Godmode not available.");
                    return;
                }

                // save return state to db in property string
                session.Player.SetProperty(PropertyString.GodState, returnState);
                session.Player.SaveBiotaToDatabase();
            }

            // Begin Godly Stats Increase

            var currentPlayer = session.Player;
            currentPlayer.Level = 999;
            currentPlayer.AvailableExperience = 0;
            currentPlayer.AvailableSkillCredits = 0;
            currentPlayer.TotalExperience = 191226310247;

            var levelMsg = new GameMessagePrivateUpdatePropertyInt(currentPlayer, PropertyInt.Level, (int)currentPlayer.Level);
            var expMsg = new GameMessagePrivateUpdatePropertyInt64(currentPlayer, PropertyInt64.AvailableExperience, (long)currentPlayer.AvailableExperience);
            var skMsg = new GameMessagePrivateUpdatePropertyInt(currentPlayer, PropertyInt.AvailableSkillCredits, (int)currentPlayer.AvailableSkillCredits);
            var totalExpMsg = new GameMessagePrivateUpdatePropertyInt64(currentPlayer, PropertyInt64.TotalExperience, (long)currentPlayer.TotalExperience);

            currentPlayer.Session.Network.EnqueueSend(levelMsg, expMsg, skMsg, totalExpMsg);

            foreach (var s in currentPlayer.Skills)
            {
                currentPlayer.TrainSkill(s.Key, 0);
                currentPlayer.SpecializeSkill(s.Key, 0);
                var playerSkill = currentPlayer.Skills[s.Key];
                playerSkill.Ranks = 226;
                playerSkill.ExperienceSpent = 4100490438u;
                playerSkill.InitLevel = 5000;
                currentPlayer.Session.Network.EnqueueSend(new GameMessagePrivateUpdateSkill(currentPlayer, playerSkill));
            }

            foreach (var a in currentPlayer.Attributes)
            {
                var playerAttr = currentPlayer.Attributes[a.Key];
                playerAttr.StartingValue = 9809u;
                playerAttr.Ranks = 190u;
                playerAttr.ExperienceSpent = 4019438644u;
                currentPlayer.Session.Network.EnqueueSend(new GameMessagePrivateUpdateAttribute(currentPlayer, playerAttr));
            }

            currentPlayer.SetMaxVitals();

            foreach (var v in currentPlayer.Vitals)
            {
                var playerVital = currentPlayer.Vitals[v.Key];
                playerVital.Ranks = 196u;
                playerVital.ExperienceSpent = 4285430197u;
                // my OCD will not let health/stam not be equal due to the endurance calc
                playerVital.StartingValue = (v.Key == PropertyAttribute2nd.MaxHealth) ? 94803u : 89804u;
                currentPlayer.Session.Network.EnqueueSend(new GameMessagePrivateUpdateVital(currentPlayer, playerVital));
            }

            currentPlayer.PlayParticleEffect(PlayScript.LevelUp, currentPlayer.Guid);
            currentPlayer.PlayParticleEffect(PlayScript.BaelZharonSmite, currentPlayer.Guid);

            currentPlayer.SetMaxVitals();

            ChatPacket.SendServerMessage(session, "You are now a god!!!", ChatMessageType.Broadcast);
        }

        // ungod
        [CommandHandler("ungod", AccessLevel.Sentinel, CommandHandlerFlag.RequiresWorld, 0,
            "Returns character to a mortal state.",
            "Sets attributes and skills back to the values they were when you became a god.\n"
            + "If the command fails to revert your state it will default to godmode.\n"
            + "In the event of failure, contact a server administrator to sort it out.")]
        public static void HandleUngod(Session session, params string[] parameters)
        {
            // @ungod - Returns skills and attributues to pre-god levels.
            Player currentPlayer = session.Player;
            string returnString = session.Player.GodState;

            if (returnString == null)
            {
                ChatPacket.SendServerMessage(session, "Can't get any more ungodly than you already are...", ChatMessageType.Broadcast);
                return;
            }
            else
            {
                try
                {
                    string[] returnStringArr = returnString.Split("=");

                    // correctly formatted return string should have 240 entries
                    // if the construction of the string changes - this will need to be updated to match
                    if (returnStringArr.Length != 240)
                    {
                        Console.WriteLine($"The returnString was not set to the correct length while {currentPlayer.Name} was attempting to return to normal from godmode.");
                        ChatPacket.SendServerMessage(session, "Error returning to mortal state, defaulting to godmode.", ChatMessageType.Broadcast);
                        return;
                    }

                    for (int i = 2; i < returnStringArr.Length;)
                    {
                        switch (i)
                        {
                            case int n when (n <= 5):
                                currentPlayer.SetProperty((PropertyInt)uint.Parse(returnStringArr[i]), int.Parse(returnStringArr[i + 1]));
                                i += 2;
                                break;
                            case int n when (n <= 9):
                                currentPlayer.SetProperty((PropertyInt64)uint.Parse(returnStringArr[i]), long.Parse(returnStringArr[i + 1]));
                                i += 2;
                                break;
                            case int n when (n <= 33):
                                var playerAttr = currentPlayer.Attributes[(PropertyAttribute)uint.Parse(returnStringArr[i])];
                                playerAttr.StartingValue = uint.Parse(returnStringArr[i + 1]);
                                playerAttr.Ranks = uint.Parse(returnStringArr[i + 2]);
                                playerAttr.ExperienceSpent = uint.Parse(returnStringArr[i + 3]);
                                currentPlayer.Session.Network.EnqueueSend(new GameMessagePrivateUpdateAttribute(currentPlayer, playerAttr));
                                i += 4;
                                break;
                            case int n when (n <= 48):
                                var playerVital = currentPlayer.Vitals[(PropertyAttribute2nd)int.Parse(returnStringArr[i])];
                                playerVital.StartingValue = uint.Parse(returnStringArr[i + 1]);
                                playerVital.Ranks = uint.Parse(returnStringArr[i + 2]);
                                playerVital.ExperienceSpent = uint.Parse(returnStringArr[i + 3]);
                                playerVital.Current = uint.Parse(returnStringArr[i + 4]);
                                currentPlayer.Session.Network.EnqueueSend(new GameMessagePrivateUpdateVital(currentPlayer, playerVital));
                                i += 5;
                                break;
                            case int n when (n <= 238):
                                var playerSkill = currentPlayer.Skills[(Skill)int.Parse(returnStringArr[i])];
                                playerSkill.Ranks = ushort.Parse(returnStringArr[i + 1]);

                                // Handle god users stuck in god mode due to bad godstate with Enum string
                                SkillAdvancementClass advancement;
                                if (Enum.TryParse(returnStringArr[i + 2], out advancement))
                                {
                                    playerSkill.AdvancementClass = advancement;
                                }
                                else
                                {
                                    playerSkill.AdvancementClass = (SkillAdvancementClass)uint.Parse(returnStringArr[i + 2]);
                                }

                                playerSkill.ExperienceSpent = uint.Parse(returnStringArr[i + 3]);
                                playerSkill.InitLevel = uint.Parse(returnStringArr[i + 4]);
                                currentPlayer.Session.Network.EnqueueSend(new GameMessagePrivateUpdateSkill(currentPlayer, playerSkill));
                                i += 5;
                                break;
                            case 239: //end of returnString, this will need to be updated if the length of the string changes
                                GameMessagePrivateUpdatePropertyInt levelMsg = new GameMessagePrivateUpdatePropertyInt(currentPlayer, PropertyInt.Level, (int)currentPlayer.Level);
                                GameMessagePrivateUpdatePropertyInt skMsg = new GameMessagePrivateUpdatePropertyInt(currentPlayer, PropertyInt.AvailableSkillCredits, (int)currentPlayer.AvailableSkillCredits);
                                GameMessagePrivateUpdatePropertyInt64 totalExpMsg = new GameMessagePrivateUpdatePropertyInt64(currentPlayer, PropertyInt64.TotalExperience, (long)currentPlayer.TotalExperience);
                                GameMessagePrivateUpdatePropertyInt64 unassignedExpMsg = new GameMessagePrivateUpdatePropertyInt64(currentPlayer, PropertyInt64.AvailableExperience, (long)currentPlayer.AvailableExperience);
                                currentPlayer.Session.Network.EnqueueSend(levelMsg, skMsg, totalExpMsg, unassignedExpMsg);
                                i++;
                                break;
                            default:
                                // A warning that will alert on the console if the returnString length changes. This should suffice until a smoother way can be found.
                                Console.WriteLine($"Hit default case in /ungod command with i = {i}, did you change the length of the PropertyString.GodState array?");
                                i++;
                                break;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Exception ( { e.Source } - {e.Message} ) caught while {currentPlayer.Name} was attempting to return to normal from godmode.");
                    ChatPacket.SendServerMessage(session, "Error returning to mortal state, defaulting to godmode.", ChatMessageType.Broadcast);
                    DoGodMode(true, session, true);
                    return;
                }

                currentPlayer.SetMaxVitals();

                currentPlayer.RemoveProperty(PropertyString.GodState);

                currentPlayer.SaveBiotaToDatabase();

                currentPlayer.PlayParticleEffect(PlayScript.DispelAll, currentPlayer.Guid);

                ChatPacket.SendServerMessage(session, "You have returned from your godly state.", ChatMessageType.Broadcast);
            }
        }

        // magic god
        [CommandHandler("magic god", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0)]
        public static void HandleMagicGod(Session session, params string[] parameters)
        {
            // @magic god - Sets your magic stats to the specfied level.

            // TODO: output

            // output: You are now a magic god!!!

            ChatPacket.SendServerMessage(session, "You are now a magic god!!!", ChatMessageType.Broadcast);
        }


        [CommandHandler("modifyvital", AccessLevel.Admin, CommandHandlerFlag.None, 2, "Adjusts the maximum vital attribute for the last appraised mob/player and restores full vitals", "<Health|Stamina|Mana> <delta>")]
        public static void HandleModifyVital(Session session, params string[] parameters)
        {
            var lastAppraised = CommandHandlerHelper.GetLastAppraisedObject(session);
            if (lastAppraised == null || !(lastAppraised is Creature))
            {
                ChatPacket.SendServerMessage(session, "The last appraised object was not a mob/NPC/player.", ChatMessageType.Broadcast);
                return;
            }
            var creature = lastAppraised as Creature;

            if (parameters.Length < 2)
            {
                ChatPacket.SendServerMessage(session, "Usage: modifyvital <Invalid vital type, valid values are: Health,Stamina,Mana", ChatMessageType.Broadcast);
                return;
            }

            // determine the vital type
            if (!Enum.TryParse(parameters[0], out PropertyAttribute2nd vitalAttr))
            {
                ChatPacket.SendServerMessage(session, "Invalid vital type, valid values are: Health,Stamina,Mana", ChatMessageType.Broadcast);
                return;
            }

            if (!Int32.TryParse(parameters[1], out int delta))
            {
                ChatPacket.SendServerMessage(session, "Invalid vital value, values must be valid integers", ChatMessageType.Broadcast);
                return;
            }

            PropertyAttribute2nd maxAttr;
            switch (vitalAttr)
            {
                case PropertyAttribute2nd.Health:
                    maxAttr = PropertyAttribute2nd.MaxHealth;
                    break;
                case PropertyAttribute2nd.Stamina:
                    maxAttr = PropertyAttribute2nd.MaxStamina;
                    break;
                case PropertyAttribute2nd.Mana:
                    maxAttr = PropertyAttribute2nd.MaxMana;
                    break;
                default:
                    ChatPacket.SendServerMessage(session, "Unexpected vital type, valid values are: Health,Stamina,Mana", ChatMessageType.Broadcast);
                    return;
            }

            CreatureVital maxVital = new CreatureVital(creature, maxAttr);
            maxVital.Ranks = (uint)Math.Clamp(maxVital.Ranks + delta, 1, uint.MaxValue);
            creature.UpdateVital(maxVital, maxVital.MaxValue);

            CreatureVital vital = new CreatureVital(creature, vitalAttr);
            vital.Ranks = (uint)Math.Clamp(vital.Ranks + delta, 1, uint.MaxValue);
            creature.UpdateVital(vital, maxVital.MaxValue);

            if (creature is Player)
            {
                Player player = creature as Player;
                player.Session.Network.EnqueueSend(new GameMessagePrivateUpdateVital(player, maxVital));
            }

            creature.SetMaxVitals();

            // save changes
            if (creature is Player || creature.IsDynamicThatShouldPersistToShard())
            {
                creature.SaveBiotaToDatabase();
            }
        }

        [CommandHandler("modifyskill", AccessLevel.Admin, CommandHandlerFlag.None, 2, "Adjusts the skill for the last appraised mob/player", "<skillName> <delta>")]
        public static void HandleModifySkill(Session session, params string[] parameters)
        {
            var lastAppraised = CommandHandlerHelper.GetLastAppraisedObject(session);
            if (lastAppraised == null || !(lastAppraised is Creature))
            {
                ChatPacket.SendServerMessage(session, "The last appraised object was not a mob/NPC/player.", ChatMessageType.Broadcast);
                return;
            }
            var creature = lastAppraised as Creature;

            if (parameters.Length < 2)
            {
                ChatPacket.SendServerMessage(session, "Usage: modifyskill <skillName> <delta>: missing skillId and/or delta", ChatMessageType.Broadcast);
                return;
            }
            if (!Enum.TryParse(parameters[0], out Skill skill))
            {
                String[] names = Enum.GetNames(typeof(Skill));
                ChatPacket.SendServerMessage(session, "Invalid skillName, must be a valid skill name (without spaces, with capitalization), valid values are: " + String.Join(", ", names), ChatMessageType.Broadcast);
                return;
            }
            if (!Int32.TryParse(parameters[1], out int delta))
            {
                ChatPacket.SendServerMessage(session, "Invalid delta, must be a valid integer", ChatMessageType.Broadcast);
                return;
            }

            CreatureSkill creatureSkill = creature is Player ? creature.Skills[skill] : creature.GetCreatureSkill(skill);
            creatureSkill.InitLevel = (ushort)Math.Clamp(creatureSkill.InitLevel + delta, 0, (Int32)ushort.MaxValue);

            // save changes
            if (creature is Player || creature.IsDynamicThatShouldPersistToShard())
            {
                creature.SaveBiotaToDatabase();
            }
            if (creature is Player)
            {
                Player player = creature as Player;
                player.Session.Network.EnqueueSend(new GameMessagePrivateUpdateSkill(player, creatureSkill));
            }
        }

        [CommandHandler("modifyattr", AccessLevel.Admin, CommandHandlerFlag.None, 2, "Adjusts an attribute for the last appraised mob/NPC/player", "<attribute> <delta>")]
        public static void HandleModifyAttribute(Session session, params string[] parameters)
        {
            var lastAppraised = CommandHandlerHelper.GetLastAppraisedObject(session);
            if (lastAppraised == null || !(lastAppraised is Creature))
            {
                ChatPacket.SendServerMessage(session, "The last appraised object was not a mob/NPC/player.", ChatMessageType.Broadcast);
                return;
            }
            var creature = lastAppraised as Creature;

            if (parameters.Length < 2)
            {
                ChatPacket.SendServerMessage(session, "Usage: modifyattr <attribute> <delta>: missing attribute name and/or delta", ChatMessageType.Broadcast);
                return;
            }
            if (!Enum.TryParse(parameters[0], out PropertyAttribute attrType))
            {
                ChatPacket.SendServerMessage(session, "Invalid skillName, must be a valid skill name (without spaces, with capitalization), valid values are: Strength,Endurance,Coordination,Quickness,Focus,Self", ChatMessageType.Broadcast);
                return;
            }
            if (!Int32.TryParse(parameters[1], out int delta))
            {
                ChatPacket.SendServerMessage(session, "Invalid delta, must be a valid integer", ChatMessageType.Broadcast);
                return;
            }

            CreatureAttribute attr = creature.Attributes[attrType];
            attr.StartingValue = (uint)Math.Clamp(attr.StartingValue + delta, 1, 9999);

            if (creature is Player || creature.IsDynamicThatShouldPersistToShard())
            {
                creature.SaveBiotaToDatabase();
            }
            if (creature is Player)
            {
                Player player = creature as Player;
                player.Session.Network.EnqueueSend(new GameMessagePrivateUpdateAttribute(player, attr));
            }
        }

        // heal
        [CommandHandler("heal", AccessLevel.Envoy, CommandHandlerFlag.RequiresWorld, 0,
            "Heals yourself (or the selected creature)",
            "\n" + "This command fully restores your(or the selected creature's) health, mana, and stamina")]
        public static void HandleHeal(Session session, params string[] parameters)
        {
            // usage: @heal
            // This command fully restores your(or the selected creature's) health, mana, and stamina.
            // @heal - Heals yourself(or the selected creature).

            // TODO: Check if player has a selected target, heal target otherwise heal player.
            session.Player.SetMaxVitals();
        }

        // housekeep
        [CommandHandler("housekeep", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0)]
        public static void HandleHousekeep(Session session, params string[] parameters)
        {
            // @housekeep[never { off | on}] -With no parameters, this command displays the housekeeping info for the selected item.With the 'never' flag, it sets the item to never housekeep, or turns that state off.
            // @housekeep - Queries or sets the housekeeping status for the selected item.

            // TODO: output
        }

        // idlist
        [CommandHandler("idlist", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0,
            "Shows the next ID that will be allocated from GuidManager.")]
        public static void HandleIDlist(Session session, params string[] parameters)
        {
            // @idlist - Shows the next ID that will be allocated from SQL.

            var sysChatMsg = new GameMessageSystemChat(GuidManager.GetIdListCommandOutput(), ChatMessageType.WorldBroadcast);
            session.Network.EnqueueSend(sysChatMsg);
        }

        // gamecastlocalemote <message>
        [CommandHandler("gamecastlocalemote", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1,
            "Sends text to all players within chat range, formatted exactly as entered.",
            "<message>\n" +
            "Sends text to all players within chat range, formatted exactly as entered, with no prefix of any kind.\n" +
            "See Also: @gamecast, @gamecastemote, @gamecastlocal, @gamecastlocalemote.")]
        public static void HandleGameCastLocalEmote(Session session, params string[] parameters)
        {
            // usage: @gamecastlocalemote<message>
            // Sends text to all players within chat range, formatted exactly as entered, with no prefix of any kind.
            // See Also: @gamecast, @gamecastemote, @gamecastlocal, @gamecastlocalemote.
            // @gamecastlocalemote - Sends text to all players within chat range, formatted exactly as entered.

            // Since we only have one server, this command will just call the other one
            HandleGameCastEmote(session, parameters);
        }

        // location
        [CommandHandler("location", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0)]
        public static void HandleLocation(Session session, params string[] parameters)
        {
            // @location - Causes your current location to be continuously displayed on the screen.

            // TODO: output
        }

        // morph
        [CommandHandler("morph", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1,
            "Morphs your bodily form into that of the specified creature. Be careful with this one!",
            "<wcid or weenie class name> [character name]")]
        public static void HandleMorph(Session session, params string[] parameters)
        {
            // @morph - Morphs your bodily form into that of the specified creature. Be careful with this one!

            string weenieClassDescription = parameters[0];
            bool wcid = uint.TryParse(weenieClassDescription, out uint weenieClassId);
            if (wcid)
            {
                if (weenieClassId < 1 && weenieClassId > WEENIE_MAX)
                {
                    session.Network.EnqueueSend(new GameMessageSystemChat($"Not a valid weenie id - must be a number between 1 - {WEENIE_MAX}", ChatMessageType.Broadcast));
                    return;
                }
            }

            ACE.Entity.Models.Weenie weenie;
            if (wcid)
                weenie = DatabaseManager.World.GetCachedWeenie(weenieClassId);
            else
                weenie = DatabaseManager.World.GetCachedWeenie(weenieClassDescription);

            if (weenie == null)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Weenie {weenieClassDescription} not found in database, unable to morph.", ChatMessageType.Broadcast));
                return;
            }

            if (weenie.WeenieType != WeenieType.Creature && weenie.WeenieType != WeenieType.Cow
                && weenie.WeenieType != WeenieType.Admin && weenie.WeenieType != WeenieType.Sentinel && weenie.WeenieType != WeenieType.Vendor
                && weenie.WeenieType != WeenieType.Pet && weenie.WeenieType != WeenieType.CombatPet)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Weenie {weenie.GetProperty(PropertyString.Name)} ({weenieClassDescription}) is of WeenieType.{Enum.GetName(typeof(WeenieType), weenie.WeenieType)} ({weenie.WeenieType}), unable to morph because that is not allowed.", ChatMessageType.Broadcast));
                return;
            }

            session.Network.EnqueueSend(new GameMessageSystemChat($"Morphing you into {weenie.GetProperty(PropertyString.Name)} ({weenieClassDescription})... You will be logged out.", ChatMessageType.Broadcast));

            var guid = GuidManager.NewPlayerGuid();

            var player = new Player(weenie, guid, session.AccountId);

            player.Biota.WeenieType = session.Player.WeenieType;

            var name = string.Join(' ', parameters.Skip(1));
            if (parameters.Length > 1)
            {
                name = name.TrimStart('+').TrimStart(' ').TrimEnd(' ');
                player.Name = name;
                player.Character.Name = name;
            }
            else
                name = weenie.GetProperty(PropertyString.Name);

            DatabaseManager.Shard.IsCharacterNameAvailable(name, isAvailable =>
            {
                if (!isAvailable)
                {
                    CommandHandlerHelper.WriteOutputInfo(session, $"{name} is not available to use for the morphed character, try another name.", ChatMessageType.Broadcast);
                    return;
                }

                player.Location = session.Player.Location;

                player.Character.CharacterOptions1 = session.Player.Character.CharacterOptions1;
                player.Character.CharacterOptions2 = session.Player.Character.CharacterOptions2;

                //var wearables = weenie.GetCreateList((sbyte)DestinationType.Wield);
                var wearables = weenie.PropertiesCreateList.Where(x => x.DestinationType == DestinationType.Wield || x.DestinationType == DestinationType.WieldTreasure).ToList();
                foreach (var wearable in wearables)
                {
                    var weenieOfWearable = DatabaseManager.World.GetCachedWeenie(wearable.WeenieClassId);

                    if (weenieOfWearable == null)
                        continue;

                    var worldObject = WorldObjectFactory.CreateNewWorldObject(weenieOfWearable);

                    if (worldObject == null)
                        continue;

                    if (wearable.Palette > 0)
                        worldObject.PaletteTemplate = wearable.Palette;
                    if (wearable.Shade > 0)
                        worldObject.Shade = wearable.Shade;

                    player.TryEquipObjectWithNetworking(worldObject, worldObject.ValidLocations ?? 0);
                }

                var containables = weenie.PropertiesCreateList.Where(x => x.DestinationType == DestinationType.Contain || x.DestinationType == DestinationType.Shop
                || x.DestinationType == DestinationType.Treasure || x.DestinationType == DestinationType.ContainTreasure || x.DestinationType == DestinationType.ShopTreasure).ToList();
                foreach (var containable in containables)
                {
                    var weenieOfWearable = DatabaseManager.World.GetCachedWeenie(containable.WeenieClassId);

                    if (weenieOfWearable == null)
                        continue;

                    var worldObject = WorldObjectFactory.CreateNewWorldObject(weenieOfWearable);

                    if (worldObject == null)
                        continue;

                    if (containable.Palette > 0)
                        worldObject.PaletteTemplate = containable.Palette;
                    if (containable.Shade > 0)
                        worldObject.Shade = containable.Shade;
                    player.TryAddToInventory(worldObject);
                }

                var possessions = player.GetAllPossessions();
                var possessedBiotas = new Collection<(Biota biota, ReaderWriterLockSlim rwLock)>();
                foreach (var possession in possessions)
                    possessedBiotas.Add((possession.Biota, possession.BiotaDatabaseLock));

                DatabaseManager.Shard.AddCharacterInParallel(player.Biota, player.BiotaDatabaseLock, possessedBiotas, player.Character, player.CharacterDatabaseLock, null);

                PlayerManager.AddOfflinePlayer(player);
                session.Characters.Add(player.Character);

                session.LogOffPlayer();
            });

        }

        // qst
        [CommandHandler("qst", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1,
            "Query, stamp, and erase quests on the targeted player",
            "[list | bestow | erase]\n"
            + "qst list - List the quest flags for the targeted player\n"
            + "qst bestow - Stamps the specific quest flag on the targeted player. If this fails, it's probably because you spelled the quest flag wrong.\n"
            + "qst stamp - Stamps the specific quest flag on the targeted player the specified number of times. If this fails, it's probably because you spelled the quest flag wrong.\n"
            + "qst erase - Erase the specific quest flag from the targeted player. If no quest flag is given, it erases the entire quest table for the targeted player.\n")]
        public static void Handleqst(Session session, params string[] parameters)
        {
            // fellow bestow  stamp erase
            // @qst list[filter]-List the quest flags for the targeted player, if a filter is provided, you will only get quest flags back that have the filter as a substring of the quest name. (Filter IS case sensitive!)
            // @qst erase < quest flag > -Erase the specific quest flag from the targeted player.If no quest flag is given, it erases the entire quest table for the targeted player.
            // @qst erase fellow < quest flag > -Erase a fellowship quest flag.
            // @qst bestow < quest flag > -Stamps the specific quest flag on the targeted player.If this fails, it's probably because you spelled the quest flag wrong.
            // @qst - Query, stamp, and erase quests on the targeted player.
            if (parameters.Length == 0)
            {
                // todo: display help screen
                return;
            }

            var objectId = new ObjectGuid();

            if (session.Player.HealthQueryTarget.HasValue)
                objectId = new ObjectGuid((uint)session.Player.HealthQueryTarget);
            else if (session.Player.HealthQueryTarget.HasValue)
                objectId = new ObjectGuid((uint)session.Player.ManaQueryTarget);
            else if (session.Player.CurrentAppraisalTarget.HasValue)
                objectId = new ObjectGuid((uint)session.Player.CurrentAppraisalTarget);

            var wo = session.Player.CurrentLandblock?.GetObject(objectId);

            if (wo != null && wo is Creature creature)
            {
                if (parameters[0].Equals("list"))
                {
                    var questsHdr = $"Quest Registry for {creature.Name} (0x{creature.Guid}):\n";
                    questsHdr += "================================================\n";
                    var quests = "";
                    foreach (var quest in creature.QuestManager.Quests)
                    {
                        quests += $"Quest Name: {quest.QuestName}\nCompletions: {quest.NumTimesCompleted} | Last Completion: {quest.LastTimeCompleted} ({Common.Time.GetDateTimeFromTimestamp(quest.LastTimeCompleted).ToLocalTime()})\n";
                        var nextSolve = creature.QuestManager.GetNextSolveTime(quest.QuestName);

                        if (nextSolve == TimeSpan.MinValue)
                            quests += "Can Solve: Immediately\n";
                        else if (nextSolve == TimeSpan.MaxValue)
                            quests += "Can Solve: Never again\n";
                        else
                            quests += $"Can Solve: In {nextSolve:%d} days, {nextSolve:%h} hours, {nextSolve:%m} minutes and, {nextSolve:%s} seconds. ({(DateTime.UtcNow + nextSolve).ToLocalTime()})\n";

                        quests += "--====--\n";
                    }

                    session.Player.SendMessage($"{questsHdr}{(quests != "" ? quests : "No quests found.")}");
                    return;
                }

                if (parameters[0].Equals("bestow"))
                {
                    if (parameters.Length < 2)
                    {
                        // delete all quests?
                        // seems unsafe, maybe a confirmation?
                        return;
                    }
                    var questName = parameters[1];
                    if (creature.QuestManager.HasQuest(questName))
                    {
                        session.Player.SendMessage($"{creature.Name} already has {questName}");
                        return;
                    }

                    var canSolve = creature.QuestManager.CanSolve(questName);
                    if (canSolve)
                    {
                        creature.QuestManager.Update(questName);
                        session.Player.SendMessage($"{questName} bestowed on {creature.Name}");
                        return;
                    }
                    else
                    {
                        session.Player.SendMessage($"Couldn't bestow {questName} on {creature.Name}");
                        return;
                    }
                }

                if (parameters[0].Equals("erase"))
                {
                    if (parameters.Length < 2)
                    {
                        // delete all quests?
                        // seems unsafe, maybe a confirmation?
                        session.Player.SendMessage($"You must specify a quest to erase, if you want to erase all quests use the following command: /qst erase *");
                        return;
                    }
                    var questName = parameters[1];

                    if (questName == "*")
                    {
                        creature.QuestManager.EraseAll();
                        session.Player.SendMessage($"All quests erased.");
                        return;
                    }

                    if (!creature.QuestManager.HasQuest(questName))
                    {
                        session.Player.SendMessage($"{questName} not found.");
                        return;
                    }
                    creature.QuestManager.Erase(questName);
                    session.Player.SendMessage($"{questName} erased.");
                    return;
                }

                if (parameters[0].Equals("stamp"))
                {
                    if (parameters.Length < 3)
                    {
                        session.Player.SendMessage($"You must specify a quest to stamp and number completions using the following command: /qst stamp questname number");
                        return;
                    }
                    if (!int.TryParse(parameters[2], out var numCompletions))
                    {
                        session.Player.SendMessage($"{parameters[2]} is not a valid int");
                        return;
                    }
                    var questName = parameters[1];

                    creature.QuestManager.SetQuestCompletions(questName, numCompletions);
                    var quest = creature.QuestManager.GetQuest(questName);
                    if (quest != null)
                    {
                        var numTimesCompleted = quest.NumTimesCompleted;
                        session.Player.SendMessage($"{questName} stamped with {numTimesCompleted} completions.");
                    }
                    else
                    {
                        session.Player.SendMessage($"Couldn't stamp {questName} on {creature.Name}");
                    }
                    return;
                }
            }
            else
            {
                if (wo == null)
                    session.Player.SendMessage($"Selected object (0x{objectId}) not found.");
                else
                    session.Player.SendMessage($"Selected object {wo.Name} (0x{objectId}) is not a creature.");
            }
        }

        // raise
        [CommandHandler("raise", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 2)]
        public static void HandleRaise(Session session, params string[] parameters)
        {
            // @raise - Raises your experience (or the experience in a skill) by the given amount.

            // TODO: output
        }

        // rename <Current Name> <New Name>
        [CommandHandler("rename", AccessLevel.Envoy, CommandHandlerFlag.None, 2,
            "Rename a character. (Do NOT include +'s for admin names)",
            "< Current Name >, < New Name >")]
        public static void HandleRename(Session session, params string[] parameters)
        {
            // @rename <Current Name>, <New Name> - Rename a character. (Do NOT include +'s for admin names)

            if (!string.Join(" ", parameters).Contains(','))
            {
                CommandHandlerHelper.WriteOutputInfo(session, "Error, cannot rename. You must include the old name followed by a comma and then the new name.\n Example: @rename Old Name, New Name", ChatMessageType.Broadcast);
                return;
            }

            var names = string.Join(" ", parameters).Split(",");

            var oldName = names[0].TrimStart(' ').TrimEnd(' ');
            var newName = names[1].TrimStart(' ').TrimEnd(' ');

            if (oldName.StartsWith("+"))
                oldName = oldName.Substring(1);
            if (newName.StartsWith("+"))
                newName = newName.Substring(1);

            newName = newName.First().ToString().ToUpper() + newName.Substring(1);

            var onlinePlayer = PlayerManager.GetOnlinePlayer(oldName);
            var offlinePlayer = PlayerManager.GetOfflinePlayer(oldName);
            if (onlinePlayer != null)
            {
                var success = false;
                DatabaseManager.Shard.IsCharacterNameAvailable(newName, isAvailable =>
                {
                    if (!isAvailable)
                    {
                        CommandHandlerHelper.WriteOutputInfo(session, $"Error, a player named \"{newName}\" already exists.", ChatMessageType.Broadcast);
                        return;
                    }

                    onlinePlayer.Character.Name = newName;
                    onlinePlayer.CharacterChangesDetected = true;
                    onlinePlayer.Name = newName;
                    onlinePlayer.SavePlayerToDatabase();

                    success = true;
                });

                if (success)
                {
                    CommandHandlerHelper.WriteOutputInfo(session, $"Player named \"{oldName}\" renamed to \"{newName}\" succesfully!", ChatMessageType.Broadcast);

                    onlinePlayer.Session.LogOffPlayer();
                }
            }
            else if (offlinePlayer != null)
            {
                var success = false;
                DatabaseManager.Shard.IsCharacterNameAvailable(newName, isAvailable =>
                {
                    if (!isAvailable)
                    {
                        CommandHandlerHelper.WriteOutputInfo(session, $"Error, a player named \"{newName}\" already exists.", ChatMessageType.Broadcast);
                        return;
                    }

                    var character = DatabaseManager.Shard.BaseDatabase.GetCharacterStubByName(oldName);

                    character.Name = newName;
                    DatabaseManager.Shard.SaveCharacter(character, new ReaderWriterLockSlim(), null);
                    offlinePlayer.SetProperty(PropertyString.Name, newName);
                    offlinePlayer.SaveBiotaToDatabase();

                    success = true;
                });

                if (success)
                    CommandHandlerHelper.WriteOutputInfo(session, $"Player named \"{oldName}\" renamed to \"{newName}\" succesfully!", ChatMessageType.Broadcast);
            }
            else
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Error, a player named \"{oldName}\" cannot be found.", ChatMessageType.Broadcast);
                return;
            }

        }

        // setadvclass
        [CommandHandler("setadvclass", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 2)]
        public static void Handlesetadvclass(Session session, params string[] parameters)
        {
            // @setadvclass - Sets the advancement class of one of your own skills.

            // TODO: output
        }

        // spendxp
        [CommandHandler("spendxp", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 2)]
        public static void HandleSpendxp(Session session, params string[] parameters)
        {
            // @spendxp - Allows you to more quickly spend your available xp into the specified skill.

            // TODO: output
        }

        // trainskill
        [CommandHandler("trainskill", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1)]
        public static void Handletrainskill(Session session, params string[] parameters)
        {
            // @trainskill - Attempts to train the specified skill by spending skill credits on it.

            // TODO: output
        }

        // reloadsysmsg
        [CommandHandler("reloadsysmsg", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0)]
        public static void Handlereloadsysmsg(Session session, params string[] parameters)
        {
            // @reloadsysmsg - Causes all servers to reload system_messages.txt.

            // TODO: output
        }

        // gamecastlocal <message>
        [CommandHandler("gamecastlocal", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1,
            "Sends a server-wide broadcast.",
            "<message>\n" +
            "This command sends the specified text to every player on the current server.\n" +
            "See Also: @gamecast, @gamecastemote, @gamecastlocal, @gamecastlocalemote.")]
        public static void HandleGameCastLocal(Session session, params string[] parameters)
        {
            // Local Server Broadcast from
            // usage: @gamecastlocal<message>
            // This command sends the specified text to every player on the current server.
            // See Also: @gamecast, @gamecastemote, @gamecastlocal, @gamecastlocalemote.
            // @gamecastlocal Sends a server-wide broadcast.

            // Since we only have one server, this command will just call the other one
            HandleGamecast(session, parameters);
        }

        /// <summary>
        /// Sets whether you lose items should you die.
        /// </summary>
        [CommandHandler("sticky", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, "Sets whether you lose items should you die.", "<off/on>")]
        public static void HandleSticky(Session session, params string[] parameters)
        {
            bool sticky = !(parameters.Length > 0 && parameters[0] == "off");

            if (sticky)
                CommandHandlerHelper.WriteOutputInfo(session, "You will no longer drop any items on death.");
            else
                CommandHandlerHelper.WriteOutputInfo(session, "You will now drop items on death normally.");

            session.Player.NoCorpse = sticky;
        }

        // userlimit { num }
        [CommandHandler("userlimit", AccessLevel.Admin, CommandHandlerFlag.RequiresWorld, 1)]
        public static void Handleuserlimit(Session session, params string[] parameters)
        {
            // @userlimit - Sets how many clients are allowed to connect to this world.

            // TODO: output
        }

        // watchmen
        [CommandHandler("watchmen", AccessLevel.Admin, CommandHandlerFlag.None, 0,
            "Displays a list of accounts with the specified level of admin access.",
            "(accesslevel)")]
        public static void Handlewatchmen(Session session, params string[] parameters)
        {
            // @watchmen - Displays a list of accounts with the specified level of admin access.

            var defaultAccessLevel = AccessLevel.Advocate;

            var accessLevel = defaultAccessLevel;

            if (parameters.Length > 0)
            {
                if (Enum.TryParse(parameters[0], true, out accessLevel))
                {
                    if (!Enum.IsDefined(typeof(AccessLevel), accessLevel))
                        accessLevel = defaultAccessLevel;
                }
            }

            var list = DatabaseManager.Authentication.GetListofAccountsByAccessLevel(accessLevel);
            var message = "";

            if (list.Count > 0)
            {
                message = $"The following accounts have been granted {accessLevel.ToString()} rights:\n";
                foreach (var item in list)
                    message += item + "\n";
            }
            else
            {
                message = $"There are no accounts with {accessLevel.ToString()} rights.";
            }

            CommandHandlerHelper.WriteOutputInfo(session, message, ChatMessageType.WorldBroadcast);
        }

        // gamecastemote <message>
        [CommandHandler("gamecastemote", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1,
            "Sends text to all players, formatted exactly as entered.",
            "<message>\n" +
            "See Also: @gamecast, @gamecastemote, @gamecastlocal, @gamecastlocalemote.")]
        public static void HandleGameCastEmote(Session session, params string[] parameters)
        {
            // usage: "@gamecastemote <message>" or "@we <message"
            // Sends text to all players, formatted exactly as entered, with no prefix of any kind.
            // See Also: @gamecast, @gamecastemote, @gamecastlocal, @gamecastlocalemote.
            // @gamecastemote - Sends text to all players, formatted exactly as entered.

            string msg = string.Join(" ", parameters);
            msg = msg.Replace("\\n", "\n");
            session.Player.HandleActionWorldBroadcast($"{msg}", ChatMessageType.WorldBroadcast);
        }

        // we <message>
        [CommandHandler("we", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1,
            "Sends text to all players, formatted exactly as entered.",
            "<message>\n" +
            "See Also: @gamecast, @gamecastemote, @gamecastlocal, @gamecastlocalemote.")]
        public static void HandleWe(Session session, params string[] parameters)
        {
            // usage: "@gamecastemote <message>" or "@we <message"
            // Sends text to all players, formatted exactly as entered, with no prefix of any kind.
            // See Also: @gamecast, @gamecastemote, @gamecastlocal, @gamecastlocalemote.
            // @gamecastemote - Sends text to all players, formatted exactly as entered.

            HandleGameCastEmote(session, parameters);
        }

        // dumpattackers
        [CommandHandler("dumpattackers", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0)]
        public static void Handledumpattackers(Session session, params string[] parameters)
        {
            // @dumpattackers - Displays the detection and enemy information for the selected creature.

            // TODO: output
        }

        // knownobjs
        [CommandHandler("knownobjs", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0)]
        public static void Handleknownobjs(Session session, params string[] parameters)
        {
            // @knownobjs - Display a list of objects that the client is aware of.

            // TODO: output
        }

        // lbinterval
        [CommandHandler("lbinterval", AccessLevel.Admin, CommandHandlerFlag.RequiresWorld, 1)]
        public static void Handlelbinterval(Session session, params string[] parameters)
        {
            // @lbinterval - Sets how often in seconds the server farm will rebalance the server farm load.

            // TODO: output
        }

        // lbthresh
        [CommandHandler("lbthresh", AccessLevel.Admin, CommandHandlerFlag.RequiresWorld, 1)]
        public static void Handlelbthresh(Session session, params string[] parameters)
        {
            // the @lbthresh command sets the maximum amount of load servers can trade at each balance. Large load transfers at once can cause poor server performance.  (Large would be about 400, small is about 20.)
            // @lbthresh - Set how much load can be transferred between two servers during a single load balance.

            // TODO: output
        }

        // radar
        [CommandHandler("radar", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0)]
        public static void Handleradar(Session session, params string[] parameters)
        {
            // @radar - Toggles your radar on and off.

            // TODO: output
        }

        // rares dump
        [CommandHandler("rares dump", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0)]
        public static void HandleRaresDump(Session session, params string[] parameters)
        {
            // @rares dump - Lists all tiers of rare items.

            // TODO: output
        }

        // stormnumstormed
        [CommandHandler("stormnumstormed", AccessLevel.Admin, CommandHandlerFlag.RequiresWorld, 1)]
        public static void Handlestormnumstormed(Session session, params string[] parameters)
        {
            // @stormnumstormed - Sets how many characters are teleported away during a portal storm.

            // TODO: output
        }

        // stormthresh
        [CommandHandler("stormthresh", AccessLevel.Admin, CommandHandlerFlag.RequiresWorld, 1)]
        public static void Handlestormthresh(Session session, params string[] parameters)
        {
            // @stormthresh - Sets how many character can be in a landblock before we do a portal storm.

            // TODO: output
        }

        [CommandHandler("showprops", AccessLevel.Admin, CommandHandlerFlag.None, 0, "Displays the name of all properties configurable via the modify commands")]
        public static void HandleDisplayProps(Session session, params string[] paramters)
        {
            CommandHandlerHelper.WriteOutputInfo(session, PropertyManager.ListProperties());
        }

        [CommandHandler("modifybool", AccessLevel.Admin, CommandHandlerFlag.None, 2, "Modifies a server property that is a bool", "modifybool (string) (bool)")]
        public static void HandleModifyServerBoolProperty(Session session, params string[] paramters)
        {
            try
            {
                var boolVal = bool.Parse(paramters[1]);

                var prevState = PropertyManager.GetBool(paramters[0]);

                if (prevState.Item == boolVal && !string.IsNullOrWhiteSpace(prevState.Description))
                {
                    CommandHandlerHelper.WriteOutputInfo(session, $"Bool property is already {boolVal} for {paramters[0]}!");
                    return;
                }

                if (PropertyManager.ModifyBool(paramters[0], boolVal))
                {
                    CommandHandlerHelper.WriteOutputInfo(session, "Bool property successfully updated!");

                    if (paramters[0] == "pk_server" || paramters[0] == "pkl_server")
                    {
                        PlayerManager.UpdatePKStatusForAllPlayers(paramters[0], boolVal);
                    }
                }
                else
                    CommandHandlerHelper.WriteOutputInfo(session, "Unknown bool property was not updated. Type showprops for a list of properties.");
            }
            catch (Exception)
            {
                CommandHandlerHelper.WriteOutputInfo(session, "Please input a valid bool", ChatMessageType.Help);
            }
        }

        [CommandHandler("fetchbool", AccessLevel.Admin, CommandHandlerFlag.None, 1, "Fetches a server property that is a bool", "fetchbool (string)")]
        public static void HandleFetchServerBoolProperty(Session session, params string[] paramters)
        {
            var boolVal = PropertyManager.GetBool(paramters[0], cacheFallback: false);
            CommandHandlerHelper.WriteOutputInfo(session, $"{paramters[0]} - {boolVal.Description ?? "No Description"}: {boolVal.Item}");
        }

        [CommandHandler("modifylong", AccessLevel.Admin, CommandHandlerFlag.None, 2, "Modifies a server property that is a long", "modifylong (string) (long)")]
        public static void HandleModifyServerLongProperty(Session session, params string[] paramters)
        {
            try
            {
                var intVal = int.Parse(paramters[1]);
                if (PropertyManager.ModifyLong(paramters[0], intVal))
                    CommandHandlerHelper.WriteOutputInfo(session, "Long property successfully updated!");
                else
                    CommandHandlerHelper.WriteOutputInfo(session, "Unknown long property was not updated. Type showprops for a list of properties.");
            }
            catch (Exception)
            {
                CommandHandlerHelper.WriteOutputInfo(session, "Please input a valid long", ChatMessageType.Help);
            }
        }

        [CommandHandler("fetchlong", AccessLevel.Admin, CommandHandlerFlag.None, 1, "Fetches a server property that is a long", "fetchlong (string)")]
        public static void HandleFetchServerLongProperty(Session session, params string[] paramters)
        {
            var intVal = PropertyManager.GetLong(paramters[0], cacheFallback: false);
            CommandHandlerHelper.WriteOutputInfo(session, $"{paramters[0]} - {intVal.Description ?? "No Description"}: {intVal.Item}");
        }

        [CommandHandler("modifydouble", AccessLevel.Admin, CommandHandlerFlag.None, 2, "Modifies a server property that is a double", "modifyfloat (string) (double)")]
        public static void HandleModifyServerFloatProperty(Session session, params string[] paramters)
        {
            try
            {
                var floatVal = float.Parse(paramters[1]);
                if (PropertyManager.ModifyDouble(paramters[0], floatVal))
                    CommandHandlerHelper.WriteOutputInfo(session, "Double property successfully updated!");
                else
                    CommandHandlerHelper.WriteOutputInfo(session, "Unknown double property was not updated. Type showprops for a list of properties.");
            }
            catch (Exception)
            {
                CommandHandlerHelper.WriteOutputInfo(session, "Please input a valid double", ChatMessageType.Help);
            }
        }

        [CommandHandler("fetchdouble", AccessLevel.Admin, CommandHandlerFlag.None, 1, "Fetches a server property that is a double", "fetchdouble (string)")]
        public static void HandleFetchServerFloatProperty(Session session, params string[] paramters)
        {
            var floatVal = PropertyManager.GetDouble(paramters[0], cacheFallback: false);
            CommandHandlerHelper.WriteOutputInfo(session, $"{paramters[0]} - {floatVal.Description ?? "No Description"}: {floatVal.Item}");
        }

        [CommandHandler("modifystring", AccessLevel.Admin, CommandHandlerFlag.None, 2, "Modifies a server property that is a string", "modifystring (string) (string)")]
        public static void HandleModifyServerStringProperty(Session session, params string[] parameters)
        {
            if (PropertyManager.ModifyString(parameters[0], parameters[1]))
                CommandHandlerHelper.WriteOutputInfo(session, "String property successfully updated!");
            else
                CommandHandlerHelper.WriteOutputInfo(session, "Unknown string property was not updated. Type showprops for a list of properties.");
        }

        [CommandHandler("fetchstring", AccessLevel.Admin, CommandHandlerFlag.None, 1, "Fetches a server property that is a string", "fetchstring (string)")]
        public static void HandleFetchServerStringProperty(Session session, params string[] parameters)
        {
            var stringVal = PropertyManager.GetString(parameters[0], cacheFallback: false);
            CommandHandlerHelper.WriteOutputInfo(session, $"{parameters[0]} - {stringVal.Description ?? "No Description"}: {stringVal.Item}");
        }

        [CommandHandler("modifypropertydesc", AccessLevel.Admin, CommandHandlerFlag.None, 3, "Modifies a server property's description", "modifypropertydesc <STRING|BOOL|DOUBLE|LONG> (string) (string)")]
        public static void HandleModifyPropertyDescription(Session session, params string[] parameters)
        {
            var isSession = session != null;
            switch (parameters[0])
            {
                case "STRING":
                    PropertyManager.ModifyStringDescription(parameters[1], parameters[2]);
                    break;
                case "BOOL":
                    PropertyManager.ModifyBoolDescription(parameters[1], parameters[2]);
                    break;
                case "DOUBLE":
                    PropertyManager.ModifyDoubleDescription(parameters[1], parameters[2]);
                    break;
                case "LONG":
                    PropertyManager.ModifyLongDescription(parameters[1], parameters[2]);
                    break;
                default:
                    CommandHandlerHelper.WriteOutputInfo(session, "Please pick from STRING, BOOL, DOUBLE, or LONG", ChatMessageType.Help);
                    return;
            }

            CommandHandlerHelper.WriteOutputInfo(session, "Successfully updated property description!", ChatMessageType.Help);
        }

        [CommandHandler("resyncproperties", AccessLevel.Admin, CommandHandlerFlag.None, "Resync the properties database")]
        public static void HandleResyncServerProperties(Session session, params string[] parameters)
        {
            PropertyManager.ResyncVariables();
        }

        [CommandHandler("fix-allegiances", AccessLevel.Admin, CommandHandlerFlag.ConsoleInvoke, "Fixes the monarch data for allegiances")]
        public static void HandleFixAllegiances(Session session, params string[] parameters)
        {
            var players = PlayerManager.GetAllPlayers();

            // build allegiances
            foreach (var player in players)
                AllegianceManager.GetAllegiance(player);

            foreach (var player in players.Where(i => i.MonarchId != null))
            {
                var monarchID = player.MonarchId.Value;

                // find multi allegiances
                foreach (var allegiance in AllegianceManager.Allegiances.Values)
                {
                    if (allegiance.MonarchId == monarchID)
                        continue;

                    if (allegiance.Members.TryGetValue(new ObjectGuid(monarchID), out var member))
                    {
                        var desynced = PlayerManager.FindByGuid(monarchID);
                        Console.WriteLine($"{player.Name} has references to {desynced.Name} as monarch, but should be {allegiance.Monarch.Player.Name} -- fixing");

                        player.MonarchId = allegiance.MonarchId;
                        player.SaveBiotaToDatabase();
                    }
                }

                // find missing players
                var monarch = PlayerManager.FindByGuid(player.MonarchId.Value);
                var _allegiance = AllegianceManager.GetAllegiance(monarch);

                if (_allegiance != null && !_allegiance.Members.ContainsKey(player.Guid))
                {
                    // walk patrons to get the updated monarch
                    var patron = PlayerManager.FindByGuid(player.PatronId.Value);
                    if (patron == null)
                    {
                        Console.WriteLine($"{player.Name} has references to deleted patron {player.PatronId.Value:X8}, checking for vassals");
                        player.PatronId = null;

                        var vassals = players.Where(i => i.PatronId != null && i.PatronId == player.Guid.Full).ToList();
                        if (vassals.Count > 0)
                        {
                            Console.WriteLine($"Vassals found, {player.Name} is the monarch");
                            player.MonarchId = player.Guid.Full;
                        }
                        else
                        {
                            Console.WriteLine($"No vassals found, removing patron reference to deleted character");
                            player.MonarchId = null;
                        }
                        player.SaveBiotaToDatabase();
                        continue;
                    }

                    while (patron.PatronId != null)
                        patron = PlayerManager.FindByGuid(patron.PatronId.Value);

                    if (player.MonarchId != patron.Guid.Full)
                    {
                        Console.WriteLine($"{player.Name} has references to {monarch.Name} as monarch, but should be {patron.Name} -- fixing missing player");

                        player.MonarchId = patron.Guid.Full;
                        player.SaveBiotaToDatabase();
                    }
                }
            }

            foreach (var allegiance in AllegianceManager.Allegiances.Values.ToList())
                AllegianceManager.Rebuild(allegiance);
        }

        [CommandHandler("show-allegiances", AccessLevel.Admin, CommandHandlerFlag.None, "Shows all of the allegiance chains on the server.")]
        public static void HandleShowAllegiances(Session session, params string[] parameters)
        {
            var players = PlayerManager.GetAllPlayers();

            // build allegiances
            foreach (var player in players)
                AllegianceManager.GetAllegiance(player);

            foreach (var allegiance in AllegianceManager.Allegiances.Values)
            {
                allegiance.ShowInfo();
                Console.WriteLine("---------------");
            }
        }

        [CommandHandler("getenchantments", AccessLevel.Admin, CommandHandlerFlag.RequiresWorld, "Shows the enchantments for the last appraised item")]
        public static void HandleGetEnchantments(Session session, params string[] parameters)
        {
            var item = CommandHandlerHelper.GetLastAppraisedObject(session);
            if (item == null) return;

            var enchantments = item.Biota.PropertiesEnchantmentRegistry.GetEnchantmentsTopLayer(item.BiotaDatabaseLock);

            foreach (var enchantment in enchantments)
            {
                var e = new Network.Structure.Enchantment(item, enchantment);
                var info = e.GetInfo();
                session.Network.EnqueueSend(new GameMessageSystemChat(info, ChatMessageType.Broadcast));
            }
        }

        // cm <material type> <quantity> <ave. workmanship>
        [CommandHandler("cm", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1, "Create a salvage bag in your inventory", "<material_type>, optional: <structure> <workmanship> <num_items>")]
        public static void HandleCM(Session session, params string[] parameters)
        {
            // Format is: @cm <material type> <quantity> <ave. workmanship>
            HandleCISalvage(session);
        }

        [CommandHandler("cisalvage", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1, "Create a salvage bag in your inventory", "<material_type>, optional: <structure> <workmanship> <num_items>")]
        public static void HandleCISalvage(Session session, params string[] parameters)
        {
            if (!Enum.TryParse(parameters[0], true, out MaterialType materialType))
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Couldn't find material type {parameters[0]}", ChatMessageType.Broadcast));
                return;
            }

            var wcid = (uint)Player.MaterialSalvage[(int)materialType];
            var salvageBag = WorldObjectFactory.CreateNewWorldObject(wcid);

            ushort structure = 100;
            if (parameters.Length > 1)
                ushort.TryParse(parameters[1], out structure);

            var workmanship = 10f;
            if (parameters.Length > 2)
                float.TryParse(parameters[2], out workmanship);

            var numItemsInMaterial = (int)Math.Round(workmanship);
            if (parameters.Length > 3)
                int.TryParse(parameters[3], out numItemsInMaterial);

            var itemWorkmanship = (int)Math.Round(workmanship * numItemsInMaterial);

            salvageBag.Name = $"Salvage ({structure})";
            salvageBag.Structure = structure;
            salvageBag.ItemWorkmanship = itemWorkmanship;
            salvageBag.NumItemsInMaterial = numItemsInMaterial;

            session.Player.TryCreateInInventoryWithNetworking(salvageBag);
        }

        [CommandHandler("setlbenviron", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0,
            "Sets or clears your current landblock's environment option",
            "(name or id of EnvironChangeType)\nleave blank to reset to default.\nlist to get a complete list of options.")]
        public static void HandleSetLBEnviron(Session session, params string[] parameters)
        {
            EnvironChangeType environChange = EnvironChangeType.Clear;

            if (parameters.Length > 0)
            {
                if (parameters[0] == "list")
                {
                    session.Network.EnqueueSend(new GameMessageSystemChat(EnvironListMsg(), ChatMessageType.Broadcast));
                    return;
                }

                if (Enum.TryParse(parameters[0], true, out environChange))
                {
                    if (!Enum.IsDefined(typeof(EnvironChangeType), environChange))
                        environChange = EnvironChangeType.Clear;
                }
            }

            if (environChange.IsFog())
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Setting Landblock (0x{session.Player.CurrentLandblock.Id.Landblock:X4}), including direct adjacent landblocks, to EnvironChangeType.{environChange.ToString()}.", ChatMessageType.Broadcast));
                PlayerManager.BroadcastToAuditChannel(session.Player, $"{session.Player.Name} set Landblock (0x{session.Player.CurrentLandblock.Id.Landblock:X4}), including direct adjacent landblocks, to EnvironChangeType.{environChange.ToString()}.");
            }
            else
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Sending EnvironChangeType.{environChange.ToString()} to all players on Landblock (0x{session.Player.CurrentLandblock.Id.Landblock:X4}), including direct adjacent landblocks.", ChatMessageType.Broadcast));
                PlayerManager.BroadcastToAuditChannel(session.Player, $"{session.Player.Name} sent EnvironChangeType.{environChange.ToString()} to all players on Landblock (0x{session.Player.CurrentLandblock.Id.Landblock:X4}), including direct adjacent landblocks.");
            }

            session.Player.CurrentLandblock?.DoEnvironChange(environChange);
        }

        [CommandHandler("setglobalenviron", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0,
            "Sets or clears server's global environment option",
            "(name or id of EnvironChangeType)\nleave blank to reset to default.\nlist to get a complete list of options.")]
        public static void HandleSetGlobalEnviron(Session session, params string[] parameters)
        {
            EnvironChangeType environChange = EnvironChangeType.Clear;

            if (parameters.Length > 0)
            {
                if (parameters[0] == "list")
                {
                    session.Network.EnqueueSend(new GameMessageSystemChat(EnvironListMsg(), ChatMessageType.Broadcast));
                    return;
                }

                if (Enum.TryParse(parameters[0], true, out environChange))
                {
                    if (!Enum.IsDefined(typeof(EnvironChangeType), environChange))
                        environChange = EnvironChangeType.Clear;
                }
            }

            if (environChange.IsFog())
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Setting all landblocks to EnvironChangeType.{environChange.ToString()} .", ChatMessageType.Broadcast));
                PlayerManager.BroadcastToAuditChannel(session.Player, $"{session.Player.Name} set all landblocks to EnvironChangeType.{environChange.ToString()} .");
            }
            else
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Sending EnvironChangeType.{environChange.ToString()} to all players on all Landblocks.", ChatMessageType.Broadcast));
                PlayerManager.BroadcastToAuditChannel(session.Player, $"{session.Player.Name} sent EnvironChangeType.{environChange.ToString()} to all players on all Landblocks.");
            }

            LandblockManager.DoEnvironChange(environChange);
        }

        private static string EnvironListMsg()
        {
            var msg = "Complete list of EnvironChangeType:\n";
            foreach (var name in Enum.GetNames(typeof(EnvironChangeType)))
                msg += name + "\n";

            msg += "Notes about above list:\n";
            msg += "Clear resets to default.\nAll options ending with Fog are continuous.\nAll options ending with Fog2 are continuous and blank radar.\nAll options ending with Sound play once and do not repeat.";

            return msg;
        }

        [CommandHandler("movetome", AccessLevel.Admin, CommandHandlerFlag.RequiresWorld, "Moves the last appraised object to the current player location.")]
        public static void HandleMoveToMe(Session session, params string[] parameters)
        {
            var obj = CommandHandlerHelper.GetLastAppraisedObject(session);

            if (obj == null)
                return;

            if (obj.CurrentLandblock == null)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"{obj.Name} ({obj.Guid}) is not a landblock object", ChatMessageType.Broadcast));
                return;
            }

            if (obj is Player)
            {
                HandleTeleToMe(session, new string[] { obj.Name });
                return;
            }

            var prevLoc = obj.Location;
            var newLoc = new Position(session.Player.Location);
            newLoc.Rotation = prevLoc.Rotation;     // keep previous rotation

            var setPos = new Physics.Common.SetPosition(newLoc.PhysPosition(), Physics.Common.SetPositionFlags.Teleport | Physics.Common.SetPositionFlags.Slide);
            var result = obj.PhysicsObj.SetPosition(setPos);

            if (result != Physics.Common.SetPositionError.OK)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Failed to move {obj.Name} ({obj.Guid}) to current location: {result}", ChatMessageType.Broadcast));
                return;

            }
            session.Network.EnqueueSend(new GameMessageSystemChat($"Moving {obj.Name} ({obj.Guid}) to current location", ChatMessageType.Broadcast));

            obj.Location = obj.PhysicsObj.Position.ACEPosition();

            if (prevLoc.Landblock != obj.Location.Landblock)
            {
                LandblockManager.RelocateObjectForPhysics(obj, true);
            }

            obj.SendUpdatePosition(true);
        }
    }
}
