using System;

using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Managers;
using ACE.Network;
using ACE.Network.GameMessages.Messages;
using ACE.Common;
using ACE.Database;
using ACE.Network.Enum;

namespace ACE.Command.Handlers
{
    public static class AdminCommands
    {
        // // commandname parameters
        // [CommandHandler("commandname", AccessLevel.Admin, CommandHandlerFlag.RequiresWorld, 0)]
        // public static void HandleHelp(Session session, params string[] parameters)
        // {
        //     //TODO: output
        // }

        // adminvision { on | off | toggle | check}
        [CommandHandler("adminvision", AccessLevel.Sentinel, CommandHandlerFlag.RequiresWorld, 1)]
        public static void HandleAdminvision(Session session, params string[] parameters)
        {
            // @adminvision { on | off | toggle | check}
            // Controls whether or not the admin can see admin-only visible items. Note that if you turn this feature off, you will need to log out and back in before the visible items become invisible.
            // @adminvision - Allows the admin to see admin - only visible items.

            // TODO: output

            // output: Admin Visible is {state}

            ChatPacket.SendServerMessage(session, "Admin Visible is [state]", ChatMessageType.Broadcast);
        }

        // adminui
        [CommandHandler("adminui", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0)]
        public static void HandleAdminui(Session session, params string[] parameters)
        {
            // usage: @adminui
            // This command toggles whether the Admin UI is visible.

            // just a placeholder, probably not needed or should be handled by a decal plugin to replicate the admin ui
        }

        // attackable { on | off }
        [CommandHandler("attackable", AccessLevel.Sentinel, CommandHandlerFlag.RequiresWorld, 1)]
        public static void HandleAttackable(Session session, params string[] parameters)
        {
            // usage: @attackable { on,off}
            // This command sets whether monsters will attack you unprovoked.When turned on, monsters will attack you as if you are a normal player.  When turned off, monsters will ignore you.
            // @attackable - Sets whether monsters will attack you or not.

            // TODO: output
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

        // boot { account | char | iid } who
        [CommandHandler("boot", AccessLevel.Sentinel, CommandHandlerFlag.RequiresWorld, 2)]
        public static void HandleBoot(Session session, params string[] parameters)
        {
            // usage: @boot { account,char, iid} who
            // This command boots the specified character out of the game.You can specify who to boot by account, character name, or player instance id.  'who' is the account / character / instance id to actually boot.
            // @boot - Boots the character out of the game.

            // TODO: output
        }

        // cloak < on / off / player / creature >
        [CommandHandler("cloak", AccessLevel.Sentinel, CommandHandlerFlag.RequiresWorld, 1)]
        public static void HandleCloak(Session session, params string[] parameters)
        {
            // Please specify if you want cloaking on or off.usage: @cloak < on / off / player / creature >
            // This command sets your current cloaking state.
            // < on > You will be completely invisible to players.
            // < off > You will show up as a normal.
            // < player > You will appear as a player. (No + and a white radar dot.)
            // < creature > You will appear as a creature. (No + and an orange radar dot.)
            // @cloak - Sets your cloaking state.

            // TODO: output
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
        [CommandHandler("delete", AccessLevel.Envoy, CommandHandlerFlag.RequiresWorld, 0)]
        public static void HandleDeleteSelected(Session session, params string[] parameters)
        {
            // @delete - Deletes the selected object. Players may not be deleted this way.

            // TODO: output
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

        // home < recall number >
        [CommandHandler("home", AccessLevel.Sentinel, CommandHandlerFlag.RequiresWorld, 0)]
        public static void HandleHome(Session session, params string[] parameters)
        {
            // usage: @home - This command teleports you to your sanctuary position.
            // @home < recall number > -Recall to your position into the numbered recall, valid values are 1 - 9.
            // NOTE: Calling @home without a number recalls your sanctuary position, calling it with recalls to the saved numbered spot.
            // @home has the alias @recall
            // @home[recall number] -Teleports you to your sanctuary position.

            // TODO: output
        }

        // mrt
        [CommandHandler("mrt", AccessLevel.Sentinel, CommandHandlerFlag.RequiresWorld, 0)]
        public static void HandleMRT(Session session, params string[] parameters)
        {
            // @mrt - Toggles the ability to bypass housing boundaries.

            // TODO: output
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
        [CommandHandler("myiid", AccessLevel.Envoy, CommandHandlerFlag.RequiresWorld, 0)]
        public static void HandleMyIID(Session session, params string[] parameters)
        {
            // @myiid - Displays your Instance ID(IID).

            // TODO: output
        }

        // myserver
        [CommandHandler("myserver", AccessLevel.Envoy, CommandHandlerFlag.RequiresWorld, 0)]
        public static void HandleMyServer(Session session, params string[] parameters)
        {
            // @myserver - Displays the number of the game server on which you are currently located.

            // TODO: output
        }

        // neversaydie [on/off]
        [CommandHandler("neversaydie", AccessLevel.Sentinel, CommandHandlerFlag.RequiresWorld, 0)]
        public static void HandleNeverSayDie(Session session, params string[] parameters)
        {
            // @neversaydie [on/off] - Turn immortality on or off. Defaults to on.

            // TODO: output

            // output: You are now immortal.

            ChatPacket.SendServerMessage(session, "You are now immortal.", ChatMessageType.Broadcast);
        }

        // pk
        [CommandHandler("pk", AccessLevel.Sentinel, CommandHandlerFlag.RequiresWorld, 0)]
        public static void HandlePk(Session session, params string[] parameters)
        {
            // @pk - Toggles or sets your own PK state.

            // TODO: output
        }

        // portal_bypass
        [CommandHandler("portal_bypass", AccessLevel.Sentinel, CommandHandlerFlag.RequiresWorld, 0)]
        public static void HandlePortalBypass(Session session, params string[] parameters)
        {
            // @portal_bypass - Toggles the ability to bypass portal restrictions.

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

        // run < on | off | toggle | check >
        [CommandHandler("run", AccessLevel.Sentinel, CommandHandlerFlag.RequiresWorld, 0)]
        public static void HandleRun(Session session, params string[] parameters)
        {
            // usage: @run on| off | toggle | check
            // Boosts the run skill of the PSR so they can pursue the "bad folks".The enchantment will wear off after a while.This command defualts to toggle.
            // @run - Temporarily boosts your run skill.

            // TODO: output
        }

        // save < recall number >
        [CommandHandler("save", AccessLevel.Sentinel, CommandHandlerFlag.RequiresWorld, 0)]
        public static void HandleSave(Session session, params string[] parameters)
        {
            // @save - This command sets your current position to be your sanctuary position.
            // @save < recall number > -Save your position into the numbered recall, valid values are 1 - 9.
            // NOTE: Calling @save without a number saves your sanctuary position, calling it with saves  it in a separate save spot.
            // @save - Sets your sanctuary position or a named recall point.

            // TODO: output
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

        // smite [all]
        [CommandHandler("smite", AccessLevel.Envoy, CommandHandlerFlag.RequiresWorld, 0)]
        public static void HandleSmite(Session session, params string[] parameters)
        {
            // @smite [all] - Kills the selected target or all monsters in radar range if "all" is specified.

            if (parameters?.Length > 0)
            {
                if (parameters[0] == "all")
                {
                    var creatureIds = session.Player.GetKnownCreatures();
                    foreach (var cId in creatureIds)
                    {
                        var wo = LandblockManager.GetWorldObject(session, cId);
                        if (wo != null)
                            (wo as Creature).OnKill(session);
                    }
                }
                else
                    ChatPacket.SendServerMessage(session, "Select a target and use @smite, or use @smite all to kill all creatures in radar range.", ChatMessageType.Broadcast);
            }
            else
            {
                if (session.Player.SelectedTarget != 0)
                {
                    var target = new ObjectGuid(session.Player.SelectedTarget);
                    var wo = LandblockManager.GetWorldObject(session, target);

                    if (target.IsCreature())
                    {
                        if (wo != null)
                            (wo as Creature).OnKill(session);
                    }
                    if (target.IsPlayer())
                    {
                        if (wo != null)
                            (wo as Player).OnKill(session);
                    }
                }
                else
                {
                    ChatPacket.SendServerMessage(session, "No target selected, use @smite all to kill all creatures in radar range.", ChatMessageType.Broadcast);
                }
            }
        }

        // tele [name] longitude latitude
        [CommandHandler("tele", AccessLevel.Sentinel, CommandHandlerFlag.RequiresWorld, 2,
            "This command teleports yourself (or the specified character) to the given longitude and latitude.",
            "longitude latitude\n" +
            "No commas accepted between coordinates, for example: @tele 0.0N 0.0E")]
        public static void HandleTele(Session session, params string[] parameters)
        {
            // Used PhatAC source to implement most of this.  Thanks Pea!

            // usage: @tele [name,] longitude latitude
            // This command teleports yourself (or the specified character) to the given longitude and latitude.
            // @tele - Teleports you(or a player) to some location.

            string northSouth = parameters[0].ToLower();
            string eastWest = parameters[1].ToLower();

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

            float coordNS;
            if (!float.TryParse(northSouth.Substring(0, northSouth.Length - 1), out coordNS))
            {
                ChatPacket.SendServerMessage(session, "North/South coordinate is not a valid number.", ChatMessageType.Broadcast);
                return;
            }

            float coordEW;
            if (!float.TryParse(eastWest.Substring(0, eastWest.Length - 1), out coordEW))
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
            }
            catch (System.Exception)
            {
                ChatPacket.SendServerMessage(session, "There was a problem teleporting to that location (bad coordinates?).", ChatMessageType.Broadcast);
                return;
            }

            // TODO: Check if water block?

            ChatPacket.SendServerMessage(session, $"Position: [Cell: 0x{position.LandblockId.Landblock.ToString("X4")} | Offset: {position.PositionX}, {position.PositionY}, {position.PositionZ} | Facing: {position.RotationX}, {position.RotationY}, {position.RotationZ}, {position.RotationW}]", ChatMessageType.Broadcast);

            session.Player.Teleport(position);
        }

        // teleto [char]
        [CommandHandler("teleto", AccessLevel.Sentinel, CommandHandlerFlag.RequiresWorld, 1,
            "Teleport yourself to a player",
            "[Player's Name]\n" +
            "@teleto Playername")]
        public static void HandleTeleto(Session session, params string[] parameters)
        {
            // @teleto - Teleports you to the specified character.
            var playerName = String.Join(" ", parameters);
            // Lookup the player in the world
            Session playerSession = WorldManager.FindByPlayerName(playerName);
            // If the player is found, teleport the admin to the Player's location
            if (playerSession != null)
                session.Player.Teleport(playerSession.Player.Location);
            else
                session.Network.EnqueueSend(new GameMessageSystemChat($"Player {playerName} was not found.", ChatMessageType.Broadcast));
        }

        // telepoi location
        [CommandHandler("telepoi", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1,
            "Teleport yourself to a named Point of Interest",
            "[POI]\n" +
            "@telepoi Arwic")]
        public static void HandleTeleportPOI(Session session, params string[] parameters)
        {
            var poi = String.Join(" ", parameters);
            var teleportPOI = AssetManager.GetTeleport(poi);
            if (teleportPOI == null)
                return;

            session.Player.Teleport(teleportPOI);
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
                    float position;
                    if (!float.TryParse(parameters[i + 1].Trim(new Char[] { ' ', '[', ']' }), out position))
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

            DerethDateTime currentPYtime = new DerethDateTime(WorldManager.PortalYearTicks);
            String messageUTC   = "The current server time in UtcNow is: " + DateTime.UtcNow.ToString();
            String messagePY    = "The current server time in DerethDateTime is: " + currentPYtime.ToString();

            var chatSysMessageUTC = new GameMessageSystemChat(messageUTC, ChatMessageType.WorldBroadcast);
            var chatSysMessagePY = new GameMessageSystemChat(messagePY, ChatMessageType.WorldBroadcast);

            if (session != null)
                session.Network.EnqueueSend(chatSysMessageUTC, chatSysMessagePY);
            else
            {
                Console.WriteLine(messageUTC);
                Console.WriteLine(messagePY);
            }
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
        [CommandHandler("gamecast", AccessLevel.Envoy, CommandHandlerFlag.RequiresWorld, 1)]
        public static void HandleGamecast(Session session, params string[] parameters)
        {
            // > Broadcast from usage: @gamecast<message>
            // This command sends a world-wide broadcast to everyone in the game. Text is prefixed with 'Broadcast from (admin-name)> '.
            // See Also: @gamecast, @gamecastemote, @gamecastlocal, @gamecastlocalemote.
            // @gamecast - Sends a world-wide broadcast.

            // TODO: output
        }

        // add <spell>
        [CommandHandler("add", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1)]
        public static void HandleAdd(Session session, params string[] parameters)
        {
            // @add spell - Adds the specified spell to your own spellbook.

            // TODO: output
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

        // create wclassid (string or number)
        [CommandHandler("create", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1)]
        public static void HandleCreate(Session session, params string[] parameters)
        {
            // Creates the given object in the world, external to the admin
            // @create - Creates an object in the world.
            // usage: @create wclassid (string or number)

            // TODO: output
        }

        // ci wclassid (string or number)
        [CommandHandler("ci", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1)]
        public static void HandleCI(Session session, params string[] parameters)
        {
            // Creates the given object in the admin's inventory
            // @ci - Creates an object in your inventory.

            // TODO: output
        }

        // cm <material type> <quantity> <ave. workmanship>
        [CommandHandler("cm", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 3)]
        public static void HandleCM(Session session, params string[] parameters)
        {
            // Format is: @cm <material type> <quantity> <ave. workmanship>

            // TODO: output
        }

        // deathxp
        [CommandHandler("deathxp", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0)]
        public static void HandleDeathxp(Session session, params string[] parameters)
        {
            // @deathxp - Displays how much experience this creature is worth when killed.

            // TODO: output
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
        [CommandHandler("dispel", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0)]
        public static void HandleDispel(Session session, params string[] parameters)
        {
            // usage: @dispel
            // This command removes all enchantments from you, or the object you have selected.
            // @dispel - Dispels all enchantments from you (or the selected object).

            // TODO: output
        }

        // event
        [CommandHandler("event", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 2)]
        public static void HandleEvent(Session session, params string[] parameters)
        {
            // usage: @event start| stop | disable | enable name
            // @event clear < name > -clears event with name <name> or all events if you put in 'all' (All clears registered generators, <name> does not).
            // @event status<eventSubstring> - get the status of all registered events or get all of the registered events that have <eventSubstring> in the name.
            // @event - Maniuplates the state of an event.

            // TODO: output
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

            // TODO: When buffs are implemented, we'll need to revisit this command to make sure it takes those into account and restores vitals to 100%

            session.Player.Health.Current = session.Player.Health.UnbuffedValue;
            session.Player.Stamina.Current = session.Player.Stamina.UnbuffedValue;
            session.Player.Mana.Current = session.Player.Mana.UnbuffedValue;

            var updatePlayersHealth = new GameMessagePrivateUpdateAttribute2ndLevel(session, Vital.Health, session.Player.Health.Current);
            var updatePlayersStamina = new GameMessagePrivateUpdateAttribute2ndLevel(session, Vital.Stamina, session.Player.Stamina.Current);
            var updatePlayersMana = new GameMessagePrivateUpdateAttribute2ndLevel(session, Vital.Mana, session.Player.Mana.Current);

            session.Network.EnqueueSend(updatePlayersHealth, updatePlayersStamina, updatePlayersMana);
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
        [CommandHandler("idlist", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0)]
        public static void HandleIDlist(Session session, params string[] parameters)
        {
            // @idlist - Shows the next ID that will be allocated from SQL.

            // TODO: output
        }

        // gamecastlocalemote <message>
        [CommandHandler("gamecastlocalemote", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1)]
        public static void Handlegamecastlocalemote(Session session, params string[] parameters)
        {
            // usage: @gamecastlocalemote<message>
            // Sends text to all players within chat range, formatted exactly as entered, with no prefix of any kind.
            // See Also: @gamecast, @gamecastemote, @gamecastlocal, @gamecastlocalemote.
            // @gamecastlocalemote - Sends text to all players within chat range, formatted exactly as entered.

            // TODO: output
        }

        // location
        [CommandHandler("location", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0)]
        public static void HandleLocation(Session session, params string[] parameters)
        {
            // @location - Causes your current location to be continuously displayed on the screen.

            // TODO: output
        }

        // morph
        [CommandHandler("morph", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1)]
        public static void HandleMorph(Session session, params string[] parameters)
        {
            // @morph - Morphs your bodily form into that of the specified creature. Be careful with this one!

            // TODO: output
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

            uint charId = DatabaseManager.Character.RenameCharacter(fixupOldName, fixupNewName);

            string message = "";

            if (charId > 0)
                message = $"Character {fixupOldName} has been renamed to {fixupNewName}.";
            else
                message = $"Rename failed because either there is no character by the name {fixupOldName} currently in the database or the name {fixupNewName} is already taken.";

            if (session == null)
                Console.WriteLine(message);
            else
            {
                var sysChatMsg = new GameMessageSystemChat(message, ChatMessageType.WorldBroadcast);
                session.Network.EnqueueSend(sysChatMsg);
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
        [CommandHandler("gamecastlocal", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1)]
        public static void Handlegamecastlocal(Session session, params string[] parameters)
        {
            // Local Server Broadcast from
            // usage: @gamecastlocal<message>
            // This command sends the specified text to every player on the current server.
            // See Also: @gamecast, @gamecastemote, @gamecastlocal, @gamecastlocalemote.
            // @gamecastlocal Sends a server-wide broadcast.

            // TODO: output
        }

        // sticky { on | off }
        [CommandHandler("sticky", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1)]
        public static void HandleSticky(Session session, params string[] parameters)
        {
            // usage: @sticky {on,off}
            // This command sets whether you loose any items should you die.When set to 'on', you will be complete protected from item-loss rules.
            // @sticky - Sets whether you loose items should you die.

            // TODO: output
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
        [CommandHandler("gamecastemote", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1)]
        public static void Handlegamecastemote(Session session, params string[] parameters)
        {
            // usage: "@gamecastemote <message>" or "@we <message"
            // Sends text to all players, formatted exactly as entered, with no prefix of any kind.
            // See Also: @gamecast, @gamecastemote, @gamecastlocal, @gamecastlocalemote.
            // @gamecastemote - Sends text to all players, formatted exactly as entered.

            // TODO: output
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
    }
}
