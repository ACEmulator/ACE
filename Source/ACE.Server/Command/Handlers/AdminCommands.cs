using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading;

using log4net;

using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.DatLoader;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Factories;
using ACE.Server.Managers;
using ACE.Server.Network;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.WorldObjects;

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
            "Allows the admin to see admin - only visible items.", "{ on | off | toggle | check }\n" +
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

        // ban < acct > < days > < hours > < minutes >
        [CommandHandler("ban", AccessLevel.Sentinel, CommandHandlerFlag.RequiresWorld, 4)]
        public static void HandleBanAccount(Session session, params string[] parameters)
        {
            // usage: @ban < acct > < days > < hours > < minutes >
            // This command bans the specified player account for the specified time.This player will not be able to enter the game with any character until the time expires.
            // @ban - Bans the specified player account.

            // TODO: output
        }

        // unban < acct >
        [CommandHandler("unban", AccessLevel.Sentinel, CommandHandlerFlag.RequiresWorld, 1)]
        public static void HandleUnBanAccount(Session session, params string[] parameters)
        {
            // usage: @unban acct
            // This command removes the ban from the specified account.The player will then be able to log into the game.
            // @unban - Unbans the specified player account.

            // TODO: output
        }

        // banlist
        [CommandHandler("banlist", AccessLevel.Sentinel, CommandHandlerFlag.RequiresWorld, 0)]
        public static void HandleBanlist(Session session, params string[] parameters)
        {
            // @banlist - Lists all banned accounts on this world.

            // TODO: output
        }

        /// <summary>
        /// Boots the Player or Account holder from the server and displays the CoC Violation Warning
        /// </summary>
        /// <remarks>
        ///     TODO: 1. After the group messages are operational, Send out a message on the Audit Group Chat Channel and alert other admins of this command usage.
        /// </remarks>
        [CommandHandler("boot", AccessLevel.Sentinel, CommandHandlerFlag.None, 2,
            "Boots the Player or Account holder from the server and displays the CoC Violation Warning",
            "{ subscriptionId | char | iid } who")]
        public static void HandleBoot(Session session, params string[] parameters)
        {
            // usage: @boot { account, char, iid} who
            // This command boots the specified character out of the game.You can specify who to boot by account, character name, or player instance id.  'who' is the account / character / instance id to actually boot.
            // @boot - Boots the character out of the game.
            // The first parameter may be only text
            // The second paramater and proceeding parameters may also be a number or text, but the logic depends on the text from the first parameter.
            AccountLookupType selectorType = AccountLookupType.Undef;
            string bootName = "";
            uint bootId = 0;
            Session playerSession = null;

            // Loop through the AccountLookupEnum to attempt at matching the first parameter with a lookup type
            foreach (string bootType in System.Enum.GetNames(typeof(AccountLookupType)))
            {
                // Check the FIRST character of the first parameter against the Enum dictionary
                // If the first character matches (a,c,i) then the selector will be returned.
                // This allows for users to type @boot char <name>, since char is a keyword in c#
                // Switch case to Lower when matching
                if (parameters[0].ToLower()[0] == bootType.ToLower()[0])
                {
                    // If found, selectorType will hold the correct AccoutLookupType
                    // If this returns true, that means we were successful and can stop looping
                    if (Enum.TryParse(bootType, out selectorType))
                        break;
                }
            }

            // Peform logic
            if (selectorType != AccountLookupType.Undef)
            {
                // Extract the name from the parameters and get the name from the first parameter
                if (selectorType == AccountLookupType.Subscription || selectorType == AccountLookupType.Character)
                    bootName = Common.Extensions.CharacterNameExtensions.StringArrayToCharacterName(parameters, 1);

                switch (selectorType)
                {
                    case AccountLookupType.Subscription:
                        {
                            // Send the error to a player or the console
                            CommandHandlerHelper.WriteOutputInfo(session, "Boot by Subscription not implemented.", ChatMessageType.Broadcast);
                            return;
                        }
                    case AccountLookupType.Character:
                        {
                            var player = PlayerManager.GetOnlinePlayer(bootName);
                            if (player != null)
                            {
                                playerSession = player.Session;
                                bootId = player.Guid.Full;
                            }
                            break;
                        }
                    case AccountLookupType.Iid:
                        {
                            // Extract the Id from the parameters
                            uint.TryParse(parameters[1], out bootId);
                            var targetPlayer = PlayerManager.GetOnlinePlayer(bootId);
                            if (targetPlayer != null)
                            {
                                playerSession = targetPlayer.Session;
                                bootName = targetPlayer.Name;
                            }
                            break;
                        }
                }

                if (playerSession != null)
                {
                    string bootText = $"account or player: {bootName} id: {bootId}";
                    // Boot the player
                    playerSession.BootSession("Account Booted", new GameMessageBootAccount(playerSession));

                    // Send an update to the admin, but prevent sending too if the admin was the player being booted
                    if (session != null)
                    {
                        // Log the player who initiated the boot
                        bootText = "Player: " + session.Player.Name + " has booted " + bootText;
                        if (session != playerSession)
                            session.Network.EnqueueSend(new GameMessageSystemChat(bootText, ChatMessageType.Broadcast));
                        // TODO: Send out a message on the Audit Group Chat Channel, to alert other admins of the boot
                    }
                    else
                    {
                        bootText = "Console booted " + bootText;
                    }

                    // log the boot to file
                    log.Info(bootText);

                    // finish execution of command logic
                    return;
                }
            }

            // Did not find a player
            // Send the error to a player or the console
            CommandHandlerHelper.WriteOutputInfo(session, "Error locating the player or account to boot.", ChatMessageType.Broadcast);
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

        // delete
        [CommandHandler("delete", AccessLevel.Envoy, CommandHandlerFlag.RequiresWorld, 0,
             "Deletes the selected object.",
            "Players may not be deleted this way.")]
        public static void HandleDeleteSelected(Session session, params string[] parameters)
        {
            // @delete - Deletes the selected object. Players may not be deleted this way.

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

                if (wo != null)
                    wo.Destroy();
            }
        }

        // draw
        [CommandHandler("draw", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0)]
        public static void HandleDraw(Session session, params string[] parameters)
        {
            // @draw - Draws undrawable things.

            // TODO: output
        }

        // finger [ [-a] character] [-m account]
        [CommandHandler("finger", AccessLevel.Sentinel, CommandHandlerFlag.RequiresWorld, 1)]
        public static void HandleFinger(Session session, params string[] parameters)
        {
            // usage: @finger[ [-a] character] [-m account]
            // Given a character name, this command displays the name of the owning account.If the -m option is specified, the argument is considered an account name and the characters owned by that account are displayed.If the -a option is specified, then the character name is fingered but their account is implicitly fingered as well.
            // @finger - Show the given character's account name or vice-versa.

            // TODO: output
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
        [CommandHandler("gag", AccessLevel.Sentinel, CommandHandlerFlag.RequiresWorld, 1)]
        public static void HandleGag(Session session, params string[] parameters)
        {
            // usage: @gag < char name >
            // This command gags the specified character for five minutes.  The character will not be able to @tell or use chat normally.
            // @gag - Prevents a character from talking.
            // @ungag -Allows a gagged character to talk again.

            // TODO: output
        }

        // ungag < char name >
        [CommandHandler("ungag", AccessLevel.Sentinel, CommandHandlerFlag.RequiresWorld, 1)]
        public static void HandleUnGag(Session session, params string[] parameters)
        {
            // usage: @ungag < char name >
            // @ungag -Allows a gagged character to talk again.

            // TODO: output
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
            "Displays your Instance ID(IID)")]
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
        [CommandHandler("pk", AccessLevel.Sentinel, CommandHandlerFlag.RequiresWorld, 0)]
        public static void HandlePk(Session session, params string[] parameters)
        {
            // @pk - Toggles or sets your own PK state.

            // TODO: output
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
        [CommandHandler("regen", AccessLevel.Envoy, CommandHandlerFlag.RequiresWorld, 2)]
        public static void HandleRegen(Session session, params string[] parameters)
        {
            // @regen - Sends the selected generator a regeneration message.

            // TODO: output
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
                    foreach (var wo in session.Player.GetKnownObjects())
                    {
                        if (wo is Player) // I don't recall if @smite all would kill players in range, assuming it didn't
                            continue;

                        if (wo is Creature creature && creature.IsAttackable())
                            creature.Smite(session.Player);
                    }
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
                        player.Smite(session.Player);
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
                        wo.Smite(session.Player);
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
            player.Teleport(session.Player.Location);
            player.Session.Network.EnqueueSend(new GameMessageSystemChat($"{session.Player.Name} has teleported you to their current location.", ChatMessageType.Broadcast));
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
        }

        // telepoi location
        [CommandHandler("telepoi", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1,
            "Teleport yourself to a named Point of Interest",
            "[POI|list]\n" +
            "@telepoi Arwic\n"+
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
                session.Player.AdjustDungeon(portalDest);
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

            String messageUTC = "The current server time in UtcNow is: " + DateTime.UtcNow;
            String messagePY = "The current server time in DerethDateTime is: " + Timers.CurrentLoreTime;

            CommandHandlerHelper.WriteOutputInfo(session, messageUTC, ChatMessageType.Broadcast);
            CommandHandlerHelper.WriteOutputInfo(session, messagePY, ChatMessageType.WorldBroadcast);
        }

        // trophies
        [CommandHandler("trophies", AccessLevel.Envoy, CommandHandlerFlag.RequiresWorld, 0)]
        public static void HandleTrophies(Session session, params string[] parameters)
        {
            // @trophies - Shows a list of the trophies dropped by the target creature, and the percentage chance of dropping.

            // TODO: output
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
        public static void HandleAdd(Session session, params string[] parameters)
        {
            if (Enum.TryParse(parameters[0], true, out SpellId spellId))
            {
                if (Enum.IsDefined(typeof(SpellId), spellId))
                    session.Player.LearnSpellWithNetworking((uint)spellId);
            }
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

        public const uint WEENIE_MAX = 199999;
        // create wclassid (number)
        [CommandHandler("create", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1,
            "Creates an object in the world.", "wclassid (string or number)")]
        public static void HandleCreate(Session session, params string[] parameters)
        {
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
            int palette = 0;
            float shade = 0;
            int stackSize = 1;
            if (parameters.Length > 1)
                if (!int.TryParse(parameters[1], out palette))
                {
                    session.Network.EnqueueSend(new GameMessageSystemChat($"palette must be number between {int.MinValue} - {int.MaxValue}", ChatMessageType.Broadcast));
                    return;
                }
            if (parameters.Length > 2)
                if (!float.TryParse(parameters[2], out shade))
                {
                    session.Network.EnqueueSend(new GameMessageSystemChat($"shade must be number between {float.MinValue} - {float.MaxValue}", ChatMessageType.Broadcast));
                    return;
                }
            if (parameters.Length > 3)
                if (!int.TryParse(parameters[3], out stackSize))
                {
                    session.Network.EnqueueSend(new GameMessageSystemChat($"stacksize must be number between {int.MinValue} - {int.MaxValue}", ChatMessageType.Broadcast));
                    return;
                }

            WorldObject loot;
            if (wcid)
                loot = WorldObjectFactory.CreateNewWorldObject(weenieClassId);
            else
                loot = WorldObjectFactory.CreateNewWorldObject(weenieClassDescription);

            // todo: set the palette, shade, stackSize here

            if (loot == null)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"{weenieClassDescription} is not a valid weenie.", ChatMessageType.Broadcast));
                return;
            }

            if (!loot.TimeToRot.HasValue)
                loot.TimeToRot = Double.MaxValue;

            //LootGenerationFactory.Spawn(loot, session.Player.Location.InFrontOf(1.0f));
            //inventoryItem.Sequences.GetNextSequence(SequenceType.ObjectTeleport);
            //inventoryItem.Sequences.GetNextSequence(SequenceType.ObjectVector);
            if (loot.WeenieType == WeenieType.Creature)
                loot.Location = session.Player.Location.InFrontOf(5f, true);
            else
                loot.Location = session.Player.Location.InFrontOf((loot.UseRadius ?? 2) > 2 ? loot.UseRadius.Value : 2);

            loot.Location.LandblockId = new LandblockId(loot.Location.GetCell());

            //Console.WriteLine($"Spawning {loot.Name} @ {loot.Location.Cell:X8} - {loot.Location.Pos}");
            LastSpawnPos = loot.Location;

            //inventoryItem.PhysicsDescriptionFlag |= PhysicsDescriptionFlag.Position;
            //LandblockManager.AddObject(loot);
            loot.EnterWorld();
        }

        // ci wclassid (number)
        [CommandHandler("ci", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1, "Creates an object in your inventory.", "wclassid (string or number), stacksize")]
        public static void HandleCI(Session session, params string[] parameters)
        {
            string weenieClassDescription = parameters[0];
            bool wcid = uint.TryParse(weenieClassDescription, out uint weenieClassId);

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

            if (parameters.Length > 2 && !int.TryParse(parameters[2], out int palette))
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"palette must be number between {int.MinValue} - {int.MaxValue}", ChatMessageType.Broadcast));
                return;
            }

            if (parameters.Length > 3 && !float.TryParse(parameters[3], out float shade))
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"shade must be number between {float.MinValue} - {float.MaxValue}", ChatMessageType.Broadcast));
                return;
            }
            

            WorldObject loot;
            if (wcid)
                loot = WorldObjectFactory.CreateNewWorldObject(weenieClassId);
            else
                loot = WorldObjectFactory.CreateNewWorldObject(weenieClassDescription);

            if (loot == null)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"{weenieClassDescription} is not a valid weenie.", ChatMessageType.Broadcast));
                return;
            }

            if (stackSize != 0)
            {
                if (loot.MaxStackSize != null && stackSize > loot.MaxStackSize)
                    loot.SetStackSize(loot.MaxStackSize);
                else if (loot.MaxStackSize != null && stackSize <= loot.MaxStackSize)
                    loot.SetStackSize(stackSize);
            }
            

            // todo set the palette, shade here

            session.Player.TryCreateInInventoryWithNetworking(loot);
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
                        res = @lock.Unlock(session.Player.Guid.Full, lockCode);
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

        // cm <material type> <quantity> <ave. workmanship>
        [CommandHandler("cm", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 3)]
        public static void HandleCM(Session session, params string[] parameters)
        {
            // Format is: @cm <material type> <quantity> <ave. workmanship>

            // TODO: output
        }

        /// <summary>
        /// Displays how much experience the last appraised creature is worth when killed.
        /// </summary>
        [CommandHandler("deathxp", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0, "Displays how much experience the last appraised creature iw worth when killed.")]
        public static void HandleDeathxp(Session session, params string[] parameters)
        {
            var creature = CommandHandlerHelper.GetLastAppraisedObject(session);
            if (creature == null) return;

            CommandHandlerHelper.WriteOutputInfo(session, $"{creature.Name} XP: {creature.XpOverride}");
        }

        // de_n name, text
        [CommandHandler("de_n", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 2)]
        public static void Handlede_n(Session session, params string[] parameters)
        {
            // usage: @de_n name, text
            // usage: @direct_emote_name name, text
            // Sends text to named player, formatted exactly as entered, with no prefix of any kind.
            // @direct_emote_name - Sends text to named player, formatted exactly as entered.

            // usage: @de_s text
            // usage: @direct_emote_select text
            // Sends text to selected player, formatted exactly as entered, with no prefix of any kind.
            // @direct_emote_select - Sends text to selected player, formatted exactly as entered.

            // TODO: output
        }

        // direct_emote_name name, text
        [CommandHandler("direct_emote_name", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 2)]
        public static void Handledirect_emote_name(Session session, params string[] parameters)
        {
            // usage: @de_n name, text
            // usage: @direct_emote_name name, text
            // Sends text to named player, formatted exactly as entered, with no prefix of any kind.
            // @direct_emote_name - Sends text to named player, formatted exactly as entered.

            // usage: @de_s text
            // usage: @direct_emote_select text
            // Sends text to selected player, formatted exactly as entered, with no prefix of any kind.
            // @direct_emote_select - Sends text to selected player, formatted exactly as entered.

            // TODO: output
        }

        // de_s text
        [CommandHandler("de_s", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1)]
        public static void Handlede_s(Session session, params string[] parameters)
        {
            // usage: @de_n name, text
            // usage: @direct_emote_name name, text
            // Sends text to named player, formatted exactly as entered, with no prefix of any kind.
            // @direct_emote_name - Sends text to named player, formatted exactly as entered.

            // usage: @de_s text
            // usage: @direct_emote_select text
            // Sends text to selected player, formatted exactly as entered, with no prefix of any kind.
            // @direct_emote_select - Sends text to selected player, formatted exactly as entered.

            // TODO: output
        }

        // direct_emote_select text
        [CommandHandler("direct_emote_select", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1)]
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
        [CommandHandler("god", AccessLevel.Sentinel, CommandHandlerFlag.RequiresWorld, 0)]
        public static void HandleGod(Session session, params string[] parameters)
        {
            // @god - Sets your own stats to the specified level.

            // TODO: output

            // output: You are now a god!!!

            ChatPacket.SendServerMessage(session, "You are now a god!!!", ChatMessageType.Broadcast);
        }

        // magic god
        [CommandHandler("magic god", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0)]
        public static void HandleMagicGod(Session session, params string[] parameters)
        {
            // @magic god -Sets your magic stats to the specfied level.

            // TODO: output

            // output: You are now a magic god!!!

            ChatPacket.SendServerMessage(session, "You are now a magic god!!!", ChatMessageType.Broadcast);
        }

        // heal
        [CommandHandler("heal", AccessLevel.Envoy, CommandHandlerFlag.RequiresWorld, 0,
            "Heals yourself(or the selected creature)",
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

            ObjectGuid nextItemGuid = GuidManager.NextDynamicGuid();
            ObjectGuid nextPlayerGuid = GuidManager.NextPlayerGuid();

            string message = $"The next Item GUID to be allocated is expected to be: {nextItemGuid.Full} (0x{(nextItemGuid.Full):X})\n";
            message += $"The next Player GUID to be allocated is expected to be: {nextPlayerGuid.Full} (0x{(nextPlayerGuid.Full):X})";
            var sysChatMsg = new GameMessageSystemChat(message, ChatMessageType.WorldBroadcast);
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
            "[wcid or weenie class name]")]
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

            Weenie weenie;
            if (wcid)
                weenie = DatabaseManager.World.GetCachedWeenie(weenieClassId);
            else
                weenie = DatabaseManager.World.GetCachedWeenie(weenieClassDescription);

            if (weenie == null)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Weenie {weenieClassDescription} not found in database, unable to morph.", ChatMessageType.Broadcast));
                return;
            }

            if (weenie.Type != (int)WeenieType.Creature && weenie.Type != (int)WeenieType.Cow
                && weenie.Type != (int)WeenieType.Admin && weenie.Type != (int)WeenieType.Sentinel && weenie.Type != (int)WeenieType.Vendor
                && weenie.Type != (int)WeenieType.Pet && weenie.Type != (int)WeenieType.CombatPet)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Weenie {weenie.GetProperty(PropertyString.Name)} ({weenieClassDescription}) is of WeenieType.{Enum.GetName(typeof(WeenieType), weenie.Type)} ({weenie.Type}), unable to morph because that is not allowed.", ChatMessageType.Broadcast));
                return;
            }

            session.Network.EnqueueSend(new GameMessageSystemChat($"Morphing you into {weenie.GetProperty(PropertyString.Name)} ({weenieClassDescription})... You will be logged out.", ChatMessageType.Broadcast));

            var guid = GuidManager.NewPlayerGuid();

            weenie.Type = (int)session.Player.WeenieType;

            var player = new Player(weenie, guid, session.AccountId);
            player.Location = session.Player.Location;

            player.Character.CharacterOptions1 = session.Player.Character.CharacterOptions1;
            player.Character.CharacterOptions2 = session.Player.Character.CharacterOptions2;

            //var wearables = weenie.GetCreateList((sbyte)DestinationType.Wield);
            var wearables = weenie.WeeniePropertiesCreateList.Where(x => x.DestinationType == (int)DestinationType.Wield || x.DestinationType == (int)DestinationType.WieldTreasure).ToList();
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

            var containables = weenie.WeeniePropertiesCreateList.Where(x => x.DestinationType == (int)DestinationType.Contain || x.DestinationType == (int)DestinationType.Shop
            || x.DestinationType == (int)DestinationType.Treasure || x.DestinationType == (int)DestinationType.ContainTreasure || x.DestinationType == (int)DestinationType.ShopTreasure).ToList();
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

            session.LogOffPlayer();
        }

        // qst
        [CommandHandler("qst", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0)]
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

            if (parameters[0].Equals("erase"))
            {
                if (parameters.Length < 2)
                {
                    // delete all quests?
                    // seems unsafe, maybe a confirmation?
                    return;
                }
                var questName = parameters[1];
                var player = session.Player;
                if (!player.QuestManager.HasQuest(questName))
                {
                    player.SendMessage($"{questName} not found");
                    return;
                }
                player.QuestManager.Erase(questName);
                player.SendMessage($"{questName} erased");
                return;
            }

            // TODO: output
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
            "< Current Name > < New Name >")]
        public static void HandleRename(Session session, params string[] parameters)
        {
            // @rename <Current Name>, <New Name> - Rename a character. (Do NOT include +'s for admin names)

            // As currently implemented below, the command does not exactly mimic retail usage in that the command was @rename oldName, newName
            // and for us is @rename oldName newName || is this a big deal?? -Ripley

            string fixupOldName = "";
            string fixupNewName = "";

            if (parameters[0] == "" || parameters[1] == "")
                return;

            fixupOldName = parameters[0].Replace("+", "").Remove(1).ToUpper() + parameters[0].Replace("+", "").Substring(1);
            fixupNewName = parameters[1].Replace("+", "").Remove(1).ToUpper() + parameters[1].Replace("+", "").Substring(1);

            string message = "";

            DatabaseManager.Shard.RenameCharacter(fixupOldName, fixupNewName, ((uint charId) =>
            {
                if (charId > 0)
                    message = $"Character {fixupOldName} has been renamed to {fixupNewName}.";
                else
                    message = $"Rename failed because either there is no character by the name {fixupOldName} currently in the database or the name {fixupNewName} is already taken.";

                CommandHandlerHelper.WriteOutputInfo(session, message, ChatMessageType.WorldBroadcast);
            }));
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
        [CommandHandler("sticky", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0)]
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
        [CommandHandler("watchmen", AccessLevel.Admin, CommandHandlerFlag.RequiresWorld, 0)]
        public static void Handlewatchmen(Session session, params string[] parameters)
        {
            // @watchmen - Displays a list of accounts with the specified level of admin access.

            // TODO: output
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

        // serverstatus
        [CommandHandler("serverstatus", AccessLevel.Advocate, CommandHandlerFlag.None, 0, "Displays a summary of server statistics and usage")]
        public static void HandleServerStatus(Session session, params string[] parameters)
        {
            // This is formatted very similarly to GDL.

            var sb = new StringBuilder();

            var proc = Process.GetCurrentProcess();

            sb.Append($"Server Status:{'\n'}");

            var runTime = DateTime.Now - proc.StartTime;
            sb.Append($"Server Runtime: {(int)runTime.TotalHours}h {runTime.Minutes}m {runTime.Seconds}s{'\n'}");

            sb.Append($"Total CPU Time: {(int)proc.TotalProcessorTime.TotalHours}h {proc.TotalProcessorTime.Minutes}m {proc.TotalProcessorTime.Seconds}s, Threads: {proc.Threads.Count}{'\n'}");

            // todo, add actual system memory used/avail
            sb.Append($"{(proc.PrivateMemorySize64 >> 20):N0} MB used{'\n'}");  // sb.Append($"{(proc.PrivateMemorySize64 >> 20)} MB used, xxxx / yyyy MB physical mem free.{'\n'}");

            sb.Append($"{WorldManager.GetSessionCount():N0} connections, {PlayerManager.GetAllOnline().Count:N0} players online{'\n'}");
            sb.Append($"Total Accounts Created: {DatabaseManager.Authentication.GetAccountCount():N0}, Total Characters Created: {(PlayerManager.GetAllOffline().Count + PlayerManager.GetAllOnline().Count):N0}{'\n'}");

            // 330 active objects, 1931 total objects(16777216 buckets.)

            // todo, expand this
            var activeLandblocks = LandblockManager.GetActiveLandblocks();
            int players = 0, creatures = 0, missiles = 0, other = 0, total = 0;
            foreach (var landblock in activeLandblocks)
            {
                foreach (var worldObject in landblock.GetAllWorldObjectsForDiagnostics())
                {
                    if (worldObject is Player)
                        players++;
                    else if (worldObject is Creature)
                        creatures++;
                    else if (worldObject.Missile ?? false)
                        missiles++;
                    else
                        other++;

                    total++;
                }
            }
            sb.Append($"{activeLandblocks.Count:N0} active landblocks - Players: {players:N0}, Creatures: {creatures:N0}, Missiles: {missiles:N0}, Other: {other:N0}, Total: {total:N0}.{'\n'}"); // 11 total blocks loaded. 11 active. 0 pending dormancy. 0 dormant. 314 unloaded.
            // 11 total blocks loaded. 11 active. 0 pending dormancy. 0 dormant. 314 unloaded.

            sb.Append($"UpdateGameWorld {(DateTime.UtcNow - WorldManager.UpdateGameWorld5MinLastReset).TotalMinutes:N2} min - {WorldManager.UpdateGameWorld5MinRM}.{'\n'}");
            sb.Append($"UpdateGameWorld {(DateTime.UtcNow - WorldManager.UpdateGameWorld60MinLastReset).TotalMinutes:N2} min - {WorldManager.UpdateGameWorld60MinRM}.{'\n'}");

            sb.Append($"World DB Cache Counts - Weenies: {DatabaseManager.World.GetWeenieCacheCount():N0}, LandblockInstances: {DatabaseManager.World.GetLandblockInstancesCacheCount():N0}, PointsOfInterest: {DatabaseManager.World.GetPointsOfInterestCacheCount():N0}, Cookbooks: {DatabaseManager.World.GetCookbookCacheCount():N0}, Spells: {DatabaseManager.World.GetSpellCacheCount():N0}, Encounters: {DatabaseManager.World.GetEncounterCacheCount():N0}, Events: {DatabaseManager.World.GetEventsCacheCount():N0}{'\n'}");
            sb.Append($"Shard DB Counts - Biotas: {DatabaseManager.Shard.GetBiotaCount():N0}{'\n'}");

            sb.Append($"Portal.dat has {DatManager.PortalDat.FileCache.Count:N0} files cached of {DatManager.PortalDat.AllFiles.Count:N0} total{'\n'}");
            sb.Append($"Cell.dat has {DatManager.CellDat.FileCache.Count:N0} files cached of {DatManager.CellDat.AllFiles.Count:N0} total{'\n'}");

            CommandHandlerHelper.WriteOutputInfo(session, $"{sb}");
        }

        [CommandHandler("modifybool", AccessLevel.Admin, CommandHandlerFlag.None, 2, "Modifies a server property that is a bool", "modifybool (string) (bool)")]
        public static void HandleModifyServerBoolProperty(Session session, params string[] paramters)
        {
            try
            {
                var boolVal = bool.Parse(paramters[1]);
                PropertyManager.ModifyBool(paramters[0], boolVal);
                CommandHandlerHelper.WriteOutputInfo(session, "Bool property successfully updated!");
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
                PropertyManager.ModifyLong(paramters[0], intVal);
                CommandHandlerHelper.WriteOutputInfo(session, "Long property successfully updated!");
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
                PropertyManager.ModifyDouble(paramters[0], floatVal);
                CommandHandlerHelper.WriteOutputInfo(session, "Double property successfully updated!");
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
            PropertyManager.ModifyString(parameters[0], parameters[1]);
            CommandHandlerHelper.WriteOutputInfo(session, "String property successfully updated!");
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

        [CommandHandler("resyncproperties", AccessLevel.Admin, CommandHandlerFlag.None, -1,
            "Resync the properties database", "resyncproperties")]
        public static void HandleResyncServerProperties(Session session, params string[] parameters)
        {
            PropertyManager.ResyncVariables();
        }
    }
}
