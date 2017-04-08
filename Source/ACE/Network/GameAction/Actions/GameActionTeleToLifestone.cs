using ACE.Common.Extensions;
using ACE.Database;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Managers;
using ACE.Network.GameEvent;
using ACE.Network.GameEvent.Events;
using ACE.Network.GameMessages.Messages;
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
                session.Player.Teleport(session.Player.Positions[PositionType.Sanctuary]);
            }
            else
            {
                ChatPacket.SendServerMessage(session, "Your spirit has not been attuned to a sanctuary location.", ChatMessageType.Broadcast);
            }
        }
    }
}