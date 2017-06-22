using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Network.Enum;
using ACE.Network.GameEvent.Events;
using ACE.Network.GameMessages.Messages;
using ACE.Network.Motion;
using System;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionTeleToMarketPlace
    {
        [GameAction(GameActionType.TeleToMarketPlace)]
        public static void Handle(ClientMessage clientMessage, Session session)
        {
            session.Player.HandleActionTeleToMarketplace();
        }
    }
}