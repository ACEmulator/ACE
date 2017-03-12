﻿using System;

using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Managers;
using ACE.Network;
using ACE.Network.GameMessages;
using ACE.Network.GameMessages.Messages;
using ACE.Network.GameEvent.Events;
using ACE.Network.Managers;

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
                    if (Enum.IsDefined(typeof(Network.Enum.Sound), sound))
                    {
                        message = $"Playing sound {Enum.GetName(typeof(Network.Enum.Sound), sound)}";
                        soundEvent = new GameMessageSound(session.Player.Guid, sound, volume);
                    }                                            

                var sysChatMessage = new GameMessageSystemChat(message, ChatMessageType.Broadcast);
                session.WorldSession.EnqueueSend(soundEvent, sysChatMessage);
            }
            catch (Exception)
            {
                //ChatPacket.SendServerMessage(session, parameters[0], ChatMessageType.Broadcast);
            }
        }

        // gps
        [CommandHandler("gps", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld)]
        public static void HandleDebugGPS(Session session, params string[] parameters)
        {
            var position = session.Player.Position;
            ChatPacket.SendServerMessage(session, $"Position: [Cell: 0x{position.Cell.ToString("X4")} | Offset: {position.Offset.X}, {position.Offset.Y}, {position.Offset.Z} | Facing: {position.Facing.X}, {position.Facing.Y}, {position.Facing.Z}, {position.Facing.W}]", ChatMessageType.Broadcast);
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
            uint xp = 0;

            if (parameters?.Length > 0 && uint.TryParse(parameters[0], out xp))
            {
                session.Player.GrantXp(xp);
            }
            else
            {
                ChatPacket.SendServerMessage(session, "Usage: /grantxp 1234", ChatMessageType.Broadcast);
                return;
            }
        }

        // effect [Effect] (scale)
        [CommandHandler("effect", AccessLevel.Player, CommandHandlerFlag.RequiresWorld, 1)]
        public static void HandlePlayEffect(Session session, params string[] parameters)
        {          
            try
            {
                Network.Enum.Effect effect = Network.Enum.Effect.Invalid;
                string message = "";
                float volume = 1f;
                var effectEvent = new GameMessageEffect(session.Player.Guid, Network.Enum.Effect.Invalid);

                if (parameters.Length > 1)
                    if (parameters[1] != "")
                        volume = float.Parse(parameters[1]);

                message = $"Unable to find a effect called {parameters[0]} to play.";

                if (Enum.TryParse(parameters[0], true, out effect))
                    if (Enum.IsDefined(typeof(Network.Enum.Effect), effect))
                    {
                        message = $"Playing effect {Enum.GetName(typeof(Network.Enum.Effect), effect)}";
                        effectEvent = new GameMessageEffect(session.Player.Guid, effect, volume);
                    }

                var sysChatMessage = new GameMessageSystemChat(message, ChatMessageType.Broadcast);
                session.WorldSession.EnqueueSend(effectEvent, sysChatMessage);
            }
            catch (Exception)
            {
                //ChatPacket.SendServerMessage(session, parameters[0], ChatMessageType.Broadcast);
            }
        }


        [CommandHandler("spacejump", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0)]
        public static void spacejump(Session session, params string[] parameters)
        {
            Position newPosition = new Position(session.Player.Position.Cell, session.Player.Position.Offset.X, session.Player.Position.Offset.Y, session.Player.Position.Offset.Z + 8000f, session.Player.Position.Facing.X, session.Player.Position.Facing.Y, session.Player.Position.Facing.Z, session.Player.Position.Facing.W);
            session.Player.Teleport(newPosition);
        }

        [CommandHandler("createlifestone", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld)]
        public static void CreateLifeStone(Session session, params string[] parameters)
        {
            session.WorldSession.EnqueueSend(new GameMessageCreateLifestone(session.Player));
        }

    }
}
