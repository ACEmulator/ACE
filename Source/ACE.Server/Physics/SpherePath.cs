using System;
using System.Collections.Generic;
using System.Numerics;
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
        public List<Sphere> LocalSphere;            // 4
        public Vector3 LocalLowPoint;               // 8
        public List<Sphere> GlobalSphere;           // 12
        public Vector3 GlobalLowPoint;              // 16
        public List<Sphere> LocalSpaceSphere;       // 20
        public Vector3 LocalSpaceLowPoint;          // 24
        public List<Sphere> LocalSpaceCurrCenter;   // 28
        public List<Sphere> GlobalCurrCenter;       // 32
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
        public bool StepDown;                       // 89
        public InsertType Backup;                   // 90
        public ObjCell BackupCell;                  // 94
        public Position BackupCheckPos;             // 98
        public bool ObstructionEthereal;            // 102
        public bool HitsInteriorCell;               // 103
        public bool BuildingCheck;                  // 104
        public float WalkableAllowance;             // 105
        public float WalkInterp;                    // 111 *
        public float StepDownAmt;                   // 107
        public Sphere WalkableCheckPos;             // 111
        public Polygon Walkable;                    // 115
        public bool CheckWalkable;                  // 119
        public Vector3 WalkableUp;                  // 120
        public Position WalkablePos;                // 124
        public float WalkableScale;                 // 128
        public bool CellArrayValid;                 // 132
        public bool NegStepUp;                      // 133
        public Vector3 NegCollisionNormal;          // 134
        public bool NegPolyHit;                     // 138
        public bool PlacementAllowsSliding;         // 139

        public SpherePath()
        {
            LocalSpacePos = new Position();
            CurPos = new Position();
            CheckPos = new Position();
            BackupCheckPos = new Position();
            WalkableCheckPos = new Sphere();
            //NumSphere = 2;

            LocalSphere = new List<Sphere>();
            GlobalSphere = new List<Sphere>();
            LocalSpaceSphere = new List<Sphere>();

            LocalSpaceCurrCenter = new List<Sphere>();
            GlobalCurrCenter = new List<Sphere>();

            Init();
        }

        public void Init()
        {
            PlacementAllowsSliding = true;
        }

        public void InitPath(ObjCell beginCell, Position beginPos, Position endPos)
        {
            BeginPos = beginPos;
            BeginCell = beginCell;
            EndPos = endPos;

            if (beginPos != null)
            {
                InsertType = InsertType.Transition;
                CurPos = new Position(beginPos);
            }
            else
            {
                InsertType = InsertType.Placement;
                CurPos = new Position(endPos);
            }

            CurCell = beginCell;
            CacheGlobalCurrCenter();
        }

        public void InitSphere(int numSphere, List<Sphere> spheres, float scale)
        {
            if (numSphere <= 2)
                NumSphere = numSphere;
            else
                NumSphere = 2;

            for (var i = 0; i < NumSphere; i++)
                LocalSphere.Add(new Sphere(spheres[i].Center * scale, spheres[i].Radius * scale));

            // are these inited elsewhere,
            // or should they be created here?
            LocalLowPoint = LocalSphere[0].Center;
            LocalLowPoint.Z -= LocalSphere[0].Radius;
        }

        public void AddOffsetToCheckPos(Vector3 offset)
        {
            CellArrayValid = false;
            CheckPos.Frame.Origin += offset;
            CacheGlobalSphere(offset);
        }

        public void AddOffsetToCheckPos(Vector3 offset, float radius)
        {
            AddOffsetToCheckPos(offset);    // radius ignored?
        }

        public void AdjustCheckPos(uint cellID)
        {
            if ((cellID & 0xFFFF) < 0x100)
            {
                var offset = LandDefs.GetBlockOffset(cellID, CheckPos.ObjCellID);
                CacheGlobalSphere(offset);
                CheckPos.Frame.Origin += offset;
            }
            CheckPos.ObjCellID = cellID;
        }

        public void CacheGlobalCurrCenter()
        {
            while (GlobalCurrCenter.Count < NumSphere)
                GlobalCurrCenter.Add(new Sphere());

            for (var i = 0; i < NumSphere; i++)
                GlobalCurrCenter[i].Center = CurPos.LocalToGlobal(LocalSphere[i].Center);
        }

        /// <summary>
        /// Converts the local sphere to global space
        /// relative to checkPos offset
        /// </summary>
        public void CacheGlobalSphere(Vector3? offset)
        {
            if (offset != null)
            {
                foreach (var globSphere in GlobalSphere)
                    globSphere.Center += offset.Value;

                GlobalLowPoint += offset.Value;
            }
            else
            {
                while (GlobalSphere.Count < NumSphere)
                    GlobalSphere.Add(new Sphere());

                for (var i = 0; i < NumSphere; i++)
                {
                    GlobalSphere[i].Radius = LocalSphere[i].Radius;
                    GlobalSphere[i].Center = CheckPos.LocalToGlobal(LocalSphere[i].Center);
                }
                GlobalLowPoint = CheckPos.LocalToGlobal(LocalLowPoint);
            }
        }

        public void CacheLocalSpaceSphere(Position pos, float scaleZ)
        {
            var invScale = 1.0f / scaleZ;

            while (LocalSpaceCurrCenter.Count < NumSphere)
                LocalSpaceCurrCenter.Add(new Sphere());

            while (LocalSpaceSphere.Count < NumSphere)
                LocalSpaceSphere.Add(new Sphere());

            for (var i = 0; i < NumSphere; i++)
            {
                // pos = the cell location in global space
                // curpos = the current player position in global space
                // localsphere = the sphere relative to the player

                // localspacecurrcenter = curpos in local space
                // localspacesphere = checkpos in local space
                LocalSpaceCurrCenter[i].Center = pos.LocalToLocal(CurPos, LocalSphere[i].Center) * invScale;

                LocalSpaceSphere[i].Radius = LocalSphere[i].Radius * invScale;
                LocalSpaceSphere[i].Center = pos.LocalToLocal(CheckPos, LocalSphere[i].Center) * invScale;
            }
            LocalSpacePos = new Position(pos);
            LocalSpaceZ = pos.GlobalToLocalVec(Vector3.UnitZ);
            LocalSpaceLowPoint = LocalSpaceSphere[0].Center - (LocalSpaceZ * LocalSpaceSphere[0].Radius);
        }

        public bool CheckWalkables()
        {
            if (Walkable == null) return true;

            var walkCheckPos = new Sphere(WalkableCheckPos.Center, WalkableCheckPos.Radius * 0.5f);
            return Walkable.check_walkable(walkCheckPos, WalkableUp);
        }

        public Vector3 GetCurPosCheckPosBlockOffset()
        {
            return LandDefs.GetBlockOffset(CurPos.ObjCellID, CheckPos.ObjCellID);
        }

        public Position GetWalkablePos()
        {
            return WalkablePos;
        }

        public bool IsWalkableAllowable(float zval)
        {
            return zval > WalkableAllowance;
        }

        public TransitionState PrecipiceSlide(Transition transition)
        {
            var collisions = transition.CollisionInfo;
            Vector3 collisionNormal = Vector3.Zero;
            var found = Walkable.find_crossed_edge(WalkableCheckPos, WalkableUp, ref collisionNormal);

            if (!found)
            {
                Walkable = null;
                return TransitionState.Collided;
            }

            Walkable = null;
            StepUp = false;
            collisionNormal = WalkablePos.Frame.LocalToGlobalVec(collisionNormal);

            var blockOffset = LandDefs.GetBlockOffset(CurPos.ObjCellID, CheckPos.ObjCellID);
            var offset = GlobalSphere[0].Center - GlobalCurrCenter[0].Center + blockOffset;

            if (Vector3.Dot(collisionNormal, offset) > 0.0f)
                collisionNormal *= -1.0f;

            return GlobalSphere[0].SlideSphere(transition, ref collisionNormal, GlobalCurrCenter[0].Center);
        }

        public void RestoreCheckPos()
        {
            CheckPos = new Position(BackupCheckPos);
            CheckCell = BackupCell;
            CellArrayValid = false;
            CacheGlobalSphere(null);
        }

        public void SaveCheckPos()
        {
            BackupCell = CheckCell;
            BackupCheckPos = new Position(CheckPos);
        }

        public void SetCheckPos(Position position, ObjCell cell)
        {
            CheckPos = new Position(position);
            CheckCell = cell;
            CellArrayValid = false;
            CacheGlobalSphere(null);
        }

        public void SetCollide(Vector3 collisionNormal)
        {
            Collide = true;
            BackupCell = CheckCell;
            BackupCheckPos = new Position(CheckPos);
            StepUpNormal = new Vector3(collisionNormal.X, collisionNormal.Y, collisionNormal.Z);
            WalkInterp = 1.0f;
        }

        public void SetNegPolyHit(bool stepUp, Vector3 collisionNormal)
        {
            NegStepUp = stepUp;
            NegPolyHit = true;
            NegCollisionNormal = -collisionNormal;
        }

        public void SetWalkable(Sphere sphere, Polygon poly, Vector3 zAxis, Position localPos, float scale)
        {
            WalkableCheckPos = new Sphere(sphere);
            Walkable = poly;
            WalkableUp = zAxis;
            WalkablePos = new Position(localPos);
            WalkableScale = scale;
        }

        public void SetWalkableCheckPos(Sphere sphere)
        {
            WalkableCheckPos = new Sphere(sphere);
        }

        public TransitionState StepUpSlide(Transition transition)
        {
            var collisions = transition.CollisionInfo;

            collisions.ContactPlaneValid = false;
            collisions.ContactPlaneIsWater = false;

            return GlobalSphere[0].SlideSphere(transition, ref StepUpNormal, GlobalCurrCenter[0].Center);
        }
    }
}
