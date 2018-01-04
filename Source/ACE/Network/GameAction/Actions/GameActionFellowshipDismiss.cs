using ACE.Managers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionFellowshipDismiss
    {
        [GameAction(GameActionType.FellowshipDismiss)]
        public static void Handle(ClientMessage message, Session session)
        {
            if (session.Player.Fellowship != 0)
            {
                FellowshipManager.DismissPlayer(session.Player.Fellowship, session.Player);
            }
        }
    }
}
