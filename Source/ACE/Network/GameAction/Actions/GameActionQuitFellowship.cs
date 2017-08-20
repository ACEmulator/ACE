using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE.Network.GameMessages.Messages;
using ACE.Common.Extensions;

namespace ACE.Network.GameAction.Actions
{
    public class GameActionQuitFellowship
    {
        [GameAction(GameActionType.QuitFellowship)]
        public static void Handle(ClientMessage message, Session session)
        {
            session.Network.EnqueueSend(new GameMessageFellowshipQuit(session));
        }
    }
}
