using ACE.Network.GameEvent;
using System;

namespace ACE.Entity.Events
{
    public class OutboundEventArgs : EventArgs
    {
        public GameEventMessage EventMessage { get; set; }

        public BroadcastAction ActionType { get; set; }
    }
}
