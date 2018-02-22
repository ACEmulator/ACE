using System.Collections.Generic;
using System.Numerics;
using ACE.Server.Physics.Collision;
using ACE.Server.Physics.Common;

namespace ACE.Server.Physics.Animation
{
    public enum InsertType
    {
        TransitionInsert = 0x0,
        PlacementInsert = 0x1,
        InitialPlacementInsert = 0x2,
    };
    public class SpherePath
    {
        public int NumSphere;                       // 4
        public Sphere LocalSphere;                  // 8
        public Vector3 LocalLowPoint;               // 12
        public List<Sphere> GlobalSphere;           // 16
        public Vector3 GlobalLowPoint;              // 20
        public Sphere LocalSpaceSphere;             // 24
        public Vector3 LocalSpaceLowPoint;          // 28
        public Vector3 LocalSpaceCurrCenter;        // 32
        public List<Vector3> GlobalCurrCenter;      // 36
        public Position LocalSpacePos;              // 40
        public Vector3 LocalSpaceZ;                 // 44
        public ObjCell BeginCell;                   // 48
        public Position BeginPos;                   // 52
        public Position EndPos;                     // 56
        public ObjCell CurCell;                     // 60 
        public Position CurPos;                     // 64
        public Vector3 GlobalOffset;                // 68
        public bool StepUp;                         // 72
        public Vector3 StepUpNormal;                // 76
        public bool Collide;                        // 80
        public ObjCell CheckCell;                   // 84
        public Position CheckPos;                   // 88
        public InsertType InsertType;               // 92
        public int StepDown;                        // 96
        public InsertType Backup;                   // 100
        public ObjCell BackupCell;                  // 104
        public Position BackupCheckPos;             // 108
        public bool ObstructionEthereal;            // 112
        public int HitsInteriorCell;                // 116
        public int BldgCheck;                       // 120
        public float WalkableAllowance;             // 124
        public float WalkInterp;
        public float StepDownAmt;
        public Sphere WalkableCheckPos;
        public Polygon Walkable;
        public int CheckWalkable;
        public Vector3 WalkableUp;
        public Position WalkablePos;
        public float WalkableScale;
        public bool CellArrayValid;
        public int NegStepUp;
        public Vector3 NegCollisionNormal;
        public int NegPollyHit;
        public int PlacementAllowsSliding;

        public SpherePath()
        {
            CurPos = new Position();
            CheckPos = new Position();
        }

        public Vector3 GetCurPosCheckPosBlockOffset()
        {
            return Vector3.Zero;
        }

        public bool IsWalkableAllowable(float zval)
        {
            return zval > WalkableAllowance;
        }

        public void AddOffsetToCheckPos(Vector3 offset, float radius)
        {
            CellArrayValid = false;
            CheckPos.Frame.Origin += offset;
            CacheGlobalSphere(offset);
        }

        public void CacheGlobalSphere(Vector3 offset)
        {

        }

        public void SetCollide(Vector3 collisionNormal)
        {
            Collide = true;
            BackupCell = CheckCell;
            BackupCheckPos.ObjCellID = CheckPos.ObjCellID;
            //CheckPos.Frame;   //operator=
            StepUpNormal = collisionNormal;
            WalkInterp = 0; // 1065353216 & 0x0000FFFF;
        }

        public TransitionState StepUpSlide(ObjectInfo obj, CollisionInfo collisions)
        {
            return TransitionState.OK;
        }
    }
}
