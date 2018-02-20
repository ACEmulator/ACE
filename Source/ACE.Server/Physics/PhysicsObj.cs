using System.Collections.Generic;
using System.Numerics;
using ACE.Entity.Enum;
using ACE.Server.Physics.Animation;
using ACE.Server.Physics.Collision;
using ACE.Server.Physics.Combat;
using ACE.Server.Physics.Common;
using ACE.Server.Physics.Sound;

namespace ACE.Server.Physics
{
    public class PhysicsObj
    {
        public List<PhysicsPart> Parts;
        public Vector3 PlayerVector;
        public float PlayerDistance;
        public float CYpt;
        public SoundTable SoundTable;
        public bool bExaminationObject;
        public ScriptManager ScriptManager;
        public PhysicsScriptTable PhysicsScriptTable;
        public PlayScript DefaultScript;
        public float DefaultScriptIntensity;
        public PhysicsObj Parent;
        public List<PhysicsObj> Children;
        public Vector3 Position;
        public ObjCell Cell;
        public int NumShadowObjects;
        public List<int> ShadowObjects;
        public int State;
        public int TransientState;
        public float Elasticity;
        public float Translucency;
        public float TranslucencyOriginal;
        public float Friction;
        public float MassInv;
        public MovementManager MovementManager;
        public PositionManager PositionManager;
        public int LastMoveWasAutonomous;
        public int JumpedThisFrame;
        public double UpdateTime;
        public Vector3 VelocityVector;
        public Vector3 AccelerationVector;
        public Vector3 OmegaVector;
        public PhysicsObjHook Hooks;
        public List<AnimHook> AnimHooks;
        public float Scale;
        public float AttackRadius;
        public DetectionManager DetectionManager;
        public AttackManager AttackManager;
        public TargetManager TargetManager;
        public ParticleManager ParticleManager;
        public WeenieObject WeenieObj;
        public Plane ContactPlane;
        public int ContactPlaneCellID;
        public Vector3 SlidingNormal;
        public Vector3 CachedVelocity;
        public Dictionary<long, CollisionRecord> CollisionTable;
        public int CollidingWithEnvironment;
        public short[] UpdateTimes;

        public PhysicsObj()
        {
        }
    }
}
