using System.Numerics;
using ACE.Server.Physics.Common;

namespace ACE.Server.Physics.Animation
{
    public enum InsertType
    {
        TRANSITION_INSERT = 0x0,
        PLACEMENT_INSERT = 0x1,
        INITIAL_PLACEMENT_INSERT = 0x2,
        FORCE_InsertType_32_BIT = 0x7FFFFFFF,
    };
    public class SpherePath
    {
        public int NumSphere;
        public Sphere LocalSphere;
        public Vector3 LocalLowPoint;
        public Sphere GlobalSphere;
        public Vector3 GlobalLowPoint;
        public Sphere LocalSpaceSphere;
        public Vector3 LocalSpaceLowPoint;
        public Vector3 LocalSpaceCurrCenter;
        public Vector3 GlobalCurrCenter;
        public Position LocalSpacePos;
        public Vector3 LocalSpaceZ;
        public ObjCell BeginCell;
        public Position BeginPos;
        public Position EndPos;
        public ObjCell CurCell;
        public Position CurPos;
        public Vector3 GlobalOffset;
        public int StepUp;
        public Vector3 StepUpNormal;
        public int Collide;
        public ObjCell CheckCell;
        public Position CheckPos;
        public InsertType InsertType;
        public int StepDown;
        public InsertType Backup;
        public Position BackupCheckPos;
        public int ObstructionEthereal;
        public int HitsInteriorCell;
        public int BldgCheck;
        public float WalkableAllowance;
        public float WalkInterp;
        public float StepDownAmt;
        public Sphere WalkableCheckPos;
        public Polygon Walkable;
        public int CheckWalkable;
        public Vector3 WalkableUp;
        public Position WalkablePos;
        public float WalkableScale;
        public int CellArrayValid;
        public int NegStepUp;
        public Vector3 NegCollisionNormal;
        public int NegPollyHit;
        public int PlacementAllowsSliding;
    }
}
