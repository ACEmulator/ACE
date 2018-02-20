using System.Collections.Generic;
using System.Numerics;
using ACE.Server.Physics.Common;

namespace ACE.Server.Physics
{
    public enum PhysicsDescInfo
    {
        CSetup = 0x1,
        MTABLE = 0x2,
        VELOCITY = 0x4,
        ACCELERATION = 0x8,
        OMEGA = 0x10,
        PARENT = 0x20,
        CHILDREN = 0x40,
        OBJSCALE = 0x80,
        FRICTION = 0x100,
        ELASTICITY = 0x200,
        TIMESTAMPS = 0x400,
        STABLE = 0x800,
        PETABLE = 0x1000,
        DEFAULT_SCRIPT = 0x2000,
        DEFAULT_SCRIPT_INTENSITY = 0x4000,
        POSITION = 0x8000,
        MOVEMENT = 0x10000,
        ANIMFRAME_ID = 0x20000,
        TRANSLUCENCY = 0x40000,
        FORCE_PhysicsDescInfo_32_BIT = 0x7FFFFFFF,
    };

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
        public PScriptType DefaultScript;
        public float DefaultScriptIntensity;
        public int SetupID;
        public int[] Timestamps;
    }
}
