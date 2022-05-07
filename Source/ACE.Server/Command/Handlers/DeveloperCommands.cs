using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Numerics;

using log4net;

using ACE.Common;
using ACE.Common.Extensions;
using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Factories;
using ACE.Server.Factories.Enum;
using ACE.Server.Managers;
using ACE.Server.Network;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Physics.Entity;
using ACE.Server.Physics.Extensions;
using ACE.Server.Physics.Managers;
using ACE.Server.WorldObjects;
using ACE.Server.WorldObjects.Entity;


using Position = ACE.Entity.Position;
using Spell = ACE.Server.Entity.Spell;

namespace ACE.Server.Command.Handlers
{
    public static class DeveloperCommands
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // TODO: Replace later with a command to spawn a generator at the player's location
        /*
        /// <summary>
        /// Debug command to spawn a creature in front of the player and save it as a static spawn if the static option is specified.
        /// </summary>
        [CommandHandler("createcreature", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld,
            "Debug command to spawn a creature in front of the player and save it as a static spawn if the static option is specified.",
            "weenieClassId")]
        public static void CreateStaticCreature(Session session, params string[] parameters)
        {
            Creature newC = null;

            if (!(parameters?.Length > 0))
            {
                ChatPacket.SendServerMessage(session, "Usage: @createcreature [static] weenieClassId",
                   ChatMessageType.Broadcast);
                return;
            }
            if (parameters?[0] == "static")
            {
                if (parameters?.Length > 1)
                {
                    uint weenie = Convert.ToUInt32(parameters[1]);
                    newC = MonsterFactory.SpawnCreature(weenie, true, session.Player.Location.InFrontOf(2.0f));
                }
                else
                {
                    ChatPacket.SendServerMessage(session, "Specify a valid weenieClassId after the static option.",
                        ChatMessageType.Broadcast);
                    return;
                }
            }
            else
            {
                uint weenie = Convert.ToUInt32(parameters[0]);
                newC = MonsterFactory.SpawnCreature(weenie, false, session.Player.Location.InFrontOf(2.0f));
            }

            if (newC != null)
            {
                ChatPacket.SendServerMessage(session, $"Now spawning {newC.Name}.",
                    ChatMessageType.Broadcast);
                LandblockManager.AddObject(newC);
            }
            else
            {
                ChatPacket.SendServerMessage(session, "Couldn't find that creature in the database or save it's location.",
                    ChatMessageType.Broadcast);
            }
        }
        */

        [CommandHandler("nudge", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0, "Correct player position cell ID after teleporting into black space.")]
        public static void HandleNudge(Session session, params string[] parameters)
        {
            var pos = session.Player.GetPosition(PositionType.Location);
            if (WorldObject.AdjustDungeonCells(pos))
            {
                pos.PositionZ += 0.005000f;
                var posReadable = PostionAsLandblocksGoogleSpreadsheetFormat(pos);
                AdminCommands.HandleTeleportLOC(session, posReadable.Split(' '));
                var positionMessage = new GameMessageSystemChat($"Nudge player to {posReadable}", ChatMessageType.Broadcast);
                session.Network.EnqueueSend(positionMessage);
            }
        }

        /// <summary>
        /// Attempts to remove the hourglass / fix the busy state for the player
        /// </summary>
        [CommandHandler("fixbusy", AccessLevel.Player, CommandHandlerFlag.RequiresWorld, 0, "Attempts to remove the hourglass / fix the busy state for the player")]
        public static void HandleFixBusy(Session session, params string[] parameters)
        {
            session.Player.SendUseDoneEvent();
        }


        static string PostionAsLandblocksGoogleSpreadsheetFormat(Position pos)
        {
            return $"0x{pos.Cell.ToString("X")} {pos.Pos.X} {pos.Pos.Y} {pos.Pos.Z} {pos.Rotation.W} {pos.Rotation.X} {pos.Rotation.Y} {pos.Rotation.Z}";
        }

        /// <summary>
        /// Debug command to test the ObjDescEvent message.
        /// </summary>
        [CommandHandler("equiptest", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, "Simulates equipping a new item to your character, replacing all other items.")]
        public static void EquipTest(Session session, params string[] parameters)
        {
            if (!(parameters?.Length > 0))
            {
                ChatPacket.SendServerMessage(session, "Usage: @equiptest (hex)clothingTableId [palette_index] [shade].\neg '@equiptest 0x100005fd'",
                    ChatMessageType.Broadcast);
                return;
            }

            try
            {
                uint modelId;
                if (parameters[0].StartsWith("0x"))
                {
                    string strippedmodelid = parameters[0].Substring(2);
                    modelId = UInt32.Parse(strippedmodelid, NumberStyles.HexNumber);
                }
                else
                    modelId = UInt32.Parse(parameters[0], NumberStyles.HexNumber);

                int palOption = -1;
                if (parameters.Length > 1)
                    palOption = Int32.Parse(parameters[1]);
                float shade = 0;
                if (parameters.Length > 2)
                    shade = Single.Parse(parameters[2]);
                if (shade < 0)
                    shade = 0;
                if (shade > 1)
                    shade = 1;

                //if ((modelId >= 0x10000001) && (modelId <= 0x1000086B))
                //    session.Player.TestWieldItem(session, modelId, palOption, shade);
                //else
                //    ChatPacket.SendServerMessage(session, "Please enter a value greater than 0x10000000 and less than 0x1000086C",
                //        ChatMessageType.Broadcast);
            }
            catch (Exception)
            {
                ChatPacket.SendServerMessage(session, "Please enter a value greater than 0x10000000 and less than 0x1000086C", ChatMessageType.Broadcast);
            }
        }

        ///// <summary>
        ///// Force PhysicsState change that occurs upon login complete.
        ///// </summary>
        //[CommandHandler("fakelogin", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, "Fake Login Complete response")]
        //public static void HandleFakeLogin(Session session, params string[] parameters)
        //{
        //    session.Player.ReportCollisions = true;
        //    session.Player.IgnoreCollisions = false;
        //    session.Player.Hidden = false;
        //    session.Player.EnqueueBroadcastPhysicsState();
        //}

        [CommandHandler("netstats", AccessLevel.Developer, CommandHandlerFlag.None, "View network statistics")]
        public static void HandleNetStats(Session session, params string[] parameters)
        {
            CommandHandlerHelper.WriteOutputInfo(session, NetworkStatistics.Summary(), ChatMessageType.Broadcast);
        }

        /// <summary>
        /// List all clothing bases which are compatible with setup
        /// </summary>
        [CommandHandler("listcb", AccessLevel.Developer, CommandHandlerFlag.ConsoleInvoke, "List Clothing Tables available")]
        public static void HandleShowCompatibleClothingBases(Session session, params string[] parameters)
        {
            uint.TryParse(parameters[0], out var setupId);

            uint cbStart = 0x10000001;
            uint cbEnd = 0x1000086c;

            List<uint> compatibleCBs = new List<uint>();

            for (uint i = cbStart; i < cbEnd; i++)
            {
                var cbToTest = DatManager.PortalDat.ReadFromDat<ClothingTable>(i);

                if (cbToTest.ClothingBaseEffects.ContainsKey(setupId))
                    compatibleCBs.Add(i);
            }

            Console.WriteLine($"There are {compatibleCBs.Count} compatible clothingbase tables for setup {setupId}");
            Console.WriteLine("");
            Console.WriteLine($"{string.Join("\n", compatibleCBs.ToArray())}");
        }


        // ==================================
        // Client Testing
        // ==================================

        /// <summary>
        /// echo "text to send back to yourself" [ChatMessageType]
        /// </summary>
        [CommandHandler("echo", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, "Send text back to yourself.", "\"text to send back to yourself\" [ChatMessageType]\n" + "ChatMessageType can be a uint or enum name")]
        public static void HandleDebugEcho(Session session, params string[] parameters)
        {
            try
            {
                if (Enum.TryParse(parameters[1], true, out ChatMessageType cmt))
                {
                    if (Enum.IsDefined(typeof(ChatMessageType), cmt))
                        ChatPacket.SendServerMessage(session, parameters[0], cmt);
                    else
                        ChatPacket.SendServerMessage(session, parameters[0], ChatMessageType.Broadcast);
                }
            }
            catch (Exception)
            {
                ChatPacket.SendServerMessage(session, parameters[0], ChatMessageType.Broadcast);
            }
        }

        /// <summary>
        /// playsound [Sound] (volumelevel)
        /// </summary>
        [CommandHandler("playsound", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1, "Plays a sound.", "sound (volume) (guid)\n" + "Sound can be uint or enum name\n" + "Volume and source guid are optional")]
        public static void HandlePlaySound(Session session, params string[] parameters)
        {
            try
            {
                float volume = 1f;

                if (parameters.Length > 1)
                    float.TryParse(parameters[1], out volume);

                uint guid = session.Player.Guid.Full;

                if (parameters.Length > 2)
                    uint.TryParse(parameters[2].TrimStart("0x"), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out guid);

                var message = $"Unable to find a sound called {parameters[0]} to play.";

                if (Enum.TryParse(parameters[0], true, out Sound sound))
                {
                    if (Enum.IsDefined(typeof(Sound), sound))
                    {
                        message = $"Playing sound {Enum.GetName(typeof(Sound), sound)}";
                        // add the sound to the player queue for everyone to hear
                        // player action queue items will execute on the landblock
                        // player.playsound will play a sound on only the client session that called the function
                        session.Player.PlaySoundEffect(sound, new ObjectGuid(guid), volume);
                    }
                }

                var sysChatMessage = new GameMessageSystemChat(message, ChatMessageType.Broadcast);
                session.Network.EnqueueSend(sysChatMessage);
            }
            catch (Exception)
            {
                // Do Nothing
            }
        }

        /// <summary>
        /// effect [Effect] (scale)
        /// </summary>
        [CommandHandler("effect", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1, "Plays an effect.", "effect (float)\n" + "Effect can be uint or enum name" + "float is scale level")]
        public static void HandlePlayEffect(Session session, params string[] parameters)
        {
            try
            {
                float scale = 1f;
                var effectEvent = new GameMessageScript(session.Player.Guid, PlayScript.Invalid);

                if (parameters.Length > 1)
                {
                    if (parameters[1] != "")
                        scale = float.Parse(parameters[1]);
                }

                var message = $"Unable to find a effect called {parameters[0]} to play.";

                if (Enum.TryParse(parameters[0], true, out PlayScript effect))
                {
                    if (Enum.IsDefined(typeof(PlayScript), effect))
                    {
                        message = $"Playing effect {Enum.GetName(typeof(PlayScript), effect)}";
                        session.Player.ApplyVisualEffects(effect);
                    }
                }

                var sysChatMessage = new GameMessageSystemChat(message, ChatMessageType.Broadcast);
                session.Network.EnqueueSend(sysChatMessage);
            }
            catch (Exception)
            {
                // Do Nothing
            }
        }

        [CommandHandler("chatdump", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0, "Spews 1000 lines of text to you.")]
        public static void ChatDump(Session session, params string[] parameters)
        {
            for (int i = 0; i < 1000; i++)
                ChatPacket.SendServerMessage(session, "Test Message " + i, ChatMessageType.Broadcast);
        }

        [CommandHandler("animation", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1, "Plays an animation on the current player, or optionally another object", "MotionCommand (optional target guid)\n")]
        public static void Animation(Session session, params string[] parameters)
        {
            if (!Enum.TryParse(parameters[0], out MotionCommand motionCommand))
            {
                ChatPacket.SendServerMessage(session, $"MotionCommand: {parameters[0]} not found", ChatMessageType.Broadcast);
                return;
            }
            WorldObject obj = session.Player;

            if (parameters.Length > 1)
            {
                if (!uint.TryParse(parameters[1].TrimStart("0x"), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var guid))
                {
                    ChatPacket.SendServerMessage(session, $"Invalid guid: {parameters[1]}", ChatMessageType.Broadcast);
                    return;
                }
                obj = session.Player.FindObject(guid, Player.SearchLocations.Everywhere);
                if (obj == null)
                {
                    ChatPacket.SendServerMessage(session, $"Couldn't find guid: {parameters[1]}", ChatMessageType.Broadcast);
                    return;
                }
                if (obj.CurrentMotionState == null)
                {
                    ChatPacket.SendServerMessage(session, $"{obj.Name} ({obj.Guid}) has no CurrentMotionState", ChatMessageType.Broadcast);
                    return;
                }
            }
            var stance = obj.CurrentMotionState.Stance;

            var suffix = "";
            if (obj != session.Player)
                suffix = $" on {obj.Name} ({obj.Guid})";

            ChatPacket.SendServerMessage(session, $"Playing animation {stance}.{motionCommand}{suffix}", ChatMessageType.Broadcast);

            obj.EnqueueBroadcastMotion(new Motion(stance, motionCommand));
        }

        /// <summary>
        /// This function is just used to exercise the ability to have player movement without animation.   Once we are solid on this it can be removed.   Og II
        /// </summary>
        [CommandHandler("movement", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1, "Movement testing command, to be removed soon")]
        public static void Movement(Session session, params string[] parameters)
        {
            var forwardCommand = (MotionCommand)Convert.ToInt16(parameters[0]);

            var movement = new Motion(session.Player, forwardCommand);
            session.Network.EnqueueSend(new GameMessageUpdateMotion(session.Player, movement));

            movement = new Motion(session.Player, MotionCommand.Ready);
            session.Network.EnqueueSend(new GameMessageUpdateMotion(session.Player, movement));
        }

        /// <summary>
        /// This function is just used to exercise the ability to have player movement without animation.   Once we are solid on this it can be removed.   Og II
        /// </summary>
        [CommandHandler("MoveTo", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0, "Used to test the MoveToObject message.   It will spawn a training wand in front of you and then move to that object.", "moveto\n" + "optional parameter distance if omitted 10f")]
        public static void MoveTo(Session session, params string[] parameters)
        {
            var distance = 10.0f;
            ushort trainingWandTarget = 12748;

            if ((parameters?.Length > 0))
                distance = Convert.ToInt16(parameters[0]);

            WorldObject loot = WorldObjectFactory.CreateNewWorldObject(trainingWandTarget);
            loot.Location = session.Player.Location.InFrontOf((loot.UseRadius ?? 2) > 2 ? loot.UseRadius.Value : 2);
            loot.Location.LandblockId = new LandblockId(loot.Location.GetCell());

            loot.EnterWorld();

            session.Player.HandleActionPutItemInContainer(loot.Guid.Full, session.Player.Guid.Full);
        }

        /// <summary>
        /// Debug command to spawn the Barber UI
        /// </summary>
        [CommandHandler("barbershop", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, "Displays the barber ui")]
        public static void BarberShop(Session session, params string[] parameters)
        {
            session.Network.EnqueueSend(new GameEventStartBarber(session));
        }


        // ==================================
        // Server
        // ==================================

        /// <summary>
        /// Debug command to print out all of the active players connected too the server.
        /// </summary>
        [CommandHandler("listplayers", AccessLevel.Developer, CommandHandlerFlag.None, 0, "Displays all of the active players connected too the server.")]
        public static void HandleListPlayers(Session session, params string[] parameters)
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

            if (targetAccessLevel.HasValue)
                message += $"Listing only {targetAccessLevel.Value.ToString()}s:\n";

            foreach (var player in PlayerManager.GetAllOnline())
            {
                if (targetAccessLevel.HasValue && player.Account.AccessLevel != ((uint)targetAccessLevel.Value))
                    continue;
                message += $"{player.Name} : {player.Session.AccountId}\n";
                playerCounter++;
            }

            message += $"Total connected Players: {playerCounter}\n";

            CommandHandlerHelper.WriteOutputInfo(session, message, ChatMessageType.Broadcast);
        }

        /// <summary>
        /// Debug command to saves the character from in-game.
        /// </summary>
        /// <remarks>Added a quick way to invoke the character save routine.</remarks>
        [CommandHandler("save-now", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, "Saves your session.")]
        public static void HandleSaveNow(Session session, params string[] parameters)
        {
            session.Player.SavePlayerToDatabase();
        }

        /// <summary>
        /// This is a VERY crude test. It should never be used on a live server.
        /// There isn't really much point to this command other than making sure landblocks can load and are semi-efficient.
        /// </summary>
        [CommandHandler("loadalllandblocks", AccessLevel.Developer, CommandHandlerFlag.None, "Loads all Landblocks. This is VERY crude. Do NOT use it on a live server!!! It will likely crash the server.  Landblock resources will be loaded async and will continue to do work even after all landblocks have been loaded.")]
        public static void HandleLoadAllLandblocks(Session session, params string[] parameters)
        {
            CommandHandlerHelper.WriteOutputInfo(session, "Loading landblocks. This will likely crash the server. Landblock resources will be loaded async and will continue to do work even after all landblocks have been loaded.");

            Task.Run(() =>
            {
                for (int x = 0; x <= 0xFE; x++)
                {
                    CommandHandlerHelper.WriteOutputInfo(session, $"Loading landblocks, x = 0x{x:X2} of 0xFE....");

                    for (int y = 0; y <= 0xFE; y++)
                    {
                        var blockid = new LandblockId((byte)x, (byte)y);
                        LandblockManager.GetLandblock(blockid, false, false);
                    }
                }

                CommandHandlerHelper.WriteOutputInfo(session, "Loading landblocks completed. Async landblock resources are likely still loading...");
            });
        }


        // ==================================
        // World Object Properties
        // ==================================

        [CommandHandler("propertydump", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, "Lists all properties for the last world object you examined.")]
        public static void HandlePropertyDump(Session session, params string[] parameters)
        {
            var target = CommandHandlerHelper.GetLastAppraisedObject(session);

            if (target != null)
                session.Network.EnqueueSend(new GameMessageSystemChat($"\n{target.DebugOutputString(target)}", ChatMessageType.System));
        }


        // ==================================
        // Player Properties
        // ==================================

        /// <summary>
        /// Returns the Player's GUID
        /// </summary>
        /// <remarks>Added a quick way to access the player GUID.</remarks>
        [CommandHandler("whoami", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, "Shows you your GUIDs.")]
        public static void HandleWhoAmI(Session session, params string[] parameters)
        {
            ChatPacket.SendServerMessage(session, $"GUID: {session.Player.Guid.Full} (0x{session.Player.Guid}) | ID(low): {session.Player.Guid.Low} High:{session.Player.Guid.High}", ChatMessageType.Broadcast);
        }

        /// <summary>
        /// echoflags [flagtype] [int]
        /// </summary>
        [CommandHandler("echoflags", AccessLevel.Developer, CommandHandlerFlag.None, 2, "Echo flags back to you", "[type to test] [int]\n")]
        public static void HandleDebugEchoFlags(Session session, params string[] parameters)
        {
            try
            {
                if (parameters?.Length == 2)
                {
                    string debugOutput;

                    switch (parameters[0].ToLower())
                    {
                        case "descriptionflags":
                            var objectDescFlag = (ObjectDescriptionFlag)Convert.ToUInt32(parameters[1]);

                            debugOutput = $"{objectDescFlag.GetType().Name} = {objectDescFlag.ToString()}" + " (" + (uint)objectDescFlag + ")";
                            break;
                        case "weenieflags":
                            var weenieHdr = (WeenieHeaderFlag)Convert.ToUInt32(parameters[1]);

                            debugOutput = $"{weenieHdr.GetType().Name} = {weenieHdr.ToString()}" + " (" + (uint)weenieHdr + ")";
                            break;
                        case "weenieflags2":
                            var weenieHdr2 = (WeenieHeaderFlag2)Convert.ToUInt32(parameters[1]);

                            debugOutput = $"{weenieHdr2.GetType().Name} = {weenieHdr2.ToString()}" + " (" + (uint)weenieHdr2 + ")";
                            break;
                        case "positionflag":
                            var posFlag = (PositionFlags)Convert.ToUInt32(parameters[1]);

                            debugOutput = $"{posFlag.GetType().Name} = {posFlag.ToString()}" + " (" + (uint)posFlag + ")";
                            break;
                        case "type":
                            var objectType = (ItemType)Convert.ToUInt32(parameters[1]);

                            debugOutput = $"{objectType.GetType().Name} = {objectType.ToString()}" + " (" + (uint)objectType + ")";
                            break;
                        case "containertype":
                            var contType = (ContainerType)Convert.ToUInt32(parameters[1]);

                            debugOutput = $"{contType.GetType().Name} = {contType.ToString()}" + " (" + (uint)contType + ")";
                            break;
                        case "usable":
                            var usableType = (Usable)Convert.ToInt64(parameters[1]);

                            debugOutput = $"{usableType.GetType().Name} = {usableType.ToString()}" + " (" + (Int64)usableType + ")";
                            break;
                        case "radarbehavior":
                            var radarBeh = (RadarBehavior)Convert.ToUInt32(parameters[1]);

                            debugOutput = $"{radarBeh.GetType().Name} = {radarBeh.ToString()}" + " (" + (uint)radarBeh + ")";
                            break;
                        case "physicsdescriptionflags":
                            var physicsDescFlag = (PhysicsDescriptionFlag)Convert.ToUInt32(parameters[1]);

                            debugOutput = $"{physicsDescFlag.GetType().Name} = {physicsDescFlag.ToString()}" + " (" + (uint)physicsDescFlag + ")";
                            break;
                        case "physicsstate":
                            var physState = (PhysicsState)Convert.ToUInt32(parameters[1]);

                            debugOutput = $"{physState.GetType().Name} = {physState.ToString()}" + " (" + (uint)physState + ")";
                            break;
                        case "validlocations":
                            var locFlags = (EquipMask)Convert.ToUInt32(parameters[1]);

                            debugOutput = $"{locFlags.GetType().Name} = {locFlags.ToString()}" + " (" + (uint)locFlags + ")";
                            break;
                        case "currentwieldedlocation":
                            var locFlags2 = (EquipMask)Convert.ToUInt32(parameters[1]);

                            debugOutput = $"{locFlags2.GetType().Name} = {locFlags2.ToString()}" + " (" + (uint)locFlags2 + ")";
                            break;
                        case "priority":
                            var covMask = (CoverageMask)Convert.ToUInt32(parameters[1]);

                            debugOutput = $"{covMask.GetType().Name} = {covMask.ToString()}" + " (" + (uint)covMask + ")";
                            break;
                        case "radarcolor":
                            var radarBlipColor = (RadarColor)Convert.ToUInt32(parameters[1]);

                            debugOutput = $"{radarBlipColor.GetType().Name} = {radarBlipColor.ToString()}" + " (" + (uint)radarBlipColor + ")";
                            break;
                        default:
                            debugOutput = "No valid type to test";
                            break;
                    }

                    CommandHandlerHelper.WriteOutputInfo(session, debugOutput);
                }
            }
            catch (Exception)
            {
                string debugOutput = "Exception Error, check input and try again";

                CommandHandlerHelper.WriteOutputInfo(session, debugOutput);
            }
        }

        [CommandHandler("setcoin", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1, "Set Coin display debug only usage")]
        public static void HandleSetCoin(Session session, params string[] parameters)
        {
            int coins;

            try
            {
                coins = Convert.ToInt32(parameters[0]);
            }
            catch (Exception)
            {
                ChatPacket.SendServerMessage(session, "Not a valid number - must be a number between 0 - 2,147,483,647", ChatMessageType.Broadcast);
                return;
            }

            session.Player.CoinValue = coins;
            session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(session.Player, PropertyInt.CoinValue, coins));
        }


        // ==================================
        // Teleport + Positions/Locations
        // ==================================

        /// <summary>
        /// telexyz cell x y z qx qy qz qw
        /// </summary>
        [CommandHandler("telexyz", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 8, "Teleport to a location.", "cell x y z qx qy qz qw\n" + "all parameters must be specified and cell must be in decimal form")]
        public static void HandleDebugTeleportXYZ(Session session, params string[] parameters)
        {
            if (!uint.TryParse(parameters[0], out var cell))
                return;

            var positionData = new float[7];

            for (uint i = 0u; i < 7u; i++)
            {
                if (!float.TryParse(parameters[i + 1], out var position))
                    return;

                positionData[i] = position;
            }

            session.Player.Teleport(new Position(cell, positionData[0], positionData[1], positionData[2], positionData[3], positionData[4], positionData[5], positionData[6]));
        }

        /// <summary>
        /// Debug command to teleport a player to a saved position, if the position type exists within the database.
        /// </summary>
        [CommandHandler("teletype", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1, "Teleport to a saved character position.", "uint 0-22\n" + "@teletype 1")]
        public static void HandleTeleType(Session session, params string[] parameters)
        {
            if (parameters?.Length > 0)
            {
                string parsePositionString = parameters[0].Length > 3 ? parameters[0].Substring(0, 3) : parameters[0];

                if (Enum.TryParse(parsePositionString, true, out PositionType positionType))
                {
                    if (session.Player.TeleToPosition(positionType))
                        session.Network.EnqueueSend(new GameMessageSystemChat($"{PositionType.Location} {session.Player.Location}", ChatMessageType.Broadcast));
                    else
                        session.Network.EnqueueSend(new GameMessageSystemChat($"Error finding saved character position: {positionType}", ChatMessageType.Broadcast));
                }
            }
        }

        /// <summary>
        /// Debug command to print out all of the saved character positions.
        /// </summary>
        [CommandHandler("listpositions", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0, "Displays all available saved character positions from the database.")]
        public static void HandleListPositions(Session session, params string[] parameters)
        {
            var posDict = session.Player.GetAllPositions();
            string message = "Saved character positions:\n";

            foreach (var posPair in posDict)
                message += "ID: " + (uint)posPair.Key + " Loc: " + posPair.Value + "\n";

            message += "Total positions: " + posDict.Count + "\n";
            var positionMessage = new GameMessageSystemChat(message, ChatMessageType.Broadcast);
            session.Network.EnqueueSend(positionMessage);
        }

        /// <summary>
        /// Debug command to save the player's current location as specific position type.
        /// </summary>
        [CommandHandler("setposition", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1, "Saves the supplied character position type to the database.", "uint 1-27\n" + "@setposition 1")]
        public static void HandleSetPosition(Session session, params string[] parameters)
        {
            if (parameters?.Length == 1)
            {
                string parsePositionString = parameters[0].Length > 19 ? parameters[0].Substring(0, 19) : parameters[0];

                // The enum labels max character length has been observered as length 19
                // int value can be: 0-27

                if (Enum.TryParse(parsePositionString, true, out PositionType positionType))
                {
                    if (positionType != PositionType.Undef)
                    {
                        // Create a new position from the current player location
                        var playerPosition = new Position(session.Player.Location);

                        // Save the position
                        session.Player.SetPosition(positionType, playerPosition);

                        // Report changes to client
                        var positionMessage = new GameMessageSystemChat($"Set: {positionType} to Loc: {playerPosition}", ChatMessageType.Broadcast);
                        session.Network.EnqueueSend(positionMessage);
                        return;
                    }
                }
            }

            session.Network.EnqueueSend(new GameMessageSystemChat("Could not determine the correct position type.\nPlease supply a single integer value from within the range of 1 through 27.", ChatMessageType.Broadcast));
        }

        [CommandHandler("gps", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, "Display location.")]
        public static void HandleDebugGPS(Session session, params string[] parameters)
        {
            var position = session.Player.Location;
            ChatPacket.SendServerMessage(session, $"Position: [Cell: 0x{position.LandblockId.Landblock:X4} | Offset: {position.PositionX}, {position.PositionY}, {position.PositionZ} | Facing: {position.RotationX}, {position.RotationY}, {position.RotationZ}, {position.RotationW}]", ChatMessageType.Broadcast);
        }


        // ==================================
        // Titles
        // ==================================

        /// <summary>
        /// Add a specific title to yourself
        /// </summary>
        [CommandHandler("addtitle", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1, "Add title to yourself", "[titleid]")]
        public static void HandleAddTitle(Session session, params string[] parameters)
        {
            if (uint.TryParse(parameters[0], out var titleId))
                session.Player.AddTitle(titleId);
        }

        /// <summary>
        /// Add all titles to yourself
        /// </summary>
        [CommandHandler("addalltitles", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, "Add all titles to yourself")]
        public static void HandleAddAllTitles(Session session, params string[] parameters)
        {
            foreach (CharacterTitle title in Enum.GetValues(typeof(CharacterTitle)))
                session.Player.AddTitle((uint)title);
        }


        // ==================================
        // Experience
        // ==================================

        [CommandHandler("grantxp", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1, "Give XP to yourself (or the specified character).", "ulong\n" + "@grantxp [name] 191226310247 is max level 275")]
        public static void HandleGrantXp(Session session, params string[] parameters)
        {
            if (parameters?.Length > 0)
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
                        Type = CommandParameterHelpers.ACECommandParameterType.PositiveLong,
                        Required = true,
                        ErrorMessage = "You must specify the amount of xp."
                    }
                };
                if (CommandParameterHelpers.ResolveACEParameters(session, parameters, aceParams))
                {
                    try
                    {
                        var amount = aceParams[1].AsLong;
                        aceParams[0].AsPlayer.GrantXP(amount, XpType.Admin, ShareType.None);

                        session.Network.EnqueueSend(new GameMessageSystemChat($"{amount:N0} experience granted.", ChatMessageType.Advancement));

                        PlayerManager.BroadcastToAuditChannel(session.Player, $"{session.Player.Name} granted {amount:N0} experience to {aceParams[0].AsPlayer.Name}.");

                        return;
                    }
                    catch
                    {
                        //overflow
                    }
                }
            }

            ChatPacket.SendServerMessage(session, "Usage: /grantxp [name] 1234 (max 999999999999)", ChatMessageType.Broadcast);
        }

        [CommandHandler("grantluminance", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1, "Give luminance to yourself (or the specified character).", "ulong\n" + "@grantluminance [name] 1500000 is max luminance")]
        public static void HandleGrantLuminance(Session session, params string[] parameters)
        {
            if (parameters?.Length > 0)
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
                        Type = CommandParameterHelpers.ACECommandParameterType.PositiveLong,
                        Required = true,
                        ErrorMessage = "You must specify the amount of luminance."
                    }
                };
                if (CommandParameterHelpers.ResolveACEParameters(session, parameters, aceParams))
                {
                    try
                    {
                        var amount = aceParams[1].AsLong;
                        aceParams[0].AsPlayer.GrantLuminance(amount, XpType.Admin, ShareType.None);

                        session.Network.EnqueueSend(new GameMessageSystemChat($"{amount:N0} luminance granted.", ChatMessageType.Advancement));

                        PlayerManager.BroadcastToAuditChannel(session.Player, $"{session.Player.Name} granted {amount:N0} luminance to {aceParams[0].AsPlayer.Name}.");

                        return;
                    }
                    catch
                    {
                        //overflow
                    }
                }
            }

            ChatPacket.SendServerMessage(session, "Usage: /grantluminance [name] 1234 (max 999999999999)", ChatMessageType.Broadcast);
        }

        [CommandHandler("grantitemxp", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1, "Give item XP to the last appraised item.")]
        public static void HandleGrantItemXp(Session session, params string[] parameters)
        {
            if (!long.TryParse(parameters[0], out var amount))
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Invalid amount {parameters[0]}", ChatMessageType.Broadcast));
                return;
            }

            var item = CommandHandlerHelper.GetLastAppraisedObject(session);
            if (item == null) return;

            if (item is Player player)
            {
                player.GrantItemXP(amount);

                foreach (var i in player.EquippedObjects.Values.Where(i => i.HasItemLevel))
                    session.Network.EnqueueSend(new GameMessageSystemChat($"{amount:N0} experience granted to {i.Name}.", ChatMessageType.Broadcast));
            }
            else
            {
                if (item.HasItemLevel)
                {
                    session.Player.GrantItemXP(item, amount);

                    session.Network.EnqueueSend(new GameMessageSystemChat($"{amount:N0} experience granted to {item.Name}.", ChatMessageType.Broadcast));
                }
                else
                    session.Network.EnqueueSend(new GameMessageSystemChat($"{item.Name} is not a levelable item.", ChatMessageType.Broadcast));
            }
        }

        [CommandHandler("spendallxp", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0, "Spend all available XP on Attributes, Vitals and Skills.")]
        public static void HandleSpendAllXp(Session session, params string[] parameters)
        {
            session.Player.SpendAllXp();

            ChatPacket.SendServerMessage(session, "All available xp has been spent. You must now log out for the updated values to take effect.", ChatMessageType.Broadcast);
        }


        // ==================================
        // Vitals
        // ==================================

        [CommandHandler("setvital", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 2,
            "Sets the specified vital to a specified value",
            "Usage: @setvital <vital> <value>\n" +
            "<vital> is one of the following strings:\n" +
            "    health, hp\n" +
            "    stamina, stam, sp\n" +
            "    mana, mp\n" +
            "<value> is an integral value [0-9]+, or a relative value [-+][0-9]+")]
        public static void SetVital(Session session, params string[] parameters)
        {
            string paramVital = parameters[0].ToLower();
            string paramValue = parameters[1];

            bool relValue = paramValue[0] == '+' || paramValue[0] == '-';

            if (!int.TryParse(paramValue, out var value))
            {
                ChatPacket.SendServerMessage(session, "setvital Error: Invalid set value", ChatMessageType.Broadcast);
                return;
            }

            // Parse args...
            CreatureVital vital;

            if (paramVital == "health" || paramVital == "hp")
                vital = session.Player.Health;
            else if (paramVital == "stamina" || paramVital == "stam" || paramVital == "sp")
                vital = session.Player.Stamina;
            else if (paramVital == "mana" || paramVital == "mp")
                vital = session.Player.Mana;
            else
            {
                ChatPacket.SendServerMessage(session, "setvital Error: Invalid vital", ChatMessageType.Broadcast);
                return;
            }

            int delta = 0;

            if (!relValue)
                delta = session.Player.UpdateVital(vital, (uint)value);
            else
                delta = session.Player.UpdateVitalDelta(vital, value);

            if (vital == session.Player.Health)
            {
                if (delta > 0)
                    session.Player.DamageHistory.OnHeal((uint)delta);
                else
                    session.Player.DamageHistory.Add(session.Player, DamageType.Health, (uint)-delta);
            }
        }

        [CommandHandler("sethealth", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1, "sets your current health to a specific value.", "ushort")]
        public static void HandleSetHealth(Session session, params string[] parameters)
        {
            if (parameters?.Length > 0)
            {
                if (ushort.TryParse(parameters[0], out var health))
                {
                    session.Player.Health.Current = health;
                    var updatePlayersHealth = new GameMessagePrivateUpdateAttribute2ndLevel(session.Player, Vital.Health, session.Player.Health.Current);
                    var message = new GameMessageSystemChat($"Attempting to set health to {health}...", ChatMessageType.Broadcast);
                    session.Network.EnqueueSend(updatePlayersHealth, message);
                    return;
                }
            }

            ChatPacket.SendServerMessage(session, "Usage: /sethealth 200 (max Max Health)", ChatMessageType.Broadcast);
        }

        /// <summary>
        /// Debug command to set player vitals to 1
        /// </summary>
        [CommandHandler("harmself", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, "Sets all player vitals to 1")]
        public static void HarmSelf(Session session, params string[] parameters)
        {
            session.Player.UpdateVital(session.Player.Health, 1);
            session.Player.UpdateVital(session.Player.Stamina, 1);
            session.Player.UpdateVital(session.Player.Mana, 1);
        }


        // ==================================
        // Create Objects in Player Inventory
        // ==================================

        private static void AddWeeniesToInventory(Session session, HashSet<uint> weenieIds, ushort? stackSize = null)
        {
            foreach (uint weenieId in weenieIds)
            {
                var loot = WorldObjectFactory.CreateNewWorldObject(weenieId);

                if (loot == null) // weenie doesn't exist
                    continue;

                var stackSizeForThisWeenieId = stackSize ?? loot.MaxStackSize;

                if (stackSizeForThisWeenieId > 1)
                    loot.SetStackSize(stackSizeForThisWeenieId);

                session.Player.TryCreateInInventoryWithNetworking(loot);
            }
        }

        [CommandHandler("weapons", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0, "Creates testing items in your inventory.")]
        public static void HandleWeapons(Session session, params string[] parameters)
        {
            HashSet<uint> weenieIds = new HashSet<uint> { 93, 148, 300, 307, 311, 326, 338, 348, 350, 7765, 12748, 12463, 31812 };

            AddWeeniesToInventory(session, weenieIds);
        }

        [CommandHandler("inv", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0, "Creates sample items, foci and containers in your inventory.")]
        public static void HandleInv(Session session, params string[] parameters)
        {
            HashSet<uint> weenieIds = new HashSet<uint> { 44, 45, 46, 136, 5893, 15268, 15269, 15270, 15271, 12748 };

            AddWeeniesToInventory(session, weenieIds);
        }

        [CommandHandler("splits", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0, "Creates some stackable items in your inventory for testing.")]
        public static void HandleSplits(Session session, params string[] parameters)
        {
            HashSet<uint> weenieIds = new HashSet<uint> { 300, 690, 20630, 20631, 31198, 37155 };

            AddWeeniesToInventory(session, weenieIds);
        }

        [CommandHandler("comps", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0, "Creates spell component items in your inventory for testing.")]
        public static void HandleComps(Session session, params string[] parameters)
        {
            HashSet<uint> weenieIds = new HashSet<uint> { 686, 687, 688, 689, 690, 691, 740, 741, 742, 743, 744, 745, 746, 747, 748, 749, 750, 751, 752, 753, 754, 755, 756, 757, 758, 759, 760, 761, 762, 763, 764, 765, 766, 767, 768, 769, 770, 771, 772, 773, 774, 775, 776, 777, 778, 779, 780, 781, 782, 783, 784, 785, 786, 787, 788, 789, 790, 791, 792, 1643, 1644, 1645, 1646, 1647, 1648, 1649, 1650, 1651, 1652, 1653, 1654, 7299, 7581, 8897, 20631 };

            AddWeeniesToInventory(session, weenieIds, 1);
        }

        [CommandHandler("food", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0, "Creates some food items in your inventory for testing.")]
        public static void HandleFood(Session session, params string[] parameters)
        {
            HashSet<uint> weenieIds = new HashSet<uint> { 259, 259, 260, 377, 378, 379 };

            AddWeeniesToInventory(session, weenieIds);
        }

        [CommandHandler("currency", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0, "Creates some currency items in your inventory for testing.")]
        public static void HandleCurrency(Session session, params string[] parameters)
        {
            HashSet<uint> weenieIds = new HashSet<uint> { 273, 20630 };

            AddWeeniesToInventory(session, weenieIds);
        }

        [CommandHandler("cirand", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1, "Creates random objects in your inventory.", "type (string or number) <num to create> defaults to 10 if omitted, max 50")]
        public static void HandleCIRandom(Session session, params string[] parameters)
        {
            if (!Enum.TryParse(parameters[0], true, out WeenieType weenieType) || !Enum.IsDefined(typeof(WeenieType), weenieType))
            {
                ChatPacket.SendServerMessage(session, $"{parameters[0]} is not a valid WeenieType", ChatMessageType.Broadcast);
                return;
            }

            if (!AdminCommands.VerifyCreateWeenieType(weenieType))
            {
                ChatPacket.SendServerMessage(session, $"{weenieType} is not a valid WeenieType for create commands", ChatMessageType.Broadcast);
                return;
            }

            var numItems = 10;

            if (parameters.Length > 1)
            {
                if (!int.TryParse(parameters[1], out numItems) || numItems < 1 || numItems > 50)
                {
                    ChatPacket.SendServerMessage(session, $"<num to create> must be a number between 1 - 50", ChatMessageType.Broadcast);
                    return;
                }
            }

            var items = LootGenerationFactory.CreateRandomObjectsOfType(weenieType, numItems);

            var stuck = new List<WorldObject>();

            foreach (var item in items)
            {
                if (!item.Stuck)
                    session.Player.TryCreateInInventoryWithNetworking(item);
                else
                    stuck.Add(item);    
            }

            if (stuck.Count != 0)
                session.Network.EnqueueSend(new GameMessageSystemChat($"You cannot spawn {string.Join(", ", stuck.Select(i => i.WeenieClassName))} in your inventory because it cannot be picked up", ChatMessageType.Broadcast));
        }


        // ==================================
        // Spells
        // ==================================

        // addallspells
        [CommandHandler("addallspells", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0, "Adds all known spells to your own spellbook.")]
        public static void HandleAddAllSpells(Session session, params string[] parameters)
        {
            for (uint spellLevel = 1; spellLevel <= 8; spellLevel++)
            {
                session.Player.LearnSpellsInBulk(MagicSchool.CreatureEnchantment, spellLevel);
                session.Player.LearnSpellsInBulk(MagicSchool.ItemEnchantment, spellLevel);
                session.Player.LearnSpellsInBulk(MagicSchool.LifeMagic, spellLevel);
                session.Player.LearnSpellsInBulk(MagicSchool.VoidMagic, spellLevel);
                session.Player.LearnSpellsInBulk(MagicSchool.WarMagic, spellLevel);
            }
        }

        /// <summary>
        /// Debug console command to test the GetSpellFormula function.
        /// </summary>
        [CommandHandler("getspellformula", AccessLevel.Developer, CommandHandlerFlag.ConsoleInvoke, 0, "Tests spell formula calculation")]
        public static void GetSpellFormula(Session session, params string[] parameters)
        {
            if (parameters?.Length != 2)
            {
                Console.WriteLine("getspellformula <accountname> <spellid>");
                return;
            }

            if (!uint.TryParse(parameters[1], out var spellid))
            {
                Console.WriteLine("getspellformula <accountname> <spellid>");
                return;
            }

            SpellTable spellTable = DatManager.PortalDat.SpellTable;
            SpellComponentsTable comps = DatManager.PortalDat.SpellComponentsTable;

            Console.WriteLine("Formula for " + spellTable.Spells[spellid].Name);
            Console.WriteLine("Spell Words: " + spellTable.Spells[spellid].GetSpellWords(DatManager.PortalDat.SpellComponentsTable));
            Console.WriteLine(spellTable.Spells[spellid].Desc);

            var formula = SpellTable.GetSpellFormula(DatManager.PortalDat.SpellTable, spellid, parameters[0]);

            for (int i = 0; i < formula.Count; i++)
            {
                if (comps.SpellComponents.ContainsKey(formula[i]))
                    Console.WriteLine("Comp " + i + ": " + comps.SpellComponents[formula[i]].Name);
                else
                    Console.WriteLine("Comp " + i + " : Unknown Component " + formula[i]);
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Debug console command to test the GetSpellFormula function.
        /// </summary>
        [CommandHandler("getallspellformula", AccessLevel.Developer, CommandHandlerFlag.ConsoleInvoke, 0, "Tests spell formula calculation")]
        public static void GetAllSpellFormula(Session session, params string[] parameters)
        {
            if (parameters?.Length != 1)
            {
                Console.WriteLine("getallspellformula <accountname>");
                return;
            }

            SpellTable spellTable = DatManager.PortalDat.SpellTable;
            SpellComponentsTable comps = DatManager.PortalDat.SpellComponentsTable;

            foreach (KeyValuePair<uint, DatLoader.Entity.SpellBase> entry in spellTable.Spells)
            {
                uint spellid = entry.Key;
                Console.WriteLine("Formula for " + spellTable.Spells[spellid].Name + " (" + spellid + ")");

                var formula = SpellTable.GetSpellFormula(DatManager.PortalDat.SpellTable, spellid, parameters[0]);

                for (int i = 0; i < formula.Count; i++)
                    Console.WriteLine("Comp " + i + ": " + comps.SpellComponents[formula[i]].Name);

                Console.WriteLine();
            }
        }

        /// <summary>
        /// Debug console command for testing reading the client_portal.dat
        /// </summary>
        [CommandHandler("readdat", AccessLevel.Developer, CommandHandlerFlag.ConsoleInvoke, 0, "Tests reading the client_portal.dat")]
        public static void ReadDat(Session session, params string[] parameters)
        {
            //int total = 0;
            //uint min = 0x0E010000;
            //uint max = 0x0E01FFFF;

            var test = DatManager.PortalDat.SkillTable;
            return;
            /*
            foreach (KeyValuePair<uint, DatFile> entry in DatManager.PortalDat.AllFiles)
            {
                if (entry.Value.ObjectId >= min && entry.Value.ObjectId <= max)
                {
                    // Console.WriteLine("Reading " + entry.Value.ObjectId.ToString("X8"));
                    QualityFilter item = DatManager.PortalDat.ReadFromDat<QualityFilter>(entry.Value.ObjectId);
                    total++;
                }
            }
            if (DatManager.HighResDat != null)
            {
                foreach (KeyValuePair<uint, DatFile> entry in DatManager.HighResDat.AllFiles)
                {
                    if (entry.Value.ObjectId >= min && entry.Value.ObjectId <= max)
                    {
                        // Console.WriteLine("Reading " + entry.Value.ObjectId.ToString("X8"));
                        QualityFilter item = DatManager.PortalDat.ReadFromDat<QualityFilter>(entry.Value.ObjectId);
                        total++;
                    }
                }
            }
            if(DatManager.LanguageDat != null)
            {
                foreach (KeyValuePair<uint, DatFile> entry in DatManager.LanguageDat.AllFiles)
                {
                    if (entry.Value.ObjectId >= min && entry.Value.ObjectId <= max)
                    {
                        // Console.WriteLine("Reading " + entry.Value.ObjectId.ToString("X8"));
                        QualityFilter item = DatManager.PortalDat.ReadFromDat<QualityFilter>(entry.Value.ObjectId);
                        total++;
                    }
                }
            }

            Console.WriteLine(total.ToString() + " files read.");
            */
        }

        // ==================================
        // Quests/Contracts
        // ==================================

        [CommandHandler("contract", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1,
            "Query, stamp, and erase contracts on the targeted player",
            "[list | bestow | erase]\n"
            + "contract list - List the contracts for the targeted player\n"
            + "contract bestow - Stamps the specific contract on the targeted player. If this fails, it's probably because the contract is invalid.\n"
            + "contract erase - Erase the specific contract from the targeted player. If no quest flag is given, it erases the entire contract table for the targeted player.\n")]
        public static void HandleContract(Session session, params string[] parameters)
        {
            if (parameters.Length == 0)
            {
                // todo: display help screen
                return;
            }

            var objectId = new ObjectGuid();

            if (session.Player.HealthQueryTarget.HasValue)
                objectId = new ObjectGuid((uint)session.Player.HealthQueryTarget);
            else if (session.Player.ManaQueryTarget.HasValue)
                objectId = new ObjectGuid((uint)session.Player.ManaQueryTarget);
            else if (session.Player.CurrentAppraisalTarget.HasValue)
                objectId = new ObjectGuid((uint)session.Player.CurrentAppraisalTarget);

            var wo = session.Player.CurrentLandblock?.GetObject(objectId);

            if (wo != null && wo is Player player)
            {
                if (parameters[0].Equals("list"))
                {
                    var contractsHdr = $"Contract Registry for {player.Name} (0x{player.Guid}):\n";
                    contractsHdr += "================================================\n";
                    contractsHdr += $"Contracts.Count: {player.Character.GetContractsCount(player.CharacterDatabaseLock)}\n";
                    contractsHdr += "================================================\n";
                    var contracts = "";
                    foreach (var contract in player.ContractManager.ContractTrackerTable)
                    {
                        var contractTracker = contract.Value;
                        contracts += $"Contract Id: {contractTracker.Contract.ContractId} | Contract Name: {contractTracker.Contract.ContractName}\nStage: {contractTracker.Stage.ToString()}\n";

                        if (contractTracker.Stage == Network.Structure.ContractStage.InProgress)
                        {
                            var timeWhenDone = new TimeSpan(0, 0, (int)contractTracker.TimeWhenDone);

                            if (timeWhenDone == TimeSpan.MinValue || timeWhenDone.TotalSeconds == 0)
                                contracts += $"TimeWhenDone: Expired ({contractTracker.TimeWhenDone})\n";
                            else if (timeWhenDone == TimeSpan.MaxValue)
                                contracts += $"TimeWhenDone: Unlimited ({contractTracker.TimeWhenDone})\n";
                            else
                                contracts += $"TimeWhenDone: In {timeWhenDone:%d} days, {timeWhenDone:%h} hours, {timeWhenDone:%m} minutes and, {timeWhenDone:%s} seconds. ({(DateTime.UtcNow + timeWhenDone).ToLocalTime()})\n";
                        }

                        if (contractTracker.Stage == Network.Structure.ContractStage.DoneOrPendingRepeat)
                        {

                            var timeWhenRepeats = new TimeSpan(0, 0, (int)contractTracker.TimeWhenRepeats);

                            if (timeWhenRepeats == TimeSpan.MinValue || timeWhenRepeats.TotalSeconds == 0)
                                contracts += $"TimeWhenRepeats: Available ({contractTracker.TimeWhenDone})\n";
                            else if (timeWhenRepeats == TimeSpan.MaxValue)
                                contracts += $"TimeWhenRepeats: Unlimited ({contractTracker.TimeWhenDone})\n";
                            else
                                contracts += $"TimeWhenRepeats: In {timeWhenRepeats:%d} days, {timeWhenRepeats:%h} hours, {timeWhenRepeats:%m} minutes and, {timeWhenRepeats:%s} seconds. ({(DateTime.UtcNow + timeWhenRepeats).ToLocalTime()})\n";
                        }

                        contracts += "--====--\n";
                    }

                    session.Player.SendMessage($"{contractsHdr}{(contracts != "" ? contracts : "No contracts found.")}");
                    return;
                }

                if (parameters[0].Equals("bestow"))
                {
                    if (parameters.Length < 2)
                    {
                        // delete all contracts?
                        // seems unsafe, maybe a confirmation?
                        return;
                    }

                    if (!uint.TryParse(parameters[1], out var contractId))
                        return;

                    var datContract = player.ContractManager.GetContractFromDat(contractId);

                    if (datContract == null)
                    {
                        session.Player.SendMessage($"Unable to find contract for id {contractId} in dat file.");
                        return;
                    }

                    if (player.ContractManager.HasContract(contractId))
                    {
                        session.Player.SendMessage($"{player.Name} already has the contract for \"{datContract.ContractName}\" ({contractId})");
                        return;
                    }

                    var hasContract = player.ContractManager.HasContract(contractId);
                    if (!hasContract)
                    {
                        player.ContractManager.Add(contractId);
                        session.Player.SendMessage($"Contract for \"{datContract.ContractName}\" ({contractId}) bestowed on {player.Name}");
                        return;
                    }
                    else
                    {
                        session.Player.SendMessage($"Couldn't bestow {contractId} on {player.Name}");
                        return;
                    }
                }

                if (parameters[0].Equals("erase"))
                {
                    if (parameters.Length < 2)
                    {
                        // delete all contracts?
                        // seems unsafe, maybe a confirmation?
                        session.Player.SendMessage($"You must specify a contract to delete, if you want to delete all contracts use the following command: /contract delete *");
                        return;
                    }

                    if (parameters[1] == "*")
                    {
                        player.ContractManager.EraseAll();
                        session.Player.SendMessage($"All contracts deleted for {player.Name}.");
                        return;
                    }

                    if (!uint.TryParse(parameters[1], out var contractId))
                        return;

                    var datContract = player.ContractManager.GetContractFromDat(contractId);

                    if (datContract == null)
                    {
                        session.Player.SendMessage($"Unable to find contract for id {contractId} in dat file.");
                        return;
                    }

                    if (!player.ContractManager.HasContract(contractId))
                    {
                        session.Player.SendMessage($"{datContract.ContractName} ({contractId}) not found in {player.Name}'s registry.");
                        return;
                    }
                    player.ContractManager.Erase(contractId);
                    session.Player.SendMessage($"{datContract.ContractName} ({contractId}) deleted for {player.Name}.");
                    return;
                }
            }
            else
            {
                if (wo == null)
                    session.Player.SendMessage($"Selected object (0x{objectId}) not found.");
                else
                    session.Player.SendMessage($"Selected object {wo.Name} (0x{objectId}) is not a player.");
            }
        }

        // ==================================
        // Monster movement
        // ==================================

        [CommandHandler("turnto", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0, "Turns the last appraised object to the player", "turnto")]
        public static void HandleRequestTurnTo(Session session, params string[] parameters)
        {
            // get the last appraised object
            var targetID = session.Player.CurrentAppraisalTarget;
            if (targetID == null)
            {
                Console.WriteLine("ERROR: no appraisal target");
                return;
            }
            var targetGuid = new ObjectGuid(targetID.Value);
            var target = session.Player.CurrentLandblock?.GetObject(targetGuid);
            if (target == null)
            {
                Console.WriteLine("Couldn't find " + targetGuid);
                return;
            }
            var creature = target as Creature;
            if (creature == null)
            {
                Console.WriteLine(target.Name + " is not a creature / monster");
                return;
            }
            creature.TurnTo(session.Player, true);
        }

        [CommandHandler("debugmove", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0, "Toggles movement debugging for the last appraised monster", "<on/off>")]
        public static void ToggleMovementDebug(Session session, params string[] parameters)
        {
            // get the last appraised object
            var creature = CommandHandlerHelper.GetLastAppraisedObject(session) as Creature;

            if (creature == null)
                return;

            bool enabled = true;
            if (parameters.Length > 0 && parameters[0].Equals("off"))
                enabled = false;

            creature.DebugMove = enabled;
        }

        [CommandHandler("lostest", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, "Tests for direct visibilty with latest appraised object")]
        public static void HandleVisible(Session session, params string[] parameters)
        {
            // get the last appraised object
            var targetID = session.Player.CurrentAppraisalTarget;
            if (targetID == null)
            {
                Console.WriteLine("ERROR: no appraisal target");
                return;
            }
            var targetGuid = new ObjectGuid(targetID.Value);
            var target = session.Player.CurrentLandblock?.GetObject(targetGuid);
            if (target == null)
            {
                Console.WriteLine("Couldn't find " + targetGuid);
                return;
            }

            var visible = session.Player.IsDirectVisible(target);
            Console.WriteLine("Visible: " + visible);
        }

        [CommandHandler("showstats", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0, "Shows a list of a creature's current attribute/skill levels", "showstats")]
        public static void HandleShowStats(Session session, params string[] parameters)
        {
            // get the last appraised object
            var item = CommandHandlerHelper.GetLastAppraisedObject(session) as Creature;
            if (item == null)
            {
                session.Player.SendMessage("ERROR: You must appraise a creature or player to use this function.");
                return;
            }

            Creature creature = (Creature)item;
            string output = "Strength: " + creature.Strength.Current;
            output += "\nEndurance: " + creature.Endurance.Current;
            output += "\nCoordination: " + creature.Coordination.Current;
            output += "\nQuickness: " + creature.Quickness.Current;
            output += "\nFocus: " + creature.Focus.Current;
            output += "\nSelf: " + creature.Self.Current;

            output += "\n\nHealth: " + creature.Health.Current + "/" + creature.Health.MaxValue;
            output += "\nStamina: " + creature.Stamina.Current + "/" + creature.Stamina.MaxValue;
            output += "\nMana: " + creature.Mana.Current + "/" + creature.Mana.MaxValue;

            var specialized = creature.Skills.Values.Where(s => s.AdvancementClass == SkillAdvancementClass.Specialized).OrderBy(s => s.Skill.ToString());
            var trained = creature.Skills.Values.Where(s => s.AdvancementClass == SkillAdvancementClass.Trained).OrderBy(s => s.Skill.ToString());
            var untrained = creature.Skills.Values.Where(s => s.AdvancementClass == SkillAdvancementClass.Untrained && s.IsUsable).OrderBy(s => s.Skill.ToString());
            var unusable = creature.Skills.Values.Where(s => s.AdvancementClass == SkillAdvancementClass.Untrained && !s.IsUsable).OrderBy(s => s.Skill.ToString());

            if (specialized.Count() > 0)
            {
                output += "\n\n== Specialized ==";
                foreach (var skill in specialized)
                    output += "\n" + skill.Skill + ": " + skill.Current;
            }

            if (trained.Count() > 0)
            {
                output += "\n\n== Trained ==";
                foreach (var skill in trained)
                    output += "\n" + skill.Skill + ": " + skill.Current;
            }

            if (untrained.Count() > 0)
            {
                output += "\n\n== Untrained ==";
                foreach (var skill in untrained)
                    output += "\n" + skill.Skill + ": " + skill.Current;
            }

            if (unusable.Count() > 0)
            {
                output += "\n\n== Unusable ==";
                foreach (var skill in unusable)
                    output += "\n" + skill.Skill + ": " + skill.Current;
            }

            session.Player.SendMessage(output);
        }

        [CommandHandler("givemana", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0, "Gives mana to the last appraised object", "<amount>")]
        public static void HandleGiveMana(Session session, params string[] parameters)
        {
            if (parameters.Length == 0) return;
            var amount = Int32.Parse(parameters[0]);

            var obj = CommandHandlerHelper.GetLastAppraisedObject(session);
            if (obj == null) return;

            amount = Math.Min(amount, (obj.ItemMaxMana ?? 0) - (obj.ItemCurMana ?? 0));
            obj.ItemCurMana += amount;
            session.Network.EnqueueSend(new GameMessageSystemChat($"You give {amount} points of mana to the {obj.Name}.", ChatMessageType.Magic));
        }

        /// <summary>
        /// Returns the distance to the last appraised object
        /// </summary>
        [CommandHandler("dist", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0, "Returns the distance to the last appraised object")]
        public static void HandleDist(Session session, params string[] parameters)
        {
            var obj = CommandHandlerHelper.GetLastAppraisedObject(session);
            if (obj == null || obj.PhysicsObj == null) return;

            var sourcePos = session.Player.Location.ToGlobal();
            var targetPos = obj.Location.ToGlobal();

            var dist = Vector3.Distance(sourcePos, targetPos);
            var dist2d = Vector2.Distance(new Vector2(sourcePos.X, sourcePos.Y), new Vector2(targetPos.X, targetPos.Y));

            var cylDist = session.Player.PhysicsObj.get_distance_to_object(obj.PhysicsObj, true);

            session.Network.EnqueueSend(new GameMessageSystemChat($"Dist: {dist}", ChatMessageType.Broadcast));
            session.Network.EnqueueSend(new GameMessageSystemChat($"2D Dist: {dist2d}", ChatMessageType.Broadcast));

            session.Network.EnqueueSend(new GameMessageSystemChat($"CylDist: {cylDist}", ChatMessageType.Broadcast));
        }

        /// <summary>
        /// Teleport object culling precision test
        /// </summary>
        [CommandHandler("teledist", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1, "Teleports a some distance ahead of the last object spawned", "<distance>")]
        public static void HandleTeleportDist(Session session, params string[] parameters)
        {
            if (parameters.Length < 1)
                return;

            var lastSpawnPos = AdminCommands.LastSpawnPos;

            var distance = float.Parse(parameters[0]);

            var newPos = new Position();
            newPos.LandblockId = new LandblockId(lastSpawnPos.LandblockId.Raw);
            newPos.Pos = lastSpawnPos.Pos;
            newPos.Rotation = session.Player.Location.Rotation;

            var dir = Vector3.Normalize(Vector3.Transform(Vector3.UnitY, newPos.Rotation));
            var offset = dir * distance;

            newPos.SetPosition(newPos.Pos + offset);

            session.Player.Teleport(newPos);

            var globLastSpawnPos = lastSpawnPos.ToGlobal();
            var globNewPos = newPos.ToGlobal();

            var totalDist = Vector3.Distance(globLastSpawnPos, globNewPos);

            var totalDist2d = Vector2.Distance(new Vector2(globLastSpawnPos.X, globLastSpawnPos.Y), new Vector2(globNewPos.X, globNewPos.Y));

            ChatPacket.SendServerMessage(session, $"Teleporting player to {newPos.Cell:X8} @ {newPos.Pos}", ChatMessageType.System);

            ChatPacket.SendServerMessage(session, "2D Distance: " + totalDist2d, ChatMessageType.System);
            ChatPacket.SendServerMessage(session, "3D Distance: " + totalDist, ChatMessageType.System);
        }

        public static WorldObject GetObjectMaintTarget(Session session, params string[] parameters)
        {
            WorldObject target = session.Player;

            if (parameters.Length > 0)
            {
                var targetType = parameters[0];

                if (targetType.Equals("target", StringComparison.OrdinalIgnoreCase))
                    target = CommandHandlerHelper.GetLastAppraisedObject(session);
                else if (uint.TryParse(targetType, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var targetGuid))
                {
                    if (ServerObjectManager.ServerObjects.TryGetValue(targetGuid, out var physicsObj))
                        target = physicsObj.WeenieObj.WorldObject;
                }
            }
            if (target == null)
            {
                var param = parameters.Length > 0 ? $" {parameters[0]}" : "";
                session.Network.EnqueueSend(new GameMessageSystemChat($"Couldn't find target{param}", ChatMessageType.Broadcast));
            }
            return target;
        }

        /// <summary>
        /// Shows the list of objects currently known to an object
        /// </summary>
        [CommandHandler("knownobjs", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0, "Shows the list of objects currently known to an object", "<optional guid, or optional 'target' for last appraisal target>")]
        public static void HandleKnownObjs(Session session, params string[] parameters)
        {
            var target = GetObjectMaintTarget(session, parameters);
            if (target == null)
                return;

            Console.WriteLine($"\nKnown objects to {target.Name}: {target.PhysicsObj.ObjMaint.GetKnownObjectsCount()}");

            foreach (var obj in target.PhysicsObj.ObjMaint.GetKnownObjectsValues())
                Console.WriteLine($"{obj.Name} ({obj.ID:X8})");
        }

        /// <summary>
        /// Shows the list of objects currently visible to an object
        /// </summary>
        [CommandHandler("visibleobjs", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0, "Shows the list of objects currently visible to an object", "<optional guid, or optional 'target' for last appraisal target>")]
        public static void HandleVisibleObjs(Session session, params string[] parameters)
        {
            var target = GetObjectMaintTarget(session, parameters);
            if (target == null)
                return;

            Console.WriteLine($"\nVisible objects to {target.Name}: {target.PhysicsObj.ObjMaint.GetVisibleObjectsCount()}");

            foreach (var obj in target.PhysicsObj.ObjMaint.GetVisibleObjectsValues())
                Console.WriteLine($"{obj.Name} ({obj.ID:X8})");
        }

        /// <summary>
        /// Shows the list of players known to an object
        /// KnownPlayers are used for broadcasting
        /// </summary>
        [CommandHandler("knownplayers", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0, "Shows the list of players known to an object", "<optional guid, or optional 'target' for last appraisal target>")]
        public static void HandleKnownPlayers(Session session, params string[] parameters)
        {
            var target = GetObjectMaintTarget(session, parameters);
            if (target == null)
                return;

            Console.WriteLine($"\nKnown players to {target.Name}: {target.PhysicsObj.ObjMaint.GetKnownPlayersCount()}");

            foreach (var obj in target.PhysicsObj.ObjMaint.GetKnownPlayersValues())
                Console.WriteLine($"{obj.Name} ({obj.ID:X8})");
        }

        /// <summary>
        /// Shows the list of players visible to a player
        /// </summary>
        [CommandHandler("visibleplayers", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0, "Shows the list of players visible to a player", "<optional guid, or optional 'target' for last appraisal target>")]
        public static void HandleVisiblePlayers(Session session, params string[] parameters)
        {
            var target = GetObjectMaintTarget(session, parameters);
            if (target == null)
                return;

            Console.WriteLine($"\nVisible players to {target.Name}: {target.PhysicsObj.ObjMaint.GetVisibleObjectsValuesWhere(o => o.IsPlayer).Count}");

            foreach (var obj in target.PhysicsObj.ObjMaint.GetVisibleObjectsValuesWhere(o => o.IsPlayer))
                Console.WriteLine($"{obj.Name} ({obj.ID:X8})");
        }

        /// <summary>
        /// Shows the list of targets currently visible to a monster
        /// </summary>
        [CommandHandler("visibletargets", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0, "Shows the list of targets currently visible to a monster", "<optional guid, or optional 'target' for last appraisal target>")]
        public static void HandleVisibleTargets(Session session, params string[] parameters)
        {
            var target = GetObjectMaintTarget(session, parameters);
            if (target == null)
                return;

            Console.WriteLine($"\nVisible targets to {target.Name}: {target.PhysicsObj.ObjMaint.GetVisibleTargetsCount()}");

            foreach (var obj in target.PhysicsObj.ObjMaint.GetVisibleTargetsValues())
                Console.WriteLine($"{obj.Name} ({obj.ID:X8})");
        }

        /// <summary>
        /// Shows the list of retaliate targets for a monster
        /// </summary>
        [CommandHandler("retaliatetargets", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0, "Shows the list of retaliate targets for a monster", "<optional guid, or optional 'target' for last appraisal target>")]
        public static void HandleRetaliateTargets(Session session, params string[] parameters)
        {
            var target = GetObjectMaintTarget(session, parameters);
            if (target == null)
                return;

            Console.WriteLine($"\nRetaliate targets to {target.Name}: {target.PhysicsObj.ObjMaint.GetRetaliateTargetsCount()}");

            foreach (var obj in target.PhysicsObj.ObjMaint.GetRetaliateTargetsValues())
                Console.WriteLine($"{obj.Name} ({obj.ID:X8})");
        }

        /// <summary>
        /// Shows the list of previously visible objects queued for destruction for a player
        /// </summary>
        [CommandHandler("destructionqueue", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0, "Shows the list of previously visible objects queued for destruction for a player", "<optional guid, or optional 'target' for last appraisal target>")]
        public static void HandleDestructionQueue(Session session, params string[] parameters)
        {
            var target = GetObjectMaintTarget(session, parameters);
            if (target == null)
                return;

            Console.WriteLine($"\nDestruction queue for {target.Name}: {target.PhysicsObj.ObjMaint.GetDestructionQueueCount()}");

            var currentTime = Physics.Common.PhysicsTimer.CurrentTime;

            foreach (var obj in target.PhysicsObj.ObjMaint.GetDestructionQueueCopy())
                Console.WriteLine($"{obj.Key.Name} ({obj.Key.ID:X8}): {obj.Value - currentTime}");
        }

        /// <summary>
        /// Enables emote debugging for the last appraised object
        /// </summary>
        [CommandHandler("debugemote", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0, "Enables emote debugging for the last appraised object")]
        public static void HandleDebugEmote(Session session, params string[] parameters)
        {
            var obj = CommandHandlerHelper.GetLastAppraisedObject(session);
            if (obj != null)
            {
                Console.WriteLine($"Showing emotes for {obj.Name}");
                obj.EmoteManager.Debug = true;
            }
        }

        /// <summary>
        /// Shows the current player location, from the server perspective
        /// </summary>
        [CommandHandler("myloc", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0, "Shows the current player location, from the server perspective")]
        public static void HandleMyLoc(Session session, params string[] parameters)
        {
            session.Network.EnqueueSend(new GameMessageSystemChat($"CurrentLandblock: {session.Player.CurrentLandblock.Id.Landblock:X4}", ChatMessageType.Broadcast));
            session.Network.EnqueueSend(new GameMessageSystemChat($"Location: {session.Player.Location.ToLOCString()}", ChatMessageType.Broadcast));
            session.Network.EnqueueSend(new GameMessageSystemChat($"Physics : {session.Player.PhysicsObj.Position}", ChatMessageType.Broadcast));
        }

        /// <summary>
        /// Gets a property for the last appraised object
        /// </summary>
        [CommandHandler("getproperty", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1, "Gets a property for the last appraised object", "<property>")]
        public static void HandleGetProperty(Session session, params string[] parameters)
        {
            var obj = CommandHandlerHelper.GetLastAppraisedObject(session);
            if (obj == null) return;

            if (parameters.Length < 1)
                return;

            var prop = parameters[0];

            var props = prop.Split('.');
            if (props.Length != 2)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Unknown {prop}", ChatMessageType.Broadcast));
                return;
            }

            var propType = props[0];
            var propName = props[1];

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

            var value = "";
            if (propType.Equals("PropertyInt", StringComparison.OrdinalIgnoreCase))
                value = Convert.ToString(obj.GetProperty((PropertyInt)result));
            else if (propType.Equals("PropertyInt64", StringComparison.OrdinalIgnoreCase))
                value = Convert.ToString(obj.GetProperty((PropertyInt64)result));
            else if (propType.Equals("PropertyBool", StringComparison.OrdinalIgnoreCase))
                value = Convert.ToString(obj.GetProperty((PropertyBool)result));
            else if (propType.Equals("PropertyFloat", StringComparison.OrdinalIgnoreCase))
                value = Convert.ToString(obj.GetProperty((PropertyFloat)result));
            else if (propType.Equals("PropertyString", StringComparison.OrdinalIgnoreCase))
                value = Convert.ToString(obj.GetProperty((PropertyString)result));
            else if (propType.Equals("PropertyInstanceId", StringComparison.OrdinalIgnoreCase))
                value = Convert.ToString(obj.GetProperty((PropertyInstanceId)result));
            else if (propType.Equals("PropertyDataId", StringComparison.OrdinalIgnoreCase))
                value = Convert.ToString(obj.GetProperty((PropertyDataId)result));

            session.Network.EnqueueSend(new GameMessageSystemChat($"{obj.Name} ({obj.Guid}): {prop} = {value}", ChatMessageType.Broadcast));
        }

        /// <summary>
        /// Sets a property for the last appraised object
        /// </summary>
        [CommandHandler("setproperty", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 2, "Sets a property for the last appraised object", "<property> <value>")]
        public static void HandleSetProperty(Session session, params string[] parameters)
        {
            var obj = CommandHandlerHelper.GetLastAppraisedObject(session);
            if (obj == null) return;

            if (parameters.Length < 2)
                return;

            var prop = parameters[0];
            var value = parameters[1];

            var props = prop.Split('.');
            if (props.Length != 2)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Unknown {prop}", ChatMessageType.Broadcast));
                return;
            }

            var propType = props[0];
            var propName = props[1];

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

        /// <summary>
        /// Sets the house purchase time for this player
        /// </summary>
        [CommandHandler("setpurchasetime", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0, "Sets the house purchase time for this player")]
        public static void HandleSetPurchaseTime(Session session, params string[] parameters)
        {
            var currentTime = DateTime.UtcNow;
            Console.WriteLine($"Current time: {currentTime}");
            // subtract 30 days
            var purchaseTime = currentTime - TimeSpan.FromDays(30);
            // add buffer
            purchaseTime += TimeSpan.FromSeconds(1);
            //purchaseTime += TimeSpan.FromMinutes(2);
            var rentDue = DateTimeOffset.FromUnixTimeSeconds(session.Player.House.GetRentDue((uint)Time.GetUnixTime(purchaseTime))).UtcDateTime;

            var prevPurchaseTime = DateTimeOffset.FromUnixTimeSeconds(session.Player.HousePurchaseTimestamp ?? 0).UtcDateTime;
            var prevRentDue = DateTimeOffset.FromUnixTimeSeconds(session.Player.House.GetRentDue((uint)(session.Player.HousePurchaseTimestamp ?? 0))).UtcDateTime;

            Console.WriteLine($"Previous purchase time: {prevPurchaseTime}");
            Console.WriteLine($"New purchase time: {purchaseTime}");

            Console.WriteLine($"Previous rent time: {prevRentDue}");
            Console.WriteLine($"New rent time: {rentDue}");

            session.Player.HousePurchaseTimestamp = (int)Time.GetUnixTime(purchaseTime);
            session.Player.HouseRentTimestamp = (int)session.Player.House.GetRentDue((uint)Time.GetUnixTime(purchaseTime));

            HouseManager.BuildRentQueue();
        }

        /// <summary>
        /// Toggles the display for player damage info
        /// </summary>
        [CommandHandler("debugdamage", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0, "Toggles the display for player damage info", "<attack|defense|all|on|off>")]
        public static void HandleDebugDamage(Session session, params string[] parameters)
        {
            // get last appraisal creature target
            var targetCreature = CommandHandlerHelper.GetLastAppraisedObject(session) as Creature;
            if (targetCreature == null) return;

            if (parameters.Length == 0)
            {
                // toggle
                if (targetCreature.DebugDamage == Creature.DebugDamageType.None)
                    targetCreature.DebugDamage = Creature.DebugDamageType.All;
                else
                    targetCreature.DebugDamage = Creature.DebugDamageType.None;
            }
            else
            {
                var param = parameters[0].ToLower();
                if (param.Equals("on") || param.Equals("all"))
                    targetCreature.DebugDamage = Creature.DebugDamageType.All;
                else if (param.Equals("off"))
                    targetCreature.DebugDamage = Creature.DebugDamageType.None;
                else if (param.StartsWith("attack"))
                    targetCreature.DebugDamage = Creature.DebugDamageType.Attacker;
                else if (param.StartsWith("defen"))
                    targetCreature.DebugDamage = Creature.DebugDamageType.Defender;
                else
                {
                    session.Network.EnqueueSend(new GameMessageSystemChat($"DebugDamage: - unknown {param} ({targetCreature.Name})", ChatMessageType.Broadcast));
                    return;
                }
            }
            session.Network.EnqueueSend(new GameMessageSystemChat($"DebugDamage: - {targetCreature.DebugDamage} ({targetCreature.Name})", ChatMessageType.Broadcast));
            targetCreature.DebugDamageTarget = session.Player.Guid;
        }

        /// <summary>
        /// Enables the aetheria slots for the player
        /// </summary>
        [CommandHandler("enable-aetheria", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0, "Enables the aetheria slots for the player")]
        public static void HandleEnableAetheria(Session session, params string[] parameters)
        {
            var flags = (int)AetheriaBitfield.All;

            if (parameters.Length > 0)
                int.TryParse(parameters[0], out flags);

            session.Player.UpdateProperty(session.Player, PropertyInt.AetheriaBitfield, flags);
        }

        [CommandHandler("debugchess", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, "Shows the chess move history for a player")]
        public static void HandleDebugChess(Session session, params string[] parameters)
        {
            session.Player.ChessMatch?.DebugMove();
        }

        [CommandHandler("debugboard", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, "Shows the current chess board state")]
        public static void HandleDebugBoard(Session session, params string[] parameters)
        {
            session.Player.ChessMatch?.Logic?.DebugBoard();
        }

        /// <summary>
        /// Teleports directly to a dungeon by name or landblock
        /// </summary>
        [CommandHandler("teledungeon", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1, "Teleport to a dungeon", "<dungeon name or landblock>")]
        public static void HandleTeleDungeon(Session session, params string[] parameters)
        {
            var isBlock = true;
            var param = parameters[0];
            if (parameters.Length > 1)
                isBlock = false;

            var landblock = 0u;
            if (isBlock)
            {
                try
                {
                    landblock = Convert.ToUInt32(param, 16);

                    if (landblock >= 0xFFFF)
                        landblock = landblock >> 16;
                }
                catch (Exception)
                {
                    isBlock = false;
                }
            }

            // teleport to dungeon landblock
            if (isBlock)
                HandleTeleDungeonBlock(session, landblock);

            // teleport to dungeon by name
            else
                HandleTeleDungeonName(session, parameters);
        }

        public static void HandleTeleDungeonBlock(Session session, uint landblock)
        {
            using (var ctx = new WorldDbContext())
            {
                var query = from weenie in ctx.Weenie
                            join wpos in ctx.WeeniePropertiesPosition on weenie.ClassId equals wpos.ObjectId
                            where weenie.Type == (int)WeenieType.Portal && wpos.PositionType == (int)PositionType.Destination
                            select new
                            {
                                Weenie = weenie,
                                Dest = wpos
                            };

                var results = query.ToList();

                var dest = results.Where(i => i.Dest.ObjCellId >> 16 == landblock).Select(i => i.Dest).FirstOrDefault();

                if (dest == null)
                {
                    session.Network.EnqueueSend(new GameMessageSystemChat($"Couldn't find dungeon {landblock:X4}", ChatMessageType.Broadcast));
                    return;
                }

                var pos = new Position(dest.ObjCellId, dest.OriginX, dest.OriginY, dest.OriginZ, dest.AnglesX, dest.AnglesY, dest.AnglesZ, dest.AnglesW);
                WorldObject.AdjustDungeon(pos);

                session.Player.Teleport(pos);
            }
        }

        public static void HandleTeleDungeonName(Session session, params string[] parameters)
        {
            var searchName = string.Join(" ", parameters);

            using (var ctx = new WorldDbContext())
            {
                var query = from weenie in ctx.Weenie
                            join wstr in ctx.WeeniePropertiesString on weenie.ClassId equals wstr.ObjectId
                            join wpos in ctx.WeeniePropertiesPosition on weenie.ClassId equals wpos.ObjectId
                            where weenie.Type == (int)WeenieType.Portal && wstr.Type == (int)PropertyString.Name && wpos.PositionType == (int)PositionType.Destination
                            select new
                            {
                                Weenie = weenie,
                                Name = wstr,
                                Dest = wpos
                            };

                var results = query.ToList();

                var dest = results.Where(i => i.Name.Value.Contains(searchName, StringComparison.OrdinalIgnoreCase)).Select(i => i.Dest).FirstOrDefault();

                if (dest == null)
                {
                    session.Network.EnqueueSend(new GameMessageSystemChat($"Couldn't find dungeon name {searchName}", ChatMessageType.Broadcast));
                    return;
                }

                var pos = new Position(dest.ObjCellId, dest.OriginX, dest.OriginY, dest.OriginZ, dest.AnglesX, dest.AnglesY, dest.AnglesZ, dest.AnglesW);
                WorldObject.AdjustDungeon(pos);

                session.Player.Teleport(pos);
            }
        }

        /// <summary>
        /// Shows the dungeon name for the current landblock
        /// </summary>
        [CommandHandler("dungeonname", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, "Shows the dungeon name for the current landblock")]
        public static void HandleDungeonName(Session session, params string[] parameters)
        {
            var landblock = session.Player.Location.Landblock;

            var blockStart = landblock << 16;
            var blockEnd = blockStart | 0xFFFF;

            using (var ctx = new WorldDbContext())
            {
                var query = from weenie in ctx.Weenie
                            join wstr in ctx.WeeniePropertiesString on weenie.ClassId equals wstr.ObjectId
                            join wpos in ctx.WeeniePropertiesPosition on weenie.ClassId equals wpos.ObjectId
                            where weenie.Type == (int)WeenieType.Portal && wpos.PositionType == (int)PositionType.Destination && wpos.ObjCellId >= blockStart && wpos.ObjCellId <= blockEnd
                            select wstr;

                var results = query.ToList();

                if (results.Count() == 0)
                {
                    session.Network.EnqueueSend(new GameMessageSystemChat($"Couldn't find dungeon {landblock:X4}", ChatMessageType.Broadcast));
                    return;
                }

                foreach (var result in results)
                {
                    var name = result.Value.TrimStart("Portal to ").TrimEnd(" Portal");

                    session.Network.EnqueueSend(new GameMessageSystemChat(name, ChatMessageType.Broadcast));
                }
            }
        }


        [CommandHandler("clearphysicscaches", AccessLevel.Developer, CommandHandlerFlag.None, 0, "Clears Physics Object Caches")]
        public static void HandleClearPhysicsCaches(Session session, params string[] parameters)
        {
            BSPCache.Clear();
            GfxObjCache.Clear();
            PolygonCache.Clear();
            VertexCache.Clear();

            CommandHandlerHelper.WriteOutputInfo(session, "Physics caches cleared");
        }

        [CommandHandler("forcegc", AccessLevel.Developer, CommandHandlerFlag.None, 0, "Forces .NET Garbage Collection")]
        public static void HandleForceGC(Session session, params string[] parameters)
        {
            GC.Collect();

            CommandHandlerHelper.WriteOutputInfo(session, ".NET Garbage Collection forced");
        }

        [CommandHandler("auditobjectmaint", AccessLevel.Developer, CommandHandlerFlag.None, 0, "Iterates over physics objects to find leaks")]
        public static void HandleAuditObjectMaint(Session session, params string[] parameters)
        {
            var serverObjects = ServerObjectManager.ServerObjects.Keys.ToHashSet();

            int objectTableErrors = 0;
            int visibleObjectTableErrors = 0;
            int voyeurTableErrors = 0;

            foreach (var value in ServerObjectManager.ServerObjects.Values)
            {
                {
                    var kvps = value.ObjMaint.GetKnownObjectsWhere(kvp => !serverObjects.Contains(kvp.Key));
                    foreach (var kvp in kvps)
                    {
                        if (value.ObjMaint.RemoveKnownObject(kvp.Value, false))
                        {
                            log.Debug($"AuditObjectMaint removed 0x{kvp.Value.ID:X8}:{kvp.Value.Name} (IsDestroyed:{kvp.Value.WeenieObj?.WorldObject?.IsDestroyed}, Position:{kvp.Value.Position}) from 0x{value.ID:X8}:{value.Name} (IsDestroyed:{value.WeenieObj?.WorldObject?.IsDestroyed}, Position:{value.Position}) [ObjectTable]");
                            objectTableErrors++;
                        }
                    }
                }

                {
                    var kvps = value.ObjMaint.GetVisibleObjectsWhere(kvp => !serverObjects.Contains(kvp.Key));
                    foreach (var kvp in kvps)
                    {
                        if (value.ObjMaint.RemoveVisibleObject(kvp.Value, false))
                        {
                            log.Debug($"AuditObjectMaint removed 0x{kvp.Value.ID:X8}:{kvp.Value.Name} (IsDestroyed:{kvp.Value.WeenieObj?.WorldObject?.IsDestroyed}, Position:{kvp.Value.Position}) from 0x{value.ID:X8}:{value.Name} (IsDestroyed:{value.WeenieObj?.WorldObject?.IsDestroyed}, Position:{value.Position}) [VisibleObjectTable]");
                            visibleObjectTableErrors++;
                        }
                    }
                }

                {
                    var kvps = value.ObjMaint.GetKnownPlayersWhere(kvp => !serverObjects.Contains(kvp.Key));
                    foreach (var kvp in kvps)
                    {
                        if (value.ObjMaint.RemoveKnownPlayer(kvp.Value))
                        {
                            log.Debug($"AuditObjectMaint removed 0x{kvp.Value.ID:X8}:{kvp.Value.Name} (IsDestroyed:{kvp.Value.WeenieObj?.WorldObject?.IsDestroyed}, Position:{kvp.Value.Position}) from 0x{value.ID:X8}:{value.Name} (IsDestroyed:{value.WeenieObj?.WorldObject?.IsDestroyed}, Position:{value.Position}) [VoyeurTable]");
                            voyeurTableErrors++;
                        }
                    }
                }
            }

            if (session != null)
                CommandHandlerHelper.WriteOutputInfo(session, $"Physics ObjMaint Audit Completed. Errors - objectTable: {objectTableErrors}, visibleObjectTable: {visibleObjectTableErrors}, voyeurTable: {voyeurTableErrors}");
            log.Info($"Physics ObjMaint Audit Completed. Errors - objectTable: {objectTableErrors}, visibleObjectTable: {visibleObjectTableErrors}, voyeurTable: {voyeurTableErrors}");
        }

        [CommandHandler("lootgen", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1, "Generate a piece of loot from the LootGenerationFactory.", "<wcid or classname> <tier>")]
        public static void HandleLootGen(Session session, params string[] parameters)
        {
            WorldObject wo = null;

            // create base item
            if (uint.TryParse(parameters[0], out var wcid))
                wo = WorldObjectFactory.CreateNewWorldObject(wcid);
            else
                wo = WorldObjectFactory.CreateNewWorldObject(parameters[0]);

            if (wo == null)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Couldn't find {parameters[0]}", ChatMessageType.Broadcast));
                return;
            }

            int tier = 1;
            if (parameters.Length > 1)
                int.TryParse(parameters[1], out tier);

            if (tier < 1 || tier > 8)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Loot Tier must be a number between 1 and 8", ChatMessageType.Broadcast));
                return;
            }

            if (wo.TsysMutationData == null && !Aetheria.IsAetheria(wo.WeenieClassId) && !(wo is PetDevice))
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"{wo.Name} ({wo.WeenieClassId}) missing PropertyInt.TsysMutationData", ChatMessageType.Broadcast));
                return;
            }

            var profile = new TreasureDeath()
            {
                Tier = tier,
                LootQualityMod = 0
            };

            var success = LootGenerationFactory.MutateItem(wo, profile, true);

            session.Player.TryCreateInInventoryWithNetworking(wo);
        }

        [CommandHandler("ciloot", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1, "Generates randomized loot in player's inventory", "<tier> optional: <# items>")]
        public static void HandleCILoot(Session session, params string[] parameters)
        {
            var tier = 1;
            int.TryParse(parameters[0], out tier);
            tier = Math.Clamp(tier, 1, 8);

            var numItems = 1;
            if (parameters.Length > 1)
                int.TryParse(parameters[1], out numItems);

            // Create a dummy treasure profile for passing in tier value
            TreasureDeath profile = new TreasureDeath
            {
                Tier = tier,
                LootQualityMod = 0,
                MagicItemTreasureTypeSelectionChances = 9,  // 8 or 9?
            };

            for (var i = 0; i < numItems; i++)
            {
                //var wo = LootGenerationFactory.CreateRandomLootObjects(profile, true);
                var wo = LootGenerationFactory.CreateRandomLootObjects_New(profile, TreasureItemCategory.MagicItem);
                if (wo != null)
                    session.Player.TryCreateInInventoryWithNetworking(wo);
                else
                    log.Error($"{session.Player.Name}.HandleCILoot: LootGenerationFactory.CreateRandomLootObjects({tier}) returned null");
            }
        }

        [CommandHandler("makeiou", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1, "Make an IOU and put it in your inventory", "<wcid>")]
        public static void HandleMakeIOU(Session session, params string[] parameters)
        {
            string weenieClassDescription = parameters[0];
            bool wcid = uint.TryParse(weenieClassDescription, out uint weenieClassId);

            if (!wcid)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"WCID must be a valid weenie id", ChatMessageType.Broadcast));
                return;
            }

            var iou = PlayerFactory.CreateIOU(weenieClassId);

            if (iou != null)
                session.Player.TryCreateInInventoryWithNetworking(iou);
        }

        [CommandHandler("testdeathitems", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0, "Test death item selection", "")]
        public static void HandleTestDeathItems(Session session, params string[] parameters)
        {
            var target = session.Player;
            if (parameters.Length > 0)
            {
                target = PlayerManager.GetOnlinePlayer(parameters[0]);
                if (target == null)
                {
                    session.Network.EnqueueSend(new GameMessageSystemChat($"Couldn't find {parameters[0]}", ChatMessageType.Broadcast));
                    return;
                }
            }

            var inventory = target.GetAllPossessions();
            var sorted = new DeathItems(inventory);

            var i = 0;
            foreach (var item in sorted.Inventory)
            {
                var bonded = item.WorldObject.Bonded ?? BondedStatus.Normal;

                if (bonded != BondedStatus.Normal)
                    continue;

                session.Network.EnqueueSend(new GameMessageSystemChat($"{++i}. {item.Name} ({item.Category}, AdjustedValue: {item.AdjustedValue})", ChatMessageType.Broadcast));
            }
        }

        [CommandHandler("forcelogout", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, "Force log off of specified character or last appraised character")]
        public static void HandleForceLogout(Session session, params string[] parameters)
        {
            HandleForceLogoff(session, parameters);
        }

        [CommandHandler("forcelogoff", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, "Force log off of specified character or last appraised character")]
        public static void HandleForceLogoff(Session session, params string[] parameters)
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
                        CommandHandlerHelper.WriteOutputInfo(session, $"Unable to force log off for {plr.Name}: Player is not online.");
                        return;
                    }
                }
                else
                {
                    CommandHandlerHelper.WriteOutputInfo(session, $"Unable to force log off for {playerName}: Player not found in manager.");
                    return;
                }
            }
            else
                target = CommandHandlerHelper.GetLastAppraisedObject(session);

            if (target != null && target is Player player)
            {
                //if (player.Session != null)
                //    player.Session.LogOffPlayer(true);
                //else
                //    player.LogOut();

                var msg = $"Player {player.Name} (0x{player.Guid}) found in PlayerManager.onlinePlayers.\n";
                msg += $"------- Session: {(player.Session != null ? $"{player.Session.EndPoint}" : "NULL")}\n";
                msg += $"------- CurrentLandblock: {(player.CurrentLandblock != null ? $"0x{player.CurrentLandblock.Id:X4}" : "NULL")}\n";
                msg += $"------- Location: {(player.Location != null ? $"{player.Location.ToLOCString()}" : "NULL")}\n";
                msg += $"------- IsLoggingOut: {player.IsLoggingOut}\n";
                msg += $"------- IsInDeathProcess: {player.IsInDeathProcess}\n";
                var foundOnLandblock = false;
                if (player.CurrentLandblock != null)
                    foundOnLandblock = LandblockManager.GetLandblock(player.CurrentLandblock.Id, false).GetObject(player.Guid) != null;
                msg += $"------- FoundOnLandblock: {foundOnLandblock}\n";
                var playerForcedLogOffRequested = player.ForcedLogOffRequested;
                msg += $"------- ForcedLogOffRequested: {playerForcedLogOffRequested}\n";

                msg += "Log off path taken: ";
                if (playerForcedLogOffRequested)
                {
                    player.Session?.Terminate(Network.Enum.SessionTerminationReason.ForcedLogOffRequested, new GameMessageBootAccount(" because the character was forced to log off by an admin"));
                    player.ForceLogoff();
                    msg += "player.Session?.Terminate() | player.ForceLogoff()";
                }
                else if (player.Session != null)
                {
                    player.ForcedLogOffRequested = true;
                    player.Session.Terminate(Network.Enum.SessionTerminationReason.ForcedLogOffRequested, new GameMessageBootAccount(" because the character was forced to log off by an admin"));
                    msg += "player.ForcedLogOffRequested = true | player.Session.Terminate()";
                }
                else if (player.CurrentLandblock != null && foundOnLandblock)
                {
                    player.ForcedLogOffRequested = true;
                    player.LogOut();
                    msg += "player.ForcedLogOffRequested = true | player.LogOut()";
                }
                else if (player.IsInDeathProcess)
                {
                    player.ForcedLogOffRequested = true;
                    player.IsInDeathProcess = false;
                    player.LogOut_Inner(true);
                    msg += "player.ForcedLogOffRequested = true | player.IsInDeathProcess = false | player.LogOut_Inner(true)";
                }
                else
                {
                    player.ForcedLogOffRequested = true;
                    msg += "player.ForcedLogOffRequested = true";
                }

                if (!playerForcedLogOffRequested)
                    msg += "\nUse this command again if this player does not properly log off within the next minute.";
                else
                    msg += "\nPlease send the above report to ACEmulator development team via Discord.";

                CommandHandlerHelper.WriteOutputInfo(session, msg);

                PlayerManager.BroadcastToAuditChannel(session?.Player, $"Forcing Log Off of {player.Name}...");
            }
            else
            {
                if (target != null)
                    CommandHandlerHelper.WriteOutputInfo(session, $"Unable to force log off for {target.Name}: Target is not a player.");
                //else
                //    CommandHandlerHelper.WriteOutputInfo(session, $"Unable to force log off for {playerName}: Player not found in manager.");
            }
        }

        [CommandHandler("showsession", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, "Show IP and ID for network session of last appraised character")]
        public static void HandleShowSession(Session session, params string[] parameters)
        {
            var target = CommandHandlerHelper.GetLastAppraisedObject(session);

            if (target != null && target is Player player)
            {
                if (player.Session != null)
                    session.Network.EnqueueSend(new GameMessageSystemChat($"Session IP: {player.Session.EndPoint} | ClientId: {player.Session.Network.ClientId} is connected to Character: {player.Name} (0x{player.Guid.Full.ToString("X8")}), Account: {player.Account.AccountName} ({player.Account.AccountId})", ChatMessageType.Broadcast));
                else
                    session.Network.EnqueueSend(new GameMessageSystemChat($"Session is null for {player.Name} which shouldn't occur.", ChatMessageType.Broadcast));
            }
        }

        [CommandHandler("requirecomps", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1,
            "Sets whether spell components are required to cast spells.",
            "[ on | off ]\n"
            + "This command sets whether spell components are required to cast spells..\n When turned on, spell components are required.\n When turned off, spell components are ignored.")]
        public static void HandleRequireComps(Session session, params string[] parameters)
        {
            var param = parameters[0];

            switch (param)
            {
                case "off":
                    session.Player.SpellComponentsRequired = false;
                    session.Player.EnqueueBroadcast(new GameMessagePublicUpdatePropertyBool(session.Player, PropertyBool.SpellComponentsRequired, session.Player.SpellComponentsRequired));
                    session.Network.EnqueueSend(new GameMessageSystemChat("You can now cast spells without components.", ChatMessageType.Broadcast));
                    break;
                case "on":
                default:
                    session.Player.SpellComponentsRequired = true;
                    session.Player.EnqueueBroadcast(new GameMessagePublicUpdatePropertyBool(session.Player, PropertyBool.SpellComponentsRequired, session.Player.SpellComponentsRequired));
                    session.Network.EnqueueSend(new GameMessageSystemChat("You can no longer cast spells without components.", ChatMessageType.Broadcast));
                    break;
            }
        }

        /// <summary>
        /// Enables / disables spell component burning
        /// </summary>
        [CommandHandler("safecomps", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0, "Enables / disables spell component burning", "<on/off>")]
        public static void HandleSafeComps(Session session, params string[] parameters)
        {
            var safeComps = true;
            if (parameters.Length > 0 && parameters[0].ToLower().Equals("off"))
                safeComps = false;

            session.Player.SafeSpellComponents = safeComps;

            if (safeComps)
                session.Network.EnqueueSend(new GameMessageSystemChat("Your spell components are now safe, and will not be consumed when casting spells.", ChatMessageType.Broadcast));
            else
                session.Network.EnqueueSend(new GameMessageSystemChat("Your spell components will now be consumed when casting spells.", ChatMessageType.Broadcast));
        }

        /// <summary>
        /// This is to add spells to items (whether loot or quest generated).  For making weapons to check damage from pcaps or other sources
        /// </summary>
        [CommandHandler("additemspell", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1, "Adds a spell to the last appraised item's spellbook.", "<spell id>")]
        public static void HandleAddItemSpell(Session session, params string[] parameters)
        {
            var obj = CommandHandlerHelper.GetLastAppraisedObject(session);
            if (obj == null)
                return;

            if (!Enum.TryParse(parameters[0], true, out SpellId spellId))
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"{parameters[0]} is not a valid spell id", ChatMessageType.Broadcast));
                return;
            }

            // ensure valid spell id
            var spell = new Spell(spellId);

            if (spell.NotFound)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat("SpellID is not found", ChatMessageType.Broadcast));
                return;
            }

            obj.Biota.GetOrAddKnownSpell((int)spellId, obj.BiotaDatabaseLock, out var spellAdded);

            var msg = spellAdded ? "added to" : "already on";

            session.Network.EnqueueSend(new GameMessageSystemChat($"{spell.Name} ({spell.Id}) {msg} {obj.Name}", ChatMessageType.Broadcast));
        }

        [CommandHandler("removeitemspell", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1, "Removes a spell to the last appraised item's spellbook.", "<spell id>")]
        public static void HandleRemoveItemSpell(Session session, params string[] parameters)
        {
            var obj = CommandHandlerHelper.GetLastAppraisedObject(session);
            if (obj == null)
                return;

            if (!Enum.TryParse(parameters[0], true, out SpellId spellId))
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"{parameters[0]} is not a valid spell id", ChatMessageType.Broadcast));
                return;
            }

            // ensure valid spell id
            var spell = new Spell(spellId);

            if (spell.NotFound)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat("SpellID is not found", ChatMessageType.Broadcast));
                return;
            }

            var spellRemoved = obj.Biota.TryRemoveKnownSpell((int)spellId, obj.BiotaDatabaseLock);

            var msg = spellRemoved ? "removed from" : "not found on";

            session.Network.EnqueueSend(new GameMessageSystemChat($"{spell.Name} ({spell.Id}) {msg} {obj.Name}", ChatMessageType.Broadcast));
        }

        [CommandHandler("pktimer", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, "Sets your PK timer to the current time")]
        public static void HandlePKTimer(Session session, params string[] parameters)
        {
            session.Player.UpdatePKTimer();

            session.Network.EnqueueSend(new GameMessageSystemChat($"Updated PK timer", ChatMessageType.Broadcast));
        }

        [CommandHandler("fellow-info", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, "Shows debug info for fellowships.")]
        public static void HandleFellowInfo(Session session, params string[] parameters)
        {
            var player = CommandHandlerHelper.GetLastAppraisedObject(session) as Player;

            if (player == null)
                player = session.Player;

            var fellowship = player.Fellowship;

            if (fellowship == null)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat("Player target must be in a fellowship to use this command.", ChatMessageType.Broadcast));
                return;
            }

            var fellows = fellowship.GetFellowshipMembers();

            //var levelSum = fellows.Values.Select(f => f.Level.Value).Sum();
            var levelXPSum = fellows.Values.Select(f => f.GetXPToNextLevel(f.Level.Value)).Sum();

            // this should match up with the client
            foreach (var fellow in fellows.Values.OrderBy(f => f.Level))
            {
                //var levelScale = (double)fellow.Level.Value / levelSum;
                var levelXPScale = (double)fellow.GetXPToNextLevel(fellow.Level.Value) / levelXPSum;

                //session.Network.EnqueueSend(new GameMessageSystemChat($"{fellow.Name}: {Math.Round(levelScale * 100, 2)}% / {Math.Round(levelXPScale * 100, 2)}%", ChatMessageType.Broadcast));
                session.Network.EnqueueSend(new GameMessageSystemChat($"{fellow.Name}: {Math.Round(levelXPScale * 100, 2)}%", ChatMessageType.Broadcast));
            }

            session.Network.EnqueueSend(new GameMessageSystemChat($"----------", ChatMessageType.Broadcast));

            session.Network.EnqueueSend(new GameMessageSystemChat($"ShareXP: {fellowship.ShareXP}", ChatMessageType.Broadcast));
            session.Network.EnqueueSend(new GameMessageSystemChat($"EvenShare: {fellowship.EvenShare}", ChatMessageType.Broadcast));

            session.Network.EnqueueSend(new GameMessageSystemChat($"Distance scale:", ChatMessageType.Broadcast));

            foreach (var fellow in fellows.Values)
            {
                var dist = player.Location.Distance2D(fellow.Location);

                var distanceScalar = fellowship.GetDistanceScalar(player, fellow, XpType.Kill);

                session.Network.EnqueueSend(new GameMessageSystemChat($"{fellow.Name}: {Math.Round(dist):N0} ({distanceScalar:F2}) - {fellow.Location}", ChatMessageType.Broadcast));
            }
        }

        [CommandHandler("fellow-dist", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, "Shows distance to each fellowship member")]
        public static void HandleFellowDist(Session session, params string[] parameters)
        {
            var player = session.Player;

            var fellowship = player.Fellowship;

            if (fellowship == null)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat("You must be in a fellowship to use this command.", ChatMessageType.Broadcast));
                return;
            }

            var fellows = fellowship.GetFellowshipMembers();

            foreach (var fellow in fellows.Values)
            {
                var dist2d = session.Player.Location.Distance2D(fellow.Location);
                var dist3d = session.Player.Location.DistanceTo(fellow.Location);

                var scalar = session.Player.Fellowship.GetDistanceScalar(session.Player, fellow, XpType.Kill);

                session.Network.EnqueueSend(new GameMessageSystemChat($"{fellow.Name} | 2d: {dist2d:N0} | 3d: {dist3d:N0} | Scalar: {scalar:N0}", ChatMessageType.Broadcast));
            }
        }

        [CommandHandler("generatordump", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0,
            "Lists all properties for the last generator you examined.",
            "")]
        public static void HandleGeneratorDump(Session session, params string[] parameters)
        {
            // TODO: output

            var objectId = new ObjectGuid();

            if (session.Player.HealthQueryTarget.HasValue || session.Player.ManaQueryTarget.HasValue || session.Player.CurrentAppraisalTarget.HasValue)
            {
                if (session.Player.HealthQueryTarget.HasValue)
                    objectId = new ObjectGuid((uint)session.Player.HealthQueryTarget);
                else if (session.Player.ManaQueryTarget.HasValue)
                    objectId = new ObjectGuid((uint)session.Player.ManaQueryTarget);
                else
                    objectId = new ObjectGuid((uint)session.Player.CurrentAppraisalTarget);

                var wo = session.Player.CurrentLandblock?.GetObject(objectId);

                if (wo == null)
                    return;

                if (objectId.IsPlayer())
                    return;

                var msg = "";
                if (wo.IsGenerator)
                {
                    msg = $"Generator Dump for {wo.Name} (0x{wo.Guid.ToString()})\n";
                    msg += $"Generator WCID: {wo.WeenieClassId}\n";
                    msg += $"Generator WeenieClassName: {wo.WeenieClassName}\n";
                    msg += $"Generator WeenieType: {wo.WeenieType.ToString()}\n";
                    msg += $"Generator Status: {(wo.GeneratorDisabled ? "Disabled" : "Enabled")}\n";
                    msg += $"GeneratorType: {wo.GeneratorType.ToString()}\n";
                    msg += $"GeneratorTimeType: {wo.GeneratorTimeType.ToString()}\n";
                    if (wo.GeneratorTimeType == GeneratorTimeType.Event)
                        msg += $"GeneratorEvent: {(!string.IsNullOrWhiteSpace(wo.GeneratorEvent) ? wo.GeneratorEvent : "Undef")}\n";
                    if (wo.GeneratorTimeType == GeneratorTimeType.RealTime)
                    {
                        msg += $"GeneratorStartTime: {wo.GeneratorStartTime} ({Time.GetDateTimeFromTimestamp(wo.GeneratorStartTime).ToLocalTime()})\n";
                        msg += $"GeneratorEndTime: {wo.GeneratorEndTime} ({Time.GetDateTimeFromTimestamp(wo.GeneratorEndTime).ToLocalTime()})\n";
                    }
                    msg += $"GeneratorEndDestructionType: {wo.GeneratorEndDestructionType.ToString()}\n";
                    msg += $"GeneratorDestructionType: {wo.GeneratorDestructionType.ToString()}\n";
                    msg += $"GeneratorRadius: {wo.GetProperty(PropertyFloat.GeneratorRadius) ?? 0f}\n";
                    msg += $"InitGeneratedObjects: {wo.InitGeneratedObjects}\n";
                    msg += $"MaxGeneratedObjects: {wo.MaxGeneratedObjects}\n";
                    msg += $"GeneratorInitialDelay: {wo.GeneratorInitialDelay}\n";
                    msg += $"RegenerationInterval: {wo.RegenerationInterval}\n";
                    msg += $"GeneratorUpdateTimestamp: {wo.GeneratorUpdateTimestamp} ({Time.GetDateTimeFromTimestamp(wo.GeneratorUpdateTimestamp).ToLocalTime()})\n";
                    msg += $"NextGeneratorUpdateTime: {wo.NextGeneratorUpdateTime} ({((wo.NextGeneratorUpdateTime == double.MaxValue) ? "Disabled" : Time.GetDateTimeFromTimestamp(wo.NextGeneratorUpdateTime).ToLocalTime().ToString())})\n";
                    msg += $"RegenerationTimestamp: {wo.RegenerationTimestamp} ({Time.GetDateTimeFromTimestamp(wo.RegenerationTimestamp).ToLocalTime()})\n";
                    msg += $"NextGeneratorRegenerationTime: {wo.NextGeneratorRegenerationTime} ({((wo.NextGeneratorRegenerationTime == double.MaxValue) ? "On Demand" : Time.GetDateTimeFromTimestamp(wo.NextGeneratorRegenerationTime).ToLocalTime().ToString())})\n";

                    msg += $"GeneratorProfiles.Count: {wo.GeneratorProfiles.Count(g => !g.IsPlaceholder)}\n";
                    msg += $"GeneratorActiveProfiles.Count: {wo.GeneratorActiveProfiles.Count}\n";
                    msg += $"CurrentCreate: {wo.CurrentCreate}\n";

                    msg += $"===============================================\n";
                    foreach (var activeProfile in wo.GeneratorActiveProfiles)
                    {
                        var profile = wo.GeneratorProfiles[activeProfile];

                        msg += $"Active GeneratorProfile id: {activeProfile} | LinkId: {profile.LinkId}\n";

                        msg += $"Probability: {profile.Biota.Probability} | WCID: {profile.Biota.WeenieClassId} | Delay: {profile.Biota.Delay} | Init: {profile.Biota.InitCreate} | Max: {profile.Biota.MaxCreate}\n";
                        msg += $"WhenCreate: {profile.Biota.WhenCreate.ToString()} | WhereCreate: {profile.Biota.WhereCreate.ToString()}\n";
                        msg += $"StackSize: {profile.Biota.StackSize} | PaletteId: {profile.Biota.PaletteId} | Shade: {profile.Biota.Shade}\n";
                        msg += $"CurrentCreate: {profile.CurrentCreate} | Spawned.Count: {profile.Spawned.Count} | SpawnQueue.Count: {profile.SpawnQueue.Count}\n";
                        msg += $"GeneratedTreasureItem: {profile.GeneratedTreasureItem}\n";
                        msg += $"IsMaxed: {profile.IsMaxed}\n";
                        if (!profile.IsMaxed)
                            msg += $"IsAvailable: {profile.IsAvailable}{(profile.IsAvailable ? "" : $", NextAvailable: {profile.NextAvailable.ToLocalTime()}")}\n";
                        msg += $"--====--\n";
                        if (profile.Spawned.Count > 0)
                        {
                            msg += "Spawned Objects:\n";
                            foreach (var spawn in profile.Spawned.Values)
                            {
                                msg += $"0x{spawn.Guid}: {spawn.Name} - {spawn.WeenieClassId} - {spawn.WeenieType}\n";
                                var spawnWO = spawn.TryGetWorldObject();
                                if (spawnWO != null)
                                {
                                    if (spawnWO.Location != null)
                                        msg += $" LOC: {spawnWO.Location.ToLOCString()}\n";
                                    else if (spawnWO.ContainerId == wo.Guid.Full)
                                        msg += $" Contained by Generator\n";
                                    else if (spawnWO.WielderId == wo.Guid.Full)
                                        msg += $" Wielded by Generator\n";
                                    else
                                        msg += $" Location Unknown\n";
                                }
                                else
                                    msg += $" LOC: Unknown, WorldObject could not be found\n";
                            }
                            msg += $"--====--\n";
                        }

                        if (profile.SpawnQueue.Count > 0)
                        {
                            msg += "Pending Spawn Times:\n";
                            foreach (var spawn in profile.SpawnQueue)
                            {
                                msg += $"{spawn.ToLocalTime()}\n";
                            }
                            msg += $"--====--\n";
                        }

                        msg += $"===============================================\n";
                    }
                }
                else
                    msg = $"{wo.Name} (0x{wo.Guid.ToString()}) is not a generator.";

                session.Network.EnqueueSend(new GameMessageSystemChat(msg, ChatMessageType.System));
            }
        }

        [CommandHandler("purchase-house", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, "Instantly purchase the house for the last appraised covenant crystal.")]
        public static void HandlePurchaseHouse(Session session, params string[] parameters)
        {
            var slumlord = CommandHandlerHelper.GetLastAppraisedObject(session) as SlumLord;

            if (slumlord == null)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat("Couldn't find slumlord", ChatMessageType.Broadcast));
                return;
            }
            session.Player.SetHouseOwner(slumlord);
            session.Player.GiveDeed(slumlord);
        }

        [CommandHandler("barrier-test", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, "Shows debug information for house barriers")]
        public static void HandleBarrierTest(Session session, params string[] parameters)
        {
            var cell = session.Player.Location.Cell;
            Console.WriteLine($"CurCell: {cell:X8}");

            if (session.Player.CurrentLandblock.IsDungeon)
            {
                Console.WriteLine($"Dungeon landblock");

                if (!HouseManager.ApartmentBlocks.ContainsKey(session.Player.Location.Landblock))
                    return;
            }
            else
            {
                cell = session.Player.Location.GetOutdoorCell();
                Console.WriteLine($"OutdoorCell: {cell:X8}");
            }

            var barrier = HouseCell.HouseCells.ContainsKey(cell);
            Console.WriteLine($"Barrier: {barrier}");
        }

        [CommandHandler("targetloc", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, "Shows the location of the last appraised object")]
        public static void HandleTargetLoc(Session session, params string[] parameters)
        {
            WorldObject wo = null;
            if (parameters.Length == 0)
            {
                wo = CommandHandlerHelper.GetLastAppraisedObject(session);

                if (wo == null) return;
            }
            else
            {
                if (parameters[0].StartsWith("0x"))
                    parameters[0] = parameters[0].Substring(2);

                if (!uint.TryParse(parameters[0], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint guid))
                {
                    session.Network.EnqueueSend(new GameMessageSystemChat($"{parameters[0]} is not a valid guid", ChatMessageType.Broadcast));
                    return;
                }

                wo = session.Player.CurrentLandblock?.GetObject(guid);

                if (wo == null)
                    wo = ServerObjectManager.GetObjectA(guid)?.WeenieObj?.WorldObject;

                if (wo == null)
                {
                    session.Network.EnqueueSend(new GameMessageSystemChat($"Couldn't find {parameters[0]}", ChatMessageType.Broadcast));
                    return;
                }
            }

            session.Network.EnqueueSend(new GameMessageSystemChat($"CurrentLandblock: 0x{wo.CurrentLandblock?.Id.Landblock:X4}", ChatMessageType.Broadcast));
            session.Network.EnqueueSend(new GameMessageSystemChat($"Location: {wo.Location?.ToLOCString()}", ChatMessageType.Broadcast));
            session.Network.EnqueueSend(new GameMessageSystemChat($"Physics : {wo.PhysicsObj?.Position}", ChatMessageType.Broadcast));
            session.Network.EnqueueSend(new GameMessageSystemChat($"CurCell: 0x{wo.PhysicsObj?.CurCell?.ID:X8}", ChatMessageType.Broadcast));
        }

        [CommandHandler("damagehistory", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld)]
        public static void HandleDamageHistory(Session session, params string[] parameters)
        {
            session.Network.EnqueueSend(new GameMessageSystemChat(session.Player.DamageHistory.ToString(), ChatMessageType.Broadcast));
        }

        [CommandHandler("remove-vitae", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, "Removes vitae from last appraised player")]
        public static void HandleRemoveVitae(Session session, params string[] parameters)
        {
            var player = CommandHandlerHelper.GetLastAppraisedObject(session) as Player;

            if (player == null)
                player = session.Player;

            player.EnchantmentManager.RemoveVitae();

            if (player != session.Player)
                session.Network.EnqueueSend(new GameMessageSystemChat("Removed vitae for {player.Name}", ChatMessageType.Broadcast));
        }

        [CommandHandler("fast", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld)]
        public static void HandleFast(Session session, params string[] parameters)
        {
            var spell = new Spell(SpellId.QuicknessSelf8);
            session.Player.CreateEnchantment(session.Player, session.Player, null, spell);

            spell = new Spell(SpellId.SprintSelf8);
            session.Player.CreateEnchantment(session.Player, session.Player, null, spell);

            spell = new Spell(SpellId.StrengthSelf8);
            session.Player.CreateEnchantment(session.Player, session.Player, null, spell);
        }

        [CommandHandler("slow", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld)]
        public static void HandleSlow(Session session, params string[] parameters)
        {
            var spell = new Spell(SpellId.SlownessSelf8);
            session.Player.CreateEnchantment(session.Player, session.Player, null, spell);

            spell = new Spell(SpellId.LeadenFeetSelf8);
            session.Player.CreateEnchantment(session.Player, session.Player, null, spell);

            spell = new Spell(SpellId.WeaknessSelf8);
            session.Player.CreateEnchantment(session.Player, session.Player, null, spell);
        }

        [CommandHandler("rip", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld)]
        public static void HandleRip(Session session, params string[] parameters)
        {
            // insta-death, without the confirmation dialog from /die
            // useful during developer testing
            session.Player.TakeDamage(session.Player, DamageType.Bludgeon, session.Player.Health.Current);
        }

        public static List<PropertyFloat> ResistProperties = new List<PropertyFloat>()
        {
            PropertyFloat.ResistSlash,
            PropertyFloat.ResistPierce,
            PropertyFloat.ResistBludgeon,
            PropertyFloat.ResistFire,
            PropertyFloat.ResistCold,
            PropertyFloat.ResistAcid,
            PropertyFloat.ResistElectric,
            PropertyFloat.ResistNether
        };

        [CommandHandler("resist-info", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, "Shows the resistance info for the last appraised creature.")]
        public static void HandleResistInfo(Session session, params string[] parameters)
        {
            var creature = CommandHandlerHelper.GetLastAppraisedObject(session) as Creature;
            if (creature == null)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"You must appraise a creature to use this command.", ChatMessageType.Broadcast));
                return;
            }

            session.Network.EnqueueSend(new GameMessageSystemChat($"{creature.Name} ({creature.Guid}):", ChatMessageType.Broadcast));

            var resistInfo = new Dictionary<PropertyFloat, double?>();
            foreach (var prop in ResistProperties)
                resistInfo.Add(prop, creature.GetProperty(prop));

            foreach (var kvp in resistInfo.OrderByDescending(i => i.Value))
                session.Network.EnqueueSend(new GameMessageSystemChat($"{kvp.Key} - {kvp.Value}", ChatMessageType.Broadcast));
        }

        [CommandHandler("debugspell", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, "Toggles spell projectile debugging info")]
        public static void HandleDebugSpell(Session session, params string[] parameters)
        {
            if (parameters.Length == 0)
            {
                session.Player.DebugSpell = !session.Player.DebugSpell;
            }
            else
            {
                if (parameters[0].Equals("on", StringComparison.OrdinalIgnoreCase))
                    session.Player.DebugSpell = true;
                else
                    session.Player.DebugSpell = false;
            }
            session.Network.EnqueueSend(new GameMessageSystemChat($"Spell projectile debugging is {(session.Player.DebugSpell ? "enabled" : "disabled")}", ChatMessageType.Broadcast));
        }

        [CommandHandler("recordcast", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, "Records spell casting keypresses to server for debugging")]
        public static void HandleRecordCast(Session session, params string[] parameters)
        {
            if (parameters.Length == 0)
            {
                session.Player.RecordCast.Enabled = !session.Player.RecordCast.Enabled;
            }
            else
            {
                if (parameters[0].Equals("on", StringComparison.OrdinalIgnoreCase))
                    session.Player.RecordCast.Enabled = true;
                else
                    session.Player.RecordCast.Enabled = false;
            }
            session.Network.EnqueueSend(new GameMessageSystemChat($"Record cast {(session.Player.RecordCast.Enabled ? "enabled" : "disabled")}", ChatMessageType.Broadcast));
        }

        [CommandHandler("pscript", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1)]
        public static void HandlePScript(Session session, params string[] parameters)
        {
            var wo = CommandHandlerHelper.GetLastAppraisedObject(session);
            if (wo == null) return;

            if (!Enum.TryParse(typeof(PlayScript), parameters[0], true, out var pscript))
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Couldn't find PlayScript.{parameters[0]}", ChatMessageType.Broadcast));
                return;
            }
            wo.EnqueueBroadcast(new GameMessageScript(wo.Guid, (PlayScript)pscript));
        }

        [CommandHandler("getinfo", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, "Shows basic info for the last appraised object.")]
        public static void HandleGetInfo(Session session, params string[] parameters)
        {
            var wo = CommandHandlerHelper.GetLastAppraisedObject(session);

            if (wo != null)
                session.Network.EnqueueSend(new GameMessageSystemChat($"GUID: {wo.Guid}\nWeenieClassId: {wo.WeenieClassId}\nWeenieClassName: {wo.WeenieClassName}", ChatMessageType.Broadcast));
        }

        public static WorldObject LastTestAim;

        [CommandHandler("testaim", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1, "Tests the aim high/low motions, and projectile spawn position")]
        public static void HandleTestAim(Session session, params string[] parameters)
        {
            var motionStr = parameters[0];

            if (!motionStr.StartsWith("Aim", StringComparison.OrdinalIgnoreCase))
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Motion must start with Aim!", ChatMessageType.Broadcast));
                return;
            }

            if (!Enum.TryParse(motionStr, true, out MotionCommand motionCommand))
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Couldn't find MotionCommand {motionStr}", ChatMessageType.Broadcast));
                return;
            }

            var positive = motionCommand >= MotionCommand.AimHigh15 && motionCommand <= MotionCommand.AimHigh90;

            if (LastTestAim != null)
                LastTestAim.Destroy();

            var motion = new Motion(session.Player, motionCommand);

            session.Player.EnqueueBroadcastMotion(motion);

            // spawn ethereal arrow w/ no velocity or gravity
            var localOrigin = session.Player.GetProjectileSpawnOrigin(300, motionCommand);

            var globalOrigin = session.Player.Location.Pos + Vector3.Transform(localOrigin, session.Player.Location.Rotation);

            var wo = WorldObjectFactory.CreateNewWorldObject(300);
            wo.Ethereal = true;
            wo.GravityStatus = false;

            var angle = motionCommand.GetAimAngle().ToRadians();
            var zRotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, angle);

            wo.Location = new Position(session.Player.Location);
            wo.Location.Pos = globalOrigin;
            wo.Location.Rotation *= zRotation;

            session.Player.CurrentLandblock.AddWorldObject(wo);

            LastTestAim = wo;
        }

        [CommandHandler("reload-landblock", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, "Reloads the current landblock.")]
        public static void HandleReloadLandblocks(Session session, params string[] parameters)
        {
            var landblock = session.Player.CurrentLandblock;

            var landblockId = landblock.Id.Raw | 0xFFFF;

            session.Network.EnqueueSend(new GameMessageSystemChat($"Reloading 0x{landblockId:X8}", ChatMessageType.Broadcast));

            // destroy all non-player server objects
            landblock.DestroyAllNonPlayerObjects();

            // clear landblock cache
            DatabaseManager.World.ClearCachedInstancesByLandblock(landblock.Id.Landblock);

            // reload landblock
            var actionChain = new ActionChain();
            actionChain.AddDelayForOneTick();
            actionChain.AddAction(session.Player, () =>
            {
                landblock.Init(true);
            });
            actionChain.EnqueueChain();
        }

        [CommandHandler("showvelocity", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, "Shows the velocity of the last appraised object.")]
        public static void HandleShowVelocity(Session session, params string[] parameters)
        {
            var obj = CommandHandlerHelper.GetLastAppraisedObject(session);

            if (obj?.PhysicsObj == null)
                return;

            session.Network.EnqueueSend(new GameMessageSystemChat($"Velocity: {obj.Velocity}", ChatMessageType.Broadcast));
            session.Network.EnqueueSend(new GameMessageSystemChat($"Physics.Velocity: {obj.PhysicsObj.Velocity}", ChatMessageType.Broadcast));
            session.Network.EnqueueSend(new GameMessageSystemChat($"CachedVelocity: {obj.PhysicsObj.CachedVelocity}", ChatMessageType.Broadcast));
        }

        [CommandHandler("bumpvelocity", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, "Bumps the velocity of the last appraised object.")]
        public static void HandleBumpVelocity(Session session, params string[] parameters)
        {
            var obj = CommandHandlerHelper.GetLastAppraisedObject(session);

            if (obj?.PhysicsObj == null)
                return;

            var velocity = new Vector3(0, 0, 0.5f);

            obj.PhysicsObj.Velocity = velocity;

            session.Network.EnqueueSend(new GameMessageVectorUpdate(obj));
        }

        [CommandHandler("check-collision", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, "Checks if the player is currently colliding with any other objects.")]
        public static void HandleCheckEthereal(Session session, params string[] parameters)
        {
            var colliding = session.Player.PhysicsObj.ethereal_check_for_collisions();

            session.Network.EnqueueSend(new GameMessageSystemChat($"IsColliding: {colliding}", ChatMessageType.Broadcast));
        }

        // faction
        [CommandHandler("faction", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0,
            "sets your own faction state.",
            "< none / ch / ew / rb > (rank)\n" +
            "This command sets your current faction state\n" +
            "< none > No Faction\n" +
            "< ch > Celestial Hand\n" +
            "< ew > Eldrytch Web\n" +
            "< rb > Radiant Blood\n" +
            "(rank) 1 = Initiate | 2 = Adept | 3 = Knight | 4 = Lord | 5 = Master")]
        public static void HandleFaction(Session session, params string[] parameters)
        {
            var rankStr = "Initiate";
            if (parameters.Length == 0)
            {
                var message = $"Your current Faction state is: {session.Player.Society.ToSentence()}\n"
                    + "You can change it to the following:\n"
                    + "NONE      = No Faction\n"
                    + "CH        = Celestial Hand\n"
                    + "EW        = Eldrytch Web\n"
                    + "RB        = Radiant Blood\n"
                    + "Optionally you can also include a rank, otherwise rank will be set to Initiate\n1 = Initiate | 2 = Adept | 3 = Knight | 4 = Lord | 5 = Master";
                CommandHandlerHelper.WriteOutputInfo(session, message, ChatMessageType.Broadcast);
            }
            else
            {
                switch (parameters[0].ToLower())
                {
                    case "none":
                        session.Player.Faction1Bits = null;
                        session.Player.SocietyRankCelhan = null;
                        session.Player.SocietyRankEldweb = null;
                        session.Player.SocietyRankRadblo = null;
                        session.Player.QuestManager.Erase("SocietyMember");
                        session.Player.QuestManager.Erase("SocietyFlag");
                        session.Player.QuestManager.Erase("CelestialHandMember");
                        session.Player.QuestManager.Erase("EldrytchWebMember");
                        session.Player.QuestManager.Erase("RadiantBloodMember");
                        break;
                    case "ch":
                        session.Player.Faction1Bits = FactionBits.CelestialHand;
                        session.Player.SocietyRankCelhan = 1;
                        session.Player.SocietyRankEldweb = null;
                        session.Player.SocietyRankRadblo = null;
                        session.Player.QuestManager.SetQuestBits("SocietyMember", (int)FactionBits.CelestialHand, true);
                        session.Player.QuestManager.SetQuestBits("SocietyFlag", (int)FactionBits.CelestialHand, true);
                        session.Player.QuestManager.SetQuestBits("SocietyMember", (int)(FactionBits.EldrytchWeb | FactionBits.RadiantBlood), false);
                        session.Player.QuestManager.SetQuestBits("SocietyFlag", (int)(FactionBits.EldrytchWeb | FactionBits.RadiantBlood), false);
                        session.Player.QuestManager.Stamp("CelestialHandMember");
                        session.Player.QuestManager.Erase("EldrytchWebMember");
                        session.Player.QuestManager.Erase("RadiantBloodMember");
                        if (parameters.Length == 2 && int.TryParse(parameters[1], out var rank))
                        {
                            switch (rank)
                            {
                                case 1:
                                    session.Player.SocietyRankCelhan = 1;
                                    rankStr = "Initiate";
                                    break;
                                case 2:
                                    session.Player.SocietyRankCelhan = 101;
                                    rankStr = "Adept";
                                    break;
                                case 3:
                                    session.Player.SocietyRankCelhan = 301;
                                    rankStr = "Knight";
                                    break;
                                case 4:
                                    session.Player.SocietyRankCelhan = 601;
                                    rankStr = "Lord";
                                    break;
                                case 5:
                                    session.Player.SocietyRankCelhan = 1001;
                                    rankStr = "Master";
                                    break;
                            }
                        }
                        break;
                    case "ew":
                        session.Player.Faction1Bits = FactionBits.EldrytchWeb;
                        session.Player.SocietyRankCelhan = null;
                        session.Player.SocietyRankEldweb = 1;
                        session.Player.SocietyRankRadblo = null;
                        session.Player.QuestManager.SetQuestBits("SocietyMember", (int)FactionBits.EldrytchWeb, true);
                        session.Player.QuestManager.SetQuestBits("SocietyFlag", (int)FactionBits.EldrytchWeb, true);
                        session.Player.QuestManager.SetQuestBits("SocietyMember", (int)(FactionBits.CelestialHand | FactionBits.RadiantBlood), false);
                        session.Player.QuestManager.SetQuestBits("SocietyFlag", (int)(FactionBits.CelestialHand | FactionBits.RadiantBlood), false);
                        session.Player.QuestManager.Erase("CelestialHandMember");
                        session.Player.QuestManager.Stamp("EldrytchWebMember");
                        session.Player.QuestManager.Erase("RadiantBloodMember");
                        if (parameters.Length == 2 && int.TryParse(parameters[1], out rank))
                        {
                            switch (rank)
                            {
                                case 1:
                                    session.Player.SocietyRankEldweb = 1;
                                    rankStr = "Initiate";
                                    break;
                                case 2:
                                    session.Player.SocietyRankEldweb = 101;
                                    rankStr = "Adept";
                                    break;
                                case 3:
                                    session.Player.SocietyRankEldweb = 301;
                                    rankStr = "Knight";
                                    break;
                                case 4:
                                    session.Player.SocietyRankEldweb = 601;
                                    rankStr = "Lord";
                                    break;
                                case 5:
                                    session.Player.SocietyRankEldweb = 1001;
                                    rankStr = "Master";
                                    break;
                            }
                        }
                        break;
                    case "rb":
                        session.Player.Faction1Bits = FactionBits.RadiantBlood;
                        session.Player.SocietyRankCelhan = null;
                        session.Player.SocietyRankEldweb = null;
                        session.Player.SocietyRankRadblo = 1;
                        session.Player.QuestManager.SetQuestBits("SocietyMember", (int)FactionBits.RadiantBlood, true);
                        session.Player.QuestManager.SetQuestBits("SocietyFlag", (int)FactionBits.RadiantBlood, true);
                        session.Player.QuestManager.SetQuestBits("SocietyMember", (int)(FactionBits.CelestialHand | FactionBits.EldrytchWeb), false);
                        session.Player.QuestManager.SetQuestBits("SocietyFlag", (int)(FactionBits.CelestialHand | FactionBits.EldrytchWeb), false);
                        session.Player.QuestManager.Erase("CelestialHandMember");
                        session.Player.QuestManager.Erase("EldrytchWebMember");
                        session.Player.QuestManager.Stamp("RadiantBloodMember");
                        if (parameters.Length == 2 && int.TryParse(parameters[1], out rank))
                        {
                            switch (rank)
                            {
                                case 1:
                                    session.Player.SocietyRankRadblo = 1;
                                    rankStr = "Initiate";
                                    break;
                                case 2:
                                    session.Player.SocietyRankRadblo = 101;
                                    rankStr = "Adept";
                                    break;
                                case 3:
                                    session.Player.SocietyRankRadblo = 301;
                                    rankStr = "Knight";
                                    break;
                                case 4:
                                    session.Player.SocietyRankRadblo = 601;
                                    rankStr = "Lord";
                                    break;
                                case 5:
                                    session.Player.SocietyRankRadblo = 1001;
                                    rankStr = "Master";
                                    break;
                            }
                        }
                        break;
                }
                session.Player.EnqueueBroadcast(new GameMessagePrivateUpdatePropertyInt(session.Player, PropertyInt.Faction1Bits, (int)(session.Player.Faction1Bits ?? 0)));
                session.Player.EnqueueBroadcast(new GameMessagePrivateUpdatePropertyInt(session.Player, PropertyInt.SocietyRankCelhan, session.Player.SocietyRankCelhan ?? 0));
                session.Player.EnqueueBroadcast(new GameMessagePrivateUpdatePropertyInt(session.Player, PropertyInt.SocietyRankEldweb, session.Player.SocietyRankEldweb ?? 0));
                session.Player.EnqueueBroadcast(new GameMessagePrivateUpdatePropertyInt(session.Player, PropertyInt.SocietyRankRadblo, session.Player.SocietyRankRadblo ?? 0));
                session.Player.SendTurbineChatChannels();
                CommandHandlerHelper.WriteOutputInfo(session, $"Your current Faction state is now set to: {session.Player.Society.ToSentence()}{(session.Player.Society != FactionBits.None ? $" with a rank of {rankStr}" : "")}", ChatMessageType.Broadcast);

                PlayerManager.BroadcastToAuditChannel(session.Player, $"{session.Player.Name} changed their Faction state to {session.Player.Society.ToSentence()}{(session.Player.Society != FactionBits.None ? $" with a rank of {rankStr}" : "")}.");
            }
        }

        /// <summary>
        /// Shows the DeathTreasure tier for the last appraised monster
        /// </summary>
        [CommandHandler("showtier", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, "Shows the DeathTreasure tier for the last appraised monster")]
        public static void HandleShowTier(Session session, params string[] parameters)
        {
            var creature = CommandHandlerHelper.GetLastAppraisedObject(session) as Creature;

            if (creature != null)
            {
                var msg = creature.DeathTreasure != null ? $"DeathTreasure - Tier: {creature.DeathTreasure.Tier}" : "doesn't have PropertyDataId.DeathTreasureType";

                CommandHandlerHelper.WriteOutputInfo(session, $"{creature.Name} ({creature.Guid}) {msg}");
            }
        }

        /// <summary>
        /// Shows a list of monsters for a particular tier #
        /// </summary>
        [CommandHandler("tiermobs", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1, "Shows a list of monsters for a particular tier #", "tier")]
        public static void HandleTierMobs(Session session, params string[] parameters)
        {
            if (!uint.TryParse(parameters[0], out var tier))
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Invalid tier {parameters[0]}");
                return;
            }
            if (tier < 1 || tier > 8)
            {
                CommandHandlerHelper.WriteOutputInfo(session, "Please enter a tier between 1-8");
                return;
            }
            using (var ctx = new WorldDbContext())
            {
                var query = from weenie in ctx.Weenie
                            join deathTreasure in ctx.WeeniePropertiesDID on weenie.ClassId equals deathTreasure.ObjectId
                            join treasureDeath in ctx.TreasureDeath on deathTreasure.Value equals treasureDeath.TreasureType
                            where weenie.Type == (int)WeenieType.Creature && deathTreasure.Type == (ushort)PropertyDataId.DeathTreasureType && treasureDeath.Tier == tier
                            select weenie;

                var results = query.ToList();

                CommandHandlerHelper.WriteOutputInfo(session, $"Found {results.Count()} monsters for tier {tier}");

                foreach (var result in results)
                    CommandHandlerHelper.WriteOutputInfo(session, result.ClassName);
            }
        }

        [CommandHandler("delevel", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1, "Attempts to delevel the current player. Requires enough unassigned xp and unspent skill credits.", "new level")]
        public static void HandleDelevel(Session session, params string[] parameters)
        {
            HandleDelevel(session, false, parameters);
        }

        public static void HandleDelevel(Session session, bool confirmed, params string[] parameters)
        {
            if (!int.TryParse(parameters[0], out int delevel))
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Invalid level {parameters[0]}", ChatMessageType.Broadcast));
                return;
            }
            if (delevel < 1 || delevel > Player.GetMaxLevel())
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Invalid level {delevel}", ChatMessageType.Broadcast));
                return;
            }
            if (delevel > session.Player.Level)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Delevel # must be less than current level {session.Player.Level}", ChatMessageType.Broadcast));
                return;
            }

            // get amount of unassigned xp required
            var currentLevel = session.Player.Level.Value;
            var xpBetweenLevels = (long)session.Player.GetXPBetweenLevels(delevel, currentLevel);
            var xpIntoCurrentLevel = session.Player.TotalExperience - (long)DatManager.PortalDat.XpTable.CharacterLevelXPList[currentLevel];
            var unassignedXPRequired = xpBetweenLevels + xpIntoCurrentLevel;

            session.Network.EnqueueSend(new GameMessageSystemChat($"Unassigned XP required: {unassignedXPRequired:N0}", ChatMessageType.Broadcast));

            if (session.Player.AvailableExperience < unassignedXPRequired)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"You only have {session.Player.AvailableExperience:N0} unassigned XP -- delevel failed", ChatMessageType.Broadcast));
                return;
            }

            // get # of available skill credits required
            var skillCreditsRequired = 0;
            for (var i = delevel + 1; i <= currentLevel; i++)
                skillCreditsRequired += (int)DatManager.PortalDat.XpTable.CharacterLevelSkillCreditList[i];

            session.Network.EnqueueSend(new GameMessageSystemChat($"Skill credits required: {skillCreditsRequired:N0}", ChatMessageType.Broadcast));

            if (session.Player.AvailableSkillCredits < skillCreditsRequired)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"You only have {session.Player.AvailableSkillCredits:N0} available skill credits -- delevel failed", ChatMessageType.Broadcast));
                return;
            }

            if (!confirmed)
            {
                var msg = $"Are you sure you want to delevel {session.Player.Name} to level {delevel}?";
                if (!session.Player.ConfirmationManager.EnqueueSend(new Confirmation_Custom(session.Player.Guid, () => HandleDelevel(session, true, parameters)), msg))
                    session.Player.SendWeenieError(WeenieError.ConfirmationInProgress);
                return;
            }

            session.Network.EnqueueSend(new GameMessageSystemChat($"Deleveling {session.Player.Name} to level {delevel}", ChatMessageType.Broadcast));

            var newAvailableExperience = session.Player.AvailableExperience - unassignedXPRequired;
            var newTotalExperience = session.Player.TotalExperience - unassignedXPRequired;

            var newAvailableSkillCredits = session.Player.AvailableSkillCredits - skillCreditsRequired;
            var newTotalSkillCredits = session.Player.TotalSkillCredits - skillCreditsRequired;

            session.Player.UpdateProperty(session.Player, PropertyInt64.AvailableExperience, newAvailableExperience);
            session.Player.UpdateProperty(session.Player, PropertyInt64.TotalExperience, newTotalExperience);

            session.Player.UpdateProperty(session.Player, PropertyInt.AvailableSkillCredits, newAvailableSkillCredits);
            session.Player.UpdateProperty(session.Player, PropertyInt.TotalSkillCredits, newTotalSkillCredits);

            session.Player.UpdateProperty(session.Player, PropertyInt.Level, delevel);

            PlayerManager.BroadcastToAuditChannel(session.Player, $"{session.Player.Name} has deleveled themselves from {currentLevel} to {session.Player.Level} - unassignedXPRequired: {unassignedXPRequired:N0} | skillCreditsRequired: {skillCreditsRequired:N0}");
        }

        [CommandHandler("monsterspell", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1, "The last appraised creature casts a spell. For targeted spells, defaults to the current player.", "optional target guid")]
        public static void HandleMonsterProj(Session session, params string[] parameters)
        {
            if (!Enum.TryParse(parameters[0], out SpellId spellId))
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Invalid SpellId {spellId}");
                return;
            }
            var spell = new Spell(spellId);
            if (spell.NotFound)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Couldn't find SpellId {spellId}");
                return;
            }

            var attackTarget = session.Player as Creature;

            if (parameters.Length > 1)
            {
                if (!uint.TryParse(parameters[1], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var targetGuid))
                {
                    CommandHandlerHelper.WriteOutputInfo(session, $"Invalid target guid: {parameters[1]}");
                    return;
                }

                attackTarget = session.Player.FindObject(targetGuid, Player.SearchLocations.Landblock) as Creature;

                if (attackTarget == null)
                {
                    CommandHandlerHelper.WriteOutputInfo(session, $"Couldn't find attack target {targetGuid:X8}");
                    return;
                }
            }
            var monster = CommandHandlerHelper.GetLastAppraisedObject(session) as Creature;

            if (monster == null)
                return;

            var prevAttackTarget = monster.AttackTarget;
            monster.AttackTarget = attackTarget;

            monster.CastSpell(spell);

            monster.AttackTarget = prevAttackTarget;
        }

        [CommandHandler("debugspellbook", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, "Shows the spellbook for the last appraised object")]
        public static void HandleDebugSpellbook(Session session, params string[] parameters)
        {
            var creature = CommandHandlerHelper.GetLastAppraisedObject(session) as Creature;

            if (creature == null || creature.Biota.PropertiesSpellBook == null)
                return;

            var lines = new List<string>();

            foreach (var entry in creature.Biota.PropertiesSpellBook)
                lines.Add($"{(SpellId)entry.Key} - {entry.Value}");

            CommandHandlerHelper.WriteOutputInfo(session, string.Join('\n', lines));
        }

        [CommandHandler("trywield", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 2)]
        public static void HandleTryWield(Session session, params string[] parameters)
        {
            if (!uint.TryParse(parameters[0], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var itemGuid))
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Invalid item guid {parameters[0]}", ChatMessageType.Broadcast);
                return;
            }

            var item = session.Player.FindObject(itemGuid, Player.SearchLocations.MyInventory);

            if (item == null)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Couldn't find item guid {parameters[0]}", ChatMessageType.Broadcast);
                return;
            }

            if (!Enum.TryParse(parameters[1], out EquipMask equipMask))
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Invalid EquipMask {parameters[1]}", ChatMessageType.Broadcast);
                return;
            }

            session.Player.HandleActionGetAndWieldItem(itemGuid, equipMask);
        }

        [CommandHandler("show-wielded-treasure", AccessLevel.Developer, CommandHandlerFlag.None, 1, "Shows the WieldedTreasure table for a Creature", "wcid")]
        public static void HandleShowWieldedTreasure(Session session, params string[] parameters)
        {
            if (!uint.TryParse(parameters[0], out var wcid))
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Invalid wcid {parameters[0]}", ChatMessageType.Broadcast);
                return;
            }
            var creature = WorldObjectFactory.CreateNewWorldObject(wcid) as Creature;

            if (creature == null)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Couldn't find weenie {wcid}");
                return;
            }

            if (creature.WieldedTreasure == null)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"{creature.Name} ({creature.WeenieClassId}) missing WieldedTreasure");
                return;
            }
            var table = new TreasureWieldedTable(creature.WieldedTreasure);

            foreach (var set in table.Sets)
                OutputWieldedTreasureSet(session, set);
        }

        private static void OutputWieldedTreasureSet(Session session, TreasureWieldedSet set, int depth = 0)
        {
            var prefix = new string(' ', depth * 2);

            var totalProbability = 0.0f;
            var spacer = false;

            foreach (var item in set.Items)
            {
                if (totalProbability >= 1.0f)
                {
                    totalProbability = 0.0f;
                    //spacer = true;
                }
                totalProbability += item.Item.Probability;

                var wo = WorldObjectFactory.CreateNewWorldObject(item.Item.WeenieClassId);

                var itemName = wo?.Name ?? "Unknown";

                if (spacer)
                {
                    CommandHandlerHelper.WriteOutputInfo(session, "");
                    spacer = false;
                }
                CommandHandlerHelper.WriteOutputInfo(session, $"{prefix}- {item.Item.WeenieClassId} - {itemName} ({item.Item.Probability * 100}%)");

                if (item.Subset != null)
                {
                    OutputWieldedTreasureSet(session, item.Subset, depth + 1);
                    //spacer = true;
                }
            }
        }

        private static readonly Dictionary<AetheriaColor, uint> AetheriaWcids = new Dictionary<AetheriaColor, uint>()
        {
            { AetheriaColor.Blue,   Aetheria.AetheriaBlue },
            { AetheriaColor.Yellow, Aetheria.AetheriaYellow },
            { AetheriaColor.Red,    Aetheria.AetheriaRed }
        };

        private static readonly Dictionary<Surge, SpellId> SurgeSpells = new Dictionary<Surge, SpellId>()
        {
            { Surge.Destruction,  SpellId.AetheriaProcDamageBoost },
            { Surge.Protection,   SpellId.AetheriaProcDamageReduction },
            { Surge.Regeneration, SpellId.AetheriaProcHealthOverTime },
            { Surge.Affliction,   SpellId.AetheriaProcDamageOverTime },
            { Surge.Festering,    SpellId.AetheriaProcHealDebuff },
        };

        [CommandHandler("ciaetheria", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 4, "Spawns an Aetheria in the player's inventory", "[color] [set] [surge] [level]" +
            "\nColor: Blue, Yellow, Red" +
            "\nSet: Defense, Destruction, Fury, Growth, Vigor" +
            "\nSurge: Destruction, Protection, Regeneration, Affliction, Festering" +
            "\nLevel: 1 - 5")]
        public static void HandleCIAetheria(Session session, params string[] parameters)
        {
            if (!Enum.TryParse(parameters[0], true, out AetheriaColor color))
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Invalid color: {parameters[0]}", ChatMessageType.Broadcast);
                CommandHandlerHelper.WriteOutputInfo(session, $"Available colors: Blue, Yellow, Red", ChatMessageType.Broadcast);
                return;
            }

            if (!Enum.TryParse(parameters[1], true, out Sigil set))
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Invalid set: {parameters[1]}", ChatMessageType.Broadcast);
                CommandHandlerHelper.WriteOutputInfo(session, $"Available sets: Defense, Destruction, Fury, Growth, Vigor", ChatMessageType.Broadcast);
                return;
            }

            if (!Enum.TryParse(parameters[2], true, out Surge surgeSpell))
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Invalid surge spell: {parameters[2]}", ChatMessageType.Broadcast);
                CommandHandlerHelper.WriteOutputInfo(session, $"Available surge spells: Destruction, Protection, Regeneration, Affliction, Festering", ChatMessageType.Broadcast);
                return;
            }

            if (!int.TryParse(parameters[3], out var maxLevel) || maxLevel < 1 || maxLevel > 5)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Invalid level: {parameters[3]}", ChatMessageType.Broadcast);
                CommandHandlerHelper.WriteOutputInfo(session, $"Available levels: 1 - 5", ChatMessageType.Broadcast);
                return;
            }

            var wcid = AetheriaWcids[color];

            var wo = WorldObjectFactory.CreateNewWorldObject(wcid) as Gem;

            if (wo == null)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Failed to create Aetheria wcid", ChatMessageType.Broadcast);
                return;
            }

            wo.ItemMaxLevel = maxLevel;
            wo.IconOverlayId = LootGenerationFactory.IconOverlay_ItemMaxLevel[wo.ItemMaxLevel.Value - 1];

            wo.EquipmentSetId = Aetheria.SigilToEquipmentSet[set];

            wo.IconId = Aetheria.Icons[color][set];

            var procSpell = SurgeSpells[surgeSpell];

            wo.ProcSpell = (uint)procSpell;

            if (Aetheria.SurgeTargetSelf[procSpell])
                wo.ProcSpellSelfTargeted = true;

            wo.ValidLocations = Aetheria.ColorToMask[color];

            wo.ItemTotalXp = (long)ExperienceSystem.ItemLevelToTotalXP(wo.ItemMaxLevel.Value, (ulong)wo.ItemBaseXp, wo.ItemMaxLevel.Value, wo.ItemXpStyle.Value);

            if (!session.Player.TryCreateInInventoryWithNetworking(wo))
                CommandHandlerHelper.WriteOutputInfo(session, $"Failed to add Aetheria item to player inventory", ChatMessageType.Broadcast);
        }

        [CommandHandler("vendordump", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0,
            "Lists all properties for the last vendor you examined.",
            "")]
        public static void HandleVendorDump(Session session, params string[] parameters)
        {
            var objectId = new ObjectGuid();

            if (session.Player.HealthQueryTarget.HasValue || session.Player.ManaQueryTarget.HasValue || session.Player.CurrentAppraisalTarget.HasValue)
            {
                if (session.Player.HealthQueryTarget.HasValue)
                    objectId = new ObjectGuid((uint)session.Player.HealthQueryTarget);
                else if (session.Player.ManaQueryTarget.HasValue)
                    objectId = new ObjectGuid((uint)session.Player.ManaQueryTarget);
                else
                    objectId = new ObjectGuid((uint)session.Player.CurrentAppraisalTarget);

                var wo = session.Player.CurrentLandblock?.GetObject(objectId);

                if (wo == null)
                {
                    session.Network.EnqueueSend(new GameMessageSystemChat($"Unable to find 0x{objectId:X8}", ChatMessageType.System));
                    return;
                }

                if (objectId.IsPlayer())
                {
                    session.Network.EnqueueSend(new GameMessageSystemChat($"{wo.Name} (0x{wo.Guid}) is not a vendor.", ChatMessageType.System));
                    return;
                }

                var all = false;
                var summary = false;
                var createList = false;
                var uniques = false;

                if (parameters.Length > 0)
                {
                    var args = string.Join(" ", parameters);

                    if (args.Contains("all", StringComparison.OrdinalIgnoreCase))
                        all = true;
                    if (args.Contains("summary", StringComparison.OrdinalIgnoreCase))
                        summary = true;
                    if (args.Contains("createList", StringComparison.OrdinalIgnoreCase))
                        createList = true;
                    if (args.Contains("uniques", StringComparison.OrdinalIgnoreCase))
                        uniques = true;

                    if (!all && !summary && !createList && !uniques)
                        all = true;
                }
                else
                    all = true;

                var msg = "";
                if (wo is Vendor vendor)
                {
                    var currencyWCID = vendor.AlternateCurrency ?? (uint)ACE.Entity.Enum.WeenieClassName.W_COINSTACK_CLASS;
                    var currencyWeenie = DatabaseManager.World.GetCachedWeenie(currencyWCID);

                    msg = $"Vendor Dump for {wo.Name} (0x{wo.Guid})\n";

                    if (all || summary)
                    {
                        msg += $"Vendor WCID: {wo.WeenieClassId}\n";
                        msg += $"Vendor WeenieClassName: {wo.WeenieClassName}\n";
                        msg += $"Vendor WeenieType: {wo.WeenieType}\n";
                        msg += $"OpenForBusiness: {vendor.OpenForBusiness}\n";

                        msg += $"Currency: ";
                        if (currencyWeenie != null)
                        {
                            msg += $"{currencyWeenie.GetPluralName()} (WCID: {currencyWCID}){(vendor.AlternateCurrency.HasValue ? " | AlternateCurrency" : "")}\n";
                        }
                        else
                        {
                            var errorMsg = $"WCID {currencyWCID}{(vendor.AlternateCurrency.HasValue ? ", which comes from PropertyDataId.AlternateCurrency," : "")} is not found in the database, Vendor has been disabled as a result!\n";
                            msg += errorMsg;
                        }
                        msg += $"BuyPrice: {(vendor.BuyPrice.HasValue ? $"{vendor.BuyPrice:F}" : "NULL")}\n";
                        msg += $"SellPrice: {(vendor.SellPrice.HasValue ? $"{vendor.SellPrice:F}" : "NULL")}\n";

                        msg += $"DealMagicalItems: {(vendor.DealMagicalItems.HasValue ? $"{vendor.DealMagicalItems}" : "NULL")}\n";
                        msg += $"VendorService: {(vendor.VendorService.HasValue ? $"{vendor.VendorService}" : "NULL")}\n";

                        msg += $"MerchandiseItemTypes: {(vendor.MerchandiseItemTypes.HasValue ? $"{(ItemType)vendor.MerchandiseItemTypes} ({vendor.MerchandiseItemTypes})" : "NULL")}\n";
                        msg += $"MerchandiseMinValue: {(vendor.MerchandiseMinValue.HasValue ? $"{vendor.MerchandiseMinValue}" : "NULL")}\n";
                        msg += $"MerchandiseMaxValue: {(vendor.MerchandiseMaxValue.HasValue ? $"{vendor.MerchandiseMaxValue}" : "NULL")}\n";

                        msg += $"VendorHappyMean: {(vendor.VendorHappyMean.HasValue ? $"{vendor.VendorHappyMean}" : "NULL")}\n";
                        msg += $"VendorHappyVariance: {(vendor.VendorHappyVariance.HasValue ? $"{vendor.VendorHappyVariance}" : "NULL")}\n";
                        msg += $"VendorHappyMaxItems: {(vendor.VendorHappyMaxItems.HasValue ? $"{vendor.VendorHappyMaxItems}" : "NULL")}\n";

                        msg += $"MoneyOutflow: {vendor.MoneyOutflow:N0} {(vendor.MoneyOutflow == 1 ? currencyWeenie.GetName() : currencyWeenie.GetPluralName())}\n";
                        msg += $"NumItemsBought: {vendor.NumItemsBought:N0}\n";
                        msg += $"NumItemsSold: {vendor.NumItemsSold:N0}\n";
                        msg += $"NumServicesSold: {vendor.NumServicesSold:N0}\n";
                        msg += $"MoneyIncome: {vendor.MoneyIncome:N0} {(vendor.MoneyIncome == 1 ? currencyWeenie.GetName() : currencyWeenie.GetPluralName())}\n";
                    }

                    if (all || createList)
                    {
                        var createListShop = vendor.Biota.PropertiesCreateList.Where(x => x.DestinationType == DestinationType.Shop);

                        msg += $"createListShop.Count: {createListShop.Count()}\n";
                        msg += $"===============================================\n";
                        foreach (var shopItem in createListShop)
                        {
                            var itemWeenie = DatabaseManager.World.GetCachedWeenie(shopItem.WeenieClassId);
                            if (itemWeenie == null)
                                msg += $"{shopItem.WeenieClassId} is not in the database, which will be skipped on load, and will not be sold by this vendor.\n";
                            else
                            {
                                msg += $"{itemWeenie.GetName()} ({itemWeenie.WeenieClassId} | {itemWeenie.ClassName} | {itemWeenie.WeenieType})\n";
                                if (itemWeenie.IsVendorService())
                                {
                                    var serviceSpell = itemWeenie.GetProperty(PropertyDataId.Spell);
                                    var spell = new Spell(serviceSpell ?? 0);
                                    msg += $"This is a vendor service which casts the following spell on purchaser: {(serviceSpell.HasValue ? $"{spell.Name} ({spell.Id}): {spell.Description}" : "NULL SPELL")}\n";
                                }
                                else
                                    msg += $"StackSize: {shopItem.StackSize}{(shopItem.StackSize == -1 ? " (Unlimited)" : " (per single transction)")} | PaletteTemplate: {(PaletteTemplate)shopItem.Palette} ({shopItem.Palette}) | Shade: {shopItem.Shade}\n";

                                var cost = vendor.GetSellCost(itemWeenie);
                                msg += $"Cost: {cost:N0} {(cost == 1 ? currencyWeenie.GetName() : currencyWeenie.GetPluralName())}\n";
                            }

                            msg += $"===============================================\n";
                        }
                    }

                    if (all || uniques)
                    {
                        msg += $"UniqueItemsForSale.Count: {vendor.UniqueItemsForSale.Count}\n";
                        msg += $"===============================================\n";
                        foreach (var shopItem in vendor.UniqueItemsForSale.Values)
                        {
                            msg += $"{shopItem.Name} (0x{shopItem.Guid} | {shopItem.WeenieClassId} | {shopItem.WeenieClassName} | {shopItem.WeenieType})\n";
                            msg += $"StackSize: {shopItem.StackSize ?? 1} | PaletteTemplate: {(PaletteTemplate)shopItem.PaletteTemplate} ({shopItem.PaletteTemplate}) | Shade: {shopItem.Shade:F3}\n";
                            var soldTimestamp = Time.GetDateTimeFromTimestamp(shopItem.SoldTimestamp ?? 0);
                            msg += $"SoldTimestamp: {soldTimestamp.ToLocalTime()} ({(shopItem.SoldTimestamp.HasValue ? $"{shopItem.SoldTimestamp}" : "NULL")})\n";
                            var rotTime = soldTimestamp.AddSeconds(PropertyManager.GetDouble("vendor_unique_rot_time").Item);
                            msg += $"RotTimestamp: {rotTime.ToLocalTime()}\n";
                            var payout = vendor.GetBuyCost(shopItem);
                            msg += $"Paid: {payout:N0} {(payout == 1 ? currencyWeenie.GetName() : currencyWeenie.GetPluralName())}\n";
                            var cost = vendor.GetSellCost(shopItem);
                            msg += $"Cost: {cost:N0} {(cost == 1 ? currencyWeenie.GetName() : currencyWeenie.GetPluralName())}\n";


                            msg += $"===============================================\n";
                        }
                    }
                }
                else
                    msg = $"{wo.Name} (0x{wo.Guid}) is not a vendor.";

                session.Network.EnqueueSend(new GameMessageSystemChat(msg, ChatMessageType.System));
            }
        }

        [CommandHandler("castspell", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1, "Casts a spell on the last appraised object", "spell id")]
        public static void HandleCastSpell(Session session, params string[] parameters)
        {
            if (!uint.TryParse(parameters[0], out var spellId))
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Invalid spell id {parameters[0]}");
                return;
            }

            var spell = new Spell(spellId);

            if (spell.NotFound)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Spell {spellId} not found");
                return;
            }

            var target = CommandHandlerHelper.GetLastAppraisedObject(session);

            if (target == null) return;

            session.Player.TryCastSpell(spell, target, tryResist: false);
        }

        [CommandHandler("usewith", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1, "Uses specified object on last appraised object", "guid")]
        public static void HandleUseWithTarget(Session session, params string[] parameters)
        {
            uint guid;
            if (parameters[0].StartsWith("0x"))
            {
                string hex = parameters[0][2..];
                if (!uint.TryParse(hex, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out guid))
                {
                    CommandHandlerHelper.WriteOutputInfo(session, $"Invalid guid {parameters[0]}");
                    return;
                }
            }
            else
            if (!uint.TryParse(parameters[0], out guid))
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Invalid guid {parameters[0]}");
                return;
            }

            //var spell = new Spell(spellId);

            //if (spell.NotFound)
            //{
            //    CommandHandlerHelper.WriteOutputInfo(session, $"Spell {spellId} not found");
            //    return;
            //}

            var target = CommandHandlerHelper.GetLastAppraisedObject(session);

            if (target == null) return;

            //session.Player.TryCastSpell(spell, target, tryResist: false);

            session.Player.HandleActionUseWithTarget(guid, target.Guid.Full);
        }
    }
}
