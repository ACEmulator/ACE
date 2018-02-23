using System.Collections.Generic;
using System.Numerics;
using ACE.Server.Physics.Collision;
using ACE.Server.Physics.Common;

namespace ACE.Server.Physics.Animation
{
    public enum InsertType
    {
        Transition = 0x0,
        Placement = 0x1,
        InitialPlacement = 0x2,
    };
    public class SpherePath
    {
        public int NumSphere;                       // 0
        public Sphere LocalSphere;                  // 2
        public Vector3 LocalLowPoint;               // 6
        public List<Sphere> GlobalSphere;           // 10
        public Vector3 GlobalLowPoint;              // 14
        public Sphere LocalSpaceSphere;             // 20
        public Vector3 LocalSpaceLowPoint;          // 24
        public Vector3 LocalSpaceCurrCenter;        // 28
        public List<Vector3> GlobalCurrCenter;      // 32
        public Position LocalSpacePos;              // 36
        public Vector3 LocalSpaceZ;                 // 40
        public ObjCell BeginCell;                   // 44
        public Position BeginPos;                   // 48
        public Position EndPos;                     // 52
        public ObjCell CurCell;                     // 56 
        public Position CurPos;                     // 60
        public Vector3 GlobalOffset;                // 64
        public bool StepUp;                         // 68
        public Vector3 StepUpNormal;                // 69
        public bool Collide;                        // 73
        public ObjCell CheckCell;                   // 77
        public Position CheckPos;                   // 81
        public InsertType InsertType;               // 85
        public bool StepDown;                       // 86
        public InsertType Backup;                   // 87
        public ObjCell BackupCell;                  // 88
        public Position BackupCheckPos;             // 92
        public bool ObstructionEthereal;            // 96
        public bool HitsInteriorCell;               // 97
        public bool BldgCheck;                      // 98
        public float WalkableAllowance;             // 99
        public float WalkInterp;                    // 111 *
        public float StepDownAmt;                   // 107
        public Sphere WalkableCheckPos;             // 111
        public Polygon Walkable;                    // 115
        public bool CheckWalkable;                  // 119
        public Vector3 WalkableUp;                  // 120
        public Position WalkablePos;                // 124
        public float WalkableScale;                 // 128
        public bool CellArrayValid;                 // 132
        public int NegStepUp;                       // 136
        public Vector3 NegCollisionNormal;          // 140
        public int NegPollyHit;                     // 144
        public int PlacementAllowsSliding;          // 148

        public SpherePath()
        {
            CurPos = new Position();
            CheckPos = new Position();
            GlobalCurrCenter = new List<Vector3>();
            GlobalSphere = new List<Sphere>();
            BackupCheckPos = new Position();
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

        public TransitionState StepUpSlide(Transition transition)
        {
            return TransitionState.OK;
        }
    }
}
