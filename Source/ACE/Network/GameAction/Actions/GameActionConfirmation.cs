using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionConfirmation
    {
        [GameAction(GameActionType.ConfirmationResponse)]
        public static void Handle(ClientMessage clientMessage, Session session)
        {
            // TODO: implement
        }
    }
}
