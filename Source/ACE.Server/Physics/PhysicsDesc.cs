using System.Collections.Generic;
using System.Numerics;
using ACE.Entity.Enum;
using ACE.Server.Physics.Common;

namespace ACE.Server.Physics
{
    public class PhysicsDesc
    {
        public int Bitfield;
        public object MovementBuffer;
        public bool AutonomousMovement;
        public uint AnimFrameID;
        public Position Pos;
        public PhysicsState State;
        public float ObjectScale;
        public float Friction;
        public float Elasticity;
        public float Translucency;
        public Vector3 Velocity;
        public Vector3 Acceleration;
        public Vector3 Omega;
        public uint NumChildren;
        public List<int> ChildIDs;
        public List<int> ChildLocationIDs;
        public uint ParentID;
        public uint LocationID;
        public uint MTableID;
        public uint STableID;
        public uint PhsTableID;
        public PlayScript DefaultScript;
        public float DefaultScriptIntensity;
        public uint SetupID;
        public int[] Timestamps;

        public uint GetMTableID()
        {
            return 0;
        }

        public bool get_autonomous_movement()
        {
            return AutonomousMovement;
        }

        public uint get_animframe_id()
        {
            return AnimFrameID;
        }

        public int get_timestamp(int timeStampIdx)
        {
            return Timestamps[timeStampIdx];
        }
    }
}
