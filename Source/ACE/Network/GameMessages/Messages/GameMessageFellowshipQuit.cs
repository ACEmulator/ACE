using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessageFellowshipQuit : GameMessage
    {
        public GameMessageFellowshipQuit(Session session)
            : base(GameMessageOpcode.GameEvent, GameMessageGroup.Group09)
        {
            Writer.Write(session.Player.Guid.Full);
            Writer.Write(session.GameEventSequence++);
            Writer.Write((uint)GameEvent.GameEventType.FellowshipQuit);
            Writer.Write(session.Player.Guid.Full);
        }
    }
}
