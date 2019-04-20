using ACE.Server.Network.Enum;
using System;

namespace ACE.Server.Network
{
    public class SessionTerminationDetails
    {
        public string ExtraReason { get; set; } = null;
        public SessionTerminationPhase TerminationStatus { get; set; } = SessionTerminationPhase.Initialized;
        public SessionTerminationReason Reason { get; set; } = SessionTerminationReason.None;
        public long TerminationStartTicks { get; set; } = DateTime.UtcNow.Ticks;
        public long TerminationEndTicks { get; set; } = new DateTime(DateTime.UtcNow.Ticks).AddSeconds(2).Ticks;
    }
}
