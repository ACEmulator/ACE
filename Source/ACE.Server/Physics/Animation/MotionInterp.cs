using System;
using System.Collections.Generic;
using System.Numerics;
using ACE.Server.Physics.Common;
using ACE.Server.Physics.Extensions;

namespace ACE.Server.Physics.Animation
{
    public enum HoldKey
    {
        Invalid = 0x0,
        None = 0x1,
        Run = 0x2,
        NumHoldKeys = 0x3
    };

    /// <summary>
    /// Motion network command interpretor
    /// </summary>
    public class MotionInterp
    {
        public bool Initted;
        public WeenieObject WeenieObj;
        public PhysicsObj PhysicsObj;
        public RawMotionState RawState;
        public InterpretedMotionState InterpretedState;
        public float CurrentSpeedFactor;
        public bool StandingLongJump;
        public float JumpExtent;
        public int ServerActionStamp;
        public float MyRunRate;
        public List<MotionNode> PendingMotions;

        public static readonly float BackwardsFactor = 6.4999998e-1f;
        public static readonly float MaxSidestepAnimRate = 3.0f;
        public static readonly float RunAnimSpeed = 4.0f;
        public static readonly float RunTurnFactor = 1.5f;
        public static readonly float SidestepAnimSpeed = 1.25f;
        public static readonly float SidestepFactor = 0.5f;
        public static readonly float WalkAnimSpeed = 3.1199999f;

        public MotionInterp() { }

        public MotionInterp(PhysicsObj obj, WeenieObject wobj)
        {
            CurrentSpeedFactor = 1.0f;
            MyRunRate = 1.0f;

            SetPhysicsObject(obj);
            SetWeenieObject(wobj);
        }

        public static MotionInterp Create(PhysicsObj obj, WeenieObject wobj)
        {
            var motionInterp = new MotionInterp(obj, wobj);
            return motionInterp;
        }

        public Sequence DoInterpretedMotion(uint motion, MovementParameters movementParams)
        {
            if (PhysicsObj == null)
                return new Sequence(8);

            var sequence = new Sequence();

            if (contact_allows_move(motion))
            {
                if (StandingLongJump && (motion == 0x45000005 || motion == 0x44000007 || motion == 0x6500000F))
                {
                    if (movementParams.ModifyInterpretedState)
                        InterpretedState.ApplyMotion(motion, movementParams);
                }
                else
                {
                    if (motion == 0x40000011)
                        PhysicsObj.RemoveLinkAnimations();

                    sequence = PhysicsObj.DoInterpretedMotion(motion, movementParams);

                    if (sequence == null)
                    {
                        var jump_error_code = 0;

                        if (movementParams.DisableJumpDuringLink)
                            jump_error_code = 0x48;
                        else
                        {
                            jump_error_code = motion_allows_jump(motion);

                            if (jump_error_code == 0 && (motion & 0x10000000) != 0)
                                jump_error_code = motion_allows_jump(InterpretedState.ForwardCommand);
                        }

                        add_to_queue(movementParams.ContextID, motion, jump_error_code);

                        if (movementParams.ModifyInterpretedState)
                            InterpretedState.ApplyMotion(motion, movementParams);
                    }
                }
            }
            else
            {
                if ((motion & 0x10000000) != 0)
                    sequence.ID = 0x24;

                else if (movementParams.ModifyInterpretedState)
                    InterpretedState.ApplyMotion(motion, movementParams);
            }

            if (PhysicsObj != null && PhysicsObj.CurCell == null)
                PhysicsObj.RemoveLinkAnimations();

            return sequence;
        }

        public Sequence DoMotion(uint motion, MovementParameters movementParams)
        {
            if (PhysicsObj == null)
                return new Sequence(8);

            if (movementParams.CancelMoveTo)
                PhysicsObj.cancel_moveto();

            if (movementParams.SetHoldKey)
                SetHoldKey(movementParams.HoldKeyToApply, movementParams.CancelMoveTo);

            var sequence = new Sequence();

            if (InterpretedState.CurrentStyle != 0x8000003D)
            {
                switch (motion)
                {
                    case 0x41000012:
                        sequence.ID = 0x3F;
                        return sequence;
                    case 0x41000013:
                        sequence.ID = 0x40;
                        return sequence;
                    case 0x41000014:
                        sequence.ID = 0x41;
                        return sequence;
                }

                if ((motion & 0x2000000) != 0)
                {
                    sequence.ID = 0x42;
                    return sequence;
                }
            }

            if ((motion & 0x10000000) != 0)
            {
                if (InterpretedState.GetNumActions() >= 6)
                {
                    sequence.ID = 0x45;
                    return sequence;
                }
            }
            var newMotion = DoInterpretedMotion(motion, movementParams);

            if (newMotion == null && movementParams.ModifyRawState)
                RawState.ApplyMotion(motion, movementParams);

            return newMotion;
        }

        public void HandleExitWorld()
        {
            foreach (var pendingMotion in PendingMotions)
            {
                if (PhysicsObj != null && (pendingMotion.Motion & 0x10000000) != 0)
                {
                    PhysicsObj.unstick_from_object();
                    InterpretedState.RemoveAction();
                    RawState.RemoveAction();
                }
            }
            PendingMotions.Clear();
        }

        public void HitGround()
        {
            if (PhysicsObj == null) return;

            if (PhysicsObj.State.HasFlag(PhysicsState.Gravity)) return;

            if (WeenieObj != null && !WeenieObj.IsCreature()) return;

            PhysicsObj.RemoveLinkAnimations();
            apply_current_movement(false, true);
        }

        public long InqStyle()
        {
            return InterpretedState.CurrentStyle;
        }

        public void LeaveGround()
        {
            if (PhysicsObj == null) return;

            if (!PhysicsObj.State.HasFlag(PhysicsState.Gravity)) return;

            if (WeenieObj != null && !WeenieObj.IsCreature()) return;

            var velocity = get_leave_ground_velocity();
            PhysicsObj.set_local_velocity(velocity, true);

            StandingLongJump = false;
            JumpExtent = 0;

            PhysicsObj.RemoveLinkAnimations();
            apply_current_movement(false, true);
        }

        public void MotionDone(bool success)
        {
            if (PhysicsObj == null) return;

            foreach (var pendingMotion in PendingMotions)
            {
                if ((pendingMotion.Motion & 0x10000000) != 0)
                {
                    PhysicsObj.unstick_from_object();
                    InterpretedState.RemoveAction();
                    RawState.RemoveAction();
                }
            }
            PendingMotions.Clear();     // remove last motion?
        }

        public Sequence PerformMovement(MovementStruct mvs)
        {
            Sequence sequence = null;

            switch (mvs.Type)
            {
                case MovementType.RawCommand:
                    sequence = DoMotion(mvs.Motion, mvs.Params);
                    break;
                case MovementType.InterpretedCommand:
                    sequence = DoInterpretedMotion(mvs.Motion, mvs.Params);
                    break;
                case MovementType.StopRawCommand:
                    sequence = StopMotion(mvs.Motion, mvs.Params);
                    break;
                case MovementType.StopInterpretedCommand:
                    sequence = StopInterpretedMotion(mvs.Motion, mvs.Params);
                    break;
                case MovementType.StopCompletely:
                    sequence = StopCompletely();
                    break;
                default:
                    sequence.ID = 71;
                    return sequence;
            }
            PhysicsObj.CheckForCompletedMotions();
            return sequence;
        }

        public void ReportExhaustion()
        {
            if (PhysicsObj == null || !Initted) return;

            if (WeenieObj == null || WeenieObj.IsCreature() && PhysicsObj.movement_is_autonomous())
                apply_raw_movement(false, false);
            else
                apply_interpreted_movement(false, false);
        }

        public void SetHoldKey(HoldKey key, bool cancelMoveTo)
        {
            if (key == RawState.CurrentHoldKey) return;
            switch (key)
            {
                case HoldKey.None:
                    if (RawState.CurrentHoldKey == HoldKey.Run)
                    {
                        RawState.CurrentHoldKey = HoldKey.None;
                        apply_current_movement(cancelMoveTo, true);
                    }
                    break;
            }
        }

        public void SetPhysicsObject(PhysicsObj obj)
        {
            PhysicsObj = obj;
            apply_current_movement(true, true);
        }

        public void SetWeenieObject(WeenieObject wobj)
        {
            WeenieObj = wobj;
            apply_current_movement(true, true);
        }

        public Sequence StopCompletely()
        {
            if (PhysicsObj == null)
                return new Sequence(8);

            PhysicsObj.cancel_moveto();

            var jump = motion_allows_jump(InterpretedState.ForwardCommand);

            RawState.ForwardCommand = 0x41000003;
            RawState.ForwardSpeed = 1.0f;
            RawState.SideStepCommand = 0;
            RawState.TurnCommand = 0;

            InterpretedState.ForwardCommand = 0x41000003;
            InterpretedState.ForwardSpeed = 1.0f;
            InterpretedState.SideStepCommand = 0;
            InterpretedState.TurnCommand = 0;

            PhysicsObj.StopCompletely_Internal();

            add_to_queue(0, 0x41000003, jump);

            if (PhysicsObj.CurCell != null)
                PhysicsObj.RemoveLinkAnimations();

            return new Sequence(0);
        }

        public Sequence StopInterpretedMotion(uint motion, MovementParameters movementParams)
        {
            if (PhysicsObj == null)
                return new Sequence(8);

            var sequence = new Sequence();

            if (contact_allows_move(motion))
            {
                if (StandingLongJump && (motion == 0x45000005 || motion == 0x44000007 || motion == 0x6500000F))
                {
                    if (movementParams.ModifyInterpretedState)
                        InterpretedState.RemoveMotion(motion);
                }
                else
                {
                    sequence = PhysicsObj.StopInterpretedMotion(motion, movementParams);

                    if (sequence == null)
                    {
                        add_to_queue(movementParams.ContextID, 0x41000003, 0);

                        if (movementParams.ModifyInterpretedState)
                            InterpretedState.RemoveMotion(motion);
                    }
                }
            }
            else
            {
                if (movementParams.ModifyInterpretedState)
                    InterpretedState.RemoveMotion(motion);
            }

            if (PhysicsObj.CurCell == null)
                PhysicsObj.RemoveLinkAnimations();

            return sequence;
        }

        public Sequence StopMotion(uint motion, MovementParameters movementParams)
        {
            if (PhysicsObj == null) return new Sequence(8);

            if (movementParams.CancelMoveTo)
                PhysicsObj.cancel_moveto();

            adjust_motion(motion, movementParams.Speed, movementParams.HoldKeyToApply);

            var newMotion = StopInterpretedMotion(motion, movementParams);

            if (newMotion == null && movementParams.ModifyRawState)
                RawState.RemoveMotion(motion);

            return newMotion;
        }

        public void add_to_queue(int contextID, uint motion, int jumpErrorCode)
        {
            PendingMotions.Add(new MotionNode(contextID, motion, jumpErrorCode));
        }

        public void adjust_motion(uint motion, float speed, HoldKey holdKey)
        {
            if (WeenieObj != null && !WeenieObj.IsCreature())
                return;

            switch (motion)
            {
                case 0x44000007:
                    return;

                case 0x45000006:
                    motion = 0x45000005;
                    speed *= -BackwardsFactor;
                    break;

                case 0x6400000E:
                    motion = 0x6500000D;
                    speed *= -1.0f;
                    break;

                case 0x65000010:
                    motion = 0x6500000F;
                    speed *= -1.0f;
                    break;

                case 0x6500000F:
                    speed = speed * SidestepFactor * (WalkAnimSpeed / SidestepAnimSpeed);
                    break;
            }

            if (holdKey == HoldKey.Invalid)
                holdKey = RawState.CurrentHoldKey;

            if (holdKey == HoldKey.Run)
                apply_run_to_command(motion, speed);
        }

        public void apply_current_movement(bool cancelMoveTo, bool allowJump)
        {
            if (PhysicsObj == null || !Initted) return;

            if (WeenieObj == null || WeenieObj.IsCreature() && PhysicsObj.movement_is_autonomous())
                apply_raw_movement(cancelMoveTo, allowJump);
            else
                apply_interpreted_movement(cancelMoveTo, allowJump);
        }

        public void apply_interpreted_movement(bool cancelMoveTo, bool allowJump)
        {
            if (PhysicsObj == null) return;

            var movementParams = new MovementParameters();
            if (cancelMoveTo)
                movementParams.CancelMoveTo = true;
            if (!allowJump)
                movementParams.DisableJumpDuringLink = true;

            if (InterpretedState.ForwardCommand == 0x44000007)
                MyRunRate = InterpretedState.ForwardSpeed;

            DoInterpretedMotion(InterpretedState.CurrentStyle, movementParams);

            if (contact_allows_move(InterpretedState.ForwardCommand))
            {
                if (!StandingLongJump)
                {
                    movementParams.Speed = InterpretedState.ForwardSpeed;
                    DoInterpretedMotion(InterpretedState.ForwardCommand, movementParams);

                    if (InterpretedState.SideStepCommand != 0)
                    {
                        movementParams.Speed = InterpretedState.SideStepSpeed;
                        DoInterpretedMotion(InterpretedState.SideStepCommand, movementParams);
                    }
                    else
                        StopInterpretedMotion(0x6500000F, movementParams);
                }
                else
                {
                    movementParams.Speed = 1.0f;
                    DoInterpretedMotion(0x41000003, movementParams);
                    StopInterpretedMotion(0x6500000F, movementParams);
                }
            }
            else
            {
                movementParams.Speed = 1.0f;
                DoInterpretedMotion(0x40000015, movementParams);
            }

            if (InterpretedState.TurnCommand != 0)
            {
                movementParams.Speed = InterpretedState.TurnSpeed;
                DoInterpretedMotion(InterpretedState.TurnCommand, movementParams);
            }
            else
            {
                if (StopInterpretedMotion(0x6500000D, movementParams) != null)
                {
                    add_to_queue(movementParams.ContextID, 0x41000003, 0);

                    if (movementParams.ModifyInterpretedState)
                        InterpretedState.RemoveMotion(0x6500000D);
                }

                if (PhysicsObj.CurCell == null)
                    PhysicsObj.RemoveLinkAnimations();
            }
        }

        public void apply_raw_movement(bool cancelMoveTo, bool allowJump)
        {
            if (PhysicsObj == null) return;

            InterpretedState.CurrentStyle = RawState.CurrentStyle;
            InterpretedState.ForwardCommand = RawState.ForwardCommand;
            InterpretedState.ForwardSpeed = RawState.ForwardSpeed;
            InterpretedState.SideStepCommand = RawState.SideStepCommand;
            InterpretedState.SideStepSpeed = RawState.SideStepSpeed;
            InterpretedState.TurnCommand = RawState.TurnCommand;
            InterpretedState.TurnSpeed = RawState.TurnSpeed;

            adjust_motion(InterpretedState.ForwardCommand, InterpretedState.ForwardSpeed, RawState.ForwardHoldKey);
            adjust_motion(InterpretedState.SideStepCommand, InterpretedState.SideStepSpeed, RawState.SideStepHoldKey);
            adjust_motion(InterpretedState.TurnCommand, InterpretedState.TurnSpeed, RawState.TurnHoldKey);

            apply_interpreted_movement(cancelMoveTo, allowJump);
        }

        public void apply_run_to_command(uint motion, float speed)
        {
            var speedMod = 1.0f;

            if (WeenieObj != null)
            {
                var runFactor = 0.0f;
                if (WeenieObj.InqRunRate(ref runFactor))
                    speedMod = runFactor;
                else
                    speedMod = MyRunRate;
            }
            switch (motion)
            {
                case 0x45000005:
                    if (speed > 0.0f)
                        motion = 0x44000007;

                    speed *= speedMod;
                    break;

                case 0x6500000D:
                    speed *= RunTurnFactor;
                    break;

                case 0x6500000F:
                    speed *= speedMod;

                    if (MaxSidestepAnimRate < Math.Abs(speed))
                    {
                        if (speed > 0.0f)
                            speed = MaxSidestepAnimRate * 1.0f;
                        else
                            speed = MaxSidestepAnimRate * -1.0f;
                    }
                    break;
            }
        }

        public int charge_jump()
        {
            if (WeenieObj != null && !WeenieObj.CanJump(JumpExtent))
                return 0x49;

            var forward = InterpretedState.ForwardCommand;

            if (forward == 0x40000008 || forward > 0x41000011 && forward < 0x41000014)
                return 0x48;
            else
            {
                if (PhysicsObj.TransientState.HasFlag(TransientStateFlags.Contact | TransientStateFlags.OnWalkable) && forward == 0x41000003 &&
                    InterpretedState.SideStepCommand == 0 && InterpretedState.TurnCommand == 0)
                {
                    StandingLongJump = true;
                }
            }
            return 0;
        }

        public bool contact_allows_move(uint motion)
        {
            if (PhysicsObj == null) return false;

            if (motion == 0x40000011 || motion == 0x40000015 || motion >= 0x6500000D && motion <= 0x6500000E)
                return true;

            if (WeenieObj != null && !WeenieObj.IsCreature())
                return true;

            if (!PhysicsObj.State.HasFlag(PhysicsState.Gravity))
                return true;
            if (!PhysicsObj.TransientState.HasFlag(TransientStateFlags.Contact))
                return false;
            if (PhysicsObj.TransientState.HasFlag(TransientStateFlags.OnWalkable))
                return true;

            return false;
        }

        public void enter_default_state()
        {
            RawState = new RawMotionState();
            InterpretedState = new InterpretedMotionState();

            PhysicsObj.InitializeMotionTables();
            PendingMotions = new List<MotionNode>();

            add_to_queue(0, 0x41000003, 0);     // hardcoded default state?

            Initted = true;
            LeaveGround();
        }

        public float get_adjusted_max_speed()
        {
            var rate = 1.0f;

            if (WeenieObj != null)
            {
                if (!WeenieObj.InqRunRate(ref rate))
                    rate = MyRunRate;
            }

            if (InterpretedState.ForwardCommand == 0x44000007)
                rate = InterpretedState.ForwardSpeed / CurrentSpeedFactor;

            return rate * 4.0f;
        }

        public float get_jump_v_z()
        {
            if (JumpExtent < PhysicsGlobals.EPSILON)
                return 0.0f;

            var extent = JumpExtent;

            if (extent > 1.0f)
                extent = 1.0f;

            if (WeenieObj == null)
                return 10.0f;

            float vz = extent;
            if (WeenieObj.InqJumpVelocity(extent, ref vz))
                return vz;

            return 0.0f;
        }

        public Vector3 get_leave_ground_velocity()
        {
            var velocity = get_state_velocity();
            velocity.Z = get_jump_v_z();

            if (!velocity.Equals(Vector3.Zero))
                velocity = PhysicsObj.Position.GlobalToLocalVec(velocity);

            return velocity;
        }

        public float get_max_speed()
        {
            var rate = 1.0f;

            if (WeenieObj != null)
                if (!WeenieObj.InqRunRate(ref rate))
                    rate = MyRunRate;

            return RunAnimSpeed * rate;
        }

        public Vector3 get_state_velocity()
        {
            var velocity = Vector3.Zero;

            if (InterpretedState.SideStepCommand == 0x6500000F)
                velocity.X = SidestepAnimSpeed * InterpretedState.SideStepSpeed;
            if (InterpretedState.ForwardCommand == 0x45000005)
                velocity.Y = WalkAnimSpeed * InterpretedState.ForwardSpeed;
            else if (InterpretedState.ForwardCommand == 0x44000007)
                velocity.Y = RunAnimSpeed * InterpretedState.ForwardSpeed;

            var rate = MyRunRate;

            if (WeenieObj != null) WeenieObj.InqRunRate(ref rate);

            var maxSpeed = RunAnimSpeed * rate;
            if (velocity.Length() > maxSpeed)
            {
                velocity = velocity.Normalize();
                velocity *= maxSpeed;
            }
            return velocity;
        }

        public bool is_standing_still()
        {
            return PhysicsObj.TransientState.HasFlag(TransientStateFlags.Contact | TransientStateFlags.OnWalkable) &&
                InterpretedState.ForwardCommand == 0x41000003 &&
                InterpretedState.SideStepCommand == 0 &&
                InterpretedState.TurnCommand == 0;
        }

        public Sequence jump(float extent, int adjustStamina)
        {
            if (PhysicsObj == null) return new Sequence(8);

            PhysicsObj.cancel_moveto();

            var jump = jump_is_allowed(extent, adjustStamina);

            if (jump.ID != 0)
                StandingLongJump = false;
            else
            {
                JumpExtent = extent;
                PhysicsObj.set_on_walkable(false);
            }
            return jump;
        }

        public Sequence jump_charge_is_allowed()
        {
            if (WeenieObj != null && !WeenieObj.CanJump(JumpExtent))
                return new Sequence(0x49);

            var forward = InterpretedState.ForwardCommand;

            if (forward == 0x4000008 || forward > 0x41000011 && forward <= 0x41000014)
                return new Sequence(0x72);

            return new Sequence(0);
        }

        public Sequence jump_is_allowed(float extent, int staminaCost)
        {
            if (PhysicsObj == null)
                return new Sequence(0x24);

            if (WeenieObj == null && !WeenieObj.IsCreature() || !PhysicsObj.State.HasFlag(PhysicsState.Gravity) ||
                PhysicsObj.TransientState.HasFlag(TransientStateFlags.Contact | TransientStateFlags.OnWalkable))
            {
                if (PhysicsObj.IsFullyConstrained())
                    return new Sequence(0x47);

                if (PendingMotions.Count > 1 && PendingMotions[0].JumpErrorCode != 0)
                    return new Sequence(PendingMotions[0].JumpErrorCode);

                var jumpError = jump_charge_is_allowed();

                if (jumpError.ID == 0)
                {
                    jumpError.ID = motion_allows_jump(InterpretedState.ForwardCommand);

                    if (jumpError.ID == 0 && WeenieObj != null && WeenieObj.JumpStaminaCost(extent, staminaCost) != 0)
                        jumpError.ID = 0x47;
                }
                return jumpError;
            }
            return new Sequence(0x24);
        }

        public int motion_allows_jump(uint substate)
        {
            if (substate >= 0x40000016 && substate <= 0x40000018 || substate >= 0x10000128 && substate <= 0x10000131 ||
                substate >= 0x1000006F && substate <= 0x10000078 || substate >= 0x41000012 && substate <= 0x41000014 ||
                substate >= 0x4000001E && substate <= 0x40000039 || substate == 0x40000008)
            {
                return 0x48;
            }
            return 0;
        }

        public bool motions_pending()
        {
            return PendingMotions.Count > 0;
        }

        public bool move_to_interpreted_state(InterpretedMotionState state)
        {
            if (PhysicsObj == null) return false;

            RawState.CurrentStyle = state.CurrentStyle;
            PhysicsObj.cancel_moveto();

            var allowJump = motion_allows_jump(InterpretedState.ForwardCommand) == 0 ? true : false;

            InterpretedState = state;
            apply_current_movement(true, allowJump);

            var movementParams = new MovementParameters();

            foreach (var action in state.Actions)
            {
                var currentStamp = action.Stamp & 0x7FFF;
                var serverStamp = ServerActionStamp & 0x7FFFF;

                var deltaStamp = Math.Abs(currentStamp - serverStamp);

                var diff = deltaStamp <= 0x3FFF ? serverStamp < currentStamp : currentStamp < serverStamp;

                if (diff && WeenieObj != null && !WeenieObj.IsCreature() || action.Autonomous)
                {
                    ServerActionStamp = action.Stamp;
                    movementParams.Speed = action.Speed;
                    movementParams.Autonomous = action.Autonomous;
                    DoInterpretedMotion(action.Action, movementParams);
                }
            }
            return true;
        }

        public void set_hold_run(int val, bool cancelMoveTo)
        {
            if ((val == 0) != (RawState.CurrentHoldKey != HoldKey.Run))
            {
                RawState.CurrentHoldKey = val != 0 ? HoldKey.Run : HoldKey.None;
                apply_current_movement(cancelMoveTo, true);
            }
        }
    }
}
