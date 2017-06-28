using ACE.Entity;

namespace ACE.Network.Motion
{
    public abstract class MotionState
    {
        public bool IsAutonomous { get; set; }

        public virtual byte[] GetPayload(ObjectGuid guid, Sequence.SequenceManager sequence)
        {
            return null;
        }
    }
}
