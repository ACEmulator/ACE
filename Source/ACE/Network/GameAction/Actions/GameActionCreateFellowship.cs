using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE.Network.GameMessages.Messages;
using ACE.Common.Extensions;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionCreateFellowship
    {
        [GameAction(GameActionType.CreateFellowship)]
        public static void Handle(ClientMessage message, Session session)
        {
            // Should create new Fellowship with 1 member
            // todo: option for sharing XP

            var fellowshipName = message.Payload.ReadString16L();
            bool shareXp = message.Payload.ReadUInt32() > 0;

            session.Player.CreateFellowship(fellowshipName, shareXp);
            session.Network.EnqueueSend(new GameMessageFellowshipFullUpdate(session));            
        }
    }
}
