using System.Collections.Generic;

namespace ACE.Server.Physics.Combat
{
    public class TargetManager
    {
        public PhysicsObj PhysObj;
        public TargetInfo TargetInfo;
        public HashSet<TargettedVoyeurInfo> VoyeurTable;
        public double LastUpdateTime;
    }
}
