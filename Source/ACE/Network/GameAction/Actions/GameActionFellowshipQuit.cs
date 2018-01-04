using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE.Network.GameMessages.Messages;
using ACE.Common.Extensions;
using ACE.Managers;

namespace ACE.Network.GameAction.Actions
{
    public class GameActionFellowshipQuit
    {
        [GameAction(GameActionType.FellowshipQuit)]
        public static void Handle(ClientMessage message, Session session)
        {
            bool disbandFellowship = message.Payload.ReadUInt32() > 0;

            if (disbandFellowship && session.Player.Guid.Full == session.Player.Fellowship)
            {
                FellowshipManager.Disband(session.Player.Fellowship);
            }
            else
            {
                FellowshipManager.QuitFellowship(session.Player.Fellowship, session.Player);
            }
            
        }
    }
}
