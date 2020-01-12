using System;
using System.Collections.Generic;
using System.Text;

namespace ACE.Server.Network.GameEvent.Events
{
    class GameEventAllegianceLoginNotification : GameEventMessage
    {
        public GameEventAllegianceLoginNotification(Session session, uint playerGuid, bool isLoggedIn)
            : base (GameEventType.AllegianceLoginNotification, GameMessageGroup.UIQueue, session)
        {
            Writer.Write(playerGuid);
            Writer.Write(Convert.ToUInt32(isLoggedIn));
        }
    }
}
