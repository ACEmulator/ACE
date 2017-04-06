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

namespace ACE.Command.Handlers
{
    public static class DebugCommands
    {
        // echo "text to send back to yourself" [ChatMessageType]
        [CommandHandler("echo", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld)]
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
        [CommandHandler("gps", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld)]
        public static void HandleDebugGPS(Session session, params string[] parameters)
        {
            var position = session.Player.Position;
            ChatPacket.SendServerMessage(session, $"Position: [Cell: 0x{position.LandblockId.Landblock.ToString("X4")} | Offset: {position.PositionX}, {position.PositionY}, {position.PositionZ} | Facing: {position.RotationX}, {position.RotationY}, {position.RotationZ}, {position.RotationW}]", ChatMessageType.Broadcast);
        }

        // telexyz cell x y z qx qy qz qw
        [CommandHandler("telexyz", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 8)]
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

        // grantxp uint
        [CommandHandler("grantxp", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1)]
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
        [CommandHandler("playsound", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld)]
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
                        soundEvent = new GameMessageSound(session.Player.Guid, sound, volume);
                    }
                }

                var sysChatMessage = new GameMessageSystemChat(message, ChatMessageType.Broadcast);
                session.Network.EnqueueSend(soundEvent, sysChatMessage);
            }
            catch (Exception)
            {
                // Do Nothing
            }
        }

        // effect [Effect] (scale)
        [CommandHandler("effect", AccessLevel.Player, CommandHandlerFlag.RequiresWorld, 1)]
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
                        effectEvent = new GameMessageScript(session.Player.Guid, effect, scale);
                    }
                }

                var sysChatMessage = new GameMessageSystemChat(message, ChatMessageType.Broadcast);
                session.Network.EnqueueSend(effectEvent, sysChatMessage);
            }
            catch (Exception)
            {
                // Do Nothing
            }
        }

        [CommandHandler("chatdump", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0)]
        public static void ChatDump(Session session, params string[] parameters)
        {
            for (int i = 0; i < 1000; i++)
            {
                ChatPacket.SendServerMessage(session, "Test Message " + i, ChatMessageType.Broadcast);
            }
        }

        [CommandHandler("animation", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1)]
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
            session.Network.EnqueueSend(new GameMessageMotion(session.Player, session, (MotionCommand)animationId, 1.0f));
        }

        // This function is just used to exercise the ability to have player movement without animation.   Once we are solid on this it can be removed.   Og II
        [CommandHandler("movement", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0)]
        public static void Movement(Session session, params string[] parameters)
        {
            var movement = new MovementData { ForwardCommand = 24, MovementStateFlag = MovementStateFlag.ForwardCommand };
            session.Network.EnqueueSend(new GameMessageMotion(session.Player, session, MotionAutonomous.False, MovementTypes.Invalid, MotionFlags.None, MotionStance.Standing, movement));
            movement.ForwardCommand = 0;
            movement.MovementStateFlag = MovementStateFlag.NoMotionState;
            session.Network.EnqueueSend(new GameMessageMotion(session.Player, session, MotionAutonomous.False, MovementTypes.Invalid, MotionFlags.None, MotionStance.Standing, movement));
        }

        [CommandHandler("spacejump", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0)]
        public static void SpaceJump(Session session, params string[] parameters)
        {
            Position newPosition = new Position(session.Player.Position.LandblockId.Landblock, session.Player.Position.PositionX, session.Player.Position.PositionY, session.Player.Position.PositionZ + 8000f, session.Player.Position.RotationX, session.Player.Position.RotationY, session.Player.Position.RotationZ, session.Player.Position.RotationW);
            session.Player.Teleport(newPosition);
        }

        [CommandHandler("createlifestone", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld)]
        public static void CreateLifeStone(Session session, params string[] parameters)
        {
            LandblockManager.AddObject(LifestoneObjectFactory.CreateLifestone(509, session.Player.Position.InFrontOf(3.0f), LifestoneType.Original));
        }

        [CommandHandler("createportal", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld)]
        public static void CreatePortal(Session session, params string[] parameters)
        {
            LandblockManager.AddObject(PortalObjectFactory.CreatePortal(1234, session.Player.Position.InFrontOf(3.0f), "Test Portal", PortalType.Purple));
        }

        /// <summary>
        /// Debug command to saves the character from in-game.
        /// </summary>
        /// <remarks>Added a quick way to invoke the character save routine.</remarks>
        [CommandHandler("save-now", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld)]
        public static void HandleSaveNow(Session session, params string[] parameters)
        {
            session.SaveSession();
        }

        /// <summary>
        /// Returns the Player's GUID
        /// </summary>
        /// <remarks>Added a quick way to access the player GUID.</remarks>
        [CommandHandler("whoami", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld)]
        public static void HandleWhoAmI(Session session, params string[] parameters)
        {
            ChatPacket.SendServerMessage(session, $"GUID: {session.Player.Guid.Full} ID(low): {session.Player.Guid.Low} High:{session.Player.Guid.High}", ChatMessageType.Broadcast);
        }

        /// <summary>
        /// Debug command to set an invalid character position for a position type. Used to test the logic and saving data to the database.
        /// </summary>
        [CommandHandler("reset-pos", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1)]
        public static void HandleResetPosition(Session session, params string[] parameters)
        {
            try
            {
                // if (parameters.Length > 1)
                //    if (parameters[1] != "")
                //        scale = float.Parse(parameters[1]);

                string message = "Error saving position.";

                PositionType type = new PositionType();

                if (Enum.TryParse(parameters[0], true, out type))
                {
                    if (Enum.IsDefined(typeof(PositionType), type))
                    {
                        message = $"Saving position {Enum.GetName(typeof(PositionType), type)}";
                        session.Player.SetCharacterPosition(type, CharacterPositionExtensions.InvalidPosition(session.Id, type));
                    }
                }

                float scale = 1f;
                var effectEvent = new GameMessageScript(session.Player.Guid, Network.Enum.PlayScript.AttribDownRed, scale);
                var sysChatMessage = new GameMessageSystemChat(message, ChatMessageType.Broadcast);
                session.Network.EnqueueSend(effectEvent, sysChatMessage);
            }
            catch (Exception)
            {
                // Do Nothing
        }
            }

        // @testspell 0 10 10 10 10 20
        [CommandHandler("testspell", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 3)]
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
            LandblockManager.AddObject(SpellObjectFactory.CreateSpell(templatid, session.Player.Position.InFrontOf(2.0f), velocity, friction, electicity));
        }

        [CommandHandler("ctw", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld)]
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
                WorldObject loot = LootGenerationFactory.CreateTrainingWand(session.Player);
                switch (location)
                {
                    case "me":
                        {
                            LootGenerationFactory.AddToContainer(loot, session.Player);
                            session.Player.TrackObject(loot);
                            // TODO: Have to send game message CFS
                            break;
                        }
                    case "ground":
                        {
                            LootGenerationFactory.Spawn(loot, session.Player.Position.InFrontOf(1.0f));
                            LandblockManager.AddObject(loot);
                            break;
                        }
                }
            }
            else
            {
                ChatPacket.SendServerMessage(session, "Usage: @ctw me or @ctw ground",
                    ChatMessageType.Broadcast);
            }
        }
    }
}