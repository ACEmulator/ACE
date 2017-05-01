using System;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Managers;
using ACE.Network;
using ACE.Network.Enum;
using ACE.Network.GameMessages;
using ACE.Network.GameMessages.Messages;
using ACE.Network.GameEvent.Events;
using ACE.Network.Managers;
using ACE.Factories;
using System.Globalization;
using ACE.Network.Motion;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using System.Linq;

namespace ACE.Command.Handlers
{
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
                Network.Enum.Sound sound = Network.Enum.Sound.Invalid;
                string message = "";
                float volume = 1f;
                var soundEvent = new GameMessageSound(session.Player.Guid, Network.Enum.Sound.Invalid, volume);

                if (parameters.Length > 1)
                    if (parameters[1] != "")
                        volume = float.Parse(parameters[1]);

                message = $"Unable to find a sound called {parameters[0]} to play.";

                if (Enum.TryParse(parameters[0], true, out sound))
                {
                    if (Enum.IsDefined(typeof(Network.Enum.Sound), sound))
                    {
                        message = $"Playing sound {Enum.GetName(typeof(Network.Enum.Sound), sound)}";
                        // add the sound to the player queue for everyone to hear
                        // player action queue items will execute on the landblock
                        // player.playsound will play a sound on only the client session that called the function
                        session.Player.ActionApplySoundEffect(sound, session.Player.Guid);
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
                Network.Enum.PlayScript effect = Network.Enum.PlayScript.Invalid;
                string message = "";
                float scale = 1f;
                var effectEvent = new GameMessageScript(session.Player.Guid, Network.Enum.PlayScript.Invalid);

                if (parameters.Length > 1)
                    if (parameters[1] != "")
                        scale = float.Parse(parameters[1]);

                message = $"Unable to find a effect called {parameters[0]} to play.";

                if (Enum.TryParse(parameters[0], true, out effect))
                {
                    if (Enum.IsDefined(typeof(Network.Enum.PlayScript), effect))
                    {
                        message = $"Playing effect {Enum.GetName(typeof(Network.Enum.PlayScript), effect)}";
                        session.Player.ActionApplyVisualEffect(effect, session.Player.Guid);
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
            session.Player.EnqueueMovementEvent(motion, session.Player.Guid);
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
            session.Network.EnqueueSend(new GameMessageUpdateMotion(session.Player, session, movement));
            movement = new UniversalMotion(MotionStance.Standing);
            session.Network.EnqueueSend(new GameMessageUpdateMotion(session.Player, session, movement));
        }

        // This function is just used to exercise the ability to have player movement without animation.   Once we are solid on this it can be removed.   Og II
        [CommandHandler("MoveTo", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0,
             "Used to test the MoveToObject message.   It will spawn a training wand in front of you and then move to that object.",
            "moveto\n" +
            "optional parameter distance if omitted 10f")]
        public static void MoveTo(Session session, params string[] parameters)
        {
            var distance = 10.0f;
            if ((parameters?.Length > 0))
                distance = Convert.ToInt16(parameters[0]);
            var loot = LootGenerationFactory.CreateTrainingWand(session.Player);
            LootGenerationFactory.Spawn(loot, session.Player.Location.InFrontOf(distance));
            session.Player.TrackObject(loot);
            var newMotion = new UniversalMotion(MotionStance.Standing, loot);
            if ((parameters?.Length > 1))
                newMotion.Flag = (uint)Convert.ToInt32(parameters[1]);
            session.Network.EnqueueSend(new GameMessageUpdatePosition(session.Player));
            session.Network.EnqueueSend(new GameMessageUpdateMotion(session.Player, loot, newMotion, MovementTypes.MoveToObject));
        }

        [CommandHandler("spacejump", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0,
            "Teleports you to current position with PositionZ set to +8000.")]
        public static void SpaceJump(Session session, params string[] parameters)
        {
            Position newPosition = new Position(session.Player.Location.LandblockId.Landblock, session.Player.Location.PositionX, session.Player.Location.PositionY, session.Player.Location.PositionZ + 8000f, session.Player.Location.RotationX, session.Player.Location.RotationY, session.Player.Location.RotationZ, session.Player.Location.RotationW);
            session.Player.Teleport(newPosition);
        }

        [CommandHandler("createlifestone", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld,
            "Creates a lifestone in front of you.")]
        public static void CreateLifeStone(Session session, params string[] parameters)
        {
            LandblockManager.AddObject(LifestoneObjectFactory.CreateLifestone(509, session.Player.Location.InFrontOf(3.0f), LifestoneType.Original));
        }

        [CommandHandler("createportal", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld,
            "Creates a portal in front of you.")]
        public static void CreatePortal(Session session, params string[] parameters)
        {
            LandblockManager.AddObject(PortalObjectFactory.CreatePortal(1234, session.Player.Location.InFrontOf(3.0f), "Test Portal", PortalType.Purple));
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

        // @testspell 0 10 10 10 10 20
        [CommandHandler("testspell", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 3,
            "Launch a spell projectile.",
            "templateid x y z friction elasticity\n" +
            "Example: @testspell 0 10 10 10 10 20")]
        public static void TestSpell(Session session, params string[] parameters)
        {
            uint templatid;
            float x, y, z;
            float friction;
            float electicity;
            try
            {
                templatid = Convert.ToUInt32(parameters[0]);
                x = float.Parse(parameters[1], CultureInfo.InvariantCulture.NumberFormat);
                y = float.Parse(parameters[2], CultureInfo.InvariantCulture.NumberFormat);
                z = float.Parse(parameters[3], CultureInfo.InvariantCulture.NumberFormat);
                friction = float.Parse(parameters[4], CultureInfo.InvariantCulture.NumberFormat);
                electicity = float.Parse(parameters[5], CultureInfo.InvariantCulture.NumberFormat);
           }
            catch (Exception)
            {
                ChatPacket.SendServerMessage(session, $"Invalid Spell Parameters", ChatMessageType.Broadcast);
                return;
            }

            AceVector3 velocity = new AceVector3(x, y, z);
            LandblockManager.AddObject(SpellObjectFactory.CreateSpell(templatid, session.Player.Location.InFrontOf(2.0f), velocity, friction, electicity));
        }

        [CommandHandler("ctw", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld,
            "Creates a training wand on the ground or in your main pack.",
            "[me or ground]" +
            "@ctw me = Creates the wand in your main pack.\n" +
            "@ctw ground = Creates the wand in front of you on the ground.")]
        public static void CreateTrainingWand(Session session, params string[] parameters)
        {
            if (!(parameters?.Length > 0))
            {
                ChatPacket.SendServerMessage(session, "Usage: @ctw me or @ctw ground",
                   ChatMessageType.Broadcast);
                return;
            }
            string location = parameters[0];
            if (location == "me" | location == "ground")
            {
                var loot = LootGenerationFactory.CreateTrainingWand(session.Player);
                switch (location)
                {
                    case "me":
                        {
                            LootGenerationFactory.AddToContainer(loot, session.Player);
                            break;
                        }
                    case "ground":
                        {
                            LootGenerationFactory.Spawn(loot, session.Player.Location.InFrontOf(1.0f));
                            break;
                        }
                }
                session.Player.TrackObject(loot);
            }
            else
            {
                ChatPacket.SendServerMessage(session, "Usage: @ctw me or @ctw ground",
                    ChatMessageType.Broadcast);
            }
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
                    // send session a usedone
                    playerSession.Player.OnKill(playerSession);
                    return;
                }
            }

            // Did not find a player
            Console.WriteLine($"Error locating the player.");
        }

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

        /// <summary>
        /// Debug command to kill a targeted creature so it drops a corpse.
        /// </summary>
        [CommandHandler("testcorpsedrop", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld)]
        public static void TestCorpse(Session session, params string[] parameters)
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
            }
            else
            {
                ChatPacket.SendServerMessage(session, "No creature selected.", ChatMessageType.Broadcast);
            }
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
            generators.ReadItems().Where(node => node.Name == "Drudges").ToList().ForEach(gen => {
                Console.WriteLine($"{gen.Id:X8} {gen.Count:X8} {gen.Name}");
                if (gen.Count > 0)
                {
                    for (var i = 0; i < gen.Count; i++)
                        Console.WriteLine($"{gen.Items[i].Id:X8} {gen.Items[i].Count:X8} {gen.Items[i].Name}");
                }
            });
        }
    }
}