using System;
using System.Collections.Generic;
using System.Linq;
using ACE.Entity.Enum;
using ACE.Server.Physics.Combat;
using ACE.Server.Physics.Common;

namespace ACE.Server.Physics.Animation
{
    public class MoveToManager
    {
        public MovementType MovementType;
        public Position SoughtPosition;
        public Position CurrentTargetPosition;
        public Position StartingPosition;
        public MovementParameters MovementParams;
        public float PreviousHeading;
        public float PreviousDistance;
        public double PreviousDistanceTime;
        public float OriginalDistance;
        public double OriginalDistanceTime;
        public int FailProgressCount;
        public uint SoughtObjectID;
        public uint TopLevelObjectID;
        public float SoughtObjectRadius;
        public float SoughtObjectHeight;
        public uint CurrentCommand;
        public uint AuxCommand;
        public bool MovingAway;
        public bool Initialized;
        public List<MovementNode> PendingActions;
        public PhysicsObj PhysicsObj;
        public WeenieObject WeenieObj;
        public List<Action> Callbacks;

        public MoveToManager()
        {
            Init();
            InitializeLocalVars();
        }

        public MoveToManager(PhysicsObj obj, WeenieObject wobj)
        {
            PhysicsObj = obj;
            WeenieObj = wobj;
            Init();
            InitializeLocalVars();
        }

        public void Init()
        {
            PendingActions = new List<MovementNode>();
            Callbacks = new List<Action>();
        }

        public void AddMoveToPositionNode()
        {
            PendingActions.Add(new MovementNode(MovementType.MoveToPosition));
        }

        public void AddTurnToHeadingNode(float heading)
        {
            PendingActions.Add(new MovementNode(MovementType.TurnToHeading, heading));
        }

        public void BeginMoveForward()
        {
            if (PhysicsObj == null)
            {
                CancelMoveTo(WeenieError.NoPhysicsObject);
                return;
            }

            var dist = GetCurrentDistance();
            var heading = PhysicsObj.Position.heading(CurrentTargetPosition) - PhysicsObj.get_heading();
            if (Math.Abs(heading) < PhysicsGlobals.EPSILON)
                heading = 0.0f;
            if (heading < -PhysicsGlobals.EPSILON)
                heading += 360.0f;

            uint motion = 0;
            bool moveAway = false;
            HoldKey holdKey = HoldKey.Invalid;
            MovementParams.get_command(dist, heading, ref motion, ref holdKey, ref moveAway);

            if (motion == 0)
            {
                PendingActions.Clear();
                BeginNextNode();
                return;
            }
            var movementParams = new MovementParameters();
            movementParams.HoldKeyToApply = holdKey;
            movementParams.CancelMoveTo = false;
            movementParams.Speed = MovementParams.Speed;

            var result = _DoMotion(motion, movementParams);
            if (result != WeenieError.None)
            {
                CancelMoveTo(result);
                return;
            }
            CurrentCommand = motion;
            MovingAway = moveAway;
            MovementParams.HoldKeyToApply = holdKey;
            PreviousDistance = dist;
            PreviousDistanceTime = Timer.CurrentTime;
            OriginalDistance = dist;
            OriginalDistanceTime = Timer.CurrentTime;
        }

        public void BeginNextNode()
        {
            if (PendingActions.Count > 0) 
            {
                var pendingAction = PendingActions.First();

                switch (pendingAction.Type)
                {
                    case MovementType.MoveToPosition:
                        BeginMoveForward();
                        break;
                    case MovementType.TurnToHeading:
                        BeginTurnToHeading();
                        break;
                }
            }
            else
            {
                if (MovementParams.Sticky)
                {
                    var soughtObjectRadius = SoughtObjectRadius;
                    var soughtObjectHeight = SoughtObjectHeight;
                    var topLevelObjectID = TopLevelObjectID;
                    
                    // unsets sticky flag
                    CleanUpAndCallWeenie(0);

                    if (PhysicsObj != null)
                        PhysicsObj.get_position_manager().StickTo(topLevelObjectID, soughtObjectRadius, soughtObjectHeight);
                }
                else
                    CleanUpAndCallWeenie(0);

            }

            if (PendingActions.Count == 0)
                foreach (var callback in Callbacks.ToList())
                    callback();
        }

        public void BeginTurnToHeading()
        {
            //Console.WriteLine("BeginTurnToHeading");

            if (PhysicsObj == null)
            {
                CancelMoveTo(WeenieError.NoPhysicsObject);
                return;
            }

            if (PendingActions.Count == 0)
                return;

            var pendingAction = PendingActions[0];
            var headingDiff = heading_diff(pendingAction.Heading, PhysicsObj.get_heading(), (uint)MotionCommand.TurnRight);
            uint motionID = 0;
            if (headingDiff <= 180.0f)
            {
                if (headingDiff > PhysicsGlobals.EPSILON)
                    motionID = (uint)MotionCommand.TurnRight;
                else
                {
                    RemovePendingActionsHead();
                    BeginNextNode();
                    return;
                }
            }
            else
            {
                if (headingDiff + PhysicsGlobals.EPSILON <= 360.0f)
                    motionID = (uint)MotionCommand.TurnLeft;
                else
                {
                    RemovePendingActionsHead();
                    BeginNextNode();
                    return;
                }
            }

            var movementParams = new MovementParameters();
            movementParams.CancelMoveTo = false;
            //movementParams.Speed = MovementParams.Speed;
            movementParams.Speed = 1.0f;    // ??
            movementParams.HoldKeyToApply = MovementParams.HoldKeyToApply;

            var result = _DoMotion(motionID, movementParams);

            if (result != WeenieError.None)
            {
                CancelMoveTo(result);
                return;
            }

            CurrentCommand = motionID;
            PreviousHeading = headingDiff;
        }

        public void CancelMoveTo(WeenieError retval)
        {
            if (MovementType == MovementType.Invalid)
                return;

            PendingActions.Clear();
            CleanUpAndCallWeenie(retval);
        }

        public bool CheckProgressMade(float currDistance)
        {
            var deltaTime = Timer.CurrentTime - PreviousDistanceTime;

            if (deltaTime > 1.0f)
            {
                var diffDist = MovingAway ? currDistance - PreviousDistance : PreviousDistance - currDistance;

                if (diffDist / deltaTime < 0.25f)
                    return false;

                PreviousDistance = currDistance;
                PreviousDistanceTime = Timer.CurrentTime;

                var dOrigDist = MovingAway ? currDistance - OriginalDistance : OriginalDistance - currDistance;

                if (dOrigDist / (Timer.CurrentTime - OriginalDistanceTime) < 0.25f)
                    return false;
            }
            return true;
        }

        public void CleanUp()
        {
            var movementParams = new MovementParameters();
            movementParams.HoldKeyToApply = MovementParams.HoldKeyToApply;
            movementParams.CancelMoveTo = false;

            if (PhysicsObj != null)
            {
                if (CurrentCommand != 0)
                    _StopMotion(CurrentCommand, movementParams);

                if (AuxCommand != 0)
                    _StopMotion(AuxCommand, movementParams);

                if (TopLevelObjectID != 0 && MovementType != MovementType.Invalid)
                    PhysicsObj.clear_target();
            }
            InitializeLocalVars();
        }

        public void CleanUpAndCallWeenie(WeenieError status)
        {
            CleanUp();
            if (PhysicsObj != null)
                PhysicsObj.StopCompletely(false);
        }

        public static MoveToManager Create(PhysicsObj obj, WeenieObject wobj)
        {
            var moveToManager = new MoveToManager(obj, wobj);
            return moveToManager;
        }

        public float GetCurrentDistance()
        {
            if (PhysicsObj == null)
                return float.MaxValue;

            if ((MovementParams.Bitfield & 0x400) == 0)
                return PhysicsObj.Position.Distance(CurrentTargetPosition);

            return (float)Position.CylinderDistance(PhysicsObj.GetRadius(), PhysicsObj.GetHeight(), PhysicsObj.Position,
                SoughtObjectRadius, SoughtObjectHeight, CurrentTargetPosition);
        }

        public void HandleMoveToPosition()
        {
            //Console.WriteLine("HandleMoveToPosition");

            if (PhysicsObj == null)
            {
                CancelMoveTo(WeenieError.NoPhysicsObject);
                return;
            }
            var curPos = PhysicsObj.Position;

            var movementParams = new MovementParameters();
            //movementParams.Speed = MovementParams.Speed;
            movementParams.Speed = 1.0f;    // ??
            movementParams.Bitfield &= 0xFFFF7FFF;
            movementParams.HoldKeyToApply = MovementParams.HoldKeyToApply;

            if (!PhysicsObj.motions_pending())
            {
                var heading = MovementParams.get_desired_heading(CurrentCommand, MovingAway) + curPos.heading(CurrentTargetPosition);
                if (heading >= 360.0f) heading -= 360.0f;

                var diff = heading - PhysicsObj.get_heading();

                if (Math.Abs(diff) < PhysicsGlobals.EPSILON) diff = 0.0f;
                if (diff < -PhysicsGlobals.EPSILON)
                    diff += 360.0f;

                if (diff > 20.0f && diff < 340.0f)
                {
                    uint motionID = (uint)(diff >= 180.0f ? (uint)MotionCommand.TurnLeft : (uint)MotionCommand.TurnRight);
                    if (motionID != AuxCommand)
                    {
                        _DoMotion(motionID, movementParams);
                        AuxCommand = motionID;
                    }
                }
                else
                    stop_aux_command(movementParams);
            }
            else
                stop_aux_command(movementParams);

            var dist = GetCurrentDistance();

            if (!CheckProgressMade(dist))
            {
                if (!PhysicsObj.IsInterpolating() && !PhysicsObj.motions_pending())
                    FailProgressCount++;
            }
            else
            {
                FailProgressCount = 0;
                if (MovingAway && dist >= MovementParams.MinDistance && MovingAway || !MovingAway && dist <= MovementParams.DistanceToObject)
                {
                    PendingActions.RemoveAt(0);
                    _StopMotion(CurrentCommand, movementParams);

                    CurrentCommand = 0;
                    stop_aux_command(movementParams);

                    BeginNextNode();
                }
                else
                {
                    if (StartingPosition.Distance(PhysicsObj.Position) > MovementParams.FailDistance)
                        CancelMoveTo(WeenieError.YouChargedTooFar);
                }
            }
            if (TopLevelObjectID != 0 && MovementType != MovementType.Invalid)
            {
                var velocity = PhysicsObj.get_velocity();
                var velocityLength = velocity.Length();
                if (velocityLength > 0.1f)
                {
                    var time = dist / velocityLength;
                    if (Math.Abs(time - PhysicsObj.get_target_quantum()) > 1.0f)
                        PhysicsObj.set_target_quantum(time);
                }
            }
        }

        public void HandleTurnToHeading()
        {
            //Console.WriteLine("HandleTurnToHeading");

            if (PhysicsObj == null)
            {
                CancelMoveTo(WeenieError.NoPhysicsObject);
                return;
            }
            if (CurrentCommand != (uint)MotionCommand.TurnRight && CurrentCommand != (uint)MotionCommand.TurnLeft)
            {
                BeginTurnToHeading();
                return;
            }
            var pendingAction = PendingActions[0];
            var heading = PhysicsObj.get_heading();
            if (heading_greater(heading, pendingAction.Heading, CurrentCommand))
            {
                FailProgressCount = 0;
                PhysicsObj.set_heading(pendingAction.Heading, true);

                RemovePendingActionsHead();

                var movementParams = new MovementParameters();
                movementParams.CancelMoveTo = false;
                movementParams.HoldKeyToApply = MovementParams.HoldKeyToApply;

                _StopMotion(CurrentCommand, movementParams);

                CurrentCommand = 0;
                BeginNextNode();
                return;
            }
            PreviousHeading = heading;
            var diff = heading_diff(heading, PreviousHeading, CurrentCommand);
            if (diff > PhysicsGlobals.EPSILON && diff < 180.0f)
                FailProgressCount = 0;
            else if (!PhysicsObj.IsInterpolating() && !PhysicsObj.motions_pending())
                FailProgressCount++;
        }

        public void HandleUpdateTarget(TargetInfo targetInfo)
        {
            if (PhysicsObj == null)
            {
                CancelMoveTo(WeenieError.NoPhysicsObject);
                return;
            }

            if (TopLevelObjectID != targetInfo.ObjectID)
                return;

            if (Initialized)
            {
                if (targetInfo.Status == TargetStatus.OK)
                {
                    if (MovementType == MovementType.MoveToObject)
                    {
                        SoughtPosition = targetInfo.InterpolatedPosition;
                        CurrentTargetPosition = targetInfo.TargetPosition;
                        PreviousDistance = float.MaxValue;
                        PreviousDistanceTime = Timer.CurrentTime;
                        OriginalDistance = float.MaxValue;
                        OriginalDistanceTime = Timer.CurrentTime;
                    }
                }
                else
                    CancelMoveTo(WeenieError.ObjectGone);
            }
            else if (TopLevelObjectID == PhysicsObj.ID)
            {
                SoughtPosition = PhysicsObj.Position;
                CurrentTargetPosition = PhysicsObj.Position;
                CleanUpAndCallWeenie(WeenieError.None);
            }
            else if (targetInfo.Status == TargetStatus.OK)
            {
                if (MovementType == MovementType.MoveToObject)
                    MoveToObject_Internal(targetInfo.TargetPosition, targetInfo.InterpolatedPosition);
                else if (MovementType == MovementType.TurnToObject)
                    TurnToObject_Internal(targetInfo.TargetPosition);
            }
            else
                CancelMoveTo(WeenieError.NoObject);
        }

        public void HitGround()
        {
            if (MovementType != MovementType.Invalid)
                BeginNextNode();
        }

        public void InitializeLocalVars()
        {
            MovementType = MovementType.Invalid;

            MovementParams = new MovementParameters();

            PreviousDistanceTime = Timer.CurrentTime;
            OriginalDistanceTime = Timer.CurrentTime;

            PreviousHeading = 0.0f;

            FailProgressCount = 0;
            CurrentCommand = 0;
            AuxCommand = 0;
            MovingAway = false;
            Initialized = false;

            SoughtPosition = new Position();
            CurrentTargetPosition = new Position();

            SoughtObjectID = 0;
            TopLevelObjectID = 0;
            SoughtObjectRadius = 0;
            SoughtObjectHeight = 0;
        }

        public void MoveToObject(uint objectID, uint topLevelID, float radius, float height, MovementParameters movementParams)
        {
            //Console.WriteLine("MoveToObject");

            if (PhysicsObj == null) return;
            PhysicsObj.StopCompletely(false);

            StartingPosition = PhysicsObj.Position;
            SoughtObjectID = objectID;
            SoughtObjectRadius = radius;
            SoughtObjectHeight = height;
            MovementType = MovementType.MoveToObject;
            MovementParams = movementParams;
            TopLevelObjectID = topLevelID;
            Initialized = false;
            if (PhysicsObj.ID != topLevelID)
            {
                PhysicsObj.set_target(0, TopLevelObjectID, 0.5f, 0.0f);
                return;
            }
            CleanUp();
            PhysicsObj.StopCompletely(false);
        }

        public void MoveToObject_Internal(Position targetPosition, Position interpolatedPosition)
        {
            //Console.WriteLine("MoveToObject_Internal");

            if (PhysicsObj == null)
            {
                CancelMoveTo(WeenieError.NoPhysicsObject);
                return;
            }
            SoughtPosition = interpolatedPosition;
            CurrentTargetPosition = targetPosition;

            var iHeading = PhysicsObj.Position.heading(interpolatedPosition);
            var heading = iHeading - PhysicsObj.get_heading();
            var dist = GetCurrentDistance();

            if (Math.Abs(heading) < PhysicsGlobals.EPSILON)
                heading = 0.0f;
            if (heading < -PhysicsGlobals.EPSILON)
                heading += 360.0f;

            HoldKey holdKey = HoldKey.Invalid;
            uint motionID = 0;
            bool moveAway = false;
            MovementParams.get_command(dist, heading, ref motionID, ref holdKey, ref moveAway);

            if (motionID != 0)
            {
                AddTurnToHeadingNode(iHeading);
                AddMoveToPositionNode();
            }
            if (MovementParams.UseFinalHeading)
            {
                var dHeading = MovementParams.DesiredHeading;
                if (dHeading >= 360.0f)
                    dHeading -= 360.0f;
                AddTurnToHeadingNode(dHeading);
            }
            Initialized = true;
            BeginNextNode();
        }

        public void MoveToPosition(Position position, MovementParameters movementParams)
        {
            //Console.WriteLine("MoveToPosition");

            if (PhysicsObj == null)
            {
                CancelMoveTo(WeenieError.NoPhysicsObject);
                return;
            }

            PhysicsObj.StopCompletely(false);

            CurrentTargetPosition = position;
            SoughtObjectRadius = 0.0f;

            var distance = GetCurrentDistance();
            var headingDiff = PhysicsObj.Position.heading(position) - PhysicsObj.get_heading();

            if (Math.Abs(headingDiff) < PhysicsGlobals.EPSILON)
                headingDiff = 0.0f;
            if (headingDiff < -PhysicsGlobals.EPSILON)
                headingDiff += 360.0f;

            HoldKey holdKey = HoldKey.Invalid;
            uint command = 0;
            bool moveAway = false;
            movementParams.get_command(distance, headingDiff, ref command, ref holdKey, ref moveAway);

            if (command != 0)
            {
                AddTurnToHeadingNode(PhysicsObj.Position.heading(position));
                AddMoveToPositionNode();
            }

            if (MovementParams.UseFinalHeading)
                AddTurnToHeadingNode(movementParams.DesiredHeading);

            SoughtPosition = position;
            StartingPosition = PhysicsObj.Position;

            MovementType = MovementType.MoveToPosition;
            MovementParams = movementParams;
            MovementParams.Bitfield &= 0xFFFFFF7F;

            BeginNextNode();
        }

        public WeenieError PerformMovement(MovementStruct mvs)
        {
            CancelMoveTo(WeenieError.ActionCancelled);
            PhysicsObj.unstick_from_object();
            switch (mvs.Type)
            {
                case MovementType.MoveToObject:
                    MoveToObject(mvs.ObjectId, mvs.TopLevelId, mvs.Radius, mvs.Height, mvs.Params);
                    break;
                case MovementType.MoveToPosition:
                    MoveToPosition(mvs.Position, mvs.Params);
                    break;
                case MovementType.TurnToObject:
                    TurnToObject(mvs.ObjectId, mvs.TopLevelId, mvs.Params);
                    break;
                case MovementType.TurnToHeading:
                    TurnToHeading(mvs.Params);
                    break;
            }
            return WeenieError.None;
        }

        public void RemovePendingActionsHead()
        {
            if (PendingActions.Count > 0)
                PendingActions.RemoveAt(0);
        }

        public void SetPhysicsObject(PhysicsObj obj)
        {
            PhysicsObj = obj;
        }

        public void SetWeenieObject(WeenieObject wobj)
        {
            WeenieObj = wobj;
        }

        public void TurnToHeading(MovementParameters movementParams)
        {
            //Console.WriteLine("TurnToHeading");

            if (PhysicsObj == null)
            {
                MovementParams.ContextID = movementParams.ContextID;
                return;
            }

            if (movementParams.StopCompletely)
                PhysicsObj.StopCompletely(false);

            MovementParams = movementParams;
            MovementParams.Sticky = false;

            SoughtPosition.Frame.set_heading(movementParams.DesiredHeading);
            MovementType = MovementType.TurnToHeading;

            PendingActions.Add(new MovementNode(MovementType.TurnToHeading, movementParams.DesiredHeading));
        }

        public void TurnToObject(uint objectID, uint topLevelID, MovementParameters movementParams)
        {
            //Console.WriteLine("TurnToObject");

            if (PhysicsObj == null)
            {
                MovementParams.ContextID = movementParams.ContextID;
                return;
            }

            if (movementParams.StopCompletely)
                PhysicsObj.StopCompletely(false);

            MovementType = MovementType.TurnToObject;
            SoughtObjectID = objectID;

            CurrentTargetPosition.Frame.set_heading(movementParams.DesiredHeading);

            TopLevelObjectID = topLevelID;
            MovementParams = movementParams;

            if (PhysicsObj.ID != topLevelID)
            {
                Initialized = false;
                PhysicsObj.set_target(0, topLevelID, 0.5f, 0.0f);
                return;
            }
            CleanUp();
            PhysicsObj.StopCompletely(false);
        }

        public void TurnToObject_Internal(Position targetPosition)
        {
            //Console.WriteLine("TurnToObject_Internal");

            if (PhysicsObj == null)
            {
                CancelMoveTo(WeenieError.NoPhysicsObject);
                return;
            }

            CurrentTargetPosition = targetPosition; // ref?

            var targetHeading = PhysicsObj.Position.heading(CurrentTargetPosition);
            var soughtHeading = SoughtPosition.Frame.get_heading();
            var heading = (targetHeading + soughtHeading) % 360.0f;
            SoughtPosition.Frame.set_heading(heading);

            PendingActions.Add(new MovementNode(MovementType.TurnToHeading, heading));
            Initialized = true;

            BeginNextNode();
        }

        public void UseTime()
        {
            if (PhysicsObj == null || !PhysicsObj.TransientState.HasFlag(TransientStateFlags.Contact))
                return;

            if (TopLevelObjectID == 0 || MovementType == MovementType.Invalid || !Initialized || PendingActions.Count == 0)
                return;

            var pendingAction = PendingActions.First();

            switch (pendingAction.Type)
            {
                case MovementType.MoveToPosition:
                    HandleMoveToPosition();
                    break;
                case MovementType.TurnToHeading:
                    HandleTurnToHeading();
                    break;
            }
        }

        public WeenieError _DoMotion(uint motion, MovementParameters movementParams)
        {
            if (PhysicsObj == null)
                return WeenieError.NoPhysicsObject;

            var minterp = PhysicsObj.get_minterp();
            if (minterp == null)
                return WeenieError.NoMotionInterpreter;

            minterp.adjust_motion(ref motion, ref movementParams.Speed, movementParams.HoldKeyToApply);

            return minterp.DoInterpretedMotion(motion, movementParams);
        }

        public WeenieError _StopMotion(uint motion, MovementParameters movementParams)
        {
            if (PhysicsObj == null)
                return WeenieError.NoPhysicsObject;

            var minterp = PhysicsObj.get_minterp();
            if (minterp == null)
                return WeenieError.NoMotionInterpreter;

            minterp.adjust_motion(ref motion, ref movementParams.Speed, movementParams.HoldKeyToApply);

            return minterp.StopInterpretedMotion(motion, movementParams);
        }

        public static float heading_diff(float x, float y, uint motion)
        {
            var result = x - y;

            if (Math.Abs(result) < PhysicsGlobals.EPSILON)
                result = 0.0f;
            if (result < -PhysicsGlobals.EPSILON)
                result += 360.0f;
            if (result > PhysicsGlobals.EPSILON && motion != (uint)MotionCommand.TurnRight)
                result = 360.0f - result;
            return result;
        }

        public static bool heading_greater(float x, float y, uint motion)
        {
            /*var less = Math.Abs(x - y) <= 180.0f ? x < y : y < x;
            var result = (less || x == y) == false;
            if (motion != 0x6500000D)
                result = !result;
            return result;*/
            var diff = Math.Abs(x - y);

            float v1, v2;

            if (diff <= 180.0f)
            {
                v1 = y;
                v2 = x;
            }
            else
            {
                v1 = x;
                v2 = y;
            }

            var result = (v2 > v1) ? true : false;

            if (motion != (uint)MotionCommand.TurnRight)
                result = !result;

            return result;
        }

        public bool is_moving_to()
        {
            return MovementType != MovementType.Invalid;
        }

        public void stop_aux_command(MovementParameters movementParams)
        {
            if (AuxCommand != 0)
            {
                _StopMotion(AuxCommand, movementParams);
                AuxCommand = 0;
            }
        }

        public void add_listener(Action listener)
        {
            Callbacks.Add(listener);
        }

        public void remove_listener(Action listener)
        {
            Callbacks.Remove(listener);
        }
    }
}
