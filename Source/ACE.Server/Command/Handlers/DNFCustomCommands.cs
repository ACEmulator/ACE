using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using Microsoft.EntityFrameworkCore;
using log4net;
using ACE.Common;
using ACE.Common.Extensions;
using ACE.Database;
using ACE.Database.Models.Auth;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using Position = ACE.Entity.Position;
using ACE.Server.Factories;
using ACE.Server.Factories.Enum;
using ACE.Server.Factories.Entity;
using ACE.Server.Managers;
using ACE.Server.Network;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Sequence;
using ACE.Server.Network.Structure;
using ACE.Server.Physics.Common;
using ACE.Server.Physics.Entity;
using ACE.Server.Physics.Extensions;
using ACE.Server.Physics.Managers;
using ACE.Server.WorldObjects;
using ACE.Server.WorldObjects.Entity;
using ACE.Server.WorldObjects.Managers;

namespace ACE.Server.Command.Handlers
{
    public static class DNFCustomCommands // Custom Commands for DnF by Linae
    {

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // get DnF Command Update Version
        [CommandHandler("cmdver", AccessLevel.Envoy, CommandHandlerFlag.None, 0)]
        public static void Handlecmdver(Session session, params string[] parameters)
        {
            var msg = $"DnF Command Update Version 1.57.4468 Requires core >= 1.57.4468";

            if(session == null)
            {
                Console.WriteLine(msg);
            }
            else
            {
                CommandHandlerHelper.WriteOutputInfo(session, msg, ChatMessageType.Broadcast);                
            }
 	    }

        // Movetoloc - by Linae of DnF
        // movetoloc cell x y z [qx qy qz qw] 
        [CommandHandler("movetoloc", AccessLevel.Advocate, CommandHandlerFlag.RequiresWorld, "Moves the last appraised object to the specified location.")]
        public static void HandleMoveToLoc(Session session, params string[] parameters)
        {
            var obj = CommandHandlerHelper.GetLastAppraisedObject(session);

            if (obj == null)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"You must select an object to move first.", ChatMessageType.Broadcast));
                return;
            }

            if (obj.CurrentLandblock == null)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"{obj.Name} ({obj.Guid}) is not a landblock object", ChatMessageType.Broadcast));
                return;
            }

            if (obj is Player)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"{obj.Name} ({obj.Guid}) is a player.  Use /teleplayerloc instead.", ChatMessageType.Broadcast));
                return;
            }

            var prevLoc = new Position(obj.Location);

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
                    if (i > 2 && parameters.Length < 8)
                    {
                        positionData[3] = 1;
                        positionData[4] = 0;
                        positionData[5] = 0;
                        positionData[6] = 0;
                        break;
                    }

                    if (!float.TryParse(parameters[i + 1].Trim(new Char[] { ' ', '[', ']' }), out var position))
                        return;

                    positionData[i] = position;
                }

                var newLoc = new Position(cell, positionData[0], positionData[1], positionData[2], positionData[4], positionData[5], positionData[6], positionData[3]);
                newLoc.Rotation = prevLoc.Rotation;     // keep previous rotation

                var setPos = new Physics.Common.SetPosition(newLoc.PhysPosition(), Physics.Common.SetPositionFlags.Teleport | Physics.Common.SetPositionFlags.Slide);
                var result = obj.PhysicsObj.SetPosition(setPos);

                if (result != Physics.Common.SetPositionError.OK)
                {
                    session.Network.EnqueueSend(new GameMessageSystemChat($"Failed to move {obj.Name} ({obj.Guid}) to location: {result}", ChatMessageType.Broadcast));
                    return;
                }

                session.Network.EnqueueSend(new GameMessageSystemChat($"Moving {obj.Name} ({obj.Guid}) to location: {newLoc} {result}", ChatMessageType.Broadcast));

                obj.Location = obj.PhysicsObj.Position.ACEPosition();

                if (prevLoc.Landblock != obj.Location.Landblock)
                {
                    LandblockManager.RelocateObjectForPhysics(obj, true);
                }

                obj.SendUpdatePosition(true);
                PlayerManager.BroadcastToAuditChannel(session.Player, $"{session.Player.Name} has moved {obj.Name} (0x{obj.Guid}) to {cell:X8}, {positionData[0]}, {positionData[1]}, {positionData[2]}, {positionData[4]}, {positionData[5]}, {positionData[6]}, {positionData[3]}.");
            }
            catch (Exception)
            {
                ChatPacket.SendServerMessage(session, "Invalid arguments for @movetoloc", ChatMessageType.Broadcast);
                ChatPacket.SendServerMessage(session, "Hint: @movetoloc follows the same number order as displayed from @loc output", ChatMessageType.Broadcast);
                ChatPacket.SendServerMessage(session, "Usage: @movetoloc cell [x y z] (qw qx qy qz)", ChatMessageType.Broadcast);
                ChatPacket.SendServerMessage(session, "Example: @movetoloc 0x7F0401AD [12.319900 -28.482000 0.005000] -0.338946 0.000000 0.000000 -0.940806", ChatMessageType.Broadcast);
                ChatPacket.SendServerMessage(session, "Example: @movetoloc 0x7F0401AD 12.319900 -28.482000 0.005000 -0.338946 0.000000 0.000000 -0.940806", ChatMessageType.Broadcast);
                ChatPacket.SendServerMessage(session, "Example: @movetoloc 7F0401AD 12.319900 -28.482000 0.005000", ChatMessageType.Broadcast);
            }
        }

        // teleplayerloc <player name> cell x y z [qx qy qz qw]
        // original code by Harli and updated by Linae of DnF
        [CommandHandler("teleplayerloc", AccessLevel.Sentinel, CommandHandlerFlag.RequiresWorld, 4,
            "Teleports a player to the specified location.",
            "\"player\" cell [x y z] (qw qx qy qz)\n" +
            "@teleplayerloc follows the same number order as displayed from @loc output\n" +
            "Example: @teleplayerloc \"<player name>\" 0x7F0401AD [12.319900 -28.482000 0.005000] -0.338946 0.000000 0.000000 -0.940806\n" +
            "Example: @teleplayerloc \"<player name>\" 0x7F0401AD 12.319900 -28.482000 0.005000 -0.338946 0.000000 0.000000 -0.940806\n" +
            "Example: @teleplayerloc \"<player name>\" 7F0401AD 12.319900 - 28.482000 0.005000")]
        public static void HandleTeleportPlayerLOC(Session session, params string[] parameters)
        {
            var playerName = string.Join(" ", parameters[0]);

            if (parameters.Length >9)
            {
               session.Network.EnqueueSend(new GameMessageSystemChat($"Player name must be enclosed in double quotes. Ex: \"Player Name\"", ChatMessageType.Broadcast));
               return; 
            }

            var player = PlayerManager.GetOnlinePlayer(playerName);
            if (player == null)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Player {player} was not found.", ChatMessageType.Broadcast));
                return;
            } 

            var currentPos = new Position(player.Location);

            if (player.YouAreJailed == true)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Player {player.Name} is in jail and can not be teleported out.  You must use /pardon.", ChatMessageType.Broadcast));
                return;
            }
  
            try
            {
                uint cell;

                if (parameters[1].StartsWith("0x"))
                {
                    string strippedcell = parameters[1].Substring(2);
                    cell = (uint)int.Parse(strippedcell, System.Globalization.NumberStyles.HexNumber);
                }
                else
                    cell = (uint)int.Parse(parameters[1], System.Globalization.NumberStyles.HexNumber);

                var positionData = new float[8];
                for (uint i = 1u; i < 8u; i++)
                {
                    if (!float.TryParse(parameters[i + 1].Trim(new Char[] { ' ', '[', ']' }), out var position))
                        return;

                    positionData[i] = position;
                }
                player.Teleport(new Position(cell, positionData[1], positionData[2], positionData[3], positionData[5], positionData[6], positionData[7], positionData[4]));
                player.Session.Network.EnqueueSend(new GameMessageSystemChat($"{session.Player.Name} has teleported you to {parameters[1]} {parameters[2]} {parameters[3]} {parameters[4]} {parameters[5]} {parameters[6]} {parameters[7]} {parameters[8]}", ChatMessageType.Magic));
                PlayerManager.BroadcastToAuditChannel(session.Player, $"{session.Player.Name} has teleported {player.Name} to {parameters[1]:X8} {parameters[2]} {parameters[3]} {parameters[4]} {parameters[5]} {parameters[6]} {parameters[7]} {parameters[8]}");
            }

            catch (Exception)
            {
                ChatPacket.SendServerMessage(session, "Invalid arguments for @teleplayerloc", ChatMessageType.Broadcast);
                ChatPacket.SendServerMessage(session, "Hint: @teleplayerloc follows the same number order as displayed from @loc output", ChatMessageType.Broadcast);
                ChatPacket.SendServerMessage(session, "Usage: @teleplayerloc \"<player name>\" cell [x y z] (qw qx qy qz)", ChatMessageType.Broadcast);
                ChatPacket.SendServerMessage(session, "Example: @teleplayerloc \"<player name>\" 0x7F0401AD [12.319900 -28.482000 0.005000] -0.338946 0.000000 0.000000 -0.940806", ChatMessageType.Broadcast);
                ChatPacket.SendServerMessage(session, "Example: @teleplayerloc \"<player name>\" 0x7F0401AD 12.319900 -28.482000 0.005000 -0.338946 0.000000 0.000000 -0.940806", ChatMessageType.Broadcast);
                ChatPacket.SendServerMessage(session, "Example: @teleplayerloc \"<player name>\" 7F0401AD 12.319900 -28.482000 0.005000", ChatMessageType.Broadcast);
            }
        }

        // timedgag < "char name" > < duration > - by Linae of DnF
        [CommandHandler("timedgag", AccessLevel.Envoy, CommandHandlerFlag.RequiresWorld, 2,
            "Prevents a character from talking for the specified duration in seconds..",
            "< \"char name\" duration>\nThe character will not be able to @tell or use chat normally\nCharacter name MUST be enclosed in quotes.")]
        public static void HandleTimedGag(Session session, params string[] parameters)
        {
            // usage: @timedgag < char name > <duration in seconds>
            // This command gags the specified character for the specified time in seconds.  The character will not be able to @tell or use chat normally.
            // @timedgag - Prevents a character from talking for the specified timeframe.
            // @ungag -Allows a gagged character to talk again.

            var playerName = string.Join(" ", parameters[0]);

            if (parameters.Length >2)
            {
               session.Network.EnqueueSend(new GameMessageSystemChat($"Player name must be enclosed in double quotes. Ex: \"Player Name\"", ChatMessageType.Broadcast));
               return; 
            }

            var player = PlayerManager.FindByName(playerName);
            var gaglength = double.Parse(parameters[1]);

            if (player == null)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Player {playerName} is either not online or you typoed the name.", ChatMessageType.Broadcast));
                return;
            }
            else
            {
                player.SetProperty(ACE.Entity.Enum.Properties.PropertyBool.IsGagged, true);
                player.SetProperty(ACE.Entity.Enum.Properties.PropertyFloat.GagTimestamp, Common.Time.GetUnixTime());
                player.SetProperty(ACE.Entity.Enum.Properties.PropertyFloat.GagDuration, gaglength);

                player.SaveBiotaToDatabase();

                CommandHandlerHelper.WriteOutputInfo(session, session.Player.Name + " has gagged " + playerName + " for " + gaglength + " seconds.", ChatMessageType.WorldBroadcast);
                PlayerManager.BroadcastToAuditChannel(session.Player, $"{session.Player.Name} has gagged {player.Name} for {gaglength} seconds.");
            }
        }

        // jail < "char name" > - by Linae of DnF
        [CommandHandler("jail", AccessLevel.Envoy, CommandHandlerFlag.RequiresWorld, 1,
            "Jails the specified player account.",
            "\"[char name]\" Note: char name MUST be in quotes.\n"
            + "This command jails the specified player. This player will be locked in a room until pardoned.\n"
            + "Example: @jail \"char name\"\n")]
        public static void HandleJail(Session session, params string[] parameters)
        {
            var playerName = string.Join(" ", parameters[0]);

             if (parameters.Length >1)
            {
               session.Network.EnqueueSend(new GameMessageSystemChat($"Player name must be enclosed in double quotes. Ex: \"Player Name\"", ChatMessageType.Broadcast));
               return; 
            }

            var player = PlayerManager.GetOnlinePlayer(playerName);

            if (player == null)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Player {playerName} was not found.", ChatMessageType.Broadcast));
                return;
            } 

            var jailedBy = 0u;
            if (session != null)
            {
                jailedBy = session.AccountId;
            }

            // Send the player off to jail then gag and prevent recalls

            string cell = "0x010D0100";

            HandleTeleportPlayerLOC(session, playerName, cell.ToString(), 0.ToString(), 0.ToString(), 0.ToString(), 1.ToString(), 0.ToString(), 0.ToString(), 0.ToString());

            player.SetProperty(ACE.Entity.Enum.Properties.PropertyBool.IsGagged, true);
            player.SetProperty(ACE.Entity.Enum.Properties.PropertyFloat.GagTimestamp, Common.Time.GetUnixTime());
            player.SetProperty(ACE.Entity.Enum.Properties.PropertyFloat.GagDuration, 31560000);
            player.SetProperty(ACE.Entity.Enum.Properties.PropertyBool.RecallsDisabled, true);
            player.SetProperty(ACE.Entity.Enum.Properties.PropertyBool.YouAreJailed, true);

            player.SaveBiotaToDatabase();

            player.Session.Network.EnqueueSend(new GameMessageSystemChat($"{session.Player.Name} has jailed you.", ChatMessageType.Magic));
            PlayerManager.BroadcastToAuditChannel(session.Player, $"{session.Player.Name} has jailed {playerName}.");
        }

       // pardon < "char name" > - by Linae of DnF
        [CommandHandler("pardon", AccessLevel.Envoy, CommandHandlerFlag.RequiresWorld, 1,
            "Frees a jailed character.",
            "< \"char name\" \nThe character will be freed from the jail cell, restored to full ability usage and teleported to Holtburg.\nCharacter name MUST be enclosed in quotes.")]
        public static void HandlePardon(Session session, params string[] parameters)
        {
            var playerName = string.Join(" ", parameters[0]);

             if (parameters.Length >1)
            {
               session.Network.EnqueueSend(new GameMessageSystemChat($"Player name must be enclosed in double quotes. Ex: \"Player Name\"", ChatMessageType.Broadcast));
               return; 
            }

            var player = PlayerManager.GetOnlinePlayer(playerName);

            if (player == null)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Player {playerName} was not found.", ChatMessageType.Broadcast));
                return;
            } 

//            string cell = "A9B40019";
            string cell = "01AC011F"; // 0x [36.833786 -19.756327 0.005000] 0.709017 0.000000 0.000000 -0.705191

            player.RemoveProperty(ACE.Entity.Enum.Properties.PropertyBool.IsGagged);
            player.RemoveProperty(ACE.Entity.Enum.Properties.PropertyFloat.GagTimestamp);
            player.RemoveProperty(ACE.Entity.Enum.Properties.PropertyFloat.GagDuration);
            player.SetProperty(ACE.Entity.Enum.Properties.PropertyBool.RecallsDisabled, false);
            player.RemoveProperty(ACE.Entity.Enum.Properties.PropertyBool.YouAreJailed);

            player.SaveBiotaToDatabase();

//	         HandleTeleportPlayerLOC(session, playerName, cell.ToString(), 84.ToString(), 7.1.ToString(), 94.ToString(), 0.99.ToString(), 0.ToString(), 0.ToString(), 0.ToString());
        
            string pos3 = "-19.76";
            string pos8 = "-0.71";

            HandleTeleportPlayerLOC(session, playerName, cell.ToString(), 36.83.ToString(), pos3.ToString(), 0.01.ToString(), 0.71.ToString(), 0.ToString(), 0.ToString(), pos8.ToString());

            player.Session.Network.EnqueueSend(new GameMessageSystemChat($"{session.Player.Name} has freed you from jail.", ChatMessageType.Magic));
            CommandHandlerHelper.WriteOutputInfo(session, session.Player.Name + " pardoned " + playerName + ".", ChatMessageType.WorldBroadcast);
            PlayerManager.BroadcastToAuditChannel(session.Player, $"{session.Player.Name} pardoned {playerName}.");
        }
        
        // guidmovetoloc - by Linae of DNF
        // guidmovetoloc guid cell x y z [qx qy qz qw] 
        [CommandHandler("guidmovetoloc", AccessLevel.Advocate, CommandHandlerFlag.RequiresWorld,
         "Moves the specified object (guid) to the specified location.",
            "guid cell [x y z] (qw qx qy qz)\n" +
            "Hint: @guidmovetoloc follows the same number order as displayed from @loc output\n" +
            "Usage: @guidmovetoloc [guid cell x y z] (qw qx qy qz)\n" +
            "Example: @guidmovetoloc 800004DF 0x7F0401AD [12.319900 -28.482000 0.005000] -0.338946 0.000000 0.000000 -0.940806\n" +
            "Example: @guidmovetoloc 800004DF 0x7F0401AD 12.319900 -28.482000 0.005000 -0.338946 0.000000 0.000000 -0.940806\n" +
            "Example: @guidmovetoloc 800004DF 7F0401AD 12.319900 -28.482000 0.005000")]
        public static void HandleGuidMoveToLoc(Session session, params string[] parameters)
        {
            WorldObject obj = null;

            if (parameters[0].StartsWith("0x"))
                parameters[0] = parameters[0].Substring(2);

            if (!uint.TryParse(parameters[0], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint guid))
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"{parameters[0]} is not a valid guid", ChatMessageType.Broadcast));
                return;
            }

            obj = ServerObjectManager.GetObjectA(guid)?.WeenieObj?.WorldObject;

            if (obj == null)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Couldn't find object {parameters[0]}", ChatMessageType.Broadcast));
                return;
            }

            if (obj is Corpse && !obj.Level.HasValue) // Don't move creature corpses
                goto BadCorpse;

            var prevLoc = new Position(obj.Location);
            session.Network.EnqueueSend(new GameMessageSystemChat($"Object's previous location was: {prevLoc}", ChatMessageType.Broadcast));

            try
            {
                uint cell;

                if (parameters[1].StartsWith("0x"))
                {
                    string strippedcell = parameters[1].Substring(2);
                    cell = (uint)int.Parse(strippedcell, System.Globalization.NumberStyles.HexNumber);
                }
                else
                    cell = (uint)int.Parse(parameters[1], System.Globalization.NumberStyles.HexNumber);

                var positionData = new float[7];
                for (uint i = 0u; i < 7u; i++)
                {
                    if (i > 3 && parameters.Length < 9)
                    {
                        positionData[3] = 1;
                        positionData[4] = 0;
                        positionData[5] = 0;
                        positionData[6] = 0;
                        break;
                    }

                    if (!float.TryParse(parameters[i + 2].Trim(new Char[] { ' ', '[', ']' }), out var position))
                    {
                        session.Network.EnqueueSend(new GameMessageSystemChat($"A parameter is not a float and is causing an issue here.", ChatMessageType.Broadcast));
                        return;
                    }

                    positionData[i] = position;
                }

                var newLoc = new Position(cell, positionData[0], positionData[1], positionData[2], positionData[4], positionData[5], positionData[6], positionData[3]);
                newLoc.Rotation = prevLoc.Rotation;     // keep previous rotation

                var setPos = new Physics.Common.SetPosition(newLoc.PhysPosition(), Physics.Common.SetPositionFlags.Teleport | Physics.Common.SetPositionFlags.Slide);
                var result = obj.PhysicsObj.SetPosition(setPos);

                if (result != Physics.Common.SetPositionError.OK)
                {
                    session.Network.EnqueueSend(new GameMessageSystemChat($"Failed to move {obj.Name} ({obj.Guid}) to location: {result}", ChatMessageType.Broadcast));
                    return;
                }

                session.Network.EnqueueSend(new GameMessageSystemChat($"Moving {obj.Name} ({obj.Guid}) to location: {newLoc} {result}", ChatMessageType.Broadcast));

                obj.Location = obj.PhysicsObj.Position.ACEPosition();

                if (prevLoc.Landblock != obj.Location.Landblock)
                {
                    LandblockManager.RelocateObjectForPhysics(obj, true);
                }

                obj.SendUpdatePosition(true);
                PlayerManager.BroadcastToAuditChannel(session.Player, $"{session.Player.Name} has moved {obj.Name} (0x{obj.Guid}) to {cell:X8}, {positionData[0]}, {positionData[1]}, {positionData[2]}, {positionData[4]}, {positionData[5]}, {positionData[6]}, {positionData[3]}.");

            }
            catch (Exception)
            {
                ChatPacket.SendServerMessage(session, "Invalid arguments for @movetoloc", ChatMessageType.Broadcast);
                ChatPacket.SendServerMessage(session, "Hint: @guidmovetoloc follows the same number order as displayed from @loc output", ChatMessageType.Broadcast);
                ChatPacket.SendServerMessage(session, "Usage: @guidmovetoloc guid cell [x y z] (qw qx qy qz)", ChatMessageType.Broadcast);
                ChatPacket.SendServerMessage(session, "Example: @guidmovetoloc 800004DF 0x7F0401AD [12.319900 -28.482000 0.005000] -0.338946 0.000000 0.000000 -0.940806", ChatMessageType.Broadcast);
                ChatPacket.SendServerMessage(session, "Example: @guidmovetoloc 800004DF 0x7F0401AD 12.319900 -28.482000 0.005000 -0.338946 0.000000 0.000000 -0.940806", ChatMessageType.Broadcast);
                ChatPacket.SendServerMessage(session, "Example: @guidtoloc 800004DF 7F0401AD 12.319900 -28.482000 0.005000", ChatMessageType.Broadcast);
            }

        BadCorpse:
            ChatPacket.SendServerMessage(session, "Corpse was not a player corpse.", ChatMessageType.Broadcast);
        }

        // guidmovetome - by Linae of DNF
        // guidmovetome guid 
        [CommandHandler("guidmovetome", AccessLevel.Advocate, CommandHandlerFlag.RequiresWorld,
        "Moves the specified object (guid) to the current player location.",
        "Usage: @guidmovetoloc guid")]
        public static void HandleGuidMoveToMe(Session session, params string[] parameters)
        {
            WorldObject obj = null;

            if (parameters[0].StartsWith("0x"))
                parameters[0] = parameters[0].Substring(2);

            if (!uint.TryParse(parameters[0], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint guid))
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"{parameters[0]} is not a valid guid", ChatMessageType.Broadcast));
                return;
            }

            obj = ServerObjectManager.GetObjectA(guid)?.WeenieObj?.WorldObject;

            if (obj == null)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Couldn't find object {parameters[0]}", ChatMessageType.Broadcast));
                return;
            }

            if (obj.CurrentLandblock == null)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"{obj.Name} ({obj.Guid}) is not a landblock object", ChatMessageType.Broadcast));
                return;
            }

            if (obj is Player)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"{obj.Name} ({obj.Guid}) is a player.  Use /teletome instead.", ChatMessageType.Broadcast));
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
            PlayerManager.BroadcastToAuditChannel(session.Player, $"{session.Player.Name} has moved {obj.Name} (0x{obj.Guid}) to their current location.");
        }

        // roll a random number between 10 and 1000 inclusive and send it to local broadcast
        [CommandHandler("roll", AccessLevel.Player, CommandHandlerFlag.RequiresWorld, 1,
        "Rolls a random number between 1 and x where x = a number between 10 and 1000 and sends it to local broadcast.",
        "Usage: /roll <number> Where number is between 10 and 1000 inclusive.")]
        public static void DiceRoll(Session session, params string[] parameters) {

            if (!int.TryParse(parameters[0], out var maxRng))
            {
                session.Player.SendMessage($"{parameters[0]} is not a valid number. Please type a number between 10 and 1000 inclusive.");
                return;
            }

            if (maxRng < 10 || maxRng > 1000)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Please type a number between 10 and 1000 inclusive.", ChatMessageType.Broadcast));
                return;
            }

            var rng = (int)ThreadSafeRandom.Next(1,maxRng);
            var msg = $"I shook the dice and rolled a {rng}.";
            session.Player.EnqueueBroadcast(new GameMessageHearSpeech(msg, session.Player.Name, session.Player.Guid.Full, ChatMessageType.Emote), WorldObject.LocalBroadcastRange, ChatMessageType.Emote);
        }

        // roll a random number in a provided range inclusive and send it to local broadcast
        [CommandHandler("raffleroll", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 2,
        "Rolls a random number in a provided range inclusive and sends it to local broadcast in a raffle-like style.",
        "Usage: /raffleroll <min> <max>")]
        public static async void RaffleRoll(Session session, params string[] parameters)
        {
            if (!int.TryParse(parameters[0], out var minRng))
            {
                session.Player.SendMessage($"{parameters[0]} is not a valid number.");
                return;
            }

            if (!int.TryParse(parameters[1], out var maxRng))
            {
                session.Player.SendMessage($"{parameters[1]} is not a valid number.");
                return;
            }

            if(maxRng < minRng)
            {
                session.Player.SendMessage($"{parameters[1]} is not greater than {parameters[0]}. The second number must be larger than the first.");
                return;
            }            

            var rng = (int)ThreadSafeRandom.Next(minRng,maxRng);

            var msg = $"{session.Player.Name} is spinning the ticket bin!";
            GameMessageSystemChat sysMessage = new GameMessageSystemChat(msg, ChatMessageType.WorldBroadcast);
            PlayerManager.BroadcastToAll(sysMessage);
            PlayerManager.LogBroadcastChat(Channel.AllBroadcast, session?.Player, msg);

            await Task.Delay(3000);

            var msg1 = $"{session.Player.Name} has pulled a winning ticket from the bin!";
            GameMessageSystemChat sysMessage1 = new GameMessageSystemChat(msg1, ChatMessageType.WorldBroadcast);
            PlayerManager.BroadcastToAll(sysMessage1);
            PlayerManager.LogBroadcastChat(Channel.AllBroadcast, session?.Player, msg1);

            await Task.Delay(3000);

            var msg2 = $"The winning ticket number is *** ===> {rng} <=== ***";
            GameMessageSystemChat sysMessage2 = new GameMessageSystemChat(msg2, ChatMessageType.WorldBroadcast);
            PlayerManager.BroadcastToAll(sysMessage2);
            PlayerManager.LogBroadcastChat(Channel.AllBroadcast, session?.Player, msg2);

            await Task.Delay(2000);

            var msg3 = $"Congrats to the winner!!  If you didn't win, better luck next time!";
            GameMessageSystemChat sysMessage3 = new GameMessageSystemChat(msg3, ChatMessageType.WorldBroadcast);
            PlayerManager.BroadcastToAll(sysMessage3);
            PlayerManager.LogBroadcastChat(Channel.AllBroadcast, session?.Player, msg3);
        }
    
        // guidsetobjprop - by Linae of DNF
        // guidsetobjprop guid property value 
        [CommandHandler("guidsetobjprop", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 3,
         "Sets the specified property for an object (guid) to the specified value.",
            "/guidsetobjprop guid propertyname value")]
        public static void GuidSetObjProp(Session session, params string[] parameters)
        {
            WorldObject obj = null;

            if (parameters[0].StartsWith("0x"))
                parameters[0] = parameters[0].Substring(2);

            if (!uint.TryParse(parameters[0], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint guid))
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"{parameters[0]} is not a valid guid", ChatMessageType.Broadcast));
                return;
            }
            var prop = parameters[1];
            var value = parameters[2];

            var props = prop.Split('.');
            var propType = props[0];
            var propName = props[1];

        //Debugging
//            session.Network.EnqueueSend(new GameMessageSystemChat($"Parameter 0 = {parameters[0]}, Parameter 1 = {parameters[1]}, Parameter 2 = {parameters[2]}", ChatMessageType.Broadcast));
//            session.Network.EnqueueSend(new GameMessageSystemChat($"prop = {prop}, value = {value}, propType = {props[0]}, propName = {props[1]}", ChatMessageType.Broadcast));

            obj = ServerObjectManager.GetObjectA(guid)?.WeenieObj?.WorldObject;

        //Debugging
//            session.Network.EnqueueSend(new GameMessageSystemChat($"obj = {obj}", ChatMessageType.Broadcast));

            if (obj == null)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Couldn't find object {parameters[0]}", ChatMessageType.Broadcast));
                return;
            }

            if (props.Length != 2)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Unknown {prop}", ChatMessageType.Broadcast));
                return;
            }

            Type pType;
            if (propType.Equals("PropertyInt", StringComparison.OrdinalIgnoreCase))
                pType = typeof(PropertyInt);
            else if (propType.Equals("PropertyInt64", StringComparison.OrdinalIgnoreCase))
                pType = typeof(PropertyInt64);
            else if (propType.Equals("PropertyBool", StringComparison.OrdinalIgnoreCase))
                pType = typeof(PropertyBool);
            else if (propType.Equals("PropertyFloat", StringComparison.OrdinalIgnoreCase))
                pType = typeof(PropertyFloat);
            else if (propType.Equals("PropertyString", StringComparison.OrdinalIgnoreCase))
                pType = typeof(PropertyString);
            else if (propType.Equals("PropertyInstanceId", StringComparison.OrdinalIgnoreCase))
                pType = typeof(PropertyInstanceId);
            else if (propType.Equals("PropertyDataId", StringComparison.OrdinalIgnoreCase))
                pType = typeof(PropertyDataId);
            else
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Unknown property type: {propType}", ChatMessageType.Broadcast));
                return;
            }

            if (!Enum.TryParse(pType, propName, true, out var result))
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Couldn't find {prop}", ChatMessageType.Broadcast));
                return;
            }

            if (value == "null")
            {
                if (propType.Equals("PropertyInt", StringComparison.OrdinalIgnoreCase))
                    obj.RemoveProperty((PropertyInt)result);
                else if (propType.Equals("PropertyInt64", StringComparison.OrdinalIgnoreCase))
                    obj.RemoveProperty((PropertyInt64)result);
                else if (propType.Equals("PropertyBool", StringComparison.OrdinalIgnoreCase))
                    obj.RemoveProperty((PropertyBool)result);
                else if (propType.Equals("PropertyFloat", StringComparison.OrdinalIgnoreCase))
                    obj.RemoveProperty((PropertyFloat)result);
                else if (propType.Equals("PropertyString", StringComparison.OrdinalIgnoreCase))
                    obj.RemoveProperty((PropertyString)result);
                else if (propType.Equals("PropertyInstanceId", StringComparison.OrdinalIgnoreCase))
                    obj.RemoveProperty((PropertyInstanceId)result);
                else if (propType.Equals("PropertyDataId", StringComparison.OrdinalIgnoreCase))
                    obj.RemoveProperty((PropertyDataId)result);
            }
            else
            {
                try
                {
                    if (propType.Equals("PropertyInt", StringComparison.OrdinalIgnoreCase))
                    {
                        var intValue = Convert.ToInt32(value, value.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? 16 : 10);

                        session.Player.UpdateProperty(obj, (PropertyInt)result, intValue, true);
                    }
                    else if (propType.Equals("PropertyInt64", StringComparison.OrdinalIgnoreCase))
                    {
                        var int64Value = Convert.ToInt64(value, value.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? 16 : 10);

                        session.Player.UpdateProperty(obj, (PropertyInt64)result, int64Value, true);
                    }
                    else if (propType.Equals("PropertyBool", StringComparison.OrdinalIgnoreCase))
                    {
                        var boolValue = Convert.ToBoolean(value);

                        session.Player.UpdateProperty(obj, (PropertyBool)result, boolValue, true);
                    }
                    else if (propType.Equals("PropertyFloat", StringComparison.OrdinalIgnoreCase))
                    {
                        var floatValue = Convert.ToDouble(value);

                        session.Player.UpdateProperty(obj, (PropertyFloat)result, floatValue, true);
                    }
                    else if (propType.Equals("PropertyString", StringComparison.OrdinalIgnoreCase))
                    {
                        session.Player.UpdateProperty(obj, (PropertyString)result, value, true);
                    }
                    else if (propType.Equals("PropertyInstanceId", StringComparison.OrdinalIgnoreCase))
                    {
                        var iidValue = Convert.ToUInt32(value, value.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? 16 : 10);

                        session.Player.UpdateProperty(obj, (PropertyInstanceId)result, iidValue, true);
                    }
                    else if (propType.Equals("PropertyDataId", StringComparison.OrdinalIgnoreCase))
                    {
                        var didValue = Convert.ToUInt32(value, value.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? 16 : 10);

                        session.Player.UpdateProperty(obj, (PropertyDataId)result, didValue, true);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return;
                }
            }
            session.Network.EnqueueSend(new GameMessageSystemChat($"{obj.Name} ({obj.Guid}): {prop} = {value}", ChatMessageType.Broadcast));
            PlayerManager.BroadcastToAuditChannel(session.Player, $"{session.Player.Name} changed a property for {obj.Name} ({obj.Guid}): {prop} = {value}");
        }

        // guidsetplayerprop - by Linae of DNF
        // guidsetplayerprop guid property value 
        [CommandHandler("guidsetplayerprop", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 3,
         "Sets the specified property for a player (guid) to the specified value.",
            "/guidsetplayerprop guid propertyname value")]
        public static void GuidSetPlayerProp(Session session, params string[] parameters)
        {
            if (parameters[0].StartsWith("0x"))
                parameters[0] = parameters[0].Substring(2);

            if (!uint.TryParse(parameters[0], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint guid))
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"{parameters[0]} is not a valid guid", ChatMessageType.Broadcast));
                return;
            }

            var player = PlayerManager.FindByGuid(guid, out bool isOnline);
            var prop = parameters[1];
            var value = parameters[2];
            var props = prop.Split('.');
            var propType = props[0];
            var propName = props[1];

/*        //Debugging
            session.Network.EnqueueSend(new GameMessageSystemChat($"Parameter 0 = {parameters[0]}, Parameter 1 = {parameters[1]}, Parameter 2 = {parameters[2]}", ChatMessageType.Broadcast));
            session.Network.EnqueueSend(new GameMessageSystemChat($"prop = {prop}, value = {value}, propType = {props[0]}, propName = {props[1]}", ChatMessageType.Broadcast));
            session.Network.EnqueueSend(new GameMessageSystemChat($"player = {player} propslength = {props.Length}", ChatMessageType.Broadcast));
*/
            if (props.Length != 2)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Unknown property: {prop}", ChatMessageType.Broadcast));
                return;
            }

            Type pType;
            if (propType.Equals("PropertyInt", StringComparison.OrdinalIgnoreCase))
                pType = typeof(PropertyInt);
            else if (propType.Equals("PropertyInt64", StringComparison.OrdinalIgnoreCase))
                pType = typeof(PropertyInt64);
            else if (propType.Equals("PropertyBool", StringComparison.OrdinalIgnoreCase))
                pType = typeof(PropertyBool);
            else if (propType.Equals("PropertyFloat", StringComparison.OrdinalIgnoreCase))
                pType = typeof(PropertyFloat);
            else if (propType.Equals("PropertyString", StringComparison.OrdinalIgnoreCase))
                pType = typeof(PropertyString);
            else if (propType.Equals("PropertyInstanceId", StringComparison.OrdinalIgnoreCase))
                pType = typeof(PropertyInstanceId);
            else if (propType.Equals("PropertyDataId", StringComparison.OrdinalIgnoreCase))
                pType = typeof(PropertyDataId);
            else
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Unknown property type: {propType}", ChatMessageType.Broadcast));
                return;
            }

            if (!Enum.TryParse(pType, propName, true, out var result))
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Couldn't find property: {prop}", ChatMessageType.Broadcast));
                return;
            }

            if (value == "null" && !isOnline)
            {
                var offlinePlayer = PlayerManager.GetOfflinePlayer(guid);
                if (offlinePlayer == null)
                {
                    session.Network.EnqueueSend(new GameMessageSystemChat($"Couldn't find offline player: {guid}", ChatMessageType.Broadcast));
                    log.Warn($"[GUIDPROPERTY] Couldn't find offline player: {guid}");
                    return;
                }

                if (propType.Equals("PropertyInt", StringComparison.OrdinalIgnoreCase))
                    offlinePlayer.RemoveProperty((PropertyInt)result);
                else if (propType.Equals("PropertyInt64", StringComparison.OrdinalIgnoreCase))
                    offlinePlayer.RemoveProperty((PropertyInt64)result);
                else if (propType.Equals("PropertyBool", StringComparison.OrdinalIgnoreCase))
                    offlinePlayer.RemoveProperty((PropertyBool)result);
                else if (propType.Equals("PropertyFloat", StringComparison.OrdinalIgnoreCase))
                    offlinePlayer.RemoveProperty((PropertyFloat)result);
                else if (propType.Equals("PropertyString", StringComparison.OrdinalIgnoreCase))
                    offlinePlayer.RemoveProperty((PropertyString)result);
                else if (propType.Equals("PropertyInstanceId", StringComparison.OrdinalIgnoreCase))
                    offlinePlayer.RemoveProperty((PropertyInstanceId)result);
                else if (propType.Equals("PropertyDataId", StringComparison.OrdinalIgnoreCase))
                    offlinePlayer.RemoveProperty((PropertyDataId)result);
            }
            else if (value == "null" && isOnline)
            {
                var onlinePlayer = PlayerManager.GetOnlinePlayer(guid);
                if (onlinePlayer == null)
                {
                    session.Network.EnqueueSend(new GameMessageSystemChat($"Couldn't find online player: {guid}", ChatMessageType.Broadcast));
                    log.Warn($"[GUIDPROPERTY] Couldn't find online player: {guid}");
                    return;
                }

                if (propType.Equals("PropertyInt", StringComparison.OrdinalIgnoreCase))
                    onlinePlayer.RemoveProperty((PropertyInt)result);
                else if (propType.Equals("PropertyInt64", StringComparison.OrdinalIgnoreCase))
                    onlinePlayer.RemoveProperty((PropertyInt64)result);
                else if (propType.Equals("PropertyBool", StringComparison.OrdinalIgnoreCase))
                    onlinePlayer.RemoveProperty((PropertyBool)result);
                else if (propType.Equals("PropertyFloat", StringComparison.OrdinalIgnoreCase))
                    onlinePlayer.RemoveProperty((PropertyFloat)result);
                else if (propType.Equals("PropertyString", StringComparison.OrdinalIgnoreCase))
                    onlinePlayer.RemoveProperty((PropertyString)result);
                else if (propType.Equals("PropertyInstanceId", StringComparison.OrdinalIgnoreCase))
                    onlinePlayer.RemoveProperty((PropertyInstanceId)result);
                else if (propType.Equals("PropertyDataId", StringComparison.OrdinalIgnoreCase))
                    onlinePlayer.RemoveProperty((PropertyDataId)result);
            }
            else
            {
                try
                {
                    if (!isOnline)
                    {
                        // set property of offline player
                        var offlinePlayer = PlayerManager.GetOfflinePlayer(guid);
                        if (offlinePlayer == null)
                        {
                        session.Network.EnqueueSend(new GameMessageSystemChat($"Couldn't find offline player: {guid}", ChatMessageType.Broadcast));
                        log.Warn($"[GUIDPROPERTY] Couldn't find offline player: {guid}");
                        return;
                        }

                        if (propType.Equals("PropertyInt", StringComparison.OrdinalIgnoreCase))
                        {
                            var intValue = Convert.ToInt32(value, value.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? 16 : 10);
                            offlinePlayer.SetProperty((PropertyInt)result, intValue);

                        }
                        else if (propType.Equals("PropertyInt64", StringComparison.OrdinalIgnoreCase))
                        {
                            var int64Value = Convert.ToInt64(value, value.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? 16 : 10);
                            offlinePlayer.SetProperty((PropertyInt64)result, int64Value);
                        }
                        else if (propType.Equals("PropertyBool", StringComparison.OrdinalIgnoreCase))
                        {
                            var boolValue = Convert.ToBoolean(value);
                            offlinePlayer.SetProperty((PropertyBool)result, boolValue);
                        }
                        else if (propType.Equals("PropertyFloat", StringComparison.OrdinalIgnoreCase))
                        {
                            var floatValue = Convert.ToDouble(value);
                            offlinePlayer.SetProperty((PropertyFloat)result, floatValue);
                        }
                        else if (propType.Equals("PropertyString", StringComparison.OrdinalIgnoreCase))
                        {
                            offlinePlayer.SetProperty((PropertyString)result, value);
                        }
                        else if (propType.Equals("PropertyInstanceId", StringComparison.OrdinalIgnoreCase))
                        {
                            var iidValue = Convert.ToUInt32(value, value.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? 16 : 10);
                            offlinePlayer.SetProperty((PropertyInstanceId)result, iidValue);
                        }
                        else if (propType.Equals("PropertyDataId", StringComparison.OrdinalIgnoreCase))
                        {
                            var didValue = Convert.ToUInt32(value, value.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? 16 : 10);
                            offlinePlayer.SetProperty((PropertyDataId)result, didValue);
                        }

                    // show success
                        offlinePlayer.SaveBiotaToDatabase();
                        session.Network.EnqueueSend(new GameMessageSystemChat($"Player {guid}: {prop} = {value}", ChatMessageType.Broadcast));
                        PlayerManager.BroadcastToAuditChannel(session.Player, $"{session.Player.Name} changed a property for offline player {guid}: {prop} = {value}");
                        return;
                    }

                    if (isOnline)
                    {
                        // set property of online player
                        var onlinePlayer = PlayerManager.GetOnlinePlayer(guid);
                        if (onlinePlayer == null)
                        {
                        session.Network.EnqueueSend(new GameMessageSystemChat($"Couldn't find online player: {guid}", ChatMessageType.Broadcast));
                        log.Warn($"[GUIDPROPERTY] Couldn't find online player: {guid}");
                        return;
                        }
 
                        if (propType.Equals("PropertyInt", StringComparison.OrdinalIgnoreCase))
                        {
                            var intValue = Convert.ToInt32(value, value.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? 16 : 10);
                            onlinePlayer.SetProperty((PropertyInt)result, intValue);
                            onlinePlayer.EnqueueBroadcast(new GameMessagePublicUpdatePropertyInt(onlinePlayer, (PropertyInt)result, intValue));
                        }
                        else if (propType.Equals("PropertyInt64", StringComparison.OrdinalIgnoreCase))
                        {
                            var int64Value = Convert.ToInt64(value, value.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? 16 : 10);
                            onlinePlayer.SetProperty((PropertyInt64)result, int64Value);
                            onlinePlayer.EnqueueBroadcast(new GameMessagePublicUpdatePropertyInt64(onlinePlayer, (PropertyInt64)result, int64Value));
                        }
                        else if (propType.Equals("PropertyBool", StringComparison.OrdinalIgnoreCase))
                        {
                            var boolValue = Convert.ToBoolean(value);
                            onlinePlayer.SetProperty((PropertyBool)result, boolValue);
                            onlinePlayer.EnqueueBroadcast(new GameMessagePublicUpdatePropertyBool(onlinePlayer, (PropertyBool)result, boolValue));
                        }
                        else if (propType.Equals("PropertyFloat", StringComparison.OrdinalIgnoreCase))
                        {
                            var floatValue = Convert.ToDouble(value);
                            onlinePlayer.SetProperty((PropertyFloat)result, floatValue);
                            onlinePlayer.EnqueueBroadcast(new GameMessagePublicUpdatePropertyFloat(onlinePlayer, (PropertyFloat)result, floatValue));
                        }
                        else if (propType.Equals("PropertyString", StringComparison.OrdinalIgnoreCase))
                        {
                            onlinePlayer.SetProperty((PropertyString)result, value);
                            onlinePlayer.EnqueueBroadcast(new GameMessagePublicUpdatePropertyString(onlinePlayer, (PropertyString)result, value));
                        }
                        else if (propType.Equals("PropertyInstanceId", StringComparison.OrdinalIgnoreCase))
                        {
                            var iidValue = Convert.ToUInt32(value, value.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? 16 : 10);
                            onlinePlayer.SetProperty((PropertyInstanceId)result, iidValue);
                            //onlinePlayer.EnqueueBroadcast(new GameMessagePublicUpdatePropertyInstanceID(onlinePlayer, (PropertyInstanceId)result, iidValue));
                            session.Network.EnqueueSend(new GameMessageSystemChat($"You can change an IID, but it won't take immediate effect.  Player will need to relog.", ChatMessageType.Broadcast));
                            log.Warn($"[GUIDPROPERTY] Changed an IID.  Player needs to relog to take effect.");

                        }
                        else if (propType.Equals("PropertyDataId", StringComparison.OrdinalIgnoreCase))
                        {
                            var didValue = Convert.ToUInt32(value, value.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? 16 : 10);
                            onlinePlayer.SetProperty((PropertyDataId)result, didValue);
                            onlinePlayer.EnqueueBroadcast(new GameMessagePublicUpdatePropertyDataID(onlinePlayer, (PropertyDataId)result, didValue));
                        }

                    // show success
                        onlinePlayer.SaveBiotaToDatabase();
                        session.Network.EnqueueSend(new GameMessageSystemChat($"Player {guid}: {prop} = {value}", ChatMessageType.Broadcast));
                        session.Network.EnqueueSend(new GameMessageSystemChat($"Note that some properties require the player to relog before becoming effective.", ChatMessageType.Broadcast));
                        PlayerManager.BroadcastToAuditChannel(session.Player, $"{session.Player.Name} changed a property for online player {guid}: {prop} = {value}");
                        PlayerManager.BroadcastToAuditChannel(session.Player, $"Note that some properties require the player to relog before becoming effective.");
                        return;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return;
                }
            }
        }

        // char-removeplus by Linae of DnF
        // This command fixes the issue of individual characters still having admin abilities after being demoted back to level 0.
        // Use /demoteaccounttoplayer on the account if you originally used set-accountaccess to grant the rights.
        [CommandHandler("char-removeplus", AccessLevel.Admin, CommandHandlerFlag.None,
            "Removes the isPlussed attribute from a named character and strips all admin abilities", 
            "Use /demoteaccounttoplayer on the account if you originally used set-accountaccess to grant the rights." +
            "usage: /char-removeplus charactername")]

        public static void DNFHandleCharRemovePlus(Session session, params string[] parameters)
        {
            var characterName = string.Join(" ", parameters);

            var foundPlayer = PlayerManager.FindByName(characterName);

            if (foundPlayer == null)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"There is no character named '{characterName}' in the database.", ChatMessageType.Broadcast);
                return;
            }
            else
            {
                    foundPlayer.SetProperty(PropertyBool.AdvocateState, false);
                    foundPlayer.SetProperty(PropertyBool.Attackable, true);
                    foundPlayer.SetProperty(PropertyBool.DamagedByCollisions, true);
                    foundPlayer.SetProperty(PropertyBool.IgnoreHouseBarriers, false);
                    foundPlayer.SetProperty(PropertyBool.IgnorePortalRestrictions, false);
                    foundPlayer.SetProperty(PropertyBool.Invincible, false);
                    foundPlayer.SetProperty(PropertyBool.IsAdmin, false);
                    foundPlayer.SetProperty(PropertyBool.IsAdvocate, false);
                    foundPlayer.SetProperty(PropertyBool.IsEnvoy, false);
                    foundPlayer.SetProperty(PropertyBool.IsSentinel, false);
                    foundPlayer.SetProperty(PropertyBool.NeverFailCasting, false);
                    foundPlayer.SetProperty(PropertyBool.NoCorpse, false);
                    foundPlayer.SetProperty(PropertyBool.ReportCollisions, true);
                    foundPlayer.SetProperty(PropertyBool.SafeSpellComponents, false);
                    foundPlayer.SetProperty(PropertyInt.CloakStatus, (int)CloakStatus.Undef);
                    foundPlayer.RemoveProperty(PropertyInt.AdvocateLevel);
                    foundPlayer.RemoveProperty(PropertyInt.ChannelsActive);
                    foundPlayer.RemoveProperty(PropertyInt.ChannelsAllowed);

                var existingCharId = foundPlayer.Guid.Full;

                ACE.Database.DatabaseManager.Shard.GetCharacter(existingCharId, character =>
                    {
                        if (character != null)
                        {
                            character.IsPlussed = false;
                        }
                        else
                        {
                            CommandHandlerHelper.WriteOutputInfo(session, $"Unable to process character named '{characterName}' due to shard database GetCharacter failure.", ChatMessageType.Broadcast);
                            return;                        
                        }
                });

                foundPlayer.SaveBiotaToDatabase();
                CommandHandlerHelper.WriteOutputInfo(session, session.Player.Name + " has demoted '" + characterName + "' to level 0 (Player) and reset all admin abilities. If character was online a forcelogoff was issued.", ChatMessageType.WorldBroadcast);
                PlayerManager.BroadcastToAuditChannel(session.Player, $"[AUDIT] {session.Player.Name} has demoted '{characterName}' to level 0 (Player) and reset all admin abilities. If character was online a forcelogoff was issued.");
                DNFHandleForceLogoff(session, characterName);
            }
        }

        [CommandHandler("ForceLogoffStuckCharacter", AccessLevel.Player, CommandHandlerFlag.RequiresWorld, "Force log off of character that's stuck in game.  Is only allowed when initiated from a character that is on the same account as the target character.")]
        public static void HandleForceLogoffStuckCharacter(Session session, params string[] parameters)
        {
            var playerName = "";
            if (parameters.Length > 0)
                playerName = string.Join(" ", parameters);

            Player target = null;

            if (!string.IsNullOrEmpty(playerName))
            {
                var plr = PlayerManager.FindByName(playerName);
                if (plr != null)
                {
                    target = PlayerManager.GetOnlinePlayer(plr.Guid);

                    if (target == null)
                    {
                        CommandHandlerHelper.WriteOutputInfo(session, $"Unable to force log off for {plr.Name}: Player is not online.");
                        return;
                    }

                    //Verify the target is not the current player
                    if(session.Player.Guid == target.Guid)
                    {
                        CommandHandlerHelper.WriteOutputInfo(session, $"Unable to force log off for {plr.Name}: You cannot target yourself, please try with a different character on same account.");
                        return;
                    }

                    //Verify the target is on the same account as the current player
                    if (session.AccountId != target.Account.AccountId)
                    {
                        CommandHandlerHelper.WriteOutputInfo(session, $"Unable to force log off for {plr.Name}: Target must be within same account as the player who issues the logoff command. Please reach out for admin support.");
                        return;
                    }

                    DeveloperCommands.HandleForceLogoff(session, parameters);
                }
                else
                {
                    CommandHandlerHelper.WriteOutputInfo(session, $"Unable to force log off for {playerName}: Player not found.");
                    return;
                }
            }
            else
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Invalid parameters, please provide a player name for the player that needs to be logged off.");
                return;
            }            
        }

        // demoteaccounttoplayer by Linae of DnF
        // This command demotes any account and its characters back to level 0
        // and removes all admin abilities from all characters on the named account.
        // This fixes an issue with the stock ACE set-accountaccess and set-characteraccess commands
        // where admin powers were not stripped from characters post demotion.
        [CommandHandler("demoteaccounttoplayer", AccessLevel.Admin, CommandHandlerFlag.None,
            "Demotes the named Admin/Dev/Envoy/Sent/Advocate account back to player and",
            "strips all admin abilities from all characters on named account." +
            "Usage: /demoteaccounttoplayer accountname")]
        
        public static void DNFHandleDemotion(Session session, params string[] parameters)
        {
            string accountName = string.Join(" ", parameters).ToLower();
            var accountId = DatabaseManager.Authentication.GetAccountIdByName(accountName);

            CommandHandlerHelper.WriteOutputInfo(session, session.Player.Name + " has invoked demoteaccounttoplayer for account: '" + accountName + "' with ID (" + accountId + ").", ChatMessageType.WorldBroadcast);
            PlayerManager.BroadcastToAuditChannel(session.Player, $"[AUDIT] {session.Player.Name} has invoked demoteaccounttoplayer for account: '{accountName}' with ID ({accountId}).");

            if (accountId == 0)
            {
                CommandHandlerHelper.WriteOutputInfo(session, "Account: '" + accountName + "' does not exist.", ChatMessageType.Broadcast);
                return;
            }

            AccessLevel accessLevel = AccessLevel.Player;
            accessLevel = AccessLevel.Player;

            DatabaseManager.Authentication.UpdateAccountAccessLevel(accountId, accessLevel);

            if (DatabaseManager.AutoPromoteNextAccountToAdmin && accessLevel == AccessLevel.Admin)
                DatabaseManager.AutoPromoteNextAccountToAdmin = false;

            CommandHandlerHelper.WriteOutputInfo(session, "Account: '" + accountName + "' with id (" + accountId + ") updated with access rights set as a player by " + session.Player.Name + ".", ChatMessageType.Broadcast);
            PlayerManager.BroadcastToAuditChannel(session.Player, $"[AUDIT] Account: '{accountName}' with id ({accountId}) updated with access rights set as a player by {session.Player.Name}.");

            var characters = DatabaseManager.Shard.BaseDatabase.GetCharacters(accountId, true);
            var characterName = "";

            foreach(var character in characters)
            {
                characterName = character.Name;
                var foundPlayer = PlayerManager.FindByName(characterName);

                if (foundPlayer == null)
                {
                    CommandHandlerHelper.WriteOutputInfo(session, $"There is no character named '{characterName}' in the database.", ChatMessageType.Broadcast);
                    return;
                }
                else
                {
                    foundPlayer.SetProperty(PropertyBool.AdvocateState, false);
                    foundPlayer.SetProperty(PropertyBool.Attackable, true);
                    foundPlayer.SetProperty(PropertyBool.DamagedByCollisions, true);
                    foundPlayer.SetProperty(PropertyBool.IgnoreHouseBarriers, false);
                    foundPlayer.SetProperty(PropertyBool.IgnorePortalRestrictions, false);
                    foundPlayer.SetProperty(PropertyBool.Invincible, false);
                    foundPlayer.SetProperty(PropertyBool.IsAdmin, false);
                    foundPlayer.SetProperty(PropertyBool.IsAdvocate, false);
                    foundPlayer.SetProperty(PropertyBool.IsEnvoy, false);
                    foundPlayer.SetProperty(PropertyBool.IsSentinel, false);
                    foundPlayer.SetProperty(PropertyBool.NeverFailCasting, false);
                    foundPlayer.SetProperty(PropertyBool.NoCorpse, false);
                    foundPlayer.SetProperty(PropertyBool.ReportCollisions, true);
                    foundPlayer.SetProperty(PropertyBool.SafeSpellComponents, false);
                    foundPlayer.SetProperty(PropertyInt.CloakStatus, (int)CloakStatus.Undef);
                    foundPlayer.RemoveProperty(PropertyInt.AdvocateLevel);
                    foundPlayer.RemoveProperty(PropertyInt.ChannelsActive);
                    foundPlayer.RemoveProperty(PropertyInt.ChannelsAllowed);

                    var existingCharId = foundPlayer.Guid.Full;

                    ACE.Database.DatabaseManager.Shard.GetCharacter(existingCharId, character =>
                        {
                            if (character != null)
                            {
                                character.IsPlussed = false;
                            }
                            else
                            {
                                CommandHandlerHelper.WriteOutputInfo(session, $"Unable to process character named '{characterName}' due to shard database GetCharacter failure.", ChatMessageType.Broadcast);
                                return;                        
                            }
                    });

                    foundPlayer.SaveBiotaToDatabase();
                    CommandHandlerHelper.WriteOutputInfo(session, session.Player.Name + " has demoted character '" + characterName + "' to level 0 (Player) and reset all admin abilities. If player was online a forcelogout was issued.", ChatMessageType.WorldBroadcast);
                    PlayerManager.BroadcastToAuditChannel(session.Player, $"[AUDIT] {session.Player.Name} has demoted character '{characterName}' to level 0 (Player) and reset all admin abilities. If player was online a forcelogout was issued.");
                    DNFHandleForceLogoff(session, characterName);
                }
            }        
        }

        // this is just a modified version of the stock ACE handler to make it significantly less spammy.
        private static void DNFHandleForceLogoff(Session session, params string[] parameters)
        {
            var playerName = "";
            if (parameters.Length > 0)
                playerName = string.Join(" ", parameters);

            WorldObject target = null;

            if (!string.IsNullOrEmpty(playerName))
            {
                var plr = PlayerManager.FindByName(playerName);
                if (plr != null)
                {
                    target = PlayerManager.GetOnlinePlayer(plr.Guid);

                    if (target == null)
                    {
                        CommandHandlerHelper.WriteOutputInfo(session, $"Unable to force log off for '{plr.Name}': Character is not online.");
                        return;
                    }
                }
                else
                {
                    CommandHandlerHelper.WriteOutputInfo(session, $"Unable to force log off for '{playerName}': Character not found in manager.");
                    return;
                }
            }

            if (target != null && target is Player player)
            {

                var msg = $"Character '{player.Name}' found online.";
                var foundOnLandblock = false;

                if (player.CurrentLandblock != null)
                    foundOnLandblock = LandblockManager.GetLandblock(player.CurrentLandblock.Id, false).GetObject(player.Guid) != null;
                var playerForcedLogOffRequested = player.ForcedLogOffRequested;

                if (playerForcedLogOffRequested)
                {
                    player.Session?.Terminate(Network.Enum.SessionTerminationReason.ForcedLogOffRequested, new GameMessageBootAccount(" because the character was forced to log off by an admin"));
                    player.ForceLogoff();
                }
                else if (player.Session != null)
                {
                    player.ForcedLogOffRequested = true;
                    player.Session.Terminate(Network.Enum.SessionTerminationReason.ForcedLogOffRequested, new GameMessageBootAccount(" because the character was forced to log off by an admin"));
                }
                else if (player.CurrentLandblock != null && foundOnLandblock)
                {
                    player.ForcedLogOffRequested = true;
                    player.LogOut();
                }
                else if (player.IsInDeathProcess)
                {
                    player.ForcedLogOffRequested = true;
                    player.IsInDeathProcess = false;
                    player.LogOut_Inner(true);
                }
                else
                {
                    player.ForcedLogOffRequested = true;
                }

                CommandHandlerHelper.WriteOutputInfo(session, msg);
                PlayerManager.BroadcastToAuditChannel(session?.Player, $"[AUDIT] Forcing logoff of character: '{player.Name}'...");
            }
            else
            {
                if (target != null)
                    CommandHandlerHelper.WriteOutputInfo(session, $"Unable to force log off for '{target.Name}': Target is not a player.");
            }
        }

        [CommandHandler("setlbenvironbyCellID", AccessLevel.Sentinel, CommandHandlerFlag.RequiresWorld, 1,
            "Sets or clears the specified landblock's environment option",
            "(cell id, name or id of EnvironChangeType)\nleave blank to reset to default.\nlist to get a complete list of options.")]
        public static void DNFHandleSetLBEnvironbyID(Session session, params string[] parameters)
        {
            EnvironChangeType environChange = EnvironChangeType.Clear;
            uint cell;

            if (parameters[0].StartsWith("0x"))
            {
                string strippedcell = parameters[0].Substring(2);
                cell = (uint)int.Parse(strippedcell, System.Globalization.NumberStyles.HexNumber);
            }
            else
                cell = (uint)int.Parse(parameters[0], System.Globalization.NumberStyles.HexNumber);

            var landblockid = cell << 16;

            if (parameters[1] == "list")
            {
                session.Network.EnqueueSend(new GameMessageSystemChat(EnvironListMsg(), ChatMessageType.Broadcast));
                return;
            }

            var target = LandblockManager.GetLandblock(new LandblockId(landblockid), false);

            if (Enum.TryParse(parameters[1], true, out environChange))
            {
                if (!Enum.IsDefined(typeof(EnvironChangeType), environChange))
                    environChange = EnvironChangeType.Clear;
            }

            if (environChange.IsFog())
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Setting Landblock (0x{target:X4}), including direct adjacent landblocks, to EnvironChangeType.{environChange.ToString()}.", ChatMessageType.Broadcast));
                PlayerManager.BroadcastToAuditChannel(session.Player, $"{session.Player.Name} set Landblock (0x{target:X4}), including direct adjacent landblocks, to EnvironChangeType.{environChange.ToString()}.");
            }
            else
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Sending EnvironChangeType.{environChange.ToString()} to all players on Landblock (0x{target:X4}), including direct adjacent landblocks.", ChatMessageType.Broadcast));
                PlayerManager.BroadcastToAuditChannel(session.Player, $"{session.Player.Name} sent EnvironChangeType.{environChange.ToString()} to all players on Landblock (0x{target:X4}), including direct adjacent landblocks.");
            }

            target?.DoEnvironChange(environChange);
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

        [CommandHandler("ipdupes", AccessLevel.Envoy, CommandHandlerFlag.None, 0, "If a single IP address has more than 3 connections, displays all connections for that address.")]
        public static void DNFHandleIPDupes(Session session, params string[] parameters)
        {
            AccessLevel? targetAccessLevel = null;
            if (parameters?.Length > 0)
            {
                if (Enum.TryParse(parameters[0], true, out AccessLevel parsedAccessLevel))
                {
                    targetAccessLevel = parsedAccessLevel;
                }
                else
                {
                    try
                    {
                        uint accessLevel = Convert.ToUInt16(parameters[0]);
                        targetAccessLevel = (AccessLevel)accessLevel;
                    }
                    catch (Exception)
                    {
                        CommandHandlerHelper.WriteOutputInfo(session, "Invalid AccessLevel value", ChatMessageType.Broadcast);
                        return;
                    }
                }
            }

            if (targetAccessLevel.HasValue)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Listing only {targetAccessLevel.Value.ToString()}s:", ChatMessageType.Broadcast);
            }

            var players = PlayerManager.GetAllOnline().ToList();
            var addressCount = new Dictionary<string, int>();

            foreach (var player in players)
            {
                string address = player.Session.EndPointC2S.Address.ToString();
                if (addressCount.ContainsKey(address))
                {
                    addressCount[address]++;
                }
                else
                {
                    addressCount[address] = 1;
                }
            }

            int threshold = 3;
            var playersWithDuplicates = new List<Player>();

            foreach (var player in players)
            {
                string address = player.Session.EndPointC2S.Address.ToString();
                if (addressCount[address] > threshold)
                {
                    playersWithDuplicates.Add(player);
                }
            }

            playersWithDuplicates.Sort((a, b) =>
            {
                int result = a.Session.EndPointC2S.Address.ToString().CompareTo(b.Session.EndPointC2S.Address.ToString());
                if (result == 0)
                {
                    result = a.Account.AccountName.ToString().CompareTo(b.Account.AccountName.ToString());
                }
                return result;
            });

            if (!playersWithDuplicates.Any())
            {
                CommandHandlerHelper.WriteOutputInfo(session, "There are currently no IP addresses with more than 3 connections.", ChatMessageType.Broadcast);
                return;
            }

            foreach (var player in playersWithDuplicates)
            {
                if (targetAccessLevel.HasValue && player.Account.AccessLevel != ((uint)targetAccessLevel.Value))
                    continue;

                string message = $"Player:\"{player.Name}\" AccountName:\"{player.Account.AccountName}\" IP:\"{player.Session.EndPointC2S.Address}\"";
                CommandHandlerHelper.WriteOutputInfo(session, message, ChatMessageType.Broadcast);
            }
        }

       [CommandHandler("iplist", AccessLevel.Envoy, CommandHandlerFlag.None, 0, "List all connections sorted by IP address then AccountName")]
        public static void DNFHandleListAllIP(Session session, params string[] parameters)
        {
            string message = "";
            uint playerCounter = 0;

            AccessLevel? targetAccessLevel = null;
            if (parameters?.Length > 0)
            {
                if (Enum.TryParse(parameters[0], true, out AccessLevel parsedAccessLevel))
                {
                    targetAccessLevel = parsedAccessLevel;
                }
                else
                {
                    try
                    {
                        uint accessLevel = Convert.ToUInt16(parameters[0]);
                        targetAccessLevel = (AccessLevel)accessLevel;
                    }
                    catch (Exception)
                    {
                        CommandHandlerHelper.WriteOutputInfo(session, "Invalid AccessLevel value", ChatMessageType.Broadcast);
                        return;
                    }
                }
            }

            var players = PlayerManager.GetAllOnline().ToList();

            players.Sort((a, b) =>
            {
                int result = a.Session.EndPointC2S.Address.ToString().CompareTo(b.Session.EndPointC2S.Address.ToString());
                if (result == 0)
                {
                    result = a.Account.AccountName.ToString().CompareTo(b.Account.AccountName.ToString());
                }
                return result;
            });

            if (targetAccessLevel.HasValue)
                message += $"Listing only {targetAccessLevel.Value.ToString()}s:\n";

            foreach (var player in players)
            {
                if (targetAccessLevel.HasValue && player.Account.AccessLevel != ((uint)targetAccessLevel.Value))
                    continue;
                message += $"Player:\"{player.Name}\" AccountName:\"{player.Account.AccountName}\" IP:\"{player.Session.EndPointC2S.Address}\"\n";;
                playerCounter++;
            }
            CommandHandlerHelper.WriteOutputInfo(session, message, ChatMessageType.Broadcast);
        }
    }
}
