using ACE.Server.Network.Enum;

namespace ACE.Server.Network
{
    public class SessionTerminationDetails
    {
        public string ExtraReason { get; set; } = null;
        public SessionTerminationPhase TerminationStatus { get; set; } = SessionTerminationPhase.Initialized;
        public BootReason BootReason { get; set; } = BootReason.None;
    }
}
