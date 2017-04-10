using ACE.Common.Extensions;
using ACE.Database;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Managers;
using ACE.Network.Enum;
using ACE.Network.GameEvent;
using ACE.Network.GameEvent.Events;
using ACE.Network.GameMessages.Messages;
using ACE.Network.Motion;
using System;
using System.Collections.Generic;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionTeleToLifestone
    {
        [GameAction(GameActionType.TeleToLifestone)]
        public static void Handle(ClientMessage message, Session session)
        {
            if (session.Player.Positions.ContainsKey(PositionType.Sanctuary)) {
                // session.Player.Teleport(session.Player.Positions[PositionType.Sanctuary]);
                string msg = $"{session.Player.Name} is recalling to the lifestone.";

                var sysChatMessage = new GameMessageSystemChat(msg, ChatMessageType.Recall);

                session.Player.Mana.Current = session.Player.Mana.Current / 2;
                var updatePlayersMana = new GameMessagePrivateUpdateAttribute2ndLevel(session, Vital.Mana, session.Player.Mana.Current);

                var updateCombatMode = new GameMessagePrivateUpdatePropertyInt(session, PropertyInt.CombatMode, 1);

                var motionLifestoneRecall = new GeneralMotion(MotionStance.Standing, new MotionItem(MotionCommand.LifestoneRecall));

                var animationEvent = new GameMessageUpdateMotion(session.Player, session, motionLifestoneRecall);

                // TODO: This needs to be changed to broadcast sysChatMessage to only those in local chat hearing range
                // FIX: I think this is only broadcasting to client, not to any other connected players. Likely animationEvent and sysChatMessage need to be broadcast to
                //      clients that are currently tracking this player and the chat message should be within local chat hearing range.
                // session.Network.EnqueueSend(updatePlayersMana, updateCombatMode, animationEvent, sysChatMessage);
                session.Network.EnqueueSend(updatePlayersMana, updateCombatMode, sysChatMessage);
                session.Player.ActionMovementEvent(motionLifestoneRecall, session.Player);

                session.Player.SetDelayedTeleport(TimeSpan.FromSeconds(14), session.Player.Positions[PositionType.Sanctuary]);
            }
            else
            {
                ChatPacket.SendServerMessage(session, "Your spirit has not been attuned to a sanctuary location.", ChatMessageType.Broadcast);
            }
        }
    }
}