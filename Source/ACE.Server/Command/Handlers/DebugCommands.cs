using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Numerics;
using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Factories;
using ACE.Server.Managers;
using ACE.Server.Network;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Motion;
using ACE.Server.WorldObjects;
using ACE.Server.WorldObjects.Entity;

namespace ACE.Server.Command.Handlers
{
    public static class DebugCommands
    {
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
            session.Player.InWorld = true;
            session.Player.ReportCollisions = true;
            session.Player.IgnoreCollisions = false;
            session.Player.Hidden = false;
            session.Player.EnqueueBroadcastPhysicsState();
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

            UniversalMotion motion = new UniversalMotion(MotionStance.Standing, new MotionItem((MotionCommand)animationId));
            session.Player.HandleActionMotion(motion);
        }

        /// <summary>
        /// This function is just used to exercise the ability to have player movement without animation.   Once we are solid on this it can be removed.   Og II
        /// </summary>
        [CommandHandler("movement", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0, "Movement testing command, to be removed soon")]
        public static void Movement(Session session, params string[] parameters)
        {
            ushort forwardCommand = 24;

            if ((parameters?.Length > 0))
                forwardCommand = (ushort)Convert.ToInt16(parameters[0]);

            var movement = new UniversalMotion(MotionStance.Standing);
            movement.MovementData.ForwardCommand = forwardCommand;
            session.Network.EnqueueSend(new GameMessageUpdateMotion(session.Player.Guid,
                                                                    session.Player.Sequences.GetCurrentSequence(Network.Sequence.SequenceType.ObjectInstance),
                                                                    session.Player.Sequences,
                                                                    movement));
            movement = new UniversalMotion(MotionStance.Standing);
            session.Network.EnqueueSend(new GameMessageUpdateMotion(session.Player.Guid,
                                                                    session.Player.Sequences.GetCurrentSequence(Network.Sequence.SequenceType.ObjectInstance),
                                                                    session.Player.Sequences,
                                                                    movement));
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
            LootGenerationFactory.Spawn(loot, session.Player.Location.InFrontOf(distance));

            session.Player.HandleActionPutItemInContainer(loot.Guid, session.Player.Guid);
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

            foreach (Session playerSession in WorldManager.GetAll(false))
            {
                message += $"{playerSession.Player.Name} : {playerSession.Id}\n";
                playerCounter++;
            }

            message += $"Total connected Players: {playerCounter}\n";

            if (session != null)
            {
                var listPlayersMessage = new GameMessageSystemChat(message, ChatMessageType.Broadcast);
                session.Network.EnqueueSend(listPlayersMessage);
            }
            else
                Console.WriteLine(message);
        }

        /// <summary>
        /// Debug command to saves the character from in-game.
        /// </summary>
        /// <remarks>Added a quick way to invoke the character save routine.</remarks>
        [CommandHandler("save-now", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, "Saves your session.")]
        public static void HandleSaveNow(Session session, params string[] parameters)
        {
            session.SaveSessionPlayer();
        }

        /// <summary>
        /// This is a VERY crude test. It should never be used on a live server.
        /// There isn't really much point to this command other than making sure landblocks can load and are semi-efficient.
        /// </summary>
        [CommandHandler("loadalllandblocks", AccessLevel.Developer, CommandHandlerFlag.None, "Loads all Landblocks. This is VERY crude. Do NOT use it on a live server!!! It will likely crash the server.")]
        public static void HandleLoadAllLandblocks(Session session, params string[] parameters)
        {
            session.Network.EnqueueSend(new GameMessageSystemChat("Loading landblocks... This will likely crash the server...", ChatMessageType.System));

            //Task.Run(() => // Using Task.Run() seems to halt around the 0x01E# block range.
            {
                for (int x = 0; x <= 0xFE; x++)
                {
                    for (int y = 0; y <= 0xFE; y++)
                        LandblockManager.ForceLoadLandBlock(new LandblockId((byte)x, (byte)y));
                }
            }//);
        }

        [CommandHandler("cacheallweenies", AccessLevel.Developer, CommandHandlerFlag.None, "Loads and caches all Weenies. This may take 10+ minutes and is very heavy on the database.")]
        public static void HandleCacheAllWeenies(Session session, params string[] parameters)
        {
            session.Network.EnqueueSend(new GameMessageSystemChat("Caching Weenies... This may take more than 10 minutes...", ChatMessageType.System));

            Task.Run(() => DatabaseManager.World.CacheAllWeenies());
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
                            var posFlag = (UpdatePositionFlag)Convert.ToUInt32(parameters[1]);

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

                    if (session == null)
                        Console.WriteLine(debugOutput.Replace(", ", " | "));
                    else
                        session.Network.EnqueueSend(new GameMessageSystemChat(debugOutput, ChatMessageType.System));
                }
            }
            catch (Exception)
            {
                string debugOutput = "Exception Error, check input and try again";

                if (session == null)
                    Console.WriteLine(debugOutput.Replace(", ", " | "));
                else
                    session.Network.EnqueueSend(new GameMessageSystemChat(debugOutput, ChatMessageType.System));
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
            var posDict = session.Player.Positions;
            string message = "Saved character positions:\n";

            foreach (var posPair in posDict)
                message += "ID: " + (uint)posPair.Key + " Loc: " + posPair.Value + "\n";

            message += "Total positions: " + posDict.Count + "\n";
            var positionMessage = new GameMessageSystemChat(message, ChatMessageType.Broadcast);
            session.Network.EnqueueSend(positionMessage);
        }

        /// <summary>
        /// Debug command to save the player's current location as sepecific position type.
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
                        Position playerPosition = (Position)session.Player.Location.Clone();

                        // Save the position
                        session.Player.SetCharacterPosition(positionType, playerPosition);

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
            foreach(CharacterTitle title in Enum.GetValues(typeof(CharacterTitle)))
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
                        Type = CommandParameterHelpers.ACECommandParameterType.ULong,
                        Required = true,
                        ErrorMessage = "You must specify the amount of xp."
                    }
                };
                if (CommandParameterHelpers.ResolveACEParameters(session, parameters, aceParams))
                {
                    try
                    {
                        var xp = (long)aceParams[1].AsULong;
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


        // ==================================
        // Vitals
        // ==================================

        [CommandHandler("setvital", AccessLevel.Developer, CommandHandlerFlag.None, 2,
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

        private static void AddWeeniesToInventory(Session session, HashSet<uint> weenieIds)
        {
            foreach (uint weenieId in weenieIds)
            {
                var loot = WorldObjectFactory.CreateNewWorldObject(weenieId);

                if (loot == null) // weenie doesn't exist
                    continue;

                if (loot.MaxStackSize > 1)
                {
                    loot.StackSize = loot.MaxStackSize;
                    loot.EncumbranceVal = (loot.StackUnitEncumbrance ?? 0) * (loot.StackSize ?? 1);
                    loot.Value = (loot.StackUnitValue ?? 0) * (loot.StackSize ?? 1);
                }

                session.Player.TryCreateInInventoryWithNetworking(loot);
            }
        }

        [CommandHandler("weapons", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0, "Creates testing items in your inventory.")]
        public static void HandleWeapons(Session session, params string[] parameters)
        {
            HashSet<uint> weenieIds = new HashSet<uint> { 93, 127, 130, 148, 300, 307, 311, 326, 338, 348, 350, 7765, 12748, 12463, 31812 };

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
            HashSet<uint> weenieIds = new HashSet<uint> { 20631, 686, 687, 688, 689, 690, 691, 7299, 8897 };

            AddWeeniesToInventory(session, weenieIds);
        }

        [CommandHandler("food", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0, "Creates some food items in your inventory for testing.")]
        public static void HandleFood(Session session, params string[] parameters)
        {
            HashSet<uint> weenieIds = new HashSet<uint> { 259, 259, 260, 377,  378,  379 };

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
                    weenieTypeNumber = (uint) Enum.Parse(typeof(WeenieType), weenieTypeName);
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
            foreach (WorldObject.SpellLevel powerLevel in Enum.GetValues(typeof(WorldObject.SpellLevel)))
            {
                session.Player.LearnSpellsInBulk((uint)MagicSchool.CreatureEnchantment, (uint)powerLevel);
                session.Player.LearnSpellsInBulk((uint)MagicSchool.ItemEnchantment, (uint)powerLevel);
                session.Player.LearnSpellsInBulk((uint)MagicSchool.LifeMagic, (uint)powerLevel);
                session.Player.LearnSpellsInBulk((uint)MagicSchool.VoidMagic, (uint)powerLevel);
                session.Player.LearnSpellsInBulk((uint)MagicSchool.WarMagic, (uint)powerLevel);
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
            Console.WriteLine("Spell Words: " + spellTable.Spells[spellid].SpellWords);
            Console.WriteLine(spellTable.Spells[spellid].Desc);

            var formula = SpellTable.GetSpellFormula(DatManager.PortalDat.SpellTable, spellid, parameters[0]);

            for (int i = 0; i < formula.Count; i++)
            {
                if (comps.SpellComponents.ContainsKey(formula[i])) {
                    Console.WriteLine("Comp " + i + ": " + comps.SpellComponents[formula[i]].Name);
                }
                else
                {
                    Console.WriteLine("Comp " + i + " : Unknown Component " + formula[i].ToString());
                }
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
                Console.WriteLine("Formula for " + spellTable.Spells[spellid].Name + " (" + spellid.ToString() + ")");
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
            int total = 0;
            uint min = 0x0E010000;
            uint max = 0x0E01FFFF;

            var test = DatManager.PortalDat.SkillTable;
            return;
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

        [CommandHandler("turnto", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1, "Turns the input object to the player", "turnto <object_id>")]
        public static void HandleRequestTurnTo(Session session, params string[] parameters)
        {
            if (parameters.Length < 1) return;

            var objectID = Convert.ToUInt32(parameters[0], 16);
            var guid = new ObjectGuid(objectID);
            var player = session.Player;

            var obj = player.CurrentLandblock?.GetObject(guid);
            if (obj == null)
            {
                Console.WriteLine("Couldn't find " + guid);
                return;
            }

            var creature = obj as Creature;
            creature.TurnTo(player);
        }

        [CommandHandler("debugmove", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0, "Toggles movement debugging for the last appraised monster", "debugmove <on/off>")]
        public static void ToggleMovementDebug(Session session, params string[] parameters)
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

            Console.WriteLine("Setting forcepos to " + enabled);

            Creature.ForcePos = enabled;
        }

        [CommandHandler("debugemote", AccessLevel.Developer, CommandHandlerFlag.None, 0, "Debugs a hardcoded emote for the last appraised object", "debugemote")]
        public static void HandleDebugEmote(Session session, params string[] parameters)
        {
            // get the wo emotemanager for the last appraised object
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
                Console.WriteLine("ERROR: couldn't find " + targetGuid);
                return;
            }
            var actionChain = new ActionChain();

            // build the emote
            var emote = new BiotaPropertiesEmote();

            var action = new BiotaPropertiesEmoteAction();
            action.Type = (uint)EmoteType.MoveToPos;

            // get current position
            var currentPos = target.Location;

            var newPos = new Position();
            newPos.LandblockId = new LandblockId(currentPos.Cell);
            newPos.Pos = new Vector3(currentPos.PositionX - 10, currentPos.PositionY, currentPos.PositionZ);
            action.OriginX = newPos.PositionX;
            action.OriginY = newPos.PositionY;
            action.OriginZ = newPos.PositionZ;
            action.ObjCellId = newPos.Cell;

            Console.WriteLine($"Moving {target.Name} from {target.Location.LandblockId} {currentPos.Pos} to {newPos.LandblockId} {newPos.Pos}");

            target.EmoteManager.ExecuteEmote(emote, action, actionChain, target, target);
        }
    }
}
