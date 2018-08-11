using System.Collections.Generic;
using System.Numerics;
using ACE.Server.Physics.Common;

namespace ACE.Server.Physics.Combat
{
    public class DetectionManager
    {
        public PhysicsObj PhysObj;
        public HashSet<DetectionInfo> DetectionObjects;
        public int NumPendingGlobalDetectUpdates;
        public List<ObjCell> CellArray;
        public double LastUpdateTime;
        public Vector3 LastGlobalUpdate;
        public HashSet<DetectionCylsphere> DetectionTable;
        public List<long> PendingDeletions;

        public void CheckDetection()
        {

        }

        public void ReceiveDetectionUpdate(DetectionInfo info)
        {

        }

        public void DestroyDetectionCylsphere(int contextID)
        {

        }
    }
}
