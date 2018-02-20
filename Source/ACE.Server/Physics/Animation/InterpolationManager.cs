using System.Collections.Generic;
using ACE.Server.Physics.Common;

namespace ACE.Server.Physics.Animation
{
    public class InterpolationManager
    {
        public List<InterpolationNode> PositionQueue;
        public PhysicsObj PhysicsObj;
        public int KeepHeading;
        public int FrameCounter;
        public float OriginalDistance;
        public float ProgressQuantum;
        public int NodeFailCounter;
        public Position BlipToPosition;
    }
}
