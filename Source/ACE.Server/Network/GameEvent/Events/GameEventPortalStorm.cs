using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Server.Network.GameEvent.Events
{
    class GameEventPortalStorm : GameEventMessage
    {
        public GameEventPortalStorm(Session session)
            : base(GameEventType.MiscPortalStorm, GameMessageGroup.UIQueue, session)
        {
        }
    }
}
