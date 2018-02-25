using System.Collections.Generic;

namespace ACE.Server.Physics.Combat
{
    public class TargetManager
    {
        public PhysicsObj PhysObj;
        public TargetInfo TargetInfo;
        public HashSet<TargettedVoyeurInfo> VoyeurTable;
        public double LastUpdateTime;

        public void SetTarget(int contextID, int objectID, float radius, double quantum)
        {

        }

        public void SetTargetQuantum(double quantum)
        {

        }
    }
}
