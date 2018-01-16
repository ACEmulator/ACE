using ACE.Network.GameMessages;
using System;
using System.Collections.Generic;
using System.Text;

namespace ACE.Network.GameEvent.Events
{
    public class GameEventFellowshipDisband : GameMessage
    {
        public GameEventFellowshipDisband(Session session) :
            base(GameMessageOpcode.GameEvent, GameMessageGroup.Group09)
        {
            Writer.Write(session.Player.Guid.Full);
            Writer.Write(session.GameEventSequence++);
            Writer.Write((uint)GameEvent.GameEventType.FellowshipDisband);
        }
    }
}
