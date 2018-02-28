using System;
using System.Collections.Generic;
using System.Numerics;
using ACE.Server.Physics.Collision;
using ACE.Server.Physics.Common;

namespace ACE.Server.Physics.Animation
{
    public enum TransitionState
    {
        Invalid   = 0x0,
        OK        = 0x1,
        Collided  = 0x2,
        Adjusted  = 0x3,
        Slid      = 0x4
    };
    public class Transition
    {
        public ObjectInfo ObjectInfo;
        public SpherePath SpherePath;
        public CollisionInfo CollisionInfo;
        public CellArray CellArray;
        public ObjCell NewCellPtr;

        public static int TransitionLevel;

        public Transition()
        {
            Init();
        }

        public Vector3 AdjustOffset(Vector3 offset)
        {
            var collisionAngle = Vector3.Dot(offset, CollisionInfo.SlidingNormal);

            if (CollisionInfo.SlidingNormalValid && collisionAngle >= 0.0f)
                CollisionInfo.SlidingNormalValid = false;

            if (!CollisionInfo.ContactPlaneValid)
                return offset - CollisionInfo.SlidingNormal * collisionAngle;

            var slideAngle = Vector3.Dot(CollisionInfo.ContactPlane.Normal, CollisionInfo.SlidingNormal);
            if (CollisionInfo.SlidingNormalValid)
            {
                var contactSlide = Vector3.Cross(CollisionInfo.ContactPlane.Normal, CollisionInfo.SlidingNormal);
                if (!CollisionInfo.NormalizeCheckSmall(ref contactSlide))
                    offset = Vector3.Dot(contactSlide, offset) * contactSlide;
                else
                    offset = Vector3.Zero;
            }
            else if (slideAngle <= 0.0f)
                offset -= CollisionInfo.ContactPlane.Normal * slideAngle;
            else
                CollisionInfo.ContactPlane.SnapToPlane(offset);

            if (CollisionInfo.ContactPlaneCellID == 0 || CollisionInfo.ContactPlaneIsWater)
                return offset;

            var globSphere = SpherePath.GlobalSphere[0];
            var blockOffset = LandDefs.GetBlockOffset(SpherePath.CheckPos.ObjCellID, CollisionInfo.ContactPlaneCellID);
            var distSq = Vector3.Dot(globSphere.Center - blockOffset, CollisionInfo.ContactPlane.Normal);
            if (distSq >= globSphere.Radius - PhysicsGlobals.EPSILON)
                return offset;

            var zDist = (globSphere.Radius - distSq) / CollisionInfo.ContactPlane.Normal.Z;
            if (globSphere.Radius > Math.Abs(zDist))
            {
                offset = new Vector3(0.0f, 0.0f, zDist);
                SpherePath.AddOffsetToCheckPos(offset);
            }
            return offset;
        }

        public List<ObjCell> BuildCellArray()
        {
            SpherePath.CellArrayValid = true;
            SpherePath.HitsInteriorCell = false;

            return CellArray.find_cell_list(SpherePath);
        }

        public void CacheLocalSpaceSphere(Position pos, float scaleZ)
        {

        }

        public void CalcNumSteps(ref Vector3 offset, ref Vector3 offsetPerStep, ref int numSteps)
        {
            if (SpherePath.BeginPos == null)
            {
                offset = Vector3.Zero;
                offsetPerStep = Vector3.Zero;
                numSteps = 1;
                return;
            }
            offset = SpherePath.BeginPos.GetOffset(SpherePath.EndPos);
            var dist = offset.Length();
            var step = dist / SpherePath.LocalSphere[0].Radius;

            if (!ObjectInfo.State.HasFlag(ObjectInfoState.IsViewer))
            {
                if (step > 1.0f)
                {
                    numSteps = (int)Math.Ceiling(step);
                    offsetPerStep = offset * (1.0f / numSteps);
                }
                else if (!offset.Equals(Vector3.Zero))
                {
                    offsetPerStep = offset;
                    numSteps = 1;
                }
                else
                {
                    offsetPerStep = Vector3.Zero;
                    numSteps = 0;
                }
                return;
            }

            if (dist < PhysicsGlobals.EPSILON)
            {
                offsetPerStep = Vector3.Zero;
                numSteps = 0;
            }
            else
            {
                offsetPerStep = offset * (1.0f / step);
                numSteps = (int)Math.Floor(step) + 1;
            }
        }

        public bool CheckCollisions(PhysicsObj obj)
        {
            SpherePath.InsertType = InsertType.Placement;
            SpherePath.CheckPos.ObjCellID = SpherePath.CurPos.ObjCellID;
            SpherePath.CheckPos.Frame = SpherePath.CurPos.Frame;
            SpherePath.CheckCell = SpherePath.CurCell;
            SpherePath.CellArrayValid = false;
            SpherePath.CacheGlobalSphere(Vector3.Zero);

            return obj.FindObjCollisions(this) != TransitionState.OK;
        }

        public TransitionState CheckOtherCells(ObjCell currCell)
        {
            var result = TransitionState.OK;

            SpherePath.CellArrayValid = true;
            SpherePath.HitsInteriorCell = false;
            var newCell = CellArray.find_cell_list(SpherePath)[0];

            foreach (var cell in CellArray.Cells)
            {
                if (cell == null || cell.Equals(currCell)) continue;
                var collides = cell.FindCollisions(this);
                switch (collides)
                {
                    case TransitionState.Slid:
                        CollisionInfo.ContactPlaneValid = false;
                        CollisionInfo.ContactPlaneIsWater = false;
                        return collides;

                    case TransitionState.Collided:
                    case TransitionState.Adjusted:
                        return collides;
                }
            }

            SpherePath.CheckCell = newCell;
            if (newCell != null)
            {
                SpherePath.AdjustCheckPos(newCell.ID);
                return result;
            }
            if (SpherePath.StepDown) return TransitionState.Collided;

            var checkPos = SpherePath.CheckPos;
            if (checkPos.ObjCellID < 0x100)
                LandDefs.AdjustToOutside(checkPos);

            if (checkPos.ObjCellID != 0)
            {
                SpherePath.AdjustCheckPos(checkPos.ObjCellID);
                SpherePath.SetCheckPos(checkPos, null);

                SpherePath.CellArrayValid = true;

                return result;
            }
            return TransitionState.Collided;
        }

        public bool CheckWalkable(float zCheck)
        {
            return false;
        }

        public void CleanupTransition()
        {
            --TransitionLevel;
        }

        public bool CliffSlide(Plane contactPlane)
        {
            return false;
        }

        public bool EdgeSlide(TransitionState transitionState, float stepDownHeight, float zVal)
        {
            return false;
        }

        public bool FindPlacementPos()
        {
            return false;
        }

        public bool FindPlacementPosition()
        {
            return false;
        }

        public bool FindTransitionalPosition()
        {
            return false;
        }

        public bool FindValidPosition()
        {
            return false;
        }

        public void Init()
        {
            ObjectInfo = new ObjectInfo();
            SpherePath = new SpherePath();
            CollisionInfo = new CollisionInfo();
            CellArray = new CellArray();
            NewCellPtr = new ObjCell();
        }

        public void InitContactPlane(int cellID, Plane contactPlane, bool isWater)
        {
            InitLastKnownContactPlane(cellID, contactPlane, isWater);

            CollisionInfo.ContactPlaneValid = true;
            CollisionInfo.ContactPlane = contactPlane;
            CollisionInfo.ContactPlaneIsWater = isWater;
            CollisionInfo.ContactPlaneCellID = cellID;
        }

        public void InitLastKnownContactPlane(int cellID, Plane contactPlane, bool isWater)
        {
            CollisionInfo.LastKnownContactPlaneValid = true;
            CollisionInfo.LastKnownContactPlane = contactPlane;
            CollisionInfo.LastKnownContactPlaneIsWater = isWater;
            CollisionInfo.LastKnownContactPlaneCellID = cellID;
        }

        public void InitObject(PhysicsObj obj, ObjectInfoState objectState)
        {
            ObjectInfo.Init(obj, objectState);
        }

        public void InitPath(ObjCell beginCell, Position beginPos, Position endPos)
        {
            SpherePath.InitPath(beginCell, beginPos, endPos);
        }

        public void InitSlidingNormal(Vector3 normal)
        {
            CollisionInfo.SlidingNormalValid = true;
            CollisionInfo.SlidingNormal = new Vector3(normal.X, normal.Y, 0);
            if (CollisionInfo.NormalizeCheckSmall(ref CollisionInfo.SlidingNormal))
                CollisionInfo.SlidingNormal = Vector3.Zero;
        }

        public void InitSphere(int numSphere, Sphere sphere, float scale)
        {
            SpherePath.InitSphere(numSphere, new List<Sphere>() { sphere }, scale);
        }

        public bool InsertIntoCell(ObjCell cell, int attempts)
        {
            return false;
        }

        public static Transition MakeTransition()
        {
            return null;
        }

        public int PlacementInsert()
        {
            return -1;
        }

        public bool StepDown(float stepDownHeight, float zValue)
        {
            return false;
        }

        public bool StepUp(Vector3 collisionNormal)
        {
            return true;
        }

        public TransitionState TransitionalInsert(int attempts)
        {
            return TransitionState.OK;
        }

        public TransitionState ValidatePlacement(TransitionState transitionState, bool adjust)
        {
            return TransitionState.OK;
        }

        public int ValidatePlacementTransition(TransitionState transitionState, bool redo)
        {
            return -1;
        }

        public TransitionState ValidateTransition(TransitionState transitionState, bool redo)
        {
            return TransitionState.OK;
        }
    }
}
