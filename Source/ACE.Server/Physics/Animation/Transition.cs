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
            var dist = Vector3.Dot(globSphere.Center - blockOffset, CollisionInfo.ContactPlane.Normal);
            if (dist >= globSphere.Radius - PhysicsGlobals.EPSILON)
                return offset;

            var zDist = (globSphere.Radius - dist) / CollisionInfo.ContactPlane.Normal.Z;
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
            if (!ObjectInfo.State.HasFlag(ObjectInfoState.OnWalkable) || SpherePath.CheckWalkables())
                return true;

            SpherePath.BackupCheckPos = SpherePath.CheckPos;
            SpherePath.BackupCell = SpherePath.CheckCell;

            var stepHeight = ObjectInfo.StepDownHeight;

            SpherePath.WalkableAllowance = zCheck;
            SpherePath.CheckWalkable = true;

            var globSphere = SpherePath.GlobalSphere[0];
            if (SpherePath.NumSphere < 2 && stepHeight > globSphere.Radius * 2)
                stepHeight = globSphere.Radius * 0.5f;

            if (stepHeight > globSphere.Radius * 2)
                stepHeight *= 0.5f;

            var offset = new Vector3(0, 0, -stepHeight);
            SpherePath.AddOffsetToCheckPos(offset);

            var transitionState = TransitionalInsert(1);

            SpherePath.CheckWalkable = false;
            SpherePath.SetCheckPos(SpherePath.BackupCheckPos, SpherePath.BackupCell);

            return transitionState != TransitionState.OK;
        }

        public void CleanupTransition()
        {
            --TransitionLevel;
        }

        public TransitionState CliffSlide(Plane contactPlane)
        {
            var contactNormal = Vector3.Cross(contactPlane.Normal, CollisionInfo.LastKnownContactPlane.Normal);
            contactNormal.Z = 0.0f;

            var collideNormal = new Vector3(contactNormal.Z - contactNormal.Y, contactNormal.X - contactNormal.Z, 0.0f);
            if (CollisionInfo.NormalizeCheckSmall(ref collideNormal))
                return TransitionState.OK;

            var blockOffset = LandDefs.GetBlockOffset(SpherePath.CurPos.ObjCellID, SpherePath.CheckPos.ObjCellID);
            var offset = SpherePath.GlobalSphere[0].Center - SpherePath.GlobalCurrCenter[0].Center;

            var angle = Vector3.Dot(collideNormal, offset + blockOffset);
            if (angle <= 0.0f)
            {
                SpherePath.AddOffsetToCheckPos(collideNormal * angle);
                CollisionInfo.SetCollisionNormal(collideNormal);
            }
            else
            {
                SpherePath.AddOffsetToCheckPos(collideNormal * -angle);
                CollisionInfo.SetCollisionNormal(collideNormal * angle);
            }
            return TransitionState.Adjusted;
        }

        public bool EdgeSlide(ref TransitionState transitionState, float stepDownHeight, float zval)
        {
            if (!ObjectInfo.State.HasFlag(ObjectInfoState.OnWalkable) || !ObjectInfo.State.HasFlag(ObjectInfoState.EdgeSlide))
            {
                transitionState = SetEdgeSlide(true, true, TransitionState.OK);
                return true;
            }
            else if (CollisionInfo.ContactPlaneValid && CollisionInfo.ContactPlane.Normal.Z < zval)
            {
                transitionState = SetEdgeSlide(true, false, CliffSlide(CollisionInfo.ContactPlane));
                return false;
            }
            else if (SpherePath.Walkable != null)
            {
                transitionState = SetEdgeSlide(false, false, SpherePath.PrecipiceSlide(this));

                return transitionState == TransitionState.Collided;
            }
            else if (CollisionInfo.ContactPlaneValid)
            {
                transitionState = SetEdgeSlide(true, true, TransitionState.OK);
                return true;
            }
            else
            {
                var offset = SpherePath.GlobalCurrCenter[0].Center - SpherePath.GlobalSphere[0].Center;
                SpherePath.AddOffsetToCheckPos(offset);

                StepDown(stepDownHeight, zval);

                CollisionInfo.ContactPlaneValid = false;
                CollisionInfo.ContactPlaneIsWater = false;
                SpherePath.RestoreCheckPos();

                if (SpherePath.Walkable != null)
                {
                    SpherePath.CacheLocalSpaceSphere(SpherePath.GetWalkablePos(), SpherePath.WalkableScale);
                    SpherePath.SetWalkableCheckPos(SpherePath.LocalSpaceSphere[0]);

                    transitionState = SpherePath.PrecipiceSlide(this);
                    return transitionState == TransitionState.Collided;
                }
                else
                {
                    SpherePath.Walkable = null;
                    SpherePath.CellArrayValid = true;

                    transitionState = TransitionState.Collided;
                    return true;
                }
            }
        }

        public TransitionState SetEdgeSlide(bool unwalkable, bool validCell, TransitionState transitionState)
        {
            if (validCell)  SpherePath.CellArrayValid = true;
            if (unwalkable) SpherePath.Walkable = null;

            CollisionInfo.ContactPlaneValid = false;
            CollisionInfo.ContactPlaneIsWater = false;

            SpherePath.RestoreCheckPos();

            return transitionState;
        }

        public bool FindPlacementPos()
        {
            // refactor me
            SpherePath.SetCheckPos(SpherePath.CurPos, SpherePath.CurCell);

            CollisionInfo.SlidingNormalValid = false;
            CollisionInfo.ContactPlaneValid = false;
            CollisionInfo.ContactPlaneIsWater = false;

            var transitionState = TransitionalInsert(3);

            var redo = 0;
            transitionState = ValidatePlacementTransition(transitionState, ref redo);

            if (transitionState == TransitionState.OK)
                return true;

            if (!SpherePath.PlacementAllowsSliding)
                return false;

            var adjustDist = 4.0f;
            var adjustRad = adjustDist;
            var sphereRad = SpherePath.LocalSphere[0].Radius;

            var fakeSphere = false;

            if (sphereRad < 0.125f)
            {
                fakeSphere = true;
                adjustRad = 2.0f;
            }
            else if (sphereRad < 0.48f)
            {
                sphereRad = 0.48f;
            }

            var delta = sphereRad;

            var step = 4.0f / delta;

            if (fakeSphere)
                step *= 0.5f;

            if (step <= 1.0f)
                return false;

            var numSteps = (int)Math.Ceiling(step);
            var distPerStep = adjustRad / numSteps;
            var radiansPerStep = (float)(Math.PI * distPerStep / sphereRad);

            var totalDist = 0.0f;
            var totalRad = 0.0f;

            for (var i = 0; i < numSteps; i++)
            {
                totalDist += distPerStep;
                totalRad += radiansPerStep;

                var rad = (int)Math.Ceiling(totalRad);
                var angle = rad * 2;

                var frame = new AFrame();

                for (var j = 0; j < rad; j++)
                {
                    SpherePath.SetCheckPos(SpherePath.CurPos, SpherePath.CurCell);
                    frame.set_heading(angle * j);
                    var offset = frame.get_vector_heading() * totalDist;
                    SpherePath.GlobalOffset = AdjustOffset(offset);
                    if (SpherePath.GlobalOffset.Length() >= PhysicsGlobals.EPSILON)
                    {
                        SpherePath.AddOffsetToCheckPos(SpherePath.GlobalOffset);

                        // possibly reset by AdjustOffset
                        CollisionInfo.SlidingNormalValid = false;
                        CollisionInfo.ContactPlaneValid = false;
                        CollisionInfo.ContactPlaneIsWater = false;

                        transitionState = TransitionalInsert(3);
                        transitionState = ValidatePlacementTransition(transitionState, ref redo);

                        if (transitionState == TransitionState.OK) return true;
                    }
                }
            }
            return false;
        }

        public bool FindPlacementPosition()
        {
            SpherePath.SetCheckPos(SpherePath.CurPos, SpherePath.CurCell);
            SpherePath.InsertType = InsertType.InitialPlacement;
            var transitionState = new TransitionState();

            if (SpherePath.CheckCell != null)
            {
                transitionState = InsertIntoCell(SpherePath.CheckCell, 3);
                if (transitionState == TransitionState.OK)
                    transitionState = CheckOtherCells(SpherePath.CheckCell);
            }
            else
                transitionState = TransitionState.Collided;

            if (ValidatePlacement(transitionState, true) != TransitionState.OK)
                return false;

            SpherePath.InsertType = InsertType.Placement;
            if (!FindPlacementPos()) return false;

            if (!ObjectInfo.StepDown)
                return ValidatePlacement(TransitionState.OK, true) == TransitionState.OK;

            SpherePath.WalkableAllowance = PhysicsGlobals.LandingZ;
            SpherePath.SaveCheckPos();

            SpherePath.Backup = SpherePath.InsertType;
            SpherePath.InsertType = InsertType.Transition;

            var globSphere = SpherePath.GlobalSphere[0];
            var stepDownHeight = ObjectInfo.StepDownHeight;

            if (SpherePath.NumSphere < 2 && globSphere.Radius * 2 < stepDownHeight)
                stepDownHeight = globSphere.Radius * 0.5f;

            if (stepDownHeight < globSphere.Radius * 2)
            {
                if (!StepDown(stepDownHeight, PhysicsGlobals.LandingZ))
                {
                    SpherePath.RestoreCheckPos();
                    CollisionInfo.ContactPlaneValid = false;
                    CollisionInfo.ContactPlaneIsWater = false;
                }
            }
            else
            {
                stepDownHeight *= 0.5f;
                if (!StepDown(stepDownHeight, PhysicsGlobals.LandingZ))
                {
                    SpherePath.RestoreCheckPos();
                    CollisionInfo.ContactPlaneValid = false;
                    CollisionInfo.ContactPlaneIsWater = false;
                }
            }

            SpherePath.InsertType = SpherePath.Backup;
            SpherePath.Walkable = null;

            return ValidatePlacement(TransitionState.OK, true) == TransitionState.OK;
        }

        public bool FindTransitionalPosition()
        {
            if (SpherePath.BeginCell == null) return false;

            var transitionState = TransitionState.OK;
            var offset = Vector3.Zero;
            var offsetPerStep = Vector3.Zero;
            var numSteps = 0;

            CalcNumSteps(ref offset, ref offsetPerStep, ref numSteps);  // restructure as retval?

            if (ObjectInfo.State.HasFlag(ObjectInfoState.FreeRotate))
                SpherePath.CurPos.Frame.Rotate(SpherePath.EndPos.Frame.Orientation);

            SpherePath.SetCheckPos(SpherePath.CurPos, SpherePath.CurCell);

            var redo = 0;
            if (numSteps <= 0)
            {
                if (!ObjectInfo.State.HasFlag(ObjectInfoState.FreeRotate))  // ?
                    SpherePath.CurPos.Frame.Rotate(SpherePath.EndPos.Frame.Orientation);

                SpherePath.CellArrayValid = true;
                SpherePath.HitsInteriorCell = false;

                CellArray.find_cell_list(SpherePath);
                return true;
            }

            for (var step = 0; step < numSteps; step++)
            {
                if (ObjectInfo.State.HasFlag(ObjectInfoState.IsViewer))
                {
                    var lastStep = numSteps - 1;

                    if (step == lastStep)
                    {
                        var offsetLen = offset.Length();
                        if (offsetLen > PhysicsGlobals.EPSILON)
                        {
                            redo = lastStep;
                            offsetPerStep = offset * (offsetLen - SpherePath.LocalSphere[0].Radius * lastStep) / offsetLen;
                        }
                    }
                }
                SpherePath.GlobalOffset = AdjustOffset(offsetPerStep);
                if (!ObjectInfo.State.HasFlag(ObjectInfoState.IsViewer))
                {
                    if (SpherePath.GlobalOffset.LengthSquared() < PhysicsGlobals.EPSILON * PhysicsGlobals.EPSILON)
                    {
                        return (step != 0 && transitionState == TransitionState.OK);
                    }
                }
                if (!ObjectInfo.State.HasFlag(ObjectInfoState.FreeRotate))
                {
                    redo = step + 1;
                    var delta = (float)redo / numSteps;
                    SpherePath.CheckPos.Frame.InterpolateRotation(SpherePath.BeginPos.Frame, SpherePath.EndPos.Frame, delta);
                }

                CollisionInfo.SlidingNormalValid = false;
                CollisionInfo.ContactPlaneValid = false;
                CollisionInfo.ContactPlaneIsWater = false;

                if (SpherePath.InsertType != InsertType.Transition)
                {
                    var insert = TransitionalInsert(3);
                    transitionState = ValidatePlacementTransition(insert, ref redo);

                    if (transitionState == TransitionState.OK)
                        return true;

                    if (!SpherePath.PlacementAllowsSliding)
                        return false;

                    SpherePath.AddOffsetToCheckPos(SpherePath.GlobalOffset);
                }
                else
                {
                    SpherePath.CellArrayValid = false;

                    SpherePath.CheckPos.Frame.Origin += SpherePath.GlobalOffset;
                    SpherePath.CacheGlobalSphere(SpherePath.GlobalOffset);

                    transitionState = ValidateTransition(TransitionalInsert(3), ref redo);

                    if (CollisionInfo.FramesStationaryFall > 0) break;
                }

                if (CollisionInfo.CollisionNormalValid && ObjectInfo.State.HasFlag(ObjectInfoState.PathClipped)) break;
            }

            return transitionState == TransitionState.OK;
        }

        public bool FindValidPosition()
        {
            if (SpherePath.InsertType == InsertType.Transition)
                return FindTransitionalPosition();
            else
                return FindPlacementPosition();
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

        public TransitionState InsertIntoCell(ObjCell cell, int attempts)
        {
            return TransitionState.OK;
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

        public TransitionState ValidatePlacementTransition(TransitionState transitionState, ref int redo)
        {
            return TransitionState.OK;
        }

        public TransitionState ValidateTransition(TransitionState transitionState, ref int redo)
        {
            return TransitionState.OK;
        }
    }
}
