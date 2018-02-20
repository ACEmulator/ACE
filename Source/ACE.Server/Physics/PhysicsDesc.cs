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
        public int AutonomousMovement;
        public int AnimFrameID;
        public Position Pos;
        public int State;
        public float ObjectScale;
        public float Friction;
        public float Elasticity;
        public float Translucency;
        public Vector3 Velocity;
        public Vector3 Acceleration;
        public Vector3 Omega;
        public int NumChildren;
        public List<int> ChildIDs;
        public List<int> ChildLocationIDs;
        public int ParentID;
        public int LocationID;
        public int MTableID;
        public int STableID;
        public int PHSTableID;
        public PlayScript DefaultScript;
        public float DefaultScriptIntensity;
        public int SetupID;
        public int[] Timestamps;
    }
}
