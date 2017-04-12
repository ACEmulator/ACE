using ACE.Network.GameMessages;
using System;

namespace ACE.Entity.Events
{
    public class OutboundEventArgs : EventArgs
    {
        public GameMessage Message { get; set; }

        public BroadcastAction BroadcastType { get; set; }
    }
}
