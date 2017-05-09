using ACE.Entity;
using ACE.Network.Sequence;

namespace ACE.Network.Motion
{
    public abstract class MotionState
    {
        public bool IsAutonomous { get; set; }

        public virtual byte[] GetPayload(ObjectGuid animationTargetGuid, SequenceManager sequence)
        {
            return null;
        }
    }
}
