using System.Collections.Generic;

namespace ACE.Server.Physics.Combat
{
    public class TargetManager
    {
        public PhysicsObj PhysObj;
        public TargetInfo TargetInfo;
        public HashSet<TargettedVoyeurInfo> VoyeurTable;
        public double LastUpdateTime;

        public TargetManager()
        {

        }

        public TargetManager(PhysicsObj physObj)
        {
            PhysObj = physObj;
        }

        public void SetTarget(int contextID, int objectID, float radius, double quantum)
        {

        }

        public void SetTargetQuantum(double quantum)
        {

        }

        public void AddVoyeur(int objectID, float radius, double quantum)
        {

        }

        public void HandleTargetting()
        {

        }

        public void ClearTarget()
        {

        }

        public void NotifyVoyeurOfEvent(TargetStatus status)
        {

        }

        public void ReceiveUpdate(TargetInfo info)
        {

        }

        public bool RemoveVoyeur(int objectID)
        {
            return false;
        }
    }
}
