using System;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Actions;
using ACE.Managers;
using ACE.Network;
using ACE.Network.GameMessages.Messages;
using ACE.Network.GameEvent.Events;
using ACE.Factories;
using System.Globalization;
using ACE.Network.Motion;
using ACE.DatLoader.FileTypes;
using System.Linq;
using System.Collections.Generic;
using ACE.Database;
using ACE.Entity.Enum.Properties;

namespace ACE.Command.Handlers
{
    internal enum TestWeenieClassIds : uint
    {
        Pants        = 120,
        Tunic        = 134,
        TrainingWand = 12748,
        ColoBackpack = 36561
    }

    public static class DebugCommands
    {
        // echo "text to send back to yourself" [ChatMessageType]
        [CommandHandler("echo", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld,
            "Send text back to yourself.",
            "\"text to send back to yourself\" [ChatMessageType]\n" +
            "ChatMessageType can be a uint or enum name")]
        public static void HandleDebugEcho(Session session, params string[] parameters)
        {
            try
            {
                ChatMessageType cmt = ChatMessageType.Broadcast;
                if (Enum.TryParse(parameters[1], true, out cmt))
                    if (Enum.IsDefined(typeof(ChatMessageType), cmt))
                        ChatPacket.SendServerMessage(session, parameters[0], cmt);
                    else
                        ChatPacket.SendServerMessage(session, parameters[0], ChatMessageType.Broadcast);
            }
            catch (Exception)
            {
                ChatPacket.SendServerMessage(session, parameters[0], ChatMessageType.Broadcast);
            }
        }

        // echoflags [flagtype] [int]
        [CommandHandler("echoflags", AccessLevel.Developer, CommandHandlerFlag.None, 2,
            "Echo flags back to you",
            "[type to test] [int]\n")]
        public static void HandleDebugEchoFlags(Session session, params string[] parameters)
        {
            try
            {
                string debugOutput = "";
                if (parameters?.Length == 2)
                {
                    switch (parameters[0].ToLower())
                    {
                        case "descriptionflags":
                            ObjectDescriptionFlag objectDescFlag = new ObjectDescriptionFlag();
                            objectDescFlag = (ObjectDescriptionFlag)Convert.ToUInt32(parameters[1]);

                            debugOutput = $"{objectDescFlag.GetType().Name} = {objectDescFlag.ToString()}" + " (" + (uint)objectDescFlag + ")";
                            break;
                        case "weenieflags":
                            WeenieHeaderFlag weenieHdr = new WeenieHeaderFlag();
                            weenieHdr = (WeenieHeaderFlag)Convert.ToUInt32(parameters[1]);

                            debugOutput = $"{weenieHdr.GetType().Name} = {weenieHdr.ToString()}" + " (" + (uint)weenieHdr + ")";
                            break;
                        case "weenieflags2":
                            WeenieHeaderFlag2 weenieHdr2 = new WeenieHeaderFlag2();
                            weenieHdr2 = (WeenieHeaderFlag2)Convert.ToUInt32(parameters[1]);

                            debugOutput = $"{weenieHdr2.GetType().Name} = {weenieHdr2.ToString()}" + " (" + (uint)weenieHdr2 + ")";
                            break;
                        case "positionflag":
                            UpdatePositionFlag posFlag = new UpdatePositionFlag();
                            posFlag = (UpdatePositionFlag)Convert.ToUInt32(parameters[1]);

                            debugOutput = $"{posFlag.GetType().Name} = {posFlag.ToString()}" + " (" + (uint)posFlag + ")";
                            break;
                        case "type":
                            ItemType objectType = new ItemType();
                            objectType = (ItemType)Convert.ToUInt32(parameters[1]);

                            debugOutput = $"{objectType.GetType().Name} = {objectType.ToString()}" + " (" + (uint)objectType + ")";
                            break;
                        case "containertype":
                            ContainerType contType = new ContainerType();
                            contType = (ContainerType)Convert.ToUInt32(parameters[1]);

                            debugOutput = $"{contType.GetType().Name} = {contType.ToString()}" + " (" + (uint)contType + ")";
                            break;
                        case "usable":
                            Usable usableType = new Usable();
                            usableType = (Usable)Convert.ToInt64(parameters[1]);

                            debugOutput = $"{usableType.GetType().Name} = {usableType.ToString()}" + " (" + (Int64)usableType + ")";
                            break;
                        case "radarbehavior":
                            RadarBehavior radarBeh = new RadarBehavior();
                            radarBeh = (RadarBehavior)Convert.ToUInt32(parameters[1]);

                            debugOutput = $"{radarBeh.GetType().Name} = {radarBeh.ToString()}" + " (" + (uint)radarBeh + ")";
                            break;
                        case "physicsdescriptionflags":
                            PhysicsDescriptionFlag physicsDescFlag = new PhysicsDescriptionFlag();
                            physicsDescFlag = (PhysicsDescriptionFlag)Convert.ToUInt32(parameters[1]);

                            debugOutput = $"{physicsDescFlag.GetType().Name} = {physicsDescFlag.ToString()}" + " (" + (uint)physicsDescFlag + ")";
                            break;
                        case "physicsstate":
                            PhysicsState physState = new PhysicsState();
                            physState = (PhysicsState)Convert.ToUInt32(parameters[1]);

                            debugOutput = $"{physState.GetType().Name} = {physState.ToString()}" + " (" + (uint)physState + ")";
                            break;
                        case "validlocations":
                            EquipMask locFlags = new EquipMask();
                            locFlags = (EquipMask)Convert.ToUInt32(parameters[1]);

                            debugOutput = $"{locFlags.GetType().Name} = {locFlags.ToString()}" + " (" + (uint)locFlags + ")";
                            break;
                        case "currentwieldedlocation":
                            EquipMask locFlags2 = new EquipMask();
                            locFlags2 = (EquipMask)Convert.ToUInt32(parameters[1]);

                            debugOutput = $"{locFlags2.GetType().Name} = {locFlags2.ToString()}" + " (" + (uint)locFlags2 + ")";
                            break;
                        case "priority":
                            CoverageMask covMask = new CoverageMask();
                            covMask = (CoverageMask)Convert.ToUInt32(parameters[1]);

                            debugOutput = $"{covMask.GetType().Name} = {covMask.ToString()}" + " (" + (uint)covMask + ")";
                            break;
                        case "radarcolor":
                            RadarColor radarBlipColor = new RadarColor();
                            radarBlipColor = (RadarColor)Convert.ToUInt32(parameters[1]);

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

        // gps
        [CommandHandler("gps", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld,
            "Display location.")]
        public static void HandleDebugGPS(Session session, params string[] parameters)
        {
            var position = session.Player.Location;
            ChatPacket.SendServerMessage(session, $"Position: [Cell: 0x{position.LandblockId.Landblock.ToString("X4")} | Offset: {position.PositionX}, {position.PositionY}, {position.PositionZ} | Facing: {position.RotationX}, {position.RotationY}, {position.RotationZ}, {position.RotationW}]", ChatMessageType.Broadcast);
        }

        // telexyz cell x y z qx qy qz qw
        [CommandHandler("telexyz", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 8,
            "Teleport to a location.",
            "cell x y z qx qy qz qw\n" +
            "all parameters must be specified and cell must be in decimal form")]
        public static void HandleDebugTeleportXYZ(Session session, params string[] parameters)
        {
            uint cell;
            if (!uint.TryParse(parameters[0], out cell))
                return;

            var positionData = new float[7];
            for (uint i = 0u; i < 7u; i++)
            {
                float position;
                if (!float.TryParse(parameters[i + 1], out position))
                    return;

                positionData[i] = position;
            }

            session.Player.Teleport(new Position(cell, positionData[0], positionData[1], positionData[2], positionData[3], positionData[4], positionData[5], positionData[6]));
        }

        // portalrecall
        [CommandHandler("portalrecall", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0,
            "Recalls the last portal used.")]
        public static void HandleDebugPortalRecall(Session session, params string[] parameters)
        {
            session.Player.HandleActionTeleToPosition(PositionType.LastPortal, () =>
            // On error
            {
                // You are too powerful to interact with that portal!
                var portalRecallMessage = new GameEventDisplayStatusMessage(session, StatusMessageType1.Enum_04A3);
                session.Network.EnqueueSend(portalRecallMessage);
            });
        }

        // grantxp ulong
        [CommandHandler("grantxp", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1,
            "Give XP to yourself.",
            "ulong\n" +
            "@grantxp 191226310247 is max level 275")]
        public static void HandleGrantXp(Session session, params string[] parameters)
        {
            if (parameters?.Length > 0)
            {
                ulong xp = 0;
                string xpAmountToParse = parameters[0].Length > 12 ? parameters[0].Substring(0, 12) : parameters[0];
                // 12 characters : xxxxxxxxxxxx : 191,226,310,247 for 275
                if (ulong.TryParse(xpAmountToParse, out xp))
                {
                    session.Player.GrantXp(xp);
                    return;
                }
            }
            ChatPacket.SendServerMessage(session, "Usage: /grantxp 1234 (max 999999999999)", ChatMessageType.Broadcast);
            return;
        }

        // grantxp ulong
        [CommandHandler("sethealth", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1,
            "sets your current health to a specific value.",
            "ushort")]
        public static void HandleSetHealth(Session session, params string[] parameters)
        {
            if (parameters?.Length > 0)
            {
                ushort health = 0;
                if (ushort.TryParse(parameters[0], out health))
                {
                    session.Player.Health.Current = health;
                    var updatePlayersHealth = new GameMessagePrivateUpdateAttribute2ndLevel(session, Vital.Health, session.Player.Health.Current);
                    var message = new GameMessageSystemChat($"Attempting to set health to {health}...", ChatMessageType.Broadcast);
                    session.Network.EnqueueSend(updatePlayersHealth, message);
                    return;
                }
            }
            ChatPacket.SendServerMessage(session, "Usage: /sethealth 200 (max Max Health)", ChatMessageType.Broadcast);
            return;
        }

        // playsound [Sound] (volumelevel)
        [CommandHandler("playsound", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld,
            "Plays a sound.",
            "sound (float)\n" +
            "Sound can be uint or enum name" +
            "float is volume level")]
        public static void HandlePlaySound(Session session, params string[] parameters)
        {
            try
            {
                Sound sound = Sound.Invalid;
                string message = "";
                float volume = 1f;
                var soundEvent = new GameMessageSound(session.Player.Guid, Sound.Invalid, volume);

                if (parameters.Length > 1)
                    if (parameters[1] != "")
                        volume = float.Parse(parameters[1]);

                message = $"Unable to find a sound called {parameters[0]} to play.";

                if (Enum.TryParse(parameters[0], true, out sound))
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

        // effect [Effect] (scale)
        [CommandHandler("effect", AccessLevel.Player, CommandHandlerFlag.RequiresWorld, 1,
            "Plays an effect.",
            "effect (float)\n" +
            "Effect can be uint or enum name" +
            "float is scale level")]
        public static void HandlePlayEffect(Session session, params string[] parameters)
        {
            try
            {
                PlayScript effect = PlayScript.Invalid;
                string message = "";
                float scale = 1f;
                var effectEvent = new GameMessageScript(session.Player.Guid, PlayScript.Invalid);

                if (parameters.Length > 1)
                    if (parameters[1] != "")
                        scale = float.Parse(parameters[1]);

                message = $"Unable to find a effect called {parameters[0]} to play.";

                if (Enum.TryParse(parameters[0], true, out effect))
                {
                    if (Enum.IsDefined(typeof(PlayScript), effect))
                    {
                        message = $"Playing effect {Enum.GetName(typeof(PlayScript), effect)}";
                        session.Player.HandleActionApplyVisualEffect(effect);
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

        [CommandHandler("chatdump", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0,
            "Spews 1000 lines of text to you.")]
        public static void ChatDump(Session session, params string[] parameters)
        {
            for (int i = 0; i < 1000; i++)
            {
                ChatPacket.SendServerMessage(session, "Test Message " + i, ChatMessageType.Broadcast);
            }
        }

        [CommandHandler("animation", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1,
            "Sends a MovementEvent to you.",
            "uint\n")]
        public static void Animation(Session session, params string[] parameters)
        {
            uint animationId;
            try
            {
                animationId = Convert.ToUInt32(parameters[0]);
            }
            catch (Exception)
            {
                ChatPacket.SendServerMessage(session, $"Invalid Animation value", ChatMessageType.Broadcast);
                return;
            }
            UniversalMotion motion = new UniversalMotion(MotionStance.Standing, new MotionItem((MotionCommand)animationId));
            session.Player.HandleActionMotion(motion);
        }

        // This function is just used to exercise the ability to have player movement without animation.   Once we are solid on this it can be removed.   Og II
        [CommandHandler("movement", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0,
            "Movement testing command, to be removed soon")]
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

        // This function is just used to exercise the ability to have player movement without animation.   Once we are solid on this it can be removed.   Og II
        [CommandHandler("MoveTo", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0,
             "Used to test the MoveToObject message.   It will spawn a training wand in front of you and then move to that object.",
            "moveto\n" +
            "optional parameter distance if omitted 10f")]
        public static void MoveTo(Session session, params string[] parameters)
        {
            var distance = 10.0f;
            ushort trainingWandTarget = 12748;
            if ((parameters?.Length > 0))
                distance = Convert.ToInt16(parameters[0]);
            WorldObject loot = WorldObjectFactory.CreateNewWorldObject(trainingWandTarget);

            ActionChain chain = new Entity.Actions.ActionChain();

            // By chaining the spawn followed by the add pickup action, we ensure the item will be spawned before the player
            chain.AddChain(LootGenerationFactory.GetSpawnChain(loot, session.Player.Location.InFrontOf(distance)));

            chain.AddAction(session.Player,
                () => session.Player.HandleActionPutItemInContainer(loot.Guid, session.Player.Guid));

            chain.EnqueueChain();
        }

        // This function
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
            int value = int.MaxValue;

            if (!int.TryParse(paramValue, out value))
            {
                ChatPacket.SendServerMessage(session, "setvital Error: Invalid set value", ChatMessageType.Broadcast);
                return;
            }

            // Parse args...
            CreatureVital vital = null;
            if (paramVital == "health" || paramVital == "hp")
            {
                vital = session.Player.Health;
            }
            else if (paramVital == "stamina" || paramVital == "stam" || paramVital == "sp")
            {
                vital = session.Player.Stamina;
            }
            else if (paramVital == "mana" || paramVital == "mp")
            {
                vital = session.Player.Mana;
            }
            else
            {
                ChatPacket.SendServerMessage(session, "setvital Error: Invalid vital", ChatMessageType.Broadcast);
                return;
            }

            if (!relValue)
            {
                session.Player.UpdateVital(vital, (uint)value);
            }
            else
            {
                session.Player.DeltaVital(vital, value);
            }
        }

        [CommandHandler("createportal", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld,
            "Creates a portal in front of you.")]
        public static void CreatePortal(Session session, params string[] parameters)
        {
            SpecialPortalObjectFactory.SpawnPortal(SpecialPortalObjectFactory.PortalWcid.HummingCrystal, session.Player.Location.InFrontOf(3.0f), 15.0f);
        }

        /// <summary>
        /// Debug command to saves the character from in-game.
        /// </summary>
        /// <remarks>Added a quick way to invoke the character save routine.</remarks>
        [CommandHandler("save-now", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld,
            "Saves your session.")]
        public static void HandleSaveNow(Session session, params string[] parameters)
        {
            session.SaveSession();
        }

        /// <summary>
        /// Returns the Player's GUID
        /// </summary>
        /// <remarks>Added a quick way to access the player GUID.</remarks>
        [CommandHandler("whoami", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld,
            "Shows you your GUIDs.")]
        public static void HandleWhoAmI(Session session, params string[] parameters)
        {
            ChatPacket.SendServerMessage(session, $"GUID: {session.Player.Guid.Full} ID(low): {session.Player.Guid.Low} High:{session.Player.Guid.High}", ChatMessageType.Broadcast);
        }

        // Kill a player - equivalent to legal virtual murder, by admin
        // TODO: Migrate this code into "smite" Admin command
        [CommandHandler("kill", AccessLevel.Admin, CommandHandlerFlag.None, 1,
            "See @smite")]
        public static void HandleSendKill(Session session, params string[] parameters)
        {
            // lame checks on first parameter
            if (parameters?.Length > 0)
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
                // if there are now spaces, just set the characterName to the first paramter
                else
                    characterName = parameters[0];

                // look up session
                Session playerSession = WorldManager.FindByPlayerName(characterName, true);

                // playerSession will be null when the character is not found
                if (playerSession != null)
                {
                    playerSession.Player.HandleActionKill(playerSession.Player.Guid);
                    return;
                }
            }

            // Did not find a player
            Console.WriteLine($"Error locating the player.");
        }

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

        /// <summary>
        /// Debug command to kill a targeted creature so it drops a corpse.
        /// </summary>
        [CommandHandler("testcorpsedrop", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld)]
        public static void TestCorpse(Session session, params string[] parameters)
        {
            session.Player.HandleActionTestCorpseDrop();
        }

        /// <summary>
        /// Debug command to read the Generators from the DatFile 0x0E00000D in portal.dat.
        /// </summary>
        [CommandHandler("readgenerators", AccessLevel.Developer, CommandHandlerFlag.ConsoleInvoke,
            "Debug command to read the Generators from the DatFile 0x0E00000D in portal.dat")]
        public static void Treadgenerators(Session session, params string[] parameters)
        {
            var generators = GeneratorTable.ReadFromDat();

            // Example for accessing the tree with nodes of type Generator
            generators.ReadItems().Where(node => node.Name == "Drudges").ToList().ForEach(gen =>
            {
                Console.WriteLine($"{gen.Id:X8} {gen.Count:X8} {gen.Name}");
                if (gen.Count > 0)
                {
                    for (var i = 0; i < gen.Count; i++)
                        Console.WriteLine($"{gen.Items[i].Id:X8} {gen.Items[i].Count:X8} {gen.Items[i].Name}");
                }
            });
        }

        /// <summary>
        /// Debug command to save the player's current location as sepecific position type.
        /// </summary>
        /// <param name="parameters">A single uint value within the range of 1 through 27</param>
        [CommandHandler("setposition", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1,
            "Saves the supplied character position type to the database.",
            "uint 1-27\n" +
            "@setposition 1")]
        public static void HandleSetPosition(Session session, params string[] parameters)
        {
            if (parameters?.Length == 1)
            {
                PositionType positionType = new PositionType();
                string parsePositionString = parameters[0].Length > 19 ? parameters[0].Substring(0, 19) : parameters[0];
                // The enum labels max character length has been observered as length 19
                // int value can be: 0-27
                if (Enum.TryParse(parsePositionString, out positionType))
                {
                    if (positionType != PositionType.Undef)
                    {
                        // Create a new position from the current player location
                        Position playerPosition = (Position)session.Player.Location.Clone();

                        // Save the position
                        session.Player.SetCharacterPosition(positionType, playerPosition);

                        // Report changes to client
                        var positionMessage = new GameMessageSystemChat($"Set: {positionType} to Loc: {playerPosition.ToString()}", ChatMessageType.Broadcast);
                        session.Network.EnqueueSend(positionMessage);
                        return;
                    }
                }
            }
            session.Network.EnqueueSend(new GameMessageSystemChat($"Could not determine the correct position type.\nPlease supply a single integer value from within the range of 1 through 27.", ChatMessageType.Broadcast));
        }

        /// <summary>
        /// Debug command to teleport a player to a saved position, if the position type exists within the database.
        /// </summary>
        /// <param name="parameters">A single uint value within the range of 1 through 27</param>
        [CommandHandler("teletype", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1,
            "Teleport to a saved character position.",
            "uint 0-22\n" +
            "@teletype 1")]
        public static void HandleTeleType(Session session, params string[] parameters)
        {
            PositionType positionType = new PositionType();
            if (parameters?.Length > 0)
            {
                string parsePositionString = parameters[0].Length > 3 ? parameters[0].Substring(0, 3) : parameters[0];

                if (Enum.TryParse(parsePositionString, out positionType))
                {
                    bool success = true;
                    ActionChain teleChain = session.Player.GetTeleToPositionChain(positionType, () =>
                    {
                        success = false;
                    });
                    teleChain.AddAction(session.Player, () =>
                    {
                        if (success)
                        {
                            session.Network.EnqueueSend(new GameMessageSystemChat($"{PositionType.Location} {session.Player.Location.ToString()}", ChatMessageType.Broadcast));
                        }
                        else
                        {
                            session.Network.EnqueueSend(new GameMessageSystemChat($"Error finding saved character position: {positionType}", ChatMessageType.Broadcast));
                        }
                    });
                    teleChain.EnqueueChain();
                }
            }
        }

        /// <summary>
        /// Debug command to print out all of the saved character positions.
        /// </summary>
        [CommandHandler("listpositions", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0,
            "Displays all available saved character positions from the database.",
            "@listpositions")]
        public static void HandleListPositions(Session session, params string[] parameters)
        {
            // Build a string message containing all available character positions and send as a System Chat message
            string message = $"Saved character positions:\n";
            var posDict = session.Player.Positions;

            foreach (var posPair in posDict)
            {
                message += "ID: " + (uint)posPair.Key + " Loc: " + posPair.Value.ToString() + "\n";
            }

            message += $"Total positions: " + posDict.Count.ToString() + "\n";
            var positionMessage = new GameMessageSystemChat(message, ChatMessageType.Broadcast);
            session.Network.EnqueueSend(positionMessage);
        }

        /// <summary>
        /// Debug command to test the ObjDescEvent message.
        /// </summary>
        [CommandHandler("equiptest", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld,
            "Simulates equipping a new item to your character, replacing all other items.")]
        public static void EquipTest(Session session, params string[] parameters)
        {
            if (!(parameters?.Length > 0))
            {
                ChatPacket.SendServerMessage(session, "Usage: @equiptest (hex)clothingTableId [palette_index].\neg '@equiptest 0x100005fd'",
                    ChatMessageType.Broadcast);
                return;
            }

            uint modelId;
            try
            {
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

                if ((modelId >= 0x10000001) && (modelId <= 0x1000086B))
                    session.Player.TestWieldItem(session, modelId, palOption);
                else
                    ChatPacket.SendServerMessage(session, "Please enter a value greater than 0x10000000 and less than 0x1000086C",
                        ChatMessageType.Broadcast);
            }
            catch (Exception)
            {
                ChatPacket.SendServerMessage(session, "Please enter a value greater than 0x10000000 and less than 0x1000086C", ChatMessageType.Broadcast);
            }
        }

        /// <summary>
        /// Debug command to print out all of the active players connected too the server.
        /// </summary>
        [CommandHandler("listplayers", AccessLevel.Developer, CommandHandlerFlag.None, 0,
            "Displays all of the active players connected too the server.",
            "")]
        public static void HandleListPlayers(Session session, params string[] parameters)
        {
            uint playerCounter = 0;
            // Build a string message containing all available characters and send as a System Chat message
            string message = "";
            foreach (Session playerSession in WorldManager.GetAll(false))
            {
                message += $"{playerSession.Player.Name} : {(uint)playerSession.Id}\n";
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

        // This debug command was added to test combat stance - we need one of each type weapon and a shield Og II
        [CommandHandler("weapons", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0,
            "Creates testing items in your inventory.")]
        public static void HandleWeapons(Session session, params string[] parameters)
        {
            // HashSet<uint> weaponsTest = new HashSet<uint>() { 93, 127, 130, 136, 136, 136, 148, 300, 307, 311, 326, 338, 348, 350, 7765, 12748, 12463, 31812 };

            HashSet<uint> weaponsTest = new HashSet<uint>() { (uint)TestWeenieClassIds.Pants,
                                                              (uint)TestWeenieClassIds.Tunic,
                                                              (uint)TestWeenieClassIds.TrainingWand,
                                                              (uint)TestWeenieClassIds.ColoBackpack };
            ActionChain chain = new ActionChain();

            chain.AddAction(session.Player, () =>
            {
                foreach (uint weenieId in weaponsTest)
                {
                    WorldObject loot = WorldObjectFactory.CreateNewWorldObject(weenieId);
                    loot.ContainerId = session.Player.Guid.Full;
                    loot.Placement = 0;
                    // TODO: Og II
                    // Need this hack because weenies are not cleaned up.   Can be removed once weenies are fixed.
                    loot.WielderId = null;
                    loot.CurrentWieldedLocation = null;

                    session.Player.AddToInventory(loot);
                    session.Player.TrackObject(loot);
                    session.Player.UpdatePlayerBurden();
                    session.Network.EnqueueSend(
                        new GameMessagePutObjectInContainer(session, session.Player.Guid, loot, 0),
                        new GameMessageUpdateInstanceId(loot.Guid, session.Player.Guid, PropertyInstanceId.Container));
                }
                // Force a save for our test items.   Og II
                // DatabaseManager.Shard.SaveObject(session.Player.GetSavableCharacter(), null);
            });
            chain.EnqueueChain();
        }

        // This debug command was added to test combat stance - we need one of each type weapon and a shield Og II
        [CommandHandler("inv", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0,
        "Creates sample items, foci and containers in your inventory.")]
        public static void HandleInv(Session session, params string[] parameters)
        {
            HashSet<uint> weaponsTest = new HashSet<uint>() { 44, 45, 46, 15268, 15269, 15270, 15271, 12748, 5893, 136 };
            ActionChain chain = new ActionChain();

            chain.AddAction(session.Player, () =>
            {
                foreach (uint weenieId in weaponsTest)
                {
                    WorldObject loot = WorldObjectFactory.CreateNewWorldObject(weenieId);
                    loot.ContainerId = session.Player.Guid.Full;
                    loot.Placement = 0;
                    session.Player.AddToInventory(loot);
                    session.Player.TrackObject(loot);
                    chain.AddDelaySeconds(0.25);
                    session.Player.UpdatePlayerBurden();
                    session.Network.EnqueueSend(
                        new GameMessagePutObjectInContainer(session, session.Player.Guid, loot, 0),
                        new GameMessageUpdateInstanceId(loot.Guid, session.Player.Guid, PropertyInstanceId.Container));
                }
            });
            chain.EnqueueChain();
        }

        [CommandHandler("splits", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0,
            "Creates some stackable items in your inventory for testing.")]
        public static void HandleSplits(Session session, params string[] parameters)
        {
            HashSet<uint> splitsTest = new HashSet<uint>() { 237, 300, 690, 20630, 20631, 37155, 31198 };
            ActionChain chain = new ActionChain();

            chain.AddAction(session.Player, () =>
            {
                foreach (uint weenieId in splitsTest)
                {
                    WorldObject loot = WorldObjectFactory.CreateNewWorldObject(weenieId);
                    var valueEach = loot.Value / loot.StackSize;
                    loot.StackSize = loot.MaxStackSize;
                    loot.Value = loot.StackSize * valueEach;
                    loot.ContainerId = session.Player.Guid.Full;
                    loot.Placement = 0;
                    session.Player.AddToInventory(loot);
                    session.Player.TrackObject(loot);
                    session.Network.EnqueueSend(
                        new GameMessagePutObjectInContainer(session, session.Player.Guid, loot, 0),
                        new GameMessageUpdateInstanceId(loot.Guid, session.Player.Guid, PropertyInstanceId.Container));
                }
            });
            chain.EnqueueChain();
        }

        [CommandHandler("cirand", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1,
            "Creates an random object in your inventory.", "typeId (number) <num to create) defaults to 10 if omitted max 50")]
        public static void HandleCIRandom(Session session, params string[] parameters)
        {
            uint typeId;
            byte numItems = 10;
            try
            {
                typeId = Convert.ToUInt32(parameters[0]);
            }
            catch (Exception)
            {
                ChatPacket.SendServerMessage(session, "Not a valid type id - must be a number between 0 - 2,147,483,647", ChatMessageType.Broadcast);
                return;
            }
            if (parameters.Length == 2)
            {
                try
                {
                    numItems = Convert.ToByte(parameters[1]);
                }
                catch (Exception)
                {
                    ChatPacket.SendServerMessage(session, "Not a valid number - must be a number between 0 - 50", ChatMessageType.Broadcast);
                    return;
                }
            }
            ActionChain chain = new ActionChain();
            chain.AddAction(session.Player, () => LootGenerationFactory.CreateRandomTestWorldObjects(session.Player, typeId, numItems));
            chain.EnqueueChain();
        }

        /// <summary>
        /// Debug command to spawn the Barber UI
        /// </summary>
        [CommandHandler("barbershop", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld)]
        public static void BarberShop(Session session, params string[] parameters)
        {
            ActionChain chain = new ActionChain();
            chain.AddAction(session.Player, () => session.Network.EnqueueSend(new GameEventStartBarber(session)));
            chain.EnqueueChain();
        }

        // addspell <spell>
        [CommandHandler("addspell", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1,
            "Adds the specified spell to your own spellbook.",
            "<spellid>")]
        public static void HandleAddSpell(Session session, params string[] parameters)
        {
            AdminCommands.HandleAdd(session, parameters);
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

            uint spellid;
            if (!uint.TryParse(parameters[1], out spellid))
            {
                Console.WriteLine("getspellformula <accountname> <spellid>");
                return;
            }

            DatLoader.FileTypes.SpellComponentsTable comps = DatLoader.FileTypes.SpellComponentsTable.ReadFromDat();
            DatLoader.FileTypes.SpellTable spellTable = DatLoader.FileTypes.SpellTable.ReadFromDat();
            string spellName = spellTable.Spells[spellid].Name;
            System.Collections.Generic.List<uint> formula = DatLoader.FileTypes.SpellTable.GetSpellFormula(spellid, parameters[0]);
            Console.WriteLine("Formula for " + spellName);
            for (int i = 0; i < formula.Count; i++)
            {
                Console.WriteLine("Comp " + i.ToString() + ": " + comps.SpellComponents[formula[i]].Name);
            }
            Console.WriteLine();
        }
    }
}
