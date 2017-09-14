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
        public static void Handle(ClientMessage message, Session session)
        {
            int confirmType = message.Payload.ReadInt32();
            uint context = message.Payload.ReadUInt32();
            int response = message.Payload.ReadInt32();

            // TODO: do something
        }
    }
}
