using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Network.GameEvent.Events
{
    public class GameEventConfirmationRequest : GameEventMessage
    {
        public GameEventConfirmationRequest(Session session, string message)
            : base(GameEventType.CharacterConfirmationRequest, GameMessageGroup.Group09, session)
        {
            // TODO: implement
            // TODO: verify GameMessageGroup with pcap data
        }
    }
}
