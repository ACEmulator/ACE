using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Network.Enum;
using ACE.Network.GameMessages.Messages;
using ACE.Network.Motion;
using System;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionTeleToLifestone
    {
        [GameAction(GameActionType.TeleToLifestone)]
        public static void Handle(ClientMessage message, Session session)
        {
            session.Player.RequestAction(() => session.Player.ActLifestone());
        }
    }
}