using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE.Network.GameMessages.Messages;
namespace ACE.Network.GameAction.Actions
{
    public class GameActionFellowshipUpdateRequest
    {
        [GameAction(GameActionType.FellowshipUpdateRequest)]
        public static void Handle(ClientMessage message, Session session)
        {
            if (session.Player.Fellowship != null)
            {
                session.Network.EnqueueSend(new GameMessageFellowshipFullUpdate(session));
            }
        }
    }
}
