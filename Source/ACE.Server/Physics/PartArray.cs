using System.Collections.Generic;
using System.Numerics;
using ACE.Server.Physics.Animation;

namespace ACE.Server.Physics
{
    public class PartArray
    {
        public int PAState;
        public PhysicsObj Owner;
        public Sequence Sequence;
        public MotionTableManager MotionTableManager;
        public Setup Setup;
        public int NumParts;
        public List<PhysicsPart> Parts;
        public Vector3 Scale;
        public AnimFrame LastAnimFrame;
    }
}
