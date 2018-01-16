using System;
using System.Collections.Generic;
using System.Text;

namespace ACE.Network.GameAction.Actions
{
     
    public static class GameActionFellowshipChangeOpenness
    {
        [GameAction(GameActionType.FellowshipChangeOpenness)]
        public static void Handle(ClientMessage message, Session session)
        {
            var isOpen = message.Payload.ReadUInt32() == 0 ? false : true;
            session.Player.FellowshipSetOpen(isOpen);
        }
    }
}
