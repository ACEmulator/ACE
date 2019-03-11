using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Numerics;

using log4net;

using ACE.Common;
using ACE.Database;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Factories;
using ACE.Server.Managers;
using ACE.Server.Network;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.WorldObjects;
using ACE.Server.WorldObjects.Entity;

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
            if (session.Player.AdjustDungeonCells(pos))
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
        [CommandHandler("fixbusy", AccessLevel.Player, CommandHandlerFlag.RequiresWorld, 0, "Attempts to remove the hourglass / fix the busy state for the player", "/fixbusy")]
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
                    modelId = UInt32.Parse(strippedmodelid, System.Globalization.NumberStyles.HexNumber);
                }
                else
                    modelId = UInt32.Parse(parameters[0], System.Globalization.NumberStyles.HexNumber);

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

        /// <summary>
        /// Force PhysicsState change that occurs upon login complete.
        /// </summary>
        [CommandHandler("fakelogin", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, "Fake Login Complete response")]
        public static void HandleFakeLogin(Session session, params string[] parameters)
        {
            session.Player.ReportCollisions = true;
            session.Player.IgnoreCollisions = false;
            session.Player.Hidden = false;
            session.Player.EnqueueBroadcastPhysicsState();
        }

        [CommandHandler("netstats", AccessLevel.Developer, CommandHandlerFlag.None, "View network statistics")]
        public static void HandleNetStats(Session session, params string[] parameters)
        {
            CommandHandlerHelper.WriteOutputInfo(session, NetworkStatistics.Summary(), ChatMessageType.Broadcast);
        }

        [CommandHandler("trash_c2s", AccessLevel.Developer, CommandHandlerFlag.None, "Trash (corrupt) the next C2S packet that arrives.")]
        public static void HandleTrashNextPacketC2S(Session session, params string[] parameters)
        {
            CommandHandlerHelper.WriteOutputInfo(session, "The next C2S packet will be synthetically corrupted.", ChatMessageType.Broadcast);
            NetworkSyntheticTesting.TrashNextPacketC2S = true;
        }

        [CommandHandler("junk_c2s", AccessLevel.Developer, CommandHandlerFlag.None, "Toggle synthetically junky C2S connection of a 10% payload corruption rate.")]
        public static void HandleJunkC2S(Session session, params string[] parameters)
        {
            NetworkSyntheticTesting.JunkyConnectionC2S = !NetworkSyntheticTesting.JunkyConnectionC2S;
            var endis = (NetworkSyntheticTesting.JunkyConnectionC2S) ? "enabled" : "disabled";
            CommandHandlerHelper.WriteOutputInfo(session, $"Junky C2S connection {endis}.", ChatMessageType.Broadcast);
        }

        [CommandHandler("trash_s2c", AccessLevel.Developer, CommandHandlerFlag.None, "Trash (corrupt) the next S2C packet that is sent.")]
        public static void HandleTrashNextPacketS2C(Session session, params string[] parameters)
        {
            CommandHandlerHelper.WriteOutputInfo(session, "The next S2C packet will be synthetically corrupted.", ChatMessageType.Broadcast);
            NetworkSyntheticTesting.TrashNextPacketS2C = true;
        }

        [CommandHandler("junk_s2c", AccessLevel.Developer, CommandHandlerFlag.None, "Toggle synthetically junky S2C connection of a 10% payload corruption rate.")]
        public static void HandleJunkS2C(Session session, params string[] parameters)
        {
            NetworkSyntheticTesting.JunkyConnectionS2C = !NetworkSyntheticTesting.JunkyConnectionS2C;
            var endis = (NetworkSyntheticTesting.JunkyConnectionS2C) ? "enabled" : "disabled";
            CommandHandlerHelper.WriteOutputInfo(session, $"Junky S2C connection {endis}.", ChatMessageType.Broadcast);
        }


        [CommandHandler("junk", AccessLevel.Developer, CommandHandlerFlag.None, "Toggle synthetically junky S2C and C2S connections of a 10% payload corruption rate.")]
        public static void HandleJunk(Session session, params string[] parameters)
        {
            HandleJunkC2S(session, parameters);
            HandleJunkS2C(session, parameters);
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
        [CommandHandler("playsound", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, "Plays a sound.", "sound (float)\n" + "Sound can be uint or enum name" + "float is volume level")]
        public static void HandlePlaySound(Session session, params string[] parameters)
        {
            try
            {
                float volume = 1f;
                var soundEvent = new GameMessageSound(session.Player.Guid, Sound.Invalid, volume);

                if (parameters.Length > 1)
                {
                    if (parameters[1] != "")
                        volume = float.Parse(parameters[1]);
                }

                var message = $"Unable to find a sound called {parameters[0]} to play.";

                if (Enum.TryParse(parameters[0], true, out Sound sound))
                {
                    if (Enum.IsDefined(typeof(Sound), sound))
                    {
                        message = $"Playing sound {Enum.GetName(typeof(Sound), sound)}";
                        // add the sound to the player queue for everyone to hear
                        // player action queue items will execute on the landblock
                        // player.playsound will play a sound on only the client session that called the function
                        session.Player.HandleActionApplySoundEffect(sound);
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

        [CommandHandler("animation", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1, "Sends a MovementEvent to you.", "uint\n")]
        public static void Animation(Session session, params string[] parameters)
        {
            uint animationId;

            try
            {
                animationId = Convert.ToUInt32(parameters[0]);
            }
            catch (Exception)
            {
                ChatPacket.SendServerMessage(session, "Invalid Animation value", ChatMessageType.Broadcast);
                return;
            }

            session.Player.EnqueueBroadcastMotion(new Motion(session.Player, (MotionCommand)animationId));
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
        [CommandHandler("barbershop", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld)]
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

            foreach (var player in PlayerManager.GetAllOnline())
            {
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

        [CommandHandler("cacheallweenies", AccessLevel.Developer, CommandHandlerFlag.None, "Loads and caches all Weenies. This may take 15+ minutes and is very heavy on the database.")]
        public static void HandleCacheAllWeenies(Session session, params string[] parameters)
        {
            CommandHandlerHelper.WriteOutputInfo(session, "Caching Weenies... This may take more than 15 minutes...");

            Task.Run(() => DatabaseManager.World.CacheAllWeenies());
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
            ChatPacket.SendServerMessage(session, $"GUID: {session.Player.Guid.Full} ID(low): {session.Player.Guid.Low} High:{session.Player.Guid.High}", ChatMessageType.Broadcast);
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

                if (Enum.TryParse(parsePositionString, out PositionType positionType))
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
        [CommandHandler("listpositions", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0, "Displays all available saved character positions from the database.", "@listpositions")]
        public static void HandleListPositions(Session session, params string[] parameters)
        {
            var posDict = session.Player.GetPositions();
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

                if (Enum.TryParse(parsePositionString, out PositionType positionType))
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
                        Type = CommandParameterHelpers.ACECommandParameterType.Player,
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
                        var xp = (long)aceParams[1].AsLong;
                        aceParams[0].AsPlayer.GrantXP(xp);
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

            if (!relValue)
                session.Player.UpdateVital(vital, (uint)value);
            else
                session.Player.UpdateVitalDelta(vital, value);
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
        [CommandHandler("harmself", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld)]
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

        [CommandHandler("cirand", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1, "Creates random objects in your inventory.", "type (string or number) <num to create> defaults to 10 if omitted max 50")]
        public static void HandleCIRandom(Session session, params string[] parameters)
        {
            string weenieTypeName = parameters[0];
            bool isNumericType = uint.TryParse(weenieTypeName, out uint weenieTypeNumber);

            if (!isNumericType)
            {
                try
                {
                    weenieTypeNumber = (uint)Enum.Parse(typeof(WeenieType), weenieTypeName);
                }
                catch
                {
                    ChatPacket.SendServerMessage(session, "Not a valid type name", ChatMessageType.Broadcast);
                    return;
                }
            }

            if (weenieTypeNumber == 0)
            {
                ChatPacket.SendServerMessage(session, "Not a valid type id - must be a number between 0 - 2,147,483,647", ChatMessageType.Broadcast);
                return;
            }

            byte numItems = 10;

            if (parameters.Length == 2)
            {
                try
                {
                    numItems = Convert.ToByte(parameters[1]);

                    if (numItems < 1) numItems = 1;
                    if (numItems > 50) numItems = 50;
                }
                catch (Exception)
                {
                    ChatPacket.SendServerMessage(session, "Not a valid number - must be a number between 1 - 50", ChatMessageType.Broadcast);
                    return;
                }
            }

            var items = LootGenerationFactory.CreateRandomObjectsOfType((WeenieType)weenieTypeNumber, numItems);

            foreach (var item in items)
                session.Player.TryCreateInInventoryWithNetworking(item);
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

        [CommandHandler("contract", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1, "Send a contract to yourself.", "uint\n" + "@sendcontract 100 is a sample contract")]
        public static void HandleContract(Session session, params string[] parameters)
        {
            if (!(parameters?.Length > 0)) return;
            if (!uint.TryParse(parameters[0], out var contractId)) return;

            ContractTracker contractTracker = new ContractTracker(contractId, session.Player.Guid.Full)
            {
                Stage = 0,
                TimeWhenDone = 0,
                TimeWhenRepeats = 0,
                DeleteContract = 0,
                SetAsDisplayContract = 1
            };

            if (!session.Player.TrackedContracts.ContainsKey(contractId))
                session.Player.TrackedContracts.Add(contractId, contractTracker);

            GameEventSendClientContractTracker contractMsg = new GameEventSendClientContractTracker(session, contractTracker);
            session.Network.EnqueueSend(contractMsg);
            ChatPacket.SendServerMessage(session, "You just added " + contractTracker.ContractDetails.ContractName, ChatMessageType.Broadcast);
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

        [CommandHandler("debugmove", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0, "Toggles movement debugging for the last appraised monster", "debugmove <on/off>")]
        public static void ToggleMovementDebug(Session session, params string[] parameters)
        {
            // get the last appraised object
            var targetID = session.Player.CurrentAppraisalTarget;
            if (targetID == null)
            {
                ChatPacket.SendServerMessage(session, "ERROR: no appraisal target", ChatMessageType.System);
                return;
            }
            var targetGuid = new ObjectGuid(targetID.Value);
            var target = session.Player.CurrentLandblock?.GetObject(targetGuid);
            if (target == null)
            {
                ChatPacket.SendServerMessage(session, "Couldn't find " + targetGuid, ChatMessageType.System);
                return;
            }
            var creature = target as Creature;
            if (creature == null)
            {
                ChatPacket.SendServerMessage(session, target.Name + " is not a creature / monster", ChatMessageType.System);
                return;
            }

            bool enabled = true;
            if (parameters.Length > 0 && parameters[0].Equals("off"))
                enabled = false;

            creature.DebugMove = enabled;
        }

        [CommandHandler("forcepos", AccessLevel.Developer, CommandHandlerFlag.None, 0, "Toggles server monster position", "forcepos <on/off>")]
        public static void ToggleForcePos(Session session, params string[] parameters)
        {
            bool enabled = true;
            if (parameters.Length > 0 && parameters[0].Equals("off"))
                enabled = false;

            CommandHandlerHelper.WriteOutputInfo(session, "Setting forcepos to " + enabled);

            Creature.ForcePos = enabled;
        }

        [CommandHandler("lostest", AccessLevel.Developer, CommandHandlerFlag.None, 0, "Tests for direct visibilty with latest appraised object", "lostest")]
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

        [CommandHandler("showstats", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0, "Shows a list of player's current attribute/skill levels in console window", "showstats")]
        public static void HandleShowStats(Session session, params string[] parameters)
        {
            var player = session.Player;

            Console.WriteLine("Strength: " + player.Strength.Current);
            Console.WriteLine("Endurance: " + player.Endurance.Current);
            Console.WriteLine("Coordination: " + player.Coordination.Current);
            Console.WriteLine("Quickness: " + player.Quickness.Current);
            Console.WriteLine("Focus: " + player.Focus.Current);
            Console.WriteLine("Self: " + player.Self.Current);

            Console.WriteLine();

            Console.WriteLine("Health: " + player.Health.Current + "/" + player.Health.MaxValue);
            Console.WriteLine("Stamina: " + player.Stamina.Current + "/" + player.Stamina.MaxValue);
            Console.WriteLine("Mana: " + player.Mana.Current + "/" + player.Mana.MaxValue);

            Console.WriteLine();

            var specialized = player.Skills.Values.Where(s => s.AdvancementClass == SkillAdvancementClass.Specialized).OrderBy(s => s.Skill.ToString());
            var trained = player.Skills.Values.Where(s => s.AdvancementClass == SkillAdvancementClass.Trained).OrderBy(s => s.Skill.ToString());
            var untrained = player.Skills.Values.Where(s => s.AdvancementClass == SkillAdvancementClass.Untrained && s.IsUsable).OrderBy(s => s.Skill.ToString());
            var unusable = player.Skills.Values.Where(s => s.AdvancementClass == SkillAdvancementClass.Untrained && !s.IsUsable).OrderBy(s => s.Skill.ToString());

            foreach (var skill in specialized)
                Console.WriteLine(skill.Skill + ": " + skill.Current);
            Console.WriteLine("===");

            foreach (var skill in trained)
                Console.WriteLine(skill.Skill + ": " + skill.Current);
            Console.WriteLine("===");

            foreach (var skill in untrained)
                Console.WriteLine(skill.Skill + ": " + skill.Current);
            Console.WriteLine("===");

            foreach (var skill in unusable)
                Console.WriteLine(skill.Skill + ": " + skill.Current);
        }

        [CommandHandler("givemana", AccessLevel.Developer, CommandHandlerFlag.None, 0, "Gives mana to the last appraised object", "givemana <amount>")]
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
            if (obj == null) return;

            var sourcePos = session.Player.Location.ToGlobal();
            var targetPos = obj.Location.ToGlobal();

            var dist = Vector3.Distance(sourcePos, targetPos);
            var dist2d = Vector2.Distance(new Vector2(sourcePos.X, sourcePos.Y), new Vector2(targetPos.X, targetPos.Y));

            Console.WriteLine("Dist: " + dist);
            Console.WriteLine("2D Dist: " + dist2d);
        }

        /// <summary>
        /// Teleport object culling precision test
        /// </summary>
        [CommandHandler("teledist", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1, "Teleports a some distance ahead of the last object spawned", "/teletest <distance>")]
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

        /// <summary>
        /// Shows the list of objects known to this player
        /// </summary>
        [CommandHandler("knownobjs", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0, "Shows the list of objects known to this player", "/knownobjs")]
        public static void HandleKnownObjs(Session session, params string[] parameters)
        {
            Console.WriteLine($"\nKnown objects to {session.Player.Name}: {session.Player.PhysicsObj.ObjMaint.ObjectTable.Count}");

            foreach (var obj in session.Player.PhysicsObj.ObjMaint.ObjectTable.Values)
                Console.WriteLine($"{obj.Name} ({obj.ID:X8})");
        }

        /// <summary>
        /// Shows the list of objects visible to this player
        /// </summary>
        [CommandHandler("visibleobjs", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0, "Shows the list of objects known to this player", "/visibleobjs")]
        public static void HandleVisibleObjs(Session session, params string[] parameters)
        {
            Console.WriteLine($"\nVisible objects to {session.Player.Name}: {session.Player.PhysicsObj.ObjMaint.VisibleObjectTable.Count}");

            foreach (var obj in session.Player.PhysicsObj.ObjMaint.VisibleObjectTable.Values)
                Console.WriteLine($"{obj.Name} ({obj.ID:X8})");
        }

        /// <summary>
        /// Shows the list of players known to this player
        /// </summary>
        [CommandHandler("knownplayers", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0, "Shows the list of players known to this player", "/knownplayers")]
        public static void HandleKnownPlayers(Session session, params string[] parameters)
        {
            Console.WriteLine($"\nKnown players to {session.Player.Name}: {session.Player.PhysicsObj.ObjMaint.ObjectTable.Values.Where(o => o.IsPlayer).Count()}");

            foreach (var obj in session.Player.PhysicsObj.ObjMaint.ObjectTable.Values.Where(o => o.IsPlayer))
                Console.WriteLine($"{obj.Name} ({obj.ID:X8})");
        }

        /// <summary>
        /// Shows the list of players visible to this player
        /// </summary>
        [CommandHandler("visibleplayers", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0, "Shows the list of players visible to this player", "/visibleplayers")]
        public static void HandleVisiblePlayers(Session session, params string[] parameters)
        {
            Console.WriteLine($"\nVisible players to {session.Player.Name}: {session.Player.PhysicsObj.ObjMaint.VisibleObjectTable.Values.Where(o => o.IsPlayer).Count()}");

            foreach (var obj in session.Player.PhysicsObj.ObjMaint.VisibleObjectTable.Values.Where(o => o.IsPlayer))
                Console.WriteLine($"{obj.Name} ({obj.ID:X8})");
        }

        /// <summary>
        /// Shows the list of voyeurs for this player
        /// </summary>
        [CommandHandler("voyeurs", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0, "Shows the list of voyeurs for this player", "/voyeurs")]
        public static void HandleVoyeurs(Session session, params string[] parameters)
        {
            Console.WriteLine($"\nVoyeurs for {session.Player.Name}: {session.Player.PhysicsObj.ObjMaint.VoyeurTable.Values.Where(o => o.IsPlayer).Count()}");

            foreach (var obj in session.Player.PhysicsObj.ObjMaint.VoyeurTable.Values.Where(o => o.IsPlayer))
                Console.WriteLine($"{obj.Name} ({obj.ID:X8})");
        }

        /// <summary>
        /// Shows the list of previously visible objects queued for destruction for this player
        /// </summary>
        [CommandHandler("destructionqueue", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0, "Shows the list of previously visible objects queued for destruction for this player", "/destructionqueue")]
        public static void HandleDestructionQueue(Session session, params string[] parameters)
        {
            Console.WriteLine($"\nDestruction queue for {session.Player.Name}: {session.Player.PhysicsObj.ObjMaint.DestructionQueue.Count}");

            var currentTime = Physics.Common.PhysicsTimer.CurrentTime;

            foreach (var obj in session.Player.PhysicsObj.ObjMaint.DestructionQueue)
                Console.WriteLine($"{obj.Key.Name} ({obj.Key.ID:X8}): {obj.Value - currentTime}");
        }

        /// <summary>
        /// Enables emote debugging for the last appraised object
        /// </summary>
        [CommandHandler("debugemote", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0, "Enables emote debugging for the last appraised object", "/debugemote")]
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
        /// Enables / disables spell component burning
        /// </summary>
        [CommandHandler("safecomps", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0, "Enables / disables spell component burning", "/safecomps <on/off>")]
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
        /// Shows the current player location, from the server perspective
        /// </summary>
        [CommandHandler("myloc", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0, "Shows the current player location, from the server perspective", "/myloc")]
        public static void HandleMyLoc(Session session, params string[] parameters)
        {
            session.Network.EnqueueSend(new GameMessageSystemChat($"Location: {session.Player.PhysicsObj.Position}", ChatMessageType.Broadcast));
        }

        /// <summary>
        /// Gets a property for the last appraised object
        /// </summary>
        [CommandHandler("getproperty", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1, "Gets a property for the last appraised object", "/getproperty <property>")]
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

            if (!Enum.TryParse(pType, propName, out var result))
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
        [CommandHandler("setproperty", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 2, "Sets a property for the last appraised object", "/setproperty <property> <value>")]
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

            if (!Enum.TryParse(pType, propName, out var result))
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
                        obj.SetProperty((PropertyInt)result, Convert.ToInt32(value));
                        obj.EnqueueBroadcast(new GameMessagePublicUpdatePropertyInt(obj, (PropertyInt)result, Convert.ToInt32(value)));
                    }
                    else if (propType.Equals("PropertyInt64", StringComparison.OrdinalIgnoreCase))
                    {
                        obj.SetProperty((PropertyInt64)result, Convert.ToInt64(value));
                        obj.EnqueueBroadcast(new GameMessagePublicUpdatePropertyInt64(obj, (PropertyInt64)result, Convert.ToInt64(value)));
                    }
                    else if (propType.Equals("PropertyBool", StringComparison.OrdinalIgnoreCase))
                    {
                        obj.SetProperty((PropertyBool)result, Convert.ToBoolean(value));
                        obj.EnqueueBroadcast(new GameMessagePublicUpdatePropertyBool(obj, (PropertyBool)result, Convert.ToBoolean(value)));
                    }
                    else if (propType.Equals("PropertyFloat", StringComparison.OrdinalIgnoreCase))
                    {
                        obj.SetProperty((PropertyFloat)result, Convert.ToDouble(value));
                        obj.EnqueueBroadcast(new GameMessagePublicUpdatePropertyFloat(obj, (PropertyFloat)result, Convert.ToDouble(value)));
                    }
                    else if (propType.Equals("PropertyString", StringComparison.OrdinalIgnoreCase))
                    {
                        obj.SetProperty((PropertyString)result, value);
                        obj.EnqueueBroadcast(new GameMessagePublicUpdatePropertyString(obj, (PropertyString)result, value));
                    }
                    else if (propType.Equals("PropertyInstanceId", StringComparison.OrdinalIgnoreCase))
                    {
                        obj.SetProperty((PropertyInstanceId)result, Convert.ToUInt32(value));
                        obj.EnqueueBroadcast(new GameMessagePublicUpdateInstanceID(obj, (PropertyInstanceId)result, new ObjectGuid(Convert.ToUInt32(value))));
                    }
                    else if (propType.Equals("PropertyDataId", StringComparison.OrdinalIgnoreCase))
                    {
                        obj.SetProperty((PropertyDataId)result, Convert.ToUInt32(value));
                        obj.EnqueueBroadcast(new GameMessagePublicUpdatePropertyDataID(obj, (PropertyDataId)result, Convert.ToUInt32(value)));
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return;
                }
            }
            session.Network.EnqueueSend(new GameMessageSystemChat($"{obj.Name} ({obj.Guid}): {prop} = {value}", ChatMessageType.Broadcast));
        }

        /// <summary>
        /// Sets the house purchase time for this player
        /// </summary>
        [CommandHandler("setpurchasetime", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0, "Sets the house purchase time for this player", "/setpurchasetime")]
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
        [CommandHandler("debugdamage", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0, "Toggles the display for player damage info", "/debugdamage <attack|defense|all|on|off>")]
        public static void HandleDebugDamage(Session session, params string[] parameters)
        {
            if (parameters.Length == 0)
            {
                // toggle
                if (session.Player.DebugDamage == Player.DebugDamageType.None)
                    session.Player.DebugDamage = Player.DebugDamageType.All;
                else
                    session.Player.DebugDamage = Player.DebugDamageType.None;
            }
            else
            {
                var param = parameters[0].ToLower();
                if (param.Equals("on") || param.Equals("all"))
                    session.Player.DebugDamage = Player.DebugDamageType.All;
                else if (param.Equals("off"))
                    session.Player.DebugDamage = Player.DebugDamageType.None;
                else if (param.StartsWith("attack"))
                    session.Player.DebugDamage |= Player.DebugDamageType.Attacker;
                else if (param.StartsWith("defen"))
                    session.Player.DebugDamage |= Player.DebugDamageType.Defender;
                else
                {
                    session.Network.EnqueueSend(new GameMessageSystemChat($"DebugDamage: unknown {param}", ChatMessageType.Broadcast));
                    return;
                }
            }
            session.Network.EnqueueSend(new GameMessageSystemChat($"DebugDamage: {session.Player.DebugDamage}", ChatMessageType.Broadcast));
        }
    }
}
