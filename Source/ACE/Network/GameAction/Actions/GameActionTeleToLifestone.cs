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
            var sanctuary = session.Player.GetPosition(PositionType.Sanctuary);
            if (sanctuary != null) {
                // session.Player.Teleport(session.Player.Positions[PositionType.Sanctuary]);
                string msg = $"{session.Player.Name} is recalling to the lifestone.";

                var sysChatMessage = new GameMessageSystemChat(msg, ChatMessageType.Recall);

                session.Player.Mana.Current = session.Player.Mana.Current / 2;
                var updatePlayersMana = new GameMessagePrivateUpdateAttribute2ndLevel(session, Vital.Mana, session.Player.Mana.Current);

                var updateCombatMode = new GameMessagePrivateUpdatePropertyInt(session, PropertyInt.CombatMode, 1);

                var motionLifestoneRecall = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.LifestoneRecall));

                var animationEvent = new GameMessageUpdateMotion(session.Player, session, motionLifestoneRecall);

                // TODO: This needs to be changed to broadcast sysChatMessage to only those in local chat hearing range
                // FIX: Recall text isn't being broadcast yet, need to address
                session.Network.EnqueueSend(updatePlayersMana, updateCombatMode, sysChatMessage);
                session.Player.EnqueueMovementEvent(motionLifestoneRecall, session.Player.Guid);

                session.Player.SetDelayedTeleport(TimeSpan.FromSeconds(14), sanctuary);
            }
            else
            {
                ChatPacket.SendServerMessage(session, "Your spirit has not been attuned to a sanctuary location.", ChatMessageType.Broadcast);
            }
        }
    }
}